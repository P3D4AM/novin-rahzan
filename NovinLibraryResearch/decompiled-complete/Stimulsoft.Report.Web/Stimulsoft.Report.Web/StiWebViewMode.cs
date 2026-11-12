using System;
using System.ComponentModel;

namespace Stimulsoft.Report.Web;

public enum StiWebViewMode
{
	SinglePage,
	Continuous,
	MultiplePages,
	[Obsolete("This value is obsolete. It will be removed in next versions. Please use the SinglePage value instead.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	OnePage,
	[Obsolete("This value is obsolete. It will be removed in next versions. Please use the MultiplePages value instead.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	WholeReport
}
