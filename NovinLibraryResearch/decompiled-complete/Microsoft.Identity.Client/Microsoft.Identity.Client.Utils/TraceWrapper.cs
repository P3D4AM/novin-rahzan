#define TRACE
using System.Diagnostics;

namespace Microsoft.Identity.Client.Utils;

internal class TraceWrapper
{
	public static void WriteLine(string message)
	{
		Trace.WriteLine(message);
	}
}
