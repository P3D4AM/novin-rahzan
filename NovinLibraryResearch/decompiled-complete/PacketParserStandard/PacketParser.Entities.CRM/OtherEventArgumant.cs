using System;

namespace PacketParser.Entities.CRM;

public class OtherEventArgumant : Serializable<OtherEventArgumant>
{
	public Guid Guid { get; set; }

	public short LineCount { get; set; }

	public DateTime DateM { get; }

	public short DeviceNumber { get; set; }

	public string Action { get; set; }

	public string Msg { get; set; }

	public string DeviceSerial { get; set; }

	public bool IsTcpEvent { get; set; }

	public OtherEventArgumant()
	{
	}

	public OtherEventArgumant(Guid eventGuid, short deviceNumber, short lineCount, string action, string message, bool isTcpEvent, string deviceSerial)
	{
		DateM = DateTime.Now;
		try
		{
			Guid = eventGuid;
			DeviceNumber = deviceNumber;
			LineCount = lineCount;
			Action = action;
			Msg = message;
			IsTcpEvent = isTcpEvent;
			DeviceSerial = deviceSerial;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\OtherEventArgumant.cs", 31);
		}
	}
}
