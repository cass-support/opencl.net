using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace GASS.OpenCL
{
    /// <summary>
    /// This class provides the driver interface to OpenCL functions.
    /// </summary>
    public class OpenCLDriver
    {
        internal const string OPENCL_DLL_NAME = "OpenCL";

        #region Platform API
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clGetPlatformInfo(
            CLPlatformInfo param_name,
            [MarshalAs(UnmanagedType.SysUInt)] uint param_value_size,
            [Out] byte[] param_value,
            [MarshalAs(UnmanagedType.SysUInt)] out uint param_value_size_ret);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clGetPlatformInfo(
            CLPlatformInfo param_name,
            [MarshalAs(UnmanagedType.SysUInt)] ulong param_value_size,
            [Out] byte[] param_value,
            [MarshalAs(UnmanagedType.SysUInt)] out ulong param_value_size_ret);
        #endregion

        #region Device APIs
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clGetDeviceIDs(
            CLDeviceType device_type,
            uint num_entries,
            [Out] CLDeviceID[] devices,
            out uint num_devices);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clGetDeviceInfo(
            CLDeviceID device,
            CLDeviceInfo param_name,
            [MarshalAs(UnmanagedType.SysUInt)] uint param_value_size,
            IntPtr param_value,
            [MarshalAs(UnmanagedType.SysUInt)] out uint param_value_size_ret);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clGetDeviceInfo(
            CLDeviceID device,
            CLDeviceInfo param_name,
            [MarshalAs(UnmanagedType.SysUInt)] ulong param_value_size,
            IntPtr param_value,
            [MarshalAs(UnmanagedType.SysUInt)] out ulong param_value_size_ret);
        #endregion

        #region Context APIs
        public delegate void LoggingFunction(byte[] errinfo, 
            IntPtr private_info,
            [MarshalAs(UnmanagedType.SysUInt)] uint cb, 
            IntPtr user_data);

        public delegate void LoggingFunction64(byte[] errinfo,
            IntPtr private_info,
            [MarshalAs(UnmanagedType.SysUInt)] ulong cb,
            IntPtr user_data);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLContext clCreateContext(
            ulong properties,
            uint num_devices,
            [In] CLDeviceID[] devices,
            LoggingFunction pfn_notify,
            IntPtr user_data,
            out CLError errcode_ret);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLContext clCreateContext(
            ulong properties,
            uint num_devices,
            [In] CLDeviceID[] devices,
            LoggingFunction64 pfn_notify,
            IntPtr user_data,
            out CLError errcode_ret);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLContext clCreateContextFromType(
            ulong properties,
            CLDeviceType device_type,
            LoggingFunction pfn_notify,
            IntPtr user_data,
            out CLError errcode_ret);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLContext clCreateContextFromType(
            ulong properties,
            CLDeviceType device_type,
            LoggingFunction64 pfn_notify,
            IntPtr user_data,
            out CLError errcode_ret);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clRetainContext(CLContext context);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clReleaseContext(CLContext context);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clGetContextInfo(
            CLContext context,
            CLContextInfo param_name,
            [MarshalAs(UnmanagedType.SysUInt)] uint param_value_size,
            IntPtr param_value,
            [MarshalAs(UnmanagedType.SysUInt)] out uint param_value_size_ret);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clGetContextInfo(
            CLContext context,
            CLContextInfo param_name,
            [MarshalAs(UnmanagedType.SysUInt)] ulong param_value_size,
            IntPtr param_value,
            [MarshalAs(UnmanagedType.SysUInt)] out ulong param_value_size_ret);
        #endregion

        #region Command Queue APIs
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLCommandQueue clCreateCommandQueue(
            CLContext context,
            CLDeviceID device,
            CLCommandQueueProperties properties,
            out CLError errcode_ret);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clRetainCommandQueue(CLCommandQueue command_queue);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clReleaseCommandQueue(CLCommandQueue command_queue);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clGetCommandQueueInfo(
            CLCommandQueue command_queue,
            CLCommandQueueInfo param_name,
            [MarshalAs(UnmanagedType.SysUInt)] uint param_value_size,
            IntPtr param_value,
            [MarshalAs(UnmanagedType.SysUInt)] out uint param_value_size_ret);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clGetCommandQueueInfo(
            CLCommandQueue command_queue,
            CLCommandQueueInfo param_name,
            [MarshalAs(UnmanagedType.SysUInt)] ulong param_value_size,
            IntPtr param_value,
            [MarshalAs(UnmanagedType.SysUInt)] out ulong param_value_size_ret);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clSetCommandQueueProperty(
            CLCommandQueue command_queue,
            CLCommandQueueProperties properties,
            CLBool enable,
            out CLCommandQueueProperties old_properties);
        #endregion

        #region Memory Object APIs
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLMem clCreateBuffer(
            CLContext context,
            CLMemFlags flags,
            [MarshalAs(UnmanagedType.SysUInt)] uint size,
            IntPtr host_ptr,
            out CLError errcode_ret);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLMem clCreateBuffer(
            CLContext context,
            CLMemFlags flags,
            [MarshalAs(UnmanagedType.SysUInt)] ulong size,
            IntPtr host_ptr,
            out CLError errcode_ret);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern IntPtr clCreateImage2D(
            CLContext context,
            CLMemFlags flags,
            ref CLImageFormat image_format,
            [MarshalAs(UnmanagedType.SysUInt)] uint image_width,
            [MarshalAs(UnmanagedType.SysUInt)] uint image_height,
            [MarshalAs(UnmanagedType.SysUInt)] uint image_row_pitch,
            IntPtr host_ptr,
            out CLError errcode_ret);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern IntPtr clCreateImage2D(
            CLContext context,
            CLMemFlags flags,
            ref CLImageFormat image_format,
            [MarshalAs(UnmanagedType.SysUInt)] ulong image_width,
            [MarshalAs(UnmanagedType.SysUInt)] ulong image_height,
            [MarshalAs(UnmanagedType.SysUInt)] ulong image_row_pitch,
            IntPtr host_ptr,
            out CLError errcode_ret);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern IntPtr clCreateImage3D(
            CLContext context,
            CLMemFlags flags,
            ref CLImageFormat image_format,
            [MarshalAs(UnmanagedType.SysUInt)] uint image_width,
            [MarshalAs(UnmanagedType.SysUInt)] uint image_height,
            [MarshalAs(UnmanagedType.SysUInt)] uint image_depth,
            [MarshalAs(UnmanagedType.SysUInt)] uint image_row_pitch,
            [MarshalAs(UnmanagedType.SysUInt)] uint image_slice_pitch,
            IntPtr host_ptr,
            out CLError errcode_ret);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern IntPtr clCreateImage3D(
            CLContext context,
            CLMemFlags flags,
            ref CLImageFormat image_format,
            [MarshalAs(UnmanagedType.SysUInt)] ulong image_width,
            [MarshalAs(UnmanagedType.SysUInt)] ulong image_height,
            [MarshalAs(UnmanagedType.SysUInt)] ulong image_depth,
            [MarshalAs(UnmanagedType.SysUInt)] ulong image_row_pitch,
            [MarshalAs(UnmanagedType.SysUInt)] ulong image_slice_pitch,
            IntPtr host_ptr,
            out CLError errcode_ret);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clRetainMemObject(CLMem memobj);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clReleaseMemObject(CLMem memobj);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clGetSupportedImageFormats(
            CLContext context,
            CLMemFlags flags,
            CLMemObjectType image_type,
            uint num_entries,
            [Out] CLImageFormat[] image_formats,
            out uint num_image_formats);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clGetMemObjectInfo(
            CLMem memobj,
            CLMemInfo param_name,
            [MarshalAs(UnmanagedType.SysUInt)] uint param_value_size,
            IntPtr param_value,
            [MarshalAs(UnmanagedType.SysUInt)] out uint param_value_size_ret);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clGetMemObjectInfo(
            CLMem memobj,
            CLMemInfo param_name,
            [MarshalAs(UnmanagedType.SysUInt)] ulong param_value_size,
            IntPtr param_value,
            [MarshalAs(UnmanagedType.SysUInt)] out ulong param_value_size_ret);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clGetImageInfo(
            CLMem image,
            CLImageInfo param_name,
            [MarshalAs(UnmanagedType.SysUInt)] uint param_value_size,
            IntPtr param_value,
            [MarshalAs(UnmanagedType.SysUInt)] out uint param_value_size_ret);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clGetImageInfo(
            CLMem image,
            CLImageInfo param_name,
            [MarshalAs(UnmanagedType.SysUInt)] ulong param_value_size,
            IntPtr param_value,
            [MarshalAs(UnmanagedType.SysUInt)] out ulong param_value_size_ret);
        #endregion

        #region Sampler APIs
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLSampler clCreateSampler(
            CLContext context,
            CLBool normalized_coords,
            CLAddressingMode addressing_mode,
            CLFilterMode filter_mode,
            out CLError errcode_ret);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clRetainSampler(CLSampler sampler);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clReleaseSampler(CLSampler sampler);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clGetSamplerInfo(
            CLSampler sampler,
            CLSamplerInfo param_name,
            [MarshalAs(UnmanagedType.SysUInt)] uint param_value_size,
            IntPtr param_value,
            [MarshalAs(UnmanagedType.SysUInt)] out uint param_value_size_ret);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clGetSamplerInfo(
            CLSampler sampler,
            CLSamplerInfo param_name,
            [MarshalAs(UnmanagedType.SysUInt)] ulong param_value_size,
            IntPtr param_value,
            [MarshalAs(UnmanagedType.SysUInt)] out ulong param_value_size_ret);
        #endregion

        #region Program Object APIs
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLProgram clCreateProgramWithSource(
            CLContext context,
            uint count,
            [In] string[] strings,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.SysUInt),In] uint[] lengths,
            out CLError errcode_ret);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLProgram clCreateProgramWithSource(
            CLContext context,
            uint count,
            [In] string[] strings,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.SysUInt),In] ulong[] lengths,
            out CLError errcode_ret);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLProgram clCreateProgramWithBinary(
            CLContext context,
            uint num_devices,
            [In] CLDeviceID[] device_list,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.SysUInt),In] uint[] lengths,
            [In] IntPtr[] binaries,
            [In] int[] binary_status,
            out CLError errcode_ret);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLProgram clCreateProgramWithBinary(
            CLContext context,
            uint num_devices,
            [In] CLDeviceID[] device_list,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.SysUInt), In] ulong[] lengths,
            [In] IntPtr[] binaries,
            [In] int[] binary_status,
            out CLError errcode_ret);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clRetainProgram(CLProgram program);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clReleaseProgram(CLProgram program);

        public delegate void NotifyFunction(CLProgram program, IntPtr user_data);
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clBuildProgram(
            CLProgram program,
            uint num_devices,
            [In] CLDeviceID[] device_list,
            string options,
            NotifyFunction func,
            IntPtr user_data);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clUnloadCompiler();

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clGetProgramInfo(
            CLProgram program,
            CLProgramInfo param_name,
            [MarshalAs(UnmanagedType.SysUInt)] uint param_value_size,
            IntPtr param_value,
            [MarshalAs(UnmanagedType.SysUInt)] out uint param_value_size_ret);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clGetProgramInfo(
            CLProgram program,
            CLProgramInfo param_name,
            [MarshalAs(UnmanagedType.SysUInt)] ulong param_value_size,
            IntPtr param_value,
            [MarshalAs(UnmanagedType.SysUInt)] out ulong param_value_size_ret);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clGetProgramBuildInfo(
            CLProgram program,
            CLDeviceID device,
            CLProgramBuildInfo param_name,
            [MarshalAs(UnmanagedType.SysUInt)] uint param_value_size,
            IntPtr param_value,
            [MarshalAs(UnmanagedType.SysUInt)] out uint param_value_size_ret);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clGetProgramBuildInfo(
            CLProgram program,
            CLDeviceID device,
            CLProgramBuildInfo param_name,
            [MarshalAs(UnmanagedType.SysUInt)] ulong param_value_size,
            IntPtr param_value,
            [MarshalAs(UnmanagedType.SysUInt)] out ulong param_value_size_ret);
        #endregion

        #region Kernel Object APIs
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLKernel clCreateKernel(
            CLProgram program,
            string kernel_name,
            out CLError errcode_ret);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clCreateKernelsInProgram(
            CLProgram program,
            uint num_kernels,
            [Out] CLKernel[] kernels,
            out uint num_kernels_ret);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clRetainKernel(CLKernel kernel);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clReleaseKernel(CLKernel kernel);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clSetKernelArg(
            CLKernel kernel,
            uint arg_index,
            [MarshalAs(UnmanagedType.SysUInt)] uint arg_size,
            IntPtr arg_value);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clSetKernelArg(
            CLKernel kernel,
            uint arg_index,
            [MarshalAs(UnmanagedType.SysUInt)] ulong arg_size,
            IntPtr arg_value);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clGetKernelInfo(
            CLKernel kernel,
            CLKernelInfo param_name,
            [MarshalAs(UnmanagedType.SysUInt)] uint param_value_size,
            IntPtr param_value,
            [MarshalAs(UnmanagedType.SysUInt)] out uint param_value_size_ret);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clGetKernelInfo(
            CLKernel kernel,
            CLKernelInfo param_name,
            [MarshalAs(UnmanagedType.SysUInt)] ulong param_value_size,
            IntPtr param_value,
            [MarshalAs(UnmanagedType.SysUInt)] out ulong param_value_size_ret);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clGetKernelWorkGroupInfo(
            CLKernel kernel,
            CLDeviceID device,
            CLKernelWorkGroupInfo param_name,
            [MarshalAs(UnmanagedType.SysUInt)] uint param_value_size,
            IntPtr param_value,
            [MarshalAs(UnmanagedType.SysUInt)] out uint param_value_size_ret);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clGetKernelWorkGroupInfo(
            CLKernel kernel,
            CLDeviceID device,
            CLKernelWorkGroupInfo param_name,
            [MarshalAs(UnmanagedType.SysUInt)] ulong param_value_size,
            IntPtr param_value,
            [MarshalAs(UnmanagedType.SysUInt)] out ulong param_value_size_ret);
        #endregion

        #region Event Object APIs
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clWaitForEvents(
            uint num_events,
            [In] CLEvent[] event_list);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clGetEventInfo(
            CLEvent e,
            CLEventInfo param_name,
            [MarshalAs(UnmanagedType.SysUInt)] uint param_value_size,
            IntPtr param_value,
            [MarshalAs(UnmanagedType.SysUInt)] out uint param_value_size_ret);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clGetEventInfo(
            CLEvent e,
            CLEventInfo param_name,
            [MarshalAs(UnmanagedType.SysUInt)] ulong param_value_size,
            IntPtr param_value,
            [MarshalAs(UnmanagedType.SysUInt)] out ulong param_value_size_ret);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clRetainEvent(CLEvent e);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clReleaseEvent(CLEvent e);
        #endregion

        #region Profiling APIs
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clGetEventProfilingInfo(
            CLEvent e,
            CLProfilingInfo param_name,
            [MarshalAs(UnmanagedType.SysUInt)] uint param_value_size,
            IntPtr param_value,
            [MarshalAs(UnmanagedType.SysUInt)] out uint param_value_size_ret);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clGetEventProfilingInfo(
            CLEvent e,
            CLProfilingInfo param_name,
            [MarshalAs(UnmanagedType.SysUInt)] ulong param_value_size,
            IntPtr param_value,
            [MarshalAs(UnmanagedType.SysUInt)] out ulong param_value_size_ret);
        #endregion

        #region Flush and Finish APIs
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clFlush(CLCommandQueue command_queue);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clFinish(CLCommandQueue command_queue);
        #endregion

        #region Enqueued Commands APIs
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueReadBuffer(CLCommandQueue command_queue,
            CLMem buffer,
            CLBool blocking_read,
            [MarshalAs(UnmanagedType.SysUInt)] uint offset,
            [MarshalAs(UnmanagedType.SysUInt)] uint cb,
            IntPtr ptr,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            out CLEvent e);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueReadBuffer(CLCommandQueue command_queue,
            CLMem buffer,
            CLBool blocking_read,
            [MarshalAs(UnmanagedType.SysUInt)] ulong offset,
            [MarshalAs(UnmanagedType.SysUInt)] ulong cb,
            IntPtr ptr,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            out CLEvent e);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueWriteBuffer(CLCommandQueue command_queue,
            CLMem buffer,
            CLBool blocking_write,
            [MarshalAs(UnmanagedType.SysUInt)] uint offset,
            [MarshalAs(UnmanagedType.SysUInt)] uint cb,
            IntPtr ptr,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            out CLEvent e);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueWriteBuffer(CLCommandQueue command_queue,
            CLMem buffer,
            CLBool blocking_write,
            [MarshalAs(UnmanagedType.SysUInt)] ulong offset,
            [MarshalAs(UnmanagedType.SysUInt)] ulong cb,
            IntPtr ptr,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            out CLEvent e);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueCopyBuffer(CLCommandQueue command_queue,
            CLMem src_buffer,
            CLMem dst_buffer,
            [MarshalAs(UnmanagedType.SysUInt)] uint src_offset,
            [MarshalAs(UnmanagedType.SysUInt)] uint dst_offset,
            [MarshalAs(UnmanagedType.SysUInt)] uint cb,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            out CLEvent e);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueCopyBuffer(CLCommandQueue command_queue,
            CLMem src_buffer,
            CLMem dst_buffer,
            [MarshalAs(UnmanagedType.SysUInt)] ulong src_offset,
            [MarshalAs(UnmanagedType.SysUInt)] ulong dst_offset,
            [MarshalAs(UnmanagedType.SysUInt)] ulong cb,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            out CLEvent e);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueReadImage(
            CLCommandQueue command_queue,
            CLMem image,
            CLBool blocking_read,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.SysUInt)] uint[] origin,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.SysUInt)] uint[] region,
            [MarshalAs(UnmanagedType.SysUInt)] uint row_pitch,
            [MarshalAs(UnmanagedType.SysUInt)] uint slice_pitch,
            IntPtr ptr,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            out CLEvent e);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueReadImage(
            CLCommandQueue command_queue,
            CLMem image,
            CLBool blocking_read,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.SysUInt)] ulong[] origin,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.SysUInt)] ulong[] region,
            [MarshalAs(UnmanagedType.SysUInt)] ulong row_pitch,
            [MarshalAs(UnmanagedType.SysUInt)] ulong slice_pitch,
            IntPtr ptr,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            out CLEvent e);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueWriteImage(
            CLCommandQueue command_queue,
            CLMem image,
            CLBool blocking_write,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.SysUInt)] uint[] origin,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.SysUInt)] uint[] region,
            [MarshalAs(UnmanagedType.SysUInt)] uint input_row_pitch,
            [MarshalAs(UnmanagedType.SysUInt)] uint input_slice_pitch,
            IntPtr ptr,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            out CLEvent e);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueWriteImage(
            CLCommandQueue command_queue,
            CLMem image,
            CLBool blocking_write,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.SysUInt)] ulong[] origin,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.SysUInt)] ulong[] region,
            [MarshalAs(UnmanagedType.SysUInt)] ulong input_row_pitch,
            [MarshalAs(UnmanagedType.SysUInt)] ulong input_slice_pitch,
            IntPtr ptr,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            out CLEvent e);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueCopyImage(
            CLCommandQueue command_queue,
            CLMem src_image,
            CLMem dst_image,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.SysUInt)] uint[] src_origin,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.SysUInt)] uint[] dst_origin,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.SysUInt)] uint[] region,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            out CLEvent e);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueCopyImage(
            CLCommandQueue command_queue,
            CLMem src_image,
            CLMem dst_image,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.SysUInt)] ulong[] src_origin,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.SysUInt)] ulong[] dst_origin,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.SysUInt)] ulong[] region,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            out CLEvent e);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueCopyImageToBuffer(
            CLCommandQueue command_queue,
            CLMem src_image,
            CLMem dst_buffer,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.SysUInt)] uint[] src_origin,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.SysUInt)] uint[] region,
            [MarshalAs(UnmanagedType.SysUInt)] uint dst_offset,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            out CLEvent e);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueCopyImageToBuffer(
            CLCommandQueue command_queue,
            CLMem src_image,
            CLMem dst_buffer,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.SysUInt)] ulong[] src_origin,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.SysUInt)] ulong[] region,
            [MarshalAs(UnmanagedType.SysUInt)] ulong dst_offset,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            out CLEvent e);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueCopyBufferToImage(
            CLCommandQueue command_queue,
            CLMem src_buffer,
            CLMem dst_image,
            [MarshalAs(UnmanagedType.SysUInt)] uint src_offset,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.SysUInt)] uint[] dst_origin,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.SysUInt)] uint[] region,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            out CLEvent e);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueCopyBufferToImage(
            CLCommandQueue command_queue,
            CLMem src_buffer,
            CLMem dst_image,
            [MarshalAs(UnmanagedType.SysUInt)] ulong src_offset,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.SysUInt)] ulong[] dst_origin,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.SysUInt)] ulong[] region,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            out CLEvent e);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern IntPtr clEnqueueMapBuffer(
            CLCommandQueue command_queue,
            CLMem buffer,
            CLBool blocking_map,
            CLMapFlags map_flags,
            [MarshalAs(UnmanagedType.SysUInt)] uint offset,
            [MarshalAs(UnmanagedType.SysUInt)] uint cb,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            out CLEvent e,
            out CLError errcode_ret);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern IntPtr clEnqueueMapBuffer(
            CLCommandQueue command_queue,
            CLMem buffer,
            CLBool blocking_map,
            CLMapFlags map_flags,
            [MarshalAs(UnmanagedType.SysUInt)] ulong offset,
            [MarshalAs(UnmanagedType.SysUInt)] ulong cb,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            out CLEvent e,
            out CLError errcode_ret);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern IntPtr clEnqueueMapImage(
            CLCommandQueue command_queue,
            CLMem image,
            CLBool blocking_map,
            CLMapFlags map_flags,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.SysUInt)] uint[] origin,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.SysUInt)] uint[] region,
            [MarshalAs(UnmanagedType.SysUInt)] out uint image_row_pitch,
            [MarshalAs(UnmanagedType.SysUInt)] out uint image_slice_pitch,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            out CLEvent e,
            out CLError errcode_ret);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern IntPtr clEnqueueMapImage(
            CLCommandQueue command_queue,
            CLMem image,
            CLBool blocking_map,
            CLMapFlags map_flags,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.SysUInt)] ulong[] origin,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.SysUInt)] ulong[] region,
            [MarshalAs(UnmanagedType.SysUInt)] out ulong image_row_pitch,
            [MarshalAs(UnmanagedType.SysUInt)] out ulong image_slice_pitch,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            out CLEvent e,
            out CLError errcode_ret);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueUnmapMemObject(
            CLCommandQueue command_queue,
            CLMem memobj,
            IntPtr mapped_ptr,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            out CLEvent e);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueNDRangeKernel(
            CLCommandQueue command_queue,
            CLKernel kernel,
            uint work_dim,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.SysUInt), In] uint[] global_work_offset,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.SysUInt), In] uint[] global_work_size,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.SysUInt), In] uint[] local_work_size,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            out CLEvent e);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueNDRangeKernel(
            CLCommandQueue command_queue,
            CLKernel kernel,
            uint work_dim,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.SysUInt), In] ulong[] global_work_offset,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.SysUInt), In] ulong[] global_work_size,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.SysUInt), In] ulong[] local_work_size,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            out CLEvent e);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueTask(
            CLCommandQueue command_queue,
            CLKernel kernel,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            out CLEvent e);

        public delegate void UserFunction(IntPtr[] args);
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueNativeKernel(
            CLCommandQueue command_queue,
            UserFunction user_func,
            [In] IntPtr[] args,
            [MarshalAs(UnmanagedType.SysUInt)] uint cb_args,
            uint num_mem_objects,
            [In] CLMem[] mem_list,
            [In] IntPtr[] args_mem_loc,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            out CLEvent e);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueNativeKernel(
            CLCommandQueue command_queue,
            UserFunction user_func,
            [In] IntPtr[] args,
            [MarshalAs(UnmanagedType.SysUInt)] ulong cb_args,
            uint num_mem_objects,
            [In] CLMem[] mem_list,
            [In] IntPtr[] args_mem_loc,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            out CLEvent e);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueMarker(
            CLCommandQueue command_queue,
            out CLEvent e);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueWaitForEvents(
            CLCommandQueue command_queue,
            uint num_events,
            [In] CLEvent[] event_list);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueBarrier(CLCommandQueue command_queue);
        #endregion
    }
}
