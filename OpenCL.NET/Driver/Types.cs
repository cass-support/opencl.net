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

using CASS.Types;

namespace CASS.OpenCL
{
    #region Driver Types
    public struct CLPlatformID
    {
        public IntPtr Value;
    }

    public struct CLDeviceID
    {
        public IntPtr Value;
    }

    public struct CLContext
    {
        public IntPtr Value;
    }

    public struct CLCommandQueue
    {
        public IntPtr Value;
    }

    public struct CLMem
    {
        public IntPtr Value;
    }

    public struct CLProgram
    {
        public IntPtr Value;
    }

    public struct CLKernel
    {
        public IntPtr Value;
    }

    public struct CLEvent
    {
        public IntPtr Value;
    }

    public struct CLSampler
    {
        public IntPtr Value;
    }

    public struct CLImageFormat
    {
        public CLChannelOrder image_channel_order;
        public CLChannelType image_channel_data_type;
    }

    /* 1.2 */
    public struct CLImageDesc
    {
        CLMemObjectType image_type;
        SizeT image_width;
        SizeT image_height;
        SizeT image_depth;
        SizeT image_array_size;
        SizeT image_row_pitch;
        SizeT image_slice_pitch;
        uint num_mip_levels;
        uint num_samples;

        // Instead of union, buffer is represented by mem_object.
        CLMem mem_object;
    }

    /* 1.1 */
    public struct CLBufferRegion
    {
        public SizeT origin;
        public SizeT size;
    }
    #endregion

    #region Enums
    // Error Codes
    public enum CLError
    {
        Success = 0,
        DeviceNotFound = -1,
        DeviceNotAvailable = -2,
        DeviceCompilerNotAvailable = -3,
        MemObjectAllocationFailure = -4,
        OutOfResources = -5,
        OutOfHostMemory = -6,
        ProfilingInfoNotAvailable = -7,
        MemCopyOverlap = -8,
        ImageFormatMismatch = -9,
        ImageFormatNotSupported = -10,
        BuildProgramFailure = -11,
        MapFailure = -12,
        /* 1.1 */
        MisalignedSubBufferOffset = -13,
        ExecStatusErrorForEventsInWaitList = -14,
        /* 1.2 */
        CompileProgramFailure = -15,
        LinkerNotAvailable = -16,
        LinkProgramFailure = -17,
        DevicePartitionFailed = -18,
        KernelArgInfoNotAvailable = -19,

        InvalidValue = -30,
        InvalidDeviceType = -31,
        InvalidPlatform = -32,
        InvalidDevice = -33,
        InvalidContext = -34,
        InvalidQueueProperties = -35,
        InvalidCommandQueue = -36,
        InvalidHostPtr = -37,
        InvalidMemObject = -38,
        InvalidImageFormatDescriptor = -39,
        InvalidImageSize = -40,
        InvalidSampler = -41,
        InvalidBinary = -42,
        InvalidBuildOptions = -43,
        InvalidProgram = -44,
        InvalidProgramExecutable = -45,
        InvalidKernelName = -46,
        InvalidKernelDefinition = -47,
        InvalidKernel = -48,
        InvalidArgIndex = -49,
        InvalidArgValue = -50,
        InvalidArgSize = -51,
        InvalidKernelArgs = -52,
        InvalidWorkDimension = -53,
        InvalidWorkGroupSize = -54,
        InvalidWorkItemSize = -55,
        InvalidGlobalOffset = -56,
        InvalidEventWaitList = -57,
        InvalidEvent = -58,
        InvalidOperation = -59,
        InvalidGLObject = -60,
        InvalidBufferSize = -61,
        InvalidMipLevel = -62,
        InvalidGlobalWorkSize = -63,
        /* 1.1 */
        InvalidProperty = -64,
        /* 1.2 */
        InvalidImageDescriptor = -65,
        InvalidCompilerOptions = -66,
        InvalidLinkerOptions = -67,
        InvalidDevicePartitionCount = -68,
        /* 2.0 */
        InvalidPipeSize = -69,
        InvalidDeviceQueue = -70,
        /* 2.2 */
        InvalidSpecID = -71,
        MaxSizeRestrictionExceeded = -72,
    }

    // cl_bool
    public enum CLBool : uint
    {
        False = 0,
        True = 1
    }

    // cl_platform_info
    public enum CLPlatformInfo : uint
    {
        Profile = 0x0900,
        Version = 0x0901,
        Name = 0x0902,
        Vendor = 0x0903,
        Extensions = 0x0904,
        /* 2.1 */
        HostTimerResolution = 0x0905,
    }

    // cl_device_type - bitfield
    [Flags]
    public enum CLDeviceType : ulong
    {
        Default = (1 << 0),
        CPU = (1 << 1),
        GPU = (1 << 2),
        Accelerator = (1 << 3),
        /* 1.2 */
        Custom = 1 << 4,
        All = 0xFFFFFFFF,
    }

    // cl_device_info
    public enum CLDeviceInfo : uint
    {
        Type = 0x1000,
        VendorID = 0x1001,
        MaxComputeUnits = 0x1002,
        MaxWorkItemDimensions = 0x1003,
        MaxWorkGroupSize = 0x1004,
        MaxWorkItemSizes = 0x1005,
        PreferredVectorWidthChar = 0x1006,
        PreferredVectorWidthShort = 0x1007,
        PreferredVectorWidthInt = 0x1008,
        PreferredVectorWidthLong = 0x1009,
        PreferredVectorWidthFloat = 0x100A,
        PreferredVectorWidthDouble = 0x100B,
        MaxClockFrequency = 0x100C,
        AddressBits = 0x100D,
        MaxReadImageArgs = 0x100E,
        MaxWriteImageArgs = 0x100F,
        MaxMemAllocSize = 0x1010,
        Image2DMaxWidth = 0x1011,
        Image2DMaxHeight = 0x1012,
        Image3DMaxWidth = 0x1013,
        Image3DMaxHeight = 0x1014,
        Image3DMaxDepth = 0x1015,
        ImageSupport = 0x1016,
        MaxParameterSize = 0x1017,
        MaxSamplers = 0x1018,
        MemBaseAddrAlign = 0x1019,
        MinDataTypeAlignSize = 0x101A,
        SingleFPConfig = 0x101B,
        GlobalMemCacheType = 0x101C,
        GlobalMemCacheLineSize = 0x101D,
        GlobalMemCacheSize = 0x101E,
        GlobalMemSize = 0x101F,
        MaxConstantBufferSize = 0x1020,
        MaxConstantArgs = 0x1021,
        LocalMemType = 0x1022,
        LocalMemSize = 0x1023,
        ErrorCorrectionSupport = 0x1024,
        ProfilingTimerResolution = 0x1025,
        EndianLittle = 0x1026,
        Available = 0x1027,
        CompilerAvailable = 0x1028,
        ExecutionCapabilities = 0x1029,
        [Obsolete("Deprecated since OpenCL 2.0")]
        QueueProperties = 0x102A,
        /* 2.0 */
        QueueOnHostProperties = 0x102A,

        Name = 0x102B,
        Vendor = 0x102C,
        DriverVersion = 0x102D,
        Profile = 0x102E,
        Version = 0x102F,
        Extensions = 0x1030,
        Platform = 0x1031,
        /* 1.2 */
        DoubleFPConfig = 0x1032,

        /* 1.1 */
        /* 0x1032 reserved for CL_DEVICE_DOUBLE_FP_CONFIG */
        /* 0x1033 reserved for CL_DEVICE_HALF_FP_CONFIG */
        PreferredVectorWidthHalf = 0x1034,
        HostUnifiedMemory = 0x1035,
        NativeVectorWidthChar = 0x1036,
        NativeVectorWidthShort = 0x1037,
        NativeVectorWidthInt = 0x1038,
        NativeVectorWidthLong = 0x1039,
        NativeVectorWidthFloat = 0x103A,
        NativeVectorWidthDouble = 0x103B,
        NativeVectorWidthHalf = 0x103C,
        OpenCLCVersion = 0x103D,
        /* 1.2 */
        LinkerAvailable = 0x103E,
        BuiltInKernels = 0x103F,
        ImageMaxBufferSize = 0x1040,
        ImageMaxArraySize = 0x1041,
        ParentDevice = 0x1042,
        PartitionMaxSubDevices = 0x1043,
        PartitionProperties = 0x1044,
        PartitionAffinityDomain = 0x1045,
        PartitionType = 0x1046,
        ReferenceCount = 0x1047,
        PreferredInteropUserSync = 0x1048,
        PrintfBufferSize = 0x1049,
        ImagePitchAlignment = 0x104A,
        ImageBaseAddressAlignment = 0x104B,
        /* 2.0 */
        MaxReadWriteImageArgs = 0x104C,
        MaxGlobalVaribleSize = 0x104D,
        QueueOnDeviceProperties = 0x104E,
        QueueOnDevicePreferredSize = 0x104F,
        QueueOnDeviceMaxSize = 0x1050,
        MaxOnDeviceQueues = 0x1051,
        MaxOnDeviceEvents = 0x1052,
        SVMCapabilities = 0x1053,
        GlobalVariablePreferredTotalSize = 0x1054,
        MaxPipeArgs = 0x1055,
        PipeMaxActiveReservations = 0x1056,
        PipeMaxPacketSize = 0x1057,
        PreferredPlatformAtomicAlignment = 0x1058,
        PreferredGlobalAtomicAlignment = 0x1059,
        PreferredLocalAtomicAlignment = 0x105A,
        /* 2.1 */
        ILVersion = 0x105B,
        MaxNumSubGroups = 0x105C,
        SubGroupIndependentForwardProgress = 0x105D,
    }

    // cl_device_address_info - bitfield
    [Flags]
    public enum CLDeviceAddressInfo : ulong
    {
        Address32Bits = (1 << 0),
        Address64Bits = (1 << 1),
    }

    // cl_device_fp_config - bitfield
    [Flags]
    public enum CLDeviceFPConfig : ulong
    {
        Denorm = (1 << 0),
        InfNan = (1 << 1),
        RoundToNearest = (1 << 2),
        RoundToZero = (1 << 3),
        RoundToInf = (1 << 4),
        FMA = (1 << 5),
        /* 1.1 */
        SoftFloat = (1 << 6),
        /* 1.2 */
        CorrectlyRoundedDivideSqrt = 1 << 7,
    }

    // cl_device_mem_cache_type
    public enum CLDeviceMemCacheType : uint
    {
        None = 0x0,
        ReadOnlyCache = 0x1,
        ReadWriteCache = 0x2,
    }

    // cl_device_local_mem_type
    public enum CLDeviceLocalMemType : uint
    {
        Local = 0x1,
        Global = 0x2
    }

    // cl_device_exec_capabilities - bitfield
    [Flags]
    public enum CLDeviceExecCapabilities : ulong
    {
        Kernel = (1 << 0),
        NativeKernel = (1 << 1),
    }
    
    // cl_command_queue_properties - bitfield
    [Flags]
    public enum CLCommandQueueProperties : ulong
    {
        OutOfOrderExecModeEnable = 1 << 0,
        ProfilingEnable = 1 << 1,
        /* 2.0 */
        OnDevice = 1 << 2,
        OnDeviceDefault = 1 << 3,
    }
    
    // cl_context_info
    public enum CLContextInfo : uint
    {
        ReferenceCount = 0x1080,
        Devices = 0x1081,
        Properties = 0x1082,
        /* 1.1 */
        NumDevices = 0x1083,
    }

    // cl_context_properties
    public enum CLContextProperties : uint
    {
        Platform = 0x1084,
        /* 1.2 */
        InteropUserSync = 0x1085,
    }

    /* 1.2 */
    // cl_device_partition_property
    public enum CLDevicePartitionProperty : long
    {
        Equally = 0x1086,
        ByCounts = 0x1087,
        ByCountsListEnd = 0x0,
        ByAffinityDomain = 0x1088,
    }

    /* 1.2 */
    // cl_device_affinity_domain
    [Flags]
    public enum CLDeviceAffinityDomain : ulong
    {
        NUMA = 1 << 0,
        L4Cache = 1 << 1,
        L3Cache = 1 << 2,
        L2Cache = 1 << 3,
        L1Cache = 1 << 4,
        NextPartitionable = 1 << 5
    }

    /* 2.0 */
    // cl_device_svm_capabilities - bitfield
    [Flags]
    public enum CLDeviceSVMCapabilities : ulong
    {
        CoarseGrainBuffer = 1 << 0,
        FineGrainBuffer = 1 << 1,
        FineGrainSystem = 1 << 2,
        Atomics = 1 << 3,
    }

    // cl_command_queue_info
    public enum CLCommandQueueInfo : uint
    {
        Context = 0x1090,
        Device = 0x1091,
        ReferenceCount = 0x1092,
        Properties = 0x1093,
        /* 2.0 */
        Size = 0x1094,
        /* 2.1 */
        DeviceDefault = 0x1095,
    }

    // cl_mem_flags - bitfield
    [Flags]
    public enum CLMemFlags : ulong
    {
        ReadWrite = 1 << 0,
        WriteOnly = 1 << 1,
        ReadOnly = 1 << 2,
        UseHostPtr = 1 << 3,
        AllocHostPtr = 1 << 4,
        CopyHostPtr = 1 << 5,

        /* 1.2 */
        HostWriteOnly = 1 << 7,
        HostReadOnly = 1 << 8,
        HostNoAccess = 1 << 9,

        /* 2.0 */
        KernelReadAndWrite = 1 << 12,
    }

    /* 2.0 */
    // cl_svm_mem_flags - bitfield
    [Flags]
    public enum CLSVMMemFlags : ulong
    {
        ReadWrite = 1 << 0,
        WriteOnly = 1 << 1,
        ReadOnly = 1 << 2,

        /* 2.0 */
        SVMFineGrainBuffer = 1 << 10,   /* used by cl_svm_mem_flags only */
        SVMAtomics = 1 << 11,           /* used by cl_svm_mem_flags only */
    }

    /* 1.2 */
    // cl_mem_migration_flags - bitfield
    [Flags]
    public enum CLMemMigrationFlags : ulong
    {
        Host = 1 << 0,
        ContentUndefined = 1 << 1,
    }

    // cl_channel_order
    public enum CLChannelOrder : uint
    {
        R = 0x10B0,
        A = 0x10B1,
        RG = 0x10B2,
        RA = 0x10B3,
        RGB = 0x10B4,
        RGBA = 0x10B5,
        BGRA = 0x10B6,
        ARGB = 0x10B7,
        Intensity = 0x10B8,
        Luminance = 0x10B9,
        /* 1.1 */
        Rx = 0x10BA,
        RGx = 0x10BB,
        RGBx = 0x10BC,
        /* 1.2 */
        Depth = 0x10BD,
        DepthStencil = 0x10BE,
        /* 2.0 */
        sRGB = 0x10BF,
        sRGBx = 0x10C0,
        sRGBA = 0x10C1,
        sBGRA = 0x10C2,
        ABGR = 0x10C3,
    }

    // cl_channel_type
    public enum CLChannelType : uint
    {
        SNormInt8 = 0x10D0,
        SNormInt16 = 0x10D1,
        UNormInt8 = 0x10D2,
        UNormInt16 = 0x10D3,
        UNormShort565 = 0x10D4,
        UNormShort555 = 0x10D5,
        UNormInt101010 = 0x10D6,
        SignedInt8 = 0x10D7,
        SignedInt16 = 0x10D8,
        SignedInt32 = 0x10D9,
        UnSignedInt8 = 0x10DA,
        UnSignedInt16 = 0x10DB,
        UnSignedInt32 = 0x10DC,
        HalfFloat = 0x10DD,
        Float = 0x10DE,
        /* 1.2 */
        UnormInt24 = 0x10DF,
        /* 2.1 */
        UnormInt101010_2 = 0x10E0,
    }

    // cl_mem_object_type
    public enum CLMemObjectType : uint
    {
        Buffer = 0x10F0,
        Image2D = 0x10F1,
        Image3D = 0x10F2,
        /* 1.2 */
        Image2DArray = 0x10F3,
        Image1D = 0x10F4,
        Image1DArray = 0x10F5,
        Image1DBuffer = 0x10F6,
        /* 2.0 */
        Pipe = 0x10F7,
    }

    // cl_mem_info
    public enum CLMemInfo : uint
    {
        Type = 0x1100,
        Flags = 0x1101,
        Size = 0x1102,
        HostPtr = 0x1103,
        MapCount = 0x1104,
        ReferenceCount = 0x1105,
        Context = 0x1106,
        /* 1.1 */
        AssociatedMemObject = 0x1107,
        Offset = 0x1108,
        /* 2.0 */
        UsesSVMPointer = 0x1109,
    }

    // cl_image_info
    public enum CLImageInfo : uint
    {
        Format = 0x1110,
        ElementSize = 0x1111,
        RowPitch = 0x1112,
        SlicePitch = 0x1113,
        Width = 0x1114,
        Height = 0x1115,
        Depth = 0x1116,
        /* 1.2 */
        ArraySize = 0x1117,
        Buffer = 0x1118,
        NumMIPLevels = 0x1119,
        NumSamples = 0x111A,
    }

    /* 2.0 */
    // cl_pipe_info
    public enum CLPipeInfo : uint
    {
        PacketSize = 0x1120,
        MaxPackets = 0x1121,
    }

    // cl_addressing_mode
    public enum CLAddressingMode : uint
    {
        None = 0x1130,
        ClampToEdge = 0x1131,
        Clamp = 0x1132,
        Repeat = 0x1133,
        /* 1.1 */
        MirroredRepeat = 0x1134,
    }

    // cl_filter_mode
    public enum CLFilterMode : uint
    {
        Nearest = 0x1140,
        Linear = 0x1141,
    }

    // cl_sampler_info
    public enum CLSamplerInfo : uint
    {
        ReferenceCount = 0x1150,
        Context = 0x1151,
        NormalizedCoords = 0x1152,
        AddressingMode = 0x1153,
        FilterMode = 0x1154,
        /* 2.0 */
        MIPFilterMode = 0x1155,
        LODMin = 0x1156,
        LODMax = 0x1157,
    }

    // cl_map_flags - bitfield
    [Flags]
    public enum CLMapFlags : ulong
    {
        Read = 1 << 0,
        Write = 1 << 1,
        /* 1.2 */
        WriteInvalidateRegion = 1 << 2,
    }

    // cl_program_info
    public enum CLProgramInfo : uint
    {
        ReferenceCount = 0x1160,
        Context = 0x1161,
        NumDevices = 0x1162,
        Devices = 0x1163,
        Source = 0x1164,
        BinarySizes = 0x1165,
        Binaries = 0x1166,
        /* 1.2 */
        NumKernels = 0x1167,
        KernelNames = 0x1168,
        /* 2.1 */
        IL = 0x1169,
        /* 2.2 */
        ScopeGlobalCtorsPresent = 0x116A,
        ScopeGlobalDtorsPresent = 0x116B,
    }

    // cl_program_build_info
    public enum CLProgramBuildInfo : uint
    {
        Status = 0x1181,
        Options = 0x1182,
        Log = 0x1183,
        /* 1.2 */
        BinaryType = 0x1184,
        /* 2.0 */
        BuildGlobalVariableTotalSize = 0x1185,
    }

    /* 1.2 */
    // cl_program_binary_type
    public enum CLProgramBinaryType : uint
    {
        None = 0x0,
        CompiledObject = 0x1,
        Library = 0x2,
        Executable = 0x4,
    }

    // cl_build_status
    public enum CLBuildStatus
    {
        Success = 0,
        None = -1,
        Error = -2,
        InProgress = -3,
    }

    // cl_kernel_info
    public enum CLKernelInfo : uint
    {
        FunctionName = 0x1190,
        NumArgs = 0x1191,
        ReferenceCount = 0x1192,
        Context = 0x1193,
        Program = 0x1194,
        /* 1.2 */
        Attributes = 0x1195,
        /* 2.1 */
        MaxNumSubGroups = 0x11B9,
        CompileNumSubGroups = 0x11BA,
    }

    /* 1.2 */
    /* cl_kernel_arg_info */
    public enum CLKernelArgInfo : uint
    {
        AddressQualifier = 0x1196,
        AccessQualifier = 0x1197,
        TypeName = 0x1198,
        TypeQualifier = 0x1199,
        Name = 0x119A,
    }

    /* 1.2 */
    /* cl_kernel_arg_address_qualifier */
    public enum CLKernelArgAddressQualifier : uint
    {
        Global = 0x119B,
        Local = 0x119C,
        Constant = 0x119D,
        Private = 0x119E,
    }

    /* 1.2 */
    /* cl_kernel_arg_access_qualifier */
    public enum CLKernelArgAccessQualifier : uint
    {
        ReadOnly = 0x11A0,
        WriteOnly = 0x11A1,
        ReadWrite = 0x11A2,
        None = 0x11A3,
    }

    /* 1.2 */
    /* cl_kernel_arg_type_qualifier */
    [Flags]
    public enum CLKernelArgTypeQualifier : ulong
    {
        None = 0,
        Const = 1 << 0,
        Restrict = 1 << 1,
        Volatile = 1 << 2,
        /* 2.0 */
        Pipe = 1 << 3,
    }

    // cl_kernel_work_group_info
    public enum CLKernelWorkGroupInfo : uint
    {
        WorkGroupSize = 0x11B0,
        CompileWithWorkGroupSize = 0x11B1,
        LocalMemSize = 0x11B2,
        /* 1.1 */
        PreferredWorkGroupSizeMultiple = 0x11B3,
        PrivateMemSize = 0x11B4,
        /* 1.2 */
        GlobalWorkSize = 0x11B5,
    }

    /* 2.1 */
    // cl_kernel_sub_group_info
    public enum CLKernelSubGroupInfo : uint
    {
        MaxSubGroupSizeForNDRange = 0x2033,
        SubGroupCountForNDRange = 0x2034,
        LocalSizeForSubGroupCount = 0x11B8,
    }

    /* 2.0 */
    // cl_kernel_exec_info
    public enum CLKernelExecInfo : uint
    {
        SVMPtrs = 0x11B6,
        SVMFineGrainSystem = 0x11B7,
    }

    // cl_event_info
    public enum CLEventInfo : uint
    {
        CommandQueue = 0x11D0,
        CommandType = 0x11D1,
        ReferenceCount = 0x11D2,
        CommandExecutionStatus = 0x11D3,
        /* 1.1 */
        Context = 0x11D4,
    }

    // cl_command_type
    public enum CLCommandType : uint
    {
        NDRangeKernel = 0x11F0,
        Task = 0x11F1,
        NativeKernel = 0x11F2,
        ReadBuffer = 0x11F3,
        WriteBuffer = 0x11F4,
        CopyBuffer = 0x11F5,
        ReadImage = 0x11F6,
        WriteImage = 0x11F7,
        CopyImage = 0x11F8,
        CopyImageToBuffer = 0x11F9,
        CopyBufferToImage = 0x11FA,
        MapBuffer = 0x11FB,
        MapImage = 0x11FC,
        UnmapMemObject = 0x11FD,
        Marker = 0x11FE,
        AcquireGLObjects = 0x11FF,
        ReleaseGLObjects = 0x1200,
        /* 1.1 */
        ReadBufferRect = 0x1201,
        WriteBufferRect = 0x1202,
        CopyBufferRect = 0x1203,
        User = 0x1204,
        /* 1.2 */
        Barrier = 0x1205,
        MigrateMemObjects = 0x1206,
        FillBuffer = 0x1207,
        FillImage = 0x1208,
        /* 2.0 */
        SVMFree = 0x1209,
        SVMMemcpy = 0x120A,
        SVMMemFill = 0x120B,
        SVMMap = 0x120C,
        SVMUnmap = 0x120D,
    }

    // command execution status
    public enum CLExecutionStatus
    {
        Complete = 0x0,
        Running = 0x1,
        Submitted = 0x2,
        Queued = 0x3,
    }

    /* 1.1 */
    // cl_buffer_create_type
    public enum CLBufferCreateType : uint
    {
        Region = 0x1220,
    }

    // cl_profiling_info
    public enum CLProfilingInfo : uint
    {
        Queued = 0x1280,
        Submit = 0x1281,
        Start = 0x1282,
        End = 0x1283,
        /* 2.0 */
        Complete = 0x1284,
    }
    #endregion
}
