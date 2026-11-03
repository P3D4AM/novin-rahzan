using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Microsoft.Win32.SafeHandles;
using Riss.Devices;

internal class o : ZdCommBase
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	protected struct c
	{
		public int a;

		public Guid b;

		public int c;

		public int d;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct a
	{
		public int a;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
		public string b;
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
	protected class d
	{
		public int a;

		public int b;

		public int c;

		public Guid d;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
		public string e;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	private struct b
	{
		public long a;

		public long b;

		public long c;
	}

	protected const int a = 2;

	protected const int b = 16;

	protected const int c = 5;

	protected const int d = 0;

	protected const uint e = 2147483648u;

	protected const uint f = 1073741824u;

	protected const uint g = 2u;

	protected const uint h = 1u;

	protected const uint i = 1073741824u;

	protected const uint j = 3u;

	protected const uint k = 4u;

	protected const uint l = 997u;

	protected const uint m = uint.MaxValue;

	private const int n = 0;

	private const int o = 0;

	private const long p = 5L;

	private const long q = 6L;

	private const long r = 31L;

	private const long s = 258L;

	private const long t = 259L;

	private const long u = 1460L;

	private const ulong v = 20227uL;

	private static IntPtr w = (IntPtr)(-1);

	private readonly uint x = 315412u;

	private Guid y = new Guid("{0x298223d6, 0x3077, 0x40b8, { 0x8c, 0xb5, 0xf6, 0x43, 0x45, 0x97, 0xd6, 0x17 }}");

	private bool z = true;

	private char[] aa = new char[260];

	private IntPtr ab;

	private IntPtr ac = IntPtr.Zero;

	private static IntPtr ad = IntPtr.Zero;

	[DllImport("setupapi.dll", SetLastError = true)]
	protected static extern IntPtr SetupDiGetClassDevs(ref Guid A_0, [MarshalAs(UnmanagedType.LPStr)] string A_1, IntPtr A_2, uint A_3);

	[DllImport("setupapi.dll", SetLastError = true)]
	protected static extern int SetupDiDestroyDeviceInfoList(IntPtr A_0);

	[DllImport("setupapi.dll", SetLastError = true)]
	protected static extern bool SetupDiEnumDeviceInterfaces(IntPtr A_0, uint A_1, ref Guid A_2, uint A_3, ref c A_4);

	[DllImport("setupapi.dll", SetLastError = true)]
	protected static extern bool SetupDiGetDeviceInterfaceDetail(IntPtr A_0, ref c A_1, IntPtr A_2, uint A_3, ref uint A_4, IntPtr A_5);

	[DllImport("setupapi.dll", SetLastError = true)]
	protected static extern bool SetupDiGetDeviceInterfaceDetail(IntPtr A_0, ref c A_1, ref a A_2, uint A_3, ref uint A_4, IntPtr A_5);

	[DllImport("user32.dll", SetLastError = true)]
	protected static extern IntPtr RegisterDeviceNotification(IntPtr A_0, d A_1, uint A_2);

	[DllImport("user32.dll", SetLastError = true)]
	protected static extern bool UnregisterDeviceNotification(IntPtr A_0);

	[DllImport("kernel32.dll", SetLastError = true)]
	protected static extern IntPtr CreateFile([MarshalAs(UnmanagedType.LPStr)] string A_0, uint A_1, uint A_2, IntPtr A_3, uint A_4, uint A_5, IntPtr A_6);

	[DllImport("kernel32.dll", SetLastError = true)]
	public static extern bool ReadFile(IntPtr A_0, byte[] A_1, uint A_2, ref uint A_3, ref NativeOverlapped A_4);

	[DllImport("kernel32.dll", SetLastError = true)]
	public static extern bool ReadFile(uint A_0, byte[] A_1, uint A_2, ref uint A_3, ref NativeOverlapped A_4);

	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern bool WriteFile(IntPtr A_0, byte[] A_1, uint A_2, ref uint A_3, ref NativeOverlapped A_4);

	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern bool WriteFile(IntPtr A_0, byte[] A_1, uint A_2, ref uint A_3, IntPtr A_4);

	[DllImport("kernel32.dll", SetLastError = true)]
	protected static extern int CloseHandle(IntPtr A_0);

	[DllImport("kernel32.dll", SetLastError = true)]
	protected static extern bool CancelIo(IntPtr A_0);

	[DllImport("kernel32.dll")]
	protected static extern int DeviceIoControl(int A_0, int A_1, ref int A_2, int A_3, ref int A_4, int A_5, ref int A_6, ref NativeOverlapped A_7);

	[DllImport("kernel32.dll")]
	protected static extern int DeviceIoControl(int A_0, int A_1, ref int A_2, int A_3, ref int A_4, int A_5, ref int A_6, IntPtr A_7);

	[DllImport("kernel32.dll", SetLastError = true)]
	protected static extern uint WaitForSingleObject(IntPtr A_0, uint A_1);

	[DllImport("kernel32.dll", SetLastError = true)]
	protected static extern bool GetOverlappedResult(IntPtr A_0, [In] ref NativeOverlapped A_1, out uint A_2, bool A_3);

	[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
	protected static extern bool PeekNamedPipe(SafeFileHandle A_0, IntPtr A_1, uint A_2, ref uint A_3, ref uint A_4, ref uint A_5);

	[DllImport("kernel32.dll")]
	protected static extern IntPtr CreateEvent(IntPtr A_0, bool A_1, bool A_2, string A_3);

	[DllImport("Data\\SBPCCOMM.DLL", CharSet = CharSet.Unicode, SetLastError = true)]
	private static extern int UsbOpen(int A_0);

	[DllImport("Data\\SBPCCOMM.DLL", CharSet = CharSet.Unicode, SetLastError = true)]
	private static extern void UsbClose();

	[DllImport("Data\\SBPCCOMM.DLL", CharSet = CharSet.Ansi, SetLastError = true)]
	private static extern int UsbReadData(StringBuilder A_0, int A_1, int A_2);

	[DllImport("Data\\SBPCCOMM.DLL", CharSet = CharSet.Unicode, SetLastError = true)]
	private static extern int UsbSendData(string A_0, int A_1, int A_2);

	public static IntPtr a(IntPtr A_0, Guid A_1)
	{
		d d2 = new d();
		d2.a = Marshal.SizeOf((object)d2);
		d2.d = A_1;
		d2.b = 5;
		d2.c = 0;
		return RegisterDeviceNotification(A_0, d2, 0u);
	}

	public static bool b(IntPtr A_0)
	{
		return UnregisterDeviceNotification(A_0);
	}

	private bool a(IntPtr A_0)
	{
		int A_ = 0;
		int A_2 = 0;
		NativeOverlapped A_3 = c();
		bool flag = DeviceIoControl((int)A_0, (int)x, ref A_2, 0, ref A_2, 0, ref A_, ref A_3) != 0;
		if (!flag)
		{
			ulong num = (ulong)Marshal.GetLastWin32Error();
			if (num == 997)
			{
				switch (WaitForSingleObject(A_3.EventHandle, 200u))
				{
				case 0u:
				{
					uint A_4 = 0u;
					flag = GetOverlappedResult(A_0, ref A_3, out A_4, A_3: false);
					break;
				}
				case 258u:
					CancelIo(A_0);
					break;
				}
			}
		}
		CloseHandle(A_3.EventHandle);
		return flag;
	}

	private IntPtr a(IntPtr A_0, ref c A_1)
	{
		ab = w;
		uint A_2 = 0u;
		if (!SetupDiGetDeviceInterfaceDetail(A_0, ref A_1, IntPtr.Zero, 0u, ref A_2, IntPtr.Zero))
		{
			a A_3 = default(a);
			A_3.a = 5;
			if (!SetupDiGetDeviceInterfaceDetail(A_0, ref A_1, ref A_3, A_2, ref A_2, IntPtr.Zero))
			{
				return ab;
			}
			aa = A_3.b.ToCharArray();
			ab = CreateFile(A_3.b, 3221225472u, 3u, IntPtr.Zero, 3u, 0u, IntPtr.Zero);
		}
		return ab;
	}

	private IntPtr a(byte A_0)
	{
		string arg = A_0.ToString().PadLeft(2, '0');
		string a_ = string.Format("{0}\\PIPE{1}", Encoding.Default.GetString(Encoding.Default.GetBytes(aa)).Replace("\0", "").Trim(), arg);
		return CreateFile(a_, 3221225472u, 3u, IntPtr.Zero, 3u, 1073741824u, IntPtr.Zero);
	}

	private NativeOverlapped c()
	{
		NativeOverlapped result = default(NativeOverlapped);
		result.InternalLow = IntPtr.Zero;
		result.InternalHigh = IntPtr.Zero;
		result.OffsetLow = 0;
		result.OffsetHigh = 0;
		result.EventHandle = CreateEvent(IntPtr.Zero, A_1: true, A_2: false, null);
		return result;
	}

	private void b()
	{
		if (ab != w)
		{
			CancelIo(ab);
			CloseHandle(ab);
			Marshal.FreeHGlobal(ab);
		}
		if (ac != w)
		{
			CancelIo(ac);
			CloseHandle(ac);
			Marshal.FreeHGlobal(ac);
		}
		if (ad != w)
		{
			CancelIo(ad);
			CloseHandle(ad);
			Marshal.FreeHGlobal(ad);
		}
		aa[0] = '\0';
		ab = w;
		ac = w;
		ad = w;
	}

	public override void Close()
	{
		if (z)
		{
			UsbClose();
		}
		else
		{
			b();
		}
	}

	private bool a()
	{
		aa = new char[260];
		IntPtr intPtr = SetupDiGetClassDevs(ref y, null, IntPtr.Zero, 18u);
		if (intPtr == w)
		{
			return false;
		}
		try
		{
			c A_ = default(c);
			A_.a = Marshal.SizeOf((object)A_);
			int a_ = 0;
			while (true)
			{
				Close();
				if (SetupDiEnumDeviceInterfaces(intPtr, 0u, ref y, (uint)a_, ref A_))
				{
					ab = a(intPtr, ref A_);
					if (ab != w && !((ac = a(1)) == w) && !((ad = a(0)) == w))
					{
						break;
					}
				}
				else if ((long)Marshal.GetLastWin32Error() == 259)
				{
					break;
				}
			}
		}
		catch
		{
			Close();
		}
		finally
		{
			SetupDiDestroyDeviceInfoList(intPtr);
		}
		return ab != w;
	}

	public override bool Open()
	{
		if (!z)
		{
			return a();
		}
		return UsbOpen(base.Device.DN) == 1;
	}

	private int d(ref byte[] A_0, int A_1, int A_2)
	{
		if (ab == w)
		{
			return 0;
		}
		int num = 0;
		byte[] sourceArray = A_0;
		int num2 = 0;
		int num3 = A_1;
		while (num3 > 0)
		{
			NativeOverlapped A_3 = c();
			int num4 = ((num3 > 61440) ? 61440 : num3);
			byte[] array = new byte[num4];
			Array.Copy(sourceArray, num2, array, 0, num4);
			uint A_4 = 0u;
			long num5 = 0L;
			if (!WriteFile(ac, array, (uint)num4, ref A_4, ref A_3))
			{
				num5 = Marshal.GetLastWin32Error();
				if (num5 == 997)
				{
					num5 = WaitForSingleObject(A_3.EventHandle, (uint)A_2);
					if (num5 == 0)
					{
						num5 = ((!GetOverlappedResult(ac, ref A_3, out A_4, A_3: false)) ? Marshal.GetLastWin32Error() : 0);
					}
					else
					{
						num5 = ((num5 == 258) ? 1460 : Marshal.GetLastWin32Error());
						if (num5 == 258)
						{
							CancelIo(ac);
						}
					}
				}
			}
			CloseHandle(A_3.EventHandle);
			if (num5 != 0)
			{
				if (num5 == 5 || num5 == 31)
				{
					a(ab);
					Close();
				}
				else
				{
					a(ab);
				}
				return -1;
			}
			num2 += (int)A_4;
			num3 -= (int)A_4;
			num += (int)A_4;
		}
		return num;
	}

	private int c(ref byte[] A_0, int A_1, int A_2)
	{
		string text = BitConverter.ToString(A_0, 0);
		text = text.Replace("-", "");
		return UsbSendData(text, A_1, A_2);
	}

	private int b(ref byte[] A_0, int A_1, int A_2)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Capacity = A_1;
		int num = UsbReadData(stringBuilder, A_1, A_2);
		if (num == A_1)
		{
			stringBuilder.ToString();
			string text = string.Empty;
			for (int num2 = 0; num2 < 2 * A_1; num2++)
			{
				text += stringBuilder[num2];
				if (num2 % 2 != 0)
				{
					A_0[num2 / 2] = Convert.ToByte(text, 16);
					text = string.Empty;
				}
			}
		}
		return num;
	}

	public override int SendData(ref byte[] buffer, int size, int timeout)
	{
		if (!z)
		{
			return d(ref buffer, size, timeout);
		}
		return c(ref buffer, size, timeout);
	}

	private int a(ref byte[] A_0, int A_1, int A_2)
	{
		if (ab == w)
		{
			return -1;
		}
		int num = 0;
		byte[] array = A_0;
		int num2 = A_1;
		int num3 = num2;
		while (num2 > 0)
		{
			NativeOverlapped A_3 = c();
			int num4 = ((num2 > 61440) ? 61440 : num2);
			byte[] array2 = new byte[num4];
			long num5 = 0L;
			uint A_4 = 0u;
			DateTime now = DateTime.Now;
			if (!ReadFile((uint)(int)ad, array2, (uint)num4, ref A_4, ref A_3))
			{
				num5 = Marshal.GetLastWin32Error();
				if (num5 == 997)
				{
					switch (WaitForSingleObject(A_3.EventHandle, 1000u))
					{
					case 0u:
						num5 = ((!GetOverlappedResult(ad, ref A_3, out A_4, A_3: false)) ? Marshal.GetLastWin32Error() : 0);
						break;
					case 258u:
						CancelIo(ad);
						num5 = 1460L;
						break;
					default:
						num5 = Marshal.GetLastWin32Error();
						break;
					}
				}
				else
				{
					num5 = Marshal.GetLastWin32Error();
				}
			}
			CloseHandle(A_3.EventHandle);
			if (num5 != 0)
			{
				if (num5 == 5 || num5 == 31)
				{
					Close();
				}
				else
				{
					a(ab);
				}
				return -1;
			}
			double totalMilliseconds = (DateTime.Now - now).TotalMilliseconds;
			totalMilliseconds += 1.0;
			if (array2.Length != A_1)
			{
				for (int num6 = 0; num6 < A_4; num6++)
				{
					array[num + num6] = array2[num6];
				}
			}
			else
			{
				array = array2;
			}
			num2 -= (int)A_4;
			num += (int)A_4;
			base.CommunicationPercent = (double)num / (double)num3 * 100.0;
		}
		if (num == A_1)
		{
			A_0 = array;
		}
		return num;
	}

	public override int RecvData(ref byte[] buffer, int size, int timeout)
	{
		if (!z)
		{
			return a(ref buffer, size, timeout);
		}
		return b(ref buffer, size, timeout);
	}
}
