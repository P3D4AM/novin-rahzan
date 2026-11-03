using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Painters;

public class StiZipCodeGdiPainter : StiComponentGdiPainter
{
	private void DrawHorizLineUp(Graphics g, RectangleD drawRect, Pen pen)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		float num = pen.Width;
		if ((int)pen.DashStyle == 0)
		{
			num = pen.Width / 2f;
		}
		if (!StiOptions.Engine.UseOldZipCodePaintMode)
		{
			num = 0f;
		}
		float num2 = (float)(((RectangleD)(ref drawRect)).X + (double)num);
		float num3 = (float)(((RectangleD)(ref drawRect)).Y + (double)num);
		float num4 = (float)(((RectangleD)(ref drawRect)).Right - (double)num);
		float num5 = (float)(((RectangleD)(ref drawRect)).Y + (double)num);
		g.DrawLine(pen, num2, num3, num4, num5);
	}

	private void DrawHorizLineCenter(Graphics g, RectangleD drawRect, Pen pen)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		float num = pen.Width;
		if ((int)pen.DashStyle == 0)
		{
			num = pen.Width / 2f;
		}
		if (!StiOptions.Engine.UseOldZipCodePaintMode)
		{
			num = 0f;
		}
		float num2 = (float)(((RectangleD)(ref drawRect)).X + (double)num);
		float num3 = (float)(((RectangleD)(ref drawRect)).Y + ((RectangleD)(ref drawRect)).Height / 2.0);
		float num4 = (float)(((RectangleD)(ref drawRect)).Right - (double)num);
		float num5 = (float)(((RectangleD)(ref drawRect)).Y + ((RectangleD)(ref drawRect)).Height / 2.0);
		g.DrawLine(pen, num2, num3, num4, num5);
	}

	private void DrawHorizLineDown(Graphics g, RectangleD drawRect, Pen pen)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		float num = pen.Width;
		if ((int)pen.DashStyle == 0)
		{
			num = pen.Width / 2f;
		}
		if (!StiOptions.Engine.UseOldZipCodePaintMode)
		{
			num = 0f;
		}
		float num2 = (float)(((RectangleD)(ref drawRect)).X + (double)num);
		float num3 = (float)(((RectangleD)(ref drawRect)).Bottom - (double)num);
		float num4 = (float)(((RectangleD)(ref drawRect)).Right - (double)num);
		float num5 = (float)(((RectangleD)(ref drawRect)).Bottom - (double)num);
		g.DrawLine(pen, num2, num3, num4, num5);
	}

	private void DrawVerticalLineUpRight(Graphics g, RectangleD drawRect, Pen pen)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		float num = pen.Width;
		if ((int)pen.DashStyle == 0)
		{
			num = pen.Width / 2f;
		}
		if (!StiOptions.Engine.UseOldZipCodePaintMode)
		{
			num = 0f;
		}
		float num2 = (float)(((RectangleD)(ref drawRect)).Right - (double)num);
		float num3 = (float)(((RectangleD)(ref drawRect)).Y + (double)num);
		float num4 = (float)(((RectangleD)(ref drawRect)).Right - (double)num);
		float num5 = (float)(((RectangleD)(ref drawRect)).Y + ((RectangleD)(ref drawRect)).Height / 2.0);
		g.DrawLine(pen, num2, num3, num4, num5);
	}

	private void DrawVerticalLineUpLeft(Graphics g, RectangleD drawRect, Pen pen)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		float num = pen.Width;
		if ((int)pen.DashStyle == 0)
		{
			num = pen.Width / 2f;
		}
		if (!StiOptions.Engine.UseOldZipCodePaintMode)
		{
			num = 0f;
		}
		float num2 = (float)(((RectangleD)(ref drawRect)).X + (double)num);
		float num3 = (float)(((RectangleD)(ref drawRect)).Y + (double)num);
		float num4 = (float)(((RectangleD)(ref drawRect)).X + (double)num);
		float num5 = (float)(((RectangleD)(ref drawRect)).Y + ((RectangleD)(ref drawRect)).Height / 2.0);
		g.DrawLine(pen, num2, num3, num4, num5);
	}

	private void DrawVerticalLineDownRight(Graphics g, RectangleD drawRect, Pen pen)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		float num = pen.Width;
		if ((int)pen.DashStyle == 0)
		{
			num = pen.Width / 2f;
		}
		if (!StiOptions.Engine.UseOldZipCodePaintMode)
		{
			num = 0f;
		}
		float num2 = (float)(((RectangleD)(ref drawRect)).Right - (double)num);
		float num3 = (float)(((RectangleD)(ref drawRect)).Y + ((RectangleD)(ref drawRect)).Height / 2.0);
		float num4 = (float)(((RectangleD)(ref drawRect)).Right - (double)num);
		float num5 = (float)(((RectangleD)(ref drawRect)).Bottom - (double)num);
		g.DrawLine(pen, num2, num3, num4, num5);
	}

	private void DrawVerticalLineDownLeft(Graphics g, RectangleD drawRect, Pen pen)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		float num = pen.Width;
		if ((int)pen.DashStyle == 0)
		{
			num = pen.Width / 2f;
		}
		if (!StiOptions.Engine.UseOldZipCodePaintMode)
		{
			num = 0f;
		}
		float num2 = (float)(((RectangleD)(ref drawRect)).X + (double)num);
		float num3 = (float)(((RectangleD)(ref drawRect)).Y + ((RectangleD)(ref drawRect)).Height / 2.0);
		float num4 = (float)(((RectangleD)(ref drawRect)).X + (double)num);
		float num5 = (float)(((RectangleD)(ref drawRect)).Bottom - (double)num);
		g.DrawLine(pen, num2, num3, num4, num5);
	}

	private void DrawDioganalLineUp(Graphics g, RectangleD drawRect, Pen pen)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		float num = pen.Width;
		if ((int)pen.DashStyle == 0)
		{
			num = pen.Width / 2f;
		}
		if (!StiOptions.Engine.UseOldZipCodePaintMode)
		{
			num = 0f;
		}
		float num2 = (float)(((RectangleD)(ref drawRect)).Right - (double)num);
		float num3 = (float)(((RectangleD)(ref drawRect)).Y + (double)num);
		float num4 = (float)(((RectangleD)(ref drawRect)).X + (double)num);
		float num5 = (float)(((RectangleD)(ref drawRect)).Y + ((RectangleD)(ref drawRect)).Height / 2.0);
		g.DrawLine(pen, num2, num3, num4, num5);
	}

	private void DrawDioganalLineDown(Graphics g, RectangleD drawRect, Pen pen)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		float num = pen.Width;
		if ((int)pen.DashStyle == 0)
		{
			num = pen.Width / 2f;
		}
		if (!StiOptions.Engine.UseOldZipCodePaintMode)
		{
			num = 0f;
		}
		float num2 = (float)(((RectangleD)(ref drawRect)).Right - (double)num);
		float num3 = (float)(((RectangleD)(ref drawRect)).Y + ((RectangleD)(ref drawRect)).Height / 2.0);
		float num4 = (float)(((RectangleD)(ref drawRect)).X + (double)num);
		float num5 = (float)(((RectangleD)(ref drawRect)).Bottom - (double)num);
		g.DrawLine(pen, num2, num3, num4, num5);
	}

	private void DrawIndex1(Graphics g, RectangleD drawRect, Pen penSolid, Pen penDash)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		DrawHorizLineUp(g, drawRect, penDash);
		DrawHorizLineCenter(g, drawRect, penDash);
		DrawHorizLineDown(g, drawRect, penDash);
		DrawVerticalLineUpLeft(g, drawRect, penDash);
		DrawVerticalLineDownLeft(g, drawRect, penDash);
		DrawVerticalLineUpRight(g, drawRect, penSolid);
		DrawVerticalLineDownRight(g, drawRect, penSolid);
		DrawDioganalLineUp(g, drawRect, penSolid);
		DrawDioganalLineDown(g, drawRect, penDash);
	}

	private void DrawIndex2(Graphics g, RectangleD drawRect, Pen penSolid, Pen penDash, int count, int index)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		DrawHorizLineUp(g, drawRect, penSolid);
		DrawHorizLineCenter(g, drawRect, penDash);
		DrawHorizLineDown(g, drawRect, penSolid);
		DrawVerticalLineUpLeft(g, drawRect, penDash);
		DrawVerticalLineDownLeft(g, drawRect, penDash);
		DrawVerticalLineUpRight(g, drawRect, penSolid);
		DrawVerticalLineDownRight(g, drawRect, penDash);
		DrawDioganalLineUp(g, drawRect, penDash);
		DrawDioganalLineDown(g, drawRect, penSolid);
	}

	private void DrawIndex3(Graphics g, RectangleD drawRect, Pen penSolid, Pen penDash, int count, int index)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		DrawHorizLineUp(g, drawRect, penSolid);
		DrawHorizLineCenter(g, drawRect, penSolid);
		DrawHorizLineDown(g, drawRect, penDash);
		DrawVerticalLineUpLeft(g, drawRect, penDash);
		DrawVerticalLineDownLeft(g, drawRect, penDash);
		DrawVerticalLineUpRight(g, drawRect, penDash);
		DrawVerticalLineDownRight(g, drawRect, penDash);
		DrawDioganalLineUp(g, drawRect, penSolid);
		DrawDioganalLineDown(g, drawRect, penSolid);
	}

	private void DrawIndex4(Graphics g, RectangleD drawRect, Pen penSolid, Pen penDash, int count, int index)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		DrawHorizLineUp(g, drawRect, penDash);
		DrawHorizLineCenter(g, drawRect, penSolid);
		DrawHorizLineDown(g, drawRect, penDash);
		DrawVerticalLineUpLeft(g, drawRect, penSolid);
		DrawVerticalLineDownLeft(g, drawRect, penDash);
		DrawVerticalLineUpRight(g, drawRect, penSolid);
		DrawVerticalLineDownRight(g, drawRect, penSolid);
		DrawDioganalLineUp(g, drawRect, penDash);
		DrawDioganalLineDown(g, drawRect, penDash);
	}

	private void DrawIndex5(Graphics g, RectangleD drawRect, Pen penSolid, Pen penDash, int count, int index)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		DrawHorizLineUp(g, drawRect, penSolid);
		DrawHorizLineCenter(g, drawRect, penSolid);
		DrawHorizLineDown(g, drawRect, penSolid);
		DrawVerticalLineUpLeft(g, drawRect, penSolid);
		DrawVerticalLineDownLeft(g, drawRect, penDash);
		DrawVerticalLineUpRight(g, drawRect, penDash);
		DrawVerticalLineDownRight(g, drawRect, penSolid);
		DrawDioganalLineUp(g, drawRect, penDash);
		DrawDioganalLineDown(g, drawRect, penDash);
	}

	private void DrawIndex6(Graphics g, RectangleD drawRect, Pen penSolid, Pen penDash, int count, int index)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		DrawHorizLineUp(g, drawRect, penDash);
		DrawHorizLineCenter(g, drawRect, penSolid);
		DrawHorizLineDown(g, drawRect, penSolid);
		DrawVerticalLineUpLeft(g, drawRect, penDash);
		DrawVerticalLineDownLeft(g, drawRect, penSolid);
		DrawVerticalLineUpRight(g, drawRect, penDash);
		DrawVerticalLineDownRight(g, drawRect, penSolid);
		DrawDioganalLineUp(g, drawRect, penSolid);
		DrawDioganalLineDown(g, drawRect, penDash);
	}

	private void DrawIndex7(Graphics g, RectangleD drawRect, Pen penSolid, Pen penDash, int count, int index)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		DrawHorizLineUp(g, drawRect, penSolid);
		DrawHorizLineCenter(g, drawRect, penDash);
		DrawHorizLineDown(g, drawRect, penDash);
		DrawVerticalLineUpLeft(g, drawRect, penDash);
		DrawVerticalLineDownLeft(g, drawRect, penSolid);
		DrawVerticalLineUpRight(g, drawRect, penDash);
		DrawVerticalLineDownRight(g, drawRect, penDash);
		DrawDioganalLineUp(g, drawRect, penSolid);
		DrawDioganalLineDown(g, drawRect, penDash);
	}

	private void DrawIndex8(Graphics g, RectangleD drawRect, Pen penSolid, Pen penDash, int count, int index)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		DrawHorizLineUp(g, drawRect, penSolid);
		DrawHorizLineCenter(g, drawRect, penSolid);
		DrawHorizLineDown(g, drawRect, penSolid);
		DrawVerticalLineUpLeft(g, drawRect, penSolid);
		DrawVerticalLineDownLeft(g, drawRect, penSolid);
		DrawVerticalLineUpRight(g, drawRect, penSolid);
		DrawVerticalLineDownRight(g, drawRect, penSolid);
		DrawDioganalLineUp(g, drawRect, penDash);
		DrawDioganalLineDown(g, drawRect, penDash);
	}

	private void DrawIndex9(Graphics g, RectangleD drawRect, Pen penSolid, Pen penDash, int count, int index)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		DrawHorizLineUp(g, drawRect, penSolid);
		DrawHorizLineCenter(g, drawRect, penSolid);
		DrawHorizLineDown(g, drawRect, penDash);
		DrawVerticalLineUpLeft(g, drawRect, penSolid);
		DrawVerticalLineDownLeft(g, drawRect, penDash);
		DrawVerticalLineUpRight(g, drawRect, penSolid);
		DrawVerticalLineDownRight(g, drawRect, penDash);
		DrawDioganalLineUp(g, drawRect, penDash);
		DrawDioganalLineDown(g, drawRect, penSolid);
	}

	private void DrawIndex0(Graphics g, RectangleD drawRect, Pen penSolid, Pen penDash, int count, int index)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		DrawHorizLineUp(g, drawRect, penSolid);
		DrawHorizLineCenter(g, drawRect, penDash);
		DrawHorizLineDown(g, drawRect, penSolid);
		DrawVerticalLineUpLeft(g, drawRect, penSolid);
		DrawVerticalLineDownLeft(g, drawRect, penSolid);
		DrawVerticalLineUpRight(g, drawRect, penSolid);
		DrawVerticalLineDownRight(g, drawRect, penSolid);
		DrawDioganalLineUp(g, drawRect, penDash);
		DrawDioganalLineDown(g, drawRect, penDash);
	}

	private void DrawIndexSpace(Graphics g, RectangleD drawRect, Pen penSolid, Pen penDash, int count, int index)
	{
		float num = penSolid.Width / 2.5f;
		StiDrawing.DrawLine(g, penSolid, ((RectangleD)(ref drawRect)).X - (double)num, ((RectangleD)(ref drawRect)).Y - (double)num, ((RectangleD)(ref drawRect)).X + ((RectangleD)(ref drawRect)).Width + (double)num, ((RectangleD)(ref drawRect)).Y - (double)num);
	}

	private void DrawSpace(Graphics g, RectangleD rect, Pen penDash)
	{
		g.DrawRectangle(penDash, ((RectangleD)(ref rect)).ToRectangle());
		g.DrawLine(penDash, (float)((RectangleD)(ref rect)).X, (float)(((RectangleD)(ref rect)).Y + ((RectangleD)(ref rect)).Height / 2.0), (float)(((RectangleD)(ref rect)).X + ((RectangleD)(ref rect)).Width), (float)((RectangleD)(ref rect)).Y);
		g.DrawLine(penDash, (float)((RectangleD)(ref rect)).X, (float)(((RectangleD)(ref rect)).Y + ((RectangleD)(ref rect)).Height / 2.0), (float)(((RectangleD)(ref rect)).X + ((RectangleD)(ref rect)).Width), (float)(((RectangleD)(ref rect)).Y + ((RectangleD)(ref rect)).Height / 2.0));
		g.DrawLine(penDash, (float)(((RectangleD)(ref rect)).X + ((RectangleD)(ref rect)).Width), (float)(((RectangleD)(ref rect)).Y + ((RectangleD)(ref rect)).Height / 2.0), (float)((RectangleD)(ref rect)).X, (float)(((RectangleD)(ref rect)).Y + ((RectangleD)(ref rect)).Height));
	}

	public void DrawZipCode(StiZipCode zipCode, Graphics g, RectangleD rect, double zoom)
	{
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Expected O, but got Unknown
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Expected O, but got Unknown
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		//IL_025c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		//IL_0288: Unknown result type (might be due to invalid IL or missing references)
		//IL_029e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
		if (zipCode.Brush is StiSolidBrush && ((StiSolidBrush)zipCode.Brush).Color == Color.Transparent && zipCode.Report.Info.FillComponent && zipCode.IsDesigning)
		{
			Color color = Color.FromArgb(150, Color.White);
			StiDrawing.FillRectangle(g, color, ((RectangleD)(ref rect)).Left, ((RectangleD)(ref rect)).Top, ((RectangleD)(ref rect)).Width, ((RectangleD)(ref rect)).Height);
		}
		else
		{
			StiDrawing.FillRectangle(g, zipCode.Brush, rect);
		}
		GraphicsState val = g.Save();
		g.SetClip(((RectangleD)(ref rect)).ToRectangleF(), (CombineMode)1);
		Pen val2 = new Pen(zipCode.ForeColor);
		try
		{
			Pen val3 = new Pen(zipCode.ForeColor);
			try
			{
				val2.DashStyle = (DashStyle)2;
				val2.Width = 1f;
				val2.DashPattern = new float[2] { 1f, 1f };
				val3.DashStyle = (DashStyle)0;
				val3.Width = (float)(zipCode.Size * 2.0 * zoom);
				val3.StartCap = (LineCap)3;
				val3.EndCap = (LineCap)3;
				string text;
				if (zipCode.CodeValue != null)
				{
					text = zipCode.CodeValue;
				}
				else
				{
					StringBuilder stringBuilder = new StringBuilder(zipCode.Code.Value);
					for (int i = 0; i < stringBuilder.Length; i++)
					{
						if (!char.IsDigit(stringBuilder[i]))
						{
							stringBuilder[i] = ' ';
						}
					}
					stringBuilder.Replace(" ", "");
					text = stringBuilder.ToString();
				}
				for (int j = 0; j < text.Length; j++)
				{
					RectangleD markRect;
					RectangleD drawRect = zipCode.CalculateRect(rect, text.Length, j, out markRect);
					switch (text[j])
					{
					case '1':
						DrawIndex1(g, drawRect, val3, val2);
						break;
					case '2':
						DrawIndex2(g, drawRect, val3, val2, text.Length, j);
						break;
					case '3':
						DrawIndex3(g, drawRect, val3, val2, text.Length, j);
						break;
					case '4':
						DrawIndex4(g, drawRect, val3, val2, text.Length, j);
						break;
					case '5':
						DrawIndex5(g, drawRect, val3, val2, text.Length, j);
						break;
					case '6':
						DrawIndex6(g, drawRect, val3, val2, text.Length, j);
						break;
					case '7':
						DrawIndex7(g, drawRect, val3, val2, text.Length, j);
						break;
					case '8':
						DrawIndex8(g, drawRect, val3, val2, text.Length, j);
						break;
					case '9':
						DrawIndex9(g, drawRect, val3, val2, text.Length, j);
						break;
					case '0':
						DrawIndex0(g, drawRect, val3, val2, text.Length, j);
						break;
					case ' ':
						if (!StiOptions.Engine.UseOldZipCodePaintMode)
						{
							DrawIndexSpace(g, drawRect, val3, val2, text.Length, j);
						}
						break;
					}
					if (zipCode.UpperMarks && !StiOptions.Engine.UseOldZipCodePaintMode)
					{
						StiDrawing.FillRectangle(g, val3.Color, markRect);
					}
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
		g.Restore(val);
	}

	public override Image GetImage(StiComponent component, ref float zoom, StiExportFormat format)
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Expected O, but got Unknown
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		StiZipCode stiZipCode = (StiZipCode)component;
		if (format != StiExportFormat.HtmlSpan && format != StiExportFormat.HtmlDiv && format != StiExportFormat.HtmlTable)
		{
			zoom *= 2f;
		}
		double zoom2 = stiZipCode.Report.Info.Zoom;
		stiZipCode.Report.Info.Zoom = zoom;
		RectangleD val = stiZipCode.ComponentToPage(stiZipCode.ClientRectangle);
		RectangleD rect = ((RectangleD)(ref val)).Normalize();
		val = stiZipCode.Report.Unit.ConvertToHInches(rect);
		rect = ((RectangleD)(ref val)).Multiply((double)zoom);
		((RectangleD)(ref rect)).X = 0.0;
		((RectangleD)(ref rect)).Y = 0.0;
		stiZipCode.Report.Info.Zoom = zoom2;
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
			DrawZipCode(stiZipCode, val3, rect, zoom);
			return (Image)(object)val2;
		}
		finally
		{
			((IDisposable)val3)?.Dispose();
		}
	}

	public override void Paint(StiComponent component, StiPaintEventArgs e)
	{
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		StiZipCode stiZipCode = (StiZipCode)component;
		if (!e.DrawBorderFormatting && e.DrawTopmostBorderSides && !stiZipCode.Border.Topmost)
		{
			return;
		}
		if (!(e.Context is Graphics))
		{
			throw new Exception("StiGdiPainter can work only with System.Drawing.Graphics context!");
		}
		if (e.DrawBorderFormatting)
		{
			stiZipCode.InvokePainting(stiZipCode, e);
		}
		if (!e.Cancel)
		{
			Graphics graphics = e.Graphics;
			RectangleD paintRectangle = stiZipCode.GetPaintRectangle();
			double zoom = stiZipCode.Page.Zoom * StiScale.Factor;
			if (((RectangleD)(ref paintRectangle)).Width > 0.0 && ((RectangleD)(ref paintRectangle)).Height > 0.0)
			{
				RectangleD clipRectangle = e.ClipRectangle;
				if (((RectangleD)(ref clipRectangle)).IsEmpty || ((RectangleD)(ref paintRectangle)).IntersectsWith(e.ClipRectangle))
				{
					if (e.DrawBorderFormatting)
					{
						DrawZipCode(stiZipCode, graphics, paintRectangle, zoom);
					}
					if (e.DrawBorderFormatting)
					{
						PaintMarkers(stiZipCode, graphics, paintRectangle);
					}
					if (stiZipCode.HighlightState == StiHighlightState.Hide)
					{
						PaintBorder(stiZipCode, graphics, paintRectangle, zoom, e.DrawBorderFormatting, e.DrawTopmostBorderSides || !stiZipCode.Border.Topmost);
					}
					if (e.DrawBorderFormatting)
					{
						PaintEvents(stiZipCode, e.Graphics, paintRectangle);
						PaintConditions(stiZipCode, e.Graphics, paintRectangle);
					}
				}
			}
		}
		e.Cancel = false;
		if (e.DrawBorderFormatting)
		{
			stiZipCode.InvokePainted(stiZipCode, e);
		}
	}
}
