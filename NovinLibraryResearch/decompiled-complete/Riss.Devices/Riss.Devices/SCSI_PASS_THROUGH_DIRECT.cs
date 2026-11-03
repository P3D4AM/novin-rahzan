using System;
using System.Runtime.InteropServices;

namespace Riss.Devices;

[StructLayout(LayoutKind.Sequential)]
public class SCSI_PASS_THROUGH_DIRECT
{
	public ushort Length = (ushort)Marshal.SizeOf(typeof(SCSI_PASS_THROUGH_DIRECT));

	public byte ScsiStatus;

	public byte PathId;

	public byte TargetId = 1;

	public byte Lun;

	public byte CdbLength = 10;

	public byte SenseInfoLength;

	public byte DataIn;

	public uint DataTransferLength;

	public uint TimeOutValue = 500u;

	public IntPtr DataBuffer;

	public uint SenseInfoOffset = (uint)(int)Marshal.OffsetOf(typeof(SCSI_PASS_THROUGH_DIRECT_WITH_BUFFER), "ucSenseBuf");

	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
	public byte[] Cdb = new byte[16];

	public SCSI_PASS_THROUGH_DIRECT(uint dataTransferLength)
	{
		DataTransferLength = dataTransferLength;
	}
}
