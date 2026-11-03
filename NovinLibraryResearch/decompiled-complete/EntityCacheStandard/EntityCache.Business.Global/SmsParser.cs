using System;
using System.Linq;
using System.Text.RegularExpressions;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Accounting;
using PacketParser.Entities.HesabYar;

namespace EntityCache.Business.Global;

public class SmsParser
{
	private const string dontLoseStarChar = "aasdasdf";

	private readonly Regex regex;

	private string smsMessage;

	private const string _type = "Type";

	private const string _accountNumber = "AccountNumber";

	private const string _amount = "Amount";

	private const string _date = "Date";

	private const string _time = "Time";

	private const string _balance = "Balance";

	private const string _number = "Number";

	public string Type { get; set; }

	public string AccountNumber { get; set; }

	public decimal Amount { get; set; }

	public DateTime Date { get; set; }

	public decimal Balance { get; set; }

	public bool IsMatch { get; set; }

	public string SmsMessage
	{
		get
		{
			return smsMessage;
		}
		set
		{
			smsMessage = value.Replace("*", "aasdasdf").FixString().Replace("aasdasdf", "*")
				.KeepCharactersForSmsParser();
		}
	}

	public string SmsSender { get; set; }

	public string Number { get; set; }

	public string BankName { get; set; }

	public Guid Guid { get; set; } = Guid.NewGuid();


	public int NotifId { get; set; }

	public bool IsReception { get; set; }

	public SmsParser()
	{
	}

	public SmsParser(AndroidSms message, SmsParserTemplate template)
	{
		try
		{
			if (template == null)
			{
				throw new NullReferenceException("template is null!!");
			}
			if (message == null)
			{
				throw new NullReferenceException("message is null!!");
			}
			if (!template.RegexNumbers.Select((string p) => p.ToLower().FixString().Trim()).Contains(message.Sender.ToLower().FixString().Trim()))
			{
				return;
			}
			string text = "aasdasdf";
			SmsMessage = message.Message;
			SmsSender = message.Sender;
			regex = new Regex(template.StrRegex.Replace("*", text).FixString().Replace(text, "*")
				.KeepCharactersForSmsParser());
			Match match = regex.Match(SmsMessage);
			if (match.Success)
			{
				Type = match.Groups["Type"].Value;
				AccountNumber = match.Groups["AccountNumber"].Value;
				Amount = decimal.Parse(match.Groups["Amount"].Value.Replace(",", ""));
				string text2 = match.Groups["Time"]?.Value ?? "";
				if (string.IsNullOrEmpty(text2))
				{
					text2 = $"{DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second}";
				}
				string value = match.Groups["Date"].Value;
				Date = value.ShamsiToMiladi(text2, "/", value.Length <= 5);
				Balance = decimal.Parse(match.Groups["Balance"].Value);
				IsMatch = true;
				Number = match.Groups["Number"].Value;
				IsReception = template.IsReception;
				if (Amount < 0m)
				{
					IsReception = false;
				}
				BankName = template.BankName;
			}
		}
		catch (NullReferenceException ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\SmsParserBusiness.cs", 85);
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\SmsParserBusiness.cs", 88);
		}
	}

	private void SendLogToTelegram(AndroidSms message, SmsParserTemplate template)
	{
		string text = (IsMatch ? "✅✅✅✅✅✅✅✅✅✅✅✅✅" : "❌❌❌❌❌❌❌❌❌❌❌❌❌");
		WebTelegramMessage.GetNp_NovinPardaz_Android_sms_Parser_bot().Sent2web("#smsReceved\r\n\r\nsender:" + message.Sender + "\r\n" + text + "\r\n" + $"message:{message}\r\n" + "template:" + Json.ToStringJson(template, indented: true), null, "SendLogToTelegram", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\SmsParserBusiness.cs", 94);
	}
}
