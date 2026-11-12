using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Report.Components.Gauge;
using Stimulsoft.Report.Gauge;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Gauge.Design;

[ToolboxItem(false)]
internal class StiGaugePanel : Panel
{
	internal StiGauge Gauge;

	protected override void OnPaint(PaintEventArgs e)
	{
		((Control)this).OnPaint(e);
		if (Gauge == null)
		{
			return;
		}
		Graphics graphics = e.Graphics;
		RectangleF rect = RectangleF.Empty;
		bool flag = true;
		if (Gauge.Scales.Count > 0 && Gauge.Scales[0] is StiLinearScale)
		{
			flag = false;
		}
		if (flag)
		{
			float num = Math.Min(((Control)this).Width, ((Control)this).Height) - 20;
			if (num > 0f)
			{
				rect = new RectangleF(((float)((Control)this).Width - num) / 2f, ((float)((Control)this).Height - num) / 2f, num, num);
			}
		}
		else
		{
			float num2 = ((Control)this).Height - 20;
			if (num2 > 0f)
			{
				float num3 = num2 * 0.75f;
				rect = new RectangleF(((float)((Control)this).Width - num3) / 2f, ((float)((Control)this).Height - num2) / 2f, num3, num2);
			}
		}
		rect.X = 10f;
		rect.Y = 10f;
		if (rect.Width > 0f && rect.Height > 0f)
		{
			StiGaugeGdiPainter stiGaugeGdiPainter = new StiGaugeGdiPainter();
			stiGaugeGdiPainter.DrawGauge(graphics, Gauge, rect, 1f);
		}
	}

	public StiGaugePanel()
	{
		((Control)this).SetStyle((ControlStyles)65536, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).BackColor = Color.White;
	}
}
