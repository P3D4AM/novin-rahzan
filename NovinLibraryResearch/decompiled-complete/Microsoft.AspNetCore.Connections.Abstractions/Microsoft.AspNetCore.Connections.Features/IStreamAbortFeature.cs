namespace Microsoft.AspNetCore.Connections.Features;

public interface IStreamAbortFeature
{
	void AbortRead(long errorCode, ConnectionAbortedException abortReason);

	void AbortWrite(long errorCode, ConnectionAbortedException abortReason);
}
