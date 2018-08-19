/*
 * Copyright (c) 2008-2018 Company for Advanced Supercomputing Solutions LTD
 * Author: Mordechai Botrashvily (support@cass-hpc.com)
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to 
 * deal in the Software without restriction, including without limitation the 
 * rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
 * sell copies of the Software, and to permit persons to whom the Software is 
 * furnished to do so, subject to the following conditions:
 * The above copyright notice and this permission notice shall be included in 
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING 
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
 * IN THE SOFTWARE.
 */

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
