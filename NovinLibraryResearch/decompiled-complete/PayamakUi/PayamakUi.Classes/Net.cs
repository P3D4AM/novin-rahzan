using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PacketParser.Entities;

namespace PayamakUi.Classes;

public class Net
{
	private string _User = "";

	private string _Pass = "";

	private string _Sender_Number = "";

	private string _Domain = "";

	private SendType _SendType;

	private Mode _SMSMode = Mode.SaveInPhone;

	private string _Number = "";

	private int _itemIndex;

	private string _Message = "";

	private List<string> _MessageList;

	private List<string> _Numbers;

	private List<DateTime> _ScadulingDate;

	public Net(string message, List<string> Numbers, List<DateTime> Dates, string User, string Pass, string Sender_Number, string Domain)
	{
		try
		{
			_Message = message;
			_Numbers = Numbers;
			_ScadulingDate = Dates;
			_User = User;
			_Pass = Pass;
			_Sender_Number = Sender_Number;
			_Domain = Domain;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Classes\\Net.cs", 42);
		}
	}

	public Net(string message, string Number, List<DateTime> Dates, string User, string Pass, string Sender_Number, string Domain)
	{
		try
		{
			_Message = message;
			_Number = Number;
			_ScadulingDate = Dates;
			_User = User;
			_Pass = Pass;
			_Sender_Number = Sender_Number;
			_Domain = Domain;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Classes\\Net.cs", 57);
		}
	}

	public Net(string User, string Pass, string Sender_Number, string Domain)
	{
		try
		{
			_User = User;
			_Pass = Pass;
			_Sender_Number = Sender_Number;
			_Domain = Domain;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Classes\\Net.cs", 69);
		}
	}

	public async Task<bool> SaveAsync()
	{
		try
		{
			_ = _ScadulingDate.Count;
			_ = 0;
			_Message.ToUpper().Contains("FIELD");
			_itemIndex = 0;
			using (List<string>.Enumerator enumerator = _Numbers.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					_ = enumerator.Current;
					_ = _Message;
					_itemIndex++;
					throw new NotImplementedException("چند خط کد خطا داشت نمیدونستم آیا کاربرد داره یا نه که کامنت کردم");
				}
			}
			return true;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Classes\\Net.cs", 128);
			return false;
		}
	}

	public bool Saveexcel()
	{
		try
		{
			_ = _ScadulingDate.Count;
			_ = 0;
			return true;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Saveexcel", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Classes\\Net.cs", 151);
			return false;
		}
	}

	private DateTime GetDate()
	{
		try
		{
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
			WebErrorLog.Instance.StartLog(ex, "", "GetDate", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Classes\\Net.cs", 172);
			return DateTime.Now;
		}
	}

	public async Task<bool> SendAsync(List<object> List, string messageTemplate, int accountId)
	{
		try
		{
			if (List == null)
			{
				return false;
			}
			throw new NotImplementedException("چند خط کد زیر (بلوک فور ایچ کامل) خطا داشت نمیدونستم آیا کاربرد داره یا نه که کامنت کردم");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SendAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Classes\\Net.cs", 223);
			return true;
		}
	}
}
