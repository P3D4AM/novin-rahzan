using System;

namespace Stimulsoft.Report.Components;

[Flags]
public enum StiPageType
{
	Form = 1,
	Page = 2,
	Dashboard = 4,
	Screen = 8,
	All = 0xF
}
