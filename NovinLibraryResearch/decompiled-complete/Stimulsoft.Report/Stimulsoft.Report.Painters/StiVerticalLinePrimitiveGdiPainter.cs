using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Painters;

public class StiVerticalLinePrimitiveGdiPainter : StiLinePrimitiveGdiPainter
{
	public override void Paint(StiComponent component, StiPaintEventArgs e)
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Invalid comparison between Unknown and I4
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Expected O, but got Unknown
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Expected O, but got Unknown
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_022e: Expected O, but got Unknown
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Invalid comparison between Unknown and I4
		//IL_0294: Unknown result type (might be due to invalid IL or missing references)
		//IL_029a: Invalid comparison between Unknown and I4
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
		if (!(e.Context is Graphics))
		{
			throw new Exception("StiGdiPainter can work only with System.Drawing.Graphics context!");
		}
		StiVerticalLinePrimitive stiVerticalLinePrimitive = (StiVerticalLinePrimitive)component;
		stiVerticalLinePrimitive.InvokePainting(stiVerticalLinePrimitive, e);
		if (!e.Cancel && (stiVerticalLinePrimitive.Enabled || stiVerticalLinePrimitive.IsDesigning))
		{
			Graphics graphics = e.Graphics;
			RectangleD rect = stiVerticalLinePrimitive.GetPaintRectangle();
			CheckRectForOverflow(ref rect);
			if (((RectangleD)(ref rect)).Height > 0.0)
			{
				RectangleD val = e.ClipRectangle;
				if (((RectangleD)(ref val)).IsEmpty || ((RectangleD)(ref rect)).IntersectsWith(e.ClipRectangle))
				{
					double num = ((RectangleD)(ref rect)).X;
					val = stiVerticalLinePrimitive.Page.OffsetRectangle;
					if (((RectangleD)(ref val)).Width < 0.0)
					{
						num = ((RectangleD)(ref rect)).Right;
					}
					if ((stiVerticalLinePrimitive.HighlightState == StiHighlightState.Show || stiVerticalLinePrimitive.HighlightState == StiHighlightState.Active) && !stiVerticalLinePrimitive.Report.Info.IsComponentsMoving && StiRestrictionsHelper.IsAllowSelect(stiVerticalLinePrimitive))
					{
						SmoothingMode smoothingMode = graphics.SmoothingMode;
						if (!StiOptions.Engine.DisableAntialiasingInPainters)
						{
							graphics.SmoothingMode = (SmoothingMode)4;
						}
						Pen val2 = new Pen(Color.FromArgb(150, Color.Gold), (float)StiScale.I3);
						try
						{
							GraphicsPath val3 = new GraphicsPath();
							try
							{
								val2.Alignment = (PenAlignment)2;
								val2.StartCap = (LineCap)2;
								val2.EndCap = (LineCap)2;
								val3.AddLine((float)num, (float)((RectangleD)(ref rect)).Y, (float)num, (float)((RectangleD)(ref rect)).Bottom);
								if (stiVerticalLinePrimitive.IsSelected)
								{
									float size = StiScale.I3;
									AddDotToPath(val3, num, ((RectangleD)(ref rect)).Y, size);
									AddDotToPath(val3, num, ((RectangleD)(ref rect)).Bottom, size);
								}
								graphics.DrawPath(val2, val3);
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
						graphics.SmoothingMode = smoothingMode;
					}
					float num2 = (float)((stiVerticalLinePrimitive.Page.Zoom * StiScale.Factor > StiScale.Factor) ? (stiVerticalLinePrimitive.Page.Zoom * StiScale.Factor) : StiScale.Factor);
					Pen val4 = new Pen(stiVerticalLinePrimitive.Color, ((int)stiVerticalLinePrimitive.Style == 5) ? num2 : ((float)((double)stiVerticalLinePrimitive.Size * stiVerticalLinePrimitive.Page.Zoom * StiScale.Factor)));
					try
					{
						val4.DashStyle = StiPenUtils.GetPenStyle(stiVerticalLinePrimitive.Style);
						if ((int)stiVerticalLinePrimitive.Style == 5)
						{
							graphics.DrawLine(val4, (float)num - num2, (float)((RectangleD)(ref rect)).Y, (float)num - num2, (float)((RectangleD)(ref rect)).Bottom);
							graphics.DrawLine(val4, (float)num + num2, (float)((RectangleD)(ref rect)).Y, (float)num + num2, (float)((RectangleD)(ref rect)).Bottom);
						}
						else if ((int)stiVerticalLinePrimitive.Style != 6 || StiOptions.Engine.PrimitivesStyleNoneBackCompatibility)
						{
							graphics.DrawLine(val4, (float)num, (float)((RectangleD)(ref rect)).Y, (float)num, (float)((RectangleD)(ref rect)).Bottom);
						}
					}
					finally
					{
						((IDisposable)val4)?.Dispose();
					}
					DrawCap(graphics, stiVerticalLinePrimitive, rect);
					PaintEvents(stiVerticalLinePrimitive, e.Graphics, rect);
					PaintConditions(stiVerticalLinePrimitive, e.Graphics, rect);
				}
			}
		}
		e.Cancel = false;
		stiVerticalLinePrimitive.InvokePainted(stiVerticalLinePrimitive, e);
	}

	public override void PaintSelection(StiComponent component, StiPaintEventArgs e)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		Graphics graphics = e.Graphics;
		if (component.IsDesigning && component.IsSelected && !component.Report.Info.IsComponentsMoving)
		{
			RectangleD paintRectangle = component.GetPaintRectangle();
			float size = 2.5f * (float)StiScale.Factor;
			if (component.Linked)
			{
				size = 3.5f;
			}
			if (component.Locked)
			{
				DrawDot(graphics, Pens.Red, ((RectangleD)(ref paintRectangle)).X, ((RectangleD)(ref paintRectangle)).Y, size);
				DrawDot(graphics, Pens.Red, ((RectangleD)(ref paintRectangle)).X, ((RectangleD)(ref paintRectangle)).Bottom, size);
			}
			else
			{
				DrawDot(graphics, Pens.DimGray, ((RectangleD)(ref paintRectangle)).X, ((RectangleD)(ref paintRectangle)).Y, size);
				DrawDot(graphics, Pens.DimGray, ((RectangleD)(ref paintRectangle)).X, ((RectangleD)(ref paintRectangle)).Bottom, size);
			}
		}
	}

	private void DrawCap(Graphics g, StiVerticalLinePrimitive primitive, RectangleD paintRect)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_049b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Expected I4, but got Unknown
		//IL_053f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0544: Unknown result type (might be due to invalid IL or missing references)
		//IL_0546: Unknown result type (might be due to invalid IL or missing references)
		//IL_0549: Unknown result type (might be due to invalid IL or missing references)
		//IL_0567: Expected I4, but got Unknown
		//IL_023f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0246: Expected O, but got Unknown
		//IL_06c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06cf: Expected O, but got Unknown
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Expected O, but got Unknown
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Expected O, but got Unknown
		//IL_046a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0471: Expected O, but got Unknown
		//IL_0443: Unknown result type (might be due to invalid IL or missing references)
		//IL_044a: Expected O, but got Unknown
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_021c: Expected O, but got Unknown
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Expected O, but got Unknown
		//IL_0395: Unknown result type (might be due to invalid IL or missing references)
		//IL_039c: Expected O, but got Unknown
		//IL_036b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0372: Expected O, but got Unknown
		//IL_0302: Unknown result type (might be due to invalid IL or missing references)
		//IL_0309: Expected O, but got Unknown
		//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02df: Expected O, but got Unknown
		//IL_048a: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fb: Expected O, but got Unknown
		//IL_05c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d0: Expected O, but got Unknown
		//IL_08fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0903: Expected O, but got Unknown
		//IL_08d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_08db: Expected O, but got Unknown
		//IL_069d: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a4: Expected O, but got Unknown
		//IL_0672: Unknown result type (might be due to invalid IL or missing references)
		//IL_0679: Expected O, but got Unknown
		//IL_0824: Unknown result type (might be due to invalid IL or missing references)
		//IL_082b: Expected O, but got Unknown
		//IL_07f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0800: Expected O, but got Unknown
		//IL_078e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0795: Expected O, but got Unknown
		//IL_0763: Unknown result type (might be due to invalid IL or missing references)
		//IL_076a: Expected O, but got Unknown
		//IL_091c: Unknown result type (might be due to invalid IL or missing references)
		StiCap startCap = primitive.StartCap;
		double num = primitive.Page.Zoom * StiScale.Factor;
		float num2 = (float)(num * (double)primitive.Size);
		float num3;
		float y;
		float num4;
		float num5;
		if ((int)startCap.Style != 0)
		{
			SmoothingMode smoothingMode = g.SmoothingMode;
			if (!StiOptions.Engine.DisableAntialiasingInPainters)
			{
				g.SmoothingMode = (SmoothingMode)4;
			}
			PointF[] array = null;
			RectangleF rectangleF = new RectangleF((float)((RectangleD)(ref paintRect)).X - (float)(startCap.Width / 2), (float)((RectangleD)(ref paintRect)).Y, startCap.Width, startCap.Height);
			num3 = rectangleF.X + rectangleF.Width / 2f;
			y = rectangleF.Y;
			num4 = (float)((double)(rectangleF.Height / 2f) * num);
			num5 = (float)((double)(rectangleF.Width / 2f) * num);
			StiCapStyle style = startCap.Style;
			switch (style - 1)
			{
			case 0:
				array = new PointF[3]
				{
					new PointF(num3, y - num2),
					new PointF(num3 - num5, y + num4 * 2f),
					new PointF(num3 + num5, y + num4 * 2f)
				};
				if (startCap.Fill)
				{
					Brush val9 = (Brush)new SolidBrush(startCap.Color);
					try
					{
						g.FillPolygon(val9, array);
					}
					finally
					{
						((IDisposable)val9)?.Dispose();
					}
				}
				else
				{
					Pen val10 = new Pen(startCap.Color, num2);
					try
					{
						g.DrawPolygon(val10, array);
					}
					finally
					{
						((IDisposable)val10)?.Dispose();
					}
				}
				break;
			case 3:
				array = new PointF[4]
				{
					new PointF(num3, y - num4),
					new PointF(num3 - num5, y),
					new PointF(num3, y + num4),
					new PointF(num3 + num5, y)
				};
				if (startCap.Fill)
				{
					Brush val5 = (Brush)new SolidBrush(startCap.Color);
					try
					{
						g.FillPolygon(val5, array);
					}
					finally
					{
						((IDisposable)val5)?.Dispose();
					}
				}
				else
				{
					Pen val6 = new Pen(startCap.Color, num2);
					try
					{
						g.DrawPolygon(val6, array);
					}
					finally
					{
						((IDisposable)val6)?.Dispose();
					}
				}
				break;
			case 1:
			{
				Pen val11 = new Pen(startCap.Color, num2);
				try
				{
					array = new PointF[3]
					{
						new PointF(num3 - num5, y + 2f * num4),
						new PointF(num3, y),
						new PointF(num3 + num5, y + 2f * num4)
					};
					g.DrawLines(val11, array);
				}
				finally
				{
					((IDisposable)val11)?.Dispose();
				}
				break;
			}
			case 5:
			{
				RectangleF rectangleF2 = new RectangleF(num3 - num5, y - num4, num5 * 2f, num4 * 2f);
				if (startCap.Fill)
				{
					Brush val3 = (Brush)new SolidBrush(startCap.Color);
					try
					{
						g.FillEllipse(val3, rectangleF2);
					}
					finally
					{
						((IDisposable)val3)?.Dispose();
					}
				}
				else
				{
					Pen val4 = new Pen(startCap.Color, num2);
					try
					{
						g.DrawEllipse(val4, rectangleF2.X, rectangleF2.Y, rectangleF2.Width, rectangleF2.Height);
					}
					finally
					{
						((IDisposable)val4)?.Dispose();
					}
				}
				break;
			}
			case 4:
			{
				RectangleF rectangleF3 = new RectangleF(num3 - num5, y - num4, num5 * 2f, num4 * 2f);
				if (startCap.Fill)
				{
					Brush val7 = (Brush)new SolidBrush(startCap.Color);
					try
					{
						g.FillRectangle(val7, rectangleF3);
					}
					finally
					{
						((IDisposable)val7)?.Dispose();
					}
				}
				else
				{
					Pen val8 = new Pen(startCap.Color, num2);
					try
					{
						g.DrawRectangle(val8, rectangleF3.X, rectangleF3.Y, rectangleF3.Width, rectangleF3.Height);
					}
					finally
					{
						((IDisposable)val8)?.Dispose();
					}
				}
				break;
			}
			case 2:
				array = new PointF[4]
				{
					new PointF(num3, y - num2),
					new PointF(num3 - num5, y + 2f * num4),
					new PointF(num3, y + num4 * 1.3f),
					new PointF(num3 + num5, y + 2f * num4)
				};
				if (startCap.Fill)
				{
					Brush val = (Brush)new SolidBrush(startCap.Color);
					try
					{
						g.FillPolygon(val, array);
					}
					finally
					{
						((IDisposable)val)?.Dispose();
					}
				}
				else
				{
					Pen val2 = new Pen(startCap.Color, num2);
					try
					{
						g.DrawPolygon(val2, array);
					}
					finally
					{
						((IDisposable)val2)?.Dispose();
					}
				}
				break;
			}
			g.SmoothingMode = smoothingMode;
		}
		StiCap endCap = primitive.EndCap;
		if ((int)endCap.Style == 0)
		{
			return;
		}
		SmoothingMode smoothingMode2 = g.SmoothingMode;
		if (!StiOptions.Engine.DisableAntialiasingInPainters)
		{
			g.SmoothingMode = (SmoothingMode)4;
		}
		PointF[] array2 = null;
		RectangleF rectangleF4 = new RectangleF((float)((RectangleD)(ref paintRect)).X - (float)(endCap.Width / 2), (float)(((RectangleD)(ref paintRect)).Bottom - (double)endCap.Height), endCap.Width, endCap.Height);
		num3 = rectangleF4.X + rectangleF4.Width / 2f;
		y = rectangleF4.Bottom;
		num4 = (float)((double)(rectangleF4.Height / 2f) * num);
		num5 = (float)((double)(rectangleF4.Width / 2f) * num);
		StiCapStyle style2 = endCap.Style;
		switch (style2 - 1)
		{
		case 0:
			array2 = new PointF[3]
			{
				new PointF(num3, y + num2),
				new PointF(num3 - num5, y - 2f * num4),
				new PointF(num3 + num5, y - 2f * num4)
			};
			if (endCap.Fill)
			{
				Brush val20 = (Brush)new SolidBrush(endCap.Color);
				try
				{
					g.FillPolygon(val20, array2);
				}
				finally
				{
					((IDisposable)val20)?.Dispose();
				}
			}
			else
			{
				Pen val21 = new Pen(endCap.Color, num2);
				try
				{
					g.DrawPolygon(val21, array2);
				}
				finally
				{
					((IDisposable)val21)?.Dispose();
				}
			}
			break;
		case 3:
			array2 = new PointF[4]
			{
				new PointF(num3, y + num4),
				new PointF(num3 - num5, y),
				new PointF(num3, y - num4),
				new PointF(num3 + num5, y)
			};
			if (endCap.Fill)
			{
				Brush val16 = (Brush)new SolidBrush(endCap.Color);
				try
				{
					g.FillPolygon(val16, array2);
				}
				finally
				{
					((IDisposable)val16)?.Dispose();
				}
			}
			else
			{
				Pen val17 = new Pen(endCap.Color, num2);
				try
				{
					g.DrawPolygon(val17, array2);
				}
				finally
				{
					((IDisposable)val17)?.Dispose();
				}
			}
			break;
		case 1:
		{
			Pen val22 = new Pen(endCap.Color, num2);
			try
			{
				array2 = new PointF[3]
				{
					new PointF(num3 - num5, y - 2f * num4),
					new PointF(num3, y),
					new PointF(num3 + num5, y - 2f * num4)
				};
				g.DrawLines(val22, array2);
			}
			finally
			{
				((IDisposable)val22)?.Dispose();
			}
			break;
		}
		case 5:
		{
			RectangleF rectangleF5 = new RectangleF(num3 - num5, y - num4, num5 * 2f, num4 * 2f);
			if (endCap.Fill)
			{
				Brush val14 = (Brush)new SolidBrush(endCap.Color);
				try
				{
					g.FillEllipse(val14, rectangleF5);
				}
				finally
				{
					((IDisposable)val14)?.Dispose();
				}
			}
			else
			{
				Pen val15 = new Pen(endCap.Color, num2);
				try
				{
					g.DrawEllipse(val15, rectangleF5.X, rectangleF5.Y, rectangleF5.Width, rectangleF5.Height);
				}
				finally
				{
					((IDisposable)val15)?.Dispose();
				}
			}
			break;
		}
		case 4:
		{
			RectangleF rectangleF6 = new RectangleF(num3 - num5, y - num4, num5 * 2f, num4 * 2f);
			if (endCap.Fill)
			{
				Brush val18 = (Brush)new SolidBrush(endCap.Color);
				try
				{
					g.FillRectangle(val18, rectangleF6);
				}
				finally
				{
					((IDisposable)val18)?.Dispose();
				}
			}
			else
			{
				Pen val19 = new Pen(endCap.Color, num2);
				try
				{
					g.DrawRectangle(val19, rectangleF6.X, rectangleF6.Y, rectangleF6.Width, rectangleF6.Height);
				}
				finally
				{
					((IDisposable)val19)?.Dispose();
				}
			}
			break;
		}
		case 2:
			array2 = new PointF[4]
			{
				new PointF(num3, y + num2),
				new PointF(num3 - num5, y - 2f * num4),
				new PointF(num3, y - num4 * 1.3f),
				new PointF(num3 + num5, y - 2f * num4)
			};
			if (endCap.Fill)
			{
				Brush val12 = (Brush)new SolidBrush(endCap.Color);
				try
				{
					g.FillPolygon(val12, array2);
				}
				finally
				{
					((IDisposable)val12)?.Dispose();
				}
			}
			else
			{
				Pen val13 = new Pen(endCap.Color, num2);
				try
				{
					g.DrawPolygon(val13, array2);
				}
				finally
				{
					((IDisposable)val13)?.Dispose();
				}
			}
			break;
		}
		g.SmoothingMode = smoothingMode2;
	}
}
