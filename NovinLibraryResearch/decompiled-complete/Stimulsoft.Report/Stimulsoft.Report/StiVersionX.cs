using System;

namespace Stimulsoft.Report;

public sealed class StiVersionX
{
	private static bool isSvr;

	public static bool IsSvr
	{
		get
		{
			return isSvr;
		}
		set
		{
			bool flag = isSvr;
			isSvr = value;
			if (flag != isSvr)
			{
				InvokeCancel();
			}
		}
	}

	public static event EventHandler ChangeSvr;

	private static void InvokeCancel()
	{
		StiVersionX.ChangeSvr?.Invoke(null, EventArgs.Empty);
	}
}
