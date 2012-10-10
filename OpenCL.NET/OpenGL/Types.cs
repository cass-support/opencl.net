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
        CurrentDeviceForGLContextKHR = 0x2006,
        DevicesForGLContextKHR = 0x2007,
    }
    #endregion
}
