using System;
using CASS.Types;
using System.Runtime.InteropServices;

namespace CASS.OpenCL
{
    /// <summary>
    /// This class provides object oriented access to OpenCL(TM) driver API 
    /// and further utilities for simpler programming.
    /// </summary>
    public class OpenCL
    {
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
    }
}
