using System.Drawing;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Stimulsoft.Editor;

[XmlRoot("SyntaxEdit")]
public class XmlSyntaxEditInfo
{
	private SyntaxEdit owner;

	private bool hideCaret;

	private bool disableColorPaint;

	private bool disableSyntaxPaint;

	private bool acceptTabs = true;

	private bool acceptReturns = true;

	private bool transparent;

	private bool autoCorrection;

	private SearchOptions searchOptions = EditConsts.DefaultSearchOptions;

	private char[] codeCompletionChars = EditConsts.DefaultCodeCompletionChars.ToCharArray();

	private string readonlyBackColor = Color.Empty.Name;

	private string disabledForeColor = Color.Empty.Name;

	private string disabledBackColor = Color.Empty.Name;

	private XmlDisplayStringsInfo displayStrings;

	private XmlSelectionInfo selection;

	private XmlGutterInfo gutter;

	private XmlMarginInfo margin;

	private XmlLineStylesInfo lineStyles;

	private XmlLineSeparatorInfo lineSeparator;

	private XmlPrintingInfo printing;

	private XmlWhiteSpaceInfo whiteSpace;

	private XmlTextSourceInfo textSource;

	private XmlScrollingInfo scrolling;

	private XmlOutliningInfo outlining;

	private XmlHyperTextExInfo hyperText;

	private XmlSpellingInfo spelling;

	private XmlEditPagesInfo pages;

	private XmlBracesInfo braces;

	private bool drawColumnsIndent;

	private bool useDefaultMenu;

	private ContextMenuStrip contextMenuStrip;

	private EditBorderStyle borderStyle;

	private string columnsIndentForeColor = EditConsts.DefaultColumnsIndentForeColor.Name;

	public SearchOptions SearchOptions
	{
		get
		{
			if (owner == null)
			{
				return searchOptions;
			}
			return owner.SearchOptions;
		}
		set
		{
			searchOptions = value;
			if (owner != null)
			{
				owner.SearchOptions = value;
			}
		}
	}

	public char[] CodeCompletionChars
	{
		get
		{
			if (owner == null)
			{
				return codeCompletionChars;
			}
			return owner.CodeCompletionChars;
		}
		set
		{
			codeCompletionChars = value;
			if (owner != null)
			{
				owner.CodeCompletionChars = value;
			}
		}
	}

	public bool AutoCorrection
	{
		get
		{
			if (owner == null)
			{
				return autoCorrection;
			}
			return owner.AutoCorrection;
		}
		set
		{
			autoCorrection = value;
			if (owner != null)
			{
				owner.AutoCorrection = value;
			}
		}
	}

	public string ReadonlyBackColor
	{
		get
		{
			if (owner == null)
			{
				return readonlyBackColor;
			}
			return XmlHelper.SerializeColor(owner.ReadonlyBackColor);
		}
		set
		{
			readonlyBackColor = value;
			if (owner != null)
			{
				owner.ReadonlyBackColor = XmlHelper.DeserializeColor(value);
			}
		}
	}

	public string DisabledForeColor
	{
		get
		{
			if (owner == null)
			{
				return disabledForeColor;
			}
			return XmlHelper.SerializeColor(owner.DisabledForeColor);
		}
		set
		{
			disabledForeColor = value;
			if (owner != null)
			{
				owner.DisabledForeColor = XmlHelper.DeserializeColor(value);
			}
		}
	}

	public string DisabledBackColor
	{
		get
		{
			if (owner == null)
			{
				return disabledBackColor;
			}
			return XmlHelper.SerializeColor(owner.DisabledBackColor);
		}
		set
		{
			disabledBackColor = value;
			if (owner != null)
			{
				owner.DisabledBackColor = XmlHelper.DeserializeColor(value);
			}
		}
	}

	public XmlEditPagesInfo Pages
	{
		get
		{
			if (owner == null)
			{
				return Pages;
			}
			return (XmlEditPagesInfo)((EditPages)owner.Pages).XmlInfo;
		}
		set
		{
			pages = value;
			if (owner != null && value != null)
			{
				((EditPages)owner.Pages).XmlInfo = value;
			}
		}
	}

	public bool Transparent
	{
		get
		{
			if (owner == null)
			{
				return transparent;
			}
			return owner.Transparent;
		}
		set
		{
			transparent = value;
			if (owner != null)
			{
				owner.Transparent = value;
			}
		}
	}

	public bool HideCaret
	{
		get
		{
			if (owner == null)
			{
				return hideCaret;
			}
			return owner.HideCaret;
		}
		set
		{
			hideCaret = value;
			if (owner != null)
			{
				owner.HideCaret = value;
			}
		}
	}

	public bool DisableColorPaint
	{
		get
		{
			if (owner == null)
			{
				return disableColorPaint;
			}
			return owner.DisableColorPaint;
		}
		set
		{
			disableColorPaint = value;
			if (owner != null)
			{
				owner.DisableColorPaint = value;
			}
		}
	}

	public bool DisableSyntaxPaint
	{
		get
		{
			if (owner == null)
			{
				return disableSyntaxPaint;
			}
			return owner.DisableSyntaxPaint;
		}
		set
		{
			disableSyntaxPaint = value;
			if (owner != null)
			{
				owner.DisableSyntaxPaint = value;
			}
		}
	}

	public bool AcceptTabs
	{
		get
		{
			if (owner == null)
			{
				return acceptTabs;
			}
			return owner.AcceptTabs;
		}
		set
		{
			acceptTabs = value;
			if (owner != null)
			{
				owner.AcceptTabs = value;
			}
		}
	}

	public bool AcceptReturns
	{
		get
		{
			if (owner == null)
			{
				return acceptReturns;
			}
			return owner.AcceptReturns;
		}
		set
		{
			acceptReturns = value;
			if (owner != null)
			{
				owner.AcceptReturns = value;
			}
		}
	}

	public XmlDisplayStringsInfo DisplayStrings
	{
		get
		{
			if (owner == null)
			{
				return displayStrings;
			}
			return (XmlDisplayStringsInfo)((DisplayStrings)owner.DisplayLines).XmlInfo;
		}
		set
		{
			displayStrings = value;
			if (owner != null && value != null)
			{
				((DisplayStrings)owner.DisplayLines).XmlInfo = value;
			}
		}
	}

	public XmlSelectionInfo Selection
	{
		get
		{
			if (owner == null)
			{
				return selection;
			}
			return (XmlSelectionInfo)((Selection)owner.Selection).XmlInfo;
		}
		set
		{
			selection = value;
			if (owner != null && value != null)
			{
				((Selection)owner.Selection).XmlInfo = value;
			}
		}
	}

	public XmlGutterInfo Gutter
	{
		get
		{
			if (owner == null)
			{
				return gutter;
			}
			return (XmlGutterInfo)((Gutter)owner.Gutter).XmlInfo;
		}
		set
		{
			gutter = value;
			if (owner != null && value != null)
			{
				((Gutter)owner.Gutter).XmlInfo = value;
			}
		}
	}

	public XmlMarginInfo Margin
	{
		get
		{
			if (owner == null)
			{
				return margin;
			}
			return (XmlMarginInfo)((Margin)owner.EditMargin).XmlInfo;
		}
		set
		{
			margin = value;
			if (owner != null && value != null)
			{
				((Margin)owner.EditMargin).XmlInfo = value;
			}
		}
	}

	public XmlLineStylesInfo LineStyles
	{
		get
		{
			if (owner == null)
			{
				return lineStyles;
			}
			return (XmlLineStylesInfo)((LineStylesEx)owner.LineStyles).XmlInfo;
		}
		set
		{
			lineStyles = value;
			if (owner != null && value != null)
			{
				((LineStylesEx)owner.LineStyles).XmlInfo = value;
			}
		}
	}

	public XmlLineSeparatorInfo LineSeparator
	{
		get
		{
			if (owner == null)
			{
				return lineSeparator;
			}
			return (XmlLineSeparatorInfo)((LineSeparator)owner.LineSeparator).XmlInfo;
		}
		set
		{
			lineSeparator = value;
			if (owner != null && value != null)
			{
				((LineSeparator)owner.LineSeparator).XmlInfo = value;
			}
		}
	}

	public XmlPrintingInfo Printing
	{
		get
		{
			if (owner == null)
			{
				return printing;
			}
			return (XmlPrintingInfo)((Printing)owner.Printing).XmlInfo;
		}
		set
		{
			printing = value;
			if (owner != null && value != null)
			{
				((Printing)owner.Printing).XmlInfo = value;
			}
		}
	}

	public XmlWhiteSpaceInfo WhiteSpace
	{
		get
		{
			if (owner == null)
			{
				return whiteSpace;
			}
			return (XmlWhiteSpaceInfo)((WhiteSpace)owner.WhiteSpace).XmlInfo;
		}
		set
		{
			whiteSpace = value;
			if (owner != null && value != null)
			{
				((WhiteSpace)owner.WhiteSpace).XmlInfo = value;
			}
		}
	}

	public XmlTextSourceInfo TextSource
	{
		get
		{
			if (owner == null)
			{
				return textSource;
			}
			return (XmlTextSourceInfo)((TextSource)owner.Source).XmlInfo;
		}
		set
		{
			textSource = value;
			if (owner != null && value != null)
			{
				((TextSource)owner.Source).XmlInfo = value;
			}
		}
	}

	public XmlScrollingInfo Scrolling
	{
		get
		{
			if (owner == null)
			{
				return scrolling;
			}
			return (XmlScrollingInfo)((Scrolling)owner.Scrolling).XmlInfo;
		}
		set
		{
			scrolling = value;
			if (owner != null && value != null)
			{
				((Scrolling)owner.Scrolling).XmlInfo = value;
			}
		}
	}

	public XmlOutliningInfo Outlining
	{
		get
		{
			if (owner == null)
			{
				return outlining;
			}
			return (XmlOutliningInfo)((Outlining)owner.Outlining).XmlInfo;
		}
		set
		{
			outlining = value;
			if (owner != null && value != null)
			{
				((Outlining)owner.Outlining).XmlInfo = value;
			}
		}
	}

	public XmlHyperTextExInfo HyperText
	{
		get
		{
			if (owner == null)
			{
				return hyperText;
			}
			return (XmlHyperTextExInfo)((HyperTextEx)owner.HyperText).XmlInfo;
		}
		set
		{
			hyperText = value;
			if (owner != null && value != null)
			{
				((HyperTextEx)owner.HyperText).XmlInfo = value;
			}
		}
	}

	public XmlSpellingInfo Spelling
	{
		get
		{
			if (owner == null)
			{
				return spelling;
			}
			return (XmlSpellingInfo)((Spelling)owner.Spelling).XmlInfo;
		}
		set
		{
			spelling = value;
			if (owner != null && value != null)
			{
				((Spelling)owner.Spelling).XmlInfo = value;
			}
		}
	}

	public XmlBracesInfo Braces
	{
		get
		{
			if (owner == null)
			{
				return braces;
			}
			return (XmlBracesInfo)((Braces)owner.Braces).XmlInfo;
		}
		set
		{
			braces = value;
			if (owner != null && value != null)
			{
				((Braces)owner.Braces).XmlInfo = value;
			}
		}
	}

	public bool DrawColumnsIndent
	{
		get
		{
			if (owner == null)
			{
				return drawColumnsIndent;
			}
			return owner.DrawColumnsIndent;
		}
		set
		{
			drawColumnsIndent = value;
			if (owner != null)
			{
				owner.DrawColumnsIndent = value;
			}
		}
	}

	public bool UseDefaultMenu
	{
		get
		{
			if (owner == null)
			{
				return useDefaultMenu;
			}
			return owner.UseDefaultMenu;
		}
		set
		{
			useDefaultMenu = value;
			if (owner != null)
			{
				owner.UseDefaultMenu = value;
			}
		}
	}

	public ContextMenuStrip ContextMenuStrip
	{
		get
		{
			if (owner == null)
			{
				return contextMenuStrip;
			}
			return ((Control)owner).ContextMenuStrip;
		}
		set
		{
			contextMenuStrip = value;
			if (owner != null)
			{
				((Control)owner).ContextMenuStrip = value;
			}
		}
	}

	public EditBorderStyle BorderStyle
	{
		get
		{
			if (owner == null)
			{
				return borderStyle;
			}
			return owner.BorderStyle;
		}
		set
		{
			borderStyle = value;
			if (owner != null)
			{
				owner.BorderStyle = value;
			}
		}
	}

	public string ColumnsIndentForeColor
	{
		get
		{
			if (owner == null)
			{
				return columnsIndentForeColor;
			}
			return XmlHelper.SerializeColor(owner.ColumnsIndentForeColor);
		}
		set
		{
			columnsIndentForeColor = value;
			if (owner != null)
			{
				owner.ColumnsIndentForeColor = XmlHelper.DeserializeColor(value);
			}
		}
	}

	public XmlSyntaxEditInfo()
	{
	}

	public XmlSyntaxEditInfo(SyntaxEdit owner)
		: this()
	{
		this.owner = owner;
	}

	public virtual void Load()
	{
		if (owner != null)
		{
			hideCaret = HideCaret;
			disableColorPaint = DisableColorPaint;
			disableSyntaxPaint = DisableSyntaxPaint;
			acceptTabs = AcceptTabs;
			acceptReturns = AcceptReturns;
			transparent = Transparent;
			searchOptions = SearchOptions;
			codeCompletionChars = CodeCompletionChars;
			autoCorrection = AutoCorrection;
			readonlyBackColor = ReadonlyBackColor;
			disabledForeColor = DisabledForeColor;
			disabledBackColor = DisabledBackColor;
			displayStrings = DisplayStrings;
			selection = Selection;
			gutter = Gutter;
			margin = Margin;
			lineStyles = LineStyles;
			lineSeparator = LineSeparator;
			printing = Printing;
			whiteSpace = WhiteSpace;
			textSource = TextSource;
			scrolling = Scrolling;
			outlining = Outlining;
			hyperText = HyperText;
			spelling = Spelling;
			pages = Pages;
			braces = Braces;
			drawColumnsIndent = DrawColumnsIndent;
			useDefaultMenu = UseDefaultMenu;
			contextMenuStrip = ContextMenuStrip;
			borderStyle = BorderStyle;
			columnsIndentForeColor = ColumnsIndentForeColor;
			if (displayStrings != null)
			{
				displayStrings.Load();
			}
			if (selection != null)
			{
				selection.Load();
			}
			if (gutter != null)
			{
				gutter.Load();
			}
			if (margin != null)
			{
				margin.Load();
			}
			if (lineStyles != null)
			{
				lineStyles.Load();
			}
			if (lineSeparator != null)
			{
				lineSeparator.Load();
			}
			if (printing != null)
			{
				printing.Load();
			}
			if (whiteSpace != null)
			{
				whiteSpace.Load();
			}
			if (textSource != null)
			{
				textSource.Load();
			}
			if (scrolling != null)
			{
				scrolling.Load();
			}
			if (outlining != null)
			{
				outlining.Load();
			}
			if (hyperText != null)
			{
				hyperText.Load();
			}
			if (spelling != null)
			{
				spelling.Load();
			}
			if (pages != null)
			{
				pages.Load();
			}
			if (braces != null)
			{
				braces.Load();
			}
		}
	}

	public virtual void FixupReferences(SyntaxEdit owner)
	{
		this.owner = owner;
		HideCaret = hideCaret;
		DisableColorPaint = disableColorPaint;
		DisableSyntaxPaint = disableSyntaxPaint;
		AcceptTabs = acceptTabs;
		AcceptReturns = acceptReturns;
		DisplayStrings = displayStrings;
		Selection = selection;
		Gutter = gutter;
		Margin = margin;
		LineStyles = lineStyles;
		LineSeparator = lineSeparator;
		Printing = printing;
		WhiteSpace = whiteSpace;
		TextSource = textSource;
		Scrolling = scrolling;
		Outlining = outlining;
		HyperText = hyperText;
		Spelling = spelling;
		Pages = pages;
		Braces = braces;
		Transparent = transparent;
		SearchOptions = searchOptions;
		CodeCompletionChars = codeCompletionChars;
		AutoCorrection = autoCorrection;
		ReadonlyBackColor = readonlyBackColor;
		DisabledForeColor = disabledForeColor;
		DisabledBackColor = disabledBackColor;
		DrawColumnsIndent = drawColumnsIndent;
		UseDefaultMenu = useDefaultMenu;
		ContextMenuStrip = contextMenuStrip;
		BorderStyle = borderStyle;
		ColumnsIndentForeColor = columnsIndentForeColor;
	}
}
