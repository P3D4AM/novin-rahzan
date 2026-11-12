using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Metro.Rendering;

namespace Stimulsoft.Controls.Win.DotNetBar.Metro;

[ToolboxBitmap(typeof(MetroShell), "MetroStatusBar.ico")]
[ToolboxItem(false)]
public class MetroStatusBar : ItemControl
{
	private GenericItemContainer _ItemContainer;

	private readonly int ResizeHandleWidth = 10;

	private bool _ResizeHandleVisible = true;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Browsable(false)]
	public SubItemsCollection Items => _ItemContainer.SubItems;

	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates whether resize handle used to resize the parent form is visible.")]
	public bool ResizeHandleVisible
	{
		get
		{
			return _ResizeHandleVisible;
		}
		set
		{
			if (value != _ResizeHandleVisible)
			{
				bool resizeHandleVisible = _ResizeHandleVisible;
				_ResizeHandleVisible = value;
				OnResizeHandleVisibleChanged(resizeHandleVisible, value);
			}
		}
	}

	[DefaultValue(2)]
	[Category("Appearance")]
	[Description("Gets or sets spacing between items.")]
	public int ItemSpacing
	{
		get
		{
			return _ItemContainer.ItemSpacing;
		}
		set
		{
			_ItemContainer.ItemSpacing = value;
			RecalcLayout();
		}
	}

	protected override Size DefaultSize => new Size(200, 22);

	public MetroStatusBar()
	{
		_ItemContainer = new GenericItemContainer();
		_ItemContainer.GlobalItem = false;
		_ItemContainer.ContainerControl = this;
		_ItemContainer.Stretch = true;
		_ItemContainer.Displayed = true;
		_ItemContainer.WrapItems = false;
		_ItemContainer.ItemSpacing = 2;
		_ItemContainer.PaddingTop = 1;
		_ItemContainer.PaddingBottom = 4;
		_ItemContainer.PaddingLeft = 4;
		_ItemContainer.PaddingRight = 1;
		_ItemContainer.ToolbarItemsAlign = eContainerVerticalAlignment.Middle;
		_ItemContainer.EventHeight = false;
		_ItemContainer.FillsContainerControl = true;
		_ItemContainer.Style = eDotNetBarStyle.StyleManagerControlled;
		base.ColorScheme.Style = eDotNetBarStyle.StyleManagerControlled;
		_ItemContainer.SetOwner(this);
		_ItemContainer.LayoutType = eLayoutType.Toolbar;
		SetBaseItemContainer(_ItemContainer);
		DragDropSupport = true;
		base.ItemAdded += ChildItemAdded;
		base.ItemRemoved += ChildItemRemoved;
	}

	protected override void Dispose(bool disposing)
	{
		base.ItemAdded -= ChildItemAdded;
		base.ItemRemoved -= ChildItemRemoved;
		base.Dispose(disposing);
	}

	private void ChildItemAdded(object sender, EventArgs e)
	{
		RecalcLayout();
	}

	private void ChildItemRemoved(object sender, ItemRemovedEventArgs e)
	{
		RecalcLayout();
	}

	protected override void PaintControlBackground(ItemPaintArgs pa)
	{
		MetroRender.Paint(this, pa);
		base.PaintControlBackground(pa);
	}

	protected override Rectangle GetItemContainerRectangle()
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Invalid comparison between Unknown and I4
		Rectangle itemContainerRectangle = base.GetItemContainerRectangle();
		if (_ResizeHandleVisible)
		{
			itemContainerRectangle.Width -= ResizeHandleWidth;
			if ((int)((Control)this).RightToLeft == 1)
			{
				itemContainerRectangle.X += ResizeHandleWidth;
			}
		}
		return itemContainerRectangle;
	}

	protected virtual void OnResizeHandleVisibleChanged(bool oldValue, bool newValue)
	{
		RecalcLayout();
	}

	protected override void WndProc(ref Message m)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Invalid comparison between Unknown and I4
		if (_ResizeHandleVisible && ((Message)(ref m)).Msg == 132)
		{
			int x = WinApi.LOWORD(((Message)(ref m)).LParam);
			int y = WinApi.HIWORD(((Message)(ref m)).LParam);
			Point pt = ((Control)this).PointToClient(new Point(x, y));
			if ((((int)((Control)this).RightToLeft == 1) ? new Rectangle(0, ((Control)this).Height - ResizeHandleWidth, ResizeHandleWidth, ResizeHandleWidth) : new Rectangle(((Control)this).Width - ResizeHandleWidth, ((Control)this).Height - ResizeHandleWidth, ResizeHandleWidth, ResizeHandleWidth)).Contains(pt))
			{
				((Message)(ref m)).Result = new IntPtr(-1);
				return;
			}
		}
		base.WndProc(ref m);
	}
}
