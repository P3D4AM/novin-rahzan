using System;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Dialogs;
using Stimulsoft.Report.Units;

namespace Stimulsoft.Report.Components;

public class StiDimensionLinesHelper
{
	public static bool IsAllowLocationDimensionLine(StiAction action)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected I4, but got Unknown
		switch (action - 1)
		{
		case 0:
		case 2:
		case 4:
		case 6:
		case 7:
		case 8:
			return true;
		default:
			return false;
		}
	}

	private static string GetUnitValueString(double value, StiPage page)
	{
		if ((page.Unit is StiCentimetersUnit || page.Unit is StiInchesUnit) && value >= 0.2 && !(page is IStiForm))
		{
			return ((decimal)value).ToString("F");
		}
		return ((int)Math.Round(value, 0)).ToString();
	}

	private static void DrawLeftDimensionLine(Graphics g, StiPage page, Font font, RectangleF rect, double value)
	{
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Expected O, but got Unknown
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Expected O, but got Unknown
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		if (double.IsNaN(rect.X) || double.IsNaN(rect.Y) || double.IsNaN(rect.Width) || double.IsNaN(rect.Height) || rect.Height < (float)StiScale.I10)
		{
			return;
		}
		string unitValueString = GetUnitValueString(value, page);
		float num = rect.X + rect.Width / 2f;
		float num2 = rect.Y + rect.Height / 2f;
		SizeF sizeF = g.MeasureString(unitValueString, font);
		sizeF.Width += StiScale.I2;
		sizeF = new SizeF(sizeF.Height, sizeF.Width);
		g.DrawLine(Pens.Blue, rect.X + (float)StiScale.I5, rect.Y, rect.Right - (float)StiScale.I3, rect.Y);
		g.DrawLine(Pens.Blue, rect.X + (float)StiScale.I5, rect.Bottom, rect.Right - (float)StiScale.I3, rect.Bottom);
		RectangleF rectangleF = new RectangleF(num - sizeF.Width / 2f, num2 - sizeF.Height / 2f, sizeF.Width, sizeF.Height);
		if ((double)rect.Height > (double)sizeF.Height * 1.1)
		{
			g.DrawLine(Pens.Blue, num, rect.Y, num, num2 - sizeF.Height / 2f);
			g.DrawLine(Pens.Blue, num, num2 + sizeF.Height / 2f, num, rect.Bottom);
		}
		else
		{
			g.DrawLine(Pens.Blue, num, rect.Y, num, rect.Bottom);
			rectangleF.X -= rect.Width / 2f;
		}
		Brush val = (Brush)new SolidBrush(Color.FromArgb(160, Color.White));
		try
		{
			g.FillRectangle(val, rectangleF);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		StringFormat val2 = new StringFormat();
		try
		{
			val2.Alignment = (StringAlignment)1;
			val2.LineAlignment = (StringAlignment)1;
			sizeF.Height *= 2f;
			StiTextDrawing.DrawString(g, unitValueString, font, Brushes.Blue, RectangleD.CreateFromRectangle(rectangleF), val2, 270f, 1.0);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	private static void DrawRightDimensionLine(Graphics g, StiPage page, Font font, RectangleF rect, double value)
	{
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Expected O, but got Unknown
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Expected O, but got Unknown
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		if (double.IsNaN(rect.X) || double.IsNaN(rect.Y) || double.IsNaN(rect.Width) || double.IsNaN(rect.Height) || rect.Height < (float)StiScale.I10)
		{
			return;
		}
		string unitValueString = GetUnitValueString(value, page);
		float num = rect.X + rect.Width / 2f;
		float num2 = rect.Y + rect.Height / 2f;
		SizeF sizeF = g.MeasureString(unitValueString, font);
		sizeF.Width += StiScale.I2;
		sizeF = new SizeF(sizeF.Height, sizeF.Width);
		g.DrawLine(Pens.Blue, rect.X + (float)StiScale.I3, rect.Y, rect.Right - (float)StiScale.I5, rect.Y);
		g.DrawLine(Pens.Blue, rect.X + (float)StiScale.I3, rect.Bottom, rect.Right - (float)StiScale.I5, rect.Bottom);
		RectangleF rectangleF = new RectangleF(num - sizeF.Width / 2f, num2 - sizeF.Height / 2f, sizeF.Width, sizeF.Height);
		if ((double)rect.Height > (double)sizeF.Height * 1.1)
		{
			g.DrawLine(Pens.Blue, num, rect.Y, num, num2 - sizeF.Height / 2f);
			g.DrawLine(Pens.Blue, num, num2 + sizeF.Height / 2f, num, rect.Bottom);
		}
		else
		{
			g.DrawLine(Pens.Blue, num, rect.Y, num, rect.Bottom);
			rectangleF.X += rect.Width / 2f;
		}
		SolidBrush val = new SolidBrush(Color.FromArgb(160, Color.White));
		try
		{
			g.FillRectangle((Brush)(object)val, rectangleF);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		StringFormat val2 = new StringFormat();
		try
		{
			val2.Alignment = (StringAlignment)1;
			val2.LineAlignment = (StringAlignment)1;
			sizeF.Height *= 2f;
			StiTextDrawing.DrawString(g, unitValueString, font, Brushes.Blue, RectangleD.CreateFromRectangle(rectangleF), val2, 90f, 1.0);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	private static void DrawTopDimensionLine(Graphics g, StiPage page, Font font, RectangleF rect, double value)
	{
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Expected O, but got Unknown
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Expected O, but got Unknown
		if (double.IsNaN(rect.X) || double.IsNaN(rect.Y) || double.IsNaN(rect.Width) || double.IsNaN(rect.Height) || rect.Width < (float)StiScale.I10)
		{
			return;
		}
		string unitValueString = GetUnitValueString(value, page);
		float num = rect.X + rect.Width / 2f;
		float num2 = rect.Y + rect.Height / 2f;
		SizeF sizeF = g.MeasureString(unitValueString, font);
		sizeF.Width += StiScale.I2;
		g.DrawLine(Pens.Blue, rect.X, rect.Y + (float)StiScale.I5, rect.X, rect.Bottom - (float)StiScale.I3);
		g.DrawLine(Pens.Blue, rect.Right, rect.Y + (float)StiScale.I5, rect.Right, rect.Bottom - (float)StiScale.I3);
		RectangleF rectangleF = new RectangleF(num - sizeF.Width / 2f, num2 - sizeF.Height / 2f, sizeF.Width, sizeF.Height);
		if ((double)rect.Width > (double)sizeF.Width * 1.1)
		{
			g.DrawLine(Pens.Blue, rect.X, num2, num - sizeF.Width / 2f, num2);
			g.DrawLine(Pens.Blue, num + sizeF.Width / 2f, num2, rect.Right, num2);
		}
		else
		{
			g.DrawLine(Pens.Blue, rect.X, num2, rect.Right, num2);
			rectangleF.Y -= rect.Height / 2f;
		}
		SolidBrush val = new SolidBrush(Color.FromArgb(160, Color.White));
		try
		{
			g.FillRectangle((Brush)(object)val, rectangleF);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		StringFormat val2 = new StringFormat();
		try
		{
			val2.Alignment = (StringAlignment)1;
			val2.LineAlignment = (StringAlignment)1;
			sizeF.Width *= 2f;
			g.DrawString(unitValueString, font, Brushes.Blue, rectangleF, val2);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	private static void DrawBottomDimensionLine(Graphics g, StiPage page, Font font, RectangleF rect, double value)
	{
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Expected O, but got Unknown
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Expected O, but got Unknown
		if (double.IsNaN(rect.X) || double.IsNaN(rect.Y) || double.IsNaN(rect.Width) || double.IsNaN(rect.Height) || rect.Width < (float)StiScale.I10)
		{
			return;
		}
		string unitValueString = GetUnitValueString(value, page);
		float num = rect.X + rect.Width / 2f;
		float num2 = rect.Y + rect.Height / 2f;
		SizeF sizeF = g.MeasureString(unitValueString, font);
		sizeF.Width += StiScale.I2;
		g.DrawLine(Pens.Blue, rect.X, rect.Y + (float)StiScale.I3, rect.X, rect.Bottom - (float)StiScale.I5);
		g.DrawLine(Pens.Blue, rect.Right, rect.Y + (float)StiScale.I3, rect.Right, rect.Bottom - (float)StiScale.I5);
		RectangleF rectangleF = new RectangleF(num - sizeF.Width / 2f, num2 - sizeF.Height / 2f, sizeF.Width, sizeF.Height);
		if ((double)rect.Width > (double)sizeF.Width * 1.1)
		{
			g.DrawLine(Pens.Blue, rect.X, num2, num - sizeF.Width / 2f, num2);
			g.DrawLine(Pens.Blue, num + sizeF.Width / 2f, num2, rect.Right, num2);
		}
		else
		{
			g.DrawLine(Pens.Blue, rect.X, num2, rect.Right, num2);
			rectangleF.Y += rect.Height / 2f;
		}
		SolidBrush val = new SolidBrush(Color.FromArgb(160, Color.White));
		try
		{
			g.FillRectangle((Brush)(object)val, rectangleF);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		StringFormat val2 = new StringFormat();
		try
		{
			val2.Alignment = (StringAlignment)1;
			val2.LineAlignment = (StringAlignment)1;
			sizeF.Width *= 2f;
			g.DrawString(unitValueString, font, Brushes.Blue, rectangleF, val2);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	private static void DrawLocationDimensionLine(Graphics g, StiPage page, Font font, RectangleF rect, double valueX, double valueY)
	{
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Expected O, but got Unknown
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Expected O, but got Unknown
		if (!double.IsNaN(rect.X) && !double.IsNaN(rect.Y) && !double.IsNaN(rect.Width) && !double.IsNaN(rect.Height))
		{
			string unitValueString = GetUnitValueString(valueX, page);
			string unitValueString2 = GetUnitValueString(valueY, page);
			string text = unitValueString + ";" + unitValueString2;
			SizeF sizeF = g.MeasureString(text, font);
			sizeF.Width += StiScale.I2;
			RectangleF rectangleF = new RectangleF(rect.Right - (float)(int)Math.Ceiling(50.0 * StiScale.Factor), rect.Bottom - (float)(int)Math.Ceiling(16.0 * StiScale.Factor), (int)Math.Ceiling(50.0 * StiScale.Factor), (int)Math.Ceiling(16.0 * StiScale.Factor));
			SolidBrush val = new SolidBrush(Color.FromArgb(160, Color.White));
			try
			{
				g.FillRectangle((Brush)(object)val, rectangleF);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
			RectangleF rectangleF2 = new RectangleF(rect.Right - sizeF.Width, rect.Bottom - sizeF.Height, sizeF.Width, sizeF.Height);
			StringFormat val2 = new StringFormat();
			try
			{
				val2.Alignment = (StringAlignment)2;
				val2.LineAlignment = (StringAlignment)2;
				RectangleF clipBounds = g.ClipBounds;
				g.SetClip(rectangleF2);
				g.DrawString(text, font, Brushes.Blue, rectangleF2, val2);
				g.SetClip(clipBounds);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
			g.DrawLine(Pens.Blue, rect.Right - (float)StiScale.XXI(50.0), rect.Bottom, rect.Right - (float)StiScale.I2, rect.Bottom);
			g.DrawLine(Pens.Blue, rect.Right, rect.Bottom - (float)StiScale.I16, rect.Right, rect.Bottom - (float)StiScale.I2);
		}
	}

	public static void DrawDimensionLines(Graphics g, StiPage page)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0299: Unknown result type (might be due to invalid IL or missing references)
		//IL_029e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02da: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0301: Unknown result type (might be due to invalid IL or missing references)
		//IL_0306: Unknown result type (might be due to invalid IL or missing references)
		//IL_030e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0310: Unknown result type (might be due to invalid IL or missing references)
		//IL_0315: Unknown result type (might be due to invalid IL or missing references)
		//IL_032e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0333: Unknown result type (might be due to invalid IL or missing references)
		//IL_0335: Unknown result type (might be due to invalid IL or missing references)
		//IL_0338: Unknown result type (might be due to invalid IL or missing references)
		//IL_035e: Expected I4, but got Unknown
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_03aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b1: Expected O, but got Unknown
		//IL_042b: Unknown result type (might be due to invalid IL or missing references)
		bool flag = false;
		bool flag2 = false;
		RectangleD val = RectangleD.Empty;
		StiComponentsCollection selectedComponents = page.GetSelectedComponents();
		RectangleD val2 = RectangleD.Empty;
		foreach (StiComponent item in selectedComponents)
		{
			if (item is StiBand)
			{
				flag = true;
			}
			if (item.IsCross)
			{
				flag2 = true;
			}
			RectangleD paintRectangle = item.GetPaintRectangle(convertToHInches: false, convertByZoom: false);
			RectangleD clientRectangle = item.ClientRectangle;
			clientRectangle = StiComponent.DoOffsetRect(item, clientRectangle, item.Page.OffsetRectangle);
			val = ((!((RectangleD)(ref val)).IsEmpty) ? ((RectangleD)(ref val)).FitToRectangle(paintRectangle) : paintRectangle);
			val2 = ((!((RectangleD)(ref val2)).IsEmpty) ? ((RectangleD)(ref val2)).FitToRectangle(clientRectangle) : clientRectangle);
		}
		if (((RectangleD)(ref val)).IsEmpty || (((RectangleD)(ref val)).Width == 0.0 && ((RectangleD)(ref val)).Height == 0.0))
		{
			return;
		}
		double gridSize = page.GridSize;
		gridSize = Math.Min(gridSize, page.Unit.ConvertFromHInches(20.0));
		gridSize = Math.Max(gridSize, page.Unit.ConvertFromHInches(6.0));
		RectangleD val3 = val;
		RectangleD val4 = val;
		((RectangleD)(ref val3)).Inflate(gridSize * 2.0, 0.0);
		((RectangleD)(ref val4)).Inflate(0.0, gridSize * 2.0);
		double num = (((RectangleD)(ref val3)).Width - ((RectangleD)(ref val)).Width) / 2.0;
		double num2 = (((RectangleD)(ref val4)).Height - ((RectangleD)(ref val)).Height) / 2.0;
		RectangleD rect = default(RectangleD);
		((RectangleD)(ref rect))._002Ector(((RectangleD)(ref val3)).X, ((RectangleD)(ref val3)).Y, num, ((RectangleD)(ref val3)).Height);
		RectangleD rect2 = default(RectangleD);
		((RectangleD)(ref rect2))._002Ector(((RectangleD)(ref val3)).Right - num, ((RectangleD)(ref val3)).Y, num, ((RectangleD)(ref val3)).Height);
		RectangleD rect3 = default(RectangleD);
		((RectangleD)(ref rect3))._002Ector(((RectangleD)(ref val4)).X, ((RectangleD)(ref val4)).Y, ((RectangleD)(ref val4)).Width, num2);
		RectangleD rect4 = default(RectangleD);
		((RectangleD)(ref rect4))._002Ector(((RectangleD)(ref val4)).X, ((RectangleD)(ref val4)).Bottom - num2, ((RectangleD)(ref val4)).Width, num2);
		RectangleD rect5 = default(RectangleD);
		((RectangleD)(ref rect5))._002Ector(((RectangleD)(ref val)).X - gridSize * StiScale.XX(6.5), ((RectangleD)(ref val)).Y - gridSize * StiScale.YY(1.7), StiScale.XX(gridSize * 6.5), StiScale.YY(gridSize * 1.7));
		double num3 = page.Zoom * StiScale.Factor;
		rect = ((RectangleD)(ref rect)).Multiply(num3);
		rect = page.Unit.ConvertToHInches(rect);
		rect2 = ((RectangleD)(ref rect2)).Multiply(num3);
		rect2 = page.Unit.ConvertToHInches(rect2);
		rect3 = ((RectangleD)(ref rect3)).Multiply(num3);
		rect3 = page.Unit.ConvertToHInches(rect3);
		rect4 = ((RectangleD)(ref rect4)).Multiply(num3);
		rect4 = page.Unit.ConvertToHInches(rect4);
		rect5 = ((RectangleD)(ref rect5)).Multiply(num3);
		rect5 = page.Unit.ConvertToHInches(rect5);
		bool flag3 = false;
		bool flag4 = false;
		bool flag5 = false;
		bool flag6 = false;
		StiAction currentAction = page.Report.Info.CurrentAction;
		switch (currentAction - 3)
		{
		case 3:
			flag4 = true;
			break;
		case 0:
			flag6 = true;
			break;
		case 5:
			flag3 = true;
			flag6 = true;
			break;
		case 4:
			flag3 = true;
			flag5 = true;
			break;
		case 1:
			flag6 = true;
			break;
		case 7:
			flag4 = true;
			flag6 = true;
			break;
		case 6:
			flag5 = true;
			flag4 = true;
			break;
		case 2:
			flag4 = true;
			break;
		}
		if (flag2 && flag6)
		{
			flag5 = true;
			flag6 = false;
		}
		Font val5 = new Font("Arial", 7f, (FontStyle)1);
		try
		{
			if (flag3)
			{
				DrawLeftDimensionLine(g, page, val5, ((RectangleD)(ref rect)).ToRectangleF(), ((RectangleD)(ref val)).Height);
			}
			if (flag4)
			{
				DrawRightDimensionLine(g, page, val5, ((RectangleD)(ref rect2)).ToRectangleF(), ((RectangleD)(ref val)).Height);
			}
			if (flag5)
			{
				DrawTopDimensionLine(g, page, val5, ((RectangleD)(ref rect3)).ToRectangleF(), ((RectangleD)(ref val)).Width);
			}
			if (flag6)
			{
				DrawBottomDimensionLine(g, page, val5, ((RectangleD)(ref rect4)).ToRectangleF(), ((RectangleD)(ref val)).Width);
			}
			if (!flag && IsAllowLocationDimensionLine(page.Report.Info.CurrentAction))
			{
				DrawLocationDimensionLine(g, page, val5, ((RectangleD)(ref rect5)).ToRectangleF(), ((RectangleD)(ref val2)).Left, ((RectangleD)(ref val2)).Top);
			}
		}
		finally
		{
			((IDisposable)val5)?.Dispose();
		}
	}
}
