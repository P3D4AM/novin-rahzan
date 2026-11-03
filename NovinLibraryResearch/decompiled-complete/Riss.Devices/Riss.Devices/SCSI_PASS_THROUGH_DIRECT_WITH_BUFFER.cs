using System.Runtime.InteropServices;

namespace Riss.Devices;

[StructLayout(LayoutKind.Sequential)]
public class SCSI_PASS_THROUGH_DIRECT_WITH_BUFFER
{
	public SCSI_PASS_THROUGH_DIRECT sptd;

	public uint Filler;

	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 65536)]
	public byte[] ucSenseBuf;

	public SCSI_PASS_THROUGH_DIRECT_WITH_BUFFER(uint dataTransferLength)
	{
		sptd = new SCSI_PASS_THROUGH_DIRECT(dataTransferLength);
		Filler = 0u;
		ucSenseBuf = new byte[65536];
	}
}
