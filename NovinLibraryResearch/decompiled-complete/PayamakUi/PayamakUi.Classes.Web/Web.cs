using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ClientHub.AccountingHubs;
using PacketParser.Entities;
using PayamakUi.Classes.Entities;

namespace PayamakUi.Classes.Web;

public class Web
{
	private int _itemIndex;

	private string _Message = "";

	private List<string> _MessageList;

	private List<string> _Numbers;

	private string _Number;

	private bool _Delivery;

	private string _Encoding = "Unicode_16Bit";

	private short _Priority;

	private List<DateTime> _ScadulingDate;

	private PayamakClientHubInstances _instance;

	public Web(string contextGuid, string message, List<string> Numbers)
	{
		try
		{
			_Message = message;
			_Numbers = Numbers;
			_instance = PayamakClientHubInstances.GetInstance(contextGuid);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Classes\\Web\\Web.cs", 32);
		}
	}

	public Web(string contextGuid, string message, string Number)
	{
		try
		{
			_Message = message;
			_Number = Number;
			_instance = PayamakClientHubInstances.GetInstance(contextGuid);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Classes\\Web\\Web.cs", 43);
		}
	}

	public Web(string contextGuid, string message, List<string> numbers, short Priority)
	{
		try
		{
			_Message = message;
			_Numbers = numbers;
			_Priority = Priority;
			_instance = PayamakClientHubInstances.GetInstance(contextGuid);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Classes\\Web\\Web.cs", 57);
		}
	}

	public Web(string contextGuid, string message, List<string> numbers, short Priority, bool Delivery)
	{
		try
		{
			_Message = message;
			_Numbers = numbers;
			_Priority = Priority;
			_Delivery = Delivery;
			_instance = PayamakClientHubInstances.GetInstance(contextGuid);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Classes\\Web\\Web.cs", 73);
		}
	}

	public Web(string contextGuid, string message, List<string> numbers, short Priority, bool Delivery, string Encoding)
	{
		try
		{
			_Message = message;
			_Numbers = numbers;
			_Priority = Priority;
			_Delivery = Delivery;
			_Encoding = Encoding;
			_instance = PayamakClientHubInstances.GetInstance(contextGuid);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Classes\\Web\\Web.cs", 90);
		}
	}

	public Web(string contextGuid, string message, List<string> numbers, short Priority, bool Delivery, string Encoding, List<DateTime> ScadulingDate)
	{
		try
		{
			_Message = message;
			_Numbers = numbers;
			_Priority = Priority;
			_Delivery = Delivery;
			_Encoding = Encoding;
			_ScadulingDate = ScadulingDate;
			_instance = PayamakClientHubInstances.GetInstance(contextGuid);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Classes\\Web\\Web.cs", 108);
		}
	}

	public Web(string contextGuid, string message, string number, List<DateTime> ScadulingDate)
	{
		try
		{
			_Message = message;
			_Number = number;
			_ScadulingDate = ScadulingDate;
			_instance = PayamakClientHubInstances.GetInstance(contextGuid);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Classes\\Web\\Web.cs", 123);
		}
	}

	public async Task SaveExcel()
	{
		try
		{
			SmsOutBox item = new SmsOutBox(_Message, _Number, _Delivery, _Priority, GetDate());
			await _instance.SmsOutBox.SaveAsync(item);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveExcel", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Classes\\Web\\Web.cs", 164);
		}
	}

	private DateTime GetDate()
	{
		try
		{
			if (_ScadulingDate == null)
			{
				return DateTime.Now;
			}
			_itemIndex++;
			int num = (int)Math.Round((double)_Numbers.Count / (double)_ScadulingDate.Count);
			int num2 = (int)Math.Round(Math.Round((double)_itemIndex / (double)num - 0.5));
			if (num2 < 0)
			{
				num2 = 0;
			}
			if (num2 < _ScadulingDate.Count)
			{
				return _ScadulingDate[num2];
			}
			return _ScadulingDate[_ScadulingDate.Count - 1];
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetDate", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Classes\\Web\\Web.cs", 192);
			return DateTime.Now;
		}
	}
}
