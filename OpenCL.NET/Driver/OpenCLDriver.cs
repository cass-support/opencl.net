using System;
using System.Runtime.InteropServices;

using CASS.Types;

namespace CASS.OpenCL
{
    /// <summary>
    /// This class provides the driver interface to OpenCL functions.
    /// </summary>
    public class OpenCLDriver
    {
        internal const string OPENCL_DLL_NAME = "OpenCL";

        #region Platform API
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clGetPlatformIDs(
            uint num_entries,
            [Out] CLPlatformID[] platforms,
            ref uint num_platforms);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clGetPlatformInfo(
            CLPlatformID platform,
            CLPlatformInfo param_name,
            SizeT param_value_size,
            IntPtr param_value,
            ref SizeT param_value_size_ret);
        #endregion

        #region Device APIs
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clGetDeviceIDs(
            CLPlatformID platform_id,
            CLDeviceType device_type,
            uint num_entries,
            [Out] CLDeviceID[] devices,
            ref uint num_devices);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clGetDeviceIDs(
            CLPlatformID platform_id,
            CLDeviceType device_type,
            uint num_entries,
            IntPtr devices,
            ref uint num_devices);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clGetDeviceInfo(
            CLDeviceID device,
            CLDeviceInfo param_name,
            SizeT param_value_size,
            [Out] byte[] param_value,
            ref SizeT param_value_size_ret);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clGetDeviceInfo(
            CLDeviceID device,
            CLDeviceInfo param_name,
            SizeT param_value_size,
            IntPtr param_value,
            ref SizeT param_value_size_ret);
        #endregion

        #region Context APIs
        public delegate void LoggingFunction(
            IntPtr errinfo,
            IntPtr private_info,
            SizeT cb,
            IntPtr user_data);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLContext clCreateContext(
            [In] SizeT[] properties,
            uint num_devices,
            [In] CLDeviceID[] devices,
            LoggingFunction pfn_notify,
            IntPtr user_data,
            ref CLError errcode_ret);
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLContext clCreateContext(
            [In] IntPtr[] properties,
            uint num_devices,
            [In] CLDeviceID[] devices,
            LoggingFunction pfn_notify,
            IntPtr user_data,
            ref CLError errcode_ret);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLContext clCreateContextFromType(
            [In] SizeT[] properties,
            CLDeviceType device_type,
            LoggingFunction pfn_notify,
            IntPtr user_data,
            ref CLError errcode_ret);
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLContext clCreateContextFromType(
            [In] IntPtr[] properties,
            CLDeviceType device_type,
            LoggingFunction pfn_notify,
            IntPtr user_data,
            ref CLError errcode_ret);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clRetainContext(CLContext context);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clReleaseContext(CLContext context);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clGetContextInfo(
            CLContext context,
            CLContextInfo param_name,
            SizeT param_value_size,
            IntPtr param_value,
            ref SizeT param_value_size_ret);
        #endregion

        #region Command Queue APIs
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLCommandQueue clCreateCommandQueue(
            CLContext context,
            CLDeviceID device,
            CLCommandQueueProperties properties,
            ref CLError errcode_ret);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clRetainCommandQueue(CLCommandQueue command_queue);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clReleaseCommandQueue(CLCommandQueue command_queue);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clGetCommandQueueInfo(
            CLCommandQueue command_queue,
            CLCommandQueueInfo param_name,
            SizeT param_value_size,
            IntPtr param_value,
            ref SizeT param_value_size_ret);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clSetCommandQueueProperty(
            CLCommandQueue command_queue,
            CLCommandQueueProperties properties,
            CLBool enable,
            ref CLCommandQueueProperties old_properties);
        #endregion

        #region Memory Object APIs
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLMem clCreateBuffer(
            CLContext context,
            CLMemFlags flags,
            SizeT size,
            IntPtr host_ptr,
            ref CLError errcode_ret);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLMem clCreateImage2D(
            CLContext context,
            CLMemFlags flags,
            ref CLImageFormat image_format,
            SizeT image_width,
            SizeT image_height,
            SizeT image_row_pitch,
            IntPtr host_ptr,
            ref CLError errcode_ret);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLMem clCreateImage3D(
            CLContext context,
            CLMemFlags flags,
            ref CLImageFormat image_format,
            SizeT image_width,
            SizeT image_height,
            SizeT image_depth,
            SizeT image_row_pitch,
            SizeT image_slice_pitch,
            IntPtr host_ptr,
            ref CLError errcode_ret);

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
            ref uint num_image_formats);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clGetSupportedImageFormats(
            CLContext context,
            CLMemFlags flags,
            CLMemObjectType image_type,
            uint num_entries,
            IntPtr image_formats,
            ref uint num_image_formats);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clGetMemObjectInfo(
            CLMem memobj,
            CLMemInfo param_name,
            SizeT param_value_size,
            IntPtr param_value,
            ref SizeT param_value_size_ret);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clGetImageInfo(
            CLMem image,
            CLImageInfo param_name,
            SizeT param_value_size,
            IntPtr param_value,
            ref SizeT param_value_size_ret);
        #endregion

        #region Sampler APIs
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLSampler clCreateSampler(
            CLContext context,
            CLBool normalized_coords,
            CLAddressingMode addressing_mode,
            CLFilterMode filter_mode,
            ref CLError errcode_ret);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clRetainSampler(CLSampler sampler);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clReleaseSampler(CLSampler sampler);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clGetSamplerInfo(
            CLSampler sampler,
            CLSamplerInfo param_name,
            SizeT param_value_size,
            IntPtr param_value,
            ref SizeT param_value_size_ret);
        #endregion

        #region Program Object APIs
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLProgram clCreateProgramWithSource(
            CLContext context,
            uint count,
            [In] string[] strings,
            [In] SizeT[] lengths,
            ref CLError errcode_ret);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLProgram clCreateProgramWithSource(
            CLContext context,
            uint count,
            IntPtr strings,
            [In] SizeT[] lengths,
            ref CLError errcode_ret);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLProgram clCreateProgramWithSource(
            CLContext context,
            uint count,
            IntPtr[] strings,
            [In] SizeT[] lengths,
            ref CLError errcode_ret);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLProgram clCreateProgramWithBinary(
            CLContext context,
            uint num_devices,
            [In] CLDeviceID[] device_list,
            [In] SizeT[] lengths,
            [In] IntPtr[] binaries,
            [In] int[] binary_status,
            ref CLError errcode_ret);

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
        public static extern CLError clBuildProgram(
            CLProgram program,
            uint num_devices,
            [In] CLDeviceID[] device_list,
            IntPtr options,
            NotifyFunction func,
            IntPtr user_data);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clUnloadCompiler();

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clGetProgramInfo(
            CLProgram program,
            CLProgramInfo param_name,
            SizeT param_value_size,
            IntPtr param_value,
            ref SizeT param_value_size_ret);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clGetProgramBuildInfo(
            CLProgram program,
            CLDeviceID device,
            CLProgramBuildInfo param_name,
            SizeT param_value_size,
            IntPtr param_value,
            ref SizeT param_value_size_ret);
        #endregion

        #region Kernel Object APIs
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLKernel clCreateKernel(
            CLProgram program,
            string kernel_name,
            ref CLError errcode_ret);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clCreateKernelsInProgram(
            CLProgram program,
            uint num_kernels,
            [Out] CLKernel[] kernels,
            ref uint num_kernels_ret);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clRetainKernel(CLKernel kernel);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clReleaseKernel(CLKernel kernel);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clSetKernelArg(
            CLKernel kernel,
            uint arg_index,
            SizeT arg_size,
            IntPtr arg_value);
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clSetKernelArg(
            CLKernel kernel,
            uint arg_index,
            SizeT arg_size,
            ref CLMem arg_value);
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clSetKernelArg(
            CLKernel kernel,
            uint arg_index,
            SizeT arg_size,
            ref byte arg_value);
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clSetKernelArg(
            CLKernel kernel,
            uint arg_index,
            SizeT arg_size,
            byte[] arg_value);
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clSetKernelArg(
            CLKernel kernel,
            uint arg_index,
            SizeT arg_size,
            ref short arg_value);
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clSetKernelArg(
            CLKernel kernel,
            uint arg_index,
            SizeT arg_size,
            short[] arg_value);
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clSetKernelArg(
            CLKernel kernel,
            uint arg_index,
            SizeT arg_size,
            ref int arg_value);
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clSetKernelArg(
            CLKernel kernel,
            uint arg_index,
            SizeT arg_size,
            int[] arg_value);
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clSetKernelArg(
            CLKernel kernel,
            uint arg_index,
            SizeT arg_size,
            ref long arg_value);
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clSetKernelArg(
            CLKernel kernel,
            uint arg_index,
            SizeT arg_size,
            long[] arg_value);
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clSetKernelArg(
            CLKernel kernel,
            uint arg_index,
            SizeT arg_size,
            ref float arg_value);
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clSetKernelArg(
            CLKernel kernel,
            uint arg_index,
            SizeT arg_size,
            float[] arg_value);
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clSetKernelArg(
            CLKernel kernel,
            uint arg_index,
            SizeT arg_size,
            ref double arg_value);
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clSetKernelArg(
            CLKernel kernel,
            uint arg_index,
            SizeT arg_size,
            double[] arg_value);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clGetKernelInfo(
            CLKernel kernel,
            CLKernelInfo param_name,
            SizeT param_value_size,
            IntPtr param_value,
            ref SizeT param_value_size_ret);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clGetKernelWorkGroupInfo(
            CLKernel kernel,
            CLDeviceID device,
            CLKernelWorkGroupInfo param_name,
            SizeT param_value_size,
            IntPtr param_value,
            ref SizeT param_value_size_ret);
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
            SizeT param_value_size,
            IntPtr param_value,
            ref SizeT param_value_size_ret);

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
            SizeT param_value_size,
            IntPtr param_value,
            ref SizeT param_value_size_ret);
        #endregion

        #region Flush and Finish APIs
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clFlush(CLCommandQueue command_queue);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clFinish(CLCommandQueue command_queue);
        #endregion

        #region Enqueued Commands APIs
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueReadBuffer(
            CLCommandQueue command_queue,
            CLMem buffer,
            CLBool blocking_read,
            SizeT offset,
            SizeT cb,
            IntPtr ptr,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            ref CLEvent e);
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueReadBuffer(
            CLCommandQueue command_queue,
            CLMem buffer,
            CLBool blocking_read,
            SizeT offset,
            SizeT cb,
            IntPtr ptr,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            IntPtr e);
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueReadBuffer(
            CLCommandQueue command_queue,
            CLMem buffer,
            CLBool blocking_read,
            SizeT offset,
            SizeT cb,
            [Out] byte[] ptr,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            ref CLEvent e);
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueReadBuffer(
            CLCommandQueue command_queue,
            CLMem buffer,
            CLBool blocking_read,
            SizeT offset,
            SizeT cb,
            [Out] byte[] ptr,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            IntPtr e);
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueReadBuffer(
            CLCommandQueue command_queue,
            CLMem buffer,
            CLBool blocking_read,
            SizeT offset,
            SizeT cb,
            [Out] short[] ptr,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            ref CLEvent e);
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueReadBuffer(
            CLCommandQueue command_queue,
            CLMem buffer,
            CLBool blocking_read,
            SizeT offset,
            SizeT cb,
            [Out] short[] ptr,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            IntPtr e);
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueReadBuffer(
            CLCommandQueue command_queue,
            CLMem buffer,
            CLBool blocking_read,
            SizeT offset,
            SizeT cb,
            [Out] int[] ptr,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            ref CLEvent e);
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueReadBuffer(
            CLCommandQueue command_queue,
            CLMem buffer,
            CLBool blocking_read,
            SizeT offset,
            SizeT cb,
            [Out] int[] ptr,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            IntPtr e);
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueReadBuffer(
            CLCommandQueue command_queue,
            CLMem buffer,
            CLBool blocking_read,
            SizeT offset,
            SizeT cb,
            [Out] long[] ptr,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            ref CLEvent e);
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueReadBuffer(
            CLCommandQueue command_queue,
            CLMem buffer,
            CLBool blocking_read,
            SizeT offset,
            SizeT cb,
            [Out] long[] ptr,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            IntPtr e);
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueReadBuffer(
            CLCommandQueue command_queue,
            CLMem buffer,
            CLBool blocking_read,
            SizeT offset,
            SizeT cb,
            [Out] float[] ptr,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            ref CLEvent e);
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueReadBuffer(
            CLCommandQueue command_queue,
            CLMem buffer,
            CLBool blocking_read,
            SizeT offset,
            SizeT cb,
            [Out] float[] ptr,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            IntPtr e);
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueReadBuffer(
            CLCommandQueue command_queue,
            CLMem buffer,
            CLBool blocking_read,
            SizeT offset,
            SizeT cb,
            [Out] double[] ptr,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            ref CLEvent e);
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueReadBuffer(
            CLCommandQueue command_queue,
            CLMem buffer,
            CLBool blocking_read,
            SizeT offset,
            SizeT cb,
            [Out] double[] ptr,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            IntPtr e);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueWriteBuffer(
            CLCommandQueue command_queue,
            CLMem buffer,
            CLBool blocking_write,
            SizeT offset,
            SizeT cb,
            IntPtr ptr,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            ref CLEvent e);
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueWriteBuffer(
            CLCommandQueue command_queue,
            CLMem buffer,
            CLBool blocking_write,
            SizeT offset,
            SizeT cb,
            IntPtr ptr,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            IntPtr e);
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueWriteBuffer(
            CLCommandQueue command_queue,
            CLMem buffer,
            CLBool blocking_write,
            SizeT offset,
            SizeT cb,
            [In] byte[] ptr,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            ref CLEvent e);
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueWriteBuffer(
            CLCommandQueue command_queue,
            CLMem buffer,
            CLBool blocking_write,
            SizeT offset,
            SizeT cb,
            [In] byte[] ptr,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            IntPtr e);
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueWriteBuffer(
            CLCommandQueue command_queue,
            CLMem buffer,
            CLBool blocking_write,
            SizeT offset,
            SizeT cb,
            [In] short[] ptr,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            ref CLEvent e);
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueWriteBuffer(
            CLCommandQueue command_queue,
            CLMem buffer,
            CLBool blocking_write,
            SizeT offset,
            SizeT cb,
            [In] short[] ptr,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            IntPtr e);
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueWriteBuffer(
            CLCommandQueue command_queue,
            CLMem buffer,
            CLBool blocking_write,
            SizeT offset,
            SizeT cb,
            [In] int[] ptr,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            ref CLEvent e);
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueWriteBuffer(
            CLCommandQueue command_queue,
            CLMem buffer,
            CLBool blocking_write,
            SizeT offset,
            SizeT cb,
            [In] int[] ptr,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            IntPtr e);
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueWriteBuffer(
            CLCommandQueue command_queue,
            CLMem buffer,
            CLBool blocking_write,
            SizeT offset,
            SizeT cb,
            [In] long[] ptr,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            ref CLEvent e);
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueWriteBuffer(
            CLCommandQueue command_queue,
            CLMem buffer,
            CLBool blocking_write,
            SizeT offset,
            SizeT cb,
            [In] long[] ptr,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            IntPtr e);
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueWriteBuffer(
            CLCommandQueue command_queue,
            CLMem buffer,
            CLBool blocking_write,
            SizeT offset,
            SizeT cb,
            [In] float[] ptr,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            ref CLEvent e);
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueWriteBuffer(
            CLCommandQueue command_queue,
            CLMem buffer,
            CLBool blocking_write,
            SizeT offset,
            SizeT cb,
            [In] float[] ptr,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            IntPtr e);
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueWriteBuffer(
            CLCommandQueue command_queue,
            CLMem buffer,
            CLBool blocking_write,
            SizeT offset,
            SizeT cb,
            [In] double[] ptr,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            ref CLEvent e);
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueWriteBuffer(
            CLCommandQueue command_queue,
            CLMem buffer,
            CLBool blocking_write,
            SizeT offset,
            SizeT cb,
            [In] double[] ptr,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            IntPtr e);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueCopyBuffer(
            CLCommandQueue command_queue,
            CLMem src_buffer,
            CLMem dst_buffer,
            SizeT src_offset,
            SizeT dst_offset,
            SizeT cb,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            ref CLEvent e);
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueCopyBuffer(
            CLCommandQueue command_queue,
            CLMem src_buffer,
            CLMem dst_buffer,
            SizeT src_offset,
            SizeT dst_offset,
            SizeT cb,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            IntPtr e);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueReadImage(
            CLCommandQueue command_queue,
            CLMem image,
            CLBool blocking_read,
            SizeT[] origin,
            SizeT[] region,
            SizeT row_pitch,
            SizeT slice_pitch,
            IntPtr ptr,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            ref CLEvent e);
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueReadImage(
            CLCommandQueue command_queue,
            CLMem image,
            CLBool blocking_read,
            SizeT[] origin,
            SizeT[] region,
            SizeT row_pitch,
            SizeT slice_pitch,
            IntPtr ptr,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            IntPtr e);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueWriteImage(
            CLCommandQueue command_queue,
            CLMem image,
            CLBool blocking_write,
            SizeT[] origin,
            SizeT[] region,
            SizeT input_row_pitch,
            SizeT input_slice_pitch,
            IntPtr ptr,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            ref CLEvent e);
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueWriteImage(
            CLCommandQueue command_queue,
            CLMem image,
            CLBool blocking_write,
            SizeT[] origin,
            SizeT[] region,
            SizeT input_row_pitch,
            SizeT input_slice_pitch,
            IntPtr ptr,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            IntPtr e);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueCopyImage(
            CLCommandQueue command_queue,
            CLMem src_image,
            CLMem dst_image,
            SizeT[] src_origin,
            SizeT[] dst_origin,
            SizeT[] region,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            ref CLEvent e);
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueCopyImage(
            CLCommandQueue command_queue,
            CLMem src_image,
            CLMem dst_image,
            SizeT[] src_origin,
            SizeT[] dst_origin,
            SizeT[] region,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            IntPtr e);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueCopyImageToBuffer(
            CLCommandQueue command_queue,
            CLMem src_image,
            CLMem dst_buffer,
            SizeT[] src_origin,
            SizeT[] region,
            SizeT dst_offset,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            ref CLEvent e);
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueCopyImageToBuffer(
            CLCommandQueue command_queue,
            CLMem src_image,
            CLMem dst_buffer,
            SizeT[] src_origin,
            SizeT[] region,
            SizeT dst_offset,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            IntPtr e);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueCopyBufferToImage(
            CLCommandQueue command_queue,
            CLMem src_buffer,
            CLMem dst_image,
            SizeT src_offset,
            SizeT[] dst_origin,
            SizeT[] region,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            ref CLEvent e);
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueCopyBufferToImage(
            CLCommandQueue command_queue,
            CLMem src_buffer,
            CLMem dst_image,
            SizeT src_offset,
            SizeT[] dst_origin,
            SizeT[] region,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            IntPtr e);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern IntPtr clEnqueueMapBuffer(
            CLCommandQueue command_queue,
            CLMem buffer,
            CLBool blocking_map,
            CLMapFlags map_flags,
            SizeT offset,
            SizeT cb,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            ref CLEvent e,
            ref CLError errcode_ret);
        [DllImport(OPENCL_DLL_NAME)]
        public static extern IntPtr clEnqueueMapBuffer(
            CLCommandQueue command_queue,
            CLMem buffer,
            CLBool blocking_map,
            CLMapFlags map_flags,
            SizeT offset,
            SizeT cb,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            IntPtr e,
            ref CLError errcode_ret);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern IntPtr clEnqueueMapImage(
            CLCommandQueue command_queue,
            CLMem image,
            CLBool blocking_map,
            CLMapFlags map_flags,
            SizeT[] origin,
            SizeT[] region,
            ref SizeT image_row_pitch,
            ref SizeT image_slice_pitch,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            ref CLEvent e,
            ref CLError errcode_ret);
        [DllImport(OPENCL_DLL_NAME)]
        public static extern IntPtr clEnqueueMapImage(
            CLCommandQueue command_queue,
            CLMem image,
            CLBool blocking_map,
            CLMapFlags map_flags,
            SizeT[] origin,
            SizeT[] region,
            ref SizeT image_row_pitch,
            ref SizeT image_slice_pitch,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            IntPtr e,
            ref CLError errcode_ret);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueUnmapMemObject(
            CLCommandQueue command_queue,
            CLMem memobj,
            IntPtr mapped_ptr,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            ref CLEvent e);
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueUnmapMemObject(
            CLCommandQueue command_queue,
            CLMem memobj,
            IntPtr mapped_ptr,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            IntPtr e);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueNDRangeKernel(
            CLCommandQueue command_queue,
            CLKernel kernel,
            uint work_dim,
            [In] SizeT[] global_work_offset,
            [In] SizeT[] global_work_size,
            [In] SizeT[] local_work_size,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            ref CLEvent e);
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueNDRangeKernel(
            CLCommandQueue command_queue,
            CLKernel kernel,
            uint work_dim,
            [In] SizeT[] global_work_offset,
            [In] SizeT[] global_work_size,
            [In] SizeT[] local_work_size,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            IntPtr e);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueTask(
            CLCommandQueue command_queue,
            CLKernel kernel,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            ref CLEvent e);
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueTask(
            CLCommandQueue command_queue,
            CLKernel kernel,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            IntPtr e);

        public delegate void UserFunction(IntPtr[] args);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueNativeKernel(
            CLCommandQueue command_queue,
            UserFunction user_func,
            [In] IntPtr[] args,
            SizeT cb_args,
            uint num_mem_objects,
            [In] CLMem[] mem_list,
            [In] IntPtr[] args_mem_loc,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            ref CLEvent e);
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueNativeKernel(
            CLCommandQueue command_queue,
            UserFunction user_func,
            [In] IntPtr[] args,
            SizeT cb_args,
            uint num_mem_objects,
            [In] CLMem[] mem_list,
            [In] IntPtr[] args_mem_loc,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            IntPtr e);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueMarker(
            CLCommandQueue command_queue,
            ref CLEvent e);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueWaitForEvents(
            CLCommandQueue command_queue,
            uint num_events,
            [In] CLEvent[] event_list);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueBarrier(CLCommandQueue command_queue);
        #endregion

        #region Extension function access
        [DllImport(OPENCL_DLL_NAME)]
        public static extern IntPtr clGetExtensionFunctionAddress(string func_name);
        #endregion
    }
}
