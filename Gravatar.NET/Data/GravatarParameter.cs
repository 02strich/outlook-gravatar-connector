using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Gravatar.NET.Data
{
	[DataContract]
	public class GravatarParameter
	{
		private bool _boolValue;
		private int _intValue;

		internal GravatarParameter() { }

		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public string StringValue { get; set; }

		[DataMember]
		public int IntegerValue 
		{
			get { return _intValue; }
			set
			{
				_intValue = value;
				StringValue = value.ToString();
			}
		}

		[DataMember]
		public bool BooleanValue 
		{
			get { return _boolValue; }
			set
			{
				_boolValue = value;
				StringValue = value.ToString();
			}
		}

		[DataMember]
		public IEnumerable<GravatarParameter> ArrayValue { get; set; }

		[DataMember]
		public GravatarStruct StructValue { get; set; }

		[DataMember]
		public GravatarParType Type { get; set; }

		#region Static Initializers

		public static GravatarParameter NewStringParamter(string name, string value)
		{
			return new GravatarParameter { Name = name, Type = GravatarParType.String, StringValue = value };
		}

		public static GravatarParameter NewIntegerParameter(string name, int value)
		{
			return new GravatarParameter { Name = name, Type = GravatarParType.Integer, IntegerValue = value };
		}

		public static GravatarParameter NewArrayParameter(string name, IEnumerable<GravatarParameter> value)
		{
			return new GravatarParameter { Name = name, Type = GravatarParType.Array, ArrayValue = value };
		}

		public static GravatarParameter NewStructParamater(string name, GravatarStruct value)
		{
			return new GravatarParameter { Name = name, Type = GravatarParType.Struct, StructValue = value };
		}

		public static GravatarParameter NewBooleanParameter(string name, bool value)
		{
			return new GravatarParameter { Name = name, Type = GravatarParType.Bool, BooleanValue = value };
		}

		#endregion
	}
}
