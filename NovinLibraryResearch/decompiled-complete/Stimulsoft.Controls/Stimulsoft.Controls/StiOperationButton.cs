using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls;

[ToolboxItem(false)]
public class StiOperationButton : Button
{
	private bool isPressed;

	private bool isMouseOver;

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
			if (!StiUX.IsDark)
			{
				return ((ButtonBase)this).BackColor;
			}
			return StiUX.ButtonBackground;
		}
		set
		{
			if (!StiUX.IsDark)
			{
				((ButtonBase)this).BackColor = value;
			}
		}
	}

	protected override void OnMouseEnter(EventArgs e)
	{
		((Button)this).OnMouseEnter(e);
		isMouseOver = true;
		((Control)this).Invalidate();
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		((Button)this).OnMouseLeave(e);
		isMouseOver = false;
		((Control)this).Invalidate();
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		((Button)this).OnMouseUp(e);
		isPressed = false;
		((Control)this).Invalidate();
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		((ButtonBase)this).OnMouseDown(e);
		isPressed = true;
		((Control)this).Invalidate();
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
		Color color = ((Control)this).BackColor;
		if (isPressed)
		{
			color = (StiUX.IsDark ? StiUX.ButtonBackgroundPressed : StiColorUtils.Dark(color, 30));
		}
		else if (isMouseOver)
		{
			color = (StiUX.IsDark ? StiUX.ButtonBackgroundHover : StiColors.Selected);
		}
		StiDrawing.FillRectangle(g, color, buttonRect);
	}

	private void OnDrawBorder(Graphics g)
	{
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		Rectangle buttonRect = GetButtonRect();
		Color color = (StiUX.IsDark ? StiUX.ButtonBorder : Color.LightGray);
		if (isPressed)
		{
			color = (StiUX.IsDark ? StiUX.ButtonBorder : StiColorUtils.Dark(Color.LightGray, 30));
		}
		else if (isMouseOver)
		{
			color = (StiUX.IsDark ? StiUX.ButtonBorder : StiColors.SelectedText);
		}
		Pen val = new Pen(color);
		try
		{
			buttonRect.Width--;
			buttonRect.Height--;
			g.DrawRectangle(val, buttonRect);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
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

	public StiOperationButton()
	{
		((Control)this).SetStyle((ControlStyles)512, false);
		((Control)this).SetStyle((ControlStyles)131072, true);
		((ButtonBase)this).UseVisualStyleBackColor = false;
	}
}
