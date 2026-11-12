using System;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;

namespace Stimulsoft.Dashboard;

[Flags]
[JsonConverter(typeof(StringEnumConverter))]
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
	PercentageSymbol = 0x80
}
