using System;

namespace Gravatar.NET
{
	// ReSharper disable InconsistentNaming
	public class GravatarConstants
	{
		#region Error codes

		public const int USE_SECURE_URL = -7;
		public const int INTERNAL_ERROR = -8;
		public const int AUTHENTICATION_ERROR = -9;
		public const int PARAMETER_MISSING = -10;
		public const int PARAMETER_INCORRECT = -11;
		public const int MISC_ERROR = -100;
		public const int CLIENT_ERROR = -1000;
		#endregion

		#region Method names
		public const string METHOD_TEST = "grav.test";
		public const string METHOD_USER_IMAGES = "grav.userimages";
		public const string METHOD_EXISTS = "grav.exists";
		public const string METHOD_ADDRESSES = "grav.addresses";
		public const string METHOD_SAVE_DATA = "grav.saveData";
		public const string METHOD_SAVE_URL = "grav.saveUrl";
		public const string METHOD_USE_USER_IMAGE = "grav.useUserimage";
		public const string METHOD_DELETE_USER_IMAGE = "grav.deleteUserimage";
		#endregion

		#region XML literarls

		public const string XML_METHODCALL = "methodCall";
		public const string XML_PARAMS = "params";
		public const string XML_PARAM = "param";
		public const string XML_VALUE = "value";
		public const string XML_STRUCT = "struct";
		public const string XML_ARRAY = "array";
		public const string XML_DATA = "data";
		public const string XML_NAME = "name";
		public const string XML_FAULT = "fault";
		public const string XML_MEMBER = "member";
		public const string XML_BOOL = "bool";
		public const string XML_BOOLEAN = "boolean";
		public const string XML_INT = "int";
		public const string XML_STRING = "string";
		public const string XML_METHOD_RESPONSE = "methodResponse";
		#endregion
	}
	// ReSharper restore InconsistentNaming
}
