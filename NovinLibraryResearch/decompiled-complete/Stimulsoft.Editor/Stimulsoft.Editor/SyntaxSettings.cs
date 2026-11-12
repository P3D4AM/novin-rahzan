using System;
using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Editor;

public class SyntaxSettings : PersistentSettings, ISyntaxSettings, IPersistentSettings
{
	private IColorThemes colorThemes = new ColorThemes();

	private NavigateOptions navigateOptions;

	private RichTextBoxScrollBars scrollBars;

	private SelectionOptions selectionOptions;

	private GutterOptions gutterOptions;

	private SeparatorOptions separatorOptions;

	private OutlineOptions outlineOptions;

	private bool showMargin;

	private bool showGutter = true;

	private bool highlightUrls = true;

	private bool allowOutlining;

	private bool useSpaces;

	private bool wordWrap;

	private bool whiteSpaceVisible;

	private PageType pageType;

	private int gutterWidth = EditConsts.DefaultGutterWidth;

	private int marginPos = EditConsts.DefaultMarginPosition;

	private int[] tabStops = new int[1] { EditConsts.DefaultTabStop };

	private string[] eventNames = new string[0];

	private KeyListData[] eventData = new KeyListData[0];

	private KeyList keyList;

	private const int cColorStyles = 18;

	public LexStyle[] LexStyles
	{
		get
		{
			return colorThemes.ActiveTheme.LexStyles;
		}
		set
		{
			colorThemes.ActiveTheme.LexStyles = new LexStyle[value.Length];
			Array.Copy(value, colorThemes.ActiveTheme.LexStyles, value.Length);
		}
	}

	public IColorThemes ColorThemes
	{
		get
		{
			return colorThemes;
		}
		set
		{
			colorThemes = value;
		}
	}

	public ColorTheme[] Themes
	{
		get
		{
			return (ColorTheme[])colorThemes.Themes;
		}
		set
		{
			colorThemes.Themes = value;
		}
	}

	public Font Font
	{
		get
		{
			return colorThemes.ActiveTheme.Font;
		}
		set
		{
			colorThemes.ActiveTheme.Font = value;
		}
	}

	public NavigateOptions NavigateOptions
	{
		get
		{
			return navigateOptions;
		}
		set
		{
			navigateOptions = value;
		}
	}

	public RichTextBoxScrollBars ScrollBars
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return scrollBars;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			scrollBars = value;
		}
	}

	public SelectionOptions SelectionOptions
	{
		get
		{
			return selectionOptions;
		}
		set
		{
			selectionOptions = value;
		}
	}

	public GutterOptions GutterOptions
	{
		get
		{
			return gutterOptions;
		}
		set
		{
			gutterOptions = value;
		}
	}

	public SeparatorOptions SeparatorOptions
	{
		get
		{
			return separatorOptions;
		}
		set
		{
			separatorOptions = value;
		}
	}

	public OutlineOptions OutlineOptions
	{
		get
		{
			return outlineOptions;
		}
		set
		{
			outlineOptions = value;
		}
	}

	public bool ShowMargin
	{
		get
		{
			return showMargin;
		}
		set
		{
			showMargin = value;
		}
	}

	public bool ShowGutter
	{
		get
		{
			return showGutter;
		}
		set
		{
			showGutter = value;
		}
	}

	public bool HighlightUrls
	{
		get
		{
			return highlightUrls;
		}
		set
		{
			highlightUrls = value;
		}
	}

	public bool AllowOutlining
	{
		get
		{
			return allowOutlining;
		}
		set
		{
			allowOutlining = value;
		}
	}

	public bool UseSpaces
	{
		get
		{
			return useSpaces;
		}
		set
		{
			useSpaces = value;
		}
	}

	public bool WordWrap
	{
		get
		{
			return wordWrap;
		}
		set
		{
			wordWrap = value;
		}
	}

	public bool WhiteSpaceVisible
	{
		get
		{
			return whiteSpaceVisible;
		}
		set
		{
			whiteSpaceVisible = value;
		}
	}

	public int GutterWidth
	{
		get
		{
			return gutterWidth;
		}
		set
		{
			gutterWidth = value;
		}
	}

	public int MarginPos
	{
		get
		{
			return marginPos;
		}
		set
		{
			marginPos = value;
		}
	}

	public int[] TabStops
	{
		get
		{
			return tabStops;
		}
		set
		{
			tabStops = new int[value.Length];
			Array.Copy(value, tabStops, value.Length);
		}
	}

	public PageType PageType
	{
		get
		{
			return pageType;
		}
		set
		{
			pageType = value;
		}
	}

	public string[] EventNames
	{
		get
		{
			return eventNames;
		}
		set
		{
			eventNames = value;
		}
	}

	public KeyListData[] EventData
	{
		get
		{
			return eventData;
		}
		set
		{
			eventData = value;
		}
	}

	public KeyList KeyList
	{
		get
		{
			return keyList;
		}
		set
		{
			keyList = value;
		}
	}

	public override object XmlInfo
	{
		get
		{
			return new XmlSyntaxSettingsInfo(this);
		}
		set
		{
			((XmlSyntaxSettingsInfo)value).FixupReferences(this);
		}
	}

	public SyntaxSettings()
	{
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		navigateOptions = EditConsts.DefaultNavigateOptions;
		scrollBars = (RichTextBoxScrollBars)3;
		selectionOptions = EditConsts.DefaultSelectionOptions;
		gutterOptions = EditConsts.DefaultGutterOptions;
		separatorOptions = SeparatorOptions.None;
		outlineOptions = EditConsts.DefaultOutlineOptions;
		InitializeDefaultColorThemes();
	}

	private void FillStyles(out LexStyle[] lexStyleArray)
	{
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		lexStyleArray = new LexStyle[LexStyleItems.Items.Length];
		for (int i = 0; i < lexStyleArray.Length; i++)
		{
			LexStyle lexStyle = new LexStyle();
			LexStyleItem lexStyleItem = LexStyleItems.Items[i];
			lexStyle.Name = lexStyleItem.InternalName;
			lexStyle.Desc = lexStyleItem.Name;
			lexStyle.ForeColor = lexStyleItem.ForeColor;
			lexStyle.BackColor = lexStyleItem.BackColor;
			lexStyle.FontStyle = lexStyleItem.FontStyle;
			lexStyle.PlainText = lexStyleItem.PlainText;
			lexStyle.BackColorEnabled = i < 18 || lexStyleItem.BackColor != Color.Empty;
			lexStyle.ForeColorEnabled = i < 18 || lexStyleItem.ForeColor != Color.Empty;
			lexStyleArray[i] = lexStyle;
		}
	}

	private void InitializeDefaultColorThemes()
	{
		colorThemes.Clear();
		allowOutlining = true;
		gutterOptions |= GutterOptions.PaintLineNumbers | GutterOptions.PaintLinesOnGutter | GutterOptions.PaintLineModificators;
		FillStyles(out var lexStyleArray);
		ColorTheme colorTheme = new ColorTheme("Default", readOnly: true, "Courier New", 10f, (FontStyle)0, lexStyleArray);
		LexStyle[] array = lexStyleArray;
		foreach (ILexStyle lexStyle in array)
		{
			colorTheme[lexStyle.Name].Assign(lexStyle);
		}
		colorThemes.Add(colorTheme);
		colorThemes.ActiveThemeIndex = 0;
	}

	private void CopyStyles(ILexStyle[] fromStyles, ILexStyle[] toStyles)
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		if (fromStyles == null || toStyles == null)
		{
			return;
		}
		foreach (ILexStyle lexStyle in fromStyles)
		{
			int styleByName = GetStyleByName(toStyles, lexStyle.Name);
			if (styleByName >= 0)
			{
				ILexStyle lexStyle2 = toStyles[styleByName];
				lexStyle2.ForeColor = lexStyle.ForeColor;
				lexStyle2.BackColor = lexStyle.BackColor;
				lexStyle2.FontStyle = lexStyle.FontStyle;
				lexStyle2.ForeColorEnabled = lexStyle.ForeColorEnabled;
				lexStyle2.BackColorEnabled = lexStyle.BackColorEnabled;
				lexStyle2.BoldEnabled = lexStyle.BoldEnabled;
				lexStyle2.ItalicEnabled = lexStyle.ItalicEnabled;
				lexStyle2.UnderlineEnabled = lexStyle.UnderlineEnabled;
			}
		}
	}

	private int GetStyleByName(ILexStyle[] styles, string name)
	{
		for (int i = 0; i < styles.Length; i++)
		{
			if (string.Compare(styles[i].Name, name, ignoreCase: true) == 0)
			{
				return i;
			}
		}
		return -1;
	}

	private ILexStyle[] GetLexStyles(ISyntaxEdit edit)
	{
		return (edit.Source.Lexer != null) ? ((LexScheme)edit.Source.Lexer.Scheme).Styles : null;
	}

	public void Localize()
	{
		LexStyleItems.Localize();
		InitializeDefaultColorThemes();
	}

	public bool IsDescriptionEnabled(int index)
	{
		if (index >= 0)
		{
			return colorThemes.ActiveTheme.LexStyles[index].Desc != string.Empty;
		}
		return false;
	}

	public bool IsFontStyleEnabled(int index)
	{
		if (index >= 0)
		{
			return index < 18;
		}
		return false;
	}

	public bool IsBackColorEnabled(int index)
	{
		if (index >= 0)
		{
			return colorThemes.ActiveTheme.LexStyles[index].BackColorEnabled;
		}
		return false;
	}

	public void LoadFromEdit(SyntaxEdit edit)
	{
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0239: Unknown result type (might be due to invalid IL or missing references)
		GutterOptions = edit.Gutter.Options;
		SeparatorOptions = edit.LineSeparator.Options;
		GutterWidth = edit.Gutter.Width;
		HighlightUrls = edit.HyperText.HighlightUrls;
		MarginPos = edit.EditMargin.Position;
		NavigateOptions = edit.NavigateOptions;
		AllowOutlining = edit.Outlining.AllowOutlining;
		OutlineOptions = edit.Outlining.OutlineOptions;
		ScrollBars = edit.Scrolling.ScrollBars;
		SelectionOptions = edit.Selection.Options;
		ShowGutter = edit.Gutter.Visible;
		ShowMargin = edit.EditMargin.Visible;
		TabStops = edit.Source.Lines.TabStops;
		UseSpaces = edit.Source.Lines.UseSpaces;
		WordWrap = edit.WordWrap;
		WhiteSpaceVisible = edit.WhiteSpace.Visible;
		PageType = edit.Pages.PageType;
		EventNames = ((KeyList)edit.KeyList).Handlers.EventNames;
		EventData = ((KeyList)edit.KeyList).EventData;
		KeyList = (KeyList)edit.KeyList;
		Font = ((Control)edit).Font;
		if (colorThemes.ActiveTheme[StringConsts.WindowColorInternalName] != null)
		{
			colorThemes.ActiveTheme[StringConsts.WindowColorInternalName].BackColor = ((Control)edit).BackColor;
		}
		if (colorThemes.ActiveTheme[StringConsts.MisspelledWordsInternalName] != null)
		{
			colorThemes.ActiveTheme[StringConsts.MisspelledWordsInternalName].ForeColor = edit.Spelling.SpellColor;
		}
		if (colorThemes.ActiveTheme[StringConsts.HyperTextInternalName] != null)
		{
			colorThemes.ActiveTheme[StringConsts.HyperTextInternalName].ForeColor = edit.HyperText.UrlColor;
			colorThemes.ActiveTheme[StringConsts.HyperTextInternalName].FontStyle = edit.HyperText.UrlStyle;
		}
		if (colorThemes.ActiveTheme[StringConsts.LineNumbersInternalName] != null)
		{
			colorThemes.ActiveTheme[StringConsts.LineNumbersInternalName].ForeColor = edit.Gutter.LineNumbersForeColor;
		}
		if (colorThemes.ActiveTheme[StringConsts.GutterPenColorInternalName] != null)
		{
			colorThemes.ActiveTheme[StringConsts.GutterPenColorInternalName].BackColor = edit.Gutter.BrushColor;
			colorThemes.ActiveTheme[StringConsts.GutterPenColorInternalName].ForeColor = edit.Gutter.PenColor;
		}
		if (colorThemes.ActiveTheme[StringConsts.WhiteSpaceInternalName] != null)
		{
			colorThemes.ActiveTheme[StringConsts.WhiteSpaceInternalName].ForeColor = edit.WhiteSpace.SymbolColor;
		}
		if (colorThemes.ActiveTheme[StringConsts.LineModificatorChangedInternalName] != null)
		{
			colorThemes.ActiveTheme[StringConsts.LineModificatorChangedInternalName].ForeColor = edit.Gutter.LineModificatorChangedColor;
		}
		if (colorThemes.ActiveTheme[StringConsts.LineModificatorSavedInternalName] != null)
		{
			colorThemes.ActiveTheme[StringConsts.LineModificatorSavedInternalName].ForeColor = edit.Gutter.LineModificatorSavedColor;
		}
		if (colorThemes.ActiveTheme[StringConsts.LineSeparatorInternalName] != null)
		{
			colorThemes.ActiveTheme[StringConsts.LineSeparatorInternalName].ForeColor = edit.LineSeparator.LineColor;
		}
		if (colorThemes.ActiveTheme[StringConsts.BraceMatchingInternalName] != null)
		{
			colorThemes.ActiveTheme[StringConsts.BraceMatchingInternalName].ForeColor = edit.Braces.ForeColor;
			colorThemes.ActiveTheme[StringConsts.BraceMatchingInternalName].BackColor = edit.Braces.BackColor;
		}
		if (colorThemes.ActiveTheme[StringConsts.CodeOutlinePenColorInternalName] != null)
		{
			colorThemes.ActiveTheme[StringConsts.CodeOutlinePenColorInternalName].ForeColor = edit.Outlining.OutlineColor;
		}
		if (colorThemes.ActiveTheme[StringConsts.ActiveSelectionInternalName] != null)
		{
			colorThemes.ActiveTheme[StringConsts.ActiveSelectionInternalName].ForeColor = edit.Selection.ForeColor;
			colorThemes.ActiveTheme[StringConsts.ActiveSelectionInternalName].BackColor = edit.Selection.BackColor;
		}
		if (colorThemes.ActiveTheme[StringConsts.InactiveSelectionInternalName] != null)
		{
			colorThemes.ActiveTheme[StringConsts.InactiveSelectionInternalName].ForeColor = edit.Selection.InActiveForeColor;
			colorThemes.ActiveTheme[StringConsts.InactiveSelectionInternalName].BackColor = edit.Selection.InActiveBackColor;
		}
	}

	public void ApplyToEdit(SyntaxEdit edit)
	{
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		edit.Source.BeginUpdate(UpdateReason.Other);
		try
		{
			edit.Outlining.AllowOutlining = AllowOutlining;
			edit.Outlining.OutlineOptions = OutlineOptions;
			((Control)edit).Font = Font;
			edit.Gutter.Options = GutterOptions;
			edit.LineSeparator.Options = SeparatorOptions;
			edit.Gutter.Width = GutterWidth;
			edit.EditMargin.Position = MarginPos;
			edit.NavigateOptions = NavigateOptions;
			edit.Scrolling.ScrollBars = ScrollBars;
			edit.Selection.Options = SelectionOptions;
			edit.Gutter.Visible = ShowGutter;
			edit.EditMargin.Visible = ShowMargin;
			edit.Source.Lines.TabStops = TabStops;
			edit.Source.Lines.UseSpaces = UseSpaces;
			edit.WordWrap = WordWrap;
			edit.WhiteSpace.Visible = WhiteSpaceVisible;
			edit.Pages.PageType = PageType;
			ILexStyle[] lexStyles = LexStyles;
			CopyStyles(lexStyles, GetLexStyles(edit));
			edit.HyperText.HighlightUrls = HighlightUrls;
			if (colorThemes.ActiveTheme[StringConsts.MisspelledWordsInternalName] != null)
			{
				edit.Spelling.SpellColor = colorThemes.ActiveTheme[StringConsts.MisspelledWordsInternalName].ForeColor;
			}
			if (colorThemes.ActiveTheme[StringConsts.HyperTextInternalName] != null)
			{
				edit.HyperText.UrlColor = colorThemes.ActiveTheme[StringConsts.HyperTextInternalName].ForeColor;
				edit.HyperText.UrlStyle = colorThemes.ActiveTheme[StringConsts.HyperTextInternalName].FontStyle;
			}
			if (colorThemes.ActiveTheme[StringConsts.LineNumbersInternalName] != null)
			{
				edit.Gutter.LineNumbersForeColor = colorThemes.ActiveTheme[StringConsts.LineNumbersInternalName].ForeColor;
			}
			if (colorThemes.ActiveTheme[StringConsts.WindowColorInternalName] != null)
			{
				edit.Gutter.LineNumbersBackColor = colorThemes.ActiveTheme[StringConsts.WindowColorInternalName].BackColor;
			}
			if (colorThemes.ActiveTheme[StringConsts.GutterPenColorInternalName] != null)
			{
				edit.Gutter.BrushColor = colorThemes.ActiveTheme[StringConsts.GutterPenColorInternalName].BackColor;
				edit.Gutter.PenColor = colorThemes.ActiveTheme[StringConsts.GutterPenColorInternalName].ForeColor;
			}
			if (colorThemes.ActiveTheme[StringConsts.WhiteSpaceInternalName] != null)
			{
				edit.WhiteSpace.SymbolColor = colorThemes.ActiveTheme[StringConsts.WhiteSpaceInternalName].ForeColor;
			}
			if (colorThemes.ActiveTheme[StringConsts.LineModificatorChangedInternalName] != null)
			{
				edit.Gutter.LineModificatorChangedColor = colorThemes.ActiveTheme[StringConsts.LineModificatorChangedInternalName].ForeColor;
			}
			if (colorThemes.ActiveTheme[StringConsts.LineModificatorSavedInternalName] != null)
			{
				edit.Gutter.LineModificatorSavedColor = colorThemes.ActiveTheme[StringConsts.LineModificatorSavedInternalName].ForeColor;
			}
			if (colorThemes.ActiveTheme[StringConsts.LineSeparatorInternalName] != null)
			{
				edit.LineSeparator.LineColor = colorThemes.ActiveTheme[StringConsts.LineSeparatorInternalName].ForeColor;
			}
			if (colorThemes.ActiveTheme[StringConsts.WindowColorInternalName] != null && colorThemes.ActiveTheme[StringConsts.WindowColorInternalName].BackColor != Color.Empty)
			{
				((Control)edit).BackColor = colorThemes.ActiveTheme[StringConsts.WindowColorInternalName].BackColor;
			}
			if (colorThemes.ActiveTheme[StringConsts.BraceMatchingInternalName] != null)
			{
				edit.Braces.ForeColor = colorThemes.ActiveTheme[StringConsts.BraceMatchingInternalName].ForeColor;
				edit.Braces.BackColor = colorThemes.ActiveTheme[StringConsts.BraceMatchingInternalName].BackColor;
			}
			if (colorThemes.ActiveTheme[StringConsts.CodeOutlinePenColorInternalName] != null)
			{
				edit.Outlining.OutlineColor = colorThemes.ActiveTheme[StringConsts.CodeOutlinePenColorInternalName].ForeColor;
			}
			if (colorThemes.ActiveTheme[StringConsts.ActiveSelectionInternalName] != null)
			{
				edit.Selection.ForeColor = colorThemes.ActiveTheme[StringConsts.ActiveSelectionInternalName].ForeColor;
				edit.Selection.BackColor = colorThemes.ActiveTheme[StringConsts.ActiveSelectionInternalName].BackColor;
			}
			if (colorThemes.ActiveTheme[StringConsts.InactiveSelectionInternalName] != null)
			{
				edit.Selection.InActiveForeColor = colorThemes.ActiveTheme[StringConsts.InactiveSelectionInternalName].ForeColor;
				edit.Selection.InActiveBackColor = colorThemes.ActiveTheme[StringConsts.InactiveSelectionInternalName].BackColor;
			}
		}
		finally
		{
			edit.Source.EndUpdate();
		}
		((Control)edit).Invalidate();
	}

	public override void Assign(IPersistentSettings source)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		if (source is SyntaxSettings)
		{
			SyntaxSettings syntaxSettings = (SyntaxSettings)source;
			navigateOptions = syntaxSettings.navigateOptions;
			scrollBars = syntaxSettings.scrollBars;
			selectionOptions = syntaxSettings.selectionOptions;
			gutterOptions = syntaxSettings.gutterOptions;
			separatorOptions = syntaxSettings.separatorOptions;
			outlineOptions = syntaxSettings.outlineOptions;
			showMargin = syntaxSettings.showMargin;
			showGutter = syntaxSettings.showGutter;
			highlightUrls = syntaxSettings.highlightUrls;
			allowOutlining = syntaxSettings.allowOutlining;
			useSpaces = syntaxSettings.useSpaces;
			wordWrap = syntaxSettings.wordWrap;
			whiteSpaceVisible = syntaxSettings.whiteSpaceVisible;
			pageType = syntaxSettings.pageType;
			gutterWidth = syntaxSettings.gutterWidth;
			marginPos = syntaxSettings.marginPos;
			TabStops = syntaxSettings.TabStops;
			colorThemes = (ColorThemes)syntaxSettings.colorThemes.Clone();
			eventNames = new string[syntaxSettings.EventNames.Length];
			syntaxSettings.EventNames.CopyTo(eventNames, 0);
			eventData = new KeyListData[syntaxSettings.EventData.Length];
			syntaxSettings.EventData.CopyTo(eventData, 0);
			KeyList = syntaxSettings.KeyList;
		}
	}

	public override Type GetXmlType()
	{
		return typeof(XmlSyntaxSettingsInfo);
	}
}
