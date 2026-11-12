using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Riss.Devices;

public abstract class DeviceConnection
{
	internal struct a
	{
		public ushort a;

		public int b;

		public int c;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 26)]
		public ushort[] d;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 384)]
		public byte[] e;
	}

	internal struct c
	{
		public uint a;

		public int b;

		public int c;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 26)]
		public byte[] d;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 384)]
		public byte[] e;
	}

	internal struct e
	{
		public uint a;

		public ushort b;

		public byte c;

		public ushort d;

		public byte e;

		public ushort f;
	}

	internal struct f
	{
		public uint a;

		public ushort b;

		public byte c;

		public byte d;

		public ushort e;

		public ushort f;
	}

	internal struct g
	{
		public uint a;

		public ushort b;

		public byte c;

		public byte d;

		public byte e;
	}

	internal enum d
	{
		a = 0,
		b = 0,
		c = 1,
		d = 2,
		e = 3,
		f = 4,
		g = 5,
		h = 6,
		i = 7,
		j = 8,
		k = 9,
		l = 10,
		m = 11,
		n = 12,
		o = 13,
		p = 14,
		q = 15,
		r = 16,
		s = 17,
		t = 18,
		u = 19,
		v = 20,
		w = 21,
		x = 22,
		y = 23,
		z = 24,
		aa = 25,
		ab = 26,
		ac = 27,
		ad = 27
	}

	internal enum b
	{
		a,
		b,
		c,
		d
	}

	internal const int a = 1416;

	internal const int b = 32;

	internal const int c = 28;

	internal const int d = 4;

	internal const uint e = 4096u;

	internal const uint f = 4194304u;

	internal const uint g = 3852464152u;

	internal const uint h = 1380271940u;

	internal const uint i = 1397509959u;

	internal const uint j = 1196183367u;

	internal const int k = 1;

	internal const int l = -1;

	private const int m = 3584;

	private const int n = 3839;

	internal const int o = 16;

	internal const int p = 8;

	internal const int q = 24;

	internal const int r = 384;

	private const int s = 127;

	internal const int t = 104;

	internal const int u = 27;

	internal const int v = 8;

	private Device w;

	private j x;

	internal bool y;

	internal bool z;

	internal bool aa;

	internal bool ab;

	internal bool ac;

	private int[] ad = new int[12]
	{
		31, 28, 31, 30, 31, 30, 31, 31, 30, 31,
		30, 31
	};

	internal Device Device
	{
		get
		{
			return w;
		}
		set
		{
			w = value;
		}
	}

	internal virtual j DeviceComm
	{
		get
		{
			if (x == null)
			{
				x = new aa();
			}
			return x;
		}
		set
		{
			x = value;
		}
	}

	public DeviceConnection()
	{
		w = new Device();
	}

	public abstract int Open();

	public abstract void Close();

	public static DeviceConnection CreateConnection(ref Device device)
	{
		DeviceConnection deviceConnection = null;
		switch ((ac)device.ConnectionModel)
		{
		case global::ac.a:
		case global::ac.b:
		case global::ac.c:
			deviceConnection = new global::c();
			deviceConnection.Device = device;
			break;
		case global::ac.d:
			deviceConnection = new global::d();
			deviceConnection.Device = device;
			break;
		case global::ac.e:
			deviceConnection = new r();
			deviceConnection.Device = device;
			break;
		case global::ac.f:
			deviceConnection = new m();
			deviceConnection.Device = device;
			break;
		}
		return deviceConnection;
	}

	public abstract bool SetProperty(UserProperty property, object extraProperty, User user, object extraData);

	public abstract bool GetProperty(UserProperty property, object extraProperty, ref User user, ref object extraData);

	public abstract bool SetProperty(DeviceProperty property, object extraProperty, Device device, object extraData);

	public abstract bool GetProperty(DeviceProperty property, object extraProperty, ref Device device, ref object extraData);

	internal bool a(byte[] A_0, ref DateTime A_1)
	{
		ulong num = (ulong)((A_0[3] << 24) | (A_0[2] << 16) | (A_0[1] << 8) | A_0[0]);
		ulong num2 = num % 86400;
		int hour = (int)num2 / 3600;
		num2 %= 3600;
		int minute = (int)num2 / 60;
		int second = (int)num2 % 60;
		int num3 = (int)num / 3600 / 24 + 1;
		int num4 = 0;
		int num5 = 1;
		while (true)
		{
			int num6 = 365;
			if (num4 % 4 == 0)
			{
				num6++;
			}
			if (num3 <= num6)
			{
				break;
			}
			num3 -= num6;
			num4++;
		}
		num4 += 2000;
		while (true)
		{
			int num6 = ad[num5 - 1];
			if (num5 == 2 && num4 % 4 == 0)
			{
				num6++;
			}
			if (num3 <= num6)
			{
				break;
			}
			num3 -= num6;
			num5++;
		}
		bool result = true;
		try
		{
			A_1 = new DateTime(num4, num5, num3, hour, minute, second);
		}
		catch
		{
			result = false;
		}
		return result;
	}

	internal byte[] a(DateTime A_0)
	{
		DateTime dateTime = new DateTime(2000, 1, 1, 0, 0, 1);
		int num = (int)(A_0 - dateTime).TotalSeconds + 1;
		return new byte[4]
		{
			(byte)((uint)num & 0xFFu),
			(byte)((uint)(num >> 8) & 0xFFu),
			(byte)((uint)(num >> 16) & 0xFFu),
			(byte)(num >> 24)
		};
	}

	protected abstract bool EnableDevice(long bEnable);

	private unsafe Bitmap a(Bitmap A_0, int A_1)
	{
		if (A_0 == null)
		{
			return null;
		}
		int width = ((Image)A_0).Width;
		int height = ((Image)A_0).Height;
		try
		{
			byte b = 0;
			BitmapData val = A_0.LockBits(new Rectangle(0, 0, width, height), (ImageLockMode)3, (PixelFormat)137224);
			byte* ptr = (byte*)val.Scan0.ToPointer();
			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width; j++)
				{
					ptr[2] = (ptr[1] = (*ptr = ((A_1 != 0) ? ((byte)((float)(*ptr + ptr[1] + ptr[2]) / 3f)) : ((byte)((float)(int)(*ptr) * 0.114f + (float)(int)ptr[1] * 0.587f + (float)(int)ptr[2] * 0.299f)))));
					ptr += 3;
				}
				ptr += val.Stride - width * 3;
			}
			A_0.UnlockBits(val);
			return A_0;
		}
		catch
		{
			return null;
		}
	}

	protected bool ContainUnicodeChar(string charString)
	{
		bool result = false;
		for (int i = 0; i < charString.Length; i++)
		{
			if (Convert.ToUInt32(charString[i]) > 127)
			{
				result = true;
				break;
			}
		}
		return result;
	}

	private int a(string A_0)
	{
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		for (int i = 0; i < A_0.Length; i++)
		{
			if (!flag && Convert.ToUInt32(A_0[i]) > 127)
			{
				flag = true;
			}
			if (!flag2 && Convert.ToUInt32(A_0[i]) <= 127)
			{
				if (!flag)
				{
					flag3 = true;
				}
				flag2 = true;
			}
			if (flag2 && flag)
			{
				break;
			}
		}
		if (flag2 && flag)
		{
			int num = ((!flag3) ? 1 : 0);
			int num2 = 0;
			for (int j = 0; j < A_0.Length; j++)
			{
				if (flag3)
				{
					if (Convert.ToUInt32(A_0[j]) > 127)
					{
						break;
					}
				}
				else if (Convert.ToUInt32(A_0[j]) < 127)
				{
					break;
				}
				num2++;
			}
			int num3 = 0;
			num3 = ((!z) ? ((num << 3) | num2) : ((num << 7) | num2));
			if (num3 == 1 || num3 == 2)
			{
				num3 <<= 2;
			}
			return num3;
		}
		if (!flag2)
		{
			return 2;
		}
		return 1;
	}

	private byte[] a(string A_0, int A_1)
	{
		byte[] array = null;
		switch (A_1)
		{
		case 1:
			array = Encoding.Default.GetBytes(A_0);
			break;
		case 2:
			array = Encoding.Unicode.GetBytes(A_0);
			break;
		default:
		{
			List<byte> list = new List<byte>();
			for (int i = 0; i < A_0.Length; i++)
			{
				if (Convert.ToUInt32(A_0[i]) > 127)
				{
					byte[] bytes = BitConverter.GetBytes(Convert.ToUInt32(A_0[i]));
					list.Add(bytes[0]);
					list.Add(bytes[1]);
				}
				else
				{
					list.Add(BitConverter.GetBytes(Convert.ToUInt32(A_0[i]))[0]);
				}
			}
			array = list.ToArray();
			for (int j = 0; j < array.Length; j++)
			{
				if (array[j] == 0)
				{
					array[j] = byte.MaxValue;
				}
			}
			break;
		}
		}
		return array;
	}

	private Font a(string A_0, float A_1)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Expected O, but got Unknown
		bool flag = ContainUnicodeChar(A_0);
		uint num = Convert.ToUInt32(A_0[0]);
		FontFamily val = FontFamily.GenericSansSerif;
		InstalledFontCollection val2 = new InstalledFontCollection();
		if ((19968 <= num && num <= 40895) || (55296 <= num && num <= 56319) || (12448 <= num && num <= 12591))
		{
			if (z)
			{
				A_1 += 2f;
			}
		}
		else if (num < 127)
		{
			if (!flag)
			{
				A_1 = ((!z) ? ((A_0.Length > 6) ? A_1 : (A_1 + 2f)) : (A_1 + 4f));
				FontFamily[] families = ((FontCollection)val2).Families;
				foreach (FontFamily val3 in families)
				{
					if (val3.Name == "Calibri")
					{
						val = val3;
						break;
					}
				}
			}
		}
		else if (3584 <= num && num <= 3839)
		{
			A_1 = ((!z) ? ((A_0.Length > 8) ? (A_1 + 2f) : (A_1 + 4f)) : (A_1 + 4f));
			FontFamily[] families2 = ((FontCollection)val2).Families;
			foreach (FontFamily val4 in families2)
			{
				if (val4.Name == "Cordia New")
				{
					val = val4;
					break;
				}
			}
		}
		return new Font(val, A_1, (FontStyle)0);
	}

	private int a(Color A_0)
	{
		return A_0.R * 19595 + A_0.G * 38469 + A_0.B * 7472 >> 16;
	}

	private byte[] a(string A_0, Font A_1, int A_2, int A_3)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Expected O, but got Unknown
		Bitmap val = new Bitmap(A_2, A_3);
		Graphics val2 = Graphics.FromImage((Image)(object)val);
		Brush black = Brushes.Black;
		MemoryStream memoryStream = new MemoryStream();
		val2 = Graphics.FromImage((Image)(object)val);
		val2.FillRectangle(Brushes.White, 0, 0, A_2, A_3);
		val2.DrawString(A_0, A_1, black, -2f, 0f);
		((Image)val).Save((Stream)memoryStream, ImageFormat.Bmp);
		Bitmap val3 = a(val, 0);
		byte[] array = new byte[A_2 * 2];
		int num = 0;
		for (int i = 0; i < A_3 / 8; i++)
		{
			for (int j = 0; j < A_2; j++)
			{
				byte b = 0;
				for (int k = 0; k < 8; k++)
				{
					int num2 = ((val3.GetPixel(j, i * 8 + 7 - k).R <= 200) ? 1 : 0);
					b = (byte)((b << 1) + num2);
				}
				array[num] = b;
				num++;
			}
		}
		return array;
	}

	private int a(string A_0, ref byte[] A_1, float A_2)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Expected O, but got Unknown
		int num = A_1.Length / 2;
		Bitmap val = new Bitmap(num, 16);
		Graphics val2 = Graphics.FromImage((Image)(object)val);
		new MemoryStream();
		Font val3 = a(A_0, A_2);
		int num2 = (int)val2.MeasureString(A_0, val3).Width;
		StringBuilder stringBuilder = new StringBuilder();
		if (z && num2 > 104)
		{
			int length = A_0.Length;
			string text = string.Empty;
			for (int i = 0; i < length; i++)
			{
				stringBuilder.Append(A_0[i]);
				if (val2.MeasureString(stringBuilder.ToString(), val3).Width > 104f)
				{
					text = A_0.Substring(0, i);
					break;
				}
			}
			int num3 = 104;
			int num4 = num - 104;
			byte[] sourceArray = a(text, val3, num3, 16);
			string a_ = A_0.Substring(text.Length, length - text.Length);
			byte[] sourceArray2 = a(a_, val3, num4, 16);
			Array.Copy(sourceArray, 0, A_1, 0, num3);
			Array.Copy(sourceArray2, 0, A_1, num3, num4);
			Array.Copy(sourceArray, num3, A_1, num3 + num4, num3);
			Array.Copy(sourceArray2, num4, A_1, 2 * num3 + num4, num4);
		}
		else
		{
			A_1 = a(A_0, val3, num, 16);
		}
		return num2;
	}

	protected string GetDecodeName(byte[] validData, int charType)
	{
		string empty = string.Empty;
		switch (charType)
		{
		case 2:
			return Encoding.Unicode.GetString(validData);
		case 1:
			return Encoding.Default.GetString(validData);
		default:
		{
			int num = 0;
			int num2 = 0;
			bool flag = false;
			if (z || aa)
			{
				if (charType > 150 || charType >> 7 == 0)
				{
					if (charType > 150)
					{
						charType = 255 - charType;
					}
					num = charType & 0x7F;
					flag = true;
				}
				else
				{
					num = (charType & 0x7F) * 2;
				}
			}
			else if (charType > 11 || charType >> 3 == 0)
			{
				if (charType > 11)
				{
					charType = 15 - charType;
				}
				num = charType & 7;
				flag = true;
			}
			else
			{
				num = (charType & 7) * 2;
			}
			num2 = validData.Length - num;
			byte[] array = new byte[num];
			byte[] array2 = new byte[num2];
			Array.Copy(validData, array, num);
			Array.Copy(validData, num, array2, 0, num2);
			if (flag)
			{
				return Encoding.Default.GetString(array) + Encoding.Unicode.GetString(array2);
			}
			return Encoding.Unicode.GetString(array) + Encoding.Default.GetString(array2);
		}
		}
	}

	internal virtual int EncodeName(string name, ref byte[] nameInfo, ref byte[] nameData, int lenght)
	{
		int result = 0;
		int num = (y ? 2 : a(name));
		byte[] array = a(name, num);
		int num2 = ((array.Length > lenght) ? lenght : array.Length);
		if (!y)
		{
			for (int i = 0; i < lenght; i++)
			{
				nameInfo[i] = (byte)((i < num2) ? array[i] : 32);
			}
			float a_ = 8f;
			result = a(name, ref nameData, a_);
			if (z)
			{
				if (num2 > 24)
				{
					num2 = 24;
				}
				if (num >> 7 == 0 && (num >> 2 == 1 || num >> 2 == 2))
				{
					num = 255 - (num >> 2);
				}
				nameInfo[lenght - 1] = (byte)num2;
				nameInfo[lenght - 2] = (byte)num;
			}
			else if (aa)
			{
				byte[] bytes = Encoding.Unicode.GetBytes(name);
				for (int j = 0; j < nameInfo.Length - 2; j++)
				{
					if (j < bytes.Length)
					{
						nameInfo[j] = bytes[j];
					}
				}
			}
			else
			{
				if (num >> 3 == 0 && (num >> 2 == 1 || num >> 2 == 2))
				{
					num = 15 - (num >> 2);
				}
				nameInfo[lenght + 1] = (byte)((num << 4) | num2);
			}
		}
		else
		{
			for (int k = 0; k < lenght; k++)
			{
				nameData[k] = (byte)((k < num2) ? array[k] : 32);
			}
		}
		return result;
	}
}
