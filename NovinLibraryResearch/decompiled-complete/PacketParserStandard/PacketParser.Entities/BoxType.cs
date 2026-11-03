namespace PacketParser.Entities;

public enum BoxType : short
{
	None = 0,
	InBox = 1,
	OutBox = 2,
	SentItems = 3,
	Drafts = 4,
	Sim_Inbox = 10,
	Sim_OutBox = 20,
	Sim_SentItems = 30
}
