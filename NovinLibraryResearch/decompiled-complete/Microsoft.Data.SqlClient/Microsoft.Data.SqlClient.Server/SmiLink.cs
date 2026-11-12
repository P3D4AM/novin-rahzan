namespace Microsoft.Data.SqlClient.Server;

internal abstract class SmiLink
{
	internal const ulong InterfaceVersion = 210uL;

	internal abstract ulong NegotiateVersion(ulong requestedVersion);

	internal abstract object GetCurrentContext(SmiEventSink eventSink);
}
