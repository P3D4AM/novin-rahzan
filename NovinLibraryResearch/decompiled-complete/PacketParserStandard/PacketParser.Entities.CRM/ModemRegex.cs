using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PacketParser.Entities.CRM;

public class ModemRegex
{
	private static List<string> _regexHookOn;

	private static string _regexHookOnDf1 = "L*(?<Line>[ 0-9]*):Onhook@";

	private static string _regexHookOnDf2 = "D(?<Device>[0-9]+):L(?<Line>[0-9]+):HookOn.";

	private static List<string> _regexHookOff;

	private static string _regexHookOffDf1 = "L*(?<Line>[ 0-9]*):Offhook@";

	private static string _regexHookOffDf2 = "D(?<Device>[0-9]+):L(?<Line>[0-9]+):HookOff.";

	private static List<string> _regexDialed;

	private static string _regexDialedDf1 = "L*(?<Line>[ 0-9]*):Dialed:(?<Number>[0-9]*)@";

	private static string _regexDialedDf2 = "D(?<Device>[0-9]+):L(?<Line>[0-9]+):DIAL=(?<Number>[0-9]+).";

	private static List<string> _regexDialedFromServer;

	private static string _regexDialedFromServerDf1 = "D(?<Device>[0-9]+):L(?<Line>[0-9]+):Duration=(?<Duration>[0-9]*):DialedFromServer=(?<Number>[0-9]+).";

	private static List<string> _regexCallerIdNumber;

	private static string _regexCallerIdNumberDf1 = "L*(?<Line>[ 0-9]*):(?<Number>[0-9]*)@";

	private static string _regexCallerIdNumberDf2 = "D(?<Device>[0-9]+):L(?<Line>[0-9]+):CallerID=(?<Number>[Bb+]*[0-9]+).";

	private static List<string> _regexCallerIdNumberDataNumber;

	private static string _regexCallerIdNumberDataNumberDf = ":[0-9]+@";

	private static List<string> _regexCallerIdNumberDataLine;

	private static string _regexCallerIdNumberDataLineDf = "L*[ 0-9]*:";

	private static List<string> _initializer;

	private static List<string> _regexMissCall;

	private static string _regexMissCallDf = "D(?<Device>[0-9]+):L(?<Line>[0-9]+):(?<Action>MissedCall),(?<Msg>Ring=(?<Other>[0-9]+)).";

	private static List<string> _regexDeviceConnect;

	private static string _regexDeviceConnectDf = "D(?<Device>[0-9]+):(?<Action>DeviceConnect),Serial=(?<Msg>[0-9]+),CountLine=(?<LINE>[0-9]+).";

	private static List<string> _regexDeviceDisconnect;

	private static string _regexDeviceDisconnectDf = "D(?<Device>[0-9]+):DeviceDisconnect.";

	private static List<string> _regexLineDisconnect;

	private static string _regexLineDisconnectDf = "D(?<Device>[0-9]+):L(?<Line>[0-9]+):LineDisconnect.";

	private static List<string> _regexAnswer;

	private static List<string> _regexRing;

	private static string _regexRingDf = "D(?<Device>[0-9]+):L(?<Line>[0-9]+):(?<Action>Ring)=(?<MSG>[0-9]*).";

	private static List<string> _regexAnyAction;

	private static string _regexAnyActionDf2 = "D(?<Device>[0-9]+):L(?<Line>[0-9]+):Answer.";

	public static string Path { get; set; }

	public static string Content { get; set; }

	public static List<string> RegexHookOn
	{
		get
		{
			try
			{
				if (_regexHookOn == null)
				{
					_regexHookOn = ReadLineNo("HookOn :", Path ?? "");
				}
				if (_regexHookOn.Count == 0)
				{
					_regexHookOn.Add(_regexHookOnDf1);
					_regexHookOn.Add(_regexHookOnDf2);
					WriteLineNo("HookOn :", _regexHookOn, Path ?? "", Content ?? "");
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RegexHookOn", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\ModemRegex.cs", 38);
			}
			return _regexHookOn;
		}
		set
		{
			try
			{
				_regexHookOn = value;
				WriteLineNo("HookOn :", value, Path ?? "", Content ?? "");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RegexHookOn", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\ModemRegex.cs", 50);
			}
		}
	}

	public static List<string> RegexHookOff
	{
		get
		{
			try
			{
				if (_regexHookOff == null)
				{
					_regexHookOff = ReadLineNo("HookOff :", Path ?? "");
				}
				if (_regexHookOff.Count == 0)
				{
					_regexHookOff.Add(_regexHookOffDf1);
					_regexHookOff.Add(_regexHookOffDf2);
					WriteLineNo("HookOff :", _regexHookOff, Path ?? "", Content ?? "");
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RegexHookOff", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\ModemRegex.cs", 79);
			}
			return _regexHookOff;
		}
		set
		{
			try
			{
				_regexHookOff = value;
				WriteLineNo("HookOff :", value, Path ?? "", Content ?? "");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RegexHookOff", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\ModemRegex.cs", 92);
			}
		}
	}

	public static List<string> RegexDialed
	{
		get
		{
			try
			{
				if (_regexDialed == null)
				{
					_regexDialed = ReadLineNo("Dialed :", Path ?? "");
				}
				if (_regexDialed.Count == 0)
				{
					_regexDialed.Add(_regexDialedDf1);
					_regexDialed.Add(_regexDialedDf2);
					WriteLineNo("Dialed :", _regexDialed, Path ?? "", Content ?? "");
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RegexDialed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\ModemRegex.cs", 122);
			}
			return _regexDialed;
		}
		set
		{
			try
			{
				_regexDialed = value;
				WriteLineNo("Dialed :", value, Path ?? "", Content ?? "");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RegexDialed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\ModemRegex.cs", 135);
			}
		}
	}

	public static List<string> RegexDialedFromServer
	{
		get
		{
			try
			{
				if (_regexDialedFromServer == null)
				{
					_regexDialedFromServer = ReadLineNo("DialedFromServer :", Path ?? "");
				}
				if (_regexDialedFromServer.Count == 0)
				{
					_regexDialedFromServer.Add(_regexDialedFromServerDf1);
					WriteLineNo("DialedFromServer :", _regexDialedFromServer, Path ?? "", Content ?? "");
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RegexDialedFromServer", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\ModemRegex.cs", 163);
			}
			return _regexDialedFromServer;
		}
		set
		{
			try
			{
				_regexDialedFromServer = value;
				WriteLineNo("DialedFromServer :", value, Path ?? "", Content ?? "");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RegexDialedFromServer", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\ModemRegex.cs", 176);
			}
		}
	}

	public static List<string> RegexCallerIdNumber
	{
		get
		{
			try
			{
				if (_regexCallerIdNumber == null)
				{
					_regexCallerIdNumber = ReadLineNo("CallerID :", Path ?? "");
				}
				if (_regexCallerIdNumber.Count == 0)
				{
					_regexCallerIdNumber.Add(_regexCallerIdNumberDf1);
					_regexCallerIdNumber.Add(_regexCallerIdNumberDf2);
					WriteLineNo("CallerID :", _regexCallerIdNumber, Path ?? "", Content ?? "");
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RegexCallerIdNumber", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\ModemRegex.cs", 205);
			}
			return _regexCallerIdNumber;
		}
		set
		{
			try
			{
				_regexCallerIdNumber = value;
				WriteLineNo("CallerID :", value, Path ?? "", Content ?? "");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RegexCallerIdNumber", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\ModemRegex.cs", 218);
			}
		}
	}

	public static List<string> RegexCallerIdNumberDataNumber
	{
		get
		{
			try
			{
				if (_regexCallerIdNumberDataNumber == null)
				{
					_regexCallerIdNumberDataNumber = ReadLineNo("Regex_CallerIDNumber_Data_Number :", Path ?? "");
				}
				if (_regexCallerIdNumberDataNumber.Count == 0)
				{
					_regexCallerIdNumberDataNumber.Add(_regexCallerIdNumberDataNumberDf);
					WriteLineNo("Regex_CallerIDNumber_Data_Number :", _regexCallerIdNumberDataNumber, Path ?? "", Content ?? "");
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RegexCallerIdNumberDataNumber", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\ModemRegex.cs", 246);
			}
			return _regexCallerIdNumberDataNumber;
		}
		set
		{
			try
			{
				_regexCallerIdNumberDataNumber = value;
				WriteLineNo("Regex_CallerIDNumber_Data_Number :", value, Path ?? "", Content ?? "");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RegexCallerIdNumberDataNumber", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\ModemRegex.cs", 259);
			}
		}
	}

	public static List<string> RegexCallerIdNumberDataLine
	{
		get
		{
			try
			{
				if (_regexCallerIdNumberDataLine == null)
				{
					_regexCallerIdNumberDataLine = ReadLineNo("Regex_CallerIDNumber_Data_Line :", Path ?? "");
				}
				if (_regexCallerIdNumberDataLine.Count == 0)
				{
					_regexCallerIdNumberDataLine.Add(_regexCallerIdNumberDataLineDf);
					WriteLineNo("Regex_CallerIDNumber_Data_Line :", _regexCallerIdNumberDataLine, Path ?? "", Content ?? "");
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RegexCallerIdNumberDataLine", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\ModemRegex.cs", 286);
			}
			return _regexCallerIdNumberDataLine;
		}
		set
		{
			try
			{
				_regexCallerIdNumberDataLine = value;
				WriteLineNo("Regex_CallerIDNumber_Data_Line :", value, Path ?? "", Content ?? "");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RegexCallerIdNumberDataLine", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\ModemRegex.cs", 299);
			}
		}
	}

	public static List<string> Initializer
	{
		get
		{
			try
			{
				if (_initializer == null)
				{
					_initializer = ReadLineNo("Initializer", Path ?? "");
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Initializer", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\ModemRegex.cs", 320);
			}
			return _initializer;
		}
		set
		{
			try
			{
				_initializer = value;
				WriteLineNo(Convert.ToString(6), value, Path ?? "", Content ?? "");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Initializer", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\ModemRegex.cs", 333);
			}
		}
	}

	public static List<string> RegexMissCall
	{
		get
		{
			try
			{
				if (_regexMissCall == null)
				{
					_regexMissCall = ReadLineNo("MissedCall :", Path ?? "");
				}
				if (_regexMissCall.Count == 0)
				{
					_regexMissCall.Add(_regexMissCallDf);
					WriteLineNo("MissedCall :", _regexMissCall, Path ?? "", Content ?? "");
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RegexMissCall", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\ModemRegex.cs", 360);
			}
			return _regexMissCall;
		}
		set
		{
			try
			{
				_regexMissCall = value;
				WriteLineNo("MissedCall :", value, Path ?? "", Content ?? "");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RegexMissCall", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\ModemRegex.cs", 373);
			}
		}
	}

	public static List<string> RegexDeviceConnect
	{
		get
		{
			try
			{
				if (_regexDeviceConnect == null)
				{
					_regexDeviceConnect = ReadLineNo("DeviceConnect :", Path ?? "");
				}
				if (_regexDeviceConnect.Count == 0)
				{
					_regexDeviceConnect.Add(_regexDeviceConnectDf);
					WriteLineNo("DeviceConnect :", _regexDeviceConnect, Path ?? "", Content ?? "");
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RegexDeviceConnect", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\ModemRegex.cs", 400);
			}
			return _regexDeviceConnect;
		}
		set
		{
			try
			{
				_regexDeviceConnect = value;
				WriteLineNo("DeviceConnect :", value, Path ?? "", Content ?? "");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RegexDeviceConnect", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\ModemRegex.cs", 413);
			}
		}
	}

	public static List<string> RegexDeviceDisconnect
	{
		get
		{
			try
			{
				if (_regexDeviceDisconnect == null)
				{
					_regexDeviceDisconnect = ReadLineNo("DeviceDisconnect :", Path ?? "");
				}
				if (_regexDeviceDisconnect.Count == 0)
				{
					_regexDeviceDisconnect.Add(_regexDeviceDisconnectDf);
					WriteLineNo("DeviceDisconnect :", _regexDeviceDisconnect, Path ?? "", Content ?? "");
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RegexDeviceDisconnect", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\ModemRegex.cs", 440);
			}
			return _regexDeviceDisconnect;
		}
		set
		{
			try
			{
				_regexDeviceDisconnect = value;
				WriteLineNo("DeviceDisconnect :", value, Path ?? "", Content ?? "");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RegexDeviceDisconnect", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\ModemRegex.cs", 453);
			}
		}
	}

	public static List<string> RegexLineDisconnect
	{
		get
		{
			try
			{
				if (_regexLineDisconnect == null)
				{
					_regexLineDisconnect = ReadLineNo("LineDisconnect :", Path ?? "");
				}
				if (_regexLineDisconnect.Count == 0)
				{
					_regexLineDisconnect.Add(_regexLineDisconnectDf);
					WriteLineNo("LineDisconnect :", _regexLineDisconnect, Path ?? "", Content ?? "");
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RegexLineDisconnect", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\ModemRegex.cs", 480);
			}
			return _regexLineDisconnect;
		}
		set
		{
			try
			{
				_regexLineDisconnect = value;
				WriteLineNo("LineDisconnect :", value, Path ?? "", Content ?? "");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RegexLineDisconnect", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\ModemRegex.cs", 493);
			}
		}
	}

	public static List<string> RegexRing
	{
		get
		{
			try
			{
				if (_regexRing == null)
				{
					_regexRing = ReadLineNo("Ring :", Path ?? "");
				}
				if (_regexRing.Count == 0)
				{
					_regexRing.Add(_regexRingDf);
					WriteLineNo("Ring :", _regexRing, Path ?? "", Content ?? "");
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RegexRing", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\ModemRegex.cs", 521);
			}
			return _regexRing;
		}
		set
		{
			try
			{
				_regexRing = value;
				WriteLineNo("Ring :", value, Path ?? "", Content ?? "");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RegexRing", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\ModemRegex.cs", 534);
			}
		}
	}

	public static List<string> RegexAnswer
	{
		get
		{
			try
			{
				if (_regexAnswer == null)
				{
					_regexAnswer = ReadLineNo("Answer :", Path ?? "");
				}
				if (_regexAnswer.Count == 0)
				{
					_regexAnswer.Add(_regexAnyActionDf2);
					WriteLineNo("Answer :", _regexAnswer, Path ?? "", Content ?? "");
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RegexAnswer", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\ModemRegex.cs", 556);
			}
			return _regexAnswer;
		}
		set
		{
			try
			{
				_regexAnswer = value;
				WriteLineNo("Answer :", value, Path ?? "", Content ?? "");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RegexAnswer", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\ModemRegex.cs", 569);
			}
		}
	}

	public static List<string> RegexAnyAction
	{
		get
		{
			try
			{
				if (_regexAnyAction == null)
				{
					_regexAnyAction = ReadLineNo("AnyAction :", Path ?? "");
				}
				if (_regexAnyAction.Count == 0)
				{
					_regexAnyAction.Add(_regexAnyActionDf2);
					WriteLineNo("AnyAction :", _regexAnyAction, Path ?? "", Content ?? "");
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RegexAnyAction", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\ModemRegex.cs", 598);
			}
			return _regexAnyAction;
		}
		set
		{
			try
			{
				_regexAnyAction = value;
				WriteLineNo("AnyAction :", value, Path ?? "", Content ?? "");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RegexAnyAction", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\ModemRegex.cs", 611);
			}
		}
	}

	private static List<string> ReadLineNo(string sample, string path)
	{
		List<string> list = new List<string>();
		sample = sample.ToUpper();
		try
		{
			string path2 = System.IO.Path.Combine(path, "Modem.txt");
			if (!File.Exists(path2))
			{
				return list;
			}
			foreach (string item in File.ReadAllLines(path2)?.ToList())
			{
				if (item.ToUpper().StartsWith(sample))
				{
					string text = item.ToUpper().Replace(sample, "");
					list.Add(text.Trim());
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ReadLineNo", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\ModemRegex.cs", 641);
		}
		return list;
	}

	private static void WriteLineNo(string regText, List<string> data, string path, string content)
	{
		try
		{
			string path2 = System.IO.Path.Combine(path, "Modem.txt");
			if (!File.Exists(path2))
			{
				File.WriteAllText(path2, "");
			}
			List<string> list = File.ReadAllLines(path2)?.ToList();
			short num = 0;
			num = 0;
			while (num < list.Count)
			{
				if (list[num].StartsWith(regText))
				{
					list.RemoveAt(num);
				}
				else
				{
					num = checked((short)(num + 1));
				}
			}
			foreach (string datum in data)
			{
				list.Add(regText + datum);
			}
			File.WriteAllLines(path2, list.ToArray());
			string path3 = System.IO.Path.Combine(path, "Modem_Helper.txt");
			if (!File.Exists(path3))
			{
				File.WriteAllText(path3, content);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "WriteLineNo", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\ModemRegex.cs", 677);
		}
	}
}
