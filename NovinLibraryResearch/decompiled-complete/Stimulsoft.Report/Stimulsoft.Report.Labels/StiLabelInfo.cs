using System;
using Stimulsoft.Report.Units;

namespace Stimulsoft.Report.Labels;

public sealed class StiLabelInfo : ICloneable
{
	public string Manufacturer { get; }

	public string LabelName { get; }

	public double LabelWidth { get; }

	public double LabelHeight { get; }

	public double HorizontalGap { get; }

	public double VerticalGap { get; }

	public double LeftMargin { get; }

	public double TopMargin { get; }

	public double PaperWidth { get; }

	public double PaperHeight { get; }

	public int NumberOfRows { get; }

	public int NumberOfColumns { get; }

	public StiUnit Unit { get; set; }

	public object Clone()
	{
		return MemberwiseClone();
	}

	private string ConvertFromInch(double value)
	{
		if (!(Unit is StiInchesUnit))
		{
			value = new StiInchesUnit().ConvertToHInches(value);
			value = Unit.ConvertFromHInches(value);
		}
		value = Math.Round(value, 2);
		return value.ToString();
	}

	public override string ToString()
	{
		string text = ConvertFromInch(LabelWidth);
		string text2 = ConvertFromInch(LabelHeight);
		return LabelName + " (" + text + "x" + text2 + ")";
	}

	public StiLabelInfo(string manufacturer, string labelName, double labelWidth, double labelHeight, double horizontalGap, double verticalGap, double leftMargin, double topMargin, double paperWidth, double paperHeight, int numberOfRows, int numberOfColumns)
	{
		Manufacturer = manufacturer;
		LabelName = labelName;
		LabelWidth = labelWidth;
		LabelHeight = labelHeight;
		HorizontalGap = horizontalGap;
		VerticalGap = verticalGap;
		LeftMargin = leftMargin;
		TopMargin = topMargin;
		PaperWidth = paperWidth;
		PaperHeight = paperHeight;
		NumberOfColumns = numberOfColumns;
		NumberOfRows = numberOfRows;
	}
}
