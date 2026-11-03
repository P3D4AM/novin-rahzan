using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Painters;

public class StiSubReportGdiPainter : StiContainerGdiPainter
{
	public override void Paint(StiComponent component, StiPaintEventArgs e)
	{
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Expected O, but got Unknown
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_0385: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Expected O, but got Unknown
		//IL_0363: Unknown result type (might be due to invalid IL or missing references)
		//IL_036a: Invalid comparison between Unknown and I4
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Expected O, but got Unknown
		//IL_03fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_036f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Expected O, but got Unknown
		//IL_0437: Unknown result type (might be due to invalid IL or missing references)
		//IL_0440: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b7: Expected O, but got Unknown
		//IL_031b: Unknown result type (might be due to invalid IL or missing references)
		StiSubReport stiSubReport = component as StiSubReport;
		if (!e.DrawBorderFormatting && e.DrawTopmostBorderSides && !stiSubReport.Border.Topmost)
		{
			return;
		}
		if (!(e.Context is Graphics))
		{
			throw new Exception("StiGdiPainter can work only with System.Drawing.Graphics context!");
		}
		if (e.DrawBorderFormatting)
		{
			stiSubReport.InvokePainting(stiSubReport, e);
		}
		Graphics graphics = e.Graphics;
		if (!e.Cancel)
		{
			RectangleD paintRectangle = stiSubReport.GetPaintRectangle();
			if (((RectangleD)(ref paintRectangle)).Width > 0.0 && ((RectangleD)(ref paintRectangle)).Height > 0.0)
			{
				RectangleD clipRectangle = e.ClipRectangle;
				if ((((RectangleD)(ref clipRectangle)).IsEmpty || ((RectangleD)(ref paintRectangle)).IntersectsWith(e.ClipRectangle)) && stiSubReport.IsDesigning)
				{
					if (e.DrawBorderFormatting)
					{
						if (stiSubReport.Brush is StiSolidBrush && ((StiSolidBrush)stiSubReport.Brush).Color == Color.Transparent && stiSubReport.Report.Info.FillComponent && stiSubReport.IsDesigning)
						{
							Brush val = (Brush)new HatchBrush((HatchStyle)3, Color.FromArgb(240, Color.LightBlue), Color.FromArgb(240, Color.White));
							try
							{
								StiDrawing.FillRectangle(graphics, val, ((RectangleD)(ref paintRectangle)).Left, ((RectangleD)(ref paintRectangle)).Top, ((RectangleD)(ref paintRectangle)).Width, ((RectangleD)(ref paintRectangle)).Height);
								StiDrawing.DrawRectangle(graphics, Pens.Black, ((RectangleD)(ref paintRectangle)).Left, ((RectangleD)(ref paintRectangle)).Top, ((RectangleD)(ref paintRectangle)).Width, ((RectangleD)(ref paintRectangle)).Height);
							}
							finally
							{
								((IDisposable)val)?.Dispose();
							}
						}
						else
						{
							StiDrawing.FillRectangle(graphics, stiSubReport.Brush, paintRectangle);
						}
					}
					if (e.DrawBorderFormatting)
					{
						StringFormat val2 = new StringFormat();
						try
						{
							Font val3 = new Font("Segoe UI", (float)(15.0 * stiSubReport.Page.Zoom));
							try
							{
								SolidBrush val4 = new SolidBrush(Color.FromArgb(146, 146, 146));
								try
								{
									val2.LineAlignment = (StringAlignment)1;
									val2.Alignment = (StringAlignment)1;
									string text = null;
									if (!string.IsNullOrEmpty(stiSubReport.SubReportUrl))
									{
										text = $"{stiSubReport.Name}\n{stiSubReport.SubReportUrl}";
									}
									else
									{
										bool flag = ((RectangleD)(ref paintRectangle)).Height < 44.0 * StiScale.Factor;
										text = string.Format("{0}{4}{3}{1} : {2}", stiSubReport.Name, flag ? "" : StiLocalization.Get("PropertyMain", "SubReportPage"), (stiSubReport.SubReportPage != null) ? stiSubReport.SubReportPage.Name : StiLocalization.Get("Report", "NotAssigned"), flag ? "" : "\n", (!string.IsNullOrWhiteSpace(stiSubReport.Alias) && stiSubReport.Alias != stiSubReport.Name && !flag) ? $" [{stiSubReport.Alias}]" : "");
									}
									StiTextDrawing.DrawString(graphics, text, val3, (Brush)(object)val4, new RectangleD(((RectangleD)(ref paintRectangle)).Left, ((RectangleD)(ref paintRectangle)).Top, ((RectangleD)(ref paintRectangle)).Width, ((RectangleD)(ref paintRectangle)).Height), val2);
								}
								finally
								{
									((IDisposable)val4)?.Dispose();
								}
							}
							finally
							{
								((IDisposable)val3)?.Dispose();
							}
						}
						finally
						{
							((IDisposable)val2)?.Dispose();
						}
					}
					if (e.DrawBorderFormatting && stiSubReport.HighlightState == StiHighlightState.Hide && (int)stiSubReport.Border.Side != 15)
					{
						PaintMarkers(stiSubReport, graphics, paintRectangle);
					}
					if ((int)stiSubReport.Border.Side == 0 && stiSubReport.IsDesigning && e.DrawBorderFormatting)
					{
						Pen val5 = new Pen(Color.FromArgb(128, 128, 128));
						try
						{
							val5.DashStyle = (DashStyle)1;
							StiDrawing.DrawRectangle(graphics, val5, ((RectangleD)(ref paintRectangle)).Left, ((RectangleD)(ref paintRectangle)).Top, ((RectangleD)(ref paintRectangle)).Width, ((RectangleD)(ref paintRectangle)).Height);
						}
						finally
						{
							((IDisposable)val5)?.Dispose();
						}
					}
					if (stiSubReport.HighlightState == StiHighlightState.Hide)
					{
						PaintBorder(stiSubReport, graphics, paintRectangle, stiSubReport.Page.Zoom, e.DrawBorderFormatting, e.DrawTopmostBorderSides || !stiSubReport.Border.Topmost);
					}
					if (e.DrawBorderFormatting)
					{
						PaintEvents(stiSubReport, graphics, paintRectangle);
						PaintConditions(stiSubReport, graphics, paintRectangle);
					}
				}
			}
		}
		e.Cancel = false;
		if (e.DrawBorderFormatting)
		{
			stiSubReport.InvokePainted(stiSubReport, e);
			PaintColumns(stiSubReport, e.Graphics);
		}
		base.PaintComponents(stiSubReport, e);
	}
}
