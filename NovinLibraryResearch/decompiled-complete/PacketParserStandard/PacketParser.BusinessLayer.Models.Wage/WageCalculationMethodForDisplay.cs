using PacketParser.Entities;

namespace PacketParser.BusinessLayer.Models.Wage;

public class WageCalculationMethodForDisplay
{
	public EnWageCalculationMethod CalculationMethod { get; private set; }

	public string CalculationMethodName => CalculationMethod.GetDisplay().GetTranslate();

	public WageCalculationMethodForDisplay(EnWageCalculationMethod _currency)
	{
		CalculationMethod = _currency;
	}
}
