// Gravatar.NET
// Copyright (c) 2010 Yoav Niran
// 
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Text;
using Gravatar.NET.Data;
using Gravatar.NET.Exceptions;

namespace Gravatar.NET
{
	/// <summary>
	/// The GravatarService class wraps around the raw xml-rpc API of Gravatar.com
	/// to give .NET clients easy and structured way of using the API to manage, upload and remove
	/// images for a Gravatar Account
	/// </summary>	
	public sealed partial class GravatarService
	{
		#region Private Members
		private const string GravatarApiUrl = "https://secure.gravatar.com/xmlrpc?user={0}";

		private const string ParPassword = "password";
		private const string ParHashes = "hashes";
		private const string ParData = "data";
		private const string ParRating = "rating";
		private const string ParUrl = "url";
		private const string ParUserImage = "userimage";
		private const string ParAddresses = "addresses";

		private const string UrlParSize = "s={0}&";
		private const string UrlParRating = "r={0}&";
		private const string UrlParDefault = "d={0}";
		private const string UrlPar404 = "404";
		#endregion

		#region Properties
		public const string GravatarImageUrlBase = "http://www.gravatar.com/avatar/";
		public const string GravatarProfileUrlBase = "http://www.gravatar.com";

		public string Email { get; private set; }
		public string Password { get; private set; }
		public string ApiKey { get; private set; }
		#endregion

		public GravatarService(string email, string password) {
			Email = email;
			Password = password;
		}

		public GravatarService(string apiKey) {
			ApiKey = apiKey;
		}



		#region Gravatar API Methods

		/// <summary>
		/// A test function
		/// </summary>
		/// <returns>If the method call is successful, the result will be returned using the IntegerResponse property of the 		
		/// <see cref="Gravatar.NET.GravatarServiceResponse"/> instance returned by this method.
		/// </returns>
		public GravatarServiceResponse Test() {
			var request = GetTestMethodRequest();
			var response = ExecuteGravatarMethod(request);
			return response;
		}

		/// <summary>
		/// Check whether a hash has a gravatar 
		/// </summary>
		/// <returns>If the method call is successful, the result will be returned using the MultipleOperationResponse property
		/// of the <see cref="Gravatar.NET.GravatarServiceResponse"/> instance returned by this method</returns>
		/// <param name="addresses">an array of hashes to check</param>
		/// <param name="alreadyHashed">Whether the supplied addresses are already hashed</param>
		public GravatarServiceResponse Exists(string[] addresses, bool alreadyHashed=false) {
			var request = GetExistsMethodRequest(addresses, alreadyHashed);
			var response = ExecuteGravatarMethod(request);
			return response;
		}
	
		/// <summary>
		/// Get a list of addresses for this account 
		/// </summary>
		/// <returns>If the method call is successful, the result will be returned using the AddressesResponse property of the
		/// <see cref="Gravatar.NET.GravatarServiceResponse"/> instance returned by this method</returns>		
		public GravatarServiceResponse Addresses() {
			var request = GetAddressesMethodRequest();
			var response = ExecuteGravatarMethod(request);
			return response;
		}

		/// <summary>
		/// Return an array of userimages for this account 
		/// </summary>
		/// <returns>If the method call is successful, the result will be returned using the ImagesResponse property of the
		/// <see cref="Gravatar.NET.GravatarServiceResponse"/> instance returned by this method</returns>
		public GravatarServiceResponse UserImages() {
			var request = GetUserImagesMethodRequest();
			var response = ExecuteGravatarMethod(request);
			return response;
		}

		/// <summary>
		/// Save binary image data as a userimage for this account 
		/// </summary>
		/// <param name="data">The image data in bytes</param>
		/// <param name="rating">The rating of the image (g, pg, r, x)</param>
		/// <returns>If the method call is successful, the result will be returned using the SaveResponse property of the
		/// <see cref="Gravatar.NET.GravatarServiceResponse"/> instance returned by this method</returns>
		public GravatarServiceResponse SaveData(byte[] data, GravatarImageRating rating) {
			var request = GetSaveDataMethodRequest(data, rating);
			var response = ExecuteGravatarMethod(request);
			return response;
		}

		/// <summary>
		/// Read an image via its URL and save that as a userimage for this account 
		/// </summary>
		/// <param name="url">a full url to an image</param>
		/// <param name="rating">The rating of the image (g, pg, r, x)</param>
		/// <returns>If the method call is successful, the result will be returned using the SaveResponse property of the
		/// <see cref="Gravatar.NET.GravatarServiceResponse"/> instance returned by this method</returns>
		public GravatarServiceResponse SaveUrl(string url, GravatarImageRating rating) {
			var request = GetSaveUrlMethodRequest(url, rating);
			var response = ExecuteGravatarMethod(request);
			return response;
		}

		/// <summary>
		/// Use a userimage as a gravatar for one of more addresses on this account
		/// </summary>
		/// <param name="userImage">The userimage you wish to use</param>
		/// <param name="addresses">A list of the email addresses you wish to use this userimage for</param>
		/// <returns>If the method call is successful, the result will be returned using the MultipleOperationResponse property
		/// of the <see cref="Gravatar.NET.GravatarServiceResponse"/> instance returned by this method</returns>
		public GravatarServiceResponse UseUserImage(string userImage, string[] addresses) {
			var request = GetUseUserImagesMethodRequest(userImage, addresses);
			var response = ExecuteGravatarMethod(request);
			return response;
		}

		/// <summary>
		/// Remove a userimage from the account and any email addresses with which it is associated 
		/// </summary>
		/// <param name="userImage">The user image you wish to remove from the account</param>
		/// <returns>If the method call is successful, the result will be returned using the BooleanResponse property of the
		/// <see cref="Gravatar.NET.GravatarServiceResponse"/> instance returned by this method</returns>
		public GravatarServiceResponse DeleteUserImage(string userImage) {
			var request = GetDeleteImageMethodRequest(userImage);
			var response = ExecuteGravatarMethod(request);
			return response;
		}
		#endregion

		#region Gravatar Async API Methods
		/// <summary>
		/// A test function - called asynchronously
		/// </summary>
		/// <param name="callback">The callback activated when action finishes</param>
		/// <param name="state">Custom parameter to callback</param>
		/// <returns>If the method call is successful, the result will be returned using the IntegerResponse property of the 		
		/// <see cref="Gravatar.NET.GravatarServiceResponse"/> instance returned by this method.
		/// The response is returned to the callback method
		/// </returns>
		public void TestAsync(GravatarCallBack callback, object state) {
			var request = GetTestMethodRequest();
			ExecuteGravatarMethodAsync(request, callback, state);
		}

		/// <summary>
		/// Check whether a hash has a gravatar - called asynchronously
		/// </summary>
		/// <param name="addresses">an array of hashes to check</param>
		/// <param name="callback">The callback activated when action finishes</param>
		/// <param name="state">Custom parameter to callback</param>
		/// <returns>If the method call is successful, the result will be returned using the MultipleOperationResponse property
		/// of the <see cref="Gravatar.NET.GravatarServiceResponse"/> instance returned by this method
		/// The response is returned to the callback method</returns>
		public void ExistsAsync(string[] addresses, GravatarCallBack callback, object state) {
			var request = GetExistsMethodRequest(addresses);
			ExecuteGravatarMethodAsync(request, callback, state);
		}

		/// <summary>
		/// Get a list of addresses for this account - called asynchronously
		/// </summary>
		/// <param name="callback">The callback activated when action finishes</param>
		/// <param name="state">Custom parameter to callback</param>
		/// <returns>If the method call is successful, the result will be returned using the AddressesResponse property of the
		/// <see cref="Gravatar.NET.GravatarServiceResponse"/> instance returned by this method
		/// The response is returned to the callback method</returns>
		public void AddressesAsync(GravatarCallBack callback, object state) {
			var request = GetAddressesMethodRequest();
			ExecuteGravatarMethodAsync(request, callback, state);
		}

		/// <summary>
		/// Return an array of userimages for this account - called asynchronously
		/// </summary>
		/// <param name="callback">The callback activated when action finishes</param>
		/// <param name="state">Custom parameter to callback</param>
		/// <returns>If the method call is successful, the result will be returned using the ImagesResponse property of the
		/// <see cref="Gravatar.NET.GravatarServiceResponse"/> instance returned by this method
		/// The response is returned to the callback method</returns>
		public void UserImagesAsync(GravatarCallBack callback, object state) {
			var request = GetUserImagesMethodRequest();
			ExecuteGravatarMethodAsync(request, callback, state);
		}

		/// <summary>
		/// Save binary image data as a userimage for this account - called asynchronously
		/// </summary>
		/// <param name="data">The image data in bytes</param>
		/// <param name="rating">The rating of the image (g, pg, r, x)</param>
		/// <param name="callback">The callback activated when action finishes</param>
		/// <param name="state">Custom parameter to callback</param>
		/// <returns>If the method call is successful, the result will be returned using the SaveResponse property of the
		/// <see cref="Gravatar.NET.GravatarServiceResponse"/> instance returned by this method
		/// The response is returned to the callback method</returns>
		public void SaveDataAsync(byte[] data, GravatarImageRating rating, GravatarCallBack callback, object state) {
			var request = GetSaveDataMethodRequest(data, rating);
			ExecuteGravatarMethodAsync(request, callback, state);
		}

		/// <summary>
		/// Read an image via its URL and save that as a userimage for this account - called asynchronously 
		/// </summary>
		/// <param name="url">a full url to an image</param>
		/// <param name="rating">The rating of the image (g, pg, r, x)</param>
		/// <param name="callback">The callback activated when action finishes</param>
		/// <param name="state">Custom parameter to callback</param>
		/// <returns>If the method call is successful, the result will be returned using the SaveResponse property of the
		/// <see cref="Gravatar.NET.GravatarServiceResponse"/> instance returned by this method
		/// The response is returned to the callback method</returns>
		public void SaveUrlAsync(string url, GravatarImageRating rating, GravatarCallBack callback, object state) {
			var request = GetSaveUrlMethodRequest(url, rating);
			ExecuteGravatarMethodAsync(request, callback, state);
		}

		/// <summary>
		/// Use a userimage as a gravatar for one of more addresses on this account - called asynchronously 
		/// </summary>
		/// <param name="userImage">The userimage you wish to use</param>
		/// <param name="addresses">A list of the email addresses you wish to use this userimage for</param>
		/// <param name="callback">The callback activated when action finishes</param>
		/// <param name="state">Custom parameter to callback</param>
		/// <returns>If the method call is successful, the result will be returned using the MultipleOperationResponse property
		/// of the <see cref="Gravatar.NET.GravatarServiceResponse"/> instance returned by this method
		/// The response is returned to the callback method</returns>
		public void UseUserImageAsync(string userImage, string[] addresses, GravatarCallBack callback, object state) {
			var request = GetUseUserImagesMethodRequest(userImage, addresses);
			ExecuteGravatarMethodAsync(request, callback, state);
		}

		/// <summary>
		/// Remove a userimage from the account and any email addresses with which it is associated - called asynchronously 
		/// </summary>
		/// <param name="userImage">The user image you wish to remove from the account</param>
		/// <param name="callback">The callback activated when action finishes</param>
		/// <param name="state">Custom parameter to callback</param>
		/// <returns>If the method call is successful, the result will be returned using the BooleanResponse property of the
		/// <see cref="Gravatar.NET.GravatarServiceResponse"/> instance returned by this method
		/// The response is returned to the callback method</returns>
		public void DeleteUserImageAsync(string userImage, GravatarCallBack callback, object state) {
			var request = GetDeleteImageMethodRequest(userImage);
			ExecuteGravatarMethodAsync(request, callback, state);
		}
		#endregion

		#region Get Gravatar Photo URL

		/// <summary>
		/// Gets the currently active Gravatar image URL for the email address used to instantiate the service with
		/// Throws a <see cref="GravatarEmailHashFailedException"/> if the provided email address is invalid
		/// </summary>
		/// <returns>The Gravatar image URL</returns>
		public string GetGravatarUrl() {
			return GetGravatarUrlForAddress(Email);
		}

		/// <summary>
		/// Gets the currently active Gravatar image URL for the email address used to instantiate the service with		
		/// Throws a <see cref="GravatarEmailHashFailedException"/> if the provided email address is invalid
		/// </summary>
		/// <param name="pars">The available parameters passed by the request to Gravatar when retrieving the image</param>
		/// <returns>The Gravatar image URL</returns>
		public string GetGravatarUrl(GravatarUrlParameters pars) {
			return GetGravatarUrlForAddress(Email, pars);
		}

		/// <summary>
		/// Gets the currently active Gravatar image URL for the email address supplied to this method call
		/// Throws a <see cref="GravatarEmailHashFailedException"/> if the provided email address is invalid
		/// </summary>
		/// <param name="address">The address to retireve the image for</param>
		/// <param name="alreadyHashed">Whether the supplied address is already hashed</param>
		/// <returns>The Gravatar image URL</returns>
		public static string GetGravatarUrlForAddress(string address, bool alreadyHashed = false) {
			return GetGravatarUrlForAddress(address, null, alreadyHashed);
		}

		/// <summary>
		/// Gets the currently active Gravatar image URL for the email address supplied to this method call
		/// Throws a <see cref="GravatarEmailHashFailedException"/> if the provided email address is invalid
		/// </summary>
		/// <param name="address">The address to retireve the image for</param>
		/// /// <param name="pars">The available parameters passed by the request to Gravatar when retrieving the image</param>
		/// <param name="alreadyHashed">Whether the supplied address is already hashed</param>
		/// <returns>The Gravatar image URL</returns>
		public static string GetGravatarUrlForAddress(string address, GravatarUrlParameters pars, bool alreadyHashed = false) {
			var sbResult = new StringBuilder(GravatarImageUrlBase);
			sbResult.Append(alreadyHashed ? address : HashEmailAddress(address));

			if (pars != null) {
				sbResult.Append("?");

				if (pars.Size > 0) sbResult.AppendFormat(UrlParSize, pars.Size);
				if (pars.Rating != GravatarImageRating.G) sbResult.AppendFormat(UrlParRating, pars.Rating.ToString().ToLower());

				if (pars.DefaultOption != GravatarDefaultUrlOptions.None) {
					if (pars.DefaultOption == GravatarDefaultUrlOptions.Custom) {
						sbResult.AppendFormat(UrlParDefault, HttpUtility.UrlEncode(pars.CustomDefaultUrl));
					} else {
						sbResult.AppendFormat(UrlParDefault, (pars.DefaultOption == GravatarDefaultUrlOptions.Error ? UrlPar404 : pars.DefaultOption.ToString().ToLower() ));
					}
				}
			}

			return sbResult.ToString();
		}

		#endregion

		#region Get Gravatar Profile
		/// <summary>
		/// Gets the Gravatar profile for the email address used to instantiate the service with
		/// Throws a <see cref="GravatarEmailHashFailedException"/> if the provided email address is invalid
		/// </summary>
		/// <returns>The Gravatar profile</returns>
		public GravatarProfile GetGravatarProfile() {
			return GetGravatarProfile(Email);
		}

		/// <summary>
		/// Gets the Gravatar profile for the email address supplied to this method call
		/// Throws a <see cref="GravatarEmailHashFailedException"/> if the provided email address is invalid
		/// </summary>
		/// <param name="address">The address to retireve the prpfile for</param>
		/// <param name="alreadyHashed">Whether the supplied address is already hashed</param>
		/// <returns>The Gravatar profile</returns>
		public static GravatarProfile GetGravatarProfile(string address, bool alreadyHashed=false) {
			try {
				var profileUrl = String.Format("{0}/{1}.xml", GravatarProfileUrlBase, alreadyHashed ? address : HashEmailAddress(address));

				var webRequest = (HttpWebRequest)WebRequest.Create(profileUrl);
				webRequest.ContentType = "text/xml";
				var webResponse = webRequest.GetResponse();

				return new GravatarProfile(webResponse);
			}
			catch (WebException) {
				return null;
			}
		}

		#endregion

		#region Create Methods Requests
		private GravatarServiceRequest GetExistsMethodRequest(IEnumerable<string> addresses, bool alreadyHashed = false) {
			return new GravatarServiceRequest {
				Email = Email,
				MethodName = GravatarConstants.METHOD_EXISTS,
				Parameters = new List<GravatarParameter> {
					GravatarParameter.NewArrayParameter(ParHashes, addresses.Select(adr => GravatarParameter.NewStringParamter(String.Empty, alreadyHashed ? adr : HashEmailAddress(adr)))),
					GravatarParameter.NewStringParamter(ParPassword, Password)
				}
			};
		}

		private GravatarServiceRequest GetTestMethodRequest() {
			return new GravatarServiceRequest {
				Email = Email, 
				MethodName = GravatarConstants.METHOD_TEST, 
				Parameters = new List<GravatarParameter> {
					GravatarParameter.NewStringParamter(ParPassword, Password)
				}
			};
		}

		private GravatarServiceRequest GetAddressesMethodRequest() {
			return new GravatarServiceRequest {
				Email = Email, 
				MethodName = GravatarConstants.METHOD_ADDRESSES, 
				Parameters = new List<GravatarParameter> {
					GravatarParameter.NewStringParamter(ParPassword, Password)
				}
			};
		}

		private GravatarServiceRequest GetUserImagesMethodRequest() {
			return new GravatarServiceRequest {
				Email = Email, 
				MethodName = GravatarConstants.METHOD_USER_IMAGES, 
				Parameters = new List<GravatarParameter> {
					GravatarParameter.NewStringParamter(ParPassword, Password)
				}
			};
		}

		private GravatarServiceRequest GetSaveDataMethodRequest(byte[] data, GravatarImageRating rating) {
			return new GravatarServiceRequest {
				Email = Email,
				MethodName = GravatarConstants.METHOD_SAVE_DATA,
				Parameters = new List<GravatarParameter> {
					GravatarParameter.NewStringParamter(ParData, Convert.ToBase64String(data)),
					GravatarParameter.NewIntegerParameter(ParRating, (int)rating),
					GravatarParameter.NewStringParamter(ParPassword, Password)
				}
			};
		}

		private GravatarServiceRequest GetSaveUrlMethodRequest(string url, GravatarImageRating rating) {
			return new GravatarServiceRequest { 
				Email = Email, 
				MethodName = GravatarConstants.METHOD_SAVE_URL, 
				Parameters = new List<GravatarParameter> {
					GravatarParameter.NewStringParamter(ParUrl, url),
					GravatarParameter.NewIntegerParameter(ParRating, (int)rating),
					GravatarParameter.NewStringParamter(ParPassword ,Password)
				}
			};
		}

		private GravatarServiceRequest GetUseUserImagesMethodRequest(string userImage, IEnumerable<string> addresses) {
			return new GravatarServiceRequest { 
				Email = Email, 
				MethodName = GravatarConstants.METHOD_USE_USER_IMAGE, 
				Parameters = new List<GravatarParameter> {
					GravatarParameter.NewStringParamter(ParUserImage, userImage),
					GravatarParameter.NewArrayParameter(ParAddresses, addresses.Select(adr => GravatarParameter.NewStringParamter(String.Empty, adr))),
					GravatarParameter.NewStringParamter(ParPassword, Password)
				}
			};
		}

		private GravatarServiceRequest GetDeleteImageMethodRequest(string userImage) {
			return new GravatarServiceRequest {
				Email = Email,
				MethodName = GravatarConstants.METHOD_DELETE_USER_IMAGE,
				Parameters = new List<GravatarParameter> {
					GravatarParameter.NewStringParamter(ParUserImage, userImage),
					GravatarParameter.NewStringParamter(ParPassword, Password)
				}
			};
		}

		#endregion
	}
}
