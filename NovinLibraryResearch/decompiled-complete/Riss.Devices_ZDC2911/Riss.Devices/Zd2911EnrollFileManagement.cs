using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Riss.Devices;

public class Zd2911EnrollFileManagement
{
	public bool SaveAllUserEnrollDataAsDB(string fileName, List<User> userList)
	{
		bool flag = false;
		int num = 0;
		if (userList.Count == 0)
		{
			return false;
		}
		FileStream fileStream = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite);
		BinaryWriter binaryWriter = new BinaryWriter(fileStream);
		binaryWriter.Seek(0, SeekOrigin.Begin);
		byte[] array = new byte[20];
		byte[] bytes = BitConverter.GetBytes(1989071004u);
		Array.Copy(bytes, 0, array, 0, bytes.Length);
		byte[] bytes2 = BitConverter.GetBytes(userList.Count);
		Array.Copy(bytes2, 0, array, bytes.Length, bytes2.Length);
		binaryWriter.Write(array);
		byte[] array2 = new byte[116 * userList.Count];
		List<byte[]> list = new List<byte[]>();
		int num2 = 0;
		foreach (User user in userList)
		{
			byte[] array3 = new byte[116];
			byte[] bytes3 = BitConverter.GetBytes(user.DIN);
			Array.Copy(bytes3, 0, array3, 0, bytes3.Length);
			Enroll enroll = user.Enrolls[0];
			byte[] array4 = new byte[20];
			for (int i = 0; i < 10; i++)
			{
				if (Zd2911Utils.BitCheck((int)enroll.EnrollType, i) != 0)
				{
					byte[] bytes4 = BitConverter.GetBytes((ushort)(num + 1));
					Array.Copy(bytes4, 0, array4, i * 2, bytes4.Length);
					byte[] array5 = new byte[498];
					Array.Copy(enroll.Fingerprint, i * 498, array5, 0, 498);
					list.Add(array5);
					num++;
				}
			}
			Array.Copy(array4, 0, array3, bytes3.Length, array4.Length);
			byte[] array6 = new byte[8];
			if (Zd2911Utils.BitCheck((int)enroll.EnrollType, 10) != 0)
			{
				byte[] bytes5 = Encoding.ASCII.GetBytes(enroll.Password);
				Array.Copy(bytes5, 0, array6, 0, bytes5.Length);
			}
			Array.Copy(array6, 0, array3, bytes3.Length + array4.Length, array6.Length);
			byte[] array7 = new byte[4];
			if (Zd2911Utils.BitCheck((int)enroll.EnrollType, 11) != 0)
			{
				array7 = BitConverter.GetBytes(Convert.ToInt32(enroll.CardID));
			}
			Array.Copy(array7, 0, array3, bytes3.Length + array4.Length + array6.Length, array7.Length);
			byte[] array8 = new byte[24];
			if (!string.IsNullOrEmpty(user.UserName))
			{
				byte[] bytes6 = Encoding.Unicode.GetBytes(user.UserName);
				Array.Copy(bytes6, 0, array8, 0, bytes6.Length);
			}
			Array.Copy(array8, 0, array3, bytes3.Length + array4.Length + array6.Length + array7.Length, array8.Length);
			byte[] array9 = new byte[36];
			if (!string.IsNullOrEmpty(user.Comment))
			{
				byte[] bytes7 = Encoding.Unicode.GetBytes(user.Comment);
				Array.Copy(bytes7, 0, array9, 0, bytes7.Length);
			}
			Array.Copy(array9, 0, array3, bytes3.Length + array4.Length + array6.Length + array7.Length + array8.Length, array9.Length);
			byte[] array10 = new byte[12];
			byte[] array11 = Zd2911Utils.f(user.Enable ? 1 : 0, user.Privilege, user.UserGroup);
			Array.Copy(array11, 0, array10, 0, array11.Length);
			byte[] array12 = Zd2911Utils.a(user.AccessTimeZone, user.AttType);
			Array.Copy(array12, 0, array10, 2, array12.Length);
			byte[] array13 = Zd2911Utils.e(user.ValidDate.Year, user.ValidDate.Month, user.ValidDate.Day);
			Array.Copy(array13, 0, array10, 4, array13.Length);
			byte[] array14 = Zd2911Utils.a(user.ValidDate.Hour, user.ValidDate.Minute, user.AccessControl, user.ValidityPeriod ? 1 : 0, user.UseUserGroupACTZ ? 1 : 0, user.UseUserGroupVM ? 1 : 0);
			Array.Copy(array14, 0, array10, 6, array14.Length);
			byte[] array15 = Zd2911Utils.d(user.InvalidDate.Year, user.InvalidDate.Month, user.InvalidDate.Day);
			Array.Copy(array15, 0, array10, 8, array15.Length);
			byte[] array16 = Zd2911Utils.c(user.InvalidDate.Hour, user.InvalidDate.Minute, user.Department);
			Array.Copy(array16, 0, array10, 10, array16.Length);
			Array.Copy(array10, 0, array3, bytes3.Length + array4.Length + array6.Length + array7.Length + array8.Length + array9.Length, array10.Length);
			Array.Copy(array3, 0, array2, num2 * array3.Length, array3.Length);
			num2++;
		}
		binaryWriter.Write(array2);
		int num3 = 0;
		byte[] array17 = new byte[498 * list.Count];
		foreach (byte[] item in list)
		{
			Array.Copy(item, 0, array17, num3 * 498, item.Length);
			num3++;
		}
		binaryWriter.Write(array17);
		binaryWriter.Flush();
		binaryWriter.Close();
		fileStream.Close();
		return true;
	}

	public bool SaveUserEnrollDataAsDB(string fileName, User user)
	{
		bool flag = false;
		FileStream fileStream = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite);
		BinaryWriter binaryWriter = new BinaryWriter(fileStream);
		binaryWriter.Seek(0, SeekOrigin.Begin);
		byte[] bytes = BitConverter.GetBytes(20111002u);
		binaryWriter.Write(bytes);
		byte[] buffer = Zd2911Utils.a(user);
		binaryWriter.Write(buffer);
		binaryWriter.Flush();
		binaryWriter.Close();
		fileStream.Close();
		return true;
	}

	public bool LoadAllUserEnrollDataFromDB(string fileName, ref List<User> userList)
	{
		bool flag = false;
		Dictionary<User, ushort[]> dictionary = new Dictionary<User, ushort[]>();
		FileStream input = new FileStream(fileName, FileMode.Open, FileAccess.Read);
		BinaryReader binaryReader = new BinaryReader(input);
		byte[] array = binaryReader.ReadBytes(20);
		if (array == null || array.Length == 0)
		{
			return false;
		}
		uint num = BitConverter.ToUInt32(array, 0);
		int num2 = BitConverter.ToInt32(array, 4);
		if (1989071004 != num)
		{
			return false;
		}
		if (num2 == 0)
		{
			return false;
		}
		byte[] array2 = binaryReader.ReadBytes(num2 * 116);
		if (array2 == null || array2.Length == 0)
		{
			return false;
		}
		int num3 = 0;
		List<byte[]> list = Zd2911Utils.a(array2, 116);
		foreach (byte[] item in list)
		{
			int num4 = 0;
			int num5 = 0;
			User user = new User();
			Enroll enroll = new Enroll();
			user.DIN = BitConverter.ToUInt64(item, 0);
			num4 = 8;
			byte[] array3 = new byte[20];
			Array.Copy(item, num4, array3, 0, array3.Length);
			ushort[] array4 = new ushort[10];
			for (int i = 0; i < 10; i++)
			{
				if ((array4[i] = BitConverter.ToUInt16(array3, i * 2)) != 0)
				{
					num3++;
					num5 = Zd2911Utils.SetBit(num5, i);
				}
			}
			num4 += array3.Length;
			enroll.DIN = user.DIN;
			if (item[num4] != 0)
			{
				enroll.Password = Encoding.ASCII.GetString(item, num4, 8).Replace("\0", "");
				num5 = Zd2911Utils.SetBit(num5, 10);
			}
			num4 += 8;
			int num6 = BitConverter.ToInt32(item, num4);
			if (num6 != 0)
			{
				enroll.CardID = num6.ToString();
				num5 = Zd2911Utils.SetBit(num5, 11);
			}
			enroll.EnrollType = (EnrollType)num5;
			num4 += 4;
			if (item[num4] != 0)
			{
				user.UserName = Encoding.Unicode.GetString(item, num4, 24).Replace("\0", "");
			}
			num4 += 24;
			if (item[num4] != 0)
			{
				user.Comment = Encoding.Unicode.GetString(item, num4, 36).Replace("\0", "");
			}
			num4 += 36;
			byte[] array5 = new byte[12];
			Array.Copy(item, num4, array5, 0, array5.Length);
			ushort a_ = BitConverter.ToUInt16(array5, 0);
			user.Enable = Zd2911Utils.t(a_);
			user.Privilege = Zd2911Utils.s(a_);
			user.UserGroup = Zd2911Utils.r(a_);
			ushort a_2 = BitConverter.ToUInt16(array5, 2);
			user.AccessTimeZone = Zd2911Utils.q(a_2);
			user.AttType = Zd2911Utils.p(a_2);
			ushort a_3 = BitConverter.ToUInt16(array5, 4);
			int year = Zd2911Utils.o(a_3);
			int month = Zd2911Utils.n(a_3);
			int day = Zd2911Utils.m(a_3);
			ushort a_4 = BitConverter.ToUInt16(array5, 6);
			int hour = Zd2911Utils.l(a_4);
			int minute = Zd2911Utils.k(a_4);
			user.ValidDate = new DateTime(year, month, day, hour, minute, 0);
			user.AccessControl = Zd2911Utils.j(a_4);
			user.ValidityPeriod = ((1 == Zd2911Utils.i(a_4)) ? true : false);
			user.UseUserGroupACTZ = ((1 == Zd2911Utils.h(a_4)) ? true : false);
			user.UseUserGroupVM = ((1 == Zd2911Utils.g(a_4)) ? true : false);
			ushort a_5 = BitConverter.ToUInt16(array5, 8);
			int year2 = Zd2911Utils.f(a_5);
			int month2 = Zd2911Utils.e(a_5);
			int day2 = Zd2911Utils.d(a_5);
			ushort a_6 = BitConverter.ToUInt16(array5, 10);
			int hour2 = Zd2911Utils.c(a_6);
			int minute2 = Zd2911Utils.b(a_6);
			user.InvalidDate = new DateTime(year2, month2, day2, hour2, minute2, 0);
			user.Department = Zd2911Utils.a(a_6);
			user.Enrolls.Add(enroll);
			userList.Add(user);
			dictionary.Add(user, array4);
		}
		byte[] a_7 = binaryReader.ReadBytes(num3 * 498);
		List<byte[]> list2 = Zd2911Utils.a(a_7, 498);
		foreach (byte[] item2 in list2)
		{
			if (!Zd2911Utils.a(item2))
			{
				return false;
			}
		}
		foreach (KeyValuePair<User, ushort[]> item3 in dictionary)
		{
			new List<byte[]>();
			Enroll enroll2 = item3.Key.Enrolls[0];
			byte[] array6 = new byte[4980];
			for (int j = 0; j < 10; j++)
			{
				ushort num7 = item3.Value[j];
				if (num7 != 0)
				{
					Array.Copy(list2[num7 - 1], 0, array6, j * 498, 498);
				}
			}
			enroll2.Fingerprint = array6;
			item3.Key.Enrolls[0] = enroll2;
		}
		return true;
	}

	public bool LoadUserEnrollDataFromDB(string fileName, ref User user)
	{
		bool flag = false;
		FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
		BinaryReader binaryReader = new BinaryReader(fileStream);
		byte[] array = binaryReader.ReadBytes(4);
		if (array == null || array.Length == 0)
		{
			return false;
		}
		uint num = BitConverter.ToUInt32(array, 0);
		if (20111002 != num)
		{
			return false;
		}
		byte[] array2 = binaryReader.ReadBytes(5096);
		if (array2 == null || array2.Length == 0)
		{
			return false;
		}
		if (!Zd2911Utils.a(array2, ref user))
		{
			return false;
		}
		binaryReader.Close();
		fileStream.Close();
		return true;
	}

	public bool SaveUserNameData(string fileName, List<User> userList)
	{
		bool flag = false;
		if (userList.Count == 0)
		{
			return false;
		}
		int num = 0;
		foreach (User user in userList)
		{
			if (!string.IsNullOrEmpty(user.UserName))
			{
				num++;
			}
		}
		if (num == 0)
		{
			return false;
		}
		FileStream fileStream = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite);
		BinaryWriter binaryWriter = new BinaryWriter(fileStream);
		binaryWriter.Seek(0, SeekOrigin.Begin);
		byte[] bytes = BitConverter.GetBytes(4143376102u);
		byte[] bytes2 = BitConverter.GetBytes(num);
		byte[] array = new byte[8];
		Array.Copy(bytes, 0, array, 0, bytes.Length);
		Array.Copy(bytes2, 0, array, bytes.Length, bytes2.Length);
		binaryWriter.Write(array, 0, array.Length);
		byte[] array2 = new byte[32 * num];
		int num2 = 0;
		foreach (User user2 in userList)
		{
			if (!string.IsNullOrEmpty(user2.UserName))
			{
				byte[] array3 = new byte[32];
				byte[] bytes3 = BitConverter.GetBytes(user2.DIN);
				Array.Copy(bytes3, 0, array3, 0, bytes3.Length);
				byte[] bytes4 = Encoding.Unicode.GetBytes(user2.UserName);
				Array.Copy(bytes4, 0, array3, bytes3.Length, bytes4.Length);
				Array.Copy(array3, 0, array2, num2 * array3.Length, array3.Length);
				num2++;
			}
		}
		binaryWriter.Write(array2, 0, array2.Length);
		binaryWriter.Flush();
		binaryWriter.Close();
		fileStream.Close();
		return true;
	}

	public bool LoadUserNameData(string fileName, ref List<User> userList)
	{
		bool flag = false;
		FileStream input = new FileStream(fileName, FileMode.Open, FileAccess.Read);
		BinaryReader binaryReader = new BinaryReader(input);
		byte[] value = binaryReader.ReadBytes(8);
		uint num = BitConverter.ToUInt32(value, 0);
		int num2 = BitConverter.ToInt32(value, 4);
		if (-151591194 != (int)num)
		{
			return false;
		}
		if (num2 == 0)
		{
			return false;
		}
		byte[] a_ = binaryReader.ReadBytes(num2 * 32);
		List<byte[]> list = Zd2911Utils.a(a_, 32);
		foreach (byte[] item in list)
		{
			User user = new User();
			user.DIN = BitConverter.ToUInt64(item, 0);
			user.UserName = Encoding.Unicode.GetString(item, 8, 24).Replace("\0", "");
			userList.Add(user);
		}
		return true;
	}
}
