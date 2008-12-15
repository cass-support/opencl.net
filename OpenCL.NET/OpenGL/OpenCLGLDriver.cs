using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace GASS.OpenCL
{
    #region Enums
    public enum CLGLObjectType
    {
        CL_GL_OBJECT_BUFFER = 0x2000,
        CL_GL_OBJECT_TEXTURE2D = 0x2001,
        CL_GL_OBJECT_TEXTURE_RECTANGLE = 0x2002,
        CL_GL_OBJECT_TEXTURE3D = 0x2003,
        CL_GL_OBJECT_RENDERBUFFER = 0x2004,
    }

    public enum CLGLTextureInfo
    {
        CL_GL_TEXTURE_TARGET = 0x2005,
        CL_GL_MIPMAP_LEVEL = 0x2006,
    }
    #endregion

    public class OpenCLGLDriver
    {
        [DllImport("opencl")]
        public static extern CLError clCreateFromGLBuffer(
            CLContext context,
            CLMemFlags flags,
            uint bufobj,
            out CLError errcode_ret);

        [DllImport("opencl")]
        public static extern CLError clCreateFromGLTexture2D(
            CLContext context,
            CLMemFlags flags,
            int target,
            int miplevel,
            uint texture,
            out CLError errcode_ret);

        [DllImport("opencl")]
        public static extern CLError clCreateFromGLTexture3D(
            CLContext context,
            CLMemFlags flags,
            int target,
            int miplevel,
            uint texture,
            out CLError errcode_ret);

        [DllImport("opencl")]
        public static extern CLError clCreateFromGLRenderbuffer(
            CLContext context,
            CLMemFlags flags,
            uint renderbuffer,
            out CLError errcode_ret);

        [DllImport("opencl")]
        public static extern CLError clGetGLObjectInfo(
            CLMem memobj,
            out uint gl_object_type,
            out uint gl_object_name);

        [DllImport("opencl")]
        public static extern CLError clGetGLTextureInfo(
            CLMem memobj,
            uint param_name,
            uint param_value_size,
            IntPtr param_value,
            out uint param_value_size_ret);

        [DllImport("opencl")]
        public static extern CLError clEnqueueAcquireGLObjects(
            CLCommandQueue command_queue,
            uint num_objects,
            [In] CLMem[] mem_objects,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            out CLEvent e);

        [DllImport("opencl")]
        public static extern CLError clEnqueueReleaseGLObjects(
            CLCommandQueue command_queue,
            uint num_objects,
            [In] CLMem[] mem_objects,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            out CLEvent e);
    }
}
