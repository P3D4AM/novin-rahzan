namespace Microsoft.SqlServer.Management.Smo.Broker;

public enum DialogEndPointState
{
	Open = 1,
	OpenWait = 2,
	Disabled = 6,
	ClosingWait = 7,
	ErrorWait = 8,
	ClosedWait = 9
}
