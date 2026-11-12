using System.ComponentModel;
using System.Drawing;

namespace Stimulsoft.Editor;

[ToolboxItem(false)]
[ToolboxBitmap(typeof(XmlParser), "Images.XmlParser.bmp")]
public class XmlParser : SyntaxParser
{
	protected const int stateNormal = 0;

	protected const int stateTag = 1;

	protected const int stateParam = 2;

	protected const int stateComment = 3;

	protected const int stateCDATA = 4;

	protected LexerProc lexWhitespaceProc;

	protected LexerProc lexTagProc;

	protected LexerProc lexEqualProc;

	protected LexerProc lexStringProc;

	protected LexerProc lexNameProc;

	protected LexerProc lexBodyProc;

	protected LexerProc lexParamProc;

	protected LexerProc lexStringParamProc;

	protected LexerProc lexParamNumberProc;

	protected LexerProc lexCommentProc;

	protected LexerProc lexCommentEndProc;

	protected LexerProc lexCDATAProc;

	protected LexerProc lexCDATAEndProc;

	protected Point prevPosition;

	private const string CDATA = "![CDATA[";

	protected override void InitLanguage()
	{
		Scheme.FileType = "xml";
	}

	protected override void InitStyles()
	{
		AddStyle("idents", Color.Maroon);
		AddStyle("numbers", Color.Green);
		AddStyle("reswords", Color.Red);
		AddStyle("comments", Color.Green, (FontStyle)0, plainText: true);
		AddStyle("xml_comments", Color.Gray, (FontStyle)0, plainText: true);
		AddStyle("symbol", SystemColors.ControlText);
		AddStyle("whitespace", Color.Empty);
		AddStyle("strings", Color.Blue);
		AddStyle("directives", Color.Blue);
	}

	protected override void InitLexer()
	{
		base.InitLexer();
		lexWhitespaceProc = LexWhitespace;
		lexTagProc = LexTag;
		lexEqualProc = LexEqual;
		lexStringProc = LexString;
		lexNameProc = LexName;
		lexBodyProc = LexBody;
		lexStringParamProc = LexStringParam;
		lexParamProc = LexParam;
		lexParamNumberProc = LexParamNumber;
		lexCommentProc = LexComment;
		lexCommentEndProc = LexCommentEnd;
		lexCDATAProc = LexCDATA;
		lexCDATAEndProc = LexCDATAEnd;
		RegisterLexerProc(0, lexBodyProc);
		RegisterLexerProc(0, '<', lexTagProc);
		RegisterLexerProc(1, lexWhitespaceProc);
		RegisterLexerProc(1, new char[3] { '/', '>', '<' }, lexTagProc);
		RegisterLexerProc(1, '=', lexEqualProc);
		RegisterLexerProc(1, new char[2] { '"', '\'' }, lexStringProc);
		RegisterLexerProc(1, 'a', 'z', lexNameProc);
		RegisterLexerProc(1, 'A', 'Z', lexNameProc);
		RegisterLexerProc(2, lexWhitespaceProc);
		RegisterLexerProc(2, new char[2] { '/', '>' }, lexTagProc);
		RegisterLexerProc(2, new char[2] { '"', '\'' }, lexStringParamProc);
		RegisterLexerProc(2, 'A', 'Z', lexParamProc);
		RegisterLexerProc(2, 'a', 'z', lexParamProc);
		RegisterLexerProc(2, '0', '9', lexParamNumberProc);
		RegisterLexerProc(3, lexCommentEndProc);
		RegisterLexerProc(4, ']', lexCDATAEndProc);
		RegisterLexerProc(4, lexCDATAProc);
	}

	protected override int GetLexerStyle(int token)
	{
		switch (token)
		{
		case 3:
		case 4:
		case 5:
		case 6:
		case 7:
		case 12:
			return 7;
		case 8:
			return 0;
		case 9:
			return 2;
		case 10:
			return 8;
		case 2:
		case 13:
			return 5;
		case 11:
			return 3;
		default:
			return 6;
		}
	}

	protected virtual int LexComment()
	{
		bool flag = currentPos > 0 && source[currentPos - 1] == '<';
		currentPos++;
		if (flag && currentPos < source.Length - 1 && source[currentPos] == '-' && source[currentPos + 1] == '-')
		{
			currentPos += 2;
			base.State = 3;
			return 11;
		}
		return 1;
	}

	protected virtual int LexCommentEnd()
	{
		int length = source.Length;
		if (currentPos >= length)
		{
			currentPos++;
		}
		else
		{
			while (currentPos < length)
			{
				if (currentPos < length - 2 && source[currentPos] == '-' && source[currentPos + 1] == '-' && source[currentPos + 2] == '>')
				{
					currentPos += 3;
					base.State = 0;
					break;
				}
				currentPos++;
			}
		}
		return 11;
	}

	protected virtual int LexCDATAEnd()
	{
		int length = source.Length;
		if (currentPos < length - 2 && source[currentPos] == ']' && source[currentPos + 1] == ']' && source[currentPos + 2] == '>')
		{
			currentPos += 3;
			base.State = 0;
			return 12;
		}
		return LexCDATA();
	}

	protected virtual int LexCDATA()
	{
		int length = source.Length;
		if (currentPos >= length)
		{
			currentPos++;
		}
		else
		{
			while (currentPos < length)
			{
				if (currentPos < length - 2 && source[currentPos] == ']' && source[currentPos + 1] == ']' && source[currentPos + 2] == '>')
				{
					return 13;
				}
				currentPos++;
			}
		}
		return 13;
	}

	protected virtual int LexWhitespace()
	{
		LexSpace();
		return 1;
	}

	protected virtual int LexBody()
	{
		currentPos++;
		int length = source.Length;
		while (currentPos < length && source[currentPos] != '<')
		{
			currentPos++;
		}
		return 2;
	}

	protected virtual int LexTag()
	{
		char c = source[currentPos];
		int length = source.Length;
		currentPos++;
		switch (c)
		{
		case '<':
			base.State = 1;
			if (currentPos < length)
			{
				c = source[currentPos];
				switch (c)
				{
				case '/':
					currentPos++;
					return 5;
				case '!':
				{
					string text = source.Substring(currentPos);
					if (text.StartsWith("![CDATA["))
					{
						base.State = 4;
						currentPos += "![CDATA[".Length;
						return 12;
					}
					break;
				}
				}
				if (c == '!' && currentPos < length - 2 && source[currentPos + 1] == '-' && source[currentPos + 2] == '-')
				{
					return LexComment();
				}
				return 3;
			}
			return 3;
		case '>':
			base.State = 0;
			return 4;
		case '/':
			if (currentPos < length)
			{
				c = source[currentPos];
				if (c == '>')
				{
					currentPos++;
					base.State = 0;
					return 6;
				}
			}
			break;
		}
		return 0;
	}

	protected virtual int LexEqual()
	{
		currentPos++;
		base.State = 2;
		return 7;
	}

	protected virtual int LexString()
	{
		int length = source.Length;
		char c = source[currentPos];
		currentPos++;
		while (currentPos < length)
		{
			char c2 = source[currentPos];
			if (c2 == c)
			{
				currentPos++;
				break;
			}
			currentPos++;
		}
		return 10;
	}

	protected virtual int LexStringParam()
	{
		int num = LexString();
		base.State = 1;
		return 10;
	}

	protected virtual int LexParam()
	{
		LexIdent();
		base.State = 1;
		return 10;
	}

	protected virtual int LexParamNumber()
	{
		LexNum();
		base.State = 1;
		return 10;
	}

	protected override void LexIdent()
	{
		currentPos++;
		int length = source.Length;
		while (currentPos < length)
		{
			char c = source[currentPos];
			if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9') || c == '_' || c == ':' || c == '-')
			{
				currentPos++;
				continue;
			}
			break;
		}
	}

	protected virtual int LexName()
	{
		int result = (((currentPos > 0 && source[currentPos - 1] == '<') || (currentPos > 1 && source[currentPos - 1] == '/' && source[currentPos - 2] == '<')) ? 8 : 9);
		LexIdent();
		return result;
	}

	public override void ReparseText()
	{
		Reset();
		MoveNext();
		ParseUnit();
		base.ReparseText();
	}

	public override void ResetOptions()
	{
		Options = SyntaxOptions.Outline | SyntaxOptions.SmartIndent;
	}

	public override CodeCompletionType GetCompletionType(CodeCompletionArgs e)
	{
		if (e.CompletionType == CodeCompletionType.None)
		{
			char keyChar = e.KeyChar;
			if (keyChar == '<')
			{
				return CodeCompletionType.ListMembers;
			}
		}
		return base.GetCompletionType(e);
	}

	protected virtual bool IsComment(int tok)
	{
		return tok == 11;
	}

	protected override bool IsValidToken(int tok)
	{
		if (tok != 0 && !IsComment(tok) && tok != 12)
		{
			return tok != 13;
		}
		return false;
	}

	protected virtual bool ParseComment()
	{
		int num = lineIndex;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, string.Empty, 7, SyntaxNodeOptions.None);
		AddNode(syntaxNode);
		Point currentPosition = base.CurrentPosition;
		while (IsComment(base.Token))
		{
			currentPosition = base.CurrentPosition;
			NextToken();
		}
		if (lineIndex != num)
		{
			syntaxNode.Options |= SyntaxNodeOptions.Outlining;
		}
		syntaxNode.Range.EndPoint = currentPosition;
		return true;
	}

	protected virtual int MoveNext()
	{
		prevPosition = base.CurrentPosition;
		int result = NextToken();
		while (!base.Eof && !IsValidToken(result))
		{
			if (IsComment(base.Token))
			{
				ParseComment();
				result = base.Token;
			}
			else
			{
				result = NextToken();
			}
		}
		return result;
	}

	protected virtual void AddNode(ISyntaxNode node)
	{
		base.SyntaxTree.Current.AddChild(node);
	}

	protected virtual void SkipTagBody()
	{
		while (!base.Eof && base.Token != 4)
		{
			MoveNext();
		}
		MoveNext();
	}

	protected virtual bool ParseTagBody()
	{
		bool result = true;
		ISyntaxNode current = base.SyntaxTree.Current;
		bool flag = base.TokenString == "!";
		while (!base.Eof)
		{
			switch ((XmlLexerToken)base.Token)
			{
			case XmlLexerToken.ParamName:
				if (!ParseParams())
				{
					result = false;
				}
				break;
			case XmlLexerToken.TagName:
				if (current.Name == string.Empty)
				{
					current.Name = base.TokenString;
				}
				MoveNext();
				break;
			case XmlLexerToken.CloseEndTag:
				current.AddAttribute(new SyntaxAttribute(base.TokenPosition, XmlLexerToken.CloseEndTag.ToString(), null));
				MoveNext();
				return result;
			case XmlLexerToken.CloseTag:
				current.AddAttribute(new SyntaxAttribute(base.TokenPosition, XmlLexerToken.CloseTag.ToString(), null));
				MoveNext();
				if (!flag && !ParseBody())
				{
					result = false;
				}
				return result;
			default:
				MoveNext();
				break;
			}
		}
		return result;
	}

	protected virtual bool ParseTag(ISyntaxNode node)
	{
		bool result = true;
		MoveNext();
		bool flag = base.TokenString == "?";
		base.SyntaxTree.Push(node);
		try
		{
			if (!ParseTagBody())
			{
				result = false;
			}
		}
		finally
		{
			base.SyntaxTree.Pop();
		}
		node.Range.EndPoint = prevPosition;
		if (!flag && node.Range.EndPoint.Y != node.Range.StartPoint.Y)
		{
			node.Options = SyntaxNodeOptions.Indentation | SyntaxNodeOptions.Outlining;
			node.AddAttribute(new SyntaxAttribute(prevPosition, SyntaxConsts.DefinitionScopeEnd, null));
		}
		return result;
	}

	protected virtual bool ParseOpenTag()
	{
		ISyntaxNode node = new SyntaxNode(base.TokenPosition, string.Empty, 2);
		AddNode(node);
		return ParseTag(node);
	}

	protected virtual bool ParseBody()
	{
		bool result = true;
		ISyntaxNode current = base.SyntaxTree.Current;
		while (!base.Eof)
		{
			switch ((XmlLexerToken)base.Token)
			{
			case XmlLexerToken.OpenEndTag:
			{
				ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, string.Empty, 3, SyntaxNodeOptions.Outlining);
				Point tokenPosition = base.TokenPosition;
				SaveState();
				try
				{
					MoveNext();
					if (base.Token == 8 && base.TokenString == current.Name)
					{
						current.AddChild(syntaxNode);
					}
				}
				finally
				{
					RestoreState(base.TokenString != current.Name);
				}
				if (base.TokenString != current.Name)
				{
					current.AddError(new SyntaxError(base.TokenPosition, current.Name, $"{current.Name} end tag {StringConsts.ErrExpected}"));
					return result;
				}
				SkipTagBody();
				syntaxNode.Range.EndPoint = prevPosition;
				return result;
			}
			case XmlLexerToken.OpenTag:
				if (!ParseOpenTag())
				{
					result = false;
				}
				break;
			default:
				AddNode(new SyntaxNode(base.TokenPosition, base.TokenString, 6));
				MoveNext();
				break;
			}
		}
		return result;
	}

	protected virtual bool ParseParams()
	{
		string text = string.Empty;
		Point position = Point.Empty;
		ISyntaxNode syntaxNode = null;
		ISyntaxNode syntaxNode2 = new SyntaxNode(base.TokenPosition, string.Empty, 5);
		base.SyntaxTree.Push(syntaxNode2);
		try
		{
			bool flag = false;
			while (!(base.Eof || flag))
			{
				switch ((XmlLexerToken)base.Token)
				{
				case XmlLexerToken.ParamName:
					position = base.TokenPosition;
					text = base.TokenString;
					MoveNext();
					break;
				case XmlLexerToken.ParamValue:
					if (text != string.Empty)
					{
						syntaxNode = new SyntaxNode(position, text, 4);
						syntaxNode.AddAttribute(new SyntaxAttribute(base.TokenPosition, XmlNodeType.XmlParameter.ToString(), base.TokenString));
						syntaxNode.Range.EndPoint = base.CurrentPosition;
						syntaxNode2.AddChild(syntaxNode);
					}
					text = string.Empty;
					MoveNext();
					break;
				case XmlLexerToken.Equal:
					MoveNext();
					break;
				case XmlLexerToken.CloseTag:
				case XmlLexerToken.CloseEndTag:
					flag = true;
					break;
				default:
					MoveNext();
					break;
				}
			}
		}
		finally
		{
			base.SyntaxTree.Pop();
		}
		syntaxNode2.Range.EndPoint = prevPosition;
		if (syntaxNode2.HasChildren)
		{
			base.SyntaxTree.Current.AddChild(syntaxNode2);
		}
		return text == string.Empty;
	}

	protected virtual bool ParseUnit()
	{
		bool flag = true;
		ISyntaxNode root = base.SyntaxTree.Root;
		root.NodeType = 1;
		root.Position = Point.Empty;
		flag = ParseBody();
		base.SyntaxTree.Root.Range.EndPoint = prevPosition;
		root = base.SyntaxTree.Current;
		while (root != base.SyntaxTree.Root)
		{
			root.AddError(new SyntaxError(root.Position, root.Name, StringConsts.ErrEndOfFileFound));
			root.Range.EndPoint = prevPosition;
			base.SyntaxTree.Pop();
			root = base.SyntaxTree.Current;
			flag = false;
		}
		return flag;
	}
}
