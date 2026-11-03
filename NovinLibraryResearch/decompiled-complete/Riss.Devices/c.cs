using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Riss.Devices;

internal class c : DeviceConnection
{
	private new struct b
	{
		public uint a;

		public ushort b;

		public byte c;

		public byte d;
	}

	internal new struct a
	{
		public uint a;

		public ushort b;

		public ushort c;

		public byte d;

		public byte e;
	}

	private new struct e
	{
		public uint a;

		public byte b;

		public byte c;

		public byte d;

		public byte e;
	}

	private new struct d
	{
		public ushort a;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
		public byte[] b;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 96)]
		public byte[] c;
	}

	private new struct c
	{
		public ushort a;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 22)]
		public byte[] b;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 240)]
		public byte[] c;
	}

	private new const int m_a = 3;

	private new const int m_b = 2;

	private new const int m_c = 1;

	internal new const long d = 0L;

	private new const int m_e = 3584;

	private new const int m_f = 3839;

	private new const int m_g = 8;

	private new const int h = 20;

	private new const int i = 20;

	private new const int j = 10;

	private new const int k = 8;

	private new const uint l = 544768u;

	private uint[] m = new uint[46]
	{
		23u, 1u, 2u, 3u, 10u, 11u, 12u, 13u, 14u, 20u,
		21u, 4u, 5u, 6u, 7u, 22u, 0u, 24u, 25u, 26u,
		27u, 40u, 41u, 42u, 43u, 44u, 45u, 46u, 47u, 48u,
		49u, 50u, 51u, 52u, 53u, 54u, 55u, 56u, 57u, 58u,
		59u, 60u, 61u, 62u, 63u, 64u
	};

	private int[] n = new int[42]
	{
		4, 1, 2, 3, 10, 11, 12, 13, 14, 20,
		21, 15, 23, 24, 25, 26, 27, 40, 41, 42,
		43, 44, 45, 46, 47, 48, 49, 50, 51, 52,
		53, 54, 55, 56, 57, 58, 59, 60, 61, 62,
		63, 64
	};

	public override int Open()
	{
		DeviceComm.a(base.Device);
		int num = DeviceComm.s();
		y = num == 10512;
		z = num == 10514;
		aa = num == 17967458;
		ab = y | z | aa;
		if (num <= 0)
		{
			return 0;
		}
		return 100;
	}

	public override void Close()
	{
		if (DeviceComm != null)
		{
			DeviceComm.q();
		}
	}

	protected override bool EnableDevice(long bEnable)
	{
		BitConverter.GetBytes((int)bEnable);
		bool flag = ((bEnable == 0) ? (DeviceComm.e(global::k.m, 0uL, 0uL) == 1) : (DeviceComm.e(global::k.n, 0uL, 0uL) == 1));
		if (flag)
		{
			ulong A_ = 0uL;
			ulong A_2 = 0uL;
			flag = DeviceComm.h(ref A_, ref A_2) == 1 && A_ != 0;
		}
		return flag;
	}

	public override bool SetProperty(UserProperty property, object extraProperty, User user, object extraData)
	{
		bool result = false;
		switch (property)
		{
		case UserProperty.UserName:
		{
			bool a_ = (bool)extraProperty;
			result = a(user, a_);
			break;
		}
		case UserProperty.Enroll:
			if (user.Enrolls.Count > 0)
			{
				result = (((bool)extraProperty) ? b(user) : a(user.Enrolls[0]));
			}
			break;
		case UserProperty.AccessControlSettings:
			result = a(user);
			break;
		case UserProperty.Privilege:
			if (user.Enrolls.Count > 0)
			{
				result = c(user);
			}
			break;
		}
		return result;
	}

	public override bool GetProperty(UserProperty property, object extraProperty, ref User user, ref object extraData)
	{
		bool result = false;
		switch (property)
		{
		case UserProperty.UserName:
		{
			bool a_ = (bool)extraProperty;
			result = a(ref user, a_);
			break;
		}
		case UserProperty.Enroll:
			if (user.Enrolls.Count > 0)
			{
				result = b(ref user);
			}
			break;
		case UserProperty.AccessControlSettings:
		{
			Enroll enroll = new Enroll();
			enroll.DIN = user.DIN;
			result = a(ref user);
			break;
		}
		case UserProperty.Privilege:
			if (user.Enrolls.Count > 0)
			{
				result = c(ref user);
			}
			break;
		}
		return result;
	}

	public override bool SetProperty(DeviceProperty property, object extraProperty, Device device, object extraData)
	{
		bool result = false;
		switch (property)
		{
		case DeviceProperty.FirmwareUpgrade:
		{
			byte[] array = (byte[])extraData;
			result = a(array, array.Length);
			break;
		}
		case DeviceProperty.Bell:
		{
			byte[] a_7 = (byte[])extraData;
			result = e(a_7);
			break;
		}
		case DeviceProperty.DoorControl:
			result = a((DoorStatus)extraData);
			break;
		case DeviceProperty.AccessControlSettings:
			switch ((AccessContorlCommand)extraProperty)
			{
			case AccessContorlCommand.PassTime:
			{
				List<DateTime> a_6 = (List<DateTime>)extraData;
				result = a(a_6);
				break;
			}
			case AccessContorlCommand.GroupTime:
			{
				byte[] a_5 = (byte[])extraData;
				result = d(a_5);
				break;
			}
			case AccessContorlCommand.TimeZone:
			{
				byte[] a_4 = (byte[])extraData;
				result = c(a_4);
				break;
			}
			case AccessContorlCommand.LockGroup:
			{
				byte[] a_3 = (byte[])extraData;
				result = b(a_3);
				break;
			}
			case AccessContorlCommand.DoorKey:
			{
				byte[] a_2 = (byte[])extraData;
				result = a(a_2);
				break;
			}
			}
			break;
		case DeviceProperty.InitSettings:
		{
			byte[] A_ = (byte[])extraData;
			result = a(ref A_, A_1: true);
			break;
		}
		case DeviceProperty.PowerOff:
			result = b();
			break;
		case DeviceProperty.DeviceTime:
			result = a();
			break;
		case DeviceProperty.ScreenBrightness:
		{
			ulong a_ = Convert.ToUInt64(extraData.ToString());
			result = a(a_);
			break;
		}
		case DeviceProperty.ManagementRecords:
			result = d();
			break;
		case DeviceProperty.AttRecords:
			result = e();
			break;
		case DeviceProperty.Enrolls:
			result = c();
			break;
		case DeviceProperty.Enable:
		{
			long bEnable = Convert.ToInt64(extraData.ToString());
			result = EnableDevice(bEnable);
			break;
		}
		case DeviceProperty.WelcomeTitle:
			result = a(1, (string)extraData);
			break;
		}
		return result;
	}

	public override bool GetProperty(DeviceProperty property, object extraProperty, ref Device device, ref object extraData)
	{
		bool result = false;
		switch (property)
		{
		case DeviceProperty.FirmwareVersion:
		{
			string A_20 = string.Empty;
			result = a(ref A_20);
			extraData = A_20;
			break;
		}
		case DeviceProperty.Bell:
		{
			byte[] A_19 = new byte[40];
			result = e(ref A_19);
			extraData = A_19;
			break;
		}
		case DeviceProperty.DoorControl:
		{
			DoorStatus A_18 = DoorStatus.Unidefind;
			result = a(ref A_18);
			extraData = A_18;
			break;
		}
		case DeviceProperty.AccessControlSettings:
			switch ((AccessContorlCommand)extraProperty)
			{
			case AccessContorlCommand.PassTime:
			{
				List<DateTime> A_17 = new List<DateTime>();
				result = a(ref A_17);
				extraData = A_17;
				break;
			}
			case AccessContorlCommand.GroupTime:
			{
				byte[] A_16 = new byte[320];
				result = d(ref A_16);
				extraData = A_16;
				break;
			}
			case AccessContorlCommand.TimeZone:
			{
				byte[] A_15 = new byte[224];
				result = c(ref A_15);
				extraData = A_15;
				break;
			}
			case AccessContorlCommand.LockGroup:
			{
				byte[] A_14 = new byte[40];
				result = b(ref A_14);
				extraData = A_14;
				break;
			}
			case AccessContorlCommand.DoorKey:
			{
				byte[] A_13 = new byte[1];
				result = a(ref A_13);
				extraData = A_13;
				break;
			}
			case AccessContorlCommand.LogWatch:
			{
				Record A_12 = new Record();
				result = a(ref A_12);
				extraData = A_12;
				break;
			}
			}
			break;
		case DeviceProperty.WelcomeTitle:
		{
			string A_11 = string.Empty;
			result = b(ref A_11);
			extraData = A_11;
			break;
		}
		case DeviceProperty.InitSettings:
		{
			int num = (int)extraProperty;
			byte[] A_10 = new byte[num];
			result = a(ref A_10, A_1: false);
			extraData = A_10;
			break;
		}
		case DeviceProperty.Status:
		{
			ulong A_9 = 0uL;
			n a_3 = (n)extraProperty;
			result = a(a_3, ref A_9);
			extraData = BitConverter.GetBytes((int)A_9);
			break;
		}
		case DeviceProperty.DeviceTime:
		{
			DateTime A_8 = DateTime.Now;
			result = a(ref A_8);
			extraData = A_8;
			break;
		}
		case DeviceProperty.ManagementRecords:
		{
			List<Record> A_7 = new List<Record>();
			bool a_2 = (bool)extraProperty;
			result = b(a_2, ref A_7);
			extraData = A_7;
			break;
		}
		case DeviceProperty.AttRecords:
		{
			bool a_ = (bool)extraProperty;
			List<Record> A_6 = new List<Record>();
			result = a(a_, ref A_6);
			extraData = A_6;
			break;
		}
		case DeviceProperty.Enrolls:
		{
			List<User> A_5 = new List<User>();
			result = a(ref A_5);
			extraData = A_5;
			break;
		}
		case DeviceProperty.ManagementRecordsCount:
		{
			ulong A_3 = 0uL;
			ulong A_4 = 0uL;
			result = a(global::f.a, ref A_3, ref A_4);
			byte[] array2 = new byte[8];
			Array.Copy(BitConverter.GetBytes((int)A_3), array2, 4);
			Array.Copy(BitConverter.GetBytes((int)A_4), 0, array2, 4, 4);
			extraData = array2;
			break;
		}
		case DeviceProperty.AttRecordsCount:
		{
			ulong A_ = 0uL;
			ulong A_2 = 0uL;
			result = a(global::f.b, ref A_, ref A_2);
			byte[] array = new byte[8];
			Array.Copy(BitConverter.GetBytes((int)A_), array, 4);
			Array.Copy(BitConverter.GetBytes((int)A_2), 0, array, 4, 4);
			extraData = array;
			break;
		}
		}
		return result;
	}

	private new bool b(bool A_0, ref List<Record> A_1)
	{
		ulong A_2 = 0uL;
		ulong A_3 = 0uL;
		ulong A_4 = 0uL;
		ulong A_5 = 0uL;
		ulong A_6 = 0uL;
		int num = 0;
		int num2 = 0;
		byte[] A_7 = null;
		bool flag = false;
		flag = a(global::f.a, ref A_6, ref A_4);
		if (flag && A_4 != 0)
		{
			ulong num3 = (A_0 ? A_6 : A_4);
			ulong a_ = (A_0 ? (A_4 - num3 + 1) : 1);
			if (num3 != 0)
			{
				flag = false;
				if (DeviceComm.e(global::k.g, a_, num3) == 1 && DeviceComm.h(ref A_2, ref A_5) == 1 && A_2 != 0)
				{
					num2 = ((!ab) ? Marshal.SizeOf(typeof(a)) : (z ? Marshal.SizeOf(typeof(f)) : Marshal.SizeOf(typeof(DeviceConnection.e))));
					num = num2 * (int)A_5;
					A_7 = new byte[num];
					if (DeviceComm.l(ref A_7, num) == 1)
					{
						if (ab)
						{
							byte[] A_8 = BitConverter.GetBytes((int)A_5);
							if (DeviceComm.i(ref A_8, 4) != 1)
							{
								return false;
							}
						}
						flag = DeviceComm.h(ref A_2, ref A_3) == 1 && A_2 != 0 && A_2 != 0;
					}
				}
			}
		}
		if (A_7 != null)
		{
			int[] array = null;
			if (ab)
			{
				array = new int[28];
				for (int num4 = 0; num4 <= 27; num4++)
				{
					array[num4] = 255;
				}
				array[23] = 3;
				array[7] = 3;
				array[6] = 3;
				array[8] = 3;
				array[10] = 5;
				array[11] = 6;
				array[12] = 7;
				array[13] = 8;
				array[5] = 9;
				array[14] = 10;
			}
			else
			{
				int[] array2 = new int[16]
				{
					0, 3, 3, 3, 4, 4, 4, 5, 6, 7,
					8, 9, 10, 11, 12, 13
				};
				array = array2;
			}
			for (int num5 = 0; num5 < num; num5++)
			{
				if (num5 % num2 != 0)
				{
					continue;
				}
				byte[] array3 = new byte[num2];
				Array.Copy(A_7, num5, array3, 0, num2);
				Record record = new Record();
				record.DN = DeviceComm.n().DN;
				DateTime A_9 = default(DateTime);
				if (y)
				{
					DeviceConnection.e e2 = global::u.BytesToStruct<DeviceConnection.e>(array3, 0, num2);
					a(BitConverter.GetBytes((int)e2.a), ref A_9);
					record.MDIN = e2.b;
					record.DIN = e2.d;
					record.Verify = e2.e;
					if (e2.c == 7 || e2.c == 11)
					{
						record.Verify = 10;
					}
					if (e2.c == 8 || e2.c == 12)
					{
						record.Verify = 13;
					}
					record.Action = array[e2.c];
				}
				else if (z)
				{
					f f2 = global::u.BytesToStruct<f>(array3, 0, num2);
					a(BitConverter.GetBytes((int)f2.a), ref A_9);
					record.MDIN = f2.b;
					record.DIN = f2.e;
					record.Verify = f2.d;
					if (f2.c == 7 || f2.c == 11)
					{
						record.Verify = 10;
					}
					if (f2.c == 8 || f2.c == 12)
					{
						record.Verify = 13;
					}
					record.Action = array[f2.c];
				}
				else
				{
					a a2 = global::u.BytesToStruct<a>(array3, 0, num2);
					a(BitConverter.GetBytes((int)a2.a), ref A_9);
					record.DIN = a2.c;
					record.Verify = a2.d;
					record.MDIN = a2.b;
				}
				record.Clock = A_9;
				A_1.Add(record);
			}
		}
		return flag;
	}

	private new bool a(global::f A_0, ref ulong A_1, ref ulong A_2)
	{
		ulong A_3 = 0uL;
		byte[] A_4 = new byte[4];
		bool result = false;
		k a_ = ((A_0 == global::f.b) ? global::k.h : global::k.f);
		if (DeviceComm.e(a_, 0uL, 0uL) == 1 && DeviceComm.h(ref A_3, ref A_2) == 1 && A_3 != 0)
		{
			result = DeviceComm.t(ref A_4, 4) == 1;
			ulong num = (ulong)BitConverter.ToInt32(A_4, 0);
			A_1 = A_2 - num;
		}
		return result;
	}

	private new bool a(bool A_0, ref List<Record> A_1)
	{
		ulong A_2 = 0uL;
		ulong A_3 = 0uL;
		ulong A_4 = 0uL;
		ulong A_5 = 0uL;
		ulong A_6 = 0uL;
		bool flag = false;
		int num = 0;
		byte[] A_7 = null;
		int num2 = 8;
		if (aa)
		{
			num2 = 9;
		}
		flag = a(global::f.b, ref A_6, ref A_4);
		if (flag && A_4 != 0)
		{
			ulong num3 = (A_0 ? A_6 : A_4);
			ulong num4 = (A_0 ? (A_4 - num3 + 1) : 1);
			if (num3 != 0 && DeviceComm.e(global::k.i, 1uL, num3) == 1)
			{
				byte[] A_8 = BitConverter.GetBytes((int)num4);
				if (DeviceComm.i(ref A_8, 4) == 1 && DeviceComm.h(ref A_2, ref A_5) == 1 && A_2 != 0)
				{
					num = num2 * (int)A_5;
					A_7 = new byte[num];
					if (DeviceComm.l(ref A_7, num) == 1)
					{
						if (ab)
						{
							byte[] A_9 = BitConverter.GetBytes((int)A_5);
							if (DeviceComm.i(ref A_9, 4) != 1)
							{
								return false;
							}
						}
						flag = DeviceComm.h(ref A_2, ref A_3) == 1 && A_2 != 0 && A_2 != 0;
					}
				}
			}
		}
		if (A_7 != null)
		{
			uint[] array = new uint[10] { 15u, 23u, 24u, 25u, 27u, 26u, 46u, 47u, 1u, 1u };
			for (int num5 = 0; num5 < num; num5++)
			{
				if (num5 % num2 != 0)
				{
					continue;
				}
				Record record = new Record();
				record.DN = DeviceComm.n().DN;
				DateTime A_10 = default(DateTime);
				byte[] array2 = new byte[num2];
				Array.Copy(A_7, num5, array2, 0, num2);
				if (ab)
				{
					g g2 = global::u.BytesToStruct<g>(array2, 0, num2);
					a(BitConverter.GetBytes((int)g2.a), ref A_10);
					if (aa)
					{
						uint num6 = BitConverter.ToUInt32(array2, 4);
						num6 &= 0x7FFFFFFFu;
						byte b2 = array2[num2 - 1];
						record.DIN = num6;
						record.Verify = (int)array[(b2 & 0xF0) / 16];
					}
					else
					{
						record.DIN = g2.b;
						record.Verify = (int)array[g2.c];
					}
				}
				else
				{
					b b3 = global::u.BytesToStruct<b>(array2, 0, num2);
					a(BitConverter.GetBytes((int)b3.a), ref A_10);
					record.DIN = b3.b;
					record.Verify = (int)((A_3 != 1000) ? n[b3.c] : (m[b3.c & 0xF] + (b3.c & 0xF0) * 16 + b3.d * 256 * 256));
				}
				record.Clock = A_10;
				A_1.Add(record);
			}
			flag = true;
		}
		return flag;
	}

	private new bool e()
	{
		ulong A_ = 0uL;
		ulong A_2 = 0uL;
		if (DeviceComm.e(global::k.z, 0uL, 0uL) == 1 && DeviceComm.h(ref A_, ref A_2) == 1 && A_ != 0)
		{
			return true;
		}
		return false;
	}

	private new bool d()
	{
		ulong A_ = 0uL;
		ulong A_2 = 0uL;
		if (DeviceComm.e(global::k.aa, 0uL, 0uL) == 1 && DeviceComm.h(ref A_, ref A_2) == 1 && A_ != 0)
		{
			return true;
		}
		return false;
	}

	private new bool a(ref List<User> A_0)
	{
		ulong A_ = 0uL;
		ulong A_2 = 0uL;
		ulong A_3 = 0uL;
		if (DeviceComm.e(global::k.t, 0uL, 0uL) == 1 && DeviceComm.h(ref A_, ref A_2) == 1)
		{
			if (A_2 == 0)
			{
				return true;
			}
			if (DeviceComm.e(global::k.t, 1uL, A_2) == 1 && DeviceComm.h(ref A_, ref A_3) == 1 && A_ != 0)
			{
				int num = Marshal.SizeOf(typeof(e));
				int num2 = num * (int)A_3;
				byte[] A_4 = new byte[num2];
				if (DeviceComm.l(ref A_4, num2) == 1)
				{
					for (int num3 = 0; num3 < num2; num3++)
					{
						if (num3 % num != 0)
						{
							continue;
						}
						byte[] array = new byte[num];
						Array.Copy(A_4, num3, array, 0, num);
						e e2 = global::u.BytesToStruct<e>(array, 0, num);
						e2.e = (((e2.e & 0x10u) != 0) ? ((byte)((e2.e & 0xF) + 1 + 256)) : ((byte)(e2.e + 1)));
						Enroll enroll = new Enroll();
						User user = null;
						foreach (User item in A_0)
						{
							if (item.DIN == e2.a)
							{
								user = item;
								break;
							}
						}
						if (user == null)
						{
							user = new User();
							user.DIN = e2.a;
							user.Privilege = e2.d;
							user.Enable = e2.e == 1;
							A_0.Add(user);
						}
						else if (e2.d > user.Privilege)
						{
							user.Privilege = e2.d;
						}
						enroll.EnrollType = (EnrollType)e2.c;
						user.Enrolls.Add(enroll);
					}
					return true;
				}
			}
		}
		return false;
	}

	private new bool c()
	{
		ulong A_ = 0uL;
		ulong A_2 = 0uL;
		if (DeviceComm.e(global::k.y, 0uL, 0uL) == 1 && DeviceComm.h(ref A_, ref A_2) == 1 && A_ != 0)
		{
			return true;
		}
		return false;
	}

	private new bool c(User A_0)
	{
		ulong A_ = 0uL;
		ulong A_2 = 0uL;
		ulong a_ = (ulong)((int)A_0.DIN | ((int)A_0.Enrolls[0].EnrollType << 28));
		if (DeviceComm.e(global::k.s, (ulong)A_0.Privilege, a_) == 1 && DeviceComm.h(ref A_, ref A_2) == 1 && A_ != 0)
		{
			return true;
		}
		return false;
	}

	private new bool c(ref User A_0)
	{
		ulong A_ = 0uL;
		ulong A_2 = 0uL;
		ulong a_ = (ulong)((int)A_0.DIN | ((int)A_0.Enrolls[0].EnrollType << 28));
		ulong a_2 = 1uL;
		if (A_0.Enrolls[0].EnrollType == EnrollType.Password)
		{
			a_2 = 2uL;
			a_ = A_0.DIN;
		}
		else if (A_0.Enrolls[0].EnrollType == EnrollType.Card)
		{
			a_2 = 3uL;
			a_ = A_0.DIN;
		}
		if (DeviceComm.e(global::k.c, a_2, a_) == 1 && DeviceComm.h(ref A_, ref A_2) == 1 && A_ != 0)
		{
			A_0.Privilege = (int)A_2;
			return true;
		}
		return false;
	}

	private new bool a(int A_0)
	{
		if (0 > A_0 || A_0 > 15 || A_0 == 12 || A_0 == 13)
		{
			return false;
		}
		return true;
	}

	private new bool b(ref User A_0)
	{
		Enroll enroll = A_0.Enrolls[0];
		bool result = false;
		bool flag = true;
		ulong A_ = 0uL;
		ulong A_2 = 0uL;
		ulong a_ = 0uL;
		ulong a_2 = A_0.DIN;
		byte[] A_3 = new byte[4];
		bool flag2 = false;
		EnrollType enrollType = enroll.EnrollType;
		switch (enrollType)
		{
		case EnrollType.Password:
			a_ = 2uL;
			break;
		case EnrollType.Card:
			a_ = 3uL;
			break;
		default:
			if (EnrollType.Finger0 <= enrollType && enrollType <= EnrollType.Finger9)
			{
				a_ = 1uL;
				a_2 = enroll.DIN | (ulong)((long)enroll.EnrollType << 28);
				A_3 = new byte[1416];
				flag2 = true;
			}
			else
			{
				flag = false;
			}
			break;
		}
		if (flag)
		{
			int a_3 = A_3.Length;
			if (DeviceComm.e(global::k.c, a_, a_2) == 1 && DeviceComm.h(ref A_, ref A_2) == 1 && A_ != 0)
			{
				A_0.Privilege = (int)A_2;
				result = (flag2 ? (DeviceComm.l(ref A_3, a_3) == 1) : (DeviceComm.t(ref A_3, a_3) == 1));
				if (flag2)
				{
					enroll.Fingerprint = g(A_3);
				}
				else
				{
					switch (enrollType)
					{
					case EnrollType.Password:
						if (base.Device.ConnectionModel == 2)
						{
							enroll.Password = global::u.a(BitConverter.ToInt32(A_3, 0));
							string[] array = enroll.Password.Split(new char[1] { '-' });
							string text = string.Empty;
							for (int num = 0; num < array.Length; num++)
							{
								if (Convert.ToUInt32(array[num]) > 29)
								{
									text += Convert.ToUInt32(array[num]) - 30;
								}
							}
							enroll.Password = text;
						}
						else
						{
							enroll.Password = BitConverter.ToInt32(A_3, 0).ToString();
						}
						break;
					case EnrollType.Card:
						enroll.CardID = BitConverter.ToInt32(A_3, 0).ToString();
						break;
					}
				}
			}
		}
		A_0.Enrolls[0] = enroll;
		return result;
	}

	private new byte[] f(byte[] A_0)
	{
		if (A_0.Length != 1404)
		{
			return new byte[1416];
		}
		List<byte> list = new List<byte>();
		int num = 0;
		for (int num2 = 0; num2 < 1404; num2++)
		{
			if (num2 % 4 == 0)
			{
				num += BitConverter.ToInt32(A_0, num2);
			}
		}
		byte[] bytes = Encoding.ASCII.GetBytes("SmackBio");
		list.AddRange(bytes);
		list.AddRange(A_0);
		list.AddRange(BitConverter.GetBytes(num));
		return list.ToArray();
	}

	public new byte[] g(byte[] A_0)
	{
		byte[] array = new byte[1404];
		Array.Copy(A_0, 8, array, 0, 1404);
		return array;
	}

	private new bool b(User A_0)
	{
		bool flag = false;
		bool flag2 = true;
		ulong A_ = 0uL;
		ulong A_2 = 0uL;
		ulong num = (ulong)((long)A_0.Privilege << 4);
		ulong a_ = A_0.DIN;
		byte[] A_3 = new byte[4];
		bool flag3 = false;
		EnrollType enrollType = A_0.Enrolls[0].EnrollType;
		Enroll enroll = A_0.Enrolls[0];
		switch (enrollType)
		{
		case EnrollType.Password:
			num = 2 | num;
			A_3 = ((base.Device.ConnectionModel == 2) ? BitConverter.GetBytes(global::u.a(enroll.Password)) : BitConverter.GetBytes(Convert.ToUInt32(enroll.Password)));
			break;
		case EnrollType.Card:
			num = 3 | num;
			A_3 = BitConverter.GetBytes(Convert.ToUInt32(enroll.CardID));
			break;
		default:
			if (EnrollType.Finger0 <= enrollType && enrollType <= EnrollType.Finger9)
			{
				a_ = enroll.DIN | (ulong)((long)enroll.EnrollType << 28);
				A_3 = f(enroll.Fingerprint);
				flag3 = true;
				num = 1 | num;
			}
			else
			{
				flag2 = false;
			}
			break;
		}
		if (flag2)
		{
			int a_2 = A_3.Length;
			if (DeviceComm.e(global::k.d, num, a_) == 1 && DeviceComm.h(ref A_, ref A_2) == 1 && A_ != 0)
			{
				flag = (flag3 ? (DeviceComm.k(ref A_3, a_2) == 1) : (DeviceComm.i(ref A_3, a_2) == 1));
				if (flag)
				{
					flag = DeviceComm.h(ref A_, ref A_2) == 1 && A_ != 0;
				}
			}
		}
		return flag;
	}

	private new bool a(Enroll A_0)
	{
		bool result = false;
		ulong A_ = 0uL;
		ulong A_2 = 0uL;
		ulong a_ = 1uL;
		ulong a_2 = A_0.DIN;
		bool flag = true;
		EnrollType enrollType = A_0.EnrollType;
		switch (enrollType)
		{
		case EnrollType.Password:
			a_ = 2uL;
			break;
		case EnrollType.Card:
			a_ = 3uL;
			break;
		case EnrollType.AllFinger:
			a_ = 4uL;
			break;
		case EnrollType.All:
			a_ = 5uL;
			break;
		default:
			if (EnrollType.Finger0 <= enrollType && enrollType <= EnrollType.Finger9)
			{
				a_2 = (ulong)((int)A_0.DIN | ((int)A_0.EnrollType << 28));
			}
			else
			{
				flag = false;
			}
			break;
		}
		if (flag && DeviceComm.e(global::k.e, a_, a_2) == 1 && DeviceComm.h(ref A_, ref A_2) == 1 && A_ != 0)
		{
			result = true;
		}
		return result;
	}

	private new bool a(ref User A_0, bool A_1)
	{
		if (y && A_1)
		{
			return false;
		}
		ulong dIN = A_0.DIN;
		_ = string.Empty;
		int num = 10;
		if (ab)
		{
			num = (y ? 24 : 50);
		}
		if (A_1)
		{
			num = 20;
		}
		if (aa)
		{
			num = 26;
		}
		ulong A_2 = 0uL;
		ulong A_3 = 0uL;
		byte[] A_4 = new byte[num];
		if (DeviceComm.e(global::k.ab, 0uL, dIN) == 1 && DeviceComm.h(ref A_2, ref A_3) == 1 && A_2 != 0 && DeviceComm.t(ref A_4, A_4.Length) == 1)
		{
			if (ab)
			{
				if (z)
				{
					byte[] array = new byte[num / 2];
					for (int num2 = 0; num2 < num; num2++)
					{
						if (num2 % 2 == 0)
						{
							array[num2 / 2] = A_4[num2];
						}
					}
					byte b2 = (byte)(A_4[^2] & 0xFFu);
					int charType = A_4[^1];
					byte[] array2 = new byte[b2];
					Array.Copy(array, array2, b2);
					A_0.UserName = GetDecodeName(array2, charType);
				}
				else
				{
					A_0.UserName = Encoding.Unicode.GetString(A_4).Replace("†", "").Trim();
				}
			}
			else if (A_4[^1] == 0)
			{
				User obj = A_0;
				string @string = Encoding.Default.GetString(A_4);
				char[] separator = new char[1];
				obj.UserName = @string.Split(separator)[0].Replace("†", "").Trim();
			}
			else
			{
				byte b3 = (byte)(A_4[^1] & 0xFu);
				int charType2 = A_4[^1] >> 4;
				byte[] array3 = new byte[b3];
				Array.Copy(A_4, array3, b3);
				A_0.UserName = GetDecodeName(array3, charType2);
			}
			return true;
		}
		return false;
	}

	private new bool a(User A_0, bool A_1)
	{
		if (y && A_1)
		{
			return false;
		}
		bool result = false;
		byte[] A_2 = null;
		if (!z && !aa)
		{
			c c2 = default(c);
			d d2 = default(d);
			int lenght = (y ? 24 : 8);
			d2.c = new byte[96];
			d2.a = (ushort)A_0.DIN;
			d2.b = new byte[10];
			if (A_1)
			{
				c2.c = new byte[240];
				c2.a = (ushort)A_0.DIN;
				c2.b = new byte[22];
				EncodeName(A_0.UserName, ref c2.b, ref c2.c, 20);
			}
			else
			{
				EncodeName(A_0.UserName, ref d2.b, ref d2.c, lenght);
			}
			int a_ = (A_1 ? Marshal.SizeOf(typeof(c)) : Marshal.SizeOf(typeof(d)));
			A_2 = (A_1 ? global::u.a(c2, a_) : global::u.a(d2, a_));
		}
		else if (z)
		{
			DeviceConnection.a a2 = default(DeviceConnection.a);
			a2.a = (ushort)A_0.DIN;
			a2.d = new ushort[26];
			a2.e = new byte[384];
			byte[] nameInfo = new byte[26];
			a2.c = EncodeName(A_0.UserName, ref nameInfo, ref a2.e, 26);
			for (int num = 0; num < 24; num++)
			{
				a2.d[num] = nameInfo[num];
			}
			a2.d[24] = (ushort)((nameInfo[24] << 8) | nameInfo[25]);
			a2.b = 0;
			if (a2.c <= 104)
			{
				a2.c = -2500;
			}
			A_2 = global::u.a(a2, Marshal.SizeOf(typeof(DeviceConnection.a)));
		}
		else if (aa)
		{
			DeviceConnection.c c3 = default(DeviceConnection.c);
			c3.a = (uint)A_0.DIN;
			c3.d = new byte[26];
			c3.e = new byte[384];
			byte[] nameInfo2 = new byte[26];
			c3.c = EncodeName(A_0.UserName, ref nameInfo2, ref c3.e, 26);
			for (int num2 = 0; num2 < 26; num2++)
			{
				c3.d[num2] = nameInfo2[num2];
			}
			c3.b = 0;
			if (c3.c <= 104)
			{
				c3.c = -2500;
			}
			A_2 = global::u.a(c3, Marshal.SizeOf(typeof(DeviceConnection.c)));
		}
		if (DeviceComm.e(global::k.ac, 0uL, 0uL) == 1)
		{
			if (aa)
			{
				if (DeviceComm.k(ref A_2, 422) == 1)
				{
					ulong A_3 = 0uL;
					ulong A_4 = 0uL;
					if (DeviceComm.h(ref A_3, ref A_4) == 1)
					{
						return A_3 != 0;
					}
					return false;
				}
			}
			else if (DeviceComm.k(ref A_2, A_2.Length) == 1)
			{
				ulong A_5 = 0uL;
				ulong A_6 = 0uL;
				if (DeviceComm.h(ref A_5, ref A_6) == 1)
				{
					return A_5 != 0;
				}
				return false;
			}
		}
		return result;
	}

	private new bool a(int A_0, string A_1)
	{
		if (z)
		{
			return false;
		}
		ulong A_2 = 0uL;
		ulong A_3 = 0uL;
		byte[] nameData = null;
		c c2 = default(c);
		int a_ = Marshal.SizeOf(typeof(c));
		c2.a = (ushort)A_0;
		Convert.ToUInt32(A_1[0]);
		if (A_0 == 1)
		{
			c2.b = new byte[22];
			c2.c = new byte[240];
			if (y)
			{
				nameData = new byte[64];
				EncodeName(A_1, ref c2.b, ref nameData, 62);
			}
			else
			{
				EncodeName(A_1, ref c2.b, ref c2.c, 20);
				nameData = global::u.a(c2, a_);
			}
		}
		if (DeviceComm.e(global::k.ae, 0uL, 0uL) == 1 && DeviceComm.k(ref nameData, nameData.Length) == 1 && DeviceComm.h(ref A_2, ref A_3) == 1 && A_2 != 0)
		{
			return true;
		}
		return false;
	}

	private new bool b(ref string A_0)
	{
		A_0 = "NOT SUPPORT";
		return false;
	}

	private new bool a(ulong A_0)
	{
		bool result = false;
		if (!ab && DeviceComm.e(global::k.aj, A_0, 0uL) == 1)
		{
			ulong A_ = 0uL;
			ulong A_2 = 0uL;
			if (DeviceComm.h(ref A_, ref A_2) == 1 && A_ != 0)
			{
				result = true;
			}
		}
		return result;
	}

	private new bool b()
	{
		bool result = false;
		ulong A_ = 0uL;
		ulong A_2 = 0uL;
		if (DeviceComm.e(global::k.r, 0uL, 0uL) == 1 && DeviceComm.h(ref A_2, ref A_) == 1 && A_2 != 0)
		{
			result = true;
		}
		return result;
	}

	private new bool a(ref string A_0)
	{
		byte[] A_ = new byte[32];
		ulong A_2 = 0uL;
		ulong A_3 = 0uL;
		bool result = false;
		if (DeviceComm.e(global::k.x, 0uL, 0uL) == 1 && DeviceComm.h(ref A_2, ref A_3) == 1 && A_2 != 0 && DeviceComm.t(ref A_, 32) == 1)
		{
			result = true;
			A_0 = Encoding.Default.GetString(A_).Replace("\0", "").Trim();
		}
		return result;
	}

	private new bool a()
	{
		bool result = false;
		DateTime now = DateTime.Now;
		byte[] A_ = a(now);
		if (DeviceComm.e(global::k.q, 0uL, 4uL) == 1 && DeviceComm.i(ref A_, 4) == 1)
		{
			ulong A_2 = 0uL;
			ulong A_3 = 0uL;
			if (DeviceComm.h(ref A_2, ref A_3) == 1 && A_2 != 0)
			{
				result = true;
			}
		}
		return result;
	}

	private new bool a(ref DateTime A_0)
	{
		bool result = false;
		byte[] A_ = new byte[4];
		if (DeviceComm.e(global::k.p, 0uL, 4uL) == 1 && DeviceComm.t(ref A_, 4) == 1)
		{
			ulong A_2 = 0uL;
			ulong A_3 = 0uL;
			if (DeviceComm.h(ref A_2, ref A_3) == 1 && A_2 != 0)
			{
				result = a(A_, ref A_0);
			}
		}
		return result;
	}

	private new bool e(ref byte[] A_0)
	{
		bool result = false;
		int num = (ab ? 10 : 8);
		int num2 = 4 * num;
		new List<DateTime>();
		byte[] A_ = new byte[num2];
		ulong A_2 = 0uL;
		ulong A_3 = 0uL;
		if (DeviceComm.e(global::k.ah, 0uL, 0uL) == 1 && DeviceComm.t(ref A_, num2) == 1 && DeviceComm.h(ref A_3, ref A_2) == 1 && A_3 != 0)
		{
			A_0 = new byte[num2 + 1];
			Array.Copy(A_, A_0, num2);
			A_0[num2] = (byte)A_2;
			if (ab)
			{
				A_0[num2] = (byte)((A_2 > 8) ? 8 : A_2);
			}
			result = true;
		}
		return result;
	}

	private new bool e(byte[] A_0)
	{
		int num = A_0[^1];
		int num2 = (ab ? 10 : 8);
		ulong A_ = 0uL;
		ulong A_2 = 0uL;
		ulong num3 = 1uL;
		if (32 > A_0.Length)
		{
			return false;
		}
		byte[] A_3 = new byte[num2 * 4];
		Array.Copy(A_0, 0, A_3, 0, A_0.Length - 1);
		num3 = (ulong)num;
		if (ab && (num3 < 1 || num3 > 8))
		{
			return false;
		}
		if (DeviceComm.e(global::k.ai, num3, 0uL) == 1 && DeviceComm.i(ref A_3, A_3.Length) == 1 && DeviceComm.h(ref A_, ref A_2) == 1 && A_ != 0)
		{
			return true;
		}
		return false;
	}

	private new bool a(n A_0, ref ulong A_1)
	{
		ulong A_2 = 0uL;
		bool result = false;
		if (A_0 == global::n.h || A_0 == global::n.i)
		{
			global::f a_ = ((A_0 == global::n.h) ? global::f.b : global::f.a);
			ulong A_3 = 0uL;
			ulong A_4 = 0uL;
			result = a(a_, ref A_3, ref A_4);
			A_1 = A_4;
		}
		else if (DeviceComm.e(global::k.j, (ulong)A_0, 0uL) == 1)
		{
			result = DeviceComm.h(ref A_2, ref A_1) == 1 && A_2 != 0;
		}
		return result;
	}

	private new bool a(ref DoorStatus A_0)
	{
		bool result = false;
		ulong A_ = 0uL;
		ulong A_2 = 0uL;
		if (DeviceComm.e(global::k.af, 0uL, 0uL) == 1 && DeviceComm.h(ref A_, ref A_2) == 1 && A_ != 0 && A_2 != 0)
		{
			A_0 = (DoorStatus)A_2;
			result = true;
		}
		return result;
	}

	private new bool a(DoorStatus A_0)
	{
		bool result = false;
		ulong a_ = (ulong)A_0;
		ulong A_ = 0uL;
		ulong A_2 = 0uL;
		if (DeviceComm.e(global::k.ag, a_, 0uL) == 1 && DeviceComm.h(ref A_, ref A_2) == 1 && A_ != 0)
		{
			result = true;
		}
		return result;
	}

	private new bool a(ref List<DateTime> A_0)
	{
		int num = 128;
		ulong A_ = 0uL;
		ulong A_2 = 0uL;
		byte[] A_3 = new byte[num];
		k a_ = (ab ? ((k)1618) : global::k.ay);
		if (DeviceComm.e(a_, 0uL, 0uL) == 1 && DeviceComm.t(ref A_3, num) == 1 && DeviceComm.h(ref A_, ref A_2) == 1 && A_ != 0)
		{
			for (int num2 = 0; num2 < num; num2++)
			{
				if ((num2 + 1) % 2 == 0)
				{
					DateTime dateTime = DateTime.Parse("1989-07-10 00:00:00");
					dateTime = ((!ab) ? dateTime.AddMinutes((int)A_3[num2 - 1]).AddHours((int)A_3[num2]) : dateTime.AddMinutes((int)A_3[num2]).AddHours((int)A_3[num2 - 1]));
					A_0.Add(dateTime);
				}
			}
			return true;
		}
		return false;
	}

	private new bool a(List<DateTime> A_0)
	{
		int num = 128;
		ulong A_ = 0uL;
		ulong A_2 = 0uL;
		byte[] A_3 = new byte[num];
		int num2 = 0;
		for (int num3 = 0; num3 < 62; num3++)
		{
			if (ab)
			{
				A_3[num2++] = (byte)A_0[num3].Hour;
				A_3[num2++] = (byte)A_0[num3].Minute;
			}
			else
			{
				A_3[num2++] = (byte)A_0[num3].Minute;
				A_3[num2++] = (byte)A_0[num3].Hour;
			}
		}
		k a_ = (ab ? ((k)1617) : global::k.ax);
		if (DeviceComm.e(a_, 0uL, 0uL) == 1 && DeviceComm.i(ref A_3, num) == 1 && DeviceComm.h(ref A_, ref A_2) == 1 && A_ != 0)
		{
			return true;
		}
		return false;
	}

	private new bool d(ref byte[] A_0)
	{
		int a_ = A_0.Length;
		ulong A_ = 0uL;
		ulong A_2 = 0uL;
		k a_2 = (ab ? ((k)1620) : global::k.a0);
		if (DeviceComm.e(a_2, 0uL, 0uL) == 1 && DeviceComm.t(ref A_0, a_) == 1 && DeviceComm.h(ref A_, ref A_2) == 1 && A_ != 0)
		{
			return true;
		}
		return false;
	}

	private new bool d(byte[] A_0)
	{
		if (A_0.Length != 320)
		{
			return false;
		}
		for (int num = 0; num < 10; num++)
		{
			A_0[num] = 0;
			A_0[num + 310] = 31;
		}
		int a_ = A_0.Length;
		ulong A_ = 0uL;
		ulong A_2 = 0uL;
		k a_2 = (ab ? ((k)1619) : global::k.az);
		if (DeviceComm.e(a_2, 0uL, 0uL) == 1 && DeviceComm.i(ref A_0, a_) == 1 && DeviceComm.h(ref A_, ref A_2) == 1 && A_ != 0)
		{
			return true;
		}
		return false;
	}

	private new bool c(ref byte[] A_0)
	{
		if (A_0.Length != 224)
		{
			return false;
		}
		ulong A_ = 0uL;
		ulong A_2 = 0uL;
		k a_ = (ab ? ((k)1622) : global::k.a2);
		if (DeviceComm.e(a_, 0uL, 0uL) == 1 && DeviceComm.t(ref A_0, A_0.Length) == 1 && DeviceComm.h(ref A_, ref A_2) == 1 && A_ != 0)
		{
			return true;
		}
		return false;
	}

	private new bool c(byte[] A_0)
	{
		if (A_0.Length != 224)
		{
			return false;
		}
		for (int num = 0; num < 7; num++)
		{
			A_0[num] = 0;
			A_0[num + 192] = 31;
		}
		int a_ = A_0.Length;
		ulong A_ = 0uL;
		ulong A_2 = 0uL;
		k a_2 = (ab ? ((k)1621) : global::k.a1);
		if (DeviceComm.e(a_2, 0uL, 0uL) == 1 && DeviceComm.i(ref A_0, a_) == 1 && DeviceComm.h(ref A_, ref A_2) == 1 && A_ != 0)
		{
			return true;
		}
		return false;
	}

	private new bool b(ref byte[] A_0)
	{
		if (ab || A_0.Length != 40)
		{
			return false;
		}
		ulong A_ = 0uL;
		ulong A_2 = 0uL;
		if (DeviceComm.e(global::k.bb, 0uL, 0uL) == 1 && DeviceComm.t(ref A_0, A_0.Length) == 1 && DeviceComm.h(ref A_, ref A_2) == 1 && A_ != 0)
		{
			return true;
		}
		return false;
	}

	private new bool b(byte[] A_0)
	{
		if (ab || A_0.Length != 40)
		{
			return false;
		}
		ulong A_ = 0uL;
		ulong A_2 = 0uL;
		if (DeviceComm.e(global::k.ba, 0uL, 0uL) == 1 && DeviceComm.i(ref A_0, A_0.Length) == 1 && DeviceComm.h(ref A_, ref A_2) == 1 && A_ != 0)
		{
			return true;
		}
		return false;
	}

	private new bool a(ref byte[] A_0)
	{
		ulong A_ = 0uL;
		ulong A_2 = 0uL;
		byte[] A_3 = new byte[20];
		k a_ = (ab ? ((k)1626) : global::k.a6);
		if (DeviceComm.e(a_, 0uL, 0uL) == 1 && DeviceComm.t(ref A_3, A_3.Length) == 1 && DeviceComm.h(ref A_, ref A_2) == 1 && A_ != 0)
		{
			A_0 = A_3;
			return true;
		}
		return false;
	}

	private new bool a(byte[] A_0)
	{
		if (A_0.Length != 20)
		{
			return false;
		}
		ulong A_ = 0uL;
		ulong A_2 = 0uL;
		k a_ = (ab ? ((k)1625) : global::k.a5);
		if (DeviceComm.e(a_, 0uL, 0uL) == 1 && DeviceComm.i(ref A_0, A_0.Length) == 1 && DeviceComm.h(ref A_, ref A_2) == 1 && A_ != 0)
		{
			return true;
		}
		return false;
	}

	private new bool a(ref User A_0)
	{
		ulong A_ = 0uL;
		ulong A_2 = 0uL;
		byte[] A_3 = new byte[14];
		k a_ = (ab ? ((k)1624) : global::k.a4);
		if (DeviceComm.e(a_, 0uL, A_0.DIN) == 1)
		{
			if (!ab)
			{
				if (DeviceComm.t(ref A_3, A_3.Length) == 1)
				{
					if (DeviceComm.h(ref A_, ref A_2) == 1 && A_ != 0)
					{
						A_0.AccessTimeZone = A_3[2] + (A_3[3] << 8);
						long num = A_3[4] + (A_3[5] << 8) + (A_3[6] << 16) + (A_3[7] << 24);
						if (num == 0)
						{
							A_0.ValidDate = DateTime.Parse("2000-01-01 0:0:0");
						}
						else
						{
							DateTime A_4 = DateTime.Now;
							byte[] array = new byte[4];
							Array.Copy(A_3, 8, array, 0, 4);
							a(array, ref A_4);
							A_0.ValidDate = A_4;
						}
						long num2 = A_3[8] + (A_3[9] << 8) + (A_3[10] << 16) + (A_3[11] << 24);
						if (num2 == 0)
						{
							A_0.InvalidDate = DateTime.Parse("2099-01-01 0:0:0");
						}
						else
						{
							DateTime A_5 = DateTime.Now;
							byte[] array2 = new byte[4];
							Array.Copy(A_3, 4, array2, 0, 4);
							a(array2, ref A_5);
							A_0.InvalidDate = A_5;
						}
						A_0.UserGroup = A_3[12] + (A_3[13] << 8);
					}
					return true;
				}
			}
			else if (DeviceComm.h(ref A_, ref A_2) == 1 && A_ != 0)
			{
				try
				{
					A_0.AccessTimeZone = (int)A_2;
				}
				catch (Exception ex)
				{
					throw ex;
				}
				return true;
			}
		}
		return false;
	}

	private new bool a(User A_0)
	{
		ulong A_ = 0uL;
		ulong A_2 = 0uL;
		byte[] A_3 = new byte[14]
		{
			(byte)(A_0.DIN & 0xFF),
			(byte)((A_0.DIN & 0xFF00) >> 8),
			(byte)((uint)A_0.AccessTimeZone & 0xFFu),
			(byte)((A_0.AccessTimeZone & 0xFF00) >> 8),
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0
		};
		byte[] array = new byte[4];
		array = a(A_0.ValidDate);
		A_3[4] = array[0];
		A_3[5] = array[1];
		A_3[6] = array[2];
		A_3[7] = array[3];
		byte[] array2 = new byte[4];
		array2 = a(A_0.InvalidDate);
		A_3[8] = array2[0];
		A_3[9] = array2[1];
		A_3[10] = array2[2];
		A_3[11] = array2[3];
		A_3[12] = (byte)((uint)A_0.UserGroup & 0xFFu);
		A_3[13] = (byte)((A_0.UserGroup & 0xFF00) >> 8);
		k a_ = (ab ? ((k)1623) : global::k.a3);
		if (ab)
		{
			if (DeviceComm.e(a_, (ulong)A_0.AccessTimeZone, A_0.DIN) == 1 && DeviceComm.h(ref A_, ref A_2) == 1 && A_ != 0)
			{
				return true;
			}
		}
		else if (DeviceComm.e(a_, 0uL, A_0.DIN) == 1 && DeviceComm.i(ref A_3, A_3.Length) == 1 && DeviceComm.h(ref A_, ref A_2) == 1 && A_ != 0)
		{
			return true;
		}
		return false;
	}

	private new bool a(ref Record A_0)
	{
		if (ab)
		{
			return false;
		}
		ulong A_ = 0uL;
		ulong A_2 = 0uL;
		byte[] A_3 = new byte[12];
		if (DeviceComm.e(global::k.a7, 0uL, 0uL) == 1 && DeviceComm.t(ref A_3, A_3.Length) == 1 && DeviceComm.h(ref A_, ref A_2) == 1 && A_ != 0)
		{
			DateTime A_4 = default(DateTime);
			byte[] array = new byte[4];
			Array.Copy(A_3, array, 4);
			a(array, ref A_4);
			A_0.DN = DeviceComm.n().DN;
			A_0.Clock = A_4;
			A_0.DIN = (ulong)(A_3[4] + (A_3[5] << 8));
			A_0.Verify = n[A_3[6]];
			A_0.Action = A_3[7];
			A_0.DoorStatus = A_3[8] + (A_3[9] << 8) + (A_3[10] << 16) + (A_3[11] << 24);
			if ((A_0.DIN != 0 || A_0.Clock.Second > 0) && DeviceComm.e(global::k.a7, 1uL, A_2) == 1 && DeviceComm.h(ref A_, ref A_2) == 1 && A_ != 0)
			{
				return true;
			}
			return false;
		}
		return false;
	}

	private new bool a(byte[] A_0, int A_1)
	{
		if ((long)A_1 <= 544768L)
		{
			byte[] array = new byte[544768];
			Array.Copy(A_0, 0, array, 0, A_1);
			for (int num = A_1; (long)num < 544768L; num++)
			{
				array[num] = byte.MaxValue;
			}
			A_0 = array;
			bool result = false;
			if (DeviceComm.e(global::k.ak, 0uL, 0uL) == 1)
			{
				BitConverter.GetBytes(A_1);
				if (DeviceComm.k(ref A_0, A_0.Length) == 1)
				{
					ulong A_2 = 0uL;
					ulong A_3 = 0uL;
					result = DeviceComm.h(ref A_2, ref A_3) == 1;
				}
			}
			return result;
		}
		return false;
	}

	private new bool a(ref byte[] A_0, bool A_1)
	{
		bool flag = false;
		if (DeviceComm.e(A_1 ? global::k.am : global::k.al, 0uL, 0uL) == 1)
		{
			flag = (A_1 ? (DeviceComm.k(ref A_0, A_0.Length) == 1) : (DeviceComm.l(ref A_0, A_0.Length) == 1));
			if (flag)
			{
				ulong A_2 = 0uL;
				ulong A_3 = 0uL;
				flag = DeviceComm.h(ref A_2, ref A_3) == 1;
			}
		}
		return flag;
	}
}
