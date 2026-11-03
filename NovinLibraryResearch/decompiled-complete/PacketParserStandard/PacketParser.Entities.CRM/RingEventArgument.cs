using System;

namespace PacketParser.Entities.CRM;

public class RingEventArgument : Serializable<RingEventArgument>
{
	public Guid Guid { get; set; }

	public DateTime DateM { get; set; }

	public short RingNo { get; set; }

	public short LineNumber { get; set; }

	public short DeviceNumber { get; set; }

	public string DeviceSerial { get; set; }

	public bool IsTcpEvent { get; set; }

	public RingEventArgument()
	{
	}

	public RingEventArgument(Guid eventGuid, short deviceNumber, short lineNumber, short ringNo, bool isTcpEvent, string deviceSeial)
	{
		try
		{
			Guid = eventGuid;
			DeviceNumber = deviceNumber;
			LineNumber = lineNumber;
			RingNo = ringNo;
			IsTcpEvent = isTcpEvent;
			DeviceSerial = deviceSeial;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\RingEventArgument.cs", 31);
		}
	}
}
