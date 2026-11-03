using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Painters;

public class StiTableOfContentsGdiPainter : StiBandGdiPainter
{
	private string dotsString;

	private string DotsString
	{
		get
		{
			if (dotsString == null)
			{
				dotsString = new string('.', 300);
			}
			return dotsString;
		}
	}

	public override void PaintBandContent(StiBand band, Graphics g, RectangleD rect)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		StiTableOfContents stiTableOfContents = band as StiTableOfContents;
		if (stiTableOfContents.IsFirstInReport || !stiTableOfContents.Enabled)
		{
			PaintTableStyles(g, stiTableOfContents, rect);
		}
		else
		{
			PaintContainerName(g, stiTableOfContents, rect);
		}
	}

	private void PaintContainerName(Graphics g, StiTableOfContents table, RectangleD rect)
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Expected O, but got Unknown
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Expected O, but got Unknown
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		if (((RectangleD)(ref rect)).Width < 100.0 || ((RectangleD)(ref rect)).Height < 50.0 || table.Report.Info.IsComponentsMoving)
		{
			return;
		}
		float num = (float)(8.0 * table.Page.Zoom);
		StringFormat val = new StringFormat();
		try
		{
			Font val2 = new Font("Segoe UI", num);
			try
			{
				SolidBrush val3 = new SolidBrush(table.Enabled ? Color.FromArgb(102, 99, 99) : StiColor.Get("bfbfbf"));
				try
				{
					val.LineAlignment = (StringAlignment)1;
					val.Alignment = (StringAlignment)1;
					if (table.RightToLeft)
					{
						val.FormatFlags = (StringFormatFlags)(val.FormatFlags | 1);
					}
					string text = Loc.Get("Errors", "OneTableOfContentsAllowed");
					StiTextDrawing.DrawString(g, text, val2, (Brush)(object)val3, new RectangleD(((RectangleD)(ref rect)).Left, ((RectangleD)(ref rect)).Top, ((RectangleD)(ref rect)).Width, ((RectangleD)(ref rect)).Height), val);
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
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void PaintTableStyles(Graphics g, StiTableOfContents table, RectangleD rect)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		GraphicsState val = g.Save();
		g.SetClip(((RectangleD)(ref rect)).ToRectangleF());
		rect = ApplyMargins(table, rect);
		try
		{
			double num = ((RectangleD)(ref rect)).X;
			double num2 = ((RectangleD)(ref rect)).Y;
			double num3 = ((RectangleD)(ref rect)).Width;
			int num4 = 1;
			List<StiStyle> stylesList = table.GetStylesList();
			if (!stylesList.Any())
			{
				stylesList.Add(new StiStyle(Loc.GetMain("Heading") + "1"));
			}
			RectangleD rect2 = default(RectangleD);
			foreach (StiStyle item in stylesList)
			{
				bool flag = stylesList.IndexOf(item) == stylesList.Count - 1;
				int num5 = ((!flag) ? 1 : 3);
				for (int i = 0; i < num5; i++)
				{
					double num6 = (double)item.Font.GetHeight() * table.Page.Zoom * 1.3 * (double)item.LineSpacing;
					string name = string.Format("{0}{1}{2}", Loc.GetMain("Heading"), num4, DotsString);
					((RectangleD)(ref rect2))._002Ector(num, num2, num3, num6);
					PaintStyleItem(g, table, rect2, item, name, num4, (float)table.Page.Zoom);
					if (!flag)
					{
						if (!table.RightToLeft)
						{
							num += Indent(table);
						}
						num3 -= Indent(table);
					}
					num2 += num6;
					num4++;
					if (num2 > ((RectangleD)(ref rect)).Bottom)
					{
						break;
					}
				}
			}
		}
		finally
		{
			g.Restore(val);
		}
	}

	private void PaintStyleItem(Graphics g, StiTableOfContents table, RectangleD rect, StiStyle style, string name, int index, float scale)
	{
		//IL_0289: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Expected O, but got Unknown
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Expected O, but got Unknown
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_025b: Unknown result type (might be due to invalid IL or missing references)
		RectangleF rectangleF = ((RectangleD)(ref rect)).ToRectangleF();
		if (style.AllowUseBrush)
		{
			Brush brush = StiBrush.GetBrush(style.Brush, rect);
			try
			{
				StiDrawing.FillRectangle(g, brush, rect);
			}
			finally
			{
				((IDisposable)brush)?.Dispose();
			}
		}
		else
		{
			double width = ((RectangleD)(ref rect)).Width;
			((RectangleD)(ref rect)).Width = width + 1.0;
			width = ((RectangleD)(ref rect)).Height;
			((RectangleD)(ref rect)).Height = width + 1.0;
			StiDrawing.FillRectangle(g, Brushes.White, rect);
			width = ((RectangleD)(ref rect)).Width;
			((RectangleD)(ref rect)).Width = width - 1.0;
			width = ((RectangleD)(ref rect)).Height;
			((RectangleD)(ref rect)).Height = width - 1.0;
		}
		StringFormat val = new StringFormat();
		try
		{
			Brush brush2 = StiBrush.GetBrush(style.TextBrush, rect);
			try
			{
				val.Trimming = (StringTrimming)3;
				val.LineAlignment = (StringAlignment)1;
				val.FormatFlags = (StringFormatFlags)4096;
				val.Trimming = (StringTrimming)0;
				if (table.RightToLeft)
				{
					val.FormatFlags = (StringFormatFlags)(val.FormatFlags | 1);
				}
				Brush val2 = brush2;
				if (!style.AllowUseTextBrush)
				{
					val2 = Brushes.Black;
				}
				SmoothingMode smoothingMode = g.SmoothingMode;
				try
				{
					g.SmoothingMode = (SmoothingMode)4;
					if (style.AllowUseFont)
					{
						Font val3 = (style.AllowUseFont ? new Font(style.Font.Name, style.Font.Size * scale, style.Font.Style) : new Font("Arial", 8f * scale));
						try
						{
							SizeF sizeF = g.MeasureString(index.ToString(), val3);
							RectangleD val4 = (table.RightToLeft ? new RectangleD(((RectangleD)(ref rect)).X, ((RectangleD)(ref rect)).Top, (double)sizeF.Width, ((RectangleD)(ref rect)).Height) : new RectangleD(((RectangleD)(ref rect)).Right - (double)sizeF.Width, ((RectangleD)(ref rect)).Top, (double)sizeF.Width, ((RectangleD)(ref rect)).Height));
							val.Alignment = (StringAlignment)2;
							g.DrawString(index.ToString(), val3, val2, ((RectangleD)(ref val4)).ToRectangleF(), val);
							RectangleD val5 = (table.RightToLeft ? new RectangleD(((RectangleD)(ref rect)).Left + (double)sizeF.Width, ((RectangleD)(ref rect)).Top, ((RectangleD)(ref rect)).Width - (double)sizeF.Width, ((RectangleD)(ref rect)).Height) : new RectangleD(((RectangleD)(ref rect)).Left, ((RectangleD)(ref rect)).Top, ((RectangleD)(ref rect)).Width - (double)sizeF.Width, ((RectangleD)(ref rect)).Height));
							val.Alignment = (StringAlignment)0;
							g.DrawString(name, val3, val2, ((RectangleD)(ref val5)).ToRectangleF(), val);
						}
						finally
						{
							((IDisposable)val3)?.Dispose();
						}
					}
				}
				finally
				{
					g.SmoothingMode = smoothingMode;
				}
			}
			finally
			{
				((IDisposable)brush2)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		if ((style.Border != null && style.AllowUseBorderFormatting) || style.AllowUseBorderSides)
		{
			style.Border.Draw(g, rectangleF, 1f, Color.White, style.AllowUseBorderFormatting, style.AllowUseBorderSides);
		}
	}

	private double Indent(StiTableOfContents table)
	{
		return table.Report.Unit.ConvertFromHInches((double)table.Indent) * table.Page.Zoom * StiScale.Factor;
	}

	private static RectangleD ApplyMargins(StiTableOfContents table, RectangleD rect)
	{
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		StiMargins margins = table.Margins;
		((RectangleD)(ref rect)).X = ((RectangleD)(ref rect)).X + margins.Left * table.Page.Zoom;
		((RectangleD)(ref rect)).Width = ((RectangleD)(ref rect)).Width - margins.Left * table.Page.Zoom;
		((RectangleD)(ref rect)).Y = ((RectangleD)(ref rect)).Y + margins.Top * table.Page.Zoom;
		((RectangleD)(ref rect)).Height = ((RectangleD)(ref rect)).Height - margins.Top * table.Page.Zoom;
		((RectangleD)(ref rect)).Width = ((RectangleD)(ref rect)).Width - margins.Right * table.Page.Zoom;
		((RectangleD)(ref rect)).Height = ((RectangleD)(ref rect)).Height - margins.Bottom * table.Page.Zoom;
		return rect;
	}
}
