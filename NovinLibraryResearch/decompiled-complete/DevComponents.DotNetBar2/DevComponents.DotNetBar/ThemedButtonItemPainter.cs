using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace DevComponents.DotNetBar;

public class ThemedButtonItemPainter
{
	public static void PaintButton(ButtonItem button, ItemPaintArgs pa)
	{
		//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0304: Expected O, but got Unknown
		//IL_0304: Unknown result type (might be due to invalid IL or missing references)
		//IL_030b: Expected O, but got Unknown
		//IL_0344: Unknown result type (might be due to invalid IL or missing references)
		//IL_034b: Expected O, but got Unknown
		Graphics graphics = pa.Graphics;
		_0A7B obj = pa._0A7B;
		_0F6F _070B = _0F6F._0F70;
		_0F76 obj2 = _0F76.ཊ;
		Color color = _1932._1933(button, pa);
		Rectangle rectangle = Rectangle.Empty;
		Rectangle displayRectangle = button.DisplayRectangle;
		Font val = null;
		ᇎ ᇎ2 = button._0EC5();
		val = button._໘(pa, _໙: false);
		eTextFormat eTextFormat2 = ಲ(button, pa, ᇎ2);
		bool flag = (button.SubItems.Count > 0 || button.PopupType == ePopupType.Container) && button.ShowSubItems && !button.SubItemsRect.IsEmpty;
		if (flag)
		{
			_070B = _0F6F._0F72;
		}
		if (ᇎ2 != null)
		{
			rectangle = ((button.ImagePosition != eImagePosition.Top && button.ImagePosition != eImagePosition.Bottom) ? new Rectangle(button._0E60.X, button._0E60.Y, button._0E60.Width, button._0E60.Height) : new Rectangle(button._0E60.X, button._0E60.Y, displayRectangle.Width, button._0E60.Height));
			rectangle.Offset(displayRectangle.Left, displayRectangle.Top);
			rectangle.Offset((rectangle.Width - button.ImageSize.Width) / 2, (rectangle.Height - button.ImageSize.Height) / 2);
			rectangle.Width = button.ImageSize.Width;
			rectangle.Height = button.ImageSize.Height;
		}
		if (!_192F._1930(button, pa))
		{
			obj2 = _0F76.ड़;
		}
		else if (button.IsMouseDown)
		{
			obj2 = _0F76._0F78;
		}
		else if (button.IsMouseOver && button.Checked)
		{
			obj2 = _0F76._0F7A;
		}
		else if (button.IsMouseOver || button.Expanded)
		{
			obj2 = _0F76._0F77;
		}
		else if (button.Checked)
		{
			obj2 = _0F76._0F79;
		}
		Rectangle ल = button.DisplayRectangle;
		if (button.HotTrackingStyle == eHotTrackingStyle.Image && ᇎ2 != null)
		{
			ल = rectangle;
			ल.Inflate(3, 3);
		}
		else if (flag)
		{
			ल.Width -= button.SubItemsRect.Width;
		}
		if (button.HotTrackingStyle != eHotTrackingStyle.None)
		{
			obj._0EE5(graphics, _070B, obj2, ल);
		}
		if (ᇎ2 != null && button.ButtonStyle != eButtonStyle.TextOnlyAlways)
		{
			if (obj2 == _0F76.ཊ && button.HotTrackingStyle == eHotTrackingStyle.Color)
			{
				ColorMatrix colorMatrix = new ColorMatrix(new float[5][]
				{
					new float[5] { 0.2125f, 0.2125f, 0.2125f, 0f, 0f },
					new float[5] { 0.5f, 0.5f, 0.5f, 0f, 0f },
					new float[5] { 0.0361f, 0.0361f, 0.0361f, 0f, 0f },
					new float[5] { 0f, 0f, 0f, 1f, 0f },
					new float[5] { 0.2f, 0.2f, 0.2f, 0f, 1f }
				});
				ImageAttributes val2 = new ImageAttributes();
				val2.SetColorMatrix(colorMatrix);
				ᇎ2.ᇗ(graphics, rectangle, 0, 0, ᇎ2.ܐ, ᇎ2._0711, (GraphicsUnit)2, val2);
			}
			else if (obj2 == _0F76.ཊ && !ᇎ2.ᇏ)
			{
				ImageAttributes val3 = new ImageAttributes();
				val3.SetGamma(0.7f, (ColorAdjustType)1);
				ᇎ2.ᇗ(graphics, rectangle, 0, 0, ᇎ2.ܐ, ᇎ2._0711, (GraphicsUnit)2, val3);
			}
			else
			{
				ᇎ2.ᇗ(graphics, rectangle);
			}
		}
		if (button.ButtonStyle == eButtonStyle.ImageAndText || button.ButtonStyle == eButtonStyle.TextOnlyAlways || ᇎ2 == null)
		{
			Rectangle rectangle2 = button.TextDrawRect;
			if (button.ImagePosition == eImagePosition.Top || button.ImagePosition == eImagePosition.Bottom)
			{
				if (button.Orientation == eOrientation.Vertical)
				{
					rectangle2 = new Rectangle(button.TextDrawRect.X, button.TextDrawRect.Y, button.TextDrawRect.Width, button.TextDrawRect.Height);
				}
				else
				{
					rectangle2 = new Rectangle(button.TextDrawRect.X, button.TextDrawRect.Y, displayRectangle.Width, button.TextDrawRect.Height);
					if ((button.SubItems.Count > 0 || button.PopupType == ePopupType.Container) && button.ShowSubItems)
					{
						rectangle2.Width -= 10;
					}
				}
				eTextFormat2 |= eTextFormat.HorizontalCenter;
			}
			rectangle2.Offset(displayRectangle.Left, displayRectangle.Top);
			if (button.Orientation == eOrientation.Vertical)
			{
				graphics.RotateTransform(90f);
				TextDrawing.DrawStringLegacy(graphics, _192F.ಱ(button.Text), val, color, new Rectangle(rectangle2.Top, -rectangle2.Right, rectangle2.Height, rectangle2.Width), eTextFormat2);
				graphics.ResetTransform();
			}
			else
			{
				if (rectangle2.Right > button.DisplayRectangle.Right)
				{
					rectangle2.Width = button.DisplayRectangle.Right - rectangle2.Left;
				}
				TextDrawing.DrawString(graphics, _192F.ಱ(button.Text), val, color, rectangle2, eTextFormat2);
				if (!button.DesignMode && button.Focused && !pa.IsOnMenu && !pa.IsOnMenuBar)
				{
					Rectangle rectangle3 = rectangle2;
					ControlPaint.DrawFocusRectangle(graphics, rectangle3);
				}
			}
		}
		if (flag)
		{
			_070B = _0F6F._0F73;
			obj2 = (_192F._1930(button, pa) ? _0F76.ཊ : _0F76.ड़);
			if (button.HotTrackingStyle != eHotTrackingStyle.None && button.HotTrackingStyle != eHotTrackingStyle.Image && _192F._1930(button, pa))
			{
				if (button.Expanded || button.IsMouseDown)
				{
					obj2 = _0F76._0F78;
				}
				else if (button.IsMouseOver && button.Checked)
				{
					obj2 = _0F76._0F7A;
				}
				else if (button.Checked)
				{
					obj2 = _0F76._0F79;
				}
				else if (button.IsMouseOver)
				{
					obj2 = _0F76._0F77;
				}
			}
			if (!button.AutoExpandOnClick)
			{
				if (button.Orientation == eOrientation.Horizontal)
				{
					Rectangle subItemsRect = button.SubItemsRect;
					subItemsRect.Offset(displayRectangle.X, displayRectangle.Y);
					obj._0EE5(graphics, _070B, obj2, subItemsRect);
				}
				else
				{
					Rectangle subItemsRect2 = button.SubItemsRect;
					subItemsRect2.Offset(displayRectangle.X, displayRectangle.Y);
					obj._0EE5(graphics, _070B, obj2, subItemsRect2);
				}
			}
		}
		if (button.Focused && button.DesignMode)
		{
			Rectangle ल2 = displayRectangle;
			ल2.Inflate(-1, -1);
			_1A6B._1A6C(graphics, ल2, pa.Colors.ItemDesignTimeBorder);
		}
		ᇎ2?.Dispose();
	}

	private static eTextFormat ಲ(ButtonItem ᤉ, ItemPaintArgs _0BD2, ᇎ ແ)
	{
		eTextFormat eTextFormat2 = _0BD2.ButtonStringFormat;
		if (!_1936(ᤉ, _0BD2))
		{
			if (ᤉ.ContainerControl is RibbonStrip && (ແ == null || ᤉ.ImagePosition == eImagePosition.Top || ᤉ.ImagePosition == eImagePosition.Bottom))
			{
				eTextFormat2 |= eTextFormat.HorizontalCenter;
			}
			else if (_0BD2.IsOnMenuBar || (_0BD2.ContainerControl is Bar && ແ == null) || ᤉ.ImagePosition == eImagePosition.Top || ᤉ.ImagePosition == eImagePosition.Bottom)
			{
				eTextFormat2 |= eTextFormat.HorizontalCenter;
			}
		}
		return eTextFormat2;
	}

	private static bool _1936(ButtonItem ᤉ, ItemPaintArgs _0BD2)
	{
		bool flag = _0BD2.IsOnMenu;
		if (flag && ᤉ.Parent is ItemContainer)
		{
			flag = false;
		}
		return flag;
	}
}
