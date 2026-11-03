using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Units;

public class StiCentimetersUnit : StiUnit
{
	public override double RollerStep => 1.0;

	public override double Factor => 39.37007874015748;

	public override string ShortName => "cm";

	public override string ToString()
	{
		return Loc.Get("PropertyEnum", "StiReportUnitTypeCentimeters");
	}

	public override double ConvertToHInches(double value)
	{
		return value * 100.0 / 2.54;
	}

	public override decimal ConvertToHInches(decimal value)
	{
		return value * 100m / 2.54m;
	}

	public override double ConvertFromHInches(double value)
	{
		return value * 2.54 / 100.0;
	}

	public override decimal ConvertFromHInches(decimal value)
	{
		return value * 2.54m / 100m;
	}
}
