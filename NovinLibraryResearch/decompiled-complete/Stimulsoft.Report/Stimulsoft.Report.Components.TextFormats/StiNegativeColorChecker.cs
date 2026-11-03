namespace Stimulsoft.Report.Components.TextFormats;

public static class StiNegativeColorChecker
{
	public static bool IsNegativeInRed(StiFormatService format)
	{
		if (format == null)
		{
			return false;
		}
		if (format is StiNumberFormatService && (((StiNumberFormatService)format).State & StiTextFormatState.NegativeInRed) > StiTextFormatState.None)
		{
			return true;
		}
		if (format is StiCurrencyFormatService && (((StiCurrencyFormatService)format).State & StiTextFormatState.NegativeInRed) > StiTextFormatState.None)
		{
			return true;
		}
		if (format is StiPercentageFormatService && (((StiPercentageFormatService)format).State & StiTextFormatState.NegativeInRed) > StiTextFormatState.None)
		{
			return true;
		}
		return false;
	}
}
