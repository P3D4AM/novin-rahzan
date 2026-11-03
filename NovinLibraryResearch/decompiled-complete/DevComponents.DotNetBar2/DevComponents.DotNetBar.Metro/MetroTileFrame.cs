using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Threading;
using DevComponents.DotNetBar.TextMarkup;

namespace DevComponents.DotNetBar.Metro;

[DesignTimeVisible(false)]
[ToolboxItem(false)]
[Designer("DevComponents.DotNetBar.Design.MetroTileFrameDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
public class MetroTileFrame : Component, INotifyPropertyChanged, ICloneable
{
	private string _ᅶ = "";

	private _0CB4 _ᐣ;

	private EventHandler _0D83;

	private Font _ᩍ;

	private Rectangle _ᩎ = Rectangle.Empty;

	private Color _ᩏ = Color.Empty;

	private ContentAlignment _ᩐ = (ContentAlignment)256;

	private ElementStyle _ᩑ;

	private string _ລ = "";

	private string __0EA6 = "";

	private eSymbolSet _ວ;

	private float _ຨ;

	private Color __0EA4 = Color.Empty;

	private Image _ነ;

	private static readonly Point ᩒ = new Point(2, 2);

	private Point _ᩓ = ᩒ;

	private string _ኚ = "";

	private eMetroTileColor _ᩔ;

	private ElementStyle _ᎍ = new ElementStyle();

	private int __1A55;

	private _0CB4 __1A56;

	private MarkupLinkClickEventHandler Ӟ;

	private bool _ݷ = true;

	private ContentAlignment __1A57 = (ContentAlignment)1;

	private PropertyChangedEventHandler চ;

	[Category("Appearance")]
	[Description("Indicates tile title text displayed by default in lower left corner")]
	[Editor("DevComponents.DotNetBar.Design.TextMarkupUIEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[DefaultValue("")]
	public string TitleText
	{
		get
		{
			return _ᅶ;
		}
		set
		{
			if (value != _ᅶ)
			{
				string ᅶ = _ᅶ;
				_ᅶ = value;
				OnTitleTextChanged(ᅶ, value);
			}
		}
	}

	internal _0CB4 TitleTextMarkupBody => _ᐣ;

	[Category("Appearance")]
	[Description("Gets or sets the title text font.")]
	[DefaultValue(null)]
	public Font TitleTextFont
	{
		get
		{
			return _ᩍ;
		}
		set
		{
			if (value != _ᩍ)
			{
				Font ᩍ = _ᩍ;
				_ᩍ = value;
				OnTitleTextFontChanged(ᩍ, value);
			}
		}
	}

	internal Rectangle TitleTextBounds
	{
		get
		{
			return _ᩎ;
		}
		set
		{
			_ᩎ = value;
		}
	}

	[Category("Columns")]
	[Description("Indicates color of title text.")]
	public Color TitleTextColor
	{
		get
		{
			return _ᩏ;
		}
		set
		{
			if (_ᩏ != value)
			{
				Color ᩏ = _ᩏ;
				_ᩏ = value;
				OnTitleTextColorChanged(ᩏ, value);
			}
		}
	}

	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Description("Indicates title text alignment.")]
	[Category("Appearance")]
	public ContentAlignment TitleTextAlignment
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return _ᩐ;
		}
		set
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			if (value != _ᩐ)
			{
				ContentAlignment ᩐ = _ᩐ;
				_ᩐ = value;
				OnTitleTextAlignmentChanged(ᩐ, value);
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public ElementStyle EffectiveStyle => _ᎍ;

	[Browsable(true)]
	[Category("Style")]
	[Description("Gets or sets Tile style of the item.")]
	[DevCoBrowsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ElementStyle TileStyle => _ᩑ;

	[Browsable(false)]
	public string SymbolRealized => __0EA6;

	[Description("Indicates the symbol displayed on face of the tile instead of the image. Setting the symbol overrides the image setting.")]
	[DefaultValue("")]
	[Editor("DevComponents.DotNetBar.Design.SymbolTypeEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Category("Appearance")]
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

	[Description("Indicates the size of the symbol in points.")]
	[DefaultValue(0f)]
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

	[Category("Columns")]
	[Description("Indicates color of the symbol.")]
	public Color SymbolColor
	{
		get
		{
			return __0EA4;
		}
		set
		{
			__0EA4 = value;
			OnPropertyChanged(new PropertyChangedEventArgs("SymbolColor"));
		}
	}

	[DefaultValue(null)]
	[Description("Indicates image displayed on the tile.")]
	[Category("Appearance")]
	public Image Image
	{
		get
		{
			return _ነ;
		}
		set
		{
			if (value != _ነ)
			{
				Image ነ = _ነ;
				_ነ = value;
				OnImageChanged(ነ, value);
			}
		}
	}

	[Category("Appearance")]
	[Description("Indicates  top-left location of the image.")]
	public Point ImageIndent
	{
		get
		{
			return _ᩓ;
		}
		set
		{
			if (value != _ᩓ)
			{
				Point ᩓ = _ᩓ;
				_ᩓ = value;
				OnImageIndentChanged(ᩓ, value);
			}
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[Description("The text contained in the item.")]
	[Editor("DevComponents.DotNetBar.Design.TextMarkupUIEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[DefaultValue("")]
	[Localizable(true)]
	public string Text
	{
		get
		{
			return _ኚ;
		}
		set
		{
			if (value != _ኚ)
			{
				string ኚ = _ኚ;
				_ኚ = value;
				OnTextChanged(ኚ, value);
			}
		}
	}

	[Description("Indicates predefined tile color.")]
	[Category("Appearance")]
	public eMetroTileColor TileColor
	{
		get
		{
			return _ᩔ;
		}
		set
		{
			if (value != _ᩔ)
			{
				eMetroTileColor ᩔ = _ᩔ;
				_ᩔ = value;
				OnTileColorChanged(ᩔ, value);
			}
		}
	}

	[DefaultValue(0)]
	[Description("Indicates frame display duration in milliseconds during metro-tile frame animation. When not set then each frame will stay visible for duration set on MetroTileItem.AutoRotateFramesInterval.")]
	[Category("Behavior")]
	public int FrameDisplayDuration
	{
		get
		{
			return __1A55;
		}
		set
		{
			if (value != __1A55)
			{
				int oldValue = __1A55;
				__1A55 = value;
				OnFrameDisplayDurationChanged(oldValue, value);
			}
		}
	}

	internal _0CB4 TextMarkupBody => __1A56;

	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates whether text-markup can be used in Text property.")]
	public bool TextMarkupEnabled
	{
		get
		{
			return _ݷ;
		}
		set
		{
			if (value != _ݷ)
			{
				bool ݷ = _ݷ;
				_ݷ = value;
				OnTextMarkupEnabledChanged(ݷ, value);
			}
		}
	}

	[Description("Indicates the image alignment in relation to text.")]
	[Category("Appearance")]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	public ContentAlignment ImageTextAlignment
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return __1A57;
		}
		set
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			if (value != __1A57)
			{
				ContentAlignment oldValue = __1A57;
				__1A57 = value;
				OnImageTextAlignmentChanged(oldValue, value);
			}
		}
	}

	public event EventHandler TitleTextMarkupLinkClick
	{
		add
		{
			EventHandler eventHandler = _0D83;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _0D83, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _0D83;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _0D83, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when text markup link is clicked. Markup links can be created using a tag.")]
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

	[Description("Occurs when property on BindingDef object has changed.Occurs when property on BindingDef object has changed.")]
	public event PropertyChangedEventHandler PropertyChanged
	{
		add
		{
			PropertyChangedEventHandler propertyChangedEventHandler = চ;
			PropertyChangedEventHandler propertyChangedEventHandler2;
			do
			{
				propertyChangedEventHandler2 = propertyChangedEventHandler;
				PropertyChangedEventHandler value2 = (PropertyChangedEventHandler)Delegate.Combine(propertyChangedEventHandler2, value);
				propertyChangedEventHandler = Interlocked.CompareExchange(ref চ, value2, propertyChangedEventHandler2);
			}
			while ((object)propertyChangedEventHandler != propertyChangedEventHandler2);
		}
		remove
		{
			PropertyChangedEventHandler propertyChangedEventHandler = চ;
			PropertyChangedEventHandler propertyChangedEventHandler2;
			do
			{
				propertyChangedEventHandler2 = propertyChangedEventHandler;
				PropertyChangedEventHandler value2 = (PropertyChangedEventHandler)Delegate.Remove(propertyChangedEventHandler2, value);
				propertyChangedEventHandler = Interlocked.CompareExchange(ref চ, value2, propertyChangedEventHandler2);
			}
			while ((object)propertyChangedEventHandler != propertyChangedEventHandler2);
		}
	}

	public MetroTileFrame()
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		_ᩑ = new ElementStyle();
		UpdateEffectiveStyle();
		_ᩑ.StyleChanged += TileStyleChanged;
	}

	private void TileStyleChanged(object sender, EventArgs e)
	{
		UpdateEffectiveStyle();
		OnPropertyChanged(new PropertyChangedEventArgs("TileStyle"));
	}

	protected virtual void OnTitleTextChanged(string oldValue, string newValue)
	{
		TitleTextMarkupUpdate();
		OnPropertyChanged(new PropertyChangedEventArgs("TitleText"));
	}

	private void TitleTextMarkupUpdate()
	{
		if (_ᐣ != null)
		{
			_ᐣ.ವ -= TitleTextMarkupLinkClicked;
		}
		_ᐣ = null;
		if (_ݷ && ሒ.መ(ref _ᅶ))
		{
			_ᐣ = ሒ.ሕ(_ᅶ);
			if (_ᐣ != null)
			{
				_ᐣ.ವ += TitleTextMarkupLinkClicked;
			}
		}
	}

	private void TitleTextMarkupLinkClicked(object sender, EventArgs e)
	{
		if (sender is _0CFE obj)
		{
			OnTitleTextMarkupLinkClick(new MarkupLinkClickEventArgs(obj._0D01, obj._0D00));
		}
		else
		{
			OnTitleTextMarkupLinkClick(e);
		}
	}

	protected virtual void OnTitleTextMarkupLinkClick(EventArgs e)
	{
		_0D83?.Invoke(this, e);
	}

	protected virtual void OnTitleTextFontChanged(Font oldValue, Font newValue)
	{
		OnPropertyChanged(new PropertyChangedEventArgs("TitleTextFont"));
	}

	protected virtual void OnTitleTextColorChanged(Color oldValue, Color newValue)
	{
		OnPropertyChanged(new PropertyChangedEventArgs("TitleTextColor"));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTitleTextColor()
	{
		return !_ᩏ.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetTitleTextColor()
	{
		TitleTextColor = Color.Empty;
	}

	protected virtual void OnTitleTextAlignmentChanged(ContentAlignment oldValue, ContentAlignment newValue)
	{
		OnPropertyChanged(new PropertyChangedEventArgs("TitleTextAlignment"));
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
		OnPropertyChanged(new PropertyChangedEventArgs("Symbol"));
	}

	protected virtual void OnSymbolSetChanged(eSymbolSet oldValue, eSymbolSet newValue)
	{
		OnPropertyChanged(new PropertyChangedEventArgs("SymbolSet"));
	}

	protected virtual void OnSymbolSizeChanged(float oldValue, float newValue)
	{
		OnPropertyChanged(new PropertyChangedEventArgs("SymbolSize"));
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

	protected virtual void OnImageChanged(Image oldValue, Image newValue)
	{
		OnPropertyChanged(new PropertyChangedEventArgs("Image"));
	}

	protected virtual void OnImageIndentChanged(Point oldValue, Point newValue)
	{
		OnPropertyChanged(new PropertyChangedEventArgs("ImageIndent"));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeImageIndent()
	{
		return _ᩓ != ᩒ;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetImageIndent()
	{
		ImageIndent = ᩒ;
	}

	protected virtual void OnTextChanged(string oldValue, string newValue)
	{
		MarkupTextChanged();
		OnPropertyChanged(new PropertyChangedEventArgs("Text"));
	}

	protected virtual void OnTileColorChanged(eMetroTileColor oldValue, eMetroTileColor newValue)
	{
		UpdateEffectiveStyle();
		OnPropertyChanged(new PropertyChangedEventArgs("TileColor"));
	}

	private void UpdateEffectiveStyle()
	{
		ElementStyle predefinedTileStyle = GetPredefinedTileStyle(_ᩔ);
		predefinedTileStyle.ApplyStyle(_ᩑ);
		_ᎍ = predefinedTileStyle;
	}

	private ElementStyle GetPredefinedTileStyle(eMetroTileColor tileColor)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.TextColor = Color.White;
		elementStyle.PaddingTop = 4;
		elementStyle.PaddingRight = 4;
		elementStyle.PaddingBottom = 4;
		elementStyle.PaddingLeft = 4;
		switch (tileColor)
		{
		case eMetroTileColor.Default:
			elementStyle.BackColor = ColorScheme.GetColor(608946);
			elementStyle.BackColor2 = ColorScheme.GetColor(678852);
			elementStyle.BackColorGradientAngle = 45;
			elementStyle.Border = eStyleBorderType.Solid;
			elementStyle.BorderColor = ColorScheme.GetColor(2321610);
			elementStyle.BorderWidth = 1;
			break;
		case eMetroTileColor.Blue:
			elementStyle.BackColor = ColorScheme.GetColor(29382);
			elementStyle.BackColor2 = ColorScheme.GetColor(36307);
			elementStyle.BackColorGradientAngle = 45;
			elementStyle.Border = eStyleBorderType.Solid;
			elementStyle.BorderColor = ColorScheme.GetColor(1736908);
			elementStyle.BorderWidth = 1;
			break;
		case eMetroTileColor.Green:
			elementStyle.BackColor = ColorScheme.GetColor(35328);
			elementStyle.BackColor2 = ColorScheme.GetColor(42496);
			elementStyle.BackColorGradientAngle = 45;
			elementStyle.Border = eStyleBorderType.Solid;
			elementStyle.BorderColor = ColorScheme.GetColor(1742362);
			elementStyle.BorderWidth = 1;
			break;
		case eMetroTileColor.Magenta:
			elementStyle.BackColor = ColorScheme.GetColor(9175189);
			elementStyle.BackColor2 = ColorScheme.GetColor(10944686);
			elementStyle.BackColorGradientAngle = 45;
			elementStyle.Border = eStyleBorderType.Solid;
			elementStyle.BorderColor = ColorScheme.GetColor(9968288);
			elementStyle.BorderWidth = 1;
			break;
		case eMetroTileColor.Orange:
			elementStyle.BackColor = ColorScheme.GetColor(13780774);
			elementStyle.BackColor2 = ColorScheme.GetColor(14440238);
			elementStyle.BackColorGradientAngle = 45;
			elementStyle.Border = eStyleBorderType.Solid;
			elementStyle.BorderColor = ColorScheme.GetColor(14113340);
			elementStyle.BorderWidth = 1;
			break;
		case eMetroTileColor.Plum:
			elementStyle.BackColor = ColorScheme.GetColor(5321643);
			elementStyle.BackColor2 = ColorScheme.GetColor(6569663);
			elementStyle.BackColorGradientAngle = 45;
			elementStyle.Border = eStyleBorderType.Solid;
			elementStyle.BorderColor = ColorScheme.GetColor(6506676);
			elementStyle.BorderWidth = 1;
			break;
		case eMetroTileColor.Teal:
			elementStyle.BackColor = ColorScheme.GetColor(33433);
			elementStyle.BackColor2 = ColorScheme.GetColor(41137);
			elementStyle.BackColorGradientAngle = 45;
			elementStyle.Border = eStyleBorderType.Solid;
			elementStyle.BorderColor = ColorScheme.GetColor(1740707);
			elementStyle.BorderWidth = 1;
			break;
		case eMetroTileColor.Coffee:
			elementStyle.BackColor = ColorScheme.GetColor(4536873);
			elementStyle.BackColor2 = ColorScheme.GetColor(5588786);
			elementStyle.BackColorGradientAngle = 45;
			elementStyle.Border = eStyleBorderType.Solid;
			elementStyle.BorderColor = ColorScheme.GetColor(5787199);
			elementStyle.BorderWidth = 1;
			break;
		case eMetroTileColor.RedOrange:
			elementStyle.BackColor = ColorScheme.GetColor(13775654);
			elementStyle.BackColor2 = ColorScheme.GetColor(14432558);
			elementStyle.BackColorGradientAngle = 45;
			elementStyle.Border = eStyleBorderType.Solid;
			elementStyle.BorderColor = ColorScheme.GetColor(14108220);
			elementStyle.BorderWidth = 1;
			break;
		case eMetroTileColor.RedViolet:
			elementStyle.BackColor = ColorScheme.GetColor(9179985);
			elementStyle.BackColor2 = ColorScheme.GetColor(10360411);
			elementStyle.BackColorGradientAngle = 45;
			elementStyle.Border = eStyleBorderType.Solid;
			elementStyle.BorderColor = ColorScheme.GetColor(11158393);
			elementStyle.BorderWidth = 1;
			break;
		case eMetroTileColor.Olive:
			elementStyle.BackColor = ColorScheme.GetColor(3243837);
			elementStyle.BackColor2 = ColorScheme.GetColor(3839304);
			elementStyle.BackColorGradientAngle = 45;
			elementStyle.Border = eStyleBorderType.Solid;
			elementStyle.BorderColor = ColorScheme.GetColor(3905865);
			elementStyle.BorderWidth = 1;
			break;
		case eMetroTileColor.DarkOlive:
			elementStyle.BackColor = ColorScheme.GetColor(18944);
			elementStyle.BackColor2 = ColorScheme.GetColor(20736);
			elementStyle.BackColorGradientAngle = 45;
			elementStyle.Border = eStyleBorderType.Solid;
			elementStyle.BorderColor = ColorScheme.GetColor(21504);
			elementStyle.BorderWidth = 1;
			break;
		case eMetroTileColor.Rust:
			elementStyle.BackColor = ColorScheme.GetColor(6367531);
			elementStyle.BackColor2 = ColorScheme.GetColor(6827055);
			elementStyle.BackColorGradientAngle = 45;
			elementStyle.Border = eStyleBorderType.Solid;
			elementStyle.BorderColor = ColorScheme.GetColor(7024177);
			elementStyle.BorderWidth = 1;
			break;
		case eMetroTileColor.Maroon:
			elementStyle.BackColor = ColorScheme.GetColor(11278653);
			elementStyle.BackColor2 = ColorScheme.GetColor(12525131);
			elementStyle.BackColorGradientAngle = 45;
			elementStyle.Border = eStyleBorderType.Solid;
			elementStyle.BorderColor = ColorScheme.GetColor(11808849);
			elementStyle.BorderWidth = 1;
			break;
		case eMetroTileColor.Yellowish:
			elementStyle.BackColor = ColorScheme.GetColor(13014024);
			elementStyle.BackColor2 = ColorScheme.GetColor(12553737);
			elementStyle.BackColorGradientAngle = 45;
			elementStyle.Border = eStyleBorderType.Solid;
			elementStyle.BorderColor = ColorScheme.GetColor(12684553);
			elementStyle.BorderWidth = 1;
			break;
		case eMetroTileColor.Blueish:
			elementStyle.BackColor = ColorScheme.GetColor(7241364);
			elementStyle.BackColor2 = ColorScheme.GetColor(6912140);
			elementStyle.BackColorGradientAngle = 45;
			elementStyle.Border = eStyleBorderType.Solid;
			elementStyle.BorderColor = ColorScheme.GetColor(6517124);
			elementStyle.BorderWidth = 1;
			break;
		case eMetroTileColor.DarkBlue:
			elementStyle.BackColor = ColorScheme.GetColor(1460634);
			elementStyle.BackColor2 = ColorScheme.GetColor(1592992);
			elementStyle.BackColorGradientAngle = 45;
			elementStyle.Border = eStyleBorderType.Solid;
			elementStyle.BorderColor = ColorScheme.GetColor(1593251);
			elementStyle.BorderWidth = 1;
			break;
		case eMetroTileColor.Yellow:
			elementStyle.BackColor = ColorScheme.GetColor(16568598);
			elementStyle.BackColor2 = ColorScheme.GetColor(16571162);
			elementStyle.BackColorGradientAngle = 45;
			elementStyle.TextColor = ColorScheme.GetColor(10360598);
			elementStyle.Border = eStyleBorderType.Solid;
			elementStyle.BorderColor = ColorScheme.GetColor(16569902);
			elementStyle.BorderWidth = 1;
			break;
		case eMetroTileColor.Gray:
			elementStyle.BackColor = ColorScheme.GetColor(4473924);
			elementStyle.BackColor2 = ColorScheme.GetColor(5526612);
			elementStyle.BackColorGradientAngle = 45;
			elementStyle.Border = eStyleBorderType.Solid;
			elementStyle.BorderColor = ColorScheme.GetColor(5723991);
			elementStyle.BorderWidth = 1;
			break;
		case eMetroTileColor.DarkGreen:
			elementStyle.BackColor = ColorScheme.GetColor(2975019);
			elementStyle.BackColor2 = ColorScheme.GetColor(2775847);
			elementStyle.BackColorGradientAngle = 45;
			elementStyle.Border = eStyleBorderType.Solid;
			elementStyle.BorderColor = ColorScheme.GetColor(2973737);
			elementStyle.BorderWidth = 1;
			break;
		case eMetroTileColor.MaroonWashed:
			elementStyle.BackColor = ColorScheme.GetColor(8351380);
			elementStyle.BackColor2 = ColorScheme.GetColor(8022926);
			elementStyle.BackColorGradientAngle = 45;
			elementStyle.Border = eStyleBorderType.Solid;
			elementStyle.BorderColor = ColorScheme.GetColor(8483478);
			elementStyle.BorderWidth = 1;
			break;
		case eMetroTileColor.PlumWashed:
			elementStyle.BackColor = ColorScheme.GetColor(11158393);
			elementStyle.BackColor2 = ColorScheme.GetColor(11486846);
			elementStyle.BackColorGradientAngle = 45;
			elementStyle.Border = eStyleBorderType.Solid;
			elementStyle.BorderColor = ColorScheme.GetColor(12340103);
			elementStyle.BorderWidth = 1;
			break;
		case eMetroTileColor.Azure:
			elementStyle.BackColor = ColorScheme.GetColor(2519788);
			elementStyle.BackColor2 = ColorScheme.GetColor(2718194);
			elementStyle.BackColorGradientAngle = 45;
			elementStyle.Border = eStyleBorderType.Solid;
			elementStyle.BorderColor = ColorScheme.GetColor(2784761);
			elementStyle.BorderWidth = 1;
			break;
		}
		return elementStyle;
	}

	protected virtual void OnFrameDisplayDurationChanged(int oldValue, int newValue)
	{
		OnPropertyChanged(new PropertyChangedEventArgs("FrameDisplayDuration"));
	}

	private void MarkupTextChanged()
	{
		if (__1A56 != null)
		{
			__1A56.ವ -= TextMarkupLinkClick;
		}
		__1A56 = null;
		if (_ݷ && ሒ.መ(ref _ኚ))
		{
			__1A56 = ሒ.ሕ(_ኚ);
			if (__1A56 != null)
			{
				__1A56.ವ += TextMarkupLinkClick;
			}
		}
	}

	protected virtual void TextMarkupLinkClick(object sender, EventArgs e)
	{
		if (sender is _0CFE obj)
		{
			OnMarkupLinkClick(new MarkupLinkClickEventArgs(obj._0D01, obj._0D00));
		}
	}

	protected virtual void OnMarkupLinkClick(MarkupLinkClickEventArgs e)
	{
		Ӟ?.Invoke(this, e);
	}

	protected virtual void OnTextMarkupEnabledChanged(bool oldValue, bool newValue)
	{
		OnPropertyChanged(new PropertyChangedEventArgs("TextMarkupEnabled"));
	}

	internal static Size MeasureText(MetroTileFrame item, Graphics g, int containerWidth, Font font, eTextFormat stringFormat, bool rightToLeft)
	{
		if (item.Text == "" && item.TextMarkupBody == null)
		{
			return Size.Empty;
		}
		Size empty = Size.Empty;
		if (item.TextMarkupBody == null)
		{
			return TextDrawing.MeasureString(g, _192F.ಱ(item.Text), font, containerWidth, stringFormat);
		}
		Size availableSize = new Size(containerWidth, 1);
		if (containerWidth == 0)
		{
			availableSize.Width = 1600;
		}
		MarkupDrawContext markupDrawContext = new MarkupDrawContext(g, font, Color.Empty, rightToLeft: false);
		item.TextMarkupBody.Measure(availableSize, markupDrawContext);
		availableSize = item.TextMarkupBody.Bounds.Size;
		if (containerWidth != 0)
		{
			availableSize.Width = containerWidth;
		}
		markupDrawContext.RightToLeft = rightToLeft;
		item.TextMarkupBody._0CC6(new Rectangle(0, 0, availableSize.Width, availableSize.Height), markupDrawContext);
		return item.TextMarkupBody.Bounds.Size;
	}

	protected virtual void OnImageTextAlignmentChanged(ContentAlignment oldValue, ContentAlignment newValue)
	{
		OnPropertyChanged(new PropertyChangedEventArgs("ImageTextAlignment"));
	}

	protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
	{
		চ?.Invoke(this, e);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			_ᩑ.StyleChanged -= TileStyleChanged;
		}
		base.Dispose(disposing);
	}

	public object Clone()
	{
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Expected O, but got Unknown
		MetroTileFrame metroTileFrame = new MetroTileFrame();
		if (Image != null)
		{
			metroTileFrame.Image = (Image)Image.Clone();
		}
		metroTileFrame.ImageIndent = ImageIndent;
		metroTileFrame.Text = Text;
		metroTileFrame.TextMarkupEnabled = TextMarkupEnabled;
		metroTileFrame.TileStyle.ApplyStyle(TileStyle);
		metroTileFrame.TileColor = TileColor;
		metroTileFrame.TitleText = TitleText;
		metroTileFrame.TitleTextAlignment = TitleTextAlignment;
		metroTileFrame.TitleTextColor = TitleTextColor;
		if (TitleTextFont != null)
		{
			metroTileFrame.TitleTextFont = (Font)TitleTextFont.Clone();
		}
		metroTileFrame.ImageTextAlignment = ImageTextAlignment;
		metroTileFrame.FrameDisplayDuration = FrameDisplayDuration;
		metroTileFrame.Symbol = Symbol;
		metroTileFrame.SymbolSet = SymbolSet;
		metroTileFrame.SymbolSize = SymbolSize;
		metroTileFrame.SymbolColor = SymbolColor;
		return metroTileFrame;
	}
}
