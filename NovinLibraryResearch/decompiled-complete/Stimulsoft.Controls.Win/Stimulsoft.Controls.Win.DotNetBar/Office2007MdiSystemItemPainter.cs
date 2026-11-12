using System;
using System.Drawing;
using System.Drawing.Imaging;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class Office2007MdiSystemItemPainter : MdiSystemItemPainter, IOffice2007Painter
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

	public override void Paint(MdiSystemItemRendererEventArgs e)
	{
		MDISystemItem mdiSystemItem = e.MdiSystemItem;
		Graphics graphics = e.Graphics;
		Rectangle rectangle = mdiSystemItem.DisplayRectangle;
		if (mdiSystemItem.IsSystemIcon)
		{
			rectangle.Offset((rectangle.Width - 16) / 2, (rectangle.Height - 16) / 2);
			if (mdiSystemItem.Icon != null)
			{
				graphics.DrawIconUnstretched(mdiSystemItem.Icon, rectangle);
			}
			return;
		}
		rectangle = new Rectangle(mdiSystemItem.DisplayRectangle.Location, mdiSystemItem.GetButtonSize());
		if (mdiSystemItem.Orientation == eOrientation.Horizontal)
		{
			rectangle.Offset(0, (mdiSystemItem.DisplayRectangle.Height - rectangle.Height) / 2);
		}
		else
		{
			rectangle.Offset((mdiSystemItem.WidthInternal - rectangle.Width) / 2, 0);
		}
		PaintButton(graphics, mdiSystemItem, SystemButton.Minimize, rectangle, GetColorTable(mdiSystemItem, SystemButton.Minimize), m_ColorTable.LegacyColors);
		if (mdiSystemItem.Orientation == eOrientation.Horizontal)
		{
			rectangle.Offset(rectangle.Width, 0);
		}
		else
		{
			rectangle.Offset(0, rectangle.Height);
		}
		PaintButton(graphics, mdiSystemItem, SystemButton.Restore, rectangle, GetColorTable(mdiSystemItem, SystemButton.Restore), m_ColorTable.LegacyColors);
		if (mdiSystemItem.Orientation == eOrientation.Horizontal)
		{
			rectangle.Offset(rectangle.Width + 2, 0);
		}
		else
		{
			rectangle.Offset(0, rectangle.Height + 2);
		}
		PaintButton(graphics, mdiSystemItem, SystemButton.Close, rectangle, GetColorTable(mdiSystemItem, SystemButton.Close), m_ColorTable.LegacyColors);
	}

	private Office2007ButtonItemStateColorTable GetColorTable(MDISystemItem mdi, SystemButton button)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = m_ColorTable.ButtonItemColors[eButtonColor.Orange.ToString()];
		Office2007ButtonItemStateColorTable result = office2007ButtonItemColorTable.Default;
		if (mdi.MouseDownButton == button)
		{
			result = office2007ButtonItemColorTable.Pressed;
		}
		else if (mdi.MouseOverButton == button)
		{
			result = office2007ButtonItemColorTable.MouseOver;
		}
		return result;
	}

	private void PaintButton(Graphics g, MDISystemItem mdi, SystemButton button, Rectangle r, Office2007ButtonItemStateColorTable ct, ColorScheme colorScheme)
	{
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Expected O, but got Unknown
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Expected O, but got Unknown
		Region clip = g.Clip;
		g.SetClip(r);
		Office2007ButtonItemPainter.PaintBackground(g, ct, r, RoundRectangleShapeDescriptor.RoundCorner2);
		r.Inflate(-1, -1);
		r.Offset(1, 0);
		Bitmap buttonBitmap = mdi.GetButtonBitmap(g, button, r, colorScheme);
		try
		{
			if ((button == SystemButton.Minimize && !mdi.MinimizeEnabled) || (button == SystemButton.Restore && !mdi.RestoreEnabled) || (button == SystemButton.Close && !mdi.CloseEnabled))
			{
				ColorMatrix colorMatrix = new ColorMatrix(new float[5][]
				{
					new float[5],
					new float[5],
					new float[5],
					new float[5] { 0.5f, 0.5f, 0.5f, 0.5f, 0f },
					new float[5]
				});
				ImageAttributes val = new ImageAttributes();
				val.ClearColorKey();
				val.SetColorMatrix(colorMatrix);
				g.DrawImage((Image)(object)buttonBitmap, r, 0, 0, ((Image)buttonBitmap).Width, ((Image)buttonBitmap).Height, (GraphicsUnit)2, val);
			}
			else
			{
				if (button == mdi.MouseDownButton)
				{
					r.Offset(1, 1);
				}
				g.DrawImageUnscaled((Image)(object)buttonBitmap, r);
			}
		}
		finally
		{
			((IDisposable)buttonBitmap)?.Dispose();
		}
		g.Clip = clip;
	}
}
