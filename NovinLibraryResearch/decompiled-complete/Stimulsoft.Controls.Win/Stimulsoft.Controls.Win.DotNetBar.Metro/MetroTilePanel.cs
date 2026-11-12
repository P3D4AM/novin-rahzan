using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar.Metro;

public class MetroTilePanel : ItemPanel
{
	private const int DefaultItemSpacing = 32;

	[Browsable(true)]
	[DefaultValue(true)]
	public new virtual bool AutoScroll
	{
		get
		{
			return base.AutoScroll;
		}
		set
		{
			base.AutoScroll = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(32)]
	[Category("Layout")]
	[Description("Indicates spacing in pixels between items.")]
	public override int ItemSpacing
	{
		get
		{
			return base.ItemSpacing;
		}
		set
		{
			base.ItemSpacing = value;
		}
	}

	[Browsable(false)]
	[DefaultValue(false)]
	[Category("Layout")]
	public override bool ResizeItemsToFit
	{
		get
		{
			return base.ResizeItemsToFit;
		}
		set
		{
			base.ResizeItemsToFit = value;
		}
	}

	protected override Size DefaultSize => new Size(600, 400);

	public MetroTilePanel()
	{
		ResizeItemsToFit = false;
		ItemSpacing = 32;
		AutoScroll = true;
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		if (!((Component)(object)this).DesignMode)
		{
			GetBaseItemContainer().AllowDrop = false;
		}
		base.OnHandleCreated(e);
	}

	protected override void OnDragInProgressChanged()
	{
		if (!base.DragInProgress)
		{
			base.AnimateNextPaintRequest = true;
		}
		((Control)this).Invalidate();
		base.OnDragInProgressChanged();
	}

	protected override void OnMouseDragOverProcessed(int x, int y, DragEventArgs dragArgs)
	{
		base.OnMouseDragOverProcessed(x, y, dragArgs);
		((Control)this).Invalidate();
	}
}
