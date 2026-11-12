using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Dashboard.Viewer.Controls;

[ToolboxItem(false)]
public class StiActionCheckBox : CheckBox
{
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

	protected bool IsMouseOver => ((Control)this).ClientRectangle.Contains(((Control)this).PointToClient(Cursor.Position));

	protected bool IsPressed { get; set; }

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public string ToolTip { get; set; }

	protected override void OnPaint(PaintEventArgs e)
	{
		Rectangle rect = new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height);
		Graphics graphics = e.Graphics;
		PaintBackground(graphics, rect);
		PaintImage(graphics, rect);
		PaintText(graphics, rect);
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
		((CheckBox)this).OnMouseUp(mevent);
		IsPressed = false;
	}

	protected void PaintBackground(Graphics g, Rectangle rect)
	{
		Color backColor = Colors.GetBackColor(((CheckBox)this).Checked || IsPressed, IsMouseOver);
		StiDrawing.FillRectangle(g, backColor, rect);
	}

	protected void PaintImage(Graphics g, Rectangle rect)
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

	protected void PaintText(Graphics g, Rectangle rect)
	{
		if (string.IsNullOrEmpty(((Control)this).Text))
		{
			return;
		}
		GraphicsState val = g.Save();
		int num = StiScale.I2;
		if (((ButtonBase)this).Image != null)
		{
			num += StiScale.XXI(((ButtonBase)this).Image.Width) + StiScale.I2;
		}
		Rectangle rectangle = new Rectangle(num, rect.Y, rect.Width - num - StiScale.I2, rect.Height - 1);
		try
		{
			g.SetClip(rectangle);
			Color foreColor = Colors.GetForeColor(((CheckBox)this).Checked || IsPressed, IsMouseOver);
			TextRenderer.DrawText((IDeviceContext)(object)g, ((Control)this).Text, ((Control)this).Font, rectangle, foreColor);
		}
		finally
		{
			g.Restore(val);
		}
	}

	public StiActionCheckBox(StiControlColorCollection colors, Point? position = null)
	{
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)512, false);
		((Control)this).SetStyle((ControlStyles)2048, false);
		Colors = colors;
		((CheckBox)this).Appearance = (Appearance)1;
		((Control)this).Anchor = (AnchorStyles)9;
		if (position.HasValue)
		{
			((Control)this).Location = position.GetValueOrDefault();
		}
		((Control)this).Width = (int)StiScale.XX(22.0);
		((Control)this).Height = (int)StiScale.YY(22.0);
		((Control)this).TabStop = false;
		((Control)this).Visible = false;
	}
}
