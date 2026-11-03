using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Painters;

public class StiHorizontalLinePrimitiveGdiPainter : StiLinePrimitiveGdiPainter
{
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
				size = 3.5f * (float)StiScale.Factor;
			}
			if (component.Locked)
			{
				DrawDot(graphics, Pens.Red, ((RectangleD)(ref paintRectangle)).X, ((RectangleD)(ref paintRectangle)).Y, size);
				DrawDot(graphics, Pens.Red, ((RectangleD)(ref paintRectangle)).Right, ((RectangleD)(ref paintRectangle)).Y, size);
			}
			else
			{
				DrawDot(graphics, Pens.DimGray, ((RectangleD)(ref paintRectangle)).X, ((RectangleD)(ref paintRectangle)).Y, size);
				DrawDot(graphics, Pens.DimGray, ((RectangleD)(ref paintRectangle)).Right, ((RectangleD)(ref paintRectangle)).Y, size);
			}
		}
	}

	private void DrawCap(Graphics g, StiHorizontalLinePrimitive primitive, RectangleD paintRect)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_048b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0496: Unknown result type (might be due to invalid IL or missing references)
		//IL_049b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Expected I4, but got Unknown
		//IL_052f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0534: Unknown result type (might be due to invalid IL or missing references)
		//IL_0536: Unknown result type (might be due to invalid IL or missing references)
		//IL_0539: Unknown result type (might be due to invalid IL or missing references)
		//IL_0557: Expected I4, but got Unknown
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Expected O, but got Unknown
		//IL_06b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06bb: Expected O, but got Unknown
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Expected O, but got Unknown
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Expected O, but got Unknown
		//IL_045a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0461: Expected O, but got Unknown
		//IL_0432: Unknown result type (might be due to invalid IL or missing references)
		//IL_0439: Expected O, but got Unknown
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Expected O, but got Unknown
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Expected O, but got Unknown
		//IL_0384: Unknown result type (might be due to invalid IL or missing references)
		//IL_038b: Expected O, but got Unknown
		//IL_0359: Unknown result type (might be due to invalid IL or missing references)
		//IL_0360: Expected O, but got Unknown
		//IL_030a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0311: Expected O, but got Unknown
		//IL_02df: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e6: Expected O, but got Unknown
		//IL_047a: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e9: Expected O, but got Unknown
		//IL_05b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05be: Expected O, but got Unknown
		//IL_08c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_08cd: Expected O, but got Unknown
		//IL_089e: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a5: Expected O, but got Unknown
		//IL_0689: Unknown result type (might be due to invalid IL or missing references)
		//IL_0690: Expected O, but got Unknown
		//IL_065e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0665: Expected O, but got Unknown
		//IL_07f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f7: Expected O, but got Unknown
		//IL_07c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cc: Expected O, but got Unknown
		//IL_0776: Unknown result type (might be due to invalid IL or missing references)
		//IL_077d: Expected O, but got Unknown
		//IL_074b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0752: Expected O, but got Unknown
		//IL_08e6: Unknown result type (might be due to invalid IL or missing references)
		double num = primitive.Page.Zoom * StiScale.Factor;
		float num2 = (float)(num * (double)primitive.Size);
		StiCap startCap = primitive.StartCap;
		float num3;
		float x;
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
			RectangleF rectangleF = new RectangleF((float)((RectangleD)(ref paintRect)).X, (float)(((RectangleD)(ref paintRect)).Y - (double)(startCap.Height / 2)), startCap.Width, startCap.Height);
			x = rectangleF.X;
			num3 = rectangleF.Y + rectangleF.Height / 2f;
			num4 = (float)((double)(rectangleF.Height / 2f) * num);
			num5 = (float)((double)(rectangleF.Width / 2f) * num);
			StiCapStyle style = startCap.Style;
			switch (style - 1)
			{
			case 0:
				array = new PointF[3]
				{
					new PointF(x - num2, num3),
					new PointF(x + 2f * num5, num3 + num4),
					new PointF(x + 2f * num5, num3 - num4)
				};
				if (startCap.Fill)
				{
					SolidBrush val9 = new SolidBrush(startCap.Color);
					try
					{
						g.FillPolygon((Brush)(object)val9, array);
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
					new PointF(x - num5, num3),
					new PointF(x, num3 - num4),
					new PointF(x + num5, num3),
					new PointF(x, num3 + num4)
				};
				if (startCap.Fill)
				{
					SolidBrush val5 = new SolidBrush(startCap.Color);
					try
					{
						g.FillPolygon((Brush)(object)val5, array);
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
						new PointF(x + 2f * num5, num3 - num4),
						new PointF(x, num3),
						new PointF(x + 2f * num5, num3 + num4)
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
				RectangleF rectangleF2 = new RectangleF(x - num5, num3 - num4, num5 * 2f, num4 * 2f);
				if (startCap.Fill)
				{
					SolidBrush val3 = new SolidBrush(startCap.Color);
					try
					{
						g.FillEllipse((Brush)(object)val3, rectangleF2);
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
						g.DrawEllipse(val4, rectangleF2);
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
				RectangleF rectangleF3 = new RectangleF(x - num5, num3 - num4, num5 * 2f, num4 * 2f);
				if (startCap.Fill)
				{
					SolidBrush val7 = new SolidBrush(startCap.Color);
					try
					{
						g.FillRectangle((Brush)(object)val7, rectangleF3);
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
					new PointF(x - num2, num3),
					new PointF(x + 2f * num5, num3 - num4),
					new PointF(x + num5 * 1.3f, num3),
					new PointF(x + 2f * num5, num3 + num4)
				};
				if (startCap.Fill)
				{
					SolidBrush val = new SolidBrush(startCap.Color);
					try
					{
						g.FillPolygon((Brush)(object)val, array);
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
		RectangleF rectangleF4 = new RectangleF((float)((RectangleD)(ref paintRect)).Right - (float)endCap.Width, (float)(((RectangleD)(ref paintRect)).Y - (double)(endCap.Height / 2)), endCap.Width, endCap.Height);
		num3 = rectangleF4.Y + rectangleF4.Height / 2f;
		x = rectangleF4.Right;
		num4 = (float)((double)(rectangleF4.Height / 2f) * num);
		num5 = (float)((double)(rectangleF4.Width / 2f) * num);
		StiCapStyle style2 = endCap.Style;
		switch (style2 - 1)
		{
		case 0:
			array2 = new PointF[3]
			{
				new PointF(x + num2, num3),
				new PointF(x - 2f * num5, num3 - num4),
				new PointF(x - 2f * num5, num3 + num4)
			};
			if (endCap.Fill)
			{
				SolidBrush val20 = new SolidBrush(endCap.Color);
				try
				{
					g.FillPolygon((Brush)(object)val20, array2);
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
				new PointF(x + num5, num3),
				new PointF(x, num3 - num4),
				new PointF(x - num5, num3),
				new PointF(x, num3 + num4)
			};
			if (endCap.Fill)
			{
				SolidBrush val16 = new SolidBrush(endCap.Color);
				try
				{
					g.FillPolygon((Brush)(object)val16, array2);
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
					new PointF(x - 2f * num5, num3 - num4),
					new PointF(x, num3),
					new PointF(x - 2f * num5, num3 + num4)
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
			RectangleF rectangleF5 = new RectangleF(x - num5, num3 - num4, num5 * 2f, num4 * 2f);
			if (endCap.Fill)
			{
				SolidBrush val14 = new SolidBrush(endCap.Color);
				try
				{
					g.FillEllipse((Brush)(object)val14, rectangleF5);
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
					g.DrawEllipse(val15, rectangleF5);
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
			RectangleF rectangleF6 = new RectangleF(x - num5, num3 - num4, num5 * 2f, num4 * 2f);
			if (endCap.Fill)
			{
				SolidBrush val18 = new SolidBrush(endCap.Color);
				try
				{
					g.FillRectangle((Brush)(object)val18, rectangleF6);
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
				new PointF(x + num2, num3),
				new PointF(x - num5 * 2f, num3 - num4),
				new PointF(x - num5 * 1.3f, num3),
				new PointF(x - 2f * num5, num3 + num4)
			};
			if (endCap.Fill)
			{
				SolidBrush val12 = new SolidBrush(endCap.Color);
				try
				{
					g.FillPolygon((Brush)(object)val12, array2);
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

	public override void Paint(StiComponent component, StiPaintEventArgs e)
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Unknown result type (might be due to invalid IL or missing references)
		//IL_021c: Invalid comparison between Unknown and I4
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Expected O, but got Unknown
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Expected O, but got Unknown
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0243: Expected O, but got Unknown
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_025c: Invalid comparison between Unknown and I4
		//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Invalid comparison between Unknown and I4
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0303: Unknown result type (might be due to invalid IL or missing references)
		if (!(e.Context is Graphics))
		{
			throw new Exception("StiGdiPainter can work only with System.Drawing.Graphics context!");
		}
		StiHorizontalLinePrimitive stiHorizontalLinePrimitive = component as StiHorizontalLinePrimitive;
		stiHorizontalLinePrimitive.InvokePainting(stiHorizontalLinePrimitive, e);
		if (!e.Cancel && (stiHorizontalLinePrimitive.Enabled || stiHorizontalLinePrimitive.IsDesigning))
		{
			Graphics graphics = e.Graphics;
			RectangleD rect = stiHorizontalLinePrimitive.GetPaintRectangle();
			CheckRectForOverflow(ref rect);
			if (((RectangleD)(ref rect)).Width > 0.0 && ((RectangleD)(ref rect)).Height > 0.0)
			{
				RectangleD val = e.ClipRectangle;
				if (((RectangleD)(ref val)).IsEmpty || ((RectangleD)(ref rect)).IntersectsWith(e.ClipRectangle))
				{
					double num = ((RectangleD)(ref rect)).Y;
					val = stiHorizontalLinePrimitive.Page.OffsetRectangle;
					if (((RectangleD)(ref val)).Height < 0.0)
					{
						num = ((RectangleD)(ref rect)).Bottom;
					}
					if ((stiHorizontalLinePrimitive.HighlightState == StiHighlightState.Show || stiHorizontalLinePrimitive.HighlightState == StiHighlightState.Active) && !stiHorizontalLinePrimitive.Report.Info.IsComponentsMoving && StiRestrictionsHelper.IsAllowSelect(stiHorizontalLinePrimitive))
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
								val3.AddLine((float)((RectangleD)(ref rect)).X, (float)num, (float)((RectangleD)(ref rect)).Right, (float)num);
								if (stiHorizontalLinePrimitive.IsSelected)
								{
									float size = StiScale.I3;
									AddDotToPath(val3, ((RectangleD)(ref rect)).X, num, size);
									AddDotToPath(val3, ((RectangleD)(ref rect)).Right, num, size);
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
					float num2 = (float)((stiHorizontalLinePrimitive.Page.Zoom * StiScale.Factor > StiScale.Factor) ? (stiHorizontalLinePrimitive.Page.Zoom * StiScale.Factor) : StiScale.Factor);
					Pen val4 = new Pen(stiHorizontalLinePrimitive.Color, ((int)stiHorizontalLinePrimitive.Style == 5) ? num2 : ((float)((double)stiHorizontalLinePrimitive.Size * stiHorizontalLinePrimitive.Page.Zoom * StiScale.Factor)));
					try
					{
						val4.DashStyle = StiPenUtils.GetPenStyle(stiHorizontalLinePrimitive.Style);
						if ((int)stiHorizontalLinePrimitive.Style == 5)
						{
							graphics.DrawLine(val4, (float)((RectangleD)(ref rect)).X, (float)num - num2, (float)((RectangleD)(ref rect)).Right, (float)num - num2);
							graphics.DrawLine(val4, (float)((RectangleD)(ref rect)).X, (float)num + num2, (float)((RectangleD)(ref rect)).Right, (float)num + num2);
						}
						else if ((int)stiHorizontalLinePrimitive.Style != 6 || StiOptions.Engine.PrimitivesStyleNoneBackCompatibility)
						{
							graphics.DrawLine(val4, (float)((RectangleD)(ref rect)).X, (float)num, (float)((RectangleD)(ref rect)).Right, (float)num);
						}
					}
					finally
					{
						((IDisposable)val4)?.Dispose();
					}
					DrawCap(graphics, stiHorizontalLinePrimitive, rect);
					PaintEvents(stiHorizontalLinePrimitive, e.Graphics, rect);
					PaintConditions(stiHorizontalLinePrimitive, e.Graphics, rect);
				}
			}
		}
		e.Cancel = false;
		stiHorizontalLinePrimitive.InvokePainted(stiHorizontalLinePrimitive, e);
	}
}
