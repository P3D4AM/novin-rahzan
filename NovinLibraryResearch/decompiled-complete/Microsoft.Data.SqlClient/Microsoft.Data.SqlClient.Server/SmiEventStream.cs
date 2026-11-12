using System;
using Microsoft.Data.Common;

namespace Microsoft.Data.SqlClient.Server;

internal abstract class SmiEventStream : IDisposable
{
	internal abstract bool HasEvents { get; }

	internal abstract void Close(SmiEventSink sink);

	public virtual void Dispose()
	{
		ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
	}

	internal abstract void ProcessEvent(SmiEventSink sink);
}
