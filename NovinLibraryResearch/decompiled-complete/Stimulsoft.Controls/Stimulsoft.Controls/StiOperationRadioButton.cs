using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls;

[ToolboxItem(false)]
public class StiOperationRadioButton : RadioButton
{
	private bool isPressed;

	private bool isMouseOver;

	protected override void OnMouseEnter(EventArgs e)
	{
		((ButtonBase)this).OnMouseEnter(e);
		isMouseOver = true;
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		((ButtonBase)this).OnMouseLeave(e);
		isMouseOver = false;
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		((RadioButton)this).OnMouseUp(e);
		isPressed = false;
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		((ButtonBase)this).OnMouseDown(e);
		isPressed = true;
	}

	protected override void OnPaint(PaintEventArgs pevent)
	{
		Graphics graphics = pevent.Graphics;
		OnDrawBackground(graphics);
		OnDrawBorder(graphics);
		if (((ButtonBase)this).Image != null)
		{
			OnDrawImage(graphics);
		}
		else if (!string.IsNullOrWhiteSpace(((Control)this).Text))
		{
			OnDrawText(graphics);
		}
		else
		{
			OnDrawDownArrow(graphics);
		}
	}

	private void OnDrawBackground(Graphics g)
	{
		StiDrawing.FillRectangle(g, ((Control)this).BackColor, ((Control)this).ClientRectangle);
		Rectangle buttonRect = GetButtonRect();
		Color backgroundColor = GetBackgroundColor();
		StiDrawing.FillRectangle(g, backgroundColor, buttonRect);
		if (((RadioButton)this).Checked && StiUX.IsDark)
		{
			StiDrawing.FillRectangle(g, StiUX.Accent.ChangeAlpha(50), buttonRect);
		}
	}

	private Color GetBackgroundColor()
	{
		if (!((Control)this).Enabled)
		{
			if (!StiUX.IsDark)
			{
				return SystemColors.ControlLight;
			}
			return StiUX.ButtonBackgroundDisabled;
		}
		if (isPressed)
		{
			if (!StiUX.IsDark)
			{
				return StiColor.Get("#cce4f7");
			}
			return StiUX.ButtonBackgroundPressed;
		}
		if (isMouseOver)
		{
			if (!StiUX.IsDark)
			{
				return StiColor.Get("#e5f1fb");
			}
			return StiUX.ButtonBackgroundHover;
		}
		if (((RadioButton)this).Checked)
		{
			if (!StiUX.IsDark)
			{
				return StiColor.Get("#cce4f7");
			}
			return StiUX.ButtonBackgroundChecked;
		}
		if (!StiUX.IsDark)
		{
			return ((Control)this).BackColor;
		}
		return StiUX.ButtonBackground;
	}

	private void OnDrawBorder(Graphics g)
	{
		Rectangle buttonRect = GetButtonRect();
		Color borderColor = GetBorderColor();
		buttonRect.Width--;
		buttonRect.Height--;
		StiDrawing.DrawRectangle(g, borderColor, buttonRect);
	}

	private Color GetBorderColor()
	{
		if (isPressed)
		{
			if (!StiUX.IsDark)
			{
				return StiColor.Get("#005499");
			}
			return StiUX.ButtonBorder;
		}
		if (isMouseOver)
		{
			if (!StiUX.IsDark)
			{
				return StiColor.Get("#0078d7");
			}
			return StiUX.ButtonBorder;
		}
		if (((RadioButton)this).Checked)
		{
			if (!StiUX.IsDark)
			{
				return StiColor.Get("#005499");
			}
			return StiUX.Accent;
		}
		if (!StiUX.IsDark)
		{
			return StiColor.Get("#adadad");
		}
		return StiUX.ButtonBorder;
	}

	private Rectangle GetButtonRect()
	{
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		clientRectangle.Inflate(-1, -1);
		return clientRectangle;
	}

	private void OnDrawImage(Graphics g)
	{
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Expected O, but got Unknown
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Expected O, but got Unknown
		Rectangle rectangle = new Rectangle(((Control)this).ClientRectangle.X + (((Control)this).ClientRectangle.Width - ((ButtonBase)this).Image.Width) / 2, ((Control)this).ClientRectangle.Y + (((Control)this).ClientRectangle.Height - ((ButtonBase)this).Image.Height) / 2, ((ButtonBase)this).Image.Width, ((ButtonBase)this).Image.Height);
		if (((Control)this).Enabled)
		{
			g.DrawImage(((ButtonBase)this).Image, rectangle);
			return;
		}
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
		g.DrawImage(((ButtonBase)this).Image, rectangle, 0, 0, ((ButtonBase)this).Image.Width, ((ButtonBase)this).Image.Height, (GraphicsUnit)2, val);
	}

	private void OnDrawText(Graphics g)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		Color color = (isMouseOver ? StiColors.SelectedText : ((Control)this).ForeColor);
		StringFormat val = new StringFormat();
		try
		{
			SolidBrush val2 = new SolidBrush(color);
			try
			{
				val.Alignment = (StringAlignment)1;
				val.LineAlignment = (StringAlignment)1;
				val.FormatFlags = (StringFormatFlags)4096;
				val.Trimming = (StringTrimming)3;
				g.DrawString(((Control)this).Text, ((Control)this).Font, (Brush)(object)val2, (RectangleF)((Control)this).ClientRectangle, val);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void OnDrawDownArrow(Graphics g)
	{
		StiButtonHelper.DrawDownArrow(g, ((Control)this).ClientRectangle, ((Control)this).Enabled);
	}

	public StiOperationRadioButton()
	{
		((Control)this).SetStyle((ControlStyles)512, false);
		((Control)this).SetStyle((ControlStyles)131072, true);
		((ButtonBase)this).UseVisualStyleBackColor = false;
	}
}
