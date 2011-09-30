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
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using Gravatar.NET.Exceptions;

namespace Gravatar.NET
{
	/// <summary>
	/// References a method to call when a Asynchronous Gravatar API method call completes
	/// </summary>	
	public delegate void GravatarCallBack(GravatarServiceResponse response, object state);

	/// <summary>
	/// Represents the request state when async processing Gravatar requests
	/// </summary>
	class GravatarRequestState {
		public HttpWebRequest WebRequest { get; set; }
		public GravatarServiceRequest GravatarRequest { get; set; }
		public object UserState { get; set; }
		public GravatarCallBack CallBack { get; set; }
	}

	public sealed partial class GravatarService {
		private static string HashEmailAddress(string address) {
			try {
				MD5 md5 = new MD5CryptoServiceProvider();

				var hasedBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(address));
				var sb = new StringBuilder();

				for (var i = 0; i < hasedBytes.Length; i++)
					sb.Append(hasedBytes[i].ToString("X2"));

				return sb.ToString().ToLower();
			} catch (Exception ex) {
				throw new GravatarEmailHashFailedException(address, ex);
			}
		}

		private GravatarServiceResponse ExecuteGravatarMethod(GravatarServiceRequest request) {
			var webRequest = (HttpWebRequest) WebRequest.Create(String.Format(GravatarApiUrl, HashEmailAddress(Email)));
			var requestData = Encoding.UTF8.GetBytes(request.ToString());

			webRequest.Method = "POST";
			webRequest.ContentType = "text/xml";
			webRequest.ContentLength = requestData.Length;

			try {
				using (var requestStream = webRequest.GetRequestStream()) {
					requestStream.Write(requestData, 0, requestData.Length);
					requestStream.Close();
				}

				var webResponse = (HttpWebResponse)webRequest.GetResponse();
				return new GravatarServiceResponse(webResponse, request.MethodName);
			} catch (Exception ex) {
				return new GravatarServiceResponse(ex);
			}
		}

		private void ExecuteGravatarMethodAsync(GravatarServiceRequest request, GravatarCallBack callback, object state) {
			var webRequest = (HttpWebRequest) WebRequest.Create(String.Format(GravatarApiUrl, HashEmailAddress(Email)));

			webRequest.Method = "POST";
			webRequest.ContentType = "text/xml";
						
			webRequest.BeginGetRequestStream(OnGetRequestStream,  new GravatarRequestState { 
				WebRequest = webRequest, 
				GravatarRequest = request,
				UserState = state,
				CallBack = callback
			});			
		}

		private static void OnGetRequestStream(IAsyncResult ar) {
			var requestState = (GravatarRequestState) ar.AsyncState;

			try {
				var data = Encoding.UTF8.GetBytes(requestState.GravatarRequest.ToString());

				using (Stream requestStream = requestState.WebRequest.EndGetRequestStream(ar)) {
					requestStream.Write(data, 0, data.Length);
					requestStream.Close();
				}

				requestState.WebRequest.BeginGetResponse(OnGetResponse, requestState);
			} catch (Exception ex) {
				requestState.CallBack(new GravatarServiceResponse(ex), requestState.UserState);
			}
		}

		private static void OnGetResponse(IAsyncResult ar) {
			var requestState = (GravatarRequestState)ar.AsyncState;

			try {	
				var webResponse = (HttpWebResponse)requestState.WebRequest.EndGetResponse(ar);

				var gravatarResponse = new GravatarServiceResponse(webResponse, requestState.GravatarRequest.MethodName);
				requestState.CallBack(gravatarResponse, requestState.UserState);
			}
			catch (Exception ex) {
				requestState.CallBack(new GravatarServiceResponse(ex), requestState.UserState);
			}
		}
	}
}
