using System.Drawing;

namespace Stimulsoft.Editor;

public class XmlTextSourceInfo
{
	private TextSource owner;

	private string fileName;

	private Point position;

	private NavigateOptions navigateOptions = EditConsts.DefaultNavigateOptions;

	private bool readOnly;

	private bool overWrite;

	private IndentOptions indentOptions = EditConsts.DefaultIndentOptions;

	private BookMarkEx[] bookMarks = new BookMarkEx[0];

	private LStyle[] lineStyles = new LStyle[0];

	private UndoOptions undoOptions = EditConsts.DefaultUndoOptions;

	private int undoLimit;

	private bool highlightUrls;

	private bool checkSpelling;

	private int maxLength;

	private XmlLexerInfo lexer;

	private XmlSyntaxStringsInfo syntaxStrings;

	private BracesOptions bracesOptions;

	private char[] openBraces = new char[0];

	private char[] closingBraces = new char[0];

	public BracesOptions BracesOptions
	{
		get
		{
			if (owner == null)
			{
				return bracesOptions;
			}
			return owner.BracesOptions;
		}
		set
		{
			bracesOptions = value;
			if (owner != null)
			{
				owner.BracesOptions = value;
			}
		}
	}

	public int MaxLength
	{
		get
		{
			if (owner == null)
			{
				return maxLength;
			}
			return owner.MaxLength;
		}
		set
		{
			maxLength = value;
			if (owner != null)
			{
				owner.MaxLength = value;
			}
		}
	}

	public string FileName
	{
		get
		{
			if (owner == null)
			{
				return fileName;
			}
			return owner.FileName;
		}
		set
		{
			fileName = value;
			if (owner != null)
			{
				owner.FileName = value;
			}
		}
	}

	public Point Position
	{
		get
		{
			if (owner == null)
			{
				return position;
			}
			return owner.Position;
		}
		set
		{
			position = value;
			if (owner != null)
			{
				owner.Position = value;
			}
		}
	}

	public BookMarkEx[] BookMarks
	{
		get
		{
			if (owner != null)
			{
				BookMarkEx[] array = new BookMarkEx[owner.BookMarks.Count];
				for (int i = 0; i < owner.BookMarks.Count; i++)
				{
					IBookMark bookMark = owner.BookMarks[i];
					if (bookMark is IBookMarkEx)
					{
						array[i] = new BookMarkEx(bookMark.Line, bookMark.Pos, bookMark.Index, ((IBookMarkEx)bookMark).Name, ((IBookMarkEx)bookMark).Description, ((IBookMarkEx)bookMark).Url);
					}
					else
					{
						array[i] = new BookMarkEx(bookMark.Line, bookMark.Pos, bookMark.Index, null, null, null);
					}
				}
				return array;
			}
			return bookMarks;
		}
		set
		{
			bookMarks = value;
			if (owner == null)
			{
				return;
			}
			owner.BookMarks.Clear();
			foreach (IBookMarkEx bookMarkEx in value)
			{
				if (bookMarkEx.Name != null || bookMarkEx.Description != null || bookMarkEx.Url != null)
				{
					owner.BookMarks.Add(new BookMarkEx(bookMarkEx.Line, bookMarkEx.Pos, bookMarkEx.Index, bookMarkEx.Name, bookMarkEx.Description, bookMarkEx.Url));
				}
				else
				{
					owner.BookMarks.Add(new BookMark(bookMarkEx.Line, bookMarkEx.Pos, bookMarkEx.Index));
				}
			}
		}
	}

	public LStyle[] LineStyles
	{
		get
		{
			if (owner != null)
			{
				LStyle[] array = new LStyle[owner.LineStyles.Count];
				for (int i = 0; i < owner.LineStyles.Count; i++)
				{
					array[i] = ((LineStyles)owner.LineStyles).GetLStyle(i);
				}
				return array;
			}
			return lineStyles;
		}
		set
		{
			this.lineStyles = value;
			if (owner != null)
			{
				LineStyles lineStyles = (LineStyles)owner.LineStyles;
				lineStyles.Clear();
				foreach (LStyle lStyle in value)
				{
					lineStyles.SetLineStyle(lStyle.Position, lStyle.Range, lStyle.Priority, lStyle.Index);
				}
			}
		}
	}

	public char[] OpenBraces
	{
		get
		{
			if (owner == null)
			{
				return openBraces;
			}
			return owner.OpenBraces;
		}
		set
		{
			openBraces = value;
			if (owner != null)
			{
				owner.OpenBraces = value;
			}
		}
	}

	public char[] ClosingBraces
	{
		get
		{
			if (owner == null)
			{
				return closingBraces;
			}
			return owner.ClosingBraces;
		}
		set
		{
			closingBraces = value;
			if (owner != null)
			{
				owner.ClosingBraces = value;
			}
		}
	}

	public NavigateOptions NavigateOptions
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

	public bool ReadOnly
	{
		get
		{
			if (owner == null)
			{
				return readOnly;
			}
			return owner.Readonly;
		}
		set
		{
			readOnly = value;
			if (owner != null)
			{
				owner.Readonly = value;
			}
		}
	}

	public bool OverWrite
	{
		get
		{
			if (owner == null)
			{
				return overWrite;
			}
			return owner.OverWrite;
		}
		set
		{
			overWrite = value;
			if (owner != null)
			{
				owner.OverWrite = value;
			}
		}
	}

	public IndentOptions IndentOptions
	{
		get
		{
			if (owner == null)
			{
				return indentOptions;
			}
			return owner.IndentOptions;
		}
		set
		{
			indentOptions = value;
			if (owner != null)
			{
				owner.IndentOptions = value;
			}
		}
	}

	public UndoOptions UndoOptions
	{
		get
		{
			if (owner == null)
			{
				return undoOptions;
			}
			return owner.UndoOptions;
		}
		set
		{
			undoOptions = value;
			if (owner != null)
			{
				owner.UndoOptions = value;
			}
		}
	}

	public int UndoLimit
	{
		get
		{
			if (owner == null)
			{
				return undoLimit;
			}
			return owner.UndoLimit;
		}
		set
		{
			undoLimit = value;
			if (owner != null)
			{
				owner.UndoLimit = value;
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

	public bool CheckSpelling
	{
		get
		{
			if (owner == null)
			{
				return checkSpelling;
			}
			return owner.CheckSpelling;
		}
		set
		{
			checkSpelling = value;
			if (owner != null)
			{
				owner.CheckSpelling = value;
			}
		}
	}

	public XmlLexerInfo Lexer
	{
		get
		{
			if (owner == null || owner.Lexer == null)
			{
				return lexer;
			}
			return (XmlLexerInfo)((Lexer)owner.Lexer).XmlInfo;
		}
		set
		{
			lexer = value;
			if (owner != null && owner.Lexer != null && value != null)
			{
				((Lexer)owner.Lexer).XmlInfo = value;
			}
		}
	}

	public XmlSyntaxStringsInfo SyntaxStrings
	{
		get
		{
			if (owner == null)
			{
				return syntaxStrings;
			}
			return (XmlSyntaxStringsInfo)((SyntaxStrings)owner.Lines).XmlInfo;
		}
		set
		{
			syntaxStrings = value;
			if (owner != null && value != null)
			{
				((SyntaxStrings)owner.Lines).XmlInfo = value;
			}
		}
	}

	public XmlTextSourceInfo()
	{
	}

	public XmlTextSourceInfo(TextSource owner)
		: this()
	{
		this.owner = owner;
	}

	public virtual void Load()
	{
		if (owner != null)
		{
			fileName = FileName;
			position = Position;
			bookMarks = BookMarks;
			lineStyles = LineStyles;
			navigateOptions = NavigateOptions;
			readOnly = ReadOnly;
			overWrite = OverWrite;
			indentOptions = IndentOptions;
			undoOptions = UndoOptions;
			undoLimit = UndoLimit;
			highlightUrls = HighlightUrls;
			checkSpelling = CheckSpelling;
			bracesOptions = BracesOptions;
			maxLength = MaxLength;
			lexer = Lexer;
			syntaxStrings = SyntaxStrings;
			if (lexer != null)
			{
				lexer.Load();
			}
			if (syntaxStrings != null)
			{
				syntaxStrings.Load();
			}
			openBraces = OpenBraces;
			closingBraces = ClosingBraces;
		}
	}

	public virtual void FixupReferences(TextSource owner)
	{
		this.owner = owner;
		owner.BeginUpdate(UpdateReason.Other);
		try
		{
			FileName = fileName;
			Position = position;
			NavigateOptions = navigateOptions;
			ReadOnly = readOnly;
			OverWrite = overWrite;
			IndentOptions = indentOptions;
			UndoOptions = undoOptions;
			UndoLimit = undoLimit;
			HighlightUrls = highlightUrls;
			CheckSpelling = checkSpelling;
			Lexer = lexer;
			SyntaxStrings = syntaxStrings;
			BracesOptions = bracesOptions;
			MaxLength = maxLength;
			BookMarks = bookMarks;
			LineStyles = lineStyles;
			OpenBraces = openBraces;
			ClosingBraces = closingBraces;
		}
		finally
		{
			owner.EndUpdate();
		}
	}
}
