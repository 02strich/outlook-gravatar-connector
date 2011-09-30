using System;
using System.Runtime.Serialization;

namespace Gravatar.NET.Data
{
	/// <summary>
	/// Represents a Gravatar user image details
	/// </summary>
	[DataContract]
	public class GravatarUserImage
	{
		internal GravatarUserImage() { }

		/// <summary>
		/// The identifier of the image
		/// </summary>
		[DataMember]
		public string Name { get; internal set; }
		
		/// <summary>
		/// The rating associated with the image
		/// </summary>
		[DataMember]
		public GravatarImageRating Rating { get; internal set; }
		
		/// <summary>
		/// The URL of the image
		/// </summary>
		[DataMember]
		public string Url {get; internal set;}
	}
}
