using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls;

[ToolboxItem(false)]
[Designer(typeof(StiOutlookBarDesigner))]
[ToolboxBitmap(typeof(StiOutlookBar), "Toolbox.StiOutlookBar.bmp")]
public class StiOutlookBar : Panel
{
	private bool drawBorder;

	[DefaultValue(false)]
	[Category("Appearance")]
	public bool DrawBorder
	{
		get
		{
			return drawBorder;
		}
		set
		{
			drawBorder = value;
			if (value)
			{
				((ScrollableControl)this).DockPadding.All = 3;
			}
			else
			{
				((ScrollableControl)this).DockPadding.All = 0;
			}
			((Control)this).Invalidate();
		}
	}

	[Browsable(false)]
	public override bool AutoScroll
	{
		get
		{
			return ((ScrollableControl)this).AutoScroll;
		}
		set
		{
			((ScrollableControl)this).AutoScroll = value;
		}
	}

	protected override void OnSystemColorsChanged(EventArgs e)
	{
		((Control)this).OnSystemColorsChanged(e);
		StiColors.InitColors();
	}

	protected override void OnPaint(PaintEventArgs p)
	{
		if (DrawBorder)
		{
			Graphics graphics = p.Graphics;
			Rectangle rectangle = new Rectangle(0, 0, ((Control)this).Width - 1, ((Control)this).Height - 1);
			graphics.DrawRectangle(SystemPens.ControlDark, rectangle);
		}
	}

	protected override void OnBackColorChanged(EventArgs e)
	{
		((Control)this).OnBackColorChanged(e);
		((Control)this).Invalidate();
	}

	public StiOutlookPanel AddCategory(string name)
	{
		StiOutlookPanel stiOutlookPanel = new StiOutlookPanel();
		((Control)stiOutlookPanel).Text = name;
		((Control)this).Controls.Add((Control)(object)stiOutlookPanel);
		((Control)stiOutlookPanel).Dock = (DockStyle)1;
		((Control)this).Controls.SetChildIndex((Control)(object)stiOutlookPanel, 0);
		return stiOutlookPanel;
	}

	public StiOutlookBar()
	{
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((ScrollableControl)this).AutoScroll = true;
	}
}
