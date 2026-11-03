using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dialogs;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Painters;

public class StiLabelControlGdiPainter : StiReportControlGdiPainter
{
	public override void Paint(StiComponent component, StiPaintEventArgs e)
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Expected O, but got Unknown
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Expected O, but got Unknown
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Invalid comparison between Unknown and I4
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Invalid comparison between Unknown and I4
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0311: Unknown result type (might be due to invalid IL or missing references)
		//IL_031f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cf: Expected O, but got Unknown
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Invalid comparison between Unknown and I4
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Expected I4, but got Unknown
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Invalid comparison between Unknown and I4
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Invalid comparison between Unknown and I4
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Invalid comparison between Unknown and I4
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Invalid comparison between Unknown and I4
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Invalid comparison between Unknown and I4
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Expected O, but got Unknown
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Invalid comparison between Unknown and I4
		if (!(e.Context is Graphics))
		{
			throw new Exception("StiGdiPainter can work only with System.Drawing.Graphics context!");
		}
		StiLabelControl stiLabelControl = component as StiLabelControl;
		stiLabelControl.InvokePainting(stiLabelControl, e);
		if (!e.Cancel && (stiLabelControl.Enabled || stiLabelControl.IsDesigning))
		{
			Graphics graphics = e.Graphics;
			RectangleD paintRectangle = stiLabelControl.GetPaintRectangle();
			if (((RectangleD)(ref paintRectangle)).Width > 0.0 && ((RectangleD)(ref paintRectangle)).Height > 0.0)
			{
				RectangleD clipRectangle = e.ClipRectangle;
				if (((RectangleD)(ref clipRectangle)).IsEmpty || ((RectangleD)(ref paintRectangle)).IntersectsWith(e.ClipRectangle))
				{
					SolidBrush val = new SolidBrush(stiLabelControl.BackColor);
					try
					{
						graphics.FillRectangle((Brush)(object)val, ((RectangleD)(ref paintRectangle)).ToRectangleF());
					}
					finally
					{
						((IDisposable)val)?.Dispose();
					}
					if (stiLabelControl.Text != null || stiLabelControl.Text.Length > 0)
					{
						StringFormat val2 = new StringFormat();
						try
						{
							val2.Trimming = (StringTrimming)3;
							val2.FormatFlags = (StringFormatFlags)4096;
							val2.Trimming = (StringTrimming)3;
							if ((int)stiLabelControl.RightToLeft == 1)
							{
								val2.FormatFlags = (StringFormatFlags)(val2.FormatFlags | 1);
							}
							ContentAlignment textAlign = stiLabelControl.TextAlign;
							if ((int)textAlign <= 32)
							{
								switch (textAlign - 1)
								{
								default:
									if ((int)textAlign != 16)
									{
										if ((int)textAlign == 32)
										{
											val2.Alignment = (StringAlignment)1;
											val2.LineAlignment = (StringAlignment)1;
										}
									}
									else
									{
										val2.Alignment = (StringAlignment)0;
										val2.LineAlignment = (StringAlignment)1;
									}
									break;
								case 0:
									val2.Alignment = (StringAlignment)0;
									val2.LineAlignment = (StringAlignment)0;
									break;
								case 1:
									val2.Alignment = (StringAlignment)1;
									val2.LineAlignment = (StringAlignment)0;
									break;
								case 3:
									val2.Alignment = (StringAlignment)2;
									val2.LineAlignment = (StringAlignment)0;
									break;
								case 2:
									break;
								}
							}
							else if ((int)textAlign <= 256)
							{
								if ((int)textAlign != 64)
								{
									if ((int)textAlign == 256)
									{
										val2.Alignment = (StringAlignment)0;
										val2.LineAlignment = (StringAlignment)2;
									}
								}
								else
								{
									val2.Alignment = (StringAlignment)2;
									val2.LineAlignment = (StringAlignment)1;
								}
							}
							else if ((int)textAlign != 512)
							{
								if ((int)textAlign == 1024)
								{
									val2.Alignment = (StringAlignment)2;
									val2.LineAlignment = (StringAlignment)2;
								}
							}
							else
							{
								val2.Alignment = (StringAlignment)1;
								val2.LineAlignment = (StringAlignment)2;
							}
							if (stiLabelControl.Enabled)
							{
								Brush val3 = (Brush)new SolidBrush(stiLabelControl.ForeColor);
								try
								{
									graphics.DrawString(stiLabelControl.Text, stiLabelControl.Font, val3, (RectangleF)((RectangleD)(ref paintRectangle)).ToRectangle(), val2);
								}
								finally
								{
									((IDisposable)val3)?.Dispose();
								}
							}
							else
							{
								ControlPaint.DrawStringDisabled(graphics, stiLabelControl.Text, stiLabelControl.Font, SystemColors.ControlLight, (RectangleF)((RectangleD)(ref paintRectangle)).ToRectangle(), val2);
							}
						}
						finally
						{
							((IDisposable)val2)?.Dispose();
						}
					}
					if (stiLabelControl.IsDesigning)
					{
						Pen val4 = new Pen(SystemColors.ControlDarkDark, 1f);
						try
						{
							val4.DashStyle = (DashStyle)1;
							StiDrawing.DrawRectangle(graphics, val4, ((RectangleD)(ref paintRectangle)).Left, ((RectangleD)(ref paintRectangle)).Top, ((RectangleD)(ref paintRectangle)).Width, ((RectangleD)(ref paintRectangle)).Height);
						}
						finally
						{
							((IDisposable)val4)?.Dispose();
						}
					}
					PaintEvents(stiLabelControl, e.Graphics, paintRectangle);
					PaintConditions(stiLabelControl, e.Graphics, paintRectangle);
				}
			}
		}
		e.Cancel = false;
		stiLabelControl.InvokePainted(stiLabelControl, e);
	}
}
