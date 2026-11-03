using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;
using DevComponents.DotNetBar.TextMarkup;

namespace DevComponents.DotNetBar.Controls;

[ComVisible(false)]
[ToolboxItem(false)]
public class TabFormStripControl : ItemControl
{
	private Ჷ _ᐚ;

	private int _ற;

	private bool _ᐛ;

	private Rectangle _ᐜ = Rectangle.Empty;

	private Rectangle _ᐞ = Rectangle.Empty;

	private Font _ᐟ;

	private bool _ᐠ = true;

	private ElementStyle _ᐡ = new ElementStyle();

	private string _ᅶ = "";

	private MarkupLinkClickEventHandler _0D83;

	private _0CB4 _ᐣ;

	internal Rectangle අ = Rectangle.Empty;

	private int Გ = 4;

	private bool _ᐦ = true;

	private bool _ᐧ;

	private Point _ᐨ = Point.Empty;

	private Cursor _Დ;

	private eTabFormStripControlDock _Ე;

	private bool _Ვ = true;

	private bool _Ზ = true;

	private TabFormTabCloseEventHandler Თ;

	private TabFormTabCloseEventHandler Ი;

	private Form _Კ;

	private TabFormItemDetachEventHandler Ლ;

	private TabFormItemDetachEventHandler Მ;

	[Editor("DevComponents.DotNetBar.Design.TextMarkupUIEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Description("Indicates text displayed on Ribbon Title instead of the Form.Text property.")]
	[Category("Appearance")]
	[DefaultValue("")]
	[Browsable(true)]
	[EditorBrowsable(EditorBrowsableState.Always)]
	public string TitleText
	{
		get
		{
			return _ᅶ;
		}
		set
		{
			if (value == null)
			{
				value = "";
			}
			_ᅶ = value;
			_ᐣ = null;
			if (ሒ.መ(ref _ᅶ))
			{
				_ᐣ = ሒ.ሕ(_ᅶ);
				if (_ᐣ != null)
				{
					_ᐣ.ವ += ඌ;
				}
				අ = Rectangle.Empty;
				((Control)this).Invalidate();
			}
		}
	}

	internal _0CB4 _0D62 => _ᐣ;

	[Description("Indicates whether control can be customized. Caption must be visible for customization to be fully enabled.")]
	[Category("Customization")]
	[Browsable(true)]
	[DefaultValue(true)]
	public bool CanCustomize
	{
		get
		{
			return _ᐠ;
		}
		set
		{
			_ᐠ = value;
		}
	}

	[DefaultValue(0)]
	[Category("Appearance")]
	[Description("Indicates explicit height of the caption provided by control")]
	[Browsable(true)]
	public int CaptionHeight
	{
		get
		{
			return _ற;
		}
		set
		{
			_ற = value;
			_ᐚ.NeedRecalcSize = true;
		}
	}

	internal bool _0D63
	{
		get
		{
			foreach (BaseItem item in Items)
			{
				if (item.Visible)
				{
					return true;
				}
			}
			return false;
		}
	}

	internal Rectangle _0D65
	{
		get
		{
			return _ᐜ;
		}
		set
		{
			_ᐜ = value;
		}
	}

	internal Rectangle _൦
	{
		get
		{
			return _ᐞ;
		}
		set
		{
			_ᐞ = value;
		}
	}

	internal SystemCaptionItem _൧ => _ᐚ._൧;

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Appearance")]
	[Description("Indicates whether custom caption line provided by the control is visible.")]
	public bool CaptionVisible
	{
		get
		{
			return _ᐛ;
		}
		set
		{
			_ᐛ = value;
			ඍ();
		}
	}

	[Category("Appearance")]
	[Browsable(true)]
	[Description("")]
	[DefaultValue(null)]
	public Font CaptionFont
	{
		get
		{
			return _ᐟ;
		}
		set
		{
			_ᐟ = value;
			_ᐚ.NeedRecalcSize = true;
			((Control)this).Invalidate();
		}
	}

	internal eDotNetBarStyle _൨ => _ᐚ.EffectiveStyle;

	[DefaultValue(eDotNetBarStyle.Metro)]
	[DevCoBrowsable(true)]
	[Description("Specifies the visual style of the control.")]
	[Category("Appearance")]
	[Browsable(true)]
	public eDotNetBarStyle Style
	{
		get
		{
			return _ᐚ.Style;
		}
		set
		{
			base.ColorScheme.ߕ = value;
			_ᐚ.Style = value;
			((Control)this).Invalidate();
			RecalcLayout();
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Browsable(false)]
	public SubItemsCollection Items => _ᐚ._1CBC.SubItems;

	[Browsable(false)]
	public TabFormItem SelectedTab
	{
		get
		{
			foreach (BaseItem item in Items)
			{
				if (item is TabFormItem && ((TabFormItem)item).Checked)
				{
					return (TabFormItem)item;
				}
			}
			return null;
		}
	}

	internal bool _൩
	{
		get
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Invalid comparison between Unknown and I4
			Form val = ((Control)this).FindForm();
			if (val != null)
			{
				return (int)val.WindowState == 2;
			}
			return false;
		}
	}

	internal override bool ষ => false;

	[DefaultValue(true)]
	[Description("Indicates whether Form.Icon is shown in top-left corner.")]
	[Category("Appearance")]
	public bool ShowIcon
	{
		get
		{
			return _ᐦ;
		}
		set
		{
			if (_ᐦ != value)
			{
				_ᐦ = value;
				_ᐚ.NeedRecalcSize = true;
				RecalcLayout();
			}
		}
	}

	internal bool _൮ => _ᐧ;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public GenericItemContainer CaptionContainerItem => _ᐚ.ᐮ;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public SimpleItemContainer StripContainerItem => _ᐚ._1CBC;

	internal Ჷ Ბ => _ᐚ;

	[Category("Appearance")]
	[Description("Indiciates the appearance of the tab form items rendered on the strip")]
	[DefaultValue(eTabFormStripControlDock.Top)]
	public eTabFormStripControlDock TabAlignment
	{
		get
		{
			return _Ე;
		}
		set
		{
			_Ე = value;
		}
	}

	[Category("Behavior")]
	[Description("Indicates whether end-user tab reordering is enabled.")]
	[DefaultValue(true)]
	public bool IsTabDragEnabled
	{
		get
		{
			return _Ვ;
		}
		set
		{
			_Ვ = value;
		}
	}

	[Category("Behavior")]
	[Description("Indicates whether user can detach the tabs into the new forms using drag and drop. Default value is true.")]
	[DefaultValue(true)]
	public bool TabDetachEnabled
	{
		get
		{
			return _Ზ;
		}
		set
		{
			_Ზ = value;
		}
	}

	[Browsable(false)]
	public int TabsCount
	{
		get
		{
			int num = 0;
			foreach (BaseItem item in Items)
			{
				if (item is TabFormItem)
				{
					num++;
				}
			}
			return num;
		}
	}

	[DefaultValue(false)]
	[Category("Appearance")]
	[Description("Indicates whether new tab item which allows creation of new tab when clicked is visible. When visible you need to handle CreateNewTab event and create your new tab in event handler.")]
	public bool NewTabItemVisible
	{
		get
		{
			return _ᐚ.Ჽ;
		}
		set
		{
			_ᐚ.Ჽ = value;
		}
	}

	[Description("Occurs when text markup link from TitleText markup is clicked.")]
	public event MarkupLinkClickEventHandler TitleTextMarkupLinkClick
	{
		add
		{
			MarkupLinkClickEventHandler markupLinkClickEventHandler = _0D83;
			MarkupLinkClickEventHandler markupLinkClickEventHandler2;
			do
			{
				markupLinkClickEventHandler2 = markupLinkClickEventHandler;
				MarkupLinkClickEventHandler value2 = (MarkupLinkClickEventHandler)Delegate.Combine(markupLinkClickEventHandler2, value);
				markupLinkClickEventHandler = Interlocked.CompareExchange(ref _0D83, value2, markupLinkClickEventHandler2);
			}
			while ((object)markupLinkClickEventHandler != markupLinkClickEventHandler2);
		}
		remove
		{
			MarkupLinkClickEventHandler markupLinkClickEventHandler = _0D83;
			MarkupLinkClickEventHandler markupLinkClickEventHandler2;
			do
			{
				markupLinkClickEventHandler2 = markupLinkClickEventHandler;
				MarkupLinkClickEventHandler value2 = (MarkupLinkClickEventHandler)Delegate.Remove(markupLinkClickEventHandler2, value);
				markupLinkClickEventHandler = Interlocked.CompareExchange(ref _0D83, value2, markupLinkClickEventHandler2);
			}
			while ((object)markupLinkClickEventHandler != markupLinkClickEventHandler2);
		}
	}

	[Description("Occurs before tab is closed and it allows canceling of the event.")]
	public event TabFormTabCloseEventHandler TabClosing
	{
		add
		{
			TabFormTabCloseEventHandler tabFormTabCloseEventHandler = Თ;
			TabFormTabCloseEventHandler tabFormTabCloseEventHandler2;
			do
			{
				tabFormTabCloseEventHandler2 = tabFormTabCloseEventHandler;
				TabFormTabCloseEventHandler value2 = (TabFormTabCloseEventHandler)Delegate.Combine(tabFormTabCloseEventHandler2, value);
				tabFormTabCloseEventHandler = Interlocked.CompareExchange(ref Თ, value2, tabFormTabCloseEventHandler2);
			}
			while ((object)tabFormTabCloseEventHandler != tabFormTabCloseEventHandler2);
		}
		remove
		{
			TabFormTabCloseEventHandler tabFormTabCloseEventHandler = Თ;
			TabFormTabCloseEventHandler tabFormTabCloseEventHandler2;
			do
			{
				tabFormTabCloseEventHandler2 = tabFormTabCloseEventHandler;
				TabFormTabCloseEventHandler value2 = (TabFormTabCloseEventHandler)Delegate.Remove(tabFormTabCloseEventHandler2, value);
				tabFormTabCloseEventHandler = Interlocked.CompareExchange(ref Თ, value2, tabFormTabCloseEventHandler2);
			}
			while ((object)tabFormTabCloseEventHandler != tabFormTabCloseEventHandler2);
		}
	}

	[Description("Occurs after tab is closed.")]
	public event TabFormTabCloseEventHandler TabClosed
	{
		add
		{
			TabFormTabCloseEventHandler tabFormTabCloseEventHandler = Ი;
			TabFormTabCloseEventHandler tabFormTabCloseEventHandler2;
			do
			{
				tabFormTabCloseEventHandler2 = tabFormTabCloseEventHandler;
				TabFormTabCloseEventHandler value2 = (TabFormTabCloseEventHandler)Delegate.Combine(tabFormTabCloseEventHandler2, value);
				tabFormTabCloseEventHandler = Interlocked.CompareExchange(ref Ი, value2, tabFormTabCloseEventHandler2);
			}
			while ((object)tabFormTabCloseEventHandler != tabFormTabCloseEventHandler2);
		}
		remove
		{
			TabFormTabCloseEventHandler tabFormTabCloseEventHandler = Ი;
			TabFormTabCloseEventHandler tabFormTabCloseEventHandler2;
			do
			{
				tabFormTabCloseEventHandler2 = tabFormTabCloseEventHandler;
				TabFormTabCloseEventHandler value2 = (TabFormTabCloseEventHandler)Delegate.Remove(tabFormTabCloseEventHandler2, value);
				tabFormTabCloseEventHandler = Interlocked.CompareExchange(ref Ი, value2, tabFormTabCloseEventHandler2);
			}
			while ((object)tabFormTabCloseEventHandler != tabFormTabCloseEventHandler2);
		}
	}

	[Description("Occurs before TabFormItem is detached and gives you opportunity to cancel the action or provide your own new TabParentForm and TabFormControl.")]
	public event TabFormItemDetachEventHandler BeforeTabFormItemDetach
	{
		add
		{
			TabFormItemDetachEventHandler tabFormItemDetachEventHandler = Ლ;
			TabFormItemDetachEventHandler tabFormItemDetachEventHandler2;
			do
			{
				tabFormItemDetachEventHandler2 = tabFormItemDetachEventHandler;
				TabFormItemDetachEventHandler value2 = (TabFormItemDetachEventHandler)Delegate.Combine(tabFormItemDetachEventHandler2, value);
				tabFormItemDetachEventHandler = Interlocked.CompareExchange(ref Ლ, value2, tabFormItemDetachEventHandler2);
			}
			while ((object)tabFormItemDetachEventHandler != tabFormItemDetachEventHandler2);
		}
		remove
		{
			TabFormItemDetachEventHandler tabFormItemDetachEventHandler = Ლ;
			TabFormItemDetachEventHandler tabFormItemDetachEventHandler2;
			do
			{
				tabFormItemDetachEventHandler2 = tabFormItemDetachEventHandler;
				TabFormItemDetachEventHandler value2 = (TabFormItemDetachEventHandler)Delegate.Remove(tabFormItemDetachEventHandler2, value);
				tabFormItemDetachEventHandler = Interlocked.CompareExchange(ref Ლ, value2, tabFormItemDetachEventHandler2);
			}
			while ((object)tabFormItemDetachEventHandler != tabFormItemDetachEventHandler2);
		}
	}

	[Description("Occurs after TabFormItem has been detached and is added to the new form and tab control.")]
	public event TabFormItemDetachEventHandler TabFormItemDetach
	{
		add
		{
			TabFormItemDetachEventHandler tabFormItemDetachEventHandler = Მ;
			TabFormItemDetachEventHandler tabFormItemDetachEventHandler2;
			do
			{
				tabFormItemDetachEventHandler2 = tabFormItemDetachEventHandler;
				TabFormItemDetachEventHandler value2 = (TabFormItemDetachEventHandler)Delegate.Combine(tabFormItemDetachEventHandler2, value);
				tabFormItemDetachEventHandler = Interlocked.CompareExchange(ref Მ, value2, tabFormItemDetachEventHandler2);
			}
			while ((object)tabFormItemDetachEventHandler != tabFormItemDetachEventHandler2);
		}
		remove
		{
			TabFormItemDetachEventHandler tabFormItemDetachEventHandler = Მ;
			TabFormItemDetachEventHandler tabFormItemDetachEventHandler2;
			do
			{
				tabFormItemDetachEventHandler2 = tabFormItemDetachEventHandler;
				TabFormItemDetachEventHandler value2 = (TabFormItemDetachEventHandler)Delegate.Remove(tabFormItemDetachEventHandler2, value);
				tabFormItemDetachEventHandler = Interlocked.CompareExchange(ref Მ, value2, tabFormItemDetachEventHandler2);
			}
			while ((object)tabFormItemDetachEventHandler != tabFormItemDetachEventHandler2);
		}
	}

	public TabFormStripControl()
	{
		((Control)this).SetStyle((ControlStyles)4096, true);
		_ᐚ = new Ჷ(this);
		_ᐚ.GlobalItem = false;
		_ᐚ.ContainerControl = this;
		_ᐚ.Displayed = true;
		_ᐚ.SetOwner(this);
		_ᐚ.Style = eDotNetBarStyle.StyleManagerControlled;
		_ᐚ.Ს += Ნ;
		SetBaseItemContainer(_ᐚ);
		base.ColorScheme.ߕ = eDotNetBarStyle.Office2003;
		((Control)this).AutoSize = true;
		DragDropSupport = true;
		((Control)this).AllowDrop = true;
		AllowExternalDrop = true;
		_ᐚ._൧.Click += ණ;
		StyleManager.Register((Control)(object)this);
	}

	private void Ნ(object _٠, EventArgs ؿ)
	{
		if (((Control)this).Parent is TabFormControl)
		{
			((TabFormControl)(object)((Control)this).Parent).Ღ(ؿ);
		}
	}

	protected override void Dispose(bool disposing)
	{
		StyleManager.Unregister((Control)(object)this);
		base.Dispose(disposing);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void StyleManagerStyleChanged(eDotNetBarStyle newStyle)
	{
		Style = StyleManager.GetEffectiveStyle();
	}

	private void ඌ(object _٠, EventArgs ؿ)
	{
		if (_٠ is _0CFE obj && _0D83 != null)
		{
			_0D83(this, new MarkupLinkClickEventArgs(obj._0D01, obj._0D00));
		}
	}

	private void ඍ()
	{
		_ᐚ.NeedRecalcSize = true;
		_ᐚ.ඍ(_ᐛ);
		RecalcLayout();
	}

	protected override void WndProc(ref Message m)
	{
		if (((Message)(ref m)).Msg == 132)
		{
			int width = Dpi.Width6;
			int x = _0EFE._0F0B(((Message)(ref m)).LParam);
			int y = _0EFE._0F0D(((Message)(ref m)).LParam);
			Point pt = ((Control)this).PointToClient(new Point(x, y));
			if (ষ && CaptionVisible)
			{
				Rectangle rectangle = new Rectangle(((Control)this).Width - BarFunctions.CaptionButtonSize.Width * 3, 0, BarFunctions.CaptionButtonSize.Width * 3, BarFunctions.CaptionButtonSize.Height + 6);
				if (rectangle.Contains(pt))
				{
					((Message)(ref m)).Result = new IntPtr(-1);
					return;
				}
				if (new Rectangle(0, 0, ((Control)this).Width, 4).Contains(pt))
				{
					((Message)(ref m)).Result = new IntPtr(-1);
					return;
				}
				if (_ᐜ.Contains(pt))
				{
					((Message)(ref m)).Result = new IntPtr(-1);
					return;
				}
			}
			else if (CaptionVisible && !_൩)
			{
				if (new Rectangle(0, 0, ((Control)this).Width, 4).Contains(pt))
				{
					((Message)(ref m)).Result = new IntPtr(-1);
					return;
				}
			}
			else if (!_൧.Bounds.Contains(pt))
			{
				BaseItem baseItem = HitTest(pt.X, pt.Y);
				if (baseItem == null)
				{
					((Message)(ref m)).Result = new IntPtr(-1);
					return;
				}
			}
			else if (pt.X <= width || pt.X >= ((Control)this).Width - width || pt.Y <= width || pt.Y >= ((Control)this).Height - width)
			{
				((Message)(ref m)).Result = new IntPtr(-1);
				return;
			}
			if (BarFunctions.IsWindows7 && _൩ && _0D65.Contains(pt))
			{
				((Message)(ref m)).Result = new IntPtr(-1);
				return;
			}
		}
		base.WndProc(ref m);
	}

	protected override ColorScheme GetColorScheme()
	{
		BaseRenderer renderer = GetRenderer();
		if (renderer is Office2007Renderer)
		{
			return ((Office2007Renderer)renderer).ColorTable.LegacyColors;
		}
		return base.GetColorScheme();
	}

	protected override void PaintControlBackground(ItemPaintArgs pa)
	{
		base.PaintControlBackground(pa);
		_ᐜ = Rectangle.Empty;
		_ᐞ = Rectangle.Empty;
		BaseRenderer renderer = GetRenderer();
		renderer.DrawTabFormStrip(new TabFormStripPainterArgs(this, pa.Graphics, pa));
		if (_089F.फ())
		{
			pa.Graphics.Clear(Color.White);
			TextDrawing.DrawString(pa.Graphics, "Your DotNetBar trial has expired :-(", ((Control)this).Font, Color.FromArgb(128, Color.Black), ((Control)this).ClientRectangle, eTextFormat.HorizontalCenter | eTextFormat.VerticalCenter);
		}
	}

	protected override ElementStyle GetBackgroundStyle()
	{
		if (base.BackgroundStyle.Custom)
		{
			return base.GetBackgroundStyle();
		}
		return _ᐡ;
	}

	internal void ඕ()
	{
		_ᐡ = new ElementStyle();
	}

	internal ElementStyle ඖ()
	{
		return GetBackgroundStyle();
	}

	internal Rectangle _0D97()
	{
		Rectangle itemContainerRectangle = base.GetItemContainerRectangle();
		if (_ᐛ)
		{
			itemContainerRectangle.Y += ක();
		}
		else
		{
			itemContainerRectangle.Y += Გ;
		}
		return itemContainerRectangle;
	}

	internal Rectangle _0D98()
	{
		Rectangle itemContainerRectangle = base.GetItemContainerRectangle();
		if (ষ)
		{
			itemContainerRectangle.Y += 3;
		}
		return new Rectangle(itemContainerRectangle.X, itemContainerRectangle.Y, itemContainerRectangle.Width, ක());
	}

	public int GetCaptionHeight()
	{
		if (_ற == 0)
		{
			return Dpi.Height24;
		}
		return Dpi.Height(_ற);
	}

	internal int ක()
	{
		return GetCaptionHeight();
	}

	internal int ඛ()
	{
		return GetCaptionHeight();
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		if (_ᐚ.NeedRecalcSize)
		{
			RecalcSize();
		}
		ඕ();
		base.OnPaint(e);
	}

	protected override void RecalcSize()
	{
		_ᐜ = Rectangle.Empty;
		_ᐞ = Rectangle.Empty;
		ඕ();
		base.RecalcSize();
	}

	public override int GetAutoSizeHeight()
	{
		int num = base.GetAutoSizeHeight();
		if (!_ᐛ)
		{
			num += Გ;
		}
		return num;
	}

	protected override bool _੨(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		Rectangle displayRectangle = ((Control)this).DisplayRectangle;
		displayRectangle.Location = ((Control)this).PointToScreen(displayRectangle.Location);
		if (((Control)this).Parent is TabFormControl { MouseWheelTabScrollEnabled: false })
		{
			return false;
		}
		Point mousePosition = Control.MousePosition;
		bool flag = true;
		Form val = ((Control)this).FindForm();
		if (val != null && !BarFunctions.IsFormActive(val))
		{
			flag = false;
		}
		if (flag && displayRectangle.Contains(mousePosition) && !ShowKeyTips && Items.Count > 0)
		{
			IntPtr intPtr = _089F.WindowFromPoint(new _089F.POINT(mousePosition));
			Control val2 = Control.FromChildHandle(intPtr);
			if (val2 == null)
			{
				val2 = Control.FromHandle(intPtr);
			}
			if (val2 is TabFormStripControl || val2 is TabFormControl || val2 is TabFormPanel)
			{
				TabFormItem selectedTab = SelectedTab;
				int num = 0;
				int num2 = Items.Count - 1;
				int num3 = 1;
				if ((_੦.ToInt64() & 0x80000000u) > 0)
				{
					num3 = -1;
					num2 = 0;
				}
				if (selectedTab != null)
				{
					num = Items.IndexOf(selectedTab) + num3;
					if (num3 < 0 && num < 0)
					{
						return false;
					}
					if (num == Items.Count)
					{
						num = 0;
					}
					else if (num < 0)
					{
						num = Items.Count - 1;
					}
				}
				int num4 = num - num3;
				do
				{
					num4 += num3;
					if (num4 < 0 || num4 > Items.Count - 1)
					{
						break;
					}
					if (Items[num4] is TabFormItem && Items[num4].Visible)
					{
						((TabFormItem)Items[num4]).Checked = true;
						return true;
					}
				}
				while (num4 != num2);
			}
		}
		return false;
	}

	public override ArrayList GetItems(string ItemName)
	{
		ArrayList arrayList = new ArrayList(15);
		BarFunctions.GetSubItemsByName(GetBaseItemContainer(), ItemName, arrayList);
		TabFormControl tabFormControl = ᎅ();
		if (tabFormControl != null && tabFormControl.GlobalContextMenuBar != null)
		{
			BarFunctions.GetSubItemsByName(tabFormControl.GlobalContextMenuBar.ItemsContainer, ItemName, arrayList);
		}
		return arrayList;
	}

	public override ArrayList GetItems(string ItemName, Type itemType)
	{
		ArrayList arrayList = new ArrayList(15);
		BarFunctions.GetSubItemsByNameAndType(GetBaseItemContainer(), ItemName, arrayList, itemType);
		TabFormControl tabFormControl = ᎅ();
		if (tabFormControl != null && tabFormControl.GlobalContextMenuBar != null)
		{
			BarFunctions.GetSubItemsByNameAndType(tabFormControl.GlobalContextMenuBar.ItemsContainer, ItemName, arrayList, itemType);
		}
		return arrayList;
	}

	public override ArrayList GetItems(string ItemName, Type itemType, bool useGlobalName)
	{
		ArrayList arrayList = new ArrayList(15);
		BarFunctions.GetSubItemsByNameAndType(GetBaseItemContainer(), ItemName, arrayList, itemType, useGlobalName);
		TabFormControl tabFormControl = ᎅ();
		if (tabFormControl != null && tabFormControl.GlobalContextMenuBar != null)
		{
			BarFunctions.GetSubItemsByNameAndType(tabFormControl.GlobalContextMenuBar.ItemsContainer, ItemName, arrayList, itemType, useGlobalName);
		}
		return arrayList;
	}

	public override BaseItem GetItem(string ItemName)
	{
		BaseItem subItemByName = BarFunctions.GetSubItemByName(GetBaseItemContainer(), ItemName);
		if (subItemByName != null)
		{
			return subItemByName;
		}
		TabFormControl tabFormControl = ᎅ();
		if (tabFormControl != null && tabFormControl.GlobalContextMenuBar != null)
		{
			return BarFunctions.GetSubItemByName(tabFormControl.GlobalContextMenuBar.ItemsContainer, ItemName);
		}
		return null;
	}

	protected override void OnPopupItemRightClick(BaseItem item)
	{
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		if (_ᐣ != null)
		{
			_ᐣ.ભ((Control)(object)this);
		}
		base.OnMouseLeave(e);
	}

	private TabFormControl ᎅ()
	{
		return ((Control)this).Parent as TabFormControl;
	}

	internal void ᐪ(Point _065C)
	{
		Form val = ((Control)this).FindForm();
		if (val is TabParentForm)
		{
			((TabParentForm)(object)val).Ꮨ(_065C);
			return;
		}
		byte[] bytes = BitConverter.GetBytes(_065C.X);
		byte[] bytes2 = BitConverter.GetBytes(_065C.Y);
		byte[] value = new byte[4]
		{
			bytes[0],
			bytes[1],
			bytes2[0],
			bytes2[1]
		};
		int lParam = BitConverter.ToInt32(value, 0);
		((Control)this).Capture = false;
		_089F.SendMessage(((Control)val).Handle, 274, _089F.TrackPopupMenu(_089F.GetSystemMenu(((Control)val).Handle, bRevert: false), 256u, _065C.X, _065C.Y, 0, ((Control)val).Handle, IntPtr.Zero), lParam);
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Invalid comparison between Unknown and I4
		_ᐧ = false;
		_ᐨ = e.Location;
		if (_ᐛ)
		{
			_ᐧ = HitTestCaption(new Point(e.X, e.Y));
			if ((int)e.Button == 2097152 && _ᐧ)
			{
				ᐪ(Control.MousePosition);
				return;
			}
			if (_ᐣ != null)
			{
				_ᐣ.ग़((Control)(object)this, e);
			}
			e = ඪ(e);
		}
		base.OnMouseDown(e);
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		if (_ᐛ)
		{
			if (_ᐣ != null)
			{
				_ᐣ.ફ((Control)(object)this, e);
			}
			e = ඪ(e);
		}
		_ᐧ = false;
		base.OnMouseUp(e);
	}

	public void StartFormMove()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		Form val = ((Control)this).FindForm();
		if (val != null && (int)val.WindowState == 0)
		{
			Point mousePosition = Control.MousePosition;
			byte[] bytes = BitConverter.GetBytes(mousePosition.X);
			byte[] bytes2 = BitConverter.GetBytes(mousePosition.Y);
			byte[] value = new byte[4]
			{
				bytes[0],
				bytes[1],
				bytes2[0],
				bytes2[1]
			};
			int lParam = BitConverter.ToInt32(value, 0);
			((Control)this).Capture = false;
			_089F.SendMessage(((Control)val).Handle, 274, 61458, lParam);
			_ᐧ = false;
		}
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Invalid comparison between Unknown and I4
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		if (_ᐛ)
		{
			if ((int)e.Button == 1048576 && ((_ᐧ && HitTestCaption(new Point(e.X, e.Y))) || HitTest(_ᐨ.X, _ᐨ.Y) == null))
			{
				Form val = ((Control)this).FindForm();
				if (val != null && (int)val.WindowState == 0)
				{
					StartFormMove();
					return;
				}
			}
			if (_ᐣ != null)
			{
				_ᐣ.મ((Control)(object)this, e);
			}
			e = ඪ(e);
		}
		base.OnMouseMove(e);
	}

	protected override void InternalOnClick(MouseButtons mb, Point mousePos)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		if (_ᐛ)
		{
			MouseEventArgs ؿ = new MouseEventArgs(mb, 0, mousePos.X, mousePos.Y, 0);
			ؿ = ඪ(ؿ);
			mousePos = new Point(ؿ.X, ؿ.Y);
		}
		base.InternalOnClick(mb, mousePos);
	}

	private MouseEventArgs ඪ(MouseEventArgs ؿ)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Invalid comparison between Unknown and I4
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Expected O, but got Unknown
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		if (ؿ.Y <= 6)
		{
			Form val = ((Control)this).FindForm();
			if (val != null && (int)val.WindowState == 2 && val is RibbonForm && ؿ.X > 4)
			{
				ؿ = ((ؿ.X < ((Control)this).Width - 6) ? new MouseEventArgs(ؿ.Button, ؿ.Clicks, ؿ.X, CaptionContainerItem.TopInternal + 5, ؿ.Delta) : new MouseEventArgs(ؿ.Button, ؿ.Clicks, _൧.DisplayRectangle.Right - 4, _൧.DisplayRectangle.Top + 4, ؿ.Delta));
			}
		}
		return ؿ;
	}

	protected override void OnClick(EventArgs e)
	{
		if (_ᐛ && _ᐣ != null)
		{
			_ᐣ.દ((Control)(object)this);
		}
		base.OnClick(e);
	}

	protected override void OnDoubleClick(EventArgs e)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Invalid comparison between Unknown and I4
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Invalid comparison between Unknown and I4
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Invalid comparison between Unknown and I4
		if (_ᐛ)
		{
			Point p = ((Control)this).PointToClient(Control.MousePosition);
			if (HitTestCaption(p))
			{
				Form val = ((Control)this).FindForm();
				if (val != null && val.MaximizeBox && ((int)val.FormBorderStyle == 4 || (int)val.FormBorderStyle == 6))
				{
					if ((int)val.WindowState == 0)
					{
						_089F.PostMessage(((Control)val).Handle, 274, 61488, 0);
					}
					else if ((int)val.WindowState == 2)
					{
						_089F.PostMessage(((Control)val).Handle, 274, 61728, 0);
					}
				}
			}
		}
		base.OnDoubleClick(e);
	}

	protected bool HitTestCaption(Point p)
	{
		return _ᐜ.Contains(p);
	}

	private void ණ(object _٠, EventArgs ؿ)
	{
		SystemCaptionItem systemCaptionItem = _٠ as SystemCaptionItem;
		Form val = ((Control)this).FindForm();
		if (val != null && systemCaptionItem._1C7E == systemCaptionItem.ᲀ)
		{
			if (systemCaptionItem._1C7E == _1886._1C8C)
			{
				_089F.PostMessage(((Control)val).Handle, 274, 61472, 0);
			}
			else if (systemCaptionItem._1C7E == _1886._1C8F)
			{
				_089F.PostMessage(((Control)val).Handle, 274, 61488, 0);
			}
			else if (systemCaptionItem._1C7E == _1886._1C8D)
			{
				_089F.PostMessage(((Control)val).Handle, 274, 61728, 0);
			}
			else if (systemCaptionItem._1C7E == _1886.ᆤ)
			{
				_089F.PostMessage(((Control)val).Handle, 274, 61536, 0);
			}
			else if (systemCaptionItem._1C7E == _1886._1C8E)
			{
				_089F.PostMessage(((Control)val).Handle, 274, 61824, 0);
			}
		}
	}

	internal void ᐫ()
	{
		Form val = ((Control)this).FindForm();
		if (val != null)
		{
			_089F.PostMessage(((Control)val).Handle, 274, 61536, 0);
		}
	}

	protected override void OnResize(EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Invalid comparison between Unknown and I4
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Invalid comparison between Unknown and I4
		base.OnResize(e);
		Form val = ((Control)this).FindForm();
		if (val != null)
		{
			if ((int)val.WindowState == 2 || (int)val.WindowState == 1)
			{
				_ᐚ._൧.RestoreEnabled = true;
			}
			else
			{
				_ᐚ._൧.RestoreEnabled = false;
			}
		}
	}

	protected override void OnGiveFeedback(GiveFeedbackEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		if ((int)e.Effect == 0)
		{
			_ = _Დ == (Cursor)null;
			if (_Დ != (Cursor)null)
			{
				e.UseDefaultCursors = false;
				Cursor.Current = _Დ;
			}
		}
		((Control)this).OnGiveFeedback(e);
	}

	protected override void OnItemDragAndDropped(BaseItem dragItem)
	{
		base.OnItemDragAndDropped(dragItem);
		if (dragItem is TabFormItem { Enabled: not false, Visible: not false } tabFormItem)
		{
			if (tabFormItem.Checked)
			{
				tabFormItem.Checked = false;
			}
			tabFormItem.Checked = true;
		}
	}

	internal void ర(bool ఱ)
	{
		if (_ᐚ._1CBC == null)
		{
			return;
		}
		bool flag = false;
		try
		{
			if (Items.Contains("dotnetbarsysiconitem"))
			{
				Items.Remove("dotnetbarsysiconitem");
				flag = true;
			}
			if (Items.Contains("dotnetbarsysmenuitem"))
			{
				Items.Remove("dotnetbarsysmenuitem");
				flag = true;
			}
			if (ఱ && flag)
			{
				RecalcLayout();
			}
		}
		catch (Exception)
		{
		}
	}

	internal void ల(Form ళ, bool ఱ)
	{
		ర(ఱ);
		if (ళ != null)
		{
			MDISystemItem mDISystemItem = new MDISystemItem("dotnetbarsysmenuitem");
			if (!ళ.ControlBox)
			{
				mDISystemItem.CloseEnabled = false;
			}
			if (!ళ.MinimizeBox)
			{
				mDISystemItem.MinimizeEnabled = false;
			}
			if (!ళ.MaximizeBox)
			{
				mDISystemItem.RestoreEnabled = false;
			}
			mDISystemItem.ItemAlignment = eItemAlignment.Far;
			mDISystemItem.Click += ఴ;
			Items.Add(mDISystemItem);
			if (ఱ)
			{
				RecalcLayout();
			}
		}
	}

	private void ఴ(object _٠, EventArgs ؿ)
	{
		MDISystemItem mDISystemItem = _٠ as MDISystemItem;
		Form val = ((Control)this).FindForm();
		if (val != null)
		{
			val = val.ActiveMdiChild;
		}
		if (val == null)
		{
			ర(ఱ: true);
		}
		else if (mDISystemItem._1C7E == _1886._1C8C)
		{
			_089F.PostMessage(((Control)val).Handle, 274, 61472, 0);
		}
		else if (mDISystemItem._1C7E == _1886._1C8D)
		{
			_089F.PostMessage(((Control)val).Handle, 274, 61728, 0);
		}
		else if (mDISystemItem._1C7E == _1886.ᆤ)
		{
			_089F.PostMessage(((Control)val).Handle, 274, 61536, 0);
		}
		else if (mDISystemItem._1C7E == _1886.Ა)
		{
			_089F.PostMessage(((Control)val).Handle, 274, 61504, 0);
		}
	}

	internal void ඬ(MouseEventArgs ؿ)
	{
		((Control)this).OnMouseDown(ؿ);
	}

	internal void ත(MouseEventArgs ؿ)
	{
		((Control)this).OnMouseMove(ؿ);
	}

	internal void ථ(MouseEventArgs ؿ)
	{
		((Control)this).OnMouseUp(ؿ);
	}

	internal void ද(EventArgs ؿ)
	{
		((Control)this).OnClick(ؿ);
	}

	public void CloseTab(TabFormItem tab, eEventSource source)
	{
		TabFormTabCloseEventArgs tabFormTabCloseEventArgs = new TabFormTabCloseEventArgs(tab, source);
		OnTabClosing(tabFormTabCloseEventArgs);
		if (tabFormTabCloseEventArgs.Cancel)
		{
			return;
		}
		Items.Remove(tab);
		Control panel = (Control)(object)tab.Panel;
		if (panel != null)
		{
			if (panel.Parent != null)
			{
				panel.Parent.Controls.Remove(panel);
			}
			((Component)(object)panel).Dispose();
		}
		tab.Dispose();
		OnTabClosed(tabFormTabCloseEventArgs);
	}

	protected override void OnItemRemoved(BaseItem item, ItemRemovedEventArgs e)
	{
		if (item is TabFormItem { Checked: not false } && Items.Count > 0)
		{
			bool flag = false;
			int count = Items.Count;
			int num = Math.Min(Math.Max(0, e.ItemIndex), count - 1);
			for (int i = num; i < count; i++)
			{
				if (Items[i] is TabFormItem { Enabled: not false, Visible: not false } tabFormItem2)
				{
					tabFormItem2.Checked = true;
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				for (int num2 = num; num2 >= 0; num2--)
				{
					if (Items[num2] is TabFormItem { Enabled: not false, Visible: not false } tabFormItem3)
					{
						tabFormItem3.Checked = true;
						flag = true;
						break;
					}
				}
			}
		}
		base.OnItemRemoved(item, e);
	}

	protected override void OnItemAdded(BaseItem item, EventArgs e)
	{
		if (item is TabFormItem tabFormItem && SelectedTab == null && tabFormItem.Visible && tabFormItem.Enabled)
		{
			tabFormItem.Checked = true;
		}
		base.OnItemAdded(item, e);
	}

	protected virtual void OnTabClosing(TabFormTabCloseEventArgs e)
	{
		Თ?.Invoke(this, e);
	}

	protected virtual void OnTabClosed(TabFormTabCloseEventArgs e)
	{
		Ი?.Invoke(this, e);
	}

	protected override void MouseDragOver(int x, int y, DragEventArgs dragArgs)
	{
		if (!base.DragInProgress && !base.ExternalDragInProgress)
		{
			return;
		}
		BaseItem baseItem = base.DragItem;
		if (!(baseItem is TabFormItem) || UseNativeDragDrop || !_Ზ)
		{
			base.MouseDragOver(x, y, dragArgs);
			return;
		}
		if (base.ExternalDragInProgress && dragArgs != null)
		{
			baseItem = dragArgs.Data.GetData(typeof(ButtonItem)) as BaseItem;
		}
		if (base.ট != null)
		{
			base.ট.DrawReversibleMarker(base.ঠ, base.ড);
			base.ট = null;
		}
		if (base.ExternalDragInProgress && baseItem == null)
		{
			return;
		}
		Point point = ((Control)this).PointToScreen(new Point(x, y));
		BaseItem baseItemContainer = GetBaseItemContainer();
		InsertPosition insertPosition = ((IDesignTimeProvider)baseItemContainer).GetInsertPosition(point, baseItem);
		if ((insertPosition == null || insertPosition.TargetProvider == null) && !new Rectangle(((Control)this).PointToScreen(Point.Empty), ((Control)this).Size).Contains(point))
		{
			IntPtr intPtr = _089F.WindowFromPoint(new _089F.POINT(point));
			if (intPtr != IntPtr.Zero && Control.FromChildHandle(intPtr) is TabFormStripControl tabFormStripControl)
			{
				BaseItem baseItemContainer2 = tabFormStripControl.GetBaseItemContainer();
				insertPosition = ((IDesignTimeProvider)baseItemContainer2).GetInsertPosition(point, baseItem);
			}
		}
		if (insertPosition != null && insertPosition.TargetProvider != null)
		{
			Ო();
			insertPosition.TargetProvider.DrawReversibleMarker(insertPosition.Position, insertPosition.Before);
			base.ঠ = insertPosition.Position;
			base.ড = insertPosition.Before;
			base.ট = insertPosition.TargetProvider;
			Cursor.Current = Cursors.Hand;
		}
		else
		{
			Cursor.Current = Cursors.Hand;
			if (_Კ == null)
			{
				_Კ = BarFunctions.CreateTransparentOutlineForm();
				((Control)_Კ).Name = "TabFormItemDragOutline";
			}
			Rectangle rectangle = new Rectangle(point.X - 100, point.Y - 22, 200, 150);
			_089F.SetWindowPos(((Control)_Კ).Handle, new IntPtr(0), rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, 80);
		}
		OnMouseDragOverProcessed(x, y, dragArgs);
	}

	protected override void MouseDragDrop(int x, int y, DragEventArgs dragArgs)
	{
		//IL_051f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0529: Expected O, but got Unknown
		if (!base.DragInProgress && !base.ExternalDragInProgress)
		{
			return;
		}
		Ო();
		TabFormControl tabFormControl = ᎅ();
		if ((x == -1 && y == -1) || !(base.DragItem is TabFormItem) || tabFormControl == null || !_Ზ || base.ট == _ᐚ._1CBC)
		{
			base.MouseDragDrop(x, y, dragArgs);
			return;
		}
		TabFormItem tabFormItem = (TabFormItem)base.DragItem;
		if (base.ExternalDragInProgress)
		{
			tabFormItem = dragArgs.Data.GetData(typeof(ButtonItem)) as TabFormItem;
		}
		tabFormItem?.InternalMouseLeave();
		if (base.ট != null)
		{
			base.ট.DrawReversibleMarker(base.ঠ, base.ড);
		}
		if (base.ট != null && base.ট != _ᐚ._1CBC)
		{
			int num = tabFormControl.Items.IndexOf(tabFormItem);
			tabFormControl.Items.Remove(tabFormItem);
			if (tabFormItem.Checked && num < tabFormControl.Items.Count)
			{
				for (int num2 = num; num2 >= 0; num2--)
				{
					if (tabFormControl.Items[num2] is TabFormItem && tabFormControl.Items[num2].Enabled && tabFormControl.Items[num2].Visible)
					{
						((TabFormItem)tabFormControl.Items[num2]).Checked = true;
					}
				}
			}
			if (tabFormItem.Panel != null && ((Control)tabFormItem.Panel).Parent != null)
			{
				((Control)tabFormItem.Panel).Parent.Controls.Remove((Control)(object)tabFormItem.Panel);
			}
			Form val = null;
			if (tabFormControl.TabsCount == 0 && tabFormControl.IsAutoCreated)
			{
				val = ((Control)tabFormControl).FindForm();
			}
			else
			{
				tabFormControl.RecalcLayout();
			}
			base.ট.InsertItemAt(tabFormItem, base.ঠ, base.ড);
			if (tabFormItem.Panel != null && base.ট is BaseItem && ((BaseItem)base.ট).ContainerControl is TabFormStripControl tabFormStripControl && ((Control)tabFormStripControl).Parent is TabFormControl)
			{
				((Control)tabFormStripControl).Parent.Controls.Add((Control)(object)tabFormItem.Panel);
			}
			OnItemDragAndDropped(tabFormItem);
			((IOwner)this).InvokeUserCustomize((object)tabFormItem, new EventArgs());
			if (val != null)
			{
				val.Close();
			}
		}
		else if (tabFormControl.TabsCount == 1)
		{
			Form val2 = ((Control)tabFormControl).FindForm();
			if (val2 != null)
			{
				val2.Location = new Point(Control.MousePosition.X - ((Control)val2).Width / 2, Control.MousePosition.Y - Dpi.Height16);
			}
		}
		else
		{
			TabFormItemDetachEventArgs tabFormItemDetachEventArgs = new TabFormItemDetachEventArgs(tabFormItem);
			OnBeforeTabFormItemDetach(tabFormItemDetachEventArgs);
			if (!tabFormItemDetachEventArgs.Cancel)
			{
				TabParentForm tabParentForm = null;
				TabFormControl tabFormControl2 = null;
				if (tabFormItemDetachEventArgs.TabForm != null && tabFormItemDetachEventArgs.TabControl != null)
				{
					tabParentForm = tabFormItemDetachEventArgs.TabForm;
					tabFormControl2 = tabFormItemDetachEventArgs.TabControl;
				}
				else
				{
					tabParentForm = new TabParentForm();
					((ContainerControl)tabParentForm).AutoScaleDimensions = new SizeF(96f, 96f);
					((ContainerControl)tabParentForm).AutoScaleMode = (AutoScaleMode)2;
					((Control)tabParentForm).Text = tabFormItem.Text;
					tabFormControl2 = new TabFormControl();
					tabFormControl2.IsAutoCreated = true;
					((Control)tabFormControl2).Dock = (DockStyle)5;
					((Control)tabParentForm).Controls.Add((Control)(object)tabFormControl2);
				}
				int num3 = tabFormControl.Items.IndexOf(tabFormItem);
				tabFormControl.Items.Remove(tabFormItem);
				if (tabFormItem.Checked && num3 < tabFormControl.Items.Count)
				{
					for (int num4 = num3; num4 >= 0; num4--)
					{
						if (tabFormControl.Items[num4] is TabFormItem && tabFormControl.Items[num4].Enabled && tabFormControl.Items[num4].Visible)
						{
							((TabFormItem)tabFormControl.Items[num4]).Checked = true;
						}
					}
				}
				if (tabFormItem.Panel != null && ((Control)tabFormItem.Panel).Parent != null)
				{
					((Control)tabFormItem.Panel).Parent.Controls.Remove((Control)(object)tabFormItem.Panel);
				}
				if (tabFormItem.Panel != null)
				{
					((Control)tabFormControl2).Controls.Add((Control)(object)tabFormItem.Panel);
					((Control)tabFormItem.Panel).SendToBack();
				}
				tabFormControl2.Items.Add(tabFormItem);
				tabFormControl2.SelectedTab = tabFormItem;
				tabFormControl2.RecalcLayout();
				((Form)tabParentForm).StartPosition = (FormStartPosition)0;
				((Form)tabParentForm).Location = new Point(Control.MousePosition.X - 400, Control.MousePosition.Y - Dpi.Height16);
				((Form)tabParentForm).Size = new Size(800, 600);
				tabFormItemDetachEventArgs.TabForm = tabParentForm;
				tabFormItemDetachEventArgs.TabControl = tabFormControl2;
				OnTabFormItemDetach(tabFormItemDetachEventArgs);
				if (MultiFormAppContext.Current != null)
				{
					MultiFormAppContext.Current.RegisterOpenForm((Form)(object)tabParentForm);
				}
				((Control)tabParentForm).Show();
				((IOwner)this).InvokeUserCustomize((object)tabFormItem, EventArgs.Empty);
			}
		}
		base.ট = null;
		base.DragInProgress = false;
		base.ExternalDragInProgress = false;
		Cursor.Current = Cursors.Default;
		((Control)this).Capture = false;
		if (tabFormItem != null)
		{
			tabFormItem._IgnoreClick = true;
			tabFormItem.DragStartPoint = Point.Empty;
		}
		GetBaseItemContainer().InternalMouseUp(new MouseEventArgs((MouseButtons)1048576, 0, x, y, 0));
		if (tabFormItem != null)
		{
			tabFormItem._IgnoreClick = false;
		}
		base.DragItem = null;
	}

	protected virtual void OnBeforeTabFormItemDetach(TabFormItemDetachEventArgs e)
	{
		Ლ?.Invoke(this, e);
	}

	protected virtual void OnTabFormItemDetach(TabFormItemDetachEventArgs e)
	{
		Მ?.Invoke(this, e);
	}

	private void Ო()
	{
		if (_Კ != null)
		{
			_Კ.Close();
			((Component)(object)_Კ).Dispose();
			_Კ = null;
		}
	}
}
