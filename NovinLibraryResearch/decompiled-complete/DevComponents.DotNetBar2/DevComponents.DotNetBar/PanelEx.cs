using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using DevComponents.DotNetBar.Controls;
using DevComponents.DotNetBar.Rendering;
using DevComponents.DotNetBar.ScrollBar;
using DevComponents.DotNetBar.TextMarkup;

namespace DevComponents.DotNetBar;

[ToolboxItem(true)]
[Designer("DevComponents.DotNetBar.Design.PanelExDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
[DefaultEvent("Click")]
public class PanelEx : Panel, INonClientControl, IScrollBarOverrideSupport, IButtonControl
{
	private ItemStyle Ӷ;

	private ItemStyle Ӱ;

	private ItemStyle ݔ;

	private ColorScheme լ;

	private eDotNetBarStyle ᑨ = eDotNetBarStyle.Office2003;

	private bool ݡ;

	private bool ݠ;

	private Rectangle ᑩ = Rectangle.Empty;

	private bool ᑪ = true;

	private bool ᑫ;

	private string ө = "";

	private ᑧ ᑬ;

	private DialogResult ᑭ;

	private bool ᑮ;

	private bool ե = true;

	private Color ᑯ = Color.White;

	protected Bitmap m_ThemeCachedBitmap;

	private bool _055D;

	private _0CB4 ݶ;

	private bool ᖜ = true;

	private bool ᖝ;

	private bool ᖞ;

	private ᗔ _ᖟ;

	private MarkupLinkClickEventHandler Ӟ;

	private eCornerType ᑰ = eCornerType.Square;

	private int _߃ = 8;

	private bool _ݷ = true;

	private bool _ᖠ;

	private Size _ᖡ = new Size(-2, -2);

	private bool _ᖢ = true;

	private eScrollBarAppearance _ז;

	internal bool ᑲ;

	private BaseRenderer ਛ;

	private Color _ᖣ = Color.Empty;

	private EventHandler ᖤ;

	private ScrollValueChangedHandler ሁ;

	private EventHandler ᖥ;

	[Description("Indicates whether right-to-left mirror placement is turned on. ")]
	[Browsable(true)]
	[Category("Layout")]
	[DefaultValue(false)]
	public bool RightToLeftLayout
	{
		get
		{
			return ᖝ;
		}
		set
		{
			if (ᖝ != value)
			{
				ᖝ = value;
				if (((Control)this).IsHandleCreated)
				{
					((Control)this).RecreateHandle();
				}
			}
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public BorderStyle BorderStyle
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ((Panel)this).BorderStyle;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			((Panel)this).BorderStyle = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[DevCoBrowsable(false)]
	[Browsable(false)]
	[Description("Gets or sets Bar Color Scheme.")]
	[Category("Appearance")]
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
			OnColorSchemeChanged();
			if (((Control)this).Visible)
			{
				((Control)this).Refresh();
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool SuspendPaint
	{
		get
		{
			return _055D;
		}
		set
		{
			if (_055D != value)
			{
				_055D = value;
				if (!_055D)
				{
					ᑴ();
					((Control)this).Invalidate();
				}
			}
		}
	}

	private bool ᓨ
	{
		get
		{
			if (ᑲ)
			{
				return BarFunctions.ThemedOS;
			}
			return false;
		}
	}

	internal bool _0AD4
	{
		get
		{
			return ᖜ;
		}
		set
		{
			ᖜ = value;
		}
	}

	[Category("Behavior")]
	[DefaultValue(true)]
	[Description("Gets or sets whether Text supports and renders text markup.")]
	public bool TextMarkupEnabled
	{
		get
		{
			return _ݷ;
		}
		set
		{
			if (_ݷ != value)
			{
				_ݷ = value;
				((Control)this).OnTextChanged(EventArgs.Empty);
				if (_ݷ)
				{
					ResizeMarkup();
				}
				((Control)this).Invalidate();
			}
		}
	}

	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Editor("DevComponents.DotNetBar.Design.TextMarkupUIEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Browsable(true)]
	[EditorBrowsable(EditorBrowsableState.Always)]
	[Description("Gets or sets the text displayed on panel.")]
	public override string Text
	{
		get
		{
			return ө;
		}
		set
		{
			if (ө != value)
			{
				((Panel)this).Text = value;
				ө = value;
				((Control)this).OnTextChanged(new EventArgs());
				((Control)this).Refresh();
			}
		}
	}

	[DefaultValue(false)]
	[DevCoBrowsable(true)]
	[Description("Indicates whether focus rectangle is displayed when control has focus.")]
	[Browsable(true)]
	[Category("Appearance")]
	public bool ShowFocusRectangle
	{
		get
		{
			return ᑫ;
		}
		set
		{
			ᑫ = value;
			if (((Component)this).DesignMode)
			{
				((Control)this).Refresh();
			}
		}
	}

	[Description("Gets or sets the canvas color.")]
	[Browsable(true)]
	[Category("Background")]
	public Color CanvasColor
	{
		get
		{
			return ᑯ;
		}
		set
		{
			ᑯ = value;
			if (((Component)this).DesignMode)
			{
				((Control)this).Refresh();
			}
		}
	}

	[Browsable(true)]
	[Category("Style")]
	[NotifyParentProperty(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[DevCoBrowsable(true)]
	[Description("Gets or sets panel style.")]
	public ItemStyle Style => Ӷ;

	[Browsable(true)]
	[NotifyParentProperty(true)]
	[Description("Gets or sets the panel style when mouse hovers over the panel.")]
	[Category("Style")]
	[DevCoBrowsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ItemStyle StyleMouseOver => Ӱ;

	[NotifyParentProperty(true)]
	[Category("Style")]
	[Description("Gets or sets the panel style when mouse button is pressed on the panel.")]
	[DevCoBrowsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Browsable(true)]
	public ItemStyle StyleMouseDown => ݔ;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override Color BackColor
	{
		get
		{
			return ((Control)this).BackColor;
		}
		set
		{
			((Control)this).BackColor = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override Image BackgroundImage
	{
		get
		{
			return ((Control)this).BackgroundImage;
		}
		set
		{
			((Control)this).BackgroundImage = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public override Color ForeColor
	{
		get
		{
			return ((Control)this).ForeColor;
		}
		set
		{
			((Control)this).ForeColor = value;
		}
	}

	[DefaultValue(true)]
	[Description("Gets or sets whether anti-aliasing is used while painting.")]
	[Category("Appearance")]
	[Browsable(true)]
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
				OnAntiAliasChanged();
				((Control)this).Refresh();
			}
		}
	}

	[Description("Gets or sets color scheme style.")]
	[Category("Style")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(eDotNetBarStyle.Office2003)]
	public eDotNetBarStyle ColorSchemeStyle
	{
		get
		{
			return ᑨ;
		}
		set
		{
			ᑨ = value;
			if (BarFunctions.IsOffice2007Style(ᑨ) && GlobalManager.Renderer is Office2007Renderer)
			{
				լ = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.LegacyColors;
			}
			else
			{
				լ = new ColorScheme(ᑨ);
			}
			OnColorSchemeChanged();
			((Control)this).Invalidate();
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	public override bool AutoScroll
	{
		get
		{
			return ((ScrollableControl)this).AutoScroll;
		}
		set
		{
			if (!value)
			{
				((ScrollableControl)this).AutoScrollMinSize = Size.Empty;
			}
			((ScrollableControl)this).AutoScroll = value;
			RefreshTextClientRectangle();
		}
	}

	[Category("Appearance")]
	[Description("Indicates whether text rectangle painted on panel is considering docked controls inside the panel.")]
	[DefaultValue(true)]
	public bool TextDockConstrained
	{
		get
		{
			return ᑪ;
		}
		set
		{
			ᑪ = value;
			if (((Component)this).DesignMode)
			{
				((Control)this).Refresh();
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Rectangle ClientTextRectangle
	{
		get
		{
			return ᑩ;
		}
		set
		{
			ᑩ = value;
		}
	}

	protected override CreateParams CreateParams
	{
		get
		{
			CreateParams createParams = ((Panel)this).CreateParams;
			if (ᖝ)
			{
				createParams.ExStyle |= 0x500000;
			}
			return createParams;
		}
	}

	[Category("Appearance")]
	[Description("Indicates whether text markup if it occupies less space than control provides uses the Style Alignment and LineAlignment properties to align the markup inside of the control.")]
	[DefaultValue(false)]
	[Browsable(true)]
	public virtual bool MarkupUsesStyleAlignment
	{
		get
		{
			return ᖞ;
		}
		set
		{
			if (ᖞ != value)
			{
				ᖞ = value;
				if (BarFunctions.IsHandleValid((Control)(object)this) && ݶ != null)
				{
					ResizeMarkup();
					((Control)this).Invalidate();
				}
			}
		}
	}

	[Browsable(false)]
	public VScrollBarAdv VScrollBar
	{
		get
		{
			if (_ᖟ != null)
			{
				return _ᖟ.ᗖ;
			}
			return null;
		}
	}

	[Browsable(false)]
	public HScrollBarAdv HScrollBar
	{
		get
		{
			if (_ᖟ != null)
			{
				return _ᖟ.ᗗ;
			}
			return null;
		}
	}

	[Description("Indicates whether panel uses themed DotNetBar scrollbars.")]
	[DefaultValue(true)]
	[Category("Behavior")]
	public bool UseThemedScrollbars
	{
		get
		{
			return _ᖢ;
		}
		set
		{
			if (_ᖢ != value)
			{
				ᖪ();
				_ᖢ = value;
				if (_ᖢ)
				{
					_ᖟ = new ᗔ((Control)(object)this);
					_ᖟ.ᗕ = _ז;
				}
			}
		}
	}

	[DefaultValue(eScrollBarAppearance.Default)]
	[Category("Appearance")]
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
			if (_ᖟ != null)
			{
				_ᖟ.ᗕ = _ז;
			}
		}
	}

	[Browsable(true)]
	[Description("Gets or sets the value returned to the parent form when the button is clicked.")]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Category("Behavior")]
	public DialogResult DialogResult
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ᑭ;
		}
		set
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			if (Enum.IsDefined(typeof(DialogResult), value))
			{
				ᑭ = value;
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[DevCoBrowsable(true)]
	[Description("Specifies whether item is drawn using Themes when running on OS that supports themes like Windows XP.")]
	[Category("Appearance")]
	public virtual bool ThemeAware
	{
		get
		{
			return ᑲ;
		}
		set
		{
			DisposeThemeCachedBitmap();
			ᑲ = value;
			((Control)this).Refresh();
		}
	}

	internal ᑧ ᑧ
	{
		get
		{
			if (ᑬ == null)
			{
				ᑬ = new ᑧ((Control)(object)this);
			}
			return ᑬ;
		}
	}

	ElementStyle INonClientControl.ᖕ => null;

	IntPtr INonClientControl.ᖖ => ((Control)this).Handle;

	int INonClientControl.ᖗ => ((Control)this).Width;

	int INonClientControl.ᖘ => ((Control)this).Height;

	bool INonClientControl.ᖙ => ((Control)this).IsHandleCreated;

	Color INonClientControl.ᖚ => ((Control)this).BackColor;

	[Description("Specifies back color when Enabled=false")]
	[Browsable(false)]
	[Category("Appearance")]
	public Color DisabledBackColor
	{
		get
		{
			return _ᖣ;
		}
		set
		{
			_ᖣ = value;
			if (!((Control)this).Enabled)
			{
				((Control)this).Invalidate();
			}
		}
	}

	bool IScrollBarOverrideSupport.ᖛ => ((Component)this).DesignMode;

	public event MarkupLinkClickEventHandler MarkupLinkClick
	{
		add
		{
			MarkupLinkClickEventHandler markupLinkClickEventHandler = Ӟ;
			MarkupLinkClickEventHandler markupLinkClickEventHandler2;
			do
			{
				markupLinkClickEventHandler2 = markupLinkClickEventHandler;
				MarkupLinkClickEventHandler value2 = (MarkupLinkClickEventHandler)Delegate.Combine(markupLinkClickEventHandler2, value);
				markupLinkClickEventHandler = Interlocked.CompareExchange(ref Ӟ, value2, markupLinkClickEventHandler2);
			}
			while ((object)markupLinkClickEventHandler != markupLinkClickEventHandler2);
		}
		remove
		{
			MarkupLinkClickEventHandler markupLinkClickEventHandler = Ӟ;
			MarkupLinkClickEventHandler markupLinkClickEventHandler2;
			do
			{
				markupLinkClickEventHandler2 = markupLinkClickEventHandler;
				MarkupLinkClickEventHandler value2 = (MarkupLinkClickEventHandler)Delegate.Remove(markupLinkClickEventHandler2, value);
				markupLinkClickEventHandler = Interlocked.CompareExchange(ref Ӟ, value2, markupLinkClickEventHandler2);
			}
			while ((object)markupLinkClickEventHandler != markupLinkClickEventHandler2);
		}
	}

	[Browsable(false)]
	public event EventHandler NonClientSizeChanged
	{
		add
		{
			EventHandler eventHandler = ᖤ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᖤ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ᖤ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᖤ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event ScrollValueChangedHandler ScrollBarValueChanged
	{
		add
		{
			ScrollValueChangedHandler scrollValueChangedHandler = ሁ;
			ScrollValueChangedHandler scrollValueChangedHandler2;
			do
			{
				scrollValueChangedHandler2 = scrollValueChangedHandler;
				ScrollValueChangedHandler value2 = (ScrollValueChangedHandler)Delegate.Combine(scrollValueChangedHandler2, value);
				scrollValueChangedHandler = Interlocked.CompareExchange(ref ሁ, value2, scrollValueChangedHandler2);
			}
			while ((object)scrollValueChangedHandler != scrollValueChangedHandler2);
		}
		remove
		{
			ScrollValueChangedHandler scrollValueChangedHandler = ሁ;
			ScrollValueChangedHandler scrollValueChangedHandler2;
			do
			{
				scrollValueChangedHandler2 = scrollValueChangedHandler;
				ScrollValueChangedHandler value2 = (ScrollValueChangedHandler)Delegate.Remove(scrollValueChangedHandler2, value);
				scrollValueChangedHandler = Interlocked.CompareExchange(ref ሁ, value2, scrollValueChangedHandler2);
			}
			while ((object)scrollValueChangedHandler != scrollValueChangedHandler2);
		}
	}

	[Browsable(false)]
	public event EventHandler ControlMoved
	{
		add
		{
			EventHandler eventHandler = ᖥ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᖥ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ᖥ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᖥ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public PanelEx()
	{
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		if (!ColorFunctions._0893)
		{
			_089F.ळ();
			_089F.ऴ();
			ColorFunctions.LoadColors();
		}
		_ᖟ = new ᗔ((Control)(object)this);
		((Control)this).SetStyle((ControlStyles)2, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)4, true);
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle(DisplayHelp.DoubleBufferFlag, true);
		((Control)this).SetStyle((ControlStyles)2048, true);
		((Control)this).SetStyle((ControlStyles)1, true);
		((Control)this).SetStyle((ControlStyles)512, true);
		լ = new ColorScheme(ᑨ);
		ResetStyle();
		ResetStyleMouseOver();
		ResetStyleMouseDown();
		((Control)this).BackColor = Color.Transparent;
		StyleManager.Register((Control)(object)this);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			StyleManager.Unregister((Control)(object)this);
		}
		ᖪ();
		if (Ӷ != null)
		{
			Ӷ.ઐ -= ᑳ;
		}
		if (ݔ != null)
		{
			ݔ.ઐ -= ᑳ;
		}
		if (Ӱ != null)
		{
			Ӱ.ઐ -= ᑳ;
		}
		((Control)this).Dispose(disposing);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual void StyleManagerStyleChanged(eDotNetBarStyle newStyle)
	{
		OnColorSchemeChanged();
	}

	internal void ᖦ()
	{
		((Control)this).SetStyle((ControlStyles)512, false);
		((Panel)this).TabStop = false;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeColorScheme()
	{
		if (լ.SchemeChanged)
		{
			return !BarFunctions.IsOffice2007Style(ᑨ);
		}
		return false;
	}

	private void ᑳ(object _٠, EventArgs ؿ)
	{
		if (ᖞ)
		{
			ResizeMarkup();
		}
		if (!_055D)
		{
			RefreshStyleSystemColors();
			ᑴ();
			((Control)this).Invalidate();
		}
	}

	protected override void OnVisibleChanged(EventArgs e)
	{
		if (BarFunctions.IsOffice2007Style(ᑨ))
		{
			RefreshStyleSystemColors();
		}
		((ScrollableControl)this).OnVisibleChanged(e);
	}

	private void ᑴ()
	{
		ᑴ(ᑵ: false);
	}

	private void ᑴ(bool ᑵ)
	{
		if (ᑵ || ᑰ != Ӷ.CornerType || _߃ != Ӷ.CornerDiameter)
		{
			ᑰ = Ӷ.CornerType;
			_߃ = Ӷ.CornerDiameter;
			if (Ӷ != null && (!ᑲ || !BarFunctions.ThemedOS) && Ӷ.CornerType != eCornerType.Square)
			{
				Region region = Ӷ.ᗊ(((Control)this).ClientRectangle);
				((Control)this).Region = region;
			}
			else
			{
				((Control)this).Region = null;
			}
		}
	}

	public void RefreshStyleSystemColors()
	{
		ColorScheme colorScheme = GetColorScheme();
		if (Ӷ != null)
		{
			Ӷ.ᗅ(colorScheme);
		}
		if (Ӱ != null)
		{
			Ӱ.ᗅ(colorScheme);
		}
		if (ݔ != null)
		{
			ݔ.ᗅ(colorScheme);
		}
	}

	protected override void WndProc(ref Message m)
	{
		if (((Message)(ref m)).Msg == 277 || ((Message)(ref m)).Msg == 276 || ((Message)(ref m)).Msg == 522)
		{
			((ScrollableControl)this).WndProc(ref m);
			OnScrollBarValueChanged(new ScrollValueChangedEventArgs(ScrollbarControl._1BAB(((Message)(ref m)).Msg)));
		}
		else if (((Message)(ref m)).Msg == 131)
		{
			((ScrollableControl)this).WndProc(ref m);
			OnNonClientSizeChanged(EventArgs.Empty);
		}
		else if (((Message)(ref m)).Msg == 206 || ((Message)(ref m)).Msg == 8270)
		{
			((ScrollableControl)this).WndProc(ref m);
			OnScrollBarValueChanged(new ScrollValueChangedEventArgs(eScrollBarScrollChange.Vertical | eScrollBarScrollChange.Horizontal));
		}
		else if (((Message)(ref m)).Msg == 3)
		{
			((ScrollableControl)this).WndProc(ref m);
			OnControlMoved(EventArgs.Empty);
		}
		else
		{
			((ScrollableControl)this).WndProc(ref m);
		}
	}

	private void ᖧ()
	{
		IntPtr windowDC = _0EFE.GetWindowDC(((Control)this).Handle);
		try
		{
			Graphics val = Graphics.FromHdc(windowDC);
			try
			{
				val.FillRectangle(SystemBrushes.Control, ((Control)this).Width - SystemInformation.VerticalScrollBarWidth, ((Control)this).Height - SystemInformation.HorizontalScrollBarHeight, SystemInformation.VerticalScrollBarWidth, SystemInformation.HorizontalScrollBarHeight);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		finally
		{
			_0EFE.ReleaseDC(((Control)this).Handle, windowDC);
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cc: Expected O, but got Unknown
		//IL_061b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0627: Unknown result type (might be due to invalid IL or missing references)
		//IL_053e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0544: Invalid comparison between Unknown and I4
		//IL_045e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0464: Invalid comparison between Unknown and I4
		if (((Control)this).ClientRectangle.Width == 0 || ((Control)this).ClientRectangle.Height == 0 || _055D)
		{
			return;
		}
		SolidBrush val = new SolidBrush(ᑯ);
		try
		{
			e.Graphics.FillRectangle((Brush)(object)val, ((Control)this).ClientRectangle);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		if ((Ӷ.BackColor1.Color.IsEmpty || Ӷ.BackColor1.Color.A < byte.MaxValue) && (Ӷ.BackColor2.Color.IsEmpty || Ӷ.BackColor2.Color.A < byte.MaxValue))
		{
			((ScrollableControl)this).OnPaintBackground(e);
		}
		SmoothingMode smoothingMode = e.Graphics.SmoothingMode;
		TextRenderingHint textRenderingHint = e.Graphics.TextRenderingHint;
		if (ե)
		{
			e.Graphics.SmoothingMode = (SmoothingMode)4;
			e.Graphics.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
		}
		ItemStyle itemStyle = Ӷ.Clone() as ItemStyle;
		if (ݠ && ((Control)this).Enabled && ݔ.ᖸ)
		{
			itemStyle.ᗄ(ݔ);
		}
		else if (ݡ && ((Control)this).Enabled && Ӱ.ᖸ)
		{
			itemStyle.ᗄ(Ӱ);
		}
		if (ݶ == null)
		{
			RefreshTextClientRectangle();
		}
		Rectangle displayRectangle = ((Control)this).DisplayRectangle;
		displayRectangle.Width = Math.Min(displayRectangle.Width, 17200);
		displayRectangle.Height = Math.Min(displayRectangle.Height, 17200);
		int x = displayRectangle.X;
		Padding padding = ((Control)this).Padding;
		displayRectangle.X = x - ((Padding)(ref padding)).Left;
		int y = displayRectangle.Y;
		Padding padding2 = ((Control)this).Padding;
		displayRectangle.Y = y - ((Padding)(ref padding2)).Top;
		int width = displayRectangle.Width;
		Padding padding3 = ((Control)this).Padding;
		displayRectangle.Width = width + ((Padding)(ref padding3)).Horizontal;
		int height = displayRectangle.Height;
		Padding padding4 = ((Control)this).Padding;
		displayRectangle.Height = height + ((Padding)(ref padding4)).Vertical;
		Rectangle textRect = ᑩ;
		textRect.Inflate(-1, -1);
		if (!((Control)this).Enabled)
		{
			itemStyle.ForeColor.Color = լ.ItemDisabledText;
		}
		Graphics graphics = e.Graphics;
		if (ᓨ)
		{
			if (m_ThemeCachedBitmap == null || ((Image)m_ThemeCachedBitmap).Size != ((Control)this).ClientRectangle.Size)
			{
				DisposeThemeCachedBitmap();
				Bitmap val2 = new Bitmap(((Control)this).ClientRectangle.Width, ((Control)this).ClientRectangle.Height, e.Graphics);
				Graphics val3 = Graphics.FromImage((Image)(object)val2);
				try
				{
					ᑧ._0EE5(val3, ᑷ.ᒀ, ᒂ.ཊ, new Rectangle(0, 0, ((Image)val2).Width, ((Image)val2).Height));
				}
				finally
				{
					val3.Dispose();
				}
				e.Graphics.DrawImage((Image)(object)val2, 0, 0, ((Image)val2).Width, ((Image)val2).Height);
				if (Ӷ.BackgroundImage != null)
				{
					BarFunctions._0982(e.Graphics, ((Control)this).ClientRectangle, Ӷ.BackgroundImage, Ӷ.BackgroundImagePosition, Ӷ.BackgroundImageAlpha);
				}
				m_ThemeCachedBitmap = val2;
			}
			else
			{
				e.Graphics.DrawImage((Image)(object)m_ThemeCachedBitmap, 0, 0, ((Image)m_ThemeCachedBitmap).Width, ((Image)m_ThemeCachedBitmap).Height);
			}
			if (_0AD4)
			{
				Region clip = graphics.Clip;
				Rectangle clientRectangle = ((Control)this).ClientRectangle;
				clientRectangle.Inflate(-2, -2);
				clientRectangle.Height -= 3;
				graphics.SetClip(clientRectangle);
				if (ݶ == null)
				{
					itemStyle.PaintText(graphics, ((Control)this).Text, textRect, ((Control)this).Font);
				}
				else
				{
					Rectangle clipRectangle = e.ClipRectangle;
					if (clipRectangle.Height == ((Control)this).ClientRectangle.Height && clipRectangle.Width < ((Control)this).ClientRectangle.Width)
					{
						clipRectangle.Width = ((Control)this).ClientRectangle.Width;
					}
					MarkupDrawContext d = new MarkupDrawContext(graphics, ((Control)this).Font, itemStyle.ForeColor.Color, (int)((Control)this).RightToLeft == 1, clipRectangle, hotKeyPrefixVisible: true);
					ݶ.Render(d);
				}
				graphics.Clip = clip;
			}
		}
		else if (ݶ == null)
		{
			if (ᖜ)
			{
				itemStyle.Paint(graphics, displayRectangle, ((Control)this).Text, textRect, ((Control)this).Font);
			}
			else
			{
				itemStyle.Paint(graphics, displayRectangle);
			}
		}
		else
		{
			itemStyle.Paint(graphics, displayRectangle);
			if (ᖜ)
			{
				Rectangle clipRectangle2 = e.ClipRectangle;
				if (clipRectangle2.Height == ((Control)this).ClientRectangle.Height && clipRectangle2.Width < ((Control)this).ClientRectangle.Width)
				{
					clipRectangle2.Width = ((Control)this).ClientRectangle.Width;
				}
				MarkupDrawContext d2 = new MarkupDrawContext(graphics, ((Control)this).Font, itemStyle.ForeColor.Color, (int)((Control)this).RightToLeft == 1, clipRectangle2, hotKeyPrefixVisible: true);
				if (((ScrollableControl)this).AutoScroll)
				{
					ݶ.Bounds = new Rectangle(-_ᖡ.Width + ((ScrollableControl)this).AutoScrollPosition.X, -_ᖡ.Height + ((ScrollableControl)this).AutoScrollPosition.Y, ݶ.Bounds.Width, ݶ.Bounds.Height);
				}
				ݶ.Render(d2);
			}
		}
		if (((Control)this).Focused && ᑫ)
		{
			displayRectangle = ((Control)this).ClientRectangle;
			displayRectangle.Inflate(-2, -2);
			if (displayRectangle.Width > 0 && displayRectangle.Height > 0)
			{
				ControlPaint.DrawFocusRectangle(graphics, displayRectangle);
			}
		}
		e.Graphics.SmoothingMode = smoothingMode;
		e.Graphics.TextRenderingHint = textRenderingHint;
		((Control)this).OnPaint(e);
	}

	protected override void OnResize(EventArgs e)
	{
		((Panel)this).OnResize(e);
		RefreshTextClientRectangle();
		ᑴ(ᑵ: true);
	}

	protected override void OnChangeUICues(UICuesEventArgs e)
	{
		((Control)this).OnChangeUICues(e);
		if (ᑫ)
		{
			((Control)this).Refresh();
		}
	}

	protected override void OnLostFocus(EventArgs e)
	{
		((Control)this).OnLostFocus(e);
		if (ᑫ)
		{
			((Control)this).Refresh();
		}
	}

	protected override void OnGotFocus(EventArgs e)
	{
		((Control)this).OnGotFocus(e);
		if (ᑫ)
		{
			((Control)this).Refresh();
		}
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		if (ݶ != null)
		{
			ݶ.મ((Control)(object)this, e);
		}
		((Control)this).OnMouseMove(e);
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Invalid comparison between Unknown and I4
		if (ݶ != null)
		{
			ݶ.ग़((Control)(object)this, e);
		}
		if ((int)e.Button == 1048576)
		{
			SetMouseDown(mouseDown: true);
		}
		((Control)this).OnMouseDown(e);
	}

	public void SetMouseDown(bool mouseDown)
	{
		if (ݠ != mouseDown)
		{
			ݠ = mouseDown;
			if (ݔ.ᖸ)
			{
				((Control)this).Invalidate(false);
			}
		}
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		((Control)this).OnMouseUp(e);
		if (ݶ != null)
		{
			ݶ.ફ((Control)(object)this, e);
		}
		SetMouseDown(mouseDown: false);
	}

	protected override void OnClick(EventArgs e)
	{
		if (ݶ != null)
		{
			ݶ.દ((Control)(object)this);
		}
		((Control)this).OnClick(e);
	}

	protected override void OnMouseEnter(EventArgs e)
	{
		((Control)this).OnMouseEnter(e);
		SetMouseOver(mouseOver: true);
	}

	public void SetMouseOver(bool mouseOver)
	{
		if (ݡ != mouseOver)
		{
			ݡ = mouseOver;
			if (Ӱ.ᖸ)
			{
				((Control)this).Invalidate(false);
			}
		}
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		((Control)this).OnMouseLeave(e);
		if (ݶ != null)
		{
			ݶ.ભ((Control)(object)this);
		}
		SetMouseOver(mouseOver: false);
	}

	protected override void OnSystemColorsChanged(EventArgs e)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		((Control)this).OnSystemColorsChanged(e);
		BarUtilities.InvokeDelayed((MethodInvoker)delegate
		{
			լ.Refresh(null, bSystemColorEvent: true);
			RefreshStyleSystemColors();
			if (ᑬ != null)
			{
				చ();
			}
			((Control)this).Invalidate(true);
		}, 500);
	}

	protected override void OnHandleDestroyed(EventArgs e)
	{
		_0A78();
		((Control)this).OnHandleDestroyed(e);
	}

	protected override void OnFontChanged(EventArgs e)
	{
		((Control)this).OnFontChanged(e);
		if (ݶ != null)
		{
			ݶ._0CC4();
		}
		RefreshTextClientRectangle();
	}

	protected override void OnTextChanged(EventArgs e)
	{
		string _06EA = ((Control)this).Text;
		if (ݶ != null)
		{
			ݶ.ભ((Control)(object)this);
			ݶ.ವ -= ᑶ;
			ݶ = null;
		}
		if (_ݷ && ሒ.መ(ref _06EA))
		{
			ݶ = ሒ.ሕ(_06EA);
		}
		if (ݶ != null)
		{
			ݶ.ವ += ᑶ;
			RefreshTextClientRectangle();
		}
		((Control)this).OnTextChanged(e);
	}

	private void ᑶ(object _٠, EventArgs ؿ)
	{
		if (_٠ is _0CFE obj)
		{
			OnMarkupLinkClick(new MarkupLinkClickEventArgs(obj._0D01, obj._0D00));
		}
	}

	protected virtual void OnMarkupLinkClick(MarkupLinkClickEventArgs e)
	{
		if (Ӟ != null)
		{
			Ӟ(this, e);
		}
	}

	protected override bool ProcessMnemonic(char charCode)
	{
		if (Control.IsMnemonic(charCode, ((Control)this).Text))
		{
			((Control)this).OnClick(new EventArgs());
			return true;
		}
		return ((Control)this).ProcessMnemonic(charCode);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeCanvasColor()
	{
		return ᑯ != Color.White;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetCanvasColor()
	{
		ᑯ = Color.White;
	}

	public void ResetStyle()
	{
		if (Ӷ != null)
		{
			Ӷ.ઐ -= ᑳ;
		}
		Ӷ = new ItemStyle();
		Ӷ.ઐ += ᑳ;
		RefreshStyleSystemColors();
	}

	public void ResetMouseTracking()
	{
		ݡ = false;
		ݠ = false;
		((Control)this).Refresh();
	}

	public void ResetStyleMouseOver()
	{
		if (Ӱ != null)
		{
			Ӱ.ઐ -= ᑳ;
		}
		Ӱ = new ItemStyle();
		Ӱ.ઐ += ᑳ;
	}

	public void ResetStyleMouseDown()
	{
		if (ݔ != null)
		{
			ݔ.ઐ -= ᑳ;
		}
		ݔ = new ItemStyle();
		ݔ.ઐ += ᑳ;
	}

	protected virtual void OnAntiAliasChanged()
	{
	}

	protected ColorScheme GetColorScheme()
	{
		if (BarFunctions.IsOffice2007Style(ᑨ) && GlobalManager.Renderer is Office2007Renderer office2007Renderer && office2007Renderer.ColorTable.LegacyColors != null)
		{
			return office2007Renderer.ColorTable.LegacyColors;
		}
		return լ;
	}

	protected virtual void OnColorSchemeChanged()
	{
		RefreshStyleSystemColors();
	}

	public override Size GetPreferredSize(Size proposedSize)
	{
		if (((ArrangedElementCollection)((Control)this).Controls).Count == 0)
		{
			if (!BarFunctions.IsHandleValid((Control)(object)this))
			{
				return proposedSize;
			}
			Size autoSize = GetAutoSize();
			if (autoSize.IsEmpty)
			{
				return proposedSize;
			}
			return autoSize;
		}
		return ((Control)this).GetPreferredSize(proposedSize);
	}

	public Size GetAutoSize()
	{
		return GetAutoSize(0);
	}

	public Size GetAutoSize(int preferedWidth)
	{
		if (ݶ == null && ((Control)this).Text.Length == 0)
		{
			return new Size(16, 16);
		}
		Size result = Size.Empty;
		if (ݶ != null)
		{
			result = ((preferedWidth != 0) ? ᖨ(preferedWidth) : ݶ.Bounds.Size);
			result.Width += 4;
			result.Height += 6;
		}
		else if (((Control)this).Text.Length > 0)
		{
			Font font = ((Control)this).Font;
			if (Ӷ.Font != null)
			{
				font = Ӷ.Font;
			}
			Graphics val = BarFunctions.CreateGraphics((Control)(object)this);
			try
			{
				result = ((preferedWidth > 0) ? TextDrawing.MeasureString(val, ((Control)this).Text, font, preferedWidth, (!Style.UseMnemonic) ? eTextFormat.NoPrefix : eTextFormat.Default) : TextDrawing.MeasureString(val, ((Control)this).Text, font, 0, (!Style.UseMnemonic) ? eTextFormat.NoPrefix : eTextFormat.Default));
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
			result.Width += 2;
			result.Height += 2;
		}
		if (result.IsEmpty)
		{
			return result;
		}
		result.Width += Ӷ.MarginLeft + Ӷ.MarginRight;
		result.Height += Ӷ.MarginTop + Ӷ.MarginBottom;
		return result;
	}

	protected virtual void RefreshTextClientRectangle()
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Invalid comparison between Unknown and I4
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected I4, but got Unknown
		Rectangle rectangle = ((Control)this).DisplayRectangle;
		if (ᑪ)
		{
			foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
			{
				Control val = item;
				if ((int)val.Dock != 0)
				{
					if ((int)val.Dock == 5)
					{
						rectangle = Rectangle.Empty;
						break;
					}
					DockStyle dock = val.Dock;
					switch (dock - 1)
					{
					case 2:
						rectangle.X += val.Width;
						rectangle.Width -= val.Width;
						break;
					case 3:
						rectangle.Width -= val.Width;
						break;
					case 0:
						rectangle.Y += val.Height;
						rectangle.Height -= val.Height;
						break;
					case 1:
						rectangle.Height -= val.Height;
						break;
					}
				}
			}
			if (rectangle.Width <= 0 || rectangle.Height <= 0)
			{
				rectangle = Rectangle.Empty;
			}
		}
		ᑩ = rectangle;
		ResizeMarkup();
	}

	private Size ᖨ(int ᖩ)
	{
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Invalid comparison between Unknown and I4
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		Size result = Size.Empty;
		if (ݶ != null)
		{
			Rectangle rectangle = new Rectangle(0, 0, ᖩ, 500);
			rectangle.Inflate(-2, -2);
			rectangle.Inflate(-(Ӷ.MarginLeft + Ӷ.MarginRight), -(Ӷ.MarginTop + Ӷ.MarginBottom));
			if (ᓨ)
			{
				rectangle.Height -= 4;
			}
			Graphics val = ((Control)this).CreateGraphics();
			SmoothingMode smoothingMode = val.SmoothingMode;
			TextRenderingHint textRenderingHint = val.TextRenderingHint;
			_0CB4 obj = ሒ.ሕ(((Control)this).Text);
			try
			{
				if (ե)
				{
					val.SmoothingMode = (SmoothingMode)4;
					val.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
				}
				MarkupDrawContext d = new MarkupDrawContext(val, ((Control)this).Font, SystemColors.Control, (int)((Control)this).RightToLeft == 1);
				obj.Measure(rectangle.Size, d);
				result = obj.Bounds.Size;
			}
			finally
			{
				val.SmoothingMode = smoothingMode;
				val.TextRenderingHint = textRenderingHint;
				val.Dispose();
			}
		}
		return result;
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		if (_ᖠ)
		{
			ResizeMarkup();
		}
		((Control)this).OnHandleCreated(e);
	}

	public void UpdateMarkupSize()
	{
		RefreshTextClientRectangle();
		((Control)this).Invalidate();
	}

	protected virtual void ResizeMarkup()
	{
		//IL_03a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Invalid comparison between Unknown and I4
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Invalid comparison between Unknown and I4
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Invalid comparison between Unknown and I4
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Invalid comparison between Unknown and I4
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Invalid comparison between Unknown and I4
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Invalid comparison between Unknown and I4
		if (ݶ == null)
		{
			return;
		}
		if (!BarFunctions.IsHandleValid((Control)(object)this))
		{
			_ᖠ = true;
			return;
		}
		_ᖠ = false;
		Rectangle clientTextRectangle = ClientTextRectangle;
		clientTextRectangle.Inflate(_ᖡ);
		clientTextRectangle.Inflate(-(Ӷ.MarginLeft + Ӷ.MarginRight), -(Ӷ.MarginTop + Ӷ.MarginBottom));
		if (ᓨ)
		{
			clientTextRectangle.Height -= 4;
		}
		if (clientTextRectangle.Width <= 0 || clientTextRectangle.Height <= 0)
		{
			return;
		}
		Graphics val = ((Control)this).CreateGraphics();
		SmoothingMode smoothingMode = val.SmoothingMode;
		TextRenderingHint textRenderingHint = val.TextRenderingHint;
		try
		{
			if (ե)
			{
				val.SmoothingMode = (SmoothingMode)4;
				val.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
			}
			MarkupDrawContext markupDrawContext = new MarkupDrawContext(val, ((Control)this).Font, SystemColors.Control, (int)((Control)this).RightToLeft == 1);
			ݶ.Measure(clientTextRectangle.Size, markupDrawContext);
			if (ᖞ)
			{
				if (ݶ.Bounds.Height < clientTextRectangle.Height)
				{
					if ((int)Style.LineAlignment == 1)
					{
						clientTextRectangle.Y += (clientTextRectangle.Height - ݶ.Bounds.Height) / 2;
						clientTextRectangle.Height = ݶ.Bounds.Height;
					}
					else if ((int)Style.LineAlignment == 2)
					{
						clientTextRectangle.Y = clientTextRectangle.Bottom - ݶ.Bounds.Height;
						clientTextRectangle.Height = ݶ.Bounds.Height;
					}
				}
				if (ݶ.Bounds.Width < clientTextRectangle.Width)
				{
					if ((int)Style.Alignment == 1)
					{
						clientTextRectangle.X += (clientTextRectangle.Width - ݶ.Bounds.Width) / 2;
						clientTextRectangle.Width = ݶ.Bounds.Width;
					}
					else if (((int)Style.Alignment == 2 && (int)((Control)this).RightToLeft == 0) || ((int)Style.Alignment == 0 && (int)((Control)this).RightToLeft == 1))
					{
						clientTextRectangle.X = clientTextRectangle.Right - ݶ.Bounds.Width;
						clientTextRectangle.Width = ݶ.Bounds.Width;
					}
				}
			}
			ݶ._0CC6(clientTextRectangle, markupDrawContext);
			if (((ScrollableControl)this).AutoScroll)
			{
				Size empty = Size.Empty;
				if (ݶ.Bounds.Height > clientTextRectangle.Height)
				{
					empty.Height = ݶ.Bounds.Height + Ӷ.MarginTop + Ӷ.MarginBottom + 2;
				}
				if (ݶ.Bounds.Width > clientTextRectangle.Width)
				{
					empty.Width = ݶ.Bounds.Width + Ӷ.MarginLeft + Ӷ.MarginRight + 2;
				}
				if (((ScrollableControl)this).AutoScrollMinSize != empty)
				{
					((ScrollableControl)this).AutoScrollMinSize = empty;
				}
			}
			else if (!((ScrollableControl)this).AutoScrollMinSize.IsEmpty)
			{
				((ScrollableControl)this).AutoScrollMinSize = Size.Empty;
			}
		}
		finally
		{
			val.SmoothingMode = smoothingMode;
			val.TextRenderingHint = textRenderingHint;
			val.Dispose();
		}
	}

	protected override void OnControlAdded(ControlEventArgs e)
	{
		((Control)this).OnControlAdded(e);
		RefreshTextClientRectangle();
		if (((Component)this).DesignMode)
		{
			((Control)this).Refresh();
		}
	}

	protected override void OnControlRemoved(ControlEventArgs e)
	{
		((Control)this).OnControlRemoved(e);
		RefreshTextClientRectangle();
		if (ᑪ && ((Component)this).DesignMode)
		{
			((Control)this).Refresh();
		}
	}

	public void ApplyPanelStyle()
	{
		ResetStyle();
		ResetStyleMouseDown();
		ResetStyleMouseOver();
		Ӷ.Border = eBorderType.SingleLine;
		Ӷ.BorderColor.ColorSchemePart = eColorSchemePart.PanelBorder;
		Ӷ.BackColor1.ColorSchemePart = eColorSchemePart.PanelBackground;
		Ӷ.BackColor2.ColorSchemePart = eColorSchemePart.PanelBackground2;
		Ӷ.GradientAngle = լ.PanelBackgroundGradientAngle;
		Ӷ.ForeColor.ColorSchemePart = eColorSchemePart.PanelText;
		Ӷ.Alignment = (StringAlignment)1;
		Ӷ.LineAlignment = (StringAlignment)1;
		((Control)this).Refresh();
	}

	public void ApplyButtonStyle()
	{
		ResetStyle();
		ResetStyleMouseDown();
		ResetStyleMouseOver();
		Style.Alignment = (StringAlignment)1;
		Style.BackColor1.ColorSchemePart = eColorSchemePart.BarBackground;
		Style.BackColor2.ColorSchemePart = eColorSchemePart.BarBackground2;
		Style.Border = eBorderType.SingleLine;
		Style.BorderColor.ColorSchemePart = eColorSchemePart.BarDockedBorder;
		Style.ForeColor.ColorSchemePart = eColorSchemePart.ItemText;
		Style.GradientAngle = 90;
		StyleMouseDown.Alignment = (StringAlignment)1;
		StyleMouseDown.BackColor1.ColorSchemePart = eColorSchemePart.ItemPressedBackground;
		StyleMouseDown.BackColor2.ColorSchemePart = eColorSchemePart.ItemPressedBackground2;
		StyleMouseDown.BorderColor.ColorSchemePart = eColorSchemePart.ItemPressedBorder;
		StyleMouseDown.ForeColor.ColorSchemePart = eColorSchemePart.ItemPressedText;
		StyleMouseOver.Alignment = (StringAlignment)1;
		StyleMouseOver.BackColor1.ColorSchemePart = eColorSchemePart.ItemHotBackground;
		StyleMouseOver.BackColor2.ColorSchemePart = eColorSchemePart.ItemHotBackground2;
		StyleMouseOver.BorderColor.ColorSchemePart = eColorSchemePart.ItemHotBorder;
		StyleMouseOver.ForeColor.ColorSchemePart = eColorSchemePart.ItemHotText;
		((Control)this).Refresh();
	}

	public void ApplyLabelStyle()
	{
		ResetStyle();
		ResetStyleMouseDown();
		ResetStyleMouseOver();
		TypeDescriptor.GetProperties(Style)["Alignment"].SetValue(Style, (object)(StringAlignment)1);
		TypeDescriptor.GetProperties(Style.BackColor1)["ColorSchemePart"].SetValue(Style.BackColor1, eColorSchemePart.BarBackground);
		TypeDescriptor.GetProperties(Style.BorderColor)["ColorSchemePart"].SetValue(Style.BorderColor, eColorSchemePart.BarDockedBorder);
		TypeDescriptor.GetProperties(Style.ForeColor)["ColorSchemePart"].SetValue(Style.ForeColor, eColorSchemePart.ItemText);
		TypeDescriptor.GetProperties(Style)["GradientAngle"].SetValue(Style, 90);
		((Control)this).Refresh();
	}

	private void ᖪ()
	{
		if (_ᖟ != null)
		{
			_ᖟ.Dispose();
			_ᖟ = null;
		}
	}

	public void NotifyDefault(bool value)
	{
		if (ᑮ != value)
		{
			ᑮ = value;
		}
	}

	public void PerformClick()
	{
		if (((Control)this).CanSelect)
		{
			((Control)this).OnClick(EventArgs.Empty);
		}
	}

	private void చ()
	{
		DisposeThemeCachedBitmap();
		if (ᑬ != null)
		{
			ᑬ.Dispose();
			ᑬ = new ᑧ((Control)(object)this);
		}
	}

	private void _0A78()
	{
		if (ᑬ != null)
		{
			ᑬ.Dispose();
			ᑬ = null;
		}
		DisposeThemeCachedBitmap();
	}

	protected void DisposeThemeCachedBitmap()
	{
		if (m_ThemeCachedBitmap != null)
		{
			((Image)m_ThemeCachedBitmap).Dispose();
			m_ThemeCachedBitmap = null;
		}
	}

	private BaseRenderer ဣ()
	{
		if (GlobalManager.Renderer != null)
		{
			return GlobalManager.Renderer;
		}
		if (ਛ == null)
		{
			ਛ = new Office2007Renderer();
		}
		return ਛ;
	}

	private void ᖫ(ref Message Ⴘ)
	{
		((ScrollableControl)this).WndProc(ref Ⴘ);
	}

	void INonClientControl.BaseWndProc(ref Message Ⴘ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ᖫ
		this.ᖫ(ref Ⴘ);
	}

	private ItemPaintArgs ᖬ(Graphics _٥)
	{
		ItemPaintArgs itemPaintArgs = new ItemPaintArgs(this as IOwner, (Control)(object)this, _٥, GetColorScheme());
		itemPaintArgs.ໝ = ဣ();
		itemPaintArgs.DesignerSelection = false;
		itemPaintArgs.GlassEnabled = !((Component)this).DesignMode && _0EFE.ষ;
		return itemPaintArgs;
	}

	ItemPaintArgs INonClientControl.GetItemPaintArgs(Graphics _٥)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ᖬ
		return this.ᖬ(_٥);
	}

	private void ᖮ(PaintEventArgs ؿ)
	{
		((ScrollableControl)this).OnPaintBackground(ؿ);
	}

	void INonClientControl.PaintBackground(PaintEventArgs ؿ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ᖮ
		this.ᖮ(ؿ);
	}

	private Point ᖱ(Point ᖲ)
	{
		return ((Control)this).PointToScreen(ᖲ);
	}

	Point INonClientControl.PointToScreen(Point ᖲ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ᖱ
		return this.ᖱ(ᖲ);
	}

	private void ᖴ(ref Rectangle ल)
	{
	}

	void INonClientControl.AdjustClientRectangle(ref Rectangle ल)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ᖴ
		this.ᖴ(ref ल);
	}

	private void ᖵ(ref Rectangle ल)
	{
	}

	void INonClientControl.AdjustBorderRectangle(ref Rectangle ल)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ᖵ
		this.ᖵ(ref ल);
	}

	private void ᖶ(Graphics _٥)
	{
	}

	void INonClientControl.RenderNonClient(Graphics _٥)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ᖶ
		this.ᖶ(_٥);
	}

	protected virtual void OnNonClientSizeChanged(EventArgs e)
	{
		ᖤ?.Invoke(this, e);
	}

	protected virtual void OnScrollBarValueChanged(ScrollValueChangedEventArgs e)
	{
		ሁ?.Invoke(this, e);
	}

	protected virtual void OnControlMoved(EventArgs e)
	{
		ᖥ?.Invoke(this, e);
	}

	[SpecialName]
	bool INonClientControl.get_Enabled()
	{
		return ((Control)this).Enabled;
	}

	[SpecialName]
	void INonClientControl.set_Enabled(bool P_0)
	{
		((Control)this).Enabled = P_0;
	}

	[CompilerGenerated]
	private void ᖷ()
	{
		լ.Refresh(null, bSystemColorEvent: true);
		RefreshStyleSystemColors();
		if (ᑬ != null)
		{
			చ();
		}
		((Control)this).Invalidate(true);
	}
}
