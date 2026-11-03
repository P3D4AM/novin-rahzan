using System;

namespace PacketParser.Entities.CRM;

public class MissedCallEventArgument : Serializable<MissedCallEventArgument>
{
	public Guid Guid { get; set; }

	public DateTime DateM { get; set; }

	public string Action { get; set; }

	public short LineNumber { get; set; }

	public short DeviceNumber { get; set; }

	public string DeviceSerial { get; set; }

	public bool IsTcpEvent { get; set; }

	public MissedCallEventArgument()
	{
	}

	public MissedCallEventArgument(Guid eventGuid, short deviceNumber, short lineNumber, string action, bool isTcpEvent, string deviceSerial)
	{
		try
		{
			Guid = eventGuid;
			DeviceNumber = deviceNumber;
			LineNumber = lineNumber;
			Action = action;
			IsTcpEvent = isTcpEvent;
			DeviceSerial = deviceSerial;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\MissedCallEventArgument.cs", 31);
		}
	}
}
