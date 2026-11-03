using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar;

public abstract class PopupItemControl : Control, IOwner, IOwnerMenuSupport, IOwnerLocalize, IAccessibilitySupport, IRenderingSupport, ል, ሎ
{
	private EventHandler _09BE;

	private EventHandler _09BF;

	private EventHandler _09C0;

	private EventHandler _09C1;

	private EventHandler _09C2;

	private DotNetBarManager.LocalizeStringEventHandler _09D4;

	private PopupItem ᒟ;

	private ColorScheme լ;

	private bool ե;

	private bool _২;

	private bool _৪;

	private bool _0A02;

	private Timer _0A04;

	private IntPtr ਅ = IntPtr.Zero;

	private IntPtr ਆ = IntPtr.Zero;

	private Hashtable _09DA = new Hashtable();

	private BaseItem _09D8;

	private BaseItem _09D9;

	private ImageList գ;

	private ImageList _09DB;

	private ImageList ড়;

	private bool ਖ = true;

	private _0A79 ৰ;

	private _0A7A ৱ;

	private _0A7B _09F2;

	private _0A7C _09F3;

	private _0A7D _09F4;

	private _0A7E _09F5;

	private _0A7F _09F6;

	private _0A80 _09F7;

	private BaseItem ਈ;

	private BaseRenderer ਛ;

	private BaseRenderer ਜ;

	private eRenderMode ԁ = eRenderMode.Global;

	private bool _0A0B;

	private ᒺ _0A0E;

	private ArrayList ਏ = new ArrayList();

	private _20E7 ਐ;

	[DefaultValue(eDotNetBarStyle.Office2007)]
	[Category("Appearance")]
	[Description("Specifies the visual style of the button.")]
	public virtual eDotNetBarStyle Style
	{
		get
		{
			return InternalStyle;
		}
		set
		{
			InternalStyle = value;
		}
	}

	protected eDotNetBarStyle InternalStyle
	{
		get
		{
			return ᒟ.Style;
		}
		set
		{
			լ.ߕ = value;
			ᒟ.Style = value;
			OnDotNetBarStyleChanged();
			RecalcLayout();
		}
	}

	[Browsable(false)]
	[DefaultValue(eRenderMode.Global)]
	public eRenderMode RenderMode
	{
		get
		{
			return ԁ;
		}
		set
		{
			if (ԁ != value)
			{
				ԁ = value;
				((Control)this).Invalidate(true);
			}
		}
	}

	[DefaultValue(null)]
	[Browsable(false)]
	public BaseRenderer Renderer
	{
		get
		{
			return ਜ;
		}
		set
		{
			ਜ = value;
		}
	}

	[DefaultValue(false)]
	[Browsable(true)]
	[Category("Appearance")]
	[Description("Gets or sets whether anti-aliasing is used while painting.")]
	public bool AntiAlias
	{
		get
		{
			return ե;
		}
		set
		{
			if (ե != value)
			{
				ե = value;
				InvalidateAutoSize();
				((Control)this).Invalidate();
			}
		}
	}

	[DevCoBrowsable(false)]
	[Description("Gets or sets Bar Color Scheme.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Category("Appearance")]
	[Browsable(false)]
	public ColorScheme ColorScheme
	{
		get
		{
			return լ;
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentException("NULL is not a valid value for this property.");
			}
			լ = value;
			if (((Control)this).Visible)
			{
				((Control)this).Invalidate();
			}
		}
	}

	[DefaultValue(false)]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Description("Specifies whether button is drawn using Themes when running on OS that supports themes like Windows XP.")]
	[Category("Appearance")]
	public virtual bool ThemeAware
	{
		get
		{
			return ᒟ.ThemeAware;
		}
		set
		{
			ᒟ.ThemeAware = value;
			RecalcLayout();
		}
	}

	protected bool IsThemed
	{
		get
		{
			if (ThemeAware && ᒟ.EffectiveStyle != eDotNetBarStyle.Office2000 && BarFunctions.ThemedOS && _0EEE._0EF0)
			{
				return true;
			}
			return false;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public BaseItem InternalItem => ᒟ;

	[Browsable(false)]
	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Gets or sets whether hooks are used for internal DotNetBar system functionality. Using hooks is recommended only if DotNetBar is used in hybrid environments like Visual Studio designers or IE.")]
	public bool UseHook
	{
		get
		{
			return _0A0B;
		}
		set
		{
			_0A0B = value;
		}
	}

	bool IOwnerMenuSupport.ঢ
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	bool IOwnerMenuSupport.ণ
	{
		get
		{
			return true;
		}
		set
		{
		}
	}

	bool IOwnerMenuSupport.ত
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	bool IOwnerMenuSupport.থ => true;

	eMenuDropShadow IOwnerMenuSupport.দ
	{
		get
		{
			return eMenuDropShadow.SystemDefault;
		}
		set
		{
		}
	}

	ePopupAnimation IOwnerMenuSupport.ধ
	{
		get
		{
			return ePopupAnimation.SystemDefault;
		}
		set
		{
		}
	}

	bool IOwnerMenuSupport.ন
	{
		get
		{
			return true;
		}
		set
		{
		}
	}

	internal bool _09A9
	{
		get
		{
			return _0A02;
		}
		set
		{
			if (_0A02 != value)
			{
				_0A02 = value;
				if (_0A02)
				{
					SetupActiveWindowTimer();
				}
				else
				{
					ReleaseActiveWindowTimer();
					ClosePopups();
				}
				((Control)this).Invalidate();
			}
		}
	}

	bool ሎ.প
	{
		get
		{
			Form val = ((Control)this).FindForm();
			if (val != null && val.Modal && Form.ActiveForm == val)
			{
				return true;
			}
			return false;
		}
	}

	protected virtual bool EatMenuFilterKeyDown => true;

	protected bool IsParentFormActive
	{
		get
		{
			Form val = ((Control)this).FindForm();
			if (val == null)
			{
				return false;
			}
			if (val.IsMdiChild)
			{
				if (val.MdiParent == null)
				{
					return false;
				}
				if (val.MdiParent.ActiveMdiChild != val)
				{
					return false;
				}
			}
			else if (val != Form.ActiveForm)
			{
				return false;
			}
			return true;
		}
	}

	_0A79 ል.ফ
	{
		get
		{
			if (ৰ == null)
			{
				ৰ = new _0A79((Control)(object)this);
			}
			return ৰ;
		}
	}

	_0A7A ል.ব
	{
		get
		{
			if (ৱ == null)
			{
				ৱ = new _0A7A((Control)(object)this);
			}
			return ৱ;
		}
	}

	_0A7B ል.ভ
	{
		get
		{
			if (_09F2 == null)
			{
				_09F2 = new _0A7B((Control)(object)this);
			}
			return _09F2;
		}
	}

	_0A7C ል.ম
	{
		get
		{
			if (_09F3 == null)
			{
				_09F3 = new _0A7C((Control)(object)this);
			}
			return _09F3;
		}
	}

	_0A7D ል.য
	{
		get
		{
			if (_09F4 == null)
			{
				_09F4 = new _0A7D((Control)(object)this);
			}
			return _09F4;
		}
	}

	_0A7E ል.র
	{
		get
		{
			if (_09F5 == null)
			{
				_09F5 = new _0A7E((Control)(object)this);
			}
			return _09F5;
		}
	}

	_0A7F ል._09B1
	{
		get
		{
			if (_09F6 == null)
			{
				_09F6 = new _0A7F((Control)(object)this);
			}
			return _09F6;
		}
	}

	_0A80 ል.ল
	{
		get
		{
			if (_09F7 == null)
			{
				_09F7 = new _0A80((Control)(object)this);
			}
			return _09F7;
		}
	}

	Form IOwner.ছ
	{
		get
		{
			return ((Control)this).FindForm();
		}
		set
		{
		}
	}

	bool IOwner.জ => ఛ();

	Form IOwner.ঝ
	{
		get
		{
			Form val = ((Control)this).FindForm();
			if (val == null)
			{
				return null;
			}
			if (val.IsMdiContainer)
			{
				return val.ActiveMdiChild;
			}
			return null;
		}
	}

	bool IOwner.ᇢ
	{
		get
		{
			return true;
		}
		set
		{
		}
	}

	bool IOwner.ঞ
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	bool IOwner.ଜ => false;

	BaseItem IOwner.ଛ => null;

	[Browsable(false)]
	[DefaultValue(null)]
	[Category("Data")]
	[Description("ImageList for images used on Items. Images specified here will always be used on menu-items and are by default used on all Bars.")]
	public ImageList Images
	{
		get
		{
			return գ;
		}
		set
		{
			if (գ != null)
			{
				((Component)(object)գ).Disposed -= ٶ;
			}
			գ = value;
			if (գ != null)
			{
				((Component)(object)գ).Disposed += ٶ;
			}
		}
	}

	[DefaultValue(null)]
	[Category("Data")]
	[Browsable(false)]
	[Description("ImageList for medium-sized images used on Items.")]
	public ImageList ImagesMedium
	{
		get
		{
			return _09DB;
		}
		set
		{
			if (_09DB != null)
			{
				((Component)(object)_09DB).Disposed -= ٶ;
			}
			_09DB = value;
			if (_09DB != null)
			{
				((Component)(object)_09DB).Disposed += ٶ;
			}
		}
	}

	[Browsable(false)]
	[Description("ImageList for large-sized images used on Items.")]
	[Category("Data")]
	[DefaultValue(null)]
	public ImageList ImagesLarge
	{
		get
		{
			return ড়;
		}
		set
		{
			if (ড় != null)
			{
				((Component)(object)ড়).Disposed -= ٶ;
			}
			ড় = value;
			if (ড় != null)
			{
				((Component)(object)ড়).Disposed += ٶ;
			}
		}
	}

	bool IOwner.ᒞ
	{
		get
		{
			return true;
		}
		set
		{
		}
	}

	bool IOwner.ଚ
	{
		get
		{
			return true;
		}
		set
		{
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[DefaultValue(true)]
	[Description("Gets or sets whether gray-scale algorithm is used to create automatic gray-scale images.")]
	public virtual bool DisabledImagesGrayScale
	{
		get
		{
			return ਖ;
		}
		set
		{
			ਖ = value;
		}
	}

	BaseItem IAccessibilitySupport._09B3
	{
		get
		{
			return ਈ;
		}
		set
		{
			ਈ = value;
		}
	}

	[Description("Occurs when popup of type container is loading.")]
	public event EventHandler PopupContainerLoad
	{
		add
		{
			EventHandler eventHandler = _09BE;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _09BE, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _09BE;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _09BE, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when popup of type container is unloading.")]
	public event EventHandler PopupContainerUnload
	{
		add
		{
			EventHandler eventHandler = _09BF;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _09BF, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _09BF;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _09BF, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when popup item is about to open.")]
	public event EventHandler PopupOpen
	{
		add
		{
			EventHandler eventHandler = _09C0;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _09C0, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _09C0;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _09C0, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when popup item is closing.")]
	public event EventHandler PopupClose
	{
		add
		{
			EventHandler eventHandler = _09C1;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _09C1, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _09C1;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _09C1, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs just before popup window is shown.")]
	public event EventHandler PopupShowing
	{
		add
		{
			EventHandler eventHandler = _09C2;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _09C2, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _09C2;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _09C2, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event DotNetBarManager.LocalizeStringEventHandler LocalizeString
	{
		add
		{
			DotNetBarManager.LocalizeStringEventHandler localizeStringEventHandler = _09D4;
			DotNetBarManager.LocalizeStringEventHandler localizeStringEventHandler2;
			do
			{
				localizeStringEventHandler2 = localizeStringEventHandler;
				DotNetBarManager.LocalizeStringEventHandler value2 = (DotNetBarManager.LocalizeStringEventHandler)Delegate.Combine(localizeStringEventHandler2, value);
				localizeStringEventHandler = Interlocked.CompareExchange(ref _09D4, value2, localizeStringEventHandler2);
			}
			while ((object)localizeStringEventHandler != localizeStringEventHandler2);
		}
		remove
		{
			DotNetBarManager.LocalizeStringEventHandler localizeStringEventHandler = _09D4;
			DotNetBarManager.LocalizeStringEventHandler localizeStringEventHandler2;
			do
			{
				localizeStringEventHandler2 = localizeStringEventHandler;
				DotNetBarManager.LocalizeStringEventHandler value2 = (DotNetBarManager.LocalizeStringEventHandler)Delegate.Remove(localizeStringEventHandler2, value);
				localizeStringEventHandler = Interlocked.CompareExchange(ref _09D4, value2, localizeStringEventHandler2);
			}
			while ((object)localizeStringEventHandler != localizeStringEventHandler2);
		}
	}

	public PopupItemControl()
	{
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		ᣓ._1920();
		if (!ColorFunctions._0893)
		{
			_089F.ळ();
			_089F.ऴ();
			ColorFunctions.LoadColors();
		}
		ᒟ = CreatePopupItem();
		ᒟ.GlobalItem = false;
		ᒟ.ContainerControl = this;
		ᒟ.Style = eDotNetBarStyle.Office2007;
		ᒟ.SetOwner(this);
		լ = new ColorScheme(ᒟ.EffectiveStyle);
		((Control)this).SetStyle((ControlStyles)2, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)4, true);
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle(DisplayHelp.DoubleBufferFlag, true);
		((Control)this).SetStyle((ControlStyles)2048, true);
		((Control)this).SetStyle((ControlStyles)1, false);
		((Control)this).SetStyle((ControlStyles)512, true);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && ᒟ != null)
		{
			ᒟ.Dispose();
		}
		((Control)this).Dispose(disposing);
	}

	protected abstract PopupItem CreatePopupItem();

	internal void ਟ(AccessibleEvents ਠ, int ਡ)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		((Control)this).AccessibilityNotifyClients(ਠ, ਡ);
	}

	protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		if (Dpi.RecordScalePerControl)
		{
			Dpi.SetScaling(factor);
		}
		((Control)this).ScaleControl(factor, specified);
	}

	protected override void OnBindingContextChanged(EventArgs e)
	{
		((Control)this).OnBindingContextChanged(e);
		if (ᒟ != null)
		{
			ᒟ.UpdateBindings();
		}
	}

	protected override void OnFontChanged(EventArgs e)
	{
		if (ᒟ != null)
		{
			BarUtilities.ડ(ᒟ);
		}
		((Control)this).OnFontChanged(e);
		RecalcLayout();
	}

	protected override void OnEnabledChanged(EventArgs e)
	{
		if (ᒟ != null)
		{
			ᒟ.Enabled = ((Control)this).Enabled;
		}
		((Control)this).OnEnabledChanged(e);
	}

	protected internal virtual ColorScheme GetColorScheme()
	{
		if (BarFunctions.IsOffice2007Style(Style))
		{
			BaseRenderer renderer = GetRenderer();
			if (renderer is Office2007Renderer)
			{
				return ((Office2007Renderer)renderer).ColorTable.LegacyColors;
			}
		}
		return լ;
	}

	public void RecalcLayout()
	{
		RecalcSize();
		((Control)this).Invalidate();
	}

	protected abstract void RecalcSize();

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void SetDesignMode(bool value)
	{
		if (ᒟ != null)
		{
			ᒟ.SetDesignMode(value);
		}
	}

	protected virtual void OnDotNetBarStyleChanged()
	{
	}

	public Graphics CreateGraphics()
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		Graphics val = ((Control)this).CreateGraphics();
		if (ե)
		{
			val.SmoothingMode = (SmoothingMode)4;
			if (!SystemInformation.IsFontSmoothingEnabled)
			{
				val.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
			}
		}
		return val;
	}

	internal virtual ItemPaintArgs _0A92(Graphics _٥)
	{
		ItemPaintArgs itemPaintArgs = new ItemPaintArgs(this, (Control)(object)this, _٥, GetColorScheme());
		itemPaintArgs.ໝ = GetRenderer();
		itemPaintArgs.ButtonStringFormat &= ~(itemPaintArgs.ButtonStringFormat & eTextFormat.SingleLine);
		itemPaintArgs.ButtonStringFormat |= eTextFormat.EndEllipsis | eTextFormat.WordBreak;
		return itemPaintArgs;
	}

	public virtual BaseRenderer GetRenderer()
	{
		if (ԁ == eRenderMode.Global && GlobalManager.Renderer != null)
		{
			return GlobalManager.Renderer;
		}
		if (ԁ == eRenderMode.Custom && ਜ != null)
		{
			return ਜ;
		}
		if (ਛ == null && BarFunctions.IsOffice2007Style(Style))
		{
			ਛ = new Office2007Renderer();
		}
		return ਜ;
	}

	protected virtual void InvalidateAutoSize()
	{
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	private bool ShouldSerializeColorScheme()
	{
		return լ.SchemeChanged;
	}

	private bool ఛ()
	{
		return ((Component)this).DesignMode;
	}

	private void _0A3C(PopupItem _0A3D)
	{
		_09A9 = true;
		if (ਏ.Contains(_0A3D))
		{
			return;
		}
		if (!ఛ() && !_0A0B)
		{
			if (!_২)
			{
				_1BE6._1BEA(this);
				_২ = true;
			}
		}
		else if (_0A0E == null)
		{
			_0A0E = new ᒺ(this);
		}
		if (!_৪)
		{
			ੴ();
		}
		if (ਏ.Count == 0)
		{
			PopupManager.RegisterPopup(this);
		}
		ਏ.Add(_0A3D);
		if (_0A3D.GetOwner() != this)
		{
			_0A3D.SetOwner(this);
		}
	}

	void IOwnerMenuSupport.RegisterPopup(PopupItem _0A3D)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ਼
		this._0A3C(_0A3D);
	}

	private void _0A3E(PopupItem _0A3D)
	{
		if (ਏ.Contains(_0A3D))
		{
			ਏ.Remove(_0A3D);
		}
		if (ਏ.Count == 0)
		{
			_0A75();
			if (_0A0E != null)
			{
				_0A0E.Dispose();
				_0A0E = null;
			}
			PopupManager.UnregisterPopup(this);
			_09A9 = false;
		}
	}

	void IOwnerMenuSupport.UnregisterPopup(PopupItem _0A3D)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ਾ
		this._0A3E(_0A3D);
	}

	private bool _0A3F()
	{
		foreach (PopupItem item in ਏ)
		{
			Control popupControl = item.PopupControl;
			if (popupControl != null && popupControl.DisplayRectangle.Contains(Control.MousePosition))
			{
				if (popupControl is MenuPanel)
				{
					((MenuPanel)(object)popupControl).ఆ();
				}
				else if (popupControl is Bar)
				{
					((Bar)(object)popupControl).ఆ();
				}
				return true;
			}
		}
		return false;
	}

	bool IOwnerMenuSupport.RelayMouseHover()
	{
		//ILSpy generated this explicit interface implementation from .override directive in ਿ
		return this._0A3F();
	}

	private void _0A49()
	{
		ClosePopups();
	}

	void IOwnerMenuSupport.ClosePopups()
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੉
		this._0A49();
	}

	private void ClosePopups()
	{
		ArrayList arrayList = new ArrayList(ਏ);
		foreach (PopupItem item in arrayList)
		{
			item.ClosePopup();
		}
	}

	private void _0A40(PopupItem _06EC, EventArgs ؿ)
	{
		if (_09C1 != null)
		{
			_09C1(_06EC, ؿ);
		}
	}

	void IOwnerMenuSupport.InvokePopupClose(PopupItem _06EC, EventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੀ
		this._0A40(_06EC, ؿ);
	}

	private void _0A41(PopupItem _06EC, EventArgs ؿ)
	{
		if (_09BE != null)
		{
			_09BE(_06EC, ؿ);
		}
	}

	void IOwnerMenuSupport.InvokePopupContainerLoad(PopupItem _06EC, EventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੁ
		this._0A41(_06EC, ؿ);
	}

	private void _0A42(PopupItem _06EC, EventArgs ؿ)
	{
		if (_09BF != null)
		{
			_09BF(_06EC, ؿ);
		}
	}

	void IOwnerMenuSupport.InvokePopupContainerUnload(PopupItem _06EC, EventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੂ
		this._0A42(_06EC, ؿ);
	}

	private void _0A43(PopupItem _06EC, PopupOpenEventArgs ؿ)
	{
		if (_09C0 != null)
		{
			_09C0(_06EC, ؿ);
		}
	}

	void IOwnerMenuSupport.InvokePopupOpen(PopupItem _06EC, PopupOpenEventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੃
		this._0A43(_06EC, ؿ);
	}

	private void _0A44(PopupItem _06EC, EventArgs ؿ)
	{
		if (_09C2 != null)
		{
			_09C2(_06EC, ؿ);
		}
	}

	void IOwnerMenuSupport.InvokePopupShowing(PopupItem _06EC, EventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੄
		this._0A44(_06EC, ؿ);
	}

	protected virtual void SetupActiveWindowTimer()
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Expected O, but got Unknown
		if (_0A04 == null)
		{
			_0A04 = new Timer();
			_0A04.Interval = 100;
			_0A04.Tick += ણ;
			ਅ = _089F.GetForegroundWindow();
			ਆ = _089F.GetActiveWindow();
			_0A04.Start();
		}
	}

	private void ણ(object _٠, EventArgs ؿ)
	{
		if (_0A04 == null)
		{
			return;
		}
		IntPtr foregroundWindow = _089F.GetForegroundWindow();
		IntPtr activeWindow = _089F.GetActiveWindow();
		if (!(foregroundWindow != ਅ) && !(activeWindow != ਆ))
		{
			return;
		}
		Control val = Control.FromChildHandle(activeWindow);
		if (val != null)
		{
			do
			{
				if (val is MenuPanel || val is Bar || val is PopupContainer || val is PopupContainerControl)
				{
					return;
				}
				val = val.Parent;
			}
			while (val != null && val.Parent != null);
		}
		_0A04.Stop();
		OnActiveWindowChanged();
	}

	protected virtual void OnActiveWindowChanged()
	{
		if (_09A9)
		{
			_09A9 = false;
		}
	}

	protected virtual void ReleaseActiveWindowTimer()
	{
		if (_0A04 != null)
		{
			Timer val = _0A04;
			_0A04 = null;
			val.Stop();
			val.Tick -= ણ;
			((Component)(object)val).Dispose();
		}
	}

	private bool _0A64(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		return _੨(_0A65, _੦, _੧);
	}

	bool ሎ._੨(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੤
		return this._0A64(_0A65, _੦, _੧);
	}

	protected virtual bool _੨(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		return false;
	}

	private bool _੩(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		return _੪(_0A65, _੦, _੧);
	}

	bool ሎ._੪(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੩
		return this._੩(_0A65, _੦, _੧);
	}

	protected virtual bool _੪(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Expected I4, but got Unknown
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Expected O, but got Unknown
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Expected O, but got Unknown
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		int num = _0EFE._0F11(_੦);
		if (ਏ.Count > 0 && ((BaseItem)ਏ[ਏ.Count - 1]).Parent == null)
		{
			PopupItem popupItem = (PopupItem)ਏ[ਏ.Count - 1];
			Control popupControl = popupItem.PopupControl;
			Control val = Control.FromChildHandle(_0A65);
			if (val != null)
			{
				while (val.Parent != null)
				{
					val = val.Parent;
				}
			}
			bool flag = false;
			if (val != null && popupItem != null)
			{
				flag = popupItem.IsAnyOnHandle(val.Handle);
			}
			bool flag2 = (popupControl != null && val != null && popupControl.Handle == val.Handle) || flag;
			if (!flag)
			{
				Keys val2 = (Keys)_089F.MapVirtualKey((uint)(int)_੦, 2u);
				if ((int)val2 == 0)
				{
					val2 = (Keys)num;
				}
				KeyEventArgs val3 = new KeyEventArgs((Keys)(val2 | Control.ModifierKeys));
				popupItem.InternalKeyDown(val3);
				if (val3.Handled)
				{
					flag2 = false;
				}
				if (!EatMenuFilterKeyDown)
				{
					flag2 = true;
				}
			}
			if (flag2)
			{
				return false;
			}
			return true;
		}
		if (_09A9)
		{
			bool flag3 = true;
			Control val4 = Control.FromChildHandle(_0A65);
			if (val4 != null)
			{
				while (val4.Parent != null)
				{
					val4 = val4.Parent;
				}
				if ((val4 is MenuPanel || val4 is ItemControl || val4 is PopupContainer || val4 is PopupContainerControl) && val4.Handle != _0A65)
				{
					flag3 = false;
				}
			}
			if (flag3)
			{
				Keys val5 = (Keys)_089F.MapVirtualKey((uint)(int)_੦, 2u);
				if ((int)val5 == 0)
				{
					val5 = (Keys)num;
				}
				ᒟ.InternalKeyDown(new KeyEventArgs(val5));
				return true;
			}
		}
		if (!IsParentFormActive)
		{
			return false;
		}
		if (num >= 112 || (int)Control.ModifierKeys != 0 || (_0EFE._0F11(_੧) & 0x1000000000L) != 0 || num == 46 || num == 45)
		{
			int _੬ = Control.ModifierKeys | num;
			return _੫((eShortcut)_੬);
		}
		return false;
	}

	private bool _੫(eShortcut _੬)
	{
		foreach (eShortcut shortcut in ᒟ.Shortcuts)
		{
			if (shortcut == _੬)
			{
				PerformClick();
				return true;
			}
		}
		return BarFunctions.ProcessItemsShortcuts(_੬, _09DA);
	}

	public abstract void PerformClick();

	private _20E7 _੭()
	{
		if (ਐ == null)
		{
			ਐ = new _20E7();
		}
		return ਐ;
	}

	internal void ᒠ()
	{
		_੭()._20ED();
	}

	private bool _੮(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		if (ਏ.Count == 0)
		{
			return false;
		}
		BaseItem[] array = new BaseItem[ਏ.Count];
		ਏ.CopyTo(array);
		for (int num = array.Length - 1; num >= 0; num--)
		{
			PopupItem popupItem = array[num] as PopupItem;
			bool flag = popupItem.IsAnyOnHandle(_0A65);
			if (!flag)
			{
				Control val = Control.FromChildHandle(_0A65);
				if (val != null)
				{
					if (val is MenuPanel)
					{
						flag = true;
					}
					else
					{
						while (val.Parent != null)
						{
							val = val.Parent;
							if (((object)val).GetType().FullName.IndexOf("DropDownHolder") >= 0 || val is MenuPanel || val is PopupContainerControl)
							{
								flag = true;
								break;
							}
						}
					}
					if (!flag)
					{
						flag = popupItem.IsAnyOnHandle(val.Handle);
					}
				}
				else
				{
					string text = _089F.स(_0A65);
					text = text.ToLower();
					if (text.IndexOf("combolbox") >= 0)
					{
						flag = true;
					}
				}
			}
			if (!flag)
			{
				Control val2 = popupItem.PopupControl;
				if (val2 != null)
				{
					while (val2.Parent != null)
					{
						val2 = val2.Parent;
					}
				}
				if (val2 != null && val2.Bounds.Contains(Control.MousePosition))
				{
					flag = true;
				}
			}
			if (flag)
			{
				break;
			}
			if (popupItem.Displayed)
			{
				Point pt = ((Control)this).PointToClient(Control.MousePosition);
				if (popupItem.DisplayRectangle.Contains(pt))
				{
					break;
				}
			}
			if (ఛ())
			{
				_੭()._20EC(popupItem);
			}
			else if (QueryPopupCloseMouseDown())
			{
				popupItem.ClosePopup();
			}
			if (ਏ.Count == 0)
			{
				break;
			}
		}
		if (ਏ.Count == 0)
		{
			_09A9 = false;
		}
		return false;
	}

	bool ሎ.ሏ(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੮
		return this._੮(_0A65, _੦, _੧);
	}

	protected virtual bool QueryPopupCloseMouseDown()
	{
		return true;
	}

	private bool _੯(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		if (ਏ.Count > 0)
		{
			foreach (BaseItem item in ਏ)
			{
				if (item.Parent == null)
				{
					Control popupControl = ((PopupItem)item).PopupControl;
					if (popupControl != null && popupControl.Handle != _0A65 && !item.IsAnyOnHandle(_0A65) && (popupControl.Parent == null || !(popupControl.Parent.Handle != _0A65)))
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	bool ሎ.ሐ(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੯
		return this._੯(_0A65, _੦, _੧);
	}

	private bool _0A70(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		return _0A71(_0A65, _੦, _੧);
	}

	bool ሎ._0A71(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੰ
		return this._0A70(_0A65, _੦, _੧);
	}

	protected virtual bool _0A71(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected I4, but got Unknown
		if (!ఛ())
		{
			int num = _0EFE._0F11(_੦);
			if ((int)Control.ModifierKeys != 0 || (num >= 112 && num <= 123))
			{
				int _੬ = Control.ModifierKeys | num;
				if (_੫((eShortcut)_੬))
				{
					return true;
				}
			}
		}
		return false;
	}

	private bool ੲ(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		return ੳ(_0A65, _੦, _੧);
	}

	bool ሎ.ੳ(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੲ
		return this.ੲ(_0A65, _੦, _੧);
	}

	protected virtual bool ੳ(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		return false;
	}

	private void ੴ()
	{
		if (!_৪)
		{
			_৪ = true;
			Form val = ((Control)this).FindForm();
			if (val == null)
			{
				_৪ = false;
				return;
			}
			((Control)val).Resize += _0A76;
			val.Deactivate += _0A77;
			DotNetBarManager.RegisterParentMsgHandler(this, val);
		}
	}

	private void _0A75()
	{
		if (_৪)
		{
			_৪ = false;
			Form val = ((Control)this).FindForm();
			if (val != null)
			{
				DotNetBarManager.UnRegisterParentMsgHandler(this, val);
				((Control)val).Resize -= _0A76;
				val.Deactivate -= _0A77;
			}
		}
	}

	private void _0A76(object _٠, EventArgs ؿ)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Invalid comparison between Unknown and I4
		Form val = ((Control)this).FindForm();
		if (val != null && (int)val.WindowState == 1)
		{
			((IOwner)this).OnApplicationDeactivate();
		}
	}

	private void _0A77(object _٠, EventArgs ؿ)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Invalid comparison between Unknown and I4
		Form val = ((Control)this).FindForm();
		if (val != null && (int)val.WindowState == 1)
		{
			((IOwner)this).OnApplicationDeactivate();
		}
	}

	protected override void WndProc(ref Message m)
	{
		if (((Message)(ref m)).Msg == 794)
		{
			RefreshThemes();
		}
		else if (((Message)(ref m)).Msg == 1131 && ਈ != null)
		{
			if (!ਈ._ૡ)
			{
				PerformClick();
			}
			ਈ.DoAccesibleDefaultAction();
			ਈ = null;
		}
		((Control)this).WndProc(ref m);
	}

	protected void RefreshThemes()
	{
		if (ৰ != null)
		{
			ৰ.Dispose();
			ৰ = new _0A79((Control)(object)this);
		}
		if (ৱ != null)
		{
			ৱ.Dispose();
			ৱ = new _0A7A((Control)(object)this);
		}
		if (_09F2 != null)
		{
			_09F2.Dispose();
			_09F2 = new _0A7B((Control)(object)this);
		}
		if (_09F3 != null)
		{
			_09F3.Dispose();
			_09F3 = new _0A7C((Control)(object)this);
		}
		if (_09F4 != null)
		{
			_09F4.Dispose();
			_09F4 = new _0A7D((Control)(object)this);
		}
		if (_09F6 != null)
		{
			_09F6.Dispose();
			_09F6 = new _0A7F((Control)(object)this);
		}
		if (_09F5 != null)
		{
			_09F5.Dispose();
			_09F5 = new _0A7E((Control)(object)this);
		}
		if (_09F7 != null)
		{
			_09F7.Dispose();
			_09F7 = new _0A80((Control)(object)this);
		}
	}

	private void _0A78()
	{
		if (ৰ != null)
		{
			ৰ.Dispose();
			ৰ = null;
		}
		if (ৱ != null)
		{
			ৱ.Dispose();
			ৱ = null;
		}
		if (_09F2 != null)
		{
			_09F2.Dispose();
			_09F2 = null;
		}
		if (_09F3 != null)
		{
			_09F3.Dispose();
			_09F3 = null;
		}
		if (_09F4 != null)
		{
			_09F4.Dispose();
			_09F4 = null;
		}
		if (_09F6 != null)
		{
			_09F6.Dispose();
			_09F6 = null;
		}
		if (_09F5 != null)
		{
			_09F5.Dispose();
			_09F5 = null;
		}
		if (_09F7 != null)
		{
			_09F7.Dispose();
			_09F7 = null;
		}
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		((Control)this).OnHandleCreated(e);
		if (!_২ && !((Component)this).DesignMode)
		{
			_1BE6._1BEA(this);
			_২ = true;
		}
		if (((Control)this).AutoSize)
		{
			AdjustSize();
		}
		RecalcLayout();
	}

	protected virtual void AdjustSize()
	{
	}

	protected override void OnHandleDestroyed(EventArgs e)
	{
		_0A78();
		_0A75();
		((Control)this).OnHandleDestroyed(e);
		if (_২)
		{
			_1BE6._1BEB(this);
			_২ = false;
		}
	}

	private ArrayList ᒡ(string ᒢ)
	{
		ArrayList arrayList = new ArrayList(15);
		BarFunctions.GetSubItemsByName(ᒟ, ᒢ, arrayList);
		return arrayList;
	}

	ArrayList IOwner.GetItems(string ᒢ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ᒡ
		return this.ᒡ(ᒢ);
	}

	private ArrayList ᒡ(string ᒢ, Type _0962)
	{
		ArrayList arrayList = new ArrayList(15);
		BarFunctions.GetSubItemsByNameAndType(ᒟ, ᒢ, arrayList, _0962);
		return arrayList;
	}

	ArrayList IOwner.GetItems(string ᒢ, Type _0962)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ᒡ
		return this.ᒡ(ᒢ, _0962);
	}

	private ArrayList ᒡ(string ᒢ, Type _0962, bool ᒣ)
	{
		ArrayList arrayList = new ArrayList(15);
		BarFunctions.GetSubItemsByNameAndType(ᒟ, ᒢ, arrayList, _0962, ᒣ);
		return arrayList;
	}

	ArrayList IOwner.GetItems(string ᒢ, Type _0962, bool ᒣ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ᒡ
		return this.ᒡ(ᒢ, _0962, ᒣ);
	}

	private BaseItem ᒤ(string ᒢ)
	{
		BaseItem subItemByName = BarFunctions.GetSubItemByName(ᒟ, ᒢ);
		if (subItemByName != null)
		{
			return subItemByName;
		}
		return null;
	}

	BaseItem IOwner.GetItem(string ᒢ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ᒤ
		return this.ᒤ(ᒢ);
	}

	private void ਣ(BaseItem ਤ)
	{
		if (ਤ != null && ਤ.Parent is PopupItem)
		{
			return;
		}
		if (_09D8 != null)
		{
			if (ਤ == null)
			{
				BaseItem.CollapseSubItemsTree(_09D8);
			}
			if (_09D8.Expanded)
			{
				_09D8.Expanded = false;
			}
			_09D8 = null;
		}
		_09D8 = ਤ;
	}

	void IOwner.SetExpandedItem(BaseItem ਤ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ਣ
		this.ਣ(ਤ);
	}

	private BaseItem ਥ()
	{
		return _09D8;
	}

	BaseItem IOwner.GetExpandedItem()
	{
		//ILSpy generated this explicit interface implementation from .override directive in ਥ
		return this.ਥ();
	}

	private void ਦ(BaseItem ਧ)
	{
		if (_09D9 != null && _09D9 != ਧ)
		{
			_09D9.OnLostFocus();
		}
		_09D9 = ਧ;
		if (_09D9 != null)
		{
			_09D9.OnGotFocus();
		}
	}

	void IOwner.SetFocusItem(BaseItem ਧ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ਦ
		this.ਦ(ਧ);
	}

	private BaseItem ਨ()
	{
		return _09D9;
	}

	BaseItem IOwner.GetFocusItem()
	{
		//ILSpy generated this explicit interface implementation from .override directive in ਨ
		return this.ਨ();
	}

	private void _0A29(BaseItem ਤ)
	{
	}

	void IOwner.DesignTimeContextMenu(BaseItem ਤ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ਩
		this._0A29(ਤ);
	}

	private void ਪ(BaseItem ਤ)
	{
		ᯣ ᯣ2 = null;
		if (ਤ.ShortcutString != "")
		{
			foreach (eShortcut shortcut in ਤ.Shortcuts)
			{
				if (!_09DA.ContainsKey(shortcut))
				{
					continue;
				}
				ᯣ2 = (ᯣ)_09DA[shortcut];
				try
				{
					ᯣ2.ᯥ.Remove(ਤ.Id);
					if (ᯣ2.ᯥ.Count == 0)
					{
						_09DA.Remove(ᯣ2.ᯤ);
					}
				}
				catch (ArgumentException)
				{
				}
			}
		}
		foreach (BaseItem subItem in ਤ.SubItems)
		{
			((IOwner)this).RemoveShortcutsFromItem(subItem);
		}
	}

	void IOwner.RemoveShortcutsFromItem(BaseItem ਤ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ਪ
		this.ਪ(ਤ);
	}

	private void ਫ(BaseItem ਤ)
	{
		ᯣ ᯣ2 = null;
		if (ਤ.ShortcutString != "")
		{
			foreach (eShortcut shortcut in ਤ.Shortcuts)
			{
				if (_09DA.ContainsKey(shortcut))
				{
					ᯣ2 = (ᯣ)_09DA[ਤ.Shortcuts[0]];
				}
				else
				{
					ᯣ2 = new ᯣ(shortcut);
					_09DA.Add(ᯣ2.ᯤ, ᯣ2);
				}
				try
				{
					ᯣ2.ᯥ.Add(ਤ.Id, ਤ);
				}
				catch (ArgumentException)
				{
				}
			}
		}
		foreach (BaseItem subItem in ਤ.SubItems)
		{
			((IOwner)this).AddShortcutsFromItem(subItem);
		}
	}

	void IOwner.AddShortcutsFromItem(BaseItem ਤ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ਫ
		this.ਫ(ਤ);
	}

	private void ਭ()
	{
	}

	void IOwner.Customize()
	{
		//ILSpy generated this explicit interface implementation from .override directive in ਭ
		this.ਭ();
	}

	private void ਮ(BaseItem _06EC, EventArgs ؿ)
	{
	}

	void IOwner.InvokeResetDefinition(BaseItem _06EC, EventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ਮ
		this.ਮ(_06EC, ؿ);
	}

	private void ਰ()
	{
	}

	void IOwner.OnApplicationActivate()
	{
		//ILSpy generated this explicit interface implementation from .override directive in ਰ
		this.ਰ();
	}

	private void _0A31()
	{
		ClosePopups();
	}

	void IOwner.OnApplicationDeactivate()
	{
		//ILSpy generated this explicit interface implementation from .override directive in ਱
		this._0A31();
	}

	private void ਲ()
	{
	}

	void IOwner.OnParentPositionChanging()
	{
		//ILSpy generated this explicit interface implementation from .override directive in ਲ
		this.ਲ();
	}

	private void _0C77(BaseItem _06EC)
	{
	}

	void IOwner.StartItemDrag(BaseItem _06EC)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ౷
		this._0C77(_06EC);
	}

	private void ਲ਼(object _٠, EventArgs ؿ)
	{
	}

	void IOwner.InvokeUserCustomize(object _٠, EventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ਲ਼
		this.ਲ਼(_٠, ؿ);
	}

	private void _0A34(object _٠, EndUserCustomizeEventArgs ؿ)
	{
	}

	void IOwner.InvokeEndUserCustomize(object _٠, EndUserCustomizeEventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ਴
		this._0A34(_٠, ؿ);
	}

	private MdiClient ਵ(Form ਸ਼)
	{
		return BarFunctions.GetMdiClient(ਸ਼);
	}

	MdiClient IOwner.GetMdiClient(Form ਸ਼)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ਵ
		return this.ਵ(ਸ਼);
	}

	private void ٶ(object _٠, EventArgs ؿ)
	{
		if (_٠ == գ)
		{
			գ = null;
		}
		else if (_٠ == ড়)
		{
			ড় = null;
		}
		else if (_٠ == _09DB)
		{
			_09DB = null;
		}
	}

	protected override void OnParentChanged(EventArgs e)
	{
		((Control)this).OnParentChanged(e);
		if (((Component)this).DesignMode)
		{
			ᒟ.SetDesignMode(((Component)this).DesignMode);
		}
	}

	private void _0A37(object _٠, EventArgs ؿ)
	{
	}

	void IOwner.InvokeDefinitionLoaded(object _٠, EventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ਷
		this._0A37(_٠, ؿ);
	}

	private void ઌ(LocalizeEventArgs ؿ)
	{
		if (_09D4 != null)
		{
			_09D4(this, ؿ);
		}
	}

	void IOwnerLocalize.InvokeLocalizeString(LocalizeEventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ઌ
		this.ઌ(ؿ);
	}
}
