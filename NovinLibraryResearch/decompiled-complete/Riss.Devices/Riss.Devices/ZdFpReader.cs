using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Riss.Devices;

public class ZdFpReader
{
	internal struct d
	{
		public byte a;

		public byte b;

		public byte c;

		public byte d;
	}

	internal struct c
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		public byte[] a;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public byte[] b;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		public byte[] c;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		public byte[] d;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public byte[] e;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public byte[] f;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public byte[] g;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public byte[] h;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		public byte[] i;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		public byte[] j;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public byte[] k;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public byte[] l;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public byte[] m;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public byte[] n;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public byte[] o;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public byte[] p;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
		public d[] q;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 65536)]
		public byte[] r;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	private struct a
	{
		public long a;

		public long b;

		public long c;
	}

	private enum b
	{
		a = 1,
		b = 2,
		c = 5,
		d = 6,
		e = 7,
		f = 11,
		g = 12,
		h = 13,
		i = 14,
		j = 22,
		k = 31,
		l = 33,
		m = 34,
		n = 35,
		o = 43,
		p = 44,
		q = 45,
		r = 46,
		s = 51,
		t = 52,
		u = 56,
		v = 60,
		w = 61,
		x = 62,
		y = 63,
		z = 64,
		aa = 69,
		ab = 100,
		ac = 1000
	}

	private const int m_a = 1404;

	private const uint m_b = 999999999u;

	private const byte m_c = byte.MaxValue;

	private int m_d = 1;

	private bool e;

	private List<FPINFO> f = new List<FPINFO>();

	private byte[] g = new byte[65536];

	private int[] h = new int[256];

	private string i = string.Empty;

	private IntPtr j = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(FPINFO)));

	private IntPtr k = Marshal.AllocHGlobal(65536);

	private IntPtr l = Marshal.AllocHGlobal(4);

	private FileStream m;

	private IntPtr n = IntPtr.Zero;

	public List<FPINFO> FpInfo => f;

	[DllImport("Data\\SFE.dll", CharSet = CharSet.Auto, SetLastError = true)]
	private static extern int fp(b A_0, int A_1, int A_2, int A_3);

	private static T BytesToStruct<T>(byte[] bytes, int startIndex, int length)
	{
		if (bytes == null)
		{
			return default(T);
		}
		if (bytes.Length <= 0)
		{
			return default(T);
		}
		IntPtr intPtr = Marshal.AllocHGlobal(length);
		try
		{
			Marshal.Copy(bytes, startIndex, intPtr, length);
			return (T)Marshal.PtrToStructure(intPtr, typeof(T));
		}
		catch (Exception ex)
		{
			throw new Exception("Error in BytesToStruct ! " + Environment.NewLine + ex.Message);
		}
		finally
		{
			Marshal.FreeHGlobal(intPtr);
		}
	}

	private static byte[] a(object A_0, int A_1)
	{
		IntPtr intPtr = Marshal.AllocHGlobal(A_1);
		try
		{
			Marshal.StructureToPtr(A_0, intPtr, fDeleteOld: false);
			byte[] array = new byte[A_1];
			Marshal.Copy(intPtr, array, 0, A_1);
			return array;
		}
		catch (Exception ex)
		{
			throw new Exception("Error in StructToBytes ! " + Environment.NewLine + ex.Message);
		}
		finally
		{
			Marshal.FreeHGlobal(intPtr);
		}
	}

	public int Open(string strDBFileName, int nSensorType)
	{
		IntPtr intPtr = Marshal.AllocHGlobal(4);
		IntPtr intPtr2 = Marshal.AllocHGlobal(4);
		fp(ZdFpReader.b.ab, (int)intPtr, (int)intPtr2, 0);
		Marshal.ReadInt32(intPtr);
		int num = Marshal.ReadInt32(intPtr2);
		Marshal.FreeHGlobal(intPtr);
		Marshal.FreeHGlobal(intPtr2);
		this.m_d = num & 0x7FFFFFFF;
		e = (num & 0x80000000u) != 0;
		int num2 = fp(ZdFpReader.b.a, nSensorType, 0, 0);
		if (num2 != 0)
		{
			return num2;
		}
		n = (IntPtr)fp(ZdFpReader.b.ac, 0, 0, 0);
		this.i = strDBFileName;
		if (m != null && File.Exists(strDBFileName))
		{
			m.Close();
		}
		try
		{
			m = File.Open(strDBFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
			if (m.Length == 0)
			{
				byte[] array = new byte[this.m_d * 1404];
				m.Write(array, 0, array.Length);
				m.Flush();
			}
		}
		catch
		{
			return -101;
		}
		int num3 = (int)m.Length;
		if (num3 > this.m_d * 1404)
		{
			num3 = this.m_d * 1404;
		}
		int num4 = num3 / 1404;
		if (num4 > 0)
		{
			byte[] array2 = new byte[num3];
			m.Read(array2, 0, num3);
			Marshal.Copy(array2, 0, n, num3);
			f.Clear();
			for (int i = 0; i < num4; i++)
			{
				FPINFO fPINFO = default(FPINFO);
				fPINFO = BytesToStruct<FPINFO>(array2, i * 1404, 1404);
				f.Add(fPINFO);
			}
			m.SetLength(num3);
			m.Close();
		}
		return 0;
	}

	public int Close()
	{
		int result = 0;
		try
		{
			result = fp(ZdFpReader.b.b, 0, 0, 0);
			if (j != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(j);
			}
			if (k != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(k);
			}
			if (n != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(n);
			}
		}
		catch
		{
		}
		return result;
	}

	private int a(uint A_0, byte A_1)
	{
		int i;
		for (i = 0; i < this.m_d && (f[i].Valid <= 0 || f[i].ID != A_0 || f[i].FingerNum != A_1); i++)
		{
		}
		return i;
	}

	private int a(uint A_0)
	{
		int result = 0;
		h = new int[256];
		for (int i = 0; i < this.m_d; i++)
		{
			if (f[i].Valid > 0 && f[i].ID == A_0)
			{
				h[result++] = i;
			}
		}
		return result;
	}

	private void a(int A_0)
	{
		m = File.Open(i, FileMode.Open, FileAccess.Write);
		m.Seek(A_0 * 1404, SeekOrigin.Begin);
		int num = (int)m.Length;
		byte[] array = new byte[num];
		byte[] array2 = new byte[1404];
		Marshal.Copy(n, array, 0, num);
		Array.Copy(array, A_0 * 1404, array2, 0, 1404);
		m.Write(array2, 0, 1404);
		m.Flush();
		m.Close();
	}

	public void Save(int nPos, byte[] data)
	{
		m = File.Open(this.i, FileMode.Open, FileAccess.ReadWrite);
		m.Seek(nPos * 1404, SeekOrigin.Begin);
		m.Write(data, 0, data.Length);
		m.Flush();
		m.Seek(0L, SeekOrigin.Begin);
		int num = (int)m.Length;
		if (num > this.m_d * 1404)
		{
			num = this.m_d * 1404;
		}
		int num2 = num / 1404;
		byte[] array = new byte[num];
		m.Read(array, 0, num);
		Marshal.Copy(array, 0, n, num);
		f.Clear();
		for (int i = 0; i < num2; i++)
		{
			FPINFO fPINFO = default(FPINFO);
			fPINFO = BytesToStruct<FPINFO>(array, i * 1404, 1404);
			f.Add(fPINFO);
		}
		m.SetLength(num);
		m.Close();
	}

	public int Delete(uint nID, byte nFingerNum)
	{
		int num = a(nID, nFingerNum);
		if (num == this.m_d)
		{
			return -3;
		}
		fp(ZdFpReader.b.s, num, 0, 0);
		byte[] data = new byte[1404];
		Save(num, data);
		return 0;
	}

	public int DeleteID(uint nID)
	{
		int num = a(nID);
		if (num == 0)
		{
			return -3;
		}
		for (int i = 0; i < num; i++)
		{
			fp(ZdFpReader.b.s, h[i], 0, 0);
			byte[] data = new byte[1404];
			Save(h[i], data);
		}
		return 0;
	}

	public int DeleteAll()
	{
		fp(ZdFpReader.b.t, 0, 0, 0);
		m = File.Open(i, FileMode.Open, FileAccess.Write);
		m.SetLength(0L);
		m.Close();
		byte[] source = new byte[this.m_d * 1404];
		Marshal.Copy(source, 0, n, this.m_d * 1404);
		return 0;
	}

	public int AdjustSensor()
	{
		int num = fp(ZdFpReader.b.v, 0, 0, 0);
		if (num < 0)
		{
			return num;
		}
		return 0;
	}

	public int Capture()
	{
		int num = fp(ZdFpReader.b.w, 0, 0, 0);
		if (num >= 0)
		{
			return 0;
		}
		return num;
	}

	public int IsFinger()
	{
		int num = fp(ZdFpReader.b.x, 0, 0, 0);
		if (num < 0)
		{
			return num;
		}
		return num;
	}

	public Bitmap GetImage()
	{
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Expected O, but got Unknown
		int num = fp(ZdFpReader.b.y, (int)k, 0, 0);
		if (num < 0)
		{
			return null;
		}
		Marshal.Copy(k, g, 0, g.Length);
		c c = default(c);
		c.a = BitConverter.GetBytes(19778);
		c.b = BitConverter.GetBytes((uint)Marshal.SizeOf(typeof(c)));
		c.c = new byte[2];
		c.d = new byte[2];
		c.e = BitConverter.GetBytes((uint)(Marshal.SizeOf(typeof(c)) - 65536));
		c.q = new d[256];
		c.f = BitConverter.GetBytes(40);
		c.g = BitConverter.GetBytes(256);
		c.h = BitConverter.GetBytes(256);
		c.i = BitConverter.GetBytes((ushort)1);
		c.j = BitConverter.GetBytes((ushort)8);
		c.k = new byte[4];
		c.l = BitConverter.GetBytes(65536);
		c.m = new byte[4];
		c.n = new byte[4];
		c.o = new byte[4];
		c.p = new byte[4];
		for (int i = 0; i < 256; i++)
		{
			c.q[i].a = (byte)i;
			c.q[i].b = (byte)i;
			c.q[i].c = (byte)i;
			c.q[i].d = 0;
		}
		c.r = g;
		byte[] buffer = a(c, Marshal.SizeOf(typeof(c)));
		MemoryStream memoryStream = new MemoryStream(buffer);
		return new Bitmap((Stream)memoryStream);
	}

	public int ImageEnroll(uint id, byte bn, byte manager, byte[] imageByte)
	{
		c c = BytesToStruct<c>(imageByte, 0, imageByte.Length);
		if (CheckFingerNum(id, bn) == 0)
		{
			return -3;
		}
		EnrollStart();
		for (int i = 1; i < 4; i++)
		{
			int num = a(c.r);
			if (num < 0)
			{
				return num;
			}
			num = EnrollNth(i);
			if (num < 0)
			{
				return num;
			}
		}
		return EnrollEnd(id, bn, manager);
	}

	private int a(byte[] A_0)
	{
		g = A_0;
		Marshal.Copy(g, 0, k, 65536);
		if (e)
		{
			int num = fp(ZdFpReader.b.aa, (int)k, 0, 0);
			if (num < 0)
			{
				return num;
			}
			int num2 = fp(ZdFpReader.b.x, 0, 0, 0);
			if (num2 < 0)
			{
				return num2;
			}
		}
		else
		{
			int num3 = fp(ZdFpReader.b.i, (int)k, 0, 0);
			if (num3 < 0)
			{
				return num3;
			}
		}
		return 0;
	}

	public int EnrollStart()
	{
		return fp(ZdFpReader.b.k, 0, 0, 0);
	}

	public int EnrollNth(int nNth)
	{
		if (e)
		{
			int num = fp(ZdFpReader.b.z, (int)j, 0, 0);
			if (num < 0)
			{
				return num;
			}
			num = fp(ZdFpReader.b.m, (int)j, nNth, 0);
			if (num < 0)
			{
				return num;
			}
			if (num > 0)
			{
				return -102;
			}
		}
		else
		{
			int num2 = fp(ZdFpReader.b.l, (int)k, nNth, 0);
			if (num2 < 0)
			{
				return num2;
			}
			if (num2 > 0)
			{
				return -102;
			}
		}
		return 0;
	}

	private int a()
	{
		int i;
		for (i = 0; i < this.m_d && f[i].Valid != 0; i++)
		{
		}
		return i;
	}

	public int EnrollEnd(uint id, byte fingerNum, byte manager)
	{
		int num = fp(ZdFpReader.b.n, (int)j, 0, 0);
		if (num < 0)
		{
			return num;
		}
		int num2 = a(id, fingerNum);
		if (num2 != this.m_d)
		{
			return -3;
		}
		num2 = a();
		if (num2 == this.m_d)
		{
			return -4;
		}
		byte[] data = a(id, fingerNum, manager);
		Save(num2, data);
		return 0;
	}

	private byte[] a(uint A_0, byte A_1, byte A_2)
	{
		FPINFO fPINFO = default(FPINFO);
		byte[] array = new byte[1404];
		Marshal.Copy(j, array, 0, 1404);
		fPINFO.ID = A_0;
		fPINFO.Valid = 1;
		fPINFO.FingerNum = A_1;
		fPINFO.Manager = A_2;
		fPINFO.Reserved = new byte[1397];
		Array.Copy(array, 7, fPINFO.Reserved, 0, 1397);
		return a(fPINFO, 1404);
	}

	public int Identify(ref uint ID, ref byte fingerNum)
	{
		int num;
		if (e)
		{
			num = fp(ZdFpReader.b.z, (int)j, 0, 0);
			if (num < 0)
			{
				return num;
			}
			num = fp(ZdFpReader.b.o, (int)j, (int)l, 0);
			if (num < 0)
			{
				return num;
			}
			if (num == 0)
			{
				return -103;
			}
		}
		else
		{
			num = fp(ZdFpReader.b.q, (int)k, (int)j, (int)l);
			if (num < 0)
			{
				return num;
			}
			if (num == 0)
			{
				return -103;
			}
		}
		ID = f[num - 1].ID;
		fingerNum = f[num - 1].FingerNum;
		byte[] data = a(ID, fingerNum, f[num - 1].Manager);
		Save(num - 1, data);
		return 0;
	}

	public int GetEnrollCount()
	{
		int num = 0;
		foreach (FPINFO item in FpInfo)
		{
			if (item.ID != 0)
			{
				num++;
			}
		}
		return num;
	}

	public int Verify(uint nID, byte nFingerNum)
	{
		int num = a(nID, nFingerNum);
		if (num == this.m_d)
		{
			return -3;
		}
		if (e)
		{
			int num2 = fp(ZdFpReader.b.z, (int)j, 0, 0);
			if (num2 < 0)
			{
				return num2;
			}
			num2 = fp(ZdFpReader.b.p, (int)j, num, 0);
			if (num2 < 0)
			{
				return num2;
			}
			if (num2 == 0)
			{
				return -104;
			}
		}
		else
		{
			int num2 = fp(ZdFpReader.b.r, (int)k, num, (int)j);
			if (num2 < 0)
			{
				return num2;
			}
			if (num2 == 0)
			{
				return -104;
			}
		}
		a(num);
		return 0;
	}

	public int VerifyID(uint nID, ref byte pnFingerNum)
	{
		pnFingerNum = 0;
		int num;
		if (e)
		{
			num = fp(ZdFpReader.b.z, (int)j, 0, 0);
			if (num < 0)
			{
				return num;
			}
			num = fp(ZdFpReader.b.o, (int)j, (int)l, 0);
			if (num < 0)
			{
				return num;
			}
			if (num == 0 || f[num - 1].ID != nID)
			{
				return -104;
			}
		}
		else
		{
			num = fp(ZdFpReader.b.q, (int)k, (int)j, (int)l);
			if (num < 0)
			{
				return num;
			}
			if (num == 0 || f[num - 1].ID != nID)
			{
				return -104;
			}
		}
		pnFingerNum = f[num - 1].FingerNum;
		a(num - 1);
		return 0;
	}

	public uint SearchUnusedID()
	{
		int i;
		for (i = 0; (long)i < 999999999L; i++)
		{
			bool flag = false;
			foreach (FPINFO item in f)
			{
				if (item.ID == i)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				break;
			}
		}
		return (uint)i;
	}

	public int SearchUnusedFN(uint id)
	{
		foreach (FPINFO item in f)
		{
			if (item.ID != id)
			{
				continue;
			}
			for (int i = 0; i < 255; i++)
			{
				bool flag = false;
				foreach (FPINFO item2 in f)
				{
					_ = item2;
					if (item.FingerNum == i)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return i;
				}
			}
		}
		return -1;
	}

	public int CheckId(uint id)
	{
		foreach (FPINFO item in f)
		{
			if (item.ID == id)
			{
				return 0;
			}
		}
		return -1;
	}

	public int CheckFingerNum(uint id, byte fingerNum)
	{
		foreach (FPINFO item in f)
		{
			if (item.ID == id && item.FingerNum == fingerNum)
			{
				return 0;
			}
		}
		return -1;
	}

	public int CheckManager(uint id, byte fingerNum)
	{
		foreach (FPINFO item in f)
		{
			if (item.ID == id && item.FingerNum == fingerNum && item.Manager == 1)
			{
				return 0;
			}
		}
		return -1;
	}

	public int CheckManagerID(uint id)
	{
		for (int i = 0; i < 255; i++)
		{
			if (CheckManager(id, (byte)i) == 0)
			{
				return 0;
			}
		}
		return -1;
	}

	[DllImport("kernel32.dll")]
	private static extern DriveType GetDriveType([MarshalAs(UnmanagedType.LPStr)] string A_0);

	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern bool GetDiskFreeSpaceEx(string A_0, ref long A_1, ref long A_2, ref long A_3);

	[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
	private static extern bool GetVolumeInformation(string A_0, StringBuilder A_1, uint A_2, out uint A_3, out uint A_4, out uint A_5, StringBuilder A_6, uint A_7);

	private bool a(string A_0)
	{
		a a = default(a);
		if (!GetDiskFreeSpaceEx(A_0, ref a.a, ref a.b, ref a.c))
		{
			throw new Win32Exception(Marshal.GetLastWin32Error(), "Error retrieving free disk space");
		}
		if (a.b != 53248)
		{
			return false;
		}
		return Directory.GetFiles(A_0)[0].ToString().Contains("DEVICEINFO.txt");
	}

	public bool DeviceConnected()
	{
		StringBuilder stringBuilder = new StringBuilder(256);
		StringBuilder stringBuilder2 = new StringBuilder(256);
		for (char c = 'D'; c <= 'Z'; c = (char)(c + 1))
		{
			string a_ = c + ":\\";
			DriveType driveType = GetDriveType(a_);
			if (driveType == DriveType.Removable || driveType == DriveType.CDRom)
			{
				uint A_ = 0u;
				if (GetVolumeInformation(a_, stringBuilder, (uint)(stringBuilder.Capacity - 1), out var _, out var _, out A_, stringBuilder2, (uint)(stringBuilder2.Capacity - 1)) && a(a_))
				{
					return true;
				}
			}
		}
		return false;
	}
}
