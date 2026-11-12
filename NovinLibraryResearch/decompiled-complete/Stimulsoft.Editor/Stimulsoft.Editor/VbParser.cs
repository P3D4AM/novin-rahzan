using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using Microsoft.VisualBasic;

namespace Stimulsoft.Editor;

[ToolboxItem(false)]
public class VbParser : NetSyntaxParser
{
	private const int stateNormal = 0;

	private const int stateXmlComment = 2;

	protected Hashtable reswords;

	protected LexerProc lexSymbolProc;

	protected LexerProc lexWhitespaceProc;

	protected LexerProc lexIdentifierProc;

	protected LexerProc lexNumberProc;

	protected LexerProc lexStringProc;

	protected LexerProc lexCommentProc;

	protected LexerProc lexDefineProc;

	protected LexerProc lexXmlCommentProc;

	protected LexerProc lexXmlCommentTagProc;

	[Description("Gets or sets a boolean value that indicates whether \"VbSyntaxParser\" should perform case-sensitive analysis if its content.")]
	public override bool CaseSensitive => false;

	public VbParser()
	{
		Options |= SyntaxOptions.ReparseOnLineChange | SyntaxOptions.AutoComplete;
	}

	protected virtual void InitReswords()
	{
		reswords = new Hashtable();
		reswords.Add("addhandler", VbLexerToken.AddHandler);
		reswords.Add("addressof", VbLexerToken.AddressOf);
		reswords.Add("alias", VbLexerToken.Alias);
		reswords.Add("and", VbLexerToken.And);
		reswords.Add("andalso", VbLexerToken.AndAlso);
		reswords.Add("ansi", VbLexerToken.Ansi);
		reswords.Add("as", VbLexerToken.As);
		reswords.Add("assembly", VbLexerToken.Assembly);
		reswords.Add("auto", VbLexerToken.Auto);
		reswords.Add("boolean", VbLexerToken.Boolean);
		reswords.Add("byref", VbLexerToken.ByRef);
		reswords.Add("byte", VbLexerToken.Byte);
		reswords.Add("byval", VbLexerToken.ByVal);
		reswords.Add("call", VbLexerToken.Call);
		reswords.Add("case", VbLexerToken.Case);
		reswords.Add("catch", VbLexerToken.Catch);
		reswords.Add("cbool", VbLexerToken.CBool);
		reswords.Add("cbyte", VbLexerToken.CByte);
		reswords.Add("cchar", VbLexerToken.CChar);
		reswords.Add("cdate", VbLexerToken.CDate);
		reswords.Add("cdbl", VbLexerToken.CDbl);
		reswords.Add("cdec", VbLexerToken.CDec);
		reswords.Add("char", VbLexerToken.Char);
		reswords.Add("cint", VbLexerToken.CInt);
		reswords.Add("class", VbLexerToken.Class);
		reswords.Add("clng", VbLexerToken.CLng);
		reswords.Add("cobj", VbLexerToken.CObj);
		reswords.Add("const", VbLexerToken.Const);
		reswords.Add("continue", VbLexerToken.Continue);
		reswords.Add("cshort", VbLexerToken.CShort);
		reswords.Add("csng", VbLexerToken.CSng);
		reswords.Add("cstr", VbLexerToken.CStr);
		reswords.Add("ctype", VbLexerToken.CType);
		reswords.Add("date", VbLexerToken.Date);
		reswords.Add("decimal", VbLexerToken.Decimal);
		reswords.Add("declare", VbLexerToken.Declare);
		reswords.Add("default", VbLexerToken.Default);
		reswords.Add("delegate", VbLexerToken.Delegate);
		reswords.Add("dim", VbLexerToken.Dim);
		reswords.Add("directcast", VbLexerToken.DirectCast);
		reswords.Add("do", VbLexerToken.Do);
		reswords.Add("double", VbLexerToken.Double);
		reswords.Add("each", VbLexerToken.Each);
		reswords.Add("else", VbLexerToken.Else);
		reswords.Add("elseif", VbLexerToken.ElseIf);
		reswords.Add("end", VbLexerToken.End);
		reswords.Add("endif", VbLexerToken.EndIf);
		reswords.Add("enum", VbLexerToken.Enum);
		reswords.Add("erase", VbLexerToken.Erase);
		reswords.Add("error", VbLexerToken.Error);
		reswords.Add("event", VbLexerToken.Event);
		reswords.Add("exit", VbLexerToken.Exit);
		reswords.Add("false", VbLexerToken.False);
		reswords.Add("finally", VbLexerToken.Finally);
		reswords.Add("for", VbLexerToken.For);
		reswords.Add("friend", VbLexerToken.Friend);
		reswords.Add("function", VbLexerToken.Function);
		reswords.Add("get", VbLexerToken.Get);
		reswords.Add("gettype", VbLexerToken.GetType);
		reswords.Add("gosub", VbLexerToken.GoSub);
		reswords.Add("goto", VbLexerToken.GoTo);
		reswords.Add("handles", VbLexerToken.Handles);
		reswords.Add("if", VbLexerToken.If);
		reswords.Add("implements", VbLexerToken.Implements);
		reswords.Add("imports", VbLexerToken.Imports);
		reswords.Add("in", VbLexerToken.In);
		reswords.Add("inherits", VbLexerToken.Inherits);
		reswords.Add("integer", VbLexerToken.Integer);
		reswords.Add("interface", VbLexerToken.Interface);
		reswords.Add("is", VbLexerToken.Is);
		reswords.Add("isfalse", VbLexerToken.IsFalse);
		reswords.Add("istrue", VbLexerToken.IsTrue);
		reswords.Add("let", VbLexerToken.Let);
		reswords.Add("lib", VbLexerToken.Lib);
		reswords.Add("like", VbLexerToken.Like);
		reswords.Add("long", VbLexerToken.Long);
		reswords.Add("loop", VbLexerToken.Loop);
		reswords.Add("me", VbLexerToken.Me);
		reswords.Add("mid", VbLexerToken.Mid);
		reswords.Add("mod", VbLexerToken.Mod);
		reswords.Add("module", VbLexerToken.Module);
		reswords.Add("mustinherit", VbLexerToken.MustInherit);
		reswords.Add("mustoverride", VbLexerToken.MustOverride);
		reswords.Add("mybase", VbLexerToken.MyBase);
		reswords.Add("myclass", VbLexerToken.MyClass);
		reswords.Add("namespace", VbLexerToken.Namespace);
		reswords.Add("new", VbLexerToken.New);
		reswords.Add("next", VbLexerToken.Next);
		reswords.Add("not", VbLexerToken.Not);
		reswords.Add("nothing", VbLexerToken.Nothing);
		reswords.Add("notinheritable", VbLexerToken.NotInheritable);
		reswords.Add("notoverridable", VbLexerToken.NotOverridable);
		reswords.Add("object", VbLexerToken.Object);
		reswords.Add("of", VbLexerToken.Of);
		reswords.Add("on", VbLexerToken.On);
		reswords.Add("operator", VbLexerToken.Operator);
		reswords.Add("option", VbLexerToken.Option);
		reswords.Add("optional", VbLexerToken.Optional);
		reswords.Add("or", VbLexerToken.Or);
		reswords.Add("orelse", VbLexerToken.OrElse);
		reswords.Add("overloads", VbLexerToken.Overloads);
		reswords.Add("overridable", VbLexerToken.Overridable);
		reswords.Add("overrides", VbLexerToken.Overrides);
		reswords.Add("paramarray", VbLexerToken.ParamArray);
		reswords.Add("partial", VbLexerToken.Partial);
		reswords.Add("preserve", VbLexerToken.Preserve);
		reswords.Add("private", VbLexerToken.Private);
		reswords.Add("property", VbLexerToken.Property);
		reswords.Add("protected", VbLexerToken.Protected);
		reswords.Add("public", VbLexerToken.Public);
		reswords.Add("raiseevent", VbLexerToken.RaiseEvent);
		reswords.Add("readonly", VbLexerToken.ReadOnly);
		reswords.Add("redim", VbLexerToken.ReDim);
		reswords.Add("rem", VbLexerToken.REM);
		reswords.Add("removehandler", VbLexerToken.RemoveHandler);
		reswords.Add("resume", VbLexerToken.Resume);
		reswords.Add("return", VbLexerToken.Return);
		reswords.Add("select", VbLexerToken.Select);
		reswords.Add("set", VbLexerToken.Set);
		reswords.Add("shadows", VbLexerToken.Shadows);
		reswords.Add("shared", VbLexerToken.Shared);
		reswords.Add("short", VbLexerToken.Short);
		reswords.Add("single", VbLexerToken.Single);
		reswords.Add("static", VbLexerToken.Static);
		reswords.Add("step", VbLexerToken.Step);
		reswords.Add("stop", VbLexerToken.Stop);
		reswords.Add("string", VbLexerToken.String);
		reswords.Add("structure", VbLexerToken.Structure);
		reswords.Add("sub", VbLexerToken.Sub);
		reswords.Add("synclock", VbLexerToken.SyncLock);
		reswords.Add("then", VbLexerToken.Then);
		reswords.Add("throw", VbLexerToken.Throw);
		reswords.Add("to", VbLexerToken.To);
		reswords.Add("true", VbLexerToken.True);
		reswords.Add("try", VbLexerToken.Try);
		reswords.Add("typeof", VbLexerToken.TypeOf);
		reswords.Add("unicode", VbLexerToken.Unicode);
		reswords.Add("until", VbLexerToken.Until);
		reswords.Add("variant", VbLexerToken.Variant);
		reswords.Add("wend", VbLexerToken.Wend);
		reswords.Add("when", VbLexerToken.When);
		reswords.Add("while", VbLexerToken.While);
		reswords.Add("with", VbLexerToken.With);
		reswords.Add("withevents", VbLexerToken.WithEvents);
		reswords.Add("writeonly", VbLexerToken.WriteOnly);
		reswords.Add("xor", VbLexerToken.Xor);
		reswords.Add("region", VbLexerToken.Region);
	}

	protected override void InitLanguage()
	{
		Scheme.FileType = "vb";
	}

	protected override void InitLexer()
	{
		base.InitLexer();
		InitReswords();
		lexWhitespaceProc = LexWhitespace;
		lexSymbolProc = LexSymbol;
		lexIdentifierProc = LexIdentifier;
		lexNumberProc = LexNumber;
		lexStringProc = LexString;
		lexCommentProc = LexComment;
		lexDefineProc = LexDefine;
		lexXmlCommentProc = LexXmlComment;
		lexXmlCommentTagProc = LexXmlCommentTag;
		RegisterLexerProc(0, lexWhitespaceProc);
		RegisterLexerProc(0, '!', 'ÿ', lexSymbolProc);
		RegisterLexerProc(0, 'a', 'z', lexIdentifierProc);
		RegisterLexerProc(0, 'A', 'Z', lexIdentifierProc);
		RegisterLexerProc(0, '_', lexIdentifierProc);
		RegisterLexerProc(0, '0', '9', lexNumberProc);
		RegisterLexerProc(0, new char[2] { '.', '&' }, lexNumberProc);
		RegisterLexerProc(0, new char[2] { '+', '-' }, lexSymbolProc);
		RegisterLexerProc(0, '"', lexStringProc);
		RegisterLexerProc(0, new char[3] { '\'', 'r', 'R' }, lexCommentProc);
		RegisterLexerProc(0, '#', lexDefineProc);
		RegisterLexerProc(2, lexXmlCommentProc);
		RegisterLexerProc(2, '<', lexXmlCommentTagProc);
	}

	protected virtual bool IsKeywordToken(int token)
	{
		if (token >= 0)
		{
			return token <= 146;
		}
		return false;
	}

	protected override int GetLexerStyle(int token)
	{
		if (IsKeywordToken(token))
		{
			return 2;
		}
		switch (token)
		{
		case 189:
		case 190:
		case 191:
		case 192:
		case 193:
		case 194:
		case 195:
			return 1;
		case 196:
		case 197:
			return 7;
		case 188:
			return 0;
		case 198:
			return 3;
		case 199:
			return 4;
		case 200:
			return 8;
		default:
			return 6;
		}
	}

	protected virtual int LexWhitespace()
	{
		LexSpace();
		return 187;
	}

	protected virtual int LexSymbol()
	{
		VbLexerToken result = VbLexerToken.Whitespace_Literal;
		char c = source[currentPos];
		currentPos++;
		switch (c)
		{
		case '{':
			result = VbLexerToken.Open_brace;
			break;
		case '}':
			result = VbLexerToken.Close_brace;
			break;
		case '(':
			result = VbLexerToken.Open_parens;
			break;
		case ')':
			result = VbLexerToken.Close_parens;
			break;
		case '.':
			result = VbLexerToken.Dot;
			break;
		case ',':
			result = VbLexerToken.Comma;
			break;
		case ':':
			result = VbLexerToken.Colon;
			break;
		case '#':
			result = VbLexerToken.Op_double;
			break;
		case '%':
			result = VbLexerToken.Op_integer;
			break;
		case '@':
			result = VbLexerToken.Op_decimal;
			break;
		case '$':
			result = VbLexerToken.Op_string;
			break;
		case '+':
		{
			char c2 = CurChar();
			if (c2 == '=')
			{
				currentPos++;
				result = VbLexerToken.Op_add_assign;
			}
			else
			{
				result = VbLexerToken.Plus;
			}
			break;
		}
		case '-':
		{
			char c4 = CurChar();
			if (c4 == '=')
			{
				currentPos++;
				result = VbLexerToken.Op_sub_assign;
			}
			else
			{
				result = VbLexerToken.Minus;
			}
			break;
		}
		case '<':
			switch (CurChar())
			{
			case '<':
				currentPos++;
				if (CurChar() == '=')
				{
					currentPos++;
					result = VbLexerToken.Op_shift_left_assign;
				}
				else
				{
					result = VbLexerToken.Op_shift_left;
				}
				break;
			case '>':
				currentPos++;
				result = VbLexerToken.Op_ne;
				break;
			case '=':
				currentPos++;
				result = VbLexerToken.Op_le;
				break;
			default:
				result = VbLexerToken.Op_lt;
				break;
			}
			break;
		case '>':
			switch (CurChar())
			{
			case '>':
				currentPos++;
				if (CurChar() == '=')
				{
					currentPos++;
					result = VbLexerToken.Op_shift_right_assign;
				}
				else
				{
					result = VbLexerToken.Op_shift_right;
				}
				break;
			case '=':
				currentPos++;
				result = VbLexerToken.Op_ge;
				break;
			default:
				result = VbLexerToken.Op_gt;
				break;
			}
			break;
		case '=':
			result = VbLexerToken.Assign;
			break;
		case '!':
			result = VbLexerToken.Op_single;
			break;
		case '&':
		{
			char c3 = CurChar();
			if (c3 == '=')
			{
				currentPos++;
				result = VbLexerToken.String_and_assign;
			}
			else
			{
				result = VbLexerToken.Op_long;
			}
			break;
		}
		case '*':
			if (CurChar() == '=')
			{
				currentPos++;
				result = VbLexerToken.Op_mult_assign;
			}
			else
			{
				result = VbLexerToken.Star;
			}
			break;
		case '/':
			if (CurChar() == '=')
			{
				currentPos++;
				result = VbLexerToken.Op_div_assign;
			}
			else
			{
				result = VbLexerToken.Div;
			}
			break;
		case '\\':
			if (CurChar() == '=')
			{
				currentPos++;
				result = VbLexerToken.Op_intdiv_assign;
			}
			else
			{
				result = VbLexerToken.IntDiv;
			}
			break;
		case '^':
			if (CurChar() == '=')
			{
				currentPos++;
				result = VbLexerToken.Op_exp_assign;
			}
			else
			{
				result = VbLexerToken.Carret;
			}
			break;
		case ';':
			result = VbLexerToken.Semicolon;
			break;
		}
		return (int)result;
	}

	protected virtual int LexIdentifier()
	{
		if (source[currentPos] == '_' && currentPos == source.TrimEnd(new char[0]).Length - 1)
		{
			currentPos++;
			return 156;
		}
		LexIdent();
		object obj = reswords[base.TokenString.ToLower(CultureInfo.InvariantCulture)];
		if (obj == null)
		{
			return 188;
		}
		return (int)obj;
	}

	private bool IsFloatChar(char ch, ref int token)
	{
		bool result = false;
		switch (ch)
		{
		case 'R':
		case 'r':
			currentPos++;
			token = 193;
			result = true;
			break;
		case 'F':
		case 'f':
			currentPos++;
			token = 192;
			result = true;
			break;
		case 'D':
		case 'd':
			currentPos++;
			token = 194;
			result = true;
			break;
		}
		return result;
	}

	protected virtual int LexHexNumber()
	{
		int length = source.Length;
		while (currentPos < length)
		{
			char c = source[currentPos];
			if ((c < '0' || c > '9') && (c < 'a' || c > 'f') && (c < 'A' || c > 'F'))
			{
				break;
			}
			currentPos++;
		}
		return 189;
	}

	protected virtual int LexOctNumber()
	{
		int length = source.Length;
		while (currentPos < length)
		{
			char c = source[currentPos];
			if (c < '0' || c > '7')
			{
				break;
			}
			currentPos++;
		}
		return 189;
	}

	protected virtual int LexNumber()
	{
		char c = source[currentPos];
		int length = source.Length;
		int result = 189;
		if (c == '+' || c == '-' || c == '.')
		{
			int num = currentPos + 1;
			if (num >= length || ((source[num] < '0' || source[num] > '9') && source[num] != '&'))
			{
				return LexSymbol();
			}
		}
		if (c == '&')
		{
			int num = currentPos + 1;
			if (num < length && (source[num] == 'h' || source[num] == 'H'))
			{
				currentPos = num + 1;
				return LexHexNumber();
			}
			if (num < length && (source[num] == 'o' || source[num] == 'O'))
			{
				currentPos = num + 1;
				return LexOctNumber();
			}
			return LexSymbol();
		}
		base.LexNum();
		if (currentPos < length)
		{
			switch (source[currentPos])
			{
			case 'S':
			case 's':
				currentPos++;
				return 190;
			case 'I':
			case 'i':
				currentPos++;
				return 189;
			case 'L':
			case 'l':
				currentPos++;
				return 191;
			}
		}
		if (currentPos < length)
		{
			c = source[currentPos];
			if (IsFloatChar(c, ref result))
			{
				return result;
			}
			if (c == '.' && currentPos < length - 1)
			{
				c = source[currentPos + 1];
				if (c >= '0' && c <= '9')
				{
					currentPos++;
					LexNum();
					result = 192;
				}
			}
		}
		if (currentPos < length)
		{
			c = source[currentPos];
			if (IsFloatChar(c, ref result))
			{
				return result;
			}
			if (c == 'E' || c == 'e')
			{
				int num = currentPos + 1;
				if (num < length)
				{
					c = source[num];
					if (c == '+' || c == '-')
					{
						num = (currentPos = num + 1);
					}
				}
				if (num < length)
				{
					c = source[num];
					if (c >= '0' && c <= '9')
					{
						currentPos = num;
						LexNum();
						result = 193;
					}
				}
			}
		}
		if (currentPos < length)
		{
			c = source[currentPos];
			IsFloatChar(c, ref result);
			return result;
		}
		return result;
	}

	protected virtual int LexString()
	{
		char c = source[currentPos];
		currentPos++;
		int length = source.Length;
		while (currentPos < length)
		{
			char c2 = source[currentPos];
			if (c2 == c)
			{
				currentPos++;
				if (currentPos >= length || source[currentPos] != c)
				{
					break;
				}
				currentPos++;
			}
			else
			{
				currentPos++;
			}
		}
		if (currentPos >= length || source[currentPos] != 'c')
		{
			return 197;
		}
		return 196;
	}

	protected virtual int LexComment()
	{
		char c = source[currentPos];
		int length = source.Length;
		if (c == '\'')
		{
			int num = currentPos + 1;
			if (num < length - 1 && source[num] == '\'' && source[num + 1] == '\'')
			{
				currentPos = num + 2;
				if (currentPos < length)
				{
					base.State = 2;
				}
				return 199;
			}
			currentPos = length + 1;
			return 198;
		}
		int num2 = currentPos;
		LexIdent();
		if (base.TokenString.ToLower(CultureInfo.InvariantCulture) == "rem")
		{
			currentPos = length + 1;
			return 198;
		}
		currentPos = num2;
		return LexIdentifier();
	}

	protected virtual int LexXmlComment()
	{
		int length = source.Length;
		currentPos++;
		while (currentPos < length && source[currentPos] != '<')
		{
			currentPos++;
		}
		if (currentPos == length)
		{
			base.State = 0;
		}
		return 198;
	}

	protected virtual int LexXmlCommentTag()
	{
		currentPos++;
		int length = source.Length;
		while (currentPos < length)
		{
			if (source[currentPos] == '>')
			{
				currentPos++;
				break;
			}
			currentPos++;
		}
		if (currentPos == length)
		{
			base.State = 0;
		}
		return 199;
	}

	protected virtual int LexDefine()
	{
		currentPos++;
		int length = source.Length;
		if (currentPos < source.Length)
		{
			char c = source[currentPos];
			if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || c == '_')
			{
				if (source.Substring(0, currentPos - 1).Trim() != string.Empty)
				{
					return LexSymbol();
				}
				LexIdent();
				return 200;
			}
			while (currentPos < length)
			{
				c = source[currentPos];
				if (c == '#')
				{
					currentPos++;
					break;
				}
				currentPos++;
			}
			return 195;
		}
		return 200;
	}

	protected override bool IsValidToken(int tok)
	{
		if (tok != 187 && !IsComment(tok))
		{
			return tok != 156;
		}
		return false;
	}

	protected bool IsBlockEnd()
	{
		return base.Token == 45;
	}

	protected virtual void SyntaxError(int token)
	{
		if (base.Stack.Count == 0)
		{
			Point position = prevPosition;
			Point tokenPosition = base.TokenPosition;
			string tokenString = base.TokenString;
			VbLexerToken vbLexerToken = (VbLexerToken)token;
			ISyntaxError syntaxError = new SyntaxError(tokenPosition, tokenString, vbLexerToken.ToString() + " " + StringConsts.ErrExpected);
			if (position.Y != base.TokenPosition.Y)
			{
				syntaxError.Position = position;
				syntaxError.Range.EndPoint = new Point(position.X + 1, position.Y);
			}
			base.SyntaxTree.Current.AddError(syntaxError);
		}
	}

	protected virtual int GetBlockToken(int nodeType)
	{
		return nodeType switch
		{
			7 => 85, 
			8 => 24, 
			12 => 80, 
			9 => 128, 
			10 => 68, 
			11 => 47, 
			21 => 107, 
			_ => 85, 
		};
	}

	protected bool Expected(int token)
	{
		return Expected((VbLexerToken)token);
	}

	protected bool Expected(VbLexerToken token)
	{
		bool flag = base.Token == (int)token;
		if (!flag)
		{
			SyntaxError((int)token);
		}
		if (flag)
		{
			MoveNext();
		}
		return flag;
	}

	protected bool Expected(VbLexerToken token1, VbLexerToken token2)
	{
		bool flag = base.Token == (int)token1 || base.Token == (int)token2;
		if (!flag)
		{
			SyntaxError((int)token1);
		}
		if (flag)
		{
			MoveNext();
		}
		return flag;
	}

	protected bool IdentifierExpected()
	{
		return Expected(VbLexerToken.Identifier_Literal, VbLexerToken.MyClass);
	}

	protected override bool ShouldOutlineCommentNode(ISyntaxNode node)
	{
		if (!base.ShouldOutlineCommentNode(node))
		{
			return GetValidNode(node) == base.SyntaxTree.Root;
		}
		return true;
	}

	protected override bool ParseComment()
	{
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, SyntaxConsts.OutlineCommentText, 49, SyntaxNodeOptions.None);
		syntaxNode.AddAttribute(new SyntaxAttribute(syntaxNode.Position, SyntaxConsts.OutlineText, syntaxNode.Name));
		comments.Add(syntaxNode);
		Point currentPosition = base.CurrentPosition;
		bool flag = false;
		while (IsComment(base.Token) || base.Token == 187)
		{
			if (base.Token == 199)
			{
				flag = true;
			}
			if (base.Token != 187)
			{
				currentPosition = base.CurrentPosition;
				if (currentPosition.X == source.Length)
				{
					currentPosition.X = int.MaxValue;
				}
			}
			NextToken();
		}
		syntaxNode.Range.EndPoint = currentPosition;
		if (flag)
		{
			syntaxNode.NodeType = 55;
			ParseXmlComment(syntaxNode);
		}
		return true;
	}

	protected override bool IsComment(int tok)
	{
		if (tok != 198)
		{
			return tok == 199;
		}
		return true;
	}

	protected bool IsIdentifierToken(int token)
	{
		if (token != 188)
		{
			return token == 84;
		}
		return true;
	}

	protected virtual bool ParseIdentifier(out string identifier)
	{
		identifier = base.TokenString;
		if (IsIdentifierToken(base.Token))
		{
			char c = CurChar();
			switch (c)
			{
			case '!':
			case '#':
			case '$':
			case '%':
			case '&':
			case '@':
			{
				bool flag = false;
				MoveNext();
				SaveState();
				try
				{
					flag = base.TokenString == c.ToString();
					if (flag)
					{
						MoveNext();
					}
				}
				finally
				{
					RestoreState(!flag);
				}
				break;
			}
			default:
				MoveNext();
				break;
			}
			return true;
		}
		return IdentifierExpected();
	}

	protected virtual bool ParseQualifiedIdentifier(out string identifier)
	{
		identifier = base.TokenString;
		if (ParseIdentifier(out identifier))
		{
			while (base.Token == 151)
			{
				identifier += base.TokenString;
				MoveNext();
				string identifier2 = base.TokenString;
				if (IsKeywordToken(base.Token))
				{
					MoveNext();
					identifier += identifier2;
				}
				else if (ParseIdentifier(out identifier2))
				{
					identifier += identifier2;
				}
				else
				{
					bool flag = false;
				}
			}
			return true;
		}
		identifier = string.Empty;
		return false;
	}

	protected virtual bool ParseRankSpecifier(out string rank)
	{
		bool result = true;
		rank = base.TokenString;
		if (Expected(VbLexerToken.Open_parens))
		{
			while (base.Token == 152)
			{
				rank += base.TokenString;
				MoveNext();
			}
			if (base.Token == 150)
			{
				rank += base.TokenString;
			}
			result = Expected(VbLexerToken.Close_parens);
		}
		return result;
	}

	protected virtual bool TryParseRankSpecifiers(out string rank)
	{
		bool flag = true;
		rank = string.Empty;
		while (base.Token == 149)
		{
			flag = ParseRankSpecifier(out var rank2);
			if (flag)
			{
				rank += rank2;
			}
		}
		return flag;
	}

	protected virtual bool TryParseArraySizeInitializationModifier(out string rank)
	{
		bool flag = true;
		rank = string.Empty;
		while (base.Token == 149)
		{
			flag = ParseArraySizeInitializationModifier(out var rank2);
			if (flag)
			{
				rank += rank2;
			}
		}
		return flag;
	}

	protected virtual bool ParseArraySizeInitializationModifier(out string rank)
	{
		bool result = true;
		rank = base.TokenString;
		if (Expected(VbLexerToken.Open_parens))
		{
			do
			{
				rank += base.TokenString;
				if (base.Token == 150)
				{
					break;
				}
				if (base.Token == 152)
				{
					MoveNext();
					continue;
				}
				ISyntaxNode node = null;
				if (ParseExpression(ref node) && node != null && node.NodeType == 148)
				{
					rank += node.Name;
					continue;
				}
				result = false;
				break;
			}
			while (!base.Eof && base.Token != 150);
			if (base.Token == 150)
			{
				rank += base.TokenString;
			}
			result = Expected(VbLexerToken.Close_parens);
		}
		return result;
	}

	protected virtual bool IsBuiltInType(int Token)
	{
		switch ((VbLexerToken)Token)
		{
		case VbLexerToken.Boolean:
		case VbLexerToken.Byte:
		case VbLexerToken.Char:
		case VbLexerToken.Date:
		case VbLexerToken.Decimal:
		case VbLexerToken.Double:
		case VbLexerToken.Integer:
		case VbLexerToken.Long:
		case VbLexerToken.Object:
		case VbLexerToken.Short:
		case VbLexerToken.Single:
		case VbLexerToken.String:
			return true;
		default:
			return false;
		}
	}

	protected virtual bool ParseTypeName(out string type)
	{
		bool flag = true;
		if (IsBuiltInType(base.Token) || base.Token == 7)
		{
			type = base.TokenString;
			flag = true;
			MoveNext();
		}
		else
		{
			flag = ParseQualifiedIdentifier(out type);
		}
		return flag;
	}

	protected virtual bool ParseType(out string type)
	{
		bool flag = ParseTypeName(out type);
		if (flag)
		{
			flag = TryParseRankSpecifiers(out var rank);
			if (flag)
			{
				type += rank;
			}
		}
		return flag;
	}

	private bool ParseBaseList(out string types, bool isList)
	{
		bool result = true;
		if (ParseType(out types))
		{
			if (isList)
			{
				while (base.Token == 152)
				{
					types += base.TokenString;
					MoveNext();
					if (ParseType(out var type))
					{
						types += type;
						continue;
					}
					result = false;
					break;
				}
			}
		}
		else
		{
			result = false;
		}
		return result;
	}

	protected virtual bool IsBaseList(int token)
	{
		if (token != 66)
		{
			return token == 63;
		}
		return true;
	}

	protected virtual bool IsType(int token)
	{
		return token == 6;
	}

	protected virtual bool ParseInherits(out string types, bool isList)
	{
		return ParseBaseList(out types, isList);
	}

	protected virtual bool ParseImplements(out string types)
	{
		return ParseBaseList(out types, isList: true);
	}

	protected override bool ParseUsingDeclaration(ISyntaxNode node)
	{
		return ParseImportDeclaration(node);
	}

	protected override bool ParseUsingList(ISyntaxNode node)
	{
		return ParseImportsList(node);
	}

	protected virtual bool ParseImportsList()
	{
		ISyntaxNode node = new SyntaxNode(base.TokenPosition, string.Empty, 2, SyntaxNodeOptions.Outlining);
		AddNode(node);
		return ParseImportsList(node);
	}

	protected virtual bool ParseImportDeclaration()
	{
		return ParseImportDeclaration(new SyntaxNode());
	}

	protected virtual bool ParseImportDeclaration(ISyntaxNode node)
	{
		bool result = true;
		Point tokenPosition = base.TokenPosition;
		if (ParseQualifiedIdentifier(out var identifier) || identifier != string.Empty)
		{
			node.Position = tokenPosition;
			node.Name = identifier;
			node.NodeType = 3;
			AddNode(node);
			if (base.Token == 160)
			{
				MoveNext();
				Point tokenPosition2 = base.TokenPosition;
				if (ParseQualifiedIdentifier(out identifier))
				{
					node.AddAttribute(new SyntaxAttribute(tokenPosition2, NetNodeType.UsingAlias.ToString(), identifier));
				}
				else
				{
					result = false;
				}
			}
			node.Range.EndPoint = prevPosition;
		}
		return result;
	}

	protected virtual bool ParseImportsList(ISyntaxNode node)
	{
		bool result = true;
		node.AddAttribute(new SyntaxAttribute(base.TokenPosition, SyntaxConsts.OutlineText, SyntaxConsts.OutlineImportsText));
		if (!BeforeDeclaration(node))
		{
			result = false;
		}
		base.SyntaxTree.Push(node);
		try
		{
			int num = lineIndex;
			bool flag = true;
			while (base.Token == 64 || flag)
			{
				MoveNext();
				if (!ParseImportDeclaration())
				{
					result = false;
				}
				flag = base.Token == 152 && num == lineIndex;
			}
		}
		finally
		{
			base.SyntaxTree.Pop();
		}
		if (!AfterDeclaration(node))
		{
			result = false;
		}
		node.Range.EndPoint = prevPosition;
		return result;
	}

	protected virtual bool ParseOptionStatement()
	{
		bool result = true;
		Point tokenPosition = base.TokenPosition;
		MoveNext();
		switch (base.TokenString.ToLower(CultureInfo.InvariantCulture))
		{
		case "explicit":
		case "strict":
		{
			ISyntaxNode syntaxNode2 = new SyntaxNode(tokenPosition, base.TokenString, 171);
			AddNode(syntaxNode2);
			MoveNext();
			string text2 = base.TokenString.ToLower(CultureInfo.InvariantCulture);
			if (text2 == "on" || text2 == "off")
			{
				syntaxNode2.AddAttribute(new SyntaxAttribute(base.TokenPosition, NetNodeType.Attribute.ToString(), base.TokenString));
				MoveNext();
			}
			syntaxNode2.Range.EndPoint = prevPosition;
			break;
		}
		case "compare":
		{
			ISyntaxNode syntaxNode = new SyntaxNode(tokenPosition, base.TokenString, 171);
			AddNode(syntaxNode);
			MoveNext();
			string text = base.TokenString.ToLower(CultureInfo.InvariantCulture);
			if (text == "text" || text == "binary")
			{
				syntaxNode.AddAttribute(new SyntaxAttribute(base.TokenPosition, NetNodeType.Attribute.ToString(), base.TokenString));
				MoveNext();
			}
			else
			{
				SyntaxError();
				result = false;
			}
			syntaxNode.Range.EndPoint = prevPosition;
			break;
		}
		default:
			result = false;
			SyntaxError();
			break;
		}
		return result;
	}

	protected override bool ParseDeclaration(ISyntaxNode node)
	{
		ISyntaxAttributes attrs = null;
		ParseModifiers(ref attrs);
		switch ((VbLexerToken)base.Token)
		{
		case VbLexerToken.Class:
		case VbLexerToken.Enum:
		case VbLexerToken.Interface:
		case VbLexerToken.Module:
		case VbLexerToken.Namespace:
		case VbLexerToken.Structure:
			return ParseDeclaration(attrs, node, node.NodeType);
		default:
			return false;
		}
	}

	protected virtual bool ParseDeclaration(ISyntaxAttributes attrs, int nodeType)
	{
		return ParseDeclaration(attrs, null, nodeType);
	}

	protected virtual bool ParseDeclarationEnd(int nodeType)
	{
		return Expected(GetBlockToken(nodeType));
	}

	protected virtual bool ParseDeclaration(ISyntaxAttributes attrs, ISyntaxNode node, int nodeType)
	{
		Point point = prevPosition;
		bool result = true;
		bool flag = node == null;
		if (flag)
		{
			node = new SyntaxNode((attrs != null && attrs.Count > 0) ? attrs[0].Position : base.TokenPosition, base.TokenString, nodeType);
		}
		if (attrs != null)
		{
			node.AddAttributes(attrs);
		}
		MoveNext();
		Point tokenPosition = base.TokenPosition;
		if ((nodeType == 7) ? ParseQualifiedIdentifier(out var identifier) : ParseIdentifier(out identifier))
		{
			node.Name = identifier;
			node.AddAttribute(new SyntaxAttribute(tokenPosition, NetNodeType.Name.ToString(), identifier));
			node.Options |= SyntaxNodeOptions.CodeCompletion;
			if (flag)
			{
				AddNode(node);
			}
			if (!BeforeDeclaration(node))
			{
				result = false;
			}
			Point point2 = prevPosition;
			switch (nodeType)
			{
			case 11:
				if (!ParseEnumDeclaration(node))
				{
					result = false;
				}
				point2 = prevPosition;
				break;
			default:
				if (!ParseStatementTerminator() || !ParseInheritsOrImplementsClause(node))
				{
					result = false;
				}
				break;
			case 7:
				break;
			}
			point2 = ((point.Y == point2.Y) ? point : point2);
			node.AddAttribute(new SyntaxAttribute(point2, SyntaxConsts.DeclarationScope, null));
			base.SyntaxTree.Push(node);
			try
			{
				if (base.Token == 149)
				{
					MoveNext();
					if (!ParseTypeParameterList() || !Expected(VbLexerToken.Close_parens))
					{
						result = false;
					}
				}
				node.Options |= SyntaxNodeOptions.Indentation | SyntaxNodeOptions.Outlining;
				if (nodeType == 11)
				{
					if (!ParseEnumBody())
					{
						result = false;
					}
				}
				else if (!ParseClassBody())
				{
					result = false;
				}
			}
			finally
			{
				base.SyntaxTree.Pop();
			}
			if (ParseDeclarationEnd(nodeType))
			{
				node.AddAttribute(new SyntaxAttribute(prevPosition, SyntaxConsts.DefinitionScopeEnd, null));
			}
			else
			{
				string text = "\r\n" + VbLexerToken.End.ToString() + " ";
				if (nodeType == 9)
				{
					text += SyntaxConsts.StructureResWord;
				}
				else
				{
					string text2 = text;
					NetNodeType netNodeType = (NetNodeType)nodeType;
					text = text2 + netNodeType;
				}
				node.AddAttribute(new SyntaxAttribute(prevPosition, SyntaxConsts.DefinitionScopeEndExpected, text));
				result = false;
			}
		}
		if (!AfterDeclaration(node))
		{
			result = false;
		}
		node.Range.EndPoint = prevPosition;
		return result;
	}

	protected virtual bool ParseTypeParameterConstraintsClause()
	{
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, string.Empty, 54);
		bool flag = IsKeywordToken(base.Token);
		if (flag)
		{
			syntaxNode.Name = base.TokenString;
			MoveNext();
		}
		else
		{
			flag = ParseIdentifier(out var identifier);
			if (flag)
			{
				syntaxNode.Name = identifier;
			}
		}
		syntaxNode.Range.EndPoint = prevPosition;
		if (flag)
		{
			AddNode(syntaxNode);
		}
		return flag;
	}

	protected virtual bool ParseTypeParameterConstraintsClauses()
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, string.Empty, 53);
		AddNode(syntaxNode);
		bool flag = base.Token == 147;
		if (flag)
		{
			MoveNext();
		}
		base.SyntaxTree.Push(syntaxNode);
		try
		{
			do
			{
				if (!ParseTypeParameterConstraintsClause())
				{
					result = false;
				}
				if (base.Token == 152)
				{
					MoveNext();
					continue;
				}
				break;
			}
			while (!base.Eof);
		}
		finally
		{
			base.SyntaxTree.Pop();
		}
		if (flag && !Expected(VbLexerToken.Close_brace))
		{
			result = false;
		}
		syntaxNode.Range.EndPoint = prevPosition;
		return result;
	}

	protected virtual bool ParseTypeParameterList()
	{
		bool flag = Expected(VbLexerToken.Of);
		if (flag)
		{
			Point tokenPosition = base.TokenPosition;
			if (ParseTypeName(out var type))
			{
				ISyntaxNode syntaxNode = new SyntaxNode(tokenPosition, type, 51);
				AddNode(syntaxNode);
				if (base.Token == 6)
				{
					base.SyntaxTree.Push(syntaxNode);
					try
					{
						MoveNext();
						if (!ParseTypeParameterConstraintsClauses())
						{
							flag = false;
						}
					}
					finally
					{
						base.SyntaxTree.Pop();
					}
				}
				syntaxNode.Range.EndPoint = prevPosition;
			}
			else
			{
				flag = false;
			}
		}
		return flag;
	}

	protected virtual bool ParseEnumDeclaration(ISyntaxNode node)
	{
		bool result = true;
		if (IsType(base.Token))
		{
			Point tokenPosition = base.TokenPosition;
			MoveNext();
			if (ParseType(out var type))
			{
				Point point = prevPosition;
				node.AddAttribute(new SyntaxAttribute(tokenPosition, NetNodeType.Type.ToString(), type));
			}
			else
			{
				result = false;
			}
		}
		return result;
	}

	protected virtual bool ParseEnumBody()
	{
		bool result = true;
		while (!base.Eof)
		{
			if (base.Token == 45)
			{
				MoveNext();
				break;
			}
			if (!ParseEnumMember())
			{
				result = false;
			}
		}
		return result;
	}

	protected virtual bool ParseEnumMember()
	{
		bool result = true;
		Point tokenPosition = base.TokenPosition;
		ISyntaxNode syntaxNode = new SyntaxNode(tokenPosition, string.Empty, 13, SyntaxNodeOptions.CodeCompletion);
		AddNode(syntaxNode);
		base.SyntaxTree.Push(syntaxNode);
		try
		{
			if (base.Token == 161 && !ParseAttributeListDeclaration())
			{
				result = false;
			}
			if (ParseIdentifier(out var identifier))
			{
				syntaxNode.Name = identifier;
			}
			else
			{
				MoveNext();
				result = false;
			}
			if (base.Token == 160)
			{
				MoveNext();
				ISyntaxNode node = null;
				if (!ParseExpression(ref node))
				{
					result = false;
				}
				if (node != null)
				{
					syntaxNode.AddChild(node);
				}
			}
		}
		finally
		{
			base.SyntaxTree.Pop();
		}
		syntaxNode.Range.EndPoint = prevPosition;
		return result;
	}

	protected virtual bool IsDeclarationToken(int token)
	{
		switch ((VbLexerToken)token)
		{
		case VbLexerToken.Class:
		case VbLexerToken.Delegate:
		case VbLexerToken.Enum:
		case VbLexerToken.Event:
		case VbLexerToken.Function:
		case VbLexerToken.Imports:
		case VbLexerToken.Interface:
		case VbLexerToken.Module:
		case VbLexerToken.Namespace:
		case VbLexerToken.Operator:
		case VbLexerToken.Option:
		case VbLexerToken.Property:
		case VbLexerToken.Structure:
		case VbLexerToken.Sub:
			return true;
		default:
			return false;
		}
	}

	protected virtual bool IsKnownMemberDeclaration(int token)
	{
		switch ((VbLexerToken)token)
		{
		case VbLexerToken.Const:
		case VbLexerToken.Dim:
		case VbLexerToken.New:
		case VbLexerToken.Static:
			return true;
		default:
			return IsDeclarationToken(token);
		}
	}

	protected virtual bool ParseEndBlock(int token)
	{
		if (base.Token == 45)
		{
			MoveNext();
			return true;
		}
		return false;
	}

	protected virtual bool ParseClassBody()
	{
		bool result = true;
		bool flag = base.SyntaxTree.Current != base.SyntaxTree.Root;
		ISyntaxAttributes attrs = null;
		while (!base.Eof && (!flag || !ParseEndBlock(base.Token)))
		{
			ParseModifiers(ref attrs);
			if (IsKnownMemberDeclaration(base.Token))
			{
				if (!ParseKnownMemberDeclaration(attrs))
				{
					result = false;
				}
				attrs = null;
				continue;
			}
			switch ((VbLexerToken)base.Token)
			{
			case VbLexerToken.Directive_Literal:
				if (!ParseDirective())
				{
					result = false;
				}
				continue;
			case VbLexerToken.Op_lt:
				if (!ParseAttributeListDeclaration())
				{
					result = false;
				}
				continue;
			}
			if (!ParseVariableMemberDeclaration(attrs, base.TokenPosition, 13))
			{
				result = false;
			}
			attrs = null;
		}
		return result;
	}

	protected virtual bool ParseKnownMemberDeclaration(ISyntaxAttributes attrs)
	{
		bool result = true;
		switch ((VbLexerToken)base.Token)
		{
		case VbLexerToken.Option:
			if (!ParseOptionStatement())
			{
				result = false;
			}
			break;
		case VbLexerToken.Imports:
			if (!ParseImportsList())
			{
				result = false;
			}
			break;
		case VbLexerToken.Namespace:
			if (!ParseDeclaration(attrs, 7))
			{
				result = false;
			}
			break;
		case VbLexerToken.Class:
			if (!ParseDeclaration(attrs, 8))
			{
				result = false;
			}
			break;
		case VbLexerToken.Module:
			if (!ParseDeclaration(attrs, 12))
			{
				result = false;
			}
			break;
		case VbLexerToken.Structure:
			if (!ParseDeclaration(attrs, 9))
			{
				result = false;
			}
			break;
		case VbLexerToken.Interface:
			if (!ParseDeclaration(attrs, 10))
			{
				result = false;
			}
			break;
		case VbLexerToken.Enum:
			if (!ParseDeclaration(attrs, 11))
			{
				result = false;
			}
			break;
		case VbLexerToken.Function:
		case VbLexerToken.Sub:
			if (!ParseMethodDeclaration(attrs))
			{
				result = false;
			}
			break;
		case VbLexerToken.Delegate:
			if (!ParseDelegateDeclaration(attrs))
			{
				result = false;
			}
			break;
		case VbLexerToken.Event:
			if (!ParseEventDeclaration(attrs))
			{
				result = false;
			}
			break;
		case VbLexerToken.Property:
			if (!ParsePropertyDeclaration(attrs))
			{
				result = false;
			}
			break;
		case VbLexerToken.New:
			if (!ParseConstructorDeclaration(attrs))
			{
				result = false;
			}
			break;
		case VbLexerToken.Const:
		case VbLexerToken.Dim:
		case VbLexerToken.Static:
			if (!ParseDimDeclaration(attrs, 13))
			{
				result = false;
			}
			break;
		case VbLexerToken.Operator:
			if (!ParseOperatorDeclaration(attrs))
			{
				result = false;
			}
			break;
		}
		return result;
	}

	protected virtual bool ParseDimDeclaration(ISyntaxAttributes attrs, int nodeType)
	{
		Point tokenPosition = base.TokenPosition;
		bool flag = base.Token == 124;
		MoveNext();
		if (flag && base.Token == 38)
		{
			MoveNext();
		}
		return ParseVariableMemberDeclaration(attrs, tokenPosition, nodeType);
	}

	protected virtual bool ParseVariableMemberDeclaration(ISyntaxAttributes attrs, Point pos, int nodeType)
	{
		bool result = true;
		while (!base.Eof)
		{
			if (IsIdentifierToken(base.Token))
			{
				string identifier = string.Empty;
				if (ParseVariableIdentifier(out identifier, out var isArray))
				{
					ISyntaxNodes syntaxNodes = null;
					while (base.Token == 152)
					{
						MoveNext();
						Point tokenPosition = base.TokenPosition;
						if (ParseVariableIdentifier(out var identifier2, out var isArray2))
						{
							if (syntaxNodes == null)
							{
								syntaxNodes = new SyntaxNodes();
							}
							ISyntaxNode syntaxNode = new SyntaxNode(tokenPosition, identifier2, 13, SyntaxNodeOptions.Indentation | SyntaxNodeOptions.CodeCompletion);
							syntaxNodes.Add(syntaxNode);
							if (isArray2)
							{
								syntaxNode.AddAttribute(new SyntaxAttribute(base.TokenPosition, NetNodeType.ArraySpecifier.ToString(), null));
							}
						}
					}
					VbLexerToken vbLexerToken = (VbLexerToken)base.Token;
					if (vbLexerToken == VbLexerToken.As || vbLexerToken == VbLexerToken.Assign)
					{
						ParseFieldDeclaration(attrs, pos, identifier, syntaxNodes, nodeType, isArray);
					}
					else
					{
						SyntaxError(6);
						result = false;
					}
				}
				else
				{
					SyntaxError();
					result = false;
				}
				if (base.Token != 152)
				{
					break;
				}
				MoveNext();
				continue;
			}
			SyntaxError();
			MoveNext();
			result = false;
			break;
		}
		return result;
	}

	protected virtual bool ParseVariableIdentifier(out string identifier, out bool isArray)
	{
		bool result = true;
		identifier = string.Empty;
		if (!ParseQualifiedIdentifier(out identifier))
		{
			result = false;
		}
		TryParseArraySizeInitializationModifier(out var rank);
		isArray = rank != string.Empty;
		return result;
	}

	protected virtual bool ParseFieldDeclaration(ISyntaxAttributes attrs, Point pos, string name, ISyntaxNodes names, int nodeType, bool isArray)
	{
		bool result = true;
		pos = ((attrs != null && attrs.Count > 0) ? attrs[0].Position : pos);
		ISyntaxNode syntaxNode = new SyntaxNode(pos, name, nodeType, SyntaxNodeOptions.Indentation | SyntaxNodeOptions.CodeCompletion);
		if (isArray)
		{
			syntaxNode.AddAttribute(new SyntaxAttribute(base.TokenPosition, NetNodeType.ArraySpecifier.ToString(), null));
		}
		if (names != null)
		{
			syntaxNode.AddChildren(names);
		}
		if (attrs != null)
		{
			syntaxNode.AddAttributes(attrs);
		}
		if (IsType(base.Token))
		{
			syntaxNode.AddAttribute(new SyntaxAttribute(base.TokenPosition, NetNodeType.Modifier.ToString(), base.TokenString));
			MoveNext();
			Point empty = Point.Empty;
			string type = string.Empty;
			if (base.Token == 86)
			{
				ISyntaxNode node = null;
				if (!ParseVariableInitializer(ref node))
				{
					result = false;
				}
				if (node != null)
				{
					syntaxNode.AddChild(node);
				}
				empty = prevPosition;
				ISyntaxAttribute syntaxAttribute = node.FindAttribute(NetNodeType.Type.ToString());
				if (syntaxAttribute != null)
				{
					type = syntaxAttribute.Value.ToString();
				}
			}
			else
			{
				empty = base.TokenPosition;
				if (!ParseTypeName(out type))
				{
					result = false;
				}
			}
			if (type != string.Empty)
			{
				syntaxNode.AddAttribute(new SyntaxAttribute(empty, NetNodeType.Type.ToString(), type));
				if (base.Token == 149)
				{
					ISyntaxNode node2 = null;
					if (!ParseArgumentList(ref node2, generic: true))
					{
						result = false;
					}
					if (node2 != null)
					{
						syntaxNode.AddChild(node2);
					}
				}
			}
			else
			{
				result = false;
				prevPosition = base.TokenPosition;
			}
		}
		AddNode(syntaxNode);
		if (!BeforeDeclaration(syntaxNode))
		{
			result = false;
		}
		if (base.Token == 160)
		{
			MoveNext();
			ISyntaxNode node3 = null;
			if (!ParseVariableInitializer(ref node3))
			{
				result = false;
			}
			if (node3 != null)
			{
				syntaxNode.AddChild(node3);
			}
		}
		if (!AfterDeclaration(syntaxNode))
		{
			result = false;
		}
		syntaxNode.Range.EndPoint = prevPosition;
		return result;
	}

	protected virtual bool ParseLabelName(out string name)
	{
		name = string.Empty;
		VbLexerToken vbLexerToken = (VbLexerToken)base.Token;
		if ((uint)(vbLexerToken - 188) <= 3u)
		{
			name = base.TokenString;
			MoveNext();
			return true;
		}
		SyntaxError(188);
		return false;
	}

	protected virtual bool ParseVariableInitializer(ref ISyntaxNode node)
	{
		bool flag = true;
		VbLexerToken vbLexerToken = (VbLexerToken)base.Token;
		if (vbLexerToken == VbLexerToken.Open_brace)
		{
			return ParseArrayInitializerExpression(ref node);
		}
		return ParseExpression(ref node);
	}

	protected virtual bool ParseArrayInitializerExpression(ref ISyntaxNode node)
	{
		bool result = true;
		node = new SyntaxNode(base.TokenPosition, base.TokenString, 151);
		MoveNext();
		while (!base.Eof && base.Token != 148)
		{
			ISyntaxNode node2 = null;
			result = ParseVariableInitializer(ref node2);
			if (node2 != null)
			{
				node.AddChild(node2);
			}
			if (base.Token != 152)
			{
				break;
			}
			MoveNext();
		}
		if (!Expected(VbLexerToken.Close_brace))
		{
			result = false;
		}
		return result;
	}

	protected virtual bool ParseMethodDeclaration(ISyntaxAttributes attrs)
	{
		Point tokenPosition = base.TokenPosition;
		int endToken = base.Token;
		MoveNext();
		if (base.Token == 86)
		{
			return ParseConstructorDeclaration(attrs, tokenPosition);
		}
		ISyntaxNode syntaxNode = new SyntaxNode((attrs != null && attrs.Count > 0) ? attrs[0].Position : tokenPosition, string.Empty, 17, SyntaxNodeOptions.CodeCompletion);
		if (attrs != null)
		{
			syntaxNode.AddAttributes(attrs);
		}
		if (ParseIdentifier(out var identifier))
		{
			syntaxNode.Name = identifier;
			AddNode(syntaxNode);
			return ParseMethodDeclaration(syntaxNode, endToken, block: true);
		}
		return false;
	}

	protected virtual bool IsFunctionToken(int token)
	{
		return token == 56;
	}

	protected virtual bool ParseMethodDeclaration(ISyntaxNode node, int endToken, bool block)
	{
		bool result = true;
		if (!BeforeDeclaration(node))
		{
			result = false;
		}
		bool flag = base.SyntaxTree.Current.NodeType != 10;
		base.SyntaxTree.Push(node);
		try
		{
			if (base.Token == 149 && !ParseParameterListDeclaration())
			{
				result = false;
			}
			if (IsFunctionToken(endToken))
			{
				if (IsType(base.Token))
				{
					MoveNext();
					Point tokenPosition = base.TokenPosition;
					if (ParseType(out var type))
					{
						node.AddAttribute(new SyntaxAttribute(tokenPosition, NetNodeType.Type.ToString(), type));
					}
					else
					{
						result = false;
					}
				}
				else
				{
					result = false;
				}
			}
			switch ((VbLexerToken)base.Token)
			{
			case VbLexerToken.Handles:
				flag = true;
				if (!ParseHandlesClause())
				{
					result = false;
				}
				break;
			case VbLexerToken.Implements:
				flag = true;
				if (!ParseImplementsClause())
				{
					result = false;
				}
				break;
			}
			node.AddAttribute(new SyntaxAttribute(prevPosition, SyntaxConsts.DeclarationScope, null));
			if (block && flag && !ParseMethodBody(node, endToken))
			{
				result = false;
			}
		}
		finally
		{
			base.SyntaxTree.Pop();
		}
		node.Range.EndPoint = prevPosition;
		if (!AfterDeclaration(node))
		{
			result = false;
		}
		return result;
	}

	protected virtual bool ParseHandlesClause()
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString, 46);
		AddNode(syntaxNode);
		base.SyntaxTree.Push(syntaxNode);
		try
		{
			MoveNext();
			do
			{
				if (base.Token == 152)
				{
					MoveNext();
				}
				Point tokenPosition = base.TokenPosition;
				string text = string.Empty;
				if (base.Token == 83)
				{
					text = base.TokenString;
					MoveNext();
					if (Expected(VbLexerToken.Dot))
					{
						text += ".";
					}
					else
					{
						result = false;
					}
				}
				if (ParseQualifiedIdentifier(out var identifier))
				{
					text += identifier;
				}
				else
				{
					result = false;
				}
				syntaxNode.AddAttribute(new SyntaxAttribute(tokenPosition, NetNodeType.Name.ToString(), text));
			}
			while (base.Token == 152);
		}
		finally
		{
			base.SyntaxTree.Pop();
		}
		syntaxNode.Range.EndPoint = prevPosition;
		return result;
	}

	protected virtual bool ParseImplementsClause()
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString, 47);
		AddNode(syntaxNode);
		base.SyntaxTree.Push(syntaxNode);
		try
		{
			do
			{
				MoveNext();
				Point tokenPosition = base.TokenPosition;
				if (ParseType(out var type))
				{
					syntaxNode.AddAttribute(new SyntaxAttribute(tokenPosition, NetNodeType.Type.ToString(), type));
				}
				else
				{
					result = false;
				}
			}
			while (base.Token == 152);
		}
		finally
		{
			base.SyntaxTree.Pop();
		}
		syntaxNode.Range.EndPoint = prevPosition;
		return result;
	}

	protected virtual bool ParseOperatorDeclaration(ISyntaxAttributes attrs)
	{
		bool flag = true;
		Point tokenPosition = base.TokenPosition;
		MoveNext();
		ISyntaxNode syntaxNode = new SyntaxNode((attrs != null && attrs.Count > 0) ? attrs[0].Position : tokenPosition, base.TokenString, 23);
		AddNode(syntaxNode);
		if (attrs != null)
		{
			syntaxNode.AddAttributes(attrs);
		}
		if (!BeforeDeclaration(syntaxNode))
		{
			flag = false;
		}
		base.SyntaxTree.Push(syntaxNode);
		try
		{
			switch ((VbLexerToken)base.Token)
			{
			case VbLexerToken.And:
			case VbLexerToken.IsFalse:
			case VbLexerToken.IsTrue:
			case VbLexerToken.Like:
			case VbLexerToken.Mod:
			case VbLexerToken.Not:
			case VbLexerToken.Or:
			case VbLexerToken.Xor:
			case VbLexerToken.Plus:
			case VbLexerToken.Minus:
			case VbLexerToken.Assign:
			case VbLexerToken.Op_lt:
			case VbLexerToken.Op_gt:
			case VbLexerToken.Op_long:
			case VbLexerToken.Star:
			case VbLexerToken.Div:
			case VbLexerToken.IntDiv:
			case VbLexerToken.Carret:
			case VbLexerToken.Op_shift_left:
			case VbLexerToken.Op_shift_right:
			case VbLexerToken.Op_le:
			case VbLexerToken.Op_ge:
			case VbLexerToken.Op_ne:
				MoveNext();
				break;
			default:
				flag = false;
				SyntaxError();
				break;
			}
			if (flag)
			{
				if (!ParseParameterListDeclaration())
				{
					flag = false;
				}
				if (IsType(base.Token))
				{
					MoveNext();
					tokenPosition = base.TokenPosition;
					if (ParseType(out var type))
					{
						syntaxNode.AddAttribute(new SyntaxAttribute(tokenPosition, NetNodeType.Type.ToString(), type));
					}
					else
					{
						flag = false;
					}
				}
				else
				{
					flag = false;
				}
				syntaxNode.AddAttribute(new SyntaxAttribute(prevPosition, SyntaxConsts.DeclarationScope, null));
				if (!ParseMethodBody(syntaxNode, 95))
				{
					flag = false;
				}
			}
		}
		finally
		{
			base.SyntaxTree.Pop();
		}
		if (!AfterDeclaration(syntaxNode))
		{
			flag = false;
		}
		syntaxNode.Range.EndPoint = prevPosition;
		return flag;
	}

	protected virtual bool ParseDelegateDeclaration(ISyntaxAttributes attrs)
	{
		bool flag = true;
		int endToken = 129;
		ISyntaxNode syntaxNode = new SyntaxNode((attrs != null && attrs.Count > 0) ? attrs[0].Position : base.TokenPosition, string.Empty, 20, SyntaxNodeOptions.CodeCompletion);
		if (attrs != null)
		{
			syntaxNode.AddAttributes(attrs);
		}
		MoveNext();
		if (base.Token == 56 || base.Token == 129)
		{
			endToken = base.Token;
			MoveNext();
		}
		else
		{
			flag = false;
		}
		if (ParseIdentifier(out var identifier))
		{
			syntaxNode.Name = identifier;
		}
		AddNode(syntaxNode);
		return ParseMethodDeclaration(syntaxNode, endToken, block: false);
	}

	protected virtual bool ParseConstructorDeclaration(ISyntaxAttributes attrs)
	{
		return ParseConstructorDeclaration(attrs, base.TokenPosition);
	}

	protected virtual bool ParseConstructorDeclaration(ISyntaxAttributes attrs, Point pos)
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode((attrs != null && attrs.Count > 0) ? attrs[0].Position : pos, VbLexerToken.New.ToString(), 18, SyntaxNodeOptions.CodeCompletion);
		if (attrs != null)
		{
			syntaxNode.AddAttributes(attrs);
		}
		MoveNext();
		AddNode(syntaxNode);
		if (!BeforeDeclaration(syntaxNode))
		{
			result = false;
		}
		base.SyntaxTree.Push(syntaxNode);
		try
		{
			if (base.Token == 149 && !ParseParameterListDeclaration())
			{
				result = false;
			}
			syntaxNode.AddAttribute(new SyntaxAttribute(prevPosition, SyntaxConsts.DeclarationScope, null));
			if (!ParseMethodBody(syntaxNode, 129))
			{
				result = false;
			}
		}
		finally
		{
			base.SyntaxTree.Pop();
		}
		if (!AfterDeclaration(syntaxNode))
		{
			result = false;
		}
		syntaxNode.Range.EndPoint = prevPosition;
		return result;
	}

	protected virtual bool ParseMethodBlock(int endToken)
	{
		return ParseBlockStatement(45, endToken);
	}

	protected virtual bool ParseMethodBody(ISyntaxNode node, int endToken)
	{
		bool result = true;
		node.Options |= SyntaxNodeOptions.Indentation | SyntaxNodeOptions.Outlining;
		if (!ParseMethodBlock(endToken))
		{
			result = false;
		}
		node.AddAttribute(new SyntaxAttribute(prevPosition, SyntaxConsts.DefinitionScopeEnd, null));
		node.Range.EndPoint = prevPosition;
		return result;
	}

	protected virtual bool ParseEventDeclaration(ISyntaxAttributes attrs)
	{
		bool flag = true;
		MoveNext();
		Point tokenPosition = base.TokenPosition;
		if (ParseIdentifier(out var identifier))
		{
			ISyntaxNode syntaxNode = new SyntaxNode((attrs != null && attrs.Count > 0) ? attrs[0].Position : tokenPosition, identifier, 22, SyntaxNodeOptions.CodeCompletion);
			if (attrs != null)
			{
				syntaxNode.AddAttributes(attrs);
			}
			AddNode(syntaxNode);
			if (!IsType(base.Token))
			{
				flag = base.Token == 149 && ParseParameterListDeclaration();
			}
			else
			{
				MoveNext();
				Point tokenPosition2 = base.TokenPosition;
				flag = ParseType(out var type);
				syntaxNode.AddAttribute(new SyntaxAttribute(tokenPosition2, NetNodeType.Type.ToString(), type));
			}
			if (base.Token == 63)
			{
				string tokenString = base.TokenString;
				tokenPosition = base.TokenPosition;
				if (ParseImplements(out var types))
				{
					syntaxNode.AddAttribute(new SyntaxAttribute(tokenPosition, tokenString, types));
				}
			}
			syntaxNode.Range.EndPoint = prevPosition;
		}
		else
		{
			flag = false;
		}
		return flag;
	}

	protected virtual bool ParseAttributeListDeclaration()
	{
		Point tokenPosition = base.TokenPosition;
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode(tokenPosition, string.Empty, 31, SyntaxNodeOptions.Indentation);
		AddNode(syntaxNode);
		base.SyntaxTree.Push(syntaxNode);
		try
		{
			MoveNext();
			if (!TryParseAttributeModifier())
			{
				result = false;
			}
			while (!base.Eof && base.Token != 162)
			{
				if (!ParseAttributeDeclaration())
				{
					result = false;
				}
				if (base.Token == 152)
				{
					MoveNext();
					continue;
				}
				break;
			}
		}
		finally
		{
			base.SyntaxTree.Pop();
		}
		if (!Expected(VbLexerToken.Op_gt))
		{
			result = false;
		}
		syntaxNode.Range.EndPoint = prevPosition;
		return result;
	}

	protected virtual bool TryParseAttributeModifier()
	{
		bool result = true;
		VbLexerToken vbLexerToken = (VbLexerToken)base.Token;
		if (vbLexerToken == VbLexerToken.Assembly || vbLexerToken == VbLexerToken.Module)
		{
			AddAttribute(new SyntaxAttribute(base.TokenPosition, NetNodeType.AttributeTarget.ToString(), base.TokenString));
			MoveNext();
			if (!Expected(VbLexerToken.Colon))
			{
				result = false;
			}
		}
		return result;
	}

	protected virtual bool ParseAttributeDeclaration()
	{
		Point tokenPosition = base.TokenPosition;
		bool result = true;
		if (ParseQualifiedIdentifier(out var identifier))
		{
			ISyntaxNode syntaxNode = new SyntaxNode(tokenPosition, identifier, 30);
			AddNode(syntaxNode);
			if (base.Token == 149)
			{
				ISyntaxNode syntaxNode2 = new SyntaxNode(base.TokenPosition, string.Empty, 27);
				syntaxNode.AddChild(syntaxNode2);
				base.SyntaxTree.Push(syntaxNode2);
				try
				{
					MoveNext();
					while (!base.Eof && base.Token != 150)
					{
						result = ParsePositionalOrNamedParam();
						syntaxNode2.Range.EndPoint = prevPosition;
						if (base.Token == 152)
						{
							MoveNext();
							continue;
						}
						break;
					}
				}
				finally
				{
					base.SyntaxTree.Pop();
				}
				if (!Expected(VbLexerToken.Close_parens))
				{
					result = false;
				}
			}
			syntaxNode.Range.EndPoint = prevPosition;
		}
		else
		{
			result = false;
		}
		return result;
	}

	protected virtual bool ParsePositionalOrNamedParam()
	{
		bool flag = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, string.Empty, 26);
		AddNode(syntaxNode);
		if (IsIdentifierToken(base.Token))
		{
			bool flag2 = false;
			SaveState();
			try
			{
				string tokenString = base.TokenString;
				MoveNext();
				if (base.Token == 154)
				{
					MoveNext();
				}
				if (base.Token == 160)
				{
					flag2 = true;
					MoveNext();
					syntaxNode.Name = tokenString;
				}
			}
			finally
			{
				RestoreState(!flag2);
			}
		}
		ISyntaxNode node = null;
		flag = ParseExpression(ref node);
		if (node != null)
		{
			syntaxNode.AddChild(node);
		}
		syntaxNode.Range.EndPoint = prevPosition;
		return flag;
	}

	protected virtual bool ParsePropertyDeclaration(ISyntaxAttributes attrs)
	{
		Point tokenPosition = base.TokenPosition;
		MoveNext();
		ISyntaxNode syntaxNode = new SyntaxNode((attrs != null && attrs.Count > 0) ? attrs[0].Position : tokenPosition, string.Empty, 21, SyntaxNodeOptions.Indentation | SyntaxNodeOptions.CodeCompletion);
		if (attrs != null)
		{
			syntaxNode.AddAttributes(attrs);
		}
		if (ParseIdentifier(out var identifier))
		{
			syntaxNode.Name = identifier;
			AddNode(syntaxNode);
			return ParsePropertyDeclaration(syntaxNode);
		}
		return false;
	}

	protected virtual bool ParsePropertyDeclaration(ISyntaxNode node)
	{
		bool result = true;
		if (!BeforeDeclaration(node))
		{
			result = false;
		}
		bool flag = base.SyntaxTree.Current.NodeType != 10;
		base.SyntaxTree.Push(node);
		try
		{
			if (base.Token == 149 && !ParseParameterListDeclaration())
			{
				result = false;
			}
			if (IsType(base.Token))
			{
				MoveNext();
				Point tokenPosition = base.TokenPosition;
				if (ParseType(out var type))
				{
					node.AddAttribute(new SyntaxAttribute(tokenPosition, NetNodeType.Type.ToString(), type));
				}
				else
				{
					result = false;
				}
			}
			if (base.Token == 63)
			{
				string tokenString = base.TokenString;
				MoveNext();
				Point tokenPosition = base.TokenPosition;
				if (ParseImplements(out var types))
				{
					node.AddAttribute(new SyntaxAttribute(tokenPosition, tokenString, types));
				}
				else
				{
					result = false;
				}
			}
			node.AddAttribute(new SyntaxAttribute(prevPosition, SyntaxConsts.DeclarationScope, null));
			if (flag)
			{
				while (!base.Eof)
				{
					if (base.Token == 45)
					{
						MoveNext();
						break;
					}
					if (!ParseAccessor(NetNodeType.PropertyAccessor))
					{
						result = false;
					}
				}
				if (!Expected(VbLexerToken.Property))
				{
					result = false;
				}
				node.AddAttribute(new SyntaxAttribute(prevPosition, SyntaxConsts.DefinitionScopeEnd, null));
			}
		}
		finally
		{
			base.SyntaxTree.Pop();
		}
		node.Range.EndPoint = prevPosition;
		if (!AfterDeclaration(node))
		{
			result = false;
		}
		return result;
	}

	protected virtual bool IsAccessor()
	{
		if (base.Token != 57)
		{
			return base.Token == 119;
		}
		return true;
	}

	protected virtual bool ParseAccessor(NetNodeType accessorType)
	{
		bool result = true;
		Point tokenPosition = base.TokenPosition;
		ISyntaxNode syntaxNode = new SyntaxNode(tokenPosition, string.Empty, (int)accessorType, SyntaxNodeOptions.Indentation);
		AddNode(syntaxNode);
		base.SyntaxTree.Push(syntaxNode);
		try
		{
			if (base.Token == 161 && !ParseAttributeListDeclaration())
			{
				result = false;
			}
			if (IsAccessor())
			{
				VbLexerToken vbLexerToken = (VbLexerToken)base.Token;
				syntaxNode.Position = base.TokenPosition;
				syntaxNode.Name = base.TokenString;
				MoveNext();
				if (vbLexerToken == VbLexerToken.Set && base.Token == 149 && !ParseParameterListDeclaration())
				{
					result = false;
				}
				syntaxNode.AddAttribute(new SyntaxAttribute(prevPosition, SyntaxConsts.DeclarationScope, null));
				if (base.SyntaxTree.Current.NodeType != 10 && !ParseAccessorBody(syntaxNode, (int)vbLexerToken))
				{
					result = false;
				}
			}
			else
			{
				SyntaxError();
				MoveNext();
				result = false;
			}
		}
		finally
		{
			base.SyntaxTree.Pop();
		}
		syntaxNode.Range.EndPoint = prevPosition;
		return result;
	}

	protected virtual bool ParseAccessorBody(ISyntaxNode parent, int endToken)
	{
		return ParseMethodBody(parent, endToken);
	}

	protected bool ParseInheritsOrImplementsClause(ISyntaxNode node)
	{
		bool result = true;
		if (IsBaseList(base.Token))
		{
			ISyntaxAttribute syntaxAttribute = null;
			string types;
			if (base.Token == 66)
			{
				MoveNext();
				Point tokenPosition = base.TokenPosition;
				if (ParseInherits(out types, node.NodeType == 10))
				{
					syntaxAttribute = new SyntaxAttribute(tokenPosition, NetNodeType.TypeList.ToString(), types);
				}
				else
				{
					result = false;
				}
			}
			if (base.Token == 63)
			{
				MoveNext();
				Point tokenPosition = base.TokenPosition;
				if (ParseImplements(out types))
				{
					if (syntaxAttribute == null)
					{
						syntaxAttribute = new SyntaxAttribute(tokenPosition, NetNodeType.TypeList.ToString(), types);
					}
					else
					{
						syntaxAttribute.Value = (string)syntaxAttribute.Value + "," + types;
					}
				}
				else
				{
					result = false;
				}
			}
			if (syntaxAttribute != null)
			{
				node.AddAttribute(syntaxAttribute);
			}
		}
		return result;
	}

	protected virtual bool ParseParameterListDeclaration()
	{
		return ParseParameterListDeclaration(VbLexerToken.Open_parens, VbLexerToken.Close_parens);
	}

	protected virtual bool ParseParameterListDeclaration(VbLexerToken startToken, VbLexerToken endToken)
	{
		bool result = true;
		if (base.Token == (int)startToken)
		{
			ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, string.Empty, 27);
			AddNode(syntaxNode);
			base.SyntaxTree.Push(syntaxNode);
			try
			{
				MoveNext();
				while (!base.Eof && base.Token != (int)endToken)
				{
					if (!ParseParameterDeclaration())
					{
						result = false;
					}
					if (base.Token == 152)
					{
						MoveNext();
						continue;
					}
					break;
				}
			}
			finally
			{
				base.SyntaxTree.Pop();
			}
			if (!Expected(endToken))
			{
				result = false;
			}
			syntaxNode.Range.EndPoint = prevPosition;
		}
		else
		{
			SyntaxError((int)startToken);
			result = false;
		}
		return result;
	}

	protected virtual bool IsParameterModifier(int token)
	{
		switch ((VbLexerToken)token)
		{
		case VbLexerToken.ByRef:
		case VbLexerToken.ByVal:
		case VbLexerToken.Optional:
		case VbLexerToken.ParamArray:
			return true;
		default:
			return false;
		}
	}

	protected virtual bool ParseParameterDeclaration()
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, string.Empty, 26, SyntaxNodeOptions.CodeCompletion);
		if (base.Token == 161)
		{
			base.SyntaxTree.Push(syntaxNode);
			try
			{
				if (!ParseAttributeListDeclaration())
				{
					result = false;
				}
			}
			finally
			{
				base.SyntaxTree.Pop();
			}
		}
		while (IsParameterModifier(base.Token))
		{
			syntaxNode.AddAttribute(new SyntaxAttribute(base.TokenPosition, NetNodeType.ParameterModifier.ToString(), base.TokenString));
			MoveNext();
		}
		if (ParseVariableIdentifier(out var identifier, out var isArray))
		{
			syntaxNode.Name = identifier;
			if (isArray)
			{
				syntaxNode.AddAttribute(new SyntaxAttribute(base.TokenPosition, NetNodeType.ArraySpecifier.ToString(), null));
			}
			if (IsType(base.Token))
			{
				MoveNext();
				Point tokenPosition = base.TokenPosition;
				if (ParseType(out var type))
				{
					syntaxNode.AddAttribute(new SyntaxAttribute(tokenPosition, NetNodeType.Type.ToString(), type));
				}
				else
				{
					result = false;
				}
			}
			if (base.Token == 160)
			{
				MoveNext();
				syntaxNode.AddAttribute(new SyntaxAttribute(base.TokenPosition, NetNodeType.OptionalParameter.ToString(), null));
				ISyntaxNode node = null;
				if (!ParseConstantExpression(ref node))
				{
					result = false;
				}
				if (node != null)
				{
					syntaxNode.AddChild(node);
				}
			}
			AddNode(syntaxNode);
		}
		else
		{
			result = false;
		}
		syntaxNode.Range.EndPoint = prevPosition;
		return result;
	}

	protected virtual bool ParseArgument(ref ISyntaxNode node)
	{
		bool flag = true;
		node = new SyntaxNode(base.TokenPosition, string.Empty, 28);
		bool flag2 = false;
		ISyntaxNode node2 = null;
		if (IsIdentifierToken(base.Token))
		{
			SaveState();
			try
			{
				if (ParseIdentifier(out var identifier) && base.Token == 160)
				{
					flag2 = true;
					node.Name = identifier;
					MoveNext();
					flag = ParseExpression(ref node2);
				}
			}
			finally
			{
				RestoreState(!flag2);
			}
		}
		if (!flag2)
		{
			node2 = null;
			flag = ParseExpression(ref node2);
			if (!flag)
			{
				prevPosition = base.TokenPosition;
			}
		}
		if (node2 != null)
		{
			node.AddChild(node2);
		}
		node.Range.EndPoint = prevPosition;
		return flag;
	}

	protected virtual bool ParseArgumentList(ref ISyntaxNode node, bool generic)
	{
		bool result = true;
		node = new SyntaxNode(base.TokenPosition, base.TokenString, 29);
		if (base.Token == 149)
		{
			MoveNext();
			if (generic && base.Token == 93)
			{
				MoveNext();
				result = ParseTypeName(out var type);
				node.Name = type;
				node.NodeType = 50;
			}
			else
			{
				while (!base.Eof && base.Token != 150)
				{
					ISyntaxNode node2 = null;
					if (base.Token == 152)
					{
						node.AddChild(new SyntaxNode(base.TokenPosition, string.Empty, 28));
						MoveNext();
						continue;
					}
					if (!ParseArgument(ref node2))
					{
						result = false;
					}
					if (node2 != null)
					{
						node.AddChild(node2);
					}
					if (base.Token != 152)
					{
						break;
					}
					MoveNext();
				}
			}
			if (!Expected(VbLexerToken.Close_parens))
			{
				result = false;
			}
			node.Range.EndPoint = prevPosition;
		}
		else
		{
			SyntaxError(149);
			result = false;
		}
		return result;
	}

	protected virtual bool IsModifier(int token)
	{
		switch ((VbLexerToken)token)
		{
		case VbLexerToken.Assembly:
		case VbLexerToken.Default:
		case VbLexerToken.Friend:
		case VbLexerToken.MustInherit:
		case VbLexerToken.MustOverride:
		case VbLexerToken.NotInheritable:
		case VbLexerToken.NotOverridable:
		case VbLexerToken.Overloads:
		case VbLexerToken.Overridable:
		case VbLexerToken.Overrides:
		case VbLexerToken.Partial:
		case VbLexerToken.Private:
		case VbLexerToken.Protected:
		case VbLexerToken.Public:
		case VbLexerToken.ReadOnly:
		case VbLexerToken.Shadows:
		case VbLexerToken.Shared:
		case VbLexerToken.WithEvents:
		case VbLexerToken.WriteOnly:
			return true;
		default:
			return false;
		}
	}

	protected virtual bool ParseModifiers(ref ISyntaxAttributes attrs)
	{
		bool result = false;
		while (IsModifier(base.Token))
		{
			result = true;
			if (attrs == null)
			{
				attrs = new SyntaxAttributes();
			}
			attrs.Add(new SyntaxAttribute(base.TokenPosition, NetNodeType.Modifier.ToString(), base.TokenString));
			MoveNext();
		}
		return result;
	}

	protected virtual bool ParseDirective()
	{
		bool result = true;
		string tokenString = base.TokenString;
		int num = lineIndex;
		string text = tokenString.ToLower(CultureInfo.InvariantCulture);
		if (!(text == "#region"))
		{
			if (text == "#end")
			{
				Point endPoint = new Point(source.Length, lineIndex);
				MoveNext();
				if (base.Token == 113)
				{
					MoveNext();
				}
				ISyntaxNode current = base.SyntaxTree.Current;
				if (current.NodeType == 39)
				{
					base.SyntaxTree.Pop();
					current.Range.EndPoint = endPoint;
					current.AddAttribute(new SyntaxAttribute(current.Position, SyntaxConsts.OutlineText, current.Name));
					current.Options = SyntaxNodeOptions.Outlining;
				}
			}
			else
			{
				while (!base.Eof && num == lineIndex)
				{
					MoveNext();
				}
			}
		}
		else
		{
			Point tokenPosition = base.TokenPosition;
			ISyntaxNode syntaxNode = new SyntaxNode(tokenPosition, base.TokenString, 39);
			MoveNext();
			if (num == lineIndex && base.Token == 197)
			{
				syntaxNode.Name = base.TokenString.TrimEnd(new char[1] { '"' }).TrimStart(new char[1] { '"' }).Trim();
			}
			AddNode(syntaxNode);
			base.SyntaxTree.Push(syntaxNode);
			while (!base.Eof && num == lineIndex)
			{
				MoveNext();
			}
		}
		return result;
	}

	protected override bool ParseUnitBody()
	{
		return ParseClassBody();
	}

	protected virtual ISyntaxNode CreateParentNode(Point position, string name, int nodeType, ISyntaxNode refNode)
	{
		ISyntaxNode syntaxNode = new SyntaxNode(position, name, nodeType);
		if (refNode != null)
		{
			syntaxNode.Range.StartPoint = refNode.Position;
			syntaxNode.AddChild(refNode);
		}
		return syntaxNode;
	}

	protected virtual bool ParseExpression(ref ISyntaxNode node)
	{
		bool result = ParseInclusiveOrExpression(ref node);
		if (!TryParseAssignmentExpression(ref node))
		{
			result = false;
		}
		return result;
	}

	protected virtual bool ParseInclusiveOrExpression(ref ISyntaxNode node)
	{
		bool result = ParseExclusiveOrExpression(ref node);
		VbLexerToken vbLexerToken = (VbLexerToken)base.Token;
		if ((uint)(vbLexerToken - 98) <= 1u)
		{
			node = CreateParentNode(base.TokenPosition, base.TokenString, 132, node);
			MoveNext();
			ISyntaxNode node2 = new SyntaxNode();
			if (!ParseInclusiveOrExpression(ref node2))
			{
				result = false;
			}
			if (node2 != null)
			{
				node.AddChild(node2);
			}
			node.Range.EndPoint = prevPosition;
		}
		return result;
	}

	protected virtual bool ParseExclusiveOrExpression(ref ISyntaxNode node)
	{
		bool result = ParseAndExpression(ref node);
		if (base.Token == 146)
		{
			node = CreateParentNode(base.TokenPosition, base.TokenString, 133, node);
			MoveNext();
			ISyntaxNode node2 = null;
			if (!ParseExclusiveOrExpression(ref node2))
			{
				result = false;
			}
			if (node2 != null)
			{
				node.AddChild(node2);
			}
			node.Range.EndPoint = prevPosition;
		}
		return result;
	}

	protected virtual bool ParseAndExpression(ref ISyntaxNode node)
	{
		bool result = ParseEqualityExpression(ref node);
		VbLexerToken vbLexerToken = (VbLexerToken)base.Token;
		if ((uint)(vbLexerToken - 3) <= 1u)
		{
			node = CreateParentNode(base.TokenPosition, base.TokenString, 134, node);
			MoveNext();
			ISyntaxNode node2 = null;
			if (!ParseAndExpression(ref node2))
			{
				result = false;
			}
			if (node2 != null)
			{
				node.AddChild(node2);
			}
			node.Range.EndPoint = prevPosition;
		}
		return result;
	}

	protected virtual bool ParseEqualityExpression(ref ISyntaxNode node)
	{
		bool result = ParseRelationalExpression(ref node);
		switch ((VbLexerToken)base.Token)
		{
		case VbLexerToken.Assign:
		case VbLexerToken.Op_ne:
		{
			node = CreateParentNode(base.TokenPosition, base.TokenString, 172, node);
			MoveNext();
			ISyntaxNode node3 = null;
			if (!ParseEqualityExpression(ref node3))
			{
				result = false;
			}
			if (node3 != null)
			{
				node.AddChild(node3);
			}
			node.Range.EndPoint = prevPosition;
			break;
		}
		case VbLexerToken.Is:
		case VbLexerToken.Like:
		{
			node = CreateParentNode(base.TokenPosition, base.TokenString, 138, node);
			MoveNext();
			ISyntaxNode node2 = null;
			if (!ParseEqualityExpression(ref node2))
			{
				result = false;
			}
			if (node2 != null)
			{
				node.AddChild(node2);
			}
			node.Range.EndPoint = prevPosition;
			break;
		}
		}
		return result;
	}

	protected virtual bool ParseRelationalExpression(ref ISyntaxNode node)
	{
		bool result = ParseShiftExpression(ref node);
		VbLexerToken vbLexerToken = (VbLexerToken)base.Token;
		if ((uint)(vbLexerToken - 161) <= 1u || (uint)(vbLexerToken - 175) <= 1u)
		{
			node = CreateParentNode(base.TokenPosition, base.TokenString, 137, node);
			MoveNext();
			ISyntaxNode node2 = null;
			if (!ParseRelationalExpression(ref node2))
			{
				result = false;
			}
			if (node2 != null)
			{
				node.AddChild(node2);
			}
			node.Range.EndPoint = prevPosition;
		}
		return result;
	}

	protected virtual bool ParseShiftExpression(ref ISyntaxNode node)
	{
		bool result = ParseAdditiveExpression(ref node);
		VbLexerToken vbLexerToken = (VbLexerToken)base.Token;
		if ((uint)(vbLexerToken - 173) <= 1u)
		{
			node = CreateParentNode(base.TokenPosition, base.TokenString, 139, node);
			MoveNext();
			ISyntaxNode node2 = null;
			if (!ParseShiftExpression(ref node2))
			{
				result = false;
			}
			if (node2 != null)
			{
				node.AddChild(node2);
			}
			node.Range.EndPoint = prevPosition;
		}
		return result;
	}

	protected virtual bool ParseAdditiveExpression(ref ISyntaxNode node)
	{
		bool result = ParseMultiplicativeExpression(ref node);
		VbLexerToken vbLexerToken = (VbLexerToken)base.Token;
		if ((uint)(vbLexerToken - 157) <= 1u || vbLexerToken == VbLexerToken.Op_long)
		{
			node = CreateParentNode(base.TokenPosition, base.TokenString, 140, node);
			MoveNext();
			ISyntaxNode node2 = null;
			if (!ParseAdditiveExpression(ref node2))
			{
				result = false;
			}
			if (node2 != null)
			{
				node.AddChild(node2);
			}
			node.Range.EndPoint = prevPosition;
		}
		return result;
	}

	protected virtual bool ParseMultiplicativeExpression(ref ISyntaxNode node)
	{
		bool result = ParsePrefixedUnaryExpression(ref node);
		VbLexerToken vbLexerToken = (VbLexerToken)base.Token;
		if (vbLexerToken == VbLexerToken.Mod || (uint)(vbLexerToken - 169) <= 3u)
		{
			node = CreateParentNode(base.TokenPosition, base.TokenString, 141, node);
			MoveNext();
			ISyntaxNode node2 = null;
			if (!ParseMultiplicativeExpression(ref node2))
			{
				result = false;
			}
			if (node2 != null)
			{
				node.AddChild(node2);
			}
			node.Range.EndPoint = prevPosition;
		}
		return result;
	}

	protected virtual bool ParsePrefixedUnaryExpression(ref ISyntaxNode node)
	{
		bool result = true;
		VbLexerToken vbLexerToken = (VbLexerToken)base.Token;
		if (vbLexerToken == VbLexerToken.Not || (uint)(vbLexerToken - 157) <= 1u)
		{
			node = CreateParentNode(base.TokenPosition, base.TokenString, 142, node);
			MoveNext();
			ISyntaxNode node2 = null;
			if (!ParsePrefixedUnaryExpression(ref node2))
			{
				result = false;
			}
			if (node2 != null)
			{
				node.AddChild(node2);
			}
		}
		else
		{
			result = ParsePrimaryExpression(ref node);
		}
		return result;
	}

	protected virtual bool ParseSimpleExpression(ref ISyntaxNode node)
	{
		bool result = true;
		switch ((VbLexerToken)base.Token)
		{
		case VbLexerToken.False:
		case VbLexerToken.Nothing:
		case VbLexerToken.True:
		case VbLexerToken.Identifier_Literal:
		case VbLexerToken.Integer_Literal:
		case VbLexerToken.Short_Literal:
		case VbLexerToken.Long_Literal:
		case VbLexerToken.Float_Literal:
		case VbLexerToken.Double_Literal:
		case VbLexerToken.Decimal_Literal:
		case VbLexerToken.Date_Literal:
		case VbLexerToken.Character_Literal:
		case VbLexerToken.String_Literal:
			node = new SyntaxNode(base.TokenPosition, base.TokenString, 148);
			MoveNext();
			break;
		case VbLexerToken.Boolean:
		case VbLexerToken.Byte:
		case VbLexerToken.Char:
		case VbLexerToken.Date:
		case VbLexerToken.Decimal:
		case VbLexerToken.Double:
		case VbLexerToken.Integer:
		case VbLexerToken.Long:
		case VbLexerToken.Object:
		case VbLexerToken.Short:
		case VbLexerToken.Single:
		case VbLexerToken.String:
			node = new SyntaxNode(base.TokenPosition, base.TokenString, 148);
			MoveNext();
			if (base.Token != 151)
			{
				result = false;
			}
			break;
		case VbLexerToken.Open_parens:
			result = ParseParenthesizedExpression(ref node);
			break;
		case VbLexerToken.Me:
			result = ParseMeAccess(ref node);
			break;
		case VbLexerToken.MyBase:
			result = ParseBaseAccess(ref node);
			break;
		case VbLexerToken.MyClass:
			result = ParseMeAccess(ref node);
			break;
		case VbLexerToken.AddressOf:
			result = ParseAddressofExpression(ref node);
			break;
		case VbLexerToken.GetType:
			result = ParseGetTypeExpression(ref node);
			break;
		case VbLexerToken.TypeOf:
			result = ParseTypeofIsExpression(ref node);
			break;
		case VbLexerToken.New:
			result = ParseNewExpression(ref node);
			break;
		case VbLexerToken.CType:
		case VbLexerToken.DirectCast:
			result = ParseCastExpression(ref node);
			break;
		case VbLexerToken.CBool:
			result = ParseCastTargetExpression(ref node, VbLexerToken.Boolean);
			break;
		case VbLexerToken.CByte:
			result = ParseCastTargetExpression(ref node, VbLexerToken.Byte);
			break;
		case VbLexerToken.CChar:
			result = ParseCastTargetExpression(ref node, VbLexerToken.Char);
			break;
		case VbLexerToken.CDate:
			result = ParseCastTargetExpression(ref node, VbLexerToken.Date);
			break;
		case VbLexerToken.CDec:
			result = ParseCastTargetExpression(ref node, VbLexerToken.Decimal);
			break;
		case VbLexerToken.CDbl:
			result = ParseCastTargetExpression(ref node, VbLexerToken.Double);
			break;
		case VbLexerToken.CInt:
			result = ParseCastTargetExpression(ref node, VbLexerToken.Integer);
			break;
		case VbLexerToken.CLng:
			result = ParseCastTargetExpression(ref node, VbLexerToken.Long);
			break;
		case VbLexerToken.CObj:
			result = ParseCastTargetExpression(ref node, VbLexerToken.Object);
			break;
		case VbLexerToken.CShort:
			result = ParseCastTargetExpression(ref node, VbLexerToken.Short);
			break;
		case VbLexerToken.CSng:
			result = ParseCastTargetExpression(ref node, VbLexerToken.Single);
			break;
		case VbLexerToken.CStr:
			result = ParseCastTargetExpression(ref node, VbLexerToken.String);
			break;
		case VbLexerToken.Dot:
		{
			ISyntaxNode syntaxNode = FindWithStatement(base.SyntaxTree.Current);
			if (syntaxNode != null)
			{
				ISyntaxNode syntaxNode2 = new SyntaxNode(base.TokenPosition, base.TokenString, 124);
				AddNode(syntaxNode2);
				syntaxNode2.AddAttribute(new SyntaxAttribute(base.TokenPosition, NetNodeType.WithStatement.ToString(), syntaxNode));
				MoveNext();
				syntaxNode2.Range.EndPoint = prevPosition;
				result = ParseSimpleExpression(ref node);
			}
			else
			{
				result = false;
				SyntaxError();
				MoveNext();
			}
			break;
		}
		default:
			result = false;
			SyntaxError();
			if (!IsInvalidExpressionToken(base.Token))
			{
				MoveNext();
			}
			break;
		}
		return result;
	}

	protected virtual bool ParsePrimaryExpression(ref ISyntaxNode node)
	{
		bool flag = true;
		int num = lineIndex;
		flag = ParseSimpleExpression(ref node);
		if (node != null && node != null && num == lineIndex && !TryParsePostPrimaryExpression(ref node))
		{
			flag = false;
		}
		return flag;
	}

	protected virtual bool TryParsePostPrimaryExpression(ref ISyntaxNode node)
	{
		bool result = true;
		while (!base.Eof)
		{
			switch ((VbLexerToken)base.Token)
			{
			case VbLexerToken.Dot:
				if (!ParseMemberAccess(ref node))
				{
					result = false;
				}
				break;
			case VbLexerToken.Op_single:
				if (!ParseDictionaryAccess(ref node))
				{
					result = false;
				}
				break;
			case VbLexerToken.Open_parens:
				if (!ParseInvocationExpression(ref node))
				{
					result = false;
				}
				break;
			default:
				return result;
			}
		}
		return result;
	}

	protected virtual bool TryParseAssignmentExpression(ref ISyntaxNode node)
	{
		bool result = true;
		VbLexerToken vbLexerToken = (VbLexerToken)base.Token;
		if ((uint)(vbLexerToken - 178) <= 8u)
		{
			node = CreateParentNode(base.TokenPosition, base.TokenString, 129, node);
			MoveNext();
			ISyntaxNode node2 = null;
			if (!ParseExpression(ref node2))
			{
				result = false;
			}
			if (node2 != null)
			{
				node.AddChild(node2);
			}
			node.Range.EndPoint = prevPosition;
		}
		else
		{
			result = true;
		}
		return result;
	}

	protected virtual bool ParseTypeofIsExpression(ref ISyntaxNode node)
	{
		bool flag = true;
		node = new SyntaxNode(base.TokenPosition, base.TokenString, 160);
		MoveNext();
		ISyntaxNode node2 = null;
		flag = ParseExpression(ref node2);
		if (node2 != null)
		{
			node.AddChild(node2);
		}
		node.Range.EndPoint = prevPosition;
		return flag;
	}

	protected virtual bool ParseGetTypeExpression(ref ISyntaxNode node)
	{
		bool result = true;
		node = new SyntaxNode(base.TokenPosition, base.TokenString, 170);
		MoveNext();
		if (Expected(VbLexerToken.Open_parens))
		{
			Point tokenPosition = base.TokenPosition;
			if (ParseType(out var type))
			{
				node.AddAttribute(new SyntaxAttribute(tokenPosition, NetNodeType.Type.ToString(), type));
			}
			else
			{
				result = false;
			}
			if (!Expected(VbLexerToken.Close_parens))
			{
				result = false;
			}
		}
		else
		{
			result = false;
		}
		node.Range.EndPoint = prevPosition;
		return result;
	}

	protected virtual bool ParseParenthesizedExpression(ref ISyntaxNode node)
	{
		bool flag = true;
		node = new SyntaxNode(base.TokenPosition, base.TokenString, 162);
		if (base.Token == 149)
		{
			MoveNext();
			ISyntaxNode node2 = null;
			flag = ParseExpression(ref node2);
			if (node2 != null)
			{
				node.AddChild(node2);
			}
			if (!Expected(VbLexerToken.Close_parens))
			{
				flag = false;
			}
		}
		else
		{
			SyntaxError(149);
			flag = false;
		}
		node.Range.EndPoint = prevPosition;
		return flag;
	}

	protected virtual bool ParseInvocationExpression(ref ISyntaxNode node)
	{
		bool flag = true;
		node = CreateParentNode(base.TokenPosition, base.TokenString, 157, node);
		ISyntaxNode node2 = null;
		flag = ParseArgumentList(ref node2, generic: false);
		if (node2 != null)
		{
			node.AddChild(node2);
		}
		node.Range.EndPoint = prevPosition;
		return flag;
	}

	protected virtual bool ParseNewExpression(ref ISyntaxNode node)
	{
		node = new SyntaxNode(base.TokenPosition, base.TokenString, 150);
		MoveNext();
		Point tokenPosition = base.TokenPosition;
		string type;
		bool result = ParseTypeName(out type);
		if (type != string.Empty)
		{
			node.AddAttribute(new SyntaxAttribute(tokenPosition, NetNodeType.Type.ToString(), type));
			if (base.Token == 149)
			{
				ISyntaxNode node2 = null;
				result = ParseArgumentList(ref node2, generic: true);
				if (node2 != null)
				{
					node.AddChild(node2);
				}
			}
			if (base.Token == 147)
			{
				node.NodeType = 149;
				ISyntaxNode node3 = null;
				if (!ParseArrayInitializerExpression(ref node3))
				{
					result = false;
				}
				if (node3 != null)
				{
					node.AddChild(node3);
				}
			}
		}
		else
		{
			result = false;
		}
		node.Range.EndPoint = prevPosition;
		return result;
	}

	protected virtual bool ParseAddressofExpression(ref ISyntaxNode node)
	{
		bool flag = true;
		node = new SyntaxNode(base.TokenPosition, base.TokenString, 169);
		MoveNext();
		ISyntaxNode node2 = null;
		flag = ParseExpression(ref node2);
		if (node2 != null)
		{
			node.AddChild(node2);
		}
		node.Range.EndPoint = prevPosition;
		return flag;
	}

	protected virtual bool ParseMeAccess(ref ISyntaxNode node, int nodeType)
	{
		node = new SyntaxNode(base.TokenPosition, base.TokenString, nodeType);
		MoveNext();
		return true;
	}

	protected virtual bool ParseMeAccess(ref ISyntaxNode node)
	{
		return ParseMeAccess(ref node, 153);
	}

	protected virtual bool ParseBaseAccess(ref ISyntaxNode node)
	{
		return ParseMeAccess(ref node, 152);
	}

	protected virtual bool ParseCastExpression(ref ISyntaxNode node)
	{
		bool result = true;
		node = new SyntaxNode(base.TokenPosition, base.TokenString, 145);
		MoveNext();
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString, 29);
		node.AddChild(syntaxNode);
		if (Expected(VbLexerToken.Open_parens))
		{
			ISyntaxNode syntaxNode2 = new SyntaxNode(base.TokenPosition, string.Empty, 28);
			syntaxNode.AddChild(syntaxNode2);
			ISyntaxNode node2 = null;
			if (!ParseExpression(ref node2))
			{
				result = false;
			}
			if (node2 != null)
			{
				syntaxNode2.AddChild(node2);
			}
			syntaxNode2.Range.EndPoint = prevPosition;
			if (!Expected(VbLexerToken.Comma))
			{
				result = false;
			}
			syntaxNode2 = new SyntaxNode(base.TokenPosition, string.Empty, 28);
			syntaxNode.AddChild(syntaxNode2);
			Point tokenPosition;
			if (ParseType(out var type))
			{
				syntaxNode2.Name = type;
				node.AddAttribute(new SyntaxAttribute(syntaxNode2.Position, NetNodeType.Type.ToString(), type));
				tokenPosition = prevPosition;
			}
			else
			{
				result = false;
				tokenPosition = base.TokenPosition;
			}
			syntaxNode2.Range.EndPoint = tokenPosition;
			if (Expected(VbLexerToken.Close_parens))
			{
				tokenPosition = prevPosition;
			}
			else
			{
				result = false;
			}
			syntaxNode.Range.EndPoint = tokenPosition;
		}
		else
		{
			result = false;
			syntaxNode.Range.EndPoint = prevPosition;
		}
		node.Range.EndPoint = syntaxNode.Range.EndPoint;
		return result;
	}

	protected virtual bool ParseCastTargetExpression(ref ISyntaxNode node, VbLexerToken token)
	{
		bool result = true;
		node = new SyntaxNode(base.TokenPosition, base.TokenString, 146);
		node.AddAttribute(new SyntaxAttribute(base.TokenPosition, NetNodeType.Type.ToString(), token.ToString()));
		MoveNext();
		if (Expected(VbLexerToken.Open_parens))
		{
			ISyntaxNode node2 = null;
			if (!ParseExpression(ref node2))
			{
				result = false;
			}
			if (node2 != null)
			{
				node.AddChild(node2);
			}
			if (!Expected(VbLexerToken.Close_parens))
			{
				result = false;
			}
		}
		else
		{
			result = false;
		}
		node.Range.EndPoint = prevPosition;
		return result;
	}

	protected virtual bool ParseMemberAccess(ref ISyntaxNode node, int nodeType)
	{
		bool result = true;
		node = CreateParentNode(base.TokenPosition, string.Empty, nodeType, node);
		Point currentPosition = base.CurrentPosition;
		MoveNext();
		string identifier;
		if (IsKeywordToken(base.Token) && currentPosition.Y == base.TokenPosition.Y)
		{
			node.Name = base.TokenString;
			MoveNext();
		}
		else if (ParseIdentifier(out identifier))
		{
			node.Name = identifier;
		}
		else
		{
			result = false;
		}
		node.AddAttribute(new SyntaxAttribute(currentPosition, NetNodeType.Name.ToString(), node.Name));
		node.Range.EndPoint = prevPosition;
		return result;
	}

	protected virtual bool ParseMemberAccess(ref ISyntaxNode node)
	{
		return ParseMemberAccess(ref node, 154);
	}

	protected virtual bool ParseDictionaryAccess(ref ISyntaxNode node)
	{
		return ParseMemberAccess(ref node, 156);
	}

	protected virtual bool ParseExpressionList(ref ISyntaxNode node)
	{
		node = new SyntaxNode(base.TokenPosition, string.Empty, 126);
		ISyntaxNode node2 = null;
		bool result = ParseExpression(ref node2);
		if (node2 != null)
		{
			node.AddChild(node2);
		}
		while (!base.Eof && base.Token == 152)
		{
			MoveNext();
			node2 = null;
			if (!ParseExpression(ref node2))
			{
				result = false;
			}
			if (node2 != null)
			{
				node.AddChild(node2);
			}
		}
		node.Range.EndPoint = prevPosition;
		return result;
	}

	protected virtual bool ParseConstantExpression(ref ISyntaxNode node)
	{
		return ParseExpression(ref node);
	}

	protected virtual bool ParseAssignmentExpression(ref ISyntaxNode node)
	{
		bool flag = Expected(VbLexerToken.Assign);
		if (flag)
		{
			node = CreateParentNode(base.TokenPosition, "=", 129, node);
			ISyntaxNode node2 = null;
			if (!ParseExpression(ref node2))
			{
				flag = false;
			}
			if (node2 != null)
			{
				node.AddChild(node2);
			}
			node.Range.EndPoint = prevPosition;
		}
		return flag;
	}

	protected virtual bool IsComparisionOperator(int token)
	{
		if ((uint)(token - 160) <= 2u || (uint)(token - 175) <= 2u)
		{
			return true;
		}
		return false;
	}

	protected virtual bool ParseComparisionExpression(ref ISyntaxNode node)
	{
		bool flag = IsComparisionOperator(base.Token);
		if (flag)
		{
			node = CreateParentNode(base.TokenPosition, base.TokenString, 129, node);
			MoveNext();
			ISyntaxNode node2 = null;
			if (!ParseExpression(ref node2))
			{
				flag = false;
			}
			if (node2 != null)
			{
				node.AddChild(node2);
			}
			node.Range.EndPoint = prevPosition;
		}
		return flag;
	}

	protected virtual bool ParseStatement()
	{
		switch ((VbLexerToken)base.Token)
		{
		case VbLexerToken.Const:
		case VbLexerToken.Dim:
		case VbLexerToken.Static:
			return ParseLocalDeclarationStatement();
		case VbLexerToken.With:
			return ParseWithStatement();
		case VbLexerToken.SyncLock:
			return ParseSyncLockStatement();
		case VbLexerToken.RaiseEvent:
			return ParseRaiseEventStatement();
		case VbLexerToken.AddHandler:
			return ParseAddHandlerStatement();
		case VbLexerToken.RemoveHandler:
			return ParseRemoveHandlerStatement();
		case VbLexerToken.Mid:
			return ParseMidAssignmentStatement();
		case VbLexerToken.Call:
			return ParseCallStatement();
		case VbLexerToken.If:
			return ParseIfStatement();
		case VbLexerToken.Select:
			return ParseSelectStatement();
		case VbLexerToken.While:
			return ParseWhileStatement();
		case VbLexerToken.Do:
			return ParseDoStatement();
		case VbLexerToken.For:
			return ParseForStatement();
		case VbLexerToken.Throw:
			return ParseThrowStatement();
		case VbLexerToken.Try:
			return ParseTryStatement();
		case VbLexerToken.Error:
			return ParseErrorStatement();
		case VbLexerToken.On:
			return ParseOnErrorStatement();
		case VbLexerToken.Resume:
			return ParseResumeStatement();
		case VbLexerToken.GoTo:
			return ParseGotoStatement();
		case VbLexerToken.Exit:
			return ParseExitStatement();
		case VbLexerToken.Stop:
			return ParseStopStatement();
		case VbLexerToken.End:
			return ParseEndStatement();
		case VbLexerToken.Return:
			return ParseReturnStatement();
		case VbLexerToken.ReDim:
			return ParseRedimStatement();
		case VbLexerToken.Erase:
			return ParseEraseStatement();
		case VbLexerToken.Continue:
			return ParseContinueStatement();
		case VbLexerToken.Directive_Literal:
			return ParseDirective();
		default:
		{
			TryParseLabeledStatement(out var parsed);
			if (!parsed)
			{
				return ParseExpressionStatement();
			}
			return true;
		}
		}
	}

	protected virtual bool ParseLocalDeclarationStatement()
	{
		bool result = ParseDimDeclaration(null, 15);
		if (!ParseStatementTerminator())
		{
			result = false;
		}
		return result;
	}

	protected virtual bool ParseWithStatement()
	{
		return ParseExpressionBlockStatement(new int[1] { 45 }, 143, 123, scope: false);
	}

	protected virtual ISyntaxNode FindWithStatement(ISyntaxNode node)
	{
		while (node != null)
		{
			if (node.NodeType == 123)
			{
				return node;
			}
			node = node.Parent;
		}
		return null;
	}

	protected virtual bool ParseSyncLockStatement()
	{
		return ParseExpressionBlockStatement(130, 122);
	}

	protected virtual bool ParseRaiseEventStatement()
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString, 116);
		AddNode(syntaxNode);
		MoveNext();
		if (ParseIdentifier(out var identifier))
		{
			syntaxNode.Name = identifier;
		}
		else
		{
			result = false;
		}
		if (base.Token == 149)
		{
			ISyntaxNode node = null;
			if (!ParseArgumentList(ref node, generic: false))
			{
				result = false;
			}
			if (node != null)
			{
				syntaxNode.AddChild(node);
			}
		}
		if (!ParseStatementTerminator())
		{
			result = false;
		}
		syntaxNode.Range.EndPoint = prevPosition;
		return result;
	}

	protected virtual bool ParseAddHandlerStatement()
	{
		return ParseEventHandlerStatement(105);
	}

	protected virtual bool ParseRemoveHandlerStatement()
	{
		return ParseEventHandlerStatement(118);
	}

	protected virtual bool ParseEventHandlerStatement(int nodeType)
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString, nodeType);
		AddNode(syntaxNode);
		MoveNext();
		ISyntaxNode node = null;
		ISyntaxNode node2 = null;
		if (!ParseExpression(ref node) || !Expected(VbLexerToken.Comma) || !ParseExpression(ref node2))
		{
			result = false;
		}
		if (node != null)
		{
			syntaxNode.AddChild(node);
		}
		if (node2 != null)
		{
			syntaxNode.AddChild(node2);
		}
		if (!ParseStatementTerminator())
		{
			result = false;
		}
		syntaxNode.Range.EndPoint = prevPosition;
		return result;
	}

	protected virtual bool ParseMidAssignmentStatement()
	{
		bool flag = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString, 114);
		AddNode(syntaxNode);
		MoveNext();
		if (base.Token == 168)
		{
			MoveNext();
		}
		ISyntaxNode node = null;
		if (Expected(VbLexerToken.Open_parens))
		{
			flag = ParseExpressionList(ref node);
			if (node != null)
			{
				syntaxNode.AddChild(node);
			}
			if (!Expected(VbLexerToken.Close_parens))
			{
				flag = false;
			}
		}
		else
		{
			flag = false;
		}
		node = null;
		if (!ParseAssignmentExpression(ref node))
		{
			flag = false;
		}
		if (node != null)
		{
			syntaxNode.AddChild(node);
		}
		if (!ParseStatementTerminator())
		{
			flag = false;
		}
		syntaxNode.Range.EndPoint = prevPosition;
		return flag;
	}

	protected virtual bool ParseCallStatement()
	{
		bool flag = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString, 106);
		AddNode(syntaxNode);
		MoveNext();
		ISyntaxNode node = null;
		flag = ParseExpression(ref node);
		if (node != null)
		{
			syntaxNode.AddChild(node);
		}
		if (!ParseStatementTerminator())
		{
			flag = false;
		}
		syntaxNode.Range.EndPoint = prevPosition;
		return flag;
	}

	protected virtual bool ParseIfStatement()
	{
		bool flag = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString, 63, SyntaxNodeOptions.Indentation);
		AddNode(syntaxNode);
		syntaxNode.AddAttribute(new SyntaxAttribute(base.TokenPosition, SyntaxConsts.SuppressReparsingScope, null));
		bool flag2 = false;
		int line = lineIndex;
		base.SyntaxTree.Push(syntaxNode);
		try
		{
			MoveNext();
			ISyntaxNode node = null;
			if (!ParseExpression(ref node))
			{
				flag = false;
				flag2 = true;
			}
			if (node != null)
			{
				syntaxNode.AddChild(node);
			}
			bool flag3 = flag;
			if (base.Token == 131)
			{
				line = lineIndex;
				MoveNext();
				flag2 = ParseStatementTerminator(line);
			}
			else
			{
				flag2 = ParseStatementTerminator(line) || flag2;
				flag3 = false;
			}
			if (flag2)
			{
				if (!ParseBlock(new int[4] { 43, 44, 45, 46 }))
				{
					flag = false;
				}
			}
			else if (!ParseStatement())
			{
				flag = false;
			}
			if (flag2)
			{
				while (!base.Eof && (base.Token == 44 || base.Token == 43))
				{
					bool flag4 = base.Token == 44;
					if (!flag4)
					{
						SaveState();
						try
						{
							line = lineIndex;
							flag4 = MoveNext() == 62 && lineIndex == line;
						}
						finally
						{
							RestoreState(!flag4);
						}
					}
					if (flag4)
					{
						if (!ParseElseIfStatement())
						{
							flag = false;
						}
						continue;
					}
					if (!ParseElseStatement(flag2))
					{
						flag = false;
					}
					break;
				}
			}
			if (flag2)
			{
				if (base.Token == 46)
				{
					MoveNext();
				}
				else if (!Expected(VbLexerToken.End) || !Expected(VbLexerToken.If))
				{
					if (flag3)
					{
						syntaxNode.AddAttribute(new SyntaxAttribute(prevPosition, SyntaxConsts.DefinitionScopeEndExpected, "\r\n" + VbLexerToken.End.ToString() + " " + VbLexerToken.If));
					}
					flag = false;
				}
				ISyntaxAttribute syntaxAttribute = syntaxNode.FindAttribute(SyntaxConsts.DefinitionScopeEnd);
				if (syntaxAttribute != null)
				{
					syntaxAttribute.Position = prevPosition;
				}
			}
			if (!ParseStatementTerminator())
			{
				flag = false;
			}
		}
		finally
		{
			base.SyntaxTree.Pop();
		}
		syntaxNode.Range.EndPoint = prevPosition;
		return flag;
	}

	protected virtual bool ParseElseStatement(bool block)
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString, 64, SyntaxNodeOptions.BackIndentation);
		AddNode(syntaxNode);
		base.SyntaxTree.Push(syntaxNode);
		try
		{
			MoveNext();
			if (block)
			{
				if (!ParseStatementTerminator())
				{
					result = false;
				}
				if (!ParseBlock(new int[2] { 45, 46 }))
				{
					result = false;
				}
			}
			else if (!ParseStatement())
			{
				result = false;
			}
		}
		finally
		{
			base.SyntaxTree.Pop();
		}
		syntaxNode.Range.EndPoint = prevPosition;
		return result;
	}

	protected virtual bool ParseElseIfStatement()
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString, 109, SyntaxNodeOptions.BackIndentation);
		syntaxNode.AddAttribute(new SyntaxAttribute(base.TokenPosition, SyntaxConsts.SuppressReparsingScope, null));
		AddNode(syntaxNode);
		base.SyntaxTree.Push(syntaxNode);
		try
		{
			MoveNext();
			ISyntaxNode node = null;
			if (!ParseExpression(ref node))
			{
				result = false;
			}
			if (node != null)
			{
				syntaxNode.AddChild(node);
			}
			if (base.Token == 131)
			{
				MoveNext();
			}
			if (!ParseStatementTerminator())
			{
				result = false;
			}
			if (!ParseBlock(new int[4] { 43, 44, 45, 46 }))
			{
				result = false;
			}
		}
		finally
		{
			base.SyntaxTree.Pop();
		}
		syntaxNode.Range.EndPoint = prevPosition;
		return result;
	}

	protected virtual bool ParseSelectStatement()
	{
		bool flag = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString, 120, SyntaxNodeOptions.Indentation);
		AddNode(syntaxNode);
		base.SyntaxTree.Push(syntaxNode);
		try
		{
			MoveNext();
			if (base.Token == 14)
			{
				MoveNext();
			}
			ISyntaxNode node = null;
			if (!ParseExpression(ref node))
			{
				flag = false;
			}
			if (node != null)
			{
				syntaxNode.AddChild(node);
			}
			bool flag2 = flag;
			if (!ParseStatementTerminator())
			{
				flag = false;
			}
			syntaxNode.AddAttribute(new SyntaxAttribute(prevPosition, SyntaxConsts.DefinitionScope, null));
			while (!base.Eof && base.Token == 14)
			{
				if (!ParseCaseStatement())
				{
					flag = false;
				}
			}
			if (!Expected(VbLexerToken.End) || !Expected(VbLexerToken.Select))
			{
				if (flag2)
				{
					syntaxNode.AddAttribute(new SyntaxAttribute(prevPosition, SyntaxConsts.DefinitionScopeEndExpected, "\r\n" + VbLexerToken.End.ToString() + " " + VbLexerToken.Select));
				}
				flag = false;
			}
			syntaxNode.AddAttribute(new SyntaxAttribute(prevPosition, SyntaxConsts.DefinitionScopeEnd, null));
			if (!ParseStatementTerminator())
			{
				flag = false;
			}
		}
		finally
		{
			base.SyntaxTree.Pop();
		}
		syntaxNode.Range.EndPoint = prevPosition;
		return flag;
	}

	protected virtual bool ParseCaseStatement()
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString, 107, SyntaxNodeOptions.Indentation);
		AddNode(syntaxNode);
		base.SyntaxTree.Push(syntaxNode);
		try
		{
			MoveNext();
			if (base.Token == 43)
			{
				MoveNext();
				syntaxNode.NodeType = 108;
			}
			else if (!ParseCaseClauses())
			{
				result = false;
			}
			if (!ParseStatementTerminator())
			{
				result = false;
			}
			if (!ParseBlock(new int[2] { 14, 45 }))
			{
				result = false;
			}
		}
		finally
		{
			base.SyntaxTree.Pop();
		}
		syntaxNode.Range.EndPoint = prevPosition;
		return result;
	}

	protected virtual bool ParseCaseClauses()
	{
		bool result = true;
		do
		{
			if (!ParseCaseClause())
			{
				result = false;
			}
			if (base.Token != 152)
			{
				break;
			}
			MoveNext();
		}
		while (!base.Eof);
		return result;
	}

	protected virtual bool ParseCaseClause()
	{
		bool flag = true;
		if (base.Token == 69)
		{
			MoveNext();
		}
		if (IsComparisionOperator(base.Token))
		{
			ISyntaxNode node = null;
			flag = ParseComparisionExpression(ref node);
			if (node != null)
			{
				AddNode(node);
			}
		}
		else
		{
			ISyntaxNode node2 = null;
			flag = ParseExpression(ref node2);
			if (node2 != null)
			{
				AddNode(node2);
			}
			if (base.Token == 133)
			{
				MoveNext();
				node2 = null;
				if (!ParseExpression(ref node2))
				{
					flag = false;
				}
				if (node2 != null)
				{
					AddNode(node2);
				}
			}
		}
		return flag;
	}

	protected virtual bool ParseContinueStatement()
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString, 88);
		AddNode(syntaxNode);
		MoveNext();
		if (base.Token == 40 || base.Token == 54 || base.Token == 142)
		{
			MoveNext();
		}
		else
		{
			result = false;
		}
		syntaxNode.Range.EndPoint = prevPosition;
		return result;
	}

	protected virtual bool ParseWhileStatement()
	{
		return ParseExpressionBlockStatement(142, 68);
	}

	protected virtual bool ParseDoStatement()
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString, 69, SyntaxNodeOptions.Indentation);
		AddNode(syntaxNode);
		base.SyntaxTree.Push(syntaxNode);
		try
		{
			MoveNext();
			bool parsed = false;
			if (!TryParseWhileOrUntil(out parsed))
			{
				result = false;
			}
			if (!ParseStatementTerminator())
			{
				result = false;
			}
			bool blockEnd = false;
			if (!ParseBlock(new int[1] { 76 }, scope: true, out blockEnd))
			{
				if (parsed && !blockEnd)
				{
					syntaxNode.AddAttribute(new SyntaxAttribute(prevPosition, SyntaxConsts.DefinitionScopeEndExpected, "\r\n" + VbLexerToken.Loop));
				}
				result = false;
			}
			MoveNext();
			if (!TryParseWhileOrUntil(out parsed))
			{
				result = false;
			}
			if (!ParseStatementTerminator())
			{
				result = false;
			}
		}
		finally
		{
			base.SyntaxTree.Pop();
		}
		syntaxNode.Range.EndPoint = prevPosition;
		return result;
	}

	protected virtual bool TryParseWhileOrUntil(out bool parsed)
	{
		bool result = true;
		parsed = false;
		VbLexerToken vbLexerToken = (VbLexerToken)base.Token;
		if (vbLexerToken == VbLexerToken.Until || vbLexerToken == VbLexerToken.While)
		{
			int y = base.TokenPosition.Y;
			MoveNext();
			parsed = y == base.TokenPosition.Y;
			ISyntaxNode node = null;
			result = ParseExpression(ref node);
			if (node != null)
			{
				AddNode(node);
			}
		}
		return result;
	}

	protected virtual bool ParseForStatement()
	{
		bool flag = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString, 71, SyntaxNodeOptions.Indentation);
		AddNode(syntaxNode);
		base.SyntaxTree.Push(syntaxNode);
		try
		{
			MoveNext();
			if (base.Token == 42)
			{
				flag = ParseForEachStatement(syntaxNode);
			}
			else
			{
				if (!ParseLoopControlVariable())
				{
					flag = false;
				}
				ISyntaxNode node = null;
				if (base.Token == 160 && !ParseAssignmentExpression(ref node))
				{
					flag = false;
				}
				if (node != null)
				{
					syntaxNode.AddChild(node);
				}
				if (!ParseForIterator())
				{
					flag = false;
				}
				bool flag2 = flag;
				if (!ParseStatementTerminator())
				{
					flag2 = false;
					flag = false;
				}
				if (!ParseBlockStatement(87, 201))
				{
					flag = false;
					if (flag2)
					{
						syntaxNode.AddAttribute(new SyntaxAttribute(prevPosition, SyntaxConsts.DefinitionScopeEndExpected, "\r\n" + VbLexerToken.Next));
					}
				}
				int line = lineIndex;
				MoveNext();
				if (!ParseStatementTerminator(line))
				{
					node = null;
					if (!ParseExpressionList(ref node))
					{
						flag = false;
					}
					if (node != null)
					{
						syntaxNode.AddChild(node);
					}
				}
				if (!ParseStatementTerminator())
				{
					flag = false;
				}
			}
		}
		finally
		{
			base.SyntaxTree.Pop();
		}
		syntaxNode.Range.EndPoint = prevPosition;
		return flag;
	}

	protected virtual bool ParseForEachStatement(ISyntaxNode node)
	{
		bool flag = true;
		node.NodeType = 75;
		MoveNext();
		if (!ParseLoopControlVariable())
		{
			flag = false;
		}
		if (base.Token == 65)
		{
			MoveNext();
			ISyntaxNode node2 = null;
			if (!ParseExpression(ref node2))
			{
				flag = false;
			}
			if (node2 != null)
			{
				node.AddChild(node2);
			}
		}
		bool flag2 = flag;
		if (!ParseStatementTerminator())
		{
			flag2 = false;
			flag = false;
		}
		if (!ParseBlockStatement(87, 201))
		{
			flag = false;
			if (flag2)
			{
				node.AddAttribute(new SyntaxAttribute(prevPosition, SyntaxConsts.DefinitionScopeEndExpected, "\r\n" + VbLexerToken.Next));
			}
		}
		else
		{
			int line = lineIndex;
			MoveNext();
			if (!ParseStatementTerminator(line))
			{
				ISyntaxNode node3 = null;
				if (!ParseExpression(ref node3))
				{
					flag = false;
				}
				if (node3 != null)
				{
					node.AddChild(node3);
				}
			}
		}
		if (!ParseStatementTerminator())
		{
			flag = false;
		}
		node.Range.EndPoint = prevPosition;
		return flag;
	}

	protected virtual bool ParseLoopControlVariable()
	{
		bool result = true;
		bool asType = false;
		if (IsIdentifierToken(base.Token))
		{
			SaveState();
			try
			{
				result = ParseSimpleVariable(ref asType);
			}
			finally
			{
				RestoreState(!asType);
			}
		}
		if (!asType)
		{
			ISyntaxNode node = null;
			result = ParseExpression(ref node);
			if (node != null)
			{
				AddNode(node);
			}
		}
		return result;
	}

	protected virtual bool ParseSimpleVariable(ref bool asType)
	{
		bool result = true;
		Point tokenPosition = base.TokenPosition;
		if (ParseIdentifier(out var identifier))
		{
			asType = base.Token == 6;
			if (asType)
			{
				MoveNext();
				ISyntaxNode syntaxNode = new SyntaxNode(tokenPosition, identifier, 15, SyntaxNodeOptions.CodeCompletion);
				AddNode(syntaxNode);
				if (ParseType(out var type))
				{
					syntaxNode.AddAttribute(new SyntaxAttribute(tokenPosition, NetNodeType.Type.ToString(), type));
				}
				else
				{
					result = false;
				}
			}
		}
		return result;
	}

	protected virtual bool ParseForIterator()
	{
		bool flag = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString, 74);
		AddNode(syntaxNode);
		ISyntaxNode node = null;
		flag = Expected(VbLexerToken.To) && ParseExpression(ref node);
		if (node != null)
		{
			syntaxNode.AddChild(node);
		}
		if (base.Token == 125)
		{
			MoveNext();
			node = null;
			if (!ParseExpression(ref node))
			{
				flag = false;
			}
			if (node != null)
			{
				syntaxNode.AddChild(node);
			}
		}
		syntaxNode.Range.EndPoint = prevPosition;
		return flag;
	}

	protected virtual bool ParseThrowOrReturnStatement(int nodeType)
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString, nodeType, SyntaxNodeOptions.Indentation);
		AddNode(syntaxNode);
		int line = lineIndex;
		MoveNext();
		if (!ParseStatementTerminator(line))
		{
			ISyntaxNode node = null;
			if (!ParseExpression(ref node))
			{
				result = false;
			}
			if (node != null)
			{
				syntaxNode.AddChild(node);
			}
		}
		if (!ParseStatementTerminator())
		{
			result = false;
		}
		syntaxNode.Range.EndPoint = prevPosition;
		return result;
	}

	protected virtual bool ParseThrowStatement()
	{
		return ParseThrowOrReturnStatement(91);
	}

	protected virtual bool ParseReturnStatement()
	{
		return ParseThrowOrReturnStatement(90);
	}

	protected virtual bool ParseTryStatement()
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString, 92, SyntaxNodeOptions.Indentation);
		AddNode(syntaxNode);
		base.SyntaxTree.Push(syntaxNode);
		try
		{
			MoveNext();
			result = ParseBlock(new int[3] { 15, 53, 45 });
			while (!base.Eof && base.Token == 15)
			{
				if (!ParseCatchStatement())
				{
					result = false;
				}
			}
			if (base.Token == 53 && !ParseFinallyStatement())
			{
				result = false;
			}
			if (!Expected(VbLexerToken.End) || !Expected(VbLexerToken.Try))
			{
				result = false;
				syntaxNode.AddAttribute(new SyntaxAttribute(prevPosition, SyntaxConsts.DefinitionScopeEndExpected, "\r\n" + VbLexerToken.End.ToString() + " " + VbLexerToken.Try));
			}
			ISyntaxAttribute syntaxAttribute = syntaxNode.FindAttribute(SyntaxConsts.DefinitionScopeEnd);
			if (syntaxAttribute != null)
			{
				syntaxAttribute.Position = prevPosition;
			}
			if (!ParseStatementTerminator())
			{
				result = false;
			}
		}
		finally
		{
			base.SyntaxTree.Pop();
		}
		syntaxNode.Range.EndPoint = prevPosition;
		return result;
	}

	protected virtual bool ParseCatchStatement()
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString, 93, (base.SyntaxTree.Current.NodeType != 92) ? SyntaxNodeOptions.Indentation : SyntaxNodeOptions.BackIndentation);
		AddNode(syntaxNode);
		base.SyntaxTree.Push(syntaxNode);
		try
		{
			MoveNext();
			if (IsIdentifierToken(base.Token))
			{
				bool asType = false;
				if (!ParseSimpleVariable(ref asType))
				{
					result = false;
				}
			}
			if (base.Token == 141)
			{
				MoveNext();
				ISyntaxNode node = null;
				if (!ParseExpression(ref node))
				{
					result = false;
				}
				if (node != null)
				{
					syntaxNode.AddChild(node);
				}
			}
			if (!ParseStatementTerminator())
			{
				result = false;
			}
			if (!ParseBlock(new int[3] { 15, 53, 45 }))
			{
				result = false;
			}
		}
		finally
		{
			base.SyntaxTree.Pop();
		}
		syntaxNode.Range.EndPoint = prevPosition;
		return result;
	}

	protected virtual bool ParseFinallyStatement()
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString, 94, (base.SyntaxTree.Current.NodeType != 92) ? SyntaxNodeOptions.Indentation : SyntaxNodeOptions.BackIndentation);
		AddNode(syntaxNode);
		base.SyntaxTree.Push(syntaxNode);
		try
		{
			MoveNext();
			if (!ParseStatementTerminator())
			{
				result = false;
			}
			if (!ParseBlock(45))
			{
				result = false;
			}
		}
		finally
		{
			base.SyntaxTree.Pop();
		}
		syntaxNode.Range.EndPoint = prevPosition;
		return result;
	}

	protected virtual bool ParseErrorStatement()
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString, 112, SyntaxNodeOptions.Indentation);
		AddNode(syntaxNode);
		MoveNext();
		ISyntaxNode node = null;
		if (!ParseExpression(ref node))
		{
			result = false;
		}
		if (node != null)
		{
			syntaxNode.AddChild(node);
		}
		if (!ParseStatementTerminator())
		{
			result = false;
		}
		syntaxNode.Range.EndPoint = prevPosition;
		return result;
	}

	protected virtual bool ParseOnErrorStatement()
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString, 115, SyntaxNodeOptions.Indentation);
		AddNode(syntaxNode);
		base.SyntaxTree.Push(syntaxNode);
		try
		{
			MoveNext();
			if (Expected(VbLexerToken.Error))
			{
				switch ((VbLexerToken)base.Token)
				{
				case VbLexerToken.Resume:
					MoveNext();
					if (!Expected(VbLexerToken.Next))
					{
						result = false;
					}
					break;
				case VbLexerToken.GoTo:
					if (!ParseGotoStatement())
					{
						result = false;
					}
					break;
				default:
					result = false;
					SyntaxError(116);
					break;
				}
				if (!ParseStatementTerminator())
				{
					result = false;
				}
			}
			else
			{
				result = false;
			}
		}
		finally
		{
			base.SyntaxTree.Pop();
		}
		syntaxNode.Range.EndPoint = prevPosition;
		return result;
	}

	protected virtual bool ParseResumeStatement()
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString, 119, SyntaxNodeOptions.Indentation);
		AddNode(syntaxNode);
		int line = lineIndex;
		MoveNext();
		if (!ParseStatementTerminator(line))
		{
			VbLexerToken vbLexerToken = (VbLexerToken)base.Token;
			string name;
			if (vbLexerToken == VbLexerToken.Next)
			{
				MoveNext();
			}
			else if (!ParseLabelName(out name))
			{
				result = false;
			}
		}
		if (!ParseStatementTerminator())
		{
			result = false;
		}
		syntaxNode.Range.EndPoint = prevPosition;
		return result;
	}

	protected virtual bool ParseGotoStatement()
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString, 89, SyntaxNodeOptions.Indentation);
		AddNode(syntaxNode);
		MoveNext();
		if (ParseLabelName(out var name))
		{
			syntaxNode.Name = name;
		}
		else
		{
			result = false;
		}
		if (!ParseStatementTerminator())
		{
			result = false;
		}
		syntaxNode.Range.EndPoint = prevPosition;
		return result;
	}

	protected virtual bool ParseExitStatement()
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString, 113, SyntaxNodeOptions.Indentation);
		AddNode(syntaxNode);
		MoveNext();
		switch ((VbLexerToken)base.Token)
		{
		case VbLexerToken.Do:
		case VbLexerToken.For:
		case VbLexerToken.Function:
		case VbLexerToken.Property:
		case VbLexerToken.Select:
		case VbLexerToken.Sub:
		case VbLexerToken.Try:
		case VbLexerToken.While:
			MoveNext();
			break;
		default:
			result = false;
			SyntaxError();
			break;
		}
		if (!ParseStatementTerminator())
		{
			result = false;
		}
		syntaxNode.Range.EndPoint = prevPosition;
		return result;
	}

	protected virtual bool ParseStopOrEndStatement(int nodeType)
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString, nodeType);
		AddNode(syntaxNode);
		MoveNext();
		if (!ParseStatementTerminator())
		{
			result = false;
		}
		syntaxNode.Range.EndPoint = prevPosition;
		return result;
	}

	protected virtual bool ParseStopStatement()
	{
		return ParseStopOrEndStatement(121);
	}

	protected virtual bool ParseEndStatement()
	{
		return ParseStopOrEndStatement(110);
	}

	protected virtual bool ParseRedimStatement()
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString, 117, SyntaxNodeOptions.Indentation);
		AddNode(syntaxNode);
		base.SyntaxTree.Push(syntaxNode);
		try
		{
			MoveNext();
			if (base.Token == 105)
			{
				MoveNext();
			}
			if (!ParseRedimClause())
			{
				result = false;
			}
			while (!base.Eof && base.Token == 152)
			{
				if (!ParseRedimClause())
				{
					result = false;
				}
			}
			if (!ParseStatementTerminator())
			{
				result = false;
			}
		}
		finally
		{
			base.SyntaxTree.Pop();
		}
		syntaxNode.Range.EndPoint = prevPosition;
		return result;
	}

	protected virtual bool ParseRedimClause()
	{
		ISyntaxNode node = null;
		bool flag = ParseExpression(ref node);
		if (node != null)
		{
			AddNode(node);
		}
		if (flag)
		{
			Point tokenPosition = base.TokenPosition;
			if (ParseArraySizeInitializationModifier(out var rank))
			{
				AddAttribute(new SyntaxAttribute(tokenPosition, NetNodeType.ArrayModifier.ToString(), rank));
			}
			else
			{
				flag = false;
			}
		}
		return flag;
	}

	protected virtual bool ParseEraseStatement()
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString, 111, SyntaxNodeOptions.Indentation);
		AddNode(syntaxNode);
		MoveNext();
		ISyntaxNode node = null;
		if (!ParseExpressionList(ref node))
		{
			result = false;
		}
		if (node != null)
		{
			syntaxNode.AddChild(node);
		}
		if (!ParseStatementTerminator())
		{
			result = false;
		}
		syntaxNode.Range.EndPoint = prevPosition;
		return result;
	}

	protected virtual bool TryParseLabeledStatement(out bool parsed)
	{
		Point tokenPosition = base.TokenPosition;
		parsed = false;
		SaveState();
		string name;
		try
		{
			parsed = ParseLabelName(out name) && Expected(VbLexerToken.Colon);
		}
		finally
		{
			RestoreState(!parsed);
		}
		if (parsed)
		{
			ISyntaxNode syntaxNode = new SyntaxNode(tokenPosition, name, 101, SyntaxNodeOptions.Indentation);
			AddNode(syntaxNode);
			syntaxNode.Range.EndPoint = prevPosition;
		}
		return parsed;
	}

	protected virtual bool ParseExpressionStatement()
	{
		if (IsInvalidExpressionToken(base.Token))
		{
			SyntaxError();
			MoveNext();
			return false;
		}
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, string.Empty, 103, SyntaxNodeOptions.Indentation);
		AddNode(syntaxNode);
		base.SyntaxTree.Push(syntaxNode);
		try
		{
			result = ParseStatementExpression();
			if (!ParseStatementTerminator())
			{
				result = false;
			}
		}
		finally
		{
			base.SyntaxTree.Pop();
		}
		syntaxNode.Range.EndPoint = prevPosition;
		return result;
	}

	protected virtual bool ParseStatementExpression()
	{
		ISyntaxNode node = null;
		bool result = ParseExpression(ref node);
		if (node != null)
		{
			AddNode(node);
		}
		return result;
	}

	protected virtual bool ParseStatementTerminator()
	{
		if (base.Token == 154)
		{
			MoveNext();
		}
		return true;
	}

	protected virtual bool ParseStatementTerminator(int line)
	{
		if (base.Token == 154)
		{
			MoveNext();
			return true;
		}
		return lineIndex != line;
	}

	protected virtual bool ParseExpressionBlockStatement(int endToken, int nodeType)
	{
		return ParseExpressionBlockStatement(new int[1] { 45 }, endToken, nodeType, scope: true);
	}

	protected virtual bool ParseExpressionBlockStatementEnd(int endToken)
	{
		MoveNext();
		if (Expected(endToken))
		{
			return ParseStatementTerminator();
		}
		return false;
	}

	protected virtual bool ParseExpressionBlockStatement(int[] tokens, int endToken, int nodeType, bool scope)
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString, nodeType, SyntaxNodeOptions.Indentation | SyntaxNodeOptions.Outlining);
		syntaxNode.AddAttribute(new SyntaxAttribute(base.TokenPosition, SyntaxConsts.SuppressReparsingScope, null));
		AddNode(syntaxNode);
		base.SyntaxTree.Push(syntaxNode);
		try
		{
			MoveNext();
			ISyntaxNode node = null;
			bool flag = true;
			if (!ParseExpression(ref node))
			{
				result = false;
			}
			if (node != null)
			{
				syntaxNode.AddChild(node);
			}
			if (!ParseStatementTerminator())
			{
				flag = false;
				result = false;
			}
			bool blockEnd = false;
			result = ParseBlock(tokens, scope, out blockEnd);
			if (!ParseExpressionBlockStatementEnd(endToken))
			{
				if (flag && endToken != 201)
				{
					Point position = prevPosition;
					string definitionScopeEndExpected = SyntaxConsts.DefinitionScopeEndExpected;
					string text = VbLexerToken.End.ToString();
					VbLexerToken vbLexerToken = (VbLexerToken)endToken;
					syntaxNode.AddAttribute(new SyntaxAttribute(position, definitionScopeEndExpected, "\r\n" + text + " " + vbLexerToken));
				}
				result = false;
			}
		}
		finally
		{
			base.SyntaxTree.Pop();
		}
		syntaxNode.Range.EndPoint = prevPosition;
		return result;
	}

	protected virtual bool ParseBlockStatement(int token, int endToken)
	{
		return ParseBlockStatement(new int[1] { token }, endToken);
	}

	protected virtual bool ParseBlockStatementEnd(int endToken)
	{
		if (endToken != 201)
		{
			MoveNext();
			return Expected(endToken);
		}
		return true;
	}

	protected virtual bool ParseBlockStatement(int[] tokens, int endToken)
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode(prevPosition, string.Empty, 102);
		AddNode(syntaxNode);
		base.SyntaxTree.Push(syntaxNode);
		try
		{
			if (!ParseBlock(tokens))
			{
				result = false;
			}
			if (IsDeclarationToken(base.Token) || !ParseBlockStatementEnd(endToken))
			{
				Point position = prevPosition;
				string definitionScopeEndExpected = SyntaxConsts.DefinitionScopeEndExpected;
				string text = VbLexerToken.End.ToString();
				VbLexerToken vbLexerToken = (VbLexerToken)endToken;
				syntaxNode.AddAttribute(new SyntaxAttribute(position, definitionScopeEndExpected, "\r\n" + text + " " + vbLexerToken));
				result = false;
			}
		}
		finally
		{
			base.SyntaxTree.Pop();
		}
		syntaxNode.Range.EndPoint = prevPosition;
		ISyntaxAttribute syntaxAttribute = syntaxNode.FindAttribute(SyntaxConsts.DefinitionScope);
		if (syntaxAttribute != null)
		{
			AddAttribute(syntaxAttribute);
		}
		syntaxAttribute = syntaxNode.FindAttribute(SyntaxConsts.DefinitionScopeEnd);
		if (syntaxAttribute != null)
		{
			AddAttribute(syntaxAttribute);
		}
		syntaxAttribute = syntaxNode.FindAttribute(SyntaxConsts.DefinitionScopeEndExpected);
		if (syntaxAttribute != null)
		{
			AddAttribute(syntaxAttribute);
		}
		return result;
	}

	protected virtual bool ParseBlock(int[] endTokens)
	{
		bool blockEnd = false;
		return ParseBlock(endTokens, scope: true, out blockEnd);
	}

	protected virtual bool IsInvalidExpressionToken(int token)
	{
		switch ((VbLexerToken)token)
		{
		case VbLexerToken.AddHandler:
		case VbLexerToken.Call:
		case VbLexerToken.Const:
		case VbLexerToken.Continue:
		case VbLexerToken.Dim:
		case VbLexerToken.Do:
		case VbLexerToken.ElseIf:
		case VbLexerToken.End:
		case VbLexerToken.EndIf:
		case VbLexerToken.Erase:
		case VbLexerToken.Error:
		case VbLexerToken.Exit:
		case VbLexerToken.For:
		case VbLexerToken.GoTo:
		case VbLexerToken.If:
		case VbLexerToken.Loop:
		case VbLexerToken.Mid:
		case VbLexerToken.Next:
		case VbLexerToken.On:
		case VbLexerToken.RaiseEvent:
		case VbLexerToken.ReDim:
		case VbLexerToken.RemoveHandler:
		case VbLexerToken.Resume:
		case VbLexerToken.Return:
		case VbLexerToken.Select:
		case VbLexerToken.Static:
		case VbLexerToken.Stop:
		case VbLexerToken.SyncLock:
		case VbLexerToken.Throw:
		case VbLexerToken.Try:
		case VbLexerToken.While:
		case VbLexerToken.With:
			return true;
		default:
			return IsInvalidBlockToken(token);
		}
	}

	protected virtual bool IsInvalidBlockToken(int token)
	{
		if (token != 45)
		{
			return IsDeclarationToken(token);
		}
		return true;
	}

	protected virtual bool ParseBlock(int[] endTokens, bool scope, out bool blockEnd)
	{
		bool result = true;
		blockEnd = false;
		if (scope)
		{
			AddAttribute(new SyntaxAttribute(prevPosition, SyntaxConsts.BlockScope, null));
		}
		while (!base.Eof && Array.IndexOf(endTokens, base.Token) < 0)
		{
			if (IsInvalidBlockToken(base.Token))
			{
				result = false;
				break;
			}
			if (!ParseStatement())
			{
				result = false;
			}
			if (!ParseStatementTerminator())
			{
				result = false;
			}
		}
		blockEnd = Array.IndexOf(endTokens, base.Token) >= 0;
		AddAttribute(new SyntaxAttribute(base.CurrentPosition, SyntaxConsts.DefinitionScopeEnd, null));
		return result;
	}

	protected override bool ParseBlock()
	{
		bool result = ParseBlock(new int[9] { 45, 43, 44, 46, 14, 87, 76, 15, 53 });
		MoveNext();
		return result;
	}

	protected virtual bool ParseBlock(int endToken)
	{
		return ParseBlock(new int[1] { endToken });
	}

	protected virtual void InitGlobalModules(IReflectionRepository repository)
	{
		repository.RegisterType("Interaction", typeof(Interaction), global: true);
		repository.RegisterType("Information", typeof(Information), global: true);
		repository.RegisterType("Strings", typeof(Strings), global: true);
		repository.RegisterType("Err", typeof(ErrObject), global: true);
		repository.RegisterNamespace("Microsoft.VisualBasic");
	}

	protected override IListMembers CreateListMembers()
	{
		return new VbListMembers();
	}

	protected override IParameterInfo CreateParameterInfo()
	{
		return new VbParameterInfo();
	}

	protected virtual string SmartCapitalize(string s, short[] colorData)
	{
		string text = s;
		int i = 0;
		for (int length = s.Length; i < length; i++)
		{
			if ((byte)colorData[i] == 3)
			{
				int j;
				for (j = i; j < length - 1 && (byte)colorData[j + 1] == 3; j++)
				{
				}
				string text2 = s.Substring(i, j - i + 1);
				object obj = reswords[text2.ToLower(CultureInfo.InvariantCulture)];
				if (obj != null && obj.ToString() != text2)
				{
					text = text.Remove(i, j - i + 1).Insert(i, obj.ToString());
				}
				i = j;
			}
		}
		return text;
	}

	public override void ResetOptions()
	{
		Options = SyntaxConsts.DefaultNetSyntaxOptions | SyntaxOptions.ReparseOnLineChange | SyntaxOptions.AutoComplete;
	}

	public override string GetSingleLineComment()
	{
		return "'";
	}

	public override CodeCompletionType GetCompletionType(CodeCompletionArgs e)
	{
		if (e.CompletionType == CodeCompletionType.None)
		{
			switch (e.KeyChar)
			{
			case ' ':
			case '=':
				return CodeCompletionType.SpecialListMembers;
			case ',':
				return CodeCompletionType.ParameterInfo;
			case '<':
				return CodeCompletionType.ListMembers;
			}
		}
		return base.GetCompletionType(e);
	}

	protected virtual ISyntaxNode GetIncompleteNode(ISyntaxNode node)
	{
		while (node != null)
		{
			switch ((NetNodeType)node.NodeType)
			{
			case NetNodeType.Class:
			case NetNodeType.Struct:
			case NetNodeType.IfStatement:
			case NetNodeType.WhileStatement:
			case NetNodeType.DoStatement:
			case NetNodeType.ForStatement:
			case NetNodeType.ForEachStatement:
			case NetNodeType.BlockStatement:
			case NetNodeType.SelectStatement:
			case NetNodeType.WithStatement:
			{
				ISyntaxAttribute syntaxAttribute = node.FindAttribute(SyntaxConsts.DefinitionScopeEndExpected);
				if (syntaxAttribute != null)
				{
					return node;
				}
				return null;
			}
			}
			node = node.Parent;
		}
		return null;
	}

	public override bool ProcessAutoComplete(Point position, out string code)
	{
		code = string.Empty;
		ISyntaxNode nodeAt = GetNodeAt(position);
		bool flag = true;
		if (nodeAt.NodeType == 1)
		{
			ReparseText();
		}
		else
		{
			flag = ReparseBlock(new Point(0, position.Y));
		}
		if (flag)
		{
			nodeAt = GetNodeAt(position);
			if (nodeAt != null)
			{
				nodeAt = GetIncompleteNode(nodeAt);
				if (nodeAt != null)
				{
					ISyntaxAttribute syntaxAttribute = nodeAt.FindAttribute(SyntaxConsts.DefinitionScopeEndExpected);
					if (syntaxAttribute != null)
					{
						code = (string)syntaxAttribute.Value;
						return true;
					}
				}
			}
		}
		return false;
	}

	public override string SmartFormatLine(int index, string text, short[] colorData)
	{
		return SmartCapitalize(text, colorData);
	}

	public override void ResetCodeCompletionChars()
	{
		base.CodeCompletionChars = SyntaxConsts.DefaultVbCodeCompletionChars.ToCharArray();
	}

	public override bool ShouldSerializeCodeCompletionChars()
	{
		return !IsEqualArrays(base.CodeCompletionChars, SyntaxConsts.DefaultVbCodeCompletionChars.ToCharArray());
	}

	public override ICodeCompletionRepository CreateRepository()
	{
		IReflectionRepository reflectionRepository = base.CreateRepository() as IReflectionRepository;
		reflectionRepository.RegisterType("Boolean", typeof(bool));
		reflectionRepository.RegisterType("Byte", typeof(byte));
		reflectionRepository.RegisterType("Char", typeof(char));
		reflectionRepository.RegisterType("Date", typeof(DateTime));
		reflectionRepository.RegisterType("Decimal", typeof(decimal));
		reflectionRepository.RegisterType("Double", typeof(double));
		reflectionRepository.RegisterType("Single", typeof(float));
		reflectionRepository.RegisterType("Integer", typeof(int));
		reflectionRepository.RegisterType("Long", typeof(long));
		reflectionRepository.RegisterType("Object", typeof(object));
		reflectionRepository.RegisterType("Short", typeof(short));
		reflectionRepository.RegisterType("String", typeof(string));
		reflectionRepository.RegisterType("__enum", typeof(Enum));
		InitGlobalModules(reflectionRepository);
		reflectionRepository.RegisterSnippet("Boolean", isStatement: false);
		reflectionRepository.RegisterSnippet("Byte", isStatement: false);
		reflectionRepository.RegisterSnippet("Char", isStatement: false);
		reflectionRepository.RegisterSnippet("Date", isStatement: false);
		reflectionRepository.RegisterSnippet("DateTime", isStatement: false);
		reflectionRepository.RegisterSnippet("Decimal", isStatement: false);
		reflectionRepository.RegisterSnippet("Double", isStatement: false);
		reflectionRepository.RegisterSnippet("Int16", isStatement: false);
		reflectionRepository.RegisterSnippet("Int32", isStatement: false);
		reflectionRepository.RegisterSnippet("Int64", isStatement: false);
		reflectionRepository.RegisterSnippet("Integer", isStatement: false);
		reflectionRepository.RegisterSnippet("Long", isStatement: false);
		reflectionRepository.RegisterSnippet("New", isStatement: false);
		reflectionRepository.RegisterSnippet("Object", isStatement: false);
		reflectionRepository.RegisterSnippet("SByte", isStatement: false);
		reflectionRepository.RegisterSnippet("Short", isStatement: false);
		reflectionRepository.RegisterSnippet("Single", isStatement: false);
		reflectionRepository.RegisterSnippet("String", isStatement: false);
		reflectionRepository.RegisterSnippet("UInt16", isStatement: false);
		reflectionRepository.RegisterSnippet("UInt32", isStatement: false);
		reflectionRepository.RegisterSnippet("UInt64", isStatement: false);
		reflectionRepository.RegisterSnippet("UInteger", isStatement: false);
		reflectionRepository.RegisterSnippet("ULong", isStatement: false);
		reflectionRepository.RegisterSnippet("UShort", isStatement: false);
		return reflectionRepository;
	}
}
