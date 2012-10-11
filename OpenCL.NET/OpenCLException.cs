using System;

namespace CASS.OpenCL
{
    /// <summary>
    /// Represents an OpenCL(TM) exception that occured in one of the API functions.
    /// </summary>
    public class OpenCLException : Exception
    {
        /// <summary>
        /// Creates a new exception instance with given error code.
        /// </summary>
        /// <param name="error">OpenCL(TM) error code.</param>
        public OpenCLException(CLError error)
        {
            OpenCLErrorCode = error;
        }

        /// <summary>
        /// Gets OpenCL(TM) error string and code.
        /// </summary>
        public override string Message
        {
            get
            {
                return string.Format("OpenCL error {0} (Code: {1})", OpenCLErrorCode, (int)OpenCLErrorCode);
            }
        }

        /// <summary>
        /// OpenCL(TM) error code.
        /// </summary>
        public CLError OpenCLErrorCode { get; private set; }
    }
}
