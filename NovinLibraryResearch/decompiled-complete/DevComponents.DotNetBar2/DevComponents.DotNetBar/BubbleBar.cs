using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using DevComponents.UI.ContentManager;

namespace DevComponents.DotNetBar;

[ComVisible(false)]
[DefaultEvent("ButtonClick")]
[ToolboxItem(true)]
[Designer("DevComponents.DotNetBar.Design.BubbleBarDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
public class BubbleBar : Control, ሎ, ISupportInitialize
{
	private class _2839
	{
		public int و;

		public int ى;

		public float _283A;

		public float _283B;

		public float _283C;

		public float _283D;
	}

	private const float __27C8 = 0.3f;

	private ImageList ݖ;

	private ImageList _27C9;

	private bool _27CA = true;

	private eOrientation Ⴐ;

	private Size _27CB = new Size(48, 48);

	private Size _27CC = new Size(24, 24);

	private BubbleContentManager _27CD = new BubbleContentManager();

	private BubbleButtonLayoutManager _27CE = new BubbleButtonLayoutManager();

	private BubbleButton _27CF;

	private BubbleButton _27D0;

	private int ᇰ = 100;

	private BubbleButtonDisplayInfo _27D1 = new BubbleButtonDisplayInfo();

	private bool ե = true;

	private _282B _27D2;

	private bool _27D3;

	private bool ᇯ = true;

	private bool _27D4;

	private bool _27D5 = true;

	private eBubbleButtonAlignment Ḁ = eBubbleButtonAlignment.Bottom;

	private int ℯ = 6;

	private Font _27D6;

	private int _27D7;

	private Rectangle _27D8 = Rectangle.Empty;

	private BubbleBarTab ᓻ;

	private BubbleBarTab _27D9;

	private TabColors _27DA = new TabColors();

	private TabColors _27DB = new TabColors();

	private BubbleBarTabCollection ᓺ;

	private bool _27DC = true;

	private Rectangle _27DD = Rectangle.Empty;

	private SimpleTabLayoutManager _27DE;

	private SimpleTabDisplay _27DF;

	private SerialContentLayoutManager _27E0;

	private Point _27E1 = Point.Empty;

	private BubbleButton _27E2;

	private ElementStyle _27E3 = new ElementStyle();

	private ElementStyle _0599 = new ElementStyle();

	private bool _27E4 = true;

	private bool _২;

	private bool _27E5;

	private Color _27E6 = Color.White;

	private Color _27E7 = Color.Black;

	private BubbleBarTabChangingEventHadler _27E8;

	private ClickEventHandler _22E9;

	private EventHandler _27E9;

	private EventHandler _27EA;

	private EventHandler ਞ;

	private Timer _27EB;

	private BubbleBarTab _27EC;

	private int _27ED = -1;

	private int _27EE = -1;

	private BubbleButton _27EF;

	private bool _09E2;

	[Category("Appearance")]
	[Browsable(true)]
	[Description("Indicates bubble button tooltip text color.")]
	public Color TooltipTextColor
	{
		get
		{
			return _27E6;
		}
		set
		{
			_27E6 = value;
		}
	}

	[Browsable(true)]
	[Description("Indicates bubble button tooltip text outline color.")]
	[Category("Appearance")]
	public Color TooltipOutlineColor
	{
		get
		{
			return _27E7;
		}
		set
		{
			_27E7 = value;
		}
	}

	[Description("Indicates spacing in pixels between buttons.")]
	[Category("Appearance")]
	[DefaultValue(0)]
	[Browsable(true)]
	public int ButtonSpacing
	{
		get
		{
			return _27CD.BlockSpacing;
		}
		set
		{
			_27CD.BlockSpacing = value;
			RecalcLayout();
			((Control)this).Refresh();
		}
	}

	[Category("Button Background")]
	[Browsable(true)]
	[DefaultValue(true)]
	[Description("Indicates whether background for the buttons is stretched to consume complete width of the control.")]
	public bool ButtonBackgroundStretch
	{
		get
		{
			return _27E4;
		}
		set
		{
			if (_27E4 != value)
			{
				_27E4 = value;
				if (((Component)this).DesignMode)
				{
					((Control)this).Refresh();
				}
			}
		}
	}

	[Category("Appearance")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Browsable(true)]
	[Description("Gets the style for the background of the control.")]
	public ElementStyle BackgroundStyle => _0599;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Category("Button Background")]
	[Description("Gets the style for the background of the buttons.")]
	[Browsable(true)]
	public ElementStyle ButtonBackAreaStyle => _27E3;

	[Browsable(false)]
	public Rectangle ButtonBackAreaBounds => _27D8;

	[Browsable(false)]
	public Rectangle TabAreaBounds => _27DD;

	[DefaultValue(100)]
	[Description("Indicates duration of animatio.")]
	[Category("Appearance")]
	[Browsable(true)]
	public int AnimationTime
	{
		get
		{
			return ᇰ;
		}
		set
		{
			ᇰ = value;
		}
	}

	[DefaultValue(true)]
	[Description("Indicates whether animation is enabled.")]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool AnimationEnabled
	{
		get
		{
			return ᇯ;
		}
		set
		{
			ᇯ = value;
			if (!ᇯ)
			{
				_27F5();
			}
		}
	}

	[Category("Appearance")]
	[Browsable(true)]
	[Description("Gets or sets whether anti-aliasing is used while painting.")]
	[DefaultValue(false)]
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
				((Control)this).Refresh();
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(null)]
	[Category("Button Images")]
	[Description("ImageList for images used on buttons.")]
	public ImageList Images
	{
		get
		{
			return ݖ;
		}
		set
		{
			if (ݖ != null)
			{
				((Component)(object)ݖ).Disposed -= ٶ;
			}
			ݖ = value;
			if (ݖ != null)
			{
				((Component)(object)ݖ).Disposed += ٶ;
			}
		}
	}

	[Category("Button Images")]
	[Description("ImageList for large-sized images used on buttons when button is magnified.")]
	[DefaultValue(null)]
	[Browsable(true)]
	public ImageList ImagesLarge
	{
		get
		{
			return _27C9;
		}
		set
		{
			if (_27C9 != null)
			{
				((Component)(object)_27C9).Disposed -= ٶ;
			}
			_27C9 = value;
			if (_27C9 != null)
			{
				((Component)(object)_27C9).Disposed += ٶ;
			}
		}
	}

	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates whether tooltips are displayed for the buttons.")]
	[Browsable(true)]
	public bool ShowTooltips
	{
		get
		{
			return _27CA;
		}
		set
		{
			_27CA = value;
		}
	}

	[Category("Button Images")]
	[Description("Indicates size of the images when button is enlarged.")]
	[Browsable(true)]
	public Size ImageSizeLarge
	{
		get
		{
			return _27CB;
		}
		set
		{
			if (_27CB != value)
			{
				_27CB = value;
				RecalcLayout();
				((Control)this).Refresh();
			}
		}
	}

	[Browsable(true)]
	[Category("Button Images")]
	[Description("Indicates size of the images when button is enlarged.")]
	public Size ImageSizeNormal
	{
		get
		{
			return _27CC;
		}
		set
		{
			if (_27CC != value)
			{
				_27CC = value;
				RecalcLayout();
				((Control)this).Refresh();
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(null)]
	[Category("Appearance")]
	public Font TooltipFont
	{
		get
		{
			return _27D6;
		}
		set
		{
			_27D6 = value;
		}
	}

	[Browsable(false)]
	[DefaultValue(null)]
	public BubbleBarTab SelectedTab
	{
		get
		{
			return ᓻ;
		}
		set
		{
			if (ᓻ != value)
			{
				ᓻ = value;
				_1FB5();
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Gets the reference to the colors that are used when tab is selected.")]
	[Browsable(true)]
	[Category("Tabs")]
	public TabColors SelectedTabColors => _27DA;

	[Description("Gets the reference to the colors that are used when mouse is over the tab.")]
	[Category("Tabs")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Browsable(true)]
	public TabColors MouseOverTabColors => _27DB;

	[Description("Returns the collection of Tabs.")]
	[Category("Tabs")]
	[Editor("DevComponents.DotNetBar.Design.BubbleBarTabCollectionEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public BubbleBarTabCollection Tabs => ᓺ;

	[Category("Appearance")]
	[Browsable(true)]
	[Description("Indicates button alignment.")]
	public eBubbleButtonAlignment Alignment
	{
		get
		{
			return Ḁ;
		}
		set
		{
			if (Ḁ != value)
			{
				Ḁ = value;
				_280F();
				RecalcLayout();
				((Control)this).Refresh();
			}
		}
	}

	[DefaultValue(true)]
	[Browsable(true)]
	[Category("Tabs")]
	[Description("Indicates whether tabs are visible.")]
	public bool TabsVisible
	{
		get
		{
			return _27DC;
		}
		set
		{
			if (_27DC != value)
			{
				_27DC = value;
				_2806();
				_27F1();
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public bool DragInProgress => _09E2;

	bool ሎ.প
	{
		get
		{
			Form val = ((Control)this).FindForm();
			if (val != null)
			{
				return val.Modal;
			}
			return false;
		}
	}

	public event BubbleBarTabChangingEventHadler TabChanging
	{
		add
		{
			BubbleBarTabChangingEventHadler bubbleBarTabChangingEventHadler = _27E8;
			BubbleBarTabChangingEventHadler bubbleBarTabChangingEventHadler2;
			do
			{
				bubbleBarTabChangingEventHadler2 = bubbleBarTabChangingEventHadler;
				BubbleBarTabChangingEventHadler value2 = (BubbleBarTabChangingEventHadler)Delegate.Combine(bubbleBarTabChangingEventHadler2, value);
				bubbleBarTabChangingEventHadler = Interlocked.CompareExchange(ref _27E8, value2, bubbleBarTabChangingEventHadler2);
			}
			while ((object)bubbleBarTabChangingEventHadler != bubbleBarTabChangingEventHadler2);
		}
		remove
		{
			BubbleBarTabChangingEventHadler bubbleBarTabChangingEventHadler = _27E8;
			BubbleBarTabChangingEventHadler bubbleBarTabChangingEventHadler2;
			do
			{
				bubbleBarTabChangingEventHadler2 = bubbleBarTabChangingEventHadler;
				BubbleBarTabChangingEventHadler value2 = (BubbleBarTabChangingEventHadler)Delegate.Remove(bubbleBarTabChangingEventHadler2, value);
				bubbleBarTabChangingEventHadler = Interlocked.CompareExchange(ref _27E8, value2, bubbleBarTabChangingEventHadler2);
			}
			while ((object)bubbleBarTabChangingEventHadler != bubbleBarTabChangingEventHadler2);
		}
	}

	public event ClickEventHandler ButtonClick
	{
		add
		{
			ClickEventHandler clickEventHandler = _22E9;
			ClickEventHandler clickEventHandler2;
			do
			{
				clickEventHandler2 = clickEventHandler;
				ClickEventHandler value2 = (ClickEventHandler)Delegate.Combine(clickEventHandler2, value);
				clickEventHandler = Interlocked.CompareExchange(ref _22E9, value2, clickEventHandler2);
			}
			while ((object)clickEventHandler != clickEventHandler2);
		}
		remove
		{
			ClickEventHandler clickEventHandler = _22E9;
			ClickEventHandler clickEventHandler2;
			do
			{
				clickEventHandler2 = clickEventHandler;
				ClickEventHandler value2 = (ClickEventHandler)Delegate.Remove(clickEventHandler2, value);
				clickEventHandler = Interlocked.CompareExchange(ref _22E9, value2, clickEventHandler2);
			}
			while ((object)clickEventHandler != clickEventHandler2);
		}
	}

	public event EventHandler BubbleStart
	{
		add
		{
			EventHandler eventHandler = _27E9;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _27E9, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _27E9;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _27E9, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler BubbleEnd
	{
		add
		{
			EventHandler eventHandler = _27EA;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _27EA, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _27EA;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _27EA, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler ItemLayoutUpdated
	{
		add
		{
			EventHandler eventHandler = ਞ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ਞ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ਞ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ਞ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public BubbleBar()
	{
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		if (!ColorFunctions._0893)
		{
			_089F.ळ();
			_089F.ऴ();
			ColorFunctions.LoadColors();
		}
		((Control)this).SetStyle((ControlStyles)2, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)4, true);
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle(DisplayHelp.DoubleBufferFlag, true);
		((Control)this).SetStyle((ControlStyles)2048, true);
		((Control)this).SetStyle((ControlStyles)1, false);
		((Control)this).SetStyle((ControlStyles)512, false);
		ᓺ = new BubbleBarTabCollection(this);
		_27E3.StyleChanged += _065F;
		_0599.StyleChanged += _065F;
		_27CD.BlockSpacing = 0;
		_27CD.ContentVerticalAlignment = eContentVerticalAlignment.Bottom;
		_27CD.BlockLineAlignment = eContentVerticalAlignment.Bottom;
		_27CD.ContentOrientation = eContentOrientation.Horizontal;
		_27DE = new SimpleTabLayoutManager();
		_27DF = new SimpleTabDisplay();
		_27E0 = new SerialContentLayoutManager();
		_27D1.BubbleBar = this;
		_27D2 = new _282B(this);
		_27D2 = null;
		if (!((FeatureSupport)OSFeature.Feature).IsPresent(OSFeature.LayeredWindows))
		{
			_27D5 = false;
		}
		_280F();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTooltipTextColor()
	{
		return _27E6 != Color.White;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetTooltipTextColor()
	{
		TypeDescriptor.GetProperties(this)["TooltipTextColor"].SetValue(this, Color.White);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTooltipOutlineColor()
	{
		return _27E7 != Color.Black;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetTooltipOutlineColor()
	{
		TypeDescriptor.GetProperties(this)["TooltipOutlineColor"].SetValue(this, Color.Black);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeImageSizeLarge()
	{
		if (_27CB.Width != 48 || _27CB.Height != 48)
		{
			return true;
		}
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeImageSizeNormal()
	{
		if (_27CB.Width != 24 || _27CB.Height != 24)
		{
			return true;
		}
		return false;
	}

	public void RecalcLayout()
	{
		StopBubbleEffect();
		_2806();
		((Control)this).Invalidate();
		OnItemLayoutUpdated(EventArgs.Empty);
	}

	protected virtual void OnItemLayoutUpdated(EventArgs e)
	{
		ਞ?.Invoke(this, e);
	}

	public BubbleButton GetButtonAt(int x, int y)
	{
		if (ᓻ == null)
		{
			return null;
		}
		BubbleButton result = null;
		foreach (BubbleButton button in ᓻ.Buttons)
		{
			if (!button.Visible)
			{
				continue;
			}
			if (_27CF != null)
			{
				if (button.MagnifiedDisplayRectangle.Contains(x, y))
				{
					result = button;
					break;
				}
			}
			else if (button.DisplayRectangle.Contains(x, y))
			{
				result = button;
				break;
			}
		}
		return result;
	}

	public BubbleButton GetButtonAt(Point p)
	{
		return GetButtonAt(p.X, p.Y);
	}

	public BubbleBarTab GetTabAt(Point p)
	{
		return GetTabAt(p.X, p.Y);
	}

	public BubbleBarTab GetTabAt(int x, int y)
	{
		foreach (BubbleBarTab item in ᓺ)
		{
			if (item.DisplayRectangle.Contains(x, y))
			{
				return item;
			}
		}
		return null;
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

	protected override void OnCursorChanged(EventArgs e)
	{
		if (_27D2 != null)
		{
			((Control)_27D2).Cursor = ((Control)this).Cursor;
		}
		((Control)this).OnCursorChanged(e);
	}

	protected virtual void OnBubbleStart(EventArgs e)
	{
		if (_27E9 != null)
		{
			_27E9(this, e);
		}
	}

	protected virtual void OnBubbleEnd(EventArgs e)
	{
		if (_27EA != null)
		{
			_27EA(this, e);
		}
	}

	internal void _27F0(BubbleButton ᤉ, ClickEventArgs ؿ)
	{
		if (_22E9 != null)
		{
			_22E9(ᤉ, ؿ);
		}
	}

	protected override bool ProcessMnemonic(char charCode)
	{
		string text = "&" + charCode;
		text = text.ToLower();
		foreach (BubbleBarTab tab in Tabs)
		{
			string text2 = tab.Text.ToLower();
			if (text2.IndexOf(text) >= 0)
			{
				SelectedTab = tab;
				return true;
			}
		}
		return ((Control)this).ProcessMnemonic(charCode);
	}

	protected override void OnSystemColorsChanged(EventArgs e)
	{
		((Control)this).OnSystemColorsChanged(e);
		Application.DoEvents();
		if (_27E3.GetColorScheme() != null)
		{
			_27E3.GetColorScheme().Refresh();
		}
	}

	protected override void OnResize(EventArgs e)
	{
		((Control)this).OnResize(e);
		if (((Control)this).Width != 0 && ((Control)this).Height != 0)
		{
			RecalcLayout();
			((Control)this).Refresh();
		}
	}

	private void _27F1()
	{
		if (_27D2 != null)
		{
			_27D2._282E();
			((Control)this).Refresh();
		}
		else
		{
			((Control)this).Refresh();
		}
	}

	private void _065F(object _٠, EventArgs ؿ)
	{
		if (((Component)this).DesignMode)
		{
			_2806();
			((Control)this).Refresh();
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		PaintBackground(e);
		PaintControl(e);
	}

	protected virtual void PaintControl(PaintEventArgs e)
	{
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		ElementStyleDisplayInfo elementStyleDisplayInfo = new ElementStyleDisplayInfo();
		elementStyleDisplayInfo.Bounds = ((Control)this).DisplayRectangle;
		elementStyleDisplayInfo.Graphics = e.Graphics;
		elementStyleDisplayInfo.Style = _0599;
		ElementStyleDisplay.Paint(elementStyleDisplayInfo);
		if (!_27D8.IsEmpty && Tabs.Count > 0)
		{
			Rectangle bounds = _2809();
			elementStyleDisplayInfo.Bounds = bounds;
			elementStyleDisplayInfo.Graphics = e.Graphics;
			elementStyleDisplayInfo.Style = _27E3;
			ElementStyleDisplay.Paint(elementStyleDisplayInfo);
		}
		SmoothingMode smoothingMode = e.Graphics.SmoothingMode;
		TextRenderingHint textRenderingHint = e.Graphics.TextRenderingHint;
		if (ե)
		{
			e.Graphics.SmoothingMode = (SmoothingMode)4;
			e.Graphics.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
		}
		if (!_27DD.IsEmpty)
		{
			Rectangle clip = _220B();
			e.Graphics.SetClip(clip);
			ISimpleTab[] array = new ISimpleTab[ᓺ.Count];
			ᓺ._0CD0(array);
			_27DF.Paint(e.Graphics, array);
			e.Graphics.ResetClip();
		}
		if (_27D2 == null)
		{
			BubbleButtonDisplayInfo bubbleButtonDisplayInfo = _27F7();
			bubbleButtonDisplayInfo.Graphics = e.Graphics;
			if (ᓻ != null)
			{
				foreach (BubbleButton button in ᓻ.Buttons)
				{
					bubbleButtonDisplayInfo.Button = button;
					_2833._139A(bubbleButtonDisplayInfo);
				}
			}
		}
		if (((Component)this).DesignMode && Tabs.Count == 0)
		{
			Rectangle displayRectangle = ((Control)this).DisplayRectangle;
			eTextFormat format = eTextFormat.EndEllipsis | eTextFormat.HorizontalCenter | eTextFormat.VerticalCenter | eTextFormat.WordBreak;
			string text = "Right-click and choose Create Tab or Button to add new items.";
			TextDrawing.DrawString(e.Graphics, text, ((Control)this).Font, SystemColors.ControlDarkDark, displayRectangle, format);
		}
		e.Graphics.TextRenderingHint = textRenderingHint;
		e.Graphics.SmoothingMode = smoothingMode;
	}

	protected virtual void PaintBackground(PaintEventArgs e)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		if (((Control)this).BackColor.A < byte.MaxValue)
		{
			((Control)this).OnPaintBackground(e);
		}
		if (!((Control)this).BackColor.IsEmpty)
		{
			SolidBrush val = new SolidBrush(((Control)this).BackColor);
			try
			{
				e.Graphics.FillRectangle((Brush)(object)val, ((Control)this).DisplayRectangle);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
	}

	internal void _27F2(PaintEventArgs ؿ)
	{
		BubbleButtonDisplayInfo bubbleButtonDisplayInfo = _27F7();
		bubbleButtonDisplayInfo.Graphics = ؿ.Graphics;
		if (ᓻ == null)
		{
			return;
		}
		foreach (BubbleButton button in ᓻ.Buttons)
		{
			bubbleButtonDisplayInfo.Button = button;
			_2833._139A(bubbleButtonDisplayInfo);
		}
	}

	private bool ᇥ()
	{
		Form val = ((Control)this).FindForm();
		if (val != null && val.MdiParent == null && ((Control)val).Parent != null)
		{
			val = ((Control)val).Parent.FindForm();
		}
		if (val != null)
		{
			if (Form.ActiveForm != val || val.MdiParent != null)
			{
				if (val.MdiParent != null)
				{
					return val.MdiParent.ActiveMdiChild == val;
				}
				return false;
			}
			return true;
		}
		bool result = false;
		IntPtr foregroundWindow = _089F.GetForegroundWindow();
		try
		{
			Control val2 = Control.FromHandle(foregroundWindow);
			if (val2 != null)
			{
				result = true;
			}
		}
		catch
		{
			result = false;
		}
		return result;
	}

	private void _27F3()
	{
		if (_27D2 == null)
		{
			OnBubbleStart(new EventArgs());
		}
		if (_27D2 == null && ((Control)this).Parent != null && _27D5 && !((Component)this).DesignMode && ᇯ)
		{
			Rectangle bounds = _27F4();
			_27D2 = new _282B(this);
			((Control)_27D2).Bounds = bounds;
			((Control)_27D2).Visible = false;
			_27CD.MouseOverIndex = -1;
			_2806();
			if (_27CF != null)
			{
				_27CD.MouseOverIndex = _27CF.Parent.Buttons.IndexOf(_27CF);
			}
			((Control)_27D2).CreateControl();
			Point point = ((Control)this).Parent.PointToScreen(((Control)_27D2).Location);
			_27D2._2831();
			int value = -2;
			if (((Control)this).FindForm() != null && ((Control)this).FindForm().TopMost)
			{
				value = -1;
			}
			_089F.SetWindowPos(((Control)_27D2).Handle, new IntPtr(value), point.X, point.Y, ((Control)_27D2).Width, ((Control)_27D2).Height, 80);
			((Control)_27D2).Visible = true;
			_27D2._282E();
			((Control)this).Refresh();
		}
	}

	private Rectangle _27F4()
	{
		Rectangle result = new Rectangle(((Control)this).Location, ((Control)this).Size);
		if (Ḁ == eBubbleButtonAlignment.Top || Ḁ == eBubbleButtonAlignment.Bottom)
		{
			Size size = Dpi.ImageSize(_27CB);
			Size size2 = Dpi.ImageSize(_27CC);
			int num = size.Height - size2.Height;
			if (ShowTooltips)
			{
				if (TooltipFont != null)
				{
					_27D7 = TooltipFont.Height + 6;
				}
				else
				{
					_27D7 = ((Control)this).Font.Height + 6;
				}
				num += _27D7;
			}
			result.Height += num;
			if (Ḁ == eBubbleButtonAlignment.Bottom)
			{
				result.Y -= num;
			}
		}
		return result;
	}

	internal void _27F5()
	{
		_໔(null);
		_0ECC(null);
		_27F1();
	}

	public void StopBubbleEffect()
	{
		_໔(null);
		_0ECC(null);
	}

	private void _27F6()
	{
		if (_27D2 != null)
		{
			OnBubbleEnd(new EventArgs());
			((Control)_27D2).Hide();
			_27D2._2832();
			((Component)(object)_27D2).Dispose();
			_27D2 = null;
			_27CD.MouseOverIndex = -1;
			_2806();
			if (_27CF != null)
			{
				_27CD.MouseOverIndex = _27CF.Parent.Buttons.IndexOf(_27CF);
			}
			((Control)this).Invalidate();
		}
	}

	private BubbleButtonDisplayInfo _27F7()
	{
		_27D1.Magnified = _27CF != null && ᇯ;
		_27D1.Alignment = Ḁ;
		return _27D1;
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Expected O, but got Unknown
		if (ᇥ())
		{
			if (_27D2 != null && _27D7 > 0)
			{
				Point point = ((Control)this).PointToScreen(new Point(e.X, e.Y));
				point = ((Control)_27D2).PointToClient(point);
				MouseEventArgs ؿ = new MouseEventArgs(e.Button, e.Clicks, point.X, point.Y, e.Delta);
				_27F8(ؿ);
			}
			else
			{
				_27F8(e);
			}
		}
	}

	internal void _27F8(MouseEventArgs ؿ)
	{
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Invalid comparison between Unknown and I4
		((Control)this).OnMouseMove(ؿ);
		if (_27D4)
		{
			_27D4 = false;
			return;
		}
		bool flag = false;
		Point point = new Point(ؿ.X, ؿ.Y);
		BubbleButton buttonAt = GetButtonAt(point.X, point.Y);
		if (buttonAt != null && buttonAt == _27E2)
		{
			return;
		}
		if (buttonAt == null)
		{
			_27E2 = null;
		}
		if (buttonAt != null)
		{
			if (_27CF == null)
			{
				_27F3();
				_27CF = buttonAt;
				_280B(buttonAt, point, _280C: true);
				_27CF = null;
			}
			if (ᇯ)
			{
				_2839 _2839 = _280D(buttonAt.DisplayRectangle, point);
				_27CD.Factor1 = _2839._283A;
				_27CD.Factor2 = _2839._283B;
				_27CD.Factor3 = _2839._283C;
				_27CD.Factor4 = _2839._283D;
				Size bubbleSize = Dpi.ImageSize(_27CB);
				_27CD.BubbleSize = bubbleSize;
				_27CD.MouseOverPosition = _2839.و;
				_27CD.MouseOverIndex = buttonAt.Parent.Buttons.IndexOf(buttonAt);
				_2806();
				flag = true;
				_27E1 = point;
			}
		}
		if (_27D9 != null || _27DD.Contains(point))
		{
			BubbleBarTab tabAt = GetTabAt(point);
			SetMouseOverTab(tabAt);
		}
		if (_27CF != buttonAt)
		{
			_0ECC(buttonAt);
			flag = true;
		}
		if (_27CF != buttonAt && (int)ؿ.Button == 1048576)
		{
			_໔(buttonAt);
			flag = true;
		}
		if (_27CF != null)
		{
			Point mousePosition = Control.MousePosition;
			mousePosition = ((_27D2 == null) ? ((Control)this).PointToClient(mousePosition) : ((Control)_27D2).PointToClient(mousePosition));
			buttonAt = GetButtonAt(mousePosition.X, mousePosition.Y);
			if (buttonAt != _27CF)
			{
				_0ECC(buttonAt);
				flag = true;
			}
		}
		if (flag)
		{
			_27F1();
		}
	}

	private void _0ECC(BubbleButton ᔶ)
	{
		if (_27CF != null)
		{
			_27CF.SetMouseOver(value: false);
			if (ᔶ == null && !_27E1.IsEmpty)
			{
				_280B(_27CF, _27E1, _280C: false);
				_27E1 = Point.Empty;
			}
		}
		_27CF = ᔶ;
		if (_27CF != null)
		{
			_27CF.SetMouseOver(value: true);
			_27F3();
		}
		else
		{
			_໔(null);
			_27F6();
		}
		if (_27CF != null)
		{
			_27CD.MouseOverIndex = _27CF.Parent.Buttons.IndexOf(_27CF);
		}
		else
		{
			_27CD.MouseOverIndex = -1;
		}
	}

	private void _໔(BubbleButton _໕)
	{
		if (_27D0 != null)
		{
			_27D0.SetMouseDown(value: false);
		}
		if (_໕ != null && !_໕.Enabled)
		{
			_27D0 = null;
			return;
		}
		_27D0 = _໕;
		if (_27D0 != null)
		{
			_27D0.SetMouseDown(value: true);
		}
	}

	protected override void OnMouseEnter(EventArgs e)
	{
		_27FC();
		((Control)this).OnMouseEnter(e);
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		_27F9(e);
		((Control)this).OnMouseLeave(e);
	}

	internal void _27F9(EventArgs ؿ)
	{
		Point empty = Point.Empty;
		empty = ((_27CF == null || _27D2 == null) ? ((Control)this).PointToClient(Control.MousePosition) : ((Control)_27D2).PointToClient(Control.MousePosition));
		bool flag = false;
		BubbleButton buttonAt = GetButtonAt(empty.X, empty.Y);
		if (_27D0 != null && buttonAt == null)
		{
			_໔(null);
			flag = true;
		}
		SetMouseOverTab(null);
		if (flag)
		{
			_27F1();
		}
		_27FA();
		_27E2 = null;
	}

	private void _27FA()
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		if (_27D2 != null && _27CF != null && _27EB == null)
		{
			_27EB = new Timer();
			_27EB.Interval = 500;
			_27EB.Tick += _27FB;
			_27EB.Start();
		}
	}

	private void _27FB(object _٠, EventArgs ؿ)
	{
		Point empty = Point.Empty;
		empty = ((_27CF == null || _27D2 == null) ? ((Control)this).PointToClient(Control.MousePosition) : ((Control)_27D2).PointToClient(Control.MousePosition));
		BubbleButton buttonAt = GetButtonAt(empty.X, empty.Y);
		if (buttonAt == null)
		{
			StopBubbleEffect();
			_27FC();
		}
	}

	private void _27FC()
	{
		Timer val = _27EB;
		_27EB = null;
		if (val != null)
		{
			val.Stop();
			((Component)(object)val).Dispose();
		}
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		_27FE(e);
		((Control)this).OnMouseDown(e);
	}

	internal void _27FD(MouseEventArgs ؿ)
	{
		((Control)this).OnMouseDown(ؿ);
	}

	private void _27FE(MouseEventArgs ؿ)
	{
		BubbleButton buttonAt = GetButtonAt(ؿ.X, ؿ.Y);
		if (buttonAt != null)
		{
			_໔(buttonAt);
			_27F1();
		}
		else if (_27DD.Contains(ؿ.X, ؿ.Y))
		{
			BubbleBarTab tabAt = GetTabAt(new Point(ؿ.X, ؿ.Y));
			if (tabAt != null && tabAt != ᓻ)
			{
				_2816(tabAt, eEventSource.Mouse, _2815: true);
			}
		}
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		_2800(e);
		((Control)this).OnMouseUp(e);
	}

	internal void _27FF(MouseEventArgs ؿ)
	{
		((Control)this).OnMouseUp(ؿ);
	}

	private void _2800(MouseEventArgs ؿ)
	{
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		if (_27D0 != null)
		{
			BubbleButton bubbleButton = _27D0;
			Rectangle displayRectangle = bubbleButton.DisplayRectangle;
			Rectangle magnifiedDisplayRectangle = bubbleButton.MagnifiedDisplayRectangle;
			_໔(null);
			_0ECC(null);
			_27D4 = true;
			_27F1();
			if (displayRectangle.Contains(ؿ.X, ؿ.Y) || magnifiedDisplayRectangle.Contains(ؿ.X, ؿ.Y))
			{
				bubbleButton.InvokeClick(eEventSource.Mouse, ؿ.Button);
			}
			_27E2 = bubbleButton;
		}
	}

	private void ٶ(object _٠, EventArgs ؿ)
	{
		if (_٠ == ݖ)
		{
			Images = null;
		}
		else if (_٠ == _27C9)
		{
			ImagesLarge = null;
		}
	}

	internal void _2801(BubbleBarTab ᆰ)
	{
		StopBubbleEffect();
		if (((Component)this).DesignMode)
		{
			RecalcLayout();
			((Control)this).Refresh();
		}
	}

	internal void _2802(BubbleBarTab ᆰ, BubbleButton ᤉ)
	{
		StopBubbleEffect();
		if (_27E5 && !((Control)this).IsDisposed)
		{
			_2803();
		}
		if (((Component)this).DesignMode)
		{
			RecalcLayout();
			((Control)this).Refresh();
		}
	}

	internal void _2803()
	{
		_27E5 = false;
		foreach (BubbleBarTab tab in Tabs)
		{
			foreach (BubbleButton button in tab.Buttons)
			{
				if (button.Shortcut != 0)
				{
					_27E5 = true;
					break;
				}
			}
			if (_27E5)
			{
				break;
			}
		}
	}

	internal void _2804(BubbleBarTab ᆰ, BubbleButton ᤉ)
	{
		StopBubbleEffect();
		_2806();
		if (ᤉ.Shortcut != 0)
		{
			_27E5 = true;
		}
		if (((Component)this).DesignMode)
		{
			RecalcLayout();
			((Control)this).Refresh();
		}
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		((Control)this).OnHandleCreated(e);
		RecalcLayout();
		_2818();
	}

	protected override void OnHandleDestroyed(EventArgs e)
	{
		((Control)this).OnHandleDestroyed(e);
		_2819();
		_27FC();
	}

	internal void _2805(BubbleButton ᤉ)
	{
		_2806();
	}

	private void _2806()
	{
		if (ᓻ != null)
		{
			if (ᓻ.Buttons.Count > 0)
			{
				IBlock[] array = new IBlock[ᓻ.Buttons.Count];
				ᓻ.Buttons._0CD0(array);
				Rectangle rectangle = _27CD.Layout(_2808(), array, _27CE);
				if (_27D2 != null)
				{
					_27D8.X = rectangle.X;
					_27D8.Width = rectangle.Width;
				}
				else
				{
					_27D8 = rectangle;
				}
			}
			else
			{
				_27D8 = _2808();
				_27D8.Inflate(-_27D8.Width / 2, 0);
				int bottom = _27D8.Bottom;
				Size size = Dpi.ImageSize(_27CC);
				_27D8.Height = size.Height;
				if (Ḁ == eBubbleButtonAlignment.Bottom)
				{
					_27D8.Y = bottom - _27D8.Height;
				}
			}
		}
		_2807();
	}

	private void _2807()
	{
		if (!BarFunctions.IsHandleValid((Control)(object)this))
		{
			return;
		}
		if (_27DC && ᓺ.Count > 0)
		{
			IBlock[] array = new IBlock[ᓺ.Count];
			ᓺ._0CD0(array);
			Graphics val = null;
			try
			{
				val = ((Control)this).CreateGraphics();
				_27DE.Graphics = val;
				_27DD = _27E0.Layout(_280A(), array, _27DE);
				return;
			}
			finally
			{
				_27DE.Graphics = null;
				if (val != null)
				{
					val.Dispose();
				}
			}
		}
		_27DD = Rectangle.Empty;
	}

	private Rectangle _2808()
	{
		Rectangle displayRectangle = ((Control)this).DisplayRectangle;
		if (_27D2 != null)
		{
			displayRectangle = ((Control)_27D2).DisplayRectangle;
		}
		if (Ḁ == eBubbleButtonAlignment.Bottom)
		{
			displayRectangle.Height -= ℯ;
		}
		else if (Ḁ == eBubbleButtonAlignment.Top)
		{
			displayRectangle.Height -= ℯ;
			displayRectangle.Y += ℯ;
		}
		return displayRectangle;
	}

	private Rectangle _2809()
	{
		Rectangle result = _27D8;
		if (_27E4)
		{
			result.Width = ((Control)this).Width;
			result.X = 0;
		}
		else if (_27DD.Width + 16 > result.Width)
		{
			int num = _27DD.Width + 16 - result.Width;
			result.Width += num;
			result.X -= num / 2;
		}
		result.Height += _27E3.PaddingTop + _27E3.PaddingBottom;
		result.Y -= _27E3.PaddingTop;
		if (!_27E4)
		{
			result.Width += _27E3.PaddingLeft + _27E3.PaddingRight;
			result.X -= _27E3.PaddingLeft;
		}
		return result;
	}

	private Rectangle _280A()
	{
		Rectangle rectangle = _2809();
		Rectangle result = ((Ḁ != eBubbleButtonAlignment.Bottom) ? new Rectangle(rectangle.X, rectangle.Bottom, rectangle.Width, ((Control)this).Height - rectangle.Bottom) : new Rectangle(rectangle.X, 0, rectangle.Width, rectangle.Y));
		if (result.Height < 0)
		{
			result.Height = 0;
		}
		result.Width -= 16;
		result.X += 8;
		return result;
	}

	private Rectangle _220B()
	{
		Rectangle result = _27DD;
		if (!result.IsEmpty)
		{
			result.Width += 16;
			result.X -= 8;
		}
		return result;
	}

	private void _280B(BubbleButton ᤉ, Point _1DC3, bool _280C)
	{
		if (ᇰ <= 0 || _27D3 || ᓻ == null || !ᇯ)
		{
			return;
		}
		_27D3 = true;
		try
		{
			_27CD.MouseOverIndex = ᓻ.Buttons.IndexOf(ᤉ);
			bool flag = true;
			Size size = Dpi.ImageSize(_27CB);
			Size size2 = Dpi.ImageSize(_27CC);
			int num = size.Width - size2.Width;
			int num2 = 1;
			int num3 = size2.Width + num2;
			if (!_280C)
			{
				num3 = size.Width - num2;
			}
			Rectangle containerBounds = _2808();
			IBlock[] array = new IBlock[ᓻ.Buttons.Count];
			ᓻ.Buttons._0CD0(array);
			DateTime now = DateTime.Now;
			TimeSpan minValue = TimeSpan.MinValue;
			while (flag)
			{
				DateTime now2 = DateTime.Now;
				float num4 = (float)num3 / (float)size2.Width;
				Size size3 = new Size((int)((float)size2.Width * num4), (int)((float)size2.Height * num4));
				_2839 _2839 = _280D(ᤉ.DisplayRectangle, _1DC3, size3);
				_27CD.Factor1 = _2839._283A;
				_27CD.Factor2 = _2839._283B;
				_27CD.Factor3 = _2839._283C;
				_27CD.Factor4 = _2839._283D;
				_27CD.BubbleSize = size3;
				_27CD.MouseOverPosition = _2839.و;
				_27CD.Layout(containerBounds, array, _27CE);
				_27F1();
				minValue = DateTime.Now.Subtract(now2);
				num2 = (int)((float)num * ((float)minValue.TotalMilliseconds / (float)ᇰ));
				if (num2 <= 0)
				{
					int num5 = (int)((float)ᇰ / Math.Max((float)minValue.TotalMilliseconds, 1f) / (float)num);
					if (num5 <= 0)
					{
						num5 = (int)minValue.TotalMilliseconds;
					}
					try
					{
						using ManualResetEvent manualResetEvent = new ManualResetEvent(initialState: false);
						manualResetEvent.WaitOne(num5);
					}
					catch
					{
						break;
					}
					num2 = 1;
				}
				num3 = ((!_280C) ? (num3 - num2) : (num3 + num2));
				num -= num2;
				if (num <= 0 || DateTime.Now.Subtract(now).TotalMilliseconds >= (double)ᇰ)
				{
					break;
				}
			}
		}
		finally
		{
			_27D3 = false;
		}
	}

	private _2839 _280D(Rectangle _280E, Point _1DC3)
	{
		return _280D(_280E, _1DC3, Dpi.ImageSize(_27CB));
	}

	private _2839 _280D(Rectangle _280E, Point _1DC3, Size _0CC7)
	{
		_2839 _2839 = new _2839();
		float num = 0f;
		int و = 0;
		int ى = 0;
		int val = Math.Max(_1DC3.X - _280E.X, 0);
		val = Math.Min(val, _280E.Width);
		int val2 = Math.Max(_1DC3.Y - _280E.Y, 0);
		val2 = Math.Min(val2, _280E.Height);
		if (Ⴐ == eOrientation.Horizontal)
		{
			num = (float)val / (float)_280E.Width;
			و = _1DC3.X - (int)(num * (float)_0CC7.Width);
		}
		else
		{
			num = (float)val2 / (float)_280E.Height;
			ى = _1DC3.Y - (int)(num * (float)_0CC7.Height);
		}
		_2839.و = و;
		_2839.ى = ى;
		_2839._283A = (1f - num) * 0.3f;
		_2839._283B = Math.Max(1f - num, 0.3f);
		_2839._283C = Math.Max(num, 0.3f);
		_2839._283D = num * 0.3f;
		return _2839;
	}

	private void _280F()
	{
		_27E0.BlockSpacing = 8;
		if (Ḁ == eBubbleButtonAlignment.Bottom)
		{
			_27CD.ContentAlignment = eContentAlignment.Center;
			_27CD.ContentVerticalAlignment = eContentVerticalAlignment.Bottom;
			_27CD.BlockLineAlignment = eContentVerticalAlignment.Bottom;
			_27CD.ContentOrientation = eContentOrientation.Horizontal;
			_27E0.ContentAlignment = eContentAlignment.Center;
			_27E0.ContentVerticalAlignment = eContentVerticalAlignment.Bottom;
			_27E0.BlockLineAlignment = eContentVerticalAlignment.Bottom;
			_27E0.ContentOrientation = eContentOrientation.Horizontal;
		}
		else if (Ḁ == eBubbleButtonAlignment.Top)
		{
			_27CD.ContentAlignment = eContentAlignment.Center;
			_27CD.ContentVerticalAlignment = eContentVerticalAlignment.Top;
			_27CD.BlockLineAlignment = eContentVerticalAlignment.Top;
			_27CD.ContentOrientation = eContentOrientation.Horizontal;
			_27E0.ContentAlignment = eContentAlignment.Center;
			_27E0.ContentVerticalAlignment = eContentVerticalAlignment.Top;
			_27E0.BlockLineAlignment = eContentVerticalAlignment.Top;
			_27E0.ContentOrientation = eContentOrientation.Horizontal;
		}
	}

	internal void _2810(BubbleBarTab ᆰ)
	{
		if (ᓻ == ᆰ)
		{
			_2814(ᆰ, eEventSource.Code, _2815: false);
		}
		if (((Component)this).DesignMode)
		{
			RecalcLayout();
			((Control)this).Refresh();
		}
	}

	internal void ᕥ(BubbleBarTab ᆰ)
	{
		if (ᓻ == null)
		{
			ᓻ = ᆰ;
		}
		_2806();
		if (((Component)this).DesignMode)
		{
			RecalcLayout();
			((Control)this).Refresh();
		}
	}

	internal void _2811()
	{
		ᓻ = null;
		_2806();
		if (((Component)this).DesignMode)
		{
			RecalcLayout();
			((Control)this).Refresh();
		}
	}

	internal void _2812(BubbleBarTab ᆰ)
	{
		RecalcLayout();
		((Control)this).Refresh();
	}

	internal void ᕛ(BubbleBarTab ᆰ)
	{
		if (ᓻ == ᆰ)
		{
			_2814(ᆰ, eEventSource.Code, _2815: false);
		}
		_2806();
		((Control)this).Refresh();
	}

	private void _1FB5()
	{
		StopBubbleEffect();
		_27E2 = null;
		_2806();
		((Control)this).Refresh();
	}

	internal BubbleBarTab _2813()
	{
		return _27D9;
	}

	private void _2814(BubbleBarTab _1772, eEventSource ᎂ, bool _2815)
	{
		BubbleBarTab bubbleBarTab = ᓺ.ᾬ(_1772);
		if (bubbleBarTab == null)
		{
			bubbleBarTab = ᓺ._281B(_1772);
		}
		_2816(bubbleBarTab, ᎂ, _2815);
	}

	private void _2816(BubbleBarTab ᆰ, eEventSource ᎂ, bool _2815)
	{
		if (_27E8 != null)
		{
			BubbleBarTabChangingEventArgs bubbleBarTabChangingEventArgs = new BubbleBarTabChangingEventArgs();
			bubbleBarTabChangingEventArgs.CurrentTab = ᓻ;
			bubbleBarTabChangingEventArgs.NewTab = ᆰ;
			bubbleBarTabChangingEventArgs.Source = ᎂ;
			_27E8(this, bubbleBarTabChangingEventArgs);
			if (bubbleBarTabChangingEventArgs.Cancel && _2815)
			{
				return;
			}
		}
		ᓻ = ᆰ;
		_27E2 = null;
		_2806();
		_27F1();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void SetMouseOverTab(BubbleBarTab tab)
	{
		if (_27D9 != tab)
		{
			_27D9 = tab;
			((Control)this).Invalidate(_220B());
			((Control)this).Update();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void StartDrag(BubbleBarTab tab)
	{
		if (!_09E2)
		{
			_09E2 = true;
			_27EC = tab;
			Cursor.Current = Cursors.Hand;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void StartDrag(BubbleButton button)
	{
		if (!_09E2)
		{
			_09E2 = true;
			_27EF = button;
			Cursor.Current = Cursors.Hand;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void DragMouseMove(Point mousePosition)
	{
		if (_27EC != null)
		{
			BubbleBarTab tabAt = GetTabAt(mousePosition);
			if (tabAt != null && tabAt != _27EC)
			{
				if (_27ED == -1)
				{
					_27ED = Tabs.IndexOf(_27EC);
				}
				int index = Tabs.IndexOf(tabAt);
				Tabs.Remove(_27EC);
				Tabs.Insert(index, _27EC);
				SelectedTab = _27EC;
			}
		}
		else
		{
			if (_27EF == null)
			{
				return;
			}
			BubbleBarTab tabAt2 = GetTabAt(mousePosition);
			if (tabAt2 != null && tabAt2 != _27EF.Parent)
			{
				if (_27ED == -1)
				{
					_27ED = Tabs.IndexOf(_27EF.Parent);
				}
				if (_27EE == -1)
				{
					_27EE = _27EF.Parent.Buttons.IndexOf(_27EF);
				}
				SelectedTab = tabAt2;
				BubbleButton buttonAt = GetButtonAt(mousePosition);
				if (_27EF.Parent != null)
				{
					_27EF.Parent.Buttons.Remove(_27EF);
				}
				if (buttonAt != null)
				{
					tabAt2.Buttons.Insert(tabAt2.Buttons.IndexOf(buttonAt), _27EF);
				}
				else
				{
					tabAt2.Buttons.Add(_27EF);
				}
				RecalcLayout();
				((Control)this).Refresh();
				return;
			}
			BubbleButton buttonAt2 = GetButtonAt(mousePosition);
			if (buttonAt2 != null && buttonAt2 != _27EF)
			{
				if (_27ED == -1)
				{
					_27ED = Tabs.IndexOf(_27EF.Parent);
				}
				if (_27EE == -1)
				{
					_27EE = _27EF.Parent.Buttons.IndexOf(_27EF);
				}
				BubbleBarTab parent = _27EF.Parent;
				int index2 = parent.Buttons.IndexOf(buttonAt2);
				parent.Buttons.Remove(_27EF);
				parent.Buttons.Insert(index2, _27EF);
				RecalcLayout();
				((Control)this).Refresh();
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void DragMouseUp(Point mousePosition)
	{
		if (_27EC != null)
		{
			BubbleBarTab tabAt = GetTabAt(mousePosition);
			if (tabAt != _27EC)
			{
				_2817();
			}
			else
			{
				_27EC = null;
				_27ED = -1;
			}
		}
		else if (_27EF != null)
		{
			BubbleButton buttonAt = GetButtonAt(mousePosition);
			if (buttonAt != _27EF)
			{
				_2817();
			}
			else
			{
				_27EF = null;
				_27ED = -1;
				_27EE = -1;
			}
		}
		_09E2 = false;
	}

	internal void _2817()
	{
		if (!_09E2)
		{
			return;
		}
		if (_27EC != null)
		{
			if (_27ED >= 0)
			{
				Tabs.Remove(_27EC);
				Tabs.Insert(_27ED, _27EC);
				RecalcLayout();
				((Control)this).Refresh();
			}
			_27EC = null;
			_27ED = -1;
		}
		else if (_27EF != null && _27EE >= 0)
		{
			if (_27EF.Parent != null)
			{
				_27EF.Parent.Buttons.Remove(_27EF);
			}
			if (_27ED >= 0 && _27ED != Tabs.IndexOf(_27EF.Parent))
			{
				Tabs[_27ED].Buttons.Insert(_27EE, _27EF);
			}
			else
			{
				Tabs[_27ED].Buttons.Insert(_27EE, _27EF);
			}
			_27EE = -1;
			_27ED = -1;
			_27EF = null;
			RecalcLayout();
			((Control)this).Refresh();
		}
		_09E2 = false;
	}

	private void ህ()
	{
	}

	void ISupportInitialize.BeginInit()
	{
		//ILSpy generated this explicit interface implementation from .override directive in ህ
		this.ህ();
	}

	private void ሆ()
	{
	}

	void ISupportInitialize.EndInit()
	{
		//ILSpy generated this explicit interface implementation from .override directive in ሆ
		this.ሆ();
	}

	private bool _0A64(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		return false;
	}

	bool ሎ._੨(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੤
		return this._0A64(_0A65, _੦, _੧);
	}

	private bool _੩(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected I4, but got Unknown
		int num = _0EFE._0F11(_੦);
		if ((int)Control.ModifierKeys != 0 || (num >= 112 && num <= 123))
		{
			int _੬ = Control.ModifierKeys | num;
			if (_੫((eShortcut)_੬))
			{
				return true;
			}
		}
		return false;
	}

	bool ሎ._੪(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੩
		return this._੩(_0A65, _੦, _੧);
	}

	private bool _੫(eShortcut _੬)
	{
		Form val = ((Control)this).FindForm();
		if (val == null || (((val != Form.ActiveForm && val.MdiParent == null) || (val.MdiParent != null && val.MdiParent.ActiveMdiChild != val)) && !val.IsMdiContainer) || (Form.ActiveForm != null && Form.ActiveForm.Modal && Form.ActiveForm != val))
		{
			return false;
		}
		if (_27E5)
		{
			foreach (BubbleBarTab tab in Tabs)
			{
				foreach (BubbleButton button in tab.Buttons)
				{
					if (button.Shortcut == _੬ && button.Enabled)
					{
						button.InvokeClick(eEventSource.Keyboard, (MouseButtons)0);
						return true;
					}
				}
			}
		}
		return false;
	}

	private bool _੮(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		return false;
	}

	bool ሎ.ሏ(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੮
		return this._੮(_0A65, _੦, _੧);
	}

	private bool _੯(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		return false;
	}

	bool ሎ.ሐ(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੯
		return this._੯(_0A65, _੦, _੧);
	}

	private bool _0A70(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected I4, but got Unknown
		if (!((Component)this).DesignMode)
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

	bool ሎ._0A71(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੰ
		return this._0A70(_0A65, _੦, _੧);
	}

	private bool ੲ(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		return false;
	}

	bool ሎ.ੳ(IntPtr _0A65, IntPtr _੦, IntPtr _੧)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ੲ
		return this.ੲ(_0A65, _੦, _੧);
	}

	private void _2818()
	{
		if (!_২ && !((Component)this).DesignMode)
		{
			_1BE6._1BEA(this);
			_২ = true;
		}
	}

	private void _2819()
	{
		if (_২)
		{
			_1BE6._1BEB(this);
			_২ = false;
		}
	}
}
