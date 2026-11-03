using System;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dialogs;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Painters;

public class StiRadioButtonControlGdiPainter : StiReportControlGdiPainter
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
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		if (!(e.Context is Graphics))
		{
			throw new Exception("StiGdiPainter can work only with System.Drawing.Graphics context!");
		}
		StiRadioButtonControl stiRadioButtonControl = component as StiRadioButtonControl;
		stiRadioButtonControl.InvokePainting(stiRadioButtonControl, e);
		if (!e.Cancel && (stiRadioButtonControl.Enabled || stiRadioButtonControl.IsDesigning))
		{
			Graphics graphics = e.Graphics;
			RectangleD paintRectangle = stiRadioButtonControl.GetPaintRectangle();
			if (((RectangleD)(ref paintRectangle)).Width > 0.0 && ((RectangleD)(ref paintRectangle)).Height > 0.0)
			{
				RectangleD clipRectangle = e.ClipRectangle;
				if (((RectangleD)(ref clipRectangle)).IsEmpty || ((RectangleD)(ref paintRectangle)).IntersectsWith(e.ClipRectangle))
				{
					RadioButton val = new RadioButton();
					try
					{
						((Control)val).Text = stiRadioButtonControl.Text;
						val.Checked = stiRadioButtonControl.Checked;
						DrawControl(stiRadioButtonControl, e.Graphics, (Control)(object)val, paintRectangle);
					}
					finally
					{
						((IDisposable)val)?.Dispose();
					}
					PaintEvents(stiRadioButtonControl, e.Graphics, paintRectangle);
					PaintConditions(stiRadioButtonControl, e.Graphics, paintRectangle);
				}
			}
		}
		e.Cancel = false;
		stiRadioButtonControl.InvokePainted(stiRadioButtonControl, e);
	}
}
