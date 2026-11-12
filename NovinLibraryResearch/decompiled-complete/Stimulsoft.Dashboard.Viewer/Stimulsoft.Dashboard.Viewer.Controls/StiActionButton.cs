using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Dashboard.Viewer.Controls;

[ToolboxItem(false)]
public class StiActionButton : Button
{
	public const int ButtonSize = 22;

	private StiControlColorCollection colors;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiControlColorCollection Colors
	{
		get
		{
			return colors;
		}
		set
		{
			if (value != colors)
			{
				colors = value;
				colors.Control = (Control)(object)this;
				((Control)this).Invalidate();
			}
		}
	}

	private bool IsMouseOver => ((Control)this).ClientRectangle.Contains(((Control)this).PointToClient(Cursor.Position));

	private bool IsPressed { get; set; }

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public string ToolTip { get; set; }

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool DrawBorder { get; set; }

	protected override void OnPaint(PaintEventArgs e)
	{
		Rectangle rect = new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height);
		Graphics graphics = e.Graphics;
		PaintBackground(graphics, rect);
		PaintImage(graphics, rect);
		PaintText(graphics, rect);
		PaintBorder(graphics, rect);
	}

	protected override void OnPaintBackground(PaintEventArgs e)
	{
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

	protected override void OnMouseLeave(EventArgs e)
	{
		((Button)this).OnMouseLeave(e);
		IsPressed = false;
	}

	public void InvokeClick()
	{
		((Control)this).InvokeOnClick((Control)(object)this, EventArgs.Empty);
	}

	protected virtual void PaintBackground(Graphics g, Rectangle rect)
	{
		Color color = Colors.GetBackColor(IsPressed, IsMouseOver);
		if (!colors.IsDarkMode && IsPressed)
		{
			color = StiColor.Get("bbbbbb");
		}
		StiDrawing.FillRectangle(g, color, rect);
	}

	protected virtual void PaintBorder(Graphics g, Rectangle rect)
	{
		if (DrawBorder)
		{
			Color foreColor = Colors.GetForeColor(isSelected: false, isHot: false);
			foreColor = Color.FromArgb(128, foreColor);
			rect.Width--;
			rect.Height--;
			StiDrawing.DrawRectangle(g, foreColor, RectangleD.CreateFromRectangle(rect));
		}
	}

	private void PaintImage(Graphics g, Rectangle rect)
	{
		if (((ButtonBase)this).Image == null)
		{
			return;
		}
		GraphicsState val = g.Save();
		try
		{
			g.SetClip(rect);
			Point point = new Point((rect.Width - ((ButtonBase)this).Image.Width) / 2, (rect.Height - ((ButtonBase)this).Image.Height) / 2);
			if (!string.IsNullOrEmpty(((Control)this).Text))
			{
				point.X = StiScale.I2;
			}
			g.DrawImage(((ButtonBase)this).Image, new Rectangle(point.X, point.Y, ((ButtonBase)this).Image.Width, ((ButtonBase)this).Image.Height));
		}
		finally
		{
			g.Restore(val);
		}
	}

	private void PaintText(Graphics g, Rectangle rect)
	{
		if (string.IsNullOrEmpty(((Control)this).Text))
		{
			return;
		}
		GraphicsState val = g.Save();
		int num = StiScale.I2;
		if (((ButtonBase)this).Image != null)
		{
			num += ((ButtonBase)this).Image.Width + StiScale.I2;
		}
		Rectangle rectangle = new Rectangle(num, rect.Y, rect.Width - num - StiScale.I2, rect.Height - 1);
		try
		{
			g.SetClip(rectangle);
			Color foreColor = Colors.GetForeColor(IsPressed, IsMouseOver);
			PaintTextInternal(g, rectangle, foreColor);
		}
		finally
		{
			g.Restore(val);
		}
	}

	protected virtual void PaintTextInternal(Graphics g, Rectangle textRect, Color foreColor)
	{
		TextRenderer.DrawText((IDeviceContext)(object)g, ((Control)this).Text, ((Control)this).Font, textRect, foreColor);
	}

	public StiActionButton(StiControlColorCollection colors, Point? position = null)
	{
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)512, false);
		((Control)this).SetStyle((ControlStyles)2048, false);
		Colors = colors;
		((Control)this).Anchor = (AnchorStyles)9;
		_ = ((Control)this).Location;
		((Control)this).Location = position.GetValueOrDefault();
		((Control)this).Width = (int)StiScale.XX(22.0);
		((Control)this).Height = (int)StiScale.YY(22.0);
		((Control)this).TabStop = false;
		((Control)this).Visible = false;
	}
}
