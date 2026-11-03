using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Riss.Devices;

internal class v : ZdCommBase
{
	public struct a
	{
		public int a;

		public Guid b;

		public uint c;

		public UIntPtr d;
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	public struct b
	{
		public int a;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
		public ushort[] b;
	}

	protected const uint a = 2147483648u;

	protected const uint b = 1073741824u;

	protected const uint c = 2u;

	protected const uint d = 1u;

	protected const uint e = 3u;

	private const int f = 65536;

	private const int g = 0;

	private const int h = 315396;

	private const int i = 315412;

	protected const uint j = 4u;

	protected const uint k = 2u;

	protected const uint l = 16u;

	private const ulong m = 259uL;

	private const byte n = 0;

	private const byte o = 1;

	private readonly IntPtr p = (IntPtr)(-1);

	private static Guid q = new Guid("{53F56307-B6BF-11D0-94F2-00A0C91EFB8B}");

	private string r = "\\\\.\\C:";

	public IntPtr s;

	[SpecialName]
	public string c()
	{
		return r;
	}

	[SpecialName]
	public void a(string A_0)
	{
		r = r.Remove(r.Length - 2) + A_0 + ":";
	}

	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern IntPtr CreateFile([MarshalAs(UnmanagedType.LPStr)] string A_0, uint A_1, uint A_2, IntPtr A_3, uint A_4, uint A_5, IntPtr A_6);

	[DllImport("Kernel32.dll", SetLastError = true)]
	private static extern bool CloseHandle(IntPtr A_0);

	[DllImport("kernel32.dll")]
	private static extern DriveType GetDriveType([MarshalAs(UnmanagedType.LPStr)] string A_0);

	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern bool DeviceIoControl(IntPtr A_0, int A_1, IntPtr A_2, int A_3, IntPtr A_4, int A_5, ref int A_6, IntPtr A_7);

	[DllImport("setupapi.dll", SetLastError = true)]
	private static extern IntPtr SetupDiGetClassDevs(ref Guid A_0, uint A_1, IntPtr A_2, uint A_3);

	[DllImport("setupapi.dll", SetLastError = true)]
	private static extern int SetupDiDestroyDeviceInfoList(IntPtr A_0);

	[DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
	private static extern bool SetupDiEnumDeviceInterfaces(IntPtr A_0, IntPtr A_1, ref Guid A_2, uint A_3, ref a A_4);

	[DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
	private static extern bool SetupDiGetDeviceInterfaceDetail(IntPtr A_0, ref a A_1, IntPtr A_2, uint A_3, out uint A_4, IntPtr A_5);

	public override void Close()
	{
		if (p != s)
		{
			CloseHandle(s);
		}
		s = p;
	}

	public override bool Open()
	{
		bool flag = b();
		if (!flag)
		{
			for (char c2 = 'D'; c2 < 'Z'; c2 = (char)(c2 + 1))
			{
				string text = c2 + ":\\";
				DriveType driveType = GetDriveType(text);
				if (DriveType.Removable == driveType || DriveType.CDRom == driveType)
				{
					a(text.Substring(0, 1));
					s = CreateFile(c(), 3221225472u, 3u, IntPtr.Zero, 3u, 0u, IntPtr.Zero);
					if (!(p == s) && a())
					{
						flag = true;
						break;
					}
				}
			}
		}
		return flag;
	}

	private bool b()
	{
		bool result = false;
		Guid A_ = q;
		ulong num = 4uL;
		ulong num2 = 0uL;
		IntPtr intPtr = SetupDiGetClassDevs(ref A_, 0u, IntPtr.Zero, 18u);
		if (intPtr != p)
		{
			bool flag = false;
			while (!flag)
			{
				num *= 2;
				a A_2 = default(a);
				A_2.a = Marshal.SizeOf((object)A_2);
				A_2.c = 0u;
				A_2.b = Guid.Empty;
				A_2.d = UIntPtr.Zero;
				for (; num2 < num; num2++)
				{
					if (SetupDiEnumDeviceInterfaces(intPtr, IntPtr.Zero, ref A_, (uint)num2, ref A_2))
					{
						uint A_3 = 0u;
						SetupDiGetDeviceInterfaceDetail(intPtr, ref A_2, IntPtr.Zero, 0u, out A_3, IntPtr.Zero);
						uint num3 = A_3;
						IntPtr intPtr2 = Marshal.AllocHGlobal((int)num3);
						b b2 = default(b);
						if (IntPtr.Size == 8)
						{
							b2.a = 8;
						}
						else
						{
							b2.a = 4 + Marshal.SystemDefaultCharSize;
						}
						Marshal.StructureToPtr((object)b2, intPtr2, fDeleteOld: false);
						bool flag2 = SetupDiGetDeviceInterfaceDetail(intPtr, ref A_2, intPtr2, num3, out A_3, IntPtr.Zero);
						IntPtr ptr = (IntPtr)((int)intPtr2 + 4);
						string a_ = Marshal.PtrToStringAuto(ptr);
						Marshal.FreeHGlobal(intPtr2);
						if (flag2)
						{
							s = CreateFile(a_, 3221225472u, 3u, IntPtr.Zero, 3u, 0u, IntPtr.Zero);
							if (!(p == s) && a())
							{
								result = true;
								flag = true;
								break;
							}
						}
					}
					else
					{
						int lastWin32Error = Marshal.GetLastWin32Error();
						if ((long)lastWin32Error == 259)
						{
							flag = true;
							break;
						}
					}
				}
			}
		}
		num = num2;
		SetupDiDestroyDeviceInfoList(intPtr);
		return result;
	}

	public override int SendData(ref byte[] buffer, int size, int timeout)
	{
		return a(ref buffer, size, timeout, A_3: false);
	}

	public override int RecvData(ref byte[] buffer, int size, int timeout)
	{
		return a(ref buffer, size, timeout, A_3: true);
	}

	private int b(ref byte[] A_0, int A_1, int A_2, bool A_3)
	{
		bool flag = false;
		SCSI_PASS_THROUGH_DIRECT_WITH_BUFFER sCSI_PASS_THROUGH_DIRECT_WITH_BUFFER = new SCSI_PASS_THROUGH_DIRECT_WITH_BUFFER((uint)A_1);
		byte[] array = new byte[A_1 - base.CmdLen];
		Array.Copy(A_0, base.CmdLen, array, 0, array.Length);
		GCHandle gCHandle = GCHandle.Alloc(array, GCHandleType.Pinned);
		sCSI_PASS_THROUGH_DIRECT_WITH_BUFFER.sptd.DataBuffer = gCHandle.AddrOfPinnedObject();
		sCSI_PASS_THROUGH_DIRECT_WITH_BUFFER.sptd.CdbLength = 6;
		sCSI_PASS_THROUGH_DIRECT_WITH_BUFFER.sptd.DataTransferLength = (uint)(A_3 ? array.Length : (A_1 - base.CmdLen));
		Array.Copy(A_0, 0, sCSI_PASS_THROUGH_DIRECT_WITH_BUFFER.sptd.Cdb, 0, base.CmdLen);
		sCSI_PASS_THROUGH_DIRECT_WITH_BUFFER.sptd.DataIn = (byte)(A_3 ? 1 : 0);
		sCSI_PASS_THROUGH_DIRECT_WITH_BUFFER.sptd.TimeOutValue = (uint)A_2;
		int A_4 = 0;
		int num = Marshal.SizeOf(typeof(SCSI_PASS_THROUGH_DIRECT_WITH_BUFFER));
		IntPtr intPtr = Marshal.AllocHGlobal(num);
		Marshal.StructureToPtr((object)sCSI_PASS_THROUGH_DIRECT_WITH_BUFFER, intPtr, fDeleteOld: false);
		int num2 = 3;
		while (!flag && num2 > 0)
		{
			num2--;
			flag = DeviceIoControl(s, 315412, intPtr, num, intPtr, num, ref A_4, IntPtr.Zero);
			if (!flag && 121 != Marshal.GetLastWin32Error())
			{
				break;
			}
		}
		if (flag && array.Length > 0)
		{
			Array.Copy(array, 0, A_0, base.CmdLen, array.Length);
		}
		Marshal.FreeHGlobal(intPtr);
		if (gCHandle.IsAllocated)
		{
			gCHandle.Free();
		}
		if (!flag)
		{
			return 0;
		}
		return A_1;
	}

	private int a(ref byte[] A_0, int A_1, int A_2, bool A_3)
	{
		byte[] sourceArray = A_0;
		int num = A_1;
		int num2 = 0;
		while (num > 0)
		{
			int num3 = num;
			if (num3 > 65536)
			{
				num3 = 65536;
			}
			byte[] A_4 = new byte[num3];
			Array.Copy(sourceArray, num2, A_4, 0, num3);
			if (b(ref A_4, num3, A_2, A_3) != num3 || A_4.Length == 0)
			{
				break;
			}
			Array.Copy(A_4, 0, A_0, num2, num3);
			num2 += num3;
			num -= num3;
			base.CommunicationPercent = (double)(A_1 - num) / (double)A_1 * 100.0;
		}
		if (num == 0)
		{
			if (A_0.Length - base.CmdLen > 0)
			{
				byte[] array = new byte[A_0.Length - base.CmdLen];
				Array.Copy(A_0, base.CmdLen, array, 0, array.Length);
				A_0 = array;
			}
			return A_1;
		}
		return 0;
	}

	private bool a()
	{
		byte[] buffer = new byte[6];
		base.CmdLen = 6;
		buffer[0] = 239;
		buffer[1] = 1;
		buffer[2] = 4;
		if (SendData(ref buffer, buffer.Length) == 0)
		{
			return false;
		}
		buffer[0] = 239;
		buffer[1] = 2;
		byte[] buffer2 = new byte[buffer.Length + 4];
		Array.Copy(buffer, 0, buffer2, 0, buffer.Length);
		if (RecvData(ref buffer2, buffer2.Length) == 0)
		{
			return false;
		}
		if (BitConverter.ToUInt16(buffer2, 2) == 0)
		{
			return false;
		}
		buffer[1] = 3;
		buffer2 = new byte[buffer.Length + 32];
		Array.Copy(buffer, 0, buffer2, 0, buffer.Length);
		if (RecvData(ref buffer2, buffer2.Length) == 0)
		{
			return false;
		}
		if (!Encoding.ASCII.GetString(buffer2, 16, 16).Contains("Riantsoft"))
		{
			return false;
		}
		if (base.Device.DN != BitConverter.ToUInt16(buffer2, 0))
		{
			return false;
		}
		return true;
	}
}
