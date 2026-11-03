using System;

namespace PacketParser.Entities.CRM;

public class LineDisconnectEventArgs : Serializable<LineDisconnectEventArgs>
{
	public Guid Guid { get; set; }

	public DateTime DateM { get; }

	public short LineNumber { get; }

	public short DeviceNumber { get; }

	public string DeviceSerial { get; set; }

	public bool IsTcpEvent { get; set; }

	public LineDisconnectEventArgs()
	{
	}

	public LineDisconnectEventArgs(Guid eventGuid, short deviceNumber, short lineNumber, bool isTcpEvent, string deviceSerial)
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
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\LineDisconnectEventArgs.cs", 28);
		}
	}
}
