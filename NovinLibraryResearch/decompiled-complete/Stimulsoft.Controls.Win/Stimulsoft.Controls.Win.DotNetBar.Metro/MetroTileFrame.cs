using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using Stimulsoft.Controls.Win.DotNetBar.TextMarkup;

namespace Stimulsoft.Controls.Win.DotNetBar.Metro;

[DesignTimeVisible(false)]
[ToolboxItem(false)]
public class MetroTileFrame : Component, ICloneable, INotifyPropertyChanged
{
	private string _TitleText = "";

	private BodyElement _TitleTextMarkup;

	private Font _TitleTextFont;

	private Rectangle _TitleTextBounds = Rectangle.Empty;

	private Color _TitleTextColor = Color.Empty;

	private ContentAlignment _TitleTextAlignment = (ContentAlignment)256;

	private ElementStyle _TileStyle;

	private string _Symbol = "";

	private float _SymbolSize;

	private Color _SymbolColor = Color.Empty;

	private Image _Image;

	private static readonly Point DefaultImageIndent = new Point(2, 2);

	private Point _ImageIndent = DefaultImageIndent;

	private string _Text = "";

	private eMetroTileColor _TileColor;

	private ElementStyle _EffectiveStyle = new ElementStyle();

	private int _FrameDisplayDuration;

	private BodyElement _TextMarkup;

	private bool _TextMarkupEnabled = true;

	private ContentAlignment _ImageTextAlignment = (ContentAlignment)1;

	[DefaultValue("")]
	[Category("Appearance")]
	[Description("Indicates tile title text displayed by default in lower left corner")]
	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.TextMarkupUIEditor, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public string TitleText
	{
		get
		{
			return _TitleText;
		}
		set
		{
			if (value != _TitleText)
			{
				string titleText = _TitleText;
				_TitleText = value;
				OnTitleTextChanged(titleText, value);
			}
		}
	}

	internal BodyElement TitleTextMarkupBody => _TitleTextMarkup;

	[DefaultValue(null)]
	[Category("Appearance")]
	[Description("Gets or sets the title text font.")]
	public Font TitleTextFont
	{
		get
		{
			return _TitleTextFont;
		}
		set
		{
			if (value != _TitleTextFont)
			{
				Font titleTextFont = _TitleTextFont;
				_TitleTextFont = value;
				OnTitleTextFontChanged(titleTextFont, value);
			}
		}
	}

	internal Rectangle TitleTextBounds
	{
		get
		{
			return _TitleTextBounds;
		}
		set
		{
			_TitleTextBounds = value;
		}
	}

	[Category("Columns")]
	[Description("Indicates color of title text.")]
	public Color TitleTextColor
	{
		get
		{
			return _TitleTextColor;
		}
		set
		{
			if (_TitleTextColor != value)
			{
				Color titleTextColor = _TitleTextColor;
				_TitleTextColor = value;
				OnTitleTextColorChanged(titleTextColor, value);
			}
		}
	}

	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Category("Appearance")]
	[Description("Indicates title text alignment.")]
	public ContentAlignment TitleTextAlignment
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return _TitleTextAlignment;
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
			if (value != _TitleTextAlignment)
			{
				ContentAlignment titleTextAlignment = _TitleTextAlignment;
				_TitleTextAlignment = value;
				OnTitleTextAlignmentChanged(titleTextAlignment, value);
			}
		}
	}

	internal ElementStyle EffectiveStyle => _EffectiveStyle;

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Style")]
	[Description("Gets or sets Tile style of the item.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ElementStyle TileStyle => _TileStyle;

	[DefaultValue("")]
	[Category("Appearance")]
	[Description("Indicates the symbol displayed on face of the tile instead of the image. Setting the symbol overrides the image setting.")]
	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.SymbolTypeEditor, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public string Symbol
	{
		get
		{
			return _Symbol;
		}
		set
		{
			if (value != _Symbol)
			{
				string symbol = _Symbol;
				_Symbol = value;
				OnSymbolChanged(symbol, value);
			}
		}
	}

	[DefaultValue(0f)]
	[Category("Appearance")]
	[Description("Indicates the size of the symbol in points.")]
	public float SymbolSize
	{
		get
		{
			return _SymbolSize;
		}
		set
		{
			if (value != _SymbolSize)
			{
				float symbolSize = _SymbolSize;
				_SymbolSize = value;
				OnSymbolSizeChanged(symbolSize, value);
			}
		}
	}

	[Category("Columns")]
	[Description("Indicates color of the symbol.")]
	public Color SymbolColor
	{
		get
		{
			return _SymbolColor;
		}
		set
		{
			_SymbolColor = value;
			OnPropertyChanged(new PropertyChangedEventArgs("SymbolColor"));
		}
	}

	[DefaultValue(null)]
	[Category("Appearance")]
	[Description("Indicates image displayed on the tile.")]
	public Image Image
	{
		get
		{
			return _Image;
		}
		set
		{
			if (value != _Image)
			{
				Image image = _Image;
				_Image = value;
				OnImageChanged(image, value);
			}
		}
	}

	[Category("Appearance")]
	[Description("Indicates  top-left location of the image.")]
	public Point ImageIndent
	{
		get
		{
			return _ImageIndent;
		}
		set
		{
			if (value != _ImageIndent)
			{
				Point imageIndent = _ImageIndent;
				_ImageIndent = value;
				OnImageIndentChanged(imageIndent, value);
			}
		}
	}

	[Browsable(true)]
	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.TextMarkupUIEditor, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Category("Appearance")]
	[Description("The text contained in the item.")]
	[Localizable(true)]
	[DefaultValue("")]
	public string Text
	{
		get
		{
			return _Text;
		}
		set
		{
			if (value != _Text)
			{
				string text = _Text;
				_Text = value;
				OnTextChanged(text, value);
			}
		}
	}

	[Category("Appearance")]
	[Description("Indicates predefined tile color.")]
	public eMetroTileColor TileColor
	{
		get
		{
			return _TileColor;
		}
		set
		{
			if (value != _TileColor)
			{
				eMetroTileColor tileColor = _TileColor;
				_TileColor = value;
				OnTileColorChanged(tileColor, value);
			}
		}
	}

	[DefaultValue(0)]
	[Category("Behavior")]
	[Description("Indicates frame display duration in milliseconds during metro-tile frame animation. When not set then each frame will stay visible for duration set on MetroTileItem.AutoRotateFramesInterval.")]
	public int FrameDisplayDuration
	{
		get
		{
			return _FrameDisplayDuration;
		}
		set
		{
			if (value != _FrameDisplayDuration)
			{
				int frameDisplayDuration = _FrameDisplayDuration;
				_FrameDisplayDuration = value;
				OnFrameDisplayDurationChanged(frameDisplayDuration, value);
			}
		}
	}

	internal BodyElement TextMarkupBody => _TextMarkup;

	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates whether text-markup can be used in Text property.")]
	public bool TextMarkupEnabled
	{
		get
		{
			return _TextMarkupEnabled;
		}
		set
		{
			if (value != _TextMarkupEnabled)
			{
				bool textMarkupEnabled = _TextMarkupEnabled;
				_TextMarkupEnabled = value;
				OnTextMarkupEnabledChanged(textMarkupEnabled, value);
			}
		}
	}

	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Category("Appearance")]
	[Description("Indicates the image alignment in relation to text.")]
	public ContentAlignment ImageTextAlignment
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return _ImageTextAlignment;
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
			if (value != _ImageTextAlignment)
			{
				ContentAlignment imageTextAlignment = _ImageTextAlignment;
				_ImageTextAlignment = value;
				OnImageTextAlignmentChanged(imageTextAlignment, value);
			}
		}
	}

	public event EventHandler TitleTextMarkupLinkClick;

	[Description("Occurs when text markup link is clicked. Markup links can be created using a tag.")]
	public event MarkupLinkClickEventHandler MarkupLinkClick;

	[Description("Occurs when property on BindingDef object has changed.Occurs when property on BindingDef object has changed.")]
	public event PropertyChangedEventHandler PropertyChanged;

	public MetroTileFrame()
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		_TileStyle = new ElementStyle();
		UpdateEffectiveStyle();
		_TileStyle.StyleChanged += TileStyleChanged;
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
		if (_TitleTextMarkup != null)
		{
			_TitleTextMarkup.HyperLinkClick -= TitleTextMarkupLinkClicked;
		}
		_TitleTextMarkup = null;
		if (_TextMarkupEnabled && MarkupParser.IsMarkup(ref _TitleText))
		{
			_TitleTextMarkup = MarkupParser.Parse(_TitleText);
			if (_TitleTextMarkup != null)
			{
				_TitleTextMarkup.HyperLinkClick += TitleTextMarkupLinkClicked;
			}
		}
	}

	private void TitleTextMarkupLinkClicked(object sender, EventArgs e)
	{
		if (sender is HyperLink hyperLink)
		{
			OnTitleTextMarkupLinkClick(new MarkupLinkClickEventArgs(hyperLink.Name, hyperLink.HRef));
		}
		else
		{
			OnTitleTextMarkupLinkClick(e);
		}
	}

	protected virtual void OnTitleTextMarkupLinkClick(EventArgs e)
	{
		this.TitleTextMarkupLinkClick?.Invoke(this, e);
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
		return !_TitleTextColor.IsEmpty;
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
		OnPropertyChanged(new PropertyChangedEventArgs("Symbol"));
	}

	protected virtual void OnSymbolSizeChanged(float oldValue, float newValue)
	{
		OnPropertyChanged(new PropertyChangedEventArgs("SymbolSize"));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeSymbolColor()
	{
		return !_SymbolColor.IsEmpty;
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
		return _ImageIndent != DefaultImageIndent;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetImageIndent()
	{
		ImageIndent = DefaultImageIndent;
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
		ElementStyle predefinedTileStyle = GetPredefinedTileStyle(_TileColor);
		predefinedTileStyle.ApplyStyle(_TileStyle);
		_EffectiveStyle = predefinedTileStyle;
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
			elementStyle.BackColor = ColorScheme.GetColor(2380661);
			elementStyle.BackColor2 = ColorScheme.GetColor(3172251);
			elementStyle.BackColorGradientAngle = 45;
			break;
		case eMetroTileColor.Blue:
			elementStyle.BackColor = ColorScheme.GetColor(5007016);
			elementStyle.BackColor2 = ColorScheme.GetColor(5994686);
			elementStyle.BackColorGradientAngle = 45;
			break;
		case eMetroTileColor.Green:
			elementStyle.BackColor = ColorScheme.GetColor(5549838);
			elementStyle.BackColor2 = ColorScheme.GetColor(6865424);
			elementStyle.BackColorGradientAngle = 45;
			elementStyle.Border = eStyleBorderType.Solid;
			elementStyle.BorderColor = ColorScheme.GetColor(6666023);
			elementStyle.BorderWidth = 1;
			break;
		case eMetroTileColor.Magenta:
			elementStyle.BackColor = ColorScheme.GetColor(7755156);
			elementStyle.BackColor2 = ColorScheme.GetColor(8741561);
			elementStyle.BackColorGradientAngle = 45;
			break;
		case eMetroTileColor.Orange:
			elementStyle.BackColor = ColorScheme.GetColor(14648064);
			elementStyle.BackColor2 = ColorScheme.GetColor(15239168);
			elementStyle.BackColorGradientAngle = 45;
			break;
		case eMetroTileColor.Plum:
			elementStyle.BackColor = ColorScheme.GetColor(7156819);
			elementStyle.BackColor2 = ColorScheme.GetColor(8930161);
			elementStyle.BackColorGradientAngle = 45;
			break;
		case eMetroTileColor.Teal:
			elementStyle.BackColor = ColorScheme.GetColor(4565390);
			elementStyle.BackColor2 = ColorScheme.GetColor(5290910);
			elementStyle.BackColorGradientAngle = 45;
			break;
		case eMetroTileColor.Coffee:
			elementStyle.BackColor = ColorScheme.GetColor(7556137);
			elementStyle.BackColor2 = ColorScheme.GetColor(6701861);
			elementStyle.BackColorGradientAngle = 45;
			break;
		case eMetroTileColor.RedOrange:
			elementStyle.BackColor = ColorScheme.GetColor(15097376);
			elementStyle.BackColor2 = ColorScheme.GetColor(15430695);
			elementStyle.BackColorGradientAngle = 45;
			elementStyle.Border = eStyleBorderType.Solid;
			elementStyle.BorderColor = ColorScheme.GetColor(15565373);
			elementStyle.BorderWidth = 1;
			break;
		case eMetroTileColor.RedViolet:
			elementStyle.BackColor = ColorScheme.GetColor(7536710);
			elementStyle.BackColor2 = ColorScheme.GetColor(6684733);
			elementStyle.BackColorGradientAngle = 45;
			break;
		case eMetroTileColor.Olive:
			elementStyle.BackColor = ColorScheme.GetColor(12565265);
			elementStyle.BackColor2 = ColorScheme.GetColor(11710480);
			elementStyle.BackColorGradientAngle = 45;
			break;
		case eMetroTileColor.DarkOlive:
			elementStyle.BackColor = ColorScheme.GetColor(7895136);
			elementStyle.BackColor2 = ColorScheme.GetColor(7039830);
			elementStyle.BackColorGradientAngle = 45;
			break;
		case eMetroTileColor.Rust:
			elementStyle.BackColor = ColorScheme.GetColor(3151896);
			elementStyle.BackColor2 = ColorScheme.GetColor(2035471);
			elementStyle.BackColorGradientAngle = 45;
			break;
		case eMetroTileColor.Maroon:
			elementStyle.BackColor = ColorScheme.GetColor(6303792);
			elementStyle.BackColor2 = ColorScheme.GetColor(5187624);
			elementStyle.BackColorGradientAngle = 45;
			break;
		case eMetroTileColor.Yellowish:
			elementStyle.BackColor = ColorScheme.GetColor(15906565);
			elementStyle.BackColor2 = ColorScheme.GetColor(14723332);
			elementStyle.BackColorGradientAngle = 45;
			break;
		case eMetroTileColor.Blueish:
			elementStyle.BackColor = ColorScheme.GetColor(222604);
			elementStyle.BackColor2 = ColorScheme.GetColor(153722);
			elementStyle.BackColorGradientAngle = 45;
			break;
		case eMetroTileColor.DarkBlue:
			elementStyle.BackColor = ColorScheme.GetColor(213113);
			elementStyle.BackColor2 = ColorScheme.GetColor(216982);
			elementStyle.BackColorGradientAngle = 45;
			elementStyle.Border = eStyleBorderType.Solid;
			elementStyle.BorderColor = ColorScheme.GetColor(1924246);
			elementStyle.BorderWidth = 1;
			break;
		case eMetroTileColor.Yellow:
			elementStyle.BackColor = ColorScheme.GetColor(16763400);
			elementStyle.BackColor2 = ColorScheme.GetColor(16763400);
			elementStyle.BackColorGradientAngle = 45;
			elementStyle.TextColor = Color.Black;
			break;
		case eMetroTileColor.Gray:
			elementStyle.BackColor = ColorScheme.GetColor(8093566);
			elementStyle.BackColor2 = ColorScheme.GetColor(6185312);
			elementStyle.BackColorGradientAngle = 45;
			break;
		case eMetroTileColor.DarkGreen:
			elementStyle.BackColor = ColorScheme.GetColor(1545035);
			elementStyle.BackColor2 = ColorScheme.GetColor(1813852);
			elementStyle.BackColorGradientAngle = 45;
			elementStyle.Border = eStyleBorderType.Solid;
			elementStyle.BorderColor = ColorScheme.GetColor(3323245);
			elementStyle.BorderWidth = 1;
			break;
		case eMetroTileColor.MaroonWashed:
			elementStyle.BackColor = ColorScheme.GetColor(9851221);
			elementStyle.BackColor2 = ColorScheme.GetColor(9062990);
			elementStyle.BackColorGradientAngle = 45;
			break;
		case eMetroTileColor.PlumWashed:
			elementStyle.BackColor = ColorScheme.GetColor(4208460);
			elementStyle.BackColor2 = ColorScheme.GetColor(3550784);
			elementStyle.BackColorGradientAngle = 45;
			break;
		case eMetroTileColor.Azure:
			elementStyle.BackColor = ColorScheme.GetColor(43996);
			elementStyle.BackColor2 = ColorScheme.GetColor(49123);
			elementStyle.BackColorGradientAngle = 45;
			elementStyle.Border = eStyleBorderType.Solid;
			elementStyle.BorderColor = ColorScheme.GetColor(1754854);
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
		if (_TextMarkup != null)
		{
			_TextMarkup.HyperLinkClick -= TextMarkupLinkClick;
		}
		_TextMarkup = null;
		if (_TextMarkupEnabled && MarkupParser.IsMarkup(ref _Text))
		{
			_TextMarkup = MarkupParser.Parse(_Text);
			if (_TextMarkup != null)
			{
				_TextMarkup.HyperLinkClick += TextMarkupLinkClick;
			}
		}
	}

	protected virtual void TextMarkupLinkClick(object sender, EventArgs e)
	{
		if (sender is HyperLink hyperLink)
		{
			OnMarkupLinkClick(new MarkupLinkClickEventArgs(hyperLink.Name, hyperLink.HRef));
		}
	}

	protected virtual void OnMarkupLinkClick(MarkupLinkClickEventArgs e)
	{
		this.MarkupLinkClick?.Invoke(this, e);
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
			return TextDrawing.MeasureString(g, ButtonItemPainter.GetDrawText(item.Text), font, containerWidth, stringFormat);
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
		item.TextMarkupBody.Arrange(new Rectangle(0, 0, availableSize.Width, availableSize.Height), markupDrawContext);
		return item.TextMarkupBody.Bounds.Size;
	}

	protected virtual void OnImageTextAlignmentChanged(ContentAlignment oldValue, ContentAlignment newValue)
	{
		OnPropertyChanged(new PropertyChangedEventArgs("ImageTextAlignment"));
	}

	protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
	{
		this.PropertyChanged?.Invoke(this, e);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			_TileStyle.StyleChanged -= TileStyleChanged;
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
		metroTileFrame.SymbolSize = SymbolSize;
		return metroTileFrame;
	}
}
