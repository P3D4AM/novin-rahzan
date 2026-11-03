using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Painters;

public class StiRoundedRectanglePrimitiveGdiPainter : StiRectanglePrimitiveGdiPainter
{
	protected override void PaintPrimitive(StiRectanglePrimitive primitive, Graphics g, RectangleD rectD)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Expected O, but got Unknown
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Expected O, but got Unknown
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_0270: Expected O, but got Unknown
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		//IL_0278: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_0289: Invalid comparison between Unknown and I4
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02da: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e0: Invalid comparison between Unknown and I4
		RectangleF rectangleF = ((RectangleD)(ref rectD)).ToRectangleF();
		StiRoundedRectanglePrimitive stiRoundedRectanglePrimitive = (StiRoundedRectanglePrimitive)primitive;
		if ((primitive.HighlightState == StiHighlightState.Show || primitive.HighlightState == StiHighlightState.Active) && !primitive.Report.Info.IsComponentsMoving)
		{
			SmoothingMode smoothingMode = g.SmoothingMode;
			if (!StiOptions.Engine.DisableAntialiasingInPainters)
			{
				g.SmoothingMode = (SmoothingMode)4;
			}
			Pen val = new Pen(Color.FromArgb(150, Color.Gold), (float)StiScale.I3);
			try
			{
				GraphicsPath val2 = new GraphicsPath();
				try
				{
					val.Alignment = (PenAlignment)2;
					val.StartCap = (LineCap)2;
					val.EndCap = (LineCap)2;
					AddRoundedRectangleToPath(stiRoundedRectanglePrimitive, rectangleF, val2);
					if (primitive.IsSelected)
					{
						float size = StiScale.I3;
						AddDotToPath(val2, rectangleF.X, rectangleF.Y, size);
						AddDotToPath(val2, rectangleF.Right, rectangleF.Y, size);
						AddDotToPath(val2, rectangleF.X, rectangleF.Bottom, size);
						AddDotToPath(val2, rectangleF.Right, rectangleF.Bottom, size);
						AddDotToPath(val2, rectangleF.X + rectangleF.Width / 2f, rectangleF.Y, size);
						AddDotToPath(val2, rectangleF.Right, rectangleF.Y + rectangleF.Height / 2f, size);
						AddDotToPath(val2, rectangleF.X + rectangleF.Width / 2f, rectangleF.Bottom, size);
						AddDotToPath(val2, rectangleF.X, rectangleF.Y + rectangleF.Height / 2f, size);
					}
					g.DrawPath(val, val2);
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
			g.SmoothingMode = smoothingMode;
		}
		float num = (float)Math.Min((rectangleF.Height < rectangleF.Width) ? rectangleF.Height : rectangleF.Width, 100.0 * stiRoundedRectanglePrimitive.Page.Zoom * StiScale.Factor) * stiRoundedRectanglePrimitive.Round;
		if (!(rectangleF.Width >= num * 2f) || !(rectangleF.Height >= num * 2f))
		{
			return;
		}
		try
		{
			Pen val3 = new Pen(primitive.Color, (float)((double)primitive.Size * primitive.Page.Zoom * StiScale.Factor));
			try
			{
				val3.DashStyle = StiPenUtils.GetPenStyle(stiRoundedRectanglePrimitive.Style);
				if ((int)primitive.Style == 5)
				{
					RectangleF rect = rectangleF;
					RectangleF rect2 = rectangleF;
					rect.Inflate(-1f, -1f);
					rect2.Inflate(1f, 1f);
					val3.Width = 1f;
					DrawRoundedRectangleToPath(g, val3, stiRoundedRectanglePrimitive, rect);
					DrawRoundedRectangleToPath(g, val3, stiRoundedRectanglePrimitive, rect2);
				}
				else if ((int)primitive.Style != 6 || StiOptions.Engine.PrimitivesStyleNoneBackCompatibility)
				{
					DrawRoundedRectangleToPath(g, val3, stiRoundedRectanglePrimitive, rectangleF);
				}
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		catch
		{
		}
	}

	private void AddRoundedRectangleToPath(StiRoundedRectanglePrimitive roundedPrimitive, RectangleF rect, GraphicsPath path)
	{
		float num = (float)Math.Min((rect.Height < rect.Width) ? rect.Height : rect.Width, 100.0 * roundedPrimitive.Page.Zoom * StiScale.Factor) * roundedPrimitive.Round;
		if (!(rect.Width >= num * 2f) || !(rect.Height >= num * 2f))
		{
			return;
		}
		try
		{
			num = (float)Math.Round(num, 2);
			if ((double)num < 0.01)
			{
				num = 0.01f;
			}
			path.StartFigure();
			path.AddLine(rect.X + num, rect.Y, rect.Right - num, rect.Y);
			path.AddArc(rect.Right - 2f * num, rect.Y, 2f * num, 2f * num, -90f, 90f);
			path.AddLine(rect.Right, rect.Y + num, rect.Right, rect.Bottom - num);
			path.AddArc(rect.Right - 2f * num, rect.Bottom - 2f * num, 2f * num, 2f * num, 0f, 90f);
			path.AddLine(rect.Right - num, rect.Bottom, rect.X + num, rect.Bottom);
			path.AddArc(rect.X, rect.Bottom - 2f * num, 2f * num, 2f * num, 90f, 90f);
			path.AddLine(rect.X, rect.Bottom - num, rect.X, rect.Y + num);
			path.AddArc(rect.X, rect.Y, 2f * num, 2f * num, 180f, 90f);
			path.CloseFigure();
		}
		catch
		{
		}
	}

	private void DrawRoundedRectangleToPath(Graphics g, Pen pen, StiRoundedRectanglePrimitive roundedPrimitive, RectangleF rect)
	{
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Expected O, but got Unknown
		float num = (float)Math.Min((rect.Height < rect.Width) ? rect.Height : rect.Width, 100.0 * roundedPrimitive.Page.Zoom * StiScale.Factor) * roundedPrimitive.Round;
		if (!(rect.Width >= num * 2f) || !(rect.Height >= num * 2f))
		{
			return;
		}
		try
		{
			num = (float)Math.Round(num, 2);
			if ((double)num < 0.01)
			{
				num = 0.01f;
			}
			if (roundedPrimitive.TopSide && roundedPrimitive.LeftSide && roundedPrimitive.BottomSide && roundedPrimitive.RightSide)
			{
				GraphicsPath val = new GraphicsPath();
				try
				{
					val.StartFigure();
					val.AddLine(rect.X + num, rect.Y, rect.Right - num, rect.Y);
					val.AddArc(rect.Right - 2f * num, rect.Y, 2f * num, 2f * num, -90f, 90f);
					val.AddLine(rect.Right, rect.Y + num, rect.Right, rect.Bottom - num);
					val.AddArc(rect.Right - 2f * num, rect.Bottom - 2f * num, 2f * num, 2f * num, 0f, 90f);
					val.AddLine(rect.Right - num, rect.Bottom, rect.X + num, rect.Bottom);
					val.AddArc(rect.X, rect.Bottom - 2f * num, 2f * num, 2f * num, 90f, 90f);
					val.AddLine(rect.X, rect.Bottom - num, rect.X, rect.Y + num);
					val.AddArc(rect.X, rect.Y, 2f * num, 2f * num, 180f, 90f);
					val.CloseFigure();
					g.DrawPath(pen, val);
					return;
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			float num2 = 0f;
			float num3 = 0f;
			if (roundedPrimitive.TopSide)
			{
				if (roundedPrimitive.LeftSide)
				{
					num2 = num;
				}
				if (roundedPrimitive.RightSide)
				{
					num3 = num;
				}
				g.DrawLine(pen, rect.X + num2, rect.Y, rect.Right - num3, rect.Y);
				if (roundedPrimitive.RightSide)
				{
					g.DrawArc(pen, rect.Right - 2f * num, rect.Y, 2f * num, 2f * num, -90f, 90f);
				}
				num2 = 0f;
				num3 = 0f;
			}
			if (roundedPrimitive.RightSide)
			{
				if (roundedPrimitive.TopSide)
				{
					num2 = num;
				}
				if (roundedPrimitive.BottomSide)
				{
					num3 = num;
				}
				g.DrawLine(pen, rect.Right, rect.Y + num2, rect.Right, rect.Bottom - num3);
				if (roundedPrimitive.BottomSide)
				{
					g.DrawArc(pen, rect.Right - 2f * num, rect.Bottom - 2f * num, 2f * num, 2f * num, 0f, 90f);
				}
				num2 = 0f;
				num3 = 0f;
			}
			if (roundedPrimitive.BottomSide)
			{
				if (roundedPrimitive.RightSide)
				{
					num2 = num;
				}
				if (roundedPrimitive.LeftSide)
				{
					num3 = num;
				}
				g.DrawLine(pen, rect.Right - num2, rect.Bottom, rect.X + num3, rect.Bottom);
				if (roundedPrimitive.LeftSide)
				{
					g.DrawArc(pen, rect.X, rect.Bottom - 2f * num, 2f * num, 2f * num, 90f, 90f);
				}
				num2 = 0f;
				num3 = 0f;
			}
			if (roundedPrimitive.LeftSide)
			{
				if (roundedPrimitive.BottomSide)
				{
					num2 = num;
				}
				if (roundedPrimitive.TopSide)
				{
					num3 = num;
				}
				g.DrawLine(pen, rect.X, rect.Bottom - num2, rect.X, rect.Y + num3);
				if (roundedPrimitive.TopSide)
				{
					g.DrawArc(pen, rect.X, rect.Y, 2f * num, 2f * num, 180f, 90f);
				}
				num2 = 0f;
				num3 = 0f;
			}
		}
		catch
		{
		}
	}
}
