namespace Stimulsoft.Report.Chart;

internal class StiDataItem
{
	internal object Argument;

	internal object Value;

	internal object ValueEnd;

	internal object Weight;

	internal object ValueOpen;

	internal object ValueClose;

	internal object ValueLow;

	internal object ValueHigh;

	internal object Title;

	internal object Key;

	internal object Color;

	internal object ToolTip;

	internal object Tag;

	public StiDataItem(object argument, object value, object valueEnd, object weight, object valueOpen, object valueClose, object valueLow, object valueHight, object title, object key, object color, object toolTip, object tag)
	{
		Argument = argument;
		Value = value;
		ValueEnd = valueEnd;
		Weight = weight;
		ValueOpen = valueOpen;
		ValueClose = valueClose;
		ValueLow = valueLow;
		ValueHigh = valueHight;
		Title = title;
		Key = key;
		Color = color;
		ToolTip = toolTip;
		Tag = tag;
	}
}
