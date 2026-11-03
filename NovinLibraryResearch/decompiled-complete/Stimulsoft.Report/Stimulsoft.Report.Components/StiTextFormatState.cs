using System;

namespace Stimulsoft.Report.Components;

[Flags]
public enum StiTextFormatState
{
	None = 0,
	DecimalDigits = 1,
	DecimalSeparator = 2,
	GroupSeparator = 4,
	GroupSize = 8,
	PositivePattern = 0x10,
	NegativePattern = 0x20,
	CurrencySymbol = 0x40,
	PercentageSymbol = 0x80,
	Abbreviation = 0x100,
	NegativeInRed = 0x200
}
