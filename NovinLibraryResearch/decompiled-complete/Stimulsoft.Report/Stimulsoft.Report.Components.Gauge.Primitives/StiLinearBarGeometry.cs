using System;
using System.Drawing;
using Stimulsoft.Report.Gauge;
using Stimulsoft.Report.Gauge.GaugeGeoms;
using Stimulsoft.Report.Gauge.Helpers;
using Stimulsoft.Report.Gauge.Primitives;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Components.Gauge.Primitives;

internal sealed class StiLinearBarGeometry : IStiScaleBarGeometry
{
	private readonly StiLinearScale scale;

	private SizeF size;

	private RectangleF rectGeometry;

	private PointF center;

	public SizeF Size => size;

	public RectangleF RectGeometry => rectGeometry;

	public float Radius => 0f;

	public float Diameter => 0f;

	public PointF Center => center;

	public void CheckRectGeometry(RectangleF rect)
	{
		size = rect.Size;
		CheckMinMaxWidth(out var startWidth, out var endWidth);
		rectGeometry = GetRectGeometry(rect, (startWidth > endWidth) ? startWidth : endWidth);
		center = new PointF(rectGeometry.Left + rectGeometry.Width / 2f, rectGeometry.Top + rectGeometry.Height / 2f);
	}

	private RectangleF GetRectGeometry(RectangleF rect, float value)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		float num;
		float num2;
		if ((int)scale.Orientation == 0)
		{
			num = size.Width * scale.RelativeHeight;
			num2 = size.Height * value;
		}
		else
		{
			num2 = size.Height * scale.RelativeHeight;
			num = size.Width * value;
		}
		return new RectangleF(rect.X + (size.Width - num) / 2f, rect.Y + (size.Height - num2) / 2f, num, num2);
	}

	public float GetRestToLenght()
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		float num = Math.Abs(scale.StartWidth - scale.EndWidth);
		if ((int)scale.Orientation == 0)
		{
			return size.Height * num / 2f;
		}
		return size.Width * num / 2f;
	}

	private void CheckMinMaxWidth(out float startWidth, out float endWidth)
	{
		if (scale.IsReversed)
		{
			startWidth = scale.EndWidth;
			endWidth = scale.StartWidth;
		}
		else
		{
			startWidth = scale.StartWidth;
			endWidth = scale.EndWidth;
		}
	}

	public void DrawScaleGeometry(StiGaugeContextPainter context)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		CheckMinMaxWidth(out var startWidth, out var endWidth);
		PointF[] array = new PointF[4];
		if ((int)scale.Orientation == 0)
		{
			float height = size.Height;
			height *= ((startWidth < endWidth) ? startWidth : endWidth);
			height = (rectGeometry.Height - height) / 2f;
			if (startWidth > endWidth)
			{
				array[0] = new PointF(rectGeometry.Left, rectGeometry.Top);
				array[1] = new PointF(rectGeometry.Right, rectGeometry.Top + height);
				array[2] = new PointF(rectGeometry.Right, rectGeometry.Bottom - height);
				array[3] = new PointF(rectGeometry.Left, rectGeometry.Bottom);
			}
			else
			{
				array[0] = new PointF(rectGeometry.Left, rectGeometry.Top + height);
				array[1] = new PointF(rectGeometry.Left, rectGeometry.Bottom - height);
				array[2] = new PointF(rectGeometry.Right, rectGeometry.Bottom);
				array[3] = new PointF(rectGeometry.Right, rectGeometry.Top);
			}
		}
		else
		{
			float width = size.Width;
			width *= ((startWidth < endWidth) ? startWidth : endWidth);
			width = (rectGeometry.Width - width) / 2f;
			if (startWidth > endWidth)
			{
				array[0] = new PointF(rectGeometry.Left + width, rectGeometry.Top);
				array[1] = new PointF(rectGeometry.Left, rectGeometry.Bottom);
				array[2] = new PointF(rectGeometry.Right, rectGeometry.Bottom);
				array[3] = new PointF(rectGeometry.Right - width, rectGeometry.Top);
			}
			else
			{
				array[0] = new PointF(rectGeometry.Left, rectGeometry.Top);
				array[1] = new PointF(rectGeometry.Left + width, rectGeometry.Bottom);
				array[2] = new PointF(rectGeometry.Right - width, rectGeometry.Bottom);
				array[3] = new PointF(rectGeometry.Right, rectGeometry.Top);
			}
		}
		StiGraphicsPathGaugeGeom stiGraphicsPathGaugeGeom = new StiGraphicsPathGaugeGeom(rectGeometry, array[0], scale.Brush, scale.BorderBrush, 1f);
		stiGraphicsPathGaugeGeom.AddGraphicsPathLinesGaugeGeom(array);
		stiGraphicsPathGaugeGeom.AddGraphicsPathCloseFigureGaugeGeom();
		context.AddGraphicsPathGaugeGeom(stiGraphicsPathGaugeGeom);
	}

	public StiGraphicsPathLinesGaugeGeom DrawGeometry(StiGaugeContextPainter context, float StartValue, float EndValue, float StartWidth, float EndWidth, float Offset, StiPlacement Placement, ref RectangleF rect, bool returnOnlyRect)
	{
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		RectangleF rectangleF = RectGeometry;
		rect = rectangleF;
		if (rect.Width == 0f || rect.Height == 0f)
		{
			return null;
		}
		float num = StartValue;
		float num2 = EndValue;
		if (num == num2)
		{
			return null;
		}
		float actualMinimum = scale.ScaleHelper.ActualMinimum;
		float actualMaximum = scale.ScaleHelper.ActualMaximum;
		float totalLength = scale.ScaleHelper.TotalLength;
		if ((num <= actualMinimum && num2 <= actualMinimum) || (num >= actualMaximum && num2 >= actualMaximum))
		{
			return null;
		}
		float num3;
		float num4;
		if (num < num2)
		{
			if (num < actualMinimum)
			{
				num = actualMinimum;
			}
			if (num2 > actualMaximum)
			{
				num2 = actualMaximum;
			}
			num3 = StiMathHelper.Length(actualMinimum, num);
			num4 = StiMathHelper.Length(num, num2);
		}
		else
		{
			if (num < actualMinimum)
			{
				num = actualMinimum;
			}
			if (num2 > actualMaximum)
			{
				num2 = actualMaximum;
			}
			num3 = StiMathHelper.Length(actualMinimum, num2);
			num4 = StiMathHelper.Length(num2, num);
		}
		if ((int)scale.Orientation == 0)
		{
			float num5 = rect.Width / totalLength;
			float num6 = num3 * num5;
			float num7 = num4 * num5;
			if (scale.IsReversed)
			{
				rect.X = rect.Right - (num6 + num7);
				rect.Width = num7;
			}
			else
			{
				rect.X += num6;
				rect.Width = num7;
			}
		}
		else
		{
			float num8 = rect.Height / totalLength;
			float num9 = num3 * num8;
			float num10 = num4 * num8;
			if (scale.IsReversed)
			{
				rect.Y += num9;
				rect.Height = num10;
			}
			else
			{
				rect.Y += rect.Height - (num9 + num10);
				rect.Height = num10;
			}
		}
		bool flag = scale.StartWidth > scale.EndWidth;
		float restToLenght = scale.barGeometry.GetRestToLenght();
		float num11 = num3 / totalLength;
		float num12 = (num3 + num4) / totalLength;
		if (!flag)
		{
			num11 = 1f - num11;
			num12 = 1f - num12;
		}
		num11 *= restToLenght;
		num12 *= restToLenght;
		float num13 = (flag ? (num12 - num11) : (num11 - num12));
		float num14 = 0f;
		float minAscent;
		if ((int)scale.Orientation == 0)
		{
			minAscent = size.Height * Math.Min(StartWidth, EndWidth);
			num14 = size.Height * Math.Max(StartWidth, EndWidth);
			float num15 = size.Height * Offset;
			if (flag)
			{
				switch (Placement)
				{
				case StiPlacement.Outside:
					rect.Y -= num14 - num12 + num13;
					rect.Height = num14 + num13;
					rect.Y -= num15;
					break;
				case StiPlacement.Overlay:
					rect.Y = StiRectangleHelper.CenterY(rect) - num14 / 2f;
					rect.Height = num14;
					rect.Y += num15;
					break;
				case StiPlacement.Inside:
					rect.Y += rect.Height - num12;
					rect.Height = num14 + num13;
					rect.Y += num15;
					break;
				}
			}
			else
			{
				switch (Placement)
				{
				case StiPlacement.Outside:
					rect.Y -= num14 - num11 + num13;
					rect.Height = num14 + num13;
					rect.Y -= num15;
					break;
				case StiPlacement.Overlay:
					rect.Y = StiRectangleHelper.CenterY(rect) - num14 / 2f;
					rect.Height = num14;
					rect.Y += num15;
					break;
				case StiPlacement.Inside:
					rect.Y += rect.Height - num11;
					rect.Height = num14 + num13;
					rect.Y += num15;
					break;
				}
			}
		}
		else
		{
			minAscent = size.Width * Math.Min(StartWidth, EndWidth);
			num14 = size.Width * Math.Max(StartWidth, EndWidth);
			float num16 = size.Width * Offset;
			if (flag)
			{
				switch (Placement)
				{
				case StiPlacement.Outside:
					rect.X -= num14 - num12 + num13;
					rect.Width = num14 + num13;
					rect.X -= num16;
					break;
				case StiPlacement.Overlay:
					rect.X = StiRectangleHelper.CenterX(rect) - num14 / 2f;
					rect.Width = num14;
					rect.X += num16;
					break;
				case StiPlacement.Inside:
					rect.X += rect.Width - num12;
					rect.Width = num14 + num13;
					rect.X += num16;
					break;
				}
			}
			else
			{
				switch (Placement)
				{
				case StiPlacement.Outside:
					rect.X -= num14 - num11 + num13;
					rect.Width = num14 + num13;
					rect.X -= num16;
					break;
				case StiPlacement.Overlay:
					rect.X = StiRectangleHelper.CenterX(rect) - num14 / 2f;
					rect.Width = num14;
					rect.X += num16;
					break;
				case StiPlacement.Inside:
					rect.X += rect.Width - num11;
					rect.Width = num14 + num13;
					rect.X += num16;
					break;
				}
			}
		}
		if (!returnOnlyRect)
		{
			return DrawPrimitiveGeometry(context, rect, minAscent, num14, StartWidth, EndWidth, Placement, num13, flag);
		}
		return null;
	}

	public StiGraphicsPathLinesGaugeGeom DrawPrimitiveGeometry(StiGaugeContextPainter context, RectangleF rect, float minAscent, float maxAscent, float StartWidth, float EndWidth, StiPlacement Placement, float restOffset, bool isStartGreaterEnd)
	{
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		RectangleF rectangleF = rect;
		rect.X = 0f;
		rect.Y = 0f;
		PointF[] array = new PointF[4];
		if (scale.StartWidth == scale.EndWidth && minAscent == maxAscent)
		{
			array[0] = new PointF(rect.Left, rect.Top);
			array[1] = new PointF(rect.Right, rect.Top);
			array[2] = new PointF(rect.Right, rect.Bottom);
			array[3] = new PointF(rect.Left, rect.Bottom);
		}
		else
		{
			bool flag = scale.StartWidth < scale.EndWidth;
			bool flag2 = StartWidth <= EndWidth;
			if (scale.IsReversed)
			{
				flag = !flag;
				flag2 = !flag2;
			}
			if ((int)scale.Orientation == 0)
			{
				if (flag)
				{
					if (flag2)
					{
						switch (Placement)
						{
						case StiPlacement.Outside:
							array[0] = new PointF(0f, rect.Height - minAscent);
							array[1] = new PointF(rect.Width, rect.Height - restOffset - maxAscent);
							array[2] = new PointF(rect.Width, rect.Height - restOffset);
							array[3] = new PointF(0f, rect.Height);
							break;
						case StiPlacement.Overlay:
							array[0] = new PointF(0f, StiRectangleHelper.CenterY(rect) - minAscent / 2f);
							array[1] = new PointF(rect.Width, StiRectangleHelper.CenterY(rect) - maxAscent / 2f);
							array[2] = new PointF(rect.Width, StiRectangleHelper.CenterY(rect) + maxAscent / 2f);
							array[3] = new PointF(0f, StiRectangleHelper.CenterY(rect) + minAscent / 2f);
							break;
						default:
							array[0] = new PointF(0f, 0f);
							array[1] = new PointF(rect.Width, restOffset);
							array[2] = new PointF(rect.Width, restOffset + maxAscent);
							array[3] = new PointF(0f, minAscent);
							break;
						}
					}
					else
					{
						switch (Placement)
						{
						case StiPlacement.Outside:
							array[0] = new PointF(0f, rect.Height - maxAscent);
							array[1] = new PointF(rect.Width, rect.Height - restOffset - minAscent);
							array[2] = new PointF(rect.Width, rect.Height - restOffset);
							array[3] = new PointF(0f, rect.Height);
							break;
						case StiPlacement.Overlay:
							array[0] = new PointF(0f, StiRectangleHelper.CenterY(rect) - maxAscent / 2f);
							array[1] = new PointF(rect.Width, StiRectangleHelper.CenterY(rect) - minAscent / 2f);
							array[2] = new PointF(rect.Width, StiRectangleHelper.CenterY(rect) + minAscent / 2f);
							array[3] = new PointF(0f, StiRectangleHelper.CenterY(rect) + maxAscent / 2f);
							break;
						default:
							array[0] = new PointF(0f, 0f);
							array[1] = new PointF(rect.Width, restOffset);
							array[2] = new PointF(rect.Width, restOffset + minAscent);
							array[3] = new PointF(0f, maxAscent);
							break;
						}
					}
				}
				else if (flag2)
				{
					switch (Placement)
					{
					case StiPlacement.Outside:
						array[0] = new PointF(0f, rect.Height - restOffset - minAscent);
						array[1] = new PointF(rect.Width, rect.Height - maxAscent);
						array[2] = new PointF(rect.Width, rect.Height);
						array[3] = new PointF(0f, rect.Height - restOffset);
						break;
					case StiPlacement.Overlay:
						array[0] = new PointF(0f, StiRectangleHelper.CenterY(rect) - minAscent / 2f);
						array[1] = new PointF(rect.Right, StiRectangleHelper.CenterY(rect) - maxAscent / 2f);
						array[2] = new PointF(rect.Right, StiRectangleHelper.CenterY(rect) + maxAscent / 2f);
						array[3] = new PointF(0f, StiRectangleHelper.CenterY(rect) + minAscent / 2f);
						break;
					default:
						array[0] = new PointF(0f, restOffset);
						array[1] = new PointF(rect.Right, 0f);
						array[2] = new PointF(rect.Right, maxAscent);
						array[3] = new PointF(0f, restOffset + minAscent);
						break;
					}
				}
				else
				{
					switch (Placement)
					{
					case StiPlacement.Outside:
						array[0] = new PointF(0f, rect.Height - restOffset - maxAscent);
						array[1] = new PointF(rect.Right, rect.Height - minAscent);
						array[2] = new PointF(rect.Right, rect.Height);
						array[3] = new PointF(0f, rect.Height - restOffset);
						break;
					case StiPlacement.Overlay:
						array[0] = new PointF(0f, StiRectangleHelper.CenterY(rect) - maxAscent / 2f);
						array[1] = new PointF(rect.Right, StiRectangleHelper.CenterY(rect) - minAscent / 2f);
						array[2] = new PointF(rect.Right, StiRectangleHelper.CenterY(rect) + minAscent / 2f);
						array[3] = new PointF(0f, StiRectangleHelper.CenterY(rect) + maxAscent / 2f);
						break;
					default:
						array[0] = new PointF(0f, restOffset);
						array[1] = new PointF(rect.Right, 0f);
						array[2] = new PointF(rect.Right, minAscent);
						array[3] = new PointF(0f, restOffset + maxAscent);
						break;
					}
				}
			}
			else if (flag)
			{
				if (flag2)
				{
					switch (Placement)
					{
					case StiPlacement.Outside:
						array[0] = new PointF(rect.Width - restOffset - maxAscent, 0f);
						array[1] = new PointF(rect.Width - restOffset, 0f);
						array[2] = new PointF(rect.Width, rect.Height);
						array[3] = new PointF(rect.Width - minAscent, rect.Height);
						break;
					case StiPlacement.Overlay:
						array[0] = new PointF(StiRectangleHelper.CenterX(rect) - maxAscent / 2f, 0f);
						array[1] = new PointF(StiRectangleHelper.CenterX(rect) + maxAscent / 2f, 0f);
						array[2] = new PointF(StiRectangleHelper.CenterX(rect) + minAscent / 2f, rect.Height);
						array[3] = new PointF(StiRectangleHelper.CenterX(rect) - minAscent / 2f, rect.Height);
						break;
					default:
						array[0] = new PointF(restOffset, 0f);
						array[1] = new PointF(restOffset + maxAscent, 0f);
						array[2] = new PointF(minAscent, rect.Height);
						array[3] = new PointF(0f, rect.Height);
						break;
					}
				}
				else
				{
					switch (Placement)
					{
					case StiPlacement.Outside:
						array[0] = new PointF(rect.Width - restOffset - minAscent, 0f);
						array[1] = new PointF(rect.Width - restOffset, 0f);
						array[2] = new PointF(rect.Width, rect.Height);
						array[3] = new PointF(rect.Width - maxAscent, rect.Height);
						break;
					case StiPlacement.Overlay:
						array[0] = new PointF(StiRectangleHelper.CenterX(rect) - minAscent / 2f, 0f);
						array[1] = new PointF(StiRectangleHelper.CenterX(rect) + minAscent / 2f, 0f);
						array[2] = new PointF(StiRectangleHelper.CenterX(rect) + maxAscent / 2f, rect.Height);
						array[3] = new PointF(StiRectangleHelper.CenterX(rect) - maxAscent / 2f, rect.Height);
						break;
					default:
						array[0] = new PointF(restOffset, 0f);
						array[1] = new PointF(restOffset + minAscent, 0f);
						array[2] = new PointF(maxAscent, rect.Height);
						array[3] = new PointF(0f, rect.Height);
						break;
					}
				}
			}
			else if (flag2)
			{
				switch (Placement)
				{
				case StiPlacement.Outside:
					array[0] = new PointF(rect.Width - maxAscent, 0f);
					array[1] = new PointF(rect.Width, 0f);
					array[2] = new PointF(rect.Width - restOffset, rect.Height);
					array[3] = new PointF(rect.Width - restOffset - minAscent, rect.Height);
					break;
				case StiPlacement.Overlay:
					array[0] = new PointF(StiRectangleHelper.CenterX(rect) - maxAscent / 2f, 0f);
					array[1] = new PointF(StiRectangleHelper.CenterX(rect) + maxAscent / 2f, 0f);
					array[2] = new PointF(StiRectangleHelper.CenterX(rect) + minAscent / 2f, rect.Height);
					array[3] = new PointF(StiRectangleHelper.CenterX(rect) - minAscent / 2f, rect.Height);
					break;
				default:
					array[0] = new PointF(rect.Left, 0f);
					array[1] = new PointF(maxAscent, 0f);
					array[2] = new PointF(restOffset + minAscent, rect.Height);
					array[3] = new PointF(restOffset, rect.Height);
					break;
				}
			}
			else
			{
				switch (Placement)
				{
				case StiPlacement.Outside:
					array[0] = new PointF(rect.Width - minAscent, 0f);
					array[1] = new PointF(rect.Width, 0f);
					array[2] = new PointF(rect.Width - restOffset, rect.Height);
					array[3] = new PointF(rect.Width - restOffset - maxAscent, rect.Height);
					break;
				case StiPlacement.Overlay:
					array[0] = new PointF(StiRectangleHelper.CenterX(rect) - minAscent / 2f, 0f);
					array[1] = new PointF(StiRectangleHelper.CenterX(rect) + minAscent / 2f, 0f);
					array[2] = new PointF(StiRectangleHelper.CenterX(rect) + maxAscent / 2f, rect.Height);
					array[3] = new PointF(StiRectangleHelper.CenterX(rect) - maxAscent / 2f, rect.Height);
					break;
				default:
					array[0] = new PointF(0f, 0f);
					array[1] = new PointF(minAscent, 0f);
					array[2] = new PointF(restOffset + maxAscent, rect.Height);
					array[3] = new PointF(restOffset, rect.Height);
					break;
				}
			}
		}
		array[0].X += rectangleF.Left;
		array[0].Y += rectangleF.Top;
		array[1].X += rectangleF.Left;
		array[1].Y += rectangleF.Top;
		array[2].X += rectangleF.Left;
		array[2].Y += rectangleF.Top;
		array[3].X += rectangleF.Left;
		array[3].Y += rectangleF.Top;
		return new StiGraphicsPathLinesGaugeGeom(array);
	}

	public StiLinearBarGeometry(StiLinearScale scale)
	{
		size = new SizeF(0f, 0f);
		rectGeometry = RectangleF.Empty;
		center = new PointF(0f, 0f);
		this.scale = scale;
	}
}
