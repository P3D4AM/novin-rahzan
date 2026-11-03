using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PacketParser.Entities.CRM;

public class NumberDialed : Serializable<NumberDialed>
{
	private class NestedNumberDialed
	{
		internal static readonly NumberDialed instance;

		static NestedNumberDialed()
		{
			instance = new NumberDialed();
		}
	}

	public ConcurrentDictionary<string, NumberDialedDet> LstDial = new ConcurrentDictionary<string, NumberDialedDet>();

	public Thread Thread;

	private object _lock = new object();

	public static NumberDialed CallerIdInstance => NestedNumberDialed.instance;

	public void Load()
	{
		try
		{
			LstDial.Clear();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Load", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\NumberDialed.cs", 20);
		}
	}

	public List<Packet> Dialed(Guid eventGuid, string serialNumber, short deviceNumber, short lineNumber, short dialedNumber, string connectionUniqueId, ConnectionData connection, bool isTcp, DateTime date)
	{
		try
		{
			ModemDeviceKey modemDeviceKey = new ModemDeviceKey(deviceNumber, lineNumber, serialNumber);
			LstDial.TryGetValue(modemDeviceKey.toString(), out var value);
			if (value == null)
			{
				value = new NumberDialedDet
				{
					DeviceNumber = deviceNumber,
					LineNumber = lineNumber,
					DialedNumber = new List<Tuple<short, DateTime>>
					{
						new Tuple<short, DateTime>(dialedNumber, date)
					},
					DateM = DateTime.Now,
					swIsActive = true,
					serialNumber = serialNumber,
					isTcp = isTcp,
					connection = connection,
					ConnectionUniqueId = connectionUniqueId,
					Guid = eventGuid
				};
				value.OnNumber -= BufferedNumberDialed_OnNumber;
				value.OnNumber += BufferedNumberDialed_OnNumber;
				LstDial.TryAdd(modemDeviceKey.toString(), value);
				value.ResetToken();
			}
			else
			{
				value.DialedNumber.Add(new Tuple<short, DateTime>(dialedNumber, date));
				value.DateM = DateTime.Now;
				value.ResetToken();
			}
			if (new WebTafsilNumbers(value.FullDialedNumber).IsValid)
			{
				return Pop(this, value.DeviceNumber, value.LineNumber, value.serialNumber, eventGuid);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dialed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\NumberDialed.cs", 63);
		}
		return null;
	}

	private Task BufferedNumberDialed_OnNumber(object sender, NumberDialedDet e)
	{
		return e.connection.Context.WebModem.DataReceived(Pop(e, e.DeviceNumber, e.LineNumber, e.serialNumber, e.Guid), e.ConnectionUniqueId, e.connection, e.isTcp);
	}

	public List<Packet> Pop(object sender, short deviceNumber, short lineNumber, string serialNumber, Guid eventGuid)
	{
		try
		{
			ModemDeviceKey modemDeviceKey = new ModemDeviceKey(deviceNumber, lineNumber, serialNumber);
			LstDial.TryRemove(modemDeviceKey.toString(), out var value);
			if (value != null)
			{
				value.OnNumber -= BufferedNumberDialed_OnNumber;
				if (value.FullDialedNumber.Length >= 3)
				{
					DialedEventArgumen item = new DialedEventArgumen(eventGuid, deviceNumber, lineNumber, value.FullDialedNumber, value.DateM, DateTime.Now);
					List<Packet> result = WebModem.GenerateDialedResponse(new List<DialedEventArgumen> { item }, eventGuid);
					value.swIsActive = false;
					return result;
				}
				value.swIsActive = false;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Pop", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\NumberDialed.cs", 95);
		}
		return null;
	}

	private void SetEndDialed(object sender, short deviceNumber, short lineNumber, string serialNumber, Guid aggregateGuid)
	{
		try
		{
			ModemDeviceKey modemDeviceKey = new ModemDeviceKey(deviceNumber, lineNumber, serialNumber);
			LstDial.TryGetValue(modemDeviceKey.toString(), out var value);
			if (value != null)
			{
				Pop(sender, deviceNumber, lineNumber, serialNumber, aggregateGuid);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetEndDialed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\NumberDialed.cs", 112);
		}
	}

	public void EndDialed(object sender, short deviceNumber, short lineNumber, string serialNumber, Guid aggregateGuid)
	{
		SetEndDialed(sender, deviceNumber, lineNumber, serialNumber, aggregateGuid);
	}
}
