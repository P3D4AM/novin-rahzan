using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;
using DevComponents.DotNetBar.TextMarkup;

namespace DevComponents.DotNetBar;

[Designer("DevComponents.DotNetBar.Design.SuperTabItemDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
public class SuperTabItem : BaseItem
{
	private MarkupLinkClickEventHandler Ӟ;

	private EventHandler<EventArgs> Ὃ;

	private Icon _Ὄ;

	private Image _ነ;

	private int _Ὅ = -1;

	private Rectangle __1F4E;

	private eTabItemColor __1F4F;

	private SuperTabItemColorTable _ὐ = new SuperTabItemColorTable();

	private bool __0EB9 = true;

	private eItemAlignment? _ᬭ;

	private Rectangle _ὑ;

	private Font _ὒ;

	private Font _ὓ;

	private Font _ὔ;

	private bool __137B = true;

	private bool _ὕ;

	private bool _ὖ;

	private Rectangle __1378;

	private SuperTabStripItem _ὗ;

	private Control __1F58;

	private SuperTabItemBaseDisplay _Ὑ;

	private Font __1F5A;

	private bool __0EB8;

	private bool _ຫ;

	private ImageAlignment __1712 = ImageAlignment.NotSet;

	private Padding _Ὓ;

	private Size __1F5C = Size.Empty;

	private Color __0EA4 = Color.Empty;

	private string _ລ = "";

	private float _ຨ;

	private bool _Ὕ;

	private string __0EA6 = "";

	private eSymbolSet _ວ;

	protected override bool IsMarkupSupported => __0EB9;

	protected Font DefaultSelectedTabFont
	{
		get
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Expected O, but got Unknown
			if (__1F5A == null)
			{
				__1F5A = new Font(SystemFonts.CaptionFont, (FontStyle)1);
			}
			return __1F5A;
		}
		set
		{
			if (__1F5A != null)
			{
				__1F5A.Dispose();
			}
			__1F5A = value;
			_ὔ = null;
		}
	}

	internal bool _๗
	{
		get
		{
			if (__0EB8)
			{
				return _ຫ;
			}
			return false;
		}
	}

	internal Rectangle _1369
	{
		get
		{
			if (__1378.IsEmpty)
			{
				__1378 = ὣ();
			}
			return __1378;
		}
	}

	internal bool ὃ
	{
		get
		{
			return _ὕ;
		}
		set
		{
			if (_ὕ != value)
			{
				_ὕ = value;
				Refresh();
			}
		}
	}

	internal bool ὄ
	{
		get
		{
			return _ὖ;
		}
		set
		{
			if (_ὖ != value)
			{
				_ὖ = value;
				Refresh();
			}
		}
	}

	internal ImageAlignment ὅ
	{
		get
		{
			if (ImageAlignment != ImageAlignment.NotSet)
			{
				return ImageAlignment;
			}
			if (TabStripItem.ImageAlignment != ImageAlignment.NotSet)
			{
				return TabStripItem.ImageAlignment;
			}
			return ImageAlignment.MiddleLeft;
		}
	}

	internal Rectangle _1759
	{
		get
		{
			if (__1F4E.IsEmpty)
			{
				__1F4E = ὤ();
			}
			return __1F4E;
		}
	}

	internal bool ᓙ
	{
		get
		{
			if (!_ὗ.HorizontalText)
			{
				if (_ὗ.TabAlignment != 0)
				{
					return _ὗ.TabAlignment == eTabStripAlignment.Right;
				}
				return true;
			}
			return false;
		}
	}

	internal Font _1F46
	{
		get
		{
			object obj = _ὓ ?? _ὗ.SelectedTabFont;
			if (obj == null)
			{
				obj = DefaultSelectedTabFont;
			}
			return (Font)obj;
		}
	}

	internal Font _1F47
	{
		get
		{
			return _ὔ;
		}
		set
		{
			_ὔ = value;
		}
	}

	internal SuperTabItemBaseDisplay Ὀ => _Ὑ;

	internal SuperTabStrip Ὁ
	{
		get
		{
			if (TabStripItem == null)
			{
				return null;
			}
			return TabStripItem.Ὁ;
		}
	}

	internal Rectangle _175A
	{
		get
		{
			if (_ὑ.IsEmpty)
			{
				_ὑ = ὧ();
			}
			return _ὑ;
		}
	}

	internal Font Ὂ
	{
		get
		{
			object obj = _ὒ ?? _ὗ.TabFont;
			if (obj == null)
			{
				obj = SystemFonts.CaptionFont;
			}
			return (Font)obj;
		}
	}

	[DefaultValue(false)]
	[Browsable(true)]
	[Description("Indicates whether image animation is enabled.")]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	public bool EnableImageAnimation
	{
		get
		{
			return __0EB8;
		}
		set
		{
			if (__0EB8 != value)
			{
				__0EB8 = value;
				Ὤ();
			}
		}
	}

	[Category("Style")]
	[Browsable(true)]
	[Description("Contains user specified tab display colors.")]
	public SuperTabItemColorTable TabColor
	{
		get
		{
			return _ὐ;
		}
		set
		{
			if (!_ὐ.Equals(value))
			{
				if (_ὐ != null)
				{
					_ὐ.ColorTableChanged -= Ὗ;
				}
				_ὐ = value;
				if (value != null)
				{
					_ὐ.ColorTableChanged += Ὗ;
				}
				OnTabColorChanged();
				Refresh();
			}
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates whether Close button on the tab is visible when SuperTabStrip.CloseButtonOnTabsVisible property is set to true.")]
	[DefaultValue(true)]
	public bool CloseButtonVisible
	{
		get
		{
			return __137B;
		}
		set
		{
			if (__137B != value)
			{
				__137B = value;
				Ὤ();
			}
		}
	}

	[Category("Behavior")]
	[DefaultValue(true)]
	[Description("Indicates whether the tab is enabled.")]
	[Browsable(true)]
	public override bool Enabled
	{
		get
		{
			return base.Enabled;
		}
		set
		{
			if (base.Enabled != value)
			{
				base.Enabled = value;
				if (_ὗ != null && !value && IsSelected && !DesignMode)
				{
					_ὗ.ᾨ(this);
				}
				Ὤ();
			}
		}
	}

	[DefaultValue(true)]
	[Description("Indicates whether text-markup support is enabled for the control's Text property.")]
	[Category("Appearance")]
	public bool EnableMarkup
	{
		get
		{
			return __0EB9;
		}
		set
		{
			if (__0EB9 != value)
			{
				__0EB9 = value;
				NeedRecalcSize = true;
				OnTextChanged();
			}
		}
	}

	public Size FixedTabSize
	{
		get
		{
			return __1F5C;
		}
		set
		{
			value.Width = Math.Max(0, value.Width);
			value.Height = Math.Max(0, value.Height);
			if (__1F5C != value)
			{
				__1F5C = value;
				Ὤ();
			}
		}
	}

	[DefaultValue(null)]
	[Description("Indicates the tab icon. Icon has same functionality as Image except that it supports Alpha blending.")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Category("Appearance")]
	public Icon Icon
	{
		get
		{
			return _Ὄ;
		}
		set
		{
			if (_Ὄ != value)
			{
				_Ὄ = value;
				Ὤ();
			}
		}
	}

	[Description("Indicates the tab image.")]
	[Browsable(true)]
	[Category("Appearance")]
	[DevCoBrowsable(true)]
	[DefaultValue(null)]
	public Image Image
	{
		get
		{
			return _ነ;
		}
		set
		{
			if (_ነ != value)
			{
				_ነ = value;
				ᡁ();
			}
		}
	}

	[Editor("DevComponents.DotNetBar.Design.ImageIndexEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[TypeConverter(typeof(ImageIndexConverter))]
	[Category("Appearance")]
	[Description("Indicates the tab image index")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(-1)]
	public int ImageIndex
	{
		get
		{
			return _Ὅ;
		}
		set
		{
			if (_Ὅ != value)
			{
				_Ὅ = value;
				ᡁ();
			}
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public ImageList ImageList
	{
		get
		{
			if (_ὗ != null && _ὗ.Ὁ != null)
			{
				return _ὗ.Ὁ.ImageList;
			}
			return null;
		}
	}

	[Category("Appearance")]
	[DefaultValue(ImageAlignment.NotSet)]
	[Description("Indicates the alignment of the Image within the tab.")]
	public ImageAlignment ImageAlignment
	{
		get
		{
			return __1712;
		}
		set
		{
			if (__1712 != value)
			{
				__1712 = value;
				Ὤ();
			}
		}
	}

	[Description("Indicates the spacing between content and edges of the Image")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public Padding ImagePadding
	{
		get
		{
			if (_Ὓ == null)
			{
				_Ὓ = new Padding(0);
				UpdateChangeHandler(null, _Ὓ);
			}
			return _Ὓ;
		}
		set
		{
			if (_Ὓ != value)
			{
				UpdateChangeHandler(_Ὓ, value);
				_Ὓ = value;
				Ὤ();
			}
		}
	}

	[DefaultValue(eTabItemColor.Default)]
	[Browsable(true)]
	[Description("Indicates the predefined color for the tab.")]
	[Category("Style")]
	public eTabItemColor PredefinedColor
	{
		get
		{
			return __1F4F;
		}
		set
		{
			__1F4F = value;
			OnTabColorChanged();
		}
	}

	[Description("Indicates the selected tab Font")]
	[Browsable(true)]
	[DefaultValue(null)]
	[DevCoBrowsable(true)]
	[Category("Style")]
	public Font SelectedTabFont
	{
		get
		{
			return _ὓ;
		}
		set
		{
			if (_ὓ != value)
			{
				if (value != null)
				{
					DefaultSelectedTabFont = null;
				}
				_ὓ = value;
				_ὔ = null;
				Ὤ();
			}
		}
	}

	[Description("Indicates whether the tab starts a new tab line.")]
	[Category("Behavior")]
	[Browsable(true)]
	[DefaultValue(false)]
	public bool StartsNewTabLine
	{
		get
		{
			return _Ὕ;
		}
		set
		{
			if (value != _Ὕ)
			{
				_Ὕ = value;
				Ὤ();
			}
		}
	}

	[Browsable(false)]
	public string SymbolRealized => __0EA6;

	[Category("Appearance")]
	[DefaultValue("")]
	[Description("Indicates the symbol displayed on face of the button instead of the image. Setting the symbol overrides the image setting.")]
	[Editor("DevComponents.DotNetBar.Design.SymbolTypeEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	public string Symbol
	{
		get
		{
			return _ລ;
		}
		set
		{
			if (value != _ລ)
			{
				string ລ = _ລ;
				_ລ = value;
				OnSymbolChanged(ລ, value);
			}
		}
	}

	[DefaultValue(eSymbolSet.Awesome)]
	[Browsable(false)]
	public eSymbolSet SymbolSet
	{
		get
		{
			return _ວ;
		}
		set
		{
			if (_ວ != value)
			{
				eSymbolSet ວ = _ວ;
				_ວ = value;
				OnSymbolSetChanged(ວ, value);
			}
		}
	}

	[Category("Appearance")]
	[Description("Indicates color of the Symbol.")]
	public Color SymbolColor
	{
		get
		{
			return __0EA4;
		}
		set
		{
			__0EA4 = value;
			Refresh();
		}
	}

	[DefaultValue(0f)]
	[Description("Indicates the size of the symbol in points.")]
	[Category("Appearance")]
	public float SymbolSize
	{
		get
		{
			return _ຨ;
		}
		set
		{
			if (value != _ຨ)
			{
				float ຨ = _ຨ;
				_ຨ = value;
				OnSymbolSizeChanged(ຨ, value);
			}
		}
	}

	[Category("Layout")]
	[Browsable(true)]
	[DefaultValue(null)]
	[DevCoBrowsable(true)]
	[Description("Indicates text alignment.")]
	public eItemAlignment? TextAlignment
	{
		get
		{
			return _ᬭ;
		}
		set
		{
			if (_ᬭ != value)
			{
				_ᬭ = value;
				Refresh();
			}
		}
	}

	[DevCoBrowsable(true)]
	[Category("Style")]
	[Browsable(true)]
	[DefaultValue(null)]
	[Description("Indicates the tab Font")]
	public Font TabFont
	{
		get
		{
			return _ὒ;
		}
		set
		{
			if (_ὒ != value)
			{
				_ὒ = value;
				_ὔ = null;
				Ὤ();
			}
		}
	}

	[Browsable(true)]
	[Editor("DevComponents.DotNetBar.Design.TextMarkupUIEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Category("Appearance")]
	[Description("Indicates the tab text.")]
	public override string Text
	{
		get
		{
			return base.Text;
		}
		set
		{
			base.Text = value;
		}
	}

	[DevCoBrowsable(true)]
	[DefaultValue(null)]
	[Browsable(false)]
	[Category("Behavior")]
	public Control AttachedControl
	{
		get
		{
			return __1F58;
		}
		set
		{
			if (__1F58 is SuperTabControlPanel superTabControlPanel)
			{
				superTabControlPanel.PanelColorChanged -= Ὗ;
			}
			__1F58 = value;
			if (!(__1F58 is SuperTabControlPanel superTabControlPanel2))
			{
				return;
			}
			superTabControlPanel2.TabItem = this;
			((Control)superTabControlPanel2).Dock = (DockStyle)5;
			superTabControlPanel2.PanelColorChanged += Ὗ;
			if (_ὗ != null && _ὗ.Ὁ != null)
			{
				if (DesignMode)
				{
					((Control)_ὗ.Ὁ).Controls.Add((Control)(object)superTabControlPanel2);
				}
				((Control)superTabControlPanel2).BringToFront();
				((Control)superTabControlPanel2).Visible = _ὗ.SelectedTab == this;
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public override Rectangle Bounds
	{
		get
		{
			return base.Bounds;
		}
		set
		{
			base.Bounds = value;
			_ὑ = Rectangle.Empty;
			__1378 = Rectangle.Empty;
			__1F4E = Rectangle.Empty;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool IsMouseOver
	{
		get
		{
			if (_ὗ != null)
			{
				return _ὗ.ᓤ == this;
			}
			return false;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool IsSelected
	{
		get
		{
			if (_ὗ != null)
			{
				return _ὗ.SelectedTab == this;
			}
			return false;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public eTabStripAlignment TabAlignment
	{
		get
		{
			if (_ὗ != null)
			{
				return _ὗ.TabAlignment;
			}
			return eTabStripAlignment.Top;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public Rectangle ContentRectangle
	{
		get
		{
			if (_Ὑ != null)
			{
				return _Ὑ._1FDD();
			}
			return Rectangle.Empty;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public SuperTabStripItem TabStripItem
	{
		get
		{
			return _ὗ;
		}
		set
		{
			_ὗ = value;
			if (_ὗ != null)
			{
				SetOwner(_ὗ.GetOwner());
			}
			if (_ὗ != null)
			{
				TabStyle = _ὗ.TabStyle;
				ContainerControl = _ὗ.ContainerControl;
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public eSuperTabStyle TabStyle
	{
		get
		{
			if (_ὗ != null)
			{
				return _ὗ.TabStyle;
			}
			return eSuperTabStyle.Office2007;
		}
		internal set
		{
			if (_Ὑ != null)
			{
				_Ὑ.Dispose();
			}
			switch (value)
			{
			case eSuperTabStyle.Office2007:
				_Ὑ = new Office2007SuperTabItem(this);
				break;
			case eSuperTabStyle.Office2010BackstageBlue:
				_Ὑ = new Office2010BackstageSuperTabItem(this);
				break;
			case eSuperTabStyle.OneNote2007:
				_Ὑ = new OneNote2007SuperTabItem(this);
				break;
			case eSuperTabStyle.OfficeMobile2014:
				_Ὑ = new OfficeMobile2014SuperTabItem(this);
				break;
			case eSuperTabStyle.VisualStudio2008Dock:
				_Ὑ = new VS2008DockSuperTabItem(this);
				break;
			case eSuperTabStyle.VisualStudio2008Document:
				_Ὑ = new VS2008DocumentSuperTabItem(this);
				break;
			case eSuperTabStyle.WinMediaPlayer12:
				_Ὑ = new WinMediaPlayer12SuperTabItem(this);
				break;
			}
		}
	}

	public override bool Visible
	{
		get
		{
			return base.Visible;
		}
		set
		{
			base.Visible = value;
			if (_ὗ != null)
			{
				if (!value && IsSelected && !DesignMode)
				{
					_ὗ.ᾨ(this);
				}
				Ὤ();
			}
		}
	}

	[Description("Occurs when text markup link is clicked.")]
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

	[Description("Occurs when the tab colors have changed.")]
	public event EventHandler<EventArgs> TabColorChanged
	{
		add
		{
			EventHandler<EventArgs> eventHandler = Ὃ;
			EventHandler<EventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<EventArgs> value2 = (EventHandler<EventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref Ὃ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<EventArgs> eventHandler = Ὃ;
			EventHandler<EventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<EventArgs> value2 = (EventHandler<EventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref Ὃ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public SuperTabItem()
	{
		Visible = true;
		GlobalItem = false;
		Stretch = false;
		Displayed = true;
		ἧ(Ἠ: true);
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTabColor()
	{
		return !_ὐ.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public void ResetTabColor()
	{
		TabColor = new SuperTabItemColorTable();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeFixedTabSize()
	{
		return !__1F5C.IsEmpty;
	}

	[DevCoBrowsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public void ResetIcon()
	{
		Icon = null;
	}

	private void ᡁ()
	{
		if (__0EB8 && _Ὑ != null)
		{
			_Ὑ.ໂ();
		}
		Image val = _0EC5();
		_ຫ = ImageAnimator.CanAnimate(val);
		Ὤ();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[DevCoBrowsable(false)]
	[Browsable(false)]
	public void ResetImageIndex()
	{
		ImageIndex = -1;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	private bool ShouldSerializeImagePadding()
	{
		if (_Ὓ != null)
		{
			return !_Ὓ.IsEmpty;
		}
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	private void ResetImagePadding()
	{
		ImagePadding = null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public bool ShouldSerializeSelectedTabFont()
	{
		return _ὓ != null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetSelectedTabFont()
	{
		SelectedTabFont = null;
	}

	protected virtual void OnSymbolChanged(string oldValue, string newValue)
	{
		if (string.IsNullOrEmpty(newValue))
		{
			__0EA6 = "";
		}
		else
		{
			__0EA6 = Symbols.GetSymbol(newValue);
		}
		NeedRecalcSize = true;
		OnAppearanceChanged();
		Refresh();
	}

	protected virtual void OnSymbolSetChanged(eSymbolSet oldValue, eSymbolSet newValue)
	{
		NeedRecalcSize = true;
		OnAppearanceChanged();
		Refresh();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeSymbolColor()
	{
		return !__0EA4.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetSymbolColor()
	{
		SymbolColor = Color.Empty;
	}

	protected virtual void OnSymbolSizeChanged(float oldValue, float newValue)
	{
		NeedRecalcSize = true;
		OnAppearanceChanged();
		Refresh();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public bool ShouldSerializeTabFont()
	{
		return _ὒ != null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetTabFont()
	{
		TabFont = null;
	}

	private void ἧ(bool Ἠ)
	{
		if (Ἠ)
		{
			base.TextChanged += _1F5E;
			MarkupLinkClick += TextMarkupLinkClick;
			_ὐ.ColorTableChanged += Ὗ;
			return;
		}
		base.TextChanged -= _1F5E;
		MarkupLinkClick -= TextMarkupLinkClick;
		if (_ὐ != null)
		{
			_ὐ.ColorTableChanged -= Ὗ;
		}
	}

	private void _1F5E(object _٠, EventArgs ؿ)
	{
		NeedRecalcSize = true;
		Refresh();
		if (_ὗ != null)
		{
			_ὗ.NeedRecalcSize = true;
			_ὗ.Refresh();
		}
	}

	protected override void TextMarkupLinkClick(object sender, EventArgs e)
	{
		if (sender is _0CFE obj)
		{
			OnMarkupLinkClick(new MarkupLinkClickEventArgs(obj._0D01, obj._0D00));
		}
		base.TextMarkupLinkClick(sender, e);
	}

	protected virtual void OnMarkupLinkClick(MarkupLinkClickEventArgs e)
	{
		if (Ӟ != null)
		{
			Ӟ(this, e);
		}
	}

	private void Ὗ(object _٠, EventArgs ؿ)
	{
		OnTabColorChanged();
	}

	protected void OnTabColorChanged()
	{
		if (Ὃ != null)
		{
			Ὃ(this, EventArgs.Empty);
		}
		Refresh();
	}

	public void Close()
	{
		_ὗ.CloseTab(this);
	}

	public Font GetTabFont()
	{
		if (!IsSelected)
		{
			return Ὂ;
		}
		return _1F46;
	}

	internal SuperTabItemStateColorTable ὠ()
	{
		return _Ὑ.ὠ();
	}

	internal SuperTabItemStateColorTable ὠ(eTabState ὡ)
	{
		return _Ὑ.ὠ(ὡ);
	}

	internal SuperTabItemStateColorTable ὢ(eTabState ὡ)
	{
		return _Ὑ.ὢ(ὡ);
	}

	internal SuperTabPanelItemColorTable ጩ()
	{
		return _Ὑ.ጩ();
	}

	public eSuperTabArea GetTabAreaFromPoint(Point pt)
	{
		if (Bounds.Contains(pt))
		{
			if (_1369.Contains(pt))
			{
				return eSuperTabArea.InCloseBox;
			}
			if (_1759.Contains(pt))
			{
				return eSuperTabArea.InImage;
			}
			return eSuperTabArea.InContent;
		}
		return eSuperTabArea.InNone;
	}

	public bool PointInTab(Point pt)
	{
		GraphicsPath tabItemPath = GetTabItemPath();
		try
		{
			if (tabItemPath.IsVisible(pt))
			{
				return true;
			}
		}
		finally
		{
			((IDisposable)tabItemPath)?.Dispose();
		}
		return false;
	}

	public GraphicsPath GetTabItemPath()
	{
		if (_Ὑ == null)
		{
			return null;
		}
		return _Ὑ._1FDE();
	}

	private Rectangle ὣ()
	{
		if (!CloseButtonVisible || _ὗ == null || !_ὗ.CloseButtonOnTabsVisible)
		{
			return Rectangle.Empty;
		}
		Rectangle rectangle = Ὀ._1FDD();
		Rectangle rectangle2 = rectangle;
		Size ὲ = _ὗ.ὲ;
		int tabHorizontalSpacing = _ὗ.TabHorizontalSpacing;
		if (ᓙ)
		{
			rectangle = ((TabStripItem.CloseButtonPosition != 0) ? new Rectangle(rectangle.X + (rectangle.Width - ὲ.Width) / 2 + 1, rectangle.Y + tabHorizontalSpacing, ὲ.Width, ὲ.Height) : new Rectangle(rectangle.X + (rectangle.Width - ὲ.Width) / 2 + 1, rectangle.Bottom - tabHorizontalSpacing - ὲ.Height - 1, ὲ.Width, ὲ.Height));
			if (TabStripItem.RotateVerticalText ^ (TabStripItem.TabAlignment == eTabStripAlignment.Right))
			{
				rectangle.X = rectangle2.X + (rectangle2.Right - rectangle.Right);
				rectangle.Y = rectangle2.Y + (rectangle2.Bottom - rectangle.Bottom);
			}
		}
		else if (_ὗ.CloseButtonPosition == eTabCloseButtonPosition.Left)
		{
			rectangle = new Rectangle(rectangle.X + tabHorizontalSpacing, rectangle.Y + (rectangle.Height - ὲ.Height) / 2, ὲ.Width, ὲ.Height);
			if (_ὗ.TabAlignment == eTabStripAlignment.Right && IsSelected)
			{
				rectangle.X += _ὗ.έ._1FFE;
			}
		}
		else
		{
			rectangle = new Rectangle(rectangle.Right - tabHorizontalSpacing - ὲ.Width, rectangle.Y + (rectangle.Height - ὲ.Height) / 2 + 1, ὲ.Width, ὲ.Height);
			if (_ὗ.TabAlignment == eTabStripAlignment.Left && IsSelected)
			{
				rectangle.X -= _ὗ.έ._1FFE;
			}
		}
		return _ὗ.ι(this, rectangle);
	}

	private Rectangle ὤ()
	{
		ᇎ ᇎ2 = ᕂ();
		if (string.IsNullOrEmpty(SymbolRealized) && ᇎ2 == null)
		{
			return Rectangle.Empty;
		}
		Rectangle ल = Ὀ._1FDD();
		ल = Ὠ(ल);
		ल = (ᓙ ? ὥ(ᇎ2, ल) : ὦ(ᇎ2, ल));
		return _ὗ._1FBD(this, ल);
	}

	private Rectangle ὥ(ᇎ ແ, Rectangle ल)
	{
		int num = 0;
		int num2 = 0;
		if (!string.IsNullOrEmpty(SymbolRealized))
		{
			Size size = _᧗();
			num2 = size.Width;
			num = size.Height;
		}
		else
		{
			num2 = Dpi.ImageWidth(ແ.ܐ);
			num = Dpi.ImageHeight(ແ._0711);
		}
		Padding padding = Dpi.Size(ImagePadding);
		Rectangle rectangle = ल;
		switch (ὅ)
		{
		case ImageAlignment.TopLeft:
			ल.X += padding.Top + TabStripItem.TabVerticalSpacing;
			ल.Y = ल.Bottom - (num2 + padding.Left);
			break;
		case ImageAlignment.TopCenter:
			ल.X += padding.Top + TabStripItem.TabVerticalSpacing;
			ल.Y += (ल.Height - (num2 + padding.Horizontal - TabStripItem.TabHorizontalSpacing)) / 2;
			break;
		case ImageAlignment.TopRight:
			ल.X += padding.Top + TabStripItem.TabVerticalSpacing;
			ल.Y += padding.Right + TabStripItem.TabHorizontalSpacing;
			break;
		case ImageAlignment.MiddleLeft:
			ल.X += (ल.Width - (num + padding.Vertical)) / 2 + padding.Top;
			ल.Y = ल.Bottom - (num2 + padding.Left);
			break;
		case ImageAlignment.MiddleCenter:
			ल.X += (ल.Width - (num + padding.Vertical)) / 2 + padding.Top;
			ल.Y += (ल.Height - (num2 + padding.Horizontal - TabStripItem.TabHorizontalSpacing)) / 2;
			break;
		case ImageAlignment.MiddleRight:
			ल.X += (ल.Width - (num + padding.Vertical)) / 2 + padding.Top;
			ल.Y += padding.Right + TabStripItem.TabHorizontalSpacing;
			break;
		case ImageAlignment.BottomLeft:
			ल.X = ल.Right - (num2 + padding.Right + TabStripItem.TabHorizontalSpacing);
			ल.Y = ल.Bottom - (num2 + padding.Left);
			break;
		case ImageAlignment.BottomCenter:
			ल.X = ल.Right - (num2 + padding.Right + TabStripItem.TabHorizontalSpacing);
			ल.Y += (ल.Height - (num2 + padding.Horizontal - TabStripItem.TabHorizontalSpacing)) / 2;
			break;
		case ImageAlignment.BottomRight:
			ल.X = ल.Right - (num2 + padding.Right + TabStripItem.TabHorizontalSpacing);
			ल.Y += padding.Right + TabStripItem.TabHorizontalSpacing;
			break;
		}
		if (TabStripItem.RotateVerticalText ^ (TabStripItem.TabAlignment == eTabStripAlignment.Right))
		{
			ल.X = rectangle.X + (rectangle.Right - (ल.X + num));
			ल.Y = rectangle.Y + (rectangle.Bottom - (ल.Y + num2));
		}
		ल.Width = num2;
		ल.Height = num;
		return ल;
	}

	private Rectangle ὦ(ᇎ ແ, Rectangle ल)
	{
		int num = 0;
		int num2 = 0;
		if (!string.IsNullOrEmpty(SymbolRealized))
		{
			Size size = _᧗();
			num2 = size.Width;
			num = size.Height;
		}
		else
		{
			num2 = ແ.ܐ;
			num = ແ._0711;
		}
		Padding imagePadding = ImagePadding;
		switch (ὅ)
		{
		case ImageAlignment.TopLeft:
			ल.X += imagePadding.Left;
			ल.Y += imagePadding.Top + TabStripItem.TabVerticalSpacing;
			break;
		case ImageAlignment.TopCenter:
			ल.X += (ल.Width - (num2 + imagePadding.Horizontal + TabStripItem.TabHorizontalSpacing)) / 2;
			ल.Y += imagePadding.Top + TabStripItem.TabVerticalSpacing;
			break;
		case ImageAlignment.TopRight:
			ल.X = ल.Right - num2 - imagePadding.Right - TabStripItem.TabHorizontalSpacing;
			ल.Y += imagePadding.Top + TabStripItem.TabVerticalSpacing;
			break;
		case ImageAlignment.MiddleLeft:
			ल.X += imagePadding.Left;
			ल.Y += (ल.Height - (num + imagePadding.Vertical)) / 2 + imagePadding.Top;
			break;
		case ImageAlignment.MiddleCenter:
			ल.X += (ल.Width - (num2 + imagePadding.Horizontal + TabStripItem.TabHorizontalSpacing)) / 2;
			ल.Y += (ल.Height - (num + imagePadding.Vertical)) / 2 + imagePadding.Top;
			break;
		case ImageAlignment.MiddleRight:
			ल.X = ल.Right - num2 - imagePadding.Right - TabStripItem.TabHorizontalSpacing;
			ल.Y += (ल.Height - (num + imagePadding.Vertical)) / 2 + imagePadding.Top;
			break;
		case ImageAlignment.BottomLeft:
			ल.X += imagePadding.Left;
			ल.Y = ल.Bottom - (num + imagePadding.Bottom + TabStripItem.TabVerticalSpacing);
			break;
		case ImageAlignment.BottomCenter:
			ल.X += (ल.Width - (num2 + imagePadding.Horizontal + TabStripItem.TabHorizontalSpacing)) / 2;
			ल.Y = ल.Bottom - (num + imagePadding.Bottom + TabStripItem.TabVerticalSpacing);
			break;
		case ImageAlignment.BottomRight:
			ल.X = ल.Right - num2 - imagePadding.Right - TabStripItem.TabHorizontalSpacing;
			ल.Y = ल.Bottom - (num + imagePadding.Bottom + TabStripItem.TabVerticalSpacing);
			break;
		}
		ल.Width = num2;
		ल.Height = num;
		return ल;
	}

	internal ᇎ ᕂ()
	{
		Image val = _0EC5();
		if (val != null)
		{
			return new ᇎ(val, dispose: false);
		}
		if (Icon == null)
		{
			return null;
		}
		return new ᇎ(Icon, dispose: false, Icon.Size);
	}

	private Image _0EC5()
	{
		if (_ነ != null)
		{
			return _ነ;
		}
		if (_Ὅ >= 0 && Ὁ != null)
		{
			ImageList imageList = Ὁ.ImageList;
			if (imageList != null && _Ὅ < imageList.Images.Count)
			{
				return imageList.Images[_Ὅ];
			}
		}
		return null;
	}

	internal Size _᧗()
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		Graphics val = BarFunctions.CreateGraphics((Control)(object)Ὁ);
		try
		{
			Font font = Symbols.GetFont(SymbolSize, SymbolSet);
			Size result = TextDrawing.MeasureStringLegacy(val, SymbolRealized, font, Size.Empty, eTextFormat.Default);
			int num = (int)Math.Ceiling((float)font.FontFamily.GetCellDescent(font.Style) * font.Size / (float)font.FontFamily.GetEmHeight(font.Style));
			result.Height -= num;
			return result;
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private Rectangle ὧ()
	{
		Rectangle ल = Ὀ._1FDD();
		ल = Ὡ(Ὠ(ल));
		if (!ᓙ)
		{
			if (IsSelected && (_ὗ.TabAlignment == eTabStripAlignment.Left || _ὗ.TabAlignment == eTabStripAlignment.Right))
			{
				ल.Width -= Dpi.Width(_ὗ.έ._1FFE);
			}
		}
		else if ((_ὗ.TabAlignment == eTabStripAlignment.Left && _ὗ.RotateVerticalText) || (_ὗ.TabAlignment == eTabStripAlignment.Right && !_ὗ.RotateVerticalText))
		{
			if (_ὗ.TabAlignment == eTabStripAlignment.Left)
			{
				ल.X += Dpi.Width(_ὗ.TabVerticalSpacing);
				ल.Width -= Dpi.Width(_ὗ.TabVerticalSpacing);
			}
			else
			{
				ल.Width -= Dpi.Width(_ὗ.TabVerticalSpacing);
			}
		}
		return _ὗ.ᾼ(this, ल);
	}

	private Rectangle Ὠ(Rectangle ल)
	{
		int tabHorizontalSpacing = _ὗ.TabHorizontalSpacing;
		Size size = default(Size);
		if (_ὗ.CloseButtonOnTabsVisible && CloseButtonVisible)
		{
			size += _1369.Size;
			size.Width += tabHorizontalSpacing;
			size.Height += tabHorizontalSpacing;
		}
		if (ᓙ)
		{
			Rectangle rectangle = ल;
			if (TabStripItem.CloseButtonPosition == eTabCloseButtonPosition.Right)
			{
				ल.Y += size.Width;
			}
			ल.Height -= size.Width + tabHorizontalSpacing;
			if (TabStripItem.RotateVerticalText ^ (TabStripItem.TabAlignment == eTabStripAlignment.Right))
			{
				ल.X = rectangle.X + (rectangle.Right - ल.Right);
				ल.Y = rectangle.Y + (rectangle.Bottom - ल.Bottom);
			}
		}
		else
		{
			if (size.Width > 0)
			{
				if (_ὗ.CloseButtonPosition == eTabCloseButtonPosition.Left)
				{
					ल.X += size.Width + tabHorizontalSpacing;
				}
				ल.Width -= size.Width + tabHorizontalSpacing;
			}
			ल.X += tabHorizontalSpacing;
			ल.Width -= tabHorizontalSpacing * 2;
			if (IsSelected && _ὗ.TabAlignment == eTabStripAlignment.Right)
			{
				ल.X += _ὗ.έ._1FFE;
			}
		}
		return ल;
	}

	private Rectangle Ὡ(Rectangle ल)
	{
		ᇎ ᇎ2 = ᕂ();
		if (!string.IsNullOrEmpty(SymbolRealized) || ᇎ2 != null)
		{
			ल = (ᓙ ? Ὣ(ᇎ2, ल) : Ὢ(ᇎ2, ल));
		}
		return ल;
	}

	private Rectangle Ὢ(ᇎ ແ, Rectangle ल)
	{
		int num = 0;
		int num2 = 0;
		if (!string.IsNullOrEmpty(SymbolRealized))
		{
			Size size = _᧗();
			num2 = size.Width;
			num = size.Height;
		}
		else
		{
			num2 = ແ.ܐ;
			num = ແ._0711;
		}
		num2 += _ὗ.TabHorizontalSpacing + ImagePadding.Horizontal;
		num += _ὗ.TabVerticalSpacing + ImagePadding.Vertical;
		switch (ὅ)
		{
		case ImageAlignment.TopLeft:
		case ImageAlignment.TopCenter:
		case ImageAlignment.TopRight:
			ल.Y += num;
			ल.Height -= num;
			break;
		case ImageAlignment.MiddleLeft:
			ल.X += num2;
			ल.Width -= num2;
			break;
		case ImageAlignment.MiddleRight:
			ल.Width -= num2;
			break;
		case ImageAlignment.BottomLeft:
		case ImageAlignment.BottomCenter:
		case ImageAlignment.BottomRight:
			ल.Height -= num;
			break;
		}
		return ल;
	}

	private Rectangle Ὣ(ᇎ ແ, Rectangle ल)
	{
		int num = 0;
		int num2 = 0;
		if (!string.IsNullOrEmpty(SymbolRealized))
		{
			Size size = _᧗();
			num2 = size.Width;
			num = size.Height;
		}
		else
		{
			num2 = ແ.ܐ;
			num = ແ._0711;
		}
		num2 += _ὗ.TabHorizontalSpacing + ImagePadding.Horizontal;
		num += _ὗ.TabVerticalSpacing + ImagePadding.Vertical;
		Rectangle rectangle = ल;
		switch (ὅ)
		{
		case ImageAlignment.TopLeft:
		case ImageAlignment.TopCenter:
		case ImageAlignment.TopRight:
			ल.X += num;
			ल.Width -= num;
			break;
		case ImageAlignment.MiddleLeft:
			ल.Height -= num2;
			break;
		case ImageAlignment.MiddleRight:
			ल.Y += num2;
			ल.Height -= num2;
			break;
		case ImageAlignment.BottomLeft:
		case ImageAlignment.BottomCenter:
		case ImageAlignment.BottomRight:
			ल.Width -= num;
			break;
		}
		if (TabStripItem.RotateVerticalText ^ (TabStripItem.TabAlignment == eTabStripAlignment.Right))
		{
			ल.X = rectangle.X + (rectangle.Right - ल.Right);
			ल.Y = rectangle.Y + (rectangle.Bottom - ल.Bottom);
		}
		return ल;
	}

	private void Ὤ()
	{
		if (_ὗ != null)
		{
			_ὗ.NeedRecalcSize = true;
			_ὗ.Refresh();
		}
		Refresh();
	}

	public override void Refresh()
	{
		if (SuspendLayout || !Visible || !Displayed || TabStripItem == null)
		{
			return;
		}
		object containerControl = TabStripItem.ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (val == null || !BaseItem.IsHandleValid(val))
		{
			return;
		}
		if (NeedRecalcSize)
		{
			if (Parent is ItemContainer)
			{
				Parent.RecalcSize();
			}
			else
			{
				RecalcSize();
				if (Parent != null)
				{
					Parent.SubItemSizeChanged(this);
				}
			}
		}
		Invalidate(val);
	}

	public override void Paint(ItemPaintArgs p)
	{
		if (_Ὑ != null)
		{
			_Ὑ.Paint(p);
		}
	}

	public override void InternalMouseMove(MouseEventArgs objArg)
	{
		base.InternalMouseMove(objArg);
		ὃ = _1369.Contains(objArg.Location);
	}

	public override void InternalMouseEnter()
	{
		base.InternalMouseEnter();
		Refresh();
	}

	public override void InternalMouseLeave()
	{
		base.InternalMouseLeave();
		ὃ = false;
		ὄ = false;
		Refresh();
	}

	public override void InternalMouseDown(MouseEventArgs objArg)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Invalid comparison between Unknown and I4
		base.InternalMouseDown(objArg);
		if (ὃ)
		{
			ὄ = (objArg.Button & 0x100000) == 1048576;
		}
	}

	public override void InternalMouseUp(MouseEventArgs objArg)
	{
		base.InternalMouseUp(objArg);
		ὄ = false;
	}

	protected void UpdateChangeHandler(INotifyPropertyChanged oldValue, INotifyPropertyChanged newValue)
	{
		if (oldValue != null)
		{
			oldValue.PropertyChanged -= Ὥ;
		}
		if (newValue != null)
		{
			newValue.PropertyChanged += Ὥ;
		}
	}

	private void Ὥ(object _٠, PropertyChangedEventArgs ؿ)
	{
		Ὤ();
	}

	public override string ToString()
	{
		return Text;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && !base.IsDisposed)
		{
			ἧ(Ἠ: false);
		}
		base.Dispose(disposing);
	}

	public override BaseItem Copy()
	{
		SuperTabItem superTabItem = new SuperTabItem();
		CopyToItem(superTabItem);
		return superTabItem;
	}

	protected override void CopyToItem(BaseItem copy)
	{
		if (copy is SuperTabItem superTabItem)
		{
			base.CopyToItem(superTabItem);
			superTabItem.TabColor = _ὐ;
			superTabItem.CloseButtonVisible = __137B;
			superTabItem.EnableImageAnimation = __0EB8;
			superTabItem.EnableMarkup = __0EB9;
			superTabItem.Icon = _Ὄ;
			superTabItem.Image = _ነ;
			superTabItem.ImageIndex = _Ὅ;
			superTabItem.PredefinedColor = __1F4F;
			superTabItem.SelectedTabFont = _ὓ;
			superTabItem.TextAlignment = _ᬭ;
			superTabItem.TabFont = _ὒ;
			superTabItem.Symbol = _ລ;
			superTabItem.SymbolSet = _ວ;
			superTabItem.SymbolColor = __0EA4;
			superTabItem.SymbolSize = _ຨ;
			superTabItem.AttachedControl = __1F58;
		}
	}
}
