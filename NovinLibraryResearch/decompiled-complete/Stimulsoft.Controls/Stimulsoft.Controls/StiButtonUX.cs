using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls;

[ToolboxItem(false)]
public class StiButtonUX : Button
{
	private bool isPressed;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override Color ForeColor
	{
		get
		{
			if (!StiUX.IsDark)
			{
				return ((Control)this).ForeColor;
			}
			return StiUX.ButtonForeground;
		}
		set
		{
			if (!StiUX.IsDark)
			{
				((Control)this).ForeColor = value;
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override Color BackColor
	{
		get
		{
			if (StiUX.IsDark)
			{
				if (!((Control)this).Focused)
				{
					return StiUX.ButtonBackground;
				}
				return StiUX.Accent;
			}
			return Color.White;
		}
		set
		{
			if (!StiUX.IsDark)
			{
				((ButtonBase)this).BackColor = value;
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public FlatStyle FlatStyle
	{
		get
		{
			if (!StiUX.IsDark)
			{
				return (FlatStyle)2;
			}
			return (FlatStyle)0;
		}
		set
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			value = ((!StiUX.IsDark) ? ((FlatStyle)2) : ((FlatStyle)0));
			((ButtonBase)this).FlatStyle = value;
		}
	}

	private bool IsMouseOver => ((Control)this).ClientRectangle.Contains(((Control)this).PointToClient(Cursor.Position));

	private bool IsPressed
	{
		get
		{
			return isPressed;
		}
		set
		{
			if (isPressed != value)
			{
				isPressed = value;
				((Control)this).Invalidate();
			}
		}
	}

	protected override void OnMouseDown(MouseEventArgs mevent)
	{
		((ButtonBase)this).OnMouseDown(mevent);
		IsPressed = true;
	}

	protected override void OnMouseUp(MouseEventArgs mevent)
	{
		((Button)this).OnMouseUp(mevent);
		IsPressed = false;
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Expected O, but got Unknown
		if (!StiUX.IsDark)
		{
			((ButtonBase)this).OnPaint(e);
			return;
		}
		Graphics graphics = e.Graphics;
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		Color backgroundColor = GetBackgroundColor();
		Color foregroundColor = GetForegroundColor();
		StiDrawing.FillRectangle(graphics, backgroundColor, clientRectangle);
		if (((ButtonBase)this).IsDefault && StiUX.IsDark)
		{
			StiDrawing.FillRectangle(graphics, StiUX.Accent.ChangeAlpha(50), clientRectangle);
		}
		TextFormatFlags val = (TextFormatFlags)21;
		TextRenderer.DrawText((IDeviceContext)(object)graphics, ((Control)this).Text, ((Control)this).Font, clientRectangle, foregroundColor, val);
		if (((ButtonBase)this).Image != null)
		{
			Rectangle rectangle = new Rectangle(clientRectangle.X + (clientRectangle.Width - ((ButtonBase)this).Image.Width) / 2, clientRectangle.Y + (clientRectangle.Height - ((ButtonBase)this).Image.Height) / 2, ((ButtonBase)this).Image.Width, ((ButtonBase)this).Image.Height);
			if (((Control)this).Enabled)
			{
				graphics.DrawImage(((ButtonBase)this).Image, rectangle);
			}
			else
			{
				StiControlPaint.DrawImageDisabled(graphics, ((ButtonBase)this).Image, rectangle);
			}
		}
		clientRectangle.Width--;
		clientRectangle.Height--;
		Pen val2 = new Pen(GetBorderColor());
		try
		{
			if (((Control)this).Focused)
			{
				val2.Width = 2f;
				val2.Alignment = (PenAlignment)1;
			}
			graphics.DrawRectangle(val2, clientRectangle);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	protected override void OnGotFocus(EventArgs e)
	{
		((ButtonBase)this).OnGotFocus(e);
		if (StiUX.IsDark)
		{
			((ButtonBase)this).FlatAppearance.BorderColor = StiUX.Accent;
			((Control)this).Invalidate();
		}
	}

	protected override void OnLostFocus(EventArgs e)
	{
		((ButtonBase)this).OnLostFocus(e);
		if (StiUX.IsDark)
		{
			((ButtonBase)this).FlatAppearance.BorderColor = StiUX.ButtonBorder;
			((Control)this).Invalidate();
		}
	}

	private Color GetBackgroundColor()
	{
		if (!((Control)this).Enabled)
		{
			return StiUX.ButtonBackgroundDisabled;
		}
		if (IsMouseOver)
		{
			return StiUX.ButtonBackgroundHover;
		}
		if (IsPressed)
		{
			return StiUX.ButtonBackgroundPressed;
		}
		return StiUX.ButtonBackground;
	}

	private Color GetForegroundColor()
	{
		if (!((Control)this).Enabled)
		{
			return StiUX.ButtonForegroundDisabled;
		}
		if (IsMouseOver)
		{
			return StiUX.ButtonForegroundHover;
		}
		if (IsPressed)
		{
			return StiUX.ButtonForegroundPressed;
		}
		return StiUX.ButtonForeground;
	}

	private Color GetBorderColor()
	{
		if (((Control)this).Focused)
		{
			return StiUX.Accent;
		}
		if (((ButtonBase)this).IsDefault)
		{
			return StiUX.Accent;
		}
		return StiUX.ButtonBorder;
	}

	public StiButtonUX()
	{
		if (StiUX.IsDark)
		{
			((ButtonBase)this).FlatAppearance.BorderColor = StiUX.ButtonBorder;
			((ButtonBase)this).FlatAppearance.MouseDownBackColor = StiUX.ButtonBackgroundPressed;
			((ButtonBase)this).FlatAppearance.MouseOverBackColor = StiUX.ButtonBackgroundHover;
			((ButtonBase)this).FlatAppearance.CheckedBackColor = StiUX.ButtonBackgroundChecked;
			FlatStyle = (FlatStyle)0;
			((ButtonBase)this).UseVisualStyleBackColor = false;
		}
		else
		{
			FlatStyle = (FlatStyle)2;
			((ButtonBase)this).UseVisualStyleBackColor = true;
		}
		if (StiUX.IsDark)
		{
			((ButtonBase)this).BackColor = StiUX.ButtonBackground;
		}
	}
}
