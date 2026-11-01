using System;
using System.Runtime.InteropServices;
using System.Text;

namespace TimeAttendanceDevices.FLRHysoon;

internal struct ExtCmdStructHeader
{
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 56)]
	public byte[] bytCmdCode;

	public int StructVer;

	public int StructSize;

	public void Init(string asCmdCode, int aStructVer, int aStructSize)
	{
		bytCmdCode = new byte[56];
		Array.Clear(bytCmdCode, 0, bytCmdCode.Length);
		byte[] bytes = Encoding.GetEncoding("utf-8").GetBytes(asCmdCode);
		int num = bytes.Length;
		if (num > bytCmdCode.Length - 1)
		{
			num = bytCmdCode.Length - 1;
		}
		Array.Copy(bytes, bytCmdCode, num);
		StructVer = aStructVer;
		StructSize = aStructSize;
	}
}
