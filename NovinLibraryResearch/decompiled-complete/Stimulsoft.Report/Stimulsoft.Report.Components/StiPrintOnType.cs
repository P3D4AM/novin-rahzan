using System;

namespace Stimulsoft.Report.Components;

[Flags]
public enum StiPrintOnType
{
	AllPages = 0,
	ExceptFirstPage = 1,
	ExceptLastPage = 2,
	ExceptFirstAndLastPage = 3,
	OnlyFirstPage = 4,
	OnlyLastPage = 8,
	OnlyFirstAndLastPage = 0xC
}
