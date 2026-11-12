namespace mCore;

public enum USSDStatus
{
	NoFurtherUserActionRequired,
	FurtherUserActionRequired,
	USSDTerminatedByNetwork,
	OtherLocalClientHasResponded,
	OperationNotSupported,
	NetworkTimeout
}
