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
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

using CASS.Types;

namespace CASS.OpenCL
{
    /// <summary>
    /// This class provides object oriented access to OpenCL(TM) driver API 
    /// and further utilities for simpler programming.
    /// 
    /// There are three ways to instantiate this class:
    /// 1. Specify a single device - a context will be created with given 
    /// information.
    /// 2. Specify multiple devices - a context will be created with given 
    /// information and sharing of listed devices.
    /// 3. Specify a previously created context. A new context will not be 
    /// created, but the reference count of the specified context will be incremented by 1.
    /// 
    /// Throughout the lifetime of an instance, a single context can be created.
    /// </summary>
    public class OpenCL : IDisposable
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance, with an OpenCL(TM) context created using the given 
        /// platform and device.
        /// </summary>
        /// <param name="platform">Platform for OpenCL(TM) context creation.</param>
        /// <param name="device">Device to include in OpenCL(TM) context.</param>
        public OpenCL(CLPlatformID platform, CLDeviceID device) : 
            this(platform, new CLDeviceID[] { device })
        { }

        /// <summary>
        /// Creates a new instance, with an OpenCL(TM) context created using the given 
        /// platform and multiple devices.
        /// </summary>
        /// <param name="platform">Platform for OpenCL(TM) context creation.</param>
        /// <param name="devices">Devices to include in OpenCL(TM) context.</param>
        public OpenCL(CLPlatformID platform, CLDeviceID[] devices)
        {
            IntPtr[] ctxProperties = new IntPtr[3];
            ctxProperties[0] = new IntPtr((int)CLContextProperties.Platform);
            ctxProperties[1] = platform.Value;
            ctxProperties[2] = IntPtr.Zero;

            // Create OpenCL context from given platform and device.
            CLContext ctx = OpenCLDriver.clCreateContext(ctxProperties, (uint)devices.Length, devices, null, IntPtr.Zero, ref clError);
            ThrowCLException(clError);

            Context = ctx;
            Devices = devices;
            LastEnqueueEvent = new CLEvent();
        }

        /// <summary>
        /// Creates a new instance from already existing OpenCL(TM) context.
        /// Perform a retain of the context.
        /// </summary>
        /// <param name="ctx">OpenCL(TM) context to use.</param>
        public OpenCL(CLContext ctx)
        {
            clError = OpenCLDriver.clRetainContext(ctx);
            ThrowCLException(clError);

            Context = ctx;
        }
        #endregion

        #region Destructor / IDisposable
        /// <summary>
        /// Disposes OpenCL(TM) context (release).
        /// </summary>
        public void Dispose()
        {
            if (disposed)
            {
                return;
            }

            clError = OpenCLDriver.clReleaseContext(ctx);
            ThrowCLException(clError);

            disposed = true;
        }

        /// <summary>
        /// Destructor.
        /// </summary>
        ~OpenCL()
        {
            Dispose();
        }
        #endregion

        #region Context Functions
        /// <summary>
        /// Returns requested information about the context of the instance.
        /// </summary>
        /// <param name="info">Requested information.</param>
        /// <returns>Value which depends on the type of information requested.</returns>
        public object GetContextInfo(CLContextInfo info)
        {
            return GetContextInfo(ctx, info);
        }
        #endregion

        #region Queue Functions
        public CLCommandQueue CreateCommandQueue(CLDeviceID device)
        {
            return CreateCommandQueue(device, 0);
        }

        public CLCommandQueue CreateCommandQueue(CLDeviceID device, 
            CLCommandQueueProperties properties)
        {
            CLCommandQueue queue = OpenCLDriver.clCreateCommandQueue(ctx, device, properties, ref clError);
            ThrowCLException(clError);

            return queue;
        }

        public void RetainCommandQueue(CLCommandQueue command_queue)
        {
            clError = OpenCLDriver.clRetainCommandQueue(command_queue);
            ThrowCLException(clError);
        }

        public void ReleaseCommandQueue(CLCommandQueue command_queue)
        {
            clError = OpenCLDriver.clReleaseCommandQueue(command_queue);
            ThrowCLException(clError);
        }

        public void Flush(CLCommandQueue command_queue)
        {
            clError = OpenCLDriver.clFlush(command_queue);
            ThrowCLException(clError);
        }

        public void Finish(CLCommandQueue command_queue)
        {
            clError = OpenCLDriver.clFinish(command_queue);
            ThrowCLException(clError);
        }
        #endregion

        #region Memory Functions
        public CLMem CreateBuffer(SizeT sizeInBytes)
        {
            return CreateBuffer(CLMemFlags.ReadWrite, sizeInBytes);
        }

        public CLMem CreateBuffer(CLMemFlags flags, SizeT sizeInBytes)
        {
            CLMem buffer = OpenCLDriver.clCreateBuffer(ctx, flags, sizeInBytes, IntPtr.Zero, ref clError);
            ThrowCLException(clError);

            return buffer;
        }

        public CLMem CreateImage2D(CLMemFlags flags, CLImageFormat format, 
            SizeT width, SizeT height, SizeT rowPitchInBytes)
        {
            CLMem image = OpenCLDriver.clCreateImage2D(ctx, flags, ref format, 
                width, height, rowPitchInBytes, IntPtr.Zero, ref clError);
            ThrowCLException(clError);

            return image;
        }

        public CLMem CreateImage3D(CLMemFlags flags, CLImageFormat format,
            SizeT width, SizeT height, SizeT depth,
            SizeT rowPitchInBytes, SizeT slicePitchInBytes)
        {
            CLMem image = OpenCLDriver.clCreateImage3D(ctx, flags, ref format,
                width, height, depth, rowPitchInBytes, slicePitchInBytes, 
                IntPtr.Zero, ref clError);
            ThrowCLException(clError);

            return image;
        }

        public void RetainMemObject(CLMem obj)
        {
            clError = OpenCLDriver.clRetainMemObject(obj);
            ThrowCLException(clError);
        }

        public void ReleaseMemObject(CLMem obj)
        {
            clError = OpenCLDriver.clReleaseMemObject(obj);
            ThrowCLException(clError);
        }

        public CLImageFormat[] GetSupportedImageFormats(CLMemFlags flags, CLMemObjectType imageType)
        {
            uint numImageFormats = 0;
            clError = OpenCLDriver.clGetSupportedImageFormats(ctx, flags, 
                imageType, 0, IntPtr.Zero, ref numImageFormats);
            ThrowCLException(clError);

            if (numImageFormats < 1)
            {
                return new CLImageFormat[0];
            }

            CLImageFormat[] formats = new CLImageFormat[numImageFormats];
            clError = OpenCLDriver.clGetSupportedImageFormats(ctx, flags,
                imageType, numImageFormats, formats, ref numImageFormats);
            ThrowCLException(clError);

            return formats;
        }
        #endregion

        #region Program Functions
        public CLProgram CreateProgramWithSource(string source)
        {
            return CreateProgramWithSource(new string[] { source });
        }

        public CLProgram CreateProgramWithSource(string[] sources)
        {
            SizeT[] lengths = new SizeT[sources.Length];
            for (int i = 0; i < sources.Length; i++)
			{
                lengths[i] = sources[i].Length;
			}

            CLProgram program = OpenCLDriver.clCreateProgramWithSource(ctx, 
                (uint)sources.Length, sources, lengths, ref clError);
            ThrowCLException(clError);

            return program;
        }

        public CLProgram CreateProgramWithBinary(byte[] binary)
        {
            return CreateProgramWithBinary(new byte[][] { binary });
        }

        public CLProgram CreateProgramWithBinary(byte[][] binaries)
        {
            SizeT[] lengths = new SizeT[binaries.Length];
            for (int i = 0; i < binaries.Length; i++)
            {
                lengths[i] = binaries[i].Length;
            }

            // Allocate native pointers for binaries and copy data.
            IntPtr[] ptrToBinaries = new IntPtr[binaries.Length];
            try
            {
                for (int i = 0; i < binaries.Length; i++)
                {
                    ptrToBinaries[i] = Marshal.AllocHGlobal(lengths[i]);

                    Marshal.Copy(binaries[i], 0, ptrToBinaries[i], lengths[i]);
                }
            
                int[] binaryStatus = new int[binaries.Length];

                CLProgram program = OpenCLDriver.clCreateProgramWithBinary(ctx,
                    (uint)Devices.Length, Devices, lengths, ptrToBinaries, binaryStatus, ref clError);
                ThrowCLException(clError);

                return program;
            }
            finally
            {
                for (int i = 0; i < ptrToBinaries.Length; i++)
                {
                    if (ptrToBinaries[i] != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(ptrToBinaries[i]);
                    }
                }
            }
        }

        public void RetainProgram(CLProgram program)
        {
            clError = OpenCLDriver.clRetainProgram(program);
            ThrowCLException(clError);
        }

        public void ReleaseProgram(CLProgram program)
        {
            clError = OpenCLDriver.clReleaseProgram(program);
            ThrowCLException(clError);
        }

        public void BuildProgram(CLProgram program, CLDeviceID[] devices, string options)
        {
            clError = OpenCLDriver.clBuildProgram(program, (uint)devices.Length, 
                devices, options, null, IntPtr.Zero);
            ThrowCLException(clError);
        }
        #endregion

        #region Kernel Functions
        public CLKernel CreateKernel(CLProgram program, string kernelName)
        {
            CLKernel kernel = OpenCLDriver.clCreateKernel(program, kernelName, ref clError);
            ThrowCLException(clError);

            return kernel;
        }

        public void RetainKernel(CLKernel kernel)
        {
            clError = OpenCLDriver.clRetainKernel(kernel);
            ThrowCLException(clError);
        }

        public void ReleaseKernel(CLKernel kernel)
        {
            clError = OpenCLDriver.clReleaseKernel(kernel);
            ThrowCLException(clError);
        }

        #region SetKernelArg
        public void SetKernelArg(CLKernel kernel, uint index, byte value)
        {
            clError = OpenCLDriver.clSetKernelArg(kernel, index, sizeof(byte), ref value);
            ThrowCLException(clError);
        }

        public void SetKernelArg(CLKernel kernel, uint index, short value)
        {
            clError = OpenCLDriver.clSetKernelArg(kernel, index, sizeof(short), ref value);
            ThrowCLException(clError);
        }

        public void SetKernelArg(CLKernel kernel, uint index, int value)
        {
            clError = OpenCLDriver.clSetKernelArg(kernel, index, sizeof(int), ref value);
            ThrowCLException(clError);
        }

        public void SetKernelArg(CLKernel kernel, uint index, long value)
        {
            clError = OpenCLDriver.clSetKernelArg(kernel, index, sizeof(long), ref value);
            ThrowCLException(clError);
        }

        public void SetKernelArg(CLKernel kernel, uint index, float value)
        {
            clError = OpenCLDriver.clSetKernelArg(kernel, index, sizeof(float), ref value);
            ThrowCLException(clError);
        }

        public void SetKernelArg(CLKernel kernel, uint index, double value)
        {
            clError = OpenCLDriver.clSetKernelArg(kernel, index, sizeof(double), ref value);
            ThrowCLException(clError);
        }

        public void SetKernelArg(CLKernel kernel, uint index, CLMem value)
        {
            clError = OpenCLDriver.clSetKernelArg(kernel, index, Marshal.SizeOf(value), ref value);
            ThrowCLException(clError);
        }

        public void SetKernelArg(CLKernel kernel, uint index, byte[] value)
        {
            clError = OpenCLDriver.clSetKernelArg(kernel, index, sizeof(byte) * value.Length, value);
            ThrowCLException(clError);
        }

        public void SetKernelArg(CLKernel kernel, uint index, short[] value)
        {
            clError = OpenCLDriver.clSetKernelArg(kernel, index, sizeof(short) * value.Length, value);
            ThrowCLException(clError);
        }

        public void SetKernelArg(CLKernel kernel, uint index, int[] value)
        {
            clError = OpenCLDriver.clSetKernelArg(kernel, index, sizeof(int) * value.Length, value);
            ThrowCLException(clError);
        }

        public void SetKernelArg(CLKernel kernel, uint index, long[] value)
        {
            clError = OpenCLDriver.clSetKernelArg(kernel, index, sizeof(long) * value.Length, value);
            ThrowCLException(clError);
        }

        public void SetKernelArg(CLKernel kernel, uint index, float[] value)
        {
            clError = OpenCLDriver.clSetKernelArg(kernel, index, sizeof(float) * value.Length, value);
            ThrowCLException(clError);
        }

        public void SetKernelArg(CLKernel kernel, uint index, double[] value)
        {
            clError = OpenCLDriver.clSetKernelArg(kernel, index, sizeof(double) * value.Length, value);
            ThrowCLException(clError);
        }
        #endregion
        #endregion

        #region Event Functions
        //TODO: Add event functions.
        #endregion

        #region Enqueue Functions
        public void ReadBuffer<T>(CLCommandQueue queue, CLMem buffer, CLBool blocking, SizeT offset, SizeT cb, T[] dst)
        {
            GCHandle h = GCHandle.Alloc(dst, GCHandleType.Pinned);

            try
            {
                clError = OpenCLDriver.clEnqueueReadBuffer(queue, buffer, blocking, offset, cb, h.AddrOfPinnedObject(), 0, null, ref lastOperationEvent);
                ThrowCLException(clError);
            }
            finally
            {
                h.Free();
            }
        }

        public void WriteBuffer<T>(CLCommandQueue queue, CLMem buffer, CLBool blocking, SizeT offset, SizeT cb, T[] src)
        {
            GCHandle h = GCHandle.Alloc(src, GCHandleType.Pinned);

            try
            {
                clError = OpenCLDriver.clEnqueueWriteBuffer(queue, buffer, blocking, offset, cb, h.AddrOfPinnedObject(), 0, null, ref lastOperationEvent);
                ThrowCLException(clError);
            }
            finally
            {
                h.Free();
            }
        }

        public void NDRangeKernel(CLCommandQueue queue, CLKernel kernel, uint work_dim,
            SizeT[] global_work_offset, SizeT[] global_work_size, SizeT[] local_work_size)
        {
            clError = OpenCLDriver.clEnqueueNDRangeKernel(queue, kernel, work_dim, global_work_offset, global_work_size, local_work_size, 0, null, ref lastOperationEvent);
            ThrowCLException(clError);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets last OpenCL(TM) error that occured when calling an API function.
        /// </summary>
        public CLError LastCLError
        {
            get { return clError; }
        }

        /// <summary>
        /// Gets OpenCL(TM) context used by this instance.
        /// </summary>
        public CLContext Context
        {
            get { return ctx; }
            private set { ctx = value; }
        }

        /// <summary>
        /// Gets devices attached to the current context.
        /// </summary>
        public CLDeviceID[] Devices { get; private set; }

        /// <summary>
        /// Gets the event associated with the last enqueue operation.
        /// </summary>
        public CLEvent LastEnqueueEvent
        {
            get
            {
                return lastOperationEvent;
            }

            private set
            {
                lastOperationEvent = value;
            }
        }
        #endregion

        #region Internal Variables
        private CLError clError;
        private CLContext ctx;

        private bool disposed = false;
        #endregion

        #region Platform Utilities
        /// <summary>
        /// Returns an array of available platforms in the system.
        /// If no platform is found, an array with zero elements is returned.
        /// </summary>
        /// <returns>An array of available platforms in the system.</returns>
        public static CLPlatformID[] GetPlatforms()
        {
            // Check how many platforms are available.
            uint num_platforms = 0;
            CLError err = OpenCLDriver.clGetPlatformIDs(0, IntPtr.Zero, ref num_platforms);

            if (err != CLError.Success)
            {
                throw new OpenCLException(err);
            }

            if (num_platforms < 1)
            {
                return new CLPlatformID[0];
            }

            // Get the actual platforms once we know their amount.
            CLPlatformID[] platforms = new CLPlatformID[num_platforms];
            err = OpenCLDriver.clGetPlatformIDs(num_platforms, platforms, ref num_platforms);

            return platforms;
        }

        /// <summary>
        /// Returns requested information about a platform.
        /// </summary>
        /// <param name="platform">Platform ID to query.</param>
        /// <param name="info">Requested information.</param>
        /// <param name="silent">False - throw exception on on incorrect <paramref name="info"/> value. True - don't throw exception on incorrect <paramref name="info"/> value and return null. Default false.</param>
        /// <returns>Value which depends on the type of information requested.</returns>
        public static object GetPlatformInfo(CLPlatformID platform, CLPlatformInfo info, bool silent = false)
        {
            CLError err = CLError.Success;

            // Define variables to store native information.
            SizeT param_value_size_ret = 0;
            IntPtr ptr = IntPtr.Zero;
            object result = null;

            // Get initial size of buffer to allocate.
            err = OpenCLDriver.clGetPlatformInfo(platform, info, 0, IntPtr.Zero, ref param_value_size_ret);

            if (err != CLError.Success)
            {
                if (silent && err == CLError.InvalidValue)
                    return null;
                throw new OpenCLException(err);
            }

            if (param_value_size_ret < 1)
            {
                return result;
            }

            // Allocate native memory to store value.
            ptr = Marshal.AllocHGlobal(param_value_size_ret);

            // Protect following statements with try-finally in case something 
            // goes wrong.
            try
            {
                // Get actual value.
                err = OpenCLDriver.clGetPlatformInfo(platform, info,
                param_value_size_ret, ptr, ref param_value_size_ret);

                switch (info)
                {
                    case CLPlatformInfo.Profile:
                    case CLPlatformInfo.Version:
                    case CLPlatformInfo.Name:
                    case CLPlatformInfo.Vendor:
                    case CLPlatformInfo.Extensions:
                    case CLPlatformInfo.IcdSuffixKhr:
                        result = Marshal.PtrToStringAnsi(ptr, param_value_size_ret).TrimEnd(' ', '\0');
                        break;
                    default:
                        var resultArr = new byte[param_value_size_ret];
                        Marshal.Copy(ptr, resultArr, 0, param_value_size_ret);
                        result = resultArr;
                        break;
                }
            }
            finally
            {
                // Free native buffer.
                Marshal.FreeHGlobal(ptr);
            }

            return result;
        }

        /// <summary>
        /// Returns all information about a platform.
        /// </summary>
        /// <param name="platform">Platform ID to query.</param>
        public static Dictionary<CLPlatformInfo, object> GetPlatformInfos(CLPlatformID platform)
        {
            var infos = new Dictionary<CLPlatformInfo, object>();
            
            foreach (var clPlatformInfo in Enum.GetValues(typeof(CLPlatformInfo)).Cast<CLPlatformInfo>()) //Enumerable.Range(0, 0x10_0000).Select(x => (CLPlatformInfo)x))
            {
                try
                {
                    var value = GetPlatformInfo(platform, clPlatformInfo, true);
                    if (value != null)
                        infos[clPlatformInfo] = value;
                }
                catch (OpenCLException)
                { }
            }

            return infos;
        }
        #endregion

        #region Device Utilities
        /// <summary>
        /// Returns an array of available devices in the platform of all types.
        /// If no device is found, an array with zero elements is returned.
        /// </summary>
        /// <param name="platform">Platform to query devices.</param>
        /// <returns>An array of available devices in the platform of all types.</returns>
        public static CLDeviceID[] GetDevices(CLPlatformID platform)
        {
            return GetDevices(platform, CLDeviceType.All);
        }

        /// <summary>
        /// Returns an array of available devices in the platform with following type.
        /// If no device is found, an array with zero elements is returned.
        /// </summary>
        /// <param name="platform">Platform to query devices.</param>
        /// <param name="devType">Type of device to query.</param>
        /// <returns>An array of available devices in the platform with following type.</returns>
        public static CLDeviceID[] GetDevices(CLPlatformID platform, CLDeviceType devType)
        {
            // Check how many devices are available.
            uint num_devices = 0;
            CLError err = OpenCLDriver.clGetDeviceIDs(platform, devType, 0, IntPtr.Zero, ref num_devices);

            if (err != CLError.Success)
            {
                throw new OpenCLException(err);
            }

            if (num_devices < 1)
            {
                return new CLDeviceID[0];
            }

            // Get the actual devices once we know their amount.
            CLDeviceID[] devices = new CLDeviceID[num_devices];
            err = OpenCLDriver.clGetDeviceIDs(platform, devType, num_devices, devices, ref num_devices);

            return devices;
        }

        /// <summary>
        /// Returns requested information about a device.
        /// </summary>
        /// <param name="device">Device ID to query.</param>
        /// <param name="info">Requested information.</param>
        /// <param name="silent">False - throw exception on on incorrect <paramref name="info"/> value. True - don't throw exception on incorrect <paramref name="info"/> value and return null. Default false.</param>
        /// <returns>Value which depends on the type of information requested.</returns>
        public static object GetDeviceInfo(CLDeviceID device, CLDeviceInfo info, bool silent = false)
        {
            CLError err = CLError.Success;

            // Define variables to store native information.
            SizeT param_value_size_ret = 0;
            IntPtr ptr = IntPtr.Zero;
            object result = null;

            // Get initial size of buffer to allocate.
            err = OpenCLDriver.clGetDeviceInfo(device, info, 0, IntPtr.Zero, ref param_value_size_ret);

            if (err != CLError.Success)
            {
                if (silent && err == CLError.InvalidValue)
                    return null;
                throw new OpenCLException(err);
            }

            if (param_value_size_ret < 1)
            {
                return result;
            }

            // Allocate native memory to store value.
            ptr = Marshal.AllocHGlobal(param_value_size_ret);

            // Protect following statements with try-finally in case something 
            // goes wrong.
            try
            {
                // Get actual value.
                err = OpenCLDriver.clGetDeviceInfo(device, info,
                param_value_size_ret, ptr, ref param_value_size_ret);

                switch (info)
                {
                    // Read bool.
                    case CLDeviceInfo.ImageSupport:
                    case CLDeviceInfo.ErrorCorrectionSupport:
                    case CLDeviceInfo.EndianLittle:
                    case CLDeviceInfo.Available:
                    case CLDeviceInfo.CompilerAvailable:
                    case CLDeviceInfo.HostUnifiedMemory:
                    case CLDeviceInfo.LinkerAvailable:
                    case CLDeviceInfo.PreferredInteropUserSync:
                    case CLDeviceInfo.GpuOverlapNv:
                    case CLDeviceInfo.KernelExecTimeoutNv:
                    case CLDeviceInfo.IntegratedMemoryNv:
                    case CLDeviceInfo.PciBusIdNv:
                    case CLDeviceInfo.PciSlotIdNv:
                    case CLDeviceInfo.SubGroupIndependentForwardProgress:
                    case CLDeviceInfo.AvcMeSupportsTextureSamplerUseIntel:
                    case CLDeviceInfo.AvcMeSupportsPreemptionIntel:
                        if (param_value_size_ret != 4)
                            throw new InvalidOperationException($"param_value_size_ret is {param_value_size_ret}. Expected 4.");
                        var value = Marshal.ReadInt32(ptr);
                        if (value != 0 && value != 1)
                            throw new InvalidOperationException($"value of {info} is {value}. Expected bool.");
                        result = (CLBool)Marshal.ReadInt32(ptr);
                        break;

                    // Read uint.
                    case CLDeviceInfo.VendorID:
                    case CLDeviceInfo.MaxComputeUnits:
                    case CLDeviceInfo.MaxWorkItemDimensions:
                    case CLDeviceInfo.PreferredVectorWidthChar:
                    case CLDeviceInfo.PreferredVectorWidthShort:
                    case CLDeviceInfo.PreferredVectorWidthInt:
                    case CLDeviceInfo.PreferredVectorWidthLong:
                    case CLDeviceInfo.PreferredVectorWidthFloat:
                    case CLDeviceInfo.PreferredVectorWidthDouble:
                    case CLDeviceInfo.MaxClockFrequency:
                    case CLDeviceInfo.AddressBits:
                    case CLDeviceInfo.MaxReadImageArgs:
                    case CLDeviceInfo.MaxWriteImageArgs:
                    case CLDeviceInfo.MaxSamplers:
                    case CLDeviceInfo.MemBaseAddrAlign:
                    case CLDeviceInfo.MinDataTypeAlignSize:
                    case CLDeviceInfo.GlobalMemCacheLineSize:
                    case CLDeviceInfo.MaxConstantArgs:
                    case CLDeviceInfo.PreferredVectorWidthHalf:
                    case CLDeviceInfo.NativeVectorWidthChar:
                    case CLDeviceInfo.NativeVectorWidthShort:
                    case CLDeviceInfo.NativeVectorWidthInt:
                    case CLDeviceInfo.NativeVectorWidthLong:
                    case CLDeviceInfo.NativeVectorWidthFloat:
                    case CLDeviceInfo.NativeVectorWidthDouble:
                    case CLDeviceInfo.NativeVectorWidthHalf:
                    case CLDeviceInfo.PartitionMaxSubDevices:
                    case CLDeviceInfo.ReferenceCount:
                    case CLDeviceInfo.QueueOnDevicePreferredSize:
                    case CLDeviceInfo.QueueOnDeviceMaxSize:
                    case CLDeviceInfo.MaxOnDeviceQueues:
                    case CLDeviceInfo.MaxOnDeviceEvents:
                    case CLDeviceInfo.ComputeCapabilityMajorNv:
                    case CLDeviceInfo.ComputeCapabilityMinorNv:
                    case CLDeviceInfo.RegistersPerBlockNv:
                    case CLDeviceInfo.WarpSizeNv:
                    case CLDeviceInfo.AttributeAsyncEngineCountNv:
                    case CLDeviceInfo.ImagePitchAlignment:
                    case CLDeviceInfo.ImageBaseAddressAlignment:
                    case CLDeviceInfo.MaxReadWriteImageArgs:
                    case CLDeviceInfo.MaxPipeArgs:
                    case CLDeviceInfo.PipeMaxActiveReservations:
                    case CLDeviceInfo.PipeMaxPacketSize:
                    case CLDeviceInfo.PreferredPlatformAtomicAlignment:
                    case CLDeviceInfo.PreferredGlobalAtomicAlignment:
                    case CLDeviceInfo.PreferredLocalAtomicAlignment:
                    case CLDeviceInfo.MaxNumSubGroups:
                    case CLDeviceInfo.NumSimultaneousInteropsIntel:
                    case CLDeviceInfo.MeVersionIntel:
                    case CLDeviceInfo.AvcMeVersionIntel:
                        if (param_value_size_ret != 4)
                            throw new InvalidOperationException($"param_value_size_ret is {param_value_size_ret}. Expected 4.");
                        result = (uint)Marshal.ReadInt32(ptr);
                        break;

                    // Read ulong.
                    case CLDeviceInfo.MaxMemAllocSize:
                    case CLDeviceInfo.GlobalMemCacheSize:
                    case CLDeviceInfo.GlobalMemSize:
                    case CLDeviceInfo.MaxConstantBufferSize:
                    case CLDeviceInfo.LocalMemSize:
                        if (param_value_size_ret != 8)
                            throw new InvalidOperationException($"param_value_size_ret is {param_value_size_ret}. Expected 8.");
                        result = (ulong)Marshal.ReadInt64(ptr);
                        break;

                    // Read SizeT.
                    case CLDeviceInfo.MaxWorkGroupSize:
                    case CLDeviceInfo.Image2DMaxWidth:
                    case CLDeviceInfo.Image2DMaxHeight:
                    case CLDeviceInfo.Image3DMaxWidth:
                    case CLDeviceInfo.Image3DMaxHeight:
                    case CLDeviceInfo.Image3DMaxDepth:
                    case CLDeviceInfo.MaxParameterSize:
                    case CLDeviceInfo.ProfilingTimerResolution:
                    case CLDeviceInfo.ImageMaxArraySize:
                    case CLDeviceInfo.ImageMaxBufferSize:
                    case CLDeviceInfo.PrintfBufferSize:
                    case CLDeviceInfo.GlobalVariablePreferredTotalSize:
                    case CLDeviceInfo.MaxGlobalVaribleSize:
                    case CLDeviceInfo.PlanarYuvMaxWidthIntel:
                    case CLDeviceInfo.PlanarYuvMaxHeightIntel:
                        if (param_value_size_ret != IntPtr.Size)
                            throw new InvalidOperationException($"param_value_size_ret is {param_value_size_ret}. Expected {IntPtr.Size}.");
                        result = Marshal.PtrToStructure(ptr, typeof(SizeT));
                        break;

                    // Read string.
                    case CLDeviceInfo.Name:
                    case CLDeviceInfo.Vendor:
                    case CLDeviceInfo.DriverVersion:
                    case CLDeviceInfo.Profile:
                    case CLDeviceInfo.Version:
                    case CLDeviceInfo.Extensions:
                    case CLDeviceInfo.OpenCLCVersion:
                    case CLDeviceInfo.BuiltInKernels:
                    case CLDeviceInfo.ILVersion:
                    case CLDeviceInfo.SpirVersions:
                        result = Marshal.PtrToStringAnsi(ptr, param_value_size_ret).TrimEnd(' ', '\0');
                        break;

                    // Read other.
                    case CLDeviceInfo.Type:
                        result = (CLDeviceType)Marshal.ReadInt64(ptr);
                        break;
                    case CLDeviceInfo.MaxWorkItemSizes:
                    {
                        uint dims = (uint) GetDeviceInfo(device, CLDeviceInfo.MaxWorkItemDimensions);
                        SizeT[] sizes = new SizeT[dims];
                        for (int i = 0; i < dims; i++)
                        {
                            sizes[i] = new SizeT(Marshal.ReadIntPtr(ptr, i * IntPtr.Size).ToInt64());
                        }

                        result = sizes;
                    }
                        break;
                    case CLDeviceInfo.SubGroupSizesIntel:
                    {
                        var sizes = new SizeT[param_value_size_ret / IntPtr.Size];
                        for (var i = 0; i < sizes.Length; i++)
                            sizes[i] = new SizeT(Marshal.ReadIntPtr(ptr, i * IntPtr.Size).ToInt64());
                        result = sizes;
                    }
                        break;
                    case CLDeviceInfo.HalfFpConfig:
                    case CLDeviceInfo.SingleFPConfig:
                    case CLDeviceInfo.DoubleFPConfig:
                        result = (CLDeviceFPConfig)Marshal.ReadInt64(ptr);
                        break;
                    case CLDeviceInfo.GlobalMemCacheType:
                        result = (CLDeviceMemCacheType)Marshal.ReadInt32(ptr);
                        break;
                    case CLDeviceInfo.LocalMemType:
                        result = (CLDeviceLocalMemType)Marshal.ReadInt32(ptr);
                        break;
                    case CLDeviceInfo.ExecutionCapabilities:
                        result = (CLDeviceExecCapabilities)Marshal.ReadInt64(ptr);
                        break;
                    case CLDeviceInfo.QueueProperties:
                        result = (CLCommandQueueProperties)Marshal.ReadInt64(ptr);
                        break;
                    case CLDeviceInfo.PartitionAffinityDomain:
                        result = (CLDeviceAffinityDomain)Marshal.ReadInt64(ptr);
                        break;
                    case CLDeviceInfo.QueueOnDeviceProperties:
                        result = (CLCommandQueueProperties)Marshal.ReadInt64(ptr);
                        break;
                    case CLDeviceInfo.SVMCapabilities:
                        result = (CLDeviceSVMCapabilities)Marshal.ReadInt64(ptr);
                        break;
                    case CLDeviceInfo.Platform:
                        result = Marshal.PtrToStructure(ptr, typeof(CLPlatformID));
                        break;
                    case CLDeviceInfo.ParentDevice:
                        result = Marshal.PtrToStructure(ptr, typeof(CLDeviceID));
                        break;
                    case CLDeviceInfo.PartitionType:
                    case CLDeviceInfo.PartitionProperties:
                        var partitionPropertiesIntPtrs = new IntPtr[param_value_size_ret / IntPtr.Size];
                        Marshal.Copy(ptr, partitionPropertiesIntPtrs, 0, partitionPropertiesIntPtrs.Length);
                        
                        var partitionProperties = new CLDevicePartitionProperty[partitionPropertiesIntPtrs.Length];
                        for (var i = 0; i < partitionProperties.Length; i++)
                            partitionProperties[i] = (CLDevicePartitionProperty) partitionPropertiesIntPtrs[i].ToInt64();

                        result = partitionProperties;
                        break;
                    case CLDeviceInfo.SimultaneousInteropsIntel:
                        var resultIntArr = new int[param_value_size_ret / 4];
                        var resultUintArr = new uint[param_value_size_ret / 4];
                        Marshal.Copy(ptr, resultIntArr, 0, param_value_size_ret / 4);
                        for (var i = 0; i < resultIntArr.Length; i++)
                            resultUintArr[i] = (uint) resultIntArr[i];
                        result = resultUintArr;
                        break;

                    default:
                        var resultArr = new byte[param_value_size_ret];
                        Marshal.Copy(ptr, resultArr, 0, param_value_size_ret);
                        result = resultArr;
                        break;
                }
            }
            finally
            {
                // Free native buffer.
                Marshal.FreeHGlobal(ptr);
            }

            return result;
        }

        /// <summary>
        /// Returns all information about a device.
        /// </summary>
        /// <param name="device">Device ID to query.</param>
        public static Dictionary<CLDeviceInfo, object> GetDeviceInfos(CLDeviceID device)
        {
            var infos = new Dictionary<CLDeviceInfo, object>();

            foreach (var clDeviceInfo in Enum.GetValues(typeof(CLDeviceInfo)).Cast<CLDeviceInfo>()) // Enumerable.Range(0, 0x10_0000).Select(x => (CLDeviceInfo)x)
            {
                try
                {
                    var value = GetDeviceInfo(device, clDeviceInfo, true);
                    if (value != null)
                        infos[clDeviceInfo] = value;
                }
                catch (OpenCLException)
                { }
            }

            return infos;
        }

        #endregion

        #region Context Utilities
        /// <summary>
        /// Returns requested information about a context.
        /// </summary>
        /// <param name="ctx">Context to get information for.</param>
        /// <param name="info">Requested information.</param>
        /// <returns>Value which depends on the type of information requested.</returns>
        public static object GetContextInfo(CLContext ctx, CLContextInfo info)
        {
            CLError error = CLError.Success;

            // Define variables to store native information.
            SizeT param_value_size_ret = 0;
            IntPtr ptr = IntPtr.Zero;
            object result = null;

            // Get initial size of buffer to allocate.
            error = OpenCLDriver.clGetContextInfo(ctx, info, 0, IntPtr.Zero, ref param_value_size_ret);
            ThrowCLException(error);

            if (param_value_size_ret < 1)
            {
                return result;
            }

            // Allocate native memory to store value.
            ptr = Marshal.AllocHGlobal(param_value_size_ret);

            // Protect following statements with try-finally in case something 
            // goes wrong.
            try
            {
                // Get actual value.
                error = OpenCLDriver.clGetContextInfo(ctx, info,
                param_value_size_ret, ptr, ref param_value_size_ret);

                //TODO: Add implementation to missing cases.
                switch (info)
                {
                    case CLContextInfo.ReferenceCount:
                        result = (uint)Marshal.ReadInt32(ptr);
                        break;
                    case CLContextInfo.Devices:
                        break;
                    case CLContextInfo.Properties:
                        break;
                    case CLContextInfo.NumDevices:
                        result = (uint)Marshal.ReadInt32(ptr);
                        break;
                }
            }
            finally
            {
                // Free native buffer.
                Marshal.FreeHGlobal(ptr);
            }

            return result;
        }
        #endregion

        #region Command Queue Utilities
        public static object GetCommandQueueInfo(CLCommandQueue command_queue, CLCommandQueueInfo info)
        {
            CLError error = CLError.Success;

            // Define variables to store native information.
            SizeT param_value_size_ret = 0;
            IntPtr ptr = IntPtr.Zero;
            object result = null;

            // Get initial size of buffer to allocate.
            error = OpenCLDriver.clGetCommandQueueInfo(command_queue, info, 0, IntPtr.Zero, ref param_value_size_ret);
            ThrowCLException(error);

            if (param_value_size_ret < 1)
            {
                return result;
            }

            // Allocate native memory to store value.
            ptr = Marshal.AllocHGlobal(param_value_size_ret);

            // Protect following statements with try-finally in case something 
            // goes wrong.
            try
            {
                // Get actual value.
                error = OpenCLDriver.clGetCommandQueueInfo(command_queue, info,
                param_value_size_ret, ptr, ref param_value_size_ret);

                switch (info)
                {
                    case CLCommandQueueInfo.Context:
                        result = Marshal.PtrToStructure(ptr, typeof(CLContext));
                        break;
                    case CLCommandQueueInfo.Device:
                        result = Marshal.PtrToStructure(ptr, typeof(CLDeviceID));
                        break;
                    case CLCommandQueueInfo.ReferenceCount:
                        result = (uint)Marshal.ReadInt32(ptr);
                        break;
                    case CLCommandQueueInfo.Properties:
                        result = (CLCommandQueueProperties)Marshal.ReadInt64(ptr);
                        break;
                }
            }
            finally
            {
                // Free native buffer.
                Marshal.FreeHGlobal(ptr);
            }

            return result;
        }
        #endregion

        #region Memory Utilities
        public static object GetMemObjectInfo(CLMem memobj, CLMemInfo info)
        {
            CLError error = CLError.Success;

            // Define variables to store native information.
            SizeT param_value_size_ret = 0;
            IntPtr ptr = IntPtr.Zero;
            object result = null;

            // Get initial size of buffer to allocate.
            error = OpenCLDriver.clGetMemObjectInfo(memobj, info, 0, IntPtr.Zero, ref param_value_size_ret);
            ThrowCLException(error);

            if (param_value_size_ret < 1)
            {
                return result;
            }

            // Allocate native memory to store value.
            ptr = Marshal.AllocHGlobal(param_value_size_ret);

            // Protect following statements with try-finally in case something 
            // goes wrong.
            try
            {
                // Get actual value.
                error = OpenCLDriver.clGetMemObjectInfo(memobj, info,
                    param_value_size_ret, ptr, ref param_value_size_ret);

                //TODO: Add missing cases.

                switch (info)
                {
                    case CLMemInfo.Type:
                        result = (CLMemObjectType)Marshal.ReadInt32(ptr);
                        break;
                    case CLMemInfo.Flags:
                        result = (CLMemFlags)Marshal.ReadInt64(ptr);
                        break;
                    case CLMemInfo.Size:
                        result = new SizeT(Marshal.ReadIntPtr(ptr).ToInt64());
                        break;
                    case CLMemInfo.HostPtr:
                        result = Marshal.ReadIntPtr(ptr);
                        break;
                    case CLMemInfo.MapCount:
                        result = (uint)Marshal.ReadInt32(ptr);
                        break;
                    case CLMemInfo.ReferenceCount:
                        result = (uint)Marshal.ReadInt32(ptr);
                        break;
                    case CLMemInfo.Context:
                        result = Marshal.PtrToStructure(ptr, typeof(CLContext));
                        break;
                    case CLMemInfo.AssociatedMemObject:
                        result = Marshal.PtrToStructure(ptr, typeof(CLMem));
                        break;
                    case CLMemInfo.Offset:
                        result = new SizeT(Marshal.ReadIntPtr(ptr).ToInt64());
                        break;
                }
            }
            finally
            {
                // Free native buffer.
                Marshal.FreeHGlobal(ptr);
            }

            return result;
        }

        public static object GetImageInfo(CLMem memobj, CLImageInfo info)
        {
            CLError error = CLError.Success;

            // Define variables to store native information.
            SizeT param_value_size_ret = 0;
            IntPtr ptr = IntPtr.Zero;
            object result = null;

            // Get initial size of buffer to allocate.
            error = OpenCLDriver.clGetImageInfo(memobj, info, 0, IntPtr.Zero, ref param_value_size_ret);
            ThrowCLException(error);

            if (param_value_size_ret < 1)
            {
                return result;
            }

            // Allocate native memory to store value.
            ptr = Marshal.AllocHGlobal(param_value_size_ret);

            // Protect following statements with try-finally in case something 
            // goes wrong.
            try
            {
                // Get actual value.
                error = OpenCLDriver.clGetImageInfo(memobj, info,
                    param_value_size_ret, ptr, ref param_value_size_ret);

                //TODO: Add missing cases.

                switch (info)
                {
                    case CLImageInfo.Format:
                        result = Marshal.PtrToStructure(ptr, typeof(CLImageFormat));
                        break;
                    case CLImageInfo.ElementSize:
                        result = new SizeT(Marshal.ReadIntPtr(ptr).ToInt64());
                        break;
                    case CLImageInfo.RowPitch:
                        result = new SizeT(Marshal.ReadIntPtr(ptr).ToInt64());
                        break;
                    case CLImageInfo.SlicePitch:
                        result = new SizeT(Marshal.ReadIntPtr(ptr).ToInt64());
                        break;
                    case CLImageInfo.Width:
                        result = new SizeT(Marshal.ReadIntPtr(ptr).ToInt64());
                        break;
                    case CLImageInfo.Height:
                        result = new SizeT(Marshal.ReadIntPtr(ptr).ToInt64());
                        break;
                    case CLImageInfo.Depth:
                        result = new SizeT(Marshal.ReadIntPtr(ptr).ToInt64());
                        break;
                }
            }
            finally
            {
                // Free native buffer.
                Marshal.FreeHGlobal(ptr);
            }

            return result;
        }

        public static void SetMemObjectDestructorCallback(CLMem memobj, 
            OpenCLDriver.DestructionFunction function, IntPtr userData)
        {
            CLError error = OpenCLDriver.clSetMemObjectDestructorCallback(memobj, function, userData);
            ThrowCLException(error);
        }
        #endregion

        #region Program Utilities
        public static void UnloadCompiler()
        {
            ThrowCLException(OpenCLDriver.clUnloadCompiler());
        }

        public static object GetProgramInfo(CLProgram program, CLProgramInfo info)
        {
            CLError error = CLError.Success;

            // Define variables to store native information.
            SizeT param_value_size_ret = 0;
            IntPtr ptr = IntPtr.Zero;
            object result = null;

            // Get initial size of buffer to allocate.
            error = OpenCLDriver.clGetProgramInfo(program, info, 0, IntPtr.Zero, ref param_value_size_ret);
            ThrowCLException(error);

            if (param_value_size_ret < 1)
            {
                return result;
            }

            // Allocate native memory to store value.
            ptr = Marshal.AllocHGlobal(param_value_size_ret);

            // Protect following statements with try-finally in case something 
            // goes wrong.
            try
            {
                // Get actual value for normal scalars and arrays.
                if (info != CLProgramInfo.Binaries)
                {
                    error = OpenCLDriver.clGetProgramInfo(program, info,
                        param_value_size_ret, ptr, ref param_value_size_ret);
                }

                switch (info)
                {
                    case CLProgramInfo.ReferenceCount:
                        result = (uint)Marshal.ReadInt32(ptr);
                        break;
                    case CLProgramInfo.Context:
                        result = Marshal.PtrToStructure(ptr, typeof(CLContext));
                        break;
                    case CLProgramInfo.NumDevices:
                        result = (uint)Marshal.ReadInt32(ptr);
                        break;
                    case CLProgramInfo.Devices:
                        {
                            // Get number of devices.
                            uint numDevices = (uint)GetProgramInfo(program, CLProgramInfo.NumDevices);

                            // Read device IDs.
                            CLDeviceID[] devices = new CLDeviceID[numDevices];
                            for (int i = 0; i < numDevices; i++)
                            {
                                devices[i] = new CLDeviceID { Value = Marshal.ReadIntPtr(ptr, i * IntPtr.Size) };
                            }

                            result = devices;
                        }
                        break;
                    case CLProgramInfo.Source:
                        result = Marshal.PtrToStringAnsi(ptr, param_value_size_ret).TrimEnd(' ', '\0');
                        break;
                    case CLProgramInfo.BinarySizes:
                        {
                            // Get number of devices.
                            uint numDevices = (uint)GetProgramInfo(program, CLProgramInfo.NumDevices);

                            // Read binary sizes.
                            SizeT[] binarySizes = new SizeT[numDevices];
                            for (int i = 0; i < numDevices; i++)
                            {
                                binarySizes[i] = (long)Marshal.ReadIntPtr(ptr, i * IntPtr.Size);
                            }

                            result = binarySizes;
                        }
                        break;
                    case CLProgramInfo.Binaries:
                        {
                            // Get number of devices.
                            uint numDevices = (uint)GetProgramInfo(program, CLProgramInfo.NumDevices);

                            // Get binary size for each device.
                            SizeT[] binarySizes = (SizeT[])GetProgramInfo(program, CLProgramInfo.BinarySizes);

                            // Allocate native pointers to store binary data for each device.
                            for (int i = 0; i < numDevices; i++)
                            {
                                Marshal.WriteIntPtr(ptr, i * IntPtr.Size, Marshal.AllocHGlobal(binarySizes[i]));
                            }

                            // Get actual value for normal scalars and arrays.
                            error = OpenCLDriver.clGetProgramInfo(program, info,
                                    param_value_size_ret, ptr, ref param_value_size_ret);

                            // Read program binaries.
                            byte[][] binaries = new byte[numDevices][];
                            for (int i = 0; i < numDevices; i++)
                            {
                                binaries[i] = new byte[binarySizes[i]];

                                IntPtr binarySourcePtr = Marshal.ReadIntPtr(ptr, i * IntPtr.Size);
                                if (binarySourcePtr != IntPtr.Zero)
                                {
                                    Marshal.Copy(binarySourcePtr, binaries[i], 0, binarySizes[i]);

                                    // Free native pointer for binary data.
                                    Marshal.FreeHGlobal(binarySourcePtr);
                                }
                            }

                            result = binaries;
                        }
                        break;
                }
            }
            finally
            {
                // Free native buffer.
                Marshal.FreeHGlobal(ptr);
            }

            return result;
        }

        public static object GetProgramBuildInfo(CLProgram program, CLDeviceID device,
            CLProgramBuildInfo info)
        {
            CLError error = CLError.Success;

            // Define variables to store native information.
            SizeT param_value_size_ret = 0;
            IntPtr ptr = IntPtr.Zero;
            object result = null;

            // Get initial size of buffer to allocate.
            error = OpenCLDriver.clGetProgramBuildInfo(program, device, info, 0, IntPtr.Zero, ref param_value_size_ret);
            ThrowCLException(error);

            if (param_value_size_ret < 1)
            {
                return result;
            }

            // Allocate native memory to store value.
            ptr = Marshal.AllocHGlobal(param_value_size_ret);

            // Protect following statements with try-finally in case something 
            // goes wrong.
            try
            {
                // Get actual value.
                error = OpenCLDriver.clGetProgramBuildInfo(program, device, info,
                    param_value_size_ret, ptr, ref param_value_size_ret);

                switch (info)
                {
                    case CLProgramBuildInfo.Status:
                        result = (CLBuildStatus)Marshal.ReadInt32(ptr);
                        break;
                    case CLProgramBuildInfo.Options:
                    case CLProgramBuildInfo.Log:
                        result = Marshal.PtrToStringAnsi(ptr, param_value_size_ret).TrimEnd(' ', '\0');
                        break;
                }
            }
            finally
            {
                // Free native buffer.
                Marshal.FreeHGlobal(ptr);
            }

            return result;
        }
        #endregion

        #region Event Utilities
        public static object GetEventProfilingInfo(CLEvent e, CLProfilingInfo info)
        {
            CLError error = CLError.Success;

            // Define variables to store native information.
            SizeT param_value_size_ret = 0;
            IntPtr ptr = IntPtr.Zero;
            object result = null;

            // Get initial size of buffer to allocate.
            error = OpenCLDriver.clGetEventProfilingInfo(e, info, 0, IntPtr.Zero, ref param_value_size_ret);
            ThrowCLException(error);

            if (param_value_size_ret < 1)
            {
                return result;
            }

            // Allocate native memory to store value.
            ptr = Marshal.AllocHGlobal(param_value_size_ret);

            // Protect following statements with try-finally in case something 
            // goes wrong.
            try
            {
                // Get actual value.
                error = OpenCLDriver.clGetEventProfilingInfo(e, info,
                    param_value_size_ret, ptr, ref param_value_size_ret);

                switch (info)
                {
                    case CLProfilingInfo.Queued:
                        result = (ulong)Marshal.ReadInt64(ptr);
                        break;
                    case CLProfilingInfo.Submit:
                        result = (ulong)Marshal.ReadInt64(ptr);
                        break;
                    case CLProfilingInfo.Start:
                        result = (ulong)Marshal.ReadInt64(ptr);
                        break;
                    case CLProfilingInfo.End:
                        result = (ulong)Marshal.ReadInt64(ptr);
                        break;
                }
            }
            finally
            {
                // Free native buffer.
                Marshal.FreeHGlobal(ptr);
            }

            return result;
        }
        #endregion

        #region Helper Functions
        private static void ThrowCLException(CLError error)
        {
            if (error == CLError.Success)
            {
                return;
            }

            throw new OpenCLException(error);
        }
        #endregion

        #region Internal Variables
        private CLEvent lastOperationEvent;
        #endregion
    }
}
