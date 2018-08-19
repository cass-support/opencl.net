/*
 * Copyright 2018 Mordechai Botrashvily (support@cass-hpc.com)
 * Company for Advanced Supercomputing Solutions LTD
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

namespace CASS.OpenCL.OpenGL
{
    #region Enums
    public enum CLGLObjectType
    {
        Buffer = 0x2000,
        Texture2D = 0x2001,
        Texture3D = 0x2002,
        RenderBuffer = 0x2003,
    }

    public enum CLGLTextureInfo
    {
        TextureTarget = 0x2004,
        MipMapLevel = 0x2005,
    }

    public enum CLGLContextInfo
    {
        CurrentDeviceForGLContext = 0x2006,
        DevicesForGLContext = 0x2007,
    }
    #endregion
}
