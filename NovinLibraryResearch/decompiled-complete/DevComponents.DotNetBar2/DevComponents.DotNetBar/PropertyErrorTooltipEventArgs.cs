using System;

namespace DevComponents.DotNetBar;

public class PropertyErrorTooltipEventArgs : EventArgs
{
	public SuperTooltipInfo TooltipInfo;

	public readonly string PropertyName;

	public readonly Exception Exception;

	public readonly object Value;

	public PropertyErrorTooltipEventArgs(SuperTooltipInfo tooltipInfo, string propertyName, Exception exception, object value)
	{
		TooltipInfo = tooltipInfo;
		PropertyName = propertyName;
		Exception = exception;
		Value = value;
	}
}
