using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Metro;
using Stimulsoft.Controls.Win.DotNetBar.ScrollBar;
using Stimulsoft.Controls.Win.DotNetBar.Touch;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxBitmap(typeof(ItemPanel), "Ribbon.ItemPanel.ico")]
[ToolboxItem(false)]
public class ItemPanel : ItemControl, IScrollableItemControl, IBindingSupport
{
	private ItemContainer m_ItemContainer;

	private bool m_EnableDragDrop;

	private Point m_MouseDownPoint = Point.Empty;

	private TouchHandler _TouchHandler;

	private eScrollBarAppearance _ScrollBarAppearance;

	private int _EntryCount;

	private VScrollBarAdv _VScrollBar;

	private HScrollBarAdv _HScrollBar;

	private Control _Thumb;

	private bool _AutoScroll;

	private Size _AutoScrollMinSize = Size.Empty;

	private Point _AutoScrollPosition = Point.Empty;

	private bool _RepositioningScrollbars;

	private bool _SuspendPaint;

	private BaseItem _ItemTemplate;

	private ItemVisualGenerator _BindingGenerator;

	private bool _AllowSelection = true;

	private CurrencyManager _DataManager;

	private int _SelectedIndex = -1;

	private bool _TouchEnabled = true;

	private bool _TouchDrag;

	private Point _TouchStartLocation = Point.Empty;

	private Point _TouchStartScrollPosition = Point.Empty;

	private Rectangle _TouchInnerBounds = Rectangle.Empty;

	[DefaultValue(eScrollBarAppearance.Default)]
	[Category("Appearance")]
	[Description("Gets or sets the scroll-bar visual style.")]
	public eScrollBarAppearance ScrollBarAppearance
	{
		get
		{
			return _ScrollBarAppearance;
		}
		set
		{
			_ScrollBarAppearance = value;
			OnScrollBarAppearanceChanged();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Specifies the visual style of the control.")]
	[DefaultValue(eDotNetBarStyle.Office2007)]
	public eDotNetBarStyle Style
	{
		get
		{
			return m_ItemContainer.Style;
		}
		set
		{
			base.ColorScheme.SwitchStyle(value);
			m_ItemContainer.Style = value;
			((Control)this).Invalidate();
			RecalcLayout();
		}
	}

	[Browsable(true)]
	[DefaultValue(1)]
	[Category("Layout")]
	[Description("Indicates spacing in pixels between items.")]
	public virtual int ItemSpacing
	{
		get
		{
			return m_ItemContainer.ItemSpacing;
		}
		set
		{
			m_ItemContainer.ItemSpacing = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Layout")]
	[DefaultValue(eOrientation.Horizontal)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	public virtual eOrientation LayoutOrientation
	{
		get
		{
			return m_ItemContainer.LayoutOrientation;
		}
		set
		{
			m_ItemContainer.LayoutOrientation = value;
			if (((Component)(object)this).DesignMode)
			{
				RecalcLayout();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(true)]
	[Category("Layout")]
	public virtual bool ResizeItemsToFit
	{
		get
		{
			return m_ItemContainer.ResizeItemsToFit;
		}
		set
		{
			m_ItemContainer.ResizeItemsToFit = value;
		}
	}

	[DefaultValue(false)]
	[Category("Layout")]
	[Description("Indicates whether ButtonItem buttons when in vertical layout are fit into the available width so any text inside of them is wrapped if needed.")]
	public bool FitButtonsToContainerWidth
	{
		get
		{
			return m_ItemContainer.FitOversizeItemIntoAvailableWidth;
		}
		set
		{
			m_ItemContainer.FitOversizeItemIntoAvailableWidth = value;
			if (((Component)(object)this).DesignMode)
			{
				RecalcLayout();
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(eHorizontalItemsAlignment.Left)]
	[Category("Layout")]
	[Description("Indicates item alignment when container is in horizontal layout.")]
	[DevCoBrowsable(true)]
	public eHorizontalItemsAlignment HorizontalItemAlignment
	{
		get
		{
			return m_ItemContainer.HorizontalItemAlignment;
		}
		set
		{
			m_ItemContainer.HorizontalItemAlignment = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Layout")]
	[Description("Indicates whether items in horizontal layout are wrapped into the new line when they cannot fit allotted container size.")]
	public virtual bool MultiLine
	{
		get
		{
			return m_ItemContainer.MultiLine;
		}
		set
		{
			m_ItemContainer.MultiLine = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Browsable(false)]
	public SubItemsCollection Items => m_ItemContainer.SubItems;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	IList IBindingSupport.ItemsCollection => Items;

	private bool IsSelectable => ((Control)this).GetStyle((ControlStyles)512);

	[Browsable(false)]
	public VScrollBarAdv VScrollBar => _VScrollBar;

	[Browsable(false)]
	public HScrollBarAdv HScrollBar => _HScrollBar;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Size AutoScrollMargin
	{
		get
		{
			return ((ScrollableControl)this).AutoScrollMargin;
		}
		set
		{
			((ScrollableControl)this).AutoScrollMargin = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	public virtual bool AutoScroll
	{
		get
		{
			return _AutoScroll;
		}
		set
		{
			if (_AutoScroll != value)
			{
				_AutoScroll = value;
				RecalcLayout();
				UpdateScrollBars();
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Size AutoScrollMinSize
	{
		get
		{
			return _AutoScrollMinSize;
		}
		set
		{
			_AutoScrollMinSize = value;
			UpdateScrollBars();
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[Description("Indicates location of the auto-scroll position.")]
	public Point AutoScrollPosition
	{
		get
		{
			return _AutoScrollPosition;
		}
		set
		{
			if (value.X > 0)
			{
				value.X = -value.X;
			}
			if (value.Y > 0)
			{
				value.Y = -value.Y;
			}
			if (!(_AutoScrollPosition != value))
			{
				return;
			}
			_AutoScrollPosition = value;
			if (_AutoScroll)
			{
				if (_VScrollBar != null && _VScrollBar.Value != -_AutoScrollPosition.Y)
				{
					_VScrollBar.Value = Math.Min(_VScrollBar.Maximum, -_AutoScrollPosition.Y);
				}
				if (_HScrollBar != null && _HScrollBar.Value != -_AutoScrollPosition.X)
				{
					_HScrollBar.Value = Math.Min(_HScrollBar.Maximum, -_AutoScrollPosition.X);
				}
				((Control)this).Invalidate();
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool SuspendPaint
	{
		get
		{
			return _SuspendPaint;
		}
		set
		{
			_SuspendPaint = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(null)]
	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.ItemSelectorTypeEditor, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public BaseItem ItemTemplate
	{
		get
		{
			return _ItemTemplate;
		}
		set
		{
			_ItemTemplate = value;
			OnItemTemplateChanged();
		}
	}

	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public List<BindingDef> ItemTemplateBindings
	{
		get
		{
			EnsureBindingGenerator();
			return _BindingGenerator.Bindings;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public List<BaseItem> SelectedItems
	{
		get
		{
			List<BaseItem> list = new List<BaseItem>();
			SubItemsCollection items = Items;
			GetSelectedItems(list, items);
			return list;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public BaseItem SelectedItem
	{
		get
		{
			int selectedIndex = SelectedIndex;
			if (selectedIndex == -1)
			{
				return null;
			}
			return Items[selectedIndex];
		}
		set
		{
			SetItemSelection(SelectedItem, isSelected: false);
			SetItemSelection(value, isSelected: true);
			if (value != null)
			{
				_SelectedIndex = Items.IndexOf(value);
			}
			else
			{
				_SelectedIndex = -1;
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Gets or sets whether external ButtonItem object is accepted in drag and drop operation.")]
	public override bool AllowExternalDrop
	{
		get
		{
			return base.AllowExternalDrop;
		}
		set
		{
			base.AllowExternalDrop = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Specifies whether native .NET Drag and Drop is used by control to perform drag and drop operations.")]
	public override bool UseNativeDragDrop
	{
		get
		{
			return base.UseNativeDragDrop;
		}
		set
		{
			base.UseNativeDragDrop = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Specifies whether automatic drag & drop support is enabled.")]
	public virtual bool EnableDragDrop
	{
		get
		{
			return m_EnableDragDrop;
		}
		set
		{
			m_EnableDragDrop = value;
		}
	}

	protected override Size DefaultSize => new Size(200, 200);

	[AttributeProvider(typeof(IListSource))]
	[Description("Indicates data source for the ComboTree.")]
	[Category("Data")]
	[DefaultValue(null)]
	[RefreshProperties(RefreshProperties.Repaint)]
	public object DataSource
	{
		get
		{
			if (_BindingGenerator == null)
			{
				return null;
			}
			return _BindingGenerator.DataSource;
		}
		set
		{
			if (value != null || _BindingGenerator != null)
			{
				EnsureBindingGenerator();
				if (_BindingGenerator.DataSource != value)
				{
					_BindingGenerator.DataSource = value;
					OnDataSourceChanged(EventArgs.Empty);
				}
			}
		}
	}

	bool IBindingSupport.AllowSelection
	{
		get
		{
			return _AllowSelection;
		}
		set
		{
			_AllowSelection = value;
		}
	}

	protected CurrencyManager DataManager => _DataManager;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Description("Gets or sets the index specifying the currently selected item.")]
	public int SelectedIndex
	{
		get
		{
			return _SelectedIndex;
		}
		set
		{
			if (value == _SelectedIndex)
			{
				return;
			}
			if (value == -1)
			{
				_SelectedIndex = -1;
				SetItemSelection(SelectedItem, isSelected: false);
			}
			else
			{
				BaseItem item = null;
				if (_SelectedIndex >= Items.Count)
				{
					_SelectedIndex = -1;
					return;
				}
				if (_SelectedIndex > -1)
				{
					item = Items[_SelectedIndex];
				}
				SetItemSelection(SelectedItem, isSelected: false);
				SetItemSelection(item, isSelected: true);
				_SelectedIndex = value;
			}
			OnSelectedIndexChanged(EventArgs.Empty);
		}
	}

	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether touch support for scrolling is enabled.")]
	public bool TouchEnabled
	{
		get
		{
			return _TouchEnabled;
		}
		set
		{
			if (value != _TouchEnabled)
			{
				bool touchEnabled = _TouchEnabled;
				_TouchEnabled = value;
				OnTouchEnabledChanged(touchEnabled, value);
			}
		}
	}

	private int TriggerPageChangeOffset => 32;

	private int MaximumReversePageOffset => Math.Min(32, ((Control)this).Width / 6);

	[Description("Occurs when the DataSource changes.")]
	public event EventHandler DataSourceChanged;

	[Description("Occurs when a item for an data-bound object item has been created and provides you with opportunity to modify the created item")]
	public event DataItemVisualEventHandler DataItemVisualCreated;

	[Description("Occurs when value of SelectedIndex property has changed.")]
	public event EventHandler SelectedIndexChanged;

	public ItemPanel()
	{
		m_ItemContainer = new ItemContainer();
		m_ItemContainer.GlobalItem = false;
		m_ItemContainer.ContainerControl = this;
		m_ItemContainer.Stretch = false;
		m_ItemContainer.Displayed = true;
		m_ItemContainer.Style = eDotNetBarStyle.Office2007;
		base.ColorScheme.Style = eDotNetBarStyle.Office2007;
		m_ItemContainer.SetOwner(this);
		m_ItemContainer.SetSystemContainer(b: true);
		SetBaseItemContainer(m_ItemContainer);
		m_ItemContainer.Style = eDotNetBarStyle.Office2007;
		DragDropSupport = true;
		if (BarFunctions.IsWindows7 && TouchHandler.IsTouchEnabled)
		{
			_TouchHandler = new TouchHandler((Control)(object)this, eTouchHandlerType.Gesture);
			_TouchHandler.PanBegin += TouchHandlerPanBegin;
			_TouchHandler.Pan += TouchHandlerPan;
			_TouchHandler.PanEnd += TouchHandlerPanEnd;
		}
	}

	private void OnScrollBarAppearanceChanged()
	{
		if (_VScrollBar != null)
		{
			_VScrollBar.Appearance = _ScrollBarAppearance;
		}
		if (_HScrollBar != null)
		{
			_HScrollBar.Appearance = _ScrollBarAppearance;
		}
	}

	public void InvalidateNonClient()
	{
		if (BarFunctions.IsHandleValid((Control)(object)this))
		{
			WinApi.RedrawWindow(((Control)this).Handle, IntPtr.Zero, IntPtr.Zero, (WinApi.RedrawWindowFlags)1025u);
		}
	}

	public ButtonItem GetChecked()
	{
		foreach (BaseItem item in Items)
		{
			if (item.Visible && item is ButtonItem && ((ButtonItem)item).Checked)
			{
				return item as ButtonItem;
			}
		}
		return null;
	}

	public void EnsureVisible(BaseItem item)
	{
		if (item.ContainerControl == this)
		{
			Rectangle displayRectangle = item.DisplayRectangle;
			Rectangle clientRectangle = ((Control)this).ClientRectangle;
			if (clientRectangle.Width < m_ItemContainer.WidthInternal)
			{
				clientRectangle.Width = m_ItemContainer.WidthInternal;
			}
			if ((displayRectangle.Y < clientRectangle.Y || displayRectangle.Bottom > clientRectangle.Height) && AutoScroll)
			{
				Point autoScrollPosition = Point.Empty;
				autoScrollPosition = ((displayRectangle.Y >= 0) ? new Point(AutoScrollPosition.X, displayRectangle.Bottom + SystemInformation.HorizontalScrollBarHeight + 2 - (AutoScrollPosition.Y + ((Control)this).ClientRectangle.Height)) : new Point(AutoScrollPosition.X, Math.Abs(AutoScrollPosition.Y - displayRectangle.Y) - 2));
				InvalidateLayout();
				AutoScrollPosition = autoScrollPosition;
				RecalcLayout();
			}
		}
	}

	protected override Rectangle GetPaintClipRectangle()
	{
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		if (base.BackgroundStyle == null)
		{
			return clientRectangle;
		}
		clientRectangle.X += ElementStyleLayout.LeftWhiteSpace(base.BackgroundStyle);
		clientRectangle.Width -= ElementStyleLayout.HorizontalStyleWhiteSpace(base.BackgroundStyle);
		clientRectangle.Y += ElementStyleLayout.TopWhiteSpace(base.BackgroundStyle);
		clientRectangle.Height -= ElementStyleLayout.VerticalStyleWhiteSpace(base.BackgroundStyle);
		if (_VScrollBar != null)
		{
			clientRectangle.Width -= ((Control)_VScrollBar).Width;
		}
		if (_HScrollBar != null)
		{
			clientRectangle.Height -= ((Control)_HScrollBar).Height;
		}
		return clientRectangle;
	}

	protected override Rectangle GetItemContainerRectangle()
	{
		Rectangle itemContainerRectangle = base.GetItemContainerRectangle();
		if (AutoScroll && AutoScrollPosition.Y != 0)
		{
			itemContainerRectangle.Y += AutoScrollPosition.Y;
		}
		if (AutoScroll && AutoScrollPosition.X != 0)
		{
			itemContainerRectangle.X += AutoScrollPosition.X;
		}
		if (_VScrollBar != null)
		{
			itemContainerRectangle.Width -= ((Control)_VScrollBar).Width;
		}
		if (_HScrollBar != null)
		{
			itemContainerRectangle.Height -= ((Control)_HScrollBar).Height;
		}
		return itemContainerRectangle;
	}

	private void ApplyScrollChange()
	{
		if (!AutoScroll)
		{
			return;
		}
		if (m_ItemContainer.NeedRecalcSize)
		{
			RecalcSize();
			return;
		}
		Rectangle itemContainerRectangle = base.GetItemContainerRectangle();
		if (AutoScrollPosition.Y != 0)
		{
			itemContainerRectangle.Y += AutoScrollPosition.Y;
		}
		if (AutoScrollPosition.X != 0)
		{
			itemContainerRectangle.X += AutoScrollPosition.X;
		}
		itemContainerRectangle.Height = m_ItemContainer.HeightInternal;
		itemContainerRectangle.Width = m_ItemContainer.WidthInternal;
		m_ItemContainer.Bounds = itemContainerRectangle;
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		if (!((Control)this).Focused && IsSelectable)
		{
			((Control)this).Select();
		}
		m_MouseDownPoint.X = e.X;
		m_MouseDownPoint.Y = e.Y;
		base.OnMouseDown(e);
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Invalid comparison between Unknown and I4
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Invalid comparison between Unknown and I4
		base.OnMouseMove(e);
		if (m_EnableDragDrop && !base.DragInProgress && ((int)e.Button == 1048576 || ((int)e.Button == 2097152 && _TouchHandler != null)) && (Math.Abs(e.X - m_MouseDownPoint.X) > SystemInformation.DragSize.Width || Math.Abs(e.Y - m_MouseDownPoint.Y) > SystemInformation.DragSize.Height))
		{
			BaseItem baseItem = HitTest(e.X, e.Y);
			if (baseItem != null && baseItem.CanCustomize)
			{
				((IOwner)this).StartItemDrag(baseItem);
			}
		}
	}

	protected override bool ProcessDialogKey(Keys keyData)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Expected O, but got Unknown
		if (((Control)this).Focused)
		{
			KeyEventArgs val = new KeyEventArgs(keyData);
			m_ItemContainer.InternalKeyDown(val);
			if (val.Handled)
			{
				return true;
			}
		}
		return base.ProcessDialogKey(keyData);
	}

	protected override void OnMouseWheel(MouseEventArgs e)
	{
		if (_VScrollBar != null && ((Control)_VScrollBar).Visible)
		{
			int num = _VScrollBar.Value + _VScrollBar.SmallChange * ((e.Delta < 0) ? 1 : (-1));
			if (num < _VScrollBar.Minimum)
			{
				num = _VScrollBar.Minimum;
			}
			if (num > _VScrollBar.Maximum - _VScrollBar.LargeChange + 1)
			{
				num = _VScrollBar.Maximum - _VScrollBar.LargeChange + 1;
			}
			_VScrollBar.DoScroll(num, (ScrollEventType)(e.Delta < 0));
		}
		((ScrollableControl)this).OnMouseWheel(e);
	}

	protected override void RecalcSize()
	{
		_EntryCount++;
		try
		{
			m_ItemContainer.MinimumSize = new Size(GetItemContainerRectangle().Width, 0);
			base.RecalcSize();
			if (((Control)this).AutoSize || !AutoScroll)
			{
				return;
			}
			if (m_ItemContainer.HeightInternal > ((Control)this).ClientRectangle.Height || m_ItemContainer.WidthInternal > ((Control)this).ClientRectangle.Width)
			{
				Size empty = Size.Empty;
				int num = ((Control)this).ClientRectangle.Height;
				if (m_ItemContainer.WidthInternal > ((Control)this).ClientRectangle.Width)
				{
					num -= SystemInformation.HorizontalScrollBarHeight;
				}
				if (m_ItemContainer.HeightInternal > num)
				{
					empty.Height = m_ItemContainer.HeightInternal;
				}
				num = ((Control)this).ClientRectangle.Width;
				if (m_ItemContainer.HeightInternal > ((Control)this).ClientRectangle.Height)
				{
					num -= SystemInformation.VerticalScrollBarWidth;
				}
				if (m_ItemContainer.WidthInternal > num)
				{
					empty.Width = m_ItemContainer.WidthInternal + SystemInformation.VerticalScrollBarWidth;
				}
				if (base.BackgroundStyle != null)
				{
					empty.Width += ElementStyleLayout.HorizontalStyleWhiteSpace(base.BackgroundStyle);
					empty.Height += ElementStyleLayout.VerticalStyleWhiteSpace(base.BackgroundStyle);
					if (empty.Width > ((Control)this).ClientRectangle.Width)
					{
						empty.Height += SystemInformation.HorizontalScrollBarHeight + 1;
					}
				}
				bool flag = _VScrollBar == null;
				bool flag2 = _HScrollBar == null;
				if (AutoScrollMinSize != empty)
				{
					((Control)this).Invalidate();
					AutoScrollMinSize = empty;
				}
				flag ^= _VScrollBar == null;
				flag2 ^= _HScrollBar == null;
				if (_EntryCount < 4 && (flag || flag2))
				{
					RecalcSize();
				}
			}
			else if (!AutoScrollMinSize.IsEmpty)
			{
				AutoScrollMinSize = Size.Empty;
			}
		}
		finally
		{
			_EntryCount--;
		}
	}

	protected override void OnResize(EventArgs e)
	{
		base.OnResize(e);
		UpdateScrollBars();
		RepositionScrollBars();
	}

	protected override void OnLayout(LayoutEventArgs e)
	{
		((ContainerControl)this).OnLayout(e);
		RepositionScrollBars();
	}

	private void UpdateScrollBars()
	{
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Expected O, but got Unknown
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Expected O, but got Unknown
		if (!_AutoScroll)
		{
			RemoveHScrollBar();
			RemoveVScrollBar();
			if (_Thumb != null)
			{
				((Control)this).Controls.Remove(_Thumb);
				((Component)(object)_Thumb).Dispose();
				_Thumb = null;
			}
			return;
		}
		Rectangle innerRect = ElementStyleLayout.GetInnerRect(base.BackgroundStyle, ((Control)this).ClientRectangle);
		Size autoScrollMinSize = _AutoScrollMinSize;
		if (autoScrollMinSize.Height > innerRect.Height)
		{
			if (_VScrollBar == null)
			{
				_VScrollBar = new VScrollBarAdv();
				_VScrollBar.Appearance = _ScrollBarAppearance;
				((Control)_VScrollBar).Width = SystemInformation.VerticalScrollBarWidth;
				((Control)this).Controls.Add((Control)(object)_VScrollBar);
				((Control)_VScrollBar).BringToFront();
				_VScrollBar.Scroll += new ScrollEventHandler(VScrollBarScroll);
			}
			if (_VScrollBar.Minimum != 0)
			{
				_VScrollBar.Minimum = 0;
			}
			if (_VScrollBar.LargeChange != innerRect.Height && innerRect.Height > 0)
			{
				_VScrollBar.LargeChange = innerRect.Height;
			}
			_VScrollBar.SmallChange = 22;
			if (_VScrollBar.Maximum != _AutoScrollMinSize.Height)
			{
				_VScrollBar.Maximum = _AutoScrollMinSize.Height;
			}
			if (_VScrollBar.Value != -_AutoScrollPosition.Y)
			{
				_VScrollBar.Value = Math.Min(_VScrollBar.Maximum, Math.Abs(_AutoScrollPosition.Y));
			}
		}
		else
		{
			RemoveVScrollBar();
		}
		if (autoScrollMinSize.Width > innerRect.Width)
		{
			if (_HScrollBar == null)
			{
				_HScrollBar = new HScrollBarAdv();
				_HScrollBar.Appearance = _ScrollBarAppearance;
				((Control)_HScrollBar).Height = SystemInformation.HorizontalScrollBarHeight;
				((Control)this).Controls.Add((Control)(object)_HScrollBar);
				((Control)_HScrollBar).BringToFront();
				_HScrollBar.Scroll += new ScrollEventHandler(HScrollBarScroll);
			}
			if (_HScrollBar.Minimum != 0)
			{
				_HScrollBar.Minimum = 0;
			}
			if (_HScrollBar.LargeChange != innerRect.Width && innerRect.Width > 0)
			{
				_HScrollBar.LargeChange = innerRect.Width;
			}
			if (_HScrollBar.Maximum != _AutoScrollMinSize.Width)
			{
				_HScrollBar.Maximum = _AutoScrollMinSize.Width;
			}
			if (_HScrollBar.Value != -_AutoScrollPosition.X)
			{
				_HScrollBar.Value = Math.Min(_HScrollBar.Maximum, Math.Abs(_AutoScrollPosition.X));
			}
			_HScrollBar.SmallChange = 22;
		}
		else
		{
			RemoveHScrollBar();
		}
		RepositionScrollBars();
	}

	private void VScrollBarScroll(object sender, ScrollEventArgs e)
	{
		_AutoScrollPosition.Y = -e.NewValue;
		ApplyScrollChange();
		((Control)this).Invalidate();
		((ScrollableControl)this).OnScroll(e);
	}

	private void HScrollBarScroll(object sender, ScrollEventArgs e)
	{
		_AutoScrollPosition.X = -e.NewValue;
		ApplyScrollChange();
		((Control)this).Invalidate();
	}

	private void RepositionScrollBars()
	{
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Expected O, but got Unknown
		if (_RepositioningScrollbars)
		{
			return;
		}
		_RepositioningScrollbars = true;
		try
		{
			Rectangle innerRect = ElementStyleLayout.GetInnerRect(base.BackgroundStyle, ((Control)this).ClientRectangle);
			if (_HScrollBar != null)
			{
				int num = innerRect.Width;
				if (_VScrollBar != null)
				{
					num -= ((Control)_VScrollBar).Width;
				}
				((Control)_HScrollBar).Bounds = new Rectangle(innerRect.X, innerRect.Bottom - ((Control)_HScrollBar).Height + 1, num, ((Control)_HScrollBar).Height);
			}
			if (_VScrollBar != null)
			{
				int num2 = innerRect.Height;
				if (_HScrollBar != null)
				{
					num2 -= ((Control)_HScrollBar).Height;
				}
				((Control)_VScrollBar).Bounds = new Rectangle(innerRect.Right - ((Control)_VScrollBar).Width + 1, innerRect.Y, ((Control)_VScrollBar).Width, num2);
			}
			if (_VScrollBar != null && _HScrollBar != null)
			{
				if (_Thumb == null)
				{
					_Thumb = new Control();
					if (((Control)this).BackColor.A == byte.MaxValue)
					{
						_Thumb.BackColor = ((Control)this).BackColor;
					}
					else
					{
						_Thumb.BackColor = Color.White;
					}
					if (!base.BackgroundStyle.BackColor.IsEmpty && base.BackgroundStyle.BackColor.A == byte.MaxValue)
					{
						_Thumb.BackColor = base.BackgroundStyle.BackColor;
					}
					((Control)this).Controls.Add(_Thumb);
				}
				_Thumb.Bounds = new Rectangle(((Control)_HScrollBar).Bounds.Right, ((Control)_VScrollBar).Bounds.Bottom, ((Control)_VScrollBar).Width, ((Control)_HScrollBar).Height);
				_Thumb.BringToFront();
			}
			else if (_Thumb != null)
			{
				Control thumb = _Thumb;
				_Thumb = null;
				((Control)this).Controls.Remove(thumb);
				((Component)(object)thumb).Dispose();
			}
		}
		finally
		{
			_RepositioningScrollbars = false;
		}
	}

	private void RemoveHScrollBar()
	{
		if (_HScrollBar != null)
		{
			Rectangle bounds = ((Control)_HScrollBar).Bounds;
			((Control)this).Controls.Remove((Control)(object)_HScrollBar);
			((Component)(object)_HScrollBar).Dispose();
			_HScrollBar = null;
			((Control)this).Invalidate(bounds);
			_AutoScrollPosition.X = 0;
			if (m_ItemContainer != null)
			{
				m_ItemContainer.NeedRecalcSize = true;
			}
		}
	}

	private void RemoveVScrollBar()
	{
		if (_VScrollBar != null)
		{
			Rectangle bounds = ((Control)_VScrollBar).Bounds;
			((Control)this).Controls.Remove((Control)(object)_VScrollBar);
			((Component)(object)_VScrollBar).Dispose();
			_VScrollBar = null;
			((Control)this).Invalidate(bounds);
			_AutoScrollPosition.Y = 0;
			if (m_ItemContainer != null)
			{
				m_ItemContainer.NeedRecalcSize = true;
			}
		}
	}

	private void OnItemTemplateChanged()
	{
		if (_ItemTemplate != null)
		{
			_ItemTemplate.GlobalItem = false;
		}
		if (_BindingGenerator != null)
		{
			_BindingGenerator.VisualTemplate = GetItemTemplate();
		}
	}

	private BaseItem GetItemTemplate()
	{
		if (_ItemTemplate == null)
		{
			return GetDefaultItemTemplate();
		}
		return _ItemTemplate;
	}

	private BaseItem GetDefaultItemTemplate()
	{
		ButtonItem buttonItem = new ButtonItem();
		buttonItem.Shape = new RoundRectangleShapeDescriptor();
		buttonItem.OptionGroup = "items";
		buttonItem.GlobalItem = false;
		buttonItem.ColorTable = eButtonColor.Flat;
		return buttonItem;
	}

	public BaseItem AddItem(string text)
	{
		BaseItem itemTemplate = GetItemTemplate();
		if (itemTemplate == null)
		{
			throw new NullReferenceException("ItemTemplate property not set.");
		}
		BaseItem baseItem = itemTemplate.Copy();
		baseItem.Text = text;
		Items.Add(baseItem);
		return baseItem;
	}

	private static void GetSelectedItems(List<BaseItem> items, SubItemsCollection itemsCollection)
	{
		foreach (BaseItem item in itemsCollection)
		{
			if (item is ItemContainer)
			{
				GetSelectedItems(items, item.SubItems);
			}
			else if (item is ButtonItem { Checked: not false } buttonItem)
			{
				items.Add(buttonItem);
			}
			else if (item is CheckBoxItem { Checked: not false } checkBoxItem)
			{
				items.Add(checkBoxItem);
			}
			else if (item is MetroTileItem && ((MetroTileItem)item).Checked)
			{
				items.Add(item);
			}
		}
	}

	private bool SetItemSelection(BaseItem item, bool isSelected)
	{
		if (item == null)
		{
			return false;
		}
		bool result = true;
		if (item is CheckBoxItem)
		{
			((CheckBoxItem)item).Checked = isSelected;
		}
		else if (item is ButtonItem)
		{
			((ButtonItem)item).Checked = isSelected;
		}
		else if (item is MetroTileItem)
		{
			((MetroTileItem)item).Checked = isSelected;
		}
		else
		{
			result = false;
		}
		return result;
	}

	void IScrollableItemControl.KeyboardItemSelected(BaseItem item)
	{
		if (item != null)
		{
			EnsureVisible(item);
		}
	}

	private void EnsureBindingGenerator()
	{
		if (_BindingGenerator == null)
		{
			_BindingGenerator = new ItemVisualGenerator(this);
			_BindingGenerator.VisualTemplate = GetItemTemplate();
		}
	}

	protected virtual void OnDataSourceChanged(EventArgs e)
	{
		this.DataSourceChanged?.Invoke(this, e);
	}

	public virtual void RefreshItems()
	{
		if (_BindingGenerator != null)
		{
			_BindingGenerator.RefreshItems();
		}
	}

	void IBindingSupport.InvokeDataNodeCreated(DataItemVisualEventArgs e)
	{
		OnDataNodeCreated(e);
	}

	protected virtual void OnDataNodeCreated(DataItemVisualEventArgs dataNodeEventArgs)
	{
		if (this.DataItemVisualCreated != null)
		{
			this.DataItemVisualCreated(this, dataNodeEventArgs);
		}
	}

	protected override void OnButtonCheckedChanged(ButtonItem item, EventArgs e)
	{
		if (item.Checked)
		{
			_SelectedIndex = Items.IndexOf(item);
			OnSelectedIndexChanged(e);
		}
		base.OnButtonCheckedChanged(item, e);
	}

	protected virtual void OnSelectedIndexChanged(EventArgs e)
	{
		this.SelectedIndexChanged?.Invoke(this, e);
	}

	protected virtual void OnTouchEnabledChanged(bool oldValue, bool newValue)
	{
	}

	private void TouchHandlerPanBegin(object sender, GestureEventArgs e)
	{
		if (_TouchEnabled)
		{
			_TouchInnerBounds = ElementStyleLayout.GetInnerRect(base.BackgroundStyle, ((Control)this).ClientRectangle);
			_TouchStartLocation = e.Location;
			_TouchStartScrollPosition = _AutoScrollPosition;
			_TouchDrag = true;
			e.Handled = true;
		}
	}

	private void TouchHandlerPanEnd(object sender, GestureEventArgs e)
	{
		if (_TouchDrag)
		{
			EndTouchPan();
			e.Handled = true;
		}
	}

	private void EndTouchPan()
	{
		_TouchDrag = false;
		if (m_ItemContainer.LayoutOrientation == eOrientation.Horizontal)
		{
			if (_AutoScrollMinSize.Width - _TouchInnerBounds.Width < -_AutoScrollPosition.X)
			{
				AutoScrollPosition = new Point(_AutoScrollMinSize.Width - _TouchInnerBounds.Width, 0);
			}
			else if (-_AutoScrollPosition.X < 0)
			{
				AutoScrollPosition = new Point(0, 0);
			}
		}
		else if (_AutoScrollMinSize.Height - _TouchInnerBounds.Height < -_AutoScrollPosition.Y)
		{
			AutoScrollPosition = new Point(0, _AutoScrollMinSize.Height - _TouchInnerBounds.Height);
		}
		else if (-_AutoScrollPosition.Y < 0)
		{
			AutoScrollPosition = new Point(0, 0);
		}
		ApplyScrollChange();
	}

	private void TouchHandlerPan(object sender, GestureEventArgs e)
	{
		if (!_TouchDrag)
		{
			return;
		}
		if (m_ItemContainer.LayoutOrientation == eOrientation.Horizontal)
		{
			int num = e.Location.X - _TouchStartLocation.X;
			int num2 = num + _TouchStartScrollPosition.X;
			bool flag = false;
			if (Math.Abs(num2) + MaximumReversePageOffset > _AutoScrollMinSize.Width - _TouchInnerBounds.Width)
			{
				_AutoScrollPosition.X = -(_AutoScrollMinSize.Width + MaximumReversePageOffset - _TouchInnerBounds.Width);
				flag = true;
			}
			else if (num2 > MaximumReversePageOffset)
			{
				_AutoScrollPosition.X = MaximumReversePageOffset;
				flag = true;
			}
			else
			{
				_AutoScrollPosition.X = num2;
			}
			((Control)this).Invalidate();
			ApplyScrollChange();
			((Control)this).Update();
			if (flag && e.IsInertia)
			{
				EndTouchPan();
			}
		}
		else
		{
			int num3 = e.Location.Y - _TouchStartLocation.Y;
			int num4 = num3 + _TouchStartScrollPosition.Y;
			bool flag2 = false;
			if (Math.Abs(num4) + MaximumReversePageOffset > _AutoScrollMinSize.Height - _TouchInnerBounds.Height)
			{
				_AutoScrollPosition.Y = -(_AutoScrollMinSize.Height + MaximumReversePageOffset - _TouchInnerBounds.Height);
				flag2 = true;
			}
			else if (num4 > MaximumReversePageOffset)
			{
				_AutoScrollPosition.Y = MaximumReversePageOffset;
				flag2 = true;
			}
			else
			{
				_AutoScrollPosition.Y = num4;
			}
			((Control)this).Invalidate();
			ApplyScrollChange();
			((Control)this).Update();
			if (flag2 && e.IsInertia)
			{
				EndTouchPan();
			}
		}
		if (_VScrollBar != null && _VScrollBar.Value != -_AutoScrollPosition.Y)
		{
			_VScrollBar.Value = Math.Min(_VScrollBar.Maximum, Math.Max(0, -_AutoScrollPosition.Y));
		}
		if (_HScrollBar != null && _HScrollBar.Value != -_AutoScrollPosition.X)
		{
			_HScrollBar.Value = Math.Min(_HScrollBar.Maximum, Math.Max(0, -_AutoScrollPosition.X));
		}
		e.Handled = true;
	}
}
