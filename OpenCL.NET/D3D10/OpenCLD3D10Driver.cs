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

namespace CASS.OpenCL.D3D10
{
    /// <summary>
    /// This class provides the driver interface for Direct3D 10 interoperability
    /// with OpenCL standard.
    /// </summary>
    public class OpenCLD3D10Driver
    {
        [DllImport(OpenCLDriver.OPENCL_DLL_NAME)]
        public static extern CLError clGetDeviceIDsFromD3D10KHR(
            CLPlatformID platform,
            D3D10DeviceSource d3d_device_source,
            IntPtr d3d_object,
            D3D10DeviceSet d3d_device_set,
            uint num_entries,
            [Out] CLDeviceID[] devices,
            ref uint num_devices);

        [DllImport(OpenCLDriver.OPENCL_DLL_NAME)]
        public static extern CLMem clCreateFromD3D10BufferKHR(
            CLContext context,
            CLMemFlags flags,
            IntPtr resource,
            ref CLError errcode_ret);

        [DllImport(OpenCLDriver.OPENCL_DLL_NAME)]
        public static extern CLMem clCreateFromD3D10Texture2DKHR(
            CLContext context,
            CLMemFlags flags,
            IntPtr resource,
            uint subresource,
            ref CLError errcode_ret);

        [DllImport(OpenCLDriver.OPENCL_DLL_NAME)]
        public static extern CLMem clCreateFromD3D10Texture3DKHR(
            CLContext context,
            CLMemFlags flags,
            IntPtr resource,
            uint subresource,
            ref CLError errcode_ret);

        [DllImport(OpenCLDriver.OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueAcquireD3D10ObjectsKHR(
            CLCommandQueue command_queue,
            uint num_objects,
            [In] CLMem[] mem_objects,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            ref CLEvent e);

        [DllImport(OpenCLDriver.OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueReleaseD3D10ObjectsKHR(
            CLCommandQueue command_queue,
            uint num_objects,
            [In] CLMem[] mem_objects,
            uint num_events_in_wait_list,
            [In] CLEvent[]  event_wait_list,
            ref CLEvent e);
    }
}
