using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Riss.Devices;

internal class ab : ZdCommBase
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	private struct a
	{
		public long a;

		public long b;

		public long c;
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

	private const byte j = 10;

	private const byte k = 0;

	private const byte l = 1;

	private readonly IntPtr m = (IntPtr)(-1);

	private string n = "\\\\.\\C:";

	public IntPtr o;

	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern IntPtr CreateFile([MarshalAs(UnmanagedType.LPStr)] string A_0, uint A_1, uint A_2, IntPtr A_3, uint A_4, uint A_5, IntPtr A_6);

	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern bool GetDiskFreeSpaceEx(string A_0, ref long A_1, ref long A_2, ref long A_3);

	[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
	private static extern bool GetVolumeInformation(string A_0, StringBuilder A_1, uint A_2, out uint A_3, out uint A_4, out uint A_5, StringBuilder A_6, uint A_7);

	[DllImport("Kernel32.dll", SetLastError = true)]
	private static extern bool CloseHandle(IntPtr A_0);

	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern bool DeviceIoControl(IntPtr A_0, int A_1, IntPtr A_2, int A_3, IntPtr A_4, int A_5, ref int A_6, IntPtr A_7);

	[DllImport("kernel32.dll")]
	private static extern DriveType GetDriveType([MarshalAs(UnmanagedType.LPStr)] string A_0);

	[SpecialName]
	public string a()
	{
		return n;
	}

	[SpecialName]
	public void b(string A_0)
	{
		n = n.Remove(n.Length - 2) + A_0 + ":";
	}

	private bool a(string A_0)
	{
		a a2 = default(a);
		if (!GetDiskFreeSpaceEx(A_0, ref a2.a, ref a2.b, ref a2.c))
		{
			throw new Win32Exception(Marshal.GetLastWin32Error(), "Error retrieving free disk space");
		}
		if (a2.b != 53248)
		{
			return false;
		}
		return Directory.GetFiles(A_0)[0].ToString().Contains("DEVICEINFO.txt");
	}

	public override void Close()
	{
		if (o != m)
		{
			CloseHandle(o);
		}
		o = m;
	}

	public override bool Open()
	{
		StringBuilder stringBuilder = new StringBuilder(256);
		StringBuilder stringBuilder2 = new StringBuilder(256);
		for (char c2 = 'D'; c2 <= 'Z'; c2 = (char)(c2 + 1))
		{
			string text = c2 + ":\\";
			DriveType driveType = GetDriveType(text);
			if (driveType == DriveType.Removable || driveType == DriveType.CDRom)
			{
				uint A_ = 0u;
				if (GetVolumeInformation(text, stringBuilder, (uint)(stringBuilder.Capacity - 1), out var _, out var _, out A_, stringBuilder2, (uint)(stringBuilder2.Capacity - 1)) && a(text))
				{
					b(text.Substring(0, 1));
					o = CreateFile(a(), 3221225472u, 3u, IntPtr.Zero, 3u, 0u, IntPtr.Zero);
					if (!(o == m))
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	private int b(ref byte[] A_0, int A_1, int A_2, bool A_3)
	{
		bool flag = false;
		SCSI_PASS_THROUGH_DIRECT_WITH_BUFFER sCSI_PASS_THROUGH_DIRECT_WITH_BUFFER = new SCSI_PASS_THROUGH_DIRECT_WITH_BUFFER((uint)A_1);
		GCHandle gCHandle = GCHandle.Alloc(A_0, GCHandleType.Pinned);
		sCSI_PASS_THROUGH_DIRECT_WITH_BUFFER.sptd.DataBuffer = gCHandle.AddrOfPinnedObject();
		sCSI_PASS_THROUGH_DIRECT_WITH_BUFFER.sptd.DataTransferLength = (uint)A_1;
		sCSI_PASS_THROUGH_DIRECT_WITH_BUFFER.sptd.Cdb[0] = 239;
		sCSI_PASS_THROUGH_DIRECT_WITH_BUFFER.sptd.Cdb[1] = (byte)(A_3 ? byte.MaxValue : 254);
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
			flag = DeviceIoControl(o, 315412, intPtr, num, intPtr, num, ref A_4, IntPtr.Zero);
			if (!flag && Marshal.GetLastWin32Error() != 121)
			{
				break;
			}
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
			if (b(ref A_4, num3, A_2, A_3) != num3)
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
			return A_1;
		}
		return 0;
	}

	public override int SendData(ref byte[] buffer, int size, int timeout)
	{
		return a(ref buffer, size, timeout, A_3: false);
	}

	public override int RecvData(ref byte[] buffer, int size, int timeout)
	{
		return a(ref buffer, size, timeout, A_3: true);
	}
}
