using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Viewer.Elements;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Helpers;

namespace Stimulsoft.Dashboard.Viewer;

[ToolboxItem(false)]
public class StiShadowPanel : Panel
{
	protected override void OnPaint(PaintEventArgs e)
	{
		((Control)this).OnPaint(e);
		foreach (IStiElementControl item in (from Control c in (IEnumerable)((Control)this).Controls
			where c is IStiElementControl
			select c).Cast<IStiElementControl>())
		{
			if (!(item.Element is IStiSimpleShadow stiSimpleShadow) || !stiSimpleShadow.Shadow.Visible)
			{
				continue;
			}
			Control val = (Control)((item is Control) ? item : null);
			if (val.Visible)
			{
				RectangleD rect = new RectangleD(val.Left, val.Top, val.Width, val.Height);
				IStiSimpleBorder stiSimpleBorder = item.Element as IStiSimpleBorder;
				IStiCornerRadius stiCornerRadius = item.Element as IStiCornerRadius;
				StiBorderSides borderSide = ((stiSimpleBorder?.Border != null) ? stiSimpleBorder.Border.Side : StiBorderSides.All);
				bool isCircle = stiSimpleShadow is IStiButtonElement stiButtonElement && stiButtonElement.ShapeType == StiButtonShapeType.Circle;
				StiReport report = item.Element.Report;
				Bitmap val2 = StiSimpleShadowCache.GetFromCache(report, stiSimpleShadow.Shadow, stiCornerRadius?.CornerRadius, borderSide, isCircle, val.Size, 1f);
				if (val2 == null)
				{
					val2 = StiSimpleShadowPainter.GetShadowBitmap(rect, stiSimpleShadow.Shadow, stiCornerRadius?.CornerRadius, isCircle, 1f);
					StiSimpleShadowCache.AddToCache(report, stiSimpleShadow.Shadow, val2, stiCornerRadius?.CornerRadius, borderSide, isCircle, val.Size, 1f);
				}
				Point shadowLocation = StiSimpleShadowPainter.GetShadowLocation(rect, stiSimpleShadow.Shadow, isCircle, 1f);
				e.Graphics.DrawImage((Image)(object)val2, shadowLocation);
			}
		}
	}

	public StiShadowPanel()
	{
		((Control)this).SetStyle((ControlStyles)131072, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)16, true);
	}
}
