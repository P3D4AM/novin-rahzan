using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Stimulsoft.Controls.Win.DotNetBar.Rendering;

internal class Office2007ColorItemPainter : ColorItemPainter, IOffice2007Painter
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

	public override void PaintColorItem(ColorItemRendererEventArgs e)
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Expected O, but got Unknown
		Graphics graphics = e.Graphics;
		ColorItem colorItem = e.ColorItem;
		Rectangle bounds = colorItem.Bounds;
		Color border = m_ColorTable.ColorItem.Border;
		Color mouseOverOuterBorder = m_ColorTable.ColorItem.MouseOverOuterBorder;
		Color mouseOverInnerBorder = m_ColorTable.ColorItem.MouseOverInnerBorder;
		SmoothingMode smoothingMode = graphics.SmoothingMode;
		graphics.SmoothingMode = (SmoothingMode)0;
		Region clip = graphics.Clip;
		graphics.SetClip(bounds);
		Color color = Color.Empty;
		ColorPickerDropDown colorPickerDropDown = null;
		for (BaseItem parent = colorItem.Parent; parent != null; parent = parent.Parent)
		{
			if (parent is ColorPickerDropDown)
			{
				colorPickerDropDown = parent as ColorPickerDropDown;
				break;
			}
		}
		if (colorPickerDropDown != null)
		{
			color = colorPickerDropDown.SelectedColor;
		}
		try
		{
			if (!colorItem.Color.IsEmpty)
			{
				Rectangle bounds2 = bounds;
				bounds2.Inflate(1, 1);
				DisplayHelp.FillRectangle(graphics, bounds2, colorItem.Color, Color.Empty);
			}
			if (colorItem.IsMouseOver || color == colorItem.Color)
			{
				Rectangle r = bounds;
				r.Inflate(-1, -1);
				DisplayHelp.DrawRectangle(graphics, mouseOverInnerBorder, r);
				DisplayHelp.DrawRectangle(graphics, mouseOverOuterBorder, bounds);
			}
			else
			{
				eColorItemBorder border2 = colorItem.Border;
				switch (border2)
				{
				case eColorItemBorder.All:
					DisplayHelp.DrawRectangle(graphics, border, bounds);
					break;
				default:
				{
					Pen val = new Pen(border);
					try
					{
						if ((border2 & eColorItemBorder.Left) == eColorItemBorder.Left)
						{
							graphics.DrawLine(val, bounds.X, bounds.Y, bounds.X, bounds.Bottom - 1);
						}
						if ((border2 & eColorItemBorder.Right) == eColorItemBorder.Right)
						{
							graphics.DrawLine(val, bounds.Right - 1, bounds.Y, bounds.Right - 1, bounds.Bottom - 1);
						}
						if ((border2 & eColorItemBorder.Top) == eColorItemBorder.Top)
						{
							graphics.DrawLine(val, bounds.X, bounds.Y, bounds.Right - 1, bounds.Y);
						}
						if ((border2 & eColorItemBorder.Bottom) == eColorItemBorder.Bottom)
						{
							graphics.DrawLine(val, bounds.X, bounds.Bottom - 1, bounds.Right - 1, bounds.Bottom - 1);
						}
					}
					finally
					{
						((IDisposable)val)?.Dispose();
					}
					break;
				}
				case eColorItemBorder.None:
					break;
				}
			}
		}
		finally
		{
			if (clip != null)
			{
				graphics.Clip = clip;
			}
			else
			{
				graphics.ResetClip();
			}
		}
		graphics.SmoothingMode = smoothingMode;
	}
}
