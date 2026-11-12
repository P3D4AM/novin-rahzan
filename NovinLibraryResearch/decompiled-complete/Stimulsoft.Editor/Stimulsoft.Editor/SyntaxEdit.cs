using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Stimulsoft.Editor;

[ToolboxItem(false)]
public class SyntaxEdit : Control, ISyntaxEdit, ISyntaxPaint, ISearch, ITextSearch, INotifier, ICaret, INavigateEx, INavigate, IEdit, IWordWrap, IFmtExportEx, IFmtExport, IExport, IFmtImport, IImport, ICodeCompletion, IRecordPlayBack, IEditColors, ISplitView, IAutoCorrect, IControl
{
	private Container components;

	private bool hideCaret;

	private bool dragCaret;

	private Point hideCaretPoint = new Point(-100, -100);

	private Point oldDragPoint = new Point(-100, -100);

	private ISyntaxPaint syntaxPaint;

	private bool inPrinting;

	private int vertNavigateX;

	private bool vertNavigate;

	private ICodeCompletionBox codeCompletionBox;

	private ICodeCompletionHint codeCompletionHint;

	private char[] codeCompletionChars = EditConsts.DefaultCodeCompletionChars.ToCharArray();

	private ArrayList macroRecords;

	private bool macroRecording;

	private bool macroSuspended;

	private int macroUpdateCount;

	private Color readonlyForeColor = Color.Empty;

	private Color readonlyBackColor = Color.Empty;

	private Color disabledForeColor = Color.Empty;

	private Color disabledBackColor = Color.Empty;

	private Splitter vertSplitter;

	private Splitter horzSplitter;

	private ISyntaxEdit vertSplitEdit;

	private ISyntaxEdit horzSplitEdit;

	private char lastKey;

	private char[] autoCorrectDelimiters = EditConsts.DefaultAutoCorrectDelimiters.ToCharArray();

	private bool autoCorrection;

	private AutoCorrectEventArgs autoCorrectEventArgs;

	private ITextSource source;

	private TextSource internalSource;

	private IPainter painter;

	private IKeyList keyList;

	private Gutter gutter;

	private Margin margin;

	private Selection selection;

	private Printing printing;

	private WhiteSpace whiteSpace;

	private LineSeparator lineSeparator;

	private Braces braces;

	private HyperTextEx hyperText;

	private Outlining outlining;

	private Spelling spelling;

	private Scrolling scrolling;

	private LineStylesEx lineStyles;

	private DisplayStrings displayLines;

	private EditBorderStyle borderStyle = EditBorderStyle.Fixed3D;

	private Color borderColor = Color.Empty;

	private EditPages pages;

	private SyntaxSettings syntaxSettings;

	private int keyState;

	private bool keyProcessed;

	private bool acceptReturns = true;

	private bool acceptTabs = true;

	private bool syntaxErrorsHints = true;

	private Color columnsIndentForeColor = EditConsts.DefaultColumnsIndentForeColor;

	private bool drawColumnsIndent;

	private bool transparent;

	private ISearchDialog searchDialog;

	private IGotoLineDialog gotoLineDialog;

	[CompilerGenerated]
	private PaintEventHandler m_PaintBackground;

	private string searchText;

	private SearchOptions searchOptions = EditConsts.DefaultSearchOptions;

	private Regex searchExpression;

	private Point searchPos;

	private Point searchStartPos;

	private bool searchCycled;

	private int searchLen;

	private Match searchMatch;

	private bool firstSearch = true;

	private bool inIncrementalSearch;

	private bool incrSearchFlag;

	private Point incrSearchPosition;

	private Point incrStartSearchPosition;

	private Cursor incrementalSearchCursor;

	private Cursor reverseIncrementalSearchCursor;

	private Rectangle searchSelRect;

	private SelectionType searchSelType;

	private int searchUpdateCount;

	private Timer codeCompletionTimer;

	private bool needStartDrag;

	private Point startDragPos;

	private Point saveDragPos;

	private CodeCompletionArgs codeCompletionArgs;

	private NotifyEventArgs notifyEventArgs;

	private Cursor leftArrowCursor;

	private Cursor showWhiteSpaceCursor;

	private Cursor hideWhiteSpaceCursor;

	private int oldLine = -1;

	private bool urlAtCursor;

	private Point infoTipPos = new Point(-1, -1);

	private IOutlineRange mouseRange;

	private SnippetRange snippetRange;

	private ISyntaxError mouseError;

	private string mouseUrl = string.Empty;

	private Point mouseUrlPoint;

	private IBookMark mouseBookMark;

	private Point mouseBookMarkPt;

	private bool dragMargin;

	private int lbuttonClicks;

	private Timer tripleClickTimer;

	private bool useDefaultMenu;

	private ContextMenuStrip defaultMenu;

	private ToolStripMenuItem miUndo;

	private ToolStripMenuItem miRedo;

	private ToolStripMenuItem miCut;

	private ToolStripMenuItem miCopy;

	private ToolStripMenuItem miPaste;

	private ToolStripMenuItem miDelete;

	private ToolStripMenuItem miSelectAll;

	[Category("Behavior")]
	[DefaultValue(false)]
	[Description("Gets or sets a value indicating whether the control should display caret when it has input focus.")]
	public bool HideCaret
	{
		get
		{
			return hideCaret;
		}
		set
		{
			if (hideCaret != value)
			{
				hideCaret = value;
				if (hideCaret)
				{
					DestroyCaret();
				}
			}
		}
	}

	protected internal ISyntaxPaint SyntaxPaint => syntaxPaint;

	protected internal bool InPrinting => inPrinting;

	[Category("Behavior")]
	[DefaultValue(false)]
	[Description("Gets or sets a value indicating whether control uses colors to paint its content.")]
	public bool DisableColorPaint
	{
		get
		{
			return syntaxPaint.DisableColorPaint;
		}
		set
		{
			syntaxPaint.DisableColorPaint = value;
		}
	}

	[Category("Behavior")]
	[DefaultValue(false)]
	[Description("Gets or sets a value indicating whether Edit control uses lexical colors/styles to paint its content.")]
	public bool DisableSyntaxPaint
	{
		get
		{
			return syntaxPaint.DisableSyntaxPaint;
		}
		set
		{
			syntaxPaint.DisableSyntaxPaint = value;
		}
	}

	[Category("Behavior")]
	[DefaultValue(true)]
	[Description("Gets or sets a boolean value that indicates whether Edit control should display hint over each syntax error in it's content.")]
	public bool SyntaxErrorsHints
	{
		get
		{
			return syntaxErrorsHints;
		}
		set
		{
			syntaxErrorsHints = value;
		}
	}

	[Description("Indicates whether edit control should draw columns indentation marks.")]
	[DefaultValue(false)]
	public bool DrawColumnsIndent
	{
		get
		{
			return drawColumnsIndent;
		}
		set
		{
			if (drawColumnsIndent != value)
			{
				drawColumnsIndent = value;
				((Control)this).Invalidate();
			}
		}
	}

	[Description("Specifies columns indentation color.")]
	public Color ColumnsIndentForeColor
	{
		get
		{
			return columnsIndentForeColor;
		}
		set
		{
			if (columnsIndentForeColor != value)
			{
				columnsIndentForeColor = value;
				if (drawColumnsIndent)
				{
					((Control)this).Invalidate();
				}
			}
		}
	}

	[Category("SyntaxEdit")]
	[DefaultValue(null)]
	[Description("Gets or sets object that can make lexical analysis for the control's content.")]
	public ILexer Lexer
	{
		get
		{
			return Source.Lexer;
		}
		set
		{
			Source.Lexer = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Point Position
	{
		get
		{
			return Source.Position;
		}
		set
		{
			Source.Position = value;
		}
	}

	[Category("Behavior")]
	[Editor("QWhale.Design.FlagEnumerationEditor, QWhale.Editor", typeof(UITypeEditor))]
	[Description("Gets or sets navigating options.")]
	public NavigateOptions NavigateOptions
	{
		get
		{
			return Source.NavigateOptions;
		}
		set
		{
			Source.NavigateOptions = value;
		}
	}

	[Category("Appearance")]
	[DefaultValue(false)]
	[Description("Gets or sets a value indicating whether control automatically wraps words to the beginning of the next line when necessary.")]
	public bool WordWrap
	{
		get
		{
			return displayLines.WordWrap;
		}
		set
		{
			displayLines.WordWrap = value;
		}
	}

	[Category("Appearance")]
	[DefaultValue(false)]
	[Description("Gets or sets a value indicating whether control automatically wraps words at margin position.")]
	public bool WrapAtMargin
	{
		get
		{
			return displayLines.WrapAtMargin;
		}
		set
		{
			displayLines.WrapAtMargin = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int WrapMargin => displayLines.WrapMargin;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public string LineTerminator
	{
		get
		{
			return Lines.LineTerminator;
		}
		set
		{
			Lines.LineTerminator = value;
		}
	}

	[Category("Behavior")]
	[DefaultValue(false)]
	[Description("Gets or sets a value indicating whether the control's content is read-only.")]
	public bool Readonly
	{
		get
		{
			return Source.Readonly;
		}
		set
		{
			Source.Readonly = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool OverWrite
	{
		get
		{
			return Source.OverWrite;
		}
		set
		{
			Source.OverWrite = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool Modified
	{
		get
		{
			return Source.Modified;
		}
		set
		{
			Source.Modified = value;
		}
	}

	[Category("Behavior")]
	[DefaultValue(false)]
	[Description("Gets or sets a value indicating whether the control accepts only one line of the text.")]
	public bool SingleLineMode
	{
		get
		{
			return Source.SingleLineMode;
		}
		set
		{
			Source.SingleLineMode = value;
		}
	}

	[Category("Behavior")]
	[Editor("QWhale.Design.FlagEnumerationEditor, QWhale.Editor", typeof(UITypeEditor))]
	[Description("Gets or sets \"QWhale.Editor.IndentOptions\" for this class, allowing to customize behaior of Edit control when user presses Enter to insert new text line.")]
	public IndentOptions IndentOptions
	{
		get
		{
			return Source.IndentOptions;
		}
		set
		{
			Source.IndentOptions = value;
		}
	}

	[Description("Specifies the maximum number of characters that can be entered into the edit control.")]
	[Category("Behavior")]
	[DefaultValue(0)]
	public int MaxLength
	{
		get
		{
			return Source.MaxLength;
		}
		set
		{
			Source.MaxLength = value;
		}
	}

	protected internal Timer CodeCompletionTimer
	{
		get
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			if (codeCompletionTimer == null)
			{
				codeCompletionTimer = new Timer();
				codeCompletionTimer.Enabled = false;
				codeCompletionTimer.Interval = SyntaxConsts.DefaultHintDelay;
				codeCompletionTimer.Tick += OnCodeCompletion;
				components.Add((IComponent?)codeCompletionTimer);
			}
			return codeCompletionTimer;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public ICodeCompletionBox CodeCompletionBox
	{
		get
		{
			if (codeCompletionBox == null)
			{
				codeCompletionBox = CreateCodeCompletionBox();
				codeCompletionBox.ClosePopup += CloseCodeCompletionBox;
				codeCompletionBox.Disposed += DisposeCodeCompletionBox;
			}
			return codeCompletionBox;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public ICodeCompletionHint CodeCompletionHint
	{
		get
		{
			if (codeCompletionHint == null)
			{
				codeCompletionHint = CreateCodeCompletionHint();
				codeCompletionHint.ClosePopup += CloseCodeCompletionHint;
				codeCompletionHint.Disposed += DisposeCodeCompletionBox;
			}
			return codeCompletionHint;
		}
	}

	[Category("SyntaxEdit")]
	[Description("Represents a collection of characters that initializes a code completion procedure when typing in the editor.")]
	public char[] CodeCompletionChars
	{
		get
		{
			return codeCompletionChars;
		}
		set
		{
			codeCompletionChars = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool MacroRecording
	{
		get
		{
			return macroRecording;
		}
		set
		{
			if (macroRecording != value)
			{
				macroRecording = value;
				if (value && !macroSuspended)
				{
					macroRecords.Clear();
				}
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool MacroSuspendend
	{
		get
		{
			return macroSuspended;
		}
		set
		{
			macroSuspended = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public MacroKeyListData[] MacroRecords
	{
		get
		{
			MacroKeyListData[] array = new MacroKeyListData[macroRecords.Count];
			for (int i = 0; i < macroRecords.Count; i++)
			{
				array[i] = macroRecords[i] as MacroKeyListData;
			}
			return array;
		}
		set
		{
			macroRecords.Clear();
			if (value != null)
			{
				foreach (MacroKeyListData macroKeyListData in value)
				{
					macroKeyListData.Fixup(this);
					macroRecords.Add(macroKeyListData);
				}
			}
		}
	}

	[Category("Appearance")]
	[Description("Gets or sets foreground color used in the readonly state.")]
	public Color ReadonlyForeColor
	{
		get
		{
			return readonlyForeColor;
		}
		set
		{
			if (readonlyForeColor != value)
			{
				readonlyForeColor = value;
				if (Readonly)
				{
					((Control)this).Invalidate();
				}
			}
		}
	}

	[Category("Appearance")]
	[Description("Gets or sets background color used in the readonly state.")]
	public Color ReadonlyBackColor
	{
		get
		{
			return readonlyBackColor;
		}
		set
		{
			if (readonlyBackColor != value)
			{
				readonlyBackColor = value;
				if (Readonly)
				{
					((Control)this).Invalidate();
				}
			}
		}
	}

	[Category("Appearance")]
	[Description("Gets or sets foreground color used in the disabled state.")]
	public Color DisabledForeColor
	{
		get
		{
			return disabledForeColor;
		}
		set
		{
			if (disabledForeColor != value)
			{
				disabledForeColor = value;
				if (!((Control)this).Enabled)
				{
					((Control)this).Invalidate();
				}
			}
		}
	}

	[Category("Appearance")]
	[Description("Gets or sets background color used in the disabled state.")]
	public Color DisabledBackColor
	{
		get
		{
			return disabledBackColor;
		}
		set
		{
			if (disabledBackColor != value)
			{
				disabledBackColor = value;
				if (!((Control)this).Enabled)
				{
					((Control)this).Invalidate();
				}
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public ISyntaxEdit VertSplitEdit => vertSplitEdit;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public ISyntaxEdit HorzSplitEdit => horzSplitEdit;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Splitter VertSplitter => vertSplitter;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Splitter HorzSplitter => horzSplitter;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public char[] AutoCorrectDelimiters
	{
		get
		{
			return autoCorrectDelimiters;
		}
		set
		{
			autoCorrectDelimiters = value;
		}
	}

	[Category("SyntaxEdit")]
	[DefaultValue(false)]
	[Description("Gets or sets a boolean value indicating whether to auto correct words being typed.")]
	public bool AutoCorrection
	{
		get
		{
			return autoCorrection;
		}
		set
		{
			autoCorrection = value;
		}
	}

	protected internal bool IsTransparent
	{
		get
		{
			if (!transparent)
			{
				return pages.Transparent;
			}
			return true;
		}
	}

	[Category("SyntaxEdit")]
	[Description("Gets or sets an object that implements \"ITextSource\" interface containing an actual string data displayed by the control.")]
	public ITextSource Source
	{
		get
		{
			if (source == null)
			{
				return internalSource;
			}
			return source;
		}
		set
		{
			if (source == value)
			{
				return;
			}
			if (source != null)
			{
				source.RemoveNotifier(this);
				source.ActiveEdit = null;
				source.Edits.Remove(this);
			}
			source = value;
			if (source != null)
			{
				source.AddNotifier(this);
				if (source.ActiveEdit == null)
				{
					source.ActiveEdit = this;
				}
				source.Edits.Add(this);
			}
			SourceChanged();
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public ISyntaxStrings Lines
	{
		get
		{
			return Source.Lines;
		}
		set
		{
			Source.Lines = value;
		}
	}

	[Category("SyntaxEdit")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[TypeConverter(typeof(CollectionConverter))]
	[Description("Represents \"Lines\" property in the form of array of strings.")]
	public string[] Strings
	{
		get
		{
			ISyntaxStrings lines = Source.Lines;
			string[] array = new string[lines.Count];
			for (int i = 0; i < lines.Count; i++)
			{
				array[i] = lines[i];
			}
			return array;
		}
		set
		{
			ISyntaxStrings lines = Source.Lines;
			Source.BeginUpdate(UpdateReason.Other);
			lines.BeginUpdate();
			try
			{
				lines.Clear();
				foreach (string value2 in value)
				{
					lines.Add(value2);
				}
				Source.State |= NotifyState.CountChanged;
				lines.Changed(0, int.MaxValue);
			}
			finally
			{
				lines.EndUpdate();
				Source.EndUpdate();
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public IDisplayStrings DisplayLines => displayLines;

	[Category("SyntaxEdit")]
	[TypeConverter(typeof(ExpandableObjectConverter))]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Represents an object that implements \"ISelection\" interface. This object represents various properties and methods to manipulate text selection, such as copy, paste and drag selected text.")]
	public ISelection Selection
	{
		get
		{
			return selection;
		}
		set
		{
			selection.Assign(value);
		}
	}

	[Category("SyntaxEdit")]
	[TypeConverter(typeof(ExpandableObjectConverter))]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Represents object that implements \"IGutter\" interface containing methods and properties necessary to operate with gutter at the left size of the control.")]
	public IGutter Gutter
	{
		get
		{
			return gutter;
		}
		set
		{
			gutter.Assign(value);
		}
	}

	[Category("SyntaxEdit")]
	[TypeConverter(typeof(ExpandableObjectConverter))]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Represents an object that implements \"IMargin\" interface and specifies appearance of vertical line drawn over the text and used to mark some limit, for example, of the maximum string length allowed.")]
	public IMargin EditMargin
	{
		get
		{
			return margin;
		}
		set
		{
			margin.Assign(value);
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public IPainter Painter => painter;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public IKeyList KeyList => keyList;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public EventHandlers EventHandlers => keyList.Handlers;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public ILineStylesEx LineStyles
	{
		get
		{
			return lineStyles;
		}
		set
		{
			lineStyles.Assign(value);
		}
	}

	[Category("SyntaxEdit")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Editor(typeof(CollectionEditor), typeof(UITypeEditor))]
	[Description("Represents an object that implements \"ILineStylesEx\" interface holding collection of \"ILineStyle\" objects each of them determines particular style of the line in the control.")]
	public LineStylesEx LinesStyles => lineStyles;

	[Category("SyntaxEdit")]
	[TypeConverter(typeof(ExpandableObjectConverter))]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Represents an object that implements \"IPrinting\" interface allowing to perform various printing actions such as print, preview document, and setup print options.")]
	public IPrinting Printing
	{
		get
		{
			return printing;
		}
		set
		{
			printing.Assign(value);
		}
	}

	[Category("SyntaxEdit")]
	[TypeConverter(typeof(ExpandableObjectConverter))]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Represents an object that implements \"IEditPage\" interface containing properties and methods representing collection of particular pages.")]
	public IEditPages Pages
	{
		get
		{
			return pages;
		}
		set
		{
			if (pages == value)
			{
				return;
			}
			pages.BeginUpdate();
			try
			{
				pages.Clear();
				foreach (IEditPage item in value.Items)
				{
					pages.Add().Assign(item);
				}
			}
			finally
			{
				pages.EndUpdate();
			}
		}
	}

	[Category("SyntaxEdit")]
	[TypeConverter(typeof(ExpandableObjectConverter))]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Represents an object that implements \"IWhiteSpace\" interface. This object specifies appearance of white space characters, as well as End-of-line and End-of-file marks.")]
	public IWhiteSpace WhiteSpace
	{
		get
		{
			return whiteSpace;
		}
		set
		{
			whiteSpace.Assign(value);
		}
	}

	[Category("SyntaxEdit")]
	[TypeConverter(typeof(ExpandableObjectConverter))]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Represents an object that implements \"ILineSeparator\" interface containing methods and properties necessary to separate lines and highlight current line within the control.")]
	public ILineSeparator LineSeparator
	{
		get
		{
			return lineSeparator;
		}
		set
		{
			lineSeparator.Assign(value);
		}
	}

	[Category("SyntaxEdit")]
	[TypeConverter(typeof(ExpandableObjectConverter))]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Represents an object that implements \"IBraceMatchingEx\" interface allowing to change appearance of matching braces within the control.")]
	public IBraceMatchingEx Braces
	{
		get
		{
			return braces;
		}
		set
		{
			braces.Assign(value);
		}
	}

	[Category("SyntaxEdit")]
	[TypeConverter(typeof(ExpandableObjectConverter))]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Represents object that implements \"IHyperTextEx\" interface allowing to customize appearance and behaviour of hypertext sections within the control.")]
	public IHyperTextEx HyperText
	{
		get
		{
			return hyperText;
		}
		set
		{
			hyperText.Assign(value);
		}
	}

	[Category("SyntaxEdit")]
	[TypeConverter(typeof(ExpandableObjectConverter))]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Represents an object that implements \"IOutlining\" interface that specifies appearance and behaviour of outline sections within the control.")]
	public IOutlining Outlining
	{
		get
		{
			return outlining;
		}
		set
		{
			outlining.Assign(value);
		}
	}

	[Category("SyntaxEdit")]
	[TypeConverter(typeof(ExpandableObjectConverter))]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Represents object that implements \"ISpellingEx\" interface containing properties and methods to check control's content spelling and highlight mispelled words.")]
	public ISpellingEx Spelling
	{
		get
		{
			return spelling;
		}
		set
		{
			spelling.Assign(value);
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public IScrolling Scrolling
	{
		get
		{
			return scrolling;
		}
		set
		{
			scrolling.Assign(value);
		}
	}

	[Category("SyntaxEdit")]
	[TypeConverter(typeof(ExpandableObjectConverter))]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Represents an object that implements \"IScrolling\" interface containing properties and methods that describe scrolling behaviour of the control.")]
	public Scrolling Scroll
	{
		get
		{
			return scrolling;
		}
		set
		{
			scrolling.Assign(value);
		}
	}

	[Category("Behavior")]
	[DefaultValue(true)]
	[Description("Gets or set a boolean value that indicates whether TAB key should be accepted by Edit control as input key.")]
	public bool AcceptTabs
	{
		get
		{
			return acceptTabs;
		}
		set
		{
			acceptTabs = value;
		}
	}

	[Category("Behavior")]
	[DefaultValue(true)]
	[Description("Gets or set a boolean value that indicates whether Enter key should be accepted by Edit control as input key.")]
	public bool AcceptReturns
	{
		get
		{
			return acceptReturns;
		}
		set
		{
			acceptReturns = value;
		}
	}

	[Category("Appearance")]
	[DefaultValue(EditBorderStyle.Fixed3D)]
	[Description("Gets or sets the border style for the Edit control.")]
	public EditBorderStyle BorderStyle
	{
		get
		{
			return borderStyle;
		}
		set
		{
			if (borderStyle != value)
			{
				borderStyle = value;
				((Control)this).RecreateHandle();
			}
		}
	}

	[Category("Appearance")]
	[Description("Gets or sets the border color for the Edit control.")]
	public Color BorderColor
	{
		get
		{
			return borderColor;
		}
		set
		{
			if (borderColor != value)
			{
				borderColor = value;
				((Control)this).RecreateHandle();
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Rectangle ClientRect => GetClientRect(excludeNonClient: false);

	[Category("Appearance")]
	[DefaultValue(false)]
	[Description("Gets or sets a boolean value that indicates whether Edit control should draw its background.")]
	public bool Transparent
	{
		get
		{
			return transparent;
		}
		set
		{
			if (transparent != value)
			{
				transparent = value;
				TransparentChanged();
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public ISyntaxSettings SyntaxSettings => syntaxSettings;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public ISearchDialog SearchDialog
	{
		get
		{
			if (searchDialog == null)
			{
				searchDialog = new SearchDialog();
				searchDialog.SearchSettings.SearchOptions = searchOptions;
			}
			return searchDialog;
		}
		set
		{
			searchDialog = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public IGotoLineDialog GotoLineDialog
	{
		get
		{
			if (gotoLineDialog == null)
			{
				gotoLineDialog = new DlgGoto();
			}
			return gotoLineDialog;
		}
		set
		{
			gotoLineDialog = value;
		}
	}

	protected internal Cursor IncrementalSearchCursor
	{
		get
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Expected O, but got Unknown
			if (incrementalSearchCursor == (Cursor)null)
			{
				incrementalSearchCursor = new Cursor(typeof(SyntaxEdit), "Images.IncrementalSearch.cur");
			}
			return incrementalSearchCursor;
		}
	}

	protected internal Cursor ReverseIncrementalSearchCursor
	{
		get
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Expected O, but got Unknown
			if (reverseIncrementalSearchCursor == (Cursor)null)
			{
				reverseIncrementalSearchCursor = new Cursor(typeof(SyntaxEdit), "Images.ReverseIncrementalSearch.cur");
			}
			return reverseIncrementalSearchCursor;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Point SearchPos
	{
		get
		{
			return searchPos;
		}
		set
		{
			searchPos = value;
			FirstSearch = true;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public SearchOptions SearchOptions
	{
		get
		{
			return searchOptions;
		}
		set
		{
			searchOptions = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int SearchLen => searchLen;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool FirstSearch
	{
		get
		{
			return firstSearch;
		}
		set
		{
			if (firstSearch != value)
			{
				Source.BeginUpdate(UpdateReason.Other);
				try
				{
					firstSearch = value;
					Source.State |= NotifyState.FirstSearchChanged;
				}
				finally
				{
					Source.EndUpdate();
				}
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool InIncrementalSearch => inIncrementalSearch;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public string IncrementalSearchString
	{
		get
		{
			if (!inIncrementalSearch)
			{
				return string.Empty;
			}
			return searchText;
		}
	}

	protected override CreateParams CreateParams
	{
		get
		{
			CreateParams createParams = ((Control)this).CreateParams;
			switch (borderStyle)
			{
			case EditBorderStyle.FixedSingle:
				createParams.Style |= 0x800000;
				break;
			case EditBorderStyle.Fixed3D:
			case EditBorderStyle.System:
				createParams.ExStyle |= 0x200;
				break;
			}
			return createParams;
		}
	}

	protected internal Cursor LeftArrowCursor
	{
		get
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Expected O, but got Unknown
			if (leftArrowCursor == (Cursor)null)
			{
				leftArrowCursor = new Cursor(typeof(SyntaxEdit), "Images.LeftArrow.cur");
			}
			return leftArrowCursor;
		}
	}

	protected internal Cursor ShowWhiteSpaceCursor
	{
		get
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Expected O, but got Unknown
			if (showWhiteSpaceCursor == (Cursor)null)
			{
				showWhiteSpaceCursor = new Cursor(typeof(SyntaxEdit), "Images.ShowWhiteSpace.cur");
			}
			return showWhiteSpaceCursor;
		}
	}

	protected internal Cursor HideWhiteSpaceCursor
	{
		get
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Expected O, but got Unknown
			if (hideWhiteSpaceCursor == (Cursor)null)
			{
				hideWhiteSpaceCursor = new Cursor(typeof(SyntaxEdit), "Images.HideWhiteSpace.cur");
			}
			return hideWhiteSpaceCursor;
		}
	}

	[Description("Gets or sets the string collection as a single string with the individual lines delimited by carriage returns.")]
	public override string Text
	{
		get
		{
			return Lines.Text;
		}
		set
		{
			Lines.Text = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override bool AllowDrop
	{
		get
		{
			if (!((Control)this).AllowDrop)
			{
				return (selection.Options & SelectionOptions.DisableDragging) == 0;
			}
			return true;
		}
		set
		{
			((Control)this).AllowDrop = value;
		}
	}

	[Description("Indicates whether edit control should use bultin popup menu.")]
	[Category("Behavior")]
	[DefaultValue(true)]
	public bool UseDefaultMenu
	{
		get
		{
			return useDefaultMenu;
		}
		set
		{
			useDefaultMenu = value;
		}
	}

	public ContextMenuStrip DefaultMenu
	{
		get
		{
			if (defaultMenu == null)
			{
				InitDefaultMenu();
			}
			return defaultMenu;
		}
		set
		{
			defaultMenu = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual object XmlInfo
	{
		get
		{
			return new XmlSyntaxEditInfo(this);
		}
		set
		{
			((XmlSyntaxEditInfo)value).FixupReferences(this);
		}
	}

	[Category("SyntaxEdit")]
	[Description("Occurs when control draws its content.")]
	public event CustomDrawEvent CustomDraw
	{
		add
		{
			syntaxPaint.CustomDraw += value;
		}
		remove
		{
			syntaxPaint.CustomDraw -= value;
		}
	}

	[Category("SyntaxEdit")]
	[Description("Occurs when code completion window is to be displayed.")]
	public event CodeCompletionEvent NeedCodeCompletion;

	[Category("SyntaxEdit")]
	[Description("Occurs when user splits Edit control horizontally.")]
	public event EventHandler SplitHorz;

	[Category("SyntaxEdit")]
	[Description("Occurs when user splits Edit control vertically.")]
	public event EventHandler SplitVert;

	[Category("SyntaxEdit")]
	[Description("Occurs when horizontal split view is removed.")]
	public event EventHandler UnsplitHorz;

	[Category("SyntaxEdit")]
	[Description("Occurs when vertical split view is removed.")]
	public event EventHandler UnsplitVert;

	[Category("SyntaxEdit")]
	[Description("Occurs when control tries to auto correct word being typed.")]
	public event AutoCorrectEvent AutoCorrect;

	[Category("SyntaxEdit")]
	[Description("Occurs when selection bounds are changed.")]
	public event EventHandler SelectionChanged
	{
		add
		{
			selection.SelectionChanged += value;
		}
		remove
		{
			selection.SelectionChanged -= value;
		}
	}

	[Category("SyntaxEdit")]
	[Description("Occurs when header or footer part of each page is drawing.")]
	public event DrawHeaderEvent DrawHeader
	{
		add
		{
			pages.DrawHeader += value;
		}
		remove
		{
			pages.DrawHeader -= value;
		}
	}

	[Category("SyntaxEdit")]
	[Description("Occurs when user attempts to jump to url.")]
	public event UrlJumpEvent JumpToUrl
	{
		add
		{
			hyperText.JumpToUrl += value;
		}
		remove
		{
			hyperText.JumpToUrl -= value;
		}
	}

	[Category("SyntaxEdit")]
	[Description("Occurs when a control needs checking whether some string represents hypertext.")]
	public event HyperTextEvent CheckHyperText
	{
		add
		{
			hyperText.CheckHyperText += value;
		}
		remove
		{
			hyperText.CheckHyperText -= value;
		}
	}

	[Category("SyntaxEdit")]
	[Description("Occurs when the gutter part of Edit control is clicked.")]
	public event EventHandler GutterClick
	{
		add
		{
			gutter.Click += value;
		}
		remove
		{
			gutter.Click -= value;
		}
	}

	[Category("SyntaxEdit")]
	[Description("Occurs when the gutter part of Edit control is double-clicked.")]
	public event EventHandler GutterDblClick
	{
		add
		{
			gutter.DoubleClick += value;
		}
		remove
		{
			gutter.DoubleClick -= value;
		}
	}

	[Category("SyntaxEdit")]
	[Description("Occurs when user margin part of each line is drawing.")]
	public event DrawUserMarginEvent DrawUserMargin
	{
		add
		{
			gutter.DrawUserMargin += value;
		}
		remove
		{
			gutter.DrawUserMargin -= value;
		}
	}

	[Category("SyntaxEdit")]
	[Description("Occurs when spelling of some word within the text needs checking.")]
	public event WordSpellEvent WordSpell
	{
		add
		{
			spelling.WordSpell += value;
		}
		remove
		{
			spelling.WordSpell -= value;
		}
	}

	[Category("SyntaxEdit")]
	[Description("Occurs when control scrolls its content in vertical direction. This can be caused by dragging vertical scroll thumb, or caret moving.")]
	public event EventHandler VerticalScroll
	{
		add
		{
			scrolling.VerticalScroll += value;
		}
		remove
		{
			scrolling.VerticalScroll -= value;
		}
	}

	[Category("SyntaxEdit")]
	[Description("Occurs when control scrolls its content in horizonal direction. This can be caused by dragging horizonal scroll thumb, or caret moving.")]
	public event EventHandler HorizontalScroll
	{
		add
		{
			scrolling.HorizontalScroll += value;
		}
		remove
		{
			scrolling.HorizontalScroll -= value;
		}
	}

	[Category("SyntaxEdit")]
	[Description("Occurs when some scrolling button is clicked.")]
	public event EventHandler ScrollButtonClick
	{
		add
		{
			scrolling.ScrollButtonClick += value;
		}
		remove
		{
			scrolling.ScrollButtonClick -= value;
		}
	}

	[Category("SyntaxEdit")]
	[Description("Occurs when text Source's state is changed, for example when caret position moved, text edited, amount of lines changed, lexer changed, etc.")]
	public event NotifyEvent SourceStateChanged;

	[Category("SyntaxEdit")]
	[Description("Occurs when modified state is changed.")]
	public event EventHandler ModifiedChanged;

	[Category("SyntaxEdit")]
	[Description("Occurs when Edit control should paint its background in transparent mode.")]
	public event PaintEventHandler PaintBackground
	{
		[CompilerGenerated]
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			PaintEventHandler val = this.m_PaintBackground;
			PaintEventHandler val2;
			do
			{
				val2 = val;
				PaintEventHandler value2 = (PaintEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref this.m_PaintBackground, value2, val2);
			}
			while (val != val2);
		}
		[CompilerGenerated]
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			PaintEventHandler val = this.m_PaintBackground;
			PaintEventHandler val2;
			do
			{
				val2 = val;
				PaintEventHandler value2 = (PaintEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref this.m_PaintBackground, value2, val2);
			}
			while (val != val2);
		}
	}

	public SyntaxEdit(IContainer container)
		: this()
	{
		container.Add((IComponent?)this);
	}

	public SyntaxEdit()
	{
		InitializeComponent();
	}

	~SyntaxEdit()
	{
		try
		{
			Source = null;
		}
		finally
		{
			((Component)this).Finalize();
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((Control)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Expected O, but got Unknown
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Expected O, but got Unknown
		this.components = new System.ComponentModel.Container();
		this.internalSource = new Stimulsoft.Editor.TextSource();
		this.Source = this.internalSource;
		this.source = null;
		this.painter = new Stimulsoft.Editor.GdiPainter();
		this.gutter = new Stimulsoft.Editor.Gutter(this);
		this.displayLines = new Stimulsoft.Editor.DisplayStrings(this, this.Lines);
		this.margin = new Stimulsoft.Editor.Margin(this);
		this.selection = new Stimulsoft.Editor.Selection(this);
		this.scrolling = new Stimulsoft.Editor.Scrolling(this);
		this.pages = new Stimulsoft.Editor.EditPages(this);
		this.pages.Add();
		this.printing = new Stimulsoft.Editor.Printing(this);
		this.pages.InitDefaultPrinterSettings(this.printing.PrinterSettings);
		this.whiteSpace = new Stimulsoft.Editor.WhiteSpace(this);
		this.lineSeparator = new Stimulsoft.Editor.LineSeparator(this);
		this.lineStyles = new Stimulsoft.Editor.LineStylesEx(this);
		this.braces = new Stimulsoft.Editor.Braces(this);
		this.hyperText = new Stimulsoft.Editor.HyperTextEx(this);
		this.outlining = new Stimulsoft.Editor.Outlining(this);
		this.spelling = new Stimulsoft.Editor.Spelling(this);
		this.keyList = new Stimulsoft.Editor.KeyList(this);
		this.macroRecords = new System.Collections.ArrayList();
		this.syntaxPaint = new Stimulsoft.Editor.EditSyntaxPaint(this.painter, this);
		this.syntaxSettings = new Stimulsoft.Editor.SyntaxSettings();
		this.codeCompletionArgs = new Stimulsoft.Editor.CodeCompletionArgs();
		this.notifyEventArgs = new Stimulsoft.Editor.NotifyEventArgs();
		this.autoCorrectEventArgs = new Stimulsoft.Editor.AutoCorrectEventArgs();
		((Control)this).QueryContinueDrag += new QueryContinueDragEventHandler(QueryEndDrag);
		((Control)this).Cursor = Cursors.IBeam;
		((Control)this).Font = new Font(FontFamily.GenericMonospace, 10f);
		this.painter.Font = ((Control)this).Font;
		((Control)this).SetStyle((ControlStyles)5894, true);
		((Control)this).BackColor = Stimulsoft.Editor.Consts.DefaultControlBackColor;
		((Control)this).Width = 100;
		((Control)this).Height = 96;
	}

	public void InitializeForOptionHelper(bool isVBNet)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		try
		{
			((Control)this).Font = new Font("Courier New", 10f);
			Pages.DefaultPage.Footer.Font = new Font("Courier New", 10f);
			Pages.DefaultPage.Header.Font = new Font("Courier New", 10f);
		}
		catch
		{
		}
		((Control)this).BackColor = SystemColors.Window;
		BorderStyle = EditBorderStyle.Fixed3D;
		Gutter.Visible = false;
		EditMargin.Visible = false;
		if (isVBNet)
		{
			Lexer = new VbParser();
		}
		else
		{
			Lexer = new CsParser();
		}
	}

	protected internal bool IsFocused()
	{
		if (!((Control)this).Focused)
		{
			return IsCodeCompletionWindowFocused();
		}
		return true;
	}

	protected internal void UpdateCaret()
	{
		if (IsFocused() || dragCaret)
		{
			Point point = TextToScreen(Position);
			if (point.X < gutter.GetWidth() || point.Y < 0)
			{
				DisplayCaretNowhere();
			}
			else
			{
				ShowCaret(point.X, point.Y);
			}
			if (pages.Rulers != 0 && !dragCaret)
			{
				pages.UpdateRulers();
			}
		}
		UpdateSeparator();
	}

	protected internal void UpdateCaretMode()
	{
		if (IsFocused())
		{
			DestroyCaret();
			CreateCaret();
			UpdateCaret();
		}
	}

	protected void DisplayCaretNowhere()
	{
		OSUtils.SetCaretPos(hideCaretPoint.X, hideCaretPoint.Y);
	}

	protected void PaintDragCaret(Point pt, bool erase)
	{
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Expected O, but got Unknown
		if (pt.Equals((object?)hideCaretPoint))
		{
			return;
		}
		Rectangle rectangle = new Rectangle(pt.X - 1, pt.Y, 2, painter.FontHeight - 1);
		if (erase)
		{
			rectangle.Width++;
			rectangle.Height++;
			((Control)this).Invalidate(rectangle);
			return;
		}
		Graphics val = ((Control)this).CreateGraphics();
		try
		{
			Pen val2 = new Pen(Selection.IsPosInSelection(Position) ? EditConsts.DefaultSelectionDragColor : Consts.DefaultControlForeColor, 1f);
			val.DrawRectangle(val2, rectangle);
			val2.Dispose();
		}
		finally
		{
			val.Dispose();
		}
	}

	public void CreateCaret()
	{
		if (!hideCaret && ((Control)this).IsHandleCreated)
		{
			Size caretSize = GetCaretSize(Position);
			OSUtils.CreateCaret(((Control)this).Handle, caretSize.Width, caretSize.Height);
			OSUtils.ShowCaret(((Control)this).Handle);
		}
	}

	public void DestroyCaret()
	{
		OSUtils.DestroyCaret();
	}

	public void ShowCaret(int x, int y)
	{
		if (dragCaret)
		{
			PaintDragCaret(oldDragPoint, erase: true);
			oldDragPoint = new Point(x, y);
			PaintDragCaret(oldDragPoint, erase: false);
		}
		else
		{
			OSUtils.SetCaretPos(x, y);
		}
	}

	public Size GetCaretSize(Point position)
	{
		if (Source.OverWrite)
		{
			return new Size(painter.FontWidth, painter.FontHeight);
		}
		return new Size(EditConsts.DefaultCaretWidth, painter.FontHeight);
	}

	public void DisplayDragCaret()
	{
		if (!dragCaret)
		{
			if (!hideCaret && ((Control)this).IsHandleCreated && IsFocused())
			{
				DisplayCaretNowhere();
			}
			dragCaret = true;
			UpdateCaret();
		}
	}

	public void HideDragCaret()
	{
		if (dragCaret)
		{
			PaintDragCaret(oldDragPoint, erase: true);
			oldDragPoint = hideCaretPoint;
			dragCaret = false;
			UpdateCaret();
		}
	}

	public virtual void ResetHideCaret()
	{
		HideCaret = false;
	}

	protected internal Color GetBackColor(bool readOnly)
	{
		if (!((Control)this).Enabled)
		{
			Color color = disabledBackColor;
			if (color != Color.Empty)
			{
				return color;
			}
		}
		if (Readonly || readOnly)
		{
			Color color2 = ReadonlyBackColor;
			if (color2 != Color.Empty)
			{
				return color2;
			}
		}
		return ((Control)this).BackColor;
	}

	public void PaintSyntax(IPainter painter, int startLine, int endLine, Point position, Rectangle rect, bool specialPaint)
	{
		syntaxPaint.PaintSyntax(painter, startLine, endLine, position, rect, specialPaint);
	}

	public void DrawLine(int index, Point position, Rectangle clipRect)
	{
		syntaxPaint.DrawLine(index, position, clipRect);
	}

	public void DrawLine(int index, string line, short[] colorData, Point position, Rectangle clipRect)
	{
		syntaxPaint.DrawLine(index, line, colorData, position, clipRect);
	}

	public int MeasureLine(int index, int pos, int len)
	{
		return syntaxPaint.MeasureLine(index, pos, len);
	}

	public int MeasureLine(int index, int pos, int len, int width, out int chars, bool exact)
	{
		return syntaxPaint.MeasureLine(index, pos, len, width, out chars, exact);
	}

	public int MeasureLine(string line, short[] colorData, int pos, int len)
	{
		return syntaxPaint.MeasureLine(line, colorData, pos, len);
	}

	public int MeasureLine(string line, short[] colorData, int pos, int len, int width, out int chars, bool exact)
	{
		return syntaxPaint.MeasureLine(line, colorData, pos, len, width, out chars, exact);
	}

	public bool OnCustomDraw(IPainter painter, Rectangle rect, DrawStage stage, DrawState state, DrawInfo info)
	{
		return syntaxPaint.OnCustomDraw(painter, rect, stage, state, info);
	}

	public void ResetDisableColorPaint()
	{
		syntaxPaint.ResetDisableColorPaint();
	}

	public void ResetDisableSyntaxPaint()
	{
		syntaxPaint.ResetDisableSyntaxPaint();
	}

	public void ResetSyntaxErrorsHints()
	{
		SyntaxErrorsHints = true;
	}

	public void ResetDrawColumnsIndent()
	{
		DrawColumnsIndent = false;
	}

	public void ResetColumnsIndentForeColor()
	{
		ColumnsIndentForeColor = EditConsts.DefaultColumnsIndentForeColor;
	}

	public void Notification(object sender, EventArgs e)
	{
		if (sender is ITextSource)
		{
			TextSource textSource = (TextSource)sender;
			NotifyState state = textSource.State;
			if (e is PositionChangedEventArgs)
			{
				PositionChangedEventArgs positionChangedEventArgs = (PositionChangedEventArgs)e;
				PositionChanged(positionChangedEventArgs.Reason, positionChangedEventArgs.DeltaX, positionChangedEventArgs.DeltaY);
				return;
			}
			if (e is BlockDeletingEventArgs)
			{
				displayLines.BlockDeleting(((BlockDeletingEventArgs)e).Rect);
				return;
			}
			bool flag = (state & NotifyState.BookMarkChanged) != 0;
			bool flag2 = (state & NotifyState.Edit) != 0;
			bool flag3 = (state & NotifyState.ModifiedChanged) != 0;
			if ((state & NotifyState.OverWriteChanged) != 0)
			{
				UpdateCaretMode();
			}
			if ((state & NotifyState.Undo) != 0)
			{
				ClosePopupWindow();
			}
			if (flag2 || flag || (state & NotifyState.BlockChanged) != 0)
			{
				if (flag2)
				{
					RescanLines(textSource.FirstChanged, textSource.LastChanged);
					((Control)this).OnTextChanged(EventArgs.Empty);
				}
				InvalidateWindow(textSource.FirstChanged, textSource.LastChanged, flag || outlining.AllowOutlining || (gutter.Options & GutterOptions.PaintLineNumbers) != 0 || (gutter.Options & GutterOptions.PaintLineModificators) != 0 || (gutter.Options & GutterOptions.PaintUserMargin) != 0);
			}
			if (flag3 && (gutter.Options & GutterOptions.PaintLineModificators) != 0)
			{
				((Control)this).Invalidate(Gutter.Rect);
			}
			if ((state & NotifyState.PositionChanged) != 0 || (state & NotifyState.CenterLine) != 0)
			{
				CheckIncrementalSeacrh();
				if (textSource.ActiveEdit == this)
				{
					textSource.CurrentSnippet = textSource.GetSnippet(Position);
					lineSeparator.TempUnhighlightLine();
					if ((selection.Options & SelectionOptions.SmartFormat) != 0 && textSource.NeedReparseTextOnLineChange() && (state & NotifyState.TextParsed) != 0 && (state & NotifyState.Undo) == 0 && textSource.OldPosition.Y != textSource.Position.Y)
					{
						selection.SmartFormat(textSource.OldPosition.Y);
					}
					if (displayLines.LineEndUpdateCount == 0)
					{
						displayLines.AtLineEnd = false;
					}
					MakeVisible(Position, (state & NotifyState.CenterLine) != 0);
					UpdateCaret();
					ClearSelection();
					UpdateStartSearchPos();
					vertNavigate = false;
				}
			}
			if ((state & NotifyState.SelectBlock) != 0 && textSource.ActiveEdit == this && selection.UpdateCount == 0 && (selection.Options & SelectionOptions.PersistentBlocks) == 0)
			{
				selection.SetSelection(SelectionType.Stream, textSource.SelectBlockRect);
			}
			if ((state & NotifyState.CountChanged) != 0)
			{
				if (gutter.LineNumbersChanged())
				{
					((Control)this).Invalidate();
				}
				scrolling.UpdateScroll(updateSize: false);
			}
			if ((state & NotifyState.SyntaxChanged) != 0)
			{
				SyntaxChanged();
			}
			if ((state & NotifyState.Outline) != 0)
			{
				DoOutlineText(update: false);
			}
			OnSourceStateChanged(state, textSource.FirstChanged, textSource.LastChanged);
			if ((state & NotifyState.PositionChanged) != 0)
			{
				selection.DoSelectionChanged();
			}
			if (flag3)
			{
				OnModifiedChanged();
			}
		}
		else if (sender == displayLines)
		{
			NotifyEventArgs notifyEventArgs = (NotifyEventArgs)e;
			if (notifyEventArgs.Update || !WordWrap)
			{
				UpdatePages(notifyEventArgs.FirstChanged);
			}
			if (notifyEventArgs.FirstChanged == 0 && notifyEventArgs.LastChanged == int.MaxValue)
			{
				((Control)this).Invalidate();
			}
			else
			{
				InvalidateWindow(notifyEventArgs.FirstChanged, notifyEventArgs.LastChanged, (notifyEventArgs.State & NotifyState.Outline) != 0);
			}
			if (notifyEventArgs.Update)
			{
				scrolling.UpdateScroll(updateSize: false);
				UpdateCaret();
			}
			OnSourceStateChanged(notifyEventArgs.State, notifyEventArgs.FirstChanged, notifyEventArgs.LastChanged);
		}
	}

	private void BeginLineEndUpdate()
	{
		displayLines.LineEndUpdateCount++;
	}

	private void EndLineEndUpdate()
	{
		displayLines.LineEndUpdateCount--;
	}

	private void MovePage(bool direction)
	{
		BeginLineEndUpdate();
		try
		{
			Point position = ((pages.PageType == PageType.PageLayout) ? TextToScreen(Position) : displayLines.PointToDisplayPoint(Position));
			if (vertNavigate)
			{
				position.X = vertNavigateX;
			}
			int x = position.X;
			if (pages.PageType != PageType.PageLayout)
			{
				position = DisplayToScreen(position.X, position.Y);
			}
			int num = ((pages.PageType == PageType.PageLayout) ? ClientRect.Height : Math.Max(LinesInHeight() - 1, 0));
			num = ((!direction) ? (scrolling.WindowOriginY - num) : (num + scrolling.WindowOriginY));
			if (num < 0)
			{
				scrolling.WindowOriginY = 0;
				MoveToLine(0);
			}
			else
			{
				int windowOriginY = scrolling.WindowOriginY;
				scrolling.SafeSetWindowLine(num);
				try
				{
					bool lineEnd = false;
					position = ScreenToText(position.X, position.Y, ref lineEnd);
					displayLines.AtLineEnd = lineEnd;
				}
				finally
				{
					scrolling.SafeSetWindowLine(windowOriginY);
				}
				scrolling.WindowOriginY = num;
				MoveTo(position);
			}
			vertNavigate = true;
			vertNavigateX = x;
		}
		finally
		{
			EndLineEndUpdate();
		}
		ClearSelection();
	}

	protected void UpdateStartSearchPos()
	{
		if (searchUpdateCount == 0 && !FirstSearch)
		{
			searchStartPos = Position;
			searchCycled = false;
			if ((searchOptions & SearchOptions.SelectionOnly) != 0)
			{
				searchSelType = Selection.SelectionType;
				searchSelRect = Selection.SelectionRect;
			}
			InitSearchPos(searchOptions, update: true);
		}
	}

	protected internal void DoSelectionChanged()
	{
		UpdateStartSearchPos();
	}

	protected void ClearSelection()
	{
		if (selection.UpdateCount == 0 && !selection.IsEmpty && (selection.Options & SelectionOptions.PersistentBlocks) == 0)
		{
			selection.Clear();
		}
	}

	protected internal void SetNavigateOptions(NavigateOptions navigateOptions)
	{
		((TextSource)Source).SetNavigateOptions(navigateOptions);
	}

	protected internal void FinishVerticalNavigate()
	{
		vertNavigate = false;
	}

	public void Navigate(int deltaX, int deltaY)
	{
		Source.Navigate(deltaX, deltaY);
	}

	public void MoveTo(int x, int y)
	{
		Source.MoveTo(x, y);
	}

	public void MoveToChar(int x)
	{
		Source.MoveToChar(x);
	}

	public void MoveToLine(int y)
	{
		Source.MoveToLine(y);
	}

	public void MoveTo(Point position)
	{
		Source.MoveTo(position);
	}

	public void ValidatePosition(ref Point position)
	{
		Source.ValidatePosition(ref position);
	}

	public int StorePosition(Point pt)
	{
		return Source.StorePosition(pt);
	}

	public Point RestorePosition(int index)
	{
		return Source.RestorePosition(index);
	}

	public void MoveCharLeft()
	{
		bool flag = false;
		Point point = displayLines.PointToDisplayPoint(Position);
		if (point.X == 0 && (NavigateOptions.UpAtLineBegin & NavigateOptions) != 0)
		{
			point.X = displayLines[point.Y - 1].Length;
			point.Y--;
			displayLines.AtLineEnd = true;
			flag = true;
		}
		else if (point.X > 0)
		{
			point.X--;
		}
		if (flag)
		{
			BeginLineEndUpdate();
			Source.BeginUpdate(UpdateReason.Navigate);
			try
			{
				Source.State |= NotifyState.PositionChanged;
				MoveTo(displayLines.DisplayPointToPoint(point.X, point.Y, wrapEnd: true, rangeStart: true, tabEnd: false));
			}
			finally
			{
				Source.EndUpdate();
				EndLineEndUpdate();
			}
		}
		else
		{
			MoveTo(displayLines.DisplayPointToPoint(point.X, point.Y, wrapEnd: true, rangeStart: true, tabEnd: false));
		}
		ClearSelection();
	}

	public void MoveCharRight()
	{
		bool flag = false;
		Point point = displayLines.PointToDisplayPoint(Position);
		if (point.X >= displayLines[point.Y].Length && (NavigateOptions & NavigateOptions.BeyondEol) == 0)
		{
			if ((NavigateOptions & NavigateOptions.DownAtLineEnd) != 0 && ((NavigateOptions & NavigateOptions.BeyondEof) != 0 || point.Y < displayLines.GetCount() - 1))
			{
				point.Y++;
				point.X = 0;
				displayLines.AtLineEnd = false;
				flag = true;
			}
		}
		else
		{
			point.X++;
		}
		if (flag)
		{
			Source.BeginUpdate(UpdateReason.Navigate);
			try
			{
				Source.State |= NotifyState.PositionChanged;
				MoveTo(displayLines.DisplayPointToPoint(point.X, point.Y, wrapEnd: false, rangeStart: false, tabEnd: true));
			}
			finally
			{
				Source.EndUpdate();
			}
		}
		else
		{
			MoveTo(displayLines.DisplayPointToPoint(point.X, point.Y, wrapEnd: false, rangeStart: false, tabEnd: true));
		}
		ClearSelection();
	}

	public void MoveWordLeft()
	{
		Point point = displayLines.PointToDisplayPoint(Position);
		string text = displayLines[point.Y];
		if (point.X == 0)
		{
			if (point.Y > 0)
			{
				point.Y--;
				text = displayLines[point.Y];
				point.X = text.Length;
			}
			else
			{
				point = new Point(0, 0);
			}
		}
		int num = Math.Min(point.X, text.Length);
		if (num > 0)
		{
			bool flag = text[num - 1] == ' ' || text[num - 1] == '\t';
			bool flag2 = displayLines.IsDelimiter(text, num - 1);
			if (flag)
			{
				while (num > 0 && (text[num - 1] == ' ' || text[num - 1] == '\t'))
				{
					num--;
				}
			}
			if (flag2 && !flag)
			{
				num--;
			}
			else
			{
				while (num > 0 && !displayLines.IsDelimiter(text, num - 1))
				{
					num--;
				}
			}
		}
		MoveTo(displayLines.DisplayPointToPoint(num, point.Y, wrapEnd: false, rangeStart: true, tabEnd: false));
	}

	public void MoveWordRight()
	{
		Point point = displayLines.PointToDisplayPoint(Position);
		string text = displayLines[point.Y];
		int length = text.Length;
		if (point.X >= length)
		{
			if (point.Y + 1 < displayLines.GetCount())
			{
				point.Y++;
				text = displayLines[point.Y];
				point.X = text.Length - text.TrimStart(new char[0]).Length;
			}
			else
			{
				point.X = length;
			}
			MoveTo(displayLines.DisplayPointToPoint(point));
			return;
		}
		int i = point.X;
		bool flag;
		for (flag = displayLines.IsDelimiter(text, i); i < length && displayLines.IsDelimiter(text, i) == flag; i++)
		{
		}
		if (!flag)
		{
			for (; i < length && (text[i] == ' ' || text[i] == '\t'); i++)
			{
			}
		}
		if (i == point.X)
		{
			i++;
		}
		MoveTo(displayLines.DisplayPointToPoint(i, point.Y, wrapEnd: false, rangeStart: false, tabEnd: true));
	}

	public void MoveLineUp()
	{
		BeginLineEndUpdate();
		try
		{
			Point point = displayLines.PointToDisplayPoint(Position);
			if (point.Y > 0)
			{
				point.Y--;
			}
			if (pages.PageType == PageType.PageLayout)
			{
				point.X = DisplayToScreen(point.X, point.Y).X;
			}
			if (vertNavigate)
			{
				point.X = vertNavigateX;
			}
			int x = ((pages.PageType == PageType.PageLayout) ? ScreenToDisplayX(point.X, point.Y) : point.X);
			bool lineEnd = false;
			Point position = displayLines.DisplayPointToPoint(x, point.Y, wrapEnd: true, rangeStart: false, tabEnd: false, ref lineEnd);
			displayLines.AtLineEnd = lineEnd;
			MoveTo(position);
			vertNavigate = true;
			vertNavigateX = point.X;
		}
		finally
		{
			EndLineEndUpdate();
		}
		ClearSelection();
	}

	public void MoveLineDown()
	{
		BeginLineEndUpdate();
		try
		{
			Point point = displayLines.PointToDisplayPoint(Position);
			if (pages.PageType == PageType.PageLayout)
			{
				point.X = DisplayToScreen(point.X, point.Y).X;
			}
			if (vertNavigate)
			{
				point.X = vertNavigateX;
			}
			point.Y++;
			int x = ((pages.PageType == PageType.PageLayout) ? ScreenToDisplayX(point.X, point.Y) : point.X);
			bool lineEnd = false;
			Point position = displayLines.DisplayPointToPoint(x, point.Y, wrapEnd: true, rangeStart: false, tabEnd: false, ref lineEnd);
			displayLines.AtLineEnd = lineEnd;
			MoveTo(position);
			vertNavigate = true;
			vertNavigateX = point.X;
		}
		finally
		{
			EndLineEndUpdate();
		}
		ClearSelection();
	}

	public void MoveLineBegin()
	{
		Point point = displayLines.PointToDisplayPoint(Position);
		point.X = 0;
		MoveTo(displayLines.DisplayPointToPoint(point));
		ClearSelection();
	}

	public void MoveLineBeginCycled()
	{
		Point point = displayLines.PointToDisplayPoint(Position);
		string text = displayLines[point.Y];
		int num = text.Length - text.TrimStart(new char[0]).Length;
		point.X = ((point.X != num) ? num : 0);
		MoveTo(displayLines.DisplayPointToPoint(point));
		ClearSelection();
	}

	public void MoveLineEndCycled()
	{
		Point point = displayLines.PointToDisplayPoint(Position);
		string text = displayLines[point.Y];
		int length = text.TrimEnd(new char[0]).Length;
		point.X = ((point.X == length) ? text.Length : length);
		MoveTo(displayLines.DisplayPointToPoint(point));
		ClearSelection();
	}

	public void MoveLineEnd()
	{
		Point point = displayLines.PointToDisplayPoint(Position);
		point.X = displayLines[point.Y].Length;
		BeginLineEndUpdate();
		try
		{
			displayLines.AtLineEnd = true;
			MoveTo(displayLines.DisplayPointToPoint(point));
		}
		finally
		{
			EndLineEndUpdate();
		}
		ClearSelection();
	}

	public void MoveFileBegin()
	{
		MoveTo(0, 0);
		ClearSelection();
	}

	public void MoveFileEnd()
	{
		if (Lines.Count == 0)
		{
			MoveTo(0, 0);
		}
		else
		{
			MoveTo(Lines.GetLength(Lines.Count - 1), Lines.Count - 1);
		}
		ClearSelection();
	}

	public void ScrollLineDown()
	{
		if (pages.PageType == PageType.PageLayout)
		{
			scrolling.WindowOriginY += painter.FontHeight;
		}
		else
		{
			scrolling.WindowOriginY++;
		}
	}

	public void ScrollLineUp()
	{
		if (pages.PageType == PageType.PageLayout)
		{
			scrolling.WindowOriginY -= painter.FontHeight;
		}
		else
		{
			scrolling.WindowOriginY--;
		}
	}

	public void MoveToOpenBrace()
	{
		Point position = Position;
		if (Array.IndexOf(braces.ClosingBraces, Lines.GetCharAt(position)) >= 0 && braces.FindOpenBrace(ref position))
		{
			Position = position;
		}
	}

	public void MoveToCloseBrace()
	{
		Point position = Position;
		if (Array.IndexOf(braces.OpenBraces, Lines.GetCharAt(Position)) >= 0)
		{
			position.X++;
			if (braces.FindClosingBrace(ref position))
			{
				Position = position;
			}
		}
	}

	public void MoveToBrace()
	{
		Point position = Position;
		if (Array.IndexOf(braces.OpenBraces, Lines.GetCharAt(position)) >= 0)
		{
			position.X++;
			if (braces.FindClosingBrace(ref position))
			{
				Position = position;
			}
		}
		else if (Array.IndexOf(braces.ClosingBraces, Lines.GetCharAt(position)) >= 0)
		{
			if (braces.FindOpenBrace(ref position))
			{
				Position = position;
			}
		}
		else if (position.X > 0 && Array.IndexOf(braces.OpenBraces, Lines.GetCharAt(new Point(position.X - 1, position.Y))) >= 0)
		{
			if (braces.FindClosingBrace(ref position))
			{
				Position = position;
			}
		}
		else if (position.X > 0 && Array.IndexOf(braces.ClosingBraces, Lines.GetCharAt(new Point(position.X - 1, position.Y))) >= 0)
		{
			position.X--;
			if (braces.FindOpenBrace(ref position))
			{
				Position = position;
			}
		}
	}

	public void MovePageUp()
	{
		MovePage(direction: false);
	}

	public void MovePageDown()
	{
		MovePage(direction: true);
	}

	public void MoveScreenTop()
	{
		BeginLineEndUpdate();
		try
		{
			Point point = displayLines.PointToDisplayPoint(Position);
			int x = point.X;
			if (pages.PageType == PageType.PageLayout)
			{
				x = DisplayToScreen(point.X, point.Y).X;
			}
			if (vertNavigate)
			{
				x = vertNavigateX;
			}
			Point point2 = ((pages.PageType == PageType.PageLayout) ? ScreenToDisplay(x, ClientRect.Top) : new Point(x, scrolling.WindowOriginY));
			bool lineEnd = false;
			point = displayLines.DisplayPointToPoint(point2.X, point2.Y, wrapEnd: true, rangeStart: false, tabEnd: false, ref lineEnd);
			displayLines.AtLineEnd = lineEnd;
			MoveTo(point);
			vertNavigate = true;
			vertNavigateX = x;
		}
		finally
		{
			EndLineEndUpdate();
		}
		ClearSelection();
	}

	public void MoveScreenBottom()
	{
		BeginLineEndUpdate();
		try
		{
			Point point = displayLines.PointToDisplayPoint(Position);
			int x = point.X;
			if (pages.PageType == PageType.PageLayout)
			{
				x = DisplayToScreen(point.X, point.Y).X;
			}
			if (vertNavigate)
			{
				x = vertNavigateX;
			}
			Point point2 = ((pages.PageType == PageType.PageLayout) ? ScreenToDisplay(x, ClientRect.Bottom - painter.FontHeight) : new Point(x, scrolling.WindowOriginY + Math.Max(LinesInHeight() - 1, 0)));
			bool lineEnd = false;
			point = displayLines.DisplayPointToPoint(point2.X, point2.Y, wrapEnd: true, rangeStart: false, tabEnd: false, ref lineEnd);
			displayLines.AtLineEnd = lineEnd;
			MoveTo(point);
			vertNavigate = true;
			vertNavigateX = x;
		}
		finally
		{
			EndLineEndUpdate();
		}
		ClearSelection();
	}

	public virtual void ResetNavigateOptions()
	{
		NavigateOptions = EditConsts.DefaultNavigateOptions;
	}

	public bool ShouldSerializeNavigateOptions()
	{
		if (source == null)
		{
			return NavigateOptions != EditConsts.DefaultNavigateOptions;
		}
		return false;
	}

	public bool UpdateWordWrap()
	{
		return displayLines.UpdateWordWrap();
	}

	public bool UpdateWordWrap(int first, int last)
	{
		return displayLines.UpdateWordWrap(first, last);
	}

	public int GetWrapMargin()
	{
		return displayLines.GetWrapMargin();
	}

	public virtual void ResetWordWrap()
	{
		WordWrap = false;
	}

	public virtual void ResetWrapAtMargin()
	{
		WrapAtMargin = false;
	}

	public bool SaveFile(string fileName)
	{
		return displayLines.SaveFile(fileName);
	}

	public bool SaveFile(string fileName, Encoding encoding)
	{
		return SaveFile(fileName, ExportFormat.Text, encoding);
	}

	public bool SaveFile(string fileName, ExportFormat format)
	{
		return SaveFile(fileName, format, null);
	}

	public bool SaveFile(string fileName, ExportFormat format, Encoding encoding)
	{
		return displayLines.SaveFile(fileName, format, encoding);
	}

	public bool LoadStream(Stream stream)
	{
		return displayLines.LoadStream(stream);
	}

	public bool LoadStream(Stream stream, ExportFormat format)
	{
		return displayLines.LoadStream(stream, format, null);
	}

	public bool LoadStream(Stream stream, Encoding encoding)
	{
		return displayLines.LoadStream(stream, encoding);
	}

	public bool LoadStream(Stream stream, ExportFormat format, Encoding encoding)
	{
		return displayLines.LoadStream(stream, format, null);
	}

	public bool SaveStream(Stream stream)
	{
		return displayLines.SaveStream(stream);
	}

	public bool SaveStream(Stream stream, ExportFormat format)
	{
		return displayLines.SaveStream(stream, format);
	}

	public bool SaveStream(Stream stream, Encoding encoding)
	{
		return displayLines.SaveStream(stream, encoding);
	}

	public bool SaveStream(Stream stream, ExportFormat format, Encoding encoding)
	{
		return displayLines.SaveStream(stream, format, encoding);
	}

	public bool SaveStream(TextWriter writer)
	{
		return displayLines.SaveStream(writer);
	}

	public bool SaveStream(TextWriter writer, ExportFormat format)
	{
		return displayLines.SaveStream(writer, format);
	}

	public bool LoadFile(string fileName)
	{
		return displayLines.LoadFile(fileName);
	}

	public bool LoadFile(string fileName, Encoding encoding)
	{
		return LoadFile(fileName, ExportFormat.Text, encoding);
	}

	public bool LoadFile(string fileName, ExportFormat format)
	{
		return LoadFile(fileName, format, null);
	}

	public bool LoadFile(string fileName, ExportFormat format, Encoding encoding)
	{
		return displayLines.LoadFile(fileName, format, encoding);
	}

	public bool LoadStream(TextReader reader)
	{
		return displayLines.LoadStream(reader);
	}

	public bool LoadStream(TextReader reader, ExportFormat format)
	{
		return displayLines.LoadStream(reader, format);
	}

	public virtual void ResetIndentOptions()
	{
		IndentOptions = EditConsts.DefaultIndentOptions;
	}

	public virtual void ResetMaxLength()
	{
		MaxLength = 0;
	}

	public virtual void ResetModified()
	{
		Modified = false;
	}

	public virtual void ResetOverWrite()
	{
		OverWrite = false;
	}

	public void ResetSingleLineMode()
	{
		SingleLineMode = false;
	}

	public virtual void ResetReadonly()
	{
		Readonly = false;
	}

	public bool ShouldSerializeIndentOptions()
	{
		if (source == null)
		{
			return IndentOptions != EditConsts.DefaultIndentOptions;
		}
		return false;
	}

	public bool ShouldSerializeReadonly()
	{
		if (source == null)
		{
			return Readonly;
		}
		return false;
	}

	private Point PointToBlockPoint(int x, Point pt, string str)
	{
		Point point = new Point(0, 0);
		string[] array = StrItem.Split(str.Substring(0, Math.Min(x, str.Length)));
		if (array.Length != 0)
		{
			point.X = array[^1].Length;
			point.Y = array.Length - 1;
		}
		if (point.Y == 0)
		{
			return new Point(point.X + pt.X, pt.Y);
		}
		return new Point(point.X, point.Y + pt.Y);
	}

	private ICodeCompletionProvider GetQuickInfo(string text, bool useHtmlFormatting)
	{
		IQuickInfo quickInfo = new QuickInfo();
		quickInfo.Text = text;
		quickInfo.UseHtmlFormatting = useHtmlFormatting;
		return quickInfo;
	}

	private void DisableCodeCompletionTimer()
	{
		if (codeCompletionTimer != null)
		{
			codeCompletionTimer.Enabled = false;
		}
	}

	protected internal bool IsCodeCompletionWindowFocused()
	{
		Control control;
		return IsCodeCompletionWindowFocused(out control);
	}

	protected internal bool IsCodeCompletionWindowFocused(out Control control)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		if (codeCompletionBox != null && codeCompletionBox.IsFocused())
		{
			control = (Control)codeCompletionBox;
			return true;
		}
		if (codeCompletionHint != null && codeCompletionHint.IsFocused())
		{
			control = (Control)codeCompletionHint;
			return true;
		}
		control = null;
		return false;
	}

	protected internal bool ProcessSmartFormat(char ch)
	{
		if ((selection.Options & SelectionOptions.SmartFormat) != 0 && ((TextSource)Source).NeedFormatText())
		{
			ISyntaxParser syntaxParser = Lexer as ISyntaxParser;
			if (Array.IndexOf(syntaxParser.SmartFormatChars, ch) >= 0)
			{
				Selection.SmartFormatBlock();
				return true;
			}
		}
		return false;
	}

	protected internal bool ProcessAutoIndent(char ch)
	{
		if (((TextSource)Source).NeedFormatText())
		{
			ISyntaxParser syntaxParser = Lexer as ISyntaxParser;
			if (Array.IndexOf(syntaxParser.AutoIndentChars, ch) >= 0)
			{
				Selection.SmartIndent();
				return true;
			}
		}
		return false;
	}

	protected internal void ProcessCodeCompletion(CodeCompletionArgs e, string text)
	{
		if (Lexer is ISyntaxParser)
		{
			((ISyntaxParser)Lexer).CodeCompletion(text, Position, e);
		}
		if (this.NeedCodeCompletion != null)
		{
			this.NeedCodeCompletion(this, e);
		}
	}

	protected internal bool ProcessCodeCompletion(char ch)
	{
		DisableCodeCompletionTimer();
		bool flag = false;
		if (((TextSource)Source).NeedCodeCompletion() && (codeCompletionBox == null || !codeCompletionBox.Visible))
		{
			ISyntaxParser syntaxParser = Lexer as ISyntaxParser;
			if (Array.IndexOf(syntaxParser.CodeCompletionChars, ch) >= 0)
			{
				codeCompletionArgs.Init(CodeCompletionType.None, Position);
				codeCompletionArgs.KeyChar = ch;
				codeCompletionArgs.Interval = EditConsts.DefaultCompletionStartDelay;
				DoCodeCompletion();
				flag = true;
			}
		}
		if (Array.IndexOf(codeCompletionChars, ch) >= 0)
		{
			if (!flag)
			{
				codeCompletionArgs.Init(CodeCompletionType.None, Position);
				codeCompletionArgs.KeyChar = ch;
				DoCodeCompletion();
			}
			flag = true;
		}
		return flag;
	}

	protected void InsertCodeSnippet(ICodeSnippet snippet, Point startPos, Point endPos, bool useIndent)
	{
		Source.BeginUpdate(UpdateReason.InsertBlock);
		try
		{
			if (!selection.IsEmpty)
			{
				selection.SelectionType = SelectionType.Stream;
			}
			string text = (selection.IsEmpty ? string.Empty : selection.SelectedText);
			if (!selection.IsEmpty)
			{
				int num = (startPos.Equals((object?)selection.SelectionRect.Location) ? (-1) : Source.StorePosition(startPos));
				int num2 = (endPos.Equals((object?)selection.SelectionRect.Location) ? (-1) : Source.StorePosition(endPos));
				try
				{
					selection.Delete();
				}
				finally
				{
					if (num2 >= 0)
					{
						endPos = Source.RestorePosition(num2);
					}
					if (num >= 0)
					{
						startPos = Source.RestorePosition(num);
					}
				}
			}
			ArrayList arrayList = new ArrayList();
			CodeSnippetList codeSnippetList = new CodeSnippetList();
			foreach (ICodeSnippetDeclaration declaration in snippet.Declarations)
			{
				foreach (ICodeSnippetLiteral literal in declaration.Literals)
				{
					arrayList.Add(literal);
				}
				foreach (ICodeSnippetObject @object in declaration.Objects)
				{
					arrayList.Add(@object);
				}
			}
			string text2 = snippet.Code.Code;
			string delimiter = snippet.Code.Delimiter;
			Regex regex = new Regex("\\" + delimiter + EditConsts.DefaultSnippetPattern + "\\" + delimiter, RegexOptions.Multiline);
			bool flag = false;
			int num3 = 0;
			foreach (Match item in regex.Matches(text2))
			{
				if (!item.Success)
				{
					continue;
				}
				ICodeSnippetLiteral codeSnippetLiteral = null;
				bool flag2 = item.Value == delimiter + EditConsts.DefaultSnippetSelectedPattern + delimiter;
				bool flag3 = item.Value == delimiter + EditConsts.DefaultSnippetEndPattern + delimiter;
				flag = flag || flag3;
				if (!flag2)
				{
					foreach (ICodeSnippetLiteral item2 in arrayList)
					{
						if (item.Value == delimiter + item2.ID + delimiter)
						{
							codeSnippetLiteral = item2;
							break;
						}
					}
				}
				string text3 = (flag3 ? EditConsts.DefaultCaretSymbol.ToString() : (flag2 ? text : ((codeSnippetLiteral != null) ? codeSnippetLiteral.Default : string.Empty)));
				text2 = text2.Remove(item.Index - num3, item.Length).Insert(item.Index - num3, text3);
				if (codeSnippetLiteral != null && codeSnippetLiteral.Editable)
				{
					codeSnippetList.Add(new SnippetRange(new Point(item.Index - num3, 0), new Point(item.Index - num3 + codeSnippetLiteral.Default.Length, 0), codeSnippetLiteral.ID, codeSnippetLiteral.ToolTip, codeSnippetLiteral.ID != string.Empty));
				}
				else if (flag3)
				{
					codeSnippetList.Add(new SnippetRange(new Point(item.Index - num3, 0), new Point(item.Index - num3, 0), "$" + EditConsts.DefaultSnippetEndPattern + "$", string.Empty, isEditable: false));
				}
				num3 += item.Length - text3.Length;
			}
			if (codeSnippetList.Count != 0)
			{
				codeSnippetList.Add(new SnippetRange(new Point(text2.Length, 0), new Point(text2.Length, 0), string.Empty, string.Empty, isEditable: false));
			}
			startPos.X = Math.Min(startPos.X, Lines.GetLength(startPos.Y));
			InsertTextFromProvider(null, text2, startPos, endPos, useIndent, out var curPos, text == string.Empty);
			TextSource textSource = (TextSource)Source;
			foreach (IRange item3 in codeSnippetList)
			{
				item3.StartPoint = PointToBlockPoint(item3.StartPoint.X, startPos, text2);
				item3.EndPoint = PointToBlockPoint(item3.EndPoint.X, startPos, text2);
			}
			textSource.CodeSnippets = codeSnippetList;
			textSource.SnippetUpdateCount++;
			try
			{
				selection.SetSelection(SelectionType.Stream, startPos, Position);
				selection.SmartFormat();
				selection.Clear();
			}
			finally
			{
				textSource.SnippetUpdateCount--;
			}
			if (!SelectFirstSnippet() && flag && curPos.X >= 0 && curPos.Y >= 0)
			{
				MoveTo(curPos);
			}
		}
		finally
		{
			Source.EndUpdate();
		}
	}

	protected void InsertTextFromProvider(ICodeCompletionProvider provider, string text, Point startPos, Point endPos, bool useIndent)
	{
		InsertTextFromProvider(provider, text, startPos, endPos, useIndent, out var curPos, needSelect: true);
		if (curPos.X >= 0 && curPos.Y >= 0)
		{
			MoveTo(curPos);
		}
	}

	protected void InsertTextFromProvider(ICodeCompletionProvider provider, string text, Point startPos, Point endPos, bool useIndent, out Point curPos, bool needSelect)
	{
		TextSource textSource = (TextSource)Source;
		textSource.BeginUpdate(UpdateReason.Other);
		try
		{
			if (provider != null && provider.UseHtmlFormatting)
			{
				text = text.Replace("&lt;", "<").Replace("&gt;", ">").Replace("&amp;", "@")
					.Replace("&quot;", "\"")
					.Replace("&nbsp;", " ");
			}
			MoveTo(startPos);
			string text2 = Lines[Position.Y];
			if (needSelect)
			{
				if (startPos.X >= 0 && startPos.Y >= 0 && endPos.X >= 0 && endPos.Y >= 0)
				{
					selection.SetSelection(SelectionType.Stream, startPos, endPos);
				}
				else if (Position.X >= 0 && Position.X < text2.Length && !Lines.IsDelimiter(text2, Position.X))
				{
					selection.SelectWord();
					if (!selection.IsEmpty)
					{
						selection.SetSelection(Selection.SelectionType, startPos, new Point(Selection.SelectionRect.Right, Selection.SelectionRect.Bottom));
					}
				}
			}
			string[] array = StrItem.Split(text);
			curPos = new Point(-1, -1);
			int num = array[0].IndexOf(EditConsts.DefaultCaretSymbol);
			if (num >= 0)
			{
				curPos.X = startPos.X + num;
				curPos.Y = startPos.Y;
				array[0] = array[0].Remove(num, 1);
			}
			string text3 = ((useIndent && array.Length > 1) ? ((SyntaxStrings)Lines).GetIndentString(Lines.TabPosToPos(Lines[Position.Y], startPos.X), 0) : string.Empty);
			for (int i = 1; i < array.Length; i++)
			{
				array[i] = text3 + array[i];
				num = array[i].IndexOf(EditConsts.DefaultCaretSymbol);
				if (num >= 0)
				{
					array[i] = array[i].Remove(num, 1);
					curPos.X = num;
					curPos.Y = Position.Y + i;
				}
			}
			if (!selection.IsEmpty)
			{
				selection.Delete();
			}
			textSource.InsertBlock(array);
			selection.Clear();
		}
		finally
		{
			textSource.EndUpdate();
		}
	}

	protected internal bool SelectFirstSnippet()
	{
		TextSource textSource = (TextSource)Source;
		int firstSnippet = textSource.CodeSnippets.GetFirstSnippet();
		if (firstSnippet >= 0)
		{
			IRange range = textSource.CodeSnippets[firstSnippet] as IRange;
			MoveTo(range.StartPoint);
			Selection.SetSelection(SelectionType.Stream, range.StartPoint, range.EndPoint);
		}
		return firstSnippet >= 0;
	}

	protected internal bool ProcessTab(Point position)
	{
		return SelectSnippet(position, selectNext: true, (selection.Options & SelectionOptions.DisableCodeSnippetOnTab) == 0);
	}

	protected internal bool ProcessShiftTab(Point position)
	{
		return SelectSnippet(position, selectNext: false, insertNew: false);
	}

	protected internal bool ProcessEnter()
	{
		if (InIncrementalSearch)
		{
			FinishIncrementalSearch();
			return true;
		}
		TextSource textSource = (TextSource)Source;
		if (textSource.CodeSnippets.FindSnippet(Position, exact: false, out var _))
		{
			SnippetRange snippetRange = null;
			foreach (SnippetRange codeSnippet in textSource.CodeSnippets)
			{
				if (codeSnippet.Id == "$" + EditConsts.DefaultSnippetEndPattern + "$")
				{
					snippetRange = codeSnippet;
					break;
				}
			}
			textSource.CurrentSnippet = snippetRange;
			if (snippetRange != null)
			{
				MoveTo(snippetRange.StartPoint);
				selection.SmartIndent();
			}
			textSource.UnhighlightCodeSnippets();
			return true;
		}
		return false;
	}

	protected bool SelectSnippet(Point position, bool selectNext, bool insertNew)
	{
		TextSource textSource = (TextSource)Source;
		if (textSource.CodeSnippets.FindSnippet(position, exact: false, out var index))
		{
			if (!textSource.CodeSnippets.IsFirstSnippet(index))
			{
				if (!selectNext)
				{
					return true;
				}
				textSource.UnhighlightCodeSnippets();
				return false;
			}
			int num = (selectNext ? textSource.CodeSnippets.GetNextSnippet(index) : textSource.CodeSnippets.GetPrevSnippet(index));
			if (num != index)
			{
				IRange range = (IRange)textSource.CodeSnippets[num];
				MoveTo(range.StartPoint);
				Selection.SetSelection(SelectionType.Stream, range.StartPoint, range.EndPoint);
			}
			return true;
		}
		if (insertNew && !Readonly && !Source.IsLineReadonly(Position.Y))
		{
			string text = Lines[Position.Y];
			if (text.Trim() != string.Empty && ((TextSource)Source).NeedCodeCompletion() && Position.X <= text.Length && !Lines.IsDelimiter(text, Math.Max(Position.X - 1, 0)) && (Position.X == text.Length || Lines.IsDelimiter(Position.Y, Math.Max(Position.X, 0))) && Lines.GetWord(text, Math.Max(Position.X - 1, 0), out var left, out var right))
			{
				ISyntaxParser syntaxParser = Lexer as ISyntaxParser;
				ICodeSnippetsProvider codeSnippets = syntaxParser.CodeSnippets;
				if (codeSnippets != null)
				{
					ICodeSnippet codeSnippet = codeSnippets.FindByShortcut(text.Substring(left, right - left + 1), syntaxParser.CaseSensitive);
					if (codeSnippet != null)
					{
						InsertCodeSnippet(codeSnippet, new Point(left, Position.Y), new Point(right + 1, Position.Y), codeSnippets.UseIndent);
						return true;
					}
				}
			}
		}
		return false;
	}

	protected void ClosePopupWindow()
	{
		if (codeCompletionBox != null && codeCompletionBox.Visible)
		{
			codeCompletionBox.Close(accept: false);
		}
	}

	protected void CloseCodeCompletionBox(object sender, ClosingEventArgs e)
	{
		if (!e.Accepted || e.Text == null || !(e.Text != string.Empty))
		{
			return;
		}
		ICodeCompletionWindow codeCompletionWindow = (ICodeCompletionWindow)sender;
		if (e.Provider is ICodeSnippets)
		{
			if (e.Provider.SelIndex >= 0 && e.Provider.SelIndex < e.Provider.Count)
			{
				InsertCodeSnippet(((ICodeSnippets)e.Provider)[e.Provider.SelIndex], codeCompletionWindow.StartPos, codeCompletionWindow.EndPos, e.UseIndent);
			}
		}
		else
		{
			InsertTextFromProvider(e.Provider, e.Text, codeCompletionWindow.StartPos, codeCompletionWindow.EndPos, e.UseIndent);
		}
	}

	protected void DisposeCodeCompletionBox(object sender, EventArgs e)
	{
		if (sender == codeCompletionBox)
		{
			codeCompletionBox = null;
		}
		else if (sender == codeCompletionHint)
		{
			codeCompletionHint = null;
		}
	}

	protected void CloseCodeCompletionHint(object sender, ClosingEventArgs e)
	{
		if (e.Accepted && e.Text != null && e.Text != string.Empty)
		{
			ICodeCompletionWindow codeCompletionWindow = (ICodeCompletionWindow)sender;
			InsertTextFromProvider(e.Provider, e.Text, codeCompletionWindow.StartPos, codeCompletionWindow.EndPos, e.UseIndent);
		}
	}

	protected virtual void ListMembers(CodeCompletionArgs e, CodeCompletionType completionType)
	{
		e.Init(completionType, Position);
		if (((TextSource)Source).NeedCodeCompletion())
		{
			DoCodeCompletion(Lexer as ISyntaxParser, RemovePlainText(Position.Y), Position, e);
		}
	}

	protected void ListMembers(CodeCompletionArgs e)
	{
		ListMembers(e, CodeCompletionType.ListMembers);
	}

	protected void CompleteWord(CodeCompletionArgs e)
	{
		ListMembers(e, CodeCompletionType.CompleteWord);
	}

	protected virtual void ParameterInfo(CodeCompletionArgs e)
	{
		e.Init(CodeCompletionType.ParameterInfo, Position);
		e.ToolTip = true;
		if (((TextSource)Source).NeedCodeCompletion())
		{
			DoCodeCompletion(Lexer as ISyntaxParser, RemovePlainText(Position.Y), Position, e);
		}
	}

	protected virtual void QuickInfo(CodeCompletionArgs e)
	{
		QuickInfo(e, Position, needReparse: true);
	}

	protected virtual void QuickInfo(CodeCompletionArgs e, Point position, bool needReparse)
	{
		e.Init(CodeCompletionType.QuickInfo, position, needReparse);
		e.ToolTip = true;
		if (((TextSource)Source).NeedCodeCompletion())
		{
			DoCodeCompletion(Lexer as ISyntaxParser, RemovePlainText(position.Y), position, e);
		}
	}

	protected virtual void CodeSnippets(CodeCompletionArgs e)
	{
		e.Init(CodeCompletionType.CodeSnippets, Position);
		if (((TextSource)Source).NeedCodeCompletion())
		{
			DoCodeCompletion(Lexer as ISyntaxParser, RemovePlainText(Position.Y), Position, e);
		}
	}

	protected virtual ICodeCompletionBox CreateCodeCompletionBox()
	{
		return new CodeCompletionBox(this);
	}

	protected virtual ICodeCompletionHint CreateCodeCompletionHint()
	{
		return new CodeCompletionHint(this);
	}

	public void ShowCodeCompletionBox(ICodeCompletionProvider provider)
	{
		Point pt = ((Control)this).PointToScreen(TextToScreen((codeCompletionArgs.DisplayPosition.Y >= 0 && codeCompletionArgs.StartPosition.X >= 0) ? codeCompletionArgs.DisplayPosition : Position));
		pt.Y += painter.FontHeight;
		ShowCodeCompletionBox(provider, pt);
	}

	public void ShowCodeCompletionBox(ICodeCompletionProvider provider, Point pt)
	{
		ShowCodeCompletionBox(provider, pt, codeCompletionArgs.StartPosition, codeCompletionArgs.EndPosition);
	}

	public void ShowCodeCompletionBox(ICodeCompletionProvider provider, Point pt, Point startPoint, Point endPoint)
	{
		CodeCompletionBox.Provider = provider;
		CodeCompletionBox.StartPos = startPoint;
		CodeCompletionBox.EndPos = endPoint;
		CodeCompletionBox.PopupAt(pt);
	}

	public void ShowCodeCompletionHint(ICodeCompletionProvider provider)
	{
		Point pt = ((Control)this).PointToScreen(TextToScreen((codeCompletionArgs.DisplayPosition.Y >= 0 && codeCompletionArgs.DisplayPosition.X >= 0) ? codeCompletionArgs.DisplayPosition : Position));
		pt.Y += painter.FontHeight;
		ShowCodeCompletionHint(provider, pt);
	}

	public void ShowCodeCompletionHint(ICodeCompletionProvider provider, Point pt)
	{
		ShowCodeCompletionHint(provider, pt, codeCompletionArgs.StartPosition, codeCompletionArgs.EndPosition, codeCompletionArgs.DisplayPosition, keepActive: false, codeCompletionArgs.CompletionType == CodeCompletionType.SpecialListMembers, null);
	}

	public void ShowCodeCompletionHint(ICodeCompletionProvider provider, Point pt, Point startPoint, Point endPoint, Point displayPoint, bool keepActive, bool acceptOnTab, ILexer lexer)
	{
		ICodeCompletionHint codeCompletionHint = CodeCompletionHint;
		codeCompletionHint.Lexer = lexer;
		codeCompletionHint.Provider = provider;
		codeCompletionHint.StartPos = startPoint;
		codeCompletionHint.DisplayPos = displayPoint;
		codeCompletionHint.EndPos = endPoint;
		if (keepActive)
		{
			codeCompletionHint.CompletionFlags |= CodeCompletionFlags.KeepActive;
		}
		else
		{
			codeCompletionHint.CompletionFlags &= ~CodeCompletionFlags.KeepActive;
		}
		if (acceptOnTab)
		{
			codeCompletionHint.CompletionFlags |= CodeCompletionFlags.AcceptOnTab;
		}
		else
		{
			codeCompletionHint.CompletionFlags &= ~CodeCompletionFlags.AcceptOnTab;
		}
		codeCompletionHint.PopupAt(pt);
	}

	public string RemovePlainText(int line)
	{
		IStrItem item = Lines.GetItem(line);
		string text = string.Empty;
		if (item != null)
		{
			text = item.String;
			ILexScheme lexScheme = ((Lexer != null) ? Lexer.Scheme : null);
			if (lexScheme != null)
			{
				text = string.Empty;
				for (int i = 0; i < item.String.Length; i++)
				{
					text += (lexScheme.IsPlainText((byte)item.ColorData[i] - 1) ? ' ' : item.String[i]);
				}
			}
		}
		return text;
	}

	public bool IsValidText(Point position)
	{
		if (Lexer != null)
		{
			IStrItem item = Lines.GetItem(position.Y);
			if (item != null)
			{
				if (position.X == item.String.Length)
				{
					position.X--;
				}
				if (position.X >= 0 && position.X < item.ColorData.Length)
				{
					return !Lexer.Scheme.IsPlainText((byte)item.ColorData[position.X] - 1);
				}
			}
		}
		return true;
	}

	public void CompleteWord()
	{
		if (IsValidText(Position))
		{
			ListMembers(codeCompletionArgs, CodeCompletionType.CompleteWord);
			OnNeedCompletion(codeCompletionArgs);
		}
	}

	public void ListMembers()
	{
		if (IsValidText(Position))
		{
			ListMembers(codeCompletionArgs, CodeCompletionType.ListMembers);
			OnNeedCompletion(codeCompletionArgs);
		}
	}

	public void ParameterInfo()
	{
		ParameterInfo(codeCompletionArgs);
		OnNeedCompletion(codeCompletionArgs);
	}

	public void CodeSnippets()
	{
		CodeSnippets(codeCompletionArgs);
		OnNeedCompletion(codeCompletionArgs);
	}

	public void QuickInfo()
	{
		QuickInfo(codeCompletionArgs);
		OnNeedCompletion(codeCompletionArgs);
	}

	public bool ShouldSerializeCodeCompletionChars()
	{
		return !((TextSource)Source).IsEqualArrays(codeCompletionChars, EditConsts.DefaultCodeCompletionChars.ToCharArray());
	}

	public bool ShouldSerializeColumnsIndentForeColor()
	{
		return columnsIndentForeColor != EditConsts.DefaultColumnsIndentForeColor;
	}

	private void ProcessPlayBack(MacroKeyListData data)
	{
		if (data.Action != null)
		{
			data.Action();
		}
		else if (data.ActionEx != null)
		{
			data.ActionEx(data.Param);
		}
	}

	protected internal void RecordKeyData(MacroKeyListData keyData)
	{
		macroRecords.Add(keyData);
	}

	public void StartMacroRecording()
	{
		MacroRecording = true;
	}

	public void StopMacroRecording()
	{
		MacroRecording = false;
	}

	public void PauseMacroRecording()
	{
		MacroSuspendend = true;
		MacroRecording = false;
	}

	public void ResumeMacroRecording()
	{
		MacroRecording = true;
		MacroSuspendend = false;
	}

	public void ToggleMacroRecording()
	{
		if (macroUpdateCount <= 0)
		{
			MacroRecording = !MacroRecording;
		}
	}

	public void PlayBackMacro()
	{
		if (macroUpdateCount > 0)
		{
			return;
		}
		macroUpdateCount++;
		try
		{
			macroRecording = false;
			ArrayList arrayList = new ArrayList(macroRecords);
			foreach (MacroKeyListData item in arrayList)
			{
				ProcessPlayBack(item);
			}
		}
		finally
		{
			macroUpdateCount--;
		}
	}

	public void SaveMacros(Stream stream)
	{
		StreamWriter streamWriter = new StreamWriter(stream);
		try
		{
			SaveMacros(streamWriter);
		}
		finally
		{
			streamWriter.Close();
		}
	}

	public void SaveMacros(TextWriter writer)
	{
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(MacroKeyListData[]));
		try
		{
			xmlSerializer.Serialize(writer, MacroRecords);
		}
		catch (Exception exception)
		{
			writer.Flush();
			ErrorHandler.Error(exception);
		}
	}

	public void SaveMacros(string fileName)
	{
		Stream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
		try
		{
			SaveMacros(stream);
		}
		finally
		{
			stream.Close();
		}
	}

	public void LoadMacros(Stream stream)
	{
		StreamReader streamReader = new StreamReader(stream);
		try
		{
			LoadMacros(streamReader);
		}
		finally
		{
			streamReader.Close();
		}
	}

	public void LoadMacros(TextReader reader)
	{
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(MacroKeyListData[]));
		try
		{
			MacroRecords = (MacroKeyListData[])xmlSerializer.Deserialize(reader);
		}
		catch (Exception exception)
		{
			ErrorHandler.Error(exception);
		}
	}

	public void LoadMacros(string fileName)
	{
		Stream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
		try
		{
			LoadMacros(stream);
		}
		finally
		{
			stream.Close();
		}
	}

	public bool ShouldSerializeReadonlyForeColor()
	{
		return readonlyForeColor != Color.Empty;
	}

	public bool ShouldSerializeReadonlyBackColor()
	{
		return readonlyBackColor != Color.Empty;
	}

	public bool ShouldSerializeDisabledForeColor()
	{
		return disabledForeColor != Color.Empty;
	}

	public bool ShouldSerializeDisabledBackColor()
	{
		return disabledBackColor != Color.Empty;
	}

	protected virtual void AssignProps(SyntaxEdit source)
	{
		XmlSyntaxEditInfo xmlSyntaxEditInfo = source.XmlInfo as XmlSyntaxEditInfo;
		xmlSyntaxEditInfo.Load();
		XmlInfo = xmlSyntaxEditInfo;
	}

	protected virtual ISyntaxEdit CreateSplitEdit(bool vert)
	{
		SyntaxEdit syntaxEdit = new SyntaxEdit();
		((Control)syntaxEdit).Visible = false;
		((Control)syntaxEdit).Parent = ((Control)this).Parent;
		((Control)syntaxEdit).BringToFront();
		syntaxEdit.AssignProps(this);
		syntaxEdit.Source = Source;
		((Control)syntaxEdit).Location = ((Control)this).Location;
		((Control)syntaxEdit).Dock = (DockStyle)((!vert) ? 1 : 3);
		if (vert)
		{
			((Control)syntaxEdit).Width = 0;
		}
		else
		{
			((Control)syntaxEdit).Height = 0;
		}
		components.Add((IComponent?)syntaxEdit);
		if (vert)
		{
			vertSplitEdit = syntaxEdit;
		}
		else
		{
			horzSplitEdit = syntaxEdit;
		}
		return syntaxEdit;
	}

	protected void SplitterMoved(object sender, SplitterEventArgs e)
	{
		if (sender == horzSplitter && (horzSplitEdit == null || e.SplitY - horzSplitEdit.Top <= EditConsts.DefaltScrollSplitterSize + ((Control)horzSplitter).Height))
		{
			UnsplitView(vert: false);
		}
		else if (sender == vertSplitter && (vertSplitEdit == null || e.SplitX - vertSplitEdit.Top <= EditConsts.DefaltScrollSplitterSize + ((Control)vertSplitter).Width))
		{
			UnsplitView(vert: true);
		}
	}

	protected virtual Splitter CreateSplitter(bool vert)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		Splitter val = new Splitter();
		((Control)val).Visible = false;
		((Control)val).Parent = ((Control)this).Parent;
		((Control)val).BringToFront();
		((Control)val).Dock = (DockStyle)((!vert) ? 1 : 3);
		((Control)val).Location = ((Control)this).Location;
		val.MinSize = 0;
		val.BorderStyle = (BorderStyle)2;
		val.SplitterMoved += new SplitterEventHandler(SplitterMoved);
		components.Add((IComponent?)val);
		if (vert)
		{
			vertSplitter = val;
		}
		else
		{
			horzSplitter = val;
		}
		return val;
	}

	protected ISyntaxEdit GetSplitEdit(bool vert)
	{
		if (!vert)
		{
			return horzSplitEdit;
		}
		return vertSplitEdit;
	}

	protected Splitter GetSplitter(bool vert)
	{
		if (!vert)
		{
			return horzSplitter;
		}
		return vertSplitter;
	}

	protected bool CanSplit(bool vert)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Invalid comparison between Unknown and I4
		Splitter splitter = GetSplitter(vert);
		if (((Control)this).IsHandleCreated && (int)((Control)this).Dock == 5)
		{
			if (splitter != null)
			{
				return !((Control)splitter).Visible;
			}
			return true;
		}
		return false;
	}

	protected bool CanUnsplit(bool vert)
	{
		Splitter splitter = GetSplitter(vert);
		if (splitter != null)
		{
			return ((Control)splitter).Visible;
		}
		return false;
	}

	protected void SplitView(bool vert)
	{
		if (!CanSplit(vert))
		{
			return;
		}
		Splitter val = GetSplitter(vert);
		if (val == null)
		{
			val = CreateSplitter(vert);
		}
		ISyntaxEdit syntaxEdit = GetSplitEdit(vert);
		if (syntaxEdit == null)
		{
			syntaxEdit = CreateSplitEdit(vert);
		}
		if (vert)
		{
			syntaxEdit.Width = 0;
		}
		else
		{
			syntaxEdit.Height = 0;
		}
		syntaxEdit.Scrolling.WindowOriginY = Scrolling.WindowOriginY;
		syntaxEdit.Scrolling.WindowOriginX = Scrolling.WindowOriginX;
		((Control)val).Visible = true;
		syntaxEdit.Visible = true;
		((Control)this).BringToFront();
		if (vert)
		{
			if (this.SplitVert != null)
			{
				this.SplitVert(this, EventArgs.Empty);
			}
		}
		else if (this.SplitHorz != null)
		{
			this.SplitHorz(this, EventArgs.Empty);
		}
	}

	protected void UnsplitView(bool vert)
	{
		if (!CanUnsplit(vert))
		{
			return;
		}
		Splitter splitter = GetSplitter(vert);
		if (splitter != null)
		{
			((Control)splitter).Visible = false;
		}
		ISyntaxEdit splitEdit = GetSplitEdit(vert);
		if (splitEdit != null)
		{
			splitEdit.Visible = false;
		}
		if (vert)
		{
			if (this.UnsplitVert != null)
			{
				this.UnsplitVert(this, EventArgs.Empty);
			}
		}
		else if (this.UnsplitHorz != null)
		{
			this.UnsplitHorz(this, EventArgs.Empty);
		}
	}

	public bool CanSplitHorz()
	{
		return CanSplit(vert: false);
	}

	public bool CanSplitVert()
	{
		return CanSplit(vert: true);
	}

	public bool CanUnsplitHorz()
	{
		return CanUnsplit(vert: false);
	}

	public bool CanUnsplitVert()
	{
		return CanUnsplit(vert: true);
	}

	public void SplitViewHorz()
	{
		SplitView(vert: false);
	}

	public void SplitViewVert()
	{
		SplitView(vert: true);
	}

	public void UnsplitViewHorz()
	{
		UnsplitView(vert: false);
	}

	public void UnsplitViewVert()
	{
		UnsplitView(vert: true);
	}

	protected virtual void DoAutoCorrection(Point pos, string word, string correctWord)
	{
		Source.BeginUpdate(UpdateReason.Insert);
		int index = Source.StorePosition(Position);
		try
		{
			Source.Position = pos;
			Source.DeleteRight(word.Length);
			Source.Insert(correctWord);
		}
		finally
		{
			Position = Source.RestorePosition(index);
			Source.EndUpdate();
		}
	}

	protected virtual void CheckAutoCorrect(Point pos, string word)
	{
		if (word != string.Empty && (!((TextSource)Source).IsWordCorrectEventAssigned() || !spelling.IsWordCorrect(word)) && HasAutoCorrection(word, out var correctWord))
		{
			DoAutoCorrection(pos, word, correctWord);
		}
	}

	protected virtual void CheckAutoCorrect()
	{
		if (lastKey == '\0')
		{
			return;
		}
		if (autoCorrection && Array.IndexOf(autoCorrectDelimiters, lastKey) >= 0)
		{
			Point oldPosition = ((TextSource)Source).OldPosition;
			string text = Lines[oldPosition.Y];
			if (((SyntaxStrings)Lines).GetWord(text, Math.Max(oldPosition.X - 1, 0), out var left, out var right, ((TextSource)Source).SpellTable))
			{
				CheckAutoCorrect(new Point(left, oldPosition.Y), text.Substring(left, right - left + 1));
			}
		}
		lastKey = '\0';
	}

	public bool HasAutoCorrection(string word, out string correctWord)
	{
		if (this.AutoCorrect != null)
		{
			autoCorrectEventArgs.Word = word;
			autoCorrectEventArgs.CorrectWord = word;
			autoCorrectEventArgs.HasCorrection = false;
			this.AutoCorrect(this, autoCorrectEventArgs);
			correctWord = autoCorrectEventArgs.CorrectWord;
			return autoCorrectEventArgs.HasCorrection;
		}
		correctWord = word;
		return false;
	}

	private void SourceChanged()
	{
		if (displayLines != null)
		{
			displayLines.Lines = Source.Lines;
			DoOutlineText(update: true);
			MakeVisible(Position);
			UpdateCaret();
			scrolling.UpdateScroll(updateSize: false);
			selection.Clear();
			((Control)this).Invalidate();
		}
	}

	private void DoOutlineText(bool update)
	{
		if (!outlining.AllowOutlining)
		{
			return;
		}
		if (((TextSource)Source).NeedOutlineText())
		{
			ISyntaxParser syntaxParser = (ISyntaxParser)Source.Lexer;
			ArrayList ranges = new ArrayList();
			syntaxParser.Outline(ranges);
			outlining.SetOutlineRanges(ranges, preserveVisible: true);
			if (Source.ActiveEdit == this)
			{
				Point position = Position;
				if (position.Y >= Lines.Count - 1 && position.X > 0)
				{
					position.X--;
				}
				outlining.EnsureExpanded(position);
			}
		}
		else if (update)
		{
			outlining.UnOutline();
		}
	}

	protected internal Point ScreenToText(int x, int y, ref bool lineEnd)
	{
		Point point = ScreenToDisplay(x, y);
		return displayLines.DisplayPointToPoint(point.X, point.Y, wrapEnd: true, rangeStart: false, tabEnd: false, ref lineEnd);
	}

	protected internal Point TextToScreen(Point position, bool lineEnd)
	{
		return TextToScreen(position.X, position.Y, lineEnd);
	}

	protected internal Point TextToScreen(int x, int y, bool lineEnd)
	{
		Point point = displayLines.PointToDisplayPoint(x, y, lineEnd);
		return DisplayToScreen(point.X, point.Y);
	}

	protected internal Region GetRectRegion(SelectionType selectionType, Rectangle rect, bool atTopLeftEnd, bool atBottomRightEnd)
	{
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Expected O, but got Unknown
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_029e: Expected O, but got Unknown
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Expected O, but got Unknown
		Region val = null;
		if (selectionType != 0)
		{
			int fontHeight = painter.FontHeight;
			Point point = displayLines.PointToDisplayPoint(rect.Location, atTopLeftEnd);
			Point point2 = displayLines.PointToDisplayPoint(rect.Location + rect.Size, atBottomRightEnd);
			if (selectionType == SelectionType.Block && point.X == point2.X && point.Y != point2.Y)
			{
				if (point.X == 0)
				{
					point2.Y--;
				}
				point.X = 0;
				point2.X = int.MaxValue;
			}
			point = DisplayToScreen(point.X, point.Y);
			point2 = DisplayToScreen(point2.X, point2.Y);
			if (point2.X == int.MaxValue)
			{
				point2.X = ClientRect.Right;
			}
			if (point.Y == point2.Y || selectionType == SelectionType.Block)
			{
				int num = Math.Min(point.X, point2.X);
				int num2 = Math.Max(point.X, point2.X);
				val = new Region(new Rectangle(num, point.Y, num2 - num + 1, point2.Y - point.Y + fontHeight));
			}
			else
			{
				int num3 = point2.Y - point.Y;
				if (pages.PageType == PageType.PageLayout)
				{
					IEditPage pageAtPoint = pages.GetPageAtPoint(point);
					IEditPage pageAtPoint2 = pages.GetPageAtPoint(point2);
					Rectangle clientRect = pageAtPoint.ClientRect;
					Rectangle clientRect2 = pageAtPoint2.ClientRect;
					val = new Region(new Rectangle(point.X, point.Y, clientRect.Right - point.X + 1, fontHeight));
					int num4 = Math.Min(clientRect.Left, clientRect2.Left);
					val.Union(new Rectangle(num4, point.Y + fontHeight, Math.Max(clientRect.Right, clientRect2.Right) - num4, num3 - fontHeight));
					val.Union(new Rectangle(clientRect2.Left, point2.Y, point2.X - clientRect2.Left + 1, fontHeight));
				}
				else
				{
					int num5 = gutter.GetWidth() + ClientRect.Left;
					int right = ClientRect.Right;
					val = new Region(new Rectangle(point.X, point.Y, right - point.X + 1, fontHeight));
					val.Union(new Rectangle(num5, point.Y + fontHeight, right - num5 + 1, num3 - fontHeight));
					val.Union(new Rectangle(num5, point2.Y, point2.X - num5 + 1, fontHeight));
				}
			}
		}
		return val;
	}

	protected internal Region GetRectRegion(Rectangle rect)
	{
		return GetRectRegion(SelectionType.Stream, rect, atTopLeftEnd: false, atBottomRightEnd: false);
	}

	protected internal void WindowOriginChanged(int line, int ch)
	{
		InvalidateLines(ch, line);
		UpdateCaret();
		if (line != scrolling.WindowOriginY)
		{
			if ((pages.Rulers & EditRulers.Vertical) != 0)
			{
				pages.UpdateRulers();
			}
			if (gutter.LineNumbersChanged())
			{
				((Control)this).Invalidate();
			}
		}
		if (ch != scrolling.WindowOriginX)
		{
			if ((pages.Rulers & EditRulers.Horizonal) != 0)
			{
				pages.UpdateRulers();
			}
			vertNavigate = false;
		}
	}

	protected internal void TransparentChanged()
	{
		bool isTransparent = IsTransparent;
		((Control)this).SetStyle((ControlStyles)4, !isTransparent);
		((Control)this).SetStyle((ControlStyles)65536, isTransparent);
		((Control)this).Invalidate();
	}

	protected bool IsMouseOnPageWhiteSpace(int x, int y)
	{
		if (pages.PageType != 0)
		{
			IEditPage pageAtPoint = Pages.GetPageAtPoint(x, y);
			if (pageAtPoint != null)
			{
				if (!((EditPage)pageAtPoint).WhiteSpaceTopRect.Contains(x, y))
				{
					return ((EditPage)pageAtPoint).WhiteSpaceBottomRect.Contains(x, y);
				}
				return true;
			}
			return false;
		}
		return false;
	}

	protected bool IsMouseOnSyntaxError(int x, int y, out ISyntaxError err)
	{
		err = null;
		Point point = ScreenToText(x, y);
		if (point.Y >= 0 && point.Y < Lines.Count)
		{
			IStrItem item = Lines.GetItem(point.Y);
			if (point.X >= 0)
			{
				ColorFlags flag = ColorFlags.None;
				if (point.X < item.String.Length)
				{
					item.GetColorFlag(point.X, out flag);
				}
				else if (point.X == item.String.Length)
				{
					flag = ColorFlags.WaveLine;
				}
				if ((flag & ColorFlags.WaveLine) != 0)
				{
					err = ((TextSource)Source).GetSyntaxError(point.X, point.Y);
				}
			}
		}
		return err != null;
	}

	public int ScreenToDisplayX(int x, int line)
	{
		int width;
		if (pages.PageType == PageType.PageLayout)
		{
			x -= pages.GetPageAt(0, line).ClientRect.Left;
			width = x - gutter.GetWidth();
		}
		else
		{
			width = -ClientRect.Left + painter.FontWidth * scrolling.WindowOriginX + x - gutter.GetWidth();
		}
		int chars = 0;
		MeasureLine(line, 0, -1, width, out chars, exact: false);
		return Math.Max(chars, 0);
	}

	public Point ScreenToDisplay(int x, int y)
	{
		int num;
		int width;
		if (pages.PageType == PageType.PageLayout)
		{
			IEditPage pageAtPoint = pages.GetPageAtPoint(x, y);
			Rectangle clientRect = pageAtPoint.ClientRect;
			x = Math.Min(x, clientRect.Right);
			x -= clientRect.Left;
			y -= clientRect.Top;
			num = Math.Min(Math.Max(LinesInHeight(y), 0) + pageAtPoint.StartLine, pageAtPoint.EndLine);
			width = x - gutter.GetWidth();
		}
		else
		{
			Rectangle clientRect2 = ClientRect;
			num = scrolling.WindowOriginY + LinesInHeight(y - clientRect2.Top);
			width = -clientRect2.Left + painter.FontWidth * scrolling.WindowOriginX + x - gutter.GetWidth();
		}
		int chars = 0;
		MeasureLine(num, 0, -1, width, out chars, exact: false);
		return new Point(Math.Max(chars, 0), Math.Max(num, 0));
	}

	public Point ScreenToText(Point position)
	{
		bool lineEnd = false;
		return ScreenToText(position.X, position.Y, ref lineEnd);
	}

	public Point ScreenToText(int x, int y)
	{
		bool lineEnd = false;
		return ScreenToText(x, y, ref lineEnd);
	}

	public Point TextToScreen(Point position)
	{
		return TextToScreen(position.X, position.Y);
	}

	public Point TextToScreen(int x, int y)
	{
		return TextToScreen(x, y, displayLines.AtLineEnd);
	}

	public Point DisplayToScreen(int x, int y)
	{
		return DisplayToScreen(x, y, average: false);
	}

	public Point DisplayToScreen(int x, int y, bool average)
	{
		int num = (average ? (painter.FontWidth * x) : MeasureLine(y, 0, x));
		int num2;
		if (pages.PageType == PageType.PageLayout)
		{
			IEditPage pageAt = pages.GetPageAt(x, y);
			Rectangle clientRect = pageAt.ClientRect;
			if (num != int.MaxValue)
			{
				num += clientRect.Left + gutter.GetWidth();
			}
			num2 = (y - pageAt.StartLine) * painter.FontHeight;
			num2 += clientRect.Top;
		}
		else
		{
			Rectangle clientRect2 = ClientRect;
			num2 = (y - scrolling.WindowOriginY) * painter.FontHeight + clientRect2.Top;
			if (num != int.MaxValue)
			{
				num = num - painter.FontWidth * scrolling.WindowOriginX + gutter.GetWidth() + clientRect2.Left;
			}
		}
		return new Point(num, num2);
	}

	public void MakeVisible(Point position)
	{
		MakeVisible(position, centerLine: false);
	}

	public void MakeVisible(Point position, bool centerLine)
	{
		Outlining.EnsureExpanded(position);
		Point point = TextToScreen(position);
		if (pages.PageType == PageType.PageLayout)
		{
			Rectangle clientRect = ClientRect;
			if (point.X < clientRect.Left)
			{
				scrolling.WindowOriginX += point.X - clientRect.Left;
			}
			else
			{
				int width = GetCaretSize(position).Width;
				if (point.X > clientRect.Right - width)
				{
					scrolling.WindowOriginX += Math.Max(point.X - clientRect.Right + width, 1);
				}
			}
			if (point.Y < clientRect.Top)
			{
				if (centerLine)
				{
					scrolling.WindowOriginY += point.Y - clientRect.Height / 2;
				}
				else
				{
					scrolling.WindowOriginY += point.Y - clientRect.Top;
				}
			}
			else if (point.Y > clientRect.Bottom - painter.FontHeight)
			{
				if (centerLine)
				{
					scrolling.WindowOriginY += point.Y - (clientRect.Height - painter.FontHeight) / 2;
				}
				else
				{
					scrolling.WindowOriginY += point.Y - clientRect.Height + painter.FontHeight;
				}
			}
			return;
		}
		position = displayLines.PointToDisplayPoint(position);
		int num = position.X - scrolling.WindowOriginX;
		int num2 = position.Y - scrolling.WindowOriginY;
		if (num < 0)
		{
			scrolling.WindowOriginX = position.X - (((scrolling.Options & ScrollingOptions.UseScrollDelta) != 0) ? 1 : 0);
		}
		else
		{
			int width2 = GetCaretSize(Position).Width;
			int num3 = ClientRect.Right;
			if (point.X > num3 - width2)
			{
				if ((scrolling.Options & ScrollingOptions.UseScrollDelta) != 0)
				{
					num3 -= num3 / EditConsts.DefaultScrollDeltaRatio;
				}
				scrolling.WindowOriginX += Math.Max(CharsInWidth(point.X - num3 + width2, exact: false), 1);
			}
		}
		if (num2 < 0)
		{
			if (centerLine)
			{
				scrolling.WindowOriginY += LinesInHeight(point.Y - ClientHeight() / 2);
			}
			else
			{
				scrolling.WindowOriginY = position.Y;
			}
		}
		else if (num2 > LinesInHeight() - 1)
		{
			if (centerLine)
			{
				scrolling.WindowOriginY += LinesInHeight(point.Y - ClientHeight() / 2);
			}
			else
			{
				scrolling.WindowOriginY += num2 - LinesInHeight() + 1;
			}
		}
	}

	public void ShowScrollHint(int pos)
	{
		CodeCompletionHint.Lexer = null;
		if (Pages.PageType == PageType.PageLayout)
		{
			CodeCompletionHint.Provider = GetQuickInfo(string.Format(StringConsts.PageNofMstr, pages.GetPageIndexAtPoint(0, pos - scrolling.WindowOriginY) + 1, Pages.Count), useHtmlFormatting: true);
		}
		else
		{
			CodeCompletionHint.Provider = GetQuickInfo(string.Format(StringConsts.LineNofMstr, displayLines.DisplayPointToPoint(0, pos).Y + 1, Lines.Count), useHtmlFormatting: true);
		}
		CodeCompletionHint.StartPos = new Point(-1, -1);
		CodeCompletionHint.EndPos = new Point(-1, -1);
		Point position = ((Control)this).PointToScreen(new Point(((Control)this).Bounds.Width, 0));
		position.X -= CodeCompletionHint.Width;
		position.Y = Math.Min(Cursor.Position.Y + Painter.FontHeight, ((Control)this).PointToScreen(new Point(((Control)this).Bounds.Width, ((Control)this).Bounds.Height)).Y);
		CodeCompletionHint.PopupAt(position);
		((Control)this).Update();
	}

	public void HideScrollHint()
	{
		if (codeCompletionHint != null && codeCompletionHint.Visible)
		{
			codeCompletionHint.Close(accept: false);
		}
	}

	public int CharsInWidth()
	{
		return CharsInWidth(ClientWidth());
	}

	public int CharsInWidth(int width)
	{
		int fontWidth = painter.FontWidth;
		if (fontWidth == 0)
		{
			return 0;
		}
		return width / fontWidth;
	}

	public int CharsInWidth(int width, bool exact)
	{
		int fontWidth = painter.FontWidth;
		if (fontWidth == 0)
		{
			return 0;
		}
		int num = width / fontWidth;
		if (!exact && width % fontWidth != 0)
		{
			num++;
		}
		return num;
	}

	public int LinesInHeight()
	{
		return LinesInHeight(ClientHeight());
	}

	public int LinesInHeight(int height)
	{
		int fontHeight = painter.FontHeight;
		if (fontHeight == 0)
		{
			return 0;
		}
		return height / fontHeight;
	}

	public string GetTextToSearchAtCursor()
	{
		string text = Lines[Position.Y];
		if (Position.X > 0 && Position.X < text.Length && Lines.IsDelimiter(text, Position.X) && !Lines.IsDelimiter(text, Position.X - 1))
		{
			return Lines.GetTextAt(Position.X - 1, Position.Y);
		}
		return Lines.GetTextAt(Position);
	}

	public void GetHitTestAtTextPoint(Point point, ref HitTestInfo hitTestInfo)
	{
		GetHitTestAtTextPoint(point.X, point.Y, ref hitTestInfo);
	}

	public void GetHitTestAtTextPoint(int x, int y, ref HitTestInfo hitTestInfo)
	{
		if (y >= 0 && y < Lines.Count)
		{
			hitTestInfo.Item = Lines.GetItem(y);
			hitTestInfo.String = Lines[y];
			hitTestInfo.Line = y;
			hitTestInfo.Pos = x;
			if (x >= 0 && x < hitTestInfo.String.Length)
			{
				hitTestInfo.HitTest |= HitTest.Text;
				hitTestInfo.Style = (byte)hitTestInfo.Item.ColorData[x];
				hitTestInfo.Item.GetColorFlag(x, out hitTestInfo.ColorFlags);
				hitTestInfo.Word = Lines.GetTextAt(x, y);
				if (hyperText.IsUrlAtTextPoint(x, y, out var url, needUrl: true))
				{
					hitTestInfo.HitTest |= HitTest.HyperText;
					hitTestInfo.Url = url;
				}
			}
			else
			{
				hitTestInfo.HitTest |= HitTest.BeyondEol;
			}
		}
		else
		{
			hitTestInfo.HitTest |= HitTest.BeyondEof;
		}
		if (selection.IsPosInSelection(x, y))
		{
			hitTestInfo.HitTest |= HitTest.Selection;
		}
	}

	public void GetHitTest(Point point, ref HitTestInfo hitTestInfo)
	{
		GetHitTest(point.X, point.Y, ref hitTestInfo);
	}

	public void GetHitTest(int x, int y, ref HitTestInfo hitTestInfo)
	{
		hitTestInfo.InitHitTestInfo();
		Rectangle clientRect = ClientRect;
		if (x < clientRect.Left)
		{
			hitTestInfo.HitTest |= HitTest.Left;
		}
		if (x > clientRect.Right)
		{
			hitTestInfo.HitTest |= HitTest.Right;
		}
		if (y < clientRect.Top)
		{
			hitTestInfo.HitTest |= HitTest.Above;
		}
		if (y > clientRect.Bottom)
		{
			hitTestInfo.HitTest |= HitTest.Below;
		}
		if (pages.PageType != 0)
		{
			hitTestInfo.Page = Pages.GetPageIndexAtPoint(x, y);
			if (hitTestInfo.Page >= 0)
			{
				hitTestInfo.HitTest |= HitTest.Page;
				EditPage editPage = (EditPage)Pages[hitTestInfo.Page];
				if (editPage.WhiteSpaceTopRect.Contains(x, y) || editPage.WhiteSpaceBottomRect.Contains(x, y))
				{
					hitTestInfo.HitTest |= HitTest.PageWhiteSpace;
				}
				if (margin.Visible && margin.Contains(x, y))
				{
					hitTestInfo.HitTest |= HitTest.Margin;
				}
			}
		}
		else if (margin.Visible && margin.Contains(x, y))
		{
			hitTestInfo.HitTest |= HitTest.Margin;
		}
		if ((hitTestInfo.HitTest & HitTest.PageWhiteSpace) != 0)
		{
			return;
		}
		if (gutter.IsMouseOnOutlineArea(x, y))
		{
			hitTestInfo.HitTest |= HitTest.OutlineArea;
		}
		Point outPt = new Point(0, 0);
		if (gutter.IsMouseOnOutlineImage(x, y, ref outPt))
		{
			hitTestInfo.HitTest |= HitTest.OutlineImage;
		}
		if (gutter.IsMouseOnLineModificatorArea(x, y))
		{
			hitTestInfo.HitTest |= HitTest.LineModificator;
		}
		if (gutter.IsMouseOnGutterImage(x, y, out var imageIndex, out var bookMark))
		{
			hitTestInfo.HitTest |= HitTest.GutterImage;
			if (bookMark)
			{
				hitTestInfo.HitTest |= HitTest.BookMark;
			}
			hitTestInfo.GutterImage = imageIndex;
		}
		if (gutter.IsMouseOnLineNumberArea(x, y))
		{
			hitTestInfo.HitTest |= HitTest.LineNumber;
		}
		if (outlining.IsMouseOnOutlineButton(x, y, out var range))
		{
			hitTestInfo.HitTest |= HitTest.OutlineButton;
			hitTestInfo.OutlineRange = range;
		}
		if (gutter.IsMouseOnGutter(x, y, strictCheck: false))
		{
			hitTestInfo.HitTest |= HitTest.Gutter;
		}
		else if (ScreenToDisplay(x, y).Y >= displayLines.Count)
		{
			hitTestInfo.HitTest |= HitTest.BeyondEof;
		}
		else
		{
			GetHitTestAtTextPoint(ScreenToText(x, y), ref hitTestInfo);
		}
	}

	public virtual void ResetBorderStyle()
	{
		BorderStyle = EditBorderStyle.Fixed3D;
	}

	public virtual void ResetBorderColor()
	{
		BorderColor = Color.Empty;
	}

	public virtual void ResetAcceptTabs()
	{
		AcceptTabs = true;
	}

	public virtual void ResetAcceptReturns()
	{
		AcceptReturns = true;
	}

	public virtual bool ShouldSerializeBorderColor()
	{
		return borderColor != Color.Empty;
	}

	public DialogResult DisplayEditorSettingsDialog()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		return DisplayEditorSettingsDialog(EditorSettingsTab.None);
	}

	public DialogResult DisplayEditorSettingsDialog(EditorSettingsTab hiddenTabs)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Invalid comparison between Unknown and I4
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		syntaxSettings.LoadFromEdit(this);
		DlgSyntaxSettings dlgSyntaxSettings = new DlgSyntaxSettings(hiddenTabs);
		dlgSyntaxSettings.SyntaxSettings = syntaxSettings;
		DialogResult val = ((Form)dlgSyntaxSettings).ShowDialog();
		if ((int)val == 1)
		{
			syntaxSettings.Assign(dlgSyntaxSettings.SyntaxSettings);
			syntaxSettings.ApplyToEdit(this);
		}
		return val;
	}

	public DialogResult DisplayGotoLineDialog()
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Invalid comparison between Unknown and I4
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		int line = Position.Y;
		DialogResult val = (DialogResult)((GotoLineDialog != null) ? ((int)GotoLineDialog.Execute(this, Lines.Count, ref line)) : 0);
		if ((int)val == 1)
		{
			Source.BeginUpdate();
			try
			{
				Source.State |= NotifyState.CenterLine;
				MoveToLine(line);
			}
			finally
			{
				Source.EndUpdate();
			}
		}
		return val;
	}

	public DialogResult DisplaySearchDialog()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		if (SearchDialog == null)
		{
			return (DialogResult)0;
		}
		return SearchDialog.Execute(this, isModal: false, isReplace: false);
	}

	public DialogResult DisplayReplaceDialog()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		if (SearchDialog == null)
		{
			return (DialogResult)0;
		}
		return SearchDialog.Execute(this, isModal: false, isReplace: true);
	}

	private bool NeedIncrementalSearch(char key)
	{
		bool flag = inIncrementalSearch;
		if (flag)
		{
			IncrementalSearch(key.ToString(), deleteLast: false);
		}
		return flag;
	}

	private void CheckIncrementalSeacrh()
	{
		if (!incrSearchFlag)
		{
			FinishIncrementalSearch();
		}
	}

	protected string GetReplaceString(string replaceWith, Match match)
	{
		string result = replaceWith;
		if (match != null && match.Success)
		{
			result = match.Result(replaceWith);
		}
		return result;
	}

	protected virtual DialogResult DoReplace(string text, string replaceWith, SearchOptions options, Match match)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Invalid comparison between Unknown and I4
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		DialogResult val = (DialogResult)6;
		if ((SearchOptions.PromptOnReplace & options) != 0)
		{
			val = MessageBox.Show(string.Format(StringConsts.ConfirmReplaceText, text), StringConsts.ConfirmReplace, (MessageBoxButtons)3);
		}
		if ((int)val == 6)
		{
			searchUpdateCount++;
			Source.BeginUpdate(UpdateReason.Insert);
			try
			{
				Selection.Delete();
				Point position = Position;
				Source.Insert(GetReplaceString(replaceWith, match));
				if ((SearchOptions.BackwardSearch & options) != 0)
				{
					Position = position;
				}
			}
			finally
			{
				Source.EndUpdate();
				searchUpdateCount--;
			}
		}
		return val;
	}

	protected bool DoReplace(string str, string replaceWith, SearchOptions options, Regex expr, bool silent)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Invalid comparison between Unknown and I4
		if (!Source.Readonly && DoFind(str, options, expr, silent))
		{
			return (int)DoReplace(str, replaceWith, options, searchMatch) == 6;
		}
		return false;
	}

	protected int DoMarkAll(string str, SearchOptions options, Regex expr, bool clearPrevious)
	{
		FirstSearch = true;
		int num = 0;
		ArrayList arrayList = new ArrayList();
		Selection.BeginUpdate();
		Source.BeginUpdate(UpdateReason.Other);
		try
		{
			if (clearPrevious)
			{
				Source.BookMarks.ClearAllUnnumberedBookmarks();
			}
			while (DoFind(str, options, expr, silent: true))
			{
				bool flag = false;
				if (Source.BookMarks.GetBookMarks(new Point(0, searchPos.Y), new Point(int.MaxValue, searchPos.Y), arrayList) > 0)
				{
					foreach (IBookMark item in arrayList)
					{
						if (item.Index == int.MaxValue)
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					Source.BookMarks.SetBookMark(new Point(0, searchPos.Y), int.MaxValue);
				}
				num++;
			}
			return num;
		}
		finally
		{
			Source.EndUpdate();
			Selection.EndUpdate();
		}
	}

	protected int StorePositionWithUndo(Point position)
	{
		if ((Source.UndoOptions & UndoOptions.UndoNavigations) == 0)
		{
			((TextSource)Source).AddUndo(UndoOperation.NavigateEx, null);
		}
		return Source.StorePosition(position);
	}

	protected void RestorePositionWithUndo(int p)
	{
		Position = Source.RestorePosition(p);
		if ((Source.UndoOptions & UndoOptions.UndoNavigations) == 0)
		{
			((TextSource)Source).AddUndo(UndoOperation.NavigateEx, null);
		}
	}

	protected bool DoReplaceAll(string str, string replaceWith, SearchOptions options, Regex expr, out int count)
	{
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Invalid comparison between Unknown and I4
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Invalid comparison between Unknown and I4
		FirstSearch = true;
		count = 0;
		bool result = false;
		bool flag = (options & SearchOptions.SelectionOnly) != 0;
		bool flag2 = (options & SearchOptions.PromptOnReplace) == 0;
		searchUpdateCount++;
		if (flag2)
		{
			selection.InvalidateSelection();
			Source.BeginUpdate(UpdateReason.Other);
		}
		int p = StorePositionWithUndo(Position);
		try
		{
			while (DoFind(str, options, expr, flag2))
			{
				result = true;
				if (Readonly)
				{
					break;
				}
				DialogResult val = DoReplace(str, replaceWith, options & ~SearchOptions.CycledSearch, searchMatch);
				if ((int)val == 2)
				{
					break;
				}
				if ((int)val == 6)
				{
					count++;
				}
			}
		}
		finally
		{
			RestorePositionWithUndo(p);
			if (flag2)
			{
				Source.EndUpdate();
			}
			if (flag)
			{
				Selection.SetSelection(searchSelType, searchSelRect);
			}
			else
			{
				Selection.Clear();
			}
			searchUpdateCount--;
		}
		return result;
	}

	protected void InitSearchPos(SearchOptions options, bool update)
	{
		if ((options & SearchOptions.EntireScope) != 0)
		{
			if ((options & SearchOptions.BackwardSearch) != 0)
			{
				if ((options & SearchOptions.SelectionOnly) != 0 && !Selection.IsEmpty)
				{
					searchPos = new Point(Selection.SelectionRect.Right, Selection.SelectionRect.Bottom);
				}
				else
				{
					searchPos = new Point(Lines[Lines.Count - 1].Length, Lines.Count - 1);
				}
			}
			else if ((options & SearchOptions.SelectionOnly) != 0 && !Selection.IsEmpty)
			{
				searchPos = Selection.SelectionRect.Location;
			}
			else
			{
				searchPos = new Point(0, 0);
			}
		}
		else if (update && (options & SearchOptions.SelectionOnly) != 0 && !Selection.IsEmpty)
		{
			if ((options & SearchOptions.BackwardSearch) != 0)
			{
				searchPos = new Point(Selection.SelectionRect.Right, Selection.SelectionRect.Bottom);
			}
			else
			{
				searchPos = Selection.SelectionRect.Location;
			}
		}
		else
		{
			searchPos = Source.Position;
		}
	}

	protected bool PerformSearch(string str, SearchOptions options, Regex expression, ref Point searchPos)
	{
		bool flag;
		if ((options & SearchOptions.SelectionOnly) != 0 && !Selection.IsEmpty)
		{
			Point position = Selection.TextToSelectionPoint(searchPos);
			ISyntaxStrings syntaxStrings = new SyntaxStrings(null);
			syntaxStrings.Text = Selection.SelectedText;
			flag = syntaxStrings.Find(str, options, expression, ref position, out searchLen, out searchMatch);
			if (flag)
			{
				searchPos = Selection.SelectionToTextPoint(position);
			}
		}
		else if ((options & SearchOptions.SearchHiddenText) == 0)
		{
			Point position2 = displayLines.PointToDisplayPoint(searchPos, lineEnd: false);
			flag = displayLines.Find(str, options, expression, ref position2, out searchLen, out searchMatch);
			if (flag)
			{
				searchPos = displayLines.DisplayPointToPoint(position2);
				if (expression != null && (expression.Options & RegexOptions.Multiline) != 0)
				{
					Point point = SyntaxStrings.AbsolutePositionToTextPoint(displayLines, SyntaxStrings.TextPointToAbsolutePosition(displayLines, position2, displayLines.LineTerminator) + searchLen, displayLines.LineTerminator);
					point = displayLines.DisplayPointToPoint(point);
					searchLen = Lines.TextPointToAbsolutePosition(point) - Lines.TextPointToAbsolutePosition(searchPos);
				}
				else
				{
					searchLen = displayLines.DisplayPointToPoint(position2.X + searchLen, position2.Y, wrapEnd: false, rangeStart: false, tabEnd: true).X - searchPos.X;
				}
			}
		}
		else
		{
			flag = Find(str, searchOptions, expression, ref searchPos, out searchLen, out searchMatch);
		}
		return flag;
	}

	protected bool PerformCycledSearch(string str, SearchOptions options, Regex expression, ref Point searchPos)
	{
		bool flag = PerformSearch(str, options, expression, ref searchPos);
		if (flag && searchCycled)
		{
			flag = (((options & SearchOptions.BackwardSearch) == 0) ? (searchPos.Y < searchStartPos.Y || (searchPos.Y == searchStartPos.Y && searchPos.X < searchStartPos.X)) : (searchPos.Y > searchStartPos.Y || (searchPos.Y == searchStartPos.Y && searchPos.X + searchLen > searchStartPos.X)));
		}
		return flag;
	}

	protected bool InitCycledSearch(SearchOptions options)
	{
		if ((options & SearchOptions.CycledSearch) != 0)
		{
			InitSearchPos(options | SearchOptions.EntireScope, update: false);
			searchCycled = true;
			return true;
		}
		return false;
	}

	protected virtual bool DoFind(string str, SearchOptions options, Regex expression, bool silent)
	{
		bool flag = (options & SearchOptions.SelectionOnly) != 0 && (!Selection.IsEmpty || (!FirstSearch && searchSelType != SelectionType.None));
		if (flag && !FirstSearch)
		{
			searchUpdateCount++;
			try
			{
				Selection.SetSelection(searchSelType, searchSelRect);
			}
			finally
			{
				searchUpdateCount--;
			}
		}
		if (flag && Selection.IsEmpty)
		{
			return false;
		}
		searchMatch = null;
		if (FirstSearch)
		{
			searchText = str;
			searchOptions = options;
			searchExpression = expression;
			searchSelType = (flag ? Selection.SelectionType : SelectionType.None);
			searchSelRect = (flag ? Selection.SelectionRect : Rectangle.Empty);
			searchStartPos = Position;
			searchCycled = false;
			InitSearchPos(options, update: false);
		}
		else if (!selection.IsEmpty && selection.UpdateCount == 0)
		{
			if ((options & SearchOptions.BackwardSearch) == 0)
			{
				if (flag)
				{
					searchPos = Source.Position;
				}
				else
				{
					searchPos = new Point(selection.SelectionRect.Left, selection.SelectionRect.Top);
					if (searchPos.X < Lines.GetLength(searchPos.Y))
					{
						searchPos.X++;
					}
					else
					{
						searchPos.X = 0;
						searchPos.Y++;
					}
				}
			}
			else if (flag)
			{
				searchPos = Source.Position;
			}
			else
			{
				searchPos = new Point(selection.SelectionRect.Right, selection.SelectionRect.Bottom);
				if (searchPos.X > 0)
				{
					searchPos.X--;
				}
				else
				{
					searchPos.Y--;
					searchPos.X = Lines.GetLength(searchPos.Y);
				}
			}
		}
		else
		{
			searchPos = Source.Position;
		}
		bool flag2 = PerformCycledSearch(str, options, expression, ref searchPos);
		if (!flag2 && !searchCycled)
		{
			flag2 = InitCycledSearch(options) && PerformCycledSearch(str, options, expression, ref searchPos);
		}
		if (flag2)
		{
			TextFound(options, searchPos, searchLen, silent, expression != null && (expression.Options & RegexOptions.Multiline) != 0);
			FirstSearch = false;
		}
		else if (searchCycled)
		{
			searchStartPos = Position;
			searchCycled = false;
		}
		return flag2;
	}

	protected void TextFound(SearchOptions options, Point position, int len, bool silent, bool multiline)
	{
		searchUpdateCount++;
		try
		{
			Source.BeginUpdate(UpdateReason.Navigate);
			try
			{
				Source.State |= NotifyState.CenterLine;
				if ((SearchOptions.BackwardSearch & options) != 0)
				{
					Source.Position = position;
				}
				else
				{
					Source.Position = new Point(position.X + len, position.Y);
				}
			}
			finally
			{
				Source.EndUpdate();
			}
			if (selection.UpdateCount == 0)
			{
				selection.UpdateSelStart(position, checkIfEmpty: false);
				selection.SetSelection(SelectionType.Stream, new Rectangle(position.X, position.Y, len, 0));
				if (multiline)
				{
					selection.SelectionLength = len;
				}
			}
		}
		finally
		{
			searchUpdateCount--;
		}
		if (!silent && searchDialog != null && searchDialog.Visible)
		{
			Rectangle selectionRectInPixels = selection.GetSelectionRectInPixels();
			selectionRectInPixels.Inflate(Painter.FontWidth, 0);
			selectionRectInPixels.Location = ((Control)this).PointToScreen(selectionRectInPixels.Location);
			searchDialog.EnsureVisible(selectionRectInPixels);
		}
	}

	protected bool SelectionMatchesSearchText()
	{
		ISyntaxStrings syntaxStrings = new SyntaxStrings(null);
		string text = (syntaxStrings.Text = selection.SelectedText);
		Point position = (((searchOptions & SearchOptions.BackwardSearch) == 0) ? new Point(0, 0) : new Point(text.Length, 0));
		if (syntaxStrings.Find(searchText, searchOptions, searchExpression, ref position, out var len, out var _) && len == text.Length)
		{
			return position.X == 0;
		}
		return false;
	}

	public bool Find(string str, SearchOptions options, Regex expression, ref Point position, out int len, out Match match)
	{
		return Source.Lines.Find(str, options, expression, ref position, out len, out match);
	}

	public bool Find(string text, SearchOptions options, Regex expression, IList ranges)
	{
		Point position = new Point(0, 0);
		int len;
		Match match;
		if ((options & SearchOptions.SelectionOnly) != 0)
		{
			if (!Selection.IsEmpty)
			{
				if ((SearchOptions.BackwardSearch & options) != 0)
				{
					position.Offset(Selection.SelectionRect.Width, Selection.SelectionRect.Height);
				}
				ISyntaxStrings syntaxStrings = new SyntaxStrings(null);
				syntaxStrings.Text = Selection.SelectedText;
				Point point = position;
				while (syntaxStrings.Find(text, options, expression, ref position, out len, out match))
				{
					point = Selection.SelectionToTextPoint(position);
					ranges.Add(new Range(point.X, point.Y, point.X + len, point.Y));
					if ((SearchOptions.BackwardSearch & options) == 0)
					{
						position.X += len;
					}
				}
			}
		}
		else
		{
			while (Find(text, options, expression, ref position, out len, out match))
			{
				ranges.Add(new Range(position.X, position.Y, position.X + len, position.Y));
				if ((SearchOptions.BackwardSearch & options) == 0)
				{
					position.X += len;
				}
			}
		}
		return ranges.Count > 0;
	}

	public bool Find(string str)
	{
		return Find(str, SearchOptions.None, null);
	}

	public bool Find(string str, SearchOptions options)
	{
		return Find(str, options, null);
	}

	public bool Find(string str, SearchOptions options, Regex expression)
	{
		FirstSearch = true;
		return DoFind(str, options, expression, silent: false);
	}

	public bool FindNext()
	{
		if (CanFindNext())
		{
			return DoFind(searchText, searchOptions & ~SearchOptions.BackwardSearch, searchExpression, silent: false);
		}
		return false;
	}

	public bool FindPrevious()
	{
		if (CanFindPrevious())
		{
			return DoFind(searchText, searchOptions | SearchOptions.BackwardSearch, searchExpression, silent: false);
		}
		return false;
	}

	public bool FindNextSelected()
	{
		if (CanFindNextSelected())
		{
			return DoFind(selection.SelectedText, searchOptions & ~SearchOptions.BackwardSearch, searchExpression, silent: false);
		}
		return false;
	}

	public bool FindPreviousSelected()
	{
		if (CanFindPreviousSelected())
		{
			return DoFind(selection.SelectedText, searchOptions | SearchOptions.BackwardSearch, searchExpression, silent: false);
		}
		return false;
	}

	public virtual bool CanFindNext()
	{
		return !firstSearch;
	}

	public virtual bool CanFindPrevious()
	{
		return !firstSearch;
	}

	public virtual bool CanFindNextSelected()
	{
		if (!selection.IsEmpty)
		{
			return true;
		}
		if (GetTextToSearchAtCursor().Trim() != string.Empty)
		{
			FirstSearch = true;
			Point position = Position;
			selection.SelectWord();
			if (!selection.IsEmpty)
			{
				selection.BeginUpdate();
				try
				{
					Position = position;
				}
				finally
				{
					selection.EndUpdate();
				}
			}
		}
		return !selection.IsEmpty;
	}

	public virtual bool CanFindPreviousSelected()
	{
		if (!selection.IsEmpty)
		{
			return true;
		}
		if (GetTextToSearchAtCursor().Trim() != string.Empty)
		{
			FirstSearch = true;
			Point position = Position;
			selection.SelectWord();
			if (!selection.IsEmpty)
			{
				selection.BeginUpdate();
				try
				{
					Position = position;
				}
				finally
				{
					selection.EndUpdate();
				}
			}
		}
		return !selection.IsEmpty;
	}

	public bool Replace(string str, string replaceWith)
	{
		return Replace(str, replaceWith, SearchOptions.None, null);
	}

	public bool Replace(string str, string replaceWith, SearchOptions options)
	{
		return Replace(str, replaceWith, options, null);
	}

	public bool Replace(string str, string replaceWith, SearchOptions options, Regex expression)
	{
		FirstSearch = true;
		return DoReplace(str, replaceWith, options, expression, silent: false);
	}

	public bool ReplaceAll(string str, string replaceWith, out int count)
	{
		return ReplaceAll(str, replaceWith, SearchOptions.None, null, out count);
	}

	public bool ReplaceAll(string str, string replaceWith, SearchOptions options, out int count)
	{
		return ReplaceAll(str, replaceWith, options, null, out count);
	}

	public bool NeedReplaceCurrent(out Match match)
	{
		match = searchMatch;
		return NeedReplaceCurrent();
	}

	public bool NeedReplaceCurrent()
	{
		if (!firstSearch && !Selection.IsEmpty)
		{
			return SelectionMatchesSearchText();
		}
		return false;
	}

	public bool ReplaceCurrent(string replaceWith, SearchOptions options, Match match)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		bool flag = !Source.Readonly && NeedReplaceCurrent();
		if (flag)
		{
			DoReplace(searchText, replaceWith, options, match);
		}
		return flag;
	}

	public bool ReplaceAll(string str, string replaceWith, SearchOptions options, Regex expression, out int count)
	{
		return DoReplaceAll(str, replaceWith, options, expression, out count);
	}

	public int MarkAll(string str, bool clearPrevious)
	{
		return DoMarkAll(str, SearchOptions.None, null, clearPrevious);
	}

	public int MarkAll(string str, SearchOptions options, bool clearPrevious)
	{
		return DoMarkAll(str, options, null, clearPrevious);
	}

	public int MarkAll(string str, SearchOptions options, Regex expression, bool clearPrevious)
	{
		return DoMarkAll(str, options, expression, clearPrevious);
	}

	public void StartIncrementalSearch()
	{
		StartIncrementalSearch(backwardSearch: false);
	}

	public void StartIncrementalSearch(bool backwardSearch)
	{
		inIncrementalSearch = true;
		incrSearchPosition = Position;
		incrStartSearchPosition = Position;
		searchText = string.Empty;
		searchOptions |= SearchOptions.FindTextAtCursor;
		searchStartPos = Position;
		searchCycled = false;
		if (backwardSearch)
		{
			searchOptions |= SearchOptions.BackwardSearch;
		}
		else
		{
			searchOptions &= ~SearchOptions.BackwardSearch;
		}
		Source.BeginUpdate(UpdateReason.Other);
		try
		{
			Source.State |= NotifyState.IncrementalSearchChanged;
		}
		finally
		{
			Source.EndUpdate();
		}
		if (((Control)this).IsHandleCreated)
		{
			OSUtils.SendMessage(((Control)this).Handle, 32, IntPtr.Zero, IntPtr.Zero);
		}
	}

	public void FinishIncrementalSearch()
	{
		if (inIncrementalSearch)
		{
			inIncrementalSearch = false;
			Source.BeginUpdate(UpdateReason.Other);
			try
			{
				Source.State |= NotifyState.IncrementalSearchChanged;
			}
			finally
			{
				Source.EndUpdate();
			}
			if (((Control)this).IsHandleCreated)
			{
				OSUtils.SendMessage(((Control)this).Handle, 32, IntPtr.Zero, IntPtr.Zero);
			}
		}
	}

	public bool IncrementalSearch(string key, bool deleteLast)
	{
		bool flag = false;
		string text = searchText;
		incrSearchFlag = true;
		try
		{
			if (deleteLast)
			{
				searchText = searchText.Remove(searchText.Length - 1, 1);
			}
			else
			{
				searchText += key;
			}
			searchPos = incrStartSearchPosition;
			flag = PerformCycledSearch(searchText, searchOptions, searchExpression, ref searchPos);
			if (!flag && !searchCycled)
			{
				flag = InitCycledSearch(searchOptions);
				if (flag)
				{
					incrStartSearchPosition = searchPos;
					flag = PerformCycledSearch(searchText, searchOptions, searchExpression, ref searchPos);
				}
			}
			if (flag)
			{
				Point point = searchPos;
				int num = searchLen;
				Source.BeginUpdate(UpdateReason.Other);
				try
				{
					flag = true;
					Position = new Point(point.X + num, point.Y);
					Source.State |= NotifyState.IncrementalSearchChanged;
				}
				finally
				{
					Source.EndUpdate();
				}
				FirstSearch = false;
				selection.SetSelection(SelectionType.Stream, new Rectangle(point.X, point.Y, num, 0));
			}
			else if (deleteLast)
			{
				if (searchText == string.Empty)
				{
					selection.Clear();
				}
				Source.BeginUpdate(UpdateReason.Other);
				try
				{
					if (searchText == string.Empty)
					{
						MoveTo(incrSearchPosition);
					}
					else
					{
						selection.SetSelection(SelectionType.Stream, new Rectangle(Position.X, Position.Y, 0, 0));
					}
					Source.State |= NotifyState.IncrementalSearchChanged;
				}
				finally
				{
					Source.EndUpdate();
				}
			}
			else
			{
				searchText = text;
			}
		}
		finally
		{
			incrSearchFlag = false;
		}
		return flag;
	}

	public string GetTextAtCursor()
	{
		return Lines.GetTextAt(Position);
	}

	public bool CanSearchSelection(out string selectedText)
	{
		bool flag = !selection.IsEmpty;
		selectedText = (flag ? selection.SelectedText : string.Empty);
		return flag;
	}

	public void ShowNotFound(string caption)
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		if ((searchOptions & SearchOptions.SilentSearch) == 0)
		{
			MessageBox.Show(string.Format(((searchOptions & SearchOptions.CycledSearch) != 0 && !FirstSearch) ? StringConsts.SearchReachedStartPos : StringConsts.StringNotFound, searchText), caption);
		}
	}

	public bool ShouldSerializeSearchOptions()
	{
		return searchOptions != EditConsts.DefaultSearchOptions;
	}

	private void RescanLines(int firstLine, int lastLine)
	{
		UpdateWordWrap(firstLine, lastLine);
		scrolling.UpdateScroll(updateSize: false);
	}

	private void UpdateSeparator()
	{
		if (lineSeparator.NeedHighlight())
		{
			InvalidateLine(oldLine);
			oldLine = displayLines.PointToDisplayPoint(Position).Y;
			InvalidateLine(oldLine);
		}
	}

	private void UpdatePages(int firstLine)
	{
		if (Pages.PageType != 0)
		{
			Pages.GetPageAt(displayLines.PointToDisplayPoint(0, firstLine)).Update();
		}
	}

	private void InvalidateLine(int index)
	{
		if (index >= 0)
		{
			Point point = DisplayToScreen(0, index);
			int width = gutter.GetWidth();
			int right = ClientRect.Right;
			((Control)this).Invalidate(new Rectangle(width, point.Y - 1, right - width + 1, painter.FontHeight + 2));
		}
	}

	private void InvalidateWindow(int first, int last, bool invalidateGutter)
	{
		Point position = TextToScreen(new Point(0, first), lineEnd: false);
		Point position2;
		if (last == int.MaxValue)
		{
			position2 = new Point(ClientRect.Right, ClientRect.Bottom);
		}
		else
		{
			position2 = TextToScreen(new Point(int.MaxValue, last), lineEnd: true);
			position2.Y += painter.FontHeight;
		}
		if (Pages.PageType == PageType.PageLayout)
		{
			position.X = Pages.GetPageAtPoint(position).BoundsRect.Left;
			position2.X = Pages.GetPageAtPoint(position2).BoundsRect.Right;
		}
		else if (invalidateGutter && gutter.GetWidth() > 0)
		{
			position.X = ClientRect.Left;
			position2.X = ClientRect.Right;
		}
		position2.X = ClientRect.Right;
		if (invalidateGutter)
		{
			if (position.Y > 0)
			{
				position.Y--;
			}
			position2.Y++;
		}
		((Control)this).Invalidate(new Rectangle(position.X, position.Y, position2.X - position.X + 1, position2.Y - position.Y + 1));
	}

	private void InvalidateLines(int ch, int line)
	{
		if (!((Control)this).IsHandleCreated || (line == scrolling.WindowOriginY && ch == scrolling.WindowOriginX))
		{
			return;
		}
		if ((ch != scrolling.WindowOriginX && line != scrolling.WindowOriginY) || Selection.SelectionType != 0 || IsTransparent || lineSeparator.NeedHighlight())
		{
			((Control)this).Invalidate();
			return;
		}
		int num = 0;
		int num2 = 0;
		Rectangle clientRect = ClientRect;
		int width = clientRect.Width;
		int height = clientRect.Height;
		if (ch == scrolling.WindowOriginX)
		{
			num2 = ((pages.PageType != PageType.PageLayout) ? ((line - scrolling.WindowOriginY) * painter.FontHeight) : (line - scrolling.WindowOriginY));
		}
		else if (line == scrolling.WindowOriginY)
		{
			num = ((pages.PageType != PageType.PageLayout) ? ((ch - scrolling.WindowOriginX) * painter.FontWidth) : (ch - scrolling.WindowOriginX));
		}
		if (Math.Abs(num) < width / 2 && Math.Abs(num2) < height / 2)
		{
			int num3 = clientRect.Left + ((pages.PageType != PageType.PageLayout) ? gutter.GetWidth() : 0);
			Rectangle rect;
			Rectangle rectangle;
			if (num2 == 0)
			{
				if (num > 0)
				{
					rect = new Rectangle(num3, clientRect.Top, width - num, height);
					rectangle = new Rectangle(num3, clientRect.Top, num, height);
				}
				else
				{
					rect = new Rectangle(num3 - num, clientRect.Top, width + num, height);
					rectangle = new Rectangle(width + num, clientRect.Top, -num, height);
				}
			}
			else if (num2 > 0)
			{
				rect = new Rectangle(clientRect.Left, clientRect.Top, width, height - num2);
				rectangle = new Rectangle(clientRect.Left, clientRect.Top, width, num2 - clientRect.Top);
			}
			else
			{
				rect = new Rectangle(clientRect.Left, clientRect.Top - num2, width, height + num2);
				rectangle = new Rectangle(clientRect.Left, height + num2, width, -num2);
			}
			OSUtils.ScrollWindow(((Control)this).Handle, num, num2, rect);
			((Control)this).Invalidate(rectangle, false);
			((Control)this).Update();
		}
		else
		{
			((Control)this).Invalidate();
		}
	}

	private void EnsureLastLineParsed()
	{
		int y = ((pages.PageType != PageType.PageLayout) ? (scrolling.WindowOriginY + LinesInHeight() + 1) : pages.GetPageAtPoint(0, ClientHeight()).EndLine);
		y = displayLines.DisplayPointToPoint(0, y).Y;
		Source.ParseToString(y + EditConsts.DefaultParserDelta);
	}

	private bool CheckCursor(Point pt)
	{
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		Cursor val = null;
		HitTestInfo hitTestInfo = default(HitTestInfo);
		GetHitTest(pt, ref hitTestInfo);
		if ((hitTestInfo.HitTest & HitTest.Left) != 0 || (hitTestInfo.HitTest & HitTest.Right) != 0 || (hitTestInfo.HitTest & HitTest.Above) != 0 || (hitTestInfo.HitTest & HitTest.Below) != 0)
		{
			val = Cursors.Arrow;
		}
		else if ((hitTestInfo.HitTest & HitTest.Gutter) != 0)
		{
			val = ((((hitTestInfo.HitTest & HitTest.OutlineArea) == 0 || (hitTestInfo.HitTest & HitTest.OutlineImage) != 0) && ((hitTestInfo.HitTest & HitTest.LineNumber) == 0 || (gutter.Options & GutterOptions.PaintLinesOnGutter) != 0)) ? Cursors.Arrow : LeftArrowCursor);
		}
		else if ((hitTestInfo.HitTest & HitTest.Margin) != 0 && margin.AllowDrag && (Control.ModifierKeys & 0x20000) != 0)
		{
			val = Cursors.SizeWE;
		}
		else if ((hitTestInfo.HitTest & HitTest.HyperText) != 0 && (Control.ModifierKeys & 0x20000) != 0)
		{
			val = Cursors.Hand;
		}
		else if (InIncrementalSearch)
		{
			val = (((searchOptions & SearchOptions.BackwardSearch) == 0) ? IncrementalSearchCursor : ReverseIncrementalSearchCursor);
		}
		else if ((hitTestInfo.HitTest & HitTest.Selection) != 0)
		{
			val = Cursors.Arrow;
		}
		else if ((hitTestInfo.HitTest & HitTest.PageWhiteSpace) != 0)
		{
			val = ((!pages.DisplayWhiteSpace) ? ShowWhiteSpaceCursor : HideWhiteSpaceCursor);
		}
		if (val != (Cursor)null)
		{
			OSUtils.SetCursor(val.Handle);
		}
		return val != (Cursor)null;
	}

	private void PaintScrollRect(IPainter painter)
	{
		if (scrolling.HasVScrollBar && scrolling.HasHScrollBar)
		{
			Rectangle rect = new Rectangle(((Control)this).ClientRectangle.Right - ((Control)scrolling.VScrollBar).Width, ((Control)this).ClientRectangle.Bottom - ((Control)scrolling.HScrollBar).Height, ((Control)scrolling.VScrollBar).Width, ((Control)scrolling.HScrollBar).Height);
			Color backColor = Painter.BackColor;
			try
			{
				painter.BackColor = SystemColors.Control;
				painter.FillRectangle(rect);
				painter.ExcludeClipRect(rect.Left, rect.Top, rect.Width, rect.Height);
			}
			finally
			{
				painter.BackColor = backColor;
			}
		}
	}

	private void PaintRulerRect(IPainter painter)
	{
		if ((pages.Rulers & EditRulers.Horizonal) != 0 && (pages.Rulers & EditRulers.Vertical) != 0)
		{
			Rectangle rectangle = new Rectangle(0, 0, pages.HorzRuler.Left, pages.VertRuler.Top);
			Color backColor = Painter.BackColor;
			Color foreColor = Painter.ForeColor;
			try
			{
				painter.BackColor = ((EditPages)Pages).RulerBackColor;
				painter.FillRectangle(rectangle);
				Rectangle rect = rectangle;
				rect.Inflate(-4, -4);
				painter.DrawEdge(ref rect, (Border3DStyle)6, (Border3DSide)15);
				painter.BackColor = ((EditPages)Pages).RulerIndentBackColor;
				rect.Width -= 2;
				rect.Height -= 2;
				painter.DrawRectangle(rect);
				painter.ExcludeClipRect(rectangle.Left, rectangle.Top, rectangle.Width, rectangle.Height);
			}
			finally
			{
				painter.BackColor = backColor;
				painter.ForeColor = foreColor;
			}
		}
	}

	protected internal int ClientWidth()
	{
		return GetClientRect(excludeNonClient: true).Width - gutter.GetWidth();
	}

	protected internal int ClientHeight()
	{
		return GetClientRect(excludeNonClient: true).Height;
	}

	protected internal void PaintWindow(IPainter painter, int startLine, Rectangle rect, Point location, float scaleX, float scaleY, bool specialPaint, bool inPrinting)
	{
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		if (specialPaint)
		{
			painter.Transform(location.X, location.Y, scaleX, scaleY);
		}
		this.inPrinting = inPrinting;
		try
		{
			DrawInfo info = default(DrawInfo);
			info.Init();
			if (OnCustomDraw(painter, rect, DrawStage.Before, DrawState.Control, info))
			{
				return;
			}
			Rectangle clientRect = GetClientRect(excludeNonClient: true);
			Rectangle rectangle = new Rectangle(clientRect.Left, rect.Top, gutter.GetWidth(), rect.Bottom);
			Rectangle rectangle2 = new Rectangle(DisplayToScreen(margin.Position, startLine, average: true).X, rect.Top, 1, rect.Height);
			Point position = new Point(rectangle.Width, 0);
			if (pages.PageType == PageType.PageLayout)
			{
				rectangle2.Offset(-location.X, 0);
			}
			else
			{
				position.X -= painter.FontWidth * scrolling.WindowOriginX;
			}
			int num = 0;
			if (painter.FontHeight != 0 && pages.PageType != PageType.PageLayout)
			{
				num = (rect.Top - clientRect.Top) / painter.FontHeight;
				position.Y += num * painter.FontHeight;
			}
			num += startLine;
			bool flag = rectangle.Width > 0 && !Rectangle.Intersect(rect, rectangle).IsEmpty;
			bool flag2 = margin.Visible && !margin.IsDragging && !Rectangle.Intersect(rect, rectangle2).IsEmpty;
			IntPtr rgn = painter.SaveClip(((Control)this).ClientRectangle);
			painter.FontStyle = ((Control)this).Font.Style;
			if (flag)
			{
				rectangle.Offset(-clientRect.Left, -clientRect.Top);
				gutter.Paint(painter, rectangle, startLine);
				painter.ExcludeClipRect(rectangle.Left, rectangle.Top, rectangle.Width, rectangle.Height);
			}
			if (flag2)
			{
				rectangle2.Offset(-clientRect.Left, -clientRect.Top);
				margin.Paint(painter, rectangle2);
				painter.ExcludeClipRect(rectangle2.Left, rectangle2.Top, rectangle2.Width, rectangle2.Height);
			}
			try
			{
				if (specialPaint)
				{
					painter.IntersectClipRect(rect.Left - clientRect.Left, rect.Top - clientRect.Top, rect.Width, rect.Height);
				}
				painter.TextColor = ((Control)this).ForeColor;
				painter.BackColor = GetBackColor(readOnly: false);
				if (Source.NeedParse())
				{
					EnsureLastLineParsed();
				}
				syntaxPaint.PaintSyntax(painter, num, displayLines.GetCount(), position, rect, specialPaint);
				if (margin.ColumnsVisible)
				{
					int[] columnPositions = margin.ColumnPositions;
					foreach (int x in columnPositions)
					{
						rectangle2 = new Rectangle(DisplayToScreen(x, startLine, average: true).X, rect.Top, 1, rect.Height);
						if (rectangle2.Left > rect.Right)
						{
							break;
						}
						if (pages.PageType == PageType.PageLayout)
						{
							rectangle2.Offset(-location.X, 0);
						}
						rectangle2.Offset(-clientRect.Left, -clientRect.Top);
						margin.PaintColumn(painter, rectangle2);
					}
				}
			}
			finally
			{
				painter.RestoreClip(rgn);
			}
			info.Init();
			OnCustomDraw(painter, rect, DrawStage.After, DrawState.Control, info);
		}
		finally
		{
			this.inPrinting = false;
			if (specialPaint)
			{
				painter.EndTransform();
			}
		}
	}

	protected internal void CancelDragging()
	{
		dragMargin = false;
		pages.CancelDragging();
		margin.CancelDragging();
	}

	protected internal void PositionChanged(UpdateReason reason, int deltaX, int deltaY)
	{
		Point position = Position;
		displayLines.PositionChanged(reason, position.X, position.Y, deltaX, deltaY);
		selection.PositionChanged(Position.X, Position.Y, deltaX, deltaY);
		if (codeCompletionBox != null)
		{
			((CodeCompletionWindow)codeCompletionBox).PositionChanged(Position.X, Position.Y, deltaX, deltaY);
		}
		if (codeCompletionHint != null)
		{
			((CodeCompletionWindow)codeCompletionHint).PositionChanged(Position.X, Position.Y, deltaX, deltaY);
		}
	}

	protected void StartTripleClickTimer()
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		if (tripleClickTimer == null)
		{
			tripleClickTimer = new Timer();
			tripleClickTimer.Interval = SystemInformation.DoubleClickTime;
			tripleClickTimer.Tick += DoTripleClick;
		}
		tripleClickTimer.Enabled = true;
	}

	protected void StopTripleClickTimer()
	{
		if (tripleClickTimer != null)
		{
			tripleClickTimer.Stop();
		}
		lbuttonClicks = 0;
	}

	protected void DoTripleClick(object sender, EventArgs e)
	{
		StopTripleClickTimer();
	}

	protected bool IsSignificantMouseMove(Point pt1, Point pt2)
	{
		if (Math.Abs(pt1.X - pt2.X) <= 1)
		{
			return Math.Abs(pt1.Y - pt2.Y) > 1;
		}
		return true;
	}

	protected internal bool ShouldSerializeSourceProps()
	{
		return source == null;
	}

	protected internal void UpdateMonospaced()
	{
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		if (Lexer != null)
		{
			foreach (ILexStyle lexStyle in Lexer.Scheme.LexStyles)
			{
				painter.FontStyle = lexStyle.FontStyle;
			}
		}
		if (Braces.BracesOptions != 0)
		{
			painter.FontStyle = Braces.Style;
		}
	}

	protected override void OnPaint(PaintEventArgs pe)
	{
		if (pe.ClipRectangle.IsEmpty)
		{
			return;
		}
		painter.BeginPaint(pe.Graphics);
		try
		{
			PaintRulerRect(painter);
			PaintScrollRect(painter);
			Rectangle clientRect = GetClientRect(excludeNonClient: true);
			Rectangle rect = clientRect;
			rect.Intersect(pe.ClipRectangle);
			switch (pages.PageType)
			{
			case PageType.PageLayout:
				pages.Paint(painter, rect);
				break;
			case PageType.PageBreaks:
				PaintWindow(painter, scrolling.WindowOriginY, rect, clientRect.Location, 1f, 1f, pages.Rulers != EditRulers.None, inPrinting: false);
				pages.Paint(painter, rect);
				break;
			default:
				PaintWindow(painter, scrolling.WindowOriginY, rect, clientRect.Location, 1f, 1f, pages.Rulers != EditRulers.None, inPrinting: false);
				break;
			}
			if (Gutter.DrawLineBookmarks)
			{
				((EditSyntaxPaint)syntaxPaint).PaintLineBookMarks(painter, ClientRect);
			}
		}
		finally
		{
			painter.EndPaint();
		}
		((Control)this).OnPaint(pe);
	}

	protected override void OnPaintBackground(PaintEventArgs pe)
	{
		((Control)this).OnPaintBackground(pe);
		if (IsTransparent && this.PaintBackground != null)
		{
			this.PaintBackground.Invoke((object)this, pe);
		}
	}

	protected override void OnResize(EventArgs pe)
	{
		bool flag = gutter.LineNumbersChanged();
		((Control)this).OnResize(pe);
		if (!flag && WordWrap && GetWrapMargin() != WrapMargin && pages.PageType != PageType.PageLayout)
		{
			UpdateWordWrap();
		}
		if (IsTransparent)
		{
			((Control)this).Invalidate();
		}
		pages.UpdateRulers();
		scrolling.UpdateScroll(updateSize: true);
	}

	protected override void OnVisibleChanged(EventArgs e)
	{
		((Control)this).OnVisibleChanged(e);
		if (((Control)this).Visible)
		{
			scrolling.UpdateScroll(updateSize: true);
		}
	}

	protected override void OnForeColorChanged(EventArgs e)
	{
		((Control)this).OnForeColorChanged(e);
	}

	protected override void OnFontChanged(EventArgs e)
	{
		((Control)this).OnFontChanged(e);
		DoFontChanged();
	}

	protected void SyntaxChanged()
	{
		if (outlining.AllowOutlining && !((TextSource)Source).NeedOutlineText())
		{
			outlining.UnOutline();
		}
		DoFontChanged();
	}

	protected void DoFontChanged()
	{
		if (!gutter.LineNumbersChanged() || !WordWrap)
		{
			UpdateWordWrap();
		}
		painter.Clear();
		painter.Font = ((Control)this).Font;
		UpdateMonospaced();
		displayLines.MaxLineChanged();
		pages.UpdateRulers();
		UpdateCaretMode();
	}

	protected override void OnGotFocus(EventArgs e)
	{
		Source.ActiveEdit = this;
		if (!hideCaret)
		{
			CreateCaret();
			UpdateCaret();
		}
		selection.UpdateSelection();
		((Control)this).OnGotFocus(e);
	}

	protected override void OnLostFocus(EventArgs e)
	{
		if (!IsCodeCompletionWindowFocused())
		{
			if (!hideCaret)
			{
				DestroyCaret();
			}
			UpdateSeparator();
			selection.UpdateSelection();
			CancelDragging();
			((Control)this).OnLostFocus(e);
		}
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		((Control)this).OnMouseLeave(e);
		DisableCodeCompletionTimer();
		mouseRange = null;
		snippetRange = null;
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Invalid comparison between Unknown and I4
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Invalid comparison between Unknown and I4
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Invalid comparison between Unknown and I4
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Invalid comparison between Unknown and I4
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Invalid comparison between Unknown and I4
		//IL_030b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0315: Unknown result type (might be due to invalid IL or missing references)
		//IL_032d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0337: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d3: Invalid comparison between Unknown and I4
		//IL_0587: Unknown result type (might be due to invalid IL or missing references)
		//IL_0591: Invalid comparison between Unknown and I4
		//IL_03a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03af: Invalid comparison between Unknown and I4
		((Control)this).OnMouseDown(e);
		if (!GetClientRect(excludeNonClient: true).Contains(new Point(e.X, e.Y)))
		{
			return;
		}
		needStartDrag = false;
		if (((Control)this).CanFocus)
		{
			((Control)this).Focus();
			bool lineEnd = false;
			Point point = ScreenToText(e.X, e.Y, ref lineEnd);
			bool flag = (int)e.Button != 2097152 || (NavigateOptions.MoveOnRightButton & NavigateOptions) != 0;
			bool flag2 = (int)e.Button == 1048576 && (SelectionOptions.DisableSelection & selection.Options) == 0;
			bool flag3 = (int)e.Button == 1048576 && e.Clicks == 2 && e.X >= gutter.GetWidth();
			bool flag4 = gutter.IsMouseOnLineNumberArea(e.X, e.Y) || gutter.IsMouseOnOutlineArea(e.X, e.Y);
			if ((gutter.Options & GutterOptions.SelectLineOnClick) != 0)
			{
				flag4 = flag4 || gutter.IsMouseOnGutter(e.X, e.Y, strictCheck: true);
			}
			if ((SelectionOptions.SelectLineOnDblClick & selection.Options) == 0 && (SelectionOptions.SelectLineOnTripleClick & selection.Options) != 0 && (int)e.Button == 1048576 && flag2 && e.X >= gutter.GetWidth())
			{
				StartTripleClickTimer();
				lbuttonClicks += e.Clicks;
			}
			bool flag5 = selection.SelectionState == SelectionState.None;
			bool flag6 = flag5 && selection.IsPosInSelection(point) && (SelectionOptions.DisableDragging & selection.Options) == 0;
			Point outPt = point;
			bool flag7 = (int)e.Button == 1048576 && gutter.IsMouseOnOutlineImage(e.X, e.Y, ref outPt);
			if (IsMouseOnPageWhiteSpace(e.X, e.Y))
			{
				pages.DisplayWhiteSpace = !pages.DisplayWhiteSpace;
				return;
			}
			if (flag7)
			{
				if (outlining.IsExpanded(outPt.Y))
				{
					outlining.Collapse(outPt.Y);
					return;
				}
				if (outlining.IsCollapsed(outPt.Y))
				{
					outlining.Expand(outPt.Y);
					return;
				}
			}
			if (outlining.AllowOutlining && (outlining.OutlineOptions & OutlineOptions.DrawButtons) != 0 && outlining.IsMouseOnOutlineButton(e.X, e.Y, out var range))
			{
				if (flag3)
				{
					selection.Clear();
					range.Visible = true;
					return;
				}
				if ((int)e.Button == 1048576)
				{
					Position = point;
					selection.SelectionState = SelectionState.Select;
					Selection.SetSelection(SelectionType.Stream, range.StartPoint, range.EndPoint);
					return;
				}
			}
			if (dragMargin && (Control.ModifierKeys & 0x20000) != 0)
			{
				margin.IsDragging = true;
				return;
			}
			if (flag2 && !flag3 && (Control.ModifierKeys & 0x10000) != 0)
			{
				if (flag4)
				{
					scrolling.WindowOriginX = 0;
					selection.StartSelection();
					selection.SelectionState = SelectionState.SelectLine;
				}
				else
				{
					selection.SelectionState = SelectionState.Select;
				}
				selection.OnSelect(this, null);
			}
			else
			{
				if (flag)
				{
					displayLines.AtLineEnd = lineEnd;
					selection.BeginUpdate();
					BeginLineEndUpdate();
					try
					{
						if ((flag2 | ((int)e.Button == 2097152 && !selection.IsPosInSelection(point))) && !flag3 && flag5 && !flag6 && !flag4)
						{
							Selection.Clear();
						}
						Position = point;
					}
					finally
					{
						EndLineEndUpdate();
						selection.EndUpdate();
					}
				}
				if (flag2)
				{
					if (lbuttonClicks >= 4)
					{
						selection.SelectLine();
						StopTripleClickTimer();
						selection.SelectionState = SelectionState.SelectWord;
					}
					else if (flag3 && (selection.Options & SelectionOptions.DeselectOnDblClick) == 0)
					{
						if ((SelectionOptions.SelectLineOnDblClick & selection.Options) != 0)
						{
							selection.SelectLine();
						}
						else
						{
							selection.UpdateSelStart(checkIfEmpty: false);
							selection.SelectWord();
							selection.StartSelection();
						}
						selection.SelectionState = SelectionState.SelectWord;
					}
					else if (flag5)
					{
						if (flag4)
						{
							selection.UpdateSelStart(checkIfEmpty: false);
							scrolling.WindowOriginX = 0;
							selection.SetSelection(SelectionType.Stream, new Rectangle(0, Position.Y, 0, 1));
							selection.StartSelection();
							selection.SelectionState = SelectionState.SelectLine;
						}
						else
						{
							if (flag6)
							{
								selection.EndSelection();
								selection.SelectionState = SelectionState.Drag;
								needStartDrag = true;
								startDragPos = new Point(e.X, e.Y);
							}
							else
							{
								selection.Clear();
								selection.SelectionState = SelectionState.Select;
								if ((NavigateOptions & NavigateOptions.BeyondEof) == 0 || point.Y < Lines.Count)
								{
									selection.StartSelection();
								}
							}
							selection.UpdateSelStart(checkIfEmpty: false);
						}
					}
				}
				else if ((int)e.Button == 2097152 && selection.SelectionState != 0)
				{
					selection.EndSelection();
				}
			}
		}
		urlAtCursor = hyperText.IsUrlAtPoint(e.X, e.Y);
		if (gutter.IsMouseOnGutter(e.X, e.Y, strictCheck: true))
		{
			if (e.Clicks == 1)
			{
				gutter.OnClick(new EventArgs());
			}
			else
			{
				gutter.OnDoubleClick(new EventArgs());
			}
		}
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Invalid comparison between Unknown and I4
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Invalid comparison between Unknown and I4
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		((Control)this).OnMouseUp(e);
		if ((int)e.Button == 1048576)
		{
			switch (selection.SelectionState)
			{
			case SelectionState.None:
				selection.Clear();
				break;
			case SelectionState.Drag:
				if (needStartDrag)
				{
					selection.Clear();
				}
				selection.SelectionState = SelectionState.None;
				break;
			case SelectionState.Select:
			case SelectionState.SelectLine:
				selection.EndSelection();
				if (!selection.IsValidPos(Position))
				{
					selection.Clear();
				}
				break;
			case SelectionState.SelectWord:
				selection.SelectionState = SelectionState.None;
				selection.EndSelection();
				break;
			}
			needStartDrag = false;
			if (urlAtCursor && selection.SelectionState == SelectionState.None && (Control.ModifierKeys & 0x20000) != 0 && hyperText.IsUrlAtPoint(e.X, e.Y, out var url, needUrl: true))
			{
				hyperText.UrlJump(url);
				return;
			}
			if (mouseBookMark != null && selection.SelectionState == SelectionState.None && (Control.ModifierKeys & 0x20000) != 0)
			{
				string url2 = ((IBookMarkEx)mouseBookMark).Url;
				if (url2 != null && url2 != string.Empty)
				{
					hyperText.UrlJump(url2);
					return;
				}
			}
			if (margin.IsDragging)
			{
				margin.Position = ScreenToDisplay(e.X, e.Y).X;
				CancelDragging();
			}
		}
		if ((int)e.Button == 2097152 && useDefaultMenu && ((Control)this).ContextMenu == null)
		{
			PopupDefaultMenu(new Point(e.X, e.Y));
		}
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		((Control)this).OnMouseMove(e);
		if (needStartDrag && (e.Button & 0x100000) != 0 && IsSignificantMouseMove(new Point(e.X, e.Y), startDragPos))
		{
			StartDragging();
			return;
		}
		TextSource textSource = (TextSource)Source;
		bool lineEnd = false;
		Point point = ScreenToDisplay(e.X, e.Y);
		Point position = displayLines.DisplayPointToPoint(point.X, point.Y, wrapEnd: true, rangeStart: false, tabEnd: false, ref lineEnd);
		SnippetRange snippetRange = null;
		if (textSource.CodeSnippets.Count > 0 && textSource.CodeSnippets.FindSnippet(position, exact: false, out var index) && textSource.CodeSnippets.IsFirstSnippet(index))
		{
			snippetRange = textSource.CodeSnippets[index] as SnippetRange;
		}
		if (snippetRange != this.snippetRange)
		{
			if (codeCompletionHint != null)
			{
				codeCompletionHint.Close(accept: false);
			}
			DisableCodeCompletionTimer();
			this.snippetRange = snippetRange;
			if (this.snippetRange != null && this.snippetRange.Tooltip != null && this.snippetRange.Tooltip != string.Empty)
			{
				DoCodeToolTip(this.snippetRange.Tooltip, e.X, e.Y, useHtmlFormatting: false);
				return;
			}
		}
		if (snippetRange != null)
		{
			return;
		}
		if (outlining.AllowOutlining && (OutlineOptions.ShowHints & Outlining.OutlineOptions) != 0)
		{
			if (!outlining.IsMouseOnOutlineButton(e.X, e.Y, out var range))
			{
				range = null;
			}
			if (mouseRange != range)
			{
				if (codeCompletionHint != null)
				{
					codeCompletionHint.Close(accept: false);
				}
				DisableCodeCompletionTimer();
				mouseRange = range;
				if (mouseRange != null)
				{
					DoCodeToolTip(displayLines.GetOutlineHint(mouseRange), e.X, e.Y, useHtmlFormatting: false);
					return;
				}
			}
			if (range != null)
			{
				return;
			}
		}
		if (margin.AllowDrag && margin.Visible)
		{
			if (margin.IsDragging)
			{
				margin.DragTo(e.X, e.Y);
				return;
			}
			bool flag = margin.Contains(e.X, e.Y);
			if (dragMargin != flag)
			{
				if (margin.ShowHints)
				{
					if (codeCompletionHint != null)
					{
						codeCompletionHint.Close(accept: false);
					}
					DisableCodeCompletionTimer();
				}
				dragMargin = flag;
				if (dragMargin && margin.ShowHints)
				{
					DoCodeToolTip(StringConsts.DefaultDragMarginHint, e.X, e.Y, useHtmlFormatting: true);
				}
				return;
			}
			if (flag && margin.ShowHints)
			{
				return;
			}
		}
		if (syntaxErrorsHints)
		{
			if (!IsMouseOnSyntaxError(e.X, e.Y, out var err))
			{
				err = null;
			}
			if (mouseError != err)
			{
				if (codeCompletionHint != null)
				{
					codeCompletionHint.Close(accept: false);
				}
				DisableCodeCompletionTimer();
				mouseError = err;
				if (mouseError != null)
				{
					DoCodeToolTip(err.Description, e.X, e.Y, useHtmlFormatting: true);
					return;
				}
			}
			if (err != null)
			{
				return;
			}
		}
		if ((gutter.Options & GutterOptions.PaintBookMarks) != 0)
		{
			IBookMark bookMark = null;
			if (gutter.IsMouseOnGutterImage(e.X, e.Y, out var imageIndex, out var bookMark2) && bookMark2 && bookMark2)
			{
				int num = Source.BookMarks.FindBookMark((imageIndex == gutter.BookMarkImageIndex) ? int.MaxValue : imageIndex, position.Y);
				if (num >= 0 && Source.BookMarks[num] is IBookMarkEx)
				{
					bookMark = Source.BookMarks[num];
				}
			}
			if (bookMark != mouseBookMark)
			{
				mouseBookMark = bookMark;
				mouseBookMarkPt = new Point(e.X, e.Y);
				if (gutter.ShowBookmarkHints)
				{
					if (codeCompletionHint != null)
					{
						codeCompletionHint.Close(accept: false);
					}
					DisableCodeCompletionTimer();
					if (bookMark != null)
					{
						string text = ((IBookMarkEx)bookMark).Description;
						string url = ((IBookMarkEx)bookMark).Url;
						if (url != null && url != string.Empty)
						{
							text = text + ((text != null && text != string.Empty) ? "\r\n" : string.Empty) + string.Format(StringConsts.DefaultHyperTextHint, url);
						}
						if (text != string.Empty)
						{
							DoCodeToolTip(text, e.X, e.Y, useHtmlFormatting: true);
						}
					}
				}
			}
			if (bookMark != null && gutter.ShowBookmarkHints)
			{
				return;
			}
		}
		if (hyperText.HighlightUrls && hyperText.ShowHints)
		{
			string url2 = string.Empty;
			if (!hyperText.IsUrlAtPoint(e.X, e.Y, out url2, needUrl: true))
			{
				url2 = string.Empty;
			}
			if (url2 != mouseUrl)
			{
				if (codeCompletionHint != null)
				{
					codeCompletionHint.Close(accept: false);
				}
				DisableCodeCompletionTimer();
				mouseUrl = url2;
				if (mouseUrl != string.Empty && mouseUrl != null)
				{
					mouseUrlPoint = point;
					DoCodeToolTip(string.Format(StringConsts.DefaultHyperTextHint, mouseUrl), e.X, e.Y, useHtmlFormatting: true);
				}
			}
			if (mouseUrl != string.Empty && mouseUrl != null)
			{
				return;
			}
		}
		if (!((TextSource)Source).NeedQuickInfoTips())
		{
			return;
		}
		Point point2 = new Point(-1, -1);
		string text2 = Lines[position.Y];
		if (Lines.GetWord(text2, position.X, out var left, out var right) && text2.Substring(left, right - left + 1).Trim() != string.Empty)
		{
			point2 = new Point(left + 1, position.Y);
		}
		if (!infoTipPos.Equals((object?)point2))
		{
			if (codeCompletionHint != null)
			{
				codeCompletionHint.Close(accept: false);
			}
			DisableCodeCompletionTimer();
			infoTipPos = point2;
			if (infoTipPos.Y >= 0 && infoTipPos.X >= 0)
			{
				QuickInfo(codeCompletionArgs, infoTipPos, needReparse: false);
				OnNeedCompletion(codeCompletionArgs);
			}
		}
	}

	protected internal virtual void DoMouseWheel(int delta)
	{
		delta = -delta / 120;
		if (delta != 0)
		{
			if (pages.PageType == PageType.PageLayout)
			{
				scrolling.WindowOriginY += delta * SystemInformation.MouseWheelScrollLines * painter.FontHeight;
			}
			else
			{
				scrolling.WindowOriginY += delta * SystemInformation.MouseWheelScrollLines;
			}
		}
	}

	protected override void OnMouseWheel(MouseEventArgs e)
	{
		((Control)this).OnMouseWheel(e);
		DoMouseWheel(e.Delta);
	}

	protected override void OnDragLeave(EventArgs e)
	{
		((Control)this).OnDragLeave(e);
		HideDragCaret();
	}

	protected override void OnDragEnter(DragEventArgs drgevent)
	{
		((Control)this).OnDragEnter(drgevent);
		DisplayDragCaret();
	}

	protected override void OnDragOver(DragEventArgs drgevent)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		drgevent.Effect = (DragDropEffects)0;
		((Control)this).OnDragOver(drgevent);
		DisplayDragCaret();
		if ((int)drgevent.Effect != 0)
		{
			return;
		}
		if (!Source.Readonly && drgevent.Data != null && (drgevent.Data.GetDataPresent(DataFormats.Text) || drgevent.Data.GetDataPresent(DataFormats.UnicodeText)))
		{
			if ((Control.ModifierKeys & 0x20000) != 0)
			{
				drgevent.Effect = (DragDropEffects)1;
			}
			else
			{
				drgevent.Effect = (DragDropEffects)2;
			}
			MoveCaretOnDrag();
		}
		else
		{
			drgevent.Effect = (DragDropEffects)0;
		}
	}

	protected void QueryEndDrag(object sender, QueryContinueDragEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Invalid comparison between Unknown and I4
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Invalid comparison between Unknown and I4
		if ((int)e.Action == 2 || (int)e.Action == 1 || e.EscapePressed)
		{
			needStartDrag = false;
		}
		if ((int)e.Action == 2 || e.EscapePressed)
		{
			Selection.BeginUpdate();
			try
			{
				HideDragCaret();
				Source.Position = saveDragPos;
			}
			finally
			{
				Selection.EndUpdate();
			}
		}
	}

	protected override void OnDragDrop(DragEventArgs drgevent)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Invalid comparison between Unknown and I4
		((Control)this).OnDragDrop(drgevent);
		HideDragCaret();
		if ((int)drgevent.Effect == 0)
		{
			return;
		}
		needStartDrag = false;
		if (selection.SelectionState == SelectionState.Drag)
		{
			if (!selection.Move(Position, (Control.ModifierKeys & 0x20000) == 0))
			{
				selection.Clear();
			}
			selection.SelectionState = SelectionState.None;
			drgevent.Effect = (DragDropEffects)0;
			return;
		}
		object data = drgevent.Data.GetData(DataFormats.UnicodeText);
		if (data == null)
		{
			data = drgevent.Data.GetData(DataFormats.Text);
		}
		if (data != null)
		{
			if ((selection.Options & SelectionOptions.ClearOnDrag) != 0)
			{
				selection.Delete();
			}
			else
			{
				selection.Clear();
			}
			selection.SelectedText = (string)data;
		}
	}

	protected void OnSourceStateChanged(NotifyState state, int first, int last)
	{
		if (this.SourceStateChanged != null)
		{
			notifyEventArgs.FirstChanged = first;
			notifyEventArgs.LastChanged = last;
			notifyEventArgs.State = state;
			notifyEventArgs.Update = false;
			this.SourceStateChanged(this, notifyEventArgs);
		}
	}

	protected void OnModifiedChanged()
	{
		if (this.ModifiedChanged != null)
		{
			this.ModifiedChanged(this, EventArgs.Empty);
		}
	}

	protected override void CreateHandle()
	{
		((Control)this).CreateHandle();
		scrolling.SetFlatBars((Scrolling.Options & ScrollingOptions.FlatScrollbars) != 0);
		scrolling.UpdateScroll(updateSize: true);
		pages.UpdateRulers();
	}

	protected bool NeedResizeRedraw()
	{
		if (!scrolling.HasVScrollBar || !scrolling.HasHScrollBar || (scrolling.Options & ScrollingOptions.SystemScrollbars) != 0)
		{
			if ((selection.Options & SelectionOptions.DrawBorder) != 0 && (selection.Options & SelectionOptions.SelectBeyondEol) != 0)
			{
				return selection.BorderColor != Color.Empty;
			}
			return false;
		}
		return true;
	}

	protected internal void CheckResizeRedraw()
	{
		((Control)this).SetStyle((ControlStyles)16, NeedResizeRedraw());
	}

	protected override void WndProc(ref Message m)
	{
		//IL_0257: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Expected O, but got Unknown
		switch (((Message)(ref m)).Msg)
		{
		case 768:
			if (selection.CanCut())
			{
				selection.Cut();
			}
			break;
		case 769:
			if (selection.CanCopy())
			{
				selection.Copy();
			}
			break;
		case 770:
			if (selection.CanPaste())
			{
				selection.Paste();
			}
			break;
		case 771:
			if (!selection.IsEmpty)
			{
				selection.Delete();
			}
			break;
		case 198:
			if (Source.CanUndo())
			{
				((Message)(ref m)).Result = (IntPtr)1;
			}
			break;
		case 772:
			if (Source.CanUndo())
			{
				Source.Undo();
				((Message)(ref m)).Result = (IntPtr)1;
			}
			break;
		case 276:
			if ((scrolling.Options & ScrollingOptions.SystemScrollbars) != 0)
			{
				((Message)(ref m)).Result = (IntPtr)1;
				scrolling.DoSystemScroll(OSUtils.LoWord(((Message)(ref m)).WParam), vert: false);
				return;
			}
			break;
		case 277:
			if ((scrolling.Options & ScrollingOptions.SystemScrollbars) != 0)
			{
				((Message)(ref m)).Result = (IntPtr)1;
				scrolling.DoSystemScroll(OSUtils.LoWord(((Message)(ref m)).WParam), vert: true);
				return;
			}
			break;
		case 32:
			if (CheckCursor(((Control)this).PointToClient(Control.MousePosition)))
			{
				((Message)(ref m)).Result = (IntPtr)1;
				return;
			}
			break;
		case 133:
		{
			((Control)this).WndProc(ref m);
			if (BorderStyle != EditBorderStyle.System || XPThemes.CurrentTheme == XPThemeName.None || !(BorderColor != Color.Empty))
			{
				return;
			}
			IntPtr windowDC = OSUtils.GetWindowDC(((Control)this).Handle);
			try
			{
				if (BorderColor != Color.Empty)
				{
					Graphics val = Graphics.FromHdc(windowDC);
					Pen val2 = new Pen(BorderColor);
					try
					{
						val.DrawRectangle(val2, 0, 0, ((Control)this).Width - 1, ((Control)this).Height - 1);
						return;
					}
					finally
					{
						val2.Dispose();
						val.Dispose();
					}
				}
				OSUtils.ExcludeClipRect(windowDC, 2, 2, ((Control)this).Width - 2, ((Control)this).Height - 2);
				XPThemes.DrawEditBorder(windowDC, new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height));
				return;
			}
			finally
			{
				OSUtils.ReleaseDC(((Control)this).Handle, windowDC);
			}
		}
		case 513:
			((Control)this).Capture = true;
			break;
		case 514:
			((Control)this).Capture = false;
			break;
		}
		((Control)this).WndProc(ref m);
	}

	protected bool OnNeedCompletion(CodeCompletionArgs e)
	{
		DisableCodeCompletionTimer();
		if (this.NeedCodeCompletion != null)
		{
			this.NeedCodeCompletion(this, e);
		}
		if (e.CompletionType == CodeCompletionType.CompleteWord && e.NeedShow && e.SelIndex >= 0 && e.Provider != null && e.SelIndex < e.Provider.Count)
		{
			e.Handled = true;
			InsertTextFromProvider(e.Provider, e.Provider.GetText(e.SelIndex), e.StartPosition, e.EndPosition, e.Provider.UseIndent);
		}
		if (!e.Handled && e.NeedShow && e.Provider != null)
		{
			DoCodeCompletion();
		}
		return e.Handled;
	}

	protected internal bool DoProcessKeyMessage(ref Message msg)
	{
		return ((Control)this).ProcessKeyMessage(ref msg);
	}

	protected internal void InternalProcessKey(char ch)
	{
		if (IsValidText(Position) && !ProcessCodeCompletion(ch) && !ProcessSmartFormat(ch))
		{
			ProcessAutoIndent(ch);
		}
	}

	protected override bool IsInputChar(char charCode)
	{
		return charCode != '\t';
	}

	protected override bool IsInputKey(Keys keyData)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Invalid comparison between Unknown and I4
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		Keys val = (Keys)(keyData & 0xFFFF);
		if ((int)val == 13)
		{
			return AcceptReturns;
		}
		if ((int)val == 9)
		{
			return AcceptTabs;
		}
		if (Array.IndexOf(EditConsts.NavKeys, val) >= 0)
		{
			return true;
		}
		return ((Control)this).IsInputKey(keyData);
	}

	public bool ProcessKey(Keys keyData)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		if (keyList.ExecuteKey(keyData, ref keyState))
		{
			return true;
		}
		if ((keyData & 0xFFEE) != 0)
		{
			keyState = 0;
		}
		return false;
	}

	protected override void OnKeyUp(KeyEventArgs e)
	{
		CheckAutoCorrect();
		((Control)this).OnKeyUp(e);
		keyProcessed = false;
	}

	protected override void OnKeyDown(KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Invalid comparison between Unknown and I4
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Invalid comparison between Unknown and I4
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		if ((int)e.KeyCode == 27)
		{
			CancelDragging();
		}
		keyProcessed = false;
		((Control)this).OnKeyDown(e);
		if (e.Handled)
		{
			return;
		}
		Keys keyData = e.KeyData;
		if ((int)keyData != 9)
		{
			if ((int)keyData == 13)
			{
				lastKey = '\r';
			}
		}
		else
		{
			lastKey = '\t';
		}
		if (ProcessKey(e.KeyData))
		{
			keyProcessed = true;
			e.Handled = true;
		}
	}

	protected override void OnKeyPress(KeyPressEventArgs e)
	{
		if (keyProcessed)
		{
			keyProcessed = false;
			e.Handled = true;
			return;
		}
		((Control)this).OnKeyPress(e);
		if (!e.Handled && !NeedIncrementalSearch(e.KeyChar) && keyState == 0 && e.KeyChar >= ' ')
		{
			lastKey = e.KeyChar;
			ProcessKeyPress(e.KeyChar);
			InternalProcessKey(e.KeyChar);
			e.Handled = true;
		}
	}

	protected void DoCodeToolTip(string s, int x, int y, bool useHtmlFormatting)
	{
		codeCompletionArgs.CompletionType = CodeCompletionType.QuickInfo;
		codeCompletionArgs.Interval = SyntaxConsts.DefaultHintDelay;
		codeCompletionArgs.NeedShow = true;
		codeCompletionArgs.ToolTip = true;
		codeCompletionArgs.Provider = GetQuickInfo(s, useHtmlFormatting);
		codeCompletionArgs.DisplayPosition = ScreenToText(x, y);
		DoCodeCompletion();
	}

	protected void DoCodeCompletion(ISyntaxParser parser, string text, Point position, CodeCompletionArgs e)
	{
		parser.Strings = Lines;
		parser.CodeCompletion(text, position, e);
	}

	protected void DoCodeCompletion()
	{
		DisableCodeCompletionTimer();
		if (codeCompletionArgs.Interval == 0)
		{
			OnCodeCompletion(this, EventArgs.Empty);
			return;
		}
		CodeCompletionTimer.Interval = codeCompletionArgs.Interval;
		CodeCompletionTimer.Enabled = true;
	}

	protected void OnCodeCompletion(object source, EventArgs e)
	{
		DisableCodeCompletionTimer();
		if (codeCompletionArgs.Provider != null)
		{
			if (codeCompletionArgs.CompletionType == CodeCompletionType.QuickInfo)
			{
				if (snippetRange != null)
				{
					Point pt = ((Control)this).PointToScreen(TextToScreen(snippetRange.StartPoint, lineEnd: false));
					pt.Y += painter.FontHeight + EditConsts.DefaultHintOffsetY;
					pt.X = Cursor.Position.X + EditConsts.DefaultHintOffsetX;
					ShowCodeCompletionHint(codeCompletionArgs.Provider, pt, codeCompletionArgs.StartPosition, codeCompletionArgs.EndPosition, codeCompletionArgs.DisplayPosition, keepActive: true, acceptOnTab: false, null);
					return;
				}
				if (mouseRange != null)
				{
					Point pt2 = ((Control)this).PointToScreen(TextToScreen(mouseRange.StartPoint, lineEnd: false));
					pt2.Y += painter.FontHeight + EditConsts.DefaultHintOffsetY;
					pt2.X = Cursor.Position.X + EditConsts.DefaultHintOffsetX;
					ShowCodeCompletionHint(codeCompletionArgs.Provider, pt2, codeCompletionArgs.StartPosition, codeCompletionArgs.EndPosition, codeCompletionArgs.DisplayPosition, keepActive: true, acceptOnTab: false, Lexer);
					return;
				}
				if (mouseUrl != string.Empty && mouseUrl != null)
				{
					Point pt3 = ((Control)this).PointToScreen(DisplayToScreen(mouseUrlPoint.X, mouseUrlPoint.Y));
					pt3.Y += painter.FontHeight + EditConsts.DefaultHintOffsetY;
					pt3.X = Cursor.Position.X + EditConsts.DefaultHintOffsetX;
					ShowCodeCompletionHint(codeCompletionArgs.Provider, pt3, codeCompletionArgs.StartPosition, codeCompletionArgs.EndPosition, codeCompletionArgs.DisplayPosition, keepActive: true, acceptOnTab: false, null);
					return;
				}
				if (mouseError != null)
				{
					Point pt4 = ((Control)this).PointToScreen(TextToScreen(mouseError.Position, lineEnd: false));
					pt4.Y += painter.FontHeight + EditConsts.DefaultHintOffsetY;
					pt4.X = Cursor.Position.X + EditConsts.DefaultHintOffsetX;
					ShowCodeCompletionHint(codeCompletionArgs.Provider, pt4, codeCompletionArgs.StartPosition, codeCompletionArgs.EndPosition, codeCompletionArgs.DisplayPosition, keepActive: true, acceptOnTab: false, null);
					return;
				}
				if (mouseBookMark != null)
				{
					Point pt5 = ((Control)this).PointToScreen(mouseBookMarkPt);
					pt5.Y += painter.FontHeight + EditConsts.DefaultHintOffsetY;
					pt5.X = Cursor.Position.X + EditConsts.DefaultHintOffsetX;
					ShowCodeCompletionHint(codeCompletionArgs.Provider, pt5, codeCompletionArgs.StartPosition, codeCompletionArgs.EndPosition, codeCompletionArgs.DisplayPosition, keepActive: true, acceptOnTab: false, null);
					return;
				}
				if (dragMargin)
				{
					if (!margin.IsDragging)
					{
						Point point = ((Control)this).PointToClient(Control.MousePosition);
						point = ((Control)this).PointToScreen(DisplayToScreen(margin.Position, ScreenToDisplay(point.X, point.Y).Y));
						point.Y = Control.MousePosition.Y + EditConsts.DefaultHintOffsetY;
						point.X += EditConsts.DefaultHintOffsetX;
						ShowCodeCompletionHint(codeCompletionArgs.Provider, point, codeCompletionArgs.StartPosition, codeCompletionArgs.EndPosition, codeCompletionArgs.DisplayPosition, keepActive: true, acceptOnTab: false, null);
					}
					return;
				}
			}
			if (codeCompletionArgs.ToolTip)
			{
				ShowCodeCompletionHint(codeCompletionArgs.Provider);
			}
			else
			{
				ShowCodeCompletionBox(codeCompletionArgs.Provider);
			}
		}
		else if (codeCompletionArgs.KeyChar != 0)
		{
			if (((TextSource)Source).NeedCodeCompletion())
			{
				ISyntaxParser parser = Lexer as ISyntaxParser;
				DoCodeCompletion(parser, RemovePlainText(Position.Y), Position, codeCompletionArgs);
			}
			OnNeedCompletion(codeCompletionArgs);
		}
	}

	protected internal Rectangle GetClientRect(bool excludeNonClient)
	{
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		if (!excludeNonClient || pages.PageType != PageType.PageLayout)
		{
			if ((pages.Rulers & EditRulers.Horizonal) != 0)
			{
				int height = pages.HorzRuler.Height;
				clientRectangle.Y += height;
				clientRectangle.Height -= height;
			}
			if ((pages.Rulers & EditRulers.Vertical) != 0)
			{
				int width = pages.VertRuler.Width;
				clientRectangle.X += width;
				clientRectangle.Width -= width;
			}
		}
		if (scrolling.HasVScrollBar)
		{
			clientRectangle.Width -= ((Control)scrolling.VScrollBar).Width;
		}
		if (scrolling.HasHScrollBar)
		{
			clientRectangle.Height -= ((Control)scrolling.HScrollBar).Height;
		}
		return clientRectangle;
	}

	protected void StartDragging()
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Invalid comparison between Unknown and I4
		saveDragPos = Position;
		if (!selection.IsEmpty)
		{
			DragDropEffects val = ((Control)this).DoDragDrop((object)selection.SelectedText, (DragDropEffects)(-2147483645));
			if ((int)val == 2)
			{
				selection.Delete();
			}
			needStartDrag = false;
			selection.SelectionState = SelectionState.None;
		}
	}

	protected void DoUndo(object sender, EventArgs e)
	{
		if (Source.CanUndo())
		{
			Source.Undo();
		}
	}

	protected void DoRedo(object sender, EventArgs e)
	{
		if (Source.CanRedo())
		{
			Source.Redo();
		}
	}

	protected void DoCut(object sender, EventArgs e)
	{
		if (Selection.CanCut())
		{
			Selection.Cut();
		}
	}

	protected void DoCopy(object sender, EventArgs e)
	{
		if (Selection.CanCopy())
		{
			Selection.Copy();
		}
	}

	protected void DoPaste(object sender, EventArgs e)
	{
		if (Selection.CanPaste())
		{
			Selection.Paste();
		}
	}

	protected void DoDelete(object sender, EventArgs e)
	{
		Selection.DeleteRight();
	}

	protected void DoSelectAll(object sender, EventArgs e)
	{
		Selection.SelectAll();
	}

	protected virtual void InitDefaultMenu()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Expected O, but got Unknown
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Expected O, but got Unknown
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Expected O, but got Unknown
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Expected O, but got Unknown
		defaultMenu = new ContextMenuStrip();
		miUndo = new ToolStripMenuItem(StringConsts.MenuUndoCaption);
		((ToolStripItem)miUndo).Click += DoUndo;
		miRedo = new ToolStripMenuItem(StringConsts.MenuRedoCaption);
		((ToolStripItem)miRedo).Click += DoRedo;
		miCut = new ToolStripMenuItem(StringConsts.MenuCutCaption);
		((ToolStripItem)miCut).Click += DoCut;
		miCopy = new ToolStripMenuItem(StringConsts.MenuCopyCaption);
		((ToolStripItem)miCopy).Click += DoCopy;
		miPaste = new ToolStripMenuItem(StringConsts.MenuPasteCaption);
		((ToolStripItem)miPaste).Click += DoPaste;
		miDelete = new ToolStripMenuItem(StringConsts.MenuDeleteCaption);
		((ToolStripItem)miDelete).Click += DoDelete;
		miSelectAll = new ToolStripMenuItem(StringConsts.MenuSelectAllCaption);
		((ToolStripItem)miSelectAll).Click += DoSelectAll;
		((ToolStrip)defaultMenu).Items.Add((ToolStripItem)(object)miUndo);
		((ToolStrip)defaultMenu).Items.Add((ToolStripItem)(object)miRedo);
		((ToolStrip)defaultMenu).Items.Add("-");
		((ToolStrip)defaultMenu).Items.Add((ToolStripItem)(object)miCut);
		((ToolStrip)defaultMenu).Items.Add((ToolStripItem)(object)miCopy);
		((ToolStrip)defaultMenu).Items.Add((ToolStripItem)(object)miPaste);
		((ToolStrip)defaultMenu).Items.Add((ToolStripItem)(object)miDelete);
		((ToolStrip)defaultMenu).Items.Add("-");
		((ToolStrip)defaultMenu).Items.Add((ToolStripItem)(object)miSelectAll);
	}

	protected virtual void UpdateMenu()
	{
		((ToolStripItem)miUndo).Enabled = Source.CanUndo();
		((ToolStripItem)miRedo).Enabled = Source.CanRedo();
		((ToolStripItem)miCut).Enabled = Selection.CanCut();
		((ToolStripItem)miCopy).Enabled = Selection.CanCopy();
		((ToolStripItem)miPaste).Enabled = Selection.CanPaste();
		((ToolStripItem)miDelete).Enabled = ((Control)this).Text != string.Empty;
	}

	protected virtual void PopupDefaultMenu(Point pos)
	{
		if (useDefaultMenu)
		{
			if (defaultMenu == null)
			{
				InitDefaultMenu();
			}
			UpdateMenu();
			((ToolStripDropDown)defaultMenu).Show((Control)(object)this, pos);
		}
	}

	public void MoveCaretOnDrag()
	{
		Point pt = ((Control)this).PointToClient(Cursor.Position);
		if (selection.NeedDragScroll(pt) && selection.DoDragScroll(pt))
		{
			return;
		}
		selection.BeginUpdate();
		BeginLineEndUpdate();
		try
		{
			bool lineEnd = false;
			pt = ScreenToText(pt.X, pt.Y, ref lineEnd);
			displayLines.AtLineEnd = lineEnd;
			Position = pt;
		}
		finally
		{
			EndLineEndUpdate();
			selection.EndUpdate();
		}
	}

	public void ProcessKeyPress(char keyChar)
	{
		if (Source.OverWrite)
		{
			Source.DeleteRight(1);
		}
		selection.InsertString(keyChar.ToString());
		if (macroRecording)
		{
			RecordKeyData(new MacroKeyListData((Keys)0, null, keyList.Handlers.processKey, keyChar, -1, 0));
		}
	}

	public override bool PreProcessMessage(ref Message msg)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		if (((Message)(ref msg)).Msg == 260)
		{
			Keys keyData = (Keys)(Control.ModifierKeys | (((Message)(ref msg)).WParam.ToInt32() & 0xFFFF));
			if (ProcessKey(keyData))
			{
				return true;
			}
		}
		return ((Control)this).PreProcessMessage(ref msg);
	}

	public bool ShouldSerializeText()
	{
		return source == null;
	}

	void IControl.Invalidate()
	{
		((Control)this).Invalidate();
	}

	void IControl.Invalidate(Rectangle rect)
	{
		((Control)this).Invalidate(rect);
	}

	void IControl.Update()
	{
		((Control)this).Update();
	}

	Point IControl.PointToClient(Point p)
	{
		return ((Control)this).PointToClient(p);
	}

	Point IControl.PointToScreen(Point p)
	{
		return ((Control)this).PointToScreen(p);
	}

	bool IControl.Focus()
	{
		return ((Control)this).Focus();
	}

	bool IControl.get_CanFocus()
	{
		return ((Control)this).CanFocus;
	}

	bool IControl.get_Visible()
	{
		return ((Control)this).Visible;
	}

	void IControl.set_Visible(bool value)
	{
		((Control)this).Visible = value;
	}

	Point IControl.get_Location()
	{
		return ((Control)this).Location;
	}

	void IControl.set_Location(Point value)
	{
		((Control)this).Location = value;
	}

	int IControl.get_Left()
	{
		return ((Control)this).Left;
	}

	void IControl.set_Left(int value)
	{
		((Control)this).Left = value;
	}

	int IControl.get_Top()
	{
		return ((Control)this).Top;
	}

	void IControl.set_Top(int value)
	{
		((Control)this).Top = value;
	}

	int IControl.get_Width()
	{
		return ((Control)this).Width;
	}

	void IControl.set_Width(int value)
	{
		((Control)this).Width = value;
	}

	int IControl.get_Height()
	{
		return ((Control)this).Height;
	}

	void IControl.set_Height(int value)
	{
		((Control)this).Height = value;
	}

	void IControl.set_Bounds(Rectangle value)
	{
		((Control)this).Bounds = value;
	}

	Rectangle IControl.get_ClientRectangle()
	{
		return ((Control)this).ClientRectangle;
	}

	Control IControl.get_Parent()
	{
		return ((Control)this).Parent;
	}

	void IControl.set_Parent(Control value)
	{
		((Control)this).Parent = value;
	}

	bool IControl.get_Enabled()
	{
		return ((Control)this).Enabled;
	}

	bool IControl.get_IsHandleCreated()
	{
		return ((Control)this).IsHandleCreated;
	}

	void IControl.add_Click(EventHandler value)
	{
		((Control)this).Click += value;
	}

	void IControl.remove_Click(EventHandler value)
	{
		((Control)this).Click -= value;
	}
}
