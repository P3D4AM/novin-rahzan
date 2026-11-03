using System;

namespace PacketParser.Entities.CRM.NewEvents;

public class IncomingCallEventArgument : Serializable<IncomingCallEventArgument>
{
	public Guid Guid { get; set; }

	public DateTime DateM { get; set; }

	public short LineNumber { get; set; }

	public string Number { get; set; }

	public int? DurationSeconds { get; set; }

	public IncomingCallEventArgument()
	{
	}

	public IncomingCallEventArgument(Guid eventGuid, DateTime dateM, short lineNumber, string number, int? durationSeconds)
	{
		try
		{
			Guid = eventGuid;
			DateM = dateM;
			LineNumber = lineNumber;
			Number = number;
			DurationSeconds = durationSeconds;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\CRM\\NewEvents\\IncomingCallEventArgument.cs", 25);
		}
	}
}
