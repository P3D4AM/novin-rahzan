using System;
using System.Diagnostics;

namespace Microsoft.Identity.Json.Serialization;

internal interface ITraceWriter
{
	TraceLevel LevelFilter { get; }

	void Trace(TraceLevel level, string message, Exception ex);
}
