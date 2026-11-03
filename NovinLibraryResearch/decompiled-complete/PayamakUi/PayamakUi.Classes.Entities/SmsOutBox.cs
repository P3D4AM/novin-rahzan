using System;
using System.Drawing;
using PacketParser.Entities;
using PacketParser.Payamak.Entities;

namespace PayamakUi.Classes.Entities;

public class SmsOutBox : BaseSmsOutBox
{
	public Image Img { get; set; }

	public SmsOutBox(string message, string number, string senderNumber)
	{
		try
		{
			base.Number = number;
			base.Message = message;
			base.SenderNumber = senderNumber;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Classes\\Entities\\SMSOutBox.cs", 21);
		}
	}

	public SmsOutBox(string message = "", string number = "", short priority = 0, DateTime? schedulingM = null)
	{
		try
		{
			base.Message = message;
			base.Number = number;
			base.Priority = priority;
			base.SchadulingM = schedulingM ?? DateTime.Now;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Classes\\Entities\\SMSOutBox.cs", 32);
		}
	}

	public SmsOutBox(string message, string number, string senderNumber, int refrencedId, int accountId)
	{
		try
		{
			base.Number = number;
			base.Message = message;
			base.AccountId = accountId;
			base.SenderNumber = senderNumber;
			base.RefrencedId = refrencedId;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Classes\\Entities\\SMSOutBox.cs", 44);
		}
	}

	public SmsOutBox(string message, string number, bool delivery, short priority, DateTime schadulingM)
	{
		try
		{
			base.Number = number;
			base.Message = message;
			base.Priority = priority;
			base.Delivery = delivery;
			base.SchadulingM = schadulingM;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Classes\\Entities\\SMSOutBox.cs", 56);
		}
	}

	public SmsOutBox(string message, string number, bool delivery, string encoding, short priority)
	{
		try
		{
			base.Number = number;
			base.Message = message;
			base.Priority = priority;
			base.Delivery = delivery;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Classes\\Entities\\SMSOutBox.cs", 67);
		}
	}
}
