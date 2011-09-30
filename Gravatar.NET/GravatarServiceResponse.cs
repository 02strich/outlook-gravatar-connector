using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using Gravatar.NET.Data;
using Gravatar.NET.Exceptions;

namespace Gravatar.NET
{
	/// <summary>
	/// Represents the response returned from Gravatar after a request has been made.
	/// The object has different properties representing the different types of results which can
	/// be returned by a method call.
	/// </summary>
	public class GravatarServiceResponse
	{
		/// <summary>
		/// Create a new Gravatar response for the method that was called.
		/// Throws a <see cref="GravatarInvalidResponseXmlException">GravatarInvalidResponseXmlException</see>
		/// if the XML returned from Gravatar is in an unexpected format
		/// </summary>		
		internal GravatarServiceResponse(WebResponse webResponse, string methodName) {						
			using (var responseStream = webResponse.GetResponseStream())
			    if (responseStream != null)
			        using (var reader = new StreamReader(responseStream))
			            InitializeGravatarResponse(reader.ReadToEnd(), methodName);
		}

		/// <summary>
		/// Create a new Gravatar response with the exception information that occurred
		/// while making a request
		/// </summary>
		/// <param name="ex"></param>
		internal GravatarServiceResponse(Exception ex) {
			IsError = true;
			ErrorCode = GravatarConstants.CLIENT_ERROR;
			ErrorInfo = ex.Message;
		}

		#region Error Info
		/// <summary>
		/// Indication whether the method call resulted in an Error or not
		/// </summary>
		public bool IsError { get; private set; }

		/// <summary>
		/// The Gravatar error code
		/// </summary>
		public int ErrorCode { get; private set; }

		/// <summary>
		/// Additional information about the Gravatar error
		/// </summary>
		public string ErrorInfo { get; private set; }
		#endregion

		#region Properties
		/// <summary>
		/// The raw XML returned from Gravatar as a result of a method call
		/// </summary>
		public string GravatarResponseXml { get; private set; }
		
		/// <summary>
		/// The list of all parameters returned from the method call
		/// </summary>
		public IEnumerable<GravatarParameter> ResponseParameters { get; private set; }
		#endregion

		#region Method Responses
		/// <summary>
		/// Used when the result of a method call is of an integer type
		/// </summary>
		public int IntegerResponse { get; internal set; }

		/// <summary>
		/// Used when the result of a method call is of a Boolean type
		/// </summary>
		public bool BooleanResponse { get; internal set; }

		/// <summary>
		/// Used when the result of a method call is a list of user images
		/// </summary>
		public IEnumerable<GravatarUserImage> ImagesResponse { get; internal set; }

		/// <summary>
		/// Used when the result of a method is a Boolean indication for multiple operations such as updating a list
		/// of email addresses with an image
		/// </summary>
		public bool[] MultipleOperationResponse { get; internal set; }

		/// <summary>
		/// Used when the result of a method call is a list of email addresses
		/// </summary>
		public IEnumerable<GravatarAddress> AddressesResponse { get; internal set; }

		/// <summary>
		/// Used when the operation is an image save operation
		/// </summary>
		public GravatarSaveResponse SaveResponse { get; internal set; }
		#endregion

		#region Helper Methods
		private void InitializeGravatarResponse(string response, string methodName) {
		    GravatarResponseXml = response;

			try {
				var responseXmlDoc = XDocument.Parse(response);

				var rspElement = responseXmlDoc.Element(GravatarConstants.XML_METHOD_RESPONSE);
				if (rspElement == null) throw new GravatarInvalidResponseXmlException();

				XElement firstStruct = (from s in responseXmlDoc.Descendants(GravatarConstants.XML_STRUCT) select s).FirstOrDefault();
				var pars = firstStruct != null ? GetGravatarXmlMembers(firstStruct) : GetGravatarXmlParameters(responseXmlDoc);

				if (rspElement.Element(GravatarConstants.XML_FAULT) == null)  {
					//request was accepted, no error sent back
					ResponseParameters = pars;
				} else {
					IsError = true;
					ErrorCode = pars.First().IntegerValue;
					ErrorInfo = pars.Last().StringValue;
				}

				// set the proper response based on the method that was called
				GravatarResponseParser.ParseResponseForMethod(methodName, this); 
			} catch (Exception) {
				throw new GravatarInvalidResponseXmlException();
			}
		}

		private IEnumerable<GravatarParameter> GetGravatarXmlParameters(XContainer responseDoc) {
			return from par in responseDoc.Descendants(GravatarConstants.XML_PARAM) select GetParameterFromXml(par);
		}

		private IEnumerable<GravatarParameter> GetGravatarXmlMembers(XContainer structElement) {
			return from member in structElement.Elements(GravatarConstants.XML_MEMBER) select GetParameterFromXml(member);
		}

		private GravatarParameter GetParameterFromXml(XContainer memberXml) {
			var par = new GravatarParameter();

			// set name
			var nameElm = memberXml.Element(GravatarConstants.XML_NAME);
			if (nameElm != null)
				par.Name = nameElm.Value;

			// set value
			var valType = memberXml.Element(GravatarConstants.XML_VALUE).Elements().First();
			if (valType.Name == GravatarConstants.XML_STRUCT) {
				par.Type = GravatarParType.Struct;
				par.StructValue = new GravatarStruct { Parameters = GetGravatarXmlMembers(valType) };
			} else if (valType.Name == GravatarConstants.XML_ARRAY) {
				par.Type = GravatarParType.Array;
				par.ArrayValue = from value in valType.Descendants(GravatarConstants.XML_VALUE) select GetParameterFromXml(value);
			} else if (valType.Name == GravatarConstants.XML_INT) {
				par.Type = GravatarParType.Integer;
				par.IntegerValue = int.Parse(valType.Value);
			} else if (valType.Name == GravatarConstants.XML_STRING) {
				par.Type = GravatarParType.String;
				par.StringValue = valType.Value;
			} else if (valType.Name == GravatarConstants.XML_BOOL || valType.Name == GravatarConstants.XML_BOOLEAN) {
				par.Type = GravatarParType.Bool;

				if (valType.Value == "1")
					par.BooleanValue = true;
				else if (valType.Value == "0")
					par.BooleanValue = false;
				else
					par.BooleanValue = bool.Parse(valType.Value);
			}

			return par;
		}
		#endregion
	}
}
