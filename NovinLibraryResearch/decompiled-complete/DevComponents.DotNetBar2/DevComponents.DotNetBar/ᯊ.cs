using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DevComponents.DotNetBar.Metro;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar;

internal class ᯊ : _192E, ᙓ
{
	private int ன = 2;

	private Office2007ColorTable ຊ;

	private static RoundRectangleShapeDescriptor _ᯋ = new RoundRectangleShapeDescriptor(2);

	private static RoundRectangleShapeDescriptor _ᯌ = new RoundRectangleShapeDescriptor(2, 0, 2, 0);

	private static RoundRectangleShapeDescriptor _ᯍ = new RoundRectangleShapeDescriptor(0, 2, 0, 2);

	private static RoundRectangleShapeDescriptor _ᯎ = new RoundRectangleShapeDescriptor(2, 2, 0, 0);

	private static RoundRectangleShapeDescriptor _ᯏ = new RoundRectangleShapeDescriptor(0, 0, 2, 2);

	private static RoundRectangleShapeDescriptor _ᯐ = new RoundRectangleShapeDescriptor(0);

	protected static readonly Type ᯑ = typeof(Office2007ButtonItemColorTable);

	public Office2007ColorTable ᘽ
	{
		get
		{
			return ຊ;
		}
		set
		{
			ຊ = value;
		}
	}

	private bool ᯒ(ButtonItem ᤉ, ItemPaintArgs _0BD2)
	{
		if (ᤉ.ColorTable == eButtonColor.Flat || (_0BD2.Owner is DotNetBarManager && string.IsNullOrEmpty(ᤉ.CustomColorName)))
		{
			return true;
		}
		return false;
	}

	public override void PaintButtonMouseOver(ButtonItem button, ItemPaintArgs pa, ᇎ image, Rectangle r)
	{
		if (ᯒ(button, pa))
		{
			base.PaintButtonMouseOver(button, pa, image, r);
		}
	}

	public override void PaintButtonCheck(ButtonItem button, ItemPaintArgs pa, ᇎ image, Rectangle r)
	{
		if (ᯒ(button, pa))
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
		IShapeDescriptor result = ᒜ(button);
		if (pa.ContainerControl is ButtonX)
		{
			result = ((ButtonX)(object)pa.ContainerControl).ᒜ();
		}
		else if (pa.ContainerControl is NavigationBar)
		{
			result = ((NavigationBar)(object)pa.ContainerControl).ᯙ;
		}
		return result;
	}

	protected virtual void PaintState(ButtonItem button, ItemPaintArgs pa, ᇎ image, Rectangle r, bool isMouseDown)
	{
		if (r.IsEmpty)
		{
			return;
		}
		bool isOnMenu = pa.IsOnMenu;
		_ = ᘽ;
		Graphics graphics = pa.Graphics;
		IShapeDescriptor shapeDescriptor = GetButtonShape(button, pa);
		_21F0 buttonCont = ᯘ(pa, button);
		Office2007ButtonItemColorTable colorTable = GetColorTable(button, buttonCont);
		Region clip = graphics.Clip;
		try
		{
			graphics.SetClip(r, (CombineMode)1);
			Office2007ButtonItemStateColorTable stateColors = colorTable.Default;
			if (IsOnMenu(button, pa))
			{
				Rectangle sideRect = new Rectangle(button.DisplayRectangle.Left, button.DisplayRectangle.Top, button._0E60.Right, button.DisplayRectangle.Height);
				if (pa.RightToLeft)
				{
					sideRect = new Rectangle(button.DisplayRectangle.Right - button._0E60.Width, button.DisplayRectangle.Top, button._0E60.Width, button.DisplayRectangle.Height);
				}
				PaintMenuItemSide(button, pa, sideRect);
			}
			if (!_192F._1930(button, pa))
			{
				if (!isOnMenu)
				{
					PaintStateBackground(button, graphics, colorTable.Disabled, r, shapeDescriptor, pa._0EE0, paintBorder: true);
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
					stateColors = colorTable.MouseOver;
					flag = true;
				}
				if (isMouseDown && button.HotTrackingStyle != eHotTrackingStyle.Image && button.HotTrackingStyle != eHotTrackingStyle.None)
				{
					if (colorTable.Pressed != null)
					{
						stateColors = colorTable.Pressed;
					}
				}
				else if (button.Checked && !button.IsMouseOver && !IsOnMenu(button, pa))
				{
					stateColors = colorTable.Checked;
				}
				else if (button.Expanded && button.HotTrackingStyle != eHotTrackingStyle.Image && (flag || !pa.IsOnMenu))
				{
					stateColors = colorTable.Expanded;
					flag2 = true;
				}
			}
			else if (button.Checked && !IsOnMenu(button, pa))
			{
				stateColors = colorTable.Checked;
			}
			else if (button.Expanded && button.HotTrackingStyle != eHotTrackingStyle.Image)
			{
				stateColors = colorTable.Expanded;
				flag2 = true;
			}
			Rectangle totalSubItemsRect = GetTotalSubItemsRect(button);
			bool flag3 = (flag || flag2) && !button.AutoExpandOnClick && !totalSubItemsRect.IsEmpty && !IsOnMenu(button, pa);
			eDotNetBarStyle effectiveStyle = button.EffectiveStyle;
			totalSubItemsRect.Offset(button.DisplayRectangle.Location);
			if (flag3 && !button.Checked && (effectiveStyle == eDotNetBarStyle.Office2010 || effectiveStyle == eDotNetBarStyle.Windows7 || StyleManager.IsMetro(effectiveStyle)))
			{
				Rectangle[] array = DisplayHelp.ExcludeRectangle(r, totalSubItemsRect);
				if (array.Length > 0 && (button.IsMouseOverExpand || button.Expanded))
				{
					array = DisplayHelp.ExcludeRectangle(r, array[0]);
				}
				if (array.Length > 0)
				{
					PaintStateBackground(button, graphics, colorTable.MouseOverSplitInactive, r, shapeDescriptor, pa._0EE0, !pa.IsOnNavigationBar);
					if (array[0].X == r.X && array[0].Right < r.Right)
					{
						shapeDescriptor = (StyleManager.IsMetro(effectiveStyle) ? _ᯐ : _ᯌ);
					}
					else if (array[0].X > r.X && array[0].Right == r.Right)
					{
						shapeDescriptor = (StyleManager.IsMetro(effectiveStyle) ? _ᯐ : _ᯍ);
					}
					else if (array[0].Y == r.Y && array[0].Bottom < r.Bottom)
					{
						shapeDescriptor = (StyleManager.IsMetro(effectiveStyle) ? _ᯐ : _ᯎ);
					}
					else if (array[0].Y > r.Y && array[0].Bottom == r.Bottom)
					{
						shapeDescriptor = (StyleManager.IsMetro(effectiveStyle) ? _ᯐ : _ᯏ);
					}
					PaintStateBackground(button, graphics, stateColors, array[0], shapeDescriptor, pa._0EE0, !pa.IsOnNavigationBar);
				}
				else
				{
					PaintStateBackground(button, graphics, stateColors, r, shapeDescriptor, pa._0EE0, !pa.IsOnNavigationBar);
				}
			}
			else
			{
				PaintStateBackground(button, graphics, stateColors, r, shapeDescriptor, pa._0EE0, !pa.IsOnNavigationBar);
			}
			if (flag3 && effectiveStyle != eDotNetBarStyle.Office2010 && effectiveStyle != eDotNetBarStyle.Windows7 && !StyleManager.IsMetro(effectiveStyle))
			{
				Brush val = ᯗ();
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
							graphics.FillPath(val, shape);
							shape.Dispose();
						}
					}
					else if (!flag || button.IsMouseOverExpand)
					{
						graphics.SetClip(totalSubItemsRect, (CombineMode)4);
						GraphicsPath shape2 = shapeDescriptor.GetShape(displayRectangle);
						if (shape2 != null)
						{
							graphics.FillPath(val, shape2);
							shape2.Dispose();
						}
					}
				}
				finally
				{
					((IDisposable)val)?.Dispose();
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
		ᯓ(g, stateColors, r, shape, isDefault, paintBorder);
	}

	public static void ᯓ(Graphics _٥, Office2007ButtonItemStateColorTable ᯔ, Rectangle ल, IShapeDescriptor _166E)
	{
		ᯓ(_٥, ᯔ, ल, _166E, ᯕ: false);
	}

	public static void ᯓ(Graphics _٥, Office2007ButtonItemStateColorTable ᯔ, Rectangle ल, IShapeDescriptor _166E, bool ᯕ)
	{
		ᯓ(_٥, ᯔ, ल, _166E, ᯕ, ᯖ: true);
	}

	public static void ᯓ(Graphics _٥, Office2007ButtonItemStateColorTable ᯔ, Rectangle ल, IShapeDescriptor _166E, bool ᯕ, bool ᯖ)
	{
		//IL_0574: Unknown result type (might be due to invalid IL or missing references)
		//IL_0579: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Expected O, but got Unknown
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Expected O, but got Unknown
		//IL_0319: Unknown result type (might be due to invalid IL or missing references)
		//IL_0320: Expected O, but got Unknown
		//IL_032b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0332: Expected O, but got Unknown
		//IL_0393: Unknown result type (might be due to invalid IL or missing references)
		//IL_039a: Expected O, but got Unknown
		//IL_0485: Unknown result type (might be due to invalid IL or missing references)
		//IL_048c: Expected O, but got Unknown
		//IL_0497: Unknown result type (might be due to invalid IL or missing references)
		//IL_049e: Expected O, but got Unknown
		//IL_04ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0506: Expected O, but got Unknown
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Expected O, but got Unknown
		//IL_0709: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Expected O, but got Unknown
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Expected O, but got Unknown
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Expected O, but got Unknown
		//IL_061b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0622: Expected O, but got Unknown
		float num = 0.35f;
		float num2 = 0.65f;
		if (!_166E.CanDrawShape(ल) || ᯔ == null)
		{
			return;
		}
		Rectangle rectangle = ल;
		Rectangle rectangle2 = new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, (int)((float)rectangle.Height * num));
		if (!ᯔ.OuterBorder.IsEmpty && ᯖ)
		{
			rectangle.Width--;
		}
		GraphicsPath shape = _166E.GetShape(rectangle);
		if (ᯔ.Background != null)
		{
			if (shape != null)
			{
				if (ᯔ.Background.End.IsEmpty)
				{
					SolidBrush val = new SolidBrush(ᯔ.Background.Start);
					try
					{
						_٥.FillPath((Brush)(object)val, shape);
					}
					finally
					{
						((IDisposable)val)?.Dispose();
					}
				}
				else
				{
					LinearGradientBrush val2 = new LinearGradientBrush(rectangle, ᯔ.Background.Start, ᯔ.Background.End, (float)ᯔ.Background.GradientAngle);
					try
					{
						_٥.FillPath((Brush)(object)val2, shape);
					}
					finally
					{
						((IDisposable)val2)?.Dispose();
					}
				}
				shape.Dispose();
			}
		}
		else
		{
			if (shape != null && ᯔ.TopBackground != null && ᯔ.BottomBackground != null)
			{
				if (ᯔ.TopBackground.End.IsEmpty && ᯔ.BottomBackground.End.IsEmpty)
				{
					if (ᯔ.TopBackground.Start == ᯔ.BottomBackground.Start)
					{
						SolidBrush val3 = new SolidBrush(ᯔ.TopBackground.Start);
						try
						{
							_٥.FillPath((Brush)(object)val3, shape);
						}
						finally
						{
							((IDisposable)val3)?.Dispose();
						}
					}
					else
					{
						LinearGradientBrush val4 = new LinearGradientBrush(rectangle, ᯔ.TopBackground.Start, ᯔ.BottomBackground.Start, (float)ᯔ.TopBackground.GradientAngle);
						try
						{
							_٥.FillPath((Brush)(object)val4, shape);
						}
						finally
						{
							((IDisposable)val4)?.Dispose();
						}
					}
				}
				else
				{
					LinearGradientBrush val5 = new LinearGradientBrush(rectangle, ᯔ.TopBackground.Start, ᯔ.BottomBackground.End, (float)ᯔ.TopBackground.GradientAngle);
					try
					{
						ColorBlend val6 = new ColorBlend(4);
						val6.Colors = new Color[4]
						{
							ᯔ.TopBackground.Start,
							ᯔ.TopBackground.End,
							ᯔ.BottomBackground.Start,
							ᯔ.BottomBackground.End
						};
						val6.Positions = new float[4] { 0f, num, num, 1f };
						val5.InterpolationColors = val6;
						_٥.FillPath((Brush)(object)val5, shape);
					}
					finally
					{
						((IDisposable)val5)?.Dispose();
					}
				}
				shape.Dispose();
			}
			if (ᯔ.TopBackgroundHighlight != null && !ᯔ.TopBackgroundHighlight.IsEmpty)
			{
				Rectangle rectangle3 = new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
				GraphicsPath val7 = new GraphicsPath();
				val7.AddEllipse(rectangle3);
				PathGradientBrush val8 = new PathGradientBrush(val7);
				val8.CenterColor = ᯔ.TopBackgroundHighlight.Start;
				val8.SurroundColors = new Color[1] { ᯔ.TopBackgroundHighlight.End };
				val8.CenterPoint = new PointF(rectangle3.X + rectangle3.Width / 2, rectangle.Bottom);
				Blend val9 = new Blend();
				val9.Factors = new float[3] { 0f, 0.5f, 1f };
				val9.Positions = new float[3] { 0f, 0.4f, 1f };
				val8.Blend = val9;
				_٥.FillRectangle((Brush)(object)val8, rectangle2);
				((Brush)val8).Dispose();
				val7.Dispose();
			}
			int num3 = (int)((float)rectangle.Height * num2);
			rectangle2 = new Rectangle(rectangle.X, rectangle.Y + rectangle2.Height, rectangle.Width, rectangle.Height - rectangle2.Height);
			if (ᯔ.BottomBackgroundHighlight != null && !ᯔ.BottomBackgroundHighlight.IsEmpty)
			{
				Rectangle rectangle4 = new Rectangle(rectangle.X, rectangle.Y + num3 - 2, rectangle.Width, rectangle.Height + 4);
				GraphicsPath val10 = new GraphicsPath();
				val10.AddEllipse(rectangle4);
				PathGradientBrush val11 = new PathGradientBrush(val10);
				val11.CenterColor = ᯔ.BottomBackgroundHighlight.Start;
				val11.SurroundColors = new Color[1] { ᯔ.BottomBackgroundHighlight.End };
				val11.CenterPoint = new PointF(rectangle4.X + rectangle4.Width / 2, rectangle.Bottom);
				Blend val12 = new Blend();
				val12.Factors = new float[3] { 0f, 0.5f, 0.6f };
				val12.Positions = new float[3] { 0f, 0.4f, 1f };
				val11.Blend = val12;
				_٥.FillRectangle((Brush)(object)val11, rectangle2);
				((Brush)val11).Dispose();
				val10.Dispose();
			}
		}
		if (!ᯖ)
		{
			return;
		}
		SmoothingMode smoothingMode = _٥.SmoothingMode;
		_٥.SmoothingMode = (SmoothingMode)4;
		if (!ᯔ.OuterBorder.IsEmpty && ᯔ.OuterBorderWidth > 0)
		{
			Rectangle rectangle5 = ल;
			rectangle5.Width--;
			rectangle5.Height--;
			if (_166E is EllipticalShapeDescriptor && ᯔ.OuterBorder.End.IsEmpty)
			{
				rectangle5.Width -= ᯔ.OuterBorderWidth / 2;
				rectangle5.Height -= ᯔ.OuterBorderWidth / 2;
				Pen val13 = new Pen(ᯔ.OuterBorder.Start, (float)ᯔ.OuterBorderWidth);
				try
				{
					_٥.DrawEllipse(val13, rectangle5);
				}
				finally
				{
					((IDisposable)val13)?.Dispose();
				}
			}
			else
			{
				GraphicsPath shape2 = _166E.GetShape(rectangle5);
				if (shape2 != null)
				{
					DisplayHelp.DrawGradientPath(_٥, shape2, ल, ᯔ.OuterBorder, ᯔ.OuterBorderWidth);
					shape2.Dispose();
				}
			}
			if (ᯕ)
			{
				Color color = Color.FromArgb(128, ᯔ.OuterBorder.Start);
				ल.Inflate(-1, -1);
				DisplayHelp.DrawRectangle(_٥, color, ल);
				ल.Inflate(-1, -1);
				DisplayHelp.DrawRectangle(_٥, color, ल);
			}
		}
		if (!ᯕ && !ᯔ.InnerBorder.IsEmpty)
		{
			Rectangle bounds = ल;
			bounds.Inflate(-1, -1);
			bounds.Width--;
			bounds.Height--;
			GraphicsPath innerShape = _166E.GetInnerShape(bounds, 1);
			try
			{
				DisplayHelp.DrawGradientPath(_٥, innerShape, bounds, ᯔ.InnerBorder, 1);
			}
			finally
			{
				((IDisposable)innerShape)?.Dispose();
			}
		}
		_٥.SmoothingMode = smoothingMode;
	}

	public override void PaintButtonBackground(ButtonItem button, ItemPaintArgs pa, ᇎ image)
	{
		if (ᯒ(button, pa))
		{
			base.PaintButtonBackground(button, pa, image);
			return;
		}
		if (button is ApplicationButton && ຊ.RibbonControl.StartButtonDefault != null)
		{
			Image val = ຊ.RibbonControl.StartButtonDefault;
			if (button.IsMouseDown || button.Expanded)
			{
				val = ຊ.RibbonControl.StartButtonPressed;
			}
			else if (button.IsMouseOver)
			{
				val = ຊ.RibbonControl.StartButtonMouseOver;
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

	private Brush ᯗ()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		return (Brush)new SolidBrush(Color.FromArgb(76, Color.White));
	}

	private static IShapeDescriptor ᒜ(ButtonItem ᤉ)
	{
		if (ᤉ.Shape != null)
		{
			return ᤉ.Shape;
		}
		return _ᯋ;
	}

	public override void PaintExpandButton(ButtonItem button, ItemPaintArgs pa)
	{
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c0: Expected O, but got Unknown
		//IL_097d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0982: Unknown result type (might be due to invalid IL or missing references)
		//IL_0996: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c3c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c43: Expected O, but got Unknown
		//IL_0c84: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c8b: Expected O, but got Unknown
		if (ᯒ(button, pa))
		{
			base.PaintExpandButton(button, pa);
			return;
		}
		Graphics graphics = pa.Graphics;
		bool flag = IsOnMenu(button, pa);
		Rectangle displayRectangle = button.DisplayRectangle;
		bool isMouseOver = button.IsMouseOver;
		bool flag2 = _192F._1930(button, pa);
		Color color = Color.Empty;
		_21F0 _21F02 = ᯘ(pa, button);
		Office2007ButtonItemColorTable colorTable = GetColorTable(button, _21F02);
		if (_21F02 == _21F0._1BF3)
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
			if ((button.SubItems.Count <= 0 && button.PopupType != ePopupType.Container) || !button.ShowSubItems)
			{
				return;
			}
			if (flag)
			{
				Point[] array = new Point[3];
				if (pa.RightToLeft)
				{
					array[0].X = displayRectangle.Left + 8;
					array[0].Y = displayRectangle.Top + (displayRectangle.Height - 8) / 2;
					array[1].X = array[0].X;
					array[1].Y = array[0].Y + 8;
					array[2].X = array[0].X - 4;
					array[2].Y = array[0].Y + 4;
				}
				else
				{
					array[0].X = displayRectangle.Left + displayRectangle.Width - 12;
					array[0].Y = displayRectangle.Top + (displayRectangle.Height - 8) / 2;
					array[1].X = array[0].X;
					array[1].Y = array[0].Y + 8;
					array[2].X = array[0].X + 4;
					array[2].Y = array[0].Y + 4;
				}
				SmoothingMode smoothingMode = graphics.SmoothingMode;
				graphics.SmoothingMode = (SmoothingMode)3;
				graphics.FillPolygon((Brush)(object)val, array);
				graphics.SmoothingMode = smoothingMode;
			}
			else if (!button.SubItemsRect.IsEmpty)
			{
				Point[] array2 = new Point[3];
				Rectangle subItemsRect = button.SubItemsRect;
				if (pa.IsOnRibbonBar && button.ImagePosition == eImagePosition.Top && !button.SplitButton && button.Text.Length > 0)
				{
					subItemsRect.Y -= 3;
				}
				if (button.PopupSide == ePopupSide.Default)
				{
					if (pa.IsOnMenu)
					{
						if (pa.RightToLeft)
						{
							array2[0].X = displayRectangle.Left + subItemsRect.Left + subItemsRect.Width / 2 + 3;
							array2[0].Y = displayRectangle.Top + subItemsRect.Top + subItemsRect.Height / 2 - 3;
							array2[1].X = array2[0].X;
							array2[1].Y = array2[0].Y + 6;
							array2[2].X = array2[0].X - 3;
							array2[2].Y = array2[0].Y + 3;
						}
						else
						{
							array2[0].X = displayRectangle.Left + subItemsRect.Left + subItemsRect.Width / 2;
							array2[0].Y = displayRectangle.Top + subItemsRect.Top + subItemsRect.Height / 2 - 3;
							array2[1].X = array2[0].X;
							array2[1].Y = array2[0].Y + 6;
							array2[2].X = array2[0].X + 3;
							array2[2].Y = array2[0].Y + 3;
						}
					}
					else if (button.Orientation == eOrientation.Horizontal)
					{
						array2[0].X = displayRectangle.Left + subItemsRect.Left + (subItemsRect.Width - Dpi.Width5) / 2;
						array2[0].Y = displayRectangle.Top + subItemsRect.Top + (subItemsRect.Height - Dpi.Height3) / 2 + 1;
						array2[1].X = array2[0].X + Dpi.Width5;
						array2[1].Y = array2[0].Y;
						array2[2].X = array2[0].X + Dpi.Width2;
						array2[2].Y = array2[0].Y + Dpi.Height3;
					}
					else
					{
						array2[0].X = displayRectangle.Left + subItemsRect.Left + (subItemsRect.Width - 3) / 2 + 1;
						array2[0].Y = displayRectangle.Top + subItemsRect.Top + (subItemsRect.Height - 5) / 2;
						array2[1].X = array2[0].X;
						array2[1].Y = array2[0].Y + 6;
						array2[2].X = array2[0].X - 3;
						array2[2].Y = array2[0].Y + 3;
					}
				}
				else
				{
					switch (button.PopupSide)
					{
					case ePopupSide.Right:
						array2[0].X = displayRectangle.Left + subItemsRect.Left + subItemsRect.Width / 2;
						array2[0].Y = displayRectangle.Top + subItemsRect.Top + subItemsRect.Height / 2 - 3;
						array2[1].X = array2[0].X;
						array2[1].Y = array2[0].Y + 6;
						array2[2].X = array2[0].X + 3;
						array2[2].Y = array2[0].Y + 3;
						break;
					case ePopupSide.Left:
						array2[0].X = displayRectangle.Left + subItemsRect.Left + subItemsRect.Width / 2 + 3;
						array2[0].Y = displayRectangle.Top + subItemsRect.Top + subItemsRect.Height / 2 - 3;
						array2[1].X = array2[0].X;
						array2[1].Y = array2[0].Y + 6;
						array2[2].X = array2[0].X - 3;
						array2[2].Y = array2[0].Y + 3;
						break;
					case ePopupSide.Top:
						array2[0].X = displayRectangle.Left + subItemsRect.Left + (subItemsRect.Width - 5) / 2;
						array2[0].Y = displayRectangle.Top + subItemsRect.Top + (subItemsRect.Height - 3) / 2 + 4;
						array2[1].X = array2[0].X + 6;
						array2[1].Y = array2[0].Y;
						array2[2].X = array2[0].X + 3;
						array2[2].Y = array2[0].Y - 4;
						break;
					case ePopupSide.Bottom:
						array2[0].X = displayRectangle.Left + subItemsRect.Left + (subItemsRect.Width - 5) / 2 + 1;
						array2[0].Y = displayRectangle.Top + subItemsRect.Top + (subItemsRect.Height - 3) / 2 + 1;
						array2[1].X = array2[0].X + 5;
						array2[1].Y = array2[0].Y;
						array2[2].X = array2[0].X + 2;
						array2[2].Y = array2[0].Y + 3;
						break;
					}
				}
				if (button.SplitButton && !button.TextDrawRect.IsEmpty && button.ImagePosition == eImagePosition.Top)
				{
					array2[0].Y -= 3;
					array2[1].Y -= 3;
					array2[2].Y -= 3;
				}
				if (flag2)
				{
					SmoothingMode smoothingMode2 = graphics.SmoothingMode;
					graphics.SmoothingMode = (SmoothingMode)3;
					graphics.FillPolygon((Brush)(object)val, array2);
					graphics.SmoothingMode = smoothingMode2;
				}
				else
				{
					SmoothingMode smoothingMode3 = graphics.SmoothingMode;
					graphics.SmoothingMode = (SmoothingMode)3;
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
				ref Point reference = ref array3[0];
				reference = new Point(totalSubItemsRect.X + 1, totalSubItemsRect.Y);
				ref Point reference2 = ref array3[1];
				reference2 = new Point(totalSubItemsRect.Right - 2, totalSubItemsRect.Y);
				ref Point reference3 = ref array3[2];
				reference3 = new Point(totalSubItemsRect.X + 1, totalSubItemsRect.Y + 1);
				ref Point reference4 = ref array3[3];
				reference4 = new Point(totalSubItemsRect.Right - 2, totalSubItemsRect.Y + 1);
				if (button.SplitButton)
				{
					array3[0].Y--;
					array3[1].Y--;
					array3[2].Y--;
					array3[3].Y--;
				}
			}
			else
			{
				totalSubItemsRect.Y++;
				totalSubItemsRect.Height -= 3;
				if (pa.RightToLeft)
				{
					totalSubItemsRect.X = totalSubItemsRect.Right - 1;
				}
				ref Point reference5 = ref array3[0];
				reference5 = new Point(totalSubItemsRect.X, totalSubItemsRect.Y);
				ref Point reference6 = ref array3[1];
				reference6 = new Point(totalSubItemsRect.X, totalSubItemsRect.Bottom);
				ref Point reference7 = ref array3[2];
				reference7 = new Point(totalSubItemsRect.X + 1, totalSubItemsRect.Y);
				ref Point reference8 = ref array3[3];
				reference8 = new Point(totalSubItemsRect.X + 1, totalSubItemsRect.Bottom);
			}
			eDotNetBarStyle effectiveStyle = button.EffectiveStyle;
			if ((effectiveStyle != eDotNetBarStyle.Office2010 && effectiveStyle != eDotNetBarStyle.Windows7 && !StyleManager.IsMetro(effectiveStyle)) || button.Checked)
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
		if (ᯒ(button, pa))
		{
			base.PaintMenuItemSide(button, pa, sideRect);
			return;
		}
		Graphics graphics = pa.Graphics;
		Office2007MenuColorTable menu = ຊ.Menu;
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

	protected override void PaintButtonCheckBackground(ButtonItem button, ItemPaintArgs pa, Rectangle r)
	{
		if (ᯒ(button, pa))
		{
			base.PaintButtonCheckBackground(button, pa, r);
			return;
		}
		Graphics graphics = pa.Graphics;
		bool flag = IsOnMenu(button, pa);
		int ଖ = ன;
		if (pa.ContainerControl is ButtonX)
		{
			ଖ = ((ButtonX)(object)pa.ContainerControl).ଖ;
		}
		if (!button.IsMouseOver || flag)
		{
			Rectangle r2 = r;
			r2.Inflate(-1, -1);
			DisplayHelp.FillRectangle(graphics, r2, pa.Colors.ItemCheckedBackground, pa.Colors.ItemCheckedBackground2, pa.Colors.ItemCheckedBackgroundGradientAngle);
			DisplayHelp.DrawRoundedRectangle(graphics, pa.Colors.ItemCheckedBorder, r, ଖ);
		}
	}

	public virtual Office2007ButtonItemColorTable GetColorTable(ButtonItem button, _21F0 buttonCont)
	{
		Office2007ColorTable office2007ColorTable = ᘽ;
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = null;
		if (buttonCont == _21F0._21F4)
		{
			object value = null;
			if (office2007ColorTable.ContextualTables.TryGetValue(Office2007ColorTable.GetContextualKey(ᯑ, "StatusBar"), out value))
			{
				return (Office2007ButtonItemColorTable)value;
			}
		}
		if (buttonCont == _21F0._21F1 && office2007ColorTable.RibbonButtonItemColors.Count > 0)
		{
			if (button.CustomColorName != "")
			{
				office2007ButtonItemColorTable = office2007ColorTable.RibbonButtonItemColors[button.CustomColorName];
			}
			if (office2007ButtonItemColorTable == null)
			{
				office2007ButtonItemColorTable = office2007ColorTable.RibbonButtonItemColors[button.ຽ()];
			}
			if (office2007ButtonItemColorTable != null)
			{
				return office2007ButtonItemColorTable;
			}
		}
		else if ((buttonCont == _21F0._21F3 || buttonCont == _21F0._21F4) && office2007ColorTable.MenuButtonItemColors.Count > 0)
		{
			if (button.CustomColorName != "")
			{
				office2007ButtonItemColorTable = office2007ColorTable.MenuButtonItemColors[button.CustomColorName];
			}
			if (office2007ButtonItemColorTable == null)
			{
				office2007ButtonItemColorTable = office2007ColorTable.MenuButtonItemColors[button.ຽ()];
			}
			if (office2007ButtonItemColorTable != null)
			{
				return office2007ButtonItemColorTable;
			}
		}
		else if (buttonCont == _21F0._21F6 && office2007ColorTable.BackstageButtonItemColors.Count > 0)
		{
			if (button.CustomColorName != "")
			{
				office2007ButtonItemColorTable = office2007ColorTable.BackstageButtonItemColors[button.CustomColorName];
			}
			if (office2007ButtonItemColorTable == null)
			{
				office2007ButtonItemColorTable = office2007ColorTable.BackstageButtonItemColors[button.ຽ()];
			}
			if (office2007ButtonItemColorTable != null)
			{
				return office2007ButtonItemColorTable;
			}
		}
		else if (buttonCont == _21F0._21F7 && office2007ColorTable.NavigationPaneButtonItemColors.Count > 0)
		{
			if (button.CustomColorName != "")
			{
				office2007ButtonItemColorTable = office2007ColorTable.NavigationPaneButtonItemColors[button.CustomColorName];
			}
			if (office2007ButtonItemColorTable == null)
			{
				office2007ButtonItemColorTable = office2007ColorTable.NavigationPaneButtonItemColors[button.ຽ()];
			}
			if (office2007ButtonItemColorTable != null)
			{
				return office2007ButtonItemColorTable;
			}
		}
		if (button.CustomColorName != "")
		{
			office2007ButtonItemColorTable = office2007ColorTable.ButtonItemColors[button.CustomColorName];
		}
		try
		{
			if (office2007ButtonItemColorTable == null)
			{
				office2007ButtonItemColorTable = office2007ColorTable.ButtonItemColors[button.ຽ()];
			}
			if (office2007ButtonItemColorTable == null)
			{
				return office2007ColorTable.ButtonItemColors[0];
			}
		}
		catch
		{
			office2007ButtonItemColorTable = Office2007ColorTable.ተ();
		}
		return office2007ButtonItemColorTable;
	}

	private _21F0 ᯘ(ItemPaintArgs _0BD2, ButtonItem ᤉ)
	{
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Invalid comparison between Unknown and I4
		_21F0 result = _21F0.ᘶ;
		if (_0BD2.ContainerControl is RibbonBar)
		{
			result = _21F0._21F1;
		}
		else if (_0BD2.ContainerControl is RibbonStrip)
		{
			result = _21F0._1BF3;
		}
		else if (_0BD2.ContainerControl is MetroTabStrip)
		{
			result = _21F0._20AE;
		}
		else if (_0BD2.ContainerControl is Bar)
		{
			Bar bar = _0BD2.ContainerControl as Bar;
			if (bar.MenuBar)
			{
				result = _21F0._21F3;
			}
			else if (bar.LayoutType == eLayoutType.Toolbar)
			{
				result = ((bar.BarType != eBarType.StatusBar && bar.GrabHandleStyle != eGrabHandleStyle.ResizeHandle && (int)((Control)bar).Dock != 2 && bar.DockSide != eDockSide.Bottom) ? _21F0._21F5 : _21F0._21F4);
			}
		}
		else if (_0BD2.IsOnMenu || _0BD2.IsOnPopupBar)
		{
			result = _21F0._21F2;
		}
		else if (_0BD2.ContainerControl is SuperTabStrip && ((SuperTabStrip)(object)_0BD2.ContainerControl).ApplicationButton != null)
		{
			result = _21F0._21F6;
		}
		else if (_0BD2.ContainerControl is NavigationBar)
		{
			result = _21F0._21F7;
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
			else if (button.IsMouseOverExpand && button.SplitButton && !button.AutoExpandOnClick)
			{
				flag = false;
			}
			else if (button.IsMouseOverExpand && !isExpandPart && !button.AutoExpandOnClick)
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
				result = (button.IsMouseDown ? buttonColorTable.Pressed.Text : ((flag && (!StyleManager.IsMetro(button.EffectiveStyle) || !button.Expanded)) ? buttonColorTable.MouseOver.Text : ((button.Expanded && button.AutoExpandOnClick && !button.SplitButton) ? buttonColorTable.Expanded.Text : ((!button.Checked || IsOnMenu(button, pa)) ? buttonColorTable.Default.Text : buttonColorTable.Checked.Text))));
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
		bool flag = _192F._1930(button, pa);
		if (flag && button.IsMouseOver && !button.HotForeColor.IsEmpty)
		{
			return button.HotForeColor;
		}
		if (flag && !button.ForeColor.IsEmpty)
		{
			return button.ForeColor;
		}
		if (flag && button._0E61 != eColorSchemePart.None)
		{
			return (Color)pa.Colors.GetType().GetProperty(button._0E61.ToString()).GetValue(pa.Colors, null);
		}
		_21F0 _21F02 = ᯘ(pa, button);
		if (!flag || ᯒ(button, pa))
		{
			if (flag && _21F02 == _21F0._21F3 && !button.IsMouseDown && !button.IsMouseOver && !button.Expanded && !button.Checked)
			{
				Office2007ButtonItemColorTable colorTable = GetColorTable(button, _21F02);
				return colorTable.Default.Text;
			}
			return base.GetTextColor(button, pa);
		}
		Color empty = Color.Empty;
		if (_21F02 == _21F0._1BF3 && !button.IsMouseOver && !button.IsMouseDown && !button.Expanded)
		{
			Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = _21EB.ጧ(ຊ);
			if (pa.GlassEnabled && !office2007RibbonTabItemColorTable.Default.GlassText.IsEmpty)
			{
				return office2007RibbonTabItemColorTable.Default.GlassText;
			}
			return office2007RibbonTabItemColorTable.Default.Text;
		}
		Office2007ButtonItemColorTable colorTable2 = GetColorTable(button, _21F02);
		return GetTextColor(button, pa, colorTable2);
	}
}
