using System.Globalization;

namespace PacketParser.Settings.Accounting;

public class NumberFormatSettings
{
	public string NumberGroupingFormat => NumberFormatInfo.CurrentInfo.NumberGroupSeparator;

	public string DecimalSeprator => NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator;
}
