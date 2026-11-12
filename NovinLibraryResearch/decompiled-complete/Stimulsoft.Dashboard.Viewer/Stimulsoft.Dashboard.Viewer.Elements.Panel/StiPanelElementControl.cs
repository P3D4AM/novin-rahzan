using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Components.Panel;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Styles;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Dashboard.Viewer.Elements.Panel;

[ToolboxItem(false)]
public class StiPanelElementControl : StiShadowPanel, IStiElementControl, IStiRefreshControl
{
	public IStiElement Element { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiPanelElement PanelElement => Element as StiPanelElement;

	public void RefreshControl(object sender, StiReportControlUpdateEventArgs e)
	{
		if (sender == Element)
		{
			((Control)this).Invalidate();
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		Graphics graphics = e.Graphics;
		Rectangle rectangle = new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height);
		Color backColor = StiDashboardStyleHelper.GetBackColor(Element, null, allowOpacity: true);
		StiRoundedBackgroundGdiPainter.PaintBackground(graphics, rectangle, PanelElement.Border, PanelElement.CornerRadius, backColor, StiButtonShapeType.Rectangle, isEnabled: true, 1f);
		StiAdvancedWatermark stiAdvancedWatermark = (Element as IStiDashboardWatermark)?.DashboardWatermark;
		if (stiAdvancedWatermark != null)
		{
			StiAdvancedWatermarkGdiPainter.PaintWatermark(graphics, stiAdvancedWatermark, PanelElement.CornerRadius, RectangleD.CreateFromRectangle(rectangle), 1.0);
		}
		StiRoundedBorderGdiPainter.PaintBorder(graphics, rectangle, PanelElement.Border, PanelElement.CornerRadius, StiButtonShapeType.Rectangle, 1f, PanelElement.IsDesigning);
		base.OnPaint(e);
	}

	public StiPanelElementControl(StiPanelElement element)
	{
		Element = element;
	}
}
