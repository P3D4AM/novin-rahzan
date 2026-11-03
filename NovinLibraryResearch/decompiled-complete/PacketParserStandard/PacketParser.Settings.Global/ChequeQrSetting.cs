using System;
using System.Collections.Generic;
using PacketParser.Entities;

namespace PacketParser.Settings.Global;

public class ChequeQrSetting
{
	public const string KeyName = "Acc_ChequeQrSetting";

	public List<WebChequeQrParserTemplate> Templates { get; set; }

	public static ChequeQrSetting FromString(string data)
	{
		using (new JaegerMonitor("FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Global\\ChequeQrSetting.cs"))
		{
			try
			{
				return data.FromJson<ChequeQrSetting>("FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Global\\ChequeQrSetting.cs", 18);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Global\\ChequeQrSetting.cs", 22);
				throw;
			}
		}
	}

	public override string ToString()
	{
		using (new JaegerMonitor("ToString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Global\\ChequeQrSetting.cs"))
		{
			try
			{
				return Json.ToStringJson(this);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ToString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Global\\ChequeQrSetting.cs", 35);
				return "";
			}
		}
	}

	public static ChequeQrSetting GetDefaults()
	{
		try
		{
			return new ChequeQrSetting
			{
				Templates = new List<WebChequeQrParserTemplate>
				{
					new WebChequeQrParserTemplate
					{
						BankType = EnBank.Melli,
						RegexList = new List<string> { "^(?<Temp1>\\d{1,2})\\r?\\n(?<Temp2>1)\\r?\\n(?<NationalCode>\\d{1,10})\\r?\\n(?<ShabaNumber>IR\\d{24})\\r?\\n(?<BranchCode>[0-9]{1,2}_[0-9]{1,5})\\r?\\n(?<Serial>[0-9]{1,6})\\r?\\n(?<SayadiIdentity>[0-9]{1,16})$" }
					},
					new WebChequeQrParserTemplate
					{
						BankType = EnBank.Shahr,
						RegexList = new List<string> { "^(?<Temp1>\\d{1,2})\\r?\\n(?<Temp2>1)\\r?\\n(?<NationalCode>\\d{1,10})\\r?\\n(?<ShabaNumber>IR(?:\\s?\\d){24})\\r?\\n(?<BranchCode>[0-9]{1,7})\\r?\\n(?<Serial>[0-9]{1,6})\\r?\\n(?<SayadiIdentity>[0-9]{1,16})$" }
					},
					new WebChequeQrParserTemplate
					{
						BankType = EnBank.Refah,
						RegexList = new List<string> { "^(?<Temp1>\\d{1,2})\\r?\\n(?<Temp2>1)\\r?\\n(?<NationalCode>\\d{1,10})\\r?\\n(?<ShabaNumber>IR\\d{24})\\r?\\n(?<BranchCode>[0-9]{1,2}-[0-9]{1,3})\\r?\\n(?<Serial>[0-9]{1,6})\\r?\\n(?<SayadiIdentity>[0-9]{1,16})$" }
					},
					new WebChequeQrParserTemplate
					{
						BankType = EnBank.Tejarat,
						RegexList = new List<string> { "^(?<Temp1>\\d{1,2})\\r?\\n(?<Temp2>\\d{1,1})\\r?\\n(?<NationalCode>\\d{1,10})\\r?\\n(?<ShabaNumber>IR\\d{24})\\r?\\n(?<BranchCode>[0-9]{1,2}-[0-9]{1,5})\\r?\\n(?<Serial>[0-9]{1,6})\\r?\\n(?<SayadiIdentity>[0-9]{1,16})$" }
					},
					new WebChequeQrParserTemplate
					{
						BankType = EnBank.Maskan,
						RegexList = new List<string> { "^(?<Temp1>\\d{1,2})\\r?\\n(?<Temp2>\\d{1,1})\\r?\\n(?<NationalCode>\\d{1,10})\\r?\\n(?<ShabaNumber>IR\\d{24})\\r?\\n(?<BranchCode>[0-9]{1,7}-[0-9]{1,5})\\r?\\n(?<Serial>[0-9]{1,6})\\r?\\n(?<SayadiIdentity>[0-9]{1,16})$" }
					},
					new WebChequeQrParserTemplate
					{
						BankType = EnBank.KarAfarin,
						RegexList = new List<string> { "^(?<Temp1>\\d{1,2})\\r?\\n(?<Temp2>\\d{1,1})\\r?\\n(?<NationalCode>\\d{1,10})\\r?\\n(?<ShabaNumber>IR\\d{24})\\r?\\n(?<BranchCode>[0-9]{1,3}_[0-9]{1,5})\\r?\\n(?<Serial>[0-9]{1,6})\\r?\\n(?<SayadiIdentity>[0-9]{1,16})$" }
					},
					new WebChequeQrParserTemplate
					{
						BankType = EnBank.Keshavarzi,
						RegexList = new List<string> { "^(?<Temp1>\\d{1,2})\\r?\\n(?<Temp2>[0-9]{1,1}-[0-9]{1,1})\\r?\\n(?<NationalCode>\\d{1,10}-)\\r?\\n(?<ShabaNumber>IR\\d{24})\\r?\\n(?<BranchCode>[0-9]{1,2}-[0-9]{1,7})\\r?\\n(?<Serial>[0-9]{1,12})\\r?\\n(?<SayadiIdentity>[0-9]{1,16})$" }
					},
					new WebChequeQrParserTemplate
					{
						BankType = EnBank.Resalat,
						RegexList = new List<string> { "^(?<Temp1>\\d{1,2})\\r?\\n(?<Temp2>\\d{1,1})\\r?\\n(?<NationalCode>\\d{1,10})\\r?\\n(?<ShabaNumber>IR\\d{24})\\r?\\n(?<BranchCode>[0-9]{1,3}_[0-9]{1,7})\\r?\\n(?<Serial>[0-9]{1,6})\\r?\\n(?<SayadiIdentity>[0-9]{1,16})$" }
					},
					new WebChequeQrParserTemplate
					{
						BankType = EnBank.Mellat,
						RegexList = new List<string> { "^(?<Temp1>\\d{1,2})\\r?\\n(?<Temp2>\\d{1,1})\\r?\\n(?<NationalCode>\\d{1,10})\\r?\\n(?<ShabaNumber>IR\\d{24})\\r?\\n(?<BranchCode>[0-9]{1,2}_[0-9]{1,5})\\r?\\n(?<Serial>[0-9]{1,8})\\r?\\n(?<SayadiIdentity>[0-9]{1,16})$" }
					},
					new WebChequeQrParserTemplate
					{
						BankType = EnBank.Ayande,
						RegexList = new List<string> { "^(?<Temp1>\\d{1,2})\\r?\\n(?<Temp2>\\d{1,1})\\r?\\n(?<NationalCode>\\d{1,10})\\r?\\n(?<ShabaNumber>IR\\d{24})\\r?\\n(?<BranchCode>[0-9]{1,3}_[0-9]{1,7})\\r?\\n(?<Serial>[0-9]{1,8})\\r?\\n(?<SayadiIdentity>[0-9]{1,16})$" }
					},
					new WebChequeQrParserTemplate
					{
						BankType = EnBank.MehrIran,
						RegexList = new List<string> { "^(?<Temp1>\\d{1,2})\\r?\\n(?<Temp2>\\d{1,1})\\r?\\n(?<NationalCode>\\d{1,10})\\r?\\n(?<ShabaNumber>IR\\d{24})\\r?\\n(?<BranchCode>[0-9]{1,2}-[0-9]{1,7})\\r?\\n(?<Serial>[0-9]{1,8})\\r?\\n(?<SayadiIdentity>[0-9]{1,16})$" }
					}
				}
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetDefaults", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Global\\ChequeQrSetting.cs", 139);
			throw;
		}
	}
}
