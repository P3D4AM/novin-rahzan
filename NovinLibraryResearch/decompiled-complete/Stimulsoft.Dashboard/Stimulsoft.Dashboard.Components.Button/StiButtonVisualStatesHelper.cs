using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Dashboard.Styles;

namespace Stimulsoft.Dashboard.Components.Button;

public static class StiButtonVisualStatesHelper
{
	public static StiButtonVisualState GetDefault(StiButtonElement element)
	{
		StiControlElementStyle controlStyle = StiDashboardStyleHelper.GetControlStyle(element);
		StiButtonVisualState stiButtonVisualState = new StiButtonVisualState
		{
			Border = element.Border,
			Brush = element.Brush,
			Font = element.Font,
			IconSet = element.IconSet,
			IconBrush = element?.IconBrush,
			TextBrush = element.TextBrush
		};
		if (IsStyle(stiButtonVisualState.Brush) || IsDefault(stiButtonVisualState.Brush))
		{
			stiButtonVisualState.Brush = new StiSolidBrush(controlStyle.BackColor);
		}
		if (IsStyle(stiButtonVisualState.TextBrush) || IsDefault(stiButtonVisualState.TextBrush))
		{
			stiButtonVisualState.TextBrush = new StiSolidBrush(controlStyle.ForeColor);
		}
		if (IsStyle(stiButtonVisualState.IconBrush) || IsDefault(stiButtonVisualState.IconBrush))
		{
			stiButtonVisualState.IconBrush = new StiSolidBrush(controlStyle.GlyphColor);
		}
		return stiButtonVisualState;
	}

	public static StiButtonVisualState GetPressed(StiButtonElement element)
	{
		StiControlElementStyle controlStyle = StiDashboardStyleHelper.GetControlStyle(element);
		StiButtonVisualState @default = GetDefault(element);
		StiButtonVisualState stiButtonVisualState = element.VisualStates.Pressed.Clone() as StiButtonVisualState;
		stiButtonVisualState.Border = stiButtonVisualState.Border ?? @default.Border;
		stiButtonVisualState.Font = stiButtonVisualState.Font ?? @default.Font;
		stiButtonVisualState.Brush = FillBrush(element.Brush, controlStyle.SelectedBackColor, stiButtonVisualState.Brush);
		stiButtonVisualState.TextBrush = FillBrush(element.TextBrush, controlStyle.SelectedForeColor, stiButtonVisualState.TextBrush);
		stiButtonVisualState.IconBrush = FillBrush(element.IconBrush, controlStyle.SelectedGlyphColor, stiButtonVisualState.IconBrush);
		stiButtonVisualState.IconSet.Icon = stiButtonVisualState.IconSet.Icon ?? @default.IconSet.Icon;
		stiButtonVisualState.IconSet.CheckedIcon = stiButtonVisualState.IconSet.CheckedIcon ?? @default.IconSet.CheckedIcon;
		stiButtonVisualState.IconSet.UncheckedIcon = stiButtonVisualState.IconSet.UncheckedIcon ?? @default.IconSet.UncheckedIcon;
		return stiButtonVisualState;
	}

	public static StiButtonVisualState GetHovered(StiButtonElement element)
	{
		StiControlElementStyle controlStyle = StiDashboardStyleHelper.GetControlStyle(element);
		StiButtonVisualState @default = GetDefault(element);
		StiButtonVisualState stiButtonVisualState = element.VisualStates.Hover.Clone() as StiButtonVisualState;
		stiButtonVisualState.Border = stiButtonVisualState.Border ?? @default.Border;
		stiButtonVisualState.Font = stiButtonVisualState.Font ?? @default.Font;
		stiButtonVisualState.Brush = FillBrush(element.Brush, controlStyle.SelectedBackColor, stiButtonVisualState.Brush);
		stiButtonVisualState.TextBrush = FillBrush(element.TextBrush, controlStyle.SelectedForeColor, stiButtonVisualState.TextBrush);
		stiButtonVisualState.IconBrush = FillBrush(element.IconBrush, controlStyle.SelectedGlyphColor, stiButtonVisualState.IconBrush);
		stiButtonVisualState.IconSet.Icon = stiButtonVisualState.IconSet.Icon ?? @default.IconSet.Icon;
		stiButtonVisualState.IconSet.CheckedIcon = stiButtonVisualState.IconSet.CheckedIcon ?? @default.IconSet.CheckedIcon;
		stiButtonVisualState.IconSet.UncheckedIcon = stiButtonVisualState.IconSet.UncheckedIcon ?? @default.IconSet.UncheckedIcon;
		return stiButtonVisualState;
	}

	public static StiButtonVisualState GetChecked(StiButtonElement element)
	{
		StiControlElementStyle controlStyle = StiDashboardStyleHelper.GetControlStyle(element);
		StiButtonVisualState @default = GetDefault(element);
		StiButtonVisualState stiButtonVisualState = element.VisualStates.Checked.Clone() as StiButtonVisualState;
		stiButtonVisualState.Border = stiButtonVisualState.Border ?? @default.Border;
		stiButtonVisualState.Font = stiButtonVisualState.Font ?? @default.Font;
		stiButtonVisualState.Brush = FillBrush(element.Brush, controlStyle.SelectedBackColor, stiButtonVisualState.Brush);
		stiButtonVisualState.TextBrush = FillBrush(element.TextBrush, controlStyle.SelectedForeColor, stiButtonVisualState.TextBrush);
		stiButtonVisualState.IconBrush = FillBrush(element.IconBrush, controlStyle.SelectedGlyphColor, stiButtonVisualState.IconBrush);
		stiButtonVisualState.IconSet.Icon = stiButtonVisualState.IconSet.Icon ?? @default.IconSet.Icon;
		stiButtonVisualState.IconSet.CheckedIcon = stiButtonVisualState.IconSet.CheckedIcon ?? @default.IconSet.CheckedIcon;
		stiButtonVisualState.IconSet.UncheckedIcon = stiButtonVisualState.IconSet.UncheckedIcon ?? @default.IconSet.UncheckedIcon;
		return stiButtonVisualState;
	}

	private static StiBrush FillBrush(StiBrush elementBrush, Color styleColor, StiBrush stateBrush)
	{
		if (IsDefault(stateBrush) && IsStyle(elementBrush))
		{
			return new StiSolidBrush(styleColor);
		}
		if (IsDefault(stateBrush))
		{
			return elementBrush;
		}
		if (IsStyle(stateBrush))
		{
			return new StiSolidBrush(styleColor);
		}
		return stateBrush;
	}

	private static bool IsDefault(StiBrush brush)
	{
		if (brush != null)
		{
			return brush is StiDefaultBrush;
		}
		return true;
	}

	private static bool IsStyle(StiBrush brush)
	{
		return brush is StiStyleBrush;
	}
}
