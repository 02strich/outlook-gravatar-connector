using System;
using System.Diagnostics;
using System.Drawing;
using System.Xml.Serialization;
using System.IO;

namespace GravatarOSC
{
    class HelperMethods
    {
        //OSC Errors
        // ReSharper disable InconsistentNaming
        public const int OSC_E_FAIL             = unchecked((int)0x80004005);   // General failure error
        public const int OSC_E_INTERNAL_ERROR   = unchecked((int)0x80041400);   // An internal error has occurred due to an invalid operation
        public const int OSC_E_INVALIDARG       = unchecked((int)0x80070057);   // Invalid argument error
        public const int OSC_E_AUTH_ERROR       = unchecked((int)0x80041404);   // Authentication has failed on the network of the social network
        public const int OSC_E_NO_CHANGES       = unchecked((int)0x80041406);   // No changes have occurred since the last synchronization
        public const int OSC_E_COULDNOTCONNECT  = unchecked((int)0x80041402);   // No connection is available to connect to the social network
        public const int OSC_E_NOT_FOUND        = unchecked((int)0x80041405);   // A resource cannot be found
        public const int OSC_E_NOT_IMPLEMENTED  = unchecked((int)0x80004001);   // Not yet implemented
        public const int OSC_E_OUT_OF_MEMORY    = unchecked((int)0x8007000E);   // Out of memory error
        public const int OSC_E_PERMISSION_DENIED = unchecked((int)0x80041403);  // Permission for the resource is denied by the OSC provider
        public const int OSC_E_VERSION          = unchecked((int)0x80041401);   // The provider does not support this version of OSC provider extensibility
        // ReSharper restore InconsistentNaming

        public static string SerializeObjectToString(object o)
        {
            var writer = new StringWriter();
            var serializer = new XmlSerializer(o.GetType());
            serializer.Serialize(writer, o);

            return writer.ToString();
        }

        public static T DeserializeStringToObject<T>(String s) {
            var serializer = new XmlSerializer(typeof(T));
            return (T) serializer.Deserialize(new StringReader("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n" + s));
        }

        public static int GetTrueIndex(bool[] array) {
            if (array == null) {
                Debug.WriteLine("HelperMethods:GetTrueIndex: got empty array");
                return -1;
            }
                
            for (var i = 0; i < array.Length; i++)
                if (array[i])
                    return i;

            return -1;
        }

        public static byte[] GetProviderJpeg() {
            var ic = new ImageConverter();
            var b = Properties.Resources.Logo;
            return (byte[])ic.ConvertTo(b, typeof(byte[]));
        }
    }
}
