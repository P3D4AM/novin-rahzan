using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls;

[ToolboxItem(false)]
public class StiOperationCheckBox : CheckBox
{
	private bool isPressed;

	private bool isMouseOver;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool ShowBorder { get; set; } = true;


	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Color BorderColor { get; set; } = StiColor.Get("#adadad");


	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Color BorderColorPressed { get; set; } = StiColor.Get("#005499");


	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Color BorderColorMouseOver { get; set; } = StiColor.Get("#0078d7");


	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Color BorderColorChecked { get; set; } = StiColor.Get("#005499");


	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Color BackColorPressed { get; set; } = StiColor.Get("#cce4f7");


	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Color BackColorMouseOver { get; set; } = StiColor.Get("#e5f1fb");


	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Color BackColorChecked { get; set; } = StiColor.Get("#cce4f7");


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
		((CheckBox)this).OnMouseUp(e);
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
		if (ShowBorder)
		{
			OnDrawBorder(graphics);
		}
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
			color = BackColorPressed;
		}
		else if (isMouseOver)
		{
			color = BackColorMouseOver;
		}
		else if (((CheckBox)this).Checked)
		{
			color = BackColorChecked;
		}
		StiDrawing.FillRectangle(g, color, buttonRect);
	}

	private void OnDrawBorder(Graphics g)
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		Rectangle buttonRect = GetButtonRect();
		Color color = BorderColor;
		if (((CheckBox)this).Checked)
		{
			color = BorderColorChecked;
		}
		else if (isPressed)
		{
			color = BorderColorPressed;
		}
		else if (isMouseOver)
		{
			color = BorderColorMouseOver;
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

	public StiOperationCheckBox()
	{
		((Control)this).SetStyle((ControlStyles)512, false);
		((Control)this).SetStyle((ControlStyles)131072, true);
		((ButtonBase)this).UseVisualStyleBackColor = false;
	}
}
