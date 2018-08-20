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
            IntPtr platforms,
            ref uint num_platforms);

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
            IntPtr param_value,
            ref SizeT param_value_size_ret);

        /* 1.2 */
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clCreateSubDevices(
            CLDeviceID in_device,
            [In] IntPtr[] properties,
            uint num_devices,
            [In, Out] CLDeviceID[] out_devices,
            ref uint num_devices_ret);

        /* 1.2 */
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clRetainDevice(CLDeviceID device);

        /* 1.2 */
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clReleaseDevice(CLDeviceID device);

        /* 2.1 */
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clSetDefaultDeviceCommandQueue(
            CLContext context,
            CLDeviceID device,
            CLCommandQueue command_queue);

        /* 2.1 */
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clGetDeviceAndHostTimer(
            CLDeviceID device,
            ref ulong device_timestamp,
            ref ulong host_timestamp);

        /* 2.1 */
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clGetHostTimer(
            CLDeviceID device,
            ref ulong host_timestamp);
        #endregion

        #region Context APIs
        public delegate void LoggingFunction(
            IntPtr errinfo,
            IntPtr private_info,
            SizeT cb,
            IntPtr user_data);

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
        [Obsolete("Deprecated since OpenCL 2.0")]
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLCommandQueue clCreateCommandQueue(
            CLContext context,
            CLDeviceID device,
            CLCommandQueueProperties properties,
            ref CLError errcode_ret);

        /* 2.0 */
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLCommandQueue clCreateCommandQueueWithProperties(
            CLContext context,
            CLDeviceID device,
            [In] IntPtr[] properties,
            ref CLError errcode_ret);
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLCommandQueue clCreateCommandQueueWithProperties(
            CLContext context,
            CLDeviceID device,
            [In] IntPtr[] properties,
            IntPtr errcode_ret);

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

        [Obsolete("Deprecated since OpenCL 1.1")]
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

        /* 1.1 */
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLMem clCreateSubBuffer(
            CLMem buffer,
            CLMemFlags flags,
            CLBufferCreateType buffer_create_type,
            IntPtr buffer_create_info,
            ref CLError errcode_ret);
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLMem clCreateSubBuffer(
            CLMem buffer,
            CLMemFlags flags,
            CLBufferCreateType buffer_create_type,
            [MarshalAs(UnmanagedType.LPStruct)]
            CLBufferRegion buffer_create_info,
            ref CLError errcode_ret);

        [Obsolete("Deprecated since OpenCL 1.2")]
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

        [Obsolete("Deprecated since OpenCL 1.2")]
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

        /* 1.2 */
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLMem clCreateImage(
            CLContext context,
            CLMemFlags flags,
            [MarshalAs(UnmanagedType.LPStruct)]
            CLImageFormat image_format,
            [MarshalAs(UnmanagedType.LPStruct)]
            CLImageDesc image_desc,
            IntPtr host_ptr,
            ref CLError errcode_ret);
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLMem clCreateImage(
            CLContext context,
            CLMemFlags flags,
            [MarshalAs(UnmanagedType.LPStruct)]
            CLImageFormat image_format,
            [MarshalAs(UnmanagedType.LPStruct)]
            CLImageDesc image_desc,
            IntPtr host_ptr,
            IntPtr errcode_ret);

        /* 2.0 */
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLMem clCreatePipe(
            CLContext context,
            CLMemFlags flags,
            uint pipe_packet_size,
            uint pipe_max_packets,
            [In] IntPtr[] properties,
            ref CLError errcode_ret);
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLMem clCreatePipe(
            CLContext context,
            CLMemFlags flags,
            uint pipe_packet_size,
            uint pipe_max_packets,
            [In] IntPtr[] properties,
            IntPtr errcode_ret);

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
            IntPtr image_formats,
            ref uint num_image_formats);
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clGetSupportedImageFormats(
            CLContext context,
            CLMemFlags flags,
            CLMemObjectType image_type,
            uint num_entries,
            [Out] CLImageFormat[] image_formats,
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

        /* 2.0 */
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clGetPipeInfo(
            CLMem pipe,
            CLPipeInfo param_name,
            SizeT param_value_size,
            IntPtr param_value,
            ref SizeT param_value_size_ret);

        /* 1.1 */
        public delegate void DestructionFunction(
            CLMem memobj, 
            IntPtr user_data);
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clSetMemObjectDestructorCallback(
            CLMem memobj,
            DestructionFunction pfn_notify,
            IntPtr user_data);
        #endregion

        /* 2.0 */
        #region SVM Allocation APIs
        [DllImport(OPENCL_DLL_NAME)]
        public static extern IntPtr clSVMAlloc(
            CLContext context,
            CLSVMMemFlags flags,
            SizeT size,
            uint alignment);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern void clSVMFree(
            CLContext context,
            IntPtr svm_pointer);
        #endregion

        #region Sampler APIs
        [Obsolete("Deprecated since OpenCL 2.0")]
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLSampler clCreateSampler(
            CLContext context,
            CLBool normalized_coords,
            CLAddressingMode addressing_mode,
            CLFilterMode filter_mode,
            ref CLError errcode_ret);

        /* 2.0 */
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLSampler clCreateSamplerWithProperties(
            CLContext context,
            [In] IntPtr[] sampler_properties,
            ref CLError errcode_ret);
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLSampler clCreateSamplerWithProperties(
            CLContext context,
            [In] IntPtr[] sampler_properties,
            IntPtr errcode_ret);

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
            IntPtr strings,
            [In] SizeT[] lengths,
            ref CLError errcode_ret);
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
            [In] IntPtr[] strings,
            [In] SizeT[] lengths,
            ref CLError errcode_ret);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLProgram clCreateProgramWithBinary(
            CLContext context,
            uint num_devices,
            [In] CLDeviceID[] device_list,
            [In] SizeT[] lengths,
            [In] IntPtr[] binaries,
            [In, Out] int[] binary_status,
            ref CLError errcode_ret);

        /* 1.2 */
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLProgram clCreateProgramWithBuiltInKernels(
            CLContext context,
            uint num_devices,
            [In] CLDeviceID[] device_list,
            string kernel_names,
            ref CLError errcode_ret);

        /* 2.1 */
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLProgram clCreateProgramWithIL(
            CLContext context,
            IntPtr il,
            SizeT length,
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

        /* 1.2 */
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clCompileProgram(
            CLProgram program,
            uint num_devices,
            [In] CLDeviceID[] device_list,
            string options,
            uint num_input_headers,
            [In] CLProgram[] input_headers,
            [In] IntPtr[] header_include_names,
            NotifyFunction pfn_notify,
            IntPtr user_data);

        /* 1.2 */
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLProgram clLinkProgram(
            CLContext context,
            uint num_devices,
            [In] CLDeviceID[] device_list,
            string options,
            uint num_input_programs,
            [In] CLProgram[] input_programs,
            NotifyFunction pfn_notify,
            IntPtr user_data,
            ref CLError errcode_ret);
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLProgram clLinkProgram(
            CLContext context,
            uint num_devices,
            [In] CLDeviceID[] device_list,
            string options,
            uint num_input_programs,
            [In] CLProgram[] input_programs,
            NotifyFunction pfn_notify,
            IntPtr user_data,
            IntPtr errcode_ret);

        /* 2.2 */
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clSetProgramReleaseCallback(
            CLProgram program,
            NotifyFunction pfn_notify,
            IntPtr user_data);

        /* 2.2 */
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clSetProgramSpecializationConstant(
            CLProgram program,
            uint spec_id,
            SizeT spec_size,
            IntPtr spec_value);

        [Obsolete("Deprecated since OpenCL 1.2")]
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clUnloadCompiler();

        /* 1.2 */
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clUnloadPlatformCompiler(CLPlatformID platform);

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

        /* 2.1 */
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLKernel clCloneKernel(
            CLKernel source_kernel,
            ref CLError errcode_ret);

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
            [In] byte[] arg_value);
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
            [In] short[] arg_value);
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
            [In] int[] arg_value);
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
            [In] long[] arg_value);
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
            [In] float[] arg_value);
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
            [In] double[] arg_value);

        /* 2.0 */
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clSetKernelArgSVMPointer(
            CLKernel kernel,
            uint arg_index,
            IntPtr arg_value);

        /* 2.0 */
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clSetKernelExecInfo(
            CLKernel kernel,
            CLKernelExecInfo param_name,
            SizeT param_value_size,
            IntPtr param_value);
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clSetKernelExecInfo(
            CLKernel kernel,
            CLKernelExecInfo param_name,
            SizeT param_value_size,
            [In] IntPtr[] param_value);
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clSetKernelExecInfo(
            CLKernel kernel,
            CLKernelExecInfo param_name,
            SizeT param_value_size,
            ref CLBool param_value);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clGetKernelInfo(
            CLKernel kernel,
            CLKernelInfo param_name,
            SizeT param_value_size,
            IntPtr param_value,
            ref SizeT param_value_size_ret);

        /* 1.2 */
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clGetKernelArgInfo(
            CLKernel kernel,
            uint arg_indx,
            CLKernelArgInfo param_name,
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

        /* 2.1 */
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clGetKernelSubGroupInfo(
            CLKernel kernel,
            CLDeviceID device,
            CLKernelSubGroupInfo param_name,
            SizeT input_value_size,
            IntPtr input_value,
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
        public static extern CLEvent clCreateUserEvent(
            CLContext context,
            IntPtr errcode_ret);
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLEvent clCreateUserEvent(
            CLContext context,
            ref CLError errcode_ret);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clRetainEvent(CLEvent e);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clReleaseEvent(CLEvent e);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clSetUserEventStatus(
            CLEvent e,
            int execution_status);
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clSetUserEventStatus(
            CLEvent e,
            CLExecutionStatus execution_status);

        public delegate void EventCallback(
            CLEvent e,
            int event_command_exec_status,
            IntPtr user_data);

        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clSetEventCallback(
            CLEvent e,
            CLExecutionStatus command_exec_callback_type,
            EventCallback pfn_notify,
            IntPtr user_data);
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

        /* 1.1 */
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueReadBufferRect(
            CLCommandQueue command_queue,
            CLMem buffer,
            CLBool blocking_read,
            [In] SizeT[] buffer_origin,
            [In] SizeT[] host_origin,
            [In] SizeT[] region,
            SizeT buffer_row_pitch,
            SizeT buffer_slice_pitch,
            SizeT host_row_pitch,
            SizeT host_slice_pitch,
            IntPtr ptr,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            ref CLEvent e);
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueReadBufferRect(
            CLCommandQueue command_queue,
            CLMem buffer,
            CLBool blocking_read,
            [In] SizeT[] buffer_origin,
            [In] SizeT[] host_origin,
            [In] SizeT[] region,
            SizeT buffer_row_pitch,
            SizeT buffer_slice_pitch,
            SizeT host_row_pitch,
            SizeT host_slice_pitch,
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

        /* 1.1 */
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueWriteBufferRect(
            CLCommandQueue command_queue,
            CLMem buffer,
            CLBool blocking_read,
            [In] SizeT[] buffer_origin,
            [In] SizeT[] host_origin,
            [In] SizeT[] region,
            SizeT buffer_row_pitch,
            SizeT buffer_slice_pitch,
            SizeT host_row_pitch,
            SizeT host_slice_pitch,
            IntPtr ptr,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            ref CLEvent e);
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueWriteBufferRect(
            CLCommandQueue command_queue,
            CLMem buffer,
            CLBool blocking_read,
            [In] SizeT[] buffer_origin,
            [In] SizeT[] host_origin,
            [In] SizeT[] region,
            SizeT buffer_row_pitch,
            SizeT buffer_slice_pitch,
            SizeT host_row_pitch,
            SizeT host_slice_pitch,
            IntPtr ptr,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            IntPtr e);

        /* 1.2 */
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueFillBuffer(
            CLCommandQueue command_queue,
            CLMem buffer,
            IntPtr pattern,
            SizeT pattern_size,
            SizeT offset,
            SizeT size,
            uint  num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            ref CLEvent e);
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueFillBuffer(
            CLCommandQueue command_queue,
            CLMem buffer,
            IntPtr pattern,
            SizeT pattern_size,
            SizeT offset,
            SizeT size,
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
        public static extern CLError clEnqueueCopyBufferRect(
            CLCommandQueue command_queue,
            CLMem src_buffer,
            CLMem dst_buffer,
            [In] SizeT[] src_origin,
            [In] SizeT[] dst_origin,
            [In] SizeT[] region,
            SizeT src_row_pitch,
            SizeT src_slice_pitch,
            SizeT dst_row_pitch,
            SizeT dst_slice_pitch,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            ref CLEvent e);
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueCopyBufferRect(
            CLCommandQueue command_queue,
            CLMem src_buffer,
            CLMem dst_buffer,
            [In] SizeT[] src_origin,
            [In] SizeT[] dst_origin,
            [In] SizeT[] region,
            SizeT src_row_pitch,
            SizeT src_slice_pitch,
            SizeT dst_row_pitch,
            SizeT dst_slice_pitch,
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

        /* 1.2 */
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueFillImage(
            CLCommandQueue command_queue,
            CLMem image,
            IntPtr fill_color,
            [In] SizeT[] origin,
            [In] SizeT[] region,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            ref CLEvent e);
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueFillImage(
            CLCommandQueue command_queue,
            CLMem image,
            IntPtr fill_color,
            [In] SizeT[] origin,
            [In] SizeT[] region,
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

        /* 1.2 */
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueMigrateMemObjects(
            CLCommandQueue command_queue,
            uint num_mem_objects,
            [In] CLMem[] mem_objects,
            CLMemMigrationFlags flags,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            ref CLEvent e);
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueMigrateMemObjects(
            CLCommandQueue command_queue,
            uint num_mem_objects,
            [In] CLMem[] mem_objects,
            CLMemMigrationFlags flags,
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

        [Obsolete("Deprecated since OpenCL 2.0")]
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueTask(
            CLCommandQueue command_queue,
            CLKernel kernel,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            ref CLEvent e);
        [Obsolete("Deprecated since OpenCL 2.0")]
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

        [Obsolete("Deprecated since OpenCL 1.2")]
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueMarker(
            CLCommandQueue command_queue,
            ref CLEvent e);

        [Obsolete("Deprecated since OpenCL 1.2")]
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueWaitForEvents(
            CLCommandQueue command_queue,
            uint num_events,
            [In] CLEvent[] event_list);

        [Obsolete("Deprecated since OpenCL 1.2")]
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueBarrier(CLCommandQueue command_queue);

        /* 1.2 */
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueMarkerWithWaitList(
            CLCommandQueue command_queue,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            ref CLEvent e);

        /* 1.2 */
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueBarrierWithWaitList(
            CLCommandQueue command_queue,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            ref CLEvent e);

        /* 2.0 */
        public delegate void SVMFreeFunction(CLCommandQueue queue, uint num_svm_pointers, IntPtr[] svm_pointers, IntPtr user_data);
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueSVMFree(
            CLCommandQueue command_queue,
            uint num_svm_pointers,
            [In] IntPtr[] svm_pointers,
            SVMFreeFunction pfn_free_func,
            IntPtr user_data,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            ref CLEvent e);

        /* 2.0 */
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueSVMMemcpy(
            CLCommandQueue command_queue,
            CLBool blocking_copy,
            IntPtr dst_ptr,
            IntPtr src_ptr,
            SizeT size,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            ref CLEvent e);

        /* 2.0 */
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueSVMMemFill(
            CLCommandQueue command_queue,
            IntPtr svm_ptr,
            IntPtr pattern,
            SizeT pattern_size,
            SizeT size,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            ref CLEvent e);

        /* 2.0 */
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueSVMMap(
            CLCommandQueue command_queue,
            CLBool blocking_map,
            CLMapFlags flags,
            IntPtr svm_ptr,
            SizeT size,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            ref CLEvent e);

        /* 2.0 */
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueSVMUnmap(
            CLCommandQueue command_queue,
            IntPtr svm_ptr,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            ref CLEvent e);

        /* 2.1 */
        [DllImport(OPENCL_DLL_NAME)]
        public static extern CLError clEnqueueSVMMigrateMem(
            CLCommandQueue command_queue,
            uint num_svm_pointers,
            [In] IntPtr[] svm_pointers,
            [In] SizeT[] sizes,
            CLMemMigrationFlags flags,
            uint num_events_in_wait_list,
            [In] CLEvent[] event_wait_list,
            ref CLEvent e);
        #endregion

        #region Extension function access
        [Obsolete("Deprecated since OpenCL 1.2")]
        [DllImport(OPENCL_DLL_NAME)]
        public static extern IntPtr clGetExtensionFunctionAddress(string func_name);

        /* 1.2 */
        [DllImport(OPENCL_DLL_NAME)]
        public static extern IntPtr clGetExtensionFunctionAddressForPlatform(
            CLPlatformID platform,
            string func_name);
        #endregion
    }
}
