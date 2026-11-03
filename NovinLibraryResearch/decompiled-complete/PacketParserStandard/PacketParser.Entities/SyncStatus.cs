namespace PacketParser.Entities;

public enum SyncStatus : short
{
	SyncNotStarted,
	Syncing,
	SyncedOk,
	SyncFailed
}
