using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Drawing.Painters;

public class StiTextElementGdiPainter : StiAtomElementGdiPainter
{
	public override Color DefaultBackgroundColor => Color.FromArgb(150, Color.White);

	public override bool IsDefined(StiComponent component)
	{
		return false;
	}

	public override void PaintNoResult(Graphics g, RectangleD rect, IStiElement element, double? zoom = null)
	{
	}

	public override void PaintNotDefined(Graphics g, RectangleF rect, IStiElement element, double? zoom = null)
	{
	}
}
