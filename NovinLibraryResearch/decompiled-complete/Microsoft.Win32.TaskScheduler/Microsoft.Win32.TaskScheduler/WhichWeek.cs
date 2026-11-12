using System;

namespace Microsoft.Win32.TaskScheduler;

[Flags]
public enum WhichWeek : short
{
	FirstWeek = 1,
	SecondWeek = 2,
	ThirdWeek = 4,
	FourthWeek = 8,
	LastWeek = 0x10,
	AllWeeks = 0x1F
}
