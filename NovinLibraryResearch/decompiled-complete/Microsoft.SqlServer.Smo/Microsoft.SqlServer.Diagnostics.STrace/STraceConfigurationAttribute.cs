using System;

namespace Microsoft.SqlServer.Diagnostics.STrace;

internal class STraceConfigurationAttribute : Attribute
{
	public bool SkipAutoTrace { get; set; }
}
