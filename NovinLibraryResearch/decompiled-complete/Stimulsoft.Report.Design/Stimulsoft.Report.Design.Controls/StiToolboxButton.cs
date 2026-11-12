using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Design.Controls;

[ToolboxItem(false)]
public class StiToolboxButton : Control
{
	private double stepScale = StiScale.Step;

	private bool isMouseOver;

	private bool isPressed;

	public Image Icon;

	private IContainer components;

	public StiToolboxButton()
	{
		((Control)this).Width = (int)Math.Ceiling(22.0 * stepScale);
		((Control)this).Height = (int)Math.Ceiling(22.0 * stepScale);
		((Control)this).BackColor = Color.White;
	}

	protected override void OnMouseEnter(EventArgs e)
	{
		((Control)this).OnMouseEnter(e);
		isMouseOver = true;
		((Control)this).Invalidate();
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		((Control)this).OnMouseLeave(e);
		isMouseOver = false;
		isPressed = false;
		((Control)this).Invalidate();
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		((Control)this).OnMouseDown(e);
		isPressed = true;
		((Control)this).Invalidate();
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		((Control)this).OnMouseUp(e);
		isPressed = false;
		((Control)this).Invalidate();
	}

	protected override void OnPaint(PaintEventArgs pevent)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Expected O, but got Unknown
		((Control)this).OnPaint(pevent);
		Graphics graphics = pevent.Graphics;
		if (isPressed)
		{
			SolidBrush val = new SolidBrush(Color.FromArgb(255, 163, 189, 227));
			try
			{
				graphics.FillRectangle((Brush)(object)val, 0, 0, ((Control)this).Width, ((Control)this).Height);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		else if (isMouseOver)
		{
			SolidBrush val2 = new SolidBrush(Color.FromArgb(255, 213, 225, 242));
			try
			{
				graphics.FillRectangle((Brush)(object)val2, 0, 0, ((Control)this).Width, ((Control)this).Height);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		if (Icon != null)
		{
			Rectangle rectangle = new Rectangle((((Control)this).Width - Icon.Width) / 2, (((Control)this).Height - Icon.Height) / 2, Icon.Width, Icon.Height);
			graphics.DrawImage(Icon, rectangle);
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((Control)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		this.components = new System.ComponentModel.Container();
	}
}
