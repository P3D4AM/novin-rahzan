using System;
using System.Runtime.InteropServices;

namespace NP756P.ReadOnlyProbe
{
    /// <summary>
    /// P/Invoke declarations for the vendor Hysoon-classic SDK entry DLL.
    ///
    /// Signatures are copied EXACTLY from the decompiled vendor source:
    ///   NovinLibraryResearch/decompiled-complete/TimeAttendanceDevices/
    ///   TimeAttendanceDevices.FLRHysoon/FKAttendDLL2.cs
    /// and the usage patterns from:
    ///   NovinLibraryResearch/decompiled-complete/FK623AttendDllCSSample/
    ///   FKAttendDllCSSample/FrmConnectDeviceHysoon.cs and FrmCheckDeviceHysoon.cs
    ///
    /// ONLY the six functions allowed for this read-only probe are declared here.
    /// Mutating functions (FK_SetDeviceTime, FK_SetDeviceInfo, FK_EnableDevice,
    /// FK_Empty*, FK_DeleteEnrollData, FK_PutEnrollData, ...) are deliberately
    /// NOT declared. See README "Functions intentionally not invoked".
    ///
    /// FK_DisConnect returns void in the vendor source (not an int status).
    /// FK_GetLastError is only invoked through FkDeviceChannel with a valid handle.
    /// </summary>
    internal static class NativeMethods
    {
        /// <summary>
        /// Network connect. Machine no, IP, TCP port, timeout (ms), protocol
        /// (0=TCP, 1=UDP), numeric network password, SDK license.
        /// Returns a handle; values greater than zero mean success
        /// (vendor sample FrmConnectDeviceHysoon: "if (nCommHandleIndex > 0)").
        /// </summary>
        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_ConnectNet(int anMachineNo, string astrIpAddress, int anNetPort, int anTimeOut, int anProtocolType, int anNetPassword, int anLicense);

        /// <summary>Closes the connection for the given handle. Returns void.</summary>
        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern void FK_DisConnect(int anHandleIndex);

        /// <summary>Last error code for the handle. Only called with a valid handle.</summary>
        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_GetLastError(int anHandleIndex);

        /// <summary>
        /// Reads product data. dataIndex 1 = serial number, 3 = product code
        /// (vendor sample FrmCheckDeviceHysoon.GetProuctData).
        /// apstrValue must be pre-filled with spaces (256) and is filled by the SDK;
        /// the returned string is trimmed by the caller.
        /// </summary>
        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_GetProductData(int anHandleIndex, int anDataIndex, [MarshalAs(UnmanagedType.LPStr)] ref string apstrValue);

        /// <summary>
        /// Reads device info. infoIndex 2 = machine number (DI_MACHINENUM),
        /// 3 = language (DI_LANGAUGE, 0 = Farsi in the vendor sample).
        /// </summary>
        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_GetDeviceInfo(int anHandleIndex, int anInfoIndex, ref int apnValue);

        /// <summary>Reads the device clock. Read-only; never writes the clock.</summary>
        [DllImport("FK623Attend.dll", CharSet = CharSet.Ansi)]
        public static extern int FK_GetDeviceTime(int anHandleIndex, ref DateTime apnDateTime);
    }
}
