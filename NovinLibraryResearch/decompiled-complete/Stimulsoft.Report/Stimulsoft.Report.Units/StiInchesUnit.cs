using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Units;

public class StiInchesUnit : StiUnit
{
	public override double RollerStep => 1.0;

	public override double Factor => 100.0;

	public override string ShortName => "in";

	public override string ToString()
	{
		return Loc.Get("PropertyEnum", "StiReportUnitTypeInches");
	}

	public override double ConvertToHInches(double value)
	{
		return value * 100.0;
	}

	public override decimal ConvertToHInches(decimal value)
	{
		return value * 100m;
	}

	public override double ConvertFromHInches(double value)
	{
		return value / 100.0;
	}

	public override decimal ConvertFromHInches(decimal value)
	{
		return value / 100m;
	}
}
