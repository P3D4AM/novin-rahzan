using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar.Metro.Rendering;

namespace DevComponents.DotNetBar.Metro;

[ToolboxBitmap(typeof(MetroShell), "MetroToolbar.ico")]
[ToolboxItem(true)]
[ComVisible(false)]
[Designer("DevComponents.DotNetBar.Design.MetroToolbarDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
public class MetroToolbar : ItemControl
{
	private CancelEventHandler ℏ;

	private EventHandler _1C2B;

	private MetroToolbarContainer _ᐵ;

	private bool __2404;

	private bool __2405 = true;

	private bool __2406;

	private bool __2407;

	private bool __223A;

	private eExpandDirection __2408;

	private bool __2409;

	private Timer __240A;

	private IntPtr __240B = IntPtr.Zero;

	private IntPtr __240C = IntPtr.Zero;

	private bool __240D = true;

	private int __2397 = 150;

	private eExpandDirection __240E;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Browsable(false)]
	public SubItemsCollection Items => _ᐵ.MainItemsContainer.SubItems;

	[DefaultValue(0)]
	[Category("Appearance")]
	[Description("Gets or sets spacing between items.")]
	public int ItemSpacing
	{
		get
		{
			return _ᐵ.MainItemsContainer.ItemSpacing;
		}
		set
		{
			_ᐵ.MainItemsContainer.ItemSpacing = value;
			RecalcLayout();
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Browsable(false)]
	public SubItemsCollection ExtraItems => _ᐵ.ExtraItemsContainer.SubItems;

	[Category("Behavior")]
	[Browsable(false)]
	[Description("Indicates whether toolbar is attempted to be automatically registered with parent MetroTab control so it can participate in Quick Access Toolbar operations")]
	[DefaultValue(true)]
	public bool AutoRegister
	{
		get
		{
			return __2405;
		}
		set
		{
			__2405 = value;
		}
	}

	[Description("Indicates whether control is expanded or not. When control is expanded both main and extra toolbar items are visible.")]
	[Browsable(true)]
	[Category("Layout")]
	[DefaultValue(false)]
	public bool Expanded
	{
		get
		{
			return __223A;
		}
		set
		{
			if (__223A == value)
			{
				return;
			}
			CancelEventArgs cancelEventArgs = new CancelEventArgs();
			OnExpandedChanging(cancelEventArgs);
			if (!cancelEventArgs.Cancel)
			{
				if (base.IsUpdateSuspended || !BarFunctions.IsHandleValid((Control)(object)this))
				{
					__2406 = true;
					__2407 = value;
				}
				else
				{
					__2406 = false;
					__223A = value;
					ᱞ();
				}
			}
		}
	}

	[Description("Indicates whether control is automatically collapsed, Expanded property set to False, if control was expanded and any button on the control was clicked or mouse is clicked elsewhere, parent form has lost input focus or some other control gains input focus.")]
	[DefaultValue(true)]
	[Category("Behavior")]
	public bool AutoCollapse
	{
		get
		{
			return __240D;
		}
		set
		{
			if (value != __240D)
			{
				bool oldValue = __240D;
				__240D = value;
				OnAutoCollapseChanged(oldValue, value);
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
			return __2397;
		}
		set
		{
			if (value != __2397)
			{
				int oldValue = __2397;
				__2397 = value;
				OnAnimationSpeedChanged(oldValue, value);
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
			return __240E;
		}
		set
		{
			if (value != __240E)
			{
				eExpandDirection oldValue = __240E;
				__240E = value;
				OnExpandDirectionChanged(oldValue, value);
			}
		}
	}

	[Category("Appearance")]
	[Description("Indicates whether Expand button is visible")]
	[DefaultValue(true)]
	public bool ExpandButtonVisible
	{
		get
		{
			return _ᐵ.ExpandButtonVisible;
		}
		set
		{
			_ᐵ.ExpandButtonVisible = value;
		}
	}

	[Category("Layout")]
	[DefaultValue(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Description("Indicates whether control height is set automatically.")]
	[Browsable(false)]
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
	public event CancelEventHandler ExpandedChanging
	{
		add
		{
			CancelEventHandler cancelEventHandler = ℏ;
			CancelEventHandler cancelEventHandler2;
			do
			{
				cancelEventHandler2 = cancelEventHandler;
				CancelEventHandler value2 = (CancelEventHandler)Delegate.Combine(cancelEventHandler2, value);
				cancelEventHandler = Interlocked.CompareExchange(ref ℏ, value2, cancelEventHandler2);
			}
			while ((object)cancelEventHandler != cancelEventHandler2);
		}
		remove
		{
			CancelEventHandler cancelEventHandler = ℏ;
			CancelEventHandler cancelEventHandler2;
			do
			{
				cancelEventHandler2 = cancelEventHandler;
				CancelEventHandler value2 = (CancelEventHandler)Delegate.Remove(cancelEventHandler2, value);
				cancelEventHandler = Interlocked.CompareExchange(ref ℏ, value2, cancelEventHandler2);
			}
			while ((object)cancelEventHandler != cancelEventHandler2);
		}
	}

	[Description("Occurs after Expanded property value has changed, i.e. control was expanded or collapsed.")]
	public event EventHandler ExpandedChanged
	{
		add
		{
			EventHandler eventHandler = _1C2B;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _1C2B, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _1C2B;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _1C2B, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	protected virtual void OnExpandedChanging(CancelEventArgs e)
	{
		ℏ?.Invoke(this, e);
	}

	protected virtual void OnExpandedChanged(EventArgs e)
	{
		_1C2B?.Invoke(this, e);
	}

	public MetroToolbar()
	{
		_ᐵ = new MetroToolbarContainer(this);
		_ᐵ.GlobalItem = false;
		_ᐵ.ContainerControl = this;
		_ᐵ.Stretch = true;
		_ᐵ.Displayed = true;
		_ᐵ.Style = eDotNetBarStyle.StyleManagerControlled;
		base.ColorScheme.ߕ = eDotNetBarStyle.StyleManagerControlled;
		_ᐵ.SetOwner(this);
		SetBaseItemContainer(_ᐵ);
		DragDropSupport = true;
		base.ItemAdded += _2222;
		base.ItemRemoved += _2223;
		StyleManager.Register((Control)(object)this);
	}

	protected override void Dispose(bool disposing)
	{
		StyleManager.Unregister((Control)(object)this);
		base.ItemAdded -= _2222;
		base.ItemRemoved -= _2223;
		base.Dispose(disposing);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void StyleManagerStyleChanged(eDotNetBarStyle newStyle)
	{
		_ᐵ.NeedRecalcSize = true;
	}

	protected override void PaintControlBackground(ItemPaintArgs pa)
	{
		MetroRender.Paint(this, pa);
		base.PaintControlBackground(pa);
	}

	private void _2222(object _٠, EventArgs ؿ)
	{
		RecalcLayout();
	}

	private void _2223(object _٠, ItemRemovedEventArgs ؿ)
	{
		RecalcLayout();
	}

	protected override void OnForeColorChanged(EventArgs e)
	{
		_ᐵ.UpdateSystemColors();
		((Control)this).OnForeColorChanged(e);
	}

	protected override void OnParentChanged(EventArgs e)
	{
		if (__2405)
		{
			_240F();
		}
		base.OnParentChanged(e);
	}

	protected override void OnHandleDestroyed(EventArgs e)
	{
		if (__2405)
		{
			_2410();
		}
		base.OnHandleDestroyed(e);
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		base.OnHandleCreated(e);
		if (__2405)
		{
			_240F();
		}
		if (__2406)
		{
			Expanded = __2407;
		}
	}

	private void _240F()
	{
		RegisterToolbar(ᐩ());
	}

	public void RegisterToolbar(MetroShell tab)
	{
		if (!__2404 && tab != null)
		{
			tab.RegisterToolbar(this);
			__2404 = true;
		}
	}

	private void _2410()
	{
		UnregisterToolbar(ᐩ());
	}

	public void UnregisterToolbar(MetroShell tab)
	{
		if (__2404 && tab != null)
		{
			tab.UnregisterToolbar(this);
			__2404 = false;
		}
	}

	private MetroShell ᐩ()
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
		if (!__2409)
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
		if (backgroundStyle != null)
		{
			ᝈ.ᝏ(backgroundStyle);
		}
		if (__223A)
		{
			return _ᐵ.HeightInternal;
		}
		return _ᐵ.MainItemsContainer.HeightInternal;
	}

	private void ᱞ()
	{
		__2409 = true;
		try
		{
			Rectangle bounds = ((Control)this).Bounds;
			ElementStyle backgroundStyle = GetBackgroundStyle();
			int num = 0;
			if (backgroundStyle != null)
			{
				num = ᝈ.ᝏ(backgroundStyle);
			}
			Rectangle rectangle;
			if (!__223A)
			{
				rectangle = ((__2408 != eExpandDirection.Bottom) ? new Rectangle(((Control)this).Left, ((Control)this).Top + (_ᐵ.HeightInternal - _ᐵ.MainItemsContainer.HeightInternal), ((Control)this).Width, _ᐵ.MainItemsContainer.HeightInternal + num) : new Rectangle(((Control)this).Left, ((Control)this).Top, ((Control)this).Width, _ᐵ.MainItemsContainer.HeightInternal + num));
			}
			else
			{
				eExpandDirection eExpandDirection2 = _2416();
				rectangle = ((eExpandDirection2 != eExpandDirection.Bottom) ? new Rectangle(((Control)this).Left, ((Control)this).Top - (_ᐵ.HeightInternal - _ᐵ.MainItemsContainer.HeightInternal), ((Control)this).Width, _ᐵ.HeightInternal + num) : new Rectangle(((Control)this).Left, ((Control)this).Top, ((Control)this).Width, _ᐵ.HeightInternal + num));
				__2408 = eExpandDirection2;
			}
			if (__2397 <= 0 || ((Component)(object)this).DesignMode)
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
				BarFunctions.AnimateControl((Control)(object)this, show: true, __2397, bounds, rectangle);
			}
			OnExpandedChanged(EventArgs.Empty);
			if (__223A)
			{
				if (__240D && !((Component)(object)this).DesignMode)
				{
					_2411();
				}
			}
			else if (__240D)
			{
				_2413();
			}
		}
		finally
		{
			__2409 = false;
		}
	}

	private void _2411()
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Expected O, but got Unknown
		if (__240A == null)
		{
			__240A = new Timer();
			__240A.Interval = 100;
			__240A.Tick += ણ;
			__240B = _089F.GetForegroundWindow();
			__240C = _089F.GetActiveWindow();
			__240A.Start();
		}
	}

	private void ણ(object _٠, EventArgs ؿ)
	{
		if (__240A == null)
		{
			return;
		}
		IntPtr foregroundWindow = _089F.GetForegroundWindow();
		IntPtr activeWindow = _089F.GetActiveWindow();
		if (!(foregroundWindow != __240B) && !(activeWindow != __240C))
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
		__240A.Stop();
		_2412();
	}

	private void _2412()
	{
		if (Expanded)
		{
			_2415(eEventSource.Code);
		}
		ReleaseActiveWindowTimer();
	}

	private void _2413()
	{
		if (__240A != null)
		{
			Timer val = __240A;
			__240A = null;
			val.Stop();
			val.Tick -= ણ;
			((Component)(object)val).Dispose();
		}
	}

	protected override void OnItemMouseUp(BaseItem item, MouseEventArgs e)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Invalid comparison between Unknown and I4
		base.OnItemMouseUp(item, e);
		if ((int)e.Button == 2097152 && item.CanCustomize)
		{
			_2414()?.ᐆ(item, ᐇ: true);
		}
	}

	private MetroShell _2414()
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
		if (__223A && __240D && !item.SystemItem && (!(item is PopupItem) || ((PopupItem)item).AutoCollapseOnClick))
		{
			_2415(eEventSource.Code);
		}
	}

	protected override bool OnSysMouseDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
	{
		if (!((Component)(object)this).DesignMode && __240D && Expanded && !((Control)this).ClientRectangle.Contains(((Control)this).PointToClient(Control.MousePosition)))
		{
			_2415(eEventSource.Mouse);
		}
		return base.OnSysMouseDown(hWnd, wParam, lParam);
	}

	private void _2415(eEventSource ᎂ)
	{
		if (Expanded)
		{
			Expanded = false;
		}
	}

	private eExpandDirection _2416()
	{
		if (__240E != 0)
		{
			return __240E;
		}
		if (((Control)this).Parent == null)
		{
			return eExpandDirection.Bottom;
		}
		Rectangle clientRectangle = ((Control)this).Parent.ClientRectangle;
		if (((Control)this).Top + _ᐵ.HeightInternal > clientRectangle.Bottom)
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
