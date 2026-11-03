using System;
using PacketParser.Entities;

namespace PacketParser.PosToPc;

public class PCPOSResponceBase
{
	public string RawResponse { get; set; }

	public string GetAmount()
	{
		try
		{
			string text = "AM = ";
			if (RawResponse.IndexOf("AM") > 0)
			{
				text += checked(RawResponse.Substring(RawResponse.IndexOf("AM") + 5, RawResponse.Substring(RawResponse.IndexOf("AM") + 2, 3).ParseToInt()));
			}
			return text;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAmount", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\PosToPc\\PCPOSResponceBase.cs", 17);
			throw;
		}
	}

	public string GetBankBin(string respSuccess)
	{
		try
		{
			string text = "PN = ";
			if (RawResponse.IndexOf("PN") > 0)
			{
				text += checked(respSuccess.Substring(respSuccess.IndexOf("PN") + 5, respSuccess.Substring(respSuccess.IndexOf("PN") + 2, 3).ParseToInt())).Substring(0, 6);
			}
			return text;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetBankBin", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\PosToPc\\PCPOSResponceBase.cs", 27);
			throw;
		}
	}

	public string GetBankName()
	{
		try
		{
			string result = "";
			if (RawResponse.IndexOf("PN") > 0)
			{
				result = "بانک ناشناخته";
				string text = checked(RawResponse.Substring(RawResponse.IndexOf("PN") + 5, RawResponse.Substring(RawResponse.IndexOf("PN") + 2, 3).ParseToInt()));
				if (text.Substring(0, 6) == "621986")
				{
					return "بانک سامان";
				}
				if (text.Substring(0, 6) == "622106")
				{
					return "بانک پارسیان";
				}
				if (text.Substring(0, 6) == "603799")
				{
					return "بانک ملی";
				}
				if (text.Substring(0, 6) == "589210")
				{
					return "بانک سپه";
				}
				if (text.Substring(0, 6) == "627648")
				{
					return "بانک توسعه صادرات";
				}
				if (text.Substring(0, 6) == "628023")
				{
					return "بانک مسکن";
				}
				if (text.Substring(0, 6) == "603770")
				{
					return "بانک کشاورزی";
				}
				if (text.Substring(0, 6) == "627961")
				{
					return "بانک صنعت و معدن";
				}
				if (text.Substring(0, 6) == "627760")
				{
					return " پست بانک";
				}
				if (text.Substring(0, 6) == "502908")
				{
					return "توسعه تعاون";
				}
				if (text.Substring(0, 6) == "627412")
				{
					return "اقتصاد نوین";
				}
				if (text.Substring(0, 6) == "639347")
				{
					return "بانک پاسارگاد";
				}
				if (text.Substring(0, 6) == "627488")
				{
					return "بانک کارآفرین";
				}
				if (text.Substring(0, 6) == "639346")
				{
					return "بانک سینا";
				}
				if (text.Substring(0, 6) == "639607")
				{
					return "بانک سرمایه";
				}
				if (text.Substring(0, 6) == "502806")
				{
					return "بانک شهر";
				}
				if (text.Substring(0, 6) == "603769")
				{
					return "بانک صادرات";
				}
				if (text.Substring(0, 6) == "610433")
				{
					return "بانک ملت";
				}
				if (text.Substring(0, 6) == "627353")
				{
					return "بانک تجارت";
				}
				if (text.Substring(0, 6) == "589463")
				{
					return "بانک رفاه";
				}
				if (text.Substring(0, 6) == "627381")
				{
					return "بانک انصار";
				}
				if (text.Substring(0, 6) == "502938")
				{
					return "بانک دی";
				}
				if (text.Substring(0, 6) == "505801")
				{
					return "بانک کوثر";
				}
			}
			return result;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetBankName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\PosToPc\\PCPOSResponceBase.cs", 64);
			throw;
		}
	}

	public string GetDiscount()
	{
		try
		{
			string text = "DS = ";
			if (RawResponse.IndexOf("DS") > 0)
			{
				text += checked(RawResponse.Substring(RawResponse.IndexOf("DS") + 5, RawResponse.Substring(RawResponse.IndexOf("DS") + 2, 3).ParseToInt()));
			}
			return text;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetDiscount", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\PosToPc\\PCPOSResponceBase.cs", 74);
			throw;
		}
	}

	public string GetPANID()
	{
		try
		{
			string text = "PN = ";
			if (RawResponse.IndexOf("PN") > 0)
			{
				text += checked(RawResponse.Substring(RawResponse.IndexOf("PN") + 5, RawResponse.Substring(RawResponse.IndexOf("PN") + 2, 3).ParseToInt()));
			}
			return text;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPANID", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\PosToPc\\PCPOSResponceBase.cs", 84);
			throw;
		}
	}

	public string GetParsedResp()
	{
		try
		{
			string text = "";
			if (RawResponse != null)
			{
				text = GetTrxnResp() + "\r\n" + GetTraceNo() + "\r\n" + GetPANID() + "\r\n" + GetTerminalID() + "\r\n" + GetAmount() + "\r\n" + GetTrxnRRN() + "\r\n" + GetTrxnDateTime() + "\r\n" + GetTrxnSerial() + "\r\n" + GetBankName() + "\r\n";
				if (RawResponse.IndexOf("DS") > 0)
				{
					text = text + GetDiscount() + "\r\n";
				}
			}
			return text;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetParsedResp", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\PosToPc\\PCPOSResponceBase.cs", 98);
			throw;
		}
	}

	public string GetParsedResp(string response)
	{
		try
		{
			RawResponse = response;
			return GetParsedResp();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetParsedResp", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\PosToPc\\PCPOSResponceBase.cs", 107);
			throw;
		}
	}

	public string GetTerminalID()
	{
		try
		{
			string text = "TM = ";
			if (RawResponse.IndexOf("TM") > 0)
			{
				text += checked(RawResponse.Substring(RawResponse.IndexOf("TM") + 5, RawResponse.Substring(RawResponse.IndexOf("TM") + 2, 3).ParseToInt()));
			}
			return text;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetTerminalID", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\PosToPc\\PCPOSResponceBase.cs", 117);
			throw;
		}
	}

	public string GetTraceNo()
	{
		try
		{
			string text = "TR = ";
			if (RawResponse.IndexOf("TR") > 0)
			{
				text += checked(RawResponse.Substring(RawResponse.IndexOf("TR") + 5, RawResponse.Substring(RawResponse.IndexOf("TR") + 2, 3).ParseToInt()));
			}
			return text;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetTraceNo", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\PosToPc\\PCPOSResponceBase.cs", 127);
			throw;
		}
	}

	public string GetTrxnDate()
	{
		try
		{
			string text = "TI = ";
			if (RawResponse.IndexOf("TI") > 0)
			{
				text += RawResponse.Substring(checked(RawResponse.IndexOf("TI") + 5), 10);
			}
			return text;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetTrxnDate", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\PosToPc\\PCPOSResponceBase.cs", 137);
			throw;
		}
	}

	public string GetTrxnDateTime()
	{
		try
		{
			string text = "TI = ";
			if (RawResponse.IndexOf("TI") > 0)
			{
				text += checked(RawResponse.Substring(RawResponse.IndexOf("TI") + 5, RawResponse.Substring(RawResponse.IndexOf("TI") + 2, 3).ParseToInt()));
			}
			return text;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetTrxnDateTime", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\PosToPc\\PCPOSResponceBase.cs", 147);
			throw;
		}
	}

	public string GetTrxnResp()
	{
		return "RS = " + RawResponse.Substring(14, 2);
	}

	public string GetTrxnRRN()
	{
		try
		{
			string text = "RN = ";
			if (RawResponse.IndexOf("RN") > 0)
			{
				text += checked(RawResponse.Substring(RawResponse.IndexOf("RN") + 5, RawResponse.Substring(RawResponse.IndexOf("RN") + 2, 3).ParseToInt()));
			}
			return text;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetTrxnRRN", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\PosToPc\\PCPOSResponceBase.cs", 158);
			throw;
		}
	}

	public string GetTrxnSerial()
	{
		try
		{
			string text = "SR = ";
			if (RawResponse.IndexOf("SR") > 0)
			{
				text += checked(RawResponse.Substring(RawResponse.IndexOf("SR") + 5, RawResponse.Substring(RawResponse.IndexOf("SR") + 2, 3).ParseToInt()));
			}
			return text;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetTrxnSerial", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\PosToPc\\PCPOSResponceBase.cs", 168);
			throw;
		}
	}

	public string GetTrxnTime()
	{
		try
		{
			string text = "TI = ";
			if (RawResponse.IndexOf("TI") > 0)
			{
				text += RawResponse.Substring(checked(RawResponse.IndexOf("TI") + 5 + 10 + 1), 8);
			}
			return text;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetTrxnTime", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\PosToPc\\PCPOSResponceBase.cs", 178);
			throw;
		}
	}
}
