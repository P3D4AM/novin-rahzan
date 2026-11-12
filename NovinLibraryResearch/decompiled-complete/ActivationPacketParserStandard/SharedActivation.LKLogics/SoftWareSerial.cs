using System;
using PacketParser;
using PacketParser.Entities;

namespace SharedActivation.LKLogics;

public class SoftWareSerial
{
	public string SoftSerial { get; private set; }

	public bool Isvalid { get; private set; } = true;


	public string Version { get; set; }

	public Package InstancePackage { get; set; }

	public SoftWareSerial(string serialNumber)
	{
		ParseSerial(serialNumber);
	}

	public void ParseSerial(string serial)
	{
		try
		{
			if (serial.Length == 16)
			{
				SoftSerial = serial;
				Version = serial.Substring(2, 4);
				InstancePackage = ParsePackage(serial.Substring(0, 2));
			}
			else if (serial.Length == 10)
			{
				InstancePackage = Package.Accounting;
				SoftSerial = serial;
			}
			else if (serial.Length == 14)
			{
				SoftSerial = serial;
				Version = serial.Substring(0, 4);
				InstancePackage = Package.Accounting;
			}
			else if (serial.Length == 12)
			{
				SoftSerial = serial;
				InstancePackage = Package.Accounting;
			}
			else if (serial.Length == 17)
			{
				if (!ValidCheckSum(serial))
				{
					Isvalid = false;
					return;
				}
				SoftSerial = serial;
				Version = serial.Substring(2, 4);
				InstancePackage = ParsePackage(serial.Substring(0, 2));
			}
			else
			{
				Isvalid = false;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ParseSerial", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\LKLogics\\SoftWareSerial.cs", 63);
			Isvalid = false;
		}
	}

	private bool ValidCheckSum(string serial)
	{
		try
		{
			int num = 0;
			int num2 = 3;
			string text = serial.Substring(16, 1);
			serial = serial.Remove(16, 1);
			char[] array = serial.ToCharArray();
			Array.Reverse((Array)array);
			serial = new string(array);
			string text2 = serial;
			for (int i = 0; i < text2.Length; i++)
			{
				char c = text2[i];
				if (c >= '0' && c <= '9')
				{
					num += c.ToString().ParseToShort(0) * num2;
					num2 = ((num2 == 3) ? 1 : 3);
				}
			}
			int num3 = 10 - num % 10;
			if (num3 >= 10)
			{
				num3 = 0;
			}
			if ((text ?? "") == (num3.ToString() ?? ""))
			{
				return true;
			}
			return false;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ValidCheckSum", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\LKLogics\\SoftWareSerial.cs", 96);
			return false;
		}
	}

	public Package ParsePackage(string type)
	{
		try
		{
			if (!(type == "01"))
			{
				if (type == "05")
				{
					return Package.HoghoghVaDastmozd;
				}
				Isvalid = false;
				return Package.None;
			}
			return Package.Accounting;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ParsePackage", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\LKLogics\\SoftWareSerial.cs", 115);
			Isvalid = false;
			return Package.None;
		}
	}
}
