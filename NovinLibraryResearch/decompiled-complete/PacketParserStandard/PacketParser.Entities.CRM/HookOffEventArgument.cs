using System;

namespace PacketParser.Entities.CRM;

public class HookOffEventArgument : Serializable<HookOffEventArgument>
{
	public Guid Guid { get; set; }

	public short LineNumber { get; set; }

	public DateTime DateM { get; set; }

	public short DeviceNumber { get; set; }

	public string DeviceSerial { get; set; }

	public bool IsTcpEvent { get; set; }

	public HookOffEventArgument()
	{
	}

	public HookOffEventArgument(Guid eventGuid, short deviceNumber, short lineNumber, bool isTcpEvent, string deviceSerial)
	{
		try
		{
			Guid = eventGuid;
			DateM = DateTime.Now;
			DeviceNumber = deviceNumber;
			LineNumber = lineNumber;
			IsTcpEvent = isTcpEvent;
			DeviceSerial = deviceSerial;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\HookOffEventArgument.cs", 28);
		}
	}
}
