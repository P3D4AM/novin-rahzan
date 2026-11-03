using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar.ScrollBar;
using DevComponents.DotNetBar.Touch;

namespace DevComponents.DotNetBar;

[ComVisible(false)]
[ToolboxItem(false)]
public class ItemPanelBase : ItemControl, IScrollableItemControl, IBindingSupport
{
	protected ItemContainer m_ItemContainer;

	private bool ᄜ;

	private Point ၽ = Point.Empty;

	private TouchHandler _ظ;

	private eScrollBarAppearance _ז;

	private bool __1A8D = true;

	private bool __1A8E;

	private int __1A8F;

	private VScrollBarAdv __059D;

	private HScrollBarAdv __059E;

	private Control __059F;

	private bool __0616;

	private Size __0617 = Size.Empty;

	private Point __0619 = Point.Empty;

	private bool _᪐;

	private bool _᪑;

	protected int _SelectedIndex = -1;

	private BaseItem _᪒;

	private ItemVisualGenerator _᪓;

	private EventHandler _05C1;

	private DataItemVisualEventHandler _᪔;

	private bool _᪕ = true;

	private CurrencyManager _ر;

	private EventHandler _05CB;

	private bool _ع = true;

	private bool _غ;

	private Point _ػ = Point.Empty;

	private Point _ؼ = Point.Empty;

	private Rectangle _ؽ = Rectangle.Empty;

	[Category("Behavior")]
	[Description("Indicates whether container responds to keyboard presses and changes the active/mouse over item.")]
	[DefaultValue(true)]
	public bool KeyboardNavigationEnabled
	{
		get
		{
			return m_ItemContainer.KeyboardNavigationEnabled;
		}
		set
		{
			m_ItemContainer.KeyboardNavigationEnabled = value;
		}
	}

	protected virtual ItemContainer ItemContainer => m_ItemContainer;

	[Category("Appearance")]
	[DefaultValue(eScrollBarAppearance.Default)]
	[Description("Gets or sets the scroll-bar visual style.")]
	public eScrollBarAppearance ScrollBarAppearance
	{
		get
		{
			return _ז;
		}
		set
		{
			_ז = value;
			ف();
		}
	}

	[Description("Specifies the visual style of the control.")]
	[Browsable(true)]
	[DefaultValue(eDotNetBarStyle.Office2007)]
	[Category("Appearance")]
	[DevCoBrowsable(true)]
	public eDotNetBarStyle Style
	{
		get
		{
			return m_ItemContainer.Style;
		}
		set
		{
			base.ColorScheme._0875(value);
			m_ItemContainer.Style = value;
			((Control)this).Invalidate();
			RecalcLayout();
		}
	}

	[Browsable(true)]
	[Category("Layout")]
	[DefaultValue(1)]
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

	private bool _᪈
	{
		get
		{
			if (__059E != null)
			{
				return ((Control)__059E).Visible;
			}
			return false;
		}
	}

	private bool _᪉
	{
		get
		{
			if (__059D != null)
			{
				return ((Control)__059D).Visible;
			}
			return false;
		}
	}

	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether control can be scrolled when client area is dragged using mouse.")]
	public bool MouseClientScrollEnabled
	{
		get
		{
			return __1A8D;
		}
		set
		{
			__1A8D = value;
		}
	}

	private bool _1A8A => ((Control)this).GetStyle((ControlStyles)512);

	[Browsable(false)]
	public VScrollBarAdv VScrollBar => __059D;

	[Browsable(false)]
	public HScrollBarAdv HScrollBar => __059E;

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
			return __0616;
		}
		set
		{
			if (__0616 != value)
			{
				__0616 = value;
				RecalcLayout();
				ڦ();
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Size AutoScrollMinSize
	{
		get
		{
			return __0617;
		}
		set
		{
			__0617 = value;
			ڦ();
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[Description("Indicates location of the auto-scroll position.")]
	public Point AutoScrollPosition
	{
		get
		{
			return __0619;
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
			if (!(__0619 != value))
			{
				return;
			}
			__0619 = value;
			if (__0616)
			{
				if (__059D != null && __059D.Value != -__0619.Y)
				{
					__059D.Value = Math.Min(__059D.Maximum, -__0619.Y);
				}
				if (__059E != null && __059E.Value != -__0619.X)
				{
					__059E.Value = Math.Min(__059E.Maximum, -__0619.X);
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
			return _᪑;
		}
		set
		{
			_᪑ = value;
		}
	}

	[Description("Gets or sets the index specifying the currently selected item.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public virtual int SelectedIndex
	{
		get
		{
			return _SelectedIndex;
		}
		set
		{
			_SelectedIndex = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	IList IBindingSupport._1A8B => GetItemsCollection();

	[Browsable(true)]
	[Editor("DevComponents.DotNetBar.Design.ItemSelectorTypeEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[DefaultValue(null)]
	public BaseItem ItemTemplate
	{
		get
		{
			return _᪒;
		}
		set
		{
			_᪒ = value;
			_᪘();
		}
	}

	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public List<BindingDef> ItemTemplateBindings
	{
		get
		{
			EnsureBindingGenerator();
			return _᪓.Bindings;
		}
	}

	[Description("Gets or sets whether external ButtonItem object is accepted in drag and drop operation.")]
	[Browsable(true)]
	[Category("Behavior")]
	[DefaultValue(false)]
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

	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Specifies whether native .NET Drag and Drop is used by control to perform drag and drop operations.")]
	[Browsable(true)]
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

	[DefaultValue(false)]
	[Category("Behavior")]
	[Browsable(true)]
	[Description("Specifies whether automatic drag & drop support is enabled.")]
	public virtual bool EnableDragDrop
	{
		get
		{
			return ᄜ;
		}
		set
		{
			ᄜ = value;
		}
	}

	protected override Size DefaultSize => new Size(200, 200);

	[AttributeProvider(typeof(IListSource))]
	[Category("Data")]
	[RefreshProperties(RefreshProperties.Repaint)]
	[DefaultValue(null)]
	[Description("Indicates data source for the ComboTree.")]
	public object DataSource
	{
		get
		{
			if (_᪓ == null || _᪓.IsCustomCollection)
			{
				return null;
			}
			return _᪓.DataSource;
		}
		set
		{
			OnDataSourceChanged(value);
		}
	}

	protected virtual ItemVisualGenerator ItemGenerator
	{
		get
		{
			return _᪓;
		}
		set
		{
			_᪓ = value;
		}
	}

	bool IBindingSupport._1A8C
	{
		get
		{
			return _᪕;
		}
		set
		{
			_᪕ = value;
		}
	}

	protected CurrencyManager DataManager => _ر;

	[DefaultValue(true)]
	[Description("Indicates whether touch support for scrolling is enabled.")]
	[Category("Behavior")]
	public bool TouchEnabled
	{
		get
		{
			return _ع;
		}
		set
		{
			if (value != _ع)
			{
				bool ع = _ع;
				_ع = value;
				OnTouchEnabledChanged(ع, value);
			}
		}
	}

	private int Յ => 32;

	private int Ն => Math.Min(32, ((Control)this).Width / 6);

	[Description("Occurs when the DataSource changes.")]
	public event EventHandler DataSourceChanged
	{
		add
		{
			EventHandler eventHandler = _05C1;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _05C1, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _05C1;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _05C1, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when a item for an data-bound object item has been created and provides you with opportunity to modify the created item")]
	public event DataItemVisualEventHandler DataItemVisualCreated
	{
		add
		{
			DataItemVisualEventHandler dataItemVisualEventHandler = _᪔;
			DataItemVisualEventHandler dataItemVisualEventHandler2;
			do
			{
				dataItemVisualEventHandler2 = dataItemVisualEventHandler;
				DataItemVisualEventHandler value2 = (DataItemVisualEventHandler)Delegate.Combine(dataItemVisualEventHandler2, value);
				dataItemVisualEventHandler = Interlocked.CompareExchange(ref _᪔, value2, dataItemVisualEventHandler2);
			}
			while ((object)dataItemVisualEventHandler != dataItemVisualEventHandler2);
		}
		remove
		{
			DataItemVisualEventHandler dataItemVisualEventHandler = _᪔;
			DataItemVisualEventHandler dataItemVisualEventHandler2;
			do
			{
				dataItemVisualEventHandler2 = dataItemVisualEventHandler;
				DataItemVisualEventHandler value2 = (DataItemVisualEventHandler)Delegate.Remove(dataItemVisualEventHandler2, value);
				dataItemVisualEventHandler = Interlocked.CompareExchange(ref _᪔, value2, dataItemVisualEventHandler2);
			}
			while ((object)dataItemVisualEventHandler != dataItemVisualEventHandler2);
		}
	}

	[Description("Occurs when value of SelectedIndex property has changed.")]
	public event EventHandler SelectedIndexChanged
	{
		add
		{
			EventHandler eventHandler = _05CB;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _05CB, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _05CB;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _05CB, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public ItemPanelBase()
	{
		m_ItemContainer = new ItemContainer();
		m_ItemContainer.GlobalItem = false;
		m_ItemContainer.ContainerControl = this;
		m_ItemContainer.Stretch = false;
		m_ItemContainer.Displayed = true;
		m_ItemContainer.Style = eDotNetBarStyle.Office2007;
		base.ColorScheme.ߕ = eDotNetBarStyle.Office2007;
		m_ItemContainer.SetOwner(this);
		m_ItemContainer.ᆁ(_0BCF: true);
		SetBaseItemContainer(m_ItemContainer);
		m_ItemContainer.Style = eDotNetBarStyle.Office2007;
		DragDropSupport = true;
		if (BarFunctions.IsWindows7 && TouchHandler.IsTouchEnabled)
		{
			_ظ = new TouchHandler((Control)(object)this, eTouchHandlerType.Gesture);
			_ظ.PanBegin += _۸;
			_ظ.Pan += ۻ;
			_ظ.PanEnd += _۹;
		}
	}

	private void ف()
	{
		if (__059D != null)
		{
			__059D.Appearance = _ז;
		}
		if (__059E != null)
		{
			__059E.Appearance = _ז;
		}
	}

	public void InvalidateNonClient()
	{
		if (BarFunctions.IsHandleValid((Control)(object)this))
		{
			_0EFE.RedrawWindow(((Control)this).Handle, IntPtr.Zero, IntPtr.Zero, (_0EFE.RedrawWindowFlags)1025u);
		}
	}

	public void EnsureVisible(BaseItem item)
	{
		if (item.ContainerControl != this || !AutoScroll)
		{
			return;
		}
		Rectangle displayRectangle = item.DisplayRectangle;
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		if (_᪈)
		{
			clientRectangle.Height -= SystemInformation.HorizontalScrollBarHeight;
		}
		if (!clientRectangle.Contains(displayRectangle))
		{
			Point autoScrollPosition = AutoScrollPosition;
			if (displayRectangle.Y < clientRectangle.Y || displayRectangle.Bottom > clientRectangle.Height)
			{
				autoScrollPosition = ((displayRectangle.Y >= 0) ? new Point(autoScrollPosition.X, displayRectangle.Bottom + (_᪈ ? (SystemInformation.HorizontalScrollBarHeight + 2) : 0) - (AutoScrollPosition.Y + ((Control)this).ClientRectangle.Height)) : new Point(autoScrollPosition.X, Math.Abs(AutoScrollPosition.Y - displayRectangle.Y) - 2));
			}
			if ((displayRectangle.X < clientRectangle.X || displayRectangle.Right > clientRectangle.Width) && m_ItemContainer.LayoutOrientation == eOrientation.Horizontal)
			{
				autoScrollPosition = ((displayRectangle.X >= 0) ? new Point(displayRectangle.Right + (_᪉ ? (SystemInformation.VerticalScrollBarWidth + 2) : 0) - (AutoScrollPosition.X + ((Control)this).ClientRectangle.Width), autoScrollPosition.Y) : new Point(Math.Abs(AutoScrollPosition.X - displayRectangle.X) - 2, autoScrollPosition.Y));
			}
			InvalidateLayout();
			AutoScrollPosition = autoScrollPosition;
			RecalcLayout();
		}
	}

	protected override Rectangle GetPaintClipRectangle()
	{
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		if (base.BackgroundStyle == null)
		{
			return clientRectangle;
		}
		clientRectangle.X += ᝈ.ᝐ(base.BackgroundStyle);
		clientRectangle.Width -= ᝈ.ᝍ(base.BackgroundStyle);
		clientRectangle.Y += ᝈ._1753(base.BackgroundStyle);
		clientRectangle.Height -= ᝈ.ᝏ(base.BackgroundStyle);
		if (__059D != null)
		{
			clientRectangle.Width -= ((Control)__059D).Width;
		}
		if (__059E != null)
		{
			clientRectangle.Height -= ((Control)__059E).Height;
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
		if (__059D != null)
		{
			itemContainerRectangle.Width -= ((Control)__059D).Width;
		}
		if (__059E != null)
		{
			itemContainerRectangle.Height -= ((Control)__059E).Height;
		}
		return itemContainerRectangle;
	}

	private void _᪖()
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
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Invalid comparison between Unknown and I4
		if (!((Control)this).Focused && _1A8A)
		{
			((Control)this).Select();
		}
		ၽ.X = e.X;
		ၽ.Y = e.Y;
		if ((int)e.Button == 1048576 && _ظ == null && __1A8D)
		{
			Rectangle rectangle = ᝈ._1756(base.BackgroundStyle, ((Control)this).ClientRectangle);
			if (__0617.Width > rectangle.Width || __0617.Height > rectangle.Height)
			{
				BaseItem baseItem = HitTest(e.X, e.Y);
				if (baseItem == null || baseItem.IsContainer)
				{
					__1A8E = true;
					_ؽ = ᝈ._1756(base.BackgroundStyle, ((Control)this).ClientRectangle);
					_ػ = e.Location;
					_ؼ = __0619;
				}
			}
		}
		base.OnMouseDown(e);
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		if (__1A8E)
		{
			__1A8E = false;
			if (__0617.Width > _ؽ.Width)
			{
				if (__0617.Width - _ؽ.Width < -__0619.X)
				{
					AutoScrollPosition = new Point(__0617.Width - _ؽ.Width, 0);
				}
				else if (-__0619.X < 0)
				{
					AutoScrollPosition = new Point(0, 0);
				}
			}
			else if (__0617.Height > _ؽ.Height)
			{
				if (__0617.Height - _ؽ.Height < -__0619.Y)
				{
					AutoScrollPosition = new Point(0, __0617.Height - _ؽ.Height);
				}
				else if (-__0619.Y < 0)
				{
					AutoScrollPosition = new Point(0, 0);
				}
			}
			_᪖();
		}
		else
		{
			base.OnMouseUp(e);
		}
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		//IL_026d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0277: Invalid comparison between Unknown and I4
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Invalid comparison between Unknown and I4
		if (__1A8E)
		{
			if (__0617.Width > _ؽ.Width)
			{
				int num = e.Location.X - _ػ.X;
				int num2 = num + _ؼ.X;
				if (Math.Abs(num2) + Ն > __0617.Width - _ؽ.Width)
				{
					__0619.X = -(__0617.Width + Ն - _ؽ.Width);
				}
				else if (num2 > Ն)
				{
					__0619.X = Ն;
				}
				else
				{
					__0619.X = num2;
				}
				((Control)this).Invalidate();
				_᪖();
				((Control)this).Update();
			}
			else if (__0617.Height > _ؽ.Height)
			{
				int num3 = e.Location.Y - _ػ.Y;
				int num4 = num3 + _ؼ.Y;
				if (Math.Abs(num4) + Ն > __0617.Height - _ؽ.Height)
				{
					__0619.Y = -(__0617.Height + Ն - _ؽ.Height);
				}
				else if (num4 > Ն)
				{
					__0619.Y = Ն;
				}
				else
				{
					__0619.Y = num4;
				}
				((Control)this).Invalidate();
				_᪖();
				((Control)this).Update();
			}
			if (__059D != null && __059D.Value != -__0619.Y)
			{
				__059D.Value = Math.Min(__059D.Maximum, Math.Max(0, -__0619.Y));
			}
			if (__059E != null && __059E.Value != -__0619.X)
			{
				__059E.Value = Math.Min(__059E.Maximum, Math.Max(0, -__0619.X));
			}
			return;
		}
		base.OnMouseMove(e);
		if (ᄜ && !base.DragInProgress && ((int)e.Button == 1048576 || ((int)e.Button == 2097152 && _ظ != null)) && (Math.Abs(e.X - ၽ.X) > SystemInformation.DragSize.Width || Math.Abs(e.Y - ၽ.Y) > SystemInformation.DragSize.Height))
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
		if (__059D != null && ((Control)__059D).Visible)
		{
			int num = __059D.Value + __059D.SmallChange * ((e.Delta < 0) ? 1 : (-1));
			if (num > __059D.Maximum - __059D.LargeChange + 1)
			{
				num = __059D.Maximum - __059D.LargeChange + 1;
			}
			if (num < __059D.Minimum)
			{
				num = __059D.Minimum;
			}
			__059D.ᗾ(num, (ScrollEventType)(e.Delta < 0));
		}
		else if (__059E != null && ((Control)__059E).Visible)
		{
			int num2 = __059E.Value + __059E.SmallChange * ((e.Delta < 0) ? 1 : (-1));
			if (num2 > __059E.Maximum - __059E.LargeChange + 1)
			{
				num2 = __059E.Maximum - __059E.LargeChange + 1;
			}
			if (num2 < __059E.Minimum)
			{
				num2 = __059E.Minimum;
			}
			__059E.ᗾ(num2, (ScrollEventType)(e.Delta < 0));
		}
		((ScrollableControl)this).OnMouseWheel(e);
	}

	protected override void RecalcSize()
	{
		__1A8F++;
		try
		{
			m_ItemContainer.MinimumSize = new Size(Dpi.DescaleWidth(GetItemContainerRectangle().Width), 0);
			base.RecalcSize();
			if (!((Control)this).AutoSize && AutoScroll)
			{
				int num = m_ItemContainer.HeightInternal + ᝈ._1753(base.BackgroundStyle, eSpacePart.Padding | eSpacePart.Margin) + ᝈ._1754(base.BackgroundStyle, eSpacePart.Padding | eSpacePart.Margin);
				int num2 = m_ItemContainer.WidthInternal + ᝈ.ᝐ(base.BackgroundStyle, eSpacePart.Padding | eSpacePart.Margin) + ᝈ._1752(base.BackgroundStyle, eSpacePart.Padding | eSpacePart.Margin);
				if (num > ((Control)this).ClientRectangle.Height || num2 > ((Control)this).ClientRectangle.Width)
				{
					Size empty = Size.Empty;
					int num3 = ((Control)this).ClientRectangle.Height;
					if (num2 > ((Control)this).ClientRectangle.Width)
					{
						num3 -= SystemInformation.HorizontalScrollBarHeight;
					}
					if (num > num3)
					{
						empty.Height = num;
					}
					num3 = ((Control)this).ClientRectangle.Width;
					if (num > ((Control)this).ClientRectangle.Height)
					{
						num3 -= SystemInformation.VerticalScrollBarWidth;
					}
					if (num2 > num3)
					{
						empty.Width = num2 + SystemInformation.VerticalScrollBarWidth;
					}
					if (base.BackgroundStyle != null)
					{
						empty.Width += ᝈ.ᝍ(base.BackgroundStyle);
						empty.Height += ᝈ.ᝏ(base.BackgroundStyle);
						if (empty.Width > ((Control)this).ClientRectangle.Width)
						{
							empty.Height += SystemInformation.HorizontalScrollBarHeight + 1;
						}
					}
					bool flag = __059D == null;
					bool flag2 = __059E == null;
					if (AutoScrollMinSize != empty)
					{
						((Control)this).Invalidate();
						AutoScrollMinSize = empty;
					}
					flag ^= __059D == null;
					flag2 ^= __059E == null;
					if (__1A8F < 4 && (flag || flag2))
					{
						RecalcSize();
					}
				}
				else if (!AutoScrollMinSize.IsEmpty)
				{
					AutoScrollMinSize = Size.Empty;
				}
			}
		}
		finally
		{
			__1A8F--;
		}
		if (__1A8F == 0)
		{
			if (__059D != null)
			{
				((Control)__059D).Visible = true;
			}
			if (__059E != null)
			{
				((Control)__059E).Visible = true;
			}
		}
	}

	protected override void OnResize(EventArgs e)
	{
		base.OnResize(e);
		ڦ();
		ګ();
	}

	protected override void OnLayout(LayoutEventArgs e)
	{
		((ContainerControl)this).OnLayout(e);
		ګ();
	}

	private void ڦ()
	{
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Expected O, but got Unknown
		//IL_025b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0265: Expected O, but got Unknown
		if (!__0616)
		{
			ڭ();
			ڮ();
			if (__059F != null)
			{
				((Control)this).Controls.Remove(__059F);
				((Component)(object)__059F).Dispose();
				__059F = null;
			}
			return;
		}
		Rectangle rectangle = ᝈ._1756(base.BackgroundStyle, ((Control)this).ClientRectangle);
		Size size = __0617;
		if (size.Height > rectangle.Height)
		{
			if (__059D == null)
			{
				__059D = new VScrollBarAdv();
				__059D.TabStop = false;
				__059D.Appearance = _ז;
				((Control)__059D).Width = SystemInformation.VerticalScrollBarWidth;
				if (__1A8F > 0)
				{
					((Control)__059D).Visible = false;
				}
				((Control)this).Controls.Add((Control)(object)__059D);
				((Control)__059D).BringToFront();
				__059D.Scroll += new ScrollEventHandler(ک);
			}
			if (__059D.Minimum != 0)
			{
				__059D.Minimum = 0;
			}
			if (__059D.LargeChange != rectangle.Height && rectangle.Height > 0)
			{
				__059D.LargeChange = rectangle.Height;
			}
			__059D.SmallChange = 22;
			if (__059D.Maximum != __0617.Height)
			{
				__059D.Maximum = __0617.Height;
			}
			if (__059D.Value != -__0619.Y)
			{
				__059D.Value = Math.Min(__059D.Maximum, Math.Abs(__0619.Y));
			}
		}
		else
		{
			ڮ();
		}
		if (size.Width > rectangle.Width)
		{
			if (__059E == null)
			{
				__059E = new HScrollBarAdv();
				__059E.Appearance = _ז;
				__059E.TabStop = false;
				((Control)__059E).Height = SystemInformation.HorizontalScrollBarHeight;
				if (__1A8F > 0)
				{
					((Control)__059E).Visible = false;
				}
				((Control)this).Controls.Add((Control)(object)__059E);
				((Control)__059E).BringToFront();
				__059E.Scroll += new ScrollEventHandler(ڪ);
			}
			if (__059E.Minimum != 0)
			{
				__059E.Minimum = 0;
			}
			if (__059E.LargeChange != rectangle.Width && rectangle.Width > 0)
			{
				__059E.LargeChange = rectangle.Width;
			}
			if (__059E.Maximum != __0617.Width)
			{
				__059E.Maximum = __0617.Width;
			}
			if (__059E.Value != -__0619.X)
			{
				__059E.Value = Math.Min(__059E.Maximum, Math.Abs(__0619.X));
			}
			__059E.SmallChange = 22;
		}
		else
		{
			ڭ();
		}
		ګ();
	}

	private void ک(object _٠, ScrollEventArgs ؿ)
	{
		__0619.Y = -ؿ.NewValue;
		_᪖();
		((Control)this).Invalidate();
		((ScrollableControl)this).OnScroll(ؿ);
	}

	private void ڪ(object _٠, ScrollEventArgs ؿ)
	{
		__0619.X = -ؿ.NewValue;
		_᪖();
		((Control)this).Invalidate();
	}

	private void ګ()
	{
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Expected O, but got Unknown
		if (_᪐)
		{
			return;
		}
		_᪐ = true;
		try
		{
			Rectangle rectangle = ᝈ._1756(base.BackgroundStyle, ((Control)this).ClientRectangle);
			if (__059E != null)
			{
				int num = rectangle.Width;
				if (__059D != null)
				{
					num -= ((Control)__059D).Width;
				}
				((Control)__059E).Bounds = new Rectangle(rectangle.X, rectangle.Bottom - ((Control)__059E).Height + 1, num, ((Control)__059E).Height);
			}
			if (__059D != null)
			{
				int num2 = rectangle.Height;
				if (__059E != null)
				{
					num2 -= ((Control)__059E).Height;
				}
				((Control)__059D).Bounds = new Rectangle(rectangle.Right - ((Control)__059D).Width + 1, rectangle.Y, ((Control)__059D).Width, num2);
			}
			if (__059D != null && __059E != null)
			{
				if (__059F == null)
				{
					__059F = new Control();
					if (((Control)this).BackColor.A == byte.MaxValue)
					{
						__059F.BackColor = ((Control)this).BackColor;
					}
					else
					{
						__059F.BackColor = Color.White;
					}
					if (!base.BackgroundStyle.BackColor.IsEmpty && base.BackgroundStyle.BackColor.A == byte.MaxValue)
					{
						__059F.BackColor = base.BackgroundStyle.BackColor;
					}
					((Control)this).Controls.Add(__059F);
				}
				__059F.Bounds = new Rectangle(((Control)__059E).Bounds.Right, ((Control)__059D).Bounds.Bottom, ((Control)__059D).Width + 1, ((Control)__059E).Height + 1);
				__059F.BringToFront();
			}
			else if (__059F != null)
			{
				Control val = __059F;
				__059F = null;
				((Control)this).Controls.Remove(val);
				((Component)(object)val).Dispose();
			}
		}
		finally
		{
			_᪐ = false;
		}
	}

	private void ڭ()
	{
		if (__059E != null)
		{
			Rectangle bounds = ((Control)__059E).Bounds;
			((Control)this).Controls.Remove((Control)(object)__059E);
			((Component)(object)__059E).Dispose();
			__059E = null;
			((Control)this).Invalidate(bounds);
			__0619.X = 0;
			if (m_ItemContainer != null)
			{
				m_ItemContainer.NeedRecalcSize = true;
			}
		}
	}

	private void ڮ()
	{
		if (__059D != null)
		{
			Rectangle bounds = ((Control)__059D).Bounds;
			((Control)this).Controls.Remove((Control)(object)__059D);
			((Component)(object)__059D).Dispose();
			__059D = null;
			((Control)this).Invalidate(bounds);
			__0619.Y = 0;
			if (m_ItemContainer != null)
			{
				m_ItemContainer.NeedRecalcSize = true;
			}
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		if (_089F.फ() || !_089F.थ)
		{
			e.Graphics.Clear(SystemColors.Control);
		}
		else if (!_᪑)
		{
			base.OnPaint(e);
		}
	}

	protected virtual IList GetItemsCollection()
	{
		return m_ItemContainer.SubItems;
	}

	private void _᪘()
	{
		if (_᪒ != null)
		{
			_᪒.GlobalItem = false;
		}
		if (_᪓ != null)
		{
			_᪓.VisualTemplate = GetItemTemplate();
		}
	}

	protected virtual BaseItem GetItemTemplate()
	{
		if (_᪒ == null)
		{
			return GetDefaultItemTemplate();
		}
		return _᪒;
	}

	protected virtual BaseItem GetDefaultItemTemplate()
	{
		ButtonItem buttonItem = new ButtonItem();
		buttonItem.Shape = new RoundRectangleShapeDescriptor();
		buttonItem.OptionGroup = "items";
		buttonItem.GlobalItem = false;
		buttonItem.ColorTable = eButtonColor.Flat;
		return buttonItem;
	}

	private void _᪙(BaseItem _06EC)
	{
		OnKeyboardItemSelected(_06EC);
	}

	void IScrollableItemControl.KeyboardItemSelected(BaseItem _06EC)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ᪙
		this._᪙(_06EC);
	}

	protected virtual void OnKeyboardItemSelected(BaseItem item)
	{
		if (item != null)
		{
			EnsureVisible(item);
		}
	}

	protected virtual void OnDataSourceChanged(object newDataSource)
	{
		if (newDataSource != null || _᪓ != null)
		{
			EnsureBindingGenerator();
			if (_᪓.DataSource != newDataSource)
			{
				_᪓.DataSource = newDataSource;
				OnDataSourceChanged(EventArgs.Empty);
			}
		}
	}

	protected virtual void EnsureBindingGenerator()
	{
		if (_᪓ == null)
		{
			_᪓ = new ItemVisualGenerator(this);
			_᪓.VisualTemplate = GetItemTemplate();
		}
	}

	protected virtual void OnDataSourceChanged(EventArgs e)
	{
		_05C1?.Invoke(this, e);
	}

	public virtual void RefreshItems()
	{
		if (_᪓ != null)
		{
			_᪓.RefreshItems();
		}
	}

	private void _1A9A(DataItemVisualEventArgs ؿ)
	{
		OnDataNodeCreated(ؿ);
	}

	void IBindingSupport.InvokeDataNodeCreated(DataItemVisualEventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ᪚
		this._1A9A(ؿ);
	}

	protected virtual void OnDataNodeCreated(DataItemVisualEventArgs dataNodeEventArgs)
	{
		if (_᪔ != null)
		{
			_᪔(this, dataNodeEventArgs);
		}
	}

	protected virtual void OnSelectedIndexChanged(EventArgs e)
	{
		_05CB?.Invoke(this, e);
	}

	protected virtual void OnTouchEnabledChanged(bool oldValue, bool newValue)
	{
	}

	private void _۸(object _٠, GestureEventArgs ؿ)
	{
		Rectangle ؽ = ᝈ._1756(base.BackgroundStyle, ((Control)this).ClientRectangle);
		if (_ع && ((m_ItemContainer.LayoutOrientation == eOrientation.Horizontal && __0617.Width > ؽ.Width) || (m_ItemContainer.LayoutOrientation == eOrientation.Vertical && __0617.Height > ؽ.Height)))
		{
			_ؽ = ؽ;
			_ػ = ؿ.Location;
			_ؼ = __0619;
			_غ = true;
			ؿ.Handled = true;
		}
	}

	private void _۹(object _٠, GestureEventArgs ؿ)
	{
		if (_غ)
		{
			ۺ();
			ؿ.Handled = true;
		}
	}

	private void ۺ()
	{
		_غ = false;
		if (m_ItemContainer.LayoutOrientation == eOrientation.Horizontal)
		{
			if (__0617.Width - _ؽ.Width < -__0619.X)
			{
				AutoScrollPosition = new Point(__0617.Width - _ؽ.Width, 0);
			}
			else if (-__0619.X < 0)
			{
				AutoScrollPosition = new Point(0, 0);
			}
		}
		else if (__0617.Height - _ؽ.Height < -__0619.Y)
		{
			AutoScrollPosition = new Point(0, __0617.Height - _ؽ.Height);
		}
		else if (-__0619.Y < 0)
		{
			AutoScrollPosition = new Point(0, 0);
		}
		_᪖();
	}

	private void ۻ(object _٠, GestureEventArgs ؿ)
	{
		if (!_غ)
		{
			return;
		}
		if (m_ItemContainer.LayoutOrientation == eOrientation.Horizontal)
		{
			int num = ؿ.Location.X - _ػ.X;
			int num2 = num + _ؼ.X;
			bool flag = false;
			if (Math.Abs(num2) + Ն > __0617.Width - _ؽ.Width)
			{
				__0619.X = -(__0617.Width + Ն - _ؽ.Width);
				flag = true;
			}
			else if (num2 > Ն)
			{
				__0619.X = Ն;
				flag = true;
			}
			else
			{
				__0619.X = num2;
			}
			((Control)this).Invalidate();
			_᪖();
			((Control)this).Update();
			if (flag && ؿ.IsInertia)
			{
				ۺ();
			}
		}
		else
		{
			int num3 = ؿ.Location.Y - _ػ.Y;
			int num4 = num3 + _ؼ.Y;
			bool flag2 = false;
			if (num4 < 0 && Math.Abs(num4) + Ն > __0617.Height - _ؽ.Height)
			{
				__0619.Y = -(__0617.Height + Ն - _ؽ.Height);
				flag2 = true;
			}
			else if (num4 > Ն)
			{
				__0619.Y = Ն;
				flag2 = true;
			}
			else
			{
				__0619.Y = num4;
			}
			((Control)this).Invalidate();
			_᪖();
			((Control)this).Update();
			if (flag2 && ؿ.IsInertia)
			{
				ۺ();
			}
		}
		if (__059D != null && __059D.Value != -__0619.Y)
		{
			__059D.Value = Math.Min(__059D.Maximum, Math.Max(0, -__0619.Y));
		}
		if (__059E != null && __059E.Value != -__0619.X)
		{
			__059E.Value = Math.Min(__059E.Maximum, Math.Max(0, -__0619.X));
		}
		ؿ.Handled = true;
	}
}
