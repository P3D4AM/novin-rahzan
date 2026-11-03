using System;
using PacketParser.Entities;

namespace LK;

public class ActivatedSerial
{
	private string _activeCode;

	private short _scaleCount;

	private short _days;

	private short _clientNetNumber;

	public bool Isvalid = true;

	public string ActiveCode
	{
		get
		{
			return _activeCode;
		}
		set
		{
			_activeCode = value;
		}
	}

	public short ScaleCount
	{
		get
		{
			return _scaleCount;
		}
		set
		{
			_scaleCount = value;
		}
	}

	public short Days
	{
		get
		{
			return _days;
		}
		set
		{
			_days = value;
		}
	}

	public short ClientNetNumber
	{
		get
		{
			return _clientNetNumber;
		}
		set
		{
			_clientNetNumber = value;
		}
	}

	public ActivatedSerial(string ActivationCode)
	{
		parseActivationCode(ActivationCode);
	}

	public void parseActivationCode(string Acode)
	{
		try
		{
			if (Acode.Length < 10)
			{
				Isvalid = false;
			}
			else if (Acode.Length == 16)
			{
				parseScaleCount(Acode);
			}
			else if ((Acode.Length == 12) | (Acode.Length == 10))
			{
				ActiveCode = Acode;
			}
			else if (Acode.Length == 14)
			{
				parseOtherAppCode(Acode);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "parseActivationCode", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\ActivatedSerial.cs", 92);
			Isvalid = false;
		}
	}

	private void parseOtherAppCode(string acode)
	{
		try
		{
			ClientNetNumber = Convert.ToInt16(acode.Substring(acode.Length - 1, 1) + acode.Substring(0, 1));
			acode = acode.Remove(0, 1);
			acode = acode.Remove(acode.Length - 1, 1);
			Days = Convert.ToInt16(acode.Substring(11, 1) + acode.Substring(4, 1));
			acode = Convert.ToString(acode.Remove(4, 1).Remove(10, 1));
			ActiveCode = acode;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "parseOtherAppCode", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\ActivatedSerial.cs", 110);
			Isvalid = false;
		}
	}

	private void parseScaleCount(string acode)
	{
		try
		{
			ClientNetNumber = Convert.ToInt16(acode.Substring(acode.Length - 1, 1) + acode.Substring(0, 1));
			acode = acode.Remove(0, 1);
			acode = acode.Remove(acode.Length - 1, 1);
			ScaleCount = Convert.ToInt16(acode.Substring(12, 1) + acode.Substring(4, 1));
			Days = Convert.ToInt16(acode.Substring(13, 1) + acode.Substring(6, 1));
			acode = Convert.ToString(acode.Remove(4, 1).Remove(5, 1).Remove(10, 1)
				.Remove(10, 1));
			ActiveCode = acode;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "parseScaleCount", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\ActivatedSerial.cs", 129);
			Isvalid = false;
		}
	}
}
