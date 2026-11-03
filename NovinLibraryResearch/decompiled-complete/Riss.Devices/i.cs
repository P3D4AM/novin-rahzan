using System;
using System.Threading;
using Riss.Devices;

internal class i : j
{
	private new const int m_a = 30000;

	private new const int m_b = 65536;

	private new const int m_c = 100;

	private const int m_d = 120;

	private new const int m_e = 300;

	private new static ZdCommBase m_f;

	private new byte[] m_g;

	private new byte[] m_h;

	private new int m_i;

	private new int m_j;

	private new int m_k;

	private new bool m_l;

	private new k m_m;

	public override int s()
	{
		bool flag = false;
		switch (n().CommunicationType)
		{
		case CommunicationType.Serial:
			a(new x());
			break;
		case CommunicationType.Usb:
			if (global::i.m_f != null)
			{
				a(global::i.m_f);
				q();
			}
			else
			{
				global::i.m_f = new v();
				a(global::i.m_f);
			}
			break;
		case CommunicationType.Tcp:
			a(new s());
			break;
		default:
			a((ZdCommBase)null);
			break;
		}
		if (l() != null)
		{
			l().Device = n();
			flag = l().Open();
		}
		if (flag && a())
		{
			return 1;
		}
		return 0;
	}

	public override int e(k A_0, ulong A_1, ulong A_2)
	{
		this.m_g = new byte[65536];
		this.m_m = A_0;
		this.m_l = ((1 == A_1) ? true : false);
		byte[] array = (this.m_l ? BitConverter.GetBytes((ushort)43605) : BitConverter.GetBytes((ushort)42330));
		Array.Copy(array, 0, this.m_g, 0, array.Length);
		byte[] bytes = BitConverter.GetBytes((ushort)n().DN);
		Array.Copy(bytes, 0, this.m_g, 2, bytes.Length);
		byte[] bytes2 = BitConverter.GetBytes((ushort)A_0);
		Array.Copy(bytes2, 0, this.m_g, 4, bytes2.Length);
		byte[] bytes3 = BitConverter.GetBytes((ushort)A_2);
		Array.Copy(bytes3, 0, this.m_g, 6, bytes3.Length);
		return 1;
	}

	public override bool f(k A_0, int A_1)
	{
		return e(A_0, 0uL, 0uL) == 1;
	}

	public override bool g(k A_0)
	{
		return f(A_0, 30000);
	}

	public override int h(ref ulong A_0, ref ulong A_1)
	{
		throw new NotImplementedException();
	}

	public override int i(ref byte[] A_0, int A_1)
	{
		int result = -1;
		Array.Copy(A_0, 0, this.m_g, 8, A_0.Length);
		a(this.m_l);
		switch (n().CommunicationType)
		{
		case CommunicationType.Usb:
			if (k())
			{
				result = 1;
			}
			break;
		case CommunicationType.Serial:
		case CommunicationType.Tcp:
			if (g())
			{
				result = 1;
			}
			break;
		}
		return result;
	}

	public override int j(ref byte[] A_0, int A_1, int A_2)
	{
		int result = -1;
		switch (n().CommunicationType)
		{
		case CommunicationType.Usb:
			if (j())
			{
				result = 1;
			}
			break;
		case CommunicationType.Serial:
		case CommunicationType.Tcp:
			if (f())
			{
				result = 1;
			}
			break;
		}
		ulong A_3 = 21930uL;
		if (m(ref A_3, (int)this.m_m) == global::j.a.a)
		{
			return -1;
		}
		A_0 = new byte[this.m_i + 2];
		Array.Copy(this.m_g, 0, A_0, 0, A_0.Length);
		return result;
	}

	public override int k(ref byte[] A_0, int A_1)
	{
		int result = -1;
		Array.Copy(A_0, 0, this.m_g, 8, A_0.Length);
		a(this.m_l);
		switch (n().CommunicationType)
		{
		case CommunicationType.Usb:
			if (i())
			{
				result = 1;
			}
			break;
		case CommunicationType.Serial:
		case CommunicationType.Tcp:
			if (e())
			{
				result = 1;
			}
			break;
		}
		return result;
	}

	public override int l(ref byte[] A_0, int A_1)
	{
		int result = -1;
		switch (n().CommunicationType)
		{
		case CommunicationType.Usb:
			if (h())
			{
				result = 1;
			}
			break;
		case CommunicationType.Serial:
		case CommunicationType.Tcp:
			if (d())
			{
				result = 1;
			}
			break;
		}
		ulong A_2 = 23205uL;
		if (m(ref A_2, (int)this.m_m) == global::j.a.a)
		{
			return -1;
		}
		this.m_j = BitConverter.ToUInt16(this.m_g, 6) + 8;
		A_0 = new byte[this.m_j + 2];
		Array.Copy(this.m_g, 0, A_0, 0, A_0.Length);
		return result;
	}

	public override a m(ref ulong A_0, int A_1)
	{
		ushort num = BitConverter.ToUInt16(this.m_g, 0);
		ushort num2 = BitConverter.ToUInt16(this.m_g, 4);
		if (A_0 != num || A_1 != num2)
		{
			return global::j.a.a;
		}
		this.m_j = BitConverter.ToUInt16(this.m_g, 6) + 8;
		ushort startIndex = (ushort)(((ushort)A_0 == 21930) ? this.m_i : this.m_j);
		ushort num3 = BitConverter.ToUInt16(this.m_g, startIndex);
		if (num3 != b((ushort)A_0 == 21930))
		{
			return global::j.a.a;
		}
		return global::j.a.b;
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
		this.m_g = new byte[65536];
		ushort num = (ushort)(A_2.Length + 12);
		byte[] bytes = BitConverter.GetBytes(num);
		byte[] array = new byte[6];
		l().CmdLen = array.Length;
		array[0] = 239;
		array[1] = 5;
		array[3] = 1;
		Array.Copy(bytes, 0, array, 4, bytes.Length);
		Array.Copy(array, 0, this.m_g, 0, array.Length);
		int num2 = array.Length;
		byte[] bytes2 = BitConverter.GetBytes(A_0);
		Array.Copy(bytes2, 0, this.m_g, num2, bytes2.Length);
		num2 += bytes2.Length;
		byte[] bytes3 = BitConverter.GetBytes(A_1);
		Array.Copy(bytes3, 0, this.m_g, num2, bytes3.Length);
		num2 += bytes3.Length;
		byte[] bytes4 = BitConverter.GetBytes(A_2.Length);
		Array.Copy(bytes4, 0, this.m_g, num2, bytes4.Length);
		num2 += bytes4.Length;
		Array.Copy(A_2, 0, this.m_g, num2, A_2.Length);
		if (l().SendData(ref this.m_g, num + array.Length) == 0)
		{
			return -1;
		}
		array[3] = 2;
		byte[] buffer = new byte[array.Length + 4];
		Array.Copy(array, 0, buffer, 0, array.Length);
		if (l().RecvData(ref buffer, buffer.Length) == 0)
		{
			return -1;
		}
		if (buffer[1] != 0)
		{
			return -1;
		}
		return 1;
	}

	private bool k()
	{
		this.m_h = new byte[8];
		this.m_h[0] = 239;
		this.m_h[1] = 17;
		this.m_h[4] = (byte)(this.m_i + 2);
		l().CmdLen = 8;
		byte[] buffer = new byte[this.m_h.Length + this.m_i + 2];
		Array.Copy(this.m_h, 0, buffer, 0, this.m_h.Length);
		Array.Copy(this.m_g, 0, buffer, l().CmdLen, this.m_i + 2);
		if (l().SendData(ref buffer, buffer.Length) == 0)
		{
			return false;
		}
		return true;
	}

	private bool j()
	{
		this.m_h = new byte[8];
		l().CmdLen = 8;
		this.m_g = new byte[65536];
		byte[] array = new byte[this.m_i + 2];
		for (int num = 0; num < array.Length; num++)
		{
			array[num] = 175;
		}
		int millisecondsTimeout = 100;
		int num2 = 0;
		do
		{
			num2++;
			this.m_h[0] = 239;
			this.m_h[1] = 18;
			byte[] buffer = new byte[this.m_h.Length + this.m_i + 2];
			Array.Copy(this.m_h, 0, buffer, 0, this.m_h.Length);
			Array.Copy(this.m_g, 0, buffer, l().CmdLen, this.m_i + 2);
			if (l().RecvData(ref buffer, buffer.Length) == 0)
			{
				return false;
			}
			Thread.Sleep(millisecondsTimeout);
			Array.Copy(buffer, 0, this.m_g, 0, buffer.Length);
			millisecondsTimeout = 120;
			if (num2 > 5)
			{
				millisecondsTimeout = 240;
			}
			if (num2 > 10)
			{
				millisecondsTimeout = 360;
			}
			if (num2 > 300)
			{
				return false;
			}
		}
		while (a(this.m_g, array, array.Length));
		return true;
	}

	private bool i()
	{
		this.m_j = BitConverter.ToUInt16(this.m_g, 6) + 8;
		ushort num = (ushort)(this.m_j + 2);
		this.m_h = new byte[8];
		this.m_h[0] = 239;
		this.m_h[1] = 19;
		this.m_h[4] = (byte)(num & 0xFFu);
		this.m_h[5] = (byte)(num >> 8);
		l().CmdLen = 8;
		byte[] buffer = new byte[this.m_h.Length + this.m_j + 2];
		Array.Copy(this.m_h, 0, buffer, 0, this.m_h.Length);
		Array.Copy(this.m_g, 0, buffer, l().CmdLen, this.m_j + 2);
		if (l().SendData(ref buffer, buffer.Length) == 0)
		{
			return false;
		}
		return true;
	}

	private bool h()
	{
		this.m_h = new byte[8];
		l().CmdLen = 8;
		this.m_g = new byte[65536];
		byte[] array = new byte[8];
		for (int num = 0; num < array.Length; num++)
		{
			array[num] = 175;
		}
		int millisecondsTimeout = 120;
		int num2 = 0;
		do
		{
			num2++;
			Thread.Sleep(millisecondsTimeout);
			this.m_h[0] = 239;
			this.m_h[1] = 21;
			byte[] buffer = new byte[this.m_h.Length + 8];
			Array.Copy(this.m_h, 0, buffer, 0, this.m_h.Length);
			Array.Copy(this.m_g, 0, buffer, l().CmdLen, 8);
			if (l().RecvData(ref buffer, buffer.Length) == 0)
			{
				return false;
			}
			Thread.Sleep(millisecondsTimeout);
			Array.Copy(buffer, 0, this.m_g, 0, buffer.Length);
			millisecondsTimeout = 120;
			if (num2 > 5)
			{
				millisecondsTimeout = 240;
			}
			if (num2 > 10)
			{
				millisecondsTimeout = 360;
			}
			if (num2 > 300)
			{
				return false;
			}
		}
		while (a(this.m_g, array, array.Length));
		millisecondsTimeout = 100;
		num2 = 0;
		do
		{
			num2++;
			this.m_k = BitConverter.ToUInt16(this.m_g, 6);
			this.m_h[1] = 20;
			byte[] buffer = new byte[this.m_h.Length + this.m_k + 2];
			Array.Copy(this.m_h, 0, buffer, 0, this.m_h.Length);
			if (l().RecvData(ref buffer, buffer.Length) == 0)
			{
				return false;
			}
			Thread.Sleep(millisecondsTimeout);
			Array.Copy(buffer, 0, this.m_g, 8, buffer.Length);
			millisecondsTimeout = 100;
			if (num2 > 5)
			{
				millisecondsTimeout = 240;
			}
			if (num2 > 20)
			{
				millisecondsTimeout = 360;
			}
			if (num2 > 300)
			{
				return false;
			}
		}
		while (a(this.m_g, array, 4));
		return true;
	}

	private bool g()
	{
		int num = this.m_i + 2;
		if (num % 4 != 0)
		{
			num += 4 - num % 4;
		}
		byte[] buffer = new byte[num];
		Array.Copy(this.m_g, 0, buffer, 0, buffer.Length);
		if (l().SendData(ref buffer, buffer.Length) == 0)
		{
			return false;
		}
		return true;
	}

	private bool f()
	{
		this.m_g = new byte[65536];
		byte[] buffer = new byte[this.m_i + 2];
		if (l().RecvData(ref buffer, buffer.Length) == 0)
		{
			return false;
		}
		Array.Copy(buffer, 0, this.m_g, 0, buffer.Length);
		return true;
	}

	private bool e()
	{
		this.m_j = BitConverter.ToUInt16(this.m_g, 6) + 8;
		int num = this.m_j + 2;
		if (num % 4 != 0)
		{
			num += 4 - num % 4;
		}
		byte[] buffer = new byte[num];
		Array.Copy(this.m_g, 0, buffer, 0, buffer.Length);
		if (l().SendData(ref buffer, buffer.Length) == 0)
		{
			return false;
		}
		return true;
	}

	private bool d()
	{
		int num = 1024;
		int num2 = 0;
		this.m_g = new byte[65536];
		byte[] buffer = new byte[8];
		if (l().RecvData(ref buffer, buffer.Length) == 0)
		{
			return false;
		}
		Array.Copy(buffer, 0, this.m_g, 0, buffer.Length);
		this.m_k = BitConverter.ToUInt16(this.m_g, 6);
		int num3 = (this.m_k + 2) / num;
		for (num2 = 0; num2 < num3; num2++)
		{
			buffer = new byte[num];
			if (l().RecvData(ref buffer, buffer.Length) == 0)
			{
				return false;
			}
			Array.Copy(buffer, 0, this.m_g, 8 + num2 * num, buffer.Length);
		}
		num3 = (this.m_k + 2) % num;
		if (num3 != 0)
		{
			buffer = new byte[num3];
			if (l().RecvData(ref buffer, buffer.Length) == 0)
			{
				return false;
			}
			Array.Copy(buffer, 0, this.m_g, 8 + num2 * num, buffer.Length);
		}
		return true;
	}

	private new bool a(byte[] A_0, byte[] A_1, int A_2)
	{
		bool result = false;
		int num = 0;
		for (int num2 = 0; num2 < A_2; num2++)
		{
			if (A_0[num2].Equals(A_1[num2]))
			{
				num++;
			}
		}
		if (num == A_2)
		{
			result = true;
		}
		return result;
	}

	public new ushort a(bool A_0)
	{
		ushort num = 0;
		this.m_i = 26;
		this.m_j = BitConverter.ToUInt16(this.m_g, 6) + 8;
		int num2 = (A_0 ? this.m_i : this.m_j);
		for (int num3 = 0; num3 < num2; num3++)
		{
			num += this.m_g[num3];
		}
		byte[] bytes = BitConverter.GetBytes(num);
		Array.Copy(bytes, 0, this.m_g, num2, bytes.Length);
		return num;
	}

	public new ushort b(bool A_0)
	{
		ushort num = 0;
		this.m_j = BitConverter.ToUInt16(this.m_g, 6) + 8;
		for (ushort num2 = 0; num2 < (A_0 ? this.m_i : this.m_j); num2++)
		{
			num += this.m_g[num2];
		}
		return num;
	}

	public new byte[] a(int A_0)
	{
		ushort value = (ushort)((uint)A_0 & 0xFFFFu);
		return BitConverter.GetBytes(value);
	}

	public new byte[] b(int A_0)
	{
		ushort value = (ushort)((uint)(A_0 >> 16) & 0xFFFFu);
		return BitConverter.GetBytes(value);
	}

	private new bool c()
	{
		bool result = false;
		byte[] A_ = new byte[1];
		if (1 == e(global::k.@do, 1uL, 0uL))
		{
			if (1 != i(ref A_, A_.Length))
			{
				return false;
			}
			if (1 != t(ref A_, A_.Length))
			{
				return false;
			}
			result = Zd2911Utils.b(A_, 8);
		}
		return result;
	}

	private new bool a(ref int A_0)
	{
		bool result = false;
		byte[] A_ = new byte[1];
		if (1 == e(global::k.dn, 1uL, 0uL))
		{
			if (1 != i(ref A_, A_.Length))
			{
				return false;
			}
			if (1 != t(ref A_, A_.Length))
			{
				return false;
			}
			if (!Zd2911Utils.b(A_, 8))
			{
				return false;
			}
			A_0 = BitConverter.ToInt32(A_, 10);
			result = true;
		}
		return result;
	}

	private new bool b()
	{
		bool result = false;
		byte[] A_ = new byte[4];
		int a_ = Convert.ToInt32(n().Password);
		Array.Copy(a(a_), 0, A_, 0, 2);
		Array.Copy(b(a_), 0, A_, 2, 2);
		if (1 == e(global::k.cd, 1uL, 4uL))
		{
			if (1 != i(ref A_, A_.Length))
			{
				return false;
			}
			if (1 != t(ref A_, A_.Length))
			{
				return false;
			}
			result = Zd2911Utils.b(A_, 8);
		}
		return result;
	}

	private new bool a()
	{
		bool flag = true;
		int A_ = 0;
		if (!c())
		{
			return false;
		}
		if (!a(ref A_))
		{
			return false;
		}
		if (30000 != A_)
		{
			return false;
		}
		flag = b();
		if (!flag)
		{
			return false;
		}
		return flag;
	}
}
