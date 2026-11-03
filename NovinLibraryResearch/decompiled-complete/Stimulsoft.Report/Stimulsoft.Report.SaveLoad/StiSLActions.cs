using System;

namespace Stimulsoft.Report.SaveLoad;

[Flags]
public enum StiSLActions
{
	Save = 1,
	Load = 2,
	Merge = 4
}
