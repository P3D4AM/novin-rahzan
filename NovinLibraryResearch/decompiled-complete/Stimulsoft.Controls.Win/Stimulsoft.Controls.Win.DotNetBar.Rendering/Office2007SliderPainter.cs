using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Controls;
using Stimulsoft.Controls.Win.DotNetBar.Ribbon;
using Stimulsoft.Controls.Win.DotNetBar.TextMarkup;

namespace Stimulsoft.Controls.Win.DotNetBar.Rendering;

internal class Office2007SliderPainter : SliderPainter, IOffice2007Painter
{
	private Office2007ColorTable m_ColorTable;

	public Office2007ColorTable ColorTable
	{
		get
		{
			return m_ColorTable;
		}
		set
		{
			m_ColorTable = value;
		}
	}

	protected virtual Office2007SliderColorTable GetColorTable(SliderItem item, Control container)
	{
		if (container == null)
		{
			return m_ColorTable.Slider;
		}
		Office2007ColorTable colorTable = m_ColorTable;
		string contextualKey = Office2007ColorTable.GetContextualKey(typeof(Office2007SliderColorTable), ((object)container).GetType());
		object value = null;
		if (container is Bar && colorTable.ContextualTables.TryGetValue(contextualKey + "+" + ((Bar)(object)container).BarType, out value))
		{
			return (Office2007SliderColorTable)value;
		}
		if (colorTable.ContextualTables.TryGetValue(contextualKey, out value))
		{
			return (Office2007SliderColorTable)value;
		}
		return m_ColorTable.Slider;
	}

	public override void Paint(SliderItemRendererEventArgs e)
	{
		//IL_0485: Unknown result type (might be due to invalid IL or missing references)
		//IL_048a: Unknown result type (might be due to invalid IL or missing references)
		//IL_078d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0794: Expected O, but got Unknown
		//IL_04ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f5: Expected O, but got Unknown
		//IL_07e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e7: Expected O, but got Unknown
		//IL_0545: Unknown result type (might be due to invalid IL or missing references)
		//IL_054c: Expected O, but got Unknown
		//IL_0884: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0858: Unknown result type (might be due to invalid IL or missing references)
		//IL_085f: Expected O, but got Unknown
		//IL_05c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d0: Expected O, but got Unknown
		SliderItem sliderItem = e.SliderItem;
		Office2007SliderColorTable colorTable = GetColorTable(sliderItem, e.ItemPaintArgs.ContainerControl);
		Office2007SliderStateColorTable office2007SliderStateColorTable = null;
		Office2007SliderStateColorTable office2007SliderStateColorTable2 = null;
		Office2007SliderStateColorTable office2007SliderStateColorTable3 = null;
		eDotNetBarStyle effectiveStyle = sliderItem.EffectiveStyle;
		if (sliderItem.GetEnabled())
		{
			office2007SliderStateColorTable = colorTable.Default;
			office2007SliderStateColorTable2 = colorTable.Default;
			office2007SliderStateColorTable3 = colorTable.Default;
			if (colorTable.DecreaseButtonPart != null)
			{
				office2007SliderStateColorTable = colorTable.DecreaseButtonPart.Default;
			}
			if (colorTable.IncreaseButtonPart != null)
			{
				office2007SliderStateColorTable2 = colorTable.IncreaseButtonPart.Default;
			}
			if (colorTable.TrackPart != null)
			{
				office2007SliderStateColorTable3 = colorTable.TrackPart.Default;
			}
			if (sliderItem.MouseDownPart == eSliderPart.DecreaseButton)
			{
				office2007SliderStateColorTable = ((colorTable.DecreaseButtonPart == null) ? colorTable.Pressed : colorTable.DecreaseButtonPart.Pressed);
			}
			else if (sliderItem.MouseDownPart == eSliderPart.IncreaseButton)
			{
				office2007SliderStateColorTable2 = ((colorTable.IncreaseButtonPart == null) ? colorTable.Pressed : colorTable.IncreaseButtonPart.Pressed);
			}
			else if (sliderItem.MouseDownPart == eSliderPart.TrackArea)
			{
				office2007SliderStateColorTable3 = ((colorTable.TrackPart == null) ? colorTable.Pressed : colorTable.TrackPart.Pressed);
			}
			else if (sliderItem.MouseOverPart == eSliderPart.DecreaseButton)
			{
				office2007SliderStateColorTable = ((colorTable.DecreaseButtonPart == null) ? colorTable.MouseOver : colorTable.DecreaseButtonPart.MouseOver);
			}
			else if (sliderItem.MouseOverPart == eSliderPart.IncreaseButton)
			{
				office2007SliderStateColorTable2 = ((colorTable.IncreaseButtonPart == null) ? colorTable.MouseOver : colorTable.IncreaseButtonPart.MouseOver);
			}
			else if (sliderItem.MouseOverPart == eSliderPart.TrackArea)
			{
				office2007SliderStateColorTable3 = ((colorTable.TrackPart == null) ? colorTable.MouseOver : colorTable.TrackPart.MouseOver);
			}
		}
		else
		{
			office2007SliderStateColorTable = colorTable.Disabled;
			office2007SliderStateColorTable2 = colorTable.Disabled;
			office2007SliderStateColorTable3 = colorTable.Disabled;
		}
		Rectangle displayRectangle = sliderItem.DisplayRectangle;
		Rectangle labelBounds = sliderItem.LabelBounds;
		Graphics graphics = e.Graphics;
		string text = GetText(sliderItem);
		if (!labelBounds.IsEmpty && text.Length > 0)
		{
			labelBounds.Offset(displayRectangle.Location);
			eTextFormat eTextFormat = eTextFormat.WordEllipsis;
			if (sliderItem.LabelPosition == eSliderLabelPosition.Left)
			{
				eTextFormat |= eTextFormat.VerticalCenter;
			}
			else if (sliderItem.LabelPosition == eSliderLabelPosition.Right)
			{
				eTextFormat |= eTextFormat.Right | eTextFormat.VerticalCenter;
			}
			else if (sliderItem.LabelPosition == eSliderLabelPosition.Top)
			{
				eTextFormat |= eTextFormat.HorizontalCenter | eTextFormat.WordBreak;
			}
			else if (sliderItem.LabelPosition == eSliderLabelPosition.Bottom)
			{
				eTextFormat |= eTextFormat.Bottom | eTextFormat.HorizontalCenter | eTextFormat.WordBreak;
			}
			Color color = office2007SliderStateColorTable3.LabelColor;
			if (e.ItemPaintArgs.ContainerControl is Slider)
			{
				color = office2007SliderStateColorTable3.SliderLabelColor;
			}
			if (!sliderItem.TextColor.IsEmpty)
			{
				color = sliderItem.TextColor;
			}
			if (sliderItem.TextMarkupBody == null)
			{
				if (e.ItemPaintArgs != null && e.ItemPaintArgs.GlassEnabled && sliderItem.Parent is CaptionItemContainer && !(e.ItemPaintArgs.ContainerControl is QatToolbar))
				{
					if (!e.ItemPaintArgs.CachedPaint)
					{
						Office2007RibbonControlPainter.PaintTextOnGlass(graphics, GetText(sliderItem), e.ItemPaintArgs.Font, labelBounds, TextDrawing.GetTextFormat(eTextFormat));
					}
				}
				else if (sliderItem.SliderOrientation == eOrientation.Vertical)
				{
					graphics.RotateTransform(90f);
					TextDrawing.DrawString(graphics, GetText(sliderItem), e.ItemPaintArgs.Font, color, new Rectangle(labelBounds.Top, -labelBounds.Right, labelBounds.Height, labelBounds.Width), eTextFormat);
					graphics.ResetTransform();
				}
				else
				{
					TextDrawing.DrawString(graphics, GetText(sliderItem), e.ItemPaintArgs.Font, color, labelBounds, eTextFormat);
				}
			}
			else if (sliderItem.SliderOrientation == eOrientation.Vertical)
			{
				graphics.RotateTransform(90f);
				MarkupDrawContext markupDrawContext = new MarkupDrawContext(graphics, e.ItemPaintArgs.Font, color, e.ItemPaintArgs.RightToLeft);
				markupDrawContext.HotKeyPrefixVisible = (eTextFormat & eTextFormat.HidePrefix) != eTextFormat.HidePrefix;
				sliderItem.TextMarkupBody.Bounds = new Rectangle(labelBounds.Top, -labelBounds.Right, sliderItem.TextMarkupBody.Bounds.Width, sliderItem.TextMarkupBody.Bounds.Height);
				sliderItem.TextMarkupBody.Render(markupDrawContext);
				graphics.ResetTransform();
			}
			else
			{
				MarkupDrawContext markupDrawContext2 = new MarkupDrawContext(graphics, e.ItemPaintArgs.Font, color, e.ItemPaintArgs.RightToLeft);
				markupDrawContext2.HotKeyPrefixVisible = (eTextFormat & eTextFormat.HidePrefix) != eTextFormat.HidePrefix;
				sliderItem.TextMarkupBody.Bounds = labelBounds;
				sliderItem.TextMarkupBody.Render(markupDrawContext2);
			}
		}
		labelBounds = sliderItem.SlideBounds;
		if (!labelBounds.IsEmpty)
		{
			SmoothingMode smoothingMode = graphics.SmoothingMode;
			graphics.SmoothingMode = (SmoothingMode)3;
			labelBounds.Offset(displayRectangle.Location);
			if (sliderItem.SliderOrientation == eOrientation.Horizontal)
			{
				int num = labelBounds.Y + labelBounds.Height / 2 - StiScale2.I1;
				int num2 = labelBounds.X + labelBounds.Width / 2;
				if (!office2007SliderStateColorTable3.TrackLineColor.IsEmpty)
				{
					Pen val = new Pen(office2007SliderStateColorTable3.TrackLineColor);
					try
					{
						graphics.DrawLine(val, labelBounds.X - StiScale2.I1, num, labelBounds.Right, num);
					}
					finally
					{
						((IDisposable)val)?.Dispose();
					}
				}
				if (!office2007SliderStateColorTable3.TrackLineLightColor.IsEmpty)
				{
					num++;
					num2++;
					Pen val2 = new Pen(office2007SliderStateColorTable3.TrackLineLightColor);
					try
					{
						if (sliderItem.TrackMarker)
						{
							graphics.DrawLine(val2, num2, num - StiScale2.I3, num2, num + StiScale2.I3);
						}
						graphics.DrawLine(val2, labelBounds.X - StiScale2.I1, num, labelBounds.Right, num);
					}
					finally
					{
						((IDisposable)val2)?.Dispose();
					}
				}
				if (!office2007SliderStateColorTable3.TrackLineColor.IsEmpty && sliderItem.TrackMarker)
				{
					num--;
					num2--;
					Pen val3 = new Pen(office2007SliderStateColorTable3.TrackLineColor);
					try
					{
						graphics.DrawLine(val3, num2, num - StiScale2.I3, num2, num + StiScale2.I3);
					}
					finally
					{
						((IDisposable)val3)?.Dispose();
					}
				}
				graphics.SmoothingMode = smoothingMode;
				int num3 = 0;
				int num4 = (labelBounds.Width - StiScale2.XXI(11.0)) / 2;
				int num5 = sliderItem.Medium - sliderItem.Minimum;
				int num6 = sliderItem.Maximum - sliderItem.Medium;
				int value = sliderItem.Value;
				num3 = ((sliderItem.Value >= sliderItem.Medium) ? ((int)((float)num4 + (float)num4 * (float)(sliderItem.Value - sliderItem.Medium) / (float)(sliderItem.Maximum - sliderItem.Medium))) : ((int)((float)num4 * (float)(sliderItem.Value - sliderItem.Minimum) / (float)(sliderItem.Medium - sliderItem.Minimum))));
				num3++;
				if (e.ItemPaintArgs.RightToLeft)
				{
					num3 = labelBounds.Width - num3 - StiScale2.XXI(11.0);
				}
				Rectangle rectangle = labelBounds;
				rectangle.X += num3;
				rectangle.Width = StiScale2.XXI(11.0);
				rectangle.Height = StiScale2.YYI(15.0);
				rectangle.Y += (labelBounds.Height - rectangle.Height) / 2;
				PaintSliderPart(office2007SliderStateColorTable3, rectangle, graphics, eSliderPart.TrackArea, sliderItem.SliderOrientation, effectiveStyle);
				sliderItem.TrackBounds = rectangle;
			}
			else
			{
				int num7 = labelBounds.X + labelBounds.Width / 2 - 1;
				int num8 = labelBounds.Y + labelBounds.Height / 2;
				if (!office2007SliderStateColorTable3.TrackLineColor.IsEmpty)
				{
					Pen val4 = new Pen(office2007SliderStateColorTable3.TrackLineColor);
					try
					{
						graphics.DrawLine(val4, num7, labelBounds.Y - 1, num7, labelBounds.Bottom);
					}
					finally
					{
						((IDisposable)val4)?.Dispose();
					}
				}
				if (!office2007SliderStateColorTable3.TrackLineLightColor.IsEmpty)
				{
					num7++;
					num8++;
					Pen val5 = new Pen(office2007SliderStateColorTable3.TrackLineLightColor);
					try
					{
						if (sliderItem.TrackMarker)
						{
							graphics.DrawLine(val5, num7 - 3, num8, num7 + 3, num8);
						}
						graphics.DrawLine(val5, num7, labelBounds.Y - 1, num7, labelBounds.Bottom);
					}
					finally
					{
						((IDisposable)val5)?.Dispose();
					}
				}
				if (!office2007SliderStateColorTable3.TrackLineColor.IsEmpty && sliderItem.TrackMarker)
				{
					num7--;
					num8--;
					Pen val6 = new Pen(office2007SliderStateColorTable3.TrackLineColor);
					try
					{
						graphics.DrawLine(val6, num7 - 3, num8, num7 + 3, num8);
					}
					finally
					{
						((IDisposable)val6)?.Dispose();
					}
				}
				graphics.SmoothingMode = smoothingMode;
				int num9 = 0;
				num9 = labelBounds.Height - 16 - (int)((float)(labelBounds.Height - 11) * ((sliderItem.Maximum - sliderItem.Minimum == 0) ? 0f : ((float)(sliderItem.Value - sliderItem.Minimum) / (float)(sliderItem.Maximum - sliderItem.Minimum))));
				Rectangle rectangle2 = labelBounds;
				rectangle2.Y += num9;
				rectangle2.Width = 15;
				rectangle2.Height = 11;
				rectangle2.X += (labelBounds.Width - rectangle2.Width) / 2;
				PaintSliderPart(office2007SliderStateColorTable3, rectangle2, graphics, eSliderPart.TrackArea, sliderItem.SliderOrientation, effectiveStyle);
				sliderItem.TrackBounds = rectangle2;
			}
		}
		labelBounds = sliderItem.DecreaseBounds;
		if (!labelBounds.IsEmpty)
		{
			labelBounds.Offset(displayRectangle.Location);
			labelBounds.Width--;
			labelBounds.Height--;
			PaintSliderPart(office2007SliderStateColorTable, labelBounds, graphics, eSliderPart.DecreaseButton, sliderItem.SliderOrientation, effectiveStyle);
		}
		labelBounds = sliderItem.IncreaseBounds;
		if (!labelBounds.IsEmpty)
		{
			labelBounds.Offset(displayRectangle.Location);
			labelBounds.Width--;
			labelBounds.Height--;
			PaintSliderPart(office2007SliderStateColorTable2, labelBounds, graphics, eSliderPart.IncreaseButton, sliderItem.SliderOrientation, effectiveStyle);
		}
		base.Paint(e);
	}

	private string GetText(SliderItem item)
	{
		return item.Text;
	}

	private void PaintSliderPart(Office2007SliderStateColorTable ct, Rectangle r, Graphics g, eSliderPart part, eOrientation orientation, eDotNetBarStyle effectiveStyle)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected O, but got Unknown
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Expected O, but got Unknown
		//IL_03f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f9: Expected O, but got Unknown
		//IL_04da: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e1: Expected O, but got Unknown
		//IL_03de: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Expected O, but got Unknown
		//IL_061f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0626: Expected O, but got Unknown
		//IL_045b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0460: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Expected O, but got Unknown
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Expected O, but got Unknown
		//IL_0333: Unknown result type (might be due to invalid IL or missing references)
		//IL_033a: Expected O, but got Unknown
		//IL_064b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0650: Unknown result type (might be due to invalid IL or missing references)
		//IL_0662: Unknown result type (might be due to invalid IL or missing references)
		//IL_0669: Expected O, but got Unknown
		//IL_04ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b3: Expected O, but got Unknown
		//IL_07ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f3: Expected O, but got Unknown
		//IL_0888: Unknown result type (might be due to invalid IL or missing references)
		//IL_088f: Expected O, but got Unknown
		//IL_091a: Unknown result type (might be due to invalid IL or missing references)
		if (r.Width <= 0 || r.Height <= 0)
		{
			return;
		}
		switch (part)
		{
		case eSliderPart.IncreaseButton:
		case eSliderPart.DecreaseButton:
		{
			r.Inflate(-1, -1);
			SmoothingMode smoothingMode3 = g.SmoothingMode;
			g.SmoothingMode = (SmoothingMode)4;
			if (ct.PartBackground.Colors.Count > 0)
			{
				Brush val10 = DisplayHelp.CreateBrush(r, ct.PartBackground);
				try
				{
					g.FillEllipse(val10, r);
				}
				finally
				{
					((IDisposable)val10)?.Dispose();
				}
			}
			if (!ct.PartBorderColor.IsEmpty)
			{
				Pen val11 = new Pen(ct.PartBorderColor, 1f);
				try
				{
					g.DrawEllipse(val11, r);
				}
				finally
				{
					((IDisposable)val11)?.Dispose();
				}
			}
			r.Inflate(1, 1);
			if (!ct.PartBorderLightColor.IsEmpty)
			{
				Pen val12 = new Pen(ct.PartBorderLightColor, 1f);
				try
				{
					g.DrawEllipse(val12, r);
				}
				finally
				{
					((IDisposable)val12)?.Dispose();
				}
			}
			g.SmoothingMode = (SmoothingMode)3;
			switch (part)
			{
			case eSliderPart.DecreaseButton:
			{
				Size size2 = new Size(StiScale2.XXI(9.0), StiScale2.YYI(3.0));
				Rectangle rectangle3 = new Rectangle(r.X + (r.Width - size2.Width) / 2, r.Y + (r.Height - size2.Height) / 2, size2.Width, size2.Height);
				if (!ct.PartForeLightColor.IsEmpty)
				{
					Pen val15 = new Pen(ct.PartForeLightColor, 1f);
					try
					{
						g.DrawRectangle(val15, rectangle3);
					}
					finally
					{
						((IDisposable)val15)?.Dispose();
					}
				}
				if (!ct.PartForeColor.IsEmpty)
				{
					rectangle3.Offset(1, 1);
					rectangle3.Width--;
					rectangle3.Height--;
					SolidBrush val16 = new SolidBrush(ct.PartForeColor);
					try
					{
						g.FillRectangle((Brush)(object)val16, rectangle3);
					}
					finally
					{
						((IDisposable)val16)?.Dispose();
					}
				}
				break;
			}
			case eSliderPart.IncreaseButton:
			{
				Size size = new Size(StiScale2.XXI(8.0), StiScale2.YYI(8.0));
				Rectangle rectangle2 = new Rectangle(r.X + (r.Width - size.Width) / 2, r.Y + (r.Height - size.Height) / 2, size.Width, size.Height);
				if (!ct.TrackLineLightColor.IsEmpty)
				{
					SolidBrush val13 = new SolidBrush(ct.PartForeLightColor);
					try
					{
						g.FillRectangle((Brush)(object)val13, new Rectangle(rectangle2.X, rectangle2.Y + StiScale2.I3, rectangle2.Width + StiScale2.I2, StiScale2.I4));
						g.FillRectangle((Brush)(object)val13, new Rectangle(rectangle2.X + StiScale2.I3, rectangle2.Y, StiScale2.I4, StiScale2.I3));
						g.FillRectangle((Brush)(object)val13, new Rectangle(rectangle2.X + StiScale2.I3, rectangle2.Bottom - StiScale2.I1, StiScale2.I4, StiScale2.I3));
					}
					finally
					{
						((IDisposable)val13)?.Dispose();
					}
				}
				if (!ct.TrackLineColor.IsEmpty)
				{
					SolidBrush val14 = new SolidBrush(ct.PartForeColor);
					try
					{
						g.FillRectangle((Brush)(object)val14, new Rectangle(rectangle2.X + StiScale2.I1, rectangle2.Y + StiScale2.I4, rectangle2.Width, StiScale2.I2));
						g.FillRectangle((Brush)(object)val14, new Rectangle(rectangle2.X + StiScale2.I4, rectangle2.Y + StiScale2.I1, StiScale2.I2, StiScale2.I3));
						g.FillRectangle((Brush)(object)val14, new Rectangle(rectangle2.X + StiScale2.I4, rectangle2.Bottom - StiScale2.I2, StiScale2.I2, StiScale2.I3));
					}
					finally
					{
						((IDisposable)val14)?.Dispose();
					}
				}
				break;
			}
			}
			g.SmoothingMode = smoothingMode3;
			break;
		}
		case eSliderPart.TrackArea:
			if (orientation == eOrientation.Vertical)
			{
				Matrix val = new Matrix();
				val.RotateAt(-90f, new PointF(r.X, r.Bottom));
				val.Translate((float)r.Height, (float)(r.Width - r.Height), (MatrixOrder)1);
				g.Transform = val;
			}
			if (effectiveStyle == eDotNetBarStyle.Metro)
			{
				if (!ct.PartBackground.IsEmpty)
				{
					SmoothingMode smoothingMode = g.SmoothingMode;
					g.SmoothingMode = (SmoothingMode)0;
					Rectangle rectangle = new Rectangle(r.X + (r.Width - StiScale2.I3) / 2, r.Y + StiScale2.I2, StiScale2.I3, r.Height - StiScale2.I4);
					Brush val2 = DisplayHelp.CreateBrush(rectangle, ct.PartBackground);
					try
					{
						g.FillRectangle(val2, rectangle);
					}
					finally
					{
						((IDisposable)val2)?.Dispose();
					}
					g.SmoothingMode = smoothingMode;
				}
			}
			else
			{
				GraphicsPath val3 = new GraphicsPath();
				try
				{
					val3.AddLine(r.X, r.Y, r.X + StiScale2.XXI(11.0), r.Y);
					val3.AddLine(r.X + StiScale2.XXI(11.0), r.Y, r.X + StiScale2.XXI(11.0), r.Y + StiScale2.YYI(9.0));
					val3.AddLine(r.X + StiScale2.XXI(11.0), r.Y + StiScale2.YYI(9.0), r.X + StiScale2.XXI(6.0), r.Y + StiScale2.YYI(15.0));
					val3.AddLine(r.X + StiScale2.XXI(5.0), r.Y + StiScale2.YYI(15.0), r.X, r.Y + StiScale2.YYI(10.0));
					val3.CloseAllFigures();
					SolidBrush val4 = new SolidBrush(ct.PartBorderLightColor);
					try
					{
						g.FillPath((Brush)(object)val4, val3);
					}
					finally
					{
						((IDisposable)val4)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
				SmoothingMode smoothingMode2 = g.SmoothingMode;
				g.SmoothingMode = (SmoothingMode)4;
				r.Offset(1, 1);
				GraphicsPath val5 = new GraphicsPath();
				try
				{
					val5.AddLine(r.X, r.Y, r.X + (int)Math.Ceiling(8.0 * StiScale2.X), r.Y);
					val5.AddLine(r.X + (int)Math.Ceiling(8.0 * StiScale2.X), r.Y + (int)Math.Ceiling(8.0 * StiScale2.Y), r.X + (int)Math.Ceiling(4.0 * StiScale2.X), r.Y + (int)Math.Ceiling(12.0 * StiScale2.Y));
					val5.AddLine(r.X, r.Y + (int)Math.Ceiling(8.0 * StiScale2.Y), r.X, r.Y);
					val5.CloseAllFigures();
					if (ct.PartBackground.Colors.Count > 0)
					{
						Brush val6 = DisplayHelp.CreateBrush(Rectangle.Ceiling(val5.GetBounds()), ct.PartBackground);
						try
						{
							g.FillPath(val6, val5);
						}
						finally
						{
							((IDisposable)val6)?.Dispose();
						}
					}
					Pen val7 = new Pen(ct.PartBorderColor, 1f);
					try
					{
						g.DrawPath(val7, val5);
					}
					finally
					{
						((IDisposable)val7)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)val5)?.Dispose();
				}
				Pen val8 = new Pen(Color.FromArgb(200, ct.PartForeColor), 1f);
				try
				{
					g.DrawLine(val8, r.X + (int)Math.Ceiling(4.0 * StiScale2.X), r.Y + (int)Math.Ceiling(3.0 * StiScale2.Y), r.X + (int)Math.Ceiling(4.0 * StiScale2.X), r.Y + (int)Math.Ceiling(8.0 * StiScale2.Y));
				}
				finally
				{
					((IDisposable)val8)?.Dispose();
				}
				Pen val9 = new Pen(ct.PartForeLightColor, 1f);
				try
				{
					g.DrawLine(val9, r.X + (int)Math.Ceiling(5.0 * StiScale2.X), r.Y + (int)Math.Ceiling(5.0 * StiScale2.Y), r.X + (int)Math.Ceiling(5.0 * StiScale2.X), r.Y + (int)Math.Ceiling(7.0 * StiScale2.Y));
				}
				finally
				{
					((IDisposable)val9)?.Dispose();
				}
				g.SmoothingMode = smoothingMode2;
			}
			if (orientation == eOrientation.Vertical)
			{
				g.ResetTransform();
			}
			break;
		}
	}
}
