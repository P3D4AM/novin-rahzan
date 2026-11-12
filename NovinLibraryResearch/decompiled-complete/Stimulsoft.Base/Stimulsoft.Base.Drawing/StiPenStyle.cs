using System.ComponentModel;
using System.Drawing.Design;

namespace Stimulsoft.Base.Drawing;

[Editor("Stimulsoft.Base.Drawing.Design.StiPenStyleEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
public enum StiPenStyle
{
	Solid,
	Dash,
	DashDot,
	DashDotDot,
	Dot,
	Double,
	None
}
