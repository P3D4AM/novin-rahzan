using System;

namespace PacketParser.Entities.CRM.NewEvents;

public class OutgoingCallEventArgument : Serializable<OutgoingCallEventArgument>
{
	public Guid Guid { get; set; }

	public DateTime DateM { get; set; }

	public short CoNumber { get; set; }

	public short LineNumber { get; set; }

	public string Number { get; set; }

	public int DurationSeconds { get; set; }

	public OutgoingCallEventArgument()
	{
	}

	public OutgoingCallEventArgument(Guid eventGuid, DateTime dateM, short coNumber, short lineNumber, string number, int durationSeconds)
	{
		try
		{
			Guid = eventGuid;
			DateM = dateM;
			CoNumber = coNumber;
			LineNumber = lineNumber;
			Number = number;
			DurationSeconds = durationSeconds;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\NewEvents\\OutgoingCallEventArgument.cs", 27);
		}
	}
}
