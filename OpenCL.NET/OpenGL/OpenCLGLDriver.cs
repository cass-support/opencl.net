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
        public static extern CLMem clCreateFromGLBuffer(
            CLContext context,
            CLMemFlags flags,
            uint bufobj,
            ref CLError errcode_ret);
        [DllImport(OpenCLDriver.OPENCL_DLL_NAME)]
        public static extern CLMem clCreateFromGLBuffer(
            CLContext context,
            CLMemFlags flags,
            uint bufobj,
            IntPtr errcode_ret);

        [DllImport(OpenCLDriver.OPENCL_DLL_NAME)]
        public static extern CLMem clCreateFromGLTexture(
            CLContext context,
            CLMemFlags flags,
            int target,
            int miplevel,
            uint texture,
            ref CLError errcode_ret);
        [DllImport(OpenCLDriver.OPENCL_DLL_NAME)]
        public static extern CLMem clCreateFromGLTexture(
            CLContext context,
            CLMemFlags flags,
            int target,
            int miplevel,
            uint texture,
            IntPtr errcode_ret);

        [Obsolete("Deprecated since OpenCL 1.2")]
        [DllImport(OpenCLDriver.OPENCL_DLL_NAME)]
        public static extern CLMem clCreateFromGLTexture2D(
            CLContext context,
            CLMemFlags flags,
            int target,
            int miplevel,
            uint texture,
            ref CLError errcode_ret);
        [Obsolete("Deprecated since OpenCL 1.2")]
        [DllImport(OpenCLDriver.OPENCL_DLL_NAME)]
        public static extern CLMem clCreateFromGLTexture2D(
            CLContext context,
            CLMemFlags flags,
            int target,
            int miplevel,
            uint texture,
            IntPtr errcode_ret);

        [Obsolete("Deprecated since OpenCL 1.2")]
        [DllImport(OpenCLDriver.OPENCL_DLL_NAME)]
        public static extern CLMem clCreateFromGLTexture3D(
            CLContext context,
            CLMemFlags flags,
            int target,
            int miplevel,
            uint texture,
            ref CLError errcode_ret);
        [Obsolete("Deprecated since OpenCL 1.2")]
        [DllImport(OpenCLDriver.OPENCL_DLL_NAME)]
        public static extern CLMem clCreateFromGLTexture3D(
            CLContext context,
            CLMemFlags flags,
            int target,
            int miplevel,
            uint texture,
            IntPtr errcode_ret);

        [DllImport(OpenCLDriver.OPENCL_DLL_NAME)]
        public static extern CLMem clCreateFromGLRenderbuffer(
            CLContext context,
            CLMemFlags flags,
            uint renderbuffer,
            ref CLError errcode_ret);
        [DllImport(OpenCLDriver.OPENCL_DLL_NAME)]
        public static extern CLMem clCreateFromGLRenderbuffer(
            CLContext context,
            CLMemFlags flags,
            uint renderbuffer,
            IntPtr errcode_ret);

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
    }
}
