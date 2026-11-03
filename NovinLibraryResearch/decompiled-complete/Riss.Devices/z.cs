using System;
using System.Runtime.InteropServices;
using Riss.Devices;

internal class z : j
{
	protected new struct b
	{
		public byte a;

		public byte b;

		public ushort c;

		public uint d;

		public ushort e;

		public ushort f;
	}

	protected new struct c
	{
		public byte a;

		public byte b;

		public ushort c;

		public ushort d;

		public ushort e;
	}

	protected new struct a
	{
		public byte a;

		public byte b;

		public ushort c;

		public ushort d;

		public ushort e;

		public uint f;

		public ushort g;
	}

	protected enum d
	{
		a,
		b,
		c,
		d,
		e
	}

	internal new const int a = 20227;

	protected new const byte b = 85;

	protected new const byte c = 170;

	protected const byte d = 90;

	protected new const byte e = 165;

	protected new bool f;

	private new static ZdCommBase m_g = null;

	protected new static readonly int h = Marshal.SizeOf(typeof(b));

	internal new void a(ref byte[] A_0)
	{
		ulong num = u.b(l().Device.Password);
		int num2 = A_0.Length;
		ulong value = num + 20227;
		byte[] bytes = BitConverter.GetBytes(value);
		for (int num3 = 0; num3 < num2; num3++)
		{
			A_0[num3] ^= bytes[num3 % 4];
		}
	}

	public override int s()
	{
		if (l() == null)
		{
			switch (n().CommunicationType)
			{
			case CommunicationType.Serial:
				a(new w());
				break;
			case CommunicationType.Usb:
				if (z.m_g != null)
				{
					a(z.m_g);
					q();
				}
				else
				{
					z.m_g = new o();
					a(z.m_g);
				}
				a(z.m_g);
				break;
			case CommunicationType.Tcp:
				a(new e());
				break;
			default:
				a((ZdCommBase)null);
				break;
			}
		}
		else
		{
			q();
		}
		int num = a();
		if (num == 0)
		{
			if (l() != null)
			{
				q();
			}
			num = a();
		}
		return num;
	}

	protected new int a()
	{
		int num = 0;
		ulong A_ = 0uL;
		if (l() != null)
		{
			l().Device = n();
			if (l().Open())
			{
				if (a(global::k.b) && a(ref A_) == global::j.a.b)
				{
					byte[] A_2 = BitConverter.GetBytes(u.b(n().Password));
					if (i(ref A_2, 4) == 1 && a(ref A_) == global::j.a.b)
					{
						return (int)((A_ == 0) ? 1 : A_);
					}
				}
				if (num == 0)
				{
					q();
				}
			}
		}
		return num;
	}

	public override int e(k A_0, ulong A_1, ulong A_2)
	{
		throw new NotImplementedException();
	}

	protected new int b()
	{
		throw new NotImplementedException();
	}

	public override int h(ref ulong A_0, ref ulong A_1)
	{
		throw new NotImplementedException();
	}

	public override int i(ref byte[] A_0, int A_1)
	{
		b A_2 = default(b);
		A_2.d = a(A_0, A_1);
		b(ref A_2, z.d.c);
		byte[] A_3 = u.a(A_2, z.h);
		a(ref A_0);
		if (l().SendData(ref A_0, A_1) == A_1)
		{
			a(ref A_3);
			if (l().SendData(ref A_3, z.h) == z.h)
			{
				return 1;
			}
		}
		return -1;
	}

	public new int a(ref byte[] A_0, int A_1)
	{
		b A_2 = default(b);
		A_2.d = a(A_0, A_1);
		b(ref A_2, z.d.c);
		byte[] A_3 = u.a(A_2, z.h);
		a(ref A_0);
		if (l().SendData(ref A_0, A_1) != A_1)
		{
			a(ref A_3);
			if (l().SendData(ref A_3, z.h) != z.h)
			{
				return -1;
			}
			return 1;
		}
		return 1;
	}

	public override int t(ref byte[] A_0, int A_1)
	{
		return j(ref A_0, A_1, 30000);
	}

	public override int j(ref byte[] A_0, int A_1, int A_2)
	{
		if (l().RecvData(ref A_0, A_1, 30000) == A_1)
		{
			a(ref A_0);
			byte[] buffer = new byte[12];
			l().RecvData(ref buffer, 12, 500);
			return 1;
		}
		return -1;
	}

	public override int k(ref byte[] A_0, int A_1)
	{
		throw new NotImplementedException();
	}

	public override int l(ref byte[] A_0, int A_1)
	{
		throw new NotImplementedException();
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

	public override int r(int A_0, int A_1, byte[] A_2)
	{
		throw new NotImplementedException();
	}

	protected new virtual void b(ref b A_0, d A_1)
	{
		byte b2 = 85;
		byte b3 = 170;
		switch (A_1)
		{
		case z.d.a:
			b2 = 85;
			b3 = 170;
			break;
		case z.d.b:
			b2 = 170;
			b3 = 85;
			break;
		case z.d.c:
			b2 = 90;
			b3 = 165;
			break;
		case z.d.d:
			b2 = 165;
			b3 = 90;
			break;
		}
		A_0.a = b2;
		A_0.b = b3;
		A_0.c = (ushort)n().DN;
		A_0.f = a(A_0, z.h - 2);
	}

	protected new bool a(k A_0)
	{
		bool flag = false;
		b A_ = default(b);
		A_.d = (uint)A_0;
		b(ref A_, z.d.a);
		byte[] A_2 = u.a(A_, z.h);
		if (a(ref A_2, z.h) != 1)
		{
			return a(ref A_2, z.h) == 1;
		}
		return true;
	}

	public override bool f(k A_0, int A_1)
	{
		if (s() == 0)
		{
			return false;
		}
		if (a(A_0))
		{
			return true;
		}
		if (s() == 0)
		{
			return false;
		}
		return a(A_0);
	}

	public override bool g(k A_0)
	{
		return f(A_0, 30000);
	}

	public override bool n(uint A_0)
	{
		b A_ = default(b);
		A_.d = A_0;
		b(ref A_, z.d.b);
		byte[] A_2 = u.a(A_, z.h);
		return a(ref A_2, z.h) == 1;
	}

	public override j.a m(ref ulong A_0, int A_1)
	{
		b b2 = default(b);
		j.a result = global::j.a.a;
		byte[] buffer = new byte[z.h];
		if (l().RecvData(ref buffer, z.h, A_1) != z.h)
		{
			return result;
		}
		a(ref buffer);
		b2 = u.BytesToStruct<b>(buffer, 0, z.h);
		result = (a(ref b2, z.d.b) ? global::j.a.b : (a(ref b2, z.d.e) ? global::j.a.c : global::j.a.a));
		A_0 = b2.d;
		return result;
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

	protected new bool a(ref b A_0, d A_1)
	{
		byte b2 = 85;
		byte b3 = 170;
		switch (A_1)
		{
		case z.d.a:
			b2 = 85;
			b3 = 170;
			break;
		case z.d.b:
			b2 = 170;
			b3 = 85;
			break;
		case z.d.c:
			b2 = 90;
			b3 = 165;
			break;
		case z.d.e:
			b2 = 165;
			b3 = 90;
			break;
		}
		if (A_0.a != b2 || A_0.b != b3 || A_0.c != (ushort)n().DN || A_0.f != a(A_0, z.h - 2))
		{
			return false;
		}
		return true;
	}

	protected new ushort a(b A_0, int A_1)
	{
		byte[] a_ = u.a(A_0, z.h);
		return a(a_, A_1);
	}

	protected new ushort a(byte[] A_0, int A_1)
	{
		ushort num = 0;
		for (int num2 = 0; num2 < A_1; num2++)
		{
			num += A_0[num2];
		}
		return num;
	}
}
