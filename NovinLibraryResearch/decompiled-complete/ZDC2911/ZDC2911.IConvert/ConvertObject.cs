using System;
using System.Text;
using System.Text.RegularExpressions;
using Riss.Devices;
using ZDC2911.SysEnum;

namespace ZDC2911.IConvert;

public class ConvertObject
{
	private ConvertObject()
	{
	}

	public static int ConvertIPAddressToNumber(string strIPAddress)
	{
		string[] array = strIPAddress.Split(new char[1] { '.' });
		int num = int.Parse(array[0]);
		int num2 = int.Parse(array[1]);
		int num3 = int.Parse(array[2]);
		int num4 = int.Parse(array[3]);
		return (num << 24) + (num2 << 16) + (num3 << 8) + num4;
	}

	public static string ConvertNumberToIPAddress(int intIPAddress)
	{
		byte[] bytes = BitConverter.GetBytes(intIPAddress);
		return $"{bytes[3]}.{bytes[2]}.{bytes[1]}.{bytes[0]}";
	}

	public static bool IsCorrenctIP(string ip)
	{
		if (Regex.IsMatch(ip, "[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}"))
		{
			string[] array = ip.Split(new char[1] { '.' });
			if (4 == array.Length)
			{
				if (int.Parse(array[0]) < 256 && int.Parse(array[1]) < 256 && int.Parse(array[2]) < 256 && int.Parse(array[3]) < 256)
				{
					return true;
				}
				return false;
			}
			return false;
		}
		return false;
	}

	public static bool IsInt(string content)
	{
		if (Regex.IsMatch(content, "^\\d+$"))
		{
			return true;
		}
		return false;
	}

	public static string ConvertByteToHex(byte[] bytes)
	{
		string text = string.Empty;
		for (int i = 0; i < bytes.Length; i++)
		{
			text = text + "-" + Convert.ToString(bytes[i], 16).PadLeft(2, '0');
		}
		return text.Remove(0, 1).ToUpper();
	}

	public static string SLogType(int opertationType)
	{
		string result = string.Empty;
		switch (opertationType)
		{
		case 1:
			result = "Registered user";
			break;
		case 2:
			result = "Register super administrator";
			break;
		case 3:
			result = "Register registrar";
			break;
		case 4:
			result = "Register query member";
			break;
		case 5:
			result = "Delete fingerprint";
			break;
		case 6:
			result = "Remove the password";
			break;
		case 7:
			result = "Delete the card number";
			break;
		case 8:
			result = "Remove access to records";
			break;
		case 9:
			result = "Remove records";
			break;
		case 10:
			result = "Set system information";
			break;
		case 11:
			result = "Setup time";
			break;
		case 12:
			result = "Set the record information";
			break;
		case 13:
			result = "Set the communications and information";
			break;
		case 14:
			result = "Set the access control information";
			break;
		case 15:
			result = "Set the user type";
			break;
		case 16:
			result = "Set the attendance time";
			break;
		}
		return result;
	}

	public static string IOMode(int mode)
	{
		string result = string.Empty;
		switch (mode)
		{
		case 0:
			result = "Check in";
			break;
		case 1:
			result = "Clock in";
			break;
		case 2:
			result = "Clock out";
			break;
		case 3:
			result = "Customer in";
			break;
		case 4:
			result = "Customer out";
			break;
		case 5:
			result = "Out";
			break;
		case 6:
			result = "In";
			break;
		case 7:
			result = "User defined 1";
			break;
		case 8:
			result = "User defined 2";
			break;
		case 12:
			result = "Button open";
			break;
		case 13:
			result = "Software open";
			break;
		case 14:
			result = "Keep open";
			break;
		case 15:
			result = "Keep close";
			break;
		case 16:
			result = "Auto mode";
			break;
		case 17:
			result = "Open in";
			break;
		case 18:
			result = "Open out";
			break;
		case 19:
			result = "Overtime open alarm";
			break;
		case 20:
			result = "Forced open alarm";
			break;
		case 21:
			result = "Antihijack alarm";
			break;
		case 22:
			result = "Input action 1";
			break;
		case 23:
			result = "Input action 2";
			break;
		case 24:
			result = "Output action 1";
			break;
		case 25:
			result = "Output action 2";
			break;
		case 26:
			result = "Invalid time";
			break;
		case 27:
			result = "Invalid date";
			break;
		case 31:
			result = "Illegal operation";
			break;
		}
		return result;
	}

	public static string GLogType(int opertationType)
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < 4; i++)
		{
			if (Zd2911Utils.BitCheck(opertationType, i) != 0)
			{
				switch (i)
				{
				case 0:
					stringBuilder.Append("F ");
					break;
				case 1:
					stringBuilder.Append("P ");
					break;
				case 2:
					stringBuilder.Append("C ");
					break;
				case 3:
					stringBuilder.Append("I ");
					break;
				}
			}
		}
		return stringBuilder.ToString();
	}

	public static string GetUserPrivilege(UserPrivilege privilege)
	{
		string result = string.Empty;
		switch (privilege)
		{
		case UserPrivilege.ROLE_GENERAL_USER:
			result = "General user";
			break;
		case UserPrivilege.ROLE_SUPER_USER:
			result = "Super user";
			break;
		case UserPrivilege.ROLE_ENROLL_USER:
			result = "Enroll user";
			break;
		case UserPrivilege.ROLE_VIEW_USER:
			result = "View user";
			break;
		case UserPrivilege.ROLE_CUSTOMER:
			result = "Customer";
			break;
		}
		return result;
	}
}
