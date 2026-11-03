using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Painters;

public class StiRectanglePrimitiveGdiPainter : StiLinePrimitiveGdiPainter
{
	protected virtual void PaintPrimitive(StiRectanglePrimitive primitive, Graphics g, RectangleD rect)
	{
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Expected O, but got Unknown
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Invalid comparison between Unknown and I4
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		//IL_0293: Unknown result type (might be due to invalid IL or missing references)
		//IL_0299: Invalid comparison between Unknown and I4
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Expected O, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Expected O, but got Unknown
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_0239: Unknown result type (might be due to invalid IL or missing references)
		//IL_023f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_026d: Unknown result type (might be due to invalid IL or missing references)
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
					val2.AddRectangle(((RectangleD)(ref rect)).ToRectangleF());
					if (primitive.IsSelected)
					{
						float size = StiScale.I3;
						AddDotToPath(val2, ((RectangleD)(ref rect)).X, ((RectangleD)(ref rect)).Y, size);
						AddDotToPath(val2, ((RectangleD)(ref rect)).Right, ((RectangleD)(ref rect)).Y, size);
						AddDotToPath(val2, ((RectangleD)(ref rect)).X, ((RectangleD)(ref rect)).Bottom, size);
						AddDotToPath(val2, ((RectangleD)(ref rect)).Right, ((RectangleD)(ref rect)).Bottom, size);
						AddDotToPath(val2, ((RectangleD)(ref rect)).X + ((RectangleD)(ref rect)).Width / 2.0, ((RectangleD)(ref rect)).Y, size);
						AddDotToPath(val2, ((RectangleD)(ref rect)).Right, ((RectangleD)(ref rect)).Y + ((RectangleD)(ref rect)).Height / 2.0, size);
						AddDotToPath(val2, ((RectangleD)(ref rect)).X + ((RectangleD)(ref rect)).Width / 2.0, ((RectangleD)(ref rect)).Bottom, size);
						AddDotToPath(val2, ((RectangleD)(ref rect)).X, ((RectangleD)(ref rect)).Y + ((RectangleD)(ref rect)).Height / 2.0, size);
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
		Pen val3 = new Pen(primitive.Color, (float)((double)primitive.Size * primitive.Page.Zoom * StiScale.Factor));
		try
		{
			val3.DashStyle = StiPenUtils.GetPenStyle(primitive.Style);
			if ((int)primitive.Style == 5)
			{
				StiBorder val4 = new StiBorder
				{
					Color = primitive.Color,
					Style = (StiPenStyle)5,
					Side = (StiBorderSides)0
				};
				if (primitive.TopSide)
				{
					val4.Side = (StiBorderSides)(val4.Side | 1);
				}
				if (primitive.LeftSide)
				{
					val4.Side = (StiBorderSides)(val4.Side | 2);
				}
				if (primitive.BottomSide)
				{
					val4.Side = (StiBorderSides)(val4.Side | 8);
				}
				if (primitive.RightSide)
				{
					val4.Side = (StiBorderSides)(val4.Side | 4);
				}
				val4.Draw(g, rect, primitive.Page.Zoom * StiScale.Factor);
			}
			else
			{
				if ((int)primitive.Style == 6 && !StiOptions.Engine.PrimitivesStyleNoneBackCompatibility)
				{
					return;
				}
				if (primitive.TopSide && primitive.LeftSide && primitive.BottomSide && primitive.RightSide)
				{
					g.DrawRectangle(val3, (float)((RectangleD)(ref rect)).X, (float)((RectangleD)(ref rect)).Y, (float)((RectangleD)(ref rect)).Width, (float)((RectangleD)(ref rect)).Height);
					return;
				}
				if (primitive.TopSide)
				{
					g.DrawLine(val3, (float)((RectangleD)(ref rect)).X, (float)((RectangleD)(ref rect)).Y, (float)((RectangleD)(ref rect)).Right, (float)((RectangleD)(ref rect)).Y);
				}
				if (primitive.BottomSide)
				{
					g.DrawLine(val3, (float)((RectangleD)(ref rect)).X, (float)((RectangleD)(ref rect)).Bottom, (float)((RectangleD)(ref rect)).Right, (float)((RectangleD)(ref rect)).Bottom);
				}
				if (primitive.LeftSide)
				{
					g.DrawLine(val3, (float)((RectangleD)(ref rect)).X, (float)((RectangleD)(ref rect)).Y, (float)((RectangleD)(ref rect)).X, (float)((RectangleD)(ref rect)).Bottom);
				}
				if (primitive.RightSide)
				{
					g.DrawLine(val3, (float)((RectangleD)(ref rect)).Right, (float)((RectangleD)(ref rect)).Y, (float)((RectangleD)(ref rect)).Right, (float)((RectangleD)(ref rect)).Bottom);
				}
			}
		}
		finally
		{
			((IDisposable)val3)?.Dispose();
		}
	}

	public override void Paint(StiComponent component, StiPaintEventArgs e)
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		if (!(e.Context is Graphics))
		{
			throw new Exception("StiGdiPainter can work only with System.Drawing.Graphics context!");
		}
		StiRectanglePrimitive stiRectanglePrimitive = (StiRectanglePrimitive)component;
		stiRectanglePrimitive.InvokePainting(stiRectanglePrimitive, e);
		if (!e.Cancel && (stiRectanglePrimitive.Enabled || stiRectanglePrimitive.IsDesigning))
		{
			Graphics graphics = e.Graphics;
			RectangleD rect = stiRectanglePrimitive.GetPaintRectangle();
			CheckRectForOverflow(ref rect);
			if (((RectangleD)(ref rect)).Width > 0.0 && ((RectangleD)(ref rect)).Height > 0.0)
			{
				RectangleD clipRectangle = e.ClipRectangle;
				if (((RectangleD)(ref clipRectangle)).IsEmpty || ((RectangleD)(ref rect)).IntersectsWith(e.ClipRectangle))
				{
					PaintPrimitive(stiRectanglePrimitive, graphics, rect);
					PaintEvents(stiRectanglePrimitive, e.Graphics, rect);
					PaintConditions(stiRectanglePrimitive, e.Graphics, rect);
				}
			}
		}
		e.Cancel = false;
		stiRectanglePrimitive.InvokePainted(stiRectanglePrimitive, e);
	}

	public override void PaintSelection(StiComponent component, StiPaintEventArgs e)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		Graphics graphics = e.Graphics;
		if (component.IsDesigning && component.IsSelected && !component.Report.Info.IsComponentsMoving)
		{
			RectangleD paintRectangle = component.GetPaintRectangle();
			Pen pen = Pens.DimGray;
			if (component.Locked)
			{
				pen = Pens.Red;
			}
			float size = 2.5f * (float)StiScale.Factor;
			if (component.Linked)
			{
				size = 3.5f * (float)StiScale.Factor;
			}
			DrawDot(graphics, pen, ((RectangleD)(ref paintRectangle)).X, ((RectangleD)(ref paintRectangle)).Y, size);
			DrawDot(graphics, pen, ((RectangleD)(ref paintRectangle)).Right, ((RectangleD)(ref paintRectangle)).Y, size);
			DrawDot(graphics, pen, ((RectangleD)(ref paintRectangle)).X, ((RectangleD)(ref paintRectangle)).Bottom, size);
			DrawDot(graphics, pen, ((RectangleD)(ref paintRectangle)).Right, ((RectangleD)(ref paintRectangle)).Bottom, size);
			DrawDot(graphics, pen, ((RectangleD)(ref paintRectangle)).X + ((RectangleD)(ref paintRectangle)).Width / 2.0, ((RectangleD)(ref paintRectangle)).Y, size);
			DrawDot(graphics, pen, ((RectangleD)(ref paintRectangle)).Right, ((RectangleD)(ref paintRectangle)).Y + ((RectangleD)(ref paintRectangle)).Height / 2.0, size);
			DrawDot(graphics, pen, ((RectangleD)(ref paintRectangle)).X + ((RectangleD)(ref paintRectangle)).Width / 2.0, ((RectangleD)(ref paintRectangle)).Bottom, size);
			DrawDot(graphics, pen, ((RectangleD)(ref paintRectangle)).X, ((RectangleD)(ref paintRectangle)).Y + ((RectangleD)(ref paintRectangle)).Height / 2.0, size);
		}
	}
}
