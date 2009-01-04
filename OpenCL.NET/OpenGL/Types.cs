using System;
using System.Collections.Generic;
using System.Text;

namespace GASS.OpenCL.OpenGL
{
    #region Enums
    public enum CLGLObjectType
    {
        Buffer = 0x2000,
        Texture2D = 0x2001,
        TextureRectangle = 0x2002,
        Texture3D = 0x2003,
        RenderBuffer = 0x2004,
    }

    public enum CLGLTextureInfo
    {
        TextureTarget = 0x2005,
        MipMapLevel = 0x2006,
    }
    #endregion
}
