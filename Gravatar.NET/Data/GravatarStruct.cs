using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Gravatar.NET.Data
{
	/// <summary>
	/// Represents a Gravatar struct parameter
	/// </summary>
	[DataContract]
	public class GravatarStruct
	{
		internal GravatarStruct() { }

		[DataMember]
		public IEnumerable<GravatarParameter> Parameters { get; set; }
	}
}
