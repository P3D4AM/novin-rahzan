using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.MathFormula;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Report.Painters;

public class StiMathFormulaGdiPainter : StiComponentGdiPainter
{
	public override Image GetImage(StiComponent component, ref float zoom, StiExportFormat format)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Expected O, but got Unknown
		StiMathFormula stiMathFormula = (StiMathFormula)component;
		double zoom2 = stiMathFormula.Report.Info.Zoom;
		stiMathFormula.Report.Info.Zoom = zoom;
		RectangleD val = component.ComponentToPage(component.ClientRectangle);
		RectangleD rect = ((RectangleD)(ref val)).Normalize();
		val = stiMathFormula.Report.Unit.ConvertToHInches(rect);
		rect = ((RectangleD)(ref val)).Multiply((double)zoom);
		((RectangleD)(ref rect)).X = 0.0;
		((RectangleD)(ref rect)).Y = 0.0;
		int num = (int)((RectangleD)(ref rect)).Width + 2;
		int num2 = (int)((RectangleD)(ref rect)).Height + 2;
		Bitmap val2 = new Bitmap(num, num2);
		Graphics val3 = Graphics.FromImage((Image)(object)val2);
		try
		{
			val3.PageUnit = (GraphicsUnit)2;
			if (format == StiExportFormat.ImagePng)
			{
				val3.Clear(Color.Transparent);
			}
			else
			{
				val3.Clear(Color.White);
			}
			Draw(stiMathFormula, val3, ((RectangleD)(ref rect)).ToRectangleF());
		}
		finally
		{
			((IDisposable)val3)?.Dispose();
		}
		stiMathFormula.Report.Info.Zoom = zoom2;
		return (Image)(object)val2;
	}

	public override void Paint(StiComponent component, StiPaintEventArgs e)
	{
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		if (!(e.Context is Graphics))
		{
			throw new Exception("StiGdiPainter can work only with System.Drawing.Graphics context!");
		}
		if (!e.DrawBorderFormatting && e.DrawTopmostBorderSides)
		{
			return;
		}
		StiMathFormula stiMathFormula = (StiMathFormula)component;
		if (e.DrawBorderFormatting)
		{
			stiMathFormula.InvokePainting(stiMathFormula, e);
		}
		if (!e.Cancel && (stiMathFormula.Enabled || stiMathFormula.IsDesigning))
		{
			Graphics graphics = e.Graphics;
			RectangleD paintRectangle = stiMathFormula.GetPaintRectangle();
			if (((RectangleD)(ref paintRectangle)).Width > 0.0 && ((RectangleD)(ref paintRectangle)).Height > 0.0)
			{
				RectangleD clipRectangle = e.ClipRectangle;
				if (((RectangleD)(ref clipRectangle)).IsEmpty || ((RectangleD)(ref paintRectangle)).IntersectsWith(e.ClipRectangle))
				{
					if (e.DrawBorderFormatting)
					{
						PaintBackground(stiMathFormula, graphics, paintRectangle);
					}
					if (e.DrawBorderFormatting)
					{
						Draw(stiMathFormula, graphics, ((RectangleD)(ref paintRectangle)).ToRectangleF());
					}
					if (e.DrawBorderFormatting)
					{
						PaintMarkers(stiMathFormula, graphics, paintRectangle);
					}
					PaintBorder(stiMathFormula, graphics, paintRectangle, (float)stiMathFormula.Page.Zoom, e.DrawBorderFormatting, e.DrawTopmostBorderSides || !stiMathFormula.Border.Topmost);
					if (e.DrawBorderFormatting)
					{
						PaintEvents(stiMathFormula, e.Graphics, paintRectangle);
					}
				}
			}
		}
		e.Cancel = false;
		if (e.DrawBorderFormatting)
		{
			stiMathFormula.InvokePainted(stiMathFormula, e);
		}
	}

	public virtual void PaintBackground(StiMathFormula mathFormul, Graphics g, RectangleD rect)
	{
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		if (mathFormul.Brush is StiSolidBrush && ((StiSolidBrush)mathFormul.Brush).Color.A == 0 && mathFormul.Report.Info.FillComponent && mathFormul.IsDesigning)
		{
			Color color = Color.FromArgb(150, Color.White);
			StiDrawing.FillRectangle(g, color, ((RectangleD)(ref rect)).Left, ((RectangleD)(ref rect)).Top, ((RectangleD)(ref rect)).Width, ((RectangleD)(ref rect)).Height);
		}
		else
		{
			StiDrawing.FillRectangle(g, mathFormul.Brush, rect);
		}
	}

	private void Draw(StiMathFormula mathFormula, Graphics g, RectangleF rect)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		if (StiMathHelper.MathAssembly == null)
		{
			Font val = new Font("Arial", 8f, (FontStyle)1);
			try
			{
				object obj = StringFormat.GenericDefault.Clone();
				StringFormat val2 = (StringFormat)((obj is StringFormat) ? obj : null);
				try
				{
					Pen val3 = new Pen(Color.Gray);
					try
					{
						val3.DashStyle = (DashStyle)1;
						val2.Alignment = (StringAlignment)1;
						val2.LineAlignment = (StringAlignment)1;
						GraphicsState val4 = g.Save();
						g.SetClip(rect, (CombineMode)1);
						g.TranslateTransform((float)(int)rect.X, (float)(int)rect.Y);
						g.Clear(Color.White);
						g.DrawString(string.Format(StiLocalization.Get("Notices", "IsNotFound"), StiMathHelper.NameAssembly), val, Brushes.Gray, new RectangleF(0f, 0f, rect.Width, rect.Height), val2);
						g.Restore(val4);
						return;
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
		string value = ((mathFormula.Report != null && mathFormula.Report.IsDesigning) ? mathFormula.LaTexExpression : mathFormula.Value);
		if (string.IsNullOrEmpty(value))
		{
			PaintNoDefinedStatus(g, rect, mathFormula, "StiMathFormula", Loc.GetMain("NoElements"));
		}
		else
		{
			DrawMath(mathFormula, g, rect);
		}
	}

	private static void DrawMath(StiMathFormula mathFormula, Graphics g, RectangleF rect)
	{
		Bitmap image = StiMathFormulaCache.GetImage(mathFormula);
		bool flag = mathFormula.IsExporting && (image == null || (int)rect.Width != ((Image)image).Width);
		if (image != null && !flag)
		{
			g.DrawImage((Image)(object)image, rect.X, rect.Y);
			return;
		}
		try
		{
			string text = ((mathFormula.Report != null && mathFormula.Report.IsDesigning) ? mathFormula.LaTexExpression : mathFormula.Value);
			if (!string.IsNullOrEmpty(text))
			{
				double zoom = mathFormula.Report.Info.Zoom;
				float num = 1f;
				num = ((!string.IsNullOrEmpty(StiSvgHelper.SvgAssemblyPrefix)) ? (flag ? 1f : (1f / (float)StiDpiHelper.GraphicsScale)) : (flag ? ((float)StiDpiHelper.GraphicsScale) : 1f));
				float fontSize = mathFormula.Font.Size * (float)zoom * num;
				string colorHex = StiColorFXHelper.HexConverter(StiBrush.ToColor(mathFormula.TextBrush));
				string svgText = StiMathFormulaHelper.GetSvgText(text, fontSize, colorHex);
				RectangleF svgRect = StiMathFormulaHelper.GetSvgRect(mathFormula, svgText, rect);
				StiSvgHelper.DrawWithSvg(svgText, rect, svgRect, mathFormula.Report.Info.Zoom, g);
				if (!flag)
				{
					StiMathFormulaCache.SetImage(mathFormula, svgText, rect);
				}
			}
		}
		catch
		{
		}
	}
}
