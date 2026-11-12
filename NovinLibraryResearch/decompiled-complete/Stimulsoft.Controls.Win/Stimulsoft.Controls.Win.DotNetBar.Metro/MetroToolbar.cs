using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Metro.Rendering;

namespace Stimulsoft.Controls.Win.DotNetBar.Metro;

public class MetroToolbar : ItemControl
{
	private MetroToolbarContainer _ItemContainer;

	private bool _ToolbarRegistered;

	private bool _AutoRegister = true;

	private bool _SetExpandedDelayed;

	private bool _ExpandedDelayed;

	private bool _Expanded;

	private eExpandDirection _LastExpandDirection;

	private bool _ChangingExpanded;

	private Timer _ActiveWindowTimer;

	private IntPtr _ForegroundWindow = IntPtr.Zero;

	private IntPtr _ActiveWindow = IntPtr.Zero;

	private bool _AutoCollapse = true;

	private int _AnimationSpeed = 150;

	private eExpandDirection _ExpandDirection;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Browsable(false)]
	public SubItemsCollection Items => _ItemContainer.MainItemsContainer.SubItems;

	[DefaultValue(0)]
	[Category("Appearance")]
	[Description("Gets or sets spacing between items.")]
	public int ItemSpacing
	{
		get
		{
			return _ItemContainer.MainItemsContainer.ItemSpacing;
		}
		set
		{
			_ItemContainer.MainItemsContainer.ItemSpacing = value;
			RecalcLayout();
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Browsable(false)]
	public SubItemsCollection ExtraItems => _ItemContainer.ExtraItemsContainer.SubItems;

	[DefaultValue(true)]
	[Browsable(false)]
	[Category("Behavior")]
	[Description("Indicates whether toolbar is attempted to be automatically registered with parent MetroTab control so it can participate in Quick Access Toolbar operations")]
	public bool AutoRegister
	{
		get
		{
			return _AutoRegister;
		}
		set
		{
			_AutoRegister = value;
		}
	}

	[DefaultValue(false)]
	[Browsable(true)]
	[Category("Layout")]
	[Description("Indicates whether control is expanded or not. When control is expanded both main and extra toolbar items are visible.")]
	public bool Expanded
	{
		get
		{
			return _Expanded;
		}
		set
		{
			if (_Expanded == value)
			{
				return;
			}
			CancelEventArgs cancelEventArgs = new CancelEventArgs();
			OnExpandedChanging(cancelEventArgs);
			if (!cancelEventArgs.Cancel)
			{
				if (base.IsUpdateSuspended || !BarFunctions.IsHandleValid((Control)(object)this))
				{
					_SetExpandedDelayed = true;
					_ExpandedDelayed = value;
				}
				else
				{
					_SetExpandedDelayed = false;
					_Expanded = value;
					OnExpandedChanged();
				}
			}
		}
	}

	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether control is automatically collapsed, Expanded property set to False, if control was expanded and any button on the control was clicked or mouse is clicked elsewhere, parent form has lost input focus or some other control gains input focus.")]
	public bool AutoCollapse
	{
		get
		{
			return _AutoCollapse;
		}
		set
		{
			if (value != _AutoCollapse)
			{
				bool autoCollapse = _AutoCollapse;
				_AutoCollapse = value;
				OnAutoCollapseChanged(autoCollapse, value);
			}
		}
	}

	[DefaultValue(150)]
	[Category("Behavior")]
	[Description("Indicates animation speed duration in milliseconds. Default value is 150 milliseconds. Set to zero, 0 to disable animation.")]
	public int AnimationSpeed
	{
		get
		{
			return _AnimationSpeed;
		}
		set
		{
			if (value != _AnimationSpeed)
			{
				int animationSpeed = _AnimationSpeed;
				_AnimationSpeed = value;
				OnAnimationSpeedChanged(animationSpeed, value);
			}
		}
	}

	[DefaultValue(eExpandDirection.Auto)]
	[Category("Behavior")]
	[Description("Indicates expand direction for the toolbar")]
	public eExpandDirection ExpandDirection
	{
		get
		{
			return _ExpandDirection;
		}
		set
		{
			if (value != _ExpandDirection)
			{
				eExpandDirection expandDirection = _ExpandDirection;
				_ExpandDirection = value;
				OnExpandDirectionChanged(expandDirection, value);
			}
		}
	}

	[Browsable(false)]
	[Category("Layout")]
	[Description("Indicates whether control height is set automatically.")]
	[DefaultValue(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override bool AutoSize
	{
		get
		{
			return base.AutoSize;
		}
		set
		{
			base.AutoSize = value;
		}
	}

	[Description("Occurs before Expanded property has changed, i.e. control expanded or collapsed and allows you to cancel action by setting Cancel=true on event arguments.")]
	public event CancelEventHandler ExpandedChanging;

	[Description("Occurs after Expanded property value has changed, i.e. control was expanded or collapsed.")]
	public event EventHandler ExpandedChanged;

	protected virtual void OnExpandedChanging(CancelEventArgs e)
	{
		this.ExpandedChanging?.Invoke(this, e);
	}

	protected virtual void OnExpandedChanged(EventArgs e)
	{
		this.ExpandedChanged?.Invoke(this, e);
	}

	public MetroToolbar()
	{
		_ItemContainer = new MetroToolbarContainer(this);
		_ItemContainer.GlobalItem = false;
		_ItemContainer.ContainerControl = this;
		_ItemContainer.Stretch = true;
		_ItemContainer.Displayed = true;
		_ItemContainer.Style = eDotNetBarStyle.StyleManagerControlled;
		base.ColorScheme.Style = eDotNetBarStyle.StyleManagerControlled;
		_ItemContainer.SetOwner(this);
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

	protected override void PaintControlBackground(ItemPaintArgs pa)
	{
		MetroRender.Paint(this, pa);
		base.PaintControlBackground(pa);
	}

	private void ChildItemAdded(object sender, EventArgs e)
	{
		RecalcLayout();
	}

	private void ChildItemRemoved(object sender, ItemRemovedEventArgs e)
	{
		RecalcLayout();
	}

	protected override void OnForeColorChanged(EventArgs e)
	{
		_ItemContainer.UpdateSystemColors();
		((Control)this).OnForeColorChanged(e);
	}

	protected override void OnParentChanged(EventArgs e)
	{
		if (_AutoRegister)
		{
			RegisterToolbar();
		}
		base.OnParentChanged(e);
	}

	protected override void OnHandleDestroyed(EventArgs e)
	{
		if (_AutoRegister)
		{
			UnregisterToolbar();
		}
		base.OnHandleDestroyed(e);
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		base.OnHandleCreated(e);
		if (_AutoRegister)
		{
			RegisterToolbar();
		}
		if (_SetExpandedDelayed)
		{
			Expanded = _ExpandedDelayed;
		}
	}

	private void RegisterToolbar()
	{
		RegisterToolbar(GetMetroTab());
	}

	public void RegisterToolbar(MetroShell tab)
	{
		if (!_ToolbarRegistered && tab != null)
		{
			tab.RegisterToolbar(this);
			_ToolbarRegistered = true;
		}
	}

	private void UnregisterToolbar()
	{
		UnregisterToolbar(GetMetroTab());
	}

	public void UnregisterToolbar(MetroShell tab)
	{
		if (_ToolbarRegistered && tab != null)
		{
			tab.UnregisterToolbar(this);
			_ToolbarRegistered = false;
		}
	}

	private MetroShell GetMetroTab()
	{
		if (!(((Control)this).FindForm() is MetroAppForm metroAppForm))
		{
			for (Control parent = ((Control)this).Parent; parent != null; parent = parent.Parent)
			{
				if (parent is MetroShell)
				{
					return (MetroShell)(object)parent;
				}
			}
			return null;
		}
		return metroAppForm.MetroShell;
	}

	protected override void RecalcSize()
	{
		if (!_ChangingExpanded)
		{
			base.RecalcSize();
			int autoSizeHeight = GetAutoSizeHeight();
			if (autoSizeHeight != ((Control)this).Height)
			{
				((Control)this).Height = autoSizeHeight;
			}
		}
	}

	public override int GetAutoSizeHeight()
	{
		ElementStyle backgroundStyle = GetBackgroundStyle();
		int num = 0;
		if (backgroundStyle != null)
		{
			num = ElementStyleLayout.VerticalStyleWhiteSpace(backgroundStyle);
		}
		if (_Expanded)
		{
			return _ItemContainer.HeightInternal;
		}
		return _ItemContainer.MainItemsContainer.HeightInternal;
	}

	private void OnExpandedChanged()
	{
		_ChangingExpanded = true;
		try
		{
			Rectangle bounds = ((Control)this).Bounds;
			ElementStyle backgroundStyle = GetBackgroundStyle();
			int num = 0;
			if (backgroundStyle != null)
			{
				num = ElementStyleLayout.VerticalStyleWhiteSpace(backgroundStyle);
			}
			Rectangle rectangle;
			if (!_Expanded)
			{
				rectangle = ((_LastExpandDirection != eExpandDirection.Bottom) ? new Rectangle(((Control)this).Left, ((Control)this).Top + (_ItemContainer.HeightInternal - _ItemContainer.MainItemsContainer.HeightInternal), ((Control)this).Width, _ItemContainer.MainItemsContainer.HeightInternal + num) : new Rectangle(((Control)this).Left, ((Control)this).Top, ((Control)this).Width, _ItemContainer.MainItemsContainer.HeightInternal + num));
			}
			else
			{
				eExpandDirection expandDirection = GetExpandDirection();
				rectangle = ((expandDirection != eExpandDirection.Bottom) ? new Rectangle(((Control)this).Left, ((Control)this).Top - (_ItemContainer.HeightInternal - _ItemContainer.MainItemsContainer.HeightInternal), ((Control)this).Width, _ItemContainer.HeightInternal + num) : new Rectangle(((Control)this).Left, ((Control)this).Top, ((Control)this).Width, _ItemContainer.HeightInternal + num));
				_LastExpandDirection = expandDirection;
			}
			if (_AnimationSpeed <= 0 || ((Component)(object)this).DesignMode)
			{
				if (((Component)(object)this).DesignMode)
				{
					TypeDescriptor.GetProperties(this)["Bounds"].SetValue(this, rectangle);
				}
				else
				{
					((Control)this).Bounds = rectangle;
				}
			}
			else
			{
				BarFunctions.AnimateControl((Control)(object)this, show: true, _AnimationSpeed, bounds, rectangle);
			}
			OnExpandedChanged(EventArgs.Empty);
			if (_Expanded)
			{
				if (_AutoCollapse && !((Component)(object)this).DesignMode)
				{
					TrackAutoCollapse();
				}
			}
			else if (_AutoCollapse)
			{
				StopAutoCollapseTracking();
			}
		}
		finally
		{
			_ChangingExpanded = false;
		}
	}

	private void TrackAutoCollapse()
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Expected O, but got Unknown
		if (_ActiveWindowTimer == null)
		{
			_ActiveWindowTimer = new Timer();
			_ActiveWindowTimer.Interval = 100;
			_ActiveWindowTimer.Tick += ActiveWindowTimer_Tick;
			_ForegroundWindow = NativeFunctions.GetForegroundWindow();
			_ActiveWindow = NativeFunctions.GetActiveWindow();
			_ActiveWindowTimer.Start();
		}
	}

	private void ActiveWindowTimer_Tick(object sender, EventArgs e)
	{
		if (_ActiveWindowTimer == null)
		{
			return;
		}
		IntPtr foregroundWindow = NativeFunctions.GetForegroundWindow();
		IntPtr activeWindow = NativeFunctions.GetActiveWindow();
		if (!(foregroundWindow != _ForegroundWindow) && !(activeWindow != _ActiveWindow))
		{
			return;
		}
		if (activeWindow != IntPtr.Zero)
		{
			Control val = Control.FromHandle(activeWindow);
			if (val is PopupContainer || val is PopupContainerControl || val is Balloon)
			{
				return;
			}
		}
		_ActiveWindowTimer.Stop();
		OnActiveWindowTrackingChanged();
	}

	private void OnActiveWindowTrackingChanged()
	{
		if (Expanded)
		{
			CollapseToolbar(eEventSource.Code);
		}
		ReleaseActiveWindowTimer();
	}

	private void StopAutoCollapseTracking()
	{
		if (_ActiveWindowTimer != null)
		{
			Timer activeWindowTimer = _ActiveWindowTimer;
			_ActiveWindowTimer = null;
			activeWindowTimer.Stop();
			activeWindowTimer.Tick -= ActiveWindowTimer_Tick;
			((Component)(object)activeWindowTimer).Dispose();
		}
	}

	protected override void OnItemMouseUp(BaseItem item, MouseEventArgs e)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Invalid comparison between Unknown and I4
		base.OnItemMouseUp(item, e);
		if ((int)e.Button == 2097152 && item.CanCustomize)
		{
			GetAppMetroTab()?.ShowCustomizeContextMenu(item, isTabStrip: true);
		}
	}

	private MetroShell GetAppMetroTab()
	{
		if (((Control)this).FindForm() is MetroAppForm metroAppForm)
		{
			return metroAppForm.MetroShell;
		}
		return null;
	}

	protected override void OnItemClick(BaseItem item)
	{
		base.OnItemClick(item);
		if (_Expanded && _AutoCollapse && !item.SystemItem && (!(item is PopupItem) || ((PopupItem)item).AutoCollapseOnClick))
		{
			CollapseToolbar(eEventSource.Code);
		}
	}

	protected override bool OnSysMouseDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
	{
		if (!((Component)(object)this).DesignMode && _AutoCollapse && Expanded && !((Control)this).ClientRectangle.Contains(((Control)this).PointToClient(Control.MousePosition)))
		{
			CollapseToolbar(eEventSource.Mouse);
		}
		return base.OnSysMouseDown(hWnd, wParam, lParam);
	}

	private void CollapseToolbar(eEventSource source)
	{
		if (Expanded)
		{
			Expanded = false;
		}
	}

	private eExpandDirection GetExpandDirection()
	{
		if (_ExpandDirection != 0)
		{
			return _ExpandDirection;
		}
		if (((Control)this).Parent == null)
		{
			return eExpandDirection.Bottom;
		}
		Rectangle clientRectangle = ((Control)this).Parent.ClientRectangle;
		if (((Control)this).Top + _ItemContainer.HeightInternal > clientRectangle.Bottom)
		{
			return eExpandDirection.Top;
		}
		return eExpandDirection.Bottom;
	}

	protected virtual void OnAutoCollapseChanged(bool oldValue, bool newValue)
	{
	}

	protected virtual void OnAnimationSpeedChanged(int oldValue, int newValue)
	{
	}

	protected virtual void OnExpandDirectionChanged(eExpandDirection oldValue, eExpandDirection newValue)
	{
	}
}
