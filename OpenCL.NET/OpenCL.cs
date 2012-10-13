using System;
using CASS.Types;
using System.Runtime.InteropServices;

namespace CASS.OpenCL
{
    /// <summary>
    /// This class provides object oriented access to OpenCL(TM) driver API 
    /// and further utilities for simpler programming.
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
            ctx = OpenCLDriver.clCreateContext(ctxProperties, (uint)devices.Length, devices, null, IntPtr.Zero, ref clError);
            ThrowCLException(clError);
        }

        /// <summary>
        /// Creates a new instance from already existing OpenCL(TM) context.
        /// Perform a retain of the context.
        /// </summary>
        /// <param name="ctx">OpenCL(TM) context to use.</param>
        public OpenCL(CLContext ctx)
        {
            this.ctx = ctx;
            clError = OpenCLDriver.clRetainContext(ctx);
            ThrowCLException(clError);
        }
        #endregion

        #region Destructor / IDisposable
        /// <summary>
        /// Disposes OpenCL(TM) context (release).
        /// </summary>
        public void Dispose()
        {
            clError = OpenCLDriver.clReleaseContext(ctx);
            ThrowCLException(clError);
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

        public CLCommandQueue CreateCommandQueue(CLDeviceID device, CLCommandQueueProperties properties)
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
        public CLContext CLContext
        {
            get { return ctx; }
        }
        #endregion

        #region Internal Variables
        private CLError clError;
        private CLContext ctx;
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
                    case CLContextInfo.D3D10Device:
                        break;
                    case CLContextInfo.D3D10PreferSharedResources:
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
    }
}
