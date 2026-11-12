using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls.Menu;

[ToolboxItem(false)]
public class StiSeparatorContextMenuItem : ToolStripMenuItem
{
	public bool IsDarkMode
	{
		get
		{
			if (((ToolStripItem)this).Parent is StiContextMenu)
			{
				return (((ToolStripItem)this).Parent as StiContextMenu).IsDarkMode;
			}
			if (((ToolStripItem)this).OwnerItem is StiContextMenuItem)
			{
				return (((ToolStripItem)this).OwnerItem as StiContextMenuItem).IsDarkMode;
			}
			return false;
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		Graphics graphics = e.Graphics;
		Rectangle rect = new Rectangle(((ToolStripItem)this).ContentRectangle.X, ((ToolStripItem)this).ContentRectangle.Y, ((Control)((ToolStripItem)this).Owner).Width, ((ToolStripItem)this).ContentRectangle.Height);
		rect.Inflate(StiScaleUI.I3, StiScaleUI.I3);
		PaintBackground(graphics, rect);
		PaintSeparator(graphics, rect);
	}

	private void PaintSeparator(Graphics g, Rectangle rect)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		Color color = StiColor.Get("888888");
		Pen val = new Pen(color);
		try
		{
			g.DrawLine(val, rect.X + StiScaleUI.XXI(20.0), rect.Y + rect.Height / 2, rect.Right - StiScaleUI.I2, rect.Y + rect.Height / 2);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void PaintBackground(Graphics g, Rectangle rect)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Expected O, but got Unknown
		Color color = StiColor.Get("ffffff");
		if (IsDarkMode)
		{
			color = StiColor.Get("2b2b2b");
		}
		SolidBrush val = new SolidBrush(color);
		try
		{
			g.FillRectangle((Brush)(object)val, rect);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public StiSeparatorContextMenuItem()
	{
		((ToolStripItem)this).AutoSize = false;
		((ToolStripItem)this).Width = StiScaleUI.XXI(500.0);
		((ToolStripItem)this).Height = StiScaleUI.I10;
	}
}
