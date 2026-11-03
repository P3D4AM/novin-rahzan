using System;

namespace PacketParser.Entities.CRM;

public class AnswerEventArgument : Serializable<AnswerEventArgument>
{
	public Guid Guid { get; set; }

	public DateTime DateM { get; set; }

	public short LineNumber { get; set; }

	public short DeviceNumber { get; set; }

	public string DeviceSerial { get; set; }

	public bool IsTcpEvent { get; set; }

	public AnswerEventArgument()
	{
	}

	public AnswerEventArgument(Guid eventGuid, short deviceNumber, short lineNumber, bool isTcpEvent, string deviceSeial)
	{
		try
		{
			Guid = eventGuid;
			DeviceNumber = deviceNumber;
			LineNumber = lineNumber;
			IsTcpEvent = isTcpEvent;
			DeviceSerial = deviceSeial;
			DateM = DateTime.Now;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\AnswerEventArgument.cs", 27);
		}
	}
}
