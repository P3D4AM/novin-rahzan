using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls;

[ToolboxItem(false)]
public class StiFontIconButton : RadioButton
{
	protected bool isMouseOver;

	protected bool isPressed;

	public static void DrawButton(Graphics g, Rectangle rect, string text, Font font, Color foreColor, ImageList imageList, int imageIndex, Image image, RightToLeft rightToLeft, bool isEnabled, bool isMouseOver, bool isPressed, bool isDefault, bool isChecked, bool isFocused)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Expected O, but got Unknown
		if (isChecked)
		{
			SolidBrush val = new SolidBrush(Color.FromArgb(210, 210, 210));
			try
			{
				g.FillRectangle((Brush)(object)val, rect);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		if (isMouseOver)
		{
			SolidBrush val2 = new SolidBrush(Color.FromArgb(230, 230, 230));
			try
			{
				g.FillRectangle((Brush)(object)val2, rect);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		SolidBrush val3 = new SolidBrush(foreColor);
		try
		{
			StringFormat val4 = new StringFormat();
			try
			{
				val4.Alignment = (StringAlignment)1;
				val4.LineAlignment = (StringAlignment)1;
				g.DrawString(text, font, (Brush)(object)val3, (RectangleF)new Rectangle(0, 0, rect.Width, rect.Height), val4);
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
	}

	protected override void OnPaint(PaintEventArgs p)
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		Rectangle rectangle = new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height);
		Graphics graphics = p.Graphics;
		graphics.FillRectangle(Brushes.White, rectangle);
		DrawButton(graphics, rectangle, ((Control)this).Text, ((Control)this).Font, ((Control)this).ForeColor, ((ButtonBase)this).ImageList, ((ButtonBase)this).ImageIndex, ((ButtonBase)this).Image, ((Control)this).RightToLeft, ((Control)this).Enabled, isMouseOver, isPressed, ((ButtonBase)this).IsDefault, ((RadioButton)this).Checked, ((Control)this).Focused);
	}

	protected override void OnGotFocus(EventArgs e)
	{
		((ButtonBase)this).OnGotFocus(e);
		((Control)this).Invalidate();
	}

	protected override void OnLostFocus(EventArgs e)
	{
		((ButtonBase)this).OnLostFocus(e);
		((Control)this).Invalidate();
	}

	protected override void OnMouseEnter(EventArgs e)
	{
		((ButtonBase)this).OnMouseEnter(e);
		isMouseOver = true;
		((Control)this).Invalidate();
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		((ButtonBase)this).OnMouseLeave(e);
		isMouseOver = false;
		((Control)this).Invalidate();
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Invalid comparison between Unknown and I4
		((ButtonBase)this).OnMouseDown(e);
		if ((e.Button & 0x100000) > 0)
		{
			isPressed = true;
			((Control)this).Invalidate();
		}
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Invalid comparison between Unknown and I4
		((RadioButton)this).OnMouseUp(e);
		if ((e.Button & 0x100000) > 0)
		{
			isPressed = false;
			((Control)this).Invalidate();
		}
	}

	protected override void OnClick(EventArgs e)
	{
		((RadioButton)this).OnClick(e);
	}

	protected override void OnSystemColorsChanged(EventArgs e)
	{
		((Control)this).OnSystemColorsChanged(e);
		StiColors.InitColors();
	}

	public StiFontIconButton()
	{
		((Control)this).ForeColor = Color.FromArgb(68, 114, 196);
	}
}
