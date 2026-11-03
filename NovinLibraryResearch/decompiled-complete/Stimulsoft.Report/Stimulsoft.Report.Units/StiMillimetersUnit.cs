using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Units;

public class StiMillimetersUnit : StiUnit
{
	public override double RollerStep => 10.0;

	public override double Factor => 39.37007874015748;

	public override string ShortName => "mm";

	public override string ToString()
	{
		return Loc.Get("PropertyEnum", "StiReportUnitTypeMillimeters");
	}

	public override double ConvertToHInches(double value)
	{
		return value * 10.0 / 2.54;
	}

	public override decimal ConvertToHInches(decimal value)
	{
		return value * 10m / 2.54m;
	}

	public override double ConvertFromHInches(double value)
	{
		return value * 2.54 / 10.0;
	}

	public override decimal ConvertFromHInches(decimal value)
	{
		return value * 2.54m / 10m;
	}
}
