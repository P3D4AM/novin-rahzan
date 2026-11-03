using System;

namespace Stimulsoft.Report.Dictionary;

[Flags]
public enum StiRestrictionTypes
{
	None = 0,
	DenyEdit = 1,
	DenyDelete = 2,
	DenyMove = 4,
	DenyShow = 8
}
