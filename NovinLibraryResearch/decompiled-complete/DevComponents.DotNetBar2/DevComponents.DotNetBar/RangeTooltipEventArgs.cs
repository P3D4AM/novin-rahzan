using System;

namespace DevComponents.DotNetBar;

public class RangeTooltipEventArgs : EventArgs
{
	public string Tooltip;

	public RangeTooltipEventArgs(string tooltip)
	{
		Tooltip = tooltip;
	}
}
