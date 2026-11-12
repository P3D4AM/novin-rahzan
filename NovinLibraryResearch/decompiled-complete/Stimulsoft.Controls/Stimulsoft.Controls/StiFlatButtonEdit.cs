using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls;

[ToolboxItem(false)]
public class StiFlatButtonEdit : StiButtonEdit
{
	protected override void OnPaint(PaintEventArgs p)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		base.OnPaint(p);
		Graphics graphics = p.Graphics;
		Rectangle rectangle = new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height);
		graphics.FillRectangle((Brush)new SolidBrush(((Control)this).BackColor), rectangle);
		Rectangle buttonRect = GetButtonRect();
		if (ShowButton)
		{
			Image buttonBitmap = base.ButtonBitmap;
			PaintButtons(graphics, buttonRect, (buttonBitmap is Bitmap) ? buttonBitmap : null, base.IsPressed, IsMouseOverButton);
		}
	}

	protected override void PaintButtons(Graphics g, Rectangle buttonRect, Image bmp, bool isPressed, bool isMouseOverButton)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		Color color = ((Control)this).BackColor;
		if (isPressed)
		{
			color = StiColorUtils.Dark(color, 30);
		}
		else if (isMouseOverButton)
		{
			color = StiColorUtils.Dark(color, 10);
		}
		SolidBrush val = new SolidBrush(color);
		try
		{
			g.FillRectangle((Brush)(object)val, buttonRect);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		if (bmp != null)
		{
			Rectangle rectangle = new Rectangle(buttonRect.X + (buttonRect.Width - bmp.Width) / 2, buttonRect.Y + (buttonRect.Height - bmp.Height) / 2, bmp.Width, bmp.Height);
			if (((Control)this).Enabled)
			{
				g.DrawImage(bmp, rectangle);
			}
			else
			{
				StiControlPaint.DrawImageDisabled(g, bmp, rectangle);
			}
		}
	}

	public override Rectangle GetButtonRect()
	{
		Rectangle result = new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height);
		result.X = StiScaleUI.I1;
		result.Width = base.DropDownButtonWidth;
		return result;
	}

	protected override Rectangle GetContentRect()
	{
		return new Rectangle(base.DropDownButtonWidth + 2, StiScaleUI.I2, ((Control)this).Width - base.DropDownButtonWidth - 4, ((Control)this).Height - 4);
	}
}
