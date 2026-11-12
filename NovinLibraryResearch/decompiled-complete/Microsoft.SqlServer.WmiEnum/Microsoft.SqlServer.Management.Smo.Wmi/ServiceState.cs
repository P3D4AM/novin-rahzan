namespace Microsoft.SqlServer.Management.Smo.Wmi;

public enum ServiceState
{
	Stopped = 1,
	StartPending,
	StopPending,
	Running,
	ContinuePending,
	PausePending,
	Paused,
	Unknown
}
