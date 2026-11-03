using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Riss.Devices;

internal class r : DeviceConnection
{
	private new enum f
	{
		a,
		b,
		c
	}

	protected new enum d
	{
		a = -1,
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
		ad = 28,
		ae = 29,
		af = 30,
		ag = 250,
		ah = 1001,
		ai = 1002,
		aj = 1003,
		ak = 1004,
		al = 1005,
		am = 1006,
		an = 1007,
		ao = 1008,
		ap = 1009,
		aq = 1010,
		ar = 1011,
		@as = 1012,
		at = 2000,
		au = 2001,
		av = 2002
	}

	private new enum h
	{
		a = 1281,
		b,
		c,
		d,
		e,
		f,
		g,
		h,
		i,
		j,
		k,
		l,
		m,
		n,
		o
	}

	private new enum a
	{
		a = 1,
		b,
		c,
		d,
		e,
		f,
		g,
		h,
		i,
		j,
		k,
		l,
		m,
		n,
		o,
		p,
		q,
		r,
		s,
		t,
		u,
		v,
		w,
		x,
		y,
		z,
		aa,
		ab,
		ac,
		ad,
		ae,
		af,
		ag,
		ah,
		ai,
		aj,
		ak,
		al,
		am,
		an,
		ao,
		ap,
		aq,
		ar,
		@as,
		at,
		au,
		av,
		aw,
		ax,
		ay,
		az,
		a0,
		a1,
		a2,
		a3,
		a4,
		a5,
		a6,
		a7,
		a8,
		a9,
		ba,
		bb,
		bc,
		bd,
		be,
		bf,
		bg,
		bh,
		bi,
		bj,
		bk,
		bl
	}

	internal new enum e
	{
		a,
		b,
		c
	}

	internal new enum i
	{
		a,
		b,
		c
	}

	private new struct g
	{
		public uint a;

		public uint b;

		public uint c;

		public byte d;

		public byte e;

		public ushort f;
	}

	private new struct c
	{
		public int a;

		public uint b;

		public uint c;

		public byte d;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
		public byte[] e;
	}

	protected new struct b
	{
		public uint a;

		public uint b;
	}

	private new const int m_a = 1000;

	private new const int m_b = 10000;

	private new const int m_c = 500000;

	private new const int m_d = 1404;

	private new const int m_e = 48;

	private new const int m_f = 6;

	private new const int m_g = 4;

	private new const int m_h = 10;

	private new const int m_i = 32;

	private new const int j = 50;

	private new const int k = 5;

	private new const int l = 10;

	private const int m = 10;

	private const int n = 10;

	private new const int o = 5;

	private new const int p = 10;

	private new const int q = 60;

	private new const int m_r = 72;

	private const int s = 10;

	private new const int t = 32;

	private new const int u = 32;

	private new const int v = 10;

	private const int w = 32;

	private const int x = 2;

	private new const int y = 10;

	private new const int z = 7;

	private new const int aa = 1;

	private new const int ab = 2;

	private new const int ac = 4;

	private const int ad = 1048576;

	private const int ae = 2097152;

	private const int af = 13631488;

	private const int ag = 16777216;

	protected const int ah = 32;

	private const int ai = 24;

	private const int aj = 1380271940;

	private const int ak = 1397509959;

	private const int al = 1196183367;

	private const int am = 24;

	private const int an = 12;

	private const int ao = 16;

	protected const int ap = 8;

	private const int aq = 7;

	protected const int ar = 10000;

	protected const int @as = 8;

	private int at = 6;

	protected uint au = 3925868663u;

	private uint av = 2882400000u;

	private int aw = 262144;

	public r()
	{
		DeviceComm = new z();
	}

	public override int Open()
	{
		DeviceComm.a(base.Device);
		int num = DeviceComm.s();
		base.y = num == 10512;
		base.z = num == 10514;
		if (base.y || base.z)
		{
			base.ab = true;
			return 2910;
		}
		if (num <= 0)
		{
			return 0;
		}
		return 3000;
	}

	protected override bool EnableDevice(long bEnable)
	{
		if (bEnable != 0 && bEnable != 1)
		{
			return false;
		}
		byte[] A_ = BitConverter.GetBytes((int)bEnable);
		bool result = false;
		ulong A_2 = 0uL;
		if (DeviceComm.g(global::k.bo) && DeviceComm.a(ref A_2) == global::j.a.b && DeviceComm.i(ref A_, 4) == 1)
		{
			result = DeviceComm.a(ref A_2) == global::j.a.b;
		}
		return result;
	}

	public override void Close()
	{
		DeviceComm.q();
	}

	public override bool SetProperty(UserProperty property, object extraProperty, User user, object extraData)
	{
		bool result = false;
		switch (property)
		{
		case UserProperty.UserName:
		{
			byte[] array = new byte[24];
			byte[] bytes2 = Encoding.Unicode.GetBytes(user.UserName);
			Array.Copy(bytes2, array, (bytes2.Length < 24) ? bytes2.Length : 24);
			result = a(user.DIN, global::r.d.u, array);
			break;
		}
		case UserProperty.Enroll:
		{
			bool flag = (bool)extraProperty;
			if (user.Enrolls.Count == 0)
			{
				break;
			}
			Enroll enroll = user.Enrolls[0];
			byte[] array2 = null;
			d a_2 = (d)(enroll.EnrollType + 1);
			if (!flag)
			{
				a_2 = (d)((user == null) ? ((EnrollType)(-1)) : (enroll.EnrollType + 1000 + 1));
			}
			else
			{
				switch (enroll.EnrollType)
				{
				case EnrollType.Password:
				{
					array2 = new byte[6];
					byte[] bytes3 = Encoding.ASCII.GetBytes(enroll.Password);
					Array.Copy(bytes3, array2, (bytes3.Length < 6) ? bytes3.Length : 6);
					break;
				}
				case EnrollType.Card:
					array2 = BitConverter.GetBytes(Convert.ToUInt32(enroll.CardID));
					break;
				default:
					if (EnrollType.Finger0 <= enroll.EnrollType && enroll.EnrollType <= EnrollType.Finger9)
					{
						array2 = enroll.Fingerprint;
					}
					break;
				case EnrollType.All:
					break;
				}
			}
			result = a(enroll.DIN, a_2, array2);
			break;
		}
		case UserProperty.UserExtInfo:
			if (user.Enrolls.Count > 0)
			{
				byte[] a_ = null;
				int enrollType = (int)user.Enrolls[0].EnrollType;
				result = ((enrollType >= 10) ? ((enrollType != 10) ? a(user.DIN, global::r.d.av, a_) : a(user.DIN, global::r.d.au, a_)) : a(user.DIN, global::r.d.at, a_));
			}
			break;
		case UserProperty.Privilege:
		{
			int num = user.Privilege;
			if (!base.ab)
			{
				num = ((num == 0) ? 1 : (num + 5));
			}
			byte[] bytes = BitConverter.GetBytes(num);
			result = a(user.DIN, global::r.d.q, bytes);
			break;
		}
		}
		return result;
	}

	public override bool GetProperty(UserProperty property, object extraProperty, ref User user, ref object extraData)
	{
		bool flag = false;
		switch (property)
		{
		case UserProperty.UserName:
		{
			byte[] A_3 = new byte[24];
			flag = a(global::r.d.u, user.DIN, ref A_3);
			user.UserName = Encoding.Unicode.GetString(A_3).Replace("\0", "").Trim();
			break;
		}
		case UserProperty.Enroll:
		{
			if (user.Enrolls.Count == 0)
			{
				break;
			}
			Enroll enroll = user.Enrolls[0];
			d d2 = (d)(enroll.EnrollType + 1);
			byte[] A_2 = null;
			switch (d2)
			{
			case global::r.d.m:
				A_2 = new byte[6];
				flag = a(d2, user.DIN, ref A_2);
				if (flag)
				{
					enroll.Password = Encoding.ASCII.GetString(A_2, 0, 4).Replace("\0", "");
				}
				break;
			case global::r.d.n:
				flag = a(d2, user.DIN, ref A_2);
				if (flag)
				{
					enroll.CardID = BitConverter.ToInt32(A_2, 0).ToString();
				}
				break;
			default:
				if (global::r.d.c <= d2 && d2 <= global::r.d.l)
				{
					A_2 = new byte[1404];
					flag = a(d2, user.DIN, ref A_2);
					enroll.Fingerprint = A_2;
				}
				break;
			}
			user.Enrolls[0] = enroll;
			break;
		}
		case UserProperty.Privilege:
			if (user.Enrolls.Count != 0)
			{
				_ = user.Enrolls[0];
				byte[] A_ = null;
				flag = a(global::r.d.q, user.DIN, ref A_);
				if (flag)
				{
					int num = BitConverter.ToInt32(A_, 0);
					user.Privilege = ((num != 1) ? (num - 5) : 0);
				}
			}
			break;
		}
		return flag;
	}

	public override bool SetProperty(DeviceProperty property, object extraProperty, Device device, object extraData)
	{
		bool result = false;
		switch (property)
		{
		case DeviceProperty.FirmwareUpgrade:
		{
			byte[] array5 = (byte[])extraData;
			result = c(array5, array5.Length);
			break;
		}
		case DeviceProperty.Bell:
			result = a(global::r.a.a4, (byte[])extraData);
			break;
		case DeviceProperty.DoorControl:
		{
			DoorStatus doorStatus = (DoorStatus)extraData;
			byte[] bytes;
			byte[] a_2;
			switch (doorStatus)
			{
			case DoorStatus.ForceOpen:
			case DoorStatus.ForceClose:
			{
				byte[] bytes3 = BitConverter.GetBytes((int)doorStatus);
				result = a(global::r.a.ar, bytes3);
				break;
			}
			case DoorStatus.ProgramOpen:
			case DoorStatus.AutoRecover:
			{
				byte[] bytes2 = BitConverter.GetBytes((int)doorStatus);
				result = a(global::r.a.ay, bytes2);
				break;
			}
			case DoorStatus.ReStart:
				result = a(global::r.f.c);
				break;
			case DoorStatus.Alarm:
				bytes = BitConverter.GetBytes(1);
				goto IL_0110;
			case DoorStatus.CancelAlarm:
				{
					bytes = BitConverter.GetBytes(0);
					goto IL_0110;
				}
				IL_0110:
				a_2 = bytes;
				result = a(global::r.a.ax, a_2);
				break;
			}
			break;
		}
		case DeviceProperty.AccessControlSettings:
			switch ((AccessContorlCommand)extraProperty)
			{
			case AccessContorlCommand.PassTime:
				result = a(global::r.a.bi, (byte[])extraData);
				break;
			case AccessContorlCommand.GroupTime:
				result = a(global::r.a.bj, (byte[])extraData);
				break;
			case AccessContorlCommand.TimeZone:
				result = a(global::r.a.bk, (byte[])extraData);
				break;
			case AccessContorlCommand.LockGroup:
				result = a(global::r.a.a2, (byte[])extraData);
				break;
			}
			break;
		case DeviceProperty.InitSettings:
		{
			int num = (int)extraProperty;
			byte[] A_ = (byte[])extraData;
			byte[] array3 = null;
			if (A_.Length > num)
			{
				array3 = new byte[A_.Length - num];
				Array.Copy(A_, num, array3, 0, array3.Length);
				byte[] array4 = new byte[num];
				Array.Copy(A_, array4, num);
				A_ = array4;
			}
			result = d(ref A_, A_1: true, array3);
			break;
		}
		case DeviceProperty.PowerOff:
			result = a((f)extraData);
			break;
		case DeviceProperty.DeviceTime:
		{
			DateTime now = DateTime.Now;
			byte[] array2 = a(now);
			BitConverter.ToInt32(array2, 0);
			result = a(global::r.a.b, array2);
			break;
		}
		case DeviceProperty.ManagementRecords:
			result = a(2L);
			break;
		case DeviceProperty.AttRecords:
			result = a(4L);
			break;
		case DeviceProperty.Enrolls:
			result = a(1L);
			break;
		case DeviceProperty.Message:
		{
			byte[] a_3 = (byte[])extraData;
			result = a(global::r.a.a3, a_3);
			break;
		}
		case DeviceProperty.Enable:
		{
			long bEnable = Convert.ToInt64(extraData.ToString());
			result = EnableDevice(bEnable);
			break;
		}
		case DeviceProperty.DataFile:
		{
			byte[] a_ = (byte[])extraData;
			string[] array = extraProperty.ToString().Split(new char[1] { ';' });
			if (array.Length > 1)
			{
				result = a((y)Convert.ToUInt32(array[0]), array[1], a_);
			}
			break;
		}
		}
		return result;
	}

	public override bool GetProperty(DeviceProperty property, object extraProperty, ref Device device, ref object extraData)
	{
		bool flag = false;
		switch (property)
		{
		case DeviceProperty.FirmwareVersion:
		{
			byte[] A_19 = new byte[1];
			flag = a(global::g.r, ref A_19);
			if (flag)
			{
				string @string = Encoding.Default.GetString(A_19);
				char[] trimChars = new char[1];
				extraData = @string.TrimEnd(trimChars);
			}
			break;
		}
		case DeviceProperty.Bell:
		{
			int num3 = at * 10;
			byte[] A_20 = new byte[num3];
			flag = a(global::r.a.a4, ref A_20);
			extraData = A_20;
			break;
		}
		case DeviceProperty.DoorControl:
		{
			DoorStatus doorStatus = (DoorStatus)extraProperty;
			byte[] A_7 = null;
			switch (doorStatus)
			{
			case DoorStatus.ForceOpen:
			case DoorStatus.ForceClose:
				flag = a(global::r.a.ar, ref A_7);
				break;
			case DoorStatus.ProgramOpen:
			case DoorStatus.AutoRecover:
				flag = a(global::r.a.ay, ref A_7);
				break;
			case DoorStatus.ReStart:
				flag = a(global::r.f.c);
				break;
			}
			if (flag)
			{
				extraData = (DoorStatus)BitConverter.ToInt32(A_7, 0);
			}
			break;
		}
		case DeviceProperty.AccessControlSettings:
			switch ((AccessContorlCommand)extraProperty)
			{
			case AccessContorlCommand.PassTime:
			{
				List<DateTime> list = new List<DateTime>();
				int num = 128;
				byte[] A_6 = new byte[num];
				flag = a(global::r.a.bi, ref A_6);
				for (int num2 = 0; num2 < num; num2++)
				{
					if ((num2 + 1) % 2 == 0)
					{
						DateTime item = DateTime.Parse("2011-03-09 0:0:0").AddMinutes((int)A_6[num2 - 1]).AddHours((int)A_6[num2]);
						list.Add(item);
					}
				}
				extraData = list;
				break;
			}
			case AccessContorlCommand.GroupTime:
			{
				byte[] A_5 = new byte[320];
				flag = a(global::r.a.bj, ref A_5);
				extraData = A_5;
				break;
			}
			case AccessContorlCommand.TimeZone:
			{
				byte[] A_4 = new byte[224];
				flag = a(global::r.a.a1, ref A_4);
				extraData = A_4;
				break;
			}
			case AccessContorlCommand.LockGroup:
			{
				byte[] A_3 = new byte[20];
				flag = a(global::r.a.a2, ref A_3);
				extraData = A_3;
				break;
			}
			}
			break;
		case DeviceProperty.InitSettings:
		{
			int num4 = (int)extraProperty;
			byte[] a_4 = null;
			byte[] A_22 = new byte[num4];
			flag = d(ref A_22, A_1: false, a_4);
			extraData = A_22;
			break;
		}
		case DeviceProperty.Status:
		{
			byte[] A_21 = new byte[1];
			flag = a((global::g)extraProperty, ref A_21);
			extraData = A_21;
			break;
		}
		case DeviceProperty.DeviceTime:
		{
			byte[] A_17 = null;
			flag = a(global::r.a.b, ref A_17);
			DateTime A_18 = DateTime.Now;
			if (flag)
			{
				flag = a(A_17, ref A_18);
			}
			extraData = A_18;
			break;
		}
		case DeviceProperty.ManagementRecords:
		{
			List<Record> A_16 = new List<Record>();
			bool a_3 = (bool)extraProperty;
			flag = a(a_3, ref A_16);
			extraData = A_16;
			break;
		}
		case DeviceProperty.AttRecords:
		{
			bool a_2 = (bool)extraProperty;
			List<Record> A_15 = new List<Record>();
			flag = b(a_2, ref A_15);
			extraData = A_15;
			break;
		}
		case DeviceProperty.Enrolls:
		{
			List<User> A_14 = new List<User>();
			flag = a(ref A_14);
			extraData = A_14;
			break;
		}
		case DeviceProperty.Message:
		{
			byte[] A_13 = new byte[720];
			flag = a(global::r.a.a3, ref A_13);
			extraData = A_13;
			break;
		}
		case DeviceProperty.ManagementRecordsCount:
		{
			ulong A_11 = 0uL;
			ulong A_12 = 0uL;
			flag = a(global::f.a, ref A_11, ref A_12);
			byte[] array3 = new byte[8];
			Array.Copy(BitConverter.GetBytes((int)A_11), array3, 4);
			Array.Copy(BitConverter.GetBytes((int)A_12), 0, array3, 4, 4);
			extraData = array3;
			break;
		}
		case DeviceProperty.AttRecordsCount:
		{
			ulong A_9 = 0uL;
			ulong A_10 = 0uL;
			flag = a(global::f.b, ref A_9, ref A_10);
			byte[] array2 = new byte[8];
			Array.Copy(BitConverter.GetBytes((int)A_9), array2, 4);
			Array.Copy(BitConverter.GetBytes((int)A_10), 0, array2, 4, 4);
			extraData = array2;
			break;
		}
		case DeviceProperty.DataFile:
			try
			{
				byte[] A_8 = new byte[1];
				string[] array = extraProperty.ToString().Split(new char[1] { ';' });
				if (array.Length > 1)
				{
					flag = a((y)Convert.ToUInt32(array[0]), array[1], ref A_8);
					extraData = A_8;
				}
			}
			catch
			{
			}
			break;
		case DeviceProperty.PhotoList:
		{
			byte[] A_ = (byte[])extraProperty;
			int a_ = BitConverter.ToInt32(A_, 0);
			int A_2 = BitConverter.ToInt32(A_, 4);
			bool flag2 = A_2 == 0;
			flag = a(a_, ref A_2, ref A_);
			if (flag2)
			{
				extraData = A_2;
			}
			else
			{
				extraData = A_;
			}
			break;
		}
		}
		return flag;
	}

	protected new bool a(d A_0, ulong A_1, ref byte[] A_2)
	{
		byte[] A_3 = new byte[8];
		ulong A_4 = 0uL;
		byte[] bytes = BitConverter.GetBytes((int)A_1);
		byte[] bytes2 = BitConverter.GetBytes((int)A_0);
		Array.Copy(bytes, A_3, 4);
		Array.Copy(bytes2, 0, A_3, 4, 4);
		if (DeviceComm.g(global::k.be) && DeviceComm.a(ref A_4) == global::j.a.b && DeviceComm.i(ref A_3, A_3.Length) == 1 && DeviceComm.a(ref A_4) == global::j.a.b)
		{
			if (A_2 != null)
			{
				return DeviceComm.t(ref A_2, A_2.Length) == 1;
			}
			A_2 = BitConverter.GetBytes((int)A_4);
			return true;
		}
		return false;
	}

	protected new bool a(ulong A_0, d A_1, byte[] A_2)
	{
		byte[] A_3 = new byte[8];
		ulong A_4 = 0uL;
		bool flag = false;
		if (base.ab && A_1 == global::r.d.r)
		{
			A_2 = new byte[1] { (A_2[0] == 0) ? ((byte)1) : ((byte)0) };
		}
		byte[] bytes = BitConverter.GetBytes((int)A_0);
		byte[] bytes2 = BitConverter.GetBytes((int)A_1);
		Array.Copy(bytes, A_3, 4);
		Array.Copy(bytes2, 0, A_3, 4, 4);
		if (DeviceComm.g(global::k.bf) && DeviceComm.a(ref A_4) == global::j.a.b && DeviceComm.i(ref A_3, A_3.Length) == 1)
		{
			flag = ((A_1 != global::r.d.at && A_1 != global::r.d.au && A_1 != global::r.d.av) ? (DeviceComm.a(ref A_4) == global::j.a.b) : (DeviceComm.m(ref A_4, 30000) == global::j.a.b));
			if (flag && A_2 != null)
			{
				flag = false;
				if (DeviceComm.i(ref A_2, A_2.Length) == 1 && DeviceComm.a(ref A_4) == global::j.a.b)
				{
					flag = true;
				}
			}
		}
		return flag;
	}

	private new bool a(f A_0)
	{
		bool result = false;
		ulong A_ = 0uL;
		byte[] A_2 = BitConverter.GetBytes((int)A_0);
		if (A_0 == global::r.f.b)
		{
			DeviceComm.p();
			return true;
		}
		if (DeviceComm.g(global::k.bp) && DeviceComm.a(ref A_) == global::j.a.b && DeviceComm.i(ref A_2, A_2.Length) == A_2.Length)
		{
			result = DeviceComm.a(ref A_) == global::j.a.b;
		}
		return result;
	}

	private new bool a(global::g A_0, ref byte[] A_1)
	{
		ulong A_2 = 0uL;
		bool result = false;
		byte[] A_3 = new byte[32];
		byte[] A_4 = BitConverter.GetBytes((int)A_0);
		if (DeviceComm.g(global::k.bj) && DeviceComm.a(ref A_2) == global::j.a.b && DeviceComm.i(ref A_4, 4) == 1)
		{
			A_2 = (ulong)A_0;
			if (DeviceComm.a(ref A_2) == global::j.a.b)
			{
				if ((global::g.a <= A_0 && A_0 <= global::g.k) || A_0 == global::g.s)
				{
					A_1 = BitConverter.GetBytes((int)A_2);
					result = true;
				}
				else
				{
					result = DeviceComm.t(ref A_3, 32) == 1;
					A_1 = A_3;
				}
			}
		}
		return result;
	}

	private new bool a(a A_0, ref byte[] A_1)
	{
		byte[] A_2 = BitConverter.GetBytes((int)A_0);
		ulong A_3 = 0uL;
		if (DeviceComm.g(global::k.bk) && DeviceComm.a(ref A_3) == global::j.a.b && DeviceComm.i(ref A_2, 4) == 1 && DeviceComm.a(ref A_3) == global::j.a.b)
		{
			if (A_1 != null)
			{
				return DeviceComm.t(ref A_1, A_1.Length) == 1;
			}
			A_1 = BitConverter.GetBytes((int)A_3);
			return true;
		}
		return false;
	}

	private new bool a(a A_0, byte[] A_1)
	{
		byte[] A_2 = new byte[8];
		uint num = BitConverter.ToUInt32(A_1, 0);
		bool result = false;
		if (A_0 < global::r.a.a || A_0 > global::r.a.bl || (A_0 != global::r.a.b && num == 0))
		{
			return result;
		}
		byte[] bytes = BitConverter.GetBytes((int)A_0);
		byte[] bytes2 = BitConverter.GetBytes((int)num);
		Array.Copy(bytes, A_2, 4);
		Array.Copy(bytes2, 0, A_2, 4, 4);
		ulong A_3 = 0uL;
		if (DeviceComm.g(global::k.bl) && DeviceComm.a(ref A_3) == global::j.a.b && DeviceComm.i(ref A_2, 8) == 1)
		{
			result = DeviceComm.a(ref A_3) == global::j.a.b;
		}
		if (A_1.Length > 4)
		{
			result = false;
			if (DeviceComm.i(ref A_1, A_1.Length) == 1)
			{
				result = DeviceComm.a(ref A_3) == global::j.a.b;
			}
		}
		return result;
	}

	private new int a(y A_0, string A_1, int A_2, bool A_3)
	{
		int num = 0;
		switch (A_0)
		{
		case global::y.h:
			num = 0;
			break;
		case global::y.i:
			num = 1;
			break;
		case global::y.j:
			num = 2;
			break;
		default:
			return -1;
		}
		byte[] A_4 = BitConverter.GetBytes(num);
		int value = ((!A_3) ? 1 : 0);
		byte[] A_5 = BitConverter.GetBytes(value);
		ulong A_6 = 0uL;
		int result = 0;
		if (DeviceComm.g(global::k.bm) && DeviceComm.a(ref A_6) == global::j.a.b && DeviceComm.i(ref A_4, A_4.Length) == 1 && DeviceComm.a(ref A_6) == global::j.a.b && DeviceComm.i(ref A_5, A_5.Length) == 1 && DeviceComm.a(ref A_6) == global::j.a.b && DeviceComm.t(ref A_5, A_5.Length) == 1 && DeviceComm.n(1u))
		{
			int value2;
			if (A_2 == 0)
			{
				result = BitConverter.ToInt32(A_5, 0) + 4;
				value2 = 1;
			}
			else if (A_2 < BitConverter.ToInt32(A_5, 0) + 4)
			{
				result = -1;
				value2 = 1;
			}
			else
			{
				value2 = 0;
			}
			byte[] A_7 = BitConverter.GetBytes(value2);
			if (DeviceComm.i(ref A_7, A_7.Length) == 1 && DeviceComm.a(ref A_6) == global::j.a.b)
			{
				return result;
			}
		}
		return -1;
	}

	private new int a(y A_0, string A_1, int A_2)
	{
		byte[] A_3 = new byte[32];
		byte[] bytes = BitConverter.GetBytes((int)A_0);
		byte[] bytes2 = BitConverter.GetBytes((A_2 > 0) ? A_2 : (-1));
		byte[] bytes3 = Encoding.ASCII.GetBytes(A_1);
		Array.Copy(bytes, A_3, 4);
		Array.Copy(bytes2, 0, A_3, 4, 4);
		Array.Copy(bytes3, 0, A_3, 8, bytes3.Length);
		ulong A_4 = 0uL;
		int result = -1;
		if (DeviceComm.g(global::k.bm) && DeviceComm.a(ref A_4) == global::j.a.b && DeviceComm.i(ref A_3, 32) == 1 && DeviceComm.a(ref A_4) == global::j.a.b)
		{
			result = (int)A_4;
		}
		return result;
	}

	private new bool a(y A_0, ref string A_1)
	{
		if (A_1.Length > 24)
		{
			return false;
		}
		bool result = true;
		switch (A_0)
		{
		case global::y.b:
			A_1 = "screensaver";
			break;
		case global::y.c:
			try
			{
				uint num3 = Convert.ToUInt32(A_1);
				result = num3 >= 0 && num3 <= 49;
			}
			catch
			{
				result = false;
			}
			break;
		case global::y.d:
			try
			{
				uint num2 = Convert.ToUInt32(A_1);
				result = num2 >= 0 && num2 <= 999999999;
				A_1 = A_1.PadLeft(9, '0');
			}
			catch
			{
				result = false;
			}
			break;
		case global::y.e:
			try
			{
				uint num = Convert.ToUInt32(A_1);
				result = num >= 0 && num <= 999999999;
				A_1 = A_1.PadLeft(9, '0');
			}
			catch
			{
				result = false;
			}
			break;
		case global::y.f:
			result = A_1.Length == 23;
			break;
		case global::y.g:
			A_1 = "part";
			break;
		case global::y.h:
			A_1 = "ENROLLDB";
			break;
		case global::y.i:
			A_1 = "SLOG";
			break;
		case global::y.j:
			A_1 = "GLOG";
			break;
		}
		return result;
	}

	private new bool a(y A_0, string A_1, ref byte[] A_2)
	{
		if (!a(A_0, ref A_1))
		{
			return false;
		}
		if (!base.ab)
		{
			int num = a(A_0, A_1, 0);
			if (num > 0)
			{
				A_2 = new byte[num];
				if (a(A_0, A_1, num) > -1)
				{
					return DeviceComm.t(ref A_2, num) == 1;
				}
			}
		}
		else
		{
			int num2 = a(A_0, A_1, 0, A_3: true);
			if (num2 > 0)
			{
				A_2 = new byte[num2];
				if (a(A_0, A_1, num2, A_3: true) > -1)
				{
					return DeviceComm.j(ref A_2, num2, 30000) == 1;
				}
			}
		}
		return false;
	}

	private new bool a(y A_0, string A_1, byte[] A_2)
	{
		if (!a(A_0, ref A_1) || A_0 < global::y.a || A_0 == global::y.f || A_0 > global::y.j)
		{
			return false;
		}
		ulong A_3 = 0uL;
		if (!base.ab)
		{
			byte[] A_4 = new byte[32];
			byte[] bytes = BitConverter.GetBytes((int)A_0);
			byte[] bytes2 = BitConverter.GetBytes((A_2 != null) ? A_2.Length : (-1));
			byte[] bytes3 = Encoding.ASCII.GetBytes(A_1);
			Array.Copy(bytes, A_4, 4);
			Array.Copy(bytes2, 0, A_4, 4, 4);
			Array.Copy(bytes3, 0, A_4, 8, bytes3.Length);
			if (DeviceComm.g(global::k.bn) && DeviceComm.a(ref A_3) == global::j.a.b && DeviceComm.i(ref A_4, A_4.Length) == 1 && DeviceComm.a(ref A_3) == global::j.a.b)
			{
				if (A_2 == null)
				{
					return true;
				}
				if (A_2 != null && DeviceComm.i(ref A_2, A_2.Length) == 1)
				{
					Thread.Sleep(15000);
					return DeviceComm.m(ref A_3, 30000) == global::j.a.b;
				}
			}
			return false;
		}
		bool flag = false;
		if (A_0 == global::y.h || A_0 == global::y.i || A_0 == global::y.j)
		{
			flag = true;
		}
		if (!flag)
		{
			return false;
		}
		int num = a(A_0, A_1, A_2.Length, A_3: false);
		if (A_0 == global::y.h)
		{
			byte[] A_5 = new byte[A_2.Length - 4];
			Array.Copy(A_2, 4, A_5, 0, A_2.Length - 4);
			if (DeviceComm.i(ref A_5, A_5.Length) == 1)
			{
				int a_ = (base.y ? 30000 : 15000);
				return DeviceComm.m(ref A_3, a_) == global::j.a.b;
			}
		}
		else
		{
			int num3;
			for (int num2 = 0; num2 < num - 4; num2 += num3)
			{
				num3 = num - 4 - num2;
				if (num3 > 4096)
				{
					num3 = 4096;
				}
				byte[] A_6 = new byte[A_2.Length - 4 - num2];
				Array.Copy(A_2, 4, A_6, 0, A_2.Length - 4 - num2);
				if (DeviceComm.i(ref A_6, A_6.Length) == 1)
				{
					return DeviceComm.m(ref A_3, 30000) == global::j.a.b;
				}
			}
		}
		return false;
	}

	private new bool a(int A_0, ref int A_1, ref byte[] A_2)
	{
		if (base.ab || A_0 < 0 || A_1 < 0 || A_1 > 1024)
		{
			return false;
		}
		byte[] A_3 = new byte[8];
		byte[] bytes = BitConverter.GetBytes(A_0);
		byte[] bytes2 = BitConverter.GetBytes(A_1);
		Array.Copy(bytes, A_3, 4);
		Array.Copy(bytes2, 0, A_3, 4, 4);
		ulong A_4 = 0uL;
		if (DeviceComm.g(global::k.bs) && DeviceComm.a(ref A_4) == global::j.a.b && DeviceComm.i(ref A_3, A_3.Length) == 1 && DeviceComm.a(ref A_4) == global::j.a.b)
		{
			if (A_1 > 0 && A_4 != 0)
			{
				A_2 = new byte[A_4 * 24];
				return DeviceComm.t(ref A_2, A_2.Length) == 1;
			}
			A_1 = (int)A_4;
			return true;
		}
		return false;
	}

	private new bool a(global::f A_0, ref ulong A_1, ref ulong A_2)
	{
		byte[] A_3 = new byte[8];
		ulong A_4 = 0uL;
		byte[] A_5 = BitConverter.GetBytes((int)A_0);
		if (DeviceComm.g(global::k.bg) && DeviceComm.a(ref A_4) == global::j.a.b && DeviceComm.i(ref A_5, A_5.Length) == 1 && DeviceComm.t(ref A_3, A_3.Length) == 1)
		{
			A_2 = (ulong)BitConverter.ToInt32(A_3, 0);
			A_1 = A_2 - (ulong)BitConverter.ToInt32(A_3, 4);
			return true;
		}
		return false;
	}

	protected new virtual bool a(bool A_0, ref List<Record> A_1)
	{
		byte[] A_2 = null;
		bool flag = a(global::f.a, A_0, ref A_2);
		if (!flag || A_2 == null)
		{
			return flag;
		}
		ulong A_3 = 0uL;
		ulong A_4 = 0uL;
		if (DeviceComm.g(global::k.bh) && DeviceComm.a(ref A_3) == global::j.a.b && DeviceComm.i(ref A_2, A_2.Length) == 1)
		{
			flag = DeviceComm.a(ref A_4) == global::j.a.b;
		}
		if (flag && A_4 != 0)
		{
			flag = false;
			int num = (base.ab ? 12 : 24);
			int num2 = num * (int)A_4;
			byte[] A_5 = new byte[num2];
			if (DeviceComm.t(ref A_5, num2) == 1 && DeviceComm.n(0u))
			{
				flag = true;
				for (int num3 = 0; num3 < num2; num3++)
				{
					if (num3 % num != 0)
					{
						continue;
					}
					byte[] array = new byte[num];
					Array.Copy(A_5, num3, array, 0, num);
					Record record = new Record();
					record.DN = DeviceComm.n().DN;
					DateTime A_6 = default(DateTime);
					if (base.y)
					{
						DeviceConnection.e e2 = global::u.BytesToStruct<DeviceConnection.e>(array, 0, num);
						a(BitConverter.GetBytes((int)e2.a), ref A_6);
						record.MDIN = e2.b;
						record.DIN = e2.d;
						record.Action = e2.c;
						if (e2.c > 13)
						{
							record.Action = e2.c + 7;
						}
					}
					else if (base.z)
					{
						DeviceConnection.f f2 = global::u.BytesToStruct<DeviceConnection.f>(array, 0, num);
						a(BitConverter.GetBytes((int)f2.a), ref A_6);
						record.MDIN = f2.b;
						record.DIN = f2.e;
						record.Action = f2.c;
						if (f2.c > 13)
						{
							record.Action = f2.c + 7;
						}
					}
					else
					{
						g g2 = global::u.BytesToStruct<g>(array, 0, num);
						a(BitConverter.GetBytes((int)g2.a), ref A_6);
						record.MDIN = g2.b;
						record.DIN = g2.c;
						record.Action = g2.d;
					}
					record.Clock = A_6;
					A_1.Add(record);
				}
			}
		}
		return flag;
	}

	protected new bool a(global::f A_0, bool A_1, ref byte[] A_2)
	{
		A_2 = null;
		ulong A_3 = 0uL;
		ulong A_4 = 0uL;
		ulong num = 1uL;
		bool result = a(A_0, ref A_4, ref A_3);
		ulong num2 = (A_1 ? (A_3 - A_4) : 0);
		ulong num3 = A_3 - num2;
		if (num3 > A_3 || num3 < 1)
		{
			return result;
		}
		A_2 = new byte[12];
		byte[] bytes = BitConverter.GetBytes((int)num2);
		byte[] bytes2 = BitConverter.GetBytes((int)num3);
		byte[] bytes3 = BitConverter.GetBytes((int)num);
		Array.Copy(bytes, A_2, 4);
		Array.Copy(bytes2, 0, A_2, 4, 4);
		Array.Copy(bytes3, 0, A_2, 8, 4);
		return result;
	}

	protected new virtual bool b(bool A_0, ref List<Record> A_1)
	{
		byte[] A_2 = null;
		bool flag = a(global::f.b, A_0, ref A_2);
		if (!flag || A_2 == null)
		{
			return flag;
		}
		ulong A_3 = 0uL;
		ulong A_4 = 0uL;
		if (DeviceComm.g(global::k.bi) && DeviceComm.a(ref A_3) == global::j.a.b && DeviceComm.i(ref A_2, A_2.Length) == 1)
		{
			flag = DeviceComm.a(ref A_4) == global::j.a.b;
		}
		if (flag && A_4 != 0)
		{
			flag = false;
			int num = (base.ab ? 8 : 16);
			int num2 = num * (int)A_4;
			byte[] A_5 = new byte[num2];
			if (DeviceComm.t(ref A_5, num2) == 1 && DeviceComm.n(0u))
			{
				flag = true;
				for (int num3 = 0; num3 < num2; num3++)
				{
					if (num3 % num == 0)
					{
						byte[] array = new byte[num];
						Array.Copy(A_5, num3, array, 0, num);
						Record record = new Record();
						record.DN = DeviceComm.n().DN;
						DateTime A_6 = default(DateTime);
						if (base.ab)
						{
							DeviceConnection.g g2 = global::u.BytesToStruct<DeviceConnection.g>(array, 0, num);
							a(BitConverter.GetBytes((int)g2.a), ref A_6);
							record.DIN = g2.b;
							record.Verify = g2.d;
						}
						else
						{
							c c2 = global::u.BytesToStruct<c>(array, 0, num);
							a(BitConverter.GetBytes(c2.a), ref A_6);
							record.Action = c2.d;
							record.Verify = c2.e[0];
							record.DIN = c2.b;
						}
						record.Clock = A_6;
						A_1.Add(record);
					}
				}
			}
		}
		return flag;
	}

	private new bool a(long A_0)
	{
		ulong A_ = 0uL;
		byte[] A_2 = BitConverter.GetBytes((int)A_0 & 7);
		if (DeviceComm.g(global::k.bq) && DeviceComm.a(ref A_) == global::j.a.b && DeviceComm.i(ref A_2, A_2.Length) == 1)
		{
			return DeviceComm.a(ref A_) == global::j.a.b;
		}
		return false;
	}

	protected new virtual bool a(ref List<User> A_0)
	{
		bool flag = false;
		ulong A_ = 0uL;
		ulong A_2 = 0uL;
		if (DeviceComm.g(global::k.bc) && DeviceComm.a(ref A_2) == global::j.a.b)
		{
			flag = DeviceComm.a(ref A_) == global::j.a.b;
		}
		if (flag && A_ != 0)
		{
			if (A_ > 10000)
			{
				A_ = 10000uL;
			}
			int num = 8 * (int)A_;
			byte[] A_3 = new byte[num];
			if (DeviceComm.t(ref A_3, num) == 1)
			{
				flag = true;
				for (int num2 = 0; num2 < num; num2++)
				{
					if (num2 % 8 != 0)
					{
						continue;
					}
					byte[] array = new byte[8];
					Array.Copy(A_3, num2, array, 0, 8);
					b b2 = global::u.BytesToStruct<b>(array, 0, 8);
					for (int num3 = 0; num3 < 24; num3++)
					{
						if ((num3 % 2 != 0 && !base.ab) || ((b2.b >> num3) & 1) == 0)
						{
							continue;
						}
						User user = null;
						foreach (User item in A_0)
						{
							if (item.DIN == b2.a)
							{
								user = item;
								break;
							}
						}
						if (user == null)
						{
							User user2 = new User();
							user2.DIN = b2.a;
							user2.Privilege = (int)((b2.b >> 28) & 7);
							if (!base.ab)
							{
								user2.Privilege = ((user2.Privilege != 1) ? (user2.Privilege - 5) : 0);
							}
							user2.DIN = b2.a;
							user2.Enable = ((b2.b >> 31) & 1) == 0;
							if (base.ab)
							{
								user2.Enable = ((b2.b >> 31) & 1) != 0;
							}
							user2.AccessTimeZone = (int)((b2.b >> 24) & 1);
							user2.UserGroup = (int)((b2.b >> 25) & 1);
							A_0.Add(user2);
							user = user2;
						}
						Enroll enroll = new Enroll();
						enroll.EnrollType = (EnrollType)(num3 / 2);
						if (base.ab)
						{
							enroll.EnrollType = (EnrollType)num3;
						}
						user.Enrolls.Add(enroll);
					}
				}
			}
		}
		return false;
	}

	protected new virtual bool c(byte[] A_0, int A_1)
	{
		ulong A_2 = 0uL;
		ulong num = 0uL;
		long num2 = 0L;
		if (BitConverter.ToUInt32(A_0, 0) == au)
		{
			num2 = 1L;
		}
		else if (BitConverter.ToUInt32(A_0, 0) == av)
		{
			num2 = 2L;
		}
		if (num2 == 0 || (num2 == 1 && A_1 > 16777216))
		{
			return false;
		}
		if (num2 == 2)
		{
			A_1 -= 4;
			if (A_1 > aw)
			{
				return false;
			}
			byte[] array = new byte[A_1];
			Array.Copy(A_0, 4, array, 0, A_1);
			A_0 = array;
		}
		if (A_1 == 0)
		{
			A_1 = 16777216;
		}
		ulong A_3 = 0uL;
		bool flag = false;
		if (DeviceComm.g(global::k.br) && DeviceComm.a(ref A_3) == global::j.a.b)
		{
			byte[] A_4 = BitConverter.GetBytes((uint)A_1);
			if (DeviceComm.i(ref A_4, A_4.Length) == 1)
			{
				flag = DeviceComm.a(ref A_2) == global::j.a.b;
			}
		}
		if (!flag)
		{
			return flag;
		}
		if (num2 == 1)
		{
			if (DeviceComm.i(ref A_0, A_0.Length) == 1)
			{
				flag = DeviceComm.m(ref A_3, 50000) == global::j.a.b;
			}
		}
		else if (num2 == 2)
		{
			for (; num < (ulong)A_1; num += A_2)
			{
				if (num + A_2 > (ulong)A_1)
				{
					A_2 = (ulong)A_1 - num;
				}
				byte[] A_5 = new byte[(uint)A_2];
				Array.Copy(A_0, (int)num, A_5, 0, (int)A_2);
				if (DeviceComm.i(ref A_5, A_5.Length) != 1)
				{
					return false;
				}
				if (DeviceComm.a(ref A_3) != global::j.a.b)
				{
					return false;
				}
			}
			Thread.Sleep(15000);
		}
		return flag;
	}

	protected new virtual bool d(ref byte[] A_0, bool A_1, byte[] A_2)
	{
		bool flag = false;
		if (DeviceComm.g(A_1 ? global::k.an : global::k.am))
		{
			ulong A_3 = 0uL;
			if (DeviceComm.a(ref A_3) == global::j.a.b)
			{
				if (A_1)
				{
					byte[] A_4 = new byte[4];
					if (DeviceComm.i(ref A_4, 4) == 1 && DeviceComm.i(ref A_0, A_0.Length) == 1)
					{
						flag = DeviceComm.a(ref A_3) == global::j.a.b;
					}
				}
				else
				{
					flag = DeviceComm.t(ref A_0, A_0.Length) == 1;
				}
			}
		}
		if (flag && A_2 != null)
		{
			bool flag2 = false;
			if (DeviceComm.g(global::k.@as))
			{
				ulong A_5 = 0uL;
				if (DeviceComm.a(ref A_5) == global::j.a.b)
				{
					byte[] A_6 = BitConverter.GetBytes(A_2.Length);
					if (DeviceComm.i(ref A_6, 4) == 1 && DeviceComm.a(ref A_5) == global::j.a.b && DeviceComm.i(ref A_2, A_2.Length) == 1)
					{
						flag2 = DeviceComm.a(ref A_5) == global::j.a.b;
					}
				}
			}
			if (!flag2)
			{
				A_0 = null;
			}
		}
		return flag;
	}
}
