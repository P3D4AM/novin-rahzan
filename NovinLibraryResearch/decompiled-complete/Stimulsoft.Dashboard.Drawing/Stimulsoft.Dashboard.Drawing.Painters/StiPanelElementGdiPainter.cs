using System.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Dashboard.Drawing.Painters;

public class StiPanelElementGdiPainter : StiElementGdiPainter
{
	public override void Paint(StiComponent component, StiPaintEventArgs e)
	{
		base.Paint(component, e);
		new StiContainerGdiPainter().PaintComponents(component as StiContainer, e);
	}

	protected override string GetElementIdent(object element)
	{
		return "Panel";
	}

	public override void PaintNotDefined(Graphics g, RectangleF rect, IStiElement element, double? zoom = null)
	{
	}

	public override void PaintNotEnabled(Graphics g, RectangleF rect, IStiElement element, double? zoom = null)
	{
	}
}
