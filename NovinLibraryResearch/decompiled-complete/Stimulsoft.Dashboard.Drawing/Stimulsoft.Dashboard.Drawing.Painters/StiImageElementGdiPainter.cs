using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Drawing.Painters;

public class StiImageElementGdiPainter : StiAtomElementGdiPainter
{
	public override Color DefaultBackgroundColor => Color.FromArgb(50, Color.Green);

	public override void PaintNoResult(Graphics g, RectangleD rect, IStiElement element, double? zoom = null)
	{
	}

	public override void PaintNotDefined(Graphics g, RectangleF rect, IStiElement element, double? zoom = null)
	{
		PaintNotDefinedStatus(g, rect, element, Loc.Get("Dashboard", "ImageNotSpecified"), GetElementIdent(element));
	}
}
