namespace PacketParser.Entities;

public enum EnAsteriskEvents : short
{
	None,
	Hangup,
	NewExten,
	QueueCallerJoin,
	QueueCallerLeave,
	QueueCallerAbandon
}
