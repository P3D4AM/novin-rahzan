using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PacketParser.Assistance;
using PacketParser.Entities.CRM.NewEvents;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;

namespace PacketParser.Entities.CRM;

public class WebModem : INpDisposable, IDisposable, IReceiver
{
	private enum ModemActions
	{
		Unknown = 0,
		Dialed = 1,
		HookOn = 2,
		HookOff = 5,
		MissedCall = 6,
		Ring = 7,
		CallerIdNumber = 8,
		DeviceConnect = 9,
		DeviceDisConnect = 10,
		Answer = 11,
		LineDisconnect = 12,
		IncomingCall = 13,
		OutgoingCall = 14
	}

	private static List<Guid> _refrences = new List<Guid>();

	private static ConcurrentDictionary<short, string> DeviceNumberToserial = new ConcurrentDictionary<short, string>();

	private static List<Packet> retVal;

	[XmlIgnore]
	[JsonIgnore]
	public bool IsDisposed { get; private set; }

	public event Func<object, ConnectionEventArgs<AnswerEventArgument>, Task> AnswerEvent;

	public event Func<object, ConnectionEventArgs<DialedEventArgumen>, Task> DialedEvent;

	public event Func<object, ConnectionEventArgs<HookOffEventArgument>, Task> HookOffEvent;

	public event Func<object, ConnectionEventArgs<HookOnEventArgument>, Task> HookOnEvent;

	public event Func<object, ConnectionEventArgs<MissedCallEventArgument>, Task> MissEvent;

	public event Func<object, ConnectionEventArgs<NumberEventArgument>, Task> NumberEvent;

	public event Func<object, ConnectionEventArgs<OtherEventArgumant>, Task> DeviceConnecEvent;

	public event Func<object, ConnectionEventArgs<OtherEventArgumant>, Task> DeviceDisconnectEvent;

	public event Func<object, ConnectionEventArgs<RingEventArgument>, Task> RingEvent;

	public event Func<object, ConnectionEventArgs<LineDisconnectEventArgs>, Task> LineDisEvent;

	public event Func<object, ConnectionEventArgs<IncomingCallEventArgument>, Task> IncomingCallEvent;

	public event Func<object, ConnectionEventArgs<OutgoingCallEventArgument>, Task> OutgoingCallEvent;

	public static Task<List<Packet>> GeneratePacketAsync(DateTime date, string data, bool isTcpEvent, string connectionUniqueId, ConnectionData connection, string content)
	{
		retVal = new List<Packet>();
		int num = 0;
		if (string.IsNullOrEmpty(data))
		{
			throw new NullReferenceException("data");
		}
		try
		{
			if (string.IsNullOrEmpty(data) || data.ToLower().Contains("test") || data.ToLower().Contains("novinp"))
			{
				return Task.FromResult(retVal);
			}
			GenerateDialedEvent(date, data, connection, connectionUniqueId, isTcpEvent, content);
			GenerateDialedFromServerEvent(data, connection, connectionUniqueId, isTcpEvent, content);
			GenerateRingEvent(data, connection, connectionUniqueId, isTcpEvent, content);
			GenerateNumberEvent(data, connection, connectionUniqueId, isTcpEvent, content);
			GenerateHookOnEvent(data, connection, connectionUniqueId, isTcpEvent, content);
			GenerateHookOffEvent(data, connection, connectionUniqueId, isTcpEvent, content);
			GenerateDeviceConnectEvent(data, connection, connectionUniqueId, isTcpEvent, content);
			GenerateMissedCallEvent(data, connection, connectionUniqueId, isTcpEvent, content);
			GenerateAnswerEvent(data, connection, connectionUniqueId, isTcpEvent, content);
			GenerateLineDisconnectEvent(data, connection, connectionUniqueId, isTcpEvent, content);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "Line:" + num, "GeneratePacketAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\WebModem.cs", 76);
		}
		return Task.FromResult(retVal);
	}

	private static void GenerateDialedEvent(DateTime date, string data, ConnectionData connection, string connectionUniqueId, bool isTcpEvent, string content = null)
	{
		try
		{
			List<string> list = RegexInfo.Calc(data, ModemRegex.RegexDialed, content);
			if (list == null || !list.Any())
			{
				return;
			}
			foreach (string item in list)
			{
				data = data.ToLower().Replace(item.ToLower(), "");
				List<string> list2 = RegexInfo.CalcNumber(item, ModemRegex.RegexDialed);
				short num = 0;
				short num2 = 0;
				string text = "";
				if (list2.Count > 0 && !string.IsNullOrEmpty(list2[0]))
				{
					num = list2[0].ParseToShort(0);
				}
				if (list2.Count > 1 && !string.IsNullOrEmpty(list2[1]))
				{
					num2 = list2[1].ParseToShort(0);
				}
				if (list2.Count >= 3 && !string.IsNullOrEmpty(list2[2]))
				{
					text = list2[2].RemoveNoNumbers();
				}
				if (num2 == 0)
				{
					throw new Exception($"Generate Dialed Event!!!! Line Number is:{num2}");
				}
				if (num == 0)
				{
					throw new Exception($"Generate Dialed Event!!!! Device Number is:{num}");
				}
				List<Packet> list3 = NumberDialed.CallerIdInstance.Dialed(Guid.NewGuid(), GetSerial(num), num, num2, text?.ParseToShort(0) ?? 0, connectionUniqueId, connection, isTcpEvent, date);
				if (list3 != null)
				{
					retVal.AddRange(list3);
					connection.Context.WebModem.DataReceived(list3, connectionUniqueId, connection, isTcpEvent);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateDialedEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\WebModem.cs", 118);
		}
	}

	private static void GenerateDialedFromServerEvent(string data, ConnectionData connection, string connectionUniqueId, bool isTcpEvent, string content)
	{
		try
		{
			List<string> list = RegexInfo.Calc(data, ModemRegex.RegexDialedFromServer, content);
			if (list == null || list.Count <= 0)
			{
				return;
			}
			foreach (string item in list)
			{
				List<string> list2 = RegexInfo.CalcDialFromServer(item, list);
				short deviceNumber = list2[0]?.ParseToShort(0) ?? 0;
				short lineNumber = list2[1]?.ParseToShort(0) ?? 0;
				short duration = list2[2]?.ParseToShort(0) ?? 0;
				DialedEventArgumen dialedEventArgumen = new DialedEventArgumen(Guid.NewGuid(), deviceNumber, lineNumber, duration, list2[3] ?? "", isTcpEvent, GetSerial(deviceNumber));
				List<Packet> list3 = GenerateDialedFromServerRequest(new List<DialedEventArgumen> { dialedEventArgumen }, default(Guid));
				if (!(dialedEventArgumen.Number.Trim() == "") && dialedEventArgumen.Number.Length >= 3)
				{
					retVal.AddRange(list3);
					connection.Context.WebModem.DataReceived(list3, connectionUniqueId, connection, isTcpEvent);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateDialedFromServerEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\WebModem.cs", 143);
		}
	}

	private static void GenerateRingEvent(string data, ConnectionData connection, string connectionUniqueId, bool isTcpEvent, string content)
	{
		try
		{
			List<string> list = RegexInfo.Calc(data, ModemRegex.RegexRing, content);
			if (list == null || list.Count <= 0)
			{
				return;
			}
			foreach (string item in list)
			{
				List<string> list2 = RegexInfo.CalcOtherEvent(item, ModemRegex.RegexRing);
				short num = list2[0]?.ParseToShort(0) ?? 0;
				short num2 = list2[1]?.ParseToShort(0) ?? 0;
				Guid guid = Guid.NewGuid();
				RingEventArgument ringEventArgument = new RingEventArgument(guid, num, num2, list2[3]?.ParseToShort(0) ?? 0, isTcpEvent, GetSerial(num));
				if (num2 == 0)
				{
					throw new Exception($"Generate Ring Event!!!! Line Number is:{num2}");
				}
				if (num == 0)
				{
					throw new Exception($"Generate Ring Event!!!! Device Number is:{num}");
				}
				List<Packet> list3 = GenerateRingRequest(new List<RingEventArgument> { ringEventArgument }, Guid.NewGuid());
				if (ringEventArgument.LineNumber > 0)
				{
					retVal.AddRange(list3);
					connection.Context.WebModem.DataReceived(list3, connectionUniqueId, connection, isTcpEvent);
					Console.WriteLine($"Ring Event fired! line:{ringEventArgument.LineNumber}//Device:{ringEventArgument.DeviceNumber}//Serial:{ringEventArgument.DeviceSerial}//RingNo({ringEventArgument.RingNo}) PacketRefrence:{guid}");
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateRingEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\WebModem.cs", 169);
		}
	}

	private static void GenerateNumberEvent(string data, ConnectionData connection, string connectionUniqueId, bool isTcpEvent, string path = null, string content = null)
	{
		bool valueOrDefault = (data?.ToLower()?.Contains("CallerID".ToLower())).GetValueOrDefault();
		bool flag = false;
		try
		{
			List<string> list = RegexInfo.Calc(data, ModemRegex.RegexCallerIdNumber, content);
			if (list == null || !list.Any())
			{
				return;
			}
			foreach (string item in list)
			{
				List<string> list2 = RegexInfo.CalcNumber(item, ModemRegex.RegexCallerIdNumber);
				string text = list2[2].RemoveNoNumbers();
				string text2 = "";
				text2 = ((text.Length <= 10) ? text : ("0" + text.Substring(checked(text.Length - 10), 10)));
				short num = list2[0]?.ParseToShort(0) ?? 0;
				short num2 = list2[1]?.ParseToShort(0) ?? 0;
				if (num2 == 0)
				{
					throw new Exception($"Generate Number Event!!!! Line Number is:{num2}");
				}
				if (num == 0)
				{
					throw new Exception($"Generate Number Event!!!! Device Number is:{num}");
				}
				Guid guid = Guid.NewGuid();
				NumberEventArgument numberEventArgument = new NumberEventArgument(text2, guid, num, num2, isTcpEvent, GetSerial(num));
				List<Packet> list3 = GenerateNumberRequest(new List<NumberEventArgument> { numberEventArgument }, guid);
				if (!(numberEventArgument.CallerIdNumber.Trim() == "") && numberEventArgument.CallerIdNumber.Length >= 3)
				{
					retVal.AddRange(list3);
					connection.Context.WebModem.DataReceived(list3, connectionUniqueId, connection, isTcpEvent);
					flag = true;
				}
			}
			if (!(!flag && valueOrDefault))
			{
				return;
			}
			throw new Exception("event not fired! data:" + data);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateNumberEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\WebModem.cs", 204);
		}
	}

	private static void GenerateHookOnEvent(string data, ConnectionData connection, string connectionUniqueId, bool isTcpEvent, string path = null, string content = null)
	{
		try
		{
			List<string> list = RegexInfo.Calc(data, ModemRegex.RegexHookOn, content);
			if (list == null || list.Count <= 0)
			{
				return;
			}
			foreach (string item2 in list)
			{
				List<string> list2 = RegexInfo.CalcOtherEvent(item2, ModemRegex.RegexHookOn);
				short num = list2[0]?.ParseToShort(0) ?? 0;
				short num2 = list2[1]?.ParseToShort(0) ?? 0;
				if (num2 == 0)
				{
					throw new Exception($"Generate Hook On Event!!!! Line Number is:{num2}");
				}
				if (num == 0)
				{
					throw new Exception($"Generate Hook On Event!!!! Device Number is:{num}");
				}
				Guid guid = Guid.NewGuid();
				HookOnEventArgument item = new HookOnEventArgument(guid, num, num2, isTcpEvent, GetSerial(num));
				List<Packet> list3 = GenerateHookOnRequest(new List<HookOnEventArgument> { item }, guid);
				retVal.AddRange(list3);
				connection.Context.WebModem.DataReceived(list3, connectionUniqueId, connection, isTcpEvent);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateHookOnEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\WebModem.cs", 230);
		}
	}

	private static void GenerateHookOffEvent(string data, ConnectionData connection, string connectionUniqueId, bool isTcpEvent, string path = null, string content = null)
	{
		try
		{
			List<string> list = RegexInfo.Calc(data, ModemRegex.RegexHookOff, content);
			if (list == null || list.Count <= 0)
			{
				return;
			}
			foreach (string item2 in list)
			{
				List<string> list2 = RegexInfo.CalcOtherEvent(item2, ModemRegex.RegexHookOff);
				short num = list2[0]?.ParseToShort(0) ?? 0;
				short num2 = list2[1]?.ParseToShort(0) ?? 0;
				if (num2 == 0)
				{
					throw new Exception($"Generate Hook Off Event!!!! Line Number is:{num2}");
				}
				if (num == 0)
				{
					throw new Exception($"Generate Hook Off Event!!!! Device Number is:{num}");
				}
				Guid guid = Guid.NewGuid();
				HookOffEventArgument item = new HookOffEventArgument(guid, num, num2, isTcpEvent, GetSerial(num));
				List<Packet> list3 = GenerateHookOffRequest(new List<HookOffEventArgument> { item }, guid);
				retVal.AddRange(list3);
				connection.Context.WebModem.DataReceived(list3, connectionUniqueId, connection, isTcpEvent);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateHookOffEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\WebModem.cs", 258);
		}
	}

	private static void GenerateDeviceConnectEvent(string data, ConnectionData connection, string connectionUniqueId, bool isTcpEvent, string path = null, string content = null)
	{
		try
		{
			List<string> list = RegexInfo.Calc(data, ModemRegex.RegexDeviceConnect, content);
			if (list == null || list.Count <= 0)
			{
				return;
			}
			foreach (string item in list)
			{
				List<string> list2 = RegexInfo.CalcOtherEvent(item, ModemRegex.RegexDeviceConnect);
				short deviceNumber = list2[0]?.ParseToShort(0) ?? 0;
				Guid guid = Guid.NewGuid();
				OtherEventArgumant otherEventArgumant = new OtherEventArgumant(guid, deviceNumber, list2[1]?.ParseToShort(0) ?? 0, list2[2] ?? "", list2[3] ?? "", isTcpEvent, GetSerial(deviceNumber));
				List<Packet> list3 = GenerateDeviceConnectRequest(new List<OtherEventArgumant> { otherEventArgumant }, guid);
				if (otherEventArgumant.Action != null)
				{
					retVal.AddRange(list3);
					connection.Context.WebModem.DataReceived(list3, connectionUniqueId, connection, isTcpEvent);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateDeviceConnectEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\WebModem.cs", 284);
		}
	}

	private static void GenerateDeviceDisConnectEvent(string data, ConnectionData connection, string connectionUniqueId, bool isTcpEvent, string path = null, string content = null)
	{
		try
		{
			List<string> list = RegexInfo.Calc(data, ModemRegex.RegexDeviceDisconnect, content);
			if (list == null || list.Count <= 0)
			{
				return;
			}
			foreach (string item in list)
			{
				List<string> list2 = RegexInfo.CalcOtherEvent(item, ModemRegex.RegexDeviceDisconnect);
				short deviceNumber = list2[0]?.ParseToShort(0) ?? 0;
				Guid guid = Guid.NewGuid();
				OtherEventArgumant otherEventArgumant = new OtherEventArgumant(guid, deviceNumber, list2[1]?.ParseToShort(0) ?? 0, list2[2] ?? "", list2[3] ?? "", isTcpEvent, GetSerial(list2[0]?.ParseToShort(0) ?? 0));
				List<Packet> list3 = GenerateDeviceDisConnectRequest(new List<OtherEventArgumant> { otherEventArgumant }, guid);
				if (otherEventArgumant.Action != null)
				{
					retVal.AddRange(list3);
					connection.Context.WebModem.DataReceived(list3, connectionUniqueId, connection, isTcpEvent);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateDeviceDisConnectEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\WebModem.cs", 308);
		}
	}

	private static void GenerateMissedCallEvent(string data, ConnectionData connection, string connectionUniqueId, bool isTcpEvent, string path = null, string content = null)
	{
		try
		{
			List<string> list = RegexInfo.Calc(data, ModemRegex.RegexMissCall, content);
			if (list == null || list.Count <= 0)
			{
				return;
			}
			foreach (string item2 in list)
			{
				List<string> list2 = RegexInfo.CalcOtherEvent(item2, ModemRegex.RegexMissCall);
				short num = list2[0]?.ParseToShort(0) ?? 0;
				short num2 = list2[1]?.ParseToShort(0) ?? 0;
				if (num2 == 0)
				{
					throw new Exception($"Generate Missed Call Event!!!! Line Number is:{num2}");
				}
				if (num == 0)
				{
					throw new Exception($"Generate Missed Call Event!!!! Device Number is:{num}");
				}
				Guid guid = Guid.NewGuid();
				MissedCallEventArgument item = new MissedCallEventArgument(guid, num, num2, list2[2] ?? "", isTcpEvent, GetSerial(num));
				List<Packet> list3 = GenerateMissedCallRequest(new List<MissedCallEventArgument> { item }, guid);
				retVal.AddRange(list3);
				connection.Context.WebModem.DataReceived(list3, connectionUniqueId, connection, isTcpEvent);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateMissedCallEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\WebModem.cs", 334);
		}
	}

	private static void GenerateAnswerEvent(string data, ConnectionData connection, string connectionUniqueId, bool isTcpEvent, string path = null, string content = null)
	{
		try
		{
			List<string> list = RegexInfo.Calc(data, ModemRegex.RegexAnswer, content);
			if (list == null || list.Count <= 0)
			{
				return;
			}
			foreach (string item2 in list)
			{
				List<string> list2 = RegexInfo.CalcOtherEvent(item2, ModemRegex.RegexAnswer);
				short num = list2[0]?.ParseToShort(0) ?? 0;
				short num2 = list2[1]?.ParseToShort(0) ?? 0;
				if (num2 == 0)
				{
					throw new Exception($"Generate Answer Event!!!! Line Number is:{num2}");
				}
				if (num == 0)
				{
					throw new Exception($"Generate Answer Event!!!! Device Number is:{num}");
				}
				Guid guid = Guid.NewGuid();
				AnswerEventArgument item = new AnswerEventArgument(guid, num, num2, isTcpEvent, GetSerial(num));
				List<Packet> list3 = GenerateAnswerRequest(new List<AnswerEventArgument> { item }, guid);
				retVal.AddRange(list3);
				connection.Context.WebModem.DataReceived(list3, connectionUniqueId, connection, isTcpEvent);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateAnswerEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\WebModem.cs", 360);
		}
	}

	private static void GenerateLineDisconnectEvent(string data, ConnectionData connection, string connectionUniqueId, bool isTcpEvent, string path = null, string content = null)
	{
		try
		{
			List<string> list = RegexInfo.Calc(data, ModemRegex.RegexLineDisconnect, content);
			if (list == null || list.Count <= 0)
			{
				return;
			}
			foreach (string item2 in list)
			{
				List<string> list2 = RegexInfo.CalcOtherEvent(item2, ModemRegex.RegexLineDisconnect);
				short deviceNumber = list2[0]?.ParseToShort(0) ?? 0;
				short lineNumber = list2[1]?.ParseToShort(0) ?? 0;
				Guid guid = Guid.NewGuid();
				LineDisconnectEventArgs item = new LineDisconnectEventArgs(guid, deviceNumber, lineNumber, isTcpEvent, GetSerial(deviceNumber));
				List<Packet> list3 = GenerateLineDisconnectRequest(new List<LineDisconnectEventArgs> { item }, guid);
				retVal.AddRange(list3);
				connection.Context.WebModem.DataReceived(list3, connectionUniqueId, connection, isTcpEvent);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateLineDisconnectEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\WebModem.cs", 384);
		}
	}

	public static List<Packet> GenerateDialedResponse(IEnumerable<DialedEventArgumen> dialed, Guid guid)
	{
		using (new JaegerMonitor("GenerateDialedResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\WebModem.cs"))
		{
			try
			{
				return dialed?.GeneratePackets(PacketType.Modem, 1, new string[1] { guid.ToString() }, isSync: false, null, 0, deliveryRequst: true, "GenerateDialedResponse");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateDialedResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\WebModem.cs", 397);
				throw;
			}
		}
	}

	private static List<Packet> GenerateDialedFromServerRequest(IEnumerable<DialedEventArgumen> dialed, Guid guid)
	{
		using (new JaegerMonitor("GenerateDialedFromServerRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\WebModem.cs"))
		{
			try
			{
				return dialed?.GeneratePackets(PacketType.Modem, 1, new string[1] { guid.ToString() }, isSync: false, null, 0, deliveryRequst: true, "GenerateDialedFromServerRequest");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateDialedFromServerRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\WebModem.cs", 411);
				throw;
			}
		}
	}

	private static List<Packet> GenerateRingRequest(IEnumerable<RingEventArgument> ring, Guid guid)
	{
		using (new JaegerMonitor("GenerateRingRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\WebModem.cs"))
		{
			try
			{
				return ring?.GeneratePackets(PacketType.Modem, 7, new string[1] { guid.ToString() }, isSync: false, null, 0, deliveryRequst: true, "GenerateRingRequest");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateRingRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\WebModem.cs", 425);
				throw;
			}
		}
	}

	private static List<Packet> GenerateNumberRequest(IEnumerable<NumberEventArgument> number, Guid guid)
	{
		using (new JaegerMonitor("GenerateNumberRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\WebModem.cs"))
		{
			try
			{
				return number?.GeneratePackets(PacketType.Modem, 8, new string[1] { guid.ToString() }, isSync: false, null, 0, deliveryRequst: true, "GenerateNumberRequest");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateNumberRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\WebModem.cs", 439);
				return null;
			}
		}
	}

	private static List<Packet> GenerateHookOnRequest(IEnumerable<HookOnEventArgument> hookOn, Guid guid)
	{
		using (new JaegerMonitor("GenerateHookOnRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\WebModem.cs"))
		{
			try
			{
				return hookOn?.GeneratePackets(PacketType.Modem, 2, new string[1] { guid.ToString() }, isSync: false, null, 0, deliveryRequst: true, "GenerateHookOnRequest");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateHookOnRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\WebModem.cs", 453);
				throw;
			}
		}
	}

	private static List<Packet> GenerateHookOffRequest(IEnumerable<HookOffEventArgument> hookOff, Guid guid)
	{
		using (new JaegerMonitor("GenerateHookOffRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\WebModem.cs"))
		{
			try
			{
				return hookOff?.GeneratePackets(PacketType.Modem, 5, new string[1] { guid.ToString() }, isSync: false, null, 0, deliveryRequst: true, "GenerateHookOffRequest");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateHookOffRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\WebModem.cs", 467);
				throw;
			}
		}
	}

	private static List<Packet> GenerateDeviceConnectRequest(IEnumerable<OtherEventArgumant> connect, Guid guid)
	{
		using (new JaegerMonitor("GenerateDeviceConnectRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\WebModem.cs"))
		{
			try
			{
				return connect?.GeneratePackets(PacketType.Modem, 9, new string[1] { guid.ToString() }, isSync: false, null, 0, deliveryRequst: true, "GenerateDeviceConnectRequest");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateDeviceConnectRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\WebModem.cs", 481);
				throw;
			}
		}
	}

	private static List<Packet> GenerateDeviceDisConnectRequest(IEnumerable<OtherEventArgumant> disConnect, Guid guid)
	{
		using (new JaegerMonitor("GenerateDeviceDisConnectRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\WebModem.cs"))
		{
			try
			{
				return disConnect?.GeneratePackets(PacketType.Modem, 10, new string[1] { guid.ToString() }, isSync: false, null, 0, deliveryRequst: true, "GenerateDeviceDisConnectRequest");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateDeviceDisConnectRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\WebModem.cs", 496);
				throw;
			}
		}
	}

	private static List<Packet> GenerateMissedCallRequest(IEnumerable<MissedCallEventArgument> missedCall, Guid guid)
	{
		using (new JaegerMonitor("GenerateMissedCallRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\WebModem.cs"))
		{
			try
			{
				return missedCall?.GeneratePackets(PacketType.Modem, 6, new string[1] { guid.ToString() }, isSync: false, null, 0, deliveryRequst: true, "GenerateMissedCallRequest");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateMissedCallRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\WebModem.cs", 511);
				throw;
			}
		}
	}

	private static List<Packet> GenerateAnswerRequest(IEnumerable<AnswerEventArgument> answer, Guid guid)
	{
		using (new JaegerMonitor("GenerateAnswerRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\WebModem.cs"))
		{
			try
			{
				return answer?.GeneratePackets(PacketType.Modem, 11, new string[1] { guid.ToString() }, isSync: false, null, 0, deliveryRequst: true, "GenerateAnswerRequest");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateAnswerRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\WebModem.cs", 526);
				throw;
			}
		}
	}

	private static List<Packet> GenerateLineDisconnectRequest(IEnumerable<LineDisconnectEventArgs> lineDisconnect, Guid guid)
	{
		using (new JaegerMonitor("GenerateLineDisconnectRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\WebModem.cs"))
		{
			try
			{
				return lineDisconnect?.GeneratePackets(PacketType.Modem, 12, new string[1] { guid.ToString() }, isSync: false, null, 0, deliveryRequst: true, "GenerateLineDisconnectRequest");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateLineDisconnectRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\WebModem.cs", 541);
				throw;
			}
		}
	}

	private static List<Packet> GenerateIncomingCallRequest(IEnumerable<IncomingCallEventArgument> incCall, Guid guid)
	{
		using (new JaegerMonitor("GenerateIncomingCallRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\WebModem.cs"))
		{
			try
			{
				return incCall?.GeneratePackets(PacketType.Modem, 13, new string[1] { guid.ToString() }, isSync: false, null, 0, deliveryRequst: true, "GenerateIncomingCallRequest");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateIncomingCallRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\WebModem.cs", 555);
				throw;
			}
		}
	}

	private static List<Packet> GenerateOutgoingCallRequest(IEnumerable<OutgoingCallEventArgument> outCall, Guid guid)
	{
		using (new JaegerMonitor("GenerateOutgoingCallRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\WebModem.cs"))
		{
			try
			{
				return outCall?.GeneratePackets(PacketType.Modem, 14, new string[1] { guid.ToString() }, isSync: false, null, 0, deliveryRequst: true, "GenerateOutgoingCallRequest");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateOutgoingCallRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\WebModem.cs", 569);
				throw;
			}
		}
	}

	public static void SetSerial(short deviceNumber, string serialNumber)
	{
		if (string.IsNullOrEmpty(serialNumber))
		{
			throw new ArgumentException("serialNumber");
		}
		if (deviceNumber <= 0)
		{
			throw new ArgumentException("deviceNumber");
		}
		try
		{
			DeviceNumberToserial.TryRemove(deviceNumber, out var _);
			DeviceNumberToserial.TryAdd(deviceNumber, serialNumber);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetSerial", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\WebModem.cs", 587);
		}
	}

	private static string GetSerial(short deviceNumber)
	{
		try
		{
			string value;
			return DeviceNumberToserial.TryGetValue(deviceNumber, out value) ? value : "";
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetSerial", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\WebModem.cs", 611);
			return "";
		}
	}

	public async Task DataReceived(List<Packet> packs, string connectionUniqueId, ConnectionData connection, bool isTcp)
	{
		try
		{
			if (IsDisposed)
			{
				throw new ObjectDisposedException(GetType().Name);
			}
			if (packs == null)
			{
				return;
			}
			foreach (Packet pack in packs)
			{
				if (pack != null)
				{
					await DataReceived(pack, connectionUniqueId, connection, isTcp);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\WebModem.cs", 642);
		}
	}

	public async Task DataReceived(Packet pack, string connectionUniqueId, ConnectionData connection, bool isTcp)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\WebModem.cs"))
		{
			_ = 9;
			try
			{
				if (pack == null || _refrences.Contains(pack.PacketReferenceId))
				{
					return;
				}
				_refrences.Add(pack.PacketReferenceId);
				switch ((ModemActions)pack.PacketActions)
				{
				case ModemActions.HookOff:
					if (this.HookOffEvent != null)
					{
						ConnectionEventArgs<HookOffEventArgument> connectionEventArgs8 = new ConnectionEventArgs<HookOffEventArgument>(pack, connectionUniqueId, connection);
						connectionEventArgs8.IsTcpEvent = isTcp;
						await Utilities.RaiseEvent(this.HookOffEvent, this, connectionEventArgs8, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\WebModem.cs");
					}
					break;
				case ModemActions.Ring:
					if (this.RingEvent != null)
					{
						ConnectionEventArgs<RingEventArgument> connectionEventArgs4 = new ConnectionEventArgs<RingEventArgument>(pack, connectionUniqueId, connection);
						connectionEventArgs4.IsTcpEvent = isTcp;
						await Utilities.RaiseEvent(this.RingEvent, this, connectionEventArgs4, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\WebModem.cs");
					}
					break;
				case ModemActions.CallerIdNumber:
				{
					if (this.NumberEvent == null)
					{
						throw new Exception("Number handler is null!!");
					}
					ConnectionEventArgs<NumberEventArgument> connectionEventArgs10 = new ConnectionEventArgs<NumberEventArgument>(pack, connectionUniqueId, connection);
					connectionEventArgs10.IsTcpEvent = isTcp;
					await Utilities.RaiseEvent(this.NumberEvent, this, connectionEventArgs10, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\WebModem.cs");
					break;
				}
				case ModemActions.HookOn:
					if (this.HookOnEvent != null)
					{
						ConnectionEventArgs<HookOnEventArgument> connectionEventArgs6 = new ConnectionEventArgs<HookOnEventArgument>(pack, connectionUniqueId, connection);
						connectionEventArgs6.IsTcpEvent = isTcp;
						await Utilities.RaiseEvent(this.HookOnEvent, this, connectionEventArgs6, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\WebModem.cs");
					}
					break;
				case ModemActions.DeviceConnect:
					if (this.DeviceConnecEvent != null)
					{
						ConnectionEventArgs<OtherEventArgumant> connectionEventArgs2 = new ConnectionEventArgs<OtherEventArgumant>(pack, connectionUniqueId, connection);
						connectionEventArgs2.IsTcpEvent = isTcp;
						await Utilities.RaiseEvent(this.DeviceConnecEvent, this, connectionEventArgs2, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\WebModem.cs");
					}
					break;
				case ModemActions.DeviceDisConnect:
					if (this.DeviceDisconnectEvent != null)
					{
						ConnectionEventArgs<OtherEventArgumant> connectionEventArgs9 = new ConnectionEventArgs<OtherEventArgumant>(pack, connectionUniqueId, connection);
						connectionEventArgs9.IsTcpEvent = isTcp;
						await Utilities.RaiseEvent(this.DeviceDisconnectEvent, this, connectionEventArgs9, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\WebModem.cs");
					}
					break;
				case ModemActions.LineDisconnect:
					if (this.LineDisEvent != null)
					{
						ConnectionEventArgs<LineDisconnectEventArgs> connectionEventArgs7 = new ConnectionEventArgs<LineDisconnectEventArgs>(pack, connectionUniqueId, connection);
						connectionEventArgs7.IsTcpEvent = isTcp;
						await Utilities.RaiseEvent(this.LineDisEvent, this, connectionEventArgs7, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\WebModem.cs");
					}
					break;
				case ModemActions.MissedCall:
					if (this.MissEvent != null)
					{
						ConnectionEventArgs<MissedCallEventArgument> connectionEventArgs5 = new ConnectionEventArgs<MissedCallEventArgument>(pack, connectionUniqueId, connection);
						connectionEventArgs5.IsTcpEvent = isTcp;
						await Utilities.RaiseEvent(this.MissEvent, this, connectionEventArgs5, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\WebModem.cs");
					}
					break;
				case ModemActions.Answer:
					if (this.AnswerEvent != null)
					{
						ConnectionEventArgs<AnswerEventArgument> connectionEventArgs3 = new ConnectionEventArgs<AnswerEventArgument>(pack, connectionUniqueId, connection);
						connectionEventArgs3.IsTcpEvent = isTcp;
						await Utilities.RaiseEvent(this.AnswerEvent, this, connectionEventArgs3, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\WebModem.cs");
					}
					break;
				case ModemActions.Dialed:
					if (this.DialedEvent != null)
					{
						ConnectionEventArgs<DialedEventArgumen> connectionEventArgs = new ConnectionEventArgs<DialedEventArgumen>(pack, connectionUniqueId, connection);
						connectionEventArgs.IsTcpEvent = isTcp;
						await Utilities.RaiseEvent(this.DialedEvent, this, connectionEventArgs, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\WebModem.cs");
					}
					break;
				default:
					WebErrorLog.Instance.StartLog($"WebModem Packet with type:{pack.PacketActions} not handled", "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\WebModem.cs", 719);
					break;
				}
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\WebModem.cs", 724);
			}
		}
	}

	public void Dispose()
	{
		try
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
				if (this.AnswerEvent != null)
				{
					this.AnswerEvent = null;
				}
				if (this.AnswerEvent != null)
				{
					this.AnswerEvent = null;
				}
				if (this.HookOffEvent != null)
				{
					this.HookOffEvent = null;
				}
				if (this.MissEvent != null)
				{
					this.MissEvent = null;
				}
				if (this.NumberEvent != null)
				{
					this.NumberEvent = null;
				}
				if (this.DeviceConnecEvent != null)
				{
					this.DeviceConnecEvent = null;
				}
				if (this.AnswerEvent != null)
				{
					this.AnswerEvent = null;
				}
				if (this.RingEvent != null)
				{
					this.RingEvent = null;
				}
				if (this.LineDisEvent != null)
				{
					this.LineDisEvent = null;
				}
				if (this.NumberEvent != null)
				{
					this.NumberEvent = null;
				}
				DeviceNumberToserial = null;
				_refrences = null;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\WebModem.cs", 747);
		}
	}

	public Task DataReceived(Packet pack, string ConnectionUniqueId, ConnectionData Connection)
	{
		throw new NotImplementedException();
	}

	public string GetPacketActionEnumName(short action)
	{
		try
		{
			ModemActions modemActions = (ModemActions)action;
			return modemActions.ToString();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPacketActionEnumName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\WebModem.cs", 763);
			return $"Error({action})";
		}
	}

	public string GetPacketActionName(short action)
	{
		try
		{
			return ((ModemActions)action).GetDisplay();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPacketActionName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\WebModem.cs", 776);
			return $"Error({action})";
		}
	}
}
