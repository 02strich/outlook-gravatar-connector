using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using Gravatar.NET.Data;

namespace Gravatar.NET
{
	/// <summary>
	/// Represents a single request to the Gravatar server, encapsulating a Gravatar method
	/// </summary>
	public class GravatarServiceRequest
	{
		#region Properties
		public string Email { get; internal set; }

		/// <summary>
		/// The Gravatar method called in the context of this request
		/// </summary>
		public string MethodName { get; internal set; }

		/// <summary>
		/// The list of parameters sent to Gravatar in this method call
		/// </summary>
		public List<GravatarParameter> Parameters { get; internal set;}
		#endregion

		internal GravatarServiceRequest() {
		}
		
		private string CreateGravatarRequestXml() {
			var sb = new StringBuilder();

			using (var sw = new StringWriter(sb)) {
				using (var xw = new XmlTextWriter(sw)) {
					xw.WriteProcessingInstruction("xml", "version='1.0'");

					xw.WriteStartElement(GravatarConstants.XML_METHODCALL);
					xw.WriteElementString("methodName", MethodName);
					
					xw.WriteStartElement(GravatarConstants.XML_PARAMS);
					xw.WriteStartElement(GravatarConstants.XML_PARAM);
					xw.WriteStartElement(GravatarConstants.XML_VALUE);
					xw.WriteStartElement(GravatarConstants.XML_STRUCT);

					foreach (var p in Parameters)
						WriteGravatarRequestParam(xw, p);

					xw.WriteEndElement(); // struct
					xw.WriteEndElement(); // value
					xw.WriteEndElement(); // param
					xw.WriteEndElement(); // params					
					xw.WriteEndElement(); // methodCall					
				}
			}

			return sb.ToString();
		}

		private static void WriteGravatarRequestParam(XmlTextWriter writer, GravatarParameter par) {
			writer.WriteStartElement(GravatarConstants.XML_MEMBER);
			writer.WriteElementString(GravatarConstants.XML_NAME, par.Name);
			
			if (par.Type == GravatarParType.Array) {
				writer.WriteStartElement(GravatarConstants.XML_VALUE);
				writer.WriteStartElement(GravatarConstants.XML_ARRAY);
				writer.WriteStartElement(GravatarConstants.XML_DATA);

				foreach (var arrPar in par.ArrayValue) {
					writer.WriteStartElement(GravatarConstants.XML_VALUE);
					
					switch (arrPar.Type) {
						case GravatarParType.Bool:
							writer.WriteElementString(GravatarConstants.XML_BOOL, arrPar.StringValue);
							break;
						case GravatarParType.Integer:
							writer.WriteElementString(GravatarConstants.XML_INT, arrPar.StringValue);
							break;
						default:
							writer.WriteElementString(GravatarConstants.XML_STRING, arrPar.StringValue);
							break;
					}
					
					writer.WriteEndElement(); //value
				}

				writer.WriteEndElement(); //data
				writer.WriteEndElement(); //array
				writer.WriteEndElement(); //value
			}
			else
			{
				writer.WriteElementString(GravatarConstants.XML_VALUE, par.StringValue);
			}

			writer.WriteEndElement(); //member			
		}

		/// <summary>
		/// returns the XML structure for the method call
		/// </summary>		
		public override string ToString()
		{
			return CreateGravatarRequestXml();
		}
	}
}
