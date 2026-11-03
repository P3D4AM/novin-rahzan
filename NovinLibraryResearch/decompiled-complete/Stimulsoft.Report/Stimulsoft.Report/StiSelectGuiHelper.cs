using System;
using System.ComponentModel;

namespace Stimulsoft.Report;

public static class StiSelectGuiHelper
{
	[Obsolete("StiSelectGuiHelper.IsRibbonGui is obsolete. Please use StiOptions.Windows.GlobalGuiStyle property.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static bool IsRibbonGui
	{
		get
		{
			return StiOptions.Windows.IsRibbonGuiEnabled();
		}
		set
		{
			StiOptions.Windows.GlobalGuiStyle = (value ? StiGlobalGuiStyle.Office2007Blue : StiGlobalGuiStyle.Office2003Blue);
		}
	}
}
