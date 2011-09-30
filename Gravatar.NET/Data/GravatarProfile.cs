using System;
using System.IO;
using System.Net;
using System.Xml.Linq;
using Gravatar.NET.Exceptions;

namespace Gravatar.NET.Data
{
	public class GravatarProfile
	{
		public string ProfileUrl { get; private set; }
		public string PreferredUsername { get; set; }
		public string ThumbnailUrl { get; set; }
		
		public string DisplayName { get; set; }
		public string GivenName { get; set; }
		public string FamilyName { get; set; }

		public string CurrentLocation { get; set; }

		internal GravatarProfile(WebResponse webResponse) {
			using (var responseStream = webResponse.GetResponseStream())
				if (responseStream != null)
					using (var reader = new StreamReader(responseStream))
						LoadFromXml(reader.ReadToEnd());
		}

		// ReSharper disable PossibleNullReferenceException
		private void LoadFromXml(string xmlData) {
			try {
				var responseXmlDoc = XDocument.Parse(xmlData);
				var rspElement = responseXmlDoc.Element("response");
				var entryElement = rspElement.Element("entry");

				// profileUrl
				var profileUrlElement = entryElement.Element("profileUrl");
				if (profileUrlElement != null)
					ProfileUrl = profileUrlElement.Value;

				// preferredUsername
				var preferredUsernameElement = entryElement.Element("preferredUsername");
				if (preferredUsernameElement != null)
					PreferredUsername = preferredUsernameElement.Value;

				// thumbnailUrl
				var thumbnailUrlElement = entryElement.Element("thumbnailUrl");
				if (thumbnailUrlElement != null)
					ThumbnailUrl = thumbnailUrlElement.Value;

				// displayName
				var displayNameElement = entryElement.Element("displayName");
				if (displayNameElement != null)
					DisplayName = displayNameElement.Value;

				// currentLocation
				var currentLocationElement = entryElement.Element("currentLocation");
				if (currentLocationElement != null)
					CurrentLocation = currentLocationElement.Value;

				// name
				var nameElement = entryElement.Element("name");
				if (nameElement != null) {
					var givenNameElement = nameElement.Element("givenName");
					if (givenNameElement != null)
						GivenName = givenNameElement.Value;

					var familyNameElement = nameElement.Element("familyName");
					if (familyNameElement != null)
						FamilyName = familyNameElement.Value;
				}

			} catch (Exception) {
				throw new GravatarInvalidResponseXmlException();
			}
		}
		// ReSharper restore PossibleNullReferenceException
	}
}
