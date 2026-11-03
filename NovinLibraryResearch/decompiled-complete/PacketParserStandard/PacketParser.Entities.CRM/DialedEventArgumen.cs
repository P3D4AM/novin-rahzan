using System;

namespace PacketParser.Entities.CRM;

public class DialedEventArgumen : Serializable<DialedEventArgumen>
{
	public Guid Guid { get; set; }

	public short LineNumber { get; set; }

	public short DeviceNumber { get; set; }

	public string DeviceSerial { get; set; }

	public string Number { get; set; }

	public bool IsTcpEvent { get; set; }

	public short Duration { get; set; }

	public DateTime StartDateM { get; set; }

	public DateTime EndDateM { get; set; }

	public DialedEventArgumen()
	{
	}

	public DialedEventArgumen(Guid guid, short d1, short l1, string data, DateTime date1, DateTime date2)
	{
		try
		{
			LineNumber = l1;
			DeviceNumber = d1;
			Number = data;
			StartDateM = date1;
			EndDateM = date2;
			Duration = checked((short)date2.Subtract(date1).Seconds);
			Guid = guid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\DialedEventArgumen.cs", 36);
		}
	}

	public DialedEventArgumen(Guid eventGuid, short deviceNumber, short lineNumber, short duration, string number, bool isTcpEvent, string deviceSerial)
	{
		try
		{
			Guid = eventGuid;
			DeviceNumber = deviceNumber;
			LineNumber = lineNumber;
			Duration = duration;
			Number = number;
			IsTcpEvent = isTcpEvent;
			DeviceSerial = deviceSerial;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\DialedEventArgumen.cs", 52);
		}
	}
}
