using System;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Painters;

public interface IStiWpfBreakTextHelper
{
	string BreakText(RectangleD rect, ref string text, StiText textComp);

	bool BreakRichText(StiComponent baseComponent, StiComponent dividedComponent, double devideFactor, ref double divideLine, AppDomain domain);
}
