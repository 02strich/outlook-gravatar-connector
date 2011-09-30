using System;

namespace Gravatar.NET.Data
{
	/// <summary>
	/// A list of possible Gravatar parameters type
	/// </summary>
	public enum GravatarParType
	{
		String,
		Integer,
		Bool,
		Array,
		Struct
	}

	/// <summary>
	/// A list of possible Gravatar image rating
	/// </summary>
	public enum GravatarImageRating
	{
		G = 0,
		PG = 1,
		R = 2,
		X = 3
	}

	/// <summary>
	/// A list of possible gravatar default option for not found images 
	/// </summary>
	public enum GravatarDefaultUrlOptions
	{
		/// <summary>
		/// No default URL option is provided
		/// </summary>
		None,
		/// <summary>
		/// Return the Identicon image if gravatar image not found
		/// </summary>
		Identicon,
		/// <summary>
		/// Return the Monsterid image if gravatar image not found
		/// </summary>
		Monsterid,
		/// <summary>
		/// Return the Wavatar image if gravatar image not found
		/// </summary>
		Wavatar,
		/// <summary>
		/// Return a 404 HTTP error if gravatar image not found
		/// </summary>
		Error,
		/// <summary>
		/// Return the specified image (CustomDefault) if gravatar image not found
		/// </summary>
		Custom
	}
}
