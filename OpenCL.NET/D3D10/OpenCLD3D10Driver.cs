using System;
using System.Runtime.InteropServices;

using CASS.Types;

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
