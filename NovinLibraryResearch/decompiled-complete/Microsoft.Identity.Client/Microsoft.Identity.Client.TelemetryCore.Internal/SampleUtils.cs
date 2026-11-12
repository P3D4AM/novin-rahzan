using System;

namespace Microsoft.Identity.Client.TelemetryCore.Internal;

internal static class SampleUtils
{
	public static bool ShouldEnableDevice(string uuid)
	{
		if (GetFirstCharValue(uuid, out var outValue))
		{
			return outValue < 4;
		}
		return true;
	}

	private static bool GetFirstCharValue(string uuid, out short outValue)
	{
		outValue = 0;
		if (string.IsNullOrEmpty(uuid))
		{
			return false;
		}
		char c = uuid[0];
		if (char.IsLetterOrDigit(c))
		{
			outValue = Convert.ToInt16(char.IsDigit(c) ? (c - 48) : (10 + (c - 97)));
			return true;
		}
		return false;
	}
}
