using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using PacketParser;
using PacketParser.BusinessLayer.Models.Cheque;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Settings.Global;

namespace EntityCache.Business.Global;

public class ChequeQrParserBusiness : IHasContext
{
	private const string _nationalCode = "NationalCode";

	private const string _shabaNumber = "ShabaNumber";

	private const string _branchCode = "BranchCode";

	private const string _serial = "Serial";

	private const string _sayadiIdentity = "SayadiIdentity";

	public string OriginalQr { get; private set; }

	public string ContextGuid { get; private set; }

	public EnBank BankType { get; private set; }

	public ChequeQrDetect Model { get; private set; }

	public ChequeQrParserBusiness(string contextGuid, string qr)
	{
		try
		{
			ContextGuid = contextGuid;
			OriginalQr = qr.Replace(" ", "").Replace("\r\n\r\n", "\r\n").FixString()
				.Trim();
			DetectBank();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\ChequeQrParserBusiness.cs", 37);
		}
	}

	private void DetectBank()
	{
		try
		{
			if (string.IsNullOrEmpty(OriginalQr))
			{
				return;
			}
			List<string> list = OriginalQr.SplitString("\r\n")?.Where((string q) => !string.IsNullOrEmpty(q))?.ToList();
			if (list != null && list.Any())
			{
				string text = list.SingleOrDefault((string q) => q.ToLower().StartsWith("ir"));
				if (!string.IsNullOrEmpty(text))
				{
					BankType = BankBusiness.DetecetBankByShabaNumber(text);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DetectBank", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\ChequeQrParserBusiness.cs", 51);
		}
	}

	public async Task CalculateAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			if (BankType == EnBank.None)
			{
				WebErrorLog.Instance.StartLog("Bank was Not Detected \r\n Qr is: \r\n " + OriginalQr, "", "CalculateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\ChequeQrParserBusiness.cs", 59);
				return;
			}
			ChequeQrSetting chequeQrSetting = await LocalSettingBusiness.GetChequeQrListAsync(ContextGuid, loadDefaults: true, token, tranName);
			if ((chequeQrSetting?.Templates?.Any()).GetValueOrDefault())
			{
				WebChequeQrParserTemplate webChequeQrParserTemplate = chequeQrSetting.Templates.SingleOrDefault((WebChequeQrParserTemplate q) => q.BankType == BankType);
				if ((webChequeQrParserTemplate?.RegexList?.Any()).GetValueOrDefault())
				{
					foreach (string regex in webChequeQrParserTemplate.RegexList)
					{
						Match match = Regex.Match(OriginalQr, regex, RegexOptions.Multiline);
						if (match.Success)
						{
							Model = new ChequeQrDetect
							{
								BranchCode = match.Groups["BranchCode"].Value,
								ChequeNumber = match.Groups["Serial"].Value.WordPressFixName().TrimEnd(Array.Empty<char>()),
								NationalCode = match.Groups["NationalCode"].Value.WordPressFixName().TrimEnd(Array.Empty<char>()),
								SayadiNumber = match.Groups["SayadiIdentity"].Value.WordPressFixName().TrimEnd(Array.Empty<char>()),
								ShabaCode = match.Groups["ShabaNumber"].Value,
								BankType = BankType
							};
							return;
						}
					}
				}
			}
			WebErrorLog.Instance.StartLog("Regex was Not Detected \r\n Qr is: \r\n " + OriginalQr, "", "CalculateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\ChequeQrParserBusiness.cs", 88);
			List<string> list = OriginalQr.SplitString("\r\n")?.Where((string q) => !string.IsNullOrEmpty(q))?.ToList();
			if (list != null && list.Any())
			{
				Model = new ChequeQrDetect
				{
					NationalCode = list[2],
					ShabaCode = list[3],
					BranchCode = list[4],
					ChequeNumber = list[5],
					SayadiNumber = list[6],
					BankType = BankType
				};
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CalculateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\ChequeQrParserBusiness.cs", 101);
		}
	}
}
