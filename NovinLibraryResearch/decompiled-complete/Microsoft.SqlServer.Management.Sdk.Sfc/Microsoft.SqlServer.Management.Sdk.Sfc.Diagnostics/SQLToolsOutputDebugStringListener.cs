using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Microsoft.SqlServer.Management.Sdk.Sfc.Diagnostics;

internal class SQLToolsOutputDebugStringListener : TraceListener
{
	private string myName = "SQLToolsOutputDebugStringListener";

	public SQLToolsOutputDebugStringListener()
	{
	}

	public SQLToolsOutputDebugStringListener(string name)
		: base(name)
	{
		myName = name;
	}

	public override void Write(string message)
	{
		if (Debugger.IsLogging())
		{
			Debugger.Log(0, null, message);
		}
		else if (message != null)
		{
			OutputDebugString(message);
		}
		else
		{
			OutputDebugString(string.Empty);
		}
	}

	public override void WriteLine(string message)
	{
		Write(message + "\r\n");
	}

	[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
	internal static extern void OutputDebugString(string message);
}
