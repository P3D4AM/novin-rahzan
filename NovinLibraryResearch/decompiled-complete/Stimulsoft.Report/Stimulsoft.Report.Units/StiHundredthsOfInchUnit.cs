using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Units;

public class StiHundredthsOfInchUnit : StiUnit
{
	public override double RollerStep => 100.0;

	public override double Factor => 100.0;

	public override string ShortName => "hi";

	public override string ToString()
	{
		return Loc.Get("PropertyEnum", "StiReportUnitTypeHundredthsOfInch");
	}

	public override double ConvertToHInches(double value)
	{
		return value;
	}

	public override decimal ConvertToHInches(decimal value)
	{
		return value;
	}

	public override double ConvertFromHInches(double value)
	{
		return value;
	}

	public override decimal ConvertFromHInches(decimal value)
	{
		return value;
	}
}
