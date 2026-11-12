using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Styles;

namespace Stimulsoft.Dashboard.Viewer.Elements;

[ToolboxItem(false)]
public abstract class StiBaseElementControl : Panel, IStiElementControl
{
	public IStiElement Element { get; set; }

	protected virtual bool IsTransparent => StiDashboardStyleHelper.GetBackColor(Element, null, allowOpacity: true).A != byte.MaxValue;

	protected override void OnPaint(PaintEventArgs e)
	{
		((Control)this).OnPaint(e);
		if (!IsTransparent)
		{
			Rectangle rectangle = new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height);
			Color backColor = StiDashboardStyleHelper.GetBackColor(Element, null, allowOpacity: true);
			StiRoundedBackgroundGdiPainter.PaintBackground(e.Graphics, rectangle, null, null, backColor, StiButtonShapeType.Rectangle, isEnabled: true, 1f);
		}
	}

	protected StiBaseElementControl(IStiElement element)
	{
		((Control)this).SetStyle((ControlStyles)131072, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)16, true);
		Element = element;
	}
}
