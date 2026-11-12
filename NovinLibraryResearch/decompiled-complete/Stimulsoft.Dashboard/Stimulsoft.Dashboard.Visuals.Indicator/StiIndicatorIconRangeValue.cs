using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Dashboard.Visuals.Indicator;

internal class StiIndicatorIconRangeValue
{
	public decimal Start { get; }

	public decimal End { get; }

	public StiFontIcons Icon { get; }

	public bool IsBetween(decimal value)
	{
		if (Start <= value)
		{
			return value <= End;
		}
		return false;
	}

	public StiIndicatorIconRangeValue(decimal start, decimal end, StiFontIcons icon)
	{
		Start = start;
		End = end;
		Icon = icon;
	}
}
