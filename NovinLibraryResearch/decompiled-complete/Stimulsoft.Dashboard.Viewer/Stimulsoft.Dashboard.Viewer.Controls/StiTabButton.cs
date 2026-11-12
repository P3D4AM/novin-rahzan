using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Dashboard.Viewer.Controls;

[ToolboxItem(false)]
public class StiTabButton : StiActionCheckBox
{
	private bool isCloseButtonOver;

	internal static int DefaultCloseButtonWidth = StiScale.XXI(20.0);

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool ShowCloseButton { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	private bool IsMouseOverCloseButton
	{
		get
		{
			return isCloseButtonOver;
		}
		set
		{
			if (isCloseButtonOver != value)
			{
				isCloseButtonOver = value;
				InvalidateCloseButton();
			}
		}
	}

	public event EventHandler CloseButtonClick;

	protected override void OnPaint(PaintEventArgs e)
	{
		Rectangle rect = new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height);
		Graphics graphics = e.Graphics;
		PaintBackground(graphics, rect);
		PaintImage(graphics, rect);
		PaintCloseButton(graphics, rect);
		if (ShowCloseButton)
		{
			rect.Width -= DefaultCloseButtonWidth;
		}
		PaintText(graphics, rect);
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		((ButtonBase)this).OnMouseMove(e);
		if (ShowCloseButton)
		{
			IsMouseOverCloseButton = ((Control)this).Width - DefaultCloseButtonWidth < e.X && e.X < ((Control)this).Width;
		}
	}

	protected override void OnMouseEnter(EventArgs eventargs)
	{
		((ButtonBase)this).OnMouseEnter(eventargs);
		((Control)this).Invalidate();
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		((ButtonBase)this).OnMouseLeave(e);
		if (ShowCloseButton)
		{
			IsMouseOverCloseButton = false;
		}
		((Control)this).Invalidate();
	}

	protected override void OnClick(EventArgs e)
	{
		if (IsMouseOverCloseButton)
		{
			this.CloseButtonClick?.Invoke(this, e);
		}
		else
		{
			((CheckBox)this).OnClick(e);
		}
	}

	private void PaintCloseButton(Graphics g, Rectangle rect)
	{
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Expected O, but got Unknown
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		if (!base.IsMouseOver || !ShowCloseButton)
		{
			return;
		}
		Rectangle rect2 = new Rectangle(rect.Right - DefaultCloseButtonWidth, rect.Y, DefaultCloseButtonWidth, rect.Height);
		int i = StiScale.I7;
		int i2 = StiScale.I7;
		RectangleF rectangleF = new RectangleF(rect2.X + (rect2.Width - i) / 2, rect2.Y + (rect2.Height - i2) / 2, i, i2);
		if (IsMouseOverCloseButton)
		{
			Color backColor = base.Colors.GetBackColor(((CheckBox)this).Checked, IsMouseOverCloseButton);
			backColor = StiColorUtils.Light(backColor, 20);
			rect2.Inflate(-StiScale.I2, -StiScale.I2);
			StiDrawing.FillRectangle(g, backColor, rect2);
		}
		Color foreColor = base.Colors.GetForeColor(((CheckBox)this).Checked || base.IsPressed, base.IsMouseOver);
		Pen val = new Pen(foreColor, 1.7f);
		try
		{
			SmoothingMode smoothingMode = g.SmoothingMode;
			try
			{
				g.SmoothingMode = (SmoothingMode)4;
				g.DrawLine(val, rectangleF.X, rectangleF.Y, rectangleF.Right, rectangleF.Bottom);
				g.DrawLine(val, rectangleF.Right, rectangleF.Y, rectangleF.X, rectangleF.Bottom);
			}
			finally
			{
				g.SmoothingMode = smoothingMode;
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void InvalidateCloseButton()
	{
		Rectangle rectangle = new Rectangle(((Control)this).Width - DefaultCloseButtonWidth - 1, 1, DefaultCloseButtonWidth, ((Control)this).Height - 2);
		((Control)this).Invalidate(rectangle);
	}

	public void CalculateAutoWidth()
	{
		((Control)this).Width = TextRenderer.MeasureText(((Control)this).Text, ((Control)this).Font).Width + StiScale.I2 + (ShowCloseButton ? DefaultCloseButtonWidth : 0);
	}

	public StiTabButton(StiControlColorCollection colors, Point position)
		: base(colors, position)
	{
		((Control)this).Anchor = (AnchorStyles)4;
		((CheckBox)this).AutoCheck = false;
	}
}
