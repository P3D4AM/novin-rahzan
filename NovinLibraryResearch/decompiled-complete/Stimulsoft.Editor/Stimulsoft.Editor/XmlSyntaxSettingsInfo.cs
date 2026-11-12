using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Stimulsoft.Editor;

public class XmlSyntaxSettingsInfo
{
	private SyntaxSettings owner;

	private Font font;

	private string fontName;

	private float fontSize;

	private FontStyle fontStyle;

	private NavigateOptions navigateOptions = EditConsts.DefaultNavigateOptions;

	private RichTextBoxScrollBars scrollBars = (RichTextBoxScrollBars)3;

	private SelectionOptions selectionOptions = EditConsts.DefaultSelectionOptions;

	private GutterOptions gutterOptions = EditConsts.DefaultGutterOptions;

	private OutlineOptions outlineOptions = EditConsts.DefaultOutlineOptions;

	private bool showGutter = true;

	private bool showMargin = true;

	private bool highlightUrls = true;

	private bool allowOutlining;

	private bool useSpaces;

	private bool wordWrap;

	private int gutterWidth = EditConsts.DefaultGutterWidth;

	private int marginPos = EditConsts.DefaultMarginPosition;

	private int activeThemeIndex;

	private int[] tabStops = new int[1] { EditConsts.DefaultTabStop };

	private LexStyle[] lexStyles = new LexStyle[0];

	private ColorTheme[] colorThemes = new ColorTheme[0];

	private string[] eventNames = new string[0];

	private KeyListData[] eventData = new KeyListData[0];

	[XmlIgnore]
	public Font Font
	{
		get
		{
			if (owner == null)
			{
				return font;
			}
			return owner.Font;
		}
		set
		{
			font = value;
			if (owner != null)
			{
				owner.Font = value;
			}
		}
	}

	public string FontName
	{
		get
		{
			if (owner == null)
			{
				return fontName;
			}
			return owner.Font.Name;
		}
		set
		{
			fontName = value;
		}
	}

	public float FontSize
	{
		get
		{
			if (owner == null)
			{
				return fontSize;
			}
			return owner.Font.Size;
		}
		set
		{
			fontSize = value;
		}
	}

	public FontStyle FontStyle
	{
		get
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			if (owner == null)
			{
				return fontStyle;
			}
			return owner.Font.Style;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			fontStyle = value;
		}
	}

	public NavigateOptions NavOptions
	{
		get
		{
			if (owner == null)
			{
				return navigateOptions;
			}
			return owner.NavigateOptions;
		}
		set
		{
			navigateOptions = value;
			if (owner != null)
			{
				owner.NavigateOptions = value;
			}
		}
	}

	public RichTextBoxScrollBars ScrollBars
	{
		get
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			if (owner == null)
			{
				return scrollBars;
			}
			return owner.ScrollBars;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			scrollBars = value;
			if (owner != null)
			{
				owner.ScrollBars = value;
			}
		}
	}

	public SelectionOptions SelOptions
	{
		get
		{
			if (owner == null)
			{
				return selectionOptions;
			}
			return owner.SelectionOptions;
		}
		set
		{
			selectionOptions = value;
			if (owner != null)
			{
				owner.SelectionOptions = value;
			}
		}
	}

	public GutterOptions GutterOptions
	{
		get
		{
			if (owner == null)
			{
				return gutterOptions;
			}
			return owner.GutterOptions;
		}
		set
		{
			gutterOptions = value;
			if (owner != null)
			{
				owner.GutterOptions = value;
			}
		}
	}

	public OutlineOptions OutlineOptions
	{
		get
		{
			if (owner == null)
			{
				return outlineOptions;
			}
			return owner.OutlineOptions;
		}
		set
		{
			outlineOptions = value;
			if (owner != null)
			{
				owner.OutlineOptions = value;
			}
		}
	}

	public bool ShowGutter
	{
		get
		{
			if (owner == null)
			{
				return showGutter;
			}
			return owner.ShowGutter;
		}
		set
		{
			showGutter = value;
			if (owner != null)
			{
				owner.ShowGutter = value;
			}
		}
	}

	public bool ShowMargin
	{
		get
		{
			if (owner == null)
			{
				return showMargin;
			}
			return owner.ShowMargin;
		}
		set
		{
			showMargin = value;
			if (owner != null)
			{
				owner.ShowMargin = value;
			}
		}
	}

	public bool HighlightUrls
	{
		get
		{
			if (owner == null)
			{
				return highlightUrls;
			}
			return owner.HighlightUrls;
		}
		set
		{
			highlightUrls = value;
			if (owner != null)
			{
				owner.HighlightUrls = value;
			}
		}
	}

	public bool AllowOutlining
	{
		get
		{
			if (owner == null)
			{
				return allowOutlining;
			}
			return owner.AllowOutlining;
		}
		set
		{
			allowOutlining = value;
			if (owner != null)
			{
				owner.AllowOutlining = value;
			}
		}
	}

	public bool UseSpaces
	{
		get
		{
			if (owner == null)
			{
				return useSpaces;
			}
			return owner.UseSpaces;
		}
		set
		{
			useSpaces = value;
			if (owner != null)
			{
				owner.UseSpaces = value;
			}
		}
	}

	public bool WordWrap
	{
		get
		{
			if (owner == null)
			{
				return wordWrap;
			}
			return owner.WordWrap;
		}
		set
		{
			wordWrap = value;
			if (owner != null)
			{
				owner.WordWrap = value;
			}
		}
	}

	public int GutterWidth
	{
		get
		{
			if (owner == null)
			{
				return gutterWidth;
			}
			return owner.GutterWidth;
		}
		set
		{
			gutterWidth = value;
			if (owner != null)
			{
				owner.GutterWidth = value;
			}
		}
	}

	public int MarginPos
	{
		get
		{
			if (owner == null)
			{
				return marginPos;
			}
			return owner.MarginPos;
		}
		set
		{
			marginPos = value;
			if (owner != null)
			{
				owner.MarginPos = value;
			}
		}
	}

	public int[] TabStops
	{
		get
		{
			if (owner == null)
			{
				return tabStops;
			}
			return owner.TabStops;
		}
		set
		{
			tabStops = value;
			if (owner != null)
			{
				owner.TabStops = new int[value.Length];
				Array.Copy(value, owner.TabStops, value.Length);
			}
		}
	}

	[XmlIgnore]
	public LexStyle[] LexStyles
	{
		get
		{
			if (owner == null)
			{
				return lexStyles;
			}
			return owner.LexStyles;
		}
		set
		{
			lexStyles = value;
			if (owner != null)
			{
				owner.LexStyles = new LexStyle[value.Length];
				Array.Copy(value, owner.LexStyles, value.Length);
			}
		}
	}

	public ColorTheme[] ColorThemes
	{
		get
		{
			if (owner == null)
			{
				return colorThemes;
			}
			return owner.Themes;
		}
		set
		{
			colorThemes = value;
			if (owner != null)
			{
				ColorTheme[] array = new ColorTheme[value.Length];
				Array.Copy(value, array, value.Length);
				owner.Themes = array;
			}
		}
	}

	public int ActiveThemeIndex
	{
		get
		{
			if (owner == null)
			{
				return ActiveThemeIndex;
			}
			return owner.ColorThemes.ActiveThemeIndex;
		}
		set
		{
			activeThemeIndex = value;
			if (owner != null)
			{
				owner.ColorThemes.ActiveThemeIndex = value;
			}
		}
	}

	[XmlIgnore]
	public string[] EventNames
	{
		get
		{
			if (owner == null)
			{
				return eventNames;
			}
			return owner.EventNames;
		}
		set
		{
			eventNames = value;
		}
	}

	[XmlIgnore]
	public KeyListData[] EventData
	{
		get
		{
			if (owner == null)
			{
				return eventData;
			}
			return owner.EventData;
		}
		set
		{
			eventData = value;
		}
	}

	public XmlSyntaxSettingsInfo()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		fontName = FontFamily.GenericMonospace.Name;
		fontSize = 10f;
		fontStyle = (FontStyle)0;
	}

	public XmlSyntaxSettingsInfo(SyntaxSettings owner)
		: this()
	{
		this.owner = owner;
	}

	public virtual void Load()
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		if (owner != null)
		{
			fontName = Font.Name;
			fontSize = Font.Size;
			fontStyle = Font.Style;
			navigateOptions = NavOptions;
			scrollBars = ScrollBars;
			selectionOptions = SelOptions;
			gutterOptions = GutterOptions;
			outlineOptions = OutlineOptions;
			showGutter = ShowGutter;
			showMargin = ShowMargin;
			highlightUrls = HighlightUrls;
			allowOutlining = AllowOutlining;
			useSpaces = UseSpaces;
			wordWrap = WordWrap;
			gutterWidth = GutterWidth;
			marginPos = MarginPos;
			tabStops = TabStops;
			colorThemes = ColorThemes;
			lexStyles = LexStyles;
			activeThemeIndex = ActiveThemeIndex;
			eventNames = EventNames;
			eventData = EventData;
		}
	}

	public virtual void FixupReferences(SyntaxSettings owner)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Expected O, but got Unknown
		this.owner = owner;
		NavOptions = navigateOptions;
		ScrollBars = scrollBars;
		SelOptions = selectionOptions;
		GutterOptions = gutterOptions;
		OutlineOptions = outlineOptions;
		ShowGutter = showGutter;
		ShowMargin = showMargin;
		HighlightUrls = highlightUrls;
		AllowOutlining = allowOutlining;
		UseSpaces = useSpaces;
		WordWrap = wordWrap;
		GutterWidth = gutterWidth;
		MarginPos = marginPos;
		TabStops = tabStops;
		LexStyles = lexStyles;
		ColorThemes = colorThemes;
		Font = new Font(fontName, fontSize, fontStyle);
		ActiveThemeIndex = activeThemeIndex;
		EventNames = eventNames;
		EventData = eventData;
		ColorThemes = colorThemes;
	}
}
