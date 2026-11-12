using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Stimulsoft.Editor;

[ToolboxBitmap(typeof(TextSource), "Images.TextSource.bmp")]
[ToolboxItem(false)]
public class TextSource : Component, ITextSource, IEditEx, IEdit, INavigate, IUndo, ISourceNotify, INotifyEx, INotify, INotifier, IImport, IExportEx, IExport, IHyperText, ISpelling, IBraceMatching, ITrackChanges
{
	private class TrackChange
	{
		public int Line;

		public bool Saved;

		public TrackChange(int line, bool saved)
		{
			Line = line;
			Saved = saved;
		}
	}

	protected internal class TrackChangeComparer : IComparer
	{
		public int Compare(object x, object y)
		{
			return ((TrackChange)x).Line - (int)y;
		}
	}

	private SyntaxStrings lines;

	private ArrayList edits;

	private ISyntaxEdit activeEdit;

	private string fileName = string.Empty;

	private LineStyles lineStyles;

	private BookMarks bookMarks;

	private SyntaxErrorList syntaxErrors;

	private CodeSnippetList codeSnippets;

	private SnippetRange currentSnippet;

	private int snippetUpdateCount;

	private Timer fmtTimer;

	private Point position;

	private NavigateOptions navigateOptions = EditConsts.DefaultNavigateOptions;

	private bool readOnly;

	private static bool overWrite;

	private bool modified;

	private int stringsUpdateCount;

	private bool singleLineMode;

	private IndentOptions indentOptions = EditConsts.DefaultIndentOptions;

	private int maxLength;

	private int updatePositionCount;

	private int updateCount;

	private NotifyState state;

	private int firstChanged = -1;

	private int lastChanged = -1;

	private int lastParsed;

	private Point oldPosition;

	private Rectangle selectBlockRect;

	private int count;

	private UndoFlags undoFlag;

	private UpdateReason reason;

	private ArrayList positionList;

	private bool needFormatting;

	private bool needFmtTimer;

	private ArrayList undoList;

	private ArrayList redoList;

	private UndoOptions undoOptions = EditConsts.DefaultUndoOptions;

	private int saveModifiedIdx = -1;

	private int undoLimitDelta = 16;

	private bool redo;

	private int undoLimit;

	private int undoUpdateCount;

	private int lockUndoCount;

	private Hashtable trackChanges;

	private IComparer trackChangeComparer;

	private ILexer lexer;

	private int parserLine;

	private bool highlightUrls;

	private HyperTextEventArgs hyperTextArgs;

	private Hashtable urlTable;

	private bool checkSpelling;

	private WordSpellEventArgs wordSpellArgs;

	private Hashtable spellTable;

	private Point spellSkipPt = new Point(-1, -1);

	private char[] openBraces = EditConsts.DefaultOpenBraces;

	private char[] closingBraces = EditConsts.DefaultClosingBraces;

	private Point openBrace = new Point(-1, -1);

	private Point closingBrace = new Point(-1, -1);

	private BracesOptions bracesOptions;

	private BraceList braceList;

	private Rectangle[] tempBraceRects;

	private Timer bracesTimer;

	private string insertStr = string.Empty;

	protected internal CodeSnippetList CodeSnippets
	{
		get
		{
			return codeSnippets;
		}
		set
		{
			UnhighlightCodeSnippets();
			foreach (object item in value)
			{
				codeSnippets.Add(item);
			}
			HighlightCodeSnippets();
		}
	}

	[Category("TextSource")]
	[Description("Gets or sets name of file that holds text source content.")]
	public string FileName
	{
		get
		{
			return fileName;
		}
		set
		{
			fileName = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public IList Edits => edits;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public ISyntaxStrings Lines
	{
		get
		{
			return lines;
		}
		set
		{
			lines.Assign(value);
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public ISyntaxEdit ActiveEdit
	{
		get
		{
			return activeEdit;
		}
		set
		{
			activeEdit = value;
		}
	}

	[Description("Gets or sets text source content as a single string with the individual strings delimited by carriage returns.")]
	public string Text
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
	public ILineStyles LineStyles
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

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public IBookMarks BookMarks
	{
		get
		{
			return bookMarks;
		}
		set
		{
			bookMarks.Assign(value);
		}
	}

	[Category("TextSource")]
	[Description("Gets or sets object that can perform lexical analysis of the text source content.")]
	public ILexer Lexer
	{
		get
		{
			return lexer;
		}
		set
		{
			if (lexer != value)
			{
				if (lexer is INotify)
				{
					((INotify)lexer).RemoveNotifier(this);
				}
				lexer = value;
				if (lexer is INotify)
				{
					((INotify)lexer).AddNotifier(this);
				}
				LexerChanged();
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int ParserLine => parserLine;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Point Position
	{
		get
		{
			return position;
		}
		set
		{
			MoveTo(value.X, value.Y);
		}
	}

	[Category("Behavior")]
	[Editor("QWhale.Design.FlagEnumerationEditor, QWhale.Editor", typeof(UITypeEditor))]
	[Description("Gets or sets navigating options.")]
	public NavigateOptions NavigateOptions
	{
		get
		{
			return navigateOptions;
		}
		set
		{
			if (navigateOptions != value)
			{
				navigateOptions = value;
				ValidatePosition(ref position);
			}
		}
	}

	[Category("Behavior")]
	[DefaultValue(false)]
	[Description("Gets or sets a value indicating whether the control's content is read-only.")]
	public bool Readonly
	{
		get
		{
			return readOnly;
		}
		set
		{
			if (readOnly != value)
			{
				BeginUpdate(UpdateReason.Other);
				try
				{
					readOnly = value;
				}
				finally
				{
					EndUpdate();
				}
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool OverWrite
	{
		get
		{
			return overWrite;
		}
		set
		{
			if (overWrite != value)
			{
				BeginUpdate(UpdateReason.Other);
				try
				{
					State |= State | NotifyState.OverWriteChanged;
					overWrite = value;
				}
				finally
				{
					EndUpdate();
				}
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool Modified
	{
		get
		{
			return modified;
		}
		set
		{
			if (modified != value)
			{
				BeginUpdate(UpdateReason.Other);
				try
				{
					modified = value;
					ModifiedChanged();
				}
				finally
				{
					EndUpdate();
				}
			}
		}
	}

	[Category("Behavior")]
	[DefaultValue(false)]
	[Description("Gets or sets a value indicating whether the control accepts only one line of the text.")]
	public bool SingleLineMode
	{
		get
		{
			return singleLineMode;
		}
		set
		{
			if (singleLineMode != value)
			{
				singleLineMode = value;
				if (value && lines.Count > 1)
				{
					DeleteBlock(new Rectangle(0, 1, 0, lines.Count - 1));
					MoveToLine(0);
				}
			}
		}
	}

	[Category("Behavior")]
	[Editor("QWhale.Design.FlagEnumerationEditor, QWhale.Editor", typeof(UITypeEditor))]
	[Description("Gets or sets \"QWhale.Editor.IndentOptions\" for this class, allowing to customize behaior of Edit control when user presses Enter to insert new text line.")]
	public IndentOptions IndentOptions
	{
		get
		{
			return indentOptions;
		}
		set
		{
			indentOptions = value;
		}
	}

	[Description("Specifies the maximum number of characters that can be entered into the edit control.")]
	[Category("Behavior")]
	[DefaultValue(0)]
	public int MaxLength
	{
		get
		{
			return maxLength;
		}
		set
		{
			maxLength = value;
		}
	}

	protected internal Point OldPosition => oldPosition;

	protected internal int SnippetUpdateCount
	{
		get
		{
			return snippetUpdateCount;
		}
		set
		{
			snippetUpdateCount = value;
		}
	}

	protected internal SnippetRange CurrentSnippet
	{
		get
		{
			return currentSnippet;
		}
		set
		{
			if (currentSnippet == value)
			{
				return;
			}
			if (currentSnippet != null)
			{
				int num = codeSnippets.IndexOf(currentSnippet);
				if (num >= 0 && CodeSnippets.IsFirstSnippet(num))
				{
					UpdateSnippetValues(currentSnippet);
				}
			}
			currentSnippet = value;
			InvalidateSnippets();
		}
	}

	protected internal Rectangle SelectBlockRect => selectBlockRect;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int UpdateCount => updateCount;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public NotifyState State
	{
		get
		{
			return state;
		}
		set
		{
			state = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int FirstChanged => firstChanged;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int LastChanged => lastChanged;

	protected bool AllowUndo => (undoOptions & UndoOptions.AllowUndo) != 0;

	protected bool GroupUndo => (undoOptions & UndoOptions.GroupUndo) != 0;

	protected bool UndoAfterSave => (undoOptions & UndoOptions.UndoAfterSave) != 0;

	protected bool UndoNavigations => (undoOptions & UndoOptions.UndoNavigations) != 0;

	[Category("Behavior")]
	[Editor("QWhale.Design.FlagEnumerationEditor, QWhale.Editor", typeof(UITypeEditor))]
	[Description("Gets or sets options for undo and redo operations.")]
	public UndoOptions UndoOptions
	{
		get
		{
			return undoOptions;
		}
		set
		{
			if (undoOptions != value)
			{
				if (((undoOptions ^ value) & UndoOptions.AllowUndo) != 0)
				{
					ClearUndo();
					ClearRedo();
				}
				undoOptions = value;
			}
		}
	}

	[Category("Behavior")]
	[DefaultValue(0)]
	[Description("Gets or sets a value that limits number of undo operations.")]
	public int UndoLimit
	{
		get
		{
			return undoLimit;
		}
		set
		{
			if (undoLimit != value)
			{
				undoLimit = value;
				ApplyUndoLimit();
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int UndoUpdateCount => undoUpdateCount;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public IList UndoList => undoList;

	public IList RedoList => redoList;

	[DefaultValue("\r\n")]
	[Description("Gets or sets a string value that terminates line.")]
	public string LineTerminator
	{
		get
		{
			return lines.LineTerminator;
		}
		set
		{
			lines.LineTerminator = value;
		}
	}

	[Category("Behavior")]
	[DefaultValue(false)]
	[Description("Gets or sets a value indicating whether hypertext urls in the text should be highlighted.")]
	public bool HighlightUrls
	{
		get
		{
			return highlightUrls;
		}
		set
		{
			if (highlightUrls != value)
			{
				highlightUrls = value;
				LexerChanged();
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Hashtable UrlTable
	{
		get
		{
			if (urlTable == null)
			{
				InitUrlTable();
			}
			return urlTable;
		}
	}

	protected internal Hashtable SpellTable => spellTable;

	[Category("Behavior")]
	[DefaultValue(false)]
	[Description("Gets or sets a value indicating whether the document can check spelling for its content.")]
	public bool CheckSpelling
	{
		get
		{
			return checkSpelling;
		}
		set
		{
			if (checkSpelling != value)
			{
				checkSpelling = value;
				LexerChanged();
			}
		}
	}

	protected internal Rectangle[] TempBraceRects
	{
		get
		{
			return tempBraceRects;
		}
		set
		{
			if (tempBraceRects != value)
			{
				UpdateTempBraces(setOn: false);
				tempBraceRects = value;
				UpdateTempBraces(setOn: true);
				if (value != null)
				{
					StartBracesTimer();
				}
			}
		}
	}

	[Category("Braces")]
	[Description("Gets or sets an array of characters each one representing an open brace.")]
	public char[] OpenBraces
	{
		get
		{
			return openBraces;
		}
		set
		{
			openBraces = new char[value.Length];
			Array.Copy(value, openBraces, value.Length);
			if (bracesOptions != 0)
			{
				LexerChanged();
			}
		}
	}

	[Category("Braces")]
	[Description("Gets or sets an array of characters each one representing a closing brace.")]
	public char[] ClosingBraces
	{
		get
		{
			return closingBraces;
		}
		set
		{
			closingBraces = new char[value.Length];
			Array.Copy(value, closingBraces, value.Length);
			if (bracesOptions != 0)
			{
				LexerChanged();
			}
		}
	}

	[Category("Braces")]
	[DefaultValue(BracesOptions.None)]
	[Editor("QWhale.Design.FlagEnumerationEditor, QWhale.Editor", typeof(UITypeEditor))]
	[Description("Gets or sets options specifying appearance and behaviour of matching braces within Edit control.")]
	public BracesOptions BracesOptions
	{
		get
		{
			return bracesOptions;
		}
		set
		{
			if (bracesOptions != value)
			{
				bracesOptions = value;
				LexerChanged();
				if ((value & BracesOptions.TempHighlight) == 0)
				{
					UpdateTempBraces(setOn: false);
				}
				if ((value & BracesOptions.Highlight) != 0)
				{
					DoHighlightBraces();
				}
			}
		}
	}

	[Category("TextSource")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[TypeConverter(typeof(CollectionConverter))]
	[Description("Gets or sets text source content in the form of string array.")]
	public string[] Strings
	{
		get
		{
			string[] array = new string[lines.Count];
			for (int i = 0; i < lines.Count; i++)
			{
				array[i] = lines[i];
			}
			return array;
		}
		set
		{
			lines.BeginUpdate();
			try
			{
				lines.Clear();
				foreach (string value2 in value)
				{
					lines.Add(value2);
				}
			}
			finally
			{
				lines.EndUpdate();
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual object XmlInfo
	{
		get
		{
			return new XmlTextSourceInfo(this);
		}
		set
		{
			((XmlTextSourceInfo)value).FixupReferences(this);
		}
	}

	private event EventHandler notifyHandler;

	[Category("TextSource")]
	[Description("Occurs when a control needs checking whether some string represents hypertext.")]
	public event HyperTextEvent CheckHyperText;

	[Category("TextSource")]
	[Description("Occurs when spelling of some word within the text needs checking.")]
	public event WordSpellEvent WordSpell;

	public TextSource(IContainer container)
		: this()
	{
		container.Add(this);
	}

	public TextSource()
	{
		InitializeComponent();
	}

	~TextSource()
	{
		if (fmtTimer != null)
		{
			fmtTimer.Enabled = false;
			((Component)(object)fmtTimer).Dispose();
		}
		if (bracesTimer != null)
		{
			bracesTimer.Enabled = false;
			((Component)(object)bracesTimer).Dispose();
		}
		ClearUndo();
		ClearRedo();
		lines.RemoveNotifier(this);
	}

	private void InitializeComponent()
	{
		lines = new SyntaxStrings(this);
		lines.AddNotifier(this);
		undoList = new ArrayList();
		redoList = new ArrayList();
		edits = new ArrayList();
		positionList = new ArrayList();
		position = new Point(0, 0);
		insertStr = string.Empty;
		lineStyles = new LineStyles(this);
		bookMarks = new BookMarks(this);
		syntaxErrors = new SyntaxErrorList();
		codeSnippets = new CodeSnippetList();
		braceList = new BraceList();
		trackChangeComparer = new TrackChangeComparer();
		hyperTextArgs = new HyperTextEventArgs("", isHyperText: false);
		wordSpellArgs = new WordSpellEventArgs("", correct: true, -1);
	}

	protected void OnFormatting(object source, EventArgs e)
	{
		try
		{
			EndFmtTimer();
			if (NeedReparseText())
			{
				FormatText();
			}
		}
		catch
		{
		}
	}

	private void StartFmtTimer(bool modified, bool lineChanged)
	{
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Expected O, but got Unknown
		needFormatting |= modified;
		if (modified)
		{
			EndFmtTimer();
			if (NeedReparseText() && ((state & NotifyState.StringsChanged) != 0 || (state & NotifyState.SyntaxChanged) != 0 || !NeedReparseTextOnLineChange()))
			{
				if (fmtTimer == null)
				{
					fmtTimer = new Timer();
					fmtTimer.Enabled = false;
					fmtTimer.Interval = EditConsts.DefaultOutlineDelay;
					fmtTimer.Tick += OnFormatting;
				}
				fmtTimer.Enabled = true;
			}
		}
		if (lineChanged && needFormatting && NeedReparseTextOnLineChange())
		{
			EndFmtTimer();
			FormatText();
		}
	}

	private void EndFmtTimer()
	{
		if (fmtTimer != null)
		{
			fmtTimer.Enabled = false;
		}
	}

	private void UnhighlightSyntaxErrors(IList errors, int line)
	{
		for (int num = errors.Count - 1; num >= 0; num--)
		{
			ISyntaxError syntaxError = (ISyntaxError)errors[num];
			if (syntaxError.Position.Y == line)
			{
				SetColorFlag(new Rectangle(syntaxError.Position, syntaxError.Size), ColorFlags.WaveLine, setFlag: false);
				errors.RemoveAt(num);
			}
		}
	}

	private void UnhighlightSyntaxErrors(IList errors)
	{
		foreach (ISyntaxError error in errors)
		{
			SetColorFlag(new Rectangle(error.Position, error.Size), ColorFlags.WaveLine, setFlag: false);
		}
	}

	private void HighlightSyntaxErrors()
	{
		ISyntaxParser syntaxParser = (ISyntaxParser)Lexer;
		SyntaxErrorList syntaxErrorList = new SyntaxErrorList();
		syntaxParser.GetSyntaxErrors(syntaxErrorList);
		syntaxErrorList.Sort();
		foreach (ISyntaxError item in syntaxErrorList)
		{
			if (syntaxErrors.FindError(item.Position, exact: true, out var index) && ((ISyntaxError)syntaxErrors[index]).Range.EndPoint.Equals((object?)item.Range.EndPoint))
			{
				syntaxErrors.RemoveAt(index);
			}
		}
		if (syntaxErrorList.Count > 0)
		{
			ParseToString(Math.Min(((ISyntaxError)syntaxErrorList[syntaxErrorList.Count - 1]).Position.Y, lines.Count - 1));
		}
		UnhighlightSyntaxErrors(syntaxErrors);
		foreach (ISyntaxError item2 in syntaxErrorList)
		{
			SetColorFlag(new Rectangle(item2.Position, item2.Size), ColorFlags.WaveLine, setFlag: true);
		}
		syntaxErrors.Clear();
		foreach (ISyntaxError item3 in syntaxErrorList)
		{
			ISyntaxError syntaxError4 = new SyntaxError(item3.Position, item3.Name, item3.Description);
			syntaxError4.Range.EndPoint = item3.Range.EndPoint;
			syntaxErrors.Add(syntaxError4);
		}
	}

	private void DoFormatText()
	{
		BeginUpdate(UpdateReason.Other);
		try
		{
			ISyntaxParser syntaxParser = (ISyntaxParser)Lexer;
			syntaxParser.Strings = Lines;
			syntaxParser.ReparseText();
			if (NeedSyntaxErrors())
			{
				HighlightSyntaxErrors();
			}
			state |= NotifyState.BlockChanged | NotifyState.Outline | NotifyState.TextParsed;
		}
		finally
		{
			EndUpdate();
		}
	}

	private string GetSnippetValue(SnippetRange range)
	{
		string text = Lines[range.StartPoint.Y];
		if (range.StartPoint.X >= 0 && range.StartPoint.X < text.Length)
		{
			if (range.EndPoint.Y == range.StartPoint.Y)
			{
				if (range.StartPoint.X > range.EndPoint.X)
				{
					return string.Empty;
				}
				return text.Substring(range.StartPoint.X, Math.Min(range.EndPoint.X, text.Length) - range.StartPoint.X);
			}
			return text.Substring(range.StartPoint.X);
		}
		return string.Empty;
	}

	private void SetSnippetValue(SnippetRange range, string value)
	{
		BeginUpdate(UpdateReason.Insert);
		try
		{
			MoveTo(range.StartPoint);
			DeleteRight((range.EndPoint.Y == range.StartPoint.Y) ? (range.EndPoint.X - range.StartPoint.X) : int.MaxValue);
			Insert(value);
		}
		finally
		{
			EndUpdate();
		}
	}

	private void UpdateSnippetValues(SnippetRange snippet)
	{
		int index = StorePosition(Position);
		BeginUpdate(UpdateReason.Insert);
		try
		{
			string snippetValue = GetSnippetValue(snippet);
			for (int num = codeSnippets.Count - 1; num >= 0; num--)
			{
				SnippetRange snippetRange = (SnippetRange)codeSnippets[num];
				if (snippetRange != snippet && snippetRange.Id == snippet.Id && GetSnippetValue(snippetRange) != snippetValue)
				{
					SetSnippetValue(snippetRange, snippetValue);
				}
			}
		}
		finally
		{
			Position = RestorePosition(index);
			EndUpdate();
		}
	}

	private void InvalidateSnippets()
	{
		if (codeSnippets.Count > 0)
		{
			BeginUpdate(UpdateReason.Other);
			try
			{
				LinesChanged(((IRange)codeSnippets[0]).StartPoint.Y, ((IRange)codeSnippets[codeSnippets.Count - 1]).EndPoint.Y, modified: false);
				state |= NotifyState.BlockChanged;
			}
			finally
			{
				EndUpdate();
			}
		}
	}

	protected internal ISyntaxError GetSyntaxError(int x, int y)
	{
		if (!syntaxErrors.FindError(new Point(x, y), exact: false, out var index))
		{
			return null;
		}
		return syntaxErrors[index] as ISyntaxError;
	}

	protected internal IList GetSyntaxErrors()
	{
		return syntaxErrors;
	}

	protected internal void UnhighlightCodeSnippets()
	{
		foreach (IRange codeSnippet in codeSnippets)
		{
			SetColorFlag(new Rectangle(codeSnippet.StartPoint, new Size(codeSnippet.EndPoint.X - codeSnippet.StartPoint.X, codeSnippet.EndPoint.Y - codeSnippet.StartPoint.Y)), ColorFlags.CodeSnippet, setFlag: false);
		}
		codeSnippets.Clear();
	}

	protected internal SnippetRange GetSnippet(Point position)
	{
		if (!codeSnippets.FindSnippet(position, exact: false, out var index))
		{
			return null;
		}
		return codeSnippets[index] as SnippetRange;
	}

	protected internal bool IsFirstSnippet(Point position)
	{
		if (codeSnippets.FindSnippet(position, exact: false, out var index))
		{
			return codeSnippets.IsFirstSnippet(index);
		}
		return false;
	}

	protected internal Point IncPosition(Point position, int len)
	{
		int num = lines.Count;
		int num2 = 0;
		Point result = position;
		while (len > 0)
		{
			if (position.Y < num)
			{
				num2 = ((result.Y < num) ? lines.GetLength(result.Y) : 0);
				if (result.Y == position.Y)
				{
					num2 -= position.X;
				}
				num2 = Math.Max(num2, 0);
			}
			if (len > num2)
			{
				result.Y++;
				len -= num2 + 2;
				if (len <= 0 && position.Y != result.Y)
				{
					result.X = Math.Max(len, 0);
				}
				continue;
			}
			result.X = len;
			if (position.Y == result.Y)
			{
				result.X += position.X;
			}
			break;
		}
		return result;
	}

	public int TextPointToAbsolutePosition(Point position)
	{
		return lines.TextPointToAbsolutePosition(position);
	}

	public Point AbsolutePositionToTextPoint(int position)
	{
		return lines.AbsolutePositionToTextPoint(position);
	}

	public int GetCharIndexFromPosition(Point position)
	{
		return lines.TextPointToAbsolutePosition(position);
	}

	public Point GetPositionFromCharIndex(int charIndex)
	{
		return lines.AbsolutePositionToTextPoint(charIndex);
	}

	public void ParseString(int index)
	{
		ParseStrings(index, index);
	}

	public bool NeedParse()
	{
		if (lexer == null && !checkSpelling && !highlightUrls)
		{
			return bracesOptions != BracesOptions.None;
		}
		return true;
	}

	public void ParseStrings(int fromIndex, int toIndex)
	{
		if (!NeedParse())
		{
			return;
		}
		int num = ((fromIndex > 0 && fromIndex - 1 < lines.Count) ? lines.GetItem(fromIndex - 1).LexState : ((lexer != null) ? lexer.DefaultState : 0));
		fromIndex = Math.Max(fromIndex, 0);
		toIndex = Math.Min(toIndex, lines.Count - 1);
		for (int i = fromIndex; i <= toIndex; i++)
		{
			IStrItem item = lines.GetItem(i);
			if ((item.State & StrItemState.Parsed) == 0 || item.PrevLexState != num)
			{
				item.PrevLexState = num;
				num = (item.LexState = ParseText(num, item, i, parseTextOnly: false));
				item.State |= StrItemState.Parsed;
			}
			else
			{
				num = item.LexState;
			}
		}
		HighlightCodeSnippets();
	}

	public virtual IStrItem CreateStrItem(string s)
	{
		return new StrItem(s);
	}

	public void ParseToString(int index)
	{
		if (lastParsed < index)
		{
			ParseStrings(lastParsed, index);
			lastParsed = index + 1;
			if (bracesOptions != 0 && !DoHighlightBraces())
			{
				UpdateBraces();
			}
			if (tempBraceRects != null)
			{
				UpdateTempBraces(setOn: true);
			}
		}
	}

	public void SetColorFlag(Rectangle rect, ColorFlags flag, bool setFlag)
	{
		for (int i = rect.Top; i <= rect.Bottom; i++)
		{
			int num = 0;
			int num2 = 0;
			if (i == rect.Top)
			{
				if (i == rect.Bottom)
				{
					num = rect.Left;
					num2 = rect.Right;
				}
				else
				{
					num = rect.Left;
					num2 = int.MaxValue;
				}
			}
			else if (i == rect.Bottom)
			{
				num = 0;
				num2 = rect.Right;
			}
			else
			{
				num = 0;
				num2 = int.MaxValue;
			}
			SetColorFlag(new Point(num, i), (num2 == int.MaxValue) ? num2 : (num2 - num), flag, setFlag);
		}
	}

	public void SetColorFlag(Point pos, int len, ColorFlags flag, bool setFlag)
	{
		if (pos.Y < 0)
		{
			return;
		}
		IStrItem item = Lines.GetItem(pos.Y);
		if (item == null)
		{
			return;
		}
		BeginUpdate(UpdateReason.Other);
		try
		{
			int num = item.ColorData.Length;
			if (pos.X >= 0 && pos.X < num)
			{
				len = Math.Min(len, num - pos.X);
				if (len > 0)
				{
					item.SetColorFlag(pos.X, len, flag, setFlag);
				}
			}
			LinesChanged(pos.Y, pos.Y);
			state |= NotifyState.BlockChanged;
		}
		finally
		{
			EndUpdate();
		}
	}

	protected internal void SetNavigateOptions(NavigateOptions navigateOptions)
	{
		this.navigateOptions = navigateOptions;
	}

	protected void RemoveTrailingSpaces(bool addUndo, bool lineEnd)
	{
		if (!lines.RemoveTrailingSpaces)
		{
			return;
		}
		string text = lines[position.Y];
		string text2 = text.TrimEnd(new char[0]);
		int num = text.Length - text2.Length;
		if (num <= 0)
		{
			return;
		}
		BeginUpdate(UpdateReason.Delete);
		try
		{
			if (addUndo)
			{
				NavigateOptions navigateOptions = this.navigateOptions;
				try
				{
					this.navigateOptions |= NavigateOptions.BeyondEol;
					int x = position.X;
					MoveToChar(text2.Length);
					DeleteRight(num);
					if (lineEnd)
					{
						position.X = Math.Min(x, text2.Length);
					}
					else
					{
						position.X = x;
					}
				}
				finally
				{
					this.navigateOptions = navigateOptions;
				}
			}
			else
			{
				stringsUpdateCount++;
				try
				{
					lines[position.Y] = text2;
				}
				finally
				{
					stringsUpdateCount--;
				}
			}
			if (LexStateChanged(position.Y))
			{
				LinesChanged(position.Y, int.MaxValue, modified: true);
			}
			else
			{
				LinesChanged(position.Y, position.Y, modified: true);
			}
		}
		finally
		{
			EndUpdate();
		}
	}

	public void ValidatePosition(ref Point position)
	{
		if (position.Y < 0)
		{
			position.Y = 0;
		}
		if (position.X < 0)
		{
			position.X = 0;
		}
		if (((NavigateOptions.BeyondEof & navigateOptions) == 0 || singleLineMode) && position.Y >= lines.Count)
		{
			position.Y = Math.Max(lines.Count - 1, 0);
		}
		if ((NavigateOptions.BeyondEol & navigateOptions) == 0)
		{
			int num = ((position.Y < lines.Count) ? lines.GetLength(position.Y) : 0);
			if (position.X >= num)
			{
				position.X = num;
			}
		}
	}

	public int StorePosition(Point pt)
	{
		return positionList.Add(pt);
	}

	public Point RestorePosition(int index)
	{
		Point result;
		if (index < positionList.Count)
		{
			result = (Point)positionList[index];
			positionList.RemoveAt(index);
		}
		else
		{
			result = new Point(0, 0);
		}
		return result;
	}

	public void Navigate(int deltaX, int deltaY)
	{
		if (deltaX == 0 && deltaY == 0)
		{
			return;
		}
		BeginUpdate(UpdateReason.Navigate);
		try
		{
			if (deltaY != 0)
			{
				RemoveTrailingSpaces((NavigateOptions.BeyondEof & navigateOptions) == 0, lineEnd: false);
			}
			Point point = position;
			position.Offset(deltaX, deltaY);
			ValidatePosition(ref position);
			if (UndoNavigations)
			{
				AddUndo(UndoOperation.Navigate, new Point(position.X - point.X, position.Y - point.Y));
			}
			else
			{
				AddUndo(UndoOperation.Navigate, null);
			}
		}
		finally
		{
			EndUpdate();
		}
	}

	public void MoveTo(int x, int y)
	{
		Navigate(x - position.X, y - position.Y);
	}

	public void MoveToChar(int x)
	{
		Navigate(x - position.X, 0);
	}

	public void MoveToLine(int y)
	{
		Navigate(0, y - position.Y);
	}

	public void MoveTo(Point position)
	{
		MoveTo(position.X, position.Y);
	}

	public virtual void ResetNavigateOptions()
	{
		NavigateOptions = EditConsts.DefaultNavigateOptions;
	}

	private void StripToMaxLength(ref string str)
	{
		if (maxLength > 0)
		{
			int num = maxLength - Text.Length;
			if (num <= str.Length)
			{
				str = str.Substring(0, Math.Max(0, num));
			}
		}
	}

	private void StripToMaxLength(ref string[] strings)
	{
		if (maxLength <= 0)
		{
			return;
		}
		int num = maxLength - Text.Length;
		int num2 = 0;
		int length = Lines.LineTerminator.Length;
		int num3 = strings.Length;
		for (int i = 0; i < strings.Length; i++)
		{
			num2 += strings[i].Length;
			if (num2 > num)
			{
				int val = strings[i].Length - (num2 - num);
				strings[i] = strings[i].Substring(0, Math.Max(0, val));
				num3 = i + 1;
				break;
			}
			num2 += length;
		}
		if (num3 < strings.Length)
		{
			string[] array = new string[num3];
			Array.Copy(strings, 0, array, 0, num3);
			strings = array;
		}
	}

	private bool GetModified(ArrayList list, int index, bool saved)
	{
		index = Math.Max(0, index);
		for (int num = list.Count - 1; num >= index; num--)
		{
			UndoData undoData = (UndoData)list[num];
			if (undoData.Operation != UndoOperation.Navigate && undoData.Operation != UndoOperation.UndoBlock)
			{
				return true;
			}
		}
		return false;
	}

	private void EnsurePosInsideText()
	{
		if (position.Y >= lines.Count)
		{
			while (position.Y >= lines.Count)
			{
				lines.Add(string.Empty);
			}
			LinesChanged(position.Y, int.MaxValue, modified: false);
		}
	}

	private string GetPrevIndentStr(out int indent)
	{
		indent = 0;
		for (int num = position.Y - 1; num >= 0; num--)
		{
			string text = Lines[num];
			if (text.Trim() != string.Empty)
			{
				int num2 = text.Length - text.TrimStart(null).Length;
				indent = Lines.TabPosToPos(text, num2);
				return text.Substring(0, num2);
			}
		}
		return string.Empty;
	}

	private int GetTabIndent(int indent)
	{
		return lines.TabPosToPos(new string('\t', indent), indent);
	}

	private string GetAutoIndent()
	{
		if ((indentOptions & IndentOptions.AutoIndent) != 0)
		{
			if ((indentOptions & IndentOptions.SmartIndent) != 0 && NeedFormatText())
			{
				int indent = Math.Max(((ISyntaxParser)lexer).GetSmartIndent(Position.Y, autoIndent: true), 0);
				return lines.GetIndentString(GetTabIndent(indent), 0);
			}
			int indent2 = 0;
			string empty = string.Empty;
			empty = GetPrevIndentStr(out indent2);
			if ((indentOptions & IndentOptions.UsePrevIndent) != 0)
			{
				return empty;
			}
			return lines.GetIndentString(indent2, 0);
		}
		return string.Empty;
	}

	protected bool LexStateChanged(int line)
	{
		if (lexer != null && line >= 0 && line < lines.Count)
		{
			int lexState = lines.GetItem(line).LexState;
			int num = ((line > 0 && line - 1 < lines.Count) ? lines.GetItem(line - 1).LexState : lexer.DefaultState);
			IStrItem item = lines.GetItem(line);
			return ParseText(num, item, line, parseTextOnly: true) != lexState;
		}
		return false;
	}

	protected void ModifiedChanged()
	{
		if (!modified)
		{
			if (UndoAfterSave)
			{
				for (int i = 0; i < undoList.Count; i++)
				{
					((UndoData)undoList[i]).UndoFlag |= UndoFlags.Saved;
				}
				for (int j = 0; j < redoList.Count; j++)
				{
					((UndoData)redoList[j]).UndoFlag &= ~UndoFlags.Saved;
				}
				saveModifiedIdx = GetUndoList().Count - 1;
			}
			else
			{
				ClearUndo();
				ClearRedo();
			}
		}
		state |= NotifyState.ModifiedChanged;
	}

	protected int ParseText(int state, IStrItem item, int line, bool parseTextOnly)
	{
		parserLine = line;
		int result = state;
		if (lexer != null)
		{
			short[] colorData = item.ColorData;
			result = lexer.ParseText(state, line, item.String, ref colorData);
			item.ColorData = colorData;
		}
		if (!parseTextOnly)
		{
			if (highlightUrls)
			{
				DoHighlightUrls(item, line);
			}
			if (checkSpelling && this.WordSpell != null)
			{
				DoCheckSpelling(item, line, withUpdate: false);
			}
			if (bracesOptions != 0)
			{
				DoParseBraces(item, line);
			}
		}
		return result;
	}

	protected internal void DisablePositionUpdate()
	{
		updatePositionCount++;
	}

	protected internal void EnablePositionUpdate()
	{
		updatePositionCount--;
	}

	protected internal void PositionChanged(UpdateReason reason, int deltaX, int deltaY)
	{
		if (updatePositionCount <= 0)
		{
			if (this.notifyHandler != null)
			{
				this.notifyHandler(this, new PositionChangedEventArgs(reason, deltaX, deltaY));
			}
			braceList.PositionChanged(Position.X, Position.Y, deltaX, deltaY);
			syntaxErrors.PositionChanged(Position.X, Position.Y, deltaX, deltaY);
			codeSnippets.PositionChanged(Position.X, Position.Y, deltaX, deltaY, (state & NotifyState.SmartFormat) != 0);
			if (snippetUpdateCount == 0 && codeSnippets.NeedClear(Position.Y))
			{
				UnhighlightCodeSnippets();
			}
			if (bookMarks.PositionChanged(Position.X, Position.Y, deltaX, deltaY))
			{
				state |= NotifyState.BookMarkChanged;
			}
			if (lineStyles.PositionChanged(Position.X, Position.Y, deltaX, deltaY))
			{
				state |= NotifyState.BookMarkChanged;
			}
			if (lexer is ISyntaxParser && ((ISyntaxParser)lexer).Options != 0)
			{
				((ISyntaxParser)lexer).SyntaxTree.PositionChanged(Position.X, Position.Y, deltaX, deltaY, reason);
			}
			for (int i = 0; i < positionList.Count; i++)
			{
				Point pt = (Point)positionList[i];
				SortList.UpdatePos(Position.X, Position.Y, deltaX, deltaY, ref pt, endPos: false);
				positionList[i] = pt;
			}
		}
	}

	protected void StringsChanged(object sender, EventArgs ea)
	{
		if (stringsUpdateCount != 0)
		{
			return;
		}
		BeginUpdate(UpdateReason.Other);
		try
		{
			ClearUndo();
			ClearRedo();
			int num = ((ISyntaxStrings)sender).FirstChanged;
			int num2 = ((ISyntaxStrings)sender).LastChanged;
			BlockDeleting(new Rectangle(0, num, 0, num2));
			modified = false;
			LinesChanged(num, num2);
			State |= State | NotifyState.Edit | NotifyState.PositionChanged | NotifyState.ModifiedChanged | NotifyState.CountChanged | NotifyState.StringsChanged;
			PositionChanged(UpdateReason.Other, 0, 0);
			UpdateParsed(num, num2);
			if ((navigateOptions & NavigateOptions.LeaveCaretOnStringsChanged) == 0)
			{
				MoveToLine(Math.Max(Math.Min(num, lines.Count - 1), 0));
			}
			else if (Position.Y >= lines.Count - 1)
			{
				MoveToLine(Math.Max(lines.Count - 1, 0));
			}
		}
		finally
		{
			EndUpdate();
		}
	}

	protected void BlockDeleting(Point pt, int Len)
	{
		if (pt.X == 0 && lines[pt.Y] == string.Empty)
		{
			Len = int.MaxValue;
		}
		Rectangle rect = new Rectangle(pt.X, pt.Y, Len, 0);
		if (bookMarks.BlockDeleting(rect))
		{
			state |= NotifyState.BookMarkChanged;
		}
		if (lineStyles.BlockDeleting(rect))
		{
			state |= NotifyState.BookMarkChanged;
		}
		if (codeSnippets.BlockDeleting(rect))
		{
			state |= NotifyState.BlockChanged;
		}
	}

	protected void BlockDeleting(Rectangle rect)
	{
		if ((rect.X == 0 || rect.Height > 0) && lines.GetLength(rect.Bottom) <= rect.Width)
		{
			rect.Width = int.MaxValue;
		}
		braceList.BlockDeleting(rect);
		syntaxErrors.BlockDeleting(rect);
		codeSnippets.BlockDeleting(rect);
		if (snippetUpdateCount == 0 && codeSnippets.NeedClear(rect))
		{
			UnhighlightCodeSnippets();
		}
		if (bookMarks.BlockDeleting(rect))
		{
			state |= NotifyState.BookMarkChanged;
		}
		if (lineStyles.BlockDeleting(rect))
		{
			state |= NotifyState.BookMarkChanged;
		}
		if (lexer is ISyntaxParser && ((ISyntaxParser)lexer).Options != 0)
		{
			((ISyntaxParser)lexer).SyntaxTree.BlockDeleting(rect);
		}
	}

	protected internal void IndentLine()
	{
		string autoIndent = GetAutoIndent();
		if (!(autoIndent != string.Empty))
		{
			return;
		}
		if ((indentOptions & IndentOptions.JumpToIndent) != 0)
		{
			NavigateOptions navigateOptions = this.navigateOptions;
			try
			{
				this.navigateOptions |= NavigateOptions.BeyondEol;
				Navigate(lines.TabPosToPos(autoIndent, autoIndent.Length), 0);
				return;
			}
			finally
			{
				this.navigateOptions = navigateOptions;
			}
		}
		Insert(autoIndent);
	}

	public bool IsLineReadonly(int index)
	{
		IStrItem item = lines.GetItem(index);
		if (item != null)
		{
			return (item.State & StrItemState.Readonly) != 0;
		}
		return false;
	}

	public void SetLineReadonly(int index, bool readOnly)
	{
		IStrItem item = lines.GetItem(index);
		if (item != null)
		{
			if (readOnly)
			{
				item.State |= StrItemState.Readonly;
			}
			else
			{
				item.State &= ~StrItemState.Readonly;
			}
		}
	}

	public bool Insert(string str)
	{
		if (readOnly || IsLineReadonly(position.Y))
		{
			return false;
		}
		StripToMaxLength(ref str);
		insertStr = str;
		BeginUpdate(UpdateReason.Insert);
		stringsUpdateCount++;
		try
		{
			EnsurePosInsideText();
			string text = lines[position.Y];
			int length = text.Length;
			if (position.X > length)
			{
				str = lines.GetIndentString(position.X - length, Lines.TabPosToPos(text, length)) + str;
				MoveTo(length, position.Y);
			}
			lines[position.Y] = text.Insert(position.X, str);
			length = str.Length;
			AddUndo(UndoOperation.Insert, length);
			if (LexStateChanged(position.Y))
			{
				LinesChanged(position.Y, int.MaxValue, modified: true);
			}
			else
			{
				LinesChanged(position.Y, position.Y, modified: true);
			}
			PositionChanged(UpdateReason.Insert, length, 0);
			if (!UndoNavigations || (state & NotifyState.Undo) == 0)
			{
				if ((state & NotifyState.Undo) != 0)
				{
					DisableUndo();
					try
					{
						Navigate(length, 0);
					}
					finally
					{
						EnableUndo();
					}
				}
				else
				{
					Navigate(length, 0);
				}
			}
		}
		finally
		{
			stringsUpdateCount--;
			EndUpdate();
		}
		return true;
	}

	public bool InsertFromFile(string file)
	{
		if (readOnly)
		{
			return false;
		}
		ISyntaxStrings syntaxStrings = new SyntaxStrings(null);
		syntaxStrings.LoadFile(file);
		return InsertBlock(syntaxStrings);
	}

	public bool DeleteRight(int len)
	{
		if (readOnly || IsLineReadonly(position.Y) || len <= 0 || position.Y >= lines.Count || position.X >= lines.GetLength(position.Y))
		{
			return false;
		}
		BeginUpdate(UpdateReason.Delete);
		stringsUpdateCount++;
		try
		{
			string text = lines[position.Y];
			len = Math.Min(len, text.Length - position.X);
			AddUndo(UndoOperation.Delete, text.Substring(position.X, len));
			text = text.Remove(position.X, len);
			lines[position.Y] = text;
			BlockDeleting(position, len);
			if (LexStateChanged(position.Y))
			{
				LinesChanged(position.Y, int.MaxValue, modified: true);
			}
			else
			{
				LinesChanged(position.Y, position.Y, modified: true);
			}
			PositionChanged(UpdateReason.Delete, -len, 0);
		}
		finally
		{
			stringsUpdateCount--;
			EndUpdate();
		}
		return true;
	}

	public bool DeleteLeft(int len)
	{
		if (readOnly)
		{
			return false;
		}
		BeginUpdate(UpdateReason.Delete);
		try
		{
			len = Math.Min(len, position.X);
			Navigate(-len, 0);
			return DeleteRight(len);
		}
		finally
		{
			EndUpdate();
		}
	}

	public bool InsertBlock(string text)
	{
		return InsertBlock(StrItem.Split(text));
	}

	public bool InsertBlock(string[] strings)
	{
		return InsertBlock(strings, select: false);
	}

	public bool InsertBlock(string[] strings, bool select)
	{
		if (readOnly || IsLineReadonly(position.Y) || strings.Length == 0)
		{
			return false;
		}
		if (singleLineMode)
		{
			strings = new string[1] { strings[0] };
		}
		StripToMaxLength(ref strings);
		BeginUpdate(UpdateReason.InsertBlock);
		stringsUpdateCount++;
		try
		{
			Point location = Position;
			EnsurePosInsideText();
			string text = lines[position.Y];
			int length = text.Length;
			string text2 = string.Empty;
			if (position.X > length)
			{
				strings[0] = lines.GetIndentString(position.X - length, Lines.TabPosToPos(text, length)) + strings[0];
				MoveTo(length, position.Y);
			}
			length = 0;
			string[] array = strings;
			foreach (string text3 in array)
			{
				length += text3.Length + 2;
			}
			length -= 2;
			AddUndo(UndoOperation.InsertBlock, new Point(length, strings.Length - 1));
			for (int j = 0; j < strings.Length; j++)
			{
				if (j == 0)
				{
					string text4 = lines[position.Y];
					length = text4.Length;
					if (position.X < length)
					{
						text2 = text4.Substring(position.X, length - Position.X);
						text4 = text4.Substring(0, position.X) + strings[0];
					}
					else
					{
						text4 += strings[0];
					}
					if (j == strings.Length - 1)
					{
						text4 += text2;
					}
					lines[position.Y] = text4;
				}
				else if (j == strings.Length - 1)
				{
					lines.Insert(position.Y + j, strings[j] + text2);
				}
				else
				{
					lines.Insert(position.Y + j, strings[j]);
				}
			}
			LinesChanged(position.Y, int.MaxValue, modified: true);
			int num = strings.Length - 1;
			int num2 = strings[^1].Length;
			if (num > 0)
			{
				num2 -= Position.X;
			}
			PositionChanged(UpdateReason.InsertBlock, num2, num);
			if (!UndoNavigations || (state & NotifyState.Undo) == 0)
			{
				Navigate(num2, num);
			}
			if (select)
			{
				state |= NotifyState.SelectBlock;
				selectBlockRect = new Rectangle(location, new Size(position.X - location.X, position.Y - location.Y));
			}
		}
		finally
		{
			stringsUpdateCount--;
			EndUpdate();
		}
		return true;
	}

	public bool InsertBlock(ISyntaxStrings strings)
	{
		if (!readOnly)
		{
			return InsertBlock(strings.ToStringArray());
		}
		return false;
	}

	public bool DeleteBlock(int len)
	{
		Point point = IncPosition(position, len);
		return DeleteBlock(new Rectangle(position.X, position.Y, point.X - position.X, point.Y - position.Y));
	}

	public bool DeleteBlock(Rectangle rect)
	{
		if (readOnly)
		{
			return false;
		}
		rect.Height = Math.Min(rect.Bottom, lines.Count) - rect.Top;
		if (rect.Height < 0)
		{
			return false;
		}
		for (int i = rect.Top; i <= rect.Bottom; i++)
		{
			if (IsLineReadonly(i))
			{
				return false;
			}
		}
		BeginUpdate(UpdateReason.DeleteBlock);
		stringsUpdateCount++;
		try
		{
			BlockDeleting(rect);
			if (this.notifyHandler != null)
			{
				this.notifyHandler(this, new BlockDeletingEventArgs(rect));
			}
			MoveTo(rect.Left, rect.Top);
			string text = lines[rect.Top];
			int length = text.Length;
			int num = rect.Left - length;
			if (rect.Top == rect.Bottom && num > 0)
			{
				return false;
			}
			int num2 = rect.Bottom;
			if (num2 == lines.Count)
			{
				num2--;
			}
			string[] array = new string[num2 - rect.Top + 1];
			AddUndo(UndoOperation.DeleteBlock, array);
			for (int num3 = rect.Bottom - 1; num3 > rect.Top; num3--)
			{
				array[num3 - rect.Top] = lines[num3];
				lines.RemoveAt(num3);
			}
			if (rect.Left < length)
			{
				length = ((rect.Top != rect.Bottom) ? (text.Length - rect.Left) : (Math.Min(rect.Right, length) - rect.Left));
				if (array.Length != 0)
				{
					array[0] = text.Substring(rect.Left, length);
				}
				if (rect.Top < lines.Count)
				{
					lines[rect.Top] = text.Remove(rect.Left, length);
				}
			}
			else if (array.Length != 0)
			{
				array[0] = string.Empty;
			}
			if (rect.Top != rect.Bottom)
			{
				text = lines[rect.Top + 1];
				length = Math.Min(text.Length, rect.Right);
				if (rect.Bottom - rect.Top < array.Length)
				{
					array[rect.Bottom - rect.Top] = text.Substring(0, length);
				}
				if (rect.Top < lines.Count)
				{
					lines[rect.Top] = lines[rect.Top] + ((num > 0) ? new string(' ', num) : string.Empty) + text.Remove(0, length);
					if (rect.Top + 1 < lines.Count)
					{
						lines.RemoveAt(rect.Top + 1);
					}
				}
			}
			PositionChanged(UpdateReason.DeleteBlock, -(rect.Right - rect.Left), -(rect.Bottom - rect.Top));
			LinesChanged(rect.Top, int.MaxValue, modified: true);
		}
		finally
		{
			stringsUpdateCount--;
			EndUpdate();
		}
		return true;
	}

	public bool BreakLine()
	{
		if (readOnly || IsLineReadonly(position.Y))
		{
			return false;
		}
		if (maxLength > 0)
		{
			string str = Lines.LineTerminator;
			StripToMaxLength(ref str);
			if (str == string.Empty)
			{
				return false;
			}
		}
		if (position.Y == 0 && lines.Count == 0)
		{
			lines.Add(string.Empty);
		}
		if (singleLineMode)
		{
			return false;
		}
		if (position.Y >= lines.Count)
		{
			if ((NavigateOptions.BeyondEof & navigateOptions) == 0)
			{
				return false;
			}
			EnsurePosInsideText();
		}
		BeginUpdate(UpdateReason.Break);
		stringsUpdateCount++;
		try
		{
			string text = lines[position.Y];
			int length = text.Length;
			if (position.X < length)
			{
				lines.Insert(position.Y + 1, text.Substring(position.X, length - position.X));
				lines[position.Y] = text.Substring(0, position.X);
			}
			else
			{
				lines.Insert(position.Y + 1, string.Empty);
			}
			AddUndo(UndoOperation.Break, null);
			LinesChanged(position.Y, int.MaxValue, modified: true);
			PositionChanged(UpdateReason.Break, -position.X, 1);
			if ((state & NotifyState.Undo) == 0)
			{
				RemoveTrailingSpaces(addUndo: true, lineEnd: true);
			}
		}
		finally
		{
			stringsUpdateCount--;
			EndUpdate();
		}
		return true;
	}

	public bool UnBreakLine()
	{
		if (readOnly || position.Y >= lines.Count - 1 || lines.Count <= 1 || IsLineReadonly(position.Y) || IsLineReadonly(position.Y + 1))
		{
			return false;
		}
		string text = lines[position.Y];
		if ((state & NotifyState.Undo) != 0 && position.X + 1 < text.Length && position.X + 1 >= text.TrimEnd(new char[0]).Length)
		{
			text = text.Substring(0, position.X);
		}
		int length = text.Length;
		if (position.X + 1 >= length)
		{
			BeginUpdate(UpdateReason.UnBreak);
			stringsUpdateCount++;
			try
			{
				length = position.X - length;
				if (length >= 0 && lines[position.Y + 1] != string.Empty)
				{
					lines[position.Y] = text + new string(' ', length) + lines[position.Y + 1];
				}
				else
				{
					lines[position.Y] = text + lines[position.Y + 1];
				}
				lines.RemoveAt(position.Y + 1);
				AddUndo(UndoOperation.UnBreak, null);
				LinesChanged(position.Y, int.MaxValue, modified: true);
				PositionChanged(UpdateReason.UnBreak, position.X, -1);
			}
			finally
			{
				stringsUpdateCount--;
				EndUpdate();
			}
			return true;
		}
		return false;
	}

	public bool NewLine()
	{
		if (singleLineMode)
		{
			return false;
		}
		BeginUpdate(UpdateReason.Break);
		try
		{
			bool result = BreakLine();
			MoveTo(0, Position.Y + 1);
			IndentLine();
			return result;
		}
		finally
		{
			EndUpdate();
		}
	}

	public bool NewLineAbove()
	{
		if (singleLineMode)
		{
			return false;
		}
		BeginUpdate(UpdateReason.Break);
		try
		{
			MoveToChar(0);
			bool result = BreakLine();
			string autoIndent = GetAutoIndent();
			if (autoIndent != string.Empty)
			{
				Insert(autoIndent);
			}
			return result;
		}
		finally
		{
			EndUpdate();
		}
	}

	public bool NewLineBelow()
	{
		if (singleLineMode)
		{
			return false;
		}
		BeginUpdate(UpdateReason.Break);
		try
		{
			MoveToChar(Lines[Position.Y].Length);
			bool result = BreakLine();
			MoveTo(0, Position.Y + 1);
			string autoIndent = GetAutoIndent();
			if (autoIndent != string.Empty)
			{
				Insert(autoIndent);
			}
			return result;
		}
		finally
		{
			EndUpdate();
		}
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

	public virtual void ResetReadonly()
	{
		Readonly = false;
	}

	public void ResetSingleLineMode()
	{
		SingleLineMode = false;
	}

	public void Notification(object sender, EventArgs e)
	{
		if (sender is ISyntaxStrings)
		{
			StringsChanged(sender, e);
		}
		else if (sender is ILexer)
		{
			LexerChanged();
		}
	}

	public int BeginUpdate(UpdateReason reason)
	{
		if (updateCount == 0)
		{
			if (reason != UpdateReason.Other && reason != 0)
			{
				if (bracesOptions != 0)
				{
					DoUnhighlightBraces();
				}
				UnhighlightSyntaxErrors(syntaxErrors, position.Y);
				needFmtTimer = fmtTimer != null && fmtTimer.Enabled;
				EndFmtTimer();
			}
			if (reason != UpdateReason.Other)
			{
				TempUnhighlightBraces();
			}
			insertStr = string.Empty;
			state = NotifyState.None;
			firstChanged = -1;
			lastChanged = -1;
			oldPosition = position;
			selectBlockRect = Rectangle.Empty;
			this.reason = reason;
			undoFlag = UndoFlags.FirstTime;
			count = lines.Count;
		}
		updateCount++;
		return updateCount;
	}

	public int BeginUpdate()
	{
		return BeginUpdate(UpdateReason.Other);
	}

	public int EndUpdate()
	{
		updateCount--;
		if (updateCount == 0)
		{
			if (position != oldPosition)
			{
				state |= NotifyState.PositionChanged;
			}
			if ((state & NotifyState.Edit) != 0)
			{
				if (!modified)
				{
					state |= NotifyState.ModifiedChanged;
				}
				modified = true;
				if (firstChanged >= 0)
				{
					lastParsed = Math.Min(lastParsed, firstChanged);
				}
			}
			if ((state & NotifyState.Edit) != 0 || (state & NotifyState.ModifiedChanged) != 0)
			{
				ClearTrackChanges();
			}
			if (lines.Count != count)
			{
				state |= NotifyState.CountChanged;
			}
			if ((state & NotifyState.Undo) == 0 && ((state & NotifyState.Edit) != 0 || ((state & NotifyState.PositionChanged) != 0 && UndoNavigations)))
			{
				ClearRedo();
			}
			if (state != 0)
			{
				bool flag = modified;
				if (UndoAfterSave)
				{
					if (undoList.Count > saveModifiedIdx)
					{
						modified = GetModified(undoList, saveModifiedIdx + 1, saved: true);
					}
					else
					{
						modified = GetModified(redoList, redoList.Count + undoList.Count - (saveModifiedIdx + 1), saved: false);
					}
				}
				else
				{
					modified = GetModified(undoList, 0, saved: true);
				}
				if (flag != modified)
				{
					state |= NotifyState.ModifiedChanged;
				}
				bool flag2 = needFmtTimer || (state & NotifyState.Edit) != 0 || (state & NotifyState.SyntaxChanged) != 0 || (state & NotifyState.StringsChanged) != 0;
				bool flag3 = position.Y != oldPosition.Y || (state & NotifyState.CountChanged) != 0;
				if (flag2 || flag3)
				{
					updateCount++;
					try
					{
						StartFmtTimer(flag2, flag3);
					}
					finally
					{
						updateCount--;
					}
				}
				needFmtTimer = false;
				Notify();
			}
			if (bracesOptions != 0 && (state & NotifyState.PositionChanged) != 0 && (state & NotifyState.Edit) == 0)
			{
				DoHighlightBraces();
			}
			if (checkSpelling && (state & NotifyState.PositionChanged) != 0 && spellSkipPt.Y >= 0 && spellSkipPt.X >= 0 && spellSkipPt.Y == oldPosition.Y && oldPosition.Y != position.Y)
			{
				IStrItem item = lines.GetItem(oldPosition.Y);
				if (item != null)
				{
					DoCheckSpelling(item, oldPosition.Y, withUpdate: true);
				}
			}
		}
		return updateCount;
	}

	public void AddNotifier(INotifier sender)
	{
		notifyHandler += sender.Notification;
	}

	public void RemoveNotifier(INotifier sender)
	{
		notifyHandler -= sender.Notification;
	}

	public void Notify()
	{
		if (this.notifyHandler != null)
		{
			this.notifyHandler(this, EventArgs.Empty);
		}
	}

	public void LinesChanged(int first, int last)
	{
		LinesChanged(first, last, modified: false);
	}

	public void LinesChanged(int first, int last, bool modified)
	{
		if (firstChanged == -1)
		{
			firstChanged = first;
		}
		else
		{
			firstChanged = Math.Min(firstChanged, first);
		}
		lastChanged = Math.Max(lastChanged, last);
		if (modified)
		{
			state |= NotifyState.Edit | NotifyState.Modified;
		}
	}

	private void ApplyUndoLimit()
	{
		if (undoLimit != 0 && undoList.Count >= undoLimit)
		{
			int num = Math.Min(undoLimitDelta + (undoList.Count - undoLimit), undoList.Count);
			for (int i = 0; i < num; i++)
			{
				undoList.RemoveAt(0);
			}
			saveModifiedIdx -= num;
			if (saveModifiedIdx < 0)
			{
				saveModifiedIdx = -1;
				modified = GetModified(undoList, 0, saved: true);
			}
		}
	}

	private bool SkipNavigations(ArrayList list)
	{
		if (list.Count != 0)
		{
			UndoData undoData = (UndoData)list[list.Count - 1];
			if (undoData.Operation == UndoOperation.Navigate && undoData.Data == null)
			{
				BeginUpdate(UpdateReason.Navigate);
				try
				{
					while (list.Count > 0)
					{
						undoData = (UndoData)list[list.Count - 1];
						if (undoData.Operation == UndoOperation.Navigate && undoData.Data == null)
						{
							Undo(undoData);
							list.RemoveAt(list.Count - 1);
							continue;
						}
						break;
					}
				}
				finally
				{
					EndUpdate();
				}
			}
		}
		return list.Count == 0;
	}

	protected ArrayList GetUndoList()
	{
		if (redo)
		{
			return redoList;
		}
		return undoList;
	}

	protected internal bool IsEmptyUndoList(ArrayList list)
	{
		if (UndoNavigations)
		{
			return list.Count == 0;
		}
		for (int num = list.Count - 1; num >= 0; num--)
		{
			UndoData undoData = (UndoData)list[num];
			if (undoData.Operation != UndoOperation.Navigate)
			{
				return false;
			}
		}
		return true;
	}

	protected internal void AddUndo(UndoOperation operation, object data)
	{
		if (AllowUndo && undoUpdateCount == 0)
		{
			UndoData undoData = new UndoData(operation, data);
			undoData.Position = position;
			undoData.Reason = reason;
			undoData.UpdateCount = lockUndoCount;
			undoData.UndoFlag = undoFlag;
			GetUndoList().Add(undoData);
			if (!redo && undoLimit != 0 && undoList.Count >= undoLimit)
			{
				ApplyUndoLimit();
			}
			undoFlag = UndoFlags.None;
		}
	}

	public void Undo(UndoData undoData)
	{
		if (!UndoNavigations && (undoData.Operation <= UndoOperation.DeleteBlock || undoData.Operation == UndoOperation.NavigateEx))
		{
			position = undoData.Position;
		}
		undoFlag = (((undoData.UndoFlag & UndoFlags.Saved) != 0) ? (undoFlag | UndoFlags.Saved) : (undoFlag & ~UndoFlags.Saved));
		switch (undoData.Operation)
		{
		case UndoOperation.Insert:
			DeleteRight((int)undoData.Data);
			break;
		case UndoOperation.Delete:
			Insert((string)undoData.Data);
			break;
		case UndoOperation.InsertBlock:
			DeleteBlock(((Point)undoData.Data).X);
			break;
		case UndoOperation.DeleteBlock:
			InsertBlock((string[])undoData.Data, reason == UpdateReason.Delete || reason == UpdateReason.DeleteBlock);
			break;
		case UndoOperation.Break:
			UnBreakLine();
			break;
		case UndoOperation.UnBreak:
			BreakLine();
			break;
		case UndoOperation.Navigate:
			if (undoData.Data != null)
			{
				Navigate(-((Point)undoData.Data).X, -((Point)undoData.Data).Y);
			}
			else
			{
				AddUndo(UndoOperation.Navigate, null);
			}
			break;
		case UndoOperation.NavigateEx:
			AddUndo(UndoOperation.NavigateEx, undoData.Data);
			break;
		}
	}

	public void Undo()
	{
		ArrayList arrayList = GetUndoList();
		if (!AllowUndo || arrayList.Count <= 0 || readOnly)
		{
			return;
		}
		redo = !redo;
		try
		{
			if (SkipNavigations(arrayList))
			{
				return;
			}
			UndoData undoData = (UndoData)arrayList[arrayList.Count - 1];
			UpdateReason updateReason = undoData.Reason;
			bool flag = undoData.Operation == UndoOperation.UndoBlock;
			BeginUpdate(updateReason);
			try
			{
				state |= NotifyState.Undo;
				while (arrayList.Count > 0)
				{
					undoData = (UndoData)arrayList[arrayList.Count - 1];
					int num = undoData.UpdateCount;
					if (updateReason != undoData.Reason && num == 0)
					{
						break;
					}
					bool flag2 = (undoData.UndoFlag & UndoFlags.FirstTime) != 0;
					UndoOperation operation = undoData.Operation;
					if (operation == UndoOperation.UndoBlock && !flag)
					{
						break;
					}
					Undo(undoData);
					arrayList.RemoveAt(arrayList.Count - 1);
					if (flag2 && ((!GroupUndo && num <= 0) || ((undoOptions & UndoOptions.SingleBreakUndo) != 0 && (updateReason == UpdateReason.Break || updateReason == UpdateReason.UnBreak))))
					{
						break;
					}
				}
			}
			finally
			{
				EndUpdate();
			}
		}
		finally
		{
			redo = !redo;
		}
	}

	public void Redo()
	{
		redo = !redo;
		try
		{
			Undo();
		}
		finally
		{
			redo = !redo;
		}
	}

	public bool CanUndo()
	{
		if (AllowUndo)
		{
			return !IsEmptyUndoList(undoList);
		}
		return false;
	}

	public bool CanRedo()
	{
		if (AllowUndo)
		{
			return !IsEmptyUndoList(redoList);
		}
		return false;
	}

	public void ClearUndo()
	{
		undoList.Clear();
	}

	public void ClearRedo()
	{
		redoList.Clear();
		if (saveModifiedIdx >= undoList.Count)
		{
			saveModifiedIdx = -1;
		}
	}

	public int DisableUndo()
	{
		undoUpdateCount++;
		return undoUpdateCount;
	}

	public int EnableUndo()
	{
		undoUpdateCount--;
		return undoUpdateCount;
	}

	public int BeginUndoUpdate()
	{
		lockUndoCount++;
		return lockUndoCount;
	}

	public int EndUndoUpdate()
	{
		lockUndoCount--;
		if (lockUndoCount == 0)
		{
			AddUndo(UndoOperation.UndoBlock, null);
		}
		return lockUndoCount;
	}

	public virtual void ResetUndoLimit()
	{
		UndoLimit = 0;
	}

	public virtual void ResetUndoOptions()
	{
		UndoOptions = EditConsts.DefaultUndoOptions;
	}

	private bool FindTrackChange(SortList list, int line, out int idx)
	{
		return list.FindLast(line, out idx, trackChangeComparer);
	}

	private void AddTrackChange(SortList list, int line, bool saved)
	{
		if (FindTrackChange(list, line, out var idx))
		{
			((TrackChange)list[idx]).Saved &= saved;
		}
		else
		{
			list.Insert(idx, new TrackChange(line, saved));
		}
	}

	protected void TrackChanges(ArrayList uList, SortList list)
	{
		for (int i = 0; i < uList.Count; i++)
		{
			UndoData undoData = (UndoData)uList[i];
			int y = undoData.Position.Y;
			int num = 0;
			bool flag = true;
			switch (undoData.Operation)
			{
			case UndoOperation.Insert:
			case UndoOperation.Delete:
				num = 0;
				break;
			case UndoOperation.Break:
				num = 1;
				break;
			case UndoOperation.UnBreak:
				num = -1;
				break;
			case UndoOperation.InsertBlock:
				num = ((Point)undoData.Data).Y;
				break;
			case UndoOperation.DeleteBlock:
				num = -((string[])undoData.Data).Length + 1;
				break;
			default:
				flag = false;
				break;
			}
			if (!flag)
			{
				continue;
			}
			bool flag2 = (undoData.UndoFlag & UndoFlags.Saved) != 0;
			if (num != 0)
			{
				for (int num2 = list.Count - 1; num2 >= 0; num2--)
				{
					TrackChange trackChange = (TrackChange)list[num2];
					if (trackChange.Line < y)
					{
						break;
					}
					if (trackChange.Line + num < y)
					{
						flag2 &= trackChange.Saved;
						list.RemoveAt(num2);
					}
					else
					{
						trackChange.Line += num;
					}
				}
			}
			for (int j = 0; j <= Math.Max(num, 0); j++)
			{
				AddTrackChange(list, y + j, flag2);
			}
		}
	}

	protected void TrackChanges()
	{
		trackChanges = new Hashtable();
		SortList sortList = new SortList();
		TrackChanges(undoList, sortList);
		foreach (TrackChange item in sortList)
		{
			trackChanges[item.Line] = item.Saved;
		}
		sortList.Clear();
	}

	protected void ClearTrackChanges()
	{
		if (trackChanges != null)
		{
			trackChanges.Clear();
		}
		trackChanges = null;
	}

	public virtual bool IsLineModified(int index, out bool saved)
	{
		if (trackChanges == null)
		{
			TrackChanges();
		}
		object obj = trackChanges[index];
		saved = obj != null && (bool)obj;
		return obj != null;
	}

	private void LexerChanged()
	{
		BeginUpdate(UpdateReason.Other);
		try
		{
			lastParsed = 0;
			state |= NotifyState.SyntaxChanged;
			UpdateParsed(0, lines.Count - 1);
			braceList.Clear();
			UnhighlightSyntaxErrors(syntaxErrors);
			UnhighlightCodeSnippets();
		}
		finally
		{
			EndUpdate();
		}
	}

	protected void UpdateParsed(int fromIndex, int toIndex)
	{
		for (int i = fromIndex; i <= Math.Min(toIndex, lines.Count - 1); i++)
		{
			lines.GetItem(i).State &= ~StrItemState.Parsed;
		}
	}

	protected internal void SetLastParsed(int index)
	{
		lastParsed = index;
		firstChanged = -1;
	}

	public bool SaveFile(string fileName)
	{
		return SaveFile(fileName, null);
	}

	public bool SaveFile(string fileName, Encoding encoding)
	{
		return lines.SaveFile(fileName, encoding);
	}

	public bool SaveStream(TextWriter writer)
	{
		return lines.SaveStream(writer);
	}

	public bool SaveStream(Stream stream)
	{
		return SaveStream(stream, null);
	}

	public bool SaveStream(Stream stream, Encoding encoding)
	{
		TextWriter textWriter = ((encoding != null) ? new StreamWriter(stream, encoding) : new StreamWriter(stream));
		try
		{
			return SaveStream(textWriter);
		}
		finally
		{
			textWriter.Close();
		}
	}

	public bool LoadFile(string fileName)
	{
		return LoadFile(fileName, null);
	}

	public bool LoadFile(string fileName, Encoding encoding)
	{
		return lines.LoadFile(fileName, encoding);
	}

	public bool LoadStream(TextReader reader)
	{
		return lines.LoadStream(reader);
	}

	public bool LoadStream(Stream stream)
	{
		return LoadStream(stream, null);
	}

	public bool LoadStream(Stream stream, Encoding encoding)
	{
		TextReader textReader = ((encoding != null) ? new StreamReader(stream, encoding) : new StreamReader(stream));
		try
		{
			return LoadStream(textReader);
		}
		finally
		{
			textReader.Close();
		}
	}

	private void InitUrlTable()
	{
		if (urlTable == null)
		{
			urlTable = new Hashtable();
			for (char c = 'A'; c <= 'Z'; c = (char)(c + 1))
			{
				urlTable.Add(c, true);
			}
			for (char c2 = 'a'; c2 <= 'z'; c2 = (char)(c2 + 1))
			{
				urlTable.Add(c2, true);
			}
			for (char c3 = '0'; c3 <= '9'; c3 = (char)(c3 + 1))
			{
				urlTable.Add(c3, false);
			}
			char[] array = EditConsts.DefaultUrlChars.ToCharArray();
			foreach (char c4 in array)
			{
				urlTable.Add(c4, false);
			}
		}
	}

	protected internal void DoHighlightUrls(IStrItem item, int line)
	{
		string @string = item.String;
		int length = @string.Length;
		int i = 0;
		int num = 0;
		int num2 = 0;
		InitUrlTable();
		while (i < length)
		{
			num2 = i;
			for (; i < length; i++)
			{
				object obj = urlTable[@string[i]];
				if (obj != null && (bool)obj)
				{
					break;
				}
			}
			for (num = i; num < length && urlTable.ContainsKey(@string[num]); num++)
			{
			}
			if (num != i && IsHyperText(@string.Substring(i, num - i)))
			{
				item.SetColorFlag(i, num - i, ColorFlags.HyperText, setFlag: true);
			}
			i = num;
			if (i == num2)
			{
				i++;
			}
		}
	}

	public bool IsHyperText(string text)
	{
		bool flag = HyperText.IsHyperText(text);
		if (this.CheckHyperText != null)
		{
			hyperTextArgs.Text = text;
			hyperTextArgs.IsHyperText = flag;
			this.CheckHyperText(this, hyperTextArgs);
			flag = hyperTextArgs.IsHyperText;
		}
		return flag;
	}

	public virtual void ResetHighlightUrls()
	{
		HighlightUrls = false;
	}

	private void InitSpellTable()
	{
		if (spellTable == null)
		{
			spellTable = new Hashtable();
			char[] array = EditConsts.DefaultSpellDelimiters.ToCharArray();
			foreach (char c in array)
			{
				spellTable.Add(c, c);
			}
		}
	}

	private void DoCheckSpelling(IStrItem item, int line, int start, int end, bool withUpdate)
	{
		if (start != end && !IsWordCorrect(item.String.Substring(start, end - start), item.ColorData[start]))
		{
			item.SetColorFlag(start, end - start, ColorFlags.MisSpelledWord, setFlag: true);
			if (withUpdate)
			{
				LinesChanged(line, line);
				state |= NotifyState.BlockChanged;
			}
		}
	}

	protected internal void DoCheckSpelling(IStrItem item, int line, bool withUpdate)
	{
		string @string = item.String;
		if (!(@string != string.Empty))
		{
			return;
		}
		InitSpellTable();
		bool flag = (state & NotifyState.Edit) != 0 && position.Y == line;
		spellSkipPt = (flag ? position : new Point(-1, -1));
		int num = 0;
		bool flag2 = false;
		bool flag3 = false;
		int length = @string.Length;
		if (withUpdate)
		{
			BeginUpdate(UpdateReason.Other);
		}
		try
		{
			for (int i = 0; i < length; i++)
			{
				flag3 = spellTable.ContainsKey(@string[i]) || lines.IsDelimiter(@string, i);
				if (i == 0)
				{
					flag2 = flag3;
				}
				if (flag2 != flag3)
				{
					if (flag3 && (!flag || spellSkipPt.X <= num || spellSkipPt.X > i))
					{
						DoCheckSpelling(item, line, num, i, withUpdate);
					}
					flag2 = flag3;
					num = i;
				}
			}
			if (!flag2 && (!flag || spellSkipPt.X <= num || spellSkipPt.X > length))
			{
				DoCheckSpelling(item, line, num, length, withUpdate);
			}
		}
		finally
		{
			if (withUpdate)
			{
				EndUpdate();
			}
		}
	}

	protected internal bool IsWordCorrectEventAssigned()
	{
		return this.WordSpell != null;
	}

	public bool IsWordCorrect(string text)
	{
		return IsWordCorrect(text, 0);
	}

	public bool IsWordCorrect(string text, int colorStyle)
	{
		bool flag = true;
		if (this.WordSpell != null)
		{
			wordSpellArgs.Text = text;
			wordSpellArgs.Correct = flag;
			wordSpellArgs.ColorStyle = colorStyle;
			this.WordSpell(this, wordSpellArgs);
			flag = wordSpellArgs.Correct;
		}
		return flag;
	}

	public virtual void ResetCheckSpelling()
	{
		CheckSpelling = false;
	}

	private void StartBracesTimer()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		EndBracesTimer();
		if (bracesTimer == null)
		{
			bracesTimer = new Timer();
			bracesTimer.Enabled = false;
			bracesTimer.Interval = EditConsts.DefaultBracesDelay;
			bracesTimer.Tick += OnUnhighlightBraces;
		}
		bracesTimer.Enabled = true;
	}

	private void EndBracesTimer()
	{
		if (bracesTimer != null)
		{
			bracesTimer.Enabled = false;
		}
	}

	private void HighlightCodeSnippets()
	{
		for (int i = 0; i < codeSnippets.Count; i++)
		{
			if (codeSnippets.IsFirstSnippet(i))
			{
				IRange range = (IRange)codeSnippets[i];
				SetColorFlag(new Rectangle(range.StartPoint, new Size(range.EndPoint.X - range.StartPoint.X, range.EndPoint.Y - range.StartPoint.Y)), ColorFlags.CodeSnippet, setFlag: true);
			}
		}
	}

	private void DoParseBraces(IStrItem item, int line)
	{
		braceList.RemoveBraces(line);
		string @string = item.String;
		short[] colorData = item.ColorData;
		for (int i = 0; i < @string.Length; i++)
		{
			if (lexer != null && lexer.Scheme.IsPlainText((byte)colorData[i] - 1))
			{
				continue;
			}
			char value = @string[i];
			int num = Array.IndexOf(openBraces, value);
			if (num >= 0)
			{
				braceList.AddBrace(line, i, num, open: true);
				continue;
			}
			num = Array.IndexOf(closingBraces, value);
			if (num >= 0)
			{
				braceList.AddBrace(line, i, num, open: false);
			}
		}
	}

	private bool DoHighlightBraces(Point position)
	{
		Point point = openBrace;
		Point point2 = closingBrace;
		Point point3 = position;
		openBrace = new Point(-1, -1);
		closingBrace = new Point(-1, -1);
		bool flag = false;
		bool open = false;
		if (position.X > 0 && braceList.FindBrace(new Point(position.X - 1, position.Y), out open) && !open)
		{
			point3.X--;
		}
		else if (braceList.FindBrace(position, out open) && open)
		{
			point3.X++;
		}
		int braceIndex = -1;
		if (FindOpenBrace(ref point3, ref braceIndex))
		{
			Point point4 = point3;
			point4.X++;
			if (FindClosingBrace(ref point4, ref braceIndex) && ((bracesOptions & BracesOptions.HighlightBounds) == 0 || position.Equals((object?)point3) || position.Equals((object?)new Point(point4.X + 1, point4.Y))))
			{
				openBrace = point3;
				closingBrace = point4;
				flag = true;
			}
		}
		bool flag2 = openBrace != point || closingBrace != point2;
		if (flag2)
		{
			UpdateBrace(point, setOn: false);
			UpdateBrace(point2, setOn: false);
			UpdateBraces();
		}
		if (flag && (bracesOptions & BracesOptions.TempHighlight) != 0)
		{
			StartBracesTimer();
		}
		return flag2;
	}

	private void UpdateBraces()
	{
		UpdateBrace(openBrace, setOn: true);
		UpdateBrace(closingBrace, setOn: true);
	}

	private void UpdateTempBraces(bool setOn)
	{
		if (tempBraceRects != null)
		{
			Rectangle[] array = tempBraceRects;
			foreach (Rectangle rect in array)
			{
				SetColorFlag(rect, ColorFlags.Brace, setOn);
			}
		}
	}

	private void UpdateBrace(Point point, bool setOn)
	{
		SetColorFlag(point, 1, ColorFlags.Brace, setOn);
	}

	private bool FindClosingBrace(ref Point position, ref int braceIndex)
	{
		if (closingBraces == null)
		{
			return false;
		}
		return braceList.FindClosingBrace(ref position, ref braceIndex, closingBraces.Length);
	}

	private bool FindOpenBrace(ref Point position, ref int braceIndex)
	{
		if (openBraces == null)
		{
			return false;
		}
		return braceList.FindOpenBrace(ref position, ref braceIndex, openBraces.Length);
	}

	protected internal void DoUnhighlightBraces()
	{
		UpdateBrace(openBrace, setOn: false);
		UpdateBrace(closingBrace, setOn: false);
		openBrace = new Point(-1, -1);
		closingBrace = new Point(-1, -1);
		EndBracesTimer();
	}

	protected internal bool DoHighlightBraces()
	{
		return DoHighlightBraces(position);
	}

	protected internal bool IsEqualArrays(char[] array1, char[] array2)
	{
		bool flag = array1.Length == array2.Length;
		if (flag)
		{
			for (int i = 0; i < array1.Length; i++)
			{
				flag = array1[i] == array2[i];
				if (!flag)
				{
					break;
				}
			}
		}
		return flag;
	}

	protected void OnUnhighlightBraces(object source, EventArgs e)
	{
		if (bracesOptions != 0)
		{
			DoUnhighlightBraces();
		}
		TempUnhighlightBraces();
	}

	public bool FindClosingBrace(ref Point position)
	{
		int braceIndex = -1;
		return FindClosingBrace(ref position, ref braceIndex);
	}

	public bool FindClosingBrace(ref int x, ref int y)
	{
		Point point = new Point(x, y);
		if (FindClosingBrace(ref point))
		{
			x = point.X;
			y = point.Y;
			return true;
		}
		return false;
	}

	public bool FindOpenBrace(ref Point position)
	{
		int braceIndex = -1;
		return FindOpenBrace(ref position, ref braceIndex);
	}

	public bool FindOpenBrace(ref int x, ref int y)
	{
		Point point = new Point(x, y);
		if (FindOpenBrace(ref point))
		{
			x = point.X;
			y = point.Y;
			return true;
		}
		return false;
	}

	public void TempHighlightBraces(Rectangle[] rectangles)
	{
		TempBraceRects = rectangles;
	}

	public void TempUnhighlightBraces()
	{
		TempBraceRects = null;
		UpdateBraces();
	}

	public virtual void ResetOpenBraces()
	{
		OpenBraces = EditConsts.DefaultOpenBraces;
	}

	public virtual void ResetClosingBraces()
	{
		ClosingBraces = EditConsts.DefaultClosingBraces;
	}

	public virtual void ResetBracesOptions()
	{
		BracesOptions = BracesOptions.None;
	}

	public bool ShouldSerializeOpenBraces()
	{
		return !IsEqualArrays(openBraces, EditConsts.DefaultOpenBraces);
	}

	public bool ShouldSerializeClosingBraces()
	{
		return !IsEqualArrays(closingBraces, EditConsts.DefaultClosingBraces);
	}

	public bool ProcessAutoComplete(out string code)
	{
		code = string.Empty;
		if (NeedAutoComplete())
		{
			return ((ISyntaxParser)lexer).ProcessAutoComplete(oldPosition, out code);
		}
		return false;
	}

	public bool ShouldSerializeFileName()
	{
		return fileName != string.Empty;
	}

	public bool ShouldSerializeText()
	{
		return lines.Count > 0;
	}

	public bool ShouldSerializeLexer()
	{
		return Lexer != null;
	}

	public bool ShouldSerializeNavigateOptions()
	{
		return navigateOptions != EditConsts.DefaultNavigateOptions;
	}

	public bool ShouldSerializeUndoOptions()
	{
		return undoOptions != EditConsts.DefaultUndoOptions;
	}

	public bool ShouldSerializeIndentOptions()
	{
		return indentOptions != EditConsts.DefaultIndentOptions;
	}

	public void FormatText()
	{
		needFormatting = false;
		EndFmtTimer();
		if (NeedReparseText())
		{
			DoFormatText();
		}
	}

	public bool NeedFormatText()
	{
		if (lexer is ISyntaxParser)
		{
			return (((ISyntaxParser)lexer).Options & SyntaxOptions.SmartIndent) != 0;
		}
		return false;
	}

	public bool NeedOutlineText()
	{
		if (lexer is ISyntaxParser)
		{
			return (((ISyntaxParser)lexer).Options & SyntaxOptions.Outline) != 0;
		}
		return false;
	}

	public bool NeedReparseText()
	{
		if (lexer is ISyntaxParser)
		{
			return ((ISyntaxParser)lexer).Options != SyntaxOptions.None;
		}
		return false;
	}

	public bool NeedAutoComplete()
	{
		if (lexer is ISyntaxParser)
		{
			return (((ISyntaxParser)lexer).Options & SyntaxOptions.AutoComplete) != 0;
		}
		return false;
	}

	public bool NeedReparseTextOnLineChange()
	{
		if (lexer is ISyntaxParser)
		{
			return (((ISyntaxParser)lexer).Options & SyntaxOptions.ReparseOnLineChange) != 0;
		}
		return false;
	}

	public bool NeedCodeCompletion()
	{
		if (lexer is ISyntaxParser)
		{
			return (((ISyntaxParser)lexer).Options & SyntaxOptions.CodeCompletion) != 0;
		}
		return false;
	}

	public bool NeedSyntaxErrors()
	{
		return false;
	}

	public bool NeedQuickInfoTips()
	{
		if (lexer is ISyntaxParser)
		{
			return (((ISyntaxParser)lexer).Options & SyntaxOptions.QuickInfoTips) != 0;
		}
		return false;
	}
}
