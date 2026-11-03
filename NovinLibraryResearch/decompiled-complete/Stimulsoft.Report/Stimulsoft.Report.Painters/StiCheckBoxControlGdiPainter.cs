using System;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dialogs;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Painters;

public class StiCheckBoxControlGdiPainter : StiReportControlGdiPainter
{
	public override void Paint(StiComponent component, StiPaintEventArgs e)
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Expected O, but got Unknown
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		if (!(e.Context is Graphics))
		{
			throw new Exception("StiGdiPainter can work only with System.Drawing.Graphics context!");
		}
		StiCheckBoxControl stiCheckBoxControl = component as StiCheckBoxControl;
		stiCheckBoxControl.InvokePainting(stiCheckBoxControl, e);
		if (!e.Cancel && (stiCheckBoxControl.Enabled || stiCheckBoxControl.IsDesigning))
		{
			Graphics graphics = e.Graphics;
			RectangleD paintRectangle = stiCheckBoxControl.GetPaintRectangle();
			if (((RectangleD)(ref paintRectangle)).Width > 0.0 && ((RectangleD)(ref paintRectangle)).Height > 0.0)
			{
				RectangleD clipRectangle = e.ClipRectangle;
				if (((RectangleD)(ref clipRectangle)).IsEmpty || ((RectangleD)(ref paintRectangle)).IntersectsWith(e.ClipRectangle))
				{
					CheckBox val = new CheckBox();
					try
					{
						((Control)val).Text = stiCheckBoxControl.Text;
						val.Checked = stiCheckBoxControl.Checked;
						DrawControl(component as StiReportControl, e.Graphics, (Control)(object)val, paintRectangle);
					}
					finally
					{
						((IDisposable)val)?.Dispose();
					}
					PaintEvents(stiCheckBoxControl, e.Graphics, paintRectangle);
					PaintConditions(stiCheckBoxControl, e.Graphics, paintRectangle);
				}
			}
		}
		e.Cancel = false;
		stiCheckBoxControl.InvokePainted(stiCheckBoxControl, e);
	}
}
