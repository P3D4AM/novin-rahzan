using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Painters;

public class StiBandGdiPainter : StiContainerGdiPainter
{
	public override void PaintColumns(StiContainer container, Graphics g)
	{
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Expected O, but got Unknown
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		StiDataBand stiDataBand = container as StiDataBand;
		if (stiDataBand == null)
		{
			if (container is StiColumnHeaderBand)
			{
				stiDataBand = StiHeaderBandV1Builder.GetMaster(container as StiColumnHeaderBand) as StiDataBand;
			}
			if (container is StiColumnFooterBand)
			{
				stiDataBand = StiFooterBandV1Builder.GetMaster(container as StiColumnFooterBand) as StiDataBand;
			}
		}
		if (!container.IsDesigning || stiDataBand == null || stiDataBand.Columns < 2)
		{
			return;
		}
		double zoom = stiDataBand.Page.Zoom;
		RectangleD paintRectangle = stiDataBand.GetPaintRectangle();
		double num = stiDataBand.Page.Unit.ConvertToHInches(stiDataBand.GetColumnWidth()) * zoom * StiScale.Factor;
		double num2 = stiDataBand.Page.Unit.ConvertToHInches(stiDataBand.ColumnGaps) * zoom * StiScale.Factor;
		((RectangleD)(ref paintRectangle)).X = ((RectangleD)(ref paintRectangle)).X + (num + num2);
		StiComponentsCollection components = stiDataBand.GetComponents();
		foreach (StiComponent item in components)
		{
			RectangleD paintRectangle2 = item.GetPaintRectangle();
			if (!(((RectangleD)(ref paintRectangle2)).Width > 0.0) || !(((RectangleD)(ref paintRectangle2)).Height > 0.0))
			{
				continue;
			}
			Pen val = new Pen(Color.Blue, 1f);
			try
			{
				val.DashStyle = (DashStyle)1;
				for (int i = 0; i < stiDataBand.Columns - 1; i++)
				{
					((RectangleD)(ref paintRectangle2)).X = ((RectangleD)(ref paintRectangle2)).X + (num + num2);
					StiDrawing.FillRectangle(g, Color.FromArgb(20, Color.Blue), paintRectangle2);
					StiDrawing.DrawRectangle(g, val, paintRectangle2);
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
	}

	public override void PaintSelection(StiComponent component, StiPaintEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		if (component.IsDesigning && component.IsSelected && !component.Report.Info.IsComponentsMoving)
		{
			Graphics graphics = e.Graphics;
			RectangleD paintRectangle = component.GetPaintRectangle();
			RectangleF rectangleF = ((RectangleD)(ref paintRectangle)).ToRectangleF();
			int num = (component.Linked ? StiScale.I3 : StiScale.I2);
			DrawNoSelectedPoint(graphics, num, rectangleF.Left, rectangleF.Top);
			DrawNoSelectedPoint(graphics, num, rectangleF.Right, rectangleF.Top);
			DrawNoSelectedPoint(graphics, num, rectangleF.Left, rectangleF.Bottom);
			DrawNoSelectedPoint(graphics, num, rectangleF.Right, rectangleF.Bottom);
			if (component.IsCross)
			{
				DrawSelectedPoint(graphics, num, rectangleF.Right - (float)(num / 2), rectangleF.Top + rectangleF.Height / 2f - (float)(num / 2));
			}
			else if (StiOptions.Engine.DockPageFooterToBottom && component is StiPageFooterBand)
			{
				DrawSelectedPoint(graphics, num, rectangleF.X + rectangleF.Width / 2f - (float)(num / 2), rectangleF.Top - (float)(num / 2));
			}
			else
			{
				DrawSelectedPoint(graphics, num, rectangleF.X + rectangleF.Width / 2f - (float)(num / 2), rectangleF.Bottom - (float)(num / 2));
			}
		}
	}

	private void DrawNoSelectedPoint(Graphics g, float size, float x, float y)
	{
		RectangleF rectangleF = new RectangleF(x - size, y - size, size * 2f + 1f, size * 2f + 1f);
		g.FillRectangle(Brushes.White, rectangleF);
		g.DrawRectangle(Pens.DimGray, rectangleF.X, rectangleF.Y, rectangleF.Width, rectangleF.Height);
	}

	private void DrawSelectedPoint(Graphics g, float size, float x, float y)
	{
		RectangleF rectangleF = new RectangleF(x - size, y - size, size * 2f + 1f, size * 2f + 1f);
		g.FillRectangle(Brushes.DimGray, rectangleF);
		g.DrawRectangle(Pens.DimGray, rectangleF.X, rectangleF.Y, rectangleF.Width, rectangleF.Height);
	}

	public override void Paint(StiComponent component, StiPaintEventArgs e)
	{
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_0218: Expected O, but got Unknown
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_029e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
		StiBand stiBand = component as StiBand;
		if (!e.DrawBorderFormatting && e.DrawTopmostBorderSides && !stiBand.Border.Topmost)
		{
			return;
		}
		if (!(e.Context is Graphics))
		{
			throw new Exception("StiGdiPainter can work only with System.Drawing.Graphics context!");
		}
		if (e.DrawBorderFormatting)
		{
			stiBand.InvokePainting(stiBand, e);
		}
		Graphics graphics = e.Graphics;
		double num = stiBand.Page.Zoom * StiScale.Y;
		if (!e.Cancel && stiBand.IsDesigning)
		{
			RectangleD paintRectangle = stiBand.GetPaintRectangle();
			RectangleD val = default(RectangleD);
			((RectangleD)(ref val))._002Ector(((RectangleD)(ref paintRectangle)).Left, ((RectangleD)(ref paintRectangle)).Top - stiBand.HeaderSize * num, ((RectangleD)(ref paintRectangle)).Width, stiBand.HeaderSize * num);
			RectangleD fullRect = val;
			((RectangleD)(ref fullRect)).Height = ((RectangleD)(ref fullRect)).Height + ((RectangleD)(ref paintRectangle)).Height;
			if (((RectangleD)(ref fullRect)).Width > 0.0 && ((RectangleD)(ref fullRect)).Height > 0.0)
			{
				RectangleD clipRectangle = e.ClipRectangle;
				if (((RectangleD)(ref clipRectangle)).IsEmpty || ((RectangleD)(ref fullRect)).IntersectsWith(e.ClipRectangle))
				{
					if (e.DrawBorderFormatting)
					{
						if (stiBand.Brush is StiSolidBrush && ((StiSolidBrush)stiBand.Brush).Color == Color.Transparent && stiBand.Report.Info.FillBands && stiBand.IsDesigning)
						{
							Color fillColor = stiBand.Report.Info.GetFillColor(stiBand.HeaderEndColor);
							StiDrawing.FillRectangle(graphics, fillColor, ((RectangleD)(ref paintRectangle)).Left, ((RectangleD)(ref paintRectangle)).Top, ((RectangleD)(ref paintRectangle)).Width, ((RectangleD)(ref paintRectangle)).Height);
						}
						else
						{
							StiDrawing.FillRectangle(graphics, stiBand.Brush, paintRectangle);
						}
						PaintBandContent(stiBand, graphics, paintRectangle);
					}
					if (stiBand.Report.Info.ShowHeaders && e.DrawBorderFormatting)
					{
						PaintBandHeader(stiBand, graphics, paintRectangle, val);
					}
					if ((int)stiBand.Border.Side == 0 && stiBand.IsDesigning)
					{
						if (e.DrawBorderFormatting)
						{
							Color color = Color.FromArgb(220, stiBand.HeaderStartColor);
							Pen val2 = new Pen(color);
							try
							{
								val2.DashStyle = (DashStyle)1;
								StiDrawing.DrawRectangle(graphics, val2, ((RectangleD)(ref paintRectangle)).Left, ((RectangleD)(ref paintRectangle)).Top, ((RectangleD)(ref paintRectangle)).Width, ((RectangleD)(ref paintRectangle)).Height);
							}
							finally
							{
								((IDisposable)val2)?.Dispose();
							}
						}
					}
					else if (stiBand.HighlightState == StiHighlightState.Hide)
					{
						PaintBorder(stiBand, graphics, paintRectangle, num, e.DrawBorderFormatting, e.DrawTopmostBorderSides || !stiBand.Border.Topmost);
					}
					if (stiBand.IsDesigning && e.DrawBorderFormatting)
					{
						PaintColumns(stiBand, graphics);
						PaintDataBandColumns(stiBand, graphics, paintRectangle);
						PaintSpecialDockService(stiBand, graphics, val, fullRect);
					}
					if (e.DrawBorderFormatting)
					{
						PaintEvents(stiBand, graphics, paintRectangle);
						PaintConditions(stiBand, graphics, paintRectangle);
					}
				}
			}
		}
		e.Cancel = false;
		if (e.DrawBorderFormatting)
		{
			stiBand.InvokePainted(stiBand, e);
			PaintComponents(stiBand, e);
			PaintQuickButtons(stiBand, e.Graphics);
		}
	}

	public virtual void PaintBandContent(StiBand band, Graphics g, RectangleD rect)
	{
	}

	public virtual void PaintBandHeader(StiBand band, Graphics g, RectangleD rect, RectangleD headerRect)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected O, but got Unknown
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		double num = band.Page.Zoom * StiScale.Factor;
		Color color = Color.FromArgb(220, band.HeaderStartColor);
		SolidBrush val = new SolidBrush(color);
		try
		{
			g.FillRectangle((Brush)(object)val, ((RectangleD)(ref headerRect)).ToRectangleF());
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		StringFormat val2 = new StringFormat();
		try
		{
			Font val3 = new Font("Segoe UI", (float)(9.0 * num / StiScale.Y));
			try
			{
				val2.LineAlignment = (StringAlignment)1;
				val2.Alignment = (StringAlignment)0;
				val2.Trimming = (StringTrimming)3;
				val2.FormatFlags = (StringFormatFlags)4096;
				StiTextDrawing.DrawString(g, band.GetHeaderText(), val3, Brushes.Black, headerRect, val2);
				PaintDataBandHeader(band, g, rect, headerRect, val2, val3);
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

	public virtual void PaintDataBandHeader(StiBand band, Graphics g, RectangleD rect, RectangleD headerRect, StringFormat stringFormat, Font font)
	{
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		if (band is StiDataBand stiDataBand)
		{
			float width = g.MeasureString(band.GetHeaderText(), font, new SizeF((float)((RectangleD)(ref rect)).Width, (float)((RectangleD)(ref rect)).Height)).Width;
			if (stiDataBand.MasterComponent != null && !(width <= 0f))
			{
				stringFormat.LineAlignment = (StringAlignment)1;
				stringFormat.Alignment = (StringAlignment)2;
				stringFormat.Trimming = (StringTrimming)3;
				stringFormat.FormatFlags = (StringFormatFlags)4096;
				RectangleD val = default(RectangleD);
				((RectangleD)(ref val))._002Ector(((RectangleD)(ref headerRect)).Left + (double)width, ((RectangleD)(ref headerRect)).Top, ((RectangleD)(ref headerRect)).Width - (double)width, ((RectangleD)(ref headerRect)).Height);
				StiTextDrawing.DrawString(g, Loc.GetMain("MasterComponent") + ": " + stiDataBand.MasterComponent.Name, font, Brushes.Black, val, stringFormat);
			}
		}
	}

	public virtual void PaintDataBandColumns(StiBand band, Graphics g, RectangleD rect)
	{
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Expected O, but got Unknown
		StiDataBand stiDataBand = band as StiDataBand;
		if (stiDataBand == null)
		{
			if (band is StiColumnHeaderBand)
			{
				stiDataBand = StiHeaderBandV1Builder.GetMaster(band as StiHeaderBand) as StiDataBand;
			}
			if (band is StiColumnFooterBand)
			{
				stiDataBand = StiFooterBandV1Builder.GetMaster(band as StiFooterBand) as StiDataBand;
			}
		}
		if (stiDataBand == null || stiDataBand.Columns <= 1)
		{
			return;
		}
		double num = stiDataBand.Page.Zoom * StiScale.Factor;
		double num2 = stiDataBand.Page.Unit.ConvertToHInches(stiDataBand.GetColumnWidth()) * num;
		double num3 = stiDataBand.Page.Unit.ConvertToHInches(stiDataBand.ColumnGaps) * num;
		double num4 = num2 + ((RectangleD)(ref rect)).Left;
		Pen val = new Pen(Color.Red);
		try
		{
			val.DashStyle = (DashStyle)1;
			for (int i = 1; i < stiDataBand.Columns; i++)
			{
				g.DrawLine(val, (float)num4, (float)((RectangleD)(ref rect)).Top, (float)num4, (float)(((RectangleD)(ref rect)).Top + ((RectangleD)(ref rect)).Height));
				g.DrawLine(val, (float)(num4 + num3), (float)((RectangleD)(ref rect)).Top, (float)(num4 + num3), (float)(((RectangleD)(ref rect)).Top + ((RectangleD)(ref rect)).Height));
				num4 += num2 + num3;
			}
			g.DrawLine(val, (float)num4, (float)((RectangleD)(ref rect)).Top, (float)num4, (float)(((RectangleD)(ref rect)).Top + ((RectangleD)(ref rect)).Height));
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public virtual void PaintSpecialDockService(StiBand band, Graphics g, RectangleD headerRect, RectangleD fullRect)
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Expected O, but got Unknown
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		if (!band.RectangleMoveComponent.HasValue)
		{
			return;
		}
		double num = band.Page.Zoom * StiScale.Factor;
		RectangleD val = band.Page.Unit.ConvertToHInches(band.RectangleMoveComponent.Value);
		RectangleD val2 = ((RectangleD)(ref val)).Multiply(num);
		((RectangleD)(ref val2)).X = ((RectangleD)(ref val2)).X + ((RectangleD)(ref fullRect)).Left;
		((RectangleD)(ref val2)).Y = ((RectangleD)(ref val2)).Y + ((RectangleD)(ref fullRect)).Top;
		if (band.Report.Info.ShowHeaders)
		{
			((RectangleD)(ref val2)).Y = ((RectangleD)(ref val2)).Y + ((RectangleD)(ref headerRect)).Height;
		}
		SolidBrush val3 = new SolidBrush(Color.FromArgb(100, Color.Red));
		try
		{
			StiDrawing.FillRectangle(g, (Brush)(object)val3, val2);
		}
		finally
		{
			((IDisposable)val3)?.Dispose();
		}
	}
}
