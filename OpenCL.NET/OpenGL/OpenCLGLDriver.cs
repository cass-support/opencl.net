using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace GASS.OpenCL.OpenGL
{
    /// <summary>
    /// This class provides the driver interface for OpenGL interoperability
    /// with OpenCL standard.
    /// </summary>
    public class OpenCLGLDriver
    {
        [DllImport(OpenCLDriver.OPENCL_DLL_NAME)]
        public static extern CLError clCreateFromGLBuffer(
            CLContext context,
            CLMemFlags flags,
            uint bufobj,
            out CLError errcode_ret);

        [DllImport(OpenCLDriver.OPENCL_DLL_NAME)]
        public static extern CLError clCreateFromGLTexture2D(
            CLContext context,
            CLMemFlags flags,
            int target,
            int miplevel,
            uint texture,
            out CLError errcode_ret);

        [DllImport(OpenCLDriver.OPENCL_DLL_NAME)]
        public static extern CLError clCreateFromGLTexture3D(
            CLContext context,
            CLMemFlags flags,
            int target,
            int miplevel,
            uint texture,
            out CLError errcode_ret);

        [DllImport(OpenCLDriver.OPENCL_DLL_NAME)]
        public static extern CLError clCreateFromGLRenderbuffer(
            CLContext context,
            CLMemFlags flags,
            uint renderbuffer,
            out CLError errcode_ret);

        [DllImport(OpenCLDriver.OPENCL_DLL_NAME)]
        public static extern CLError clGetGLObjectInfo(
            CLMem memobj,
            out uint gl_object_type,
            out uint gl_object_name);

        [DllImport(OpenCLDriver.OPENCL_DLL_NAME)]
        public static extern CLError clGetGLTextureInfo(
            CLMem memobj,
            uint param_name,
            uint param_value_size,
            IntPtr param_value,
            out uint param_value_size_ret);

        [DllImport(OpenCLDriver.OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueAcquireGLObjects(
            CLCommandQueue command_queue,
            uint num_objects,
            [In] CLMem[] mem_objects,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            out CLEvent e);

        [DllImport(OpenCLDriver.OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueReleaseGLObjects(
            CLCommandQueue command_queue,
            uint num_objects,
            [In] CLMem[] mem_objects,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            out CLEvent e);
    }
}
