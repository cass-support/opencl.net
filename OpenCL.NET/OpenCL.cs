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
        /// <returns>Value which depends on the type of information requested.</returns>
        public static object GetPlatformInfo(CLPlatformID platform, CLPlatformInfo info)
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
                        result = Marshal.PtrToStringAnsi(ptr, param_value_size_ret);
                        break;
                    case CLPlatformInfo.Version:
                        result = Marshal.PtrToStringAnsi(ptr, param_value_size_ret);
                        break;
                    case CLPlatformInfo.Name:
                        result = Marshal.PtrToStringAnsi(ptr, param_value_size_ret);
                        break;
                    case CLPlatformInfo.Vendor:
                        result = Marshal.PtrToStringAnsi(ptr, param_value_size_ret);
                        break;
                    case CLPlatformInfo.Extensions:
                        result = Marshal.PtrToStringAnsi(ptr, param_value_size_ret);
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
        /// <returns>Value which depends on the type of information requested.</returns>
        public static object GetDeviceInfo(CLDeviceID device, CLDeviceInfo info)
        {
            //OpenCLDriver.clGetDeviceInfo(device, info, 

            CLError err = CLError.Success;

            // Define variables to store native information.
            SizeT param_value_size_ret = 0;
            IntPtr ptr = IntPtr.Zero;
            object result = null;

            // Get initial size of buffer to allocate.
            err = OpenCLDriver.clGetDeviceInfo(device, info, 0, IntPtr.Zero, ref param_value_size_ret);

            if (err != CLError.Success)
            {
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
                    case CLDeviceInfo.Type:
                        result = (CLDeviceType)Marshal.ReadInt64(ptr);
                        break;
                    case CLDeviceInfo.VendorID:
                        result = (uint)Marshal.ReadInt32(ptr);
                        break;
                    case CLDeviceInfo.MaxComputeUnits:
                        result = (uint)Marshal.ReadInt32(ptr);
                        break;
                    case CLDeviceInfo.MaxWorkItemDimensions:
                        result = (uint)Marshal.ReadInt32(ptr);
                        break;
                    case CLDeviceInfo.MaxWorkGroupSize:
                        result = Marshal.PtrToStructure(ptr, typeof(SizeT));
                        break;
                    case CLDeviceInfo.MaxWorkItemSizes:
                        uint dims = (uint)GetDeviceInfo(device, CLDeviceInfo.MaxWorkItemDimensions);
                        SizeT[] sizes = new SizeT[dims];
                        for (int i = 0; i < dims; i++)
                        {
                            sizes[i] = new SizeT(Marshal.ReadIntPtr(ptr, i * IntPtr.Size).ToInt64());
                        }

                        result = sizes;
                        break;
                    case CLDeviceInfo.PreferredVectorWidthChar:
                        result = (uint)Marshal.ReadInt32(ptr);
                        break;
                    case CLDeviceInfo.PreferredVectorWidthShort:
                        result = (uint)Marshal.ReadInt32(ptr);
                        break;
                    case CLDeviceInfo.PreferredVectorWidthInt:
                        result = (uint)Marshal.ReadInt32(ptr);
                        break;
                    case CLDeviceInfo.PreferredVectorWidthLong:
                        result = (uint)Marshal.ReadInt32(ptr);
                        break;
                    case CLDeviceInfo.PreferredVectorWidthFloat:
                        result = (uint)Marshal.ReadInt32(ptr);
                        break;
                    case CLDeviceInfo.PreferredVectorWidthDouble:
                        result = (uint)Marshal.ReadInt32(ptr);
                        break;
                    case CLDeviceInfo.MaxClockFrequency:
                        result = (uint)Marshal.ReadInt32(ptr);
                        break;
                    case CLDeviceInfo.AddressBits:
                        result = (uint)Marshal.ReadInt32(ptr);
                        break;
                    case CLDeviceInfo.MaxReadImageArgs:
                        result = (uint)Marshal.ReadInt32(ptr);
                        break;
                    case CLDeviceInfo.MaxWriteImageArgs:
                        result = (uint)Marshal.ReadInt32(ptr);
                        break;
                    case CLDeviceInfo.MaxMemAllocSize:
                        result = (ulong)Marshal.ReadInt64(ptr);
                        break;
                    case CLDeviceInfo.Image2DMaxWidth:
                        result = Marshal.PtrToStructure(ptr, typeof(SizeT));
                        break;
                    case CLDeviceInfo.Image2DMaxHeight:
                        result = Marshal.PtrToStructure(ptr, typeof(SizeT));
                        break;
                    case CLDeviceInfo.Image3DMaxWidth:
                        result = Marshal.PtrToStructure(ptr, typeof(SizeT));
                        break;
                    case CLDeviceInfo.Image3DMaxHeight:
                        result = Marshal.PtrToStructure(ptr, typeof(SizeT));
                        break;
                    case CLDeviceInfo.Image3DMaxDepth:
                        result = Marshal.PtrToStructure(ptr, typeof(SizeT));
                        break;
                    case CLDeviceInfo.ImageSupport:
                        result = (CLBool)Marshal.ReadInt32(ptr);
                        break;
                    case CLDeviceInfo.MaxParameterSize:
                        result = Marshal.PtrToStructure(ptr, typeof(SizeT));
                        break;
                    case CLDeviceInfo.MaxSamplers:
                        result = (uint)Marshal.ReadInt32(ptr);
                        break;
                    case CLDeviceInfo.MemBaseAddrAlign:
                        result = (uint)Marshal.ReadInt32(ptr);
                        break;
                    case CLDeviceInfo.MinDataTypeAlignSize:
                        result = (uint)Marshal.ReadInt32(ptr);
                        break;
                    case CLDeviceInfo.SingleFPConfig:
                        result = (CLDeviceFPConfig)Marshal.ReadInt64(ptr);
                        break;
                    case CLDeviceInfo.GlobalMemCacheType:
                        result = (CLDeviceMemCacheType)Marshal.ReadInt32(ptr);
                        break;
                    case CLDeviceInfo.GlobalMemCacheLineSize:
                        result = (uint)Marshal.ReadInt32(ptr);
                        break;
                    case CLDeviceInfo.GlobalMemCacheSize:
                        result = (ulong)Marshal.ReadInt64(ptr);
                        break;
                    case CLDeviceInfo.GlobalMemSize:
                        result = (ulong)Marshal.ReadInt64(ptr);
                        break;
                    case CLDeviceInfo.MaxConstantBufferSize:
                        result = (ulong)Marshal.ReadInt64(ptr);
                        break;
                    case CLDeviceInfo.MaxConstantArgs:
                        result = (uint)Marshal.ReadInt32(ptr);
                        break;
                    case CLDeviceInfo.LocalMemType:
                        result = (CLDeviceLocalMemType)Marshal.ReadInt32(ptr);
                        break;
                    case CLDeviceInfo.LocalMemSize:
                        result = (ulong)Marshal.ReadInt64(ptr);
                        break;
                    case CLDeviceInfo.ErrorCorrectionSupport:
                        result = (CLBool)Marshal.ReadInt32(ptr);
                        break;
                    case CLDeviceInfo.ProfilingTimerResolution:
                        result = Marshal.PtrToStructure(ptr, typeof(SizeT));
                        break;
                    case CLDeviceInfo.EndianLittle:
                        result = (CLBool)Marshal.ReadInt32(ptr);
                        break;
                    case CLDeviceInfo.Available:
                        result = (CLBool)Marshal.ReadInt32(ptr);
                        break;
                    case CLDeviceInfo.CompilerAvailable:
                        result = (CLBool)Marshal.ReadInt32(ptr);
                        break;
                    case CLDeviceInfo.ExecutionCapabilities:
                        result = (CLDeviceExecCapabilities)Marshal.ReadInt64(ptr);
                        break;
                    case CLDeviceInfo.QueueProperties:
                        result = (CLCommandQueueProperties)Marshal.ReadInt64(ptr);
                        break;
                    case CLDeviceInfo.Name:
                        result = Marshal.PtrToStringAnsi(ptr, param_value_size_ret);
                        break;
                    case CLDeviceInfo.Vendor:
                        result = Marshal.PtrToStringAnsi(ptr, param_value_size_ret);
                        break;
                    case CLDeviceInfo.DriverVersion:
                        result = Marshal.PtrToStringAnsi(ptr, param_value_size_ret);
                        break;
                    case CLDeviceInfo.Profile:
                        result = Marshal.PtrToStringAnsi(ptr, param_value_size_ret);
                        break;
                    case CLDeviceInfo.Version:
                        result = Marshal.PtrToStringAnsi(ptr, param_value_size_ret);
                        break;
                    case CLDeviceInfo.Extensions:
                        result = Marshal.PtrToStringAnsi(ptr, param_value_size_ret);
                        break;
                    case CLDeviceInfo.Platform:
                        result = Marshal.PtrToStructure(ptr, typeof(CLPlatformID));
                        break;
                    case CLDeviceInfo.PreferredVectorWidthHalf:
                        result = (uint)Marshal.ReadInt32(ptr);
                        break;
                    case CLDeviceInfo.HostUnifiedMemory:
                        result = (CLBool)Marshal.ReadInt32(ptr);
                        break;
                    case CLDeviceInfo.NativeVectorWidthChar:
                        result = (uint)Marshal.ReadInt32(ptr);
                        break;
                    case CLDeviceInfo.NativeVectorWidthShort:
                        result = (uint)Marshal.ReadInt32(ptr);
                        break;
                    case CLDeviceInfo.NativeVectorWidthInt:
                        result = (uint)Marshal.ReadInt32(ptr);
                        break;
                    case CLDeviceInfo.NativeVectorWidthLong:
                        result = (uint)Marshal.ReadInt32(ptr);
                        break;
                    case CLDeviceInfo.NativeVectorWidthFloat:
                        result = (uint)Marshal.ReadInt32(ptr);
                        break;
                    case CLDeviceInfo.NativeVectorWidthDouble:
                        result = (uint)Marshal.ReadInt32(ptr);
                        break;
                    case CLDeviceInfo.NativeVectorWidthHalf:
                        result = (uint)Marshal.ReadInt32(ptr);
                        break;
                    case CLDeviceInfo.OpenCLCVersion:
                        result = Marshal.PtrToStringAnsi(ptr, param_value_size_ret);
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
                        result = Marshal.PtrToStringAnsi(ptr, param_value_size_ret);
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
                        result = Marshal.PtrToStringAnsi(ptr, param_value_size_ret);
                        break;
                    case CLProgramBuildInfo.Log:
                        result = Marshal.PtrToStringAnsi(ptr, param_value_size_ret);
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
