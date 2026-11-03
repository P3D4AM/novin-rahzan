using System;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Units;

namespace Stimulsoft.Report.Labels;

public sealed class StiPaperLabelInfo : ICloneable
{
	public string Name { get; }

	public StiPageOrientation Orientation { get; }

	public double Width { get; }

	public double Height { get; }

	public StiUnit Unit { get; set; }

	public object Clone()
	{
		return MemberwiseClone();
	}

	private string ConvertFromCM(double value)
	{
		if (!(Unit is StiCentimetersUnit))
		{
			value = new StiCentimetersUnit().ConvertToHInches(value);
			value = Unit.ConvertFromHInches(value);
		}
		value = Math.Round(value, 2);
		return value.ToString();
	}

	public override string ToString()
	{
		if (Width == 0.0 || Height == 0.0)
		{
			return Name;
		}
		string text = ConvertFromCM(Width);
		string text2 = ConvertFromCM(Height);
		string text3 = string.Empty;
		if (Orientation == StiPageOrientation.Landscape)
		{
			text3 = " Landscape";
			string text4 = text;
			text = text2;
			text2 = text4;
		}
		return Name + text3 + " (" + text + "x" + text2 + ")";
	}

	public StiPaperLabelInfo(string name, StiPageOrientation orientation, double width, double height)
	{
		Name = name;
		Orientation = orientation;
		Width = width;
		Height = height;
	}
}
