using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Styles;

namespace Stimulsoft.Dashboard.Viewer.Elements;

[ToolboxItem(false)]
public class StiScrollableControl : ScrollableControl
{
	public IStiElement Element { get; set; }

	private bool IsTransparent => StiDashboardStyleHelper.GetBackColor(Element, null, allowOpacity: true).A != byte.MaxValue;

	protected override void OnPaintBackground(PaintEventArgs e)
	{
		if (IsTransparent)
		{
			((ScrollableControl)this).OnPaintBackground(e);
		}
		else
		{
			DrawBackground(e);
		}
	}

	private void DrawBackground(PaintEventArgs e)
	{
		Rectangle rectangle = new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height);
		Color backColor = StiDashboardStyleHelper.GetBackColor(Element, null, allowOpacity: true);
		StiRoundedBackgroundGdiPainter.PaintBackground(e.Graphics, rectangle, null, null, backColor, StiButtonShapeType.Rectangle, isEnabled: true, 1f);
	}

	protected override Point ScrollToControl(Control activeControl)
	{
		return ((ScrollableControl)this).AutoScrollPosition;
	}

	public StiScrollableControl(IStiElement element)
	{
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle((ControlStyles)2048, true);
		((Control)this).SetStyle((ControlStyles)131072, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		Element = element;
	}
}
