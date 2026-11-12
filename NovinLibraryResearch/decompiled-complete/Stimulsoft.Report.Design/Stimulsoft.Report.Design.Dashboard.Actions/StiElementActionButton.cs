using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Design.Dashboard.Actions;

[ToolboxItem(false)]
public class StiElementActionButton : Button
{
	private bool IsMouseOver => ((Control)this).ClientRectangle.Contains(((Control)this).PointToClient(Cursor.Position));

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public string ToolTip { get; set; }

	public void InvokeClick()
	{
		((Control)this).InvokeOnClick((Control)(object)this, EventArgs.Empty);
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		Rectangle rect = new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height);
		Graphics graphics = e.Graphics;
		PaintBackground(graphics, rect);
		PaintBorder(graphics, rect);
		PaintImage(graphics, rect);
	}

	protected override void OnPaintBackground(PaintEventArgs e)
	{
	}

	private void PaintBorder(Graphics g, Rectangle rect)
	{
		Color color = (IsMouseOver ? Color.Gray : Color.DarkGray);
		rect.Width--;
		rect.Height--;
		StiDrawing.DrawRectangle(g, color, rect.X, rect.Y, rect.Width, rect.Height);
	}

	private void PaintBackground(Graphics g, Rectangle rect)
	{
		Color color = (IsMouseOver ? StiColor.Get("e2e2e2") : StiColor.Get("ffffff"));
		StiDrawing.FillRectangle(g, color, rect);
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
			rect.Inflate(-StiScale.I3, -StiScale.I3);
			rect = new Rectangle(rect.X + (rect.Width - ((ButtonBase)this).Image.Width) / 2, rect.Y + (rect.Width - ((ButtonBase)this).Image.Width) / 2, ((ButtonBase)this).Image.Width, ((ButtonBase)this).Image.Height);
			g.DrawImage(((ButtonBase)this).Image, rect);
		}
		finally
		{
			g.Restore(val);
		}
	}

	public StiElementActionButton()
	{
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)512, false);
		((Control)this).SetStyle((ControlStyles)2048, false);
		((Control)this).TabStop = false;
	}
}
