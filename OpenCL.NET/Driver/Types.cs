using System;
using System.Collections.Generic;
using System.Text;

namespace GASS.OpenCL
{
    #region Driver Types
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
        CLChannelOrder image_channel_order;
        CLChannelType image_channel_data_type;
    }
    #endregion

    #region Enums
    // Error Codes
    public enum CLError
    {
        CL_SUCCESS = 0,
        CL_DEVICE_NOT_FOUND = -1,
        CL_DEVICE_NOT_AVAILABLE = -2,
        CL_DEVICE_COMPILER_NOT_AVAILABLE = -3,
        CL_MEM_OBJECT_ALLOCATION_FAILURE = -4,
        CL_OUT_OF_RESOURCES = -5,
        CL_OUT_OF_HOST_MEMORY = -6,
        CL_PROFILING_INFO_NOT_AVAILABLE = -7,
        CL_MEM_COPY_OVERLAP = -8,
        CL_IMAGE_FORMAT_MISMATCH = -9,
        CL_IMAGE_FORMAT_NOT_SUPPORTED = -10,

        CL_INVALID_VALUE = -30,
        CL_INVALID_DEVICE_TYPE = -31,
        CL_INVALID_DEVICE = -32,
        CL_INVALID_CONTEXT = -33,
        CL_INVALID_QUEUE_PROPERTIES = -34,
        CL_INVALID_COMMAND_QUEUE = -35,
        CL_INVALID_HOST_PTR = -36,
        CL_INVALID_MEM_OBJECT = -37,
        CL_INVALID_IMAGE_FORMAT_DESCRIPTOR = -38,
        CL_INVALID_IMAGE_SIZE = -39,
        CL_INVALID_SAMPLER = -40,
        CL_INVALID_BINARY = -41,
        CL_INVALID_BUILD_OPTIONS = -42,
        CL_INVALID_PROGRAM = -43,
        CL_INVALID_PROGRAM_EXECUTABLE = -44,
        CL_INVALID_KERNEL_NAME = -45,
        CL_INVALID_KERNEL_DEFINITION = -46,
        CL_INVALID_KERNEL = -47,
        CL_INVALID_ARG_INDEX = -48,
        CL_INVALID_ARG_VALUE = -49,
        CL_INVALID_ARG_SIZE = -50,
        CL_INVALID_KERNEL_ARGS = -51,
        CL_INVALID_WORK_DIMENSION = -52,
        CL_INVALID_WORK_GROUP_SIZE = -53,
        CL_INVALID_WORK_ITEM_SIZE = -54,
        CL_INVALID_GLOBAL_OFFSET = -55,
        CL_INVALID_EVENT_WAIT_LIST = -56,
        CL_INVALID_EVENT = -57,
        CL_INVALID_OPERATION = -58,
        CL_INVALID_GL_OBJECT = -59,
        CL_INVALID_BUFFER_SIZE = -60,
    }

    // OpenCL Version    
    public enum OpenCLVersion : uint
    {
        CL_VERSION_1_0 = 1,
    }


    // cl_bool
    public enum CLBool : uint
    {
        CL_FALSE = 0,
        CL_TRUE = 1
    }


    // cl_platform_info
    public enum CLPlatformInfo : uint
    {
        CL_PLATFORM_PROFILE = 0x0900,
        CL_PLATFORM_VERSION = 0x0901,
    }


    // cl_device_type - bitfield
    public enum CLDeviceType : ulong
    {
        CL_DEVICE_TYPE_DEFAULT = (1 << 0),
        CL_DEVICE_TYPE_CPU = (1 << 1),
        CL_DEVICE_TYPE_GPU = (1 << 2),
        CL_DEVICE_TYPE_ACCELERATOR = (1 << 3),
        CL_DEVICE_TYPE_ALL = 0xFFFFFFFF,
    }


    // cl_device_info
    public enum CLDeviceInfo : uint
    {
        CL_DEVICE_TYPE = 0x1000,
        CL_DEVICE_VENDOR_ID = 0x1001,
        CL_DEVICE_MAX_COMPUTE_UNITS = 0x1002,
        CL_DEVICE_MAX_WORK_ITEM_DIMENSIONS = 0x1003,
        CL_DEVICE_MAX_WORK_GROUP_SIZE = 0x1004,
        CL_DEVICE_MAX_WORK_ITEM_SIZES = 0x1005,
        CL_DEVICE_PREFERRED_VECTOR_WIDTH_CHAR = 0x1006,
        CL_DEVICE_PREFERRED_VECTOR_WIDTH_SHORT = 0x1007,
        CL_DEVICE_PREFERRED_VECTOR_WIDTH_INT = 0x1008,
        CL_DEVICE_PREFERRED_VECTOR_WIDTH_LONG = 0x1009,
        CL_DEVICE_PREFERRED_VECTOR_WIDTH_FLOAT = 0x100A,
        CL_DEVICE_PREFERRED_VECTOR_WIDTH_DOUBLE = 0x100B,
        CL_DEVICE_MAX_CLOCK_FREQUENCY = 0x100C,
        CL_DEVICE_ADDRESS_BITS = 0x100D,
        CL_DEVICE_MAX_READ_IMAGE_ARGS = 0x100E,
        CL_DEVICE_MAX_WRITE_IMAGE_ARGS = 0x100F,
        CL_DEVICE_MAX_MEM_ALLOC_SIZE = 0x1010,
        CL_DEVICE_IMAGE2D_MAX_WIDTH = 0x1011,
        CL_DEVICE_IMAGE2D_MAX_HEIGHT = 0x1012,
        CL_DEVICE_IMAGE3D_MAX_WIDTH = 0x1013,
        CL_DEVICE_IMAGE3D_MAX_HEIGHT = 0x1014,
        CL_DEVICE_IMAGE3D_MAX_DEPTH = 0x1015,
        CL_DEVICE_IMAGE_SUPPORT = 0x1016,
        CL_DEVICE_MAX_PARAMETER_SIZE = 0x1017,
        CL_DEVICE_MAX_SAMPLERS = 0x1018,
        CL_DEVICE_MEM_BASE_ADDR_ALIGN = 0x1019,
        CL_DEVICE_MIN_DATA_TYPE_ALIGN_SIZE = 0x101A,
        CL_DEVICE_SINGLE_FP_CONFIG = 0x101B,
        CL_DEVICE_GLOBAL_MEM_CACHE_TYPE = 0x101C,
        CL_DEVICE_GLOBAL_MEM_CACHELINE_SIZE = 0x101D,
        CL_DEVICE_GLOBAL_MEM_CACHE_SIZE = 0x101E,
        CL_DEVICE_GLOBAL_MEM_SIZE = 0x101F,
        CL_DEVICE_MAX_CONSTANT_BUFFER_SIZE = 0x1020,
        CL_DEVICE_MAX_CONSTANT_ARGS = 0x1021,
        CL_DEVICE_LOCAL_MEM_TYPE = 0x1022,
        CL_DEVICE_LOCAL_MEM_SIZE = 0x1023,
        CL_DEVICE_ERROR_CORRECTION_SUPPORT = 0x1024,
        CL_DEVICE_PROFILING_TIMER_RESOLUTION = 0x1025,
        CL_DEVICE_ENDIAN_LITTLE = 0x1026,
        CL_DEVICE_AVAILABLE = 0x1027,
        CL_DEVICE_COMPILER_AVAILABLE = 0x1028,
        CL_DEVICE_EXECUTION_CAPABILITIES = 0x1029,
        CL_DEVICE_QUEUE_PROPERTIES = 0x102A,
        CL_DEVICE_NAME = 0x102B,
        CL_DEVICE_VENDOR = 0x102C,
        CL_DRIVER_VERSION = 0x102D,
        CL_DEVICE_PROFILE = 0x102E,
        CL_DEVICE_VERSION = 0x102F,
        CL_DEVICE_EXTENSIONS = 0x1030,
    }


    // cl_device_address_info - bitfield
    public enum CLDeviceAddressInfo : ulong
    {
        CL_DEVICE_ADDRESS_32_BITS = (1 << 0),
        CL_DEVICE_ADDRESS_64_BITS = (1 << 1),
    }


    // cl_device_fp_config - bitfield
    public enum CLDeviceFPConfig : ulong
    {
        CL_FP_DENORM = (1 << 0),
        CL_FP_INF_NAN = (1 << 1),
        CL_FP_ROUND_TO_NEAREST = (1 << 2),
        CL_FP_ROUND_TO_ZERO = (1 << 3),
        CL_FP_ROUND_TO_INF = (1 << 4),
        CL_FP_FMA = (1 << 5),
    }


    // cl_device_mem_cache_type
    public enum CLDeviceMemCacheType : uint
    {
        CL_NONE = 0x0,
        CL_READ_ONLY_CACHE = 0x1,
        CL_READ_WRITE_CACHE = 0x2,
    }


    // cl_device_local_mem_type
    public enum CLDeviceLocalMemType : uint
    {
        CL_LOCAL = 0x1,
        CL_GLOBAL = 0x2
    }


    // cl_device_exec_capabilities - bitfield
    public enum CLDeviceExecCapabilities : ulong
    {
        CL_EXEC_KERNEL = (1 << 0),
        CL_EXEC_NATIVE_FN_AS_KERNEL = (1 << 1),
    }


    // cl_command_queue_properties - bitfield
    public enum CLCommandQueueProperties : ulong
    {
        CL_QUEUE_OUT_OF_ORDER_EXEC_MODE_ENABLE = (1 << 0),
        CL_QUEUE_PROFILING_ENABLE = (1 << 1),
    }


    // cl_context_info
    public enum CLContextInfo : uint
    {
        CL_CONTEXT_REFERENCE_COUNT = 0x1080,
        CL_CONTEXT_NUM_DEVICES = 0x1081,
        CL_CONTEXT_DEVICES = 0x1082,
        CL_CONTEXT_PROPERTIES = 0x1083,
    }


    // cl_command_queue_info
    public enum CLCommandQueueInfo : uint
    {
        CL_QUEUE_CONTEXT = 0x1090,
        CL_QUEUE_DEVICE = 0x1091,
        CL_QUEUE_REFERENCE_COUNT = 0x1092,
        CL_QUEUE_PROPERTIES = 0x1093,
    }


    // cl_mem_flags - bitfield
    [Flags]
    public enum CLMemFlags : ulong
    {
        CL_MEM_READ_WRITE = (1 << 0),
        CL_MEM_WRITE_ONLY = (1 << 1),
        CL_MEM_READ_ONLY = (1 << 2),
        CL_MEM_USE_HOST_PTR = (1 << 3),
        CL_MEM_ALLOC_HOST_PTR = (1 << 4),
        CL_MEM_COPY_HOST_PTR = (1 << 5),
    }


    // cl_channel_order
    public enum CLChannelOrder : uint
    {
        CL_R = 0x10B0,
        CL_A = 0x10B1,
        CL_RG = 0x10B2,
        CL_RA = 0x10B3,
        CL_RGB = 0x10B4,
        CL_RGBA = 0x10B5,
        CL_BGRA = 0x10B6,
        CL_ARGB = 0x10B7,
    }


    // cl_channel_type
    public enum CLChannelType : uint
    {
        CL_SNORM_INT8 = 0x10D0,
        CL_SNORM_INT16 = 0x10D1,
        CL_UNORM_INT8 = 0x10D2,
        CL_UNORM_INT16 = 0x10D3,
        CL_UNORM_SHORT_565 = 0x10D4,
        CL_UNORM_SHORT_555 = 0x10D5,
        CL_UNORM_INT_101010 = 0x10D6,
        CL_SIGNED_INT8 = 0x10D7,
        CL_SIGNED_INT16 = 0x10D8,
        CL_SIGNED_INT32 = 0x10D9,
        CL_UNSIGNED_INT8 = 0x10DA,
        CL_UNSIGNED_INT16 = 0x10DB,
        CL_UNSIGNED_INT32 = 0x10DC,
        CL_HALF_FLOAT = 0x10DD,
        CL_FLOAT = 0x10DE,
    }


    // cl_mem_object_type
    public enum CLMemObjectType : uint
    {
        CL_MEM_OBJECT_BUFFER = 0x10F0,
        CL_MEM_OBJECT_IMAGE2D = 0x10F1,
        CL_MEM_OBJECT_IMAGE3D = 0x10F2,
    }


    // cl_mem_info
    public enum CLMemInfo : uint
    {
        CL_MEM_TYPE = 0x1100,
        CL_MEM_FLAGS = 0x1101,
        CL_MEM_SIZE = 0x1102,
        CL_MEM_HOST_PTR = 0x1103,
        CL_MEM_MAP_COUNT = 0x1104,
        CL_MEM_REFERENCE_COUNT = 0x1105,
        CL_MEM_CONTEXT = 0x1106,
    }


    // cl_image_info
    public enum CLImageInfo : uint
    {
        CL_IMAGE_FORMAT = 0x1110,
        CL_IMAGE_ELEMENT_SIZE = 0x1111,
        CL_IMAGE_ROW_PITCH = 0x1112,
        CL_IMAGE_SLICE_PITCH = 0x1113,
        CL_IMAGE_WIDTH = 0x1114,
        CL_IMAGE_HEIGHT = 0x1115,
        CL_IMAGE_DEPTH = 0x1116,
    }

    // cl_addressing_mode
    public enum CLAddressingMode : uint
    {
        CL_ADDRESS_NONE = 0x1130,
        CL_ADDRESS_CLAMP_TO_EDGE = 0x1131,
        CL_ADDRESS_CLAMP = 0x1132,
        CL_ADDRESS_REPEAT = 0x1133,
    }


    // cl_filter_mode
    public enum CLFilterMode : uint
    {
        CL_FILTER_NEAREST = 0x1140,
        CL_FILTER_LINEAR = 0x1141,
    }


    // cl_sampler_info
    public enum CLSamplerInfo : uint
    {
        CL_SAMPLER_REFERENCE_COUNT = 0x1150,
        CL_SAMPLER_CONTEXT = 0x1151,
        CL_SAMPLER_NORMALIZED_COORDS = 0x1152,
        CL_SAMPLER_ADDRESSING_MODE = 0x1153,
        CL_SAMPLER_FILTER_MODE = 0x1154,
    }


    // cl_map_flags - bitfield
    public enum CLMapFlags : ulong
    {
        CL_MAP_READ = (1 << 0),
        CL_MAP_WRITE = (1 << 1),
    }


    // cl_program_info
    public enum CLProgramInfo : uint
    {
        CL_PROGRAM_REFERENCE_COUNT = 0x1160,
        CL_PROGRAM_CONTEXT = 0x1161,
        CL_PROGRAM_NUM_DEVICES = 0x1162,
        CL_PROGRAM_DEVICES = 0x1163,
        CL_PROGRAM_SOURCE = 0x1164,
        CL_PROGRAM_BINARY_SIZES = 0x1165,
        CL_PROGRAM_BINARIES = 0x1166,
    }


    // cl_program_build_info
    public enum CLProgramBuildInfo : uint
    {
        CL_PROGRAM_BUILD_STATUS = 0x1181,
        CL_PROGRAM_BUILD_OPTIONS = 0x1182,
        CL_PROGRAM_BUILD_LOG = 0x1183,
    }


    // cl_build_status
    public enum CLBuildStatus : int
    {
        CL_BUILD_SUCCESS = 0,
        CL_BUILD_NONE = -1,
        CL_BUILD_ERROR = -2,
        CL_BUILD_IN_PROGRESS = -3,
    }

    // cl_kernel_info
    public enum CLKernelInfo : uint
    {
        CL_KERNEL_FUNCTION_NAME = 0x1190,
        CL_KERNEL_NUM_ARGS = 0x1191,
        CL_KERNEL_REFERENCE_COUNT = 0x1192,
        CL_KERNEL_CONTEXT = 0x1193,
        CL_KERNEL_PROGRAM = 0x1194,
    }


    // cl_kernel_work_group_info
    public enum CLKernelWorkGroupInfo : uint
    {
        CL_KERNEL_WORK_GROUP_SIZE = 0x11B0,
        CL_KERNEL_COMPILE_WORK_GROUP_SIZE = 0x11B1,
    }


    // cl_event_info
    public enum CLEventInfo : uint
    {
        CL_EVENT_COMMAND_QUEUE = 0x11D0,
        CL_EVENT_COMMAND_TYPE = 0x11D1,
        CL_EVENT_REFERENCE_COUNT = 0x11D2,
        CL_EVENT_COMMAND_EXECUTION_STATUS = 0x11D3,
    }

    // cl_command_type
    public enum CLCommandType : uint
    {
        CL_COMMAND_NDRANGE_KERNEL = 0x11F0,
        CL_COMMAND_TASK = 0x11F1,
        CL_COMMAND_NATIVE_KERNEL = 0x11F2,
        CL_COMMAND_READ_BUFFER = 0x11F3,
        CL_COMMAND_WRITE_BUFFER = 0x11F4,
        CL_COMMAND_COPY_BUFFER = 0x11F5,
        CL_COMMAND_READ_IMAGE = 0x11F6,
        CL_COMMAND_WRITE_IMAGE = 0x11F7,
        CL_COMMAND_COPY_IMAGE = 0x11F8,
        CL_COMMAND_COPY_IMAGE_TO_BUFFER = 0x11F9,
        CL_COMMAND_COPY_BUFFER_TO_IMAGE = 0x11FA,
        CL_COMMAND_MAP_BUFFER = 0x11FB,
        CL_COMMAND_MAP_IMAGE = 0x11FC,
        CL_COMMAND_UNMAP_MEM_OBJECT = 0x11FD,
        CL_COMMAND_MARKER = 0x11FE,
        CL_COMMAND_WAIT_FOR_EVENTS = 0x11FF,
        CL_COMMAND_BARRIER = 0x1200,
        CL_COMMAND_ACQUIRE_GL_OBJECTS = 0x1201,
        CL_COMMAND_RELEASE_GL_OBJECTS = 0x1202,
    }

    // command execution status
    public enum CLExecutionStatus : uint
    {
        CL_COMPLETE = 0x0,
        CL_RUNNING = 0x1,
        CL_SUBMITTED = 0x2,
        CL_QUEUED = 0x3,
    }

    // cl_profiling_info
    public enum CLProfilingInfo : uint
    {
        CL_PROFILING_COMMAND_QUEUED = 0x1280,
        CL_PROFILING_COMMAND_SUBMIT = 0x1281,
        CL_PROFILING_COMMAND_START = 0x1282,
        CL_PROFILING_COMMAND_END = 0x1283,
    }
    #endregion
}
