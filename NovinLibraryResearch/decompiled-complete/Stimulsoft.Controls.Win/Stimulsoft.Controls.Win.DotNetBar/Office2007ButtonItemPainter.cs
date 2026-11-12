using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class Office2007ButtonItemPainter : Office2003ButtonItemPainter, IOffice2007Painter
{
	private Office2007ColorTable m_ColorTable;

	private static RoundRectangleShapeDescriptor _DefaultOffice2007Shape = new RoundRectangleShapeDescriptor(2);

	private static RoundRectangleShapeDescriptor _DefaultOffice2007LeftShape = new RoundRectangleShapeDescriptor(2, 0, 2, 0);

	private static RoundRectangleShapeDescriptor _DefaultOffice2007RightShape = new RoundRectangleShapeDescriptor(0, 2, 0, 2);

	private static RoundRectangleShapeDescriptor _DefaultOffice2007TopShape = new RoundRectangleShapeDescriptor(2, 2, 0, 0);

	private static RoundRectangleShapeDescriptor _DefaultOffice2007BottomShape = new RoundRectangleShapeDescriptor(0, 0, 2, 2);

	private static Type ButtonColorTableType = typeof(Office2007ButtonItemColorTable);

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

	private bool IsOffice2003Rendering(ButtonItem button, ItemPaintArgs pa)
	{
		if (button.ColorTable == eButtonColor.Flat || pa.Owner is DotNetBarManager)
		{
			return true;
		}
		return false;
	}

	public override void PaintButtonMouseOver(ButtonItem button, ItemPaintArgs pa, CompositeImage image, Rectangle r)
	{
		if (IsOffice2003Rendering(button, pa))
		{
			base.PaintButtonMouseOver(button, pa, image, r);
		}
		else if (!(button is Office2007StartButton))
		{
			PaintState(button, pa, image, r, button.IsMouseDown);
		}
	}

	public override void PaintButtonCheck(ButtonItem button, ItemPaintArgs pa, CompositeImage image, Rectangle r)
	{
		if (IsOffice2003Rendering(button, pa))
		{
			base.PaintButtonCheck(button, pa, image, r);
		}
		else if (IsOnMenu(button, pa))
		{
			base.PaintButtonCheck(button, pa, image, r);
		}
	}

	protected virtual IShapeDescriptor GetButtonShape(ButtonItem button, ItemPaintArgs pa)
	{
		IShapeDescriptor result = GetButtonShape(button);
		if (button is ColorPickerDropDown)
		{
			result = new RoundRectangleShapeDescriptor(0);
		}
		else if (pa.ContainerControl is ButtonX)
		{
			result = ((ButtonX)(object)pa.ContainerControl).GetButtonShape();
		}
		else if (pa.ContainerControl is NavigationBar)
		{
			result = ((NavigationBar)(object)pa.ContainerControl).ButtonShape;
		}
		return result;
	}

	protected virtual void PaintState(ButtonItem button, ItemPaintArgs pa, CompositeImage image, Rectangle r, bool isMouseDown)
	{
		if (r.IsEmpty)
		{
			return;
		}
		bool isOnMenu = pa.IsOnMenu;
		Office2007ColorTable colorTable = ColorTable;
		Graphics graphics = pa.Graphics;
		IShapeDescriptor shapeDescriptor = GetButtonShape(button, pa);
		eButtonContainer buttonContainerQualifier = GetButtonContainerQualifier(pa, button);
		Office2007ButtonItemColorTable colorTable2 = GetColorTable(button, buttonContainerQualifier);
		Region clip = graphics.Clip;
		try
		{
			graphics.SetClip(r, (CombineMode)1);
			Office2007ButtonItemStateColorTable stateColors = colorTable2.Default;
			if (IsOnMenu(button, pa))
			{
				Rectangle sideRect = new Rectangle(button.DisplayRectangle.Left, button.DisplayRectangle.Top, button.ImageDrawRect.Right, button.DisplayRectangle.Height);
				if (pa.RightToLeft)
				{
					sideRect = new Rectangle(button.DisplayRectangle.Right - button.ImageDrawRect.Width, button.DisplayRectangle.Top, button.ImageDrawRect.Width, button.DisplayRectangle.Height);
				}
				PaintMenuItemSide(button, pa, sideRect);
			}
			if (!ButtonItemPainter.IsItemEnabled(button, pa))
			{
				if (!isOnMenu)
				{
					PaintStateBackground(button, graphics, colorTable2.Disabled, r, shapeDescriptor, pa.IsDefaultButton, paintBorder: true);
				}
				graphics.Clip = clip;
				return;
			}
			bool flag = false;
			bool flag2 = false;
			if (!button.DesignMode)
			{
				if (button.IsMouseOver && button.HotTrackingStyle != eHotTrackingStyle.Image && button.HotTrackingStyle != eHotTrackingStyle.None)
				{
					stateColors = colorTable2.MouseOver;
					flag = true;
				}
				if (isMouseDown && button.HotTrackingStyle != eHotTrackingStyle.Image && button.HotTrackingStyle != eHotTrackingStyle.None)
				{
					if (colorTable2.Pressed != null)
					{
						stateColors = colorTable2.Pressed;
					}
				}
				else if (button.Checked && !button.IsMouseOver && !IsOnMenu(button, pa))
				{
					stateColors = colorTable2.Checked;
				}
				else if (button.Expanded && button.HotTrackingStyle != eHotTrackingStyle.Image && (flag || !pa.IsOnMenu))
				{
					stateColors = colorTable2.Expanded;
					flag2 = true;
				}
			}
			else if (button.Checked && !IsOnMenu(button, pa))
			{
				stateColors = colorTable2.Checked;
			}
			else if (button.Expanded && button.HotTrackingStyle != eHotTrackingStyle.Image)
			{
				stateColors = colorTable2.Expanded;
				flag2 = true;
			}
			Rectangle totalSubItemsRect = GetTotalSubItemsRect(button);
			bool flag3 = (flag || flag2) && !button.AutoExpandOnClick && !totalSubItemsRect.IsEmpty && !IsOnMenu(button, pa);
			eDotNetBarStyle effectiveStyle = button.EffectiveStyle;
			totalSubItemsRect.Offset(button.DisplayRectangle.Location);
			if (flag3 && !button.Checked && (effectiveStyle == eDotNetBarStyle.Office2010 || effectiveStyle == eDotNetBarStyle.Windows7 || effectiveStyle == eDotNetBarStyle.Metro))
			{
				Rectangle[] array = DisplayHelp.ExcludeRectangle(r, totalSubItemsRect);
				if (array.Length != 0 && (button.IsMouseOverExpand || button.Expanded))
				{
					array = DisplayHelp.ExcludeRectangle(r, array[0]);
				}
				if (array.Length != 0)
				{
					PaintStateBackground(button, graphics, colorTable2.MouseOverSplitInactive, r, shapeDescriptor, pa.IsDefaultButton, !pa.IsOnNavigationBar);
					if (array[0].X == r.X && array[0].Right < r.Right)
					{
						shapeDescriptor = _DefaultOffice2007LeftShape;
					}
					else if (array[0].X > r.X && array[0].Right == r.Right)
					{
						shapeDescriptor = _DefaultOffice2007RightShape;
					}
					else if (array[0].Y == r.Y && array[0].Bottom < r.Bottom)
					{
						shapeDescriptor = _DefaultOffice2007TopShape;
					}
					else if (array[0].Y > r.Y && array[0].Bottom == r.Bottom)
					{
						shapeDescriptor = _DefaultOffice2007BottomShape;
					}
					PaintStateBackground(button, graphics, stateColors, array[0], shapeDescriptor, pa.IsDefaultButton, !pa.IsOnNavigationBar);
				}
				else
				{
					PaintStateBackground(button, graphics, stateColors, r, shapeDescriptor, pa.IsDefaultButton, !pa.IsOnNavigationBar);
				}
			}
			else
			{
				PaintStateBackground(button, graphics, stateColors, r, shapeDescriptor, pa.IsDefaultButton, !pa.IsOnNavigationBar);
			}
			if (flag3 && effectiveStyle != eDotNetBarStyle.Office2010 && effectiveStyle != eDotNetBarStyle.Windows7 && effectiveStyle != eDotNetBarStyle.Metro)
			{
				Brush overlayInactiveBrush = GetOverlayInactiveBrush();
				try
				{
					Rectangle displayRectangle = button.DisplayRectangle;
					displayRectangle.Inflate(-1, -1);
					if (!button.IsMouseOverExpand && !flag2)
					{
						totalSubItemsRect.Inflate(-1, -1);
						graphics.SetClip(totalSubItemsRect, (CombineMode)1);
						GraphicsPath shape = shapeDescriptor.GetShape(displayRectangle);
						if (shape != null)
						{
							graphics.FillPath(overlayInactiveBrush, shape);
							shape.Dispose();
						}
					}
					else if (!flag || button.IsMouseOverExpand)
					{
						graphics.SetClip(totalSubItemsRect, (CombineMode)4);
						GraphicsPath shape2 = shapeDescriptor.GetShape(displayRectangle);
						if (shape2 != null)
						{
							graphics.FillPath(overlayInactiveBrush, shape2);
							shape2.Dispose();
						}
					}
				}
				finally
				{
					((IDisposable)overlayInactiveBrush)?.Dispose();
				}
			}
			graphics.Clip = clip;
		}
		finally
		{
			if (clip != null)
			{
				clip.Dispose();
			}
		}
	}

	protected virtual void PaintStateBackground(ButtonItem button, Graphics g, Office2007ButtonItemStateColorTable stateColors, Rectangle r, IShapeDescriptor shape, bool isDefault, bool paintBorder)
	{
		PaintBackground(g, stateColors, r, shape, isDefault, paintBorder);
	}

	public static void PaintBackground(Graphics g, Office2007ButtonItemStateColorTable stateColors, Rectangle r, IShapeDescriptor shape)
	{
		PaintBackground(g, stateColors, r, shape, isDefault: false);
	}

	public static void PaintBackground(Graphics g, Office2007ButtonItemStateColorTable stateColors, Rectangle r, IShapeDescriptor shape, bool isDefault)
	{
		PaintBackground(g, stateColors, r, shape, isDefault, paintBorder: true);
	}

	public static void PaintBackground(Graphics g, Office2007ButtonItemStateColorTable stateColors, Rectangle r, IShapeDescriptor shape, bool isDefault, bool paintBorder)
	{
		//IL_052b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0530: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Expected O, but got Unknown
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected O, but got Unknown
		//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0303: Expected O, but got Unknown
		//IL_030e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0315: Expected O, but got Unknown
		//IL_036c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0373: Expected O, but got Unknown
		//IL_0452: Unknown result type (might be due to invalid IL or missing references)
		//IL_0459: Expected O, but got Unknown
		//IL_0464: Unknown result type (might be due to invalid IL or missing references)
		//IL_046b: Expected O, but got Unknown
		//IL_04c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c9: Expected O, but got Unknown
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_021c: Expected O, but got Unknown
		//IL_06d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Expected O, but got Unknown
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Expected O, but got Unknown
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Expected O, but got Unknown
		//IL_05da: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e1: Expected O, but got Unknown
		float num = 0.35f;
		float num2 = 0.65f;
		if (!shape.CanDrawShape(r) || stateColors == null)
		{
			return;
		}
		Rectangle rectangle = r;
		Rectangle rectangle2 = new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, (int)((float)rectangle.Height * num));
		if (!stateColors.OuterBorder.IsEmpty && paintBorder)
		{
			rectangle.Width--;
		}
		GraphicsPath shape2 = shape.GetShape(rectangle);
		if (stateColors.Background != null)
		{
			if (shape2 != null)
			{
				if (stateColors.Background.End.IsEmpty)
				{
					SolidBrush val = new SolidBrush(stateColors.Background.Start);
					try
					{
						g.FillPath((Brush)(object)val, shape2);
					}
					finally
					{
						((IDisposable)val)?.Dispose();
					}
				}
				else
				{
					LinearGradientBrush val2 = new LinearGradientBrush(rectangle, stateColors.Background.Start, stateColors.Background.End, (float)stateColors.Background.GradientAngle);
					try
					{
						g.FillPath((Brush)(object)val2, shape2);
					}
					finally
					{
						((IDisposable)val2)?.Dispose();
					}
				}
				shape2.Dispose();
			}
		}
		else
		{
			if (shape2 != null && stateColors.TopBackground != null && stateColors.BottomBackground != null)
			{
				if (stateColors.TopBackground.End.IsEmpty && stateColors.BottomBackground.End.IsEmpty)
				{
					if (stateColors.TopBackground.Start == stateColors.BottomBackground.Start)
					{
						SolidBrush val3 = new SolidBrush(stateColors.TopBackground.Start);
						try
						{
							g.FillPath((Brush)(object)val3, shape2);
						}
						finally
						{
							((IDisposable)val3)?.Dispose();
						}
					}
					else
					{
						LinearGradientBrush val4 = new LinearGradientBrush(rectangle, stateColors.TopBackground.Start, stateColors.BottomBackground.Start, (float)stateColors.TopBackground.GradientAngle);
						try
						{
							g.FillPath((Brush)(object)val4, shape2);
						}
						finally
						{
							((IDisposable)val4)?.Dispose();
						}
					}
				}
				else
				{
					LinearGradientBrush val5 = new LinearGradientBrush(rectangle, stateColors.TopBackground.Start, stateColors.BottomBackground.End, (float)stateColors.TopBackground.GradientAngle);
					try
					{
						ColorBlend val6 = new ColorBlend(4);
						val6.Colors = new Color[4]
						{
							stateColors.TopBackground.Start,
							stateColors.TopBackground.End,
							stateColors.BottomBackground.Start,
							stateColors.BottomBackground.End
						};
						val6.Positions = new float[4] { 0f, num, num, 1f };
						val5.InterpolationColors = val6;
						g.FillPath((Brush)(object)val5, shape2);
					}
					finally
					{
						((IDisposable)val5)?.Dispose();
					}
				}
				shape2.Dispose();
			}
			if (stateColors.TopBackgroundHighlight != null && !stateColors.TopBackgroundHighlight.IsEmpty)
			{
				Rectangle rectangle3 = new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
				GraphicsPath val7 = new GraphicsPath();
				val7.AddEllipse(rectangle3);
				PathGradientBrush val8 = new PathGradientBrush(val7);
				val8.CenterColor = stateColors.TopBackgroundHighlight.Start;
				val8.SurroundColors = new Color[1] { stateColors.TopBackgroundHighlight.End };
				val8.CenterPoint = new PointF(rectangle3.X + rectangle3.Width / 2, rectangle.Bottom);
				Blend val9 = new Blend();
				val9.Factors = new float[3] { 0f, 0.5f, 1f };
				val9.Positions = new float[3] { 0f, 0.4f, 1f };
				val8.Blend = val9;
				g.FillRectangle((Brush)(object)val8, rectangle2);
				((Brush)val8).Dispose();
				val7.Dispose();
			}
			int num3 = (int)((float)rectangle.Height * num2);
			rectangle2 = new Rectangle(rectangle.X, rectangle.Y + rectangle2.Height, rectangle.Width, rectangle.Height - rectangle2.Height);
			if (stateColors.BottomBackgroundHighlight != null && !stateColors.BottomBackgroundHighlight.IsEmpty)
			{
				Rectangle rectangle4 = new Rectangle(rectangle.X, rectangle.Y + num3 - 2, rectangle.Width, rectangle.Height + 4);
				GraphicsPath val10 = new GraphicsPath();
				val10.AddEllipse(rectangle4);
				PathGradientBrush val11 = new PathGradientBrush(val10);
				val11.CenterColor = stateColors.BottomBackgroundHighlight.Start;
				val11.SurroundColors = new Color[1] { stateColors.BottomBackgroundHighlight.End };
				val11.CenterPoint = new PointF(rectangle4.X + rectangle4.Width / 2, rectangle.Bottom);
				Blend val12 = new Blend();
				val12.Factors = new float[3] { 0f, 0.5f, 0.6f };
				val12.Positions = new float[3] { 0f, 0.4f, 1f };
				val11.Blend = val12;
				g.FillRectangle((Brush)(object)val11, rectangle2);
				((Brush)val11).Dispose();
				val10.Dispose();
			}
		}
		if (!paintBorder)
		{
			return;
		}
		SmoothingMode smoothingMode = g.SmoothingMode;
		g.SmoothingMode = (SmoothingMode)4;
		if (!stateColors.OuterBorder.IsEmpty && stateColors.OuterBorderWidth > 0)
		{
			Rectangle rectangle5 = r;
			rectangle5.Width--;
			rectangle5.Height--;
			if (shape is EllipticalShapeDescriptor && stateColors.OuterBorder.End.IsEmpty)
			{
				rectangle5.Width -= stateColors.OuterBorderWidth / 2;
				rectangle5.Height -= stateColors.OuterBorderWidth / 2;
				Pen val13 = new Pen(stateColors.OuterBorder.Start, (float)stateColors.OuterBorderWidth);
				try
				{
					g.DrawEllipse(val13, rectangle5);
				}
				finally
				{
					((IDisposable)val13)?.Dispose();
				}
			}
			else
			{
				GraphicsPath shape3 = shape.GetShape(rectangle5);
				if (shape3 != null)
				{
					DisplayHelp.DrawGradientPath(g, shape3, r, stateColors.OuterBorder, stateColors.OuterBorderWidth);
					shape3.Dispose();
				}
			}
			if (isDefault)
			{
				Color color = Color.FromArgb(128, stateColors.OuterBorder.Start);
				r.Inflate(-1, -1);
				DisplayHelp.DrawRectangle(g, color, r);
				r.Inflate(-1, -1);
				DisplayHelp.DrawRectangle(g, color, r);
			}
		}
		if (!isDefault && !stateColors.InnerBorder.IsEmpty)
		{
			Rectangle bounds = r;
			bounds.Inflate(-1, -1);
			bounds.Width--;
			bounds.Height--;
			GraphicsPath innerShape = shape.GetInnerShape(bounds, 1);
			try
			{
				DisplayHelp.DrawGradientPath(g, innerShape, bounds, stateColors.InnerBorder, 1);
			}
			finally
			{
				((IDisposable)innerShape)?.Dispose();
			}
		}
		g.SmoothingMode = smoothingMode;
	}

	public override void PaintButtonBackground(ButtonItem button, ItemPaintArgs pa, CompositeImage image)
	{
		if (IsOffice2003Rendering(button, pa))
		{
			base.PaintButtonBackground(button, pa, image);
			return;
		}
		if (button is ApplicationButton && m_ColorTable.RibbonControl.StartButtonDefault != null)
		{
			Image val = m_ColorTable.RibbonControl.StartButtonDefault;
			if (button.IsMouseDown || button.Expanded)
			{
				val = m_ColorTable.RibbonControl.StartButtonPressed;
			}
			else if (button.IsMouseOver)
			{
				val = m_ColorTable.RibbonControl.StartButtonMouseOver;
			}
			if (val != null)
			{
				if (Office2007ColorTable.CloneImagesOnAccess)
				{
					object obj = val.Clone();
					val = (Image)((obj is Image) ? obj : null);
				}
				pa.Graphics.DrawImageUnscaled(val, new Point(button.LeftInternal + (button.WidthInternal - val.Width) / 2, button.TopInternal + (button.HeightInternal - val.Height) / 2));
				if (Office2007ColorTable.CloneImagesOnAccess)
				{
					val.Dispose();
				}
			}
		}
		PaintState(button, pa, image, button.DisplayRectangle, button.IsMouseDown);
	}

	private Brush GetOverlayInactiveBrush()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		return (Brush)new SolidBrush(Color.FromArgb(76, Color.White));
	}

	private static IShapeDescriptor GetButtonShape(ButtonItem button)
	{
		if (button.Shape != null)
		{
			return button.Shape;
		}
		return _DefaultOffice2007Shape;
	}

	public override void PaintExpandButton(ButtonItem button, ItemPaintArgs pa)
	{
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_08fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_090e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0915: Expected O, but got Unknown
		//IL_08d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_08eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_092e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b96: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b9d: Expected O, but got Unknown
		//IL_0bd4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bdb: Expected O, but got Unknown
		if (IsOffice2003Rendering(button, pa))
		{
			base.PaintExpandButton(button, pa);
			return;
		}
		Graphics graphics = pa.Graphics;
		bool flag = IsOnMenu(button, pa);
		Rectangle displayRectangle = button.DisplayRectangle;
		bool isMouseOver = button.IsMouseOver;
		bool flag2 = ButtonItemPainter.IsItemEnabled(button, pa);
		Color color = Color.Empty;
		eButtonContainer buttonContainerQualifier = GetButtonContainerQualifier(pa, button);
		Office2007ButtonItemColorTable colorTable = GetColorTable(button, buttonContainerQualifier);
		if (buttonContainerQualifier == eButtonContainer.RibbonStrip)
		{
			color = GetTextColor(button, pa);
		}
		if (color.IsEmpty)
		{
			color = GetTextColor(button, pa, colorTable, isExpandPart: true);
		}
		SolidBrush val = new SolidBrush(color);
		try
		{
			if ((button.SubItems.Count <= 0 && button.PopupType != ePopupType.Container && !button.ShowArrow) || !button.ShowSubItems)
			{
				return;
			}
			if (flag)
			{
				Point[] array = new Point[3];
				if (!pa.RightToLeft)
				{
					int verticalScrollBarWidth = SystemInformation.VerticalScrollBarWidth;
					Rectangle rect = new Rectangle(displayRectangle.Right - verticalScrollBarWidth, displayRectangle.Y, verticalScrollBarWidth, displayRectangle.Height);
					StiButtonHelper.DrawRightArrow(graphics, rect, flag2, color, pa.Colors.ItemDisabledText);
					return;
				}
				array[0].X = displayRectangle.Left + StiScale2.I8;
				array[0].Y = displayRectangle.Top + (displayRectangle.Height - StiScale2.I8) / 2;
				array[1].X = array[0].X;
				array[1].Y = array[0].Y + StiScale2.I8;
				array[2].X = array[0].X - StiScale2.I4;
				array[2].Y = array[0].Y + StiScale2.I4;
				SmoothingMode smoothingMode = graphics.SmoothingMode;
				graphics.SmoothingMode = (SmoothingMode)3;
				graphics.FillPolygon((Brush)(object)val, array);
				graphics.SmoothingMode = smoothingMode;
			}
			else if (!button.SubItemsRect.IsEmpty)
			{
				PointF[] array2 = new PointF[3];
				Rectangle subItemsRect = button.SubItemsRect;
				if (pa.IsOnRibbonBar && button.ImagePosition == eImagePosition.Top && !button.SplitButton && button.Text.Length > 0)
				{
					subItemsRect.Y -= StiScale2.I3;
				}
				if (button.PopupSide == ePopupSide.Default)
				{
					if (pa.IsOnMenu)
					{
						if (pa.RightToLeft)
						{
							array2[0].X = displayRectangle.Left + subItemsRect.Left + subItemsRect.Width / 2 + StiScale2.I3;
							array2[0].Y = displayRectangle.Top + subItemsRect.Top + subItemsRect.Height / 2 - StiScale2.I3;
							array2[1].X = array2[0].X;
							array2[1].Y = array2[0].Y + (float)StiScale2.I6;
							array2[2].X = array2[0].X - (float)StiScale2.I3;
							array2[2].Y = array2[0].Y + (float)StiScale2.I3;
						}
						else
						{
							array2[0].X = displayRectangle.Left + subItemsRect.Left + subItemsRect.Width / 2;
							array2[0].Y = displayRectangle.Top + subItemsRect.Top + subItemsRect.Height / 2 - StiScale2.I3;
							array2[1].X = array2[0].X;
							array2[1].Y = array2[0].Y + (float)StiScale2.I6;
							array2[2].X = array2[0].X + (float)StiScale2.I3;
							array2[2].Y = array2[0].Y + (float)StiScale2.I3;
						}
					}
					else
					{
						if (button.Orientation == eOrientation.Horizontal)
						{
							float num = (float)displayRectangle.Left + (float)subItemsRect.Left + (float)(subItemsRect.Width / 2);
							float num2 = (float)displayRectangle.Top + (float)subItemsRect.Top + (float)(subItemsRect.Height / 2);
							int num3 = ((button.ImagePosition != eImagePosition.Top) ? StiScale2.I3 : 0);
							num -= (float)num3;
							num2 -= (float)(num3 / 2);
							Rectangle rect2 = (button.IsCenteredButton ? new Rectangle(displayRectangle.Left + (displayRectangle.Width - num3 * 2) / 2, displayRectangle.Top + (displayRectangle.Height - num3) / 2, num3 * 2, num3) : new Rectangle((int)(num - (float)num3), (int)num2, num3 * 2, num3));
							if (!button.IsCenteredButton && !pa.IsOnRibbonBar && StiScale2.Factor != 1.0)
							{
								rect2.X -= StiScale2.I3;
							}
							if (pa.IsOnRibbonBar && StiScale2.Factor == 1.0)
							{
								rect2.Y--;
							}
							rect2.X += StiScale2.XXI(button.DropArrowShift);
							StiButtonHelper.DrawDownArrow(graphics, rect2, flag2, color, pa.Colors.ItemDisabledText);
							return;
						}
						array2[0].X = displayRectangle.Left + subItemsRect.Left + (subItemsRect.Width - StiScale2.I3) / 2 + StiScale2.I1;
						array2[0].Y = displayRectangle.Top + subItemsRect.Top + (subItemsRect.Height - StiScale2.I5) / 2;
						array2[1].X = array2[0].X;
						array2[1].Y = array2[0].Y + (float)StiScale2.I6;
						array2[2].X = array2[0].X - (float)StiScale2.I3;
						array2[2].Y = array2[0].Y + (float)StiScale2.I3;
					}
				}
				else
				{
					switch (button.PopupSide)
					{
					case ePopupSide.Right:
					{
						int verticalScrollBarWidth3 = SystemInformation.VerticalScrollBarWidth;
						Rectangle rect5 = new Rectangle(displayRectangle.Right - verticalScrollBarWidth3, displayRectangle.Y, verticalScrollBarWidth3, displayRectangle.Height);
						StiButtonHelper.DrawRightArrow(graphics, rect5, flag2, color, pa.Colors.ItemDisabledText);
						return;
					}
					case ePopupSide.Left:
						array2[0].X = displayRectangle.Left + subItemsRect.Left + subItemsRect.Width / 2 + StiScale2.I3;
						array2[0].Y = displayRectangle.Top + subItemsRect.Top + subItemsRect.Height / 2 - StiScale2.I3;
						array2[1].X = array2[0].X;
						array2[1].Y = array2[0].Y + (float)StiScale2.I6;
						array2[2].X = array2[0].X - (float)StiScale2.I3;
						array2[2].Y = array2[0].Y + (float)StiScale2.I3;
						break;
					case ePopupSide.Top:
					{
						int num4 = SystemInformation.VerticalScrollBarWidth - StiScale2.I2;
						Rectangle rect4 = new Rectangle(displayRectangle.Right - num4, displayRectangle.Y, num4, displayRectangle.Height);
						StiButtonHelper.DrawUpArrow(graphics, rect4, flag2, color, pa.Colors.ItemDisabledText);
						return;
					}
					case ePopupSide.Bottom:
					{
						int verticalScrollBarWidth2 = SystemInformation.VerticalScrollBarWidth;
						Rectangle rect3 = new Rectangle(displayRectangle.Right - verticalScrollBarWidth2, displayRectangle.Y, verticalScrollBarWidth2, displayRectangle.Height);
						StiButtonHelper.DrawDownArrow(graphics, rect3, flag2, color, pa.Colors.ItemDisabledText);
						return;
					}
					}
				}
				if (button.SplitButton && !button.TextDrawRect.IsEmpty && button.ImagePosition == eImagePosition.Top)
				{
					array2[0].Y -= StiScale2.I3;
					array2[1].Y -= StiScale2.I3;
					array2[2].Y -= StiScale2.I3;
				}
				if (flag2)
				{
					SmoothingMode smoothingMode2 = graphics.SmoothingMode;
					graphics.SmoothingMode = (SmoothingMode)4;
					graphics.FillPolygon((Brush)(object)val, array2);
					graphics.SmoothingMode = smoothingMode2;
				}
				else
				{
					SmoothingMode smoothingMode3 = graphics.SmoothingMode;
					graphics.SmoothingMode = (SmoothingMode)4;
					SolidBrush val2 = new SolidBrush(pa.Colors.ItemDisabledText);
					try
					{
						graphics.FillPolygon((Brush)(object)val2, array2);
					}
					finally
					{
						((IDisposable)val2)?.Dispose();
					}
					graphics.SmoothingMode = smoothingMode3;
				}
			}
			Rectangle totalSubItemsRect = GetTotalSubItemsRect(button);
			if (!flag2 || totalSubItemsRect.IsEmpty || (!isMouseOver && (!button.Expanded || pa.IsOnMenu)) || button.AutoExpandOnClick)
			{
				return;
			}
			totalSubItemsRect.Offset(displayRectangle.Location);
			Point[] array3 = new Point[4];
			if (pa.ContainerControl is RibbonBar && (button.ImagePosition == eImagePosition.Top || button.ImagePosition == eImagePosition.Bottom))
			{
				array3[0] = new Point(totalSubItemsRect.X + StiScale2.I1, totalSubItemsRect.Y);
				array3[1] = new Point(totalSubItemsRect.Right - StiScale2.I2, totalSubItemsRect.Y);
				array3[2] = new Point(totalSubItemsRect.X + StiScale2.I1, totalSubItemsRect.Y + StiScale2.I1);
				array3[3] = new Point(totalSubItemsRect.Right - StiScale2.I2, totalSubItemsRect.Y + StiScale2.I1);
				if (button.SplitButton)
				{
					array3[0].Y -= StiScale2.I1;
					array3[1].Y -= StiScale2.I1;
					array3[2].Y -= StiScale2.I1;
					array3[3].Y -= StiScale2.I1;
				}
			}
			else
			{
				totalSubItemsRect.Y += StiScale2.I1;
				totalSubItemsRect.Height -= StiScale2.I3;
				if (pa.RightToLeft)
				{
					totalSubItemsRect.X = totalSubItemsRect.Right - StiScale2.I1;
				}
				array3[0] = new Point(totalSubItemsRect.X, totalSubItemsRect.Y);
				array3[1] = new Point(totalSubItemsRect.X, totalSubItemsRect.Bottom);
				array3[2] = new Point(totalSubItemsRect.X + 1, totalSubItemsRect.Y);
				array3[3] = new Point(totalSubItemsRect.X + 1, totalSubItemsRect.Bottom);
			}
			eDotNetBarStyle effectiveStyle = button.EffectiveStyle;
			if ((effectiveStyle != eDotNetBarStyle.Office2010 && effectiveStyle != eDotNetBarStyle.Windows7 && effectiveStyle != eDotNetBarStyle.Metro) || button.Checked)
			{
				Pen val3 = new Pen(colorTable.MouseOver.SplitBorder.Start);
				try
				{
					graphics.DrawLine(val3, array3[0], array3[1]);
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
				Pen val4 = new Pen(colorTable.MouseOver.SplitBorderLight.Start);
				try
				{
					graphics.DrawLine(val4, array3[2], array3[3]);
					return;
				}
				finally
				{
					((IDisposable)val4)?.Dispose();
				}
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	protected override void PaintMenuItemSide(ButtonItem button, ItemPaintArgs pa, Rectangle sideRect)
	{
		if (IsOffice2003Rendering(button, pa))
		{
			base.PaintMenuItemSide(button, pa, sideRect);
			return;
		}
		Graphics graphics = pa.Graphics;
		Office2007MenuColorTable menu = m_ColorTable.Menu;
		Region val = graphics.Clip.Clone();
		graphics.SetClip(sideRect);
		sideRect.Inflate(0, 1);
		if (button.MenuVisibility == eMenuVisibility.VisibleIfRecentlyUsed && !button.RecentlyUsed)
		{
			DisplayHelp.FillRectangle(graphics, sideRect, menu.SideUnused);
		}
		else
		{
			DisplayHelp.FillRectangle(graphics, sideRect, menu.Side);
		}
		if (pa.RightToLeft)
		{
			Point start = new Point(sideRect.X, sideRect.Y);
			DisplayHelp.DrawGradientLine(graphics, start, new Point(start.X, start.Y + sideRect.Height), menu.SideBorder, 1);
			start.X++;
			DisplayHelp.DrawGradientLine(graphics, start, new Point(start.X, start.Y + sideRect.Height), menu.SideBorderLight, 1);
		}
		else
		{
			Point start2 = new Point(sideRect.Right - 2, sideRect.Y);
			DisplayHelp.DrawGradientLine(graphics, start2, new Point(start2.X, start2.Y + sideRect.Height), menu.SideBorder, 1);
			start2.X++;
			DisplayHelp.DrawGradientLine(graphics, start2, new Point(start2.X, start2.Y + sideRect.Height), menu.SideBorderLight, 1);
		}
		if (val != null)
		{
			graphics.Clip = val;
		}
		else
		{
			graphics.ResetClip();
		}
	}

	public override void PaintButtonCheckBackground(ButtonItem button, ItemPaintArgs pa, Rectangle r)
	{
		if (IsOffice2003Rendering(button, pa))
		{
			base.PaintButtonCheckBackground(button, pa, r);
			return;
		}
		Graphics graphics = pa.Graphics;
		bool flag = IsOnMenu(button, pa);
		int cornerSize = 0;
		if (!button.IsMouseOver || flag)
		{
			Rectangle r2 = r;
			r2.Inflate(-1, -1);
			DisplayHelp.FillRectangle(graphics, r2, pa.Colors.ItemCheckedBackground, pa.Colors.ItemCheckedBackground2, pa.Colors.ItemCheckedBackgroundGradientAngle);
			DisplayHelp.DrawRoundedRectangle(graphics, pa.Colors.ItemCheckedBorder, r, cornerSize);
		}
	}

	public virtual Office2007ButtonItemColorTable GetColorTable(ButtonItem button, eButtonContainer buttonCont)
	{
		Office2007ColorTable colorTable = ColorTable;
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = null;
		if (buttonCont == eButtonContainer.StatusBar)
		{
			object value = null;
			if (colorTable.ContextualTables.TryGetValue(Office2007ColorTable.GetContextualKey(ButtonColorTableType, "StatusBar"), out value))
			{
				Office2007ButtonItemColorTable office2007ButtonItemColorTable2 = (Office2007ButtonItemColorTable)value;
				if (button.EffectiveStyle == eDotNetBarStyle.Metro)
				{
					Office2007ButtonItemStateColorTable office2007ButtonItemStateColorTable = new Office2007ButtonItemStateColorTable();
					office2007ButtonItemStateColorTable.Background = new LinearGradientColorTable(Color.FromArgb(187, 212, 253), Color.FromArgb(187, 212, 253));
					office2007ButtonItemStateColorTable.OuterBorder = new LinearGradientColorTable(Color.FromArgb(187, 212, 253), Color.FromArgb(187, 212, 253));
					office2007ButtonItemColorTable2.MouseOver = office2007ButtonItemStateColorTable;
					Office2007ButtonItemStateColorTable office2007ButtonItemStateColorTable2 = new Office2007ButtonItemStateColorTable();
					office2007ButtonItemStateColorTable2.Background = new LinearGradientColorTable(Color.FromArgb(158, 186, 230), Color.FromArgb(158, 186, 230));
					office2007ButtonItemStateColorTable2.OuterBorder = new LinearGradientColorTable(Color.FromArgb(158, 186, 230), Color.FromArgb(158, 186, 230));
					office2007ButtonItemColorTable2.Pressed = (office2007ButtonItemColorTable2.Checked = (office2007ButtonItemColorTable2.Expanded = office2007ButtonItemStateColorTable2));
					return office2007ButtonItemColorTable2;
				}
				return office2007ButtonItemColorTable2;
			}
		}
		if (buttonCont == eButtonContainer.RibbonBar && colorTable.RibbonButtonItemColors.Count > 0)
		{
			if (button.CustomColorName != "")
			{
				office2007ButtonItemColorTable = colorTable.RibbonButtonItemColors[button.CustomColorName];
			}
			if (office2007ButtonItemColorTable == null)
			{
				office2007ButtonItemColorTable = colorTable.RibbonButtonItemColors[button.GetColorTableName()];
			}
			if (office2007ButtonItemColorTable != null)
			{
				return office2007ButtonItemColorTable;
			}
		}
		else if ((buttonCont == eButtonContainer.MenuBar || buttonCont == eButtonContainer.StatusBar) && colorTable.MenuButtonItemColors.Count > 0)
		{
			if (button.CustomColorName != "")
			{
				office2007ButtonItemColorTable = colorTable.MenuButtonItemColors[button.CustomColorName];
			}
			if (office2007ButtonItemColorTable == null)
			{
				office2007ButtonItemColorTable = colorTable.MenuButtonItemColors[button.GetColorTableName()];
			}
			if (office2007ButtonItemColorTable != null)
			{
				return office2007ButtonItemColorTable;
			}
		}
		else if (buttonCont == eButtonContainer.Backstage && colorTable.BackstageButtonItemColors.Count > 0)
		{
			if (button.CustomColorName != "")
			{
				office2007ButtonItemColorTable = colorTable.BackstageButtonItemColors[button.CustomColorName];
			}
			if (office2007ButtonItemColorTable == null)
			{
				office2007ButtonItemColorTable = colorTable.BackstageButtonItemColors[button.GetColorTableName()];
			}
			if (office2007ButtonItemColorTable != null)
			{
				return office2007ButtonItemColorTable;
			}
		}
		else if (buttonCont == eButtonContainer.NavigationPane && colorTable.NavigationPaneButtonItemColors.Count > 0)
		{
			if (button.CustomColorName != "")
			{
				office2007ButtonItemColorTable = colorTable.NavigationPaneButtonItemColors[button.CustomColorName];
			}
			if (office2007ButtonItemColorTable == null)
			{
				office2007ButtonItemColorTable = colorTable.NavigationPaneButtonItemColors[button.GetColorTableName()];
			}
			if (office2007ButtonItemColorTable != null)
			{
				return office2007ButtonItemColorTable;
			}
		}
		if (button.CustomColorName != "")
		{
			office2007ButtonItemColorTable = colorTable.ButtonItemColors[button.CustomColorName];
		}
		try
		{
			if (office2007ButtonItemColorTable == null)
			{
				office2007ButtonItemColorTable = colorTable.ButtonItemColors[button.GetColorTableName()];
			}
			if (office2007ButtonItemColorTable == null)
			{
				return colorTable.ButtonItemColors[0];
			}
		}
		catch
		{
			office2007ButtonItemColorTable = Office2007ColorTable.GetFallBackButtonItemColorTable();
		}
		return office2007ButtonItemColorTable;
	}

	private eButtonContainer GetButtonContainerQualifier(ItemPaintArgs pa, ButtonItem button)
	{
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Invalid comparison between Unknown and I4
		eButtonContainer result = eButtonContainer.None;
		if (pa.ContainerControl is RibbonBar)
		{
			result = eButtonContainer.RibbonBar;
		}
		else if (pa.ContainerControl is RibbonStrip)
		{
			result = eButtonContainer.RibbonStrip;
		}
		else if (pa.ContainerControl is Bar)
		{
			Bar bar = pa.ContainerControl as Bar;
			if (bar.MenuBar)
			{
				result = eButtonContainer.MenuBar;
			}
			else if (bar.LayoutType == eLayoutType.Toolbar)
			{
				result = ((bar.BarType != eBarType.StatusBar && bar.GrabHandleStyle != eGrabHandleStyle.ResizeHandle && (int)((Control)bar).Dock != 2 && bar.DockSide != eDockSide.Bottom) ? eButtonContainer.Toolbar : eButtonContainer.StatusBar);
			}
		}
		else if (pa.IsOnMenu || pa.IsOnPopupBar)
		{
			result = eButtonContainer.Popup;
		}
		else if (pa.ContainerControl is SuperTabStrip && ((SuperTabStrip)(object)pa.ContainerControl).ApplicationButton != null)
		{
			result = eButtonContainer.Backstage;
		}
		else if (pa.ContainerControl is NavigationBar)
		{
			result = eButtonContainer.NavigationPane;
		}
		return result;
	}

	protected virtual Color GetTextColor(ButtonItem button, ItemPaintArgs pa, Office2007ButtonItemColorTable buttonColorTable)
	{
		return GetTextColor(button, pa, buttonColorTable, isExpandPart: false);
	}

	protected virtual Color GetTextColor(ButtonItem button, ItemPaintArgs pa, Office2007ButtonItemColorTable buttonColorTable, bool isExpandPart)
	{
		Color result = Color.Empty;
		bool flag = button.IsMouseOver;
		if (flag)
		{
			if (isExpandPart)
			{
				if (!button.IsMouseOverExpand)
				{
					flag = false;
				}
			}
			else if (!button.IsMouseOverExpand && button.SplitButton)
			{
				flag = false;
			}
		}
		if (flag)
		{
			if (!button.HotForeColor.IsEmpty)
			{
				result = button.HotForeColor;
			}
		}
		else if (!button.ForeColor.IsEmpty)
		{
			result = button.ForeColor;
		}
		if (result.IsEmpty && buttonColorTable != null)
		{
			if (button.GetEnabled(pa.ContainerControl))
			{
				result = (button.IsMouseDown ? buttonColorTable.Pressed.Text : ((flag && (button.EffectiveStyle != eDotNetBarStyle.Metro || !button.Expanded)) ? buttonColorTable.MouseOver.Text : (button.Expanded ? buttonColorTable.Expanded.Text : ((!button.Checked || IsOnMenu(button, pa)) ? buttonColorTable.Default.Text : buttonColorTable.Checked.Text))));
			}
			else if (buttonColorTable.Disabled != null)
			{
				result = buttonColorTable.Disabled.Text;
			}
		}
		if (result.IsEmpty)
		{
			return base.GetTextColor(button, pa);
		}
		return result;
	}

	protected override Color GetTextColor(ButtonItem button, ItemPaintArgs pa)
	{
		bool flag = ButtonItemPainter.IsItemEnabled(button, pa);
		if (flag && !button.ForeColor.IsEmpty)
		{
			return button.ForeColor;
		}
		if (flag && button.ForeColorColorSchemePart != eColorSchemePart.None)
		{
			return (Color)pa.Colors.GetType().GetProperty(button.ForeColorColorSchemePart.ToString()).GetValue(pa.Colors, null);
		}
		eButtonContainer buttonContainerQualifier = GetButtonContainerQualifier(pa, button);
		if (!flag || IsOffice2003Rendering(button, pa))
		{
			if (flag && buttonContainerQualifier == eButtonContainer.MenuBar && !button.IsMouseDown && !button.IsMouseOver && !button.Expanded && !button.Checked)
			{
				Office2007ButtonItemColorTable colorTable = GetColorTable(button, buttonContainerQualifier);
				return colorTable.Default.Text;
			}
			return base.GetTextColor(button, pa);
		}
		Color empty = Color.Empty;
		if (buttonContainerQualifier == eButtonContainer.RibbonStrip && !button.IsMouseOver && !button.IsMouseDown && !button.Expanded)
		{
			Office2007RibbonTabItemColorTable colorTable2 = Office2007RibbonTabItemPainter.GetColorTable(m_ColorTable, button);
			if (pa.GlassEnabled && !colorTable2.Default.GlassText.IsEmpty)
			{
				return colorTable2.Default.GlassText;
			}
			return colorTable2.Default.Text;
		}
		Office2007ButtonItemColorTable colorTable3 = GetColorTable(button, buttonContainerQualifier);
		return GetTextColor(button, pa, colorTable3);
	}
}
