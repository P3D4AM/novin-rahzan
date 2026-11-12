using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls;

[ToolboxItem(false)]
public class StiRadioButtonUX : RadioButton
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
			return StiUX.InputForeground;
		}
		set
		{
			if (!StiUX.IsDark)
			{
				((Control)this).ForeColor = value;
			}
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
		((RadioButton)this).OnMouseUp(mevent);
		IsPressed = false;
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		if (!StiUX.IsDark || (int)((RadioButton)this).Appearance == 0)
		{
			((ButtonBase)this).OnPaint(e);
			return;
		}
		Graphics graphics = e.Graphics;
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		Color backgroundColor = GetBackgroundColor();
		Color foregroundColor = GetForegroundColor();
		StiDrawing.FillRectangle(graphics, backgroundColor, clientRectangle);
		if (((RadioButton)this).Checked && StiUX.IsDark)
		{
			StiDrawing.FillRectangle(graphics, StiUX.Accent.ChangeAlpha(50), clientRectangle);
		}
		TextFormatFlags val = (TextFormatFlags)21;
		if (string.IsNullOrWhiteSpace(((Control)this).Text))
		{
			TextRenderer.DrawText((IDeviceContext)(object)graphics, ((Control)this).Text, ((Control)this).Font, clientRectangle, foregroundColor, val);
		}
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
		StiDrawing.DrawRectangle(graphics, GetBorderColor(), clientRectangle);
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
		if (((RadioButton)this).Checked)
		{
			return StiUX.ButtonForegroundChecked;
		}
		return StiUX.ButtonForeground;
	}

	private Color GetBorderColor()
	{
		if (((RadioButton)this).Checked)
		{
			return StiUX.Accent;
		}
		return StiUX.ButtonBorder;
	}
}
