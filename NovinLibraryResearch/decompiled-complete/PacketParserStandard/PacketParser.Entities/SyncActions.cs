namespace PacketParser.Entities;

internal enum SyncActions
{
	Unknown,
	StateChange,
	Done,
	Reset,
	ClientSynced,
	ResetDone,
	SyncDoneResponse,
	DeleteDone
}
