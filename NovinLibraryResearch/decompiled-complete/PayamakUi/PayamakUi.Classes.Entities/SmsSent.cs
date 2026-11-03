using System;
using System.Drawing;
using PacketParser.Entities;
using PacketParser.Payamak.Entities;

namespace PayamakUi.Classes.Entities;

public class SmsSent : BaseSmsSent
{
	public Image Img { get; set; }

	public bool IsChecked { get; set; }

	public SmsSent(string message, string number, string refrencedIds, DateTime dateM, DateTime sendDateM)
	{
		try
		{
			base.Message = message;
			base.Number = number;
			base.RefrencedIds = refrencedIds;
			base.DateM = DateTime.Now;
			base.SendDateM = sendDateM;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Classes\\Entities\\SMSSent.cs", 25);
		}
	}

	public SmsSent(string message, string number, string refrencedIds, DateTime dateM, DateTime sendDateM, string senderNumber)
	{
		try
		{
			base.Message = message;
			base.Number = number;
			base.RefrencedIds = refrencedIds;
			base.DateM = DateTime.Now;
			base.SendDateM = sendDateM;
			base.SenderNumber = senderNumber;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Classes\\Entities\\SMSSent.cs", 38);
		}
	}

	public SmsSent()
	{
		try
		{
			base.DateM = DateTime.Now;
			base.RefrencedIds = "";
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Classes\\Entities\\SMSSent.cs", 47);
		}
	}
}
