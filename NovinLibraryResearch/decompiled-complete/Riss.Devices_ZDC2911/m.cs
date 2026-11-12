using System;
using System.Collections.Generic;
using System.Text;
using Riss.Devices;

internal class m : DeviceConnection
{
	private new byte[] m_a;

	private new byte[] m_b;

	private new byte[] m_c;

	public m()
	{
		DeviceComm = new i();
	}

	public override int Open()
	{
		DeviceComm.a(base.Device);
		int num = DeviceComm.s();
		ac = num == 10513;
		if (num <= 0)
		{
			return 0;
		}
		return 2911;
	}

	public override void Close()
	{
		if (DeviceComm != null)
		{
			DeviceComm.q();
		}
	}

	public override bool SetProperty(UserProperty property, object extraProperty, User user, object extraData)
	{
		bool result = false;
		switch (property)
		{
		case UserProperty.UserName:
			result = g(user);
			break;
		case UserProperty.UserExtInfo:
			result = f(user);
			break;
		case UserProperty.Privilege:
			result = e(user);
			break;
		case UserProperty.AccessControlSettings:
			switch ((AccessContorlCommand)extraProperty)
			{
			case AccessContorlCommand.UserAccessCtrl:
				result = d(user);
				break;
			case AccessContorlCommand.UserPeriod:
			{
				byte[] a_2 = (byte[])extraData;
				result = a(user, a_2);
				break;
			}
			}
			break;
		case UserProperty.Enroll:
			result = (((bool)extraData) ? a(user) : b(user));
			break;
		case UserProperty.Attendance:
		{
			int a_ = (int)extraData;
			result = a(user, a_);
			break;
		}
		case UserProperty.UserEnroll:
			switch ((UserEnrollCommand)extraProperty)
			{
			case UserEnrollCommand.ClearCard:
				result = a(user.Enrolls[0]);
				break;
			case UserEnrollCommand.ClearFingerprint:
				result = a(user.Enrolls[0], (int)user.Enrolls[0].EnrollType);
				break;
			case UserEnrollCommand.ClearPassword:
				result = b(user.Enrolls[0]);
				break;
			case UserEnrollCommand.WriteCard:
				result = b(user.Enrolls[0], user.Privilege);
				break;
			case UserEnrollCommand.WriteFingerprint:
				result = a(user.Enrolls[0], (int)user.Enrolls[0].EnrollType, user.Privilege);
				break;
			case UserEnrollCommand.WritePassword:
				result = c(user.Enrolls[0], user.Privilege);
				break;
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
			result = d(ref user);
			break;
		case UserProperty.UserExtInfo:
			result = c(ref user);
			break;
		case UserProperty.AccessControlSettings:
			switch ((AccessContorlCommand)extraProperty)
			{
			case AccessContorlCommand.UserAccessCtrl:
				result = b(ref user);
				break;
			case AccessContorlCommand.UserPeriod:
			{
				byte[] A_5 = new byte[8];
				result = a(user, ref A_5);
				extraData = A_5;
				break;
			}
			}
			break;
		case UserProperty.Enroll:
			result = a(ref user);
			break;
		case UserProperty.Attendance:
		{
			int A_4 = -1;
			result = a(user, ref A_4);
			extraData = A_4;
			break;
		}
		case UserProperty.UserEnroll:
			switch ((UserEnrollCommand)extraProperty)
			{
			case UserEnrollCommand.ReadCard:
			{
				Enroll A_3 = user.Enrolls[0];
				result = a(ref A_3);
				user.Enrolls[0] = A_3;
				break;
			}
			case UserEnrollCommand.ReadFingerprint:
			{
				Enroll A_2 = user.Enrolls[0];
				result = a(ref A_2, (int)A_2.EnrollType);
				user.Enrolls[0] = A_2;
				break;
			}
			case UserEnrollCommand.ReadPassword:
			{
				Enroll A_ = user.Enrolls[0];
				result = b(ref A_);
				user.Enrolls[0] = A_;
				break;
			}
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
		case DeviceProperty.DoorControl:
		{
			byte[] A_6 = (byte[])extraData;
			result = h(ref A_6);
			break;
		}
		case DeviceProperty.SysParam:
		{
			byte[] A_5 = (byte[])extraData;
			result = i(ref A_5);
			break;
		}
		case DeviceProperty.InitSettings:
			result = c();
			break;
		case DeviceProperty.PowerOff:
		{
			byte[] A_4 = (byte[])extraData;
			result = h(ref A_4);
			break;
		}
		case DeviceProperty.Enable:
		{
			long bEnable = Convert.ToInt64(extraData.ToString());
			result = EnableDevice(bEnable);
			break;
		}
		case DeviceProperty.DeviceTime:
			result = d();
			break;
		case DeviceProperty.Bell:
		case DeviceProperty.AccessControlSettings:
		case DeviceProperty.WelcomeTitle:
		case DeviceProperty.StandbyTitle:
		case DeviceProperty.Message:
		case DeviceProperty.PowerOnOffTime:
		case DeviceProperty.Model:
		case DeviceProperty.SerialNo:
		case DeviceProperty.PassSegment:
		case DeviceProperty.PassGroup:
		case DeviceProperty.PassZone:
		case DeviceProperty.MultOpenGroup:
		{
			int a_4 = (int)extraProperty;
			string a_5 = (string)extraData;
			result = a(a_4, a_5);
			break;
		}
		case DeviceProperty.ManagementRecords:
			result = b();
			break;
		case DeviceProperty.AttRecords:
			result = a();
			break;
		case DeviceProperty.Enrolls:
		{
			ulong a_3 = (ulong)extraData;
			result = a(a_3);
			break;
		}
		case DeviceProperty.MacAddress:
		{
			byte[] a_2 = (byte[])extraData;
			result = a(a_2);
			break;
		}
		case DeviceProperty.Attendance:
			switch ((AttendanceCommand)extraProperty)
			{
			case AttendanceCommand.TimeZone:
			{
				byte[] A_3 = (byte[])extraData;
				result = e(ref A_3);
				break;
			}
			case AttendanceCommand.Holiday:
			{
				byte[] A_2 = (byte[])extraData;
				result = c(ref A_2);
				break;
			}
			case AttendanceCommand.LogTime:
			{
				byte[] A_ = (byte[])extraData;
				result = a(ref A_);
				break;
			}
			}
			break;
		case DeviceProperty.UploadSound:
		{
			List<int> list = (List<int>)extraProperty;
			byte[] a_ = (byte[])extraData;
			result = a(list[0], list[1], a_);
			break;
		}
		}
		return result;
	}

	public override bool GetProperty(DeviceProperty property, object extraProperty, ref Device device, ref object extraData)
	{
		bool result = false;
		switch (property)
		{
		case DeviceProperty.SysParam:
		{
			byte[] A_9 = (byte[])extraData;
			result = g(ref A_9);
			extraData = A_9;
			break;
		}
		case DeviceProperty.DeviceTime:
		{
			DateTime A_8 = DateTime.Now;
			result = a(ref A_8);
			extraData = A_8;
			break;
		}
		case DeviceProperty.FirmwareVersion:
		case DeviceProperty.Bell:
		case DeviceProperty.AccessControlSettings:
		case DeviceProperty.WelcomeTitle:
		case DeviceProperty.StandbyTitle:
		case DeviceProperty.Message:
		case DeviceProperty.PowerOnOffTime:
		case DeviceProperty.Model:
		case DeviceProperty.SerialNo:
		case DeviceProperty.PassSegment:
		case DeviceProperty.PassGroup:
		case DeviceProperty.PassZone:
		case DeviceProperty.MultOpenGroup:
		{
			int a_10 = (int)extraData;
			string A_14 = string.Empty;
			result = a(a_10, ref A_14);
			extraData = A_14;
			break;
		}
		case DeviceProperty.ManagementRecords:
		{
			List<bool> a_8 = (List<bool>)extraProperty;
			List<DateTime> a_9 = (List<DateTime>)extraData;
			List<Record> A_13 = new List<Record>();
			result = b(a_8, a_9, ref A_13);
			extraData = A_13;
			break;
		}
		case DeviceProperty.ManagementRecordsCount:
		{
			bool a_6 = (bool)extraProperty;
			List<DateTime> a_7 = (List<DateTime>)extraData;
			int A_12 = 0;
			result = b(a_6, a_7, ref A_12);
			extraData = A_12;
			break;
		}
		case DeviceProperty.AttRecords:
		{
			List<bool> a_4 = (List<bool>)extraProperty;
			List<DateTime> a_5 = (List<DateTime>)extraData;
			List<Record> A_11 = new List<Record>();
			result = a(a_4, a_5, ref A_11);
			extraData = A_11;
			break;
		}
		case DeviceProperty.AttRecordsCount:
		{
			bool a_2 = (bool)extraProperty;
			List<DateTime> a_3 = (List<DateTime>)extraData;
			int A_10 = 0;
			result = a(a_2, a_3, ref A_10);
			extraData = A_10;
			break;
		}
		case DeviceProperty.Enrolls:
		{
			ulong num = (ulong)extraProperty;
			if (0 == num)
			{
				List<User> A_6 = new List<User>();
				result = a(ushort.MaxValue, ref A_6);
				extraData = A_6;
			}
			else
			{
				User A_7 = new User();
				result = a(num, ref A_7);
				extraData = A_7;
			}
			break;
		}
		case DeviceProperty.MacAddress:
		{
			byte[] A_5 = new byte[6];
			result = f(ref A_5);
			extraData = A_5;
			break;
		}
		case DeviceProperty.Status:
		{
			uint[] A_4 = new uint[9];
			result = a(ref A_4);
			extraData = A_4;
			break;
		}
		case DeviceProperty.Attendance:
			switch ((AttendanceCommand)extraProperty)
			{
			case AttendanceCommand.TimeZone:
			{
				byte[] A_3 = new byte[42];
				result = d(ref A_3);
				extraData = A_3;
				break;
			}
			case AttendanceCommand.Holiday:
			{
				byte[] A_2 = new byte[300];
				result = b(ref A_2);
				extraData = A_2;
				break;
			}
			case AttendanceCommand.LogTime:
			{
				int a_ = (int)extraData;
				byte[] A_ = new byte[144];
				result = a(a_, ref A_);
				extraData = A_;
				break;
			}
			}
			break;
		}
		return result;
	}

	protected override bool EnableDevice(long bEnable)
	{
		byte[] A_ = BitConverter.GetBytes((ushort)bEnable);
		bool flag = 1 == DeviceComm.e(global::k.dd, 1uL, 1uL);
		if (flag)
		{
			if (1 != DeviceComm.i(ref A_, A_.Length))
			{
				return false;
			}
			if (1 != DeviceComm.t(ref A_, A_.Length))
			{
				return false;
			}
			flag = Zd2911Utils.b(A_, 8);
		}
		return flag;
	}

	private new bool a(ref DateTime A_0)
	{
		bool result = false;
		byte[] A_ = new byte[6];
		if (1 == DeviceComm.e(global::k.dj, 1uL, 0uL))
		{
			if (1 != DeviceComm.i(ref A_, A_.Length))
			{
				return false;
			}
			if (1 != DeviceComm.t(ref A_, A_.Length))
			{
				return false;
			}
			if (!Zd2911Utils.b(A_, 8))
			{
				return false;
			}
			result = Zd2911Utils.a(A_, 10, ref A_0);
		}
		return result;
	}

	private new bool d()
	{
		bool result = false;
		byte[] A_ = new byte[6]
		{
			(byte)(DateTime.Now.Year - 2000),
			(byte)DateTime.Now.Month,
			(byte)DateTime.Now.Day,
			(byte)DateTime.Now.Hour,
			(byte)DateTime.Now.Minute,
			(byte)DateTime.Now.Second
		};
		if (1 == DeviceComm.e(global::k.dk, 1uL, 6uL))
		{
			if (1 != DeviceComm.i(ref A_, A_.Length))
			{
				return false;
			}
			if (1 != DeviceComm.t(ref A_, A_.Length))
			{
				return false;
			}
			result = Zd2911Utils.b(A_, 8);
		}
		return result;
	}

	private bool m(ref byte[] A_0)
	{
		bool result = false;
		if (A_0.Length != 300)
		{
			return false;
		}
		int num = 0;
		List<byte[]> list = Zd2911Utils.a(A_0, 10);
		foreach (byte[] item in list)
		{
			byte[] A_ = item;
			if (1 == DeviceComm.e(global::k.c0, 1uL, 10uL))
			{
				if (1 != DeviceComm.i(ref A_, A_.Length))
				{
					return false;
				}
				if (1 != DeviceComm.t(ref A_, A_.Length))
				{
					return false;
				}
				if (!Zd2911Utils.b(A_, 8))
				{
					return false;
				}
				num++;
			}
		}
		if (num == list.Count)
		{
			result = true;
		}
		return result;
	}

	private new bool l(ref byte[] A_0)
	{
		bool result = false;
		int num = 0;
		for (int num2 = 0; num2 < 30; num2++)
		{
			byte[] A_ = new byte[10]
			{
				(byte)(num2 + 1),
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
			if (1 == DeviceComm.e(global::k.c1, 1uL, 10uL))
			{
				if (1 != DeviceComm.i(ref A_, A_.Length))
				{
					return false;
				}
				if (1 != DeviceComm.t(ref A_, A_.Length))
				{
					return false;
				}
				if (!Zd2911Utils.b(A_, 8))
				{
					return false;
				}
				byte[] array = new byte[10]
				{
					(byte)(num2 + 1),
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
				Array.Copy(A_, 10, array, 2, 8);
				Array.Copy(array, 0, A_0, num2 * 10, array.Length);
				num++;
			}
		}
		if (num == 30)
		{
			result = true;
		}
		return result;
	}

	private new bool k(ref byte[] A_0)
	{
		bool result = false;
		if (A_0.Length != 1260)
		{
			return false;
		}
		int num = 0;
		List<byte[]> list = Zd2911Utils.a(A_0, 6);
		foreach (byte[] item in list)
		{
			byte[] A_ = item;
			if (1 == DeviceComm.e(global::k.cy, 1uL, 6uL))
			{
				if (1 != DeviceComm.i(ref A_, A_.Length))
				{
					return false;
				}
				if (1 != DeviceComm.t(ref A_, A_.Length))
				{
					return false;
				}
				if (!Zd2911Utils.b(A_, 8))
				{
					return false;
				}
				num++;
			}
		}
		if (num == list.Count)
		{
			result = true;
		}
		return result;
	}

	private new bool j(ref byte[] A_0)
	{
		bool result = false;
		int num = 0;
		for (int num2 = 0; num2 < 30; num2++)
		{
			for (int num3 = 0; num3 < 7; num3++)
			{
				byte[] A_ = new byte[6]
				{
					(byte)(num2 + 1),
					(byte)num3,
					0,
					0,
					0,
					0
				};
				if (1 == DeviceComm.e(global::k.cz, 1uL, 6uL))
				{
					if (1 != DeviceComm.i(ref A_, A_.Length))
					{
						return false;
					}
					if (1 != DeviceComm.t(ref A_, A_.Length))
					{
						return false;
					}
					if (!Zd2911Utils.b(A_, 8))
					{
						return false;
					}
					byte[] array = new byte[6]
					{
						(byte)(num2 + 1),
						(byte)num3,
						0,
						0,
						0,
						0
					};
					Array.Copy(A_, 10, array, 2, 4);
					Array.Copy(array, 0, A_0, num * 6, array.Length);
					num++;
				}
			}
		}
		if (num == 210)
		{
			result = true;
		}
		return result;
	}

	private new bool i(ref byte[] A_0)
	{
		bool result = false;
		if (1 == DeviceComm.e(global::k.dl, 1uL, 8uL))
		{
			if (1 != DeviceComm.i(ref A_0, A_0.Length))
			{
				return false;
			}
			if (1 != DeviceComm.t(ref A_0, A_0.Length))
			{
				return false;
			}
			result = Zd2911Utils.b(A_0, 8);
		}
		return result;
	}

	private new bool h(ref byte[] A_0)
	{
		bool result = false;
		if (1 == DeviceComm.e(global::k.dp, 1uL, 8uL))
		{
			if (1 != DeviceComm.i(ref A_0, A_0.Length))
			{
				return false;
			}
			if (1 != DeviceComm.t(ref A_0, A_0.Length))
			{
				return false;
			}
			result = Zd2911Utils.b(A_0, 8);
		}
		return result;
	}

	private new bool g(ref byte[] A_0)
	{
		bool result = false;
		byte[] array = new byte[4];
		if (1 == DeviceComm.e(global::k.dm, 1uL, 4uL))
		{
			if (1 != DeviceComm.i(ref A_0, A_0.Length))
			{
				return false;
			}
			if (1 != DeviceComm.t(ref A_0, A_0.Length))
			{
				return false;
			}
			if (!Zd2911Utils.b(A_0, 8))
			{
				return false;
			}
			Array.Copy(A_0, 10, array, 0, array.Length);
			A_0 = array;
			result = true;
		}
		return result;
	}

	private new bool c()
	{
		bool result = false;
		byte[] A_ = new byte[1];
		if (1 == DeviceComm.e(global::k.cf, 1uL, 0uL))
		{
			if (1 != DeviceComm.i(ref A_, A_.Length))
			{
				return false;
			}
			if (1 != DeviceComm.t(ref A_, A_.Length))
			{
				return false;
			}
			result = Zd2911Utils.b(A_, 8);
		}
		return result;
	}

	private new bool a(int A_0, string A_1)
	{
		bool result = false;
		byte[] bytes = BitConverter.GetBytes(A_0);
		byte[] array = Zd2911Utils.a(A_0, A_1);
		int num = Zd2911Utils.a(A_0);
		byte[] A_2 = new byte[2 * bytes.Length + num];
		Array.Copy(bytes, 0, A_2, 0, bytes.Length);
		Array.Copy(bytes, 0, A_2, bytes.Length, bytes.Length);
		Array.Copy(array, 0, A_2, 2 * bytes.Length, array.Length);
		if (1 == DeviceComm.e(global::k.du, 0uL, (ulong)A_2.Length))
		{
			if (1 != DeviceComm.k(ref A_2, A_2.Length))
			{
				return false;
			}
			if (1 != DeviceComm.l(ref A_2, A_2.Length))
			{
				return false;
			}
			result = Zd2911Utils.b(A_2, 8);
		}
		return result;
	}

	private new bool a(int A_0, ref string A_1)
	{
		bool result = false;
		int a_ = Zd2911Utils.a(A_0);
		if (1 == DeviceComm.e(global::k.dv, 0uL, 4uL))
		{
			byte[] A_2 = BitConverter.GetBytes(A_0);
			if (1 != DeviceComm.i(ref A_2, A_2.Length))
			{
				return false;
			}
			if (1 != DeviceComm.t(ref A_2, A_2.Length))
			{
				return false;
			}
			if (!Zd2911Utils.b(A_2, 8))
			{
				return false;
			}
			if (1 != DeviceComm.l(ref A_2, A_2.Length))
			{
				return false;
			}
			A_1 = Zd2911Utils.a(A_0, A_2, a_);
			result = true;
		}
		return result;
	}

	private new bool a(global::f A_0, ref int A_1, ref int A_2)
	{
		bool result = false;
		byte[] A_3 = new byte[1] { (byte)A_0 };
		if (1 == DeviceComm.e(global::k.db, 1uL, 2uL))
		{
			if (1 != DeviceComm.i(ref A_3, A_3.Length))
			{
				return false;
			}
			if (1 != DeviceComm.t(ref A_3, A_3.Length))
			{
				return false;
			}
			if (!Zd2911Utils.b(A_3, 8))
			{
				return false;
			}
			A_1 = BitConverter.ToInt32(A_3, 10);
			A_2 = BitConverter.ToInt32(A_3, 14);
			result = true;
		}
		return result;
	}

	private new bool a(global::f A_0, List<DateTime> A_1, ref int A_2, ref int A_3)
	{
		bool result = false;
		byte[] A_4 = new byte[7]
		{
			(byte)A_0,
			(byte)(A_1[0].Year - 2000),
			(byte)A_1[0].Month,
			(byte)A_1[0].Day,
			(byte)(A_1[1].Year - 2000),
			(byte)A_1[1].Month,
			(byte)A_1[1].Day
		};
		if (1 == DeviceComm.e(global::k.dc, 1uL, 7uL))
		{
			if (1 != DeviceComm.i(ref A_4, A_4.Length))
			{
				return false;
			}
			if (1 != DeviceComm.t(ref A_4, A_4.Length))
			{
				return false;
			}
			if (!Zd2911Utils.b(A_4, 8))
			{
				return false;
			}
			A_2 = BitConverter.ToInt32(A_4, 10);
			A_3 = BitConverter.ToInt32(A_4, 14);
			result = true;
		}
		return result;
	}

	private new bool a(global::f A_0, byte A_1, uint A_2, uint A_3, ref byte[] A_4)
	{
		bool result = false;
		byte[] A_5 = new byte[10];
		byte[] bytes = BitConverter.GetBytes(A_2);
		byte[] bytes2 = BitConverter.GetBytes(A_3);
		Array.Copy(bytes, 0, A_5, 0, bytes.Length);
		Array.Copy(bytes2, 0, A_5, 4, bytes2.Length);
		A_5[8] = (byte)A_0;
		A_5[9] = A_1;
		if (1 == DeviceComm.e(global::k.ca, 1uL, 10uL))
		{
			if (1 != DeviceComm.i(ref A_5, A_5.Length))
			{
				return false;
			}
			if (1 != DeviceComm.t(ref A_5, A_5.Length))
			{
				return false;
			}
			if (!Zd2911Utils.b(A_5, 8))
			{
				return false;
			}
			if (1 != DeviceComm.l(ref A_5, A_5.Length))
			{
				return false;
			}
			if (A_0 == global::f.a)
			{
				A_4 = new byte[(A_3 - A_2 + 1) * 28];
				Array.Copy(A_5, 10, A_4, 0, A_4.Length);
			}
			else
			{
				A_4 = new byte[(A_3 - A_2 + 1) * 16];
				Array.Copy(A_5, 10, A_4, 0, A_4.Length);
			}
			result = true;
		}
		return result;
	}

	private new bool a(global::f A_0, ref int A_1)
	{
		bool flag = false;
		int A_2 = 0;
		int A_3 = 0;
		byte a_ = 0;
		flag = a(A_0, ref A_2, ref A_3);
		if (!flag || -1 == A_2 || -1 == A_3)
		{
			A_1 = 0;
			return false;
		}
		int num = 0;
		A_1 = A_3 - A_2 + 1;
		if (A_1 == 0)
		{
			return false;
		}
		int num2 = A_1 / 2000;
		if (A_0 == global::f.a)
		{
			this.m_a = new byte[28000];
			int num3;
			for (num3 = 0; num3 < num2; num3++)
			{
				if (num2 - 1 == num3 && A_1 % 2000 == 0)
				{
					a_ = 1;
				}
				int num4 = (A_2 + (num3 + 1) * 2000 - 1 - A_2 + num3 * 2000 + 1) * 28;
				byte[] A_4 = new byte[num4];
				flag = a(A_0, a_, (uint)(A_2 + num3 * 2000), (uint)(A_2 + (num3 + 1) * 2000 - 1), ref A_4);
				if (!flag)
				{
					return false;
				}
				Array.Copy(A_4, 0, this.m_a, num * 28, A_4.Length);
				num += 2000;
			}
			if (A_1 % 2000 > 0)
			{
				int num5 = (A_2 + num3 * 2000 + A_1 % 2000 - A_2 + num3 * 2000 + 1) * 28;
				byte[] A_5 = new byte[num5];
				flag = a(A_0, 1, (uint)(A_2 + num3 * 2000), (uint)(A_2 + num3 * 2000 + A_1 % 2000), ref A_5);
				if (!flag)
				{
					return false;
				}
				Array.Copy(A_5, 0, this.m_a, num * 28, A_5.Length);
			}
		}
		else
		{
			this.m_b = new byte[1600000];
			int num3;
			for (num3 = 0; num3 < num2; num3++)
			{
				if (num2 - 1 == num3 && A_1 % 2000 == 0)
				{
					a_ = 1;
				}
				int num6 = (A_2 + (num3 + 1) * 2000 - 1 - A_2 + num3 * 2000) * 16;
				byte[] A_6 = new byte[num6];
				flag = a(A_0, a_, (uint)(A_2 + num3 * 2000), (uint)(A_2 + (num3 + 1) * 2000 - 1), ref A_6);
				if (!flag)
				{
					return false;
				}
				Array.Copy(A_6, 0, this.m_b, num * 16, A_6.Length);
				num += 2000;
			}
			if (A_1 % 2000 > 0)
			{
				int num7 = (A_2 + num3 * 2000 + A_1 % 2000 - A_2 + num3 * 2000 - 1) * 16;
				byte[] A_7 = new byte[num7];
				flag = a(A_0, 1, (uint)(A_2 + num3 * 2000), (uint)(A_2 + num3 * 2000 + A_1 % 2000), ref A_7);
				if (!flag)
				{
					return false;
				}
				Array.Copy(A_7, 0, this.m_b, num * 16, A_7.Length);
			}
		}
		A_1 = A_3 - A_2 + 1;
		return flag;
	}

	private new bool a(global::f A_0, byte A_1, List<DateTime> A_2, ref int A_3)
	{
		bool flag = false;
		int A_4 = 0;
		int A_5 = 0;
		flag = a(A_0, A_2, ref A_4, ref A_5);
		if (!flag || -1 == A_4 || -1 == A_5)
		{
			A_3 = 0;
			return false;
		}
		int num = 0;
		A_3 = A_5 - A_4 + 1;
		if (A_3 == 0)
		{
			return false;
		}
		int num2 = A_3 / 2000;
		if (A_0 == global::f.a)
		{
			this.m_a = new byte[28000];
			int num3;
			for (num3 = 0; num3 < num2; num3++)
			{
				int num4 = (A_4 + (num3 + 1) * 2000 - 1 - A_4 + num3 * 2000 + 1) * 28;
				byte[] A_6 = new byte[num4];
				flag = a(A_0, A_1, (uint)(A_4 + num3 * 2000), (uint)(A_4 + (num3 + 1) * 2000 - 1), ref A_6);
				if (!flag)
				{
					return false;
				}
				Array.Copy(A_6, 0, this.m_a, num * 28, A_6.Length);
				num += 2000;
			}
			if (A_3 % 2000 > 0)
			{
				int num5 = (A_4 + num3 * 2000 + A_3 % 2000 - A_4 + num3 * 2000 + 1) * 28;
				byte[] A_7 = new byte[num5];
				flag = a(A_0, A_1, (uint)(A_4 + num3 * 2000), (uint)(A_4 + num3 * 2000 + A_3 % 2000), ref A_7);
				if (!flag)
				{
					return false;
				}
				Array.Copy(A_7, 0, this.m_a, num * 28, A_7.Length);
			}
		}
		else
		{
			this.m_b = new byte[1600000];
			int num3;
			for (num3 = 0; num3 < num2; num3++)
			{
				int num6 = (A_4 + (num3 + 1) * 2000 - 1 - A_4 + num3 * 2000) * 16;
				byte[] A_8 = new byte[num6];
				flag = a(A_0, A_1, (uint)(A_4 + num3 * 2000), (uint)(A_4 + (num3 + 1) * 2000 - 1), ref A_8);
				if (!flag)
				{
					return false;
				}
				Array.Copy(A_8, 0, this.m_b, num * 16, A_8.Length);
				num += 2000;
			}
			if (A_3 % 2000 > 0)
			{
				int num7 = (A_4 + num3 * 2000 + A_3 % 2000 - A_4 + num3 * 2000 - 1) * 16;
				byte[] A_9 = new byte[num7];
				flag = a(A_0, A_1, (uint)(A_4 + num3 * 2000), (uint)(A_4 + num3 * 2000 + A_3 % 2000), ref A_9);
				if (!flag)
				{
					return false;
				}
				Array.Copy(A_9, 0, this.m_b, num * 16, A_9.Length);
			}
		}
		A_3 = A_5 - A_4 + 1;
		return flag;
	}

	private new bool b(List<bool> A_0, List<DateTime> A_1, ref List<Record> A_2)
	{
		bool flag = false;
		int A_3 = 0;
		if (A_0[0])
		{
			flag = a(global::f.a, ref A_3);
		}
		else
		{
			byte a_ = Convert.ToByte(A_0[1]);
			flag = a(global::f.a, a_, A_1, ref A_3);
		}
		if (!flag)
		{
			this.m_a = null;
			return false;
		}
		for (int num = 0; num < A_3; num++)
		{
			byte[] array = new byte[28];
			Array.Copy(this.m_a, num * 28, array, 0, array.Length);
			Record record = new Record();
			record.DIN = BitConverter.ToUInt64(array, 0);
			record.MDIN = BitConverter.ToUInt64(array, 8);
			record.DN = BitConverter.ToUInt16(array, 16);
			record.Verify = array[18];
			record.Action = array[19];
			record.Clock = new DateTime(array[20] + 2000, array[21], array[22], array[23], array[24], array[25]);
			A_2.Add(record);
		}
		this.m_a = null;
		if (A_3 == A_2.Count)
		{
			flag = true;
		}
		return flag;
	}

	private new bool b()
	{
		bool result = false;
		byte[] A_ = new byte[1];
		if (1 == DeviceComm.e(global::k.ck, 1uL, 0uL))
		{
			if (1 != DeviceComm.i(ref A_, A_.Length))
			{
				return false;
			}
			if (1 != DeviceComm.t(ref A_, A_.Length))
			{
				return false;
			}
			result = Zd2911Utils.b(A_, 8);
		}
		return result;
	}

	private new bool b(bool A_0, List<DateTime> A_1, ref int A_2)
	{
		bool flag = false;
		int A_3 = 0;
		int A_4 = 0;
		flag = ((!A_0) ? a(global::f.a, A_1, ref A_3, ref A_4) : a(global::f.a, ref A_3, ref A_4));
		if (flag && -1 != A_3 && -1 != A_4)
		{
			A_2 = A_4 - A_3 + 1;
		}
		else
		{
			A_2 = 0;
		}
		return true;
	}

	private new bool a(List<bool> A_0, List<DateTime> A_1, ref List<Record> A_2)
	{
		bool flag = false;
		int A_3 = 0;
		if (A_0[0])
		{
			flag = a(global::f.b, ref A_3);
		}
		else
		{
			byte a_ = Convert.ToByte(A_0[1]);
			flag = a(global::f.b, a_, A_1, ref A_3);
		}
		if (!flag)
		{
			this.m_b = null;
			return false;
		}
		for (int num = 0; num < A_3; num++)
		{
			byte[] array = new byte[16];
			Array.Copy(this.m_b, num * 16, array, 0, array.Length);
			Record record = new Record();
			record.DIN = BitConverter.ToUInt64(array, 0);
			ulong a_2 = BitConverter.ToUInt64(array, 8);
			int year = Zd2911Utils.k(a_2);
			int month = Zd2911Utils.j(a_2);
			int day = Zd2911Utils.i(a_2);
			record.Verify = Zd2911Utils.g(a_2);
			record.Action = Zd2911Utils.h(a_2);
			record.JobCode = Zd2911Utils.f(a_2);
			record.Antipassback = Zd2911Utils.e(a_2);
			record.DN = base.Device.DN;
			int hour = Zd2911Utils.c(a_2);
			int minute = Zd2911Utils.b(a_2);
			int second = Zd2911Utils.a(a_2);
			record.Clock = new DateTime(year, month, day, hour, minute, second);
			A_2.Add(record);
		}
		this.m_b = null;
		if (A_3 == A_2.Count)
		{
			flag = true;
		}
		return flag;
	}

	private new bool a()
	{
		bool result = false;
		byte[] A_ = new byte[1];
		if (1 == DeviceComm.e(global::k.cj, 1uL, 0uL))
		{
			if (1 != DeviceComm.i(ref A_, A_.Length))
			{
				return false;
			}
			if (1 != DeviceComm.t(ref A_, A_.Length))
			{
				return false;
			}
			result = Zd2911Utils.b(A_, 8);
		}
		return result;
	}

	private new bool a(bool A_0, List<DateTime> A_1, ref int A_2)
	{
		bool flag = false;
		int A_3 = 0;
		int A_4 = 0;
		flag = ((!A_0) ? a(global::f.b, A_1, ref A_3, ref A_4) : a(global::f.b, ref A_3, ref A_4));
		if (flag && -1 != A_3 && -1 != A_4)
		{
			A_2 = A_4 - A_3 + 1;
		}
		else
		{
			A_2 = 0;
		}
		return true;
	}

	private new bool a(ushort A_0, ref int A_1)
	{
		bool result = false;
		byte[] A_2 = new byte[4];
		Array.Copy(BitConverter.GetBytes(A_0), 0, A_2, 2, 2);
		if (1 == DeviceComm.e(global::k.de, 1uL, 4uL))
		{
			if (1 != DeviceComm.i(ref A_2, A_2.Length))
			{
				return false;
			}
			if (1 != DeviceComm.t(ref A_2, A_2.Length))
			{
				return false;
			}
			if (!Zd2911Utils.b(A_2, 8))
			{
				return false;
			}
			A_1 = BitConverter.ToUInt16(A_2, 10);
			if (1 != DeviceComm.l(ref A_2, A_2.Length))
			{
				return false;
			}
			this.m_c = new byte[A_0 * 12];
			Array.Copy(A_2, 12, this.m_c, 0, A_1 * 12);
			result = true;
		}
		return result;
	}

	public new bool a(ushort A_0, ref List<User> A_1)
	{
		bool flag = false;
		int A_2 = 0;
		flag = a(A_0, ref A_2);
		if (!flag || A_2 == 0)
		{
			this.m_c = null;
			return false;
		}
		for (int num = 0; num < A_2; num++)
		{
			byte[] array = new byte[12];
			Array.Copy(this.m_c, num * 12, array, 0, array.Length);
			User user = new User();
			user.Enrolls = new List<Enroll>();
			Enroll enroll = new Enroll();
			user.DIN = BitConverter.ToUInt64(array, 0);
			enroll.DIN = user.DIN;
			enroll.EnrollType = (EnrollType)BitConverter.ToUInt16(array, 8);
			user.Privilege = array[10];
			user.Enrolls.Add(enroll);
			A_1.Add(user);
		}
		this.m_c = null;
		if (A_2 == A_1.Count)
		{
			flag = true;
		}
		return flag;
	}

	private new bool a(ulong A_0, ref User A_1)
	{
		bool result = false;
		byte[] A_2 = BitConverter.GetBytes(A_0);
		if (1 == DeviceComm.e(global::k.cl, 1uL, 8uL))
		{
			if (1 != DeviceComm.i(ref A_2, A_2.Length))
			{
				return false;
			}
			if (1 != DeviceComm.t(ref A_2, A_2.Length))
			{
				return false;
			}
			if (!Zd2911Utils.b(A_2, 8))
			{
				return false;
			}
			A_1.Enrolls = new List<Enroll>();
			Enroll enroll = new Enroll();
			A_1.DIN = Convert.ToUInt64(A_0);
			enroll.DIN = A_1.DIN;
			enroll.EnrollType = (EnrollType)BitConverter.ToUInt16(A_2, 22);
			A_1.Privilege = A_2[24];
			A_1.Enrolls.Add(enroll);
			result = true;
		}
		return result;
	}

	private new bool a(ulong A_0)
	{
		bool result = false;
		byte[] A_ = new byte[8];
		ulong a_ = 0uL;
		k a_2 = global::k.b4;
		if (0 != A_0)
		{
			a_ = 8uL;
			A_ = BitConverter.GetBytes(A_0);
			a_2 = global::k.b3;
		}
		if (1 == DeviceComm.e(a_2, 1uL, a_))
		{
			if (1 != DeviceComm.i(ref A_, A_.Length))
			{
				return false;
			}
			if (1 != DeviceComm.t(ref A_, A_.Length))
			{
				return false;
			}
			result = Zd2911Utils.b(A_, 8);
		}
		return result;
	}

	private new bool a(byte[] A_0)
	{
		bool result = false;
		if (1 == DeviceComm.e(global::k.cg, 1uL, (ulong)A_0.Length))
		{
			if (1 != DeviceComm.i(ref A_0, A_0.Length))
			{
				return false;
			}
			if (1 != DeviceComm.t(ref A_0, A_0.Length))
			{
				return false;
			}
			result = Zd2911Utils.b(A_0, 8);
		}
		return result;
	}

	private new bool f(ref byte[] A_0)
	{
		bool result = false;
		byte[] A_ = new byte[6];
		if (1 == DeviceComm.e(global::k.ch, 1uL, 0uL))
		{
			if (1 != DeviceComm.i(ref A_, A_.Length))
			{
				return false;
			}
			if (1 != DeviceComm.t(ref A_, A_.Length))
			{
				return false;
			}
			if (!Zd2911Utils.b(A_, 8))
			{
				return false;
			}
			Array.Copy(A_, 10, A_0, 0, A_0.Length);
			result = true;
		}
		return result;
	}

	private new bool a(ref uint[] A_0)
	{
		bool result = false;
		byte[] A_ = new byte[18];
		if (1 == DeviceComm.e(global::k.b5, 0uL, 0uL))
		{
			if (1 != DeviceComm.k(ref A_, A_.Length))
			{
				return false;
			}
			if (1 != DeviceComm.l(ref A_, A_.Length))
			{
				return false;
			}
			if (!Zd2911Utils.b(A_, 8))
			{
				return false;
			}
			A_0[0] = BitConverter.ToUInt16(A_, 10);
			A_0[1] = BitConverter.ToUInt16(A_, 12);
			A_0[2] = BitConverter.ToUInt16(A_, 14);
			A_0[3] = BitConverter.ToUInt16(A_, 16);
			A_0[4] = BitConverter.ToUInt16(A_, 18);
			A_0[5] = BitConverter.ToUInt32(A_, 20);
			A_0[6] = BitConverter.ToUInt32(A_, 24);
			A_0[7] = BitConverter.ToUInt32(A_, 28);
			A_0[8] = BitConverter.ToUInt32(A_, 32);
			result = true;
		}
		return result;
	}

	private new bool e(ref byte[] A_0)
	{
		bool result = false;
		int num = 0;
		if (A_0.Length != 42)
		{
			return false;
		}
		List<byte[]> list = Zd2911Utils.a(A_0, 14);
		foreach (byte[] item in list)
		{
			byte[] A_ = item;
			byte[] A_2 = BitConverter.GetBytes((ushort)14);
			if (1 == DeviceComm.e(global::k.cw, 1uL, 2uL))
			{
				if (1 != DeviceComm.i(ref A_2, A_2.Length))
				{
					return false;
				}
				if (1 != DeviceComm.t(ref A_2, A_2.Length))
				{
					return false;
				}
				if (!Zd2911Utils.b(A_2, 8))
				{
					return false;
				}
			}
			if (1 == DeviceComm.e(global::k.cw, 0uL, 14uL))
			{
				if (1 != DeviceComm.k(ref A_, A_.Length))
				{
					return false;
				}
				if (1 != DeviceComm.l(ref A_, A_.Length))
				{
					return false;
				}
				if (!Zd2911Utils.b(A_, 8))
				{
					return false;
				}
				num++;
			}
		}
		if (num == list.Count)
		{
			result = true;
		}
		return result;
	}

	private new bool d(ref byte[] A_0)
	{
		bool result = false;
		int num = 0;
		for (int num2 = 0; num2 < 3; num2++)
		{
			byte[] A_ = BitConverter.GetBytes((ushort)(num2 + 1));
			if (1 == DeviceComm.e(global::k.cx, 1uL, 2uL))
			{
				if (1 != DeviceComm.i(ref A_, A_.Length))
				{
					return false;
				}
				if (1 != DeviceComm.t(ref A_, A_.Length))
				{
					return false;
				}
				if (!Zd2911Utils.b(A_, 8))
				{
					return false;
				}
				if (1 != DeviceComm.l(ref A_, A_.Length))
				{
					return false;
				}
				byte[] array = new byte[14];
				Array.Copy(BitConverter.GetBytes((ushort)(num2 + 1)), 0, array, 0, 2);
				Array.Copy(A_, 12, array, 2, 12);
				Array.Copy(array, 0, A_0, num2 * array.Length, array.Length);
				num++;
			}
		}
		if (num == 3)
		{
			result = true;
		}
		return result;
	}

	private new bool c(ref byte[] A_0)
	{
		bool result = false;
		int num = 0;
		if (A_0.Length != 300)
		{
			return false;
		}
		List<byte[]> list = Zd2911Utils.a(A_0, 10);
		foreach (byte[] item in list)
		{
			byte[] A_ = item;
			if (1 == DeviceComm.e(global::k.c6, 1uL, 10uL))
			{
				if (1 != DeviceComm.i(ref A_, A_.Length))
				{
					return false;
				}
				if (1 != DeviceComm.t(ref A_, A_.Length))
				{
					return false;
				}
				if (!Zd2911Utils.b(A_, 8))
				{
					return false;
				}
				num++;
			}
		}
		if (num == list.Count)
		{
			result = true;
		}
		return result;
	}

	private new bool b(ref byte[] A_0)
	{
		bool result = false;
		int num = 0;
		for (int num2 = 0; num2 < 30; num2++)
		{
			byte[] A_ = new byte[2]
			{
				(byte)(num2 + 1),
				0
			};
			if (1 == DeviceComm.e(global::k.c7, 1uL, 2uL))
			{
				if (1 != DeviceComm.i(ref A_, A_.Length))
				{
					return false;
				}
				if (1 != DeviceComm.t(ref A_, A_.Length))
				{
					return false;
				}
				if (!Zd2911Utils.b(A_, 8))
				{
					return false;
				}
				byte[] array = new byte[10]
				{
					(byte)(num2 + 1),
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
				Array.Copy(A_, 10, array, 2, 8);
				Array.Copy(array, 0, A_0, num2 * 10, array.Length);
				num++;
			}
		}
		if (num == 30)
		{
			result = true;
		}
		return result;
	}

	private new bool a(ref byte[] A_0)
	{
		bool result = false;
		int num = 0;
		if (A_0.Length != 144)
		{
			return false;
		}
		List<byte[]> list = Zd2911Utils.a(A_0, 6);
		foreach (byte[] item in list)
		{
			byte[] A_ = item;
			if (1 == DeviceComm.e(global::k.cs, 1uL, 8uL))
			{
				if (1 != DeviceComm.i(ref A_, A_.Length))
				{
					return false;
				}
				if (1 != DeviceComm.t(ref A_, A_.Length))
				{
					return false;
				}
				if (!Zd2911Utils.b(A_, 8))
				{
					return false;
				}
				num++;
			}
		}
		if (num == list.Count)
		{
			result = true;
		}
		return result;
	}

	private new bool a(int A_0, ref byte[] A_1)
	{
		bool result = false;
		int num = 0;
		for (int num2 = 0; num2 < 24; num2++)
		{
			byte[] A_2 = new byte[2]
			{
				(byte)A_0,
				(byte)(num2 + 1)
			};
			if (1 == DeviceComm.e(global::k.ct, 1uL, 2uL))
			{
				if (1 != DeviceComm.i(ref A_2, A_2.Length))
				{
					return false;
				}
				if (1 != DeviceComm.t(ref A_2, A_2.Length))
				{
					return false;
				}
				if (!Zd2911Utils.b(A_2, 8))
				{
					return false;
				}
				byte[] array = new byte[6]
				{
					(byte)A_0,
					(byte)(num2 + 1),
					0,
					0,
					0,
					0
				};
				Array.Copy(A_2, 10, array, 2, 4);
				Array.Copy(array, 0, A_1, num2 * 6, array.Length);
				num++;
			}
		}
		if (num == 24)
		{
			result = true;
		}
		return result;
	}

	private new bool a(int A_0, int A_1, byte[] A_2)
	{
		bool result = false;
		if (1 == DeviceComm.r(A_0, A_1, A_2))
		{
			result = true;
		}
		return result;
	}

	private new bool g(User A_0)
	{
		bool result = false;
		if (string.IsNullOrEmpty(A_0.UserName))
		{
			return false;
		}
		byte[] bytes = BitConverter.GetBytes(A_0.DIN);
		byte[] bytes2 = Encoding.Unicode.GetBytes(A_0.UserName);
		if (bytes2.Length > 24)
		{
			return false;
		}
		int num = 32;
		byte[] A_ = new byte[num];
		Array.Copy(bytes, 0, A_, 0, bytes.Length);
		Array.Copy(bytes2, 0, A_, 8, bytes2.Length);
		if (1 == DeviceComm.e(global::k.co, 0uL, (ulong)num))
		{
			if (1 != DeviceComm.k(ref A_, A_.Length))
			{
				return false;
			}
			if (1 != DeviceComm.l(ref A_, A_.Length))
			{
				return false;
			}
			result = Zd2911Utils.b(A_, 8);
		}
		return result;
	}

	private new bool d(ref User A_0)
	{
		bool result = false;
		byte[] A_ = BitConverter.GetBytes(A_0.DIN);
		if (1 == DeviceComm.e(global::k.cp, 1uL, 8uL))
		{
			if (1 != DeviceComm.i(ref A_, A_.Length))
			{
				return false;
			}
			if (1 != DeviceComm.t(ref A_, A_.Length))
			{
				return false;
			}
			if (!Zd2911Utils.b(A_, 8))
			{
				return false;
			}
			if (1 != DeviceComm.l(ref A_, A_.Length))
			{
				return false;
			}
			A_0.UserName = Encoding.Unicode.GetString(A_, 12, 24).Replace("\0", "");
			result = true;
		}
		return result;
	}

	private new bool f(User A_0)
	{
		bool result = false;
		if (string.IsNullOrEmpty(A_0.Comment))
		{
			return false;
		}
		byte[] bytes = BitConverter.GetBytes(A_0.DIN);
		byte[] bytes2 = Encoding.Unicode.GetBytes(A_0.Comment);
		if (bytes2.Length > 36)
		{
			return false;
		}
		int num = 44;
		byte[] A_ = new byte[num];
		Array.Copy(bytes, 0, A_, 0, bytes.Length);
		Array.Copy(bytes2, 0, A_, 8, bytes2.Length);
		if (1 == DeviceComm.e(global::k.dw, 0uL, (ulong)num))
		{
			if (1 != DeviceComm.k(ref A_, A_.Length))
			{
				return false;
			}
			if (1 != DeviceComm.l(ref A_, A_.Length))
			{
				return false;
			}
			result = Zd2911Utils.b(A_, 8);
		}
		return result;
	}

	private new bool c(ref User A_0)
	{
		bool result = false;
		byte[] A_ = BitConverter.GetBytes(A_0.DIN);
		if (1 == DeviceComm.e(global::k.dx, 1uL, 8uL))
		{
			if (1 != DeviceComm.i(ref A_, A_.Length))
			{
				return false;
			}
			if (1 != DeviceComm.t(ref A_, A_.Length))
			{
				return false;
			}
			if (!Zd2911Utils.b(A_, 8))
			{
				return false;
			}
			if (1 != DeviceComm.l(ref A_, A_.Length))
			{
				return false;
			}
			A_0.Comment = Encoding.Unicode.GetString(A_, 12, 36).Replace("\0", "");
			result = true;
		}
		return result;
	}

	private new bool e(User A_0)
	{
		bool result = false;
		byte[] bytes = BitConverter.GetBytes(A_0.DIN);
		int num = bytes.Length + 2;
		byte[] A_ = new byte[num];
		Array.Copy(bytes, 0, A_, 0, bytes.Length);
		A_[8] = (byte)A_0.Privilege;
		A_[9] = 0;
		if (1 == DeviceComm.e(global::k.di, 1uL, (ulong)num))
		{
			if (1 != DeviceComm.i(ref A_, A_.Length))
			{
				return false;
			}
			if (1 != DeviceComm.t(ref A_, A_.Length))
			{
				return false;
			}
			result = Zd2911Utils.b(A_, 8);
		}
		return result;
	}

	private new bool d(User A_0)
	{
		bool result = false;
		byte[] bytes = BitConverter.GetBytes(A_0.DIN);
		int num = bytes.Length + 4;
		byte[] A_ = new byte[num];
		Array.Copy(bytes, 0, A_, 0, bytes.Length);
		A_[8] = (byte)A_0.AccessTimeZone;
		A_[9] = Convert.ToByte(A_0.Enable);
		A_[10] = 0;
		A_[11] = 0;
		if (1 == DeviceComm.e(global::k.cm, 1uL, (ulong)num))
		{
			if (1 != DeviceComm.i(ref A_, A_.Length))
			{
				return false;
			}
			if (1 != DeviceComm.t(ref A_, A_.Length))
			{
				return false;
			}
			result = Zd2911Utils.b(A_, 8);
		}
		return result;
	}

	private new bool b(ref User A_0)
	{
		bool result = false;
		byte[] A_ = BitConverter.GetBytes(A_0.DIN);
		if (1 == DeviceComm.e(global::k.cn, 1uL, 8uL))
		{
			if (1 != DeviceComm.i(ref A_, A_.Length))
			{
				return false;
			}
			if (1 != DeviceComm.t(ref A_, A_.Length))
			{
				return false;
			}
			if (!Zd2911Utils.b(A_, 8))
			{
				return false;
			}
			A_0.AccessTimeZone = A_[10];
			A_0.Enable = Convert.ToBoolean(A_[11]);
			result = true;
		}
		return result;
	}

	private new bool a(ref Enroll A_0, int A_1)
	{
		bool result = false;
		byte[] bytes = BitConverter.GetBytes(A_0.DIN);
		int num = 9;
		byte[] A_2 = new byte[num];
		Array.Copy(bytes, 0, A_2, 0, bytes.Length);
		A_2[8] = (byte)A_1;
		if (1 == DeviceComm.e(global::k.b6, 1uL, (ulong)num))
		{
			if (1 != DeviceComm.i(ref A_2, A_2.Length))
			{
				return false;
			}
			if (1 != DeviceComm.t(ref A_2, A_2.Length))
			{
				return false;
			}
			if (!Zd2911Utils.b(A_2, 8))
			{
				return false;
			}
			if (1 != DeviceComm.l(ref A_2, A_2.Length))
			{
				return false;
			}
			Array.Copy(A_2, 12, A_0.Fingerprint, 0, 498);
			result = true;
		}
		return result;
	}

	private new bool b(ref Enroll A_0)
	{
		bool result = false;
		byte[] A_ = BitConverter.GetBytes(A_0.DIN);
		if (1 == DeviceComm.e(global::k.b9, 1uL, 8uL))
		{
			if (1 != DeviceComm.i(ref A_, A_.Length))
			{
				return false;
			}
			if (1 != DeviceComm.t(ref A_, A_.Length))
			{
				return false;
			}
			if (!Zd2911Utils.b(A_, 8))
			{
				return false;
			}
			A_0.Password = Encoding.ASCII.GetString(A_, 10, 8).Replace("\0", "");
			result = true;
		}
		return result;
	}

	private new bool a(ref Enroll A_0)
	{
		bool result = false;
		byte[] A_ = BitConverter.GetBytes(A_0.DIN);
		if (1 == DeviceComm.e(global::k.cc, 1uL, 8uL))
		{
			if (1 != DeviceComm.i(ref A_, A_.Length))
			{
				return false;
			}
			if (1 != DeviceComm.t(ref A_, A_.Length))
			{
				return false;
			}
			if (!Zd2911Utils.b(A_, 8))
			{
				return false;
			}
			A_0.CardID = BitConverter.ToUInt32(A_, 12).ToString();
			result = true;
		}
		return result;
	}

	private new bool a(ref User A_0)
	{
		bool result = false;
		byte[] A_ = BitConverter.GetBytes(A_0.DIN);
		if (1 == DeviceComm.e(global::k.dq, 0uL, (ulong)A_.Length))
		{
			if (1 != DeviceComm.i(ref A_, A_.Length))
			{
				return false;
			}
			if (1 != DeviceComm.t(ref A_, A_.Length))
			{
				return false;
			}
			if (!Zd2911Utils.b(A_, 8))
			{
				return false;
			}
			if (1 != DeviceComm.l(ref A_, A_.Length))
			{
				return false;
			}
			byte[] array = new byte[5096];
			Array.Copy(A_, 12, array, 0, array.Length);
			if (!Zd2911Utils.a(array, ref A_0))
			{
				return false;
			}
			result = true;
		}
		return result;
	}

	private new bool a(Enroll A_0, int A_1, int A_2)
	{
		bool result = false;
		int num = 508;
		byte[] A_3 = new byte[num];
		byte[] bytes = BitConverter.GetBytes(A_0.DIN);
		Array.Copy(bytes, 0, A_3, 0, bytes.Length);
		A_3[8] = (byte)A_1;
		A_3[9] = (byte)A_2;
		Array.Copy(A_0.Fingerprint, A_1 * 498, A_3, 10, 498);
		if (1 == DeviceComm.e(global::k.b7, 0uL, (ulong)num))
		{
			if (1 != DeviceComm.k(ref A_3, A_3.Length))
			{
				return false;
			}
			if (1 != DeviceComm.l(ref A_3, A_3.Length))
			{
				return false;
			}
			result = Zd2911Utils.b(A_3, 8);
		}
		return result;
	}

	private new bool c(Enroll A_0, int A_1)
	{
		bool result = false;
		byte[] bytes = BitConverter.GetBytes(A_0.DIN);
		int num = bytes.Length + 8 + 2;
		byte[] A_2 = new byte[num];
		Array.Copy(bytes, 0, A_2, 0, bytes.Length);
		A_2[8] = (byte)A_1;
		A_2[9] = 1;
		byte[] bytes2 = Encoding.ASCII.GetBytes(A_0.Password);
		Array.Copy(bytes2, 0, A_2, 10, bytes2.Length);
		if (1 == DeviceComm.e(global::k.b8, 0uL, (ulong)num))
		{
			if (1 != DeviceComm.k(ref A_2, A_2.Length))
			{
				return false;
			}
			if (1 != DeviceComm.l(ref A_2, A_2.Length))
			{
				return false;
			}
			result = Zd2911Utils.b(A_2, 8);
		}
		return result;
	}

	private new bool b(Enroll A_0, int A_1)
	{
		bool result = false;
		byte[] bytes = BitConverter.GetBytes(A_0.DIN);
		int num = bytes.Length + 4 + 1;
		byte[] A_2 = new byte[num];
		Array.Copy(bytes, 0, A_2, 0, bytes.Length);
		uint value = Convert.ToUInt32(A_0.CardID);
		Array.Copy(BitConverter.GetBytes(value), 0, A_2, 8, 4);
		A_2[12] = (byte)A_1;
		if (1 == DeviceComm.e(global::k.cb, 1uL, (ulong)num))
		{
			if (1 != DeviceComm.i(ref A_2, A_2.Length))
			{
				return false;
			}
			if (1 != DeviceComm.t(ref A_2, A_2.Length))
			{
				return false;
			}
			result = Zd2911Utils.b(A_2, 8);
		}
		return result;
	}

	private new bool c(User A_0)
	{
		bool flag = false;
		Enroll enroll = A_0.Enrolls[0];
		for (int num = 0; num < 10; num++)
		{
			if (Zd2911Utils.BitCheck((int)enroll.EnrollType, num) != 0 && !a(enroll, num, A_0.Privilege))
			{
				return false;
			}
		}
		if (!string.IsNullOrEmpty(enroll.Password) && !c(enroll, A_0.Privilege))
		{
			return false;
		}
		if (Convert.ToInt32(enroll.CardID) != 0 && !b(enroll, A_0.Privilege))
		{
			return false;
		}
		if (!e(A_0))
		{
			return false;
		}
		if (!string.IsNullOrEmpty(A_0.UserName) && !g(A_0))
		{
			return false;
		}
		if (!string.IsNullOrEmpty(A_0.Comment) && !f(A_0))
		{
			return false;
		}
		return true;
	}

	private new bool b(User A_0)
	{
		bool result = false;
		if (DeviceComm.n().CommunicationType.Equals(CommunicationType.Serial))
		{
			result = c(A_0);
		}
		else
		{
			byte[] A_ = Zd2911Utils.a(A_0);
			if (1 == DeviceComm.e(global::k.dr, 0uL, (ulong)A_.Length))
			{
				if (1 != DeviceComm.k(ref A_, A_.Length))
				{
					return false;
				}
				if (1 != DeviceComm.l(ref A_, A_.Length))
				{
					return false;
				}
				result = Zd2911Utils.b(A_, 8);
			}
		}
		return result;
	}

	private new bool a(Enroll A_0, int A_1)
	{
		bool result = false;
		byte[] bytes = BitConverter.GetBytes(A_0.DIN);
		int num = bytes.Length + 1;
		byte[] A_2 = new byte[num];
		Array.Copy(bytes, 0, A_2, 0, bytes.Length);
		A_2[bytes.Length] = (byte)A_1;
		if (1 == DeviceComm.e(global::k.b1, 1uL, (ulong)num))
		{
			if (1 != DeviceComm.i(ref A_2, A_2.Length))
			{
				return false;
			}
			if (1 != DeviceComm.t(ref A_2, A_2.Length))
			{
				return false;
			}
			result = Zd2911Utils.b(A_2, 8);
		}
		return result;
	}

	private new bool b(Enroll A_0)
	{
		bool result = false;
		byte[] A_ = BitConverter.GetBytes(A_0.DIN);
		if (1 == DeviceComm.e(global::k.b2, 1uL, (ulong)A_.Length))
		{
			if (1 != DeviceComm.i(ref A_, A_.Length))
			{
				return false;
			}
			if (1 != DeviceComm.t(ref A_, A_.Length))
			{
				return false;
			}
			result = Zd2911Utils.b(A_, 8);
		}
		return result;
	}

	private new bool a(Enroll A_0)
	{
		bool result = false;
		byte[] A_ = BitConverter.GetBytes(A_0.DIN);
		if (1 == DeviceComm.e(global::k.ci, 1uL, (ulong)A_.Length))
		{
			if (1 != DeviceComm.i(ref A_, A_.Length))
			{
				return false;
			}
			if (1 != DeviceComm.t(ref A_, A_.Length))
			{
				return false;
			}
			result = Zd2911Utils.b(A_, 8);
		}
		return result;
	}

	private new bool a(User A_0)
	{
		bool flag = false;
		Enroll enroll = A_0.Enrolls[0];
		for (int num = 0; num < 10; num++)
		{
			if (Zd2911Utils.BitCheck((int)enroll.EnrollType, num) != 0 && !a(enroll, num))
			{
				return false;
			}
		}
		if (Zd2911Utils.BitCheck((int)enroll.EnrollType, 10) != 0 && !b(enroll))
		{
			return false;
		}
		if (Zd2911Utils.BitCheck((int)enroll.EnrollType, 11) != 0 && !a(enroll))
		{
			return false;
		}
		return true;
	}

	private new bool a(User A_0, int A_1)
	{
		bool result = false;
		byte[] bytes = BitConverter.GetBytes(A_0.DIN);
		int num = bytes.Length + 1;
		byte[] A_2 = new byte[num];
		Array.Copy(bytes, 0, A_2, 0, bytes.Length);
		A_2[bytes.Length] = (byte)A_1;
		if (1 == DeviceComm.e(global::k.c2, 1uL, (ulong)num))
		{
			if (1 != DeviceComm.i(ref A_2, A_2.Length))
			{
				return false;
			}
			if (1 != DeviceComm.t(ref A_2, A_2.Length))
			{
				return false;
			}
			result = Zd2911Utils.b(A_2, 8);
		}
		return result;
	}

	private new bool a(User A_0, ref int A_1)
	{
		bool result = false;
		byte[] A_2 = BitConverter.GetBytes(A_0.DIN);
		if (1 == DeviceComm.e(global::k.c3, 1uL, (ulong)A_2.Length))
		{
			if (1 != DeviceComm.i(ref A_2, A_2.Length))
			{
				return false;
			}
			if (1 != DeviceComm.t(ref A_2, A_2.Length))
			{
				return false;
			}
			if (!Zd2911Utils.b(A_2, 8))
			{
				return false;
			}
			A_1 = A_2[10];
			result = true;
		}
		return result;
	}

	private new bool a(User A_0, byte[] A_1)
	{
		bool result = false;
		byte[] bytes = BitConverter.GetBytes(A_0.DIN);
		int num = bytes.Length + A_1.Length;
		byte[] A_2 = new byte[num];
		Array.Copy(bytes, 0, A_2, 0, bytes.Length);
		Array.Copy(A_1, 0, A_2, bytes.Length, A_1.Length);
		if (1 == DeviceComm.e(global::k.c4, 1uL, (ulong)num))
		{
			if (1 != DeviceComm.i(ref A_2, A_2.Length))
			{
				return false;
			}
			if (1 != DeviceComm.t(ref A_2, A_2.Length))
			{
				return false;
			}
			result = Zd2911Utils.b(A_2, 8);
		}
		return result;
	}

	private new bool a(User A_0, ref byte[] A_1)
	{
		bool result = false;
		byte[] A_2 = BitConverter.GetBytes(A_0.DIN);
		if (1 == DeviceComm.e(global::k.c5, 1uL, (ulong)A_2.Length))
		{
			if (1 != DeviceComm.i(ref A_2, A_2.Length))
			{
				return false;
			}
			if (1 != DeviceComm.t(ref A_2, A_2.Length))
			{
				return false;
			}
			if (!Zd2911Utils.b(A_2, 8))
			{
				return false;
			}
			Array.Copy(A_2, 10, A_1, 0, A_1.Length);
			result = true;
		}
		return result;
	}
}
