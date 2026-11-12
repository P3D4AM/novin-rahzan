using System;
using PacketParser;
using PacketParser.Entities;

namespace SharedActivation.LKLogics;

public class ClsActivation
{
	public static string _numberOfDays = "";

	public static string _scaleNO = "";

	public static string _activeDate = "";

	public static string _clientNetNumber = "";

	public static string _isClientSet = "";

	public static string _softwareType = "";

	public static string Accounting_Sade(string SoftwareSerial, string HardwareSerial, AccountingPackage AccountingPack, short Days, short ClientNetNumber, DateTime ExpiredDate)
	{
		try
		{
			if (string.IsNullOrEmpty(SoftwareSerial) || string.IsNullOrEmpty(HardwareSerial))
			{
				return "";
			}
			Days = ChoiceExpiredDayes(Days, ExpiredDate);
			double num = Math.Round(SoftwareSerialManager.Conv2Number(Fix(SoftwareSerial.ParseToLong(0L))) * 2.4);
			double num2 = Math.Round(SoftwareSerialManager.Conv2Number(Fix(SoftwareSerial.ParseToLong(0L))) / 72.0);
			double num3 = Math.Round(SoftwareSerialManager.Conv2Number(HardwareSerial) / 21.0);
			string text = Fix((long)(num + num2 - num3 + SoftwareSerialManager.Conv2Number(HardwareSerial)));
			if (SoftwareSerial.Length == 17)
			{
				return InsertCountDays(text, Days, ClientNetNumber, (short)AccountingPack);
			}
			return text;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Accounting_Sade", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\LKLogics\\ClsActivation.cs", 34);
			return "";
		}
	}

	public static string Accounting_Motavaset(string SoftwareSerial, string HardwareSerial, AccountingPackage AccountingPack, short Days, short ClientNetNumber, DateTime ExpiredDate)
	{
		try
		{
			if (string.IsNullOrEmpty(SoftwareSerial) || string.IsNullOrEmpty(HardwareSerial))
			{
				return "";
			}
			Days = ChoiceExpiredDayes(Days, ExpiredDate);
			double num = Math.Round(SoftwareSerialManager.Conv2Number(HardwareSerial) * 3.34);
			double num2 = Math.Round(SoftwareSerialManager.Conv2Number(Fix(SoftwareSerial.ParseToLong(0L))) / 27.0);
			double num3 = Math.Round(SoftwareSerialManager.Conv2Number(HardwareSerial) / 2.0);
			double num4 = Math.Round(SoftwareSerialManager.Conv2Number(HardwareSerial) * 93.0);
			string text = ((double)Fix((long)(num - num2 + num3)).ParseToLong(0L) + num4).ToString();
			if (SoftwareSerial.Length == 17)
			{
				if (text.Length > 10)
				{
					text = text.Remove(0, 1);
					text = text.Remove(0, 1);
				}
				return InsertCountDays(text, Days, ClientNetNumber, (short)AccountingPack);
			}
			return text;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Accounting_Motavaset", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\LKLogics\\ClsActivation.cs", 64);
			return "";
		}
	}

	public static string Accounting_Pishrafteh(string SoftwareSerial, string HardwareSerial, AccountingPackage AccountingPack, short Days, short ClientNetNumber, DateTime ExpiredDate)
	{
		try
		{
			if (string.IsNullOrEmpty(SoftwareSerial) || string.IsNullOrEmpty(HardwareSerial))
			{
				return "";
			}
			Days = ChoiceExpiredDayes(Days, ExpiredDate);
			double num = Math.Round(SoftwareSerialManager.Conv2Number(Fix(SoftwareSerial.ParseToLong(0L))) / 37.0);
			double num2 = Math.Round(SoftwareSerialManager.Conv2Number(Fix(SoftwareSerial.ParseToLong(0L))) * 35.0 / 13.0);
			double num3 = Math.Round(SoftwareSerialManager.Conv2Number(HardwareSerial) / 9.223);
			double num4 = Math.Round(SoftwareSerialManager.Conv2Number(HardwareSerial) * 765.0 / 235.0);
			string text = Fix((long)(num - num2 - num3 - num4));
			if (SoftwareSerial.Length == 17)
			{
				return InsertCountDays(text, Days, ClientNetNumber, (short)AccountingPack);
			}
			return text;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Accounting_Pishrafteh", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\LKLogics\\ClsActivation.cs", 88);
			return "";
		}
	}

	public static string Accounting_Jame(string SoftwareSerial, string HardwareSerial, AccountingPackage AccountingPack, short Days, short ClientNetNumber, DateTime ExpiredDate)
	{
		try
		{
			if (string.IsNullOrEmpty(SoftwareSerial) || string.IsNullOrEmpty(HardwareSerial))
			{
				return "";
			}
			Days = ChoiceExpiredDayes(Days, ExpiredDate);
			double num = Math.Round(SoftwareSerialManager.Conv2Number(Fix(SoftwareSerial.ParseToLong(0L))) / 260.0);
			double num2 = Math.Round(SoftwareSerialManager.Conv2Number(Fix(SoftwareSerial.ParseToLong(0L))) * 3.0 / 7.0);
			double num3 = Math.Round(SoftwareSerialManager.Conv2Number(HardwareSerial) / 2.73);
			double num4 = Math.Round(SoftwareSerialManager.Conv2Number(HardwareSerial) * 26.0 / 751.0);
			string text = Fix((long)(num - num2 - num3 - num4 + 233679.0));
			if (SoftwareSerial.Length == 17)
			{
				return InsertCountDays(text, Days, ClientNetNumber, (short)AccountingPack);
			}
			return text;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Accounting_Jame", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\LKLogics\\ClsActivation.cs", 110);
			return "";
		}
	}

	public static string ProductDepot_Pishrafteh(string SoftwareSerial, string HardwareSerial, AccountingPackage AccountingPack, short Days, short ClientNetNumber, DateTime ExpiredDate)
	{
		try
		{
			if (string.IsNullOrEmpty(SoftwareSerial) || string.IsNullOrEmpty(HardwareSerial))
			{
				return "";
			}
			Days = ChoiceExpiredDayes(Days, ExpiredDate);
			double num = Math.Round(SoftwareSerialManager.Conv2Number(Fix(SoftwareSerial.ParseToLong(0L))) * 7.4);
			double num2 = Math.Round(SoftwareSerialManager.Conv2Number(Fix(SoftwareSerial.ParseToLong(0L))) / 49.0);
			double num3 = Math.Round(SoftwareSerialManager.Conv2Number(HardwareSerial) / 18.0);
			string text = Fix((long)(num + num2 - num3 + SoftwareSerialManager.Conv2Number(HardwareSerial)));
			if (SoftwareSerial.Length == 17)
			{
				return InsertCountDays(text, Days, ClientNetNumber, (short)AccountingPack);
			}
			return text;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ProductDepot_Pishrafteh", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\LKLogics\\ClsActivation.cs", 131);
			return "";
		}
	}

	public static string Tolid(string SoftwareSerial, string HardwareSerial, AccountingPackage AccountingPack, short Days, short ClientNetNumber, DateTime ExpiredDate)
	{
		try
		{
			if (string.IsNullOrEmpty(SoftwareSerial) || string.IsNullOrEmpty(HardwareSerial))
			{
				return "";
			}
			Days = ChoiceExpiredDayes(Days, ExpiredDate);
			double num = Math.Round(SoftwareSerialManager.Conv2Number(Fix(SoftwareSerial.ParseToLong(0L))) / 63.0);
			double num2 = Math.Round(SoftwareSerialManager.Conv2Number(Fix(SoftwareSerial.ParseToLong(0L))) * 7.0 / 2.0);
			double num3 = Math.Round(SoftwareSerialManager.Conv2Number(HardwareSerial) / 6.45);
			double num4 = Math.Round(SoftwareSerialManager.Conv2Number(HardwareSerial) * 28.0 / 87.0);
			string text = Fix((long)(num - num2 - num3 - num4 + 10020.0));
			if (SoftwareSerial.Length == 17)
			{
				return InsertCountDays(text, Days, ClientNetNumber, (short)AccountingPack);
			}
			return text;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Tolid", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\LKLogics\\ClsActivation.cs", 156);
			return "";
		}
	}

	public static string Tax(string SoftwareSerial, string HardwareSerial, AccountingPackage AccountingPack, short Days, short ClientNetNumber, DateTime ExpiredDate)
	{
		try
		{
			if (string.IsNullOrEmpty(SoftwareSerial) || string.IsNullOrEmpty(HardwareSerial))
			{
				return "";
			}
			Days = ChoiceExpiredDayes(Days, ExpiredDate);
			double num = Math.Round(SoftwareSerialManager.Conv2Number(Fix(SoftwareSerial.ParseToLong(0L))) / 56.0);
			double num2 = Math.Round(SoftwareSerialManager.Conv2Number(Fix(SoftwareSerial.ParseToLong(0L))) * 6.0 / 4.0);
			double num3 = Math.Round(SoftwareSerialManager.Conv2Number(HardwareSerial) / 5.32);
			double num4 = Math.Round(SoftwareSerialManager.Conv2Number(HardwareSerial) * 218.0 / 63.0);
			string text = Fix((long)(num - num2 - num3 - num4 + 94825.0));
			if (SoftwareSerial.Length == 17)
			{
				return InsertCountDays(text, Days, ClientNetNumber, (short)AccountingPack);
			}
			return text;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Tax", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\LKLogics\\ClsActivation.cs", 181);
			return "";
		}
	}

	public static string Cheque_Khazane(string SoftwareSerial, string HardwareSerial, AccountingPackage AccountingPack, short Days, short ClientNetNumber, DateTime ExpiredDate)
	{
		try
		{
			if (string.IsNullOrEmpty(SoftwareSerial) || string.IsNullOrEmpty(HardwareSerial))
			{
				return "";
			}
			Days = ChoiceExpiredDayes(Days, ExpiredDate);
			double num = Math.Round(SoftwareSerialManager.Conv2Number(Fix(SoftwareSerial.ParseToLong(0L))) / 106.0);
			double num2 = Math.Round(SoftwareSerialManager.Conv2Number(Fix(SoftwareSerial.ParseToLong(0L))) * 82.0 / 32.0);
			double num3 = Math.Round(SoftwareSerialManager.Conv2Number(HardwareSerial) / 8.45);
			double num4 = Math.Round(SoftwareSerialManager.Conv2Number(HardwareSerial) * 612.0 / 306.0);
			string text = Fix((long)(num - num2 - num3 - num4 + 94825.0));
			if (SoftwareSerial.Length == 17)
			{
				return InsertCountDays(text, Days, ClientNetNumber, (short)AccountingPack);
			}
			return text;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Cheque_Khazane", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\LKLogics\\ClsActivation.cs", 206);
			return "";
		}
	}

	public static string Special(string SoftwareSerial, string HardwareSerial, AccountingPackage AccountingPack, short Days, short ClientNetNumber, DateTime ExpiredDate)
	{
		try
		{
			if (string.IsNullOrEmpty(SoftwareSerial) || string.IsNullOrEmpty(HardwareSerial))
			{
				return "";
			}
			Days = ChoiceExpiredDayes(Days, ExpiredDate);
			string text = "";
			double num = Math.Round(Fix(SoftwareSerial.ParseToLong(0L)).ParseToDouble(0.0, 3) / 8734.0);
			double num2 = Math.Round(Fix(SoftwareSerial.ParseToLong(0L)).ParseToDouble(0.0, 3) * 81.0 / 349.0);
			double num3 = Math.Round(HardwareSerial.ParseToDouble(0.0, 3) / 97.25);
			double num4 = Math.Round(HardwareSerial.ParseToDouble(0.0, 3) * 1734.0 / 5412.0);
			text = Fix((long)(num - num2 - num3 - num4 + 31972.0));
			if (SoftwareSerial.Length == 17)
			{
				return InsertCountDays(text, Days, ClientNetNumber, (short)AccountingPack);
			}
			return text;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Special", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\LKLogics\\ClsActivation.cs", 230);
			return "";
		}
	}

	public static string Aghsat(string SoftwareSerial, string HardwareSerial, AccountingPackage AccountingPack, short Days, short ClientNetNumber, DateTime ExpiredDate)
	{
		try
		{
			if (string.IsNullOrEmpty(SoftwareSerial) || string.IsNullOrEmpty(HardwareSerial))
			{
				return "";
			}
			Days = ChoiceExpiredDayes(Days, ExpiredDate);
			double num = Math.Round(SoftwareSerialManager.Conv2Number(Fix(SoftwareSerial.ParseToLong(0L))) / 31.0);
			double num2 = Math.Round(SoftwareSerialManager.Conv2Number(Fix(SoftwareSerial.ParseToLong(0L))) * 27.0 / 12.0);
			double num3 = Math.Round(SoftwareSerialManager.Conv2Number(HardwareSerial) / 13.5);
			double num4 = Math.Round(SoftwareSerialManager.Conv2Number(HardwareSerial) * 94.0 / 185.0);
			string text = Fix((long)(num - num2 - num3 - num4 + 764285.0));
			if (SoftwareSerial.Length == 17)
			{
				return InsertCountDays(text, Days, ClientNetNumber, (short)AccountingPack);
			}
			return text;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Aghsat", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\LKLogics\\ClsActivation.cs", 252);
			return "";
		}
	}

	public static string ProductSerial(string SoftwareSerial, string HardwareSerial, AccountingPackage AccountingPack, short Days, short ClientNetNumber, DateTime ExpiredDate)
	{
		try
		{
			if (string.IsNullOrEmpty(SoftwareSerial) || string.IsNullOrEmpty(HardwareSerial))
			{
				return "";
			}
			Days = ChoiceExpiredDayes(Days, ExpiredDate);
			double num = Math.Round(SoftwareSerialManager.Conv2Number(Fix(SoftwareSerial.ParseToLong(0L))) * 11.26);
			double num2 = Math.Round(SoftwareSerialManager.Conv2Number(Fix(SoftwareSerial.ParseToLong(0L))) / 57.0);
			double num3 = Math.Round(SoftwareSerialManager.Conv2Number(HardwareSerial) / 52.0);
			string text = Fix((long)(num + num2 - num3 + SoftwareSerialManager.Conv2Number(HardwareSerial)));
			if (SoftwareSerial.Length == 17)
			{
				return InsertCountDays(text, Days, ClientNetNumber, (short)AccountingPack);
			}
			return text;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ProductSerial", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\LKLogics\\ClsActivation.cs", 273);
			return "";
		}
	}

	public static string MultiCurrency(string SoftwareSerial, string HardwareSerial, AccountingPackage AccountingPack, short Days, short ClientNetNumber, DateTime ExpiredDate)
	{
		try
		{
			if (string.IsNullOrEmpty(SoftwareSerial) || string.IsNullOrEmpty(HardwareSerial))
			{
				return "";
			}
			Days = ChoiceExpiredDayes(Days, ExpiredDate);
			double num = Math.Round(SoftwareSerialManager.Conv2Number(Fix(SoftwareSerial.ParseToLong(0L))) / 60.0);
			double num2 = Math.Round(SoftwareSerialManager.Conv2Number(Fix(SoftwareSerial.ParseToLong(0L))) * 27.0 / 4.0);
			double num3 = Math.Round(SoftwareSerialManager.Conv2Number(HardwareSerial) / 74.2);
			double num4 = Math.Round(SoftwareSerialManager.Conv2Number(HardwareSerial) * 57.0 / 24.0);
			string text = Fix((long)(num - num2 - num3 - num4 + 94020.0));
			if (SoftwareSerial.Length == 17)
			{
				return InsertCountDays(text, Days, ClientNetNumber, (short)AccountingPack);
			}
			return text;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MultiCurrency", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\LKLogics\\ClsActivation.cs", 298);
			return "";
		}
	}

	public static string CustomerClub(string SoftwareSerial, string HardwareSerial, AccountingPackage AccountingPack, short Days, short ClientNetNumber, DateTime ExpiredDate)
	{
		try
		{
			if (string.IsNullOrEmpty(SoftwareSerial) || string.IsNullOrEmpty(HardwareSerial))
			{
				return "";
			}
			Days = ChoiceExpiredDayes(Days, ExpiredDate);
			string text = "";
			double num = Math.Round(Fix(SoftwareSerial.ParseToLong(0L)).ParseToDouble(0.0, 3) / 472.0);
			double num2 = Math.Round(Fix(SoftwareSerial.ParseToLong(0L)).ParseToDouble(0.0, 3) * 52.0 / 473.0);
			double num3 = Math.Round(HardwareSerial.ParseToDouble(0.0, 3) / 25.94);
			double num4 = Math.Round(HardwareSerial.ParseToDouble(0.0, 3) * 1373.0 / 1995.0);
			text = Fix((long)(num - num2 - num3 - num4 + 95731.0));
			if (SoftwareSerial.Length == 17)
			{
				return InsertCountDays(text, Days, ClientNetNumber, (short)AccountingPack);
			}
			return text;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CustomerClub", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\LKLogics\\ClsActivation.cs", 322);
			return "";
		}
	}

	public static string Service(string SoftwareSerial, string HardwareSerial, AccountingPackage AccountingPack, short Days, short ClientNetNumber, DateTime ExpiredDate)
	{
		try
		{
			if (string.IsNullOrEmpty(SoftwareSerial) || string.IsNullOrEmpty(HardwareSerial))
			{
				return "";
			}
			Days = ChoiceExpiredDayes(Days, ExpiredDate);
			string text = "";
			double num = Math.Round(Fix(SoftwareSerial.ParseToLong(0L)).ParseToDouble(0.0, 3) / 728.0);
			double num2 = Math.Round(Fix(SoftwareSerial.ParseToLong(0L)).ParseToDouble(0.0, 3) * 48.0 / 954.0);
			double num3 = Math.Round(HardwareSerial.ParseToDouble(0.0, 3) / 34.74);
			double num4 = Math.Round(HardwareSerial.ParseToDouble(0.0, 3) * 1745.0 / 9571.0);
			text = Fix((long)(num - num2 - num3 - num4 + 925841.0));
			if (SoftwareSerial.Length == 17)
			{
				return InsertCountDays(text, Days, ClientNetNumber, (short)AccountingPack);
			}
			return text;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Service", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\LKLogics\\ClsActivation.cs", 346);
			return "";
		}
	}

	public static string VoiceCall(string SoftwareSerial, string HardwareSerial, AccountingPackage AccountingPack, short Days, short ClientNetNumber, DateTime ExpiredDate)
	{
		try
		{
			if (string.IsNullOrEmpty(SoftwareSerial) || string.IsNullOrEmpty(HardwareSerial))
			{
				return "";
			}
			Days = ChoiceExpiredDayes(Days, ExpiredDate);
			double num = Math.Round(SoftwareSerialManager.Conv2Number(Fix(SoftwareSerial.ParseToLong(0L))) * 7.4);
			double num2 = Math.Round(SoftwareSerialManager.Conv2Number(Fix(SoftwareSerial.ParseToLong(0L))) / 69.0);
			double num3 = Math.Round(SoftwareSerialManager.Conv2Number(HardwareSerial) / 32.0);
			string text = Fix((long)(num + num2 - num3 + SoftwareSerialManager.Conv2Number(HardwareSerial)));
			if (SoftwareSerial.Length == 17)
			{
				return InsertCountDays(text, Days, ClientNetNumber, (short)AccountingPack);
			}
			return text;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "VoiceCall", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\LKLogics\\ClsActivation.cs", 367);
			return "";
		}
	}

	public static string BarCode(string SoftwareSerial, string HardwareSerial, AccountingPackage AccountingPack, short Days, short ClientNetNumber, DateTime ExpiredDate)
	{
		try
		{
			if (string.IsNullOrEmpty(SoftwareSerial) || string.IsNullOrEmpty(HardwareSerial))
			{
				return "";
			}
			Days = ChoiceExpiredDayes(Days, ExpiredDate);
			double num = Math.Round(SoftwareSerialManager.Conv2Number(Fix(SoftwareSerial.ParseToLong(0L))) / 620.0);
			double num2 = Math.Round(SoftwareSerialManager.Conv2Number(Fix(SoftwareSerial.ParseToLong(0L))) * 7.0 / 3.0);
			double num3 = Math.Round(SoftwareSerialManager.Conv2Number(HardwareSerial) / 7.23);
			double num4 = Math.Round(SoftwareSerialManager.Conv2Number(HardwareSerial) * 67.0 / 275.0);
			string text = Fix((long)(num - num2 - num3 - num4 + 909482.0));
			if (SoftwareSerial.Length == 17)
			{
				return InsertCountDays(text, Days, ClientNetNumber, (short)AccountingPack);
			}
			return text;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BarCode", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\LKLogics\\ClsActivation.cs", 389);
			return "";
		}
	}

	public static string Scale(string SoftwareSerial, string HardwareSerial, short CountScale, short Days, short ClientNetNumber, DateTime ExpiredDate)
	{
		try
		{
			if (string.IsNullOrEmpty(SoftwareSerial) || string.IsNullOrEmpty(HardwareSerial))
			{
				return "";
			}
			SoftwareSerial = Fix(SoftwareSerial.ParseToLong(0L));
			Days = ChoiceExpiredDayes(Days, ExpiredDate);
			string text = null;
			long num = (long)(Math.Round(SoftwareSerialManager.Conv2Number(SoftwareSerial)) / 84123.0);
			long num2 = (long)Math.Round(SoftwareSerialManager.Conv2Number(SoftwareSerial) * 7203.0 / 32.0);
			long num3 = (long)Math.Round(SoftwareSerialManager.Conv2Number(HardwareSerial) / 312.243);
			long num4 = (long)Math.Round(SoftwareSerialManager.Conv2Number(HardwareSerial) * 7104.0 / 5196.0);
			text = Fix(num - num2 - num3 - num4 + 90982);
			return InsertCountScaleDays(text, CountScale, Days, ClientNetNumber);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Scale", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\LKLogics\\ClsActivation.cs", 410);
			return "";
		}
	}

	public static string Tamdid(string SoftwareSerial, string HardwareSerial, AccountingPackage AccountingPack, short Days, short ClientNetNumber, DateTime ExpiredDate)
	{
		try
		{
			if (string.IsNullOrEmpty(SoftwareSerial) || string.IsNullOrEmpty(HardwareSerial))
			{
				return "";
			}
			Days = ChoiceExpiredDayes(Days, ExpiredDate);
			double num = Math.Round(Fix(SoftwareSerial.ParseToLong(0L)).ParseToDouble(0.0, 3) / 10.0);
			double num2 = Math.Round(Fix(SoftwareSerial.ParseToLong(0L)).ParseToDouble(0.0, 3) * 8.0 / 9.0);
			double num3 = Math.Round(HardwareSerial.ParseToDouble(0.0, 3) / 2.3);
			double num4 = Math.Round(HardwareSerial.ParseToDouble(0.0, 3) * 83.0 / 275.0);
			string text = Fix((long)(num - num2 - num3 - num4 + 6032.0));
			if (SoftwareSerial.Length == 17)
			{
				return InsertCountDays(text, Days, ClientNetNumber, (short)AccountingPack);
			}
			return text;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Tamdid", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\LKLogics\\ClsActivation.cs", 432);
			return "";
		}
	}

	public static string CallerID(string SoftwareSerial, string HardwareSerial, short Days, short ClientNetNumber, DateTime ExpiredDate)
	{
		try
		{
			if (string.IsNullOrEmpty(SoftwareSerial) || string.IsNullOrEmpty(HardwareSerial))
			{
				return "";
			}
			Days = ChoiceExpiredDayes(Days, ExpiredDate);
			double num = Math.Round(SoftwareSerialManager.Conv2Number(Fix(SoftwareSerial.ParseToLong(0L))) / 8438.0);
			double num2 = Math.Round(SoftwareSerialManager.Conv2Number(Fix(SoftwareSerial.ParseToLong(0L))) * 89.0 / 304.0);
			double num3 = Math.Round(SoftwareSerialManager.Conv2Number(HardwareSerial) / 43.92);
			double num4 = Math.Round(SoftwareSerialManager.Conv2Number(HardwareSerial) * 7104.0 / 720.0);
			string text = Fix((long)(num - num2 - num3 - num4 + 92053.0));
			if (SoftwareSerial.Length == 17)
			{
				return InsertCountDays(text, Days, ClientNetNumber, 0);
			}
			return text;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CallerID", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\LKLogics\\ClsActivation.cs", 455);
			return "";
		}
	}

	public static string Level1(string SoftwareSerial, string HardwareSerial, HozoorPackage SoftType, short Days, short ClientNetNumber, DateTime ExpiredDate)
	{
		try
		{
			if (string.IsNullOrEmpty(SoftwareSerial) || string.IsNullOrEmpty(HardwareSerial))
			{
				return "";
			}
			Days = ChoiceExpiredDayes(Days, ExpiredDate);
			double num = Math.Round(SoftwareSerialManager.Conv2Number(Fix(SoftwareSerial.ParseToLong(0L))) / 690.0);
			double num2 = Math.Round(SoftwareSerialManager.Conv2Number(Fix(SoftwareSerial.ParseToLong(0L))) * 223.0 / 32.0);
			double num3 = Math.Round(SoftwareSerialManager.Conv2Number(HardwareSerial) / 8.32);
			double num4 = Math.Round(SoftwareSerialManager.Conv2Number(HardwareSerial) * 322.0 / 32.0);
			string text = Fix((long)(num - num2 - num3 - num4 + 5356.0));
			if (SoftwareSerial.Length == 17)
			{
				return InsertCountDays(text, Days, ClientNetNumber, (short)SoftType);
			}
			return text;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Level1", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\LKLogics\\ClsActivation.cs", 478);
			return "";
		}
	}

	public static string Level2(string SoftwareSerial, string HardwareSerial, HozoorPackage SoftType, short Days, short ClientNetNumber, DateTime ExpiredDate)
	{
		try
		{
			if (string.IsNullOrEmpty(SoftwareSerial) || string.IsNullOrEmpty(HardwareSerial))
			{
				return "";
			}
			Days = ChoiceExpiredDayes(Days, ExpiredDate);
			string text = "";
			double num = Math.Round(SoftwareSerialManager.Conv2Number(Fix(SoftwareSerial.ParseToLong(0L))) / 467.0);
			double num2 = Math.Round(SoftwareSerialManager.Conv2Number(Fix(SoftwareSerial.ParseToLong(0L))) * 34.0 / 98.0);
			double num3 = Math.Round(SoftwareSerialManager.Conv2Number(HardwareSerial) / 2.8);
			double num4 = Math.Round(SoftwareSerialManager.Conv2Number(HardwareSerial) * 45.0 / 98.0);
			text = Fix((long)(num - num2 - num3 - num4 + 56782.0));
			if (SoftwareSerial.Length == 17)
			{
				return InsertCountDays(text, Days, ClientNetNumber, (short)SoftType);
			}
			return text;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Level2", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\LKLogics\\ClsActivation.cs", 503);
			return "";
		}
	}

	public static string Level3(string SoftwareSerial, string HardwareSerial, HozoorPackage SoftType, short Days, short ClientNetNumber, DateTime ExpiredDate)
	{
		try
		{
			if (string.IsNullOrEmpty(SoftwareSerial) || string.IsNullOrEmpty(HardwareSerial))
			{
				return "";
			}
			Days = ChoiceExpiredDayes(Days, ExpiredDate);
			string text = null;
			double num = Math.Round(SoftwareSerialManager.Conv2Number(Fix(SoftwareSerial.ParseToLong(0L))) / 790.0);
			double num2 = Math.Round(SoftwareSerialManager.Conv2Number(Fix(SoftwareSerial.ParseToLong(0L))) * 32.0 / 7.0);
			double num3 = Math.Round(SoftwareSerialManager.Conv2Number(HardwareSerial) / 8.32);
			double num4 = Math.Round(SoftwareSerialManager.Conv2Number(HardwareSerial) * 64.0 / 44.0);
			text = Fix((long)(num - num2 - num3 - num4 + 98765.0));
			if (SoftwareSerial.Length == 17)
			{
				return InsertCountDays(text, Days, ClientNetNumber, (short)SoftType);
			}
			return text;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Level3", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\LKLogics\\ClsActivation.cs", 528);
			return "";
		}
	}

	public static string Level4(string SoftwareSerial, string HardwareSerial, HozoorPackage SoftType, short Days, short ClientNetNumber, DateTime ExpiredDate)
	{
		try
		{
			if (string.IsNullOrEmpty(SoftwareSerial) || string.IsNullOrEmpty(HardwareSerial))
			{
				return "";
			}
			Days = ChoiceExpiredDayes(Days, ExpiredDate);
			string text = "";
			double num = Math.Round(SoftwareSerialManager.Conv2Number(Fix(SoftwareSerial.ParseToLong(0L))) / 246.0);
			double num2 = Math.Round(SoftwareSerialManager.Conv2Number(Fix(SoftwareSerial.ParseToLong(0L))) * 71.0 / 12.0);
			double num3 = Math.Round(SoftwareSerialManager.Conv2Number(HardwareSerial) / 3.2);
			double num4 = Math.Round(SoftwareSerialManager.Conv2Number(HardwareSerial) * 12.0 / 102.0);
			text = Fix((long)(num - num2 - num3 - num4 + 74516.0));
			if (SoftwareSerial.Length == 17)
			{
				return InsertCountDays(text, Days, ClientNetNumber, (short)SoftType);
			}
			return text;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Level4", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\LKLogics\\ClsActivation.cs", 551);
			return "";
		}
	}

	public static string Level5(string SoftwareSerial, string HardwareSerial, HozoorPackage SoftType, short Days, short ClientNetNumber, DateTime ExpiredDate)
	{
		try
		{
			if (string.IsNullOrEmpty(SoftwareSerial) || string.IsNullOrEmpty(HardwareSerial))
			{
				return "";
			}
			Days = ChoiceExpiredDayes(Days, ExpiredDate);
			double num = Math.Round(SoftwareSerialManager.Conv2Number(Fix(SoftwareSerial.ParseToLong(0L))) / 497.0);
			double num2 = Math.Round(SoftwareSerialManager.Conv2Number(Fix(SoftwareSerial.ParseToLong(0L))) * 12.0 / 4.5);
			double num3 = Math.Round(SoftwareSerialManager.Conv2Number(HardwareSerial) / 4.3);
			double num4 = Math.Round(SoftwareSerialManager.Conv2Number(HardwareSerial) * 47.0 / 45.0);
			string text = Fix((long)(num - num2 - num3 - num4 + 467541.0));
			if (SoftwareSerial.Length == 17)
			{
				return InsertCountDays(text, Days, ClientNetNumber, (short)SoftType);
			}
			return text;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Level5", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\LKLogics\\ClsActivation.cs", 574);
			return "";
		}
	}

	public static string PasswordRecovery(string lk, string softwareSerial)
	{
		string text = "  ";
		if (string.IsNullOrEmpty(lk))
		{
			throw new ArgumentException("lk value is null or empty!");
		}
		if (lk.ParseToLong(0L).ToString().Length != 12 && lk.ParseToLong(0L) != 1234567890)
		{
			throw new ArgumentOutOfRangeException("value(" + lk + ") is not valid for lkSerial number!");
		}
		try
		{
			text = (Math.Round(double.Parse(lk) * 2.0 + (10.0 * double.Parse(softwareSerial) - 0.0)) - 4000.0).ToString();
			if (text.Length > 10)
			{
				text = text.Substring(0, 10);
			}
			if (text.Length < 10)
			{
				while (text.Length == 10)
				{
					text += "1";
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "Lk:" + (lk ?? "Null!") + "//SoftwareSerial:" + (softwareSerial ?? "Null!"), "PasswordRecovery", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\LKLogics\\ClsActivation.cs", 595);
		}
		return text;
	}

	public static string InsertCountScaleDays(string ActivationCode, short countscale, short Days, short ClientNetNumber)
	{
		try
		{
			string text = Convert2String(countscale);
			string text2 = Convert2String(Days);
			string text3 = Convert2String(ClientNetNumber);
			ActivationCode = ActivationCode.Insert(4, text.ToString().Substring(1, 1)).Insert(11, text.ToString().Substring(0, 1));
			ActivationCode = ActivationCode.Insert(6, text2.Substring(1, 1)).Insert(13, text2.Substring(0, 1));
			ActivationCode = ActivationCode.Insert(0, text3.Substring(1, 1)).Insert(ActivationCode.Length + 1, text3.Substring(0, 1));
			return ActivationCode;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "InsertCountScaleDays", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\LKLogics\\ClsActivation.cs", 613);
			return "0000000000000000";
		}
	}

	public static string InsertCountDays(string ActivationCode, short Days, short ClientNetNumber, short SoftType)
	{
		try
		{
			string text = Convert2String(Days);
			string text2 = Convert2String(SoftType);
			string text3 = Convert2String(ClientNetNumber);
			ActivationCode = ActivationCode.Insert(4, text.Substring(1, 1)).Insert(11, text.Substring(0, 1));
			ActivationCode = ActivationCode.Insert(0, text3.Substring(1, 1)).Insert(ActivationCode.Length + 1, text3.Substring(0, 1));
			if (!((text2 ?? "") == "00"))
			{
				ActivationCode = ActivationCode.Insert(0, text2.Substring(1, 1)).Insert(ActivationCode.Length + 1, text2.Substring(0, 1));
			}
			return ActivationCode;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "InsertCountDays", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\LKLogics\\ClsActivation.cs", 632);
			return "0000000000000000";
		}
	}

	public static string Fix(long num)
	{
		try
		{
			string text = Math.Round((decimal)Math.Abs(num)).ToString();
			if (text.Length > 10)
			{
				return text.Substring(0, 10);
			}
			while (text.Length != 10)
			{
				if (text.Length > 10)
				{
					return text.Substring(0, 10);
				}
				text += text;
			}
			return text;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Fix", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\LKLogics\\ClsActivation.cs", 653);
			return "";
		}
	}

	public static string Convert2String(short number)
	{
		try
		{
			string result = "";
			short num = number;
			short num2 = num;
			if (0 <= number && number <= 9)
			{
				result = "0" + number;
			}
			else if (10 <= number && number <= 99)
			{
				result = number.ToString();
			}
			else if (number > 99)
			{
				result = "00";
			}
			return result;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Convert2String", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\LKLogics\\ClsActivation.cs", 688);
			return "";
		}
	}

	private static short ChoiceExpiredDayes(short Days, DateTime ExpiredDate)
	{
		try
		{
			if (ExpiredDate <= DateTime.Now)
			{
				return 0;
			}
			short num = 0;
			num = (short)DateTime.Now.Subtract(ExpiredDate).TotalDays;
			if (Days > num && num > 0)
			{
				return num;
			}
			return Days;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ChoiceExpiredDayes", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\LKLogics\\ClsActivation.cs", 704);
			return 0;
		}
	}
}
