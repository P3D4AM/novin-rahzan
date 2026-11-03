using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClientHub.AccountingHubs;
using PacketParser.Entities;
using PayamakUi.Classes.Entities;

namespace PayamakUi.Classes.GSM;

public class GSM
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

	public GSM(string contextGuid, string message, List<string> Numbers)
	{
		try
		{
			_Message = message;
			_Numbers = Numbers;
			_instance = PayamakClientHubInstances.GetInstance(contextGuid);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Classes\\GSM\\GSM.cs", 37);
		}
	}

	public GSM(string message, string Number, short Priority, List<DateTime> ScadulingDate)
	{
		try
		{
			_Message = message;
			_Number = Number;
			_Priority = Priority;
			_ScadulingDate = ScadulingDate;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Classes\\GSM\\GSM.cs", 52);
		}
	}

	public GSM(string message, string Number, short Priority)
	{
		try
		{
			_Message = message;
			_Number = Number;
			_Priority = Priority;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Classes\\GSM\\GSM.cs", 66);
		}
	}

	public GSM(string message, List<string> numbers, short Priority)
	{
		try
		{
			_Message = message;
			_Numbers = numbers;
			_Priority = Priority;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Classes\\GSM\\GSM.cs", 80);
		}
	}

	public GSM(string message, List<string> numbers, short Priority, bool Delivery)
	{
		try
		{
			_Message = message;
			_Numbers = numbers;
			_Priority = Priority;
			_Delivery = Delivery;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Classes\\GSM\\GSM.cs", 95);
		}
	}

	public GSM(string message, List<string> numbers, short Priority, bool Delivery, string Encoding)
	{
		try
		{
			_Message = message;
			_Numbers = numbers;
			_Priority = Priority;
			_Delivery = Delivery;
			_Encoding = Encoding;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Classes\\GSM\\GSM.cs", 111);
		}
	}

	public GSM(string message, List<string> numbers, short Priority, bool Delivery, string Encoding, List<DateTime> ScadulingDate)
	{
		try
		{
			_Message = message;
			_Numbers = numbers;
			_Priority = Priority;
			_Delivery = Delivery;
			_Encoding = Encoding;
			_ScadulingDate = ScadulingDate;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Classes\\GSM\\GSM.cs", 128);
		}
	}

	public GSM(string message, string number, List<DateTime> ScadulingDate)
	{
		try
		{
			_Message = message;
			_Number = number;
			_ScadulingDate = ScadulingDate;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Classes\\GSM\\GSM.cs", 142);
		}
	}

	public async Task SaveAsync(string contextGuid)
	{
		int num = 0;
		try
		{
			throw new NotImplementedException();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"Line:{num}", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Classes\\GSM\\GSM.cs", 212);
		}
	}

	public async Task SaveExcelAsync()
	{
		_ = 1;
		try
		{
			if (_ScadulingDate == null || !_ScadulingDate.Any())
			{
				DateTime now = DateTime.Now;
				SmsOutBox item = new SmsOutBox(_Message, _Number, _Delivery, _Priority, now);
				await _instance.SmsOutBox.SaveAsync(item);
				return;
			}
			foreach (DateTime item3 in _ScadulingDate)
			{
				SmsOutBox item2 = new SmsOutBox(_Message, _Number, _Delivery, _Priority, item3);
				await _instance.SmsOutBox.SaveAsync(item2);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveExcelAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Classes\\GSM\\GSM.cs", 232);
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
			WebErrorLog.Instance.StartLog(ex, "", "GetDate", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Classes\\GSM\\GSM.cs", 260);
			return DateTime.Now;
		}
	}
}
