using System;

namespace Stimulsoft.Report.Web;

[Flags]
public enum StiDesignerPermissions
{
	None = 0,
	Create = 1,
	Delete = 2,
	Modify = 4,
	View = 8,
	ModifyView = 0xC,
	All = 0xF
}
