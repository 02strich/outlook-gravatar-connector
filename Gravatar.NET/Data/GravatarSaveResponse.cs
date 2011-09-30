using System;
using System.Runtime.Serialization;

namespace Gravatar.NET.Data
{
	/// <summary>
	/// Represents the result of a Save method call
	/// </summary>
	[DataContract]
	public class GravatarSaveResponse
	{
		internal GravatarSaveResponse()
		{
		}

		/// <summary>
		/// Whether the save operation was successful
		/// </summary>
		[DataMember]
		public bool Success { get; internal set; }

		/// <summary>
		/// The ID of the newly saved image
		/// </summary>
		[DataMember]
		public string SavedImageId { get; internal set; }
	}
}
