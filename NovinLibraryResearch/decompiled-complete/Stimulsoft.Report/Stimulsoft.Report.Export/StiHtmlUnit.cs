namespace Stimulsoft.Report.Export;

public class StiHtmlUnit
{
	public enum StiHtmlUnitType
	{
		Pixel,
		Point
	}

	private const double hiToPt = 0.716;

	public double Value;

	public StiHtmlUnitType UnitType;

	public override string ToString()
	{
		return (int)Value + "px";
	}

	public static string ToPixelString(int value)
	{
		int num = value;
		return num + "px";
	}

	public static StiHtmlUnit NewUnit(double value)
	{
		return NewUnit(value, StiHtmlUnitType.Pixel);
	}

	public static StiHtmlUnit NewUnit(double value, StiHtmlUnitType unitType)
	{
		StiHtmlUnit stiHtmlUnit = new StiHtmlUnit();
		stiHtmlUnit.UnitType = unitType;
		stiHtmlUnit.Value = value;
		return stiHtmlUnit;
	}

	public static StiHtmlUnit NewUnit(double value, bool usePoints)
	{
		return NewUnit(value, usePoints ? StiHtmlUnitType.Point : StiHtmlUnitType.Pixel);
	}

	public static bool IsNullOrZero(StiHtmlUnit unit)
	{
		if (unit != null)
		{
			return unit.Value == 0.0;
		}
		return true;
	}
}
