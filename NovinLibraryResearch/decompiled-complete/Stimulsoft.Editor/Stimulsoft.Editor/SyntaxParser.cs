using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.IO;
using System.Text;

namespace Stimulsoft.Editor;

[ToolboxItem(false)]
public class SyntaxParser : Parser, ISyntaxParser, IParser, ILexer, IImport
{
	private class LexerState
	{
		private Hashtable hash;

		private LexerProc stateProc;

		public Hashtable Hash => hash;

		public LexerProc StateProc
		{
			get
			{
				return stateProc;
			}
			set
			{
				stateProc = value;
			}
		}

		public LexerState()
		{
			hash = new Hashtable();
		}

		public void Add(char[] chars, LexerProc proc)
		{
			foreach (char c in chars)
			{
				hash[c] = proc;
			}
		}

		public LexerProc GetProc(char ch)
		{
			LexerProc lexerProc = (LexerProc)hash[ch];
			if (lexerProc == null)
			{
				lexerProc = stateProc;
			}
			return lexerProc;
		}

		public void Clear()
		{
			hash.Clear();
		}
	}

	private class LineNodeComparer : IComparer
	{
		public int Compare(object x, object y)
		{
			IRange range = ((ISyntaxNode)x).Range;
			int num = (int)y;
			if (num <= range.StartPoint.Y || num > range.EndPoint.Y)
			{
				return range.StartPoint.Y - num;
			}
			return 0;
		}
	}

	private class PointNodeComparer : IComparer
	{
		public int Compare(object x, object y)
		{
			IRange range = ((ISyntaxNode)x).Range;
			Point point = (Point)y;
			if ((point.Y <= range.StartPoint.Y && (point.Y != range.StartPoint.Y || point.X < range.StartPoint.X)) || (point.Y >= range.EndPoint.Y && (point.Y != range.EndPoint.Y || point.X > range.EndPoint.X)))
			{
				if (!point.Equals((object?)range.StartPoint))
				{
					if (range.StartPoint.Y != point.Y)
					{
						return range.StartPoint.Y - point.Y;
					}
					return range.StartPoint.X - point.X;
				}
				return 1;
			}
			return 0;
		}
	}

	private class NodeComparer : IComparer
	{
		public int Compare(object x, object y)
		{
			IRange range = ((ISyntaxNode)x).Range;
			IRange range2 = ((ISyntaxNode)y).Range;
			int num = range.StartPoint.Y - range2.StartPoint.Y;
			if (num == 0)
			{
				num = range.StartPoint.X - range2.StartPoint.X;
			}
			if (num == 0)
			{
				num = range.EndPoint.Y - range2.EndPoint.Y;
			}
			if (num == 0)
			{
				num = range.EndPoint.X - range2.EndPoint.X;
			}
			return num;
		}
	}

	private class ListMemberComparer : IComparer
	{
		private ISyntaxParser owner;

		public ListMemberComparer(ISyntaxParser owner)
		{
			this.owner = owner;
		}

		public int Compare(object x, object y)
		{
			return string.Compare(((IListMember)x).Name, ((IListMember)y).Name, !owner.CaseSensitive);
		}
	}

	private Hashtable lexerStates;

	private LexerState lexerState;

	private bool useScheme;

	private SyntaxOptions options;

	private ISyntaxTree syntaxTree;

	private ICodeCompletionRepository repository;

	private char[] codeCompletionChars;

	private char[] codeCompletionStopChars;

	private char[] smartFormatChars;

	private char[] autoIndentChars;

	protected IComparer lineNodeComparer;

	protected IComparer pointNodeComparer;

	protected IComparer nodeComparer;

	protected IComparer listMemberComparer;

	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	[Editor("QWhale.Design.FlagEnumerationEditor, QWhale.Editor", typeof(UITypeEditor))]
	[Description("Gets or sets a flags determining syntax parsing and formatting behavior.")]
	public virtual SyntaxOptions Options
	{
		get
		{
			return options;
		}
		set
		{
			options = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public ISyntaxTree SyntaxTree
	{
		get
		{
			return syntaxTree;
		}
		set
		{
			syntaxTree = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual bool CaseSensitive => false;

	[DefaultValue(false)]
	[Description("Gets or sets a boolean value that indicates whether \"ISyntaxParser\" should perform lexical analysis based on it's rules rather than using internal method.")]
	public bool UseScheme
	{
		get
		{
			return useScheme;
		}
		set
		{
			if (useScheme != value)
			{
				useScheme = value;
				Update();
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual ICodeCompletionRepository CompletionRepository
	{
		get
		{
			if (repository == null)
			{
				repository = CreateRepository();
			}
			return repository;
		}
		set
		{
			repository = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public ICodeSnippetsProvider CodeSnippets => CompletionRepository.GetCodeSnippets(Scheme.FileType);

	[Description("Gets or sets a collection of characters that initializes a code completion procedure when typing.")]
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

	[Description("Gets or sets a collection of characters that finalizes a code completion procedure when typing.")]
	public char[] CodeCompletionStopChars
	{
		get
		{
			return codeCompletionStopChars;
		}
		set
		{
			codeCompletionStopChars = value;
		}
	}

	[Description("Gets or sets a collection of characters that initializes a smart formatting procedure when typing.")]
	public char[] SmartFormatChars
	{
		get
		{
			return smartFormatChars;
		}
		set
		{
			smartFormatChars = value;
		}
	}

	[Description("Gets or sets a collection of characters that initializes an indentation procedure when typing.")]
	public char[] AutoIndentChars
	{
		get
		{
			return autoIndentChars;
		}
		set
		{
			autoIndentChars = value;
		}
	}

	[Browsable(false)]
	public override string XmlScheme
	{
		get
		{
			return base.XmlScheme;
		}
		set
		{
			base.XmlScheme = value;
			InitLanguage();
		}
	}

	[Description("Occurs when \"SyntaxParser\" text content is fully parsed.")]
	public event EventHandler TextReparsed;

	protected void AddStyle(string name, Color color, Color backColor, FontStyle fontStyle, bool plainText)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		ILexStyle lexStyle = Scheme.LexStyles.AddLexStyle();
		lexStyle.Name = name;
		lexStyle.FontStyle = fontStyle;
		if (color != Color.Empty)
		{
			lexStyle.ForeColor = color;
		}
		if (backColor != Color.Empty)
		{
			lexStyle.BackColor = backColor;
		}
		lexStyle.PlainText = plainText;
	}

	protected void AddStyle(string name, Color color, FontStyle fontStyle, bool plainText)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		AddStyle(name, color, Color.Empty, fontStyle, plainText);
	}

	protected void AddStyle(string name, Color color, FontStyle fontStyle)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		AddStyle(name, color, Color.Empty, fontStyle, plainText: false);
	}

	protected void AddStyle(string name, Color color)
	{
		AddStyle(name, color, Color.Empty, (FontStyle)0, plainText: false);
	}

	protected virtual void InitCopyright()
	{
		Scheme.Author = "Quantum Whale LLC.";
		Scheme.Copyright = "Copyright (c) 2004, 2005 Quantum Whale LLC.";
	}

	protected virtual void InitLanguage()
	{
	}

	protected virtual void InitStyles()
	{
	}

	public SyntaxParser()
	{
		lexerStates = new Hashtable();
		syntaxTree = new SyntaxTree();
		lineNodeComparer = new LineNodeComparer();
		pointNodeComparer = new PointNodeComparer();
		listMemberComparer = new ListMemberComparer(this);
		nodeComparer = new NodeComparer();
		ResetOptions();
		ResetCodeCompletionChars();
		ResetCodeCompletionStopChars();
		ResetAutoIndentChars();
		ResetSmartFormatChars();
		InitLexer();
	}

	private LexerState GetLexerState(int state)
	{
		LexerState lexerState = (LexerState)lexerStates[state];
		if (lexerState == null)
		{
			lexerState = new LexerState();
			lexerStates[state] = lexerState;
		}
		return lexerState;
	}

	protected char CurChar()
	{
		if (currentPos >= source.Length)
		{
			return '\0';
		}
		return source[currentPos];
	}

	protected virtual void LexSpace()
	{
		currentPos++;
		int length = source.Length;
		while (currentPos < length)
		{
			char c = source[currentPos];
			if (c >= '\0' && c <= ' ')
			{
				currentPos++;
				continue;
			}
			break;
		}
	}

	protected virtual void LexIdent()
	{
		currentPos++;
		int length = source.Length;
		while (currentPos < length)
		{
			char c = source[currentPos];
			if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9') || c == '_')
			{
				currentPos++;
				continue;
			}
			break;
		}
	}

	protected virtual void LexNum()
	{
		currentPos++;
		int length = source.Length;
		while (currentPos < length)
		{
			char c = source[currentPos];
			if (c >= '0' && c <= '9')
			{
				currentPos++;
				continue;
			}
			break;
		}
	}

	protected virtual void InitLexer()
	{
		lexerStates.Clear();
		InitCopyright();
		InitLanguage();
		InitStyles();
		StateChanged();
	}

	protected override void StateChanged()
	{
		lexerState = GetLexerState(base.State);
	}

	protected void RegisterLexerProc(int state, LexerProc proc)
	{
		GetLexerState(state).StateProc = proc;
	}

	protected void RegisterLexerProc(int state, char[] chars, LexerProc proc)
	{
		GetLexerState(state).Add(chars, proc);
	}

	protected void RegisterLexerProc(int state, char ch, LexerProc proc)
	{
		RegisterLexerProc(state, new char[1] { ch }, proc);
	}

	protected void RegisterLexerProc(int state, char startChar, char endChar, LexerProc proc)
	{
		char[] array = new char[endChar - startChar + 1];
		for (char c = startChar; c <= endChar; c = (char)(c + 1))
		{
			array[c - startChar] = c;
		}
		RegisterLexerProc(state, array, proc);
	}

	protected virtual int GetLexerStyle(int token)
	{
		return token;
	}

	public override int ParseText(int state, int line, string s, ref int pos, ref int len, ref int token)
	{
		if (useScheme)
		{
			return base.ParseText(state, line, s, ref pos, ref len, ref token);
		}
		base.State = state;
		lineIndex = line;
		source = s;
		int length = s.Length;
		LexerProc lexerProc = null;
		while (pos < length)
		{
			lexerProc = lexerState.GetProc(s[pos]);
			if (lexerProc != null)
			{
				break;
			}
			pos++;
		}
		if (lexerProc != null)
		{
			currentPos = pos;
			tokenPos = pos;
			token = lexerProc();
			len = currentPos - pos;
		}
		return base.State;
	}

	public override int ParseText(int state, int line, string str, ref short[] colorData)
	{
		if (useScheme)
		{
			return base.ParseText(state, line, str, ref colorData);
		}
		base.State = state;
		lineIndex = line;
		source = str;
		int length = str.Length;
		int num = 0;
		while (num < length)
		{
			int num2 = num;
			LexerProc proc = lexerState.GetProc(str[num]);
			if (proc == null)
			{
				colorData[num] = 0;
				num++;
				continue;
			}
			if (num2 != num)
			{
				for (int i = num; i < num2; i++)
				{
					colorData[i] = 0;
				}
			}
			currentPos = num;
			tokenPos = num;
			int lexerStyle = GetLexerStyle(proc());
			for (int j = num; j < Math.Min(currentPos, length); j++)
			{
				colorData[j] = (byte)(lexerStyle + 1);
			}
			num = currentPos;
		}
		OnTextParsed(str, ref colorData);
		return base.State;
	}

	protected virtual void BeforeLoad()
	{
		if (Strings == null)
		{
			Strings = new StringList();
		}
	}

	protected virtual void AfterLoad()
	{
		ReparseText();
	}

	public bool LoadFile(string fileName)
	{
		return LoadFile(fileName, null);
	}

	public bool LoadFile(string fileName, Encoding encoding)
	{
		BeforeLoad();
		try
		{
			return Strings.LoadFile(fileName, encoding);
		}
		finally
		{
			AfterLoad();
		}
	}

	public bool LoadStream(TextReader reader)
	{
		BeforeLoad();
		try
		{
			return Strings.LoadStream(reader);
		}
		finally
		{
			AfterLoad();
		}
	}

	public bool LoadStream(Stream stream)
	{
		return LoadStream(stream, null);
	}

	public bool LoadStream(Stream stream, Encoding encoding)
	{
		BeforeLoad();
		try
		{
			return Strings.LoadStream(stream, encoding);
		}
		finally
		{
			AfterLoad();
		}
	}

	protected virtual void Outline(IList ranges, ISyntaxNode node, int level)
	{
		if ((node.Options & SyntaxNodeOptions.Outlining) != 0)
		{
			Point start = node.Range.StartPoint;
			ISyntaxAttribute syntaxAttribute = node.FindAttribute(SyntaxConsts.DeclarationScope);
			if (syntaxAttribute != null)
			{
				start = syntaxAttribute.Position;
			}
			syntaxAttribute = node.FindAttribute(SyntaxConsts.OutlineText);
			ranges.Add(new OutlineRange(start, node.Range.EndPoint, level, (syntaxAttribute != null) ? ((string)syntaxAttribute.Value) : "..."));
			level++;
		}
		if (!node.HasChildren)
		{
			return;
		}
		foreach (ISyntaxNode child in node.ChildList)
		{
			Outline(ranges, child, level);
		}
	}

	protected virtual bool FindMember(string name, IListMembers members, out int idx)
	{
		idx = -1;
		if (name == string.Empty)
		{
			return false;
		}
		SortList sortList = new SortList();
		for (int i = 0; i < members.Count; i++)
		{
			sortList.Add(new MemberItem(members[i].Name, i));
		}
		MemberItemComparer memberItemComparer = new MemberItemComparer();
		sortList.Sort(memberItemComparer);
		if (memberItemComparer.FindPartial(sortList, name, out idx))
		{
			bool result = idx == sortList.Count - 1 || memberItemComparer.PartialCompare(name, sortList[idx + 1]) != 0;
			idx = ((MemberItem)sortList[idx]).Index;
			return result;
		}
		while (name != string.Empty)
		{
			name = name.Substring(0, name.Length - 1);
			if (name != string.Empty && memberItemComparer.FindPartial(sortList, name, out idx))
			{
				idx = ((MemberItem)sortList[idx]).Index;
				return false;
			}
		}
		idx = -1;
		return false;
	}

	protected virtual void GetSyntaxErrors(IList errors, ISyntaxNode node)
	{
		if (node.HasErrors)
		{
			foreach (ISyntaxError error in node.ErrorList)
			{
				errors.Add(error);
			}
		}
		if (!node.HasChildren)
		{
			return;
		}
		foreach (ISyntaxNode child in node.ChildList)
		{
			GetSyntaxErrors(errors, child);
		}
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

	protected virtual IParameterInfo CreateParameterInfo()
	{
		return new ParameterInfo();
	}

	protected virtual IListMembers CreateListMembers()
	{
		return new ListMembers();
	}

	protected virtual void SortMembers(IListMembers members)
	{
		members.Sort(listMemberComparer);
	}

	public virtual int Outline(IList ranges)
	{
		Outline(ranges, syntaxTree.Root, 0);
		return ranges.Count;
	}

	public virtual int GetSmartIndent(int index, bool autoIndent)
	{
		int num = -1;
		ISyntaxNode syntaxNode = syntaxTree.Root;
		ISyntaxNode syntaxNode2 = null;
		while (syntaxNode != null)
		{
			if ((syntaxNode.Options & SyntaxNodeOptions.Indentation) != 0 || (syntaxNode.Options & SyntaxNodeOptions.BackIndentation) != 0 || (syntaxNode.Options & SyntaxNodeOptions.KeepIndentation) != 0)
			{
				if ((syntaxNode.Options & SyntaxNodeOptions.BackIndentation) == 0 && (syntaxNode.Options & SyntaxNodeOptions.KeepIndentation) == 0)
				{
					num = ((num < 0) ? 1 : (num + 1));
				}
				syntaxNode2 = syntaxNode;
			}
			if (syntaxNode.Position.Y == index && syntaxNode != syntaxTree.Root)
			{
				break;
			}
			syntaxNode = syntaxNode.FindNode(index, lineNodeComparer);
		}
		if (!autoIndent && num >= 0 && syntaxNode2 != null)
		{
			num = syntaxNode2.GetIndent(index, num);
		}
		return num;
	}

	public virtual string SmartFormatLine(int index, string text, short[] colorData)
	{
		return text;
	}

	public virtual ICodeCompletionRepository CreateRepository()
	{
		return new CodeCompletionRepository(CaseSensitive, SyntaxTree);
	}

	public virtual CodeCompletionType GetCompletionType(CodeCompletionArgs e)
	{
		return e.CompletionType;
	}

	public virtual bool ProcessAutoComplete(Point position, out string code)
	{
		code = string.Empty;
		return false;
	}

	public virtual void CodeCompletion(string text, Point position, CodeCompletionArgs e)
	{
		CodeCompletionType completionType = GetCompletionType(e);
		switch (completionType)
		{
		case CodeCompletionType.CompleteWord:
		case CodeCompletionType.ListMembers:
		case CodeCompletionType.ParameterInfo:
		case CodeCompletionType.SpecialListMembers:
		{
			Point point = position;
			ISyntaxNode node2;
			if (e.NeedReparse)
			{
				ReparseBlock(position, out node2, completionType);
			}
			else
			{
				node2 = GetNodeForCodeCompletion(position);
			}
			if (node2 != null)
			{
				IListMembers listMembers = null;
				StaticScope scope = StaticScope.None;
				Point endPos = new Point(-1, -1);
				string name = string.Empty;
				if (completionType == CodeCompletionType.ParameterInfo)
				{
					int paramIndex;
					int paramCount;
					object methodType = CompletionRepository.GetMethodType(text, node2, ref name, ref position, ref endPos, out paramIndex, out paramCount, out scope);
					if (methodType != null)
					{
						listMembers = CreateParameterInfo();
						listMembers.ShowParams = true;
						listMembers.ShowQualifiers = false;
						CompletionRepository.FillMember(listMembers, methodType, name, paramIndex, scope);
					}
					e.ToolTip = true;
				}
				else
				{
					object obj2 = ((completionType == CodeCompletionType.SpecialListMembers) ? CompletionRepository.GetSpecialMemberType(text, node2, ref name, ref position, ref endPos, out scope) : CompletionRepository.GetMemberType(text, node2, ref name, ref position, ref endPos, out scope));
					bool flag = false;
					if (obj2 != null)
					{
						flag = (scope & StaticScope.Delegate) != 0;
						if (flag)
						{
							listMembers = CreateParameterInfo();
							listMembers.ShowParams = true;
							listMembers.ShowQualifiers = false;
						}
						else
						{
							listMembers = CreateListMembers();
							listMembers.ShowDescriptions = true;
							listMembers.ShowResults = false;
							listMembers.ShowQualifiers = false;
						}
						int selIndex = -1;
						CompletionRepository.FillMembers(node2, position, listMembers, obj2, name, scope, ref selIndex);
						SortMembers(listMembers);
						if (!flag && listMembers.Count > 0)
						{
							int idx = 0;
							if ((selIndex >= 0 || FindMember(name, listMembers, out idx)) && completionType == CodeCompletionType.CompleteWord && (selIndex >= 0 || !position.Equals((object?)point)))
							{
								e.SelIndex = ((selIndex >= 0) ? selIndex : idx);
							}
							listMembers.SelIndex = ((selIndex >= 0) ? selIndex : idx);
						}
					}
					e.ToolTip = flag;
				}
				e.StartPosition = position;
				e.DisplayPosition = position;
				e.EndPosition = endPos;
				e.Provider = listMembers;
			}
			goto default;
		}
		case CodeCompletionType.QuickInfo:
		{
			ISyntaxNode node;
			if (e.NeedReparse)
			{
				ReparseBlock(position, out node, completionType);
			}
			else
			{
				node = GetNodeForCodeCompletion(position);
			}
			if (node != null)
			{
				object obj = CompletionRepository.FindDeclaration(text, node, position);
				if (obj != null)
				{
					string description = CompletionRepository.GetDescription(CreateListMembers(), (obj is ISyntaxNode) ? node : null, obj, SyntaxConsts.SummaryDescription, fullDescription: true);
					if (description != null && description != string.Empty)
					{
						e.Provider = new QuickInfo();
						e.ToolTip = true;
						((QuickInfo)e.Provider).Text = description;
					}
				}
			}
			goto default;
		}
		case CodeCompletionType.CodeSnippets:
			e.Provider = CodeSnippets;
			goto default;
		default:
			e.CompletionType = completionType;
			break;
		case CodeCompletionType.None:
			break;
		}
		e.Interval = SyntaxConsts.DefaultCompletionDelay;
		e.NeedShow = e.Provider != null && e.Provider.Count > 0;
	}

	public virtual int GetSyntaxErrors(IList errors)
	{
		GetSyntaxErrors(errors, syntaxTree.Root);
		return errors.Count;
	}

	public virtual string GetSingleLineComment()
	{
		return string.Empty;
	}

	public override void Reset()
	{
		base.Reset();
		syntaxTree.Clear();
	}

	public virtual ISyntaxNode GetBlock(Point position, out Point blockPt)
	{
		blockPt = Point.Empty;
		return null;
	}

	public virtual bool IsDeclaration(ISyntaxNode node)
	{
		return false;
	}

	public virtual ISyntaxNode GetNodeAt(Point position)
	{
		return SyntaxTree.FindNode(position, pointNodeComparer);
	}

	public object FindDeclaration(string text, Point position)
	{
		ISyntaxNode nodeAt = GetNodeAt(position);
		if (nodeAt == null)
		{
			return null;
		}
		return CompletionRepository.FindDeclaration(text, nodeAt, position);
	}

	public virtual int FindReferences(ISyntaxNode node, ISyntaxNodes references)
	{
		return CompletionRepository.FindReferences(node, references);
	}

	public virtual bool ReparseBlock(Point position)
	{
		return false;
	}

	public virtual bool ReparseBlock(Point position, out ISyntaxNode node, CodeCompletionType completionType)
	{
		bool result = ReparseBlock(position);
		node = GetNodeAt(position);
		return result;
	}

	public virtual void ReparseText()
	{
		if (this.TextReparsed != null)
		{
			this.TextReparsed(this, EventArgs.Empty);
		}
	}

	public virtual ISyntaxNode GetNodeForCodeCompletion(Point position)
	{
		return GetNodeAt(position);
	}

	public virtual bool IsContentDivider(int index)
	{
		return false;
	}

	public virtual void ResetOptions()
	{
		Options = SyntaxOptions.None;
	}

	public virtual void ResetCodeCompletionChars()
	{
		CodeCompletionChars = new char[0];
	}

	public virtual void ResetCodeCompletionStopChars()
	{
		CodeCompletionStopChars = new char[0];
	}

	public virtual void ResetAutoIndentChars()
	{
		AutoIndentChars = new char[0];
	}

	public virtual void ResetSmartFormatChars()
	{
		SmartFormatChars = new char[0];
	}

	public virtual bool ShouldSerializeCodeCompletionChars()
	{
		return codeCompletionChars.Length != 0;
	}

	public virtual bool ShouldSerializeCodeCompletionStopChars()
	{
		return codeCompletionStopChars.Length != 0;
	}

	public virtual bool ShouldSerializeSmartFormatChars()
	{
		return smartFormatChars.Length != 0;
	}

	public virtual bool ShouldSerializeAutoIndentChars()
	{
		return autoIndentChars.Length != 0;
	}
}
