using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using CASS.Types;

namespace CASS.OpenCL.OpenGL
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
            ref CLError errcode_ret);

        [DllImport(OpenCLDriver.OPENCL_DLL_NAME)]
        public static extern CLError clCreateFromGLTexture2D(
            CLContext context,
            CLMemFlags flags,
            int target,
            int miplevel,
            uint texture,
            ref CLError errcode_ret);

        [DllImport(OpenCLDriver.OPENCL_DLL_NAME)]
        public static extern CLError clCreateFromGLTexture3D(
            CLContext context,
            CLMemFlags flags,
            int target,
            int miplevel,
            uint texture,
            ref CLError errcode_ret);

        [DllImport(OpenCLDriver.OPENCL_DLL_NAME)]
        public static extern CLError clCreateFromGLRenderbuffer(
            CLContext context,
            CLMemFlags flags,
            uint renderbuffer,
            ref CLError errcode_ret);

        [DllImport(OpenCLDriver.OPENCL_DLL_NAME)]
        public static extern CLError clGetGLObjectInfo(
            CLMem memobj,
            ref uint gl_object_type,
            ref uint gl_object_name);

        [DllImport(OpenCLDriver.OPENCL_DLL_NAME)]
        public static extern CLError clGetGLTextureInfo(
            CLMem memobj,
            uint param_name,
            SizeT param_value_size,
            IntPtr param_value,
            ref SizeT param_value_size_ret);

        [DllImport(OpenCLDriver.OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueAcquireGLObjects(
            CLCommandQueue command_queue,
            uint num_objects,
            [In] CLMem[] mem_objects,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            ref CLEvent e);

        [DllImport(OpenCLDriver.OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueReleaseGLObjects(
            CLCommandQueue command_queue,
            uint num_objects,
            [In] CLMem[] mem_objects,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            ref CLEvent e);

        /* cl_khr_gl_sharing extension */
        [DllImport(OpenCLDriver.OPENCL_DLL_NAME)]
        public static extern CLError clGetGLContextInfoKHR(
            [In] SizeT[] properties,
            CLGLContextInfo param_name,
            SizeT param_value_size,
            IntPtr param_value,
            ref SizeT param_value_size_ret);

        /* cl_khr_gl_event extension */
        [DllImport(OpenCLDriver.OPENCL_DLL_NAME)]
        public static extern CLEvent clCreateEventFromGLsyncKHR(
            CLContext context,
            IntPtr cl_GLsync,
            ref CLError errcode_ret);
    }
}
