using System;
using System.Runtime.InteropServices;
using Riss.Devices;

internal class aa : j
{
	private new struct c
	{
		public byte a;

		public byte b;

		public ushort c;

		public ushort d;

		public ushort e;

		public uint f;

		public ushort g;

		public ushort h;
	}

	private new struct b
	{
		public byte a;

		public byte b;

		public ushort c;

		public ushort d;

		public ushort e;
	}

	private new struct a
	{
		public byte a;

		public byte b;

		public ushort c;

		public ushort d;

		public ushort e;

		public uint f;

		public ushort g;
	}

	private new const byte m_a = 85;

	private new const byte m_b = 170;

	private new const byte m_c = 90;

	private const byte m_d = 165;

	public new const int e = 1024;

	public new const int f = 627;

	public new const int g = 16;

	public new const int h = 8;

	public new const int i = 14;

	public new const int j = 14;

	private new static ZdCommBase m_k;

	public override int s()
	{
		bool flag = false;
		switch (n().CommunicationType)
		{
		case CommunicationType.Serial:
			a((ZdCommBase)new q());
			break;
		case CommunicationType.Usb:
			if (aa.m_k != null)
			{
				a(aa.m_k);
				q();
			}
			else
			{
				aa.m_k = new ab();
				a(aa.m_k);
			}
			a(aa.m_k);
			break;
		case CommunicationType.Tcp:
			a((ZdCommBase)new global::a());
			break;
		default:
			a((ZdCommBase)null);
			break;
		}
		_ = DateTime.Now;
		if (l() != null)
		{
			l().Device = n();
			flag = l().Open();
			if (!flag && n().CommunicationType == CommunicationType.Usb)
			{
				aa.m_k = new o();
				a(aa.m_k);
				l().Device = n();
				flag = l().Open();
			}
		}
		if (flag && ((n().CommunicationType != CommunicationType.Tcp) ? g(global::k.b) : (e(global::k.b, 0uL, Convert.ToUInt32(n().Password)) == 1)))
		{
			ulong A_ = 0uL;
			ulong A_2 = 0uL;
			if (h(ref A_, ref A_2) == 1)
			{
				return (int)((A_2 == 0) ? 1 : A_2);
			}
		}
		return 0;
	}

	protected new byte b(object A_0)
	{
		return (byte)(((ulong)Convert.ToUInt32(A_0.ToString()) >> 8) & 0xFF);
	}

	protected new byte c(object A_0)
	{
		return (byte)((ulong)Convert.ToUInt32(A_0.ToString()) & 0xFFuL);
	}

	protected new int a(object A_0)
	{
		return (int)(((ulong)Convert.ToUInt32(A_0.ToString()) >> 16) & 0xFFFF);
	}

	protected int d(object A_0)
	{
		return (int)((long)Convert.ToUInt32(A_0.ToString()) & 0xFFFFL);
	}

	public override int e(k A_0, ulong A_1, ulong A_2)
	{
		c c2 = default(c);
		int num = -1;
		c2.a = 85;
		ushort num2 = c2.a;
		c2.b = 170;
		num2 += c2.b;
		c2.c = (ushort)n().DN;
		num2 += b(n().DN);
		num2 += c(n().DN);
		c2.d = 6521;
		num2 += c(c2.d);
		num2 += b(c2.d);
		c2.e = (ushort)A_0;
		num2 += b(c2.e);
		num2 += c(c2.e);
		c2.f = (uint)A_2;
		num2 += b(a(c2.f));
		num2 += c(a(c2.f));
		num2 += b(d(c2.f));
		num2 += c(d(c2.f));
		c2.g = (ushort)A_1;
		num2 += b(c2.g);
		num2 += c(c2.g);
		c2.h = num2;
		byte[] array = new byte[16];
		array = u.a(c2, 16);
		num = l().SendData(ref array, 16);
		if (num >= 16)
		{
			num = (int)m(ref A_1, 30000);
		}
		return num;
	}

	public override int h(ref ulong A_0, ref ulong A_1)
	{
		uint num = 0u;
		a a2 = default(a);
		int num2 = -1;
		byte[] array = new byte[14];
		array = u.a(a2, 14);
		num2 = l().RecvData(ref array, 14);
		a2 = u.BytesToStruct<a>(array, 0, 14);
		if (num2 != 14)
		{
			return -1;
		}
		if (a2.a != 170)
		{
			return -1;
		}
		num = a2.a;
		if (a2.b != 85)
		{
			return -1;
		}
		num += a2.b;
		if (a2.c != n().DN)
		{
			return -1;
		}
		num += b(a2.c);
		num += c(a2.c);
		num += b(a2.d);
		num += c(a2.d);
		A_0 = a2.e;
		num += b(a2.e);
		num += c(a2.e);
		A_1 = a2.f;
		num += b(a(a2.f));
		num += c(a(a2.f));
		num += b(d(a2.f));
		num += c(d(a2.f));
		if (a2.g != num || A_0 == 0)
		{
			return -1;
		}
		return 1;
	}

	public override int i(ref byte[] A_0, int A_1)
	{
		ushort num = 0;
		int num2 = A_1 + 6;
		byte[] buffer = new byte[num2];
		buffer[0] = 90;
		buffer[1] = 165;
		buffer[2] = (byte)n().DN;
		buffer[3] = 0;
		A_0.CopyTo(buffer, 4);
		for (int num3 = 0; num3 < num2 - 2; num3++)
		{
			num += buffer[num3];
		}
		buffer[A_1 + 4] = (byte)num;
		buffer[A_1 + 5] = (byte)(num >> 8);
		int num4 = l().SendData(ref buffer, num2);
		return (num4 == num2) ? 1 : (-1);
	}

	public override int t(ref byte[] A_0, int A_1)
	{
		return j(ref A_0, A_1, 30000);
	}

	public override int k(ref byte[] A_0, int A_1)
	{
		int result = 1;
		int num = 1020;
		int num2 = A_1;
		int num3 = 0;
		int num4 = 0;
		while (num2 > 0)
		{
			ushort num5 = 0;
			num4 = ((num2 > num) ? num : num2);
			byte[] buffer = new byte[num4 + 6];
			buffer[0] = 90;
			buffer[1] = 165;
			buffer[2] = (byte)n().DN;
			buffer[3] = 0;
			for (int num6 = 0; num6 < num4; num6++)
			{
				buffer[num6 + 4] = A_0[num3 + num6];
			}
			for (int num7 = 0; num7 < num4 + 4; num7++)
			{
				num5 += buffer[num7];
			}
			buffer[num4 + 4] = (byte)num5;
			buffer[num4 + 5] = (byte)(num5 >> 8);
			int num8 = l().SendData(ref buffer, num4 + 6);
			if (num8 != num4 + 6)
			{
				return -1;
			}
			num3 += num4;
			num2 = A_1 - num3;
		}
		return result;
	}

	public override int l(ref byte[] A_0, int A_1)
	{
		int num = 0;
		int num2 = 1020;
		int num3 = A_1;
		int num4 = 0;
		int num5 = 0;
		byte[] array = new byte[A_1 + 6];
		bool flag = true;
		while (num3 > 0)
		{
			int num6 = 0;
			num5 = ((num3 > num2) ? num2 : num3);
			byte[] buffer = new byte[num5 + 6];
			num = l().RecvData(ref buffer, num5 + 6);
			if (num == num5 + 6)
			{
				if (buffer[0] != 90 || buffer[1] != 165)
				{
					return -1;
				}
				if (n().DN != (buffer[2] | (buffer[3] << 8)))
				{
					return -1;
				}
				if (A_1 - num4 - num5 == 0)
				{
					for (int num7 = 0; num7 < num - 2; num7++)
					{
						num6 += buffer[num7];
					}
					if ((ushort)num6 != (buffer[num - 2] | (buffer[num - 1] << 8)))
					{
						A_0 = null;
						return -1;
					}
				}
				if (flag)
				{
					buffer.CopyTo(array, 0);
					flag = false;
				}
				else if (A_0.Length - num4 - num5 == 0)
				{
					for (int num8 = 4; num8 < num; num8++)
					{
						array[num8 + num4] = buffer[num8];
					}
				}
				else
				{
					for (int num9 = 0; num9 < num5; num9++)
					{
						array[num9 + num4 + 4] = buffer[num9 + 4];
					}
				}
				num4 += num5;
				num3 = A_0.Length - num4;
				continue;
			}
			return -1;
		}
		for (int num10 = 0; num10 < A_1; num10++)
		{
			A_0[num10] = array[num10 + 4];
		}
		return 1;
	}

	public override void p()
	{
		throw new NotImplementedException();
	}

	public override void q()
	{
		if (l() != null)
		{
			l().Close();
		}
	}

	public override bool f(k A_0, int A_1)
	{
		return e(A_0, 0uL, 0uL) == 1;
	}

	public override bool g(k A_0)
	{
		return f(A_0, 30000);
	}

	public override int j(ref byte[] A_0, int A_1, int A_2)
	{
		ushort num = 0;
		int num2 = A_1 + 6;
		byte[] buffer = new byte[num2];
		int num3 = l().RecvData(ref buffer, num2, A_2);
		if (num3 == num2 && buffer[0] == 165 && buffer[1] == 90 && n().DN == (buffer[2] | (buffer[3] << 8)))
		{
			for (int num4 = 0; num4 < num2 - 2; num4++)
			{
				num += buffer[num4];
			}
			if (num == (buffer[num2 - 2] | (buffer[num2 - 1] << 8)))
			{
				for (int num5 = 0; num5 < A_1; num5++)
				{
					A_0[num5] = buffer[num5 + 4];
				}
				return 1;
			}
		}
		return -1;
	}

	public override j.a m(ref ulong A_0, int A_1)
	{
		b b2 = default(b);
		int num = Marshal.SizeOf((object)b2);
		byte[] array = new byte[num];
		array = u.a(b2, num);
		DateTime now = DateTime.Now;
		int num2 = 0;
		while ((DateTime.Now - now).TotalMilliseconds < (double)((n().CommunicationType != CommunicationType.Tcp) ? A_1 : (A_1 * 5)))
		{
			if (n().CommunicationType != 0)
			{
				num2 = l().RecvData(ref array, num);
			}
			else
			{
				try
				{
					num2 = l().RecvData(ref array, num, A_1);
				}
				catch
				{
				}
				if (num2 == 0)
				{
					continue;
				}
			}
			if (num2 != num)
			{
				num2 = 0;
				break;
			}
			b2 = u.BytesToStruct<b>(array, 0, num);
			uint num3 = b2.a;
			num3 += b2.b;
			num3 += b(b2.c);
			num3 += c(b2.c);
			num3 += b2.d;
			num2 = ((b2.a == 90 && b2.b == 165 && b2.c == n().DN && b2.e == num3 && b2.d == 1) ? 1 : 0);
			break;
		}
		return (j.a)num2;
	}

	public override bool n(uint A_0)
	{
		throw new NotImplementedException();
	}

	public override bool o()
	{
		int[] array = new int[5] { 9600, 19200, 38400, 57600, 115200 };
		m();
		for (int num = 0; num < array.Length; num++)
		{
			n().Baudrate = array[num];
			n().CommunicationType = CommunicationType.Serial;
			if (l().Open())
			{
				return true;
			}
		}
		return false;
	}

	public override int r(int A_0, int A_1, byte[] A_2)
	{
		throw new NotImplementedException();
	}
}
