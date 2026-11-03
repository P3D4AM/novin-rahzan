using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DevComponents.DotNetBar.Metro.Rendering;

namespace DevComponents.DotNetBar.Metro;

[Designer("DevComponents.DotNetBar.Design.MetroStatusBarDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
[ToolboxBitmap(typeof(MetroShell), "MetroStatusBar.ico")]
[ToolboxItem(true)]
[ComVisible(false)]
public class MetroStatusBar : ItemControl
{
	private GenericItemContainer _ᐵ;

	private readonly int _2220 = 10;

	private bool __2221 = true;

	[Description("Indicates whether items that cannot fit are displayed on popup.")]
	[DefaultValue(true)]
	[Category("Behavior")]
	public bool OverflowEnabled
	{
		get
		{
			return _ᐵ._0DD8;
		}
		set
		{
			_ᐵ._0DD8 = value;
			if (((Control)this).IsHandleCreated)
			{
				RecalcLayout();
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public SubItemsCollection Items => _ᐵ.SubItems;

	[Category("Appearance")]
	[Description("Indicates whether resize handle used to resize the parent form is visible.")]
	[DefaultValue(true)]
	public bool ResizeHandleVisible
	{
		get
		{
			return __2221;
		}
		set
		{
			if (value != __2221)
			{
				bool oldValue = __2221;
				__2221 = value;
				OnResizeHandleVisibleChanged(oldValue, value);
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
			return _ᐵ.ItemSpacing;
		}
		set
		{
			_ᐵ.ItemSpacing = value;
			RecalcLayout();
		}
	}

	protected override Size DefaultSize => new Size(200, 22);

	public MetroStatusBar()
	{
		_ᐵ = new GenericItemContainer();
		_ᐵ.GlobalItem = false;
		_ᐵ.ContainerControl = this;
		_ᐵ.Stretch = true;
		_ᐵ.Displayed = true;
		_ᐵ.WrapItems = false;
		_ᐵ.ItemSpacing = 2;
		_ᐵ.PaddingTop = 1;
		_ᐵ.PaddingBottom = 4;
		_ᐵ.PaddingLeft = 4;
		_ᐵ.PaddingRight = 1;
		_ᐵ.ව = eContainerVerticalAlignment.Middle;
		_ᐵ._0DBE = false;
		_ᐵ.FillsContainerControl = true;
		_ᐵ.Style = eDotNetBarStyle.StyleManagerControlled;
		base.ColorScheme.ߕ = eDotNetBarStyle.StyleManagerControlled;
		_ᐵ.SetOwner(this);
		_ᐵ.LayoutType = eLayoutType.Toolbar;
		SetBaseItemContainer(_ᐵ);
		DragDropSupport = true;
		base.ItemAdded += _2222;
		base.ItemRemoved += _2223;
	}

	protected override void Dispose(bool disposing)
	{
		base.ItemAdded -= _2222;
		base.ItemRemoved -= _2223;
		base.Dispose(disposing);
	}

	private void _2222(object _٠, EventArgs ؿ)
	{
		RecalcLayout();
	}

	private void _2223(object _٠, ItemRemovedEventArgs ؿ)
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
		if (__2221)
		{
			itemContainerRectangle.Width -= _2220;
			if ((int)((Control)this).RightToLeft == 1)
			{
				itemContainerRectangle.X += _2220;
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
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Invalid comparison between Unknown and I4
		if (__2221 && ((Message)(ref m)).Msg == 132)
		{
			int x = _0EFE._0F0B(((Message)(ref m)).LParam);
			int y = _0EFE._0F0D(((Message)(ref m)).LParam);
			Point pt = ((Control)this).PointToClient(new Point(x, y));
			int num = Dpi.Width(_2220);
			if ((((int)((Control)this).RightToLeft == 1) ? new Rectangle(0, ((Control)this).Height - num, num, num) : new Rectangle(((Control)this).Width - num, ((Control)this).Height - num, num, num)).Contains(pt))
			{
				((Message)(ref m)).Result = new IntPtr(-1);
				return;
			}
		}
		base.WndProc(ref m);
	}
}
