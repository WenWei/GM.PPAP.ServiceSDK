using System;

namespace GM.PPAP.ServiceSDK.Exceptions
{
    /// <summary>
    /// Base TwilioException
    /// </summary>
    public class GmServiceException : Exception
    {
        /// <summary>
        /// Create an empty TwilioException
        /// </summary>
        public GmServiceException() { }

        /// <summary>
        /// Create a TwilioException from an error message
        /// </summary>
        /// <param name="message">Error message</param>
        public GmServiceException(string message) : base(message) { }

        /// <summary>
        /// Create a TwilioException from message and another exception
        /// </summary>
        /// <param name="message">Error message</param>
        /// <param name="exception">Original Exception</param>
        public GmServiceException(string message, Exception exception) : base(message, exception) { }
    }

    /// <summary>
    /// Exception related to connection errors
    /// </summary>
    public class ApiConnectionException : GmServiceException
    {
        /// <summary>
        /// Create an ApiConnectionException from a message
        /// </summary>
        /// <param name="message">Error message</param>
        public ApiConnectionException(string message) : base(message) { }

        /// <summary>
        /// Create an ApiConnectionException from a message and another Exception
        /// </summary>
        /// <param name="message">Error message</param>
        /// <param name="exception">Original Exception</param>
        public ApiConnectionException(string message, Exception exception) : base(message, exception) { }
    }

    /// <summary>
    /// Exception related to Authentication Errors
    /// </summary>
    public class AuthenticationException : GmServiceException
    {
        /// <summary>
        /// Create AuthenticationException from an error messsage
        /// </summary>
        /// <param name="message">Error message</param>
        public AuthenticationException(string message) : base(message) { }
    }
}