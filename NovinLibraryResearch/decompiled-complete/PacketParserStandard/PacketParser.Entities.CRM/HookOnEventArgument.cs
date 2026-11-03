using System;

namespace PacketParser.Entities.CRM;

public class HookOnEventArgument : Serializable<HookOnEventArgument>
{
	public Guid Guid { get; set; }

	public DateTime DateM { get; set; }

	public short LineNumber { get; set; }

	public short DeviceNumber { get; set; }

	public string DeviceSerial { get; set; }

	public bool IsTcpEvent { get; set; }

	public HookOnEventArgument()
	{
	}

	public HookOnEventArgument(Guid eventGuid, short deviceNumber, short lineNumber, bool isTcpEvent, string deviceSerial)
	{
		DateM = DateTime.Now;
		try
		{
			Guid = eventGuid;
			DeviceNumber = deviceNumber;
			LineNumber = lineNumber;
			IsTcpEvent = isTcpEvent;
			DeviceSerial = deviceSerial;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\HookOnEventArgument.cs", 30);
		}
	}
}
