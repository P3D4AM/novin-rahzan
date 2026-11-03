using System;
using PacketParser.Entities;

namespace PacketParser;

public class PossBarcodeSeprator
{
	public string Barcode { get; private set; }

	public string Pattern { get; private set; }

	public bool IsValid { get; private set; }

	public decimal Price { get; private set; }

	public string PeygiriNo { get; set; }

	public string DateSh { get; set; }

	public string Time { get; set; }

	public PossBarcodeSeprator(string pattern, string barcode)
	{
		using (new JaegerMonitor(".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\PossBarcodeSeprator.cs"))
		{
			try
			{
				Barcode = barcode;
				Pattern = pattern.ToUpper();
				IsValid = CheckBarcodeWithPattern(barcode, pattern);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\PossBarcodeSeprator.cs", 25);
			}
		}
	}

	private bool CheckBarcodeWithPattern(string barcode, string pattern)
	{
		using (new JaegerMonitor("CheckBarcodeWithPattern", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\PossBarcodeSeprator.cs"))
		{
			try
			{
				if (barcode.Length != pattern.Length)
				{
					return false;
				}
				Price = GetSpecialParameter("A").Trim().ParseToShort(0);
				PeygiriNo = GetSpecialParameter("B").Trim();
				DateSh = GetSpecialParameter("C").Trim();
				Time = GetSpecialParameter("D").Trim();
				return Price > 0m;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CheckBarcodeWithPattern", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\PossBarcodeSeprator.cs", 42);
				return false;
			}
		}
	}

	private string GetSpecialParameter(string ch)
	{
		using (new JaegerMonitor("GetSpecialParameter", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\PossBarcodeSeprator.cs"))
		{
			try
			{
				int num = Pattern.IndexOf(ch);
				int num2 = checked(Pattern.Split(new char[1] { Convert.ToChar(ch) }).Length - 1);
				if (num < 0)
				{
					return "";
				}
				if (num2 == 0)
				{
					return "";
				}
				return Barcode.Substring(num, num2);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetSpecialParameter", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\PossBarcodeSeprator.cs", 60);
				return "";
			}
		}
	}
}
