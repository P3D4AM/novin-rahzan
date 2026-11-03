using System;
using System.Linq;
using PacketParser.Entities;

namespace PacketParser;

public class BarcodeCreator
{
	private string _Format = "";

	private string _Control = "";

	private double _ZaribRial = 1.0;

	private int _ID;

	private int _Number;

	private decimal _TotalSum;

	private string _DateM = "";

	private string _cashierCode;

	private int _Count;

	private string _DateSH = "";

	public BarcodeCreator(string Format, string Control, double ZaribRial, int ID, int Number, decimal TotalSum, DateTime DateM, string cashierCode, int Count, short digitForRound)
	{
		using (new JaegerMonitor(".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BarcodeCreator.cs"))
		{
			try
			{
				_Format = Format;
				_Control = Control;
				_ZaribRial = ZaribRial;
				_ID = ID;
				_Number = Number;
				_TotalSum = decimal.Parse(Math.Round(ZaribRial * double.Parse(TotalSum.ToString()), digitForRound).ToString());
				_cashierCode = cashierCode;
				_Count = Count;
				_DateSH = DateM.MiladiToCalture();
				_DateM = DateM.Year.ToString();
				if (DateM.Month >= 10)
				{
					_DateM = DateM.Month.ToString();
				}
				else
				{
					_DateM = "0" + DateM.Month;
				}
				if (DateM.Day >= 10)
				{
					_DateM = DateM.Day.ToString();
				}
				else
				{
					_DateM = "0" + DateM.Day;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BarcodeCreator.cs", 42);
			}
		}
	}

	public string TotalBarcode()
	{
		using (new JaegerMonitor("TotalBarcode", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BarcodeCreator.cs"))
		{
			try
			{
				_Format = TotalBarcode("J", _Format, _ID.ToString());
				_Format = TotalBarcode("K", _Format, _cashierCode);
				_Format = TotalBarcode("G", _Format, _TotalSum.ToString());
				_Format = TotalBarcode("M", _Format, _DateM);
				_Format = TotalBarcode("C", _Format, _Count.ToString());
				_Format = TotalBarcode("B", _Format, _Number.ToString());
				_Format = TotalBarcode("T", _Format, _DateSH);
				_Format = TotalBarcode("H", _Format, _Control);
				return _Format;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "TotalBarcode", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BarcodeCreator.cs", 62);
				return "";
			}
		}
	}

	public string TotalBarcode(string CH, string Format, string val)
	{
		checked
		{
			using (new JaegerMonitor("TotalBarcode", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BarcodeCreator.cs"))
			{
				try
				{
					int num = 0;
					Format = Format.ToLower();
					CH = CH.ToLower();
					string text = "";
					if (Format.Contains(CH))
					{
						num = Format.Split(CH.ToCharArray()).Count();
					}
					if (val.Length > num && num > 0)
					{
						val = val.Substring(val.Length - num, num);
					}
					while (val.Length < num)
					{
						val = "0" + val;
					}
					for (int i = 1; i <= num; i++)
					{
						text += CH;
					}
					if (text != null && !string.IsNullOrEmpty(text.Trim()))
					{
						Format = Format.Replace(text, val);
					}
					return Format;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "TotalBarcode", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BarcodeCreator.cs", 94);
					return "";
				}
			}
		}
	}
}
