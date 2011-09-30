using System;

namespace Gravatar.NET.Exceptions {
    public class GravatarEmailHashFailedException : Exception {
        public GravatarEmailHashFailedException(string address, Exception ex)
            : base(String.Format("Address: '{0}' failed to be hashed", address), ex) {
        }
    }

    public class GravatarInvalidResponseXmlException : Exception {
        public GravatarInvalidResponseXmlException() : base("The XML received from the Gravatar server is invalid") {
        }
    }

    public class GravatarRequestException : Exception {
        public GravatarRequestException(Exception exception)
            : base("Failed to make request to Gravatar server", exception) {
        }
    }

    public class GravatarUrlInvalidSizeExcetion : ArgumentException {
        public GravatarUrlInvalidSizeExcetion(int size)
            : base(String.Format("Requested size {0} is not a valid value between 1 and 512", size), "Size") {
        }
    }

    public class UnknownGravatarMethodException : ArgumentException {
        public UnknownGravatarMethodException(string method)
            : base(String.Format("An unknown method '{0}' was called", method)) {
        }
    }
}
