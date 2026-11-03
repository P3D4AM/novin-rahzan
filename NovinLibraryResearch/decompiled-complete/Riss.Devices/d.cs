using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Riss.Devices;

internal class d : r
{
	private new const int m_a = 1543;

	private new const int m_b = 1537;

	private new const int m_c = 1545;

	public d()
	{
		DeviceComm = new l();
	}

	public override bool SetProperty(UserProperty property, object extraProperty, User user, object extraData)
	{
		bool flag = false;
		switch (property)
		{
		case UserProperty.UserName:
		{
			a a2 = default(a);
			a2.a = (ushort)user.DIN;
			a2.d = new ushort[26];
			a2.e = new byte[384];
			byte[] nameInfo = new byte[26];
			a2.c = EncodeName(user.UserName, ref nameInfo, ref a2.e, 24);
			for (int num = 0; num < 24; num++)
			{
				a2.d[num] = nameInfo[num];
			}
			a2.d[24] = (ushort)((nameInfo[23] << 8) | nameInfo[24]);
			if (a2.c < 100)
			{
				a2.c = -100;
			}
			byte[] a_ = global::u.a(a2, Marshal.SizeOf(typeof(a)));
			return a(user.DIN, global::r.d.u, a_);
		}
		case UserProperty.Privilege:
		{
			int privilege = user.Privilege;
			byte[] bytes = BitConverter.GetBytes(privilege);
			return a(user.DIN, global::r.d.q, bytes);
		}
		default:
			return base.SetProperty(property, extraProperty, user, extraData);
		}
	}

	public override bool GetProperty(UserProperty property, object extraProperty, ref User user, ref object extraData)
	{
		bool result = false;
		switch (property)
		{
		case UserProperty.UserName:
			if (z)
			{
				byte[] A_2 = new byte[50];
				result = a(global::r.d.u, user.DIN, ref A_2);
				byte[] array = new byte[A_2.Length / 2];
				for (int num = 0; num < A_2.Length; num++)
				{
					if (num % 2 == 0)
					{
						array[num / 2] = A_2[num];
					}
				}
				byte b2 = (byte)(A_2[^2] & 0xFFu);
				int charType = A_2[^1];
				byte[] array2 = new byte[b2];
				Array.Copy(array, array2, b2);
				user.UserName = GetDecodeName(array2, charType);
			}
			else
			{
				result = base.GetProperty(property, extraProperty, ref user, ref extraData);
			}
			break;
		case UserProperty.Privilege:
			if (user.Enrolls.Count != 0)
			{
				_ = user.Enrolls[0];
				byte[] A_ = null;
				result = a(global::r.d.q, user.DIN, ref A_);
			}
			break;
		default:
			result = base.GetProperty(property, extraProperty, ref user, ref extraData);
			break;
		}
		return result;
	}

	public override bool SetProperty(DeviceProperty property, object extraProperty, Device device, object extraData)
	{
		bool result = false;
		if (property == DeviceProperty.DoorControl)
		{
			DoorStatus doorStatus = (DoorStatus)extraData;
			if (doorStatus == DoorStatus.ReStart)
			{
				result = a();
			}
		}
		else
		{
			result = base.SetProperty(property, extraProperty, device, extraData);
		}
		return result;
	}

	protected override bool a(ref List<User> A_0)
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
						if (((b2.b >> num3) & 1) == 0)
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
							user2.DIN = b2.a;
							user2.Enable = ((b2.b >> 31) & 1) != 0;
							user2.AccessTimeZone = (int)((b2.b >> 24) & 1);
							user2.UserGroup = (int)((b2.b >> 25) & 1);
							A_0.Add(user2);
							user = user2;
						}
						Enroll enroll = new Enroll();
						enroll.EnrollType = (EnrollType)num3;
						user.Enrolls.Add(enroll);
					}
				}
			}
		}
		return false;
	}

	protected override bool b(bool A_0, ref List<Record> A_1)
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
			int num = 8;
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
						g g2 = global::u.BytesToStruct<g>(array, 0, num);
						a(BitConverter.GetBytes((int)g2.a), ref A_6);
						record.DIN = g2.b;
						record.Verify = g2.d;
						record.Clock = A_6;
						A_1.Add(record);
					}
				}
			}
		}
		return flag;
	}

	protected override bool c(byte[] A_0, int A_1)
	{
		ulong A_2 = 0uL;
		if (A_1 <= 0 || BitConverter.ToUInt32(A_0, 0) != 3852464152u)
		{
			return false;
		}
		ulong A_3 = 0uL;
		bool flag = false;
		if (DeviceComm.g(global::k.a3) && DeviceComm.a(ref A_3) == global::j.a.b)
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
		int num2;
		for (int num = 0; num < A_1; num += num2)
		{
			num2 = A_1 - num;
			if ((long)num2 > 4096L)
			{
				num2 = 4096;
			}
			byte[] A_5 = new byte[4096];
			for (int num3 = 0; (long)num3 < 4096L; num3++)
			{
				A_5[num3] = ((num3 < num2) ? A_0[num + num3] : byte.MaxValue);
			}
			if (DeviceComm.i(ref A_5, A_5.Length) != 1)
			{
				return false;
			}
			if (DeviceComm.a(ref A_3) != global::j.a.b)
			{
				return false;
			}
		}
		return true;
	}

	protected override bool d(ref byte[] A_0, bool A_1, byte[] A_2)
	{
		if (A_1 && DeviceComm.g(global::k.a5))
		{
			ulong A_3 = 0uL;
			if (DeviceComm.a(ref A_3) == global::j.a.b && DeviceComm.i(ref A_0, A_0.Length) == 1)
			{
				return DeviceComm.a(ref A_3) == global::j.a.b;
			}
		}
		return false;
	}

	protected new bool a()
	{
		ulong A_ = 0uL;
		if (DeviceComm.g(global::k.ax))
		{
			return DeviceComm.a(ref A_) == global::j.a.b;
		}
		return false;
	}
}
