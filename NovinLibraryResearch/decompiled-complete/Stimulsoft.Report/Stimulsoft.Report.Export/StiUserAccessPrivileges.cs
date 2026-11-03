using System;

namespace Stimulsoft.Report.Export;

[Flags]
public enum StiUserAccessPrivileges
{
	None = 0,
	PrintDocument = 1,
	ModifyContents = 2,
	CopyTextAndGraphics = 4,
	AddOrModifyTextAnnotations = 8,
	All = 0xF
}
