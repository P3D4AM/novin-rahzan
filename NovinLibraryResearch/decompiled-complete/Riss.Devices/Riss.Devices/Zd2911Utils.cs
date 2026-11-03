using System;
using System.Collections.Generic;
using System.Text;

namespace Riss.Devices;

public class Zd2911Utils
{
	internal const ushort a = 0;

	internal const ushort b = ushort.MaxValue;

	internal const int c = 32;

	internal const int d = 16;

	internal const int e = 16;

	internal const int f = 2000;

	internal const int g = 1000;

	internal const int h = 100000;

	internal const int i = 16;

	internal const int j = 12;

	internal const int k = 8;

	internal const int l = 12;

	internal const int m = 18;

	internal const int n = 4;

	internal const uint o = 1989071004u;

	internal const uint p = 20111002u;

	internal const uint q = 4143376102u;

	internal const uint r = 2012071004u;

	internal const int s = 20;

	internal const int t = 4;

	internal const int u = 8;

	internal const int v = 116;

	internal const int w = 5096;

	internal const int x = 8;

	internal const int y = 76800;

	internal const int z = 28;

	internal const int aa = 2000;

	internal const int ab = 2099;

	public const int MaxAttendTimeZoneCount = 3;

	public const int MaxHolidayCount = 30;

	public const int MaxValidAttendTimeCount = 24;

	public const int MaxFingerprintLength = 498;

	public const int MaxFingerprintCount = 10;

	public const int GroupTimeCount = 30;

	public const int TimeZoneCount = 30;

	public const int TimeZoneWeekCount = 7;

	public const int DeviceTile = 60;

	public const int DeviceStandbyTitle = 61;

	public const int DeviceModel = 62;

	public const int DeviceFirmwareVersion = 63;

	public const int DeviceMessage = 64;

	public const int DeviceAlarmClock = 65;

	public const int DevicePowerTimer = 66;

	public const int DeviceSerialNo = 67;

	public const int DevicePassSegment = 68;

	public const int DevicePassGroup = 69;

	public const int DevicePassZone = 70;

	public const int DeviceMultOpenGroup = 71;

	public const int DeviceAccessControlSettings = 72;

	public const uint DeviceParamLock1Status = 40u;

	public const uint DeviceParamLock1Event = 41u;

	public const uint DeviceParamLock2Status = 42u;

	public const uint DeviceParamLock2Event = 43u;

	public const int PassItemCount = 32;

	public const int MultOpenGroupItemCount = 10;

	public const int AccessControlSettingsSize = 32;

	public const int PasswordLength = 8;

	public const int BellGroupCount = 24;

	public const int BellLength = 8;

	public const int PowerTimeCount = 12;

	public const int PowerTimeLength = 4;

	public const int MaxDeviceMessageLength = 84;

	public const int MaxDeviceMessageCount = 10;

	public const int DeviceBusy = 1;

	public const int DeviceIdle = 0;

	private Zd2911Utils()
	{
	}

	internal static bool b(byte[] A_0, int A_1)
	{
		if (BitConverter.ToUInt16(A_0, A_1) != 0)
		{
			return false;
		}
		return true;
	}

	internal static ushort b(int A_0, int A_1)
	{
		return (ushort)((byte)A_0 | ((byte)A_1 << 8));
	}

	internal static bool a(byte[] A_0)
	{
		int num = 0;
		int num2 = 0;
		int i;
		for (i = 0; i < 496; i++)
		{
			num2 += A_0[i];
		}
		num = b(A_0[i], A_0[i + 1]);
		if (num2 != num)
		{
			return false;
		}
		return true;
	}

	internal static int a(int A_0)
	{
		int result = 0;
		switch (A_0)
		{
		case 60:
		case 61:
			result = 64;
			break;
		case 62:
		case 67:
			result = 16;
			break;
		case 63:
			result = 16;
			break;
		case 64:
			result = 840;
			break;
		case 68:
			result = 128;
			break;
		case 69:
			result = 320;
			break;
		case 70:
			result = 224;
			break;
		case 72:
			result = 32;
			break;
		case 71:
			result = 40;
			break;
		case 65:
			result = 192;
			break;
		case 66:
			result = 96;
			break;
		}
		return result;
	}

	internal static string a(int A_0, byte[] A_1, int A_2)
	{
		string result = string.Empty;
		switch (A_0)
		{
		case 60:
		case 61:
			result = Encoding.Unicode.GetString(A_1, 12, A_2).Trim();
			break;
		case 62:
		case 63:
		case 67:
			result = Encoding.ASCII.GetString(A_1, 12, A_2).Trim();
			break;
		case 64:
		case 65:
		case 66:
		case 68:
		case 69:
		case 70:
		case 71:
		case 72:
			result = Encoding.Unicode.GetString(A_1, 12, A_2);
			break;
		}
		return result;
	}

	internal static string a(int A_0, byte[] A_1)
	{
		string result = string.Empty;
		switch (A_0)
		{
		case 60:
		case 61:
			result = Encoding.Unicode.GetString(A_1, 0, A_1.Length).Trim();
			break;
		case 62:
		case 63:
		case 67:
			result = Encoding.ASCII.GetString(A_1, 0, A_1.Length).Trim();
			break;
		case 64:
		case 65:
		case 66:
		case 68:
		case 69:
		case 70:
		case 71:
		case 72:
			result = Encoding.Unicode.GetString(A_1, 0, A_1.Length);
			break;
		}
		return result;
	}

	internal static byte[] a(int A_0, string A_1)
	{
		byte[] result = null;
		switch (A_0)
		{
		case 60:
		case 61:
			result = Encoding.Unicode.GetBytes(A_1);
			break;
		case 62:
		case 63:
			result = Encoding.ASCII.GetBytes(A_1);
			break;
		case 64:
		case 65:
		case 66:
		case 68:
		case 69:
		case 70:
		case 71:
		case 72:
			result = Encoding.Unicode.GetBytes(A_1);
			break;
		}
		return result;
	}

	internal static bool a(byte[] A_0, int A_1, ref DateTime A_2)
	{
		bool flag = false;
		try
		{
			A_2 = new DateTime(A_0[A_1] + 2000, A_0[A_1 + 1], A_0[A_1 + 2], A_0[A_1 + 3], A_0[A_1 + 4], A_0[A_1 + 5]);
			return true;
		}
		catch
		{
			return false;
		}
	}

	internal static byte[] a(DateTime A_0)
	{
		return new byte[6]
		{
			(byte)(A_0.Year - 2000),
			(byte)A_0.Month,
			(byte)A_0.Day,
			(byte)A_0.Hour,
			(byte)A_0.Minute,
			(byte)A_0.Second
		};
	}

	internal static List<byte[]> a(byte[] A_0, int A_1)
	{
		List<byte[]> list = new List<byte[]>();
		int num = A_0.Length / A_1;
		for (int i = 0; i < num; i++)
		{
			byte[] array = new byte[A_1];
			Array.Copy(A_0, i * A_1, array, 0, array.Length);
			list.Add(array);
		}
		return list;
	}

	public static int BitCheck(int num, int index)
	{
		return num & (1 << index);
	}

	public static int SetBit(int num, int index)
	{
		return num |= 1 << index;
	}

	public static byte[] CreateChunkHeader(byte[] buffer, int dataChunk)
	{
		byte[] array = new byte[8];
		Array.Copy(buffer, dataChunk, array, 0, array.Length);
		return array;
	}

	internal static byte[] f(int A_0, int A_1, int A_2)
	{
		ushort value = (ushort)((uint)A_0 | ((uint)(A_1 << 1) & 0x3Eu) | ((uint)(A_2 << 6) & 0xFFC0u));
		return BitConverter.GetBytes(value);
	}

	internal static byte[] a(int A_0, int A_1)
	{
		ushort value = (ushort)((uint)A_0 | ((uint)(A_1 << 8) & 0xFF00u));
		return BitConverter.GetBytes(value);
	}

	internal static byte[] e(int A_0, int A_1, int A_2)
	{
		ushort value = (ushort)((uint)(A_0 - 2000) | ((uint)(A_1 << 7) & 0x780u) | ((uint)(A_2 << 11) & 0xF800u));
		return BitConverter.GetBytes(value);
	}

	internal static byte[] a(int A_0, int A_1, int A_2, int A_3, int A_4, int A_5)
	{
		ushort value = (ushort)((uint)A_0 | ((uint)(A_1 << 5) & 0x7E0u) | ((uint)(A_2 << 11) & 0x1800u) | ((uint)(A_3 << 13) & 0x2000u) | ((uint)(A_4 << 14) & 0x4000u) | ((uint)(A_5 << 15) & 0x8000u));
		return BitConverter.GetBytes(value);
	}

	internal static byte[] d(int A_0, int A_1, int A_2)
	{
		ushort value = (ushort)((uint)(A_0 - 2000) | ((uint)(A_1 << 7) & 0x780u) | ((uint)(A_2 << 11) & 0xF800u));
		return BitConverter.GetBytes(value);
	}

	internal static byte[] c(int A_0, int A_1, int A_2)
	{
		ushort value = (ushort)((uint)A_0 | ((uint)(A_1 << 5) & 0x7E0u) | ((uint)(A_2 << 11) & 0xF800u));
		return BitConverter.GetBytes(value);
	}

	internal static bool t(ushort A_0)
	{
		ushort num = (ushort)(A_0 & 1u);
		if (num != 1)
		{
			return false;
		}
		return true;
	}

	internal static int s(ushort A_0)
	{
		return (A_0 & 0x3E) >> 1;
	}

	internal static int r(ushort A_0)
	{
		return (A_0 & 0xFFC0) >> 6;
	}

	internal static int q(ushort A_0)
	{
		return A_0 & 0xFF;
	}

	internal static int p(ushort A_0)
	{
		return (A_0 & 0xFF00) >> 8;
	}

	internal static int o(ushort A_0)
	{
		return (A_0 & 0x7F) + 2000;
	}

	internal static int n(ushort A_0)
	{
		return (A_0 & 0x780) >> 7;
	}

	internal static int m(ushort A_0)
	{
		return (A_0 & 0xF800) >> 11;
	}

	internal static int l(ushort A_0)
	{
		return A_0 & 0x1F;
	}

	internal static int k(ushort A_0)
	{
		return (A_0 & 0x7E0) >> 5;
	}

	internal static int j(ushort A_0)
	{
		return (A_0 & 0x1800) >> 11;
	}

	internal static int i(ushort A_0)
	{
		return (A_0 & 0x2000) >> 13;
	}

	internal static int h(ushort A_0)
	{
		return (A_0 & 0x4000) >> 14;
	}

	internal static int g(ushort A_0)
	{
		return (A_0 & 0x8000) >> 15;
	}

	internal static int f(ushort A_0)
	{
		return (A_0 & 0x7F) + 2000;
	}

	internal static int e(ushort A_0)
	{
		return (A_0 & 0x780) >> 7;
	}

	internal static int d(ushort A_0)
	{
		return (A_0 & 0xF800) >> 11;
	}

	internal static int c(ushort A_0)
	{
		return A_0 & 0x1F;
	}

	internal static int b(ushort A_0)
	{
		return (A_0 & 0x7E0) >> 5;
	}

	internal static int a(ushort A_0)
	{
		return (A_0 & 0xF800) >> 11;
	}

	internal static int k(ulong A_0)
	{
		return (int)(A_0 & 0x7F) + 2000;
	}

	internal static int j(ulong A_0)
	{
		return (int)((A_0 >> 7) & 0xF);
	}

	internal static int i(ulong A_0)
	{
		return (int)((A_0 >> 11) & 0x1F);
	}

	internal static int h(ulong A_0)
	{
		return (int)((A_0 >> 16) & 0xF);
	}

	internal static int g(ulong A_0)
	{
		return (int)((A_0 >> 20) & 0x1F);
	}

	internal static int f(ulong A_0)
	{
		return (int)((A_0 >> 25) & 0x7F);
	}

	internal static int e(ulong A_0)
	{
		return (int)((A_0 >> 32) & 1);
	}

	internal static int d(ulong A_0)
	{
		return (int)((A_0 >> 33) & 0x3FFF);
	}

	internal static int c(ulong A_0)
	{
		return (int)((A_0 >> 47) & 0x1F);
	}

	internal static int b(ulong A_0)
	{
		return (int)((A_0 >> 52) & 0x3F);
	}

	internal static int a(ulong A_0)
	{
		return (int)((A_0 >> 58) & 0x3F);
	}

	internal static byte[] b(int A_0, int A_1, int A_2)
	{
		ushort value = (ushort)((uint)(A_0 - 2000) | ((uint)(A_1 << 7) & 0x780u) | ((uint)(A_2 << 11) & 0xF800u));
		return BitConverter.GetBytes(value);
	}

	internal static byte[] a(int A_0, int A_1, int A_2)
	{
		ushort value = (ushort)((uint)A_0 | ((uint)(A_1 << 4) & 0x1F0u) | ((uint)(A_2 << 9) & 0xFE00u));
		return BitConverter.GetBytes(value);
	}

	internal static byte[] a(int A_0, int A_1, int A_2, int A_3, int A_4)
	{
		uint value = (uint)A_0 | ((uint)(A_1 << 1) & 0x7FFEu) | ((uint)(A_2 << 15) & 0xF8000u) | ((uint)(A_3 << 20) & 0x3F00000u) | (uint)(int)((A_4 << 26) & 0xFC000000u);
		return BitConverter.GetBytes(value);
	}

	internal static byte[] a(User A_0)
	{
		int num = 0;
		int num2 = 0;
		byte[] array = new byte[4980];
		byte[] array2 = new byte[5096];
		byte[] bytes = BitConverter.GetBytes(A_0.DIN);
		Array.Copy(bytes, 0, array2, num, bytes.Length);
		num += bytes.Length;
		Enroll enroll = A_0.Enrolls[0];
		byte[] array3 = new byte[20];
		for (int i = 0; i < 10; i++)
		{
			if (BitCheck((int)enroll.EnrollType, i) != 0)
			{
				byte[] bytes2 = BitConverter.GetBytes((ushort)(num2 + 1));
				Array.Copy(bytes2, 0, array3, i * 2, bytes2.Length);
				Array.Copy(enroll.Fingerprint, i * 498, array, i * 498, 498);
				num2++;
			}
		}
		Array.Copy(array3, 0, array2, num, array3.Length);
		num += array3.Length;
		byte[] array4 = new byte[8];
		if (BitCheck((int)enroll.EnrollType, 10) != 0)
		{
			byte[] bytes3 = Encoding.ASCII.GetBytes(enroll.Password);
			Array.Copy(bytes3, 0, array4, 0, bytes3.Length);
		}
		Array.Copy(array4, 0, array2, num, array4.Length);
		num += array4.Length;
		byte[] array5 = new byte[4];
		if (BitCheck((int)enroll.EnrollType, 11) != 0)
		{
			array5 = BitConverter.GetBytes(Convert.ToInt32(enroll.CardID));
		}
		Array.Copy(array5, 0, array2, num, array5.Length);
		num += array5.Length;
		byte[] array6 = new byte[24];
		if (!string.IsNullOrEmpty(A_0.UserName))
		{
			byte[] bytes4 = Encoding.Unicode.GetBytes(A_0.UserName);
			Array.Copy(bytes4, 0, array6, 0, bytes4.Length);
		}
		Array.Copy(array6, 0, array2, num, array6.Length);
		num += array6.Length;
		byte[] array7 = new byte[36];
		if (!string.IsNullOrEmpty(A_0.Comment))
		{
			byte[] bytes5 = Encoding.Unicode.GetBytes(A_0.Comment);
			Array.Copy(bytes5, 0, array7, 0, bytes5.Length);
		}
		Array.Copy(array7, 0, array2, num, array7.Length);
		num += array7.Length;
		byte[] array8 = new byte[12];
		byte[] array9 = f(A_0.Enable ? 1 : 0, A_0.Privilege, A_0.UserGroup);
		Array.Copy(array9, 0, array8, 0, array9.Length);
		byte[] array10 = a(A_0.AccessTimeZone, A_0.AttType);
		Array.Copy(array10, 0, array8, 2, array10.Length);
		byte[] array11 = e(A_0.ValidDate.Year, A_0.ValidDate.Month, A_0.ValidDate.Day);
		Array.Copy(array11, 0, array8, 4, array11.Length);
		byte[] array12 = a(A_0.ValidDate.Hour, A_0.ValidDate.Minute, A_0.AccessControl, A_0.ValidityPeriod ? 1 : 0, A_0.UseUserGroupACTZ ? 1 : 0, A_0.UseUserGroupVM ? 1 : 0);
		Array.Copy(array12, 0, array8, 6, array12.Length);
		byte[] array13 = d(A_0.InvalidDate.Year, A_0.InvalidDate.Month, A_0.InvalidDate.Day);
		Array.Copy(array13, 0, array8, 8, array13.Length);
		byte[] array14 = c(A_0.InvalidDate.Hour, A_0.InvalidDate.Minute, A_0.Department);
		Array.Copy(array14, 0, array8, 10, array14.Length);
		Array.Copy(array8, 0, array2, num, array8.Length);
		num += array8.Length;
		byte[] bytes6 = BitConverter.GetBytes(A_0.Res);
		Array.Copy(bytes6, 0, array2, num, bytes6.Length);
		num += 4;
		Array.Copy(array, 0, array2, num, array.Length);
		return array2;
	}

	internal static bool a(byte[] A_0, ref User A_1)
	{
		int num = 0;
		int num2 = 0;
		Enroll enroll = new Enroll();
		A_1.DIN = BitConverter.ToUInt64(A_0, 0);
		num = 8;
		byte[] array = new byte[20];
		Array.Copy(A_0, num, array, 0, array.Length);
		ushort[] array2 = new ushort[10];
		for (int i = 0; i < 10; i++)
		{
			if ((array2[i] = BitConverter.ToUInt16(array, i * 2)) != 0)
			{
				num2 = SetBit(num2, i);
			}
		}
		num += array.Length;
		enroll.DIN = A_1.DIN;
		if (A_0[num] != 0)
		{
			enroll.Password = Encoding.ASCII.GetString(A_0, num, 8).Replace("\0", "");
			num2 = SetBit(num2, 10);
		}
		num += 8;
		int num3 = BitConverter.ToInt32(A_0, num);
		if (num3 != 0)
		{
			enroll.CardID = num3.ToString();
			num2 = SetBit(num2, 11);
		}
		enroll.EnrollType = (EnrollType)num2;
		num += 4;
		if (A_0[num] != 0)
		{
			A_1.UserName = Encoding.Unicode.GetString(A_0, num, 24).Replace("\0", "");
		}
		num += 24;
		if (A_0[num] != 0)
		{
			A_1.Comment = Encoding.Unicode.GetString(A_0, num, 36).Replace("\0", "");
		}
		num += 36;
		byte[] array3 = new byte[12];
		Array.Copy(A_0, num, array3, 0, array3.Length);
		ushort a_ = BitConverter.ToUInt16(array3, 0);
		A_1.Enable = t(a_);
		A_1.Privilege = s(a_);
		A_1.UserGroup = r(a_);
		ushort a_2 = BitConverter.ToUInt16(array3, 2);
		A_1.AccessTimeZone = q(a_2);
		A_1.AttType = p(a_2);
		ushort a_3 = BitConverter.ToUInt16(array3, 4);
		int year = o(a_3);
		int month = n(a_3);
		int day = m(a_3);
		ushort a_4 = BitConverter.ToUInt16(array3, 6);
		int hour = l(a_4);
		int minute = k(a_4);
		A_1.ValidDate = new DateTime(year, month, day, hour, minute, 0);
		A_1.AccessControl = j(a_4);
		A_1.ValidityPeriod = ((1 == Zd2911Utils.i(a_4)) ? true : false);
		A_1.UseUserGroupACTZ = ((1 == h(a_4)) ? true : false);
		A_1.UseUserGroupVM = ((1 == g(a_4)) ? true : false);
		ushort a_5 = BitConverter.ToUInt16(array3, 8);
		int year2 = f(a_5);
		int month2 = e(a_5);
		int day2 = d(a_5);
		ushort a_6 = BitConverter.ToUInt16(array3, 10);
		int hour2 = c(a_6);
		int minute2 = b(a_6);
		A_1.InvalidDate = new DateTime(year2, month2, day2, hour2, minute2, 0);
		A_1.Department = a(a_6);
		num += array3.Length;
		byte[] array4 = new byte[4];
		Array.Copy(A_0, num, array4, 0, array4.Length);
		A_1.Res = BitConverter.ToUInt32(array4, 0);
		num += 4;
		byte[] array5 = new byte[4980];
		Array.Copy(A_0, num, array5, 0, array5.Length);
		List<byte[]> list = a(array5, 498);
		foreach (byte[] item in list)
		{
			if (!a(item))
			{
				return false;
			}
		}
		enroll.Fingerprint = array5;
		A_1.Enrolls = new List<Enroll>();
		A_1.Enrolls.Add(enroll);
		return true;
	}
}
