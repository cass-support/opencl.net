using System;
using System.Collections.Generic;
using System.Text;

namespace CASS.OpenCL.D3D10
{
    #region Enums
    public enum D3D10DeviceSource : uint
    {
        Device = 0x4010,
        DXGIAdapter = 0x4011,
    }

    public enum D3D10DeviceSet : uint
    {
        PreferredDevices = 0x4012,
        AllDevices = 0x4013,
    }
    #endregion
}
