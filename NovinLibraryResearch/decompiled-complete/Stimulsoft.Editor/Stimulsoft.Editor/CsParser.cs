using System.Collections;
using System.ComponentModel;
using System.Drawing;

namespace Stimulsoft.Editor;

[ToolboxItem(false)]
public class CsParser : NetSyntaxParser
{
	private const int stateNormal = 0;

	private const int stateComment = 1;

	private const int stateXmlComment = 2;

	private const int stateDefine = 3;

	private const int stateString = 4;

	protected Hashtable reswords;

	protected LexerProc lexSymbolProc;

	protected LexerProc lexWhitespaceProc;

	protected LexerProc lexIdentifierProc;

	protected LexerProc lexNumberProc;

	protected LexerProc lexStringProc;

	protected LexerProc lexStringEndProc;

	protected LexerProc lexCommentProc;

	protected LexerProc lexCommentEndProc;

	protected LexerProc lexDefineProc;

	protected LexerProc lexDefineEndProc;

	protected LexerProc lexXmlCommentProc;

	protected LexerProc lexXmlCommentTagProc;

	protected virtual void InitReswords()
	{
		reswords = new Hashtable();
		reswords.Add("abstract", CsLexerToken.Abstract);
		reswords.Add("as", CsLexerToken.As);
		reswords.Add("add", CsLexerToken.Add);
		reswords.Add("base", CsLexerToken.Base);
		reswords.Add("bool", CsLexerToken.Bool);
		reswords.Add("break", CsLexerToken.Break);
		reswords.Add("byte", CsLexerToken.Byte);
		reswords.Add("case", CsLexerToken.Case);
		reswords.Add("catch", CsLexerToken.Catch);
		reswords.Add("char", CsLexerToken.Char);
		reswords.Add("checked", CsLexerToken.Checked);
		reswords.Add("class", CsLexerToken.Class);
		reswords.Add("const", CsLexerToken.Consts);
		reswords.Add("continue", CsLexerToken.Continue);
		reswords.Add("decimal", CsLexerToken.Decimal);
		reswords.Add("default", CsLexerToken.Default);
		reswords.Add("delegate", CsLexerToken.Delegate);
		reswords.Add("do", CsLexerToken.Do);
		reswords.Add("double", CsLexerToken.Double);
		reswords.Add("else", CsLexerToken.Else);
		reswords.Add("enum", CsLexerToken.Enum);
		reswords.Add("event", CsLexerToken.Event);
		reswords.Add("explicit", CsLexerToken.Explicit);
		reswords.Add("extern", CsLexerToken.Extern);
		reswords.Add("false", CsLexerToken.False);
		reswords.Add("finally", CsLexerToken.Finally);
		reswords.Add("fixed", CsLexerToken.Fixed);
		reswords.Add("float", CsLexerToken.Float);
		reswords.Add("for", CsLexerToken.For);
		reswords.Add("foreach", CsLexerToken.Foreach);
		reswords.Add("goto", CsLexerToken.Goto);
		reswords.Add("if", CsLexerToken.If);
		reswords.Add("implicit", CsLexerToken.Implicit);
		reswords.Add("in", CsLexerToken.In);
		reswords.Add("int", CsLexerToken.Int);
		reswords.Add("interface", CsLexerToken.Interface);
		reswords.Add("internal", CsLexerToken.Internal);
		reswords.Add("is", CsLexerToken.Is);
		reswords.Add("lock", CsLexerToken.Lock);
		reswords.Add("long", CsLexerToken.Long);
		reswords.Add("namespace", CsLexerToken.Namespace);
		reswords.Add("new", CsLexerToken.New);
		reswords.Add("null", CsLexerToken.Null);
		reswords.Add("object", CsLexerToken.Object);
		reswords.Add("operator", CsLexerToken.Operator);
		reswords.Add("out", CsLexerToken.Out);
		reswords.Add("override", CsLexerToken.Override);
		reswords.Add("params", CsLexerToken.Params);
		reswords.Add("private", CsLexerToken.Private);
		reswords.Add("protected", CsLexerToken.Protected);
		reswords.Add("public", CsLexerToken.Public);
		reswords.Add("readonly", CsLexerToken.Readonly);
		reswords.Add("ref", CsLexerToken.Ref);
		reswords.Add("return", CsLexerToken.Return);
		reswords.Add("remove", CsLexerToken.Remove);
		reswords.Add("sbyte", CsLexerToken.Sbyte);
		reswords.Add("sealed", CsLexerToken.Sealed);
		reswords.Add("short", CsLexerToken.Short);
		reswords.Add("sizeof", CsLexerToken.Sizeof);
		reswords.Add("stackalloc", CsLexerToken.Stackalloc);
		reswords.Add("static", CsLexerToken.Static);
		reswords.Add("string", CsLexerToken.String);
		reswords.Add("struct", CsLexerToken.Struct);
		reswords.Add("switch", CsLexerToken.Switch);
		reswords.Add("this", CsLexerToken.This);
		reswords.Add("throw", CsLexerToken.Throw);
		reswords.Add("true", CsLexerToken.True);
		reswords.Add("try", CsLexerToken.Try);
		reswords.Add("typeof", CsLexerToken.Typeof);
		reswords.Add("uint", CsLexerToken.Uint);
		reswords.Add("ulong", CsLexerToken.Ulong);
		reswords.Add("unchecked", CsLexerToken.Unchecked);
		reswords.Add("unsafe", CsLexerToken.Unsafe);
		reswords.Add("ushort", CsLexerToken.Ushort);
		reswords.Add("using", CsLexerToken.Using);
		reswords.Add("virtual", CsLexerToken.Virtual);
		reswords.Add("void", CsLexerToken.Void);
		reswords.Add("volatile", CsLexerToken.Volatile);
		reswords.Add("while", CsLexerToken.While);
		reswords.Add("alias", CsLexerToken.Alias);
		reswords.Add("where", CsLexerToken.Where);
		reswords.Add("argList", CsLexerToken.ArgList);
		reswords.Add("partial", CsLexerToken.Partial);
		reswords.Add("yield", CsLexerToken.Yield);
		reswords.Add("value", CsLexerToken.Value);
		reswords.Add("get", CsLexerToken.Get);
		reswords.Add("set", CsLexerToken.Set);
	}

	protected virtual int LexSymbol()
	{
		CsLexerToken result = CsLexerToken.Whitespace_Literal;
		char c = source[currentPos];
		currentPos++;
		switch (c)
		{
		case '{':
			result = CsLexerToken.Open_brace;
			break;
		case '}':
			result = CsLexerToken.Close_brace;
			break;
		case '[':
			result = CsLexerToken.Open_bracket;
			break;
		case ']':
			result = CsLexerToken.Close_bracket;
			break;
		case '(':
			result = CsLexerToken.Open_parens;
			break;
		case ')':
			result = CsLexerToken.Close_parens;
			break;
		case '.':
			result = CsLexerToken.Dot;
			break;
		case ',':
			result = CsLexerToken.Comma;
			break;
		case ':':
			if (CurChar() == ':')
			{
				currentPos++;
				result = CsLexerToken.DblColon;
			}
			else
			{
				result = CsLexerToken.Colon;
			}
			break;
		case ';':
			result = CsLexerToken.Semicolon;
			break;
		case '~':
			result = CsLexerToken.Tilde;
			break;
		case '?':
			if (CurChar() == '?')
			{
				currentPos++;
				result = CsLexerToken.DblInterr;
			}
			else
			{
				result = CsLexerToken.Interr;
			}
			break;
		case '+':
			switch (CurChar())
			{
			case '+':
				currentPos++;
				result = CsLexerToken.Op_inc;
				break;
			case '=':
				currentPos++;
				result = CsLexerToken.Op_add_assign;
				break;
			default:
				result = CsLexerToken.Plus;
				break;
			}
			break;
		case '-':
			switch (CurChar())
			{
			case '-':
				currentPos++;
				result = CsLexerToken.Op_dec;
				break;
			case '=':
				currentPos++;
				result = CsLexerToken.Op_sub_assign;
				break;
			case '>':
				currentPos++;
				result = CsLexerToken.Op_ptr;
				break;
			default:
				result = CsLexerToken.Minus;
				break;
			}
			break;
		case '<':
			switch (CurChar())
			{
			case '<':
				currentPos++;
				if (CurChar() == '=')
				{
					currentPos++;
					result = CsLexerToken.Op_shift_left_assign;
				}
				else
				{
					result = CsLexerToken.Op_shift_left;
				}
				break;
			case '=':
				currentPos++;
				result = CsLexerToken.Op_le;
				break;
			default:
				result = CsLexerToken.Op_lt;
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
					result = CsLexerToken.Op_shift_right_assign;
				}
				else
				{
					result = CsLexerToken.Op_shift_right;
				}
				break;
			case '=':
				currentPos++;
				result = CsLexerToken.Op_ge;
				break;
			default:
				result = CsLexerToken.Op_gt;
				break;
			}
			break;
		case '=':
			if (CurChar() == '=')
			{
				currentPos++;
				result = CsLexerToken.Op_eq;
			}
			else
			{
				result = CsLexerToken.Assign;
			}
			break;
		case '!':
			if (CurChar() == '=')
			{
				currentPos++;
				result = CsLexerToken.Op_ne;
			}
			else
			{
				result = CsLexerToken.Bang;
			}
			break;
		case '&':
			switch (CurChar())
			{
			case '&':
				currentPos++;
				result = CsLexerToken.Op_and;
				break;
			case '=':
				currentPos++;
				result = CsLexerToken.Op_and_assign;
				break;
			default:
				result = CsLexerToken.Bitwise_and;
				break;
			}
			break;
		case '|':
			switch (CurChar())
			{
			case '|':
				currentPos++;
				result = CsLexerToken.Op_or;
				break;
			case '=':
				currentPos++;
				result = CsLexerToken.Op_or_assign;
				break;
			default:
				result = CsLexerToken.Bitwise_or;
				break;
			}
			break;
		case '*':
			if (CurChar() == '=')
			{
				currentPos++;
				result = CsLexerToken.Op_mult_assign;
			}
			else
			{
				result = CsLexerToken.Star;
			}
			break;
		case '/':
			if (CurChar() == '=')
			{
				currentPos++;
				result = CsLexerToken.Op_div_assign;
			}
			else
			{
				result = CsLexerToken.Div;
			}
			break;
		case '%':
			if (CurChar() == '=')
			{
				currentPos++;
				result = CsLexerToken.Op_mod_assign;
			}
			else
			{
				result = CsLexerToken.Percent;
			}
			break;
		case '^':
			if (CurChar() == '=')
			{
				currentPos++;
				result = CsLexerToken.Op_xor_assign;
			}
			else
			{
				result = CsLexerToken.Carret;
			}
			break;
		}
		return (int)result;
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
		return 137;
	}

	protected virtual int LexNumber()
	{
		char c = source[currentPos];
		int length = source.Length;
		int result = 137;
		if (c == '+' || c == '-')
		{
			int num = currentPos + 1;
			if (num >= length || source[num] < '0' || source[num] > '9')
			{
				return LexSymbol();
			}
		}
		if (c == '0')
		{
			int num = currentPos + 1;
			if (num < length && (source[num] == 'x' || source[num] == 'X'))
			{
				currentPos = num + 1;
				return LexHexNumber();
			}
		}
		base.LexNum();
		if (currentPos < length)
		{
			switch (source[currentPos])
			{
			case 'D':
			case 'd':
				currentPos++;
				return 139;
			case 'F':
			case 'f':
				currentPos++;
				return 138;
			case 'M':
			case 'm':
				currentPos++;
				return 140;
			case '.':
				if (currentPos >= length - 1)
				{
					break;
				}
				c = source[currentPos + 1];
				if (c < '0' || c > '9')
				{
					break;
				}
				currentPos++;
				LexNum();
				result = 138;
				if (currentPos < length)
				{
					c = source[currentPos];
					if (c == 'F' || c == 'f')
					{
						currentPos++;
						return 138;
					}
				}
				break;
			}
		}
		if (currentPos < length)
		{
			c = source[currentPos];
			if (c == 'E' || c == 'e')
			{
				int num = currentPos + 1;
				if (num < length)
				{
					c = source[num];
					if (c == '+' || c == '-')
					{
						num++;
					}
				}
				if (num < length)
				{
					c = source[num];
					if (c >= '0' && c <= '9')
					{
						currentPos = num;
						LexNum();
						result = 139;
					}
				}
			}
		}
		return result;
	}

	protected virtual int LexCommentEnd()
	{
		int length = source.Length;
		while (currentPos < length)
		{
			char c = source[currentPos];
			if (c == '*')
			{
				currentPos++;
				if (currentPos < length && source[currentPos] == '/')
				{
					base.State = 0;
					currentPos++;
					return 144;
				}
			}
			else
			{
				currentPos++;
			}
		}
		base.State = 1;
		return 144;
	}

	protected virtual int LexComment()
	{
		currentPos++;
		int length = source.Length;
		if (currentPos < length)
		{
			switch (source[currentPos])
			{
			case '/':
			{
				int num = currentPos + 1;
				if (num < length && source[num] == '/')
				{
					currentPos = num + 1;
					if (currentPos < length)
					{
						base.State = 2;
					}
					return 145;
				}
				currentPos = length + 1;
				return 144;
			}
			case '*':
				currentPos++;
				return LexCommentEnd();
			}
		}
		currentPos--;
		return LexSymbol();
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
		return 144;
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
		return 145;
	}

	protected virtual int LexDefineEnd()
	{
		currentPos = source.Length;
		base.State = 0;
		return 143;
	}

	protected virtual int LexDefine()
	{
		int length = source.Length;
		currentPos++;
		if (currentPos < source.Length)
		{
			char c = source[currentPos];
			if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || c == '_')
			{
				LexIdent();
			}
		}
		base.State = ((currentPos != length) ? 3 : 0);
		return 146;
	}

	protected virtual int LexString(char startChar)
	{
		int length = source.Length;
		bool flag = false;
		while (currentPos < length)
		{
			char c = source[currentPos];
			if (c == startChar && !flag)
			{
				currentPos++;
				break;
			}
			flag = c == '\\' && !flag;
			currentPos++;
		}
		if (startChar != '\'')
		{
			return 142;
		}
		return 141;
	}

	protected virtual int LexString()
	{
		char c = source[currentPos];
		currentPos++;
		if (c == '@')
		{
			int length = source.Length;
			if (currentPos < length)
			{
				c = source[currentPos];
				if (c == '"')
				{
					currentPos++;
					return LexStringEnd();
				}
			}
			return 5;
		}
		return LexString(c);
	}

	protected virtual int LexStringEnd()
	{
		int length = source.Length;
		while (currentPos < length)
		{
			char c = source[currentPos];
			if (c == '"')
			{
				currentPos++;
				if (currentPos >= length || source[currentPos] != '"')
				{
					base.State = 0;
					return 142;
				}
				currentPos++;
			}
			else
			{
				currentPos++;
			}
		}
		base.State = 4;
		return 142;
	}

	protected virtual int LexWhitespace()
	{
		LexSpace();
		return 147;
	}

	protected virtual int LexIdentifier()
	{
		LexIdent();
		object obj = reswords[base.TokenString];
		if (obj == null)
		{
			return 143;
		}
		return (int)obj;
	}

	protected override void InitLanguage()
	{
		Scheme.FileType = "c#";
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
		lexStringEndProc = LexStringEnd;
		lexCommentProc = LexComment;
		lexCommentEndProc = LexCommentEnd;
		lexDefineProc = LexDefine;
		lexDefineEndProc = LexDefineEnd;
		lexXmlCommentProc = LexXmlComment;
		lexXmlCommentTagProc = LexXmlCommentTag;
		RegisterLexerProc(0, lexWhitespaceProc);
		RegisterLexerProc(0, '!', 'ÿ', lexSymbolProc);
		RegisterLexerProc(0, 'a', 'z', lexIdentifierProc);
		RegisterLexerProc(0, 'A', 'Z', lexIdentifierProc);
		RegisterLexerProc(0, '_', lexIdentifierProc);
		RegisterLexerProc(0, '0', '9', lexNumberProc);
		RegisterLexerProc(0, new char[2] { '+', '-' }, lexSymbolProc);
		RegisterLexerProc(0, new char[3] { '@', '"', '\'' }, lexStringProc);
		RegisterLexerProc(0, '/', lexCommentProc);
		RegisterLexerProc(0, '#', lexDefineProc);
		RegisterLexerProc(4, lexStringEndProc);
		RegisterLexerProc(1, lexCommentEndProc);
		RegisterLexerProc(2, lexXmlCommentProc);
		RegisterLexerProc(2, '<', lexXmlCommentTagProc);
		RegisterLexerProc(3, lexDefineEndProc);
	}

	protected virtual bool IsKeywordToken(int token)
	{
		if (token >= 1)
		{
			return token <= 88;
		}
		return false;
	}

	protected override int GetLexerStyle(int token)
	{
		if (IsKeywordToken(token))
		{
			return 2;
		}
		if (token >= 89 && token <= 135)
		{
			return 5;
		}
		switch (token)
		{
		case 137:
		case 138:
		case 139:
		case 140:
			return 1;
		case 141:
		case 142:
			return 7;
		case 143:
			return 0;
		case 144:
			return 3;
		case 145:
			return 4;
		case 146:
			return 8;
		default:
			return 6;
		}
	}

	protected override bool IsComment(int tok)
	{
		if (tok != 144)
		{
			return tok == 145;
		}
		return true;
	}

	protected virtual void SkipComment()
	{
		while (IsComment(base.Token) || base.Token == 147)
		{
			NextToken();
		}
	}

	protected override bool ParseComment()
	{
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, SyntaxConsts.OutlineCommentText, 49, (base.SyntaxTree.Current == base.SyntaxTree.Root || base.State == 1) ? SyntaxNodeOptions.KeepIndentation : SyntaxNodeOptions.None);
		syntaxNode.AddAttribute(new SyntaxAttribute(syntaxNode.Position, SyntaxConsts.OutlineText, syntaxNode.Name));
		comments.Add(syntaxNode);
		bool flag = false;
		Point currentPosition = base.CurrentPosition;
		while (IsComment(base.Token) || base.Token == 147)
		{
			if (base.Token == 145)
			{
				flag = true;
			}
			if (base.Token != 147)
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

	protected override bool IsValidToken(int tok)
	{
		if (tok != 147)
		{
			return !IsComment(tok);
		}
		return false;
	}

	protected virtual void SyntaxError(int token)
	{
		if (base.Stack.Count == 0)
		{
			Point point = prevPosition;
			Point tokenPosition = base.TokenPosition;
			string tokenString = base.TokenString;
			CsLexerToken csLexerToken = (CsLexerToken)token;
			ISyntaxError syntaxError = new SyntaxError(tokenPosition, tokenString, csLexerToken.ToString() + " " + StringConsts.ErrExpected);
			if (point.Y != base.TokenPosition.Y)
			{
				syntaxError.Position = prevPosition;
				syntaxError.Range.EndPoint = new Point(point.X + 1, point.Y);
			}
			base.SyntaxTree.Current.AddError(syntaxError);
		}
	}

	protected virtual bool SkipTo(int token1, int token2)
	{
		while (!base.Eof && base.Token != token1 && base.Token != token2)
		{
			SyntaxError();
			MoveNext();
			if (IsKeywordToken(base.Token))
			{
				break;
			}
		}
		if (base.Token != token1)
		{
			return base.Token == token2;
		}
		return true;
	}

	protected virtual bool SkipTo(int token)
	{
		return SkipTo(token, token);
	}

	protected bool IsIdentifierToken(int token)
	{
		if (token != 143)
		{
			return token == 85;
		}
		return true;
	}

	protected bool IdentifierExpected()
	{
		return Expected(CsLexerToken.Identifier_Literal, CsLexerToken.Value);
	}

	protected bool Expected(int token)
	{
		return Expected((CsLexerToken)token);
	}

	protected bool Expected(CsLexerToken token)
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

	protected bool Expected(CsLexerToken token1, CsLexerToken token2)
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

	protected override int MoveNext()
	{
		int result = base.MoveNext();
		while (!base.Eof && base.State == 4)
		{
			result = NextToken();
		}
		return result;
	}

	protected virtual bool ParseIdentifier(out string identifier)
	{
		identifier = base.TokenString;
		return IdentifierExpected();
	}

	protected virtual bool ParseTypeIdentifier(out string identifier, bool checkBuiltIn)
	{
		identifier = base.TokenString;
		bool flag = true;
		if (checkBuiltIn && IsBuiltInType(base.Token))
		{
			MoveNext();
		}
		else
		{
			flag = IdentifierExpected();
		}
		if (flag)
		{
			if (IsTypeArgumentList(base.Token) && !ParseTypeArgumentList(out var _))
			{
				flag = false;
			}
			if (IsNullableType(base.Token) && !ParseNullableType(out var _))
			{
				flag = false;
			}
			if (IsPointerType(base.Token) && !ParsePointerType(out var _))
			{
				flag = false;
			}
		}
		else
		{
			identifier = string.Empty;
		}
		return flag;
	}

	protected virtual bool ParseQualifiedIdentifier(out string identifier)
	{
		bool flag = ParseTypeIdentifier(out identifier, checkBuiltIn: false);
		if (flag)
		{
			while (base.Token == 95 || base.Token == 98)
			{
				identifier += base.TokenString;
				MoveNext();
				if (base.Token == 66)
				{
					break;
				}
				if (ParseTypeIdentifier(out var identifier2, checkBuiltIn: false))
				{
					identifier += identifier2;
				}
				else
				{
					flag = false;
				}
			}
		}
		return flag;
	}

	protected virtual bool ParseRankSpecifier(out string rank)
	{
		bool result = true;
		rank = base.TokenString;
		if (Expected(CsLexerToken.Open_bracket))
		{
			while (base.Token == 96)
			{
				rank += base.TokenString;
				MoveNext();
			}
			if (base.Token == 92)
			{
				rank += base.TokenString;
			}
			result = Expected(CsLexerToken.Close_bracket);
		}
		return result;
	}

	protected virtual bool TryParseRankSpecifiers(out string rank)
	{
		bool flag = true;
		rank = string.Empty;
		while (base.Token == 91)
		{
			flag = ParseRankSpecifier(out var rank2);
			if (flag)
			{
				rank += rank2;
			}
		}
		return flag;
	}

	protected virtual bool IsBuiltInType(int Token)
	{
		switch ((CsLexerToken)Token)
		{
		case CsLexerToken.Bool:
		case CsLexerToken.Byte:
		case CsLexerToken.Char:
		case CsLexerToken.Decimal:
		case CsLexerToken.Double:
		case CsLexerToken.Float:
		case CsLexerToken.Int:
		case CsLexerToken.Long:
		case CsLexerToken.Object:
		case CsLexerToken.Sbyte:
		case CsLexerToken.Short:
		case CsLexerToken.String:
		case CsLexerToken.Uint:
		case CsLexerToken.Ulong:
		case CsLexerToken.Ushort:
		case CsLexerToken.Void:
			return true;
		default:
			return false;
		}
	}

	protected virtual bool ParseTypeName(out string type)
	{
		if (!IsBuiltInType(base.Token))
		{
			return ParseQualifiedIdentifier(out type);
		}
		return ParseTypeIdentifier(out type, checkBuiltIn: true);
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

	protected virtual bool ParseBaseList(out string types)
	{
		bool result = true;
		if (ParseType(out types))
		{
			while (base.Token == 96)
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
		else
		{
			result = false;
		}
		return result;
	}

	protected virtual bool IsBaseList(int token)
	{
		return token == 97;
	}

	protected virtual bool SkipToDeclarationStart(int nodeType)
	{
		return SkipTo(89);
	}

	protected override bool ParseDeclaration(ISyntaxNode node)
	{
		ISyntaxAttributes attrs = null;
		ParseModifiers(ref attrs);
		switch ((CsLexerToken)base.Token)
		{
		case CsLexerToken.Class:
		case CsLexerToken.Enum:
		case CsLexerToken.Interface:
		case CsLexerToken.Namespace:
		case CsLexerToken.Struct:
			return ParseDeclaration(attrs, node, node.NodeType);
		default:
			return false;
		}
	}

	protected virtual bool ParseDeclaration(ISyntaxAttributes attrs, int nodeType)
	{
		return ParseDeclaration(attrs, null, nodeType);
	}

	protected virtual bool ParseDeclaration(ISyntaxAttributes attrs, ISyntaxNode node, int nodeType)
	{
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
			if (nodeType != 7)
			{
				base.SyntaxTree.Push(node);
				try
				{
					if (nodeType != 11 && IsTypeParameterList(base.Token) && !ParseTypeParameterList())
					{
						result = false;
					}
					if (IsBaseList(base.Token) && !ParseBaseList())
					{
						result = false;
					}
					if (IsTypeParameterConstraintsClause(base.Token) && !ParseTypeParameterConstraintsClauses())
					{
						result = false;
					}
				}
				finally
				{
					base.SyntaxTree.Pop();
				}
			}
			node.AddAttribute(new SyntaxAttribute(prevPosition, SyntaxConsts.DeclarationScope, null));
			if (!ParseDeclarationBody(node, nodeType))
			{
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

	protected virtual bool ParseBaseList()
	{
		MoveNext();
		bool result = true;
		Point tokenPosition = base.TokenPosition;
		if (ParseBaseList(out var types))
		{
			AddAttribute(new SyntaxAttribute(tokenPosition, NetNodeType.TypeList.ToString(), types));
		}
		else
		{
			result = false;
		}
		return result;
	}

	protected virtual bool ParseDeclarationBodyEnd(ISyntaxNode node)
	{
		return Expected(CsLexerToken.Close_brace);
	}

	protected virtual bool ParseDeclarationBody(ISyntaxNode node, int nodeType)
	{
		bool result = true;
		Point tokenPosition = base.TokenPosition;
		base.SyntaxTree.Push(node);
		bool flag = false;
		try
		{
			flag = SkipToDeclarationStart(nodeType);
			if (flag)
			{
				node.AddAttribute(new SyntaxAttribute(tokenPosition, SyntaxConsts.DefinitionScope, null));
				node.Options |= SyntaxNodeOptions.Indentation | SyntaxNodeOptions.Outlining;
				MoveNext();
				switch ((NetNodeType)node.NodeType)
				{
				case NetNodeType.Namespace:
				case NetNodeType.Class:
				case NetNodeType.Struct:
				case NetNodeType.Interface:
					if (!ParseClassBody())
					{
						result = false;
					}
					break;
				case NetNodeType.Enum:
					if (!ParseEnumBody())
					{
						result = false;
					}
					break;
				default:
					result = false;
					break;
				}
			}
		}
		finally
		{
			base.SyntaxTree.Pop();
		}
		if (flag && ParseDeclarationBodyEnd(node))
		{
			node.AddAttribute(new SyntaxAttribute(prevPosition, SyntaxConsts.DefinitionScopeEnd, null));
			if (base.Token == 99)
			{
				MoveNext();
			}
		}
		else
		{
			result = false;
		}
		return result;
	}

	protected virtual bool ParseEnumMember()
	{
		bool flag = true;
		Point tokenPosition = base.TokenPosition;
		flag = ParseIdentifier(out var identifier);
		if (flag)
		{
			ISyntaxNode syntaxNode = new SyntaxNode(tokenPosition, identifier, 13, SyntaxNodeOptions.CodeCompletion);
			AddNode(syntaxNode);
			if (base.Token == 104)
			{
				MoveNext();
				ISyntaxNode node = null;
				if (!ParseExpression(ref node))
				{
					flag = false;
				}
				if (node != null)
				{
					syntaxNode.AddChild(node);
				}
			}
			if (base.Token == 96)
			{
				MoveNext();
			}
			syntaxNode.Range.EndPoint = prevPosition;
		}
		else
		{
			MoveNext();
		}
		return flag;
	}

	protected virtual bool ParseEnumBody()
	{
		bool result = true;
		while (!base.Eof && base.Token != 90)
		{
			if (!ParseEnumMember())
			{
				result = false;
			}
		}
		return result;
	}

	protected virtual bool ParseClassBody()
	{
		bool result = true;
		bool flag = base.SyntaxTree.Current != base.SyntaxTree.Root;
		ISyntaxAttributes attrs = null;
		while (!base.Eof && (!flag || base.Token != 90))
		{
			ParseModifiers(ref attrs);
			switch ((CsLexerToken)base.Token)
			{
			case CsLexerToken.Directive_Literal:
				if (!ParseDirective())
				{
					result = false;
				}
				break;
			case CsLexerToken.Open_bracket:
				if (!ParseAttributeListDeclaration())
				{
					result = false;
				}
				break;
			case CsLexerToken.Class:
			case CsLexerToken.Delegate:
			case CsLexerToken.Enum:
			case CsLexerToken.Event:
			case CsLexerToken.Explicit:
			case CsLexerToken.Implicit:
			case CsLexerToken.Interface:
			case CsLexerToken.Namespace:
			case CsLexerToken.Struct:
			case CsLexerToken.Using:
			case CsLexerToken.Alias:
			case CsLexerToken.Tilde:
				if (!ParseKnownMemberDeclaration(attrs))
				{
					result = false;
				}
				attrs = null;
				break;
			default:
				if (!ParseUnknownMemberDeclaration(attrs))
				{
					result = false;
				}
				attrs = null;
				break;
			}
		}
		return result;
	}

	protected virtual bool ParseKnownMemberDeclaration(ISyntaxAttributes attrs)
	{
		bool result = true;
		switch ((CsLexerToken)base.Token)
		{
		case CsLexerToken.Using:
			if (!ParseUsingList())
			{
				result = false;
			}
			break;
		case CsLexerToken.Namespace:
			if (!ParseDeclaration(attrs, 7))
			{
				result = false;
			}
			break;
		case CsLexerToken.Class:
			if (!ParseDeclaration(attrs, 8))
			{
				result = false;
			}
			break;
		case CsLexerToken.Struct:
			if (!ParseDeclaration(attrs, 9))
			{
				result = false;
			}
			break;
		case CsLexerToken.Interface:
			if (!ParseDeclaration(attrs, 10))
			{
				result = false;
			}
			break;
		case CsLexerToken.Enum:
			if (!ParseDeclaration(attrs, 11))
			{
				result = false;
			}
			break;
		case CsLexerToken.Delegate:
			if (!ParseDelegateDeclaration(attrs))
			{
				result = false;
			}
			break;
		case CsLexerToken.Event:
			if (!ParseEventDeclaration(attrs))
			{
				result = false;
			}
			break;
		case CsLexerToken.Tilde:
			if (!ParseDestructorDeclaration(attrs))
			{
				result = false;
			}
			break;
		case CsLexerToken.Explicit:
		case CsLexerToken.Implicit:
			if (!ParseOperatorDeclaration(attrs))
			{
				result = false;
			}
			break;
		case CsLexerToken.Alias:
			if (!ParseAliasList(attrs))
			{
				result = false;
			}
			break;
		}
		return result;
	}

	protected virtual bool ParseUnknownMemberDeclaration(ISyntaxAttributes attrs)
	{
		Point tokenPosition = base.TokenPosition;
		string type;
		bool flag = ParseType(out type);
		if (flag)
		{
			switch ((CsLexerToken)base.Token)
			{
			case CsLexerToken.Open_parens:
				if (!ParseConstructorDeclaration(attrs, type, tokenPosition))
				{
					flag = false;
				}
				break;
			case CsLexerToken.This:
				if (!ParseIndexerDeclaration(attrs, type, tokenPosition))
				{
					flag = false;
				}
				break;
			case CsLexerToken.Operator:
				if (!ParseOperatorDeclaration(attrs, type, tokenPosition, isExplicit: false))
				{
					flag = false;
				}
				break;
			case CsLexerToken.Value:
			case CsLexerToken.Identifier_Literal:
			{
				Point tokenPosition2 = base.TokenPosition;
				if (!ParseQualifiedIdentifier(out var identifier) && (!(identifier != string.Empty) || base.Token != 66))
				{
					break;
				}
				switch ((CsLexerToken)base.Token)
				{
				case CsLexerToken.This:
					if (!ParseIndexerDeclaration(attrs, type, tokenPosition))
					{
						flag = false;
					}
					break;
				case CsLexerToken.Open_parens:
					if (!ParseMethodDeclaration(attrs, type, tokenPosition, identifier))
					{
						flag = false;
					}
					break;
				case CsLexerToken.Open_brace:
					if (!ParsePropertyDeclaration(attrs, type, tokenPosition, identifier))
					{
						flag = false;
					}
					break;
				case CsLexerToken.Open_bracket:
				case CsLexerToken.Comma:
				case CsLexerToken.Semicolon:
				case CsLexerToken.Assign:
					if (!ParseFieldDeclaration(attrs, type, tokenPosition, identifier))
					{
						flag = false;
					}
					break;
				default:
					SyntaxError();
					flag = false;
					break;
				}
				break;
			}
			default:
				SyntaxError();
				flag = false;
				break;
			}
		}
		else if (base.TokenPosition.Equals((object?)tokenPosition))
		{
			MoveNext();
		}
		return flag;
	}

	protected virtual bool IsModifier(int token)
	{
		switch ((CsLexerToken)token)
		{
		case CsLexerToken.Abstract:
		case CsLexerToken.Consts:
		case CsLexerToken.Extern:
		case CsLexerToken.Fixed:
		case CsLexerToken.Internal:
		case CsLexerToken.New:
		case CsLexerToken.Override:
		case CsLexerToken.Private:
		case CsLexerToken.Protected:
		case CsLexerToken.Public:
		case CsLexerToken.Readonly:
		case CsLexerToken.Sealed:
		case CsLexerToken.Static:
		case CsLexerToken.Unsafe:
		case CsLexerToken.Virtual:
		case CsLexerToken.Volatile:
		case CsLexerToken.Partial:
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
		if (!(tokenString == "#region"))
		{
			if (tokenString == "#endregion")
			{
				Point endPoint = new Point(source.Length, lineIndex);
				if (base.State == 3)
				{
					MoveNext();
				}
				MoveNext();
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
				if (base.State == 3)
				{
					MoveNext();
				}
				MoveNext();
			}
		}
		else
		{
			Point tokenPosition = base.TokenPosition;
			ISyntaxNode syntaxNode = new SyntaxNode(tokenPosition, base.TokenString, 39);
			if (base.State == 3)
			{
				MoveNext();
			}
			syntaxNode.Name = base.TokenString.Trim();
			AddNode(syntaxNode);
			base.SyntaxTree.Push(syntaxNode);
			MoveNext();
		}
		return result;
	}

	protected virtual bool ParseUsingDeclaration()
	{
		return ParseUsingDeclaration(new SyntaxNode());
	}

	protected override bool ParseUsingDeclaration(ISyntaxNode node)
	{
		bool result = true;
		Point tokenPosition = base.TokenPosition;
		if (ParseQualifiedIdentifier(out var identifier) || identifier != string.Empty)
		{
			node.NodeType = 3;
			node.Position = tokenPosition;
			node.Name = identifier;
			AddNode(node);
			if (base.Token == 104)
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
			if (Expected(CsLexerToken.Semicolon))
			{
				result = false;
			}
			node.Range.EndPoint = prevPosition;
		}
		else
		{
			result = false;
		}
		return result;
	}

	protected virtual bool ParseUsingList()
	{
		ISyntaxNode node = new SyntaxNode(base.TokenPosition, string.Empty, 2, SyntaxNodeOptions.Outlining);
		return ParseUsingList(node);
	}

	protected override bool ParseUsingList(ISyntaxNode node)
	{
		bool result = true;
		node.AddAttribute(new SyntaxAttribute(base.TokenPosition, SyntaxConsts.OutlineText, SyntaxConsts.OutlineUsingText));
		AddNode(node);
		if (!BeforeDeclaration(node))
		{
			result = false;
		}
		base.SyntaxTree.Push(node);
		try
		{
			while (base.Token == 76)
			{
				MoveNext();
				ParseUsingDeclaration();
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
			if (!TryParseAttributeSpecifier())
			{
				result = false;
			}
			while (!base.Eof && base.Token != 92)
			{
				if (!ParseAttributeDeclaration())
				{
					result = false;
				}
				if (base.Token == 96)
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
		if (!Expected(CsLexerToken.Close_bracket))
		{
			result = false;
		}
		syntaxNode.Range.EndPoint = prevPosition;
		return result;
	}

	protected virtual bool TryParseAttributeSpecifier()
	{
		bool result = true;
		switch ((CsLexerToken)base.Token)
		{
		case CsLexerToken.Assembly:
		case CsLexerToken.Event:
		case CsLexerToken.Return:
			AddAttribute(new SyntaxAttribute(base.TokenPosition, NetNodeType.AttributeTarget.ToString(), base.TokenString));
			MoveNext();
			if (!Expected(CsLexerToken.Colon))
			{
				result = false;
			}
			break;
		case CsLexerToken.Value:
		case CsLexerToken.Identifier_Literal:
		{
			bool flag = false;
			SaveState();
			try
			{
				Point tokenPosition = base.TokenPosition;
				if (ParseIdentifier(out var identifier))
				{
					flag = base.Token == 97;
					if (flag)
					{
						MoveNext();
						AddAttribute(new SyntaxAttribute(tokenPosition, NetNodeType.AttributeTarget.ToString(), identifier));
					}
				}
			}
			finally
			{
				RestoreState(!flag);
			}
			break;
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
			if (base.Token == 93)
			{
				ISyntaxNode syntaxNode2 = new SyntaxNode(base.TokenPosition, string.Empty, 27);
				syntaxNode.AddChild(syntaxNode2);
				base.SyntaxTree.Push(syntaxNode2);
				try
				{
					MoveNext();
					while (!base.Eof && base.Token != 94)
					{
						result = ParsePositionalOrNamedParam();
						syntaxNode2.Range.EndPoint = prevPosition;
						if (base.Token == 96)
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
				if (!Expected(CsLexerToken.Close_parens))
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
				if (base.Token == 104)
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

	protected virtual bool ParseDelegateDeclaration(ISyntaxAttributes attrs)
	{
		bool result = true;
		MoveNext();
		Point tokenPosition = base.TokenPosition;
		if (ParseType(out var type) && ParseQualifiedIdentifier(out var identifier))
		{
			ISyntaxNode syntaxNode = new SyntaxNode((attrs != null && attrs.Count > 0) ? attrs[0].Position : tokenPosition, identifier, 20, SyntaxNodeOptions.CodeCompletion);
			syntaxNode.AddAttribute(new SyntaxAttribute(tokenPosition, NetNodeType.Type.ToString(), type));
			if (attrs != null)
			{
				syntaxNode.AddAttributes(attrs);
			}
			if (!BeforeDeclaration(syntaxNode))
			{
				result = false;
			}
			CsLexerToken csLexerToken = (CsLexerToken)base.Token;
			if (csLexerToken == CsLexerToken.Open_parens)
			{
				base.SyntaxTree.Push(syntaxNode);
				try
				{
					if (!ParseParameterListDeclaration())
					{
						result = false;
					}
				}
				finally
				{
					base.SyntaxTree.Pop();
				}
				if (!Expected(CsLexerToken.Semicolon))
				{
					result = false;
				}
			}
			if (!AfterDeclaration(syntaxNode))
			{
				result = false;
			}
			syntaxNode.Range.EndPoint = prevPosition;
		}
		else
		{
			result = false;
		}
		return result;
	}

	protected virtual bool ParseParameterListDeclaration()
	{
		return ParseParameterListDeclaration(93, 94);
	}

	protected virtual bool ParseParameterListDeclaration(int startToken, int endToken)
	{
		bool result = true;
		if (base.Token == startToken)
		{
			ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, string.Empty, 27);
			AddNode(syntaxNode);
			base.SyntaxTree.Push(syntaxNode);
			try
			{
				MoveNext();
				while (!base.Eof && base.Token != endToken)
				{
					if (!ParseParameterDeclaration())
					{
						result = false;
					}
					if (base.Token == 96)
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
			SyntaxError(startToken);
			result = false;
		}
		return result;
	}

	protected virtual bool ParseParameterDeclaration()
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, string.Empty, 26, SyntaxNodeOptions.CodeCompletion);
		switch ((CsLexerToken)base.Token)
		{
		case CsLexerToken.Open_bracket:
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
			break;
		case CsLexerToken.Out:
		case CsLexerToken.Params:
		case CsLexerToken.Ref:
			syntaxNode.AddAttribute(new SyntaxAttribute(base.TokenPosition, NetNodeType.ParameterModifier.ToString(), base.TokenString));
			MoveNext();
			break;
		}
		if (base.Token == 81)
		{
			MoveNext();
			syntaxNode.Name = base.TokenString;
			if (base.Token != 94)
			{
				SyntaxError(94);
				result = false;
			}
			AddNode(syntaxNode);
		}
		else
		{
			Point tokenPosition = base.TokenPosition;
			if (ParseType(out var type) && ParseIdentifier(out var identifier))
			{
				syntaxNode.Name = identifier;
				syntaxNode.AddAttribute(new SyntaxAttribute(tokenPosition, NetNodeType.Type.ToString(), type));
				if (base.Token == 91)
				{
					ParseRankSpecifier(out var _);
					SyntaxError(StringConsts.ErrArrayParamSpecifier);
					result = false;
				}
				if (base.Token == 104)
				{
					MoveNext();
					ISyntaxNode node = null;
					ParseConstantExpression(ref node);
					if (node != null)
					{
						syntaxNode.AddChild(node);
					}
					SyntaxError(StringConsts.ErrDefaultParamNotPermitted);
					result = false;
				}
				AddNode(syntaxNode);
			}
			else
			{
				result = false;
			}
		}
		syntaxNode.Range.EndPoint = prevPosition;
		return result;
	}

	protected virtual bool ParseEventDeclaration(ISyntaxAttributes attrs)
	{
		bool result = true;
		MoveNext();
		Point tokenPosition = base.TokenPosition;
		if (ParseType(out var type))
		{
			Point tokenPosition2 = base.TokenPosition;
			if (ParseQualifiedIdentifier(out var identifier))
			{
				ISyntaxNode syntaxNode = new SyntaxNode((attrs != null && attrs.Count > 0) ? attrs[0].Position : tokenPosition, identifier, 22, SyntaxNodeOptions.CodeCompletion);
				if (attrs != null)
				{
					syntaxNode.AddAttributes(attrs);
				}
				AddNode(syntaxNode);
				syntaxNode.AddAttribute(new SyntaxAttribute(tokenPosition, NetNodeType.Type.ToString(), type));
				syntaxNode.AddAttribute(new SyntaxAttribute(prevPosition, SyntaxConsts.DeclarationScope, null));
				if (!BeforeDeclaration(syntaxNode))
				{
					result = false;
				}
				switch ((CsLexerToken)base.Token)
				{
				case CsLexerToken.Open_brace:
					syntaxNode.AddAttribute(new SyntaxAttribute(base.TokenPosition, SyntaxConsts.DefinitionScope, null));
					syntaxNode.Options |= SyntaxNodeOptions.Indentation;
					base.SyntaxTree.Push(syntaxNode);
					try
					{
						MoveNext();
						while (!base.Eof)
						{
							if (base.Token == 91 && !ParseAttributeListDeclaration())
							{
								result = false;
							}
							if (IsAccessor(NetNodeType.EventAccessor))
							{
								if (!ParseAccessors(null, NetNodeType.EventAccessor))
								{
									result = false;
								}
								continue;
							}
							break;
						}
					}
					finally
					{
						base.SyntaxTree.Pop();
					}
					if (Expected(CsLexerToken.Close_brace))
					{
						syntaxNode.AddAttribute(new SyntaxAttribute(prevPosition, SyntaxConsts.DefinitionScopeEnd, null));
					}
					else
					{
						result = false;
					}
					break;
				case CsLexerToken.Semicolon:
					MoveNext();
					break;
				default:
					SyntaxError();
					result = false;
					break;
				}
				syntaxNode.Range.EndPoint = prevPosition;
				if (!AfterDeclaration(syntaxNode))
				{
					result = false;
				}
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
		return result;
	}

	protected virtual bool ParsePropertyDeclaration(ISyntaxAttributes attrs, string type, Point typePos, string name)
	{
		ISyntaxNode syntaxNode = new SyntaxNode((attrs != null && attrs.Count > 0) ? attrs[0].Position : typePos, name, 21, SyntaxNodeOptions.CodeCompletion);
		if (attrs != null)
		{
			syntaxNode.AddAttributes(attrs);
		}
		syntaxNode.AddAttribute(new SyntaxAttribute(typePos, NetNodeType.Type.ToString(), type));
		AddNode(syntaxNode);
		return ParsePropertyDeclaration(syntaxNode);
	}

	protected virtual bool ParsePropertyDeclaration(ISyntaxNode node)
	{
		bool result = true;
		if (!BeforeDeclaration(node))
		{
			result = false;
		}
		if (base.Token == 89)
		{
			node.Options |= SyntaxNodeOptions.Indentation | SyntaxNodeOptions.Outlining;
			node.AddAttribute(new SyntaxAttribute(base.TokenPosition, SyntaxConsts.DefinitionScope, null));
			base.SyntaxTree.Push(node);
			try
			{
				MoveNext();
				while (!base.Eof)
				{
					if (base.Token == 91 && !ParseAttributeListDeclaration())
					{
						result = false;
					}
					ISyntaxAttributes attrs = null;
					if (IsAccessorModifier(base.Token))
					{
						ParseAccessorModifiers(ref attrs);
					}
					if (IsAccessor(NetNodeType.PropertyAccessor))
					{
						if (!ParseAccessors(attrs, NetNodeType.PropertyAccessor))
						{
							result = false;
						}
						continue;
					}
					break;
				}
			}
			finally
			{
				base.SyntaxTree.Pop();
			}
			if (Expected(CsLexerToken.Close_brace))
			{
				node.AddAttribute(new SyntaxAttribute(prevPosition, SyntaxConsts.DefinitionScopeEnd, null));
			}
			else
			{
				result = false;
			}
		}
		else
		{
			SyntaxError(89);
			result = false;
		}
		node.Range.EndPoint = prevPosition;
		if (!AfterDeclaration(node))
		{
			result = false;
		}
		return result;
	}

	protected virtual bool ParseIndexerDeclaration(ISyntaxAttributes attrs, string type, Point typePos)
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode((attrs != null && attrs.Count > 0) ? attrs[0].Position : typePos, base.TokenString, 25, SyntaxNodeOptions.CodeCompletion);
		if (attrs != null)
		{
			syntaxNode.AddAttributes(attrs);
		}
		syntaxNode.AddAttribute(new SyntaxAttribute(typePos, NetNodeType.Type.ToString(), type));
		AddNode(syntaxNode);
		MoveNext();
		if (base.Token == 91)
		{
			base.SyntaxTree.Push(syntaxNode);
			try
			{
				if (!ParseParameterListDeclaration(91, 92))
				{
					result = false;
				}
			}
			finally
			{
				base.SyntaxTree.Pop();
			}
		}
		syntaxNode.AddAttribute(new SyntaxAttribute(prevPosition, SyntaxConsts.DeclarationScope, null));
		if (base.Token == 89 && !ParsePropertyDeclaration(syntaxNode))
		{
			result = false;
		}
		syntaxNode.Range.EndPoint = prevPosition;
		return result;
	}

	protected virtual bool IsAccessor(NetNodeType accessorType)
	{
		if (base.Token != 87 && (base.Token != 88 || accessorType != NetNodeType.PropertyAccessor))
		{
			if (base.Token != 3)
			{
				if (base.Token == 56)
				{
					return accessorType == NetNodeType.EventAccessor;
				}
				return false;
			}
			return true;
		}
		return true;
	}

	protected virtual bool ParseAccessors(ISyntaxAttributes attrs, NetNodeType accessorType)
	{
		bool result = true;
		if (IsAccessor(accessorType))
		{
			ISyntaxNode syntaxNode = new SyntaxNode((attrs != null && attrs.Count > 0) ? attrs[0].Position : base.TokenPosition, base.TokenString, 35);
			AddNode(syntaxNode);
			if (attrs != null)
			{
				syntaxNode.AddAttributes(attrs);
			}
			syntaxNode.AddAttribute(new SyntaxAttribute(base.CurrentPosition, SyntaxConsts.DeclarationScope, null));
			base.SyntaxTree.Push(syntaxNode);
			try
			{
				MoveNext();
				if (!ParseAccessorBody())
				{
					result = false;
				}
			}
			finally
			{
				base.SyntaxTree.Pop();
			}
			syntaxNode.Range.EndPoint = prevPosition;
		}
		else
		{
			SyntaxError();
			result = false;
		}
		return result;
	}

	protected virtual bool ParseConstructorDeclaration(ISyntaxAttributes attrs, string type, Point typePos)
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode((attrs != null && attrs.Count > 0) ? attrs[0].Position : typePos, type, 18);
		if (attrs != null)
		{
			syntaxNode.AddAttributes(attrs);
		}
		AddNode(syntaxNode);
		if (!BeforeDeclaration(syntaxNode))
		{
			result = false;
		}
		base.SyntaxTree.Push(syntaxNode);
		try
		{
			if (!ParseParameterListDeclaration())
			{
				result = false;
			}
			if (base.Token == 97)
			{
				MoveNext();
				if (!ParseConstructorInitializer())
				{
					result = false;
				}
			}
			syntaxNode.AddAttribute(new SyntaxAttribute(prevPosition, SyntaxConsts.DeclarationScope, null));
			if (!ParseMethodBody())
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

	protected virtual bool ParseConstructorInitializer()
	{
		bool result = true;
		CsLexerToken csLexerToken = (CsLexerToken)base.Token;
		if (csLexerToken == CsLexerToken.Base || csLexerToken == CsLexerToken.This)
		{
			MoveNext();
			ISyntaxNode node = null;
			if (!ParseArgumentList(ref node))
			{
				result = false;
			}
			if (node != null)
			{
				AddNode(node);
			}
		}
		else
		{
			SyntaxError();
			result = false;
		}
		return result;
	}

	protected virtual bool ParseDestructorDeclaration(ISyntaxAttributes attrs)
	{
		bool result = true;
		Point tokenPosition = base.TokenPosition;
		MoveNext();
		if (ParseQualifiedIdentifier(out var identifier))
		{
			ISyntaxNode syntaxNode = new SyntaxNode((attrs != null && attrs.Count > 0) ? attrs[0].Position : tokenPosition, "~" + identifier, 19);
			AddNode(syntaxNode);
			if (attrs != null)
			{
				syntaxNode.AddAttributes(attrs);
			}
			result = ParseMethodDeclaration(syntaxNode);
		}
		return result;
	}

	protected virtual bool ParseFieldDeclaration(ISyntaxAttributes attrs, string type, Point typePos, string name)
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode((attrs != null && attrs.Count > 0) ? attrs[0].Position : typePos, name, 13, SyntaxNodeOptions.Indentation | SyntaxNodeOptions.CodeCompletion);
		syntaxNode.AddAttribute(new SyntaxAttribute(typePos, NetNodeType.Type.ToString(), type));
		if (attrs != null)
		{
			syntaxNode.AddAttributes(attrs);
		}
		AddNode(syntaxNode);
		if (!BeforeDeclaration(syntaxNode))
		{
			result = false;
		}
		base.SyntaxTree.Push(syntaxNode);
		try
		{
			if (!ParseOptionalExressionInBracket())
			{
				result = false;
			}
			if (!ParseVariableDeclarators(NetNodeType.Field))
			{
				result = false;
			}
		}
		finally
		{
			base.SyntaxTree.Pop();
		}
		if (!Expected(CsLexerToken.Semicolon))
		{
			result = false;
		}
		if (!AfterDeclaration(syntaxNode))
		{
			result = false;
		}
		syntaxNode.Range.EndPoint = prevPosition;
		return result;
	}

	protected virtual bool ParseVariableDeclaration(Point position, NetNodeType nodeType)
	{
		bool result = true;
		Point tokenPosition = base.TokenPosition;
		if (ParseType(out var type) && ParseIdentifier(out var identifier))
		{
			ISyntaxNode syntaxNode = new SyntaxNode(position, identifier, (int)nodeType, SyntaxNodeOptions.Indentation | SyntaxNodeOptions.CodeCompletion);
			syntaxNode.AddAttribute(new SyntaxAttribute(tokenPosition, NetNodeType.Type.ToString(), type));
			AddNode(syntaxNode);
			base.SyntaxTree.Push(syntaxNode);
			try
			{
				if (!ParseOptionalExressionInBracket())
				{
					result = false;
				}
				if (!ParseVariableDeclarators(nodeType))
				{
					result = false;
				}
			}
			finally
			{
				base.SyntaxTree.Pop();
			}
			syntaxNode.Range.EndPoint = prevPosition;
		}
		else
		{
			result = false;
		}
		return result;
	}

	protected virtual bool ParseVariableDeclarators(NetNodeType nodeType)
	{
		bool result = true;
		if (base.Token == 104)
		{
			MoveNext();
			if (!ParseVariableInitializer())
			{
				result = false;
			}
		}
		while (base.Token == 96)
		{
			MoveNext();
			Point tokenPosition = base.TokenPosition;
			if (ParseIdentifier(out var identifier))
			{
				ISyntaxNode node = new SyntaxNode(tokenPosition, identifier, (int)nodeType, SyntaxNodeOptions.CodeCompletion);
				AddNode(node);
				if (!ParseOptionalExressionInBracket())
				{
					result = false;
				}
			}
		}
		return result;
	}

	protected virtual bool ParseArrayInitializerExpression(ref ISyntaxNode node)
	{
		bool result = true;
		node = new SyntaxNode(base.TokenPosition, base.TokenString, 151);
		base.SyntaxTree.Push(node);
		try
		{
			MoveNext();
			while (!base.Eof && base.Token != 90)
			{
				result = ParseVariableInitializer();
				if (base.Token == 96)
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
		if (!Expected(CsLexerToken.Close_brace))
		{
			result = false;
		}
		node.Range.EndPoint = prevPosition;
		return result;
	}

	protected virtual bool ParseVariableInitializer()
	{
		bool result = true;
		switch ((CsLexerToken)base.Token)
		{
		case CsLexerToken.Open_brace:
		{
			ISyntaxNode node2 = null;
			result = ParseArrayInitializerExpression(ref node2);
			if (node2 != null)
			{
				AddNode(node2);
			}
			break;
		}
		case CsLexerToken.Stackalloc:
		{
			MoveNext();
			if (!ParseType(out var _))
			{
				break;
			}
			if (Expected(CsLexerToken.Open_bracket))
			{
				if (!ParseOptionalExressionInBracket())
				{
					result = false;
				}
			}
			else
			{
				result = false;
			}
			break;
		}
		default:
		{
			ISyntaxNode node = null;
			result = ParseExpression(ref node);
			if (node != null)
			{
				AddNode(node);
			}
			break;
		}
		}
		return result;
	}

	protected virtual bool ParseMethodDeclaration(ISyntaxAttributes attrs, string type, Point typePos, string name)
	{
		ISyntaxNode syntaxNode = new SyntaxNode((attrs != null && attrs.Count > 0) ? attrs[0].Position : typePos, name, 17, SyntaxNodeOptions.CodeCompletion);
		if (attrs != null)
		{
			syntaxNode.AddAttributes(attrs);
		}
		syntaxNode.AddAttribute(new SyntaxAttribute(typePos, NetNodeType.Type.ToString(), type));
		AddNode(syntaxNode);
		return ParseMethodDeclaration(syntaxNode);
	}

	protected virtual bool ParseMethodDeclaration(ISyntaxNode node)
	{
		bool result = true;
		if (!BeforeDeclaration(node))
		{
			result = false;
		}
		base.SyntaxTree.Push(node);
		try
		{
			if (!ParseParameterListDeclaration())
			{
				result = false;
			}
			if (IsTypeParameterConstraintsClause(base.Token) && !ParseTypeParameterConstraintsClauses())
			{
				result = false;
			}
			node.AddAttribute(new SyntaxAttribute(prevPosition, SyntaxConsts.DeclarationScope, null));
			if (!ParseMethodBody())
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

	protected virtual bool ParseMethodBody()
	{
		if (!SkipTo(89, 99))
		{
			return false;
		}
		bool result = true;
		ISyntaxNode current = base.SyntaxTree.Current;
		switch ((CsLexerToken)base.Token)
		{
		case CsLexerToken.Open_brace:
			current.Options |= SyntaxNodeOptions.Indentation | SyntaxNodeOptions.Outlining;
			if (base.Token == 89 && !ParseBlockStatement())
			{
				result = false;
			}
			current.Range.EndPoint = prevPosition;
			break;
		case CsLexerToken.Semicolon:
			current.Range.EndPoint = base.CurrentPosition;
			MoveNext();
			break;
		default:
			SyntaxError();
			result = false;
			break;
		}
		return result;
	}

	protected virtual bool ParseAccessorBody()
	{
		return ParseMethodBody();
	}

	protected virtual bool ParseOperatorDeclaration(ISyntaxAttributes attrs)
	{
		string tokenString = base.TokenString;
		Point tokenPosition = base.TokenPosition;
		MoveNext();
		return ParseOperatorDeclaration(null, tokenString, tokenPosition, isExplicit: true);
	}

	protected virtual bool ParseOperatorDeclaration(ISyntaxAttributes attrs, string type, Point typePos, bool isExplicit)
	{
		bool flag = true;
		ISyntaxNode syntaxNode = new SyntaxNode((attrs != null && attrs.Count > 0) ? attrs[0].Position : typePos, base.TokenString, 23);
		AddNode(syntaxNode);
		if (attrs != null)
		{
			syntaxNode.AddAttributes(attrs);
		}
		syntaxNode.AddAttribute(new SyntaxAttribute(typePos, NetNodeType.Type.ToString(), type));
		if (!BeforeDeclaration(syntaxNode))
		{
			flag = false;
		}
		base.SyntaxTree.Push(syntaxNode);
		try
		{
			MoveNext();
			if (isExplicit)
			{
				typePos = base.TokenPosition;
				if (ParseType(out type))
				{
					syntaxNode.AddAttribute(new SyntaxAttribute(typePos, NetNodeType.Operator.ToString(), type));
				}
				else
				{
					flag = false;
				}
			}
			else
			{
				switch ((CsLexerToken)base.Token)
				{
				case CsLexerToken.False:
				case CsLexerToken.True:
				case CsLexerToken.Tilde:
				case CsLexerToken.Plus:
				case CsLexerToken.Minus:
				case CsLexerToken.Bang:
				case CsLexerToken.Op_lt:
				case CsLexerToken.Op_gt:
				case CsLexerToken.Bitwise_and:
				case CsLexerToken.Bitwise_or:
				case CsLexerToken.Star:
				case CsLexerToken.Percent:
				case CsLexerToken.Div:
				case CsLexerToken.Carret:
				case CsLexerToken.Op_inc:
				case CsLexerToken.Op_dec:
				case CsLexerToken.Op_shift_left:
				case CsLexerToken.Op_shift_right:
				case CsLexerToken.Op_le:
				case CsLexerToken.Op_ge:
				case CsLexerToken.Op_eq:
				case CsLexerToken.Op_ne:
					syntaxNode.AddAttribute(new SyntaxAttribute(base.TokenPosition, NetNodeType.Operator.ToString(), base.TokenString));
					MoveNext();
					break;
				default:
					flag = false;
					SyntaxError();
					break;
				}
			}
			if (flag)
			{
				if (Expected(CsLexerToken.Open_parens))
				{
					if (!ParseOperatorType())
					{
						flag = false;
					}
					if (!Expected(CsLexerToken.Close_parens))
					{
						flag = false;
					}
				}
				syntaxNode.AddAttribute(new SyntaxAttribute(prevPosition, SyntaxConsts.DeclarationScope, null));
				if (!ParseMethodBody())
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

	protected virtual bool ParseOperatorType()
	{
		Point tokenPosition = base.TokenPosition;
		bool result = true;
		while (!base.Eof)
		{
			if (ParseType(out var type))
			{
				Point tokenPosition2 = base.TokenPosition;
				if (ParseIdentifier(out var identifier))
				{
					ISyntaxNode syntaxNode = new SyntaxNode(tokenPosition2, identifier, 24);
					AddNode(syntaxNode);
					syntaxNode.AddAttribute(new SyntaxAttribute(tokenPosition, NetNodeType.Type.ToString(), type));
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
			if (base.Token != 96)
			{
				break;
			}
			MoveNext();
		}
		return result;
	}

	protected override bool ParseUnitBody()
	{
		return ParseClassBody();
	}

	protected virtual bool ParseAliasDeclaration(ISyntaxAttributes attrs)
	{
		bool result = true;
		Point tokenPosition = base.TokenPosition;
		if (ParseIdentifier(out var identifier))
		{
			ISyntaxNode syntaxNode = new SyntaxNode(tokenPosition, identifier, 5);
			AddNode(syntaxNode);
			if (Expected(CsLexerToken.Semicolon))
			{
				result = false;
			}
			syntaxNode.Range.EndPoint = prevPosition;
		}
		else
		{
			result = false;
		}
		return result;
	}

	protected virtual bool ParseAliasList(ISyntaxAttributes attrs)
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode((attrs != null && attrs.Count > 0) ? attrs[0].Position : base.TokenPosition, string.Empty, 6, SyntaxNodeOptions.Outlining);
		AddNode(syntaxNode);
		if (!BeforeDeclaration(syntaxNode))
		{
			result = false;
		}
		base.SyntaxTree.Push(syntaxNode);
		try
		{
			if (attrs == null)
			{
				attrs = new SyntaxAttributes();
			}
			while (base.Token == 86)
			{
				MoveNext();
				if (!ParseAliasDeclaration(attrs))
				{
					result = false;
				}
				attrs.Clear();
				if (base.Token == 25)
				{
					attrs.Add(new SyntaxAttribute(base.TokenPosition, NetNodeType.Modifier.ToString(), base.TokenString));
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
		if (!AfterDeclaration(syntaxNode))
		{
			result = false;
		}
		syntaxNode.Range.EndPoint = prevPosition;
		return result;
	}

	protected virtual bool IsTypeParameterConstraintsClause(int token)
	{
		return token == 83;
	}

	protected virtual bool ParseTypeParameterConstraintsClauses()
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, string.Empty, 53);
		AddNode(syntaxNode);
		base.SyntaxTree.Push(syntaxNode);
		try
		{
			while (IsTypeParameterConstraintsClause(base.Token))
			{
				if (!ParseTypeParameterConstraintsClause())
				{
					result = false;
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

	protected virtual bool ParseTypeParameterConstraintsClause()
	{
		bool result = Expected(CsLexerToken.Where);
		Point tokenPosition = base.TokenPosition;
		if (ParseIdentifier(out var identifier))
		{
			ISyntaxNode syntaxNode = new SyntaxNode(tokenPosition, identifier, 54);
			AddNode(syntaxNode);
			if (Expected(CsLexerToken.Colon))
			{
				base.SyntaxTree.Push(syntaxNode);
				try
				{
					if (!ParseTypeParameterConstraints())
					{
						result = false;
					}
				}
				finally
				{
					base.SyntaxTree.Pop();
				}
			}
			else
			{
				result = false;
			}
			syntaxNode.Range.EndPoint = prevPosition;
		}
		else
		{
			result = false;
		}
		return result;
	}

	protected virtual bool ParseTypeParameterConstraint()
	{
		bool flag = true;
		Point tokenPosition = base.TokenPosition;
		string type = string.Empty;
		switch ((CsLexerToken)base.Token)
		{
		case CsLexerToken.Class:
		case CsLexerToken.Struct:
			type = base.TokenString;
			MoveNext();
			break;
		case CsLexerToken.New:
			type = base.TokenString;
			MoveNext();
			if (!Expected(CsLexerToken.Open_parens) || !Expected(CsLexerToken.Close_parens))
			{
				flag = false;
			}
			break;
		default:
			if (!ParseTypeName(out type))
			{
				flag = false;
			}
			break;
		}
		if (flag)
		{
			ISyntaxNode syntaxNode = new SyntaxNode(tokenPosition, type, 52);
			AddNode(syntaxNode);
			syntaxNode.Range.EndPoint = prevPosition;
		}
		return flag;
	}

	protected virtual bool ParseTypeParameterConstraints()
	{
		bool result = ParseTypeParameterConstraint();
		while (base.Token == 96)
		{
			MoveNext();
			if (ParseTypeParameterConstraint())
			{
				result = false;
			}
		}
		return result;
	}

	protected virtual bool IsTypeParameterList(int token)
	{
		return token == 105;
	}

	protected virtual bool ParseTypeParameterList()
	{
		bool result = true;
		if (Expected(CsLexerToken.Op_lt))
		{
			ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, string.Empty, 51);
			AddNode(syntaxNode);
			base.SyntaxTree.Push(syntaxNode);
			try
			{
				result = ParseTypeParameter();
				while (base.Token == 96)
				{
					MoveNext();
					if (!ParseTypeParameter())
					{
						result = false;
					}
				}
			}
			finally
			{
				base.SyntaxTree.Pop();
			}
			if (base.Token == 118)
			{
				tokenPos++;
				base.Token = 106;
			}
			else if (!Expected(CsLexerToken.Op_gt))
			{
				result = false;
			}
			syntaxNode.Range.EndPoint = prevPosition;
		}
		return result;
	}

	protected virtual bool ParseTypeParameter()
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, string.Empty, 50);
		AddNode(syntaxNode);
		if (base.Token == 91)
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
		if (ParseIdentifier(out var identifier))
		{
			syntaxNode.Name = identifier;
		}
		else
		{
			result = false;
		}
		return result;
	}

	protected virtual bool IsNullableType(int token)
	{
		return token == 113;
	}

	protected virtual bool ParseNullableType(out string type)
	{
		type = base.TokenString;
		MoveNext();
		return true;
	}

	protected virtual bool IsPointerType(int token)
	{
		return token == 109;
	}

	protected virtual bool ParsePointerType(out string type)
	{
		type = base.TokenString;
		MoveNext();
		return true;
	}

	protected virtual bool IsTypeArgumentList(int token)
	{
		return token == 105;
	}

	protected virtual bool ParseTypeArgumentList(out string typeList)
	{
		typeList = string.Empty;
		bool flag = Expected(CsLexerToken.Op_lt);
		if (flag)
		{
			typeList = "<";
			flag = ParseType(out var type);
			if (flag)
			{
				typeList += type;
			}
			while (base.Token == 96)
			{
				MoveNext();
				if (ParseType(out type))
				{
					typeList = typeList + "," + type;
				}
			}
			if (base.Token == 118)
			{
				tokenPos++;
				base.Token = 106;
				typeList += ">";
			}
			else if (Expected(CsLexerToken.Op_gt))
			{
				typeList += ">";
			}
			else
			{
				flag = false;
			}
		}
		return flag;
	}

	protected virtual bool IsAccessorModifier(int token)
	{
		if (token == 38 || (uint)(token - 50) <= 1u)
		{
			return true;
		}
		return false;
	}

	protected virtual bool ParseAccessorModifiers(ref ISyntaxAttributes attrs)
	{
		bool result = false;
		while (IsAccessorModifier(base.Token))
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

	protected virtual bool ParseYieldStatement()
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString, 104);
		AddNode(syntaxNode);
		base.SyntaxTree.Push(syntaxNode);
		try
		{
			MoveNext();
			result = (CsLexerToken)base.Token switch
			{
				CsLexerToken.Break => ParseBreakStatement(), 
				CsLexerToken.Return => ParseReturnStatement(), 
				_ => false, 
			};
		}
		finally
		{
			base.SyntaxTree.Pop();
		}
		syntaxNode.Range.EndPoint = prevPosition;
		return result;
	}

	protected virtual bool ParseParenthesizedStatementExpression(ref ISyntaxNode node)
	{
		if (base.Token == 93)
		{
			return ParseParenthesizedExpression(ref node);
		}
		Expected(CsLexerToken.Open_parens);
		return ParseExpression(ref node);
	}

	protected virtual bool ParseDefaultExpression(ref ISyntaxNode node)
	{
		return ParseTypeofExpression(NetNodeType.DefaultExpression, ref node);
	}

	protected virtual bool TryParseGenericExpression(ref ISyntaxNode node, out bool generic)
	{
		bool result = true;
		generic = false;
		node = null;
		if (node != null)
		{
			NetNodeType nodeType = (NetNodeType)node.NodeType;
			if (nodeType != NetNodeType.PrimaryExpression && nodeType != NetNodeType.MemberAccessExpression)
			{
				return result;
			}
			string typeList = string.Empty;
			Point tokenPosition = base.TokenPosition;
			SaveState();
			try
			{
				if (ParseTypeArgumentList(out typeList))
				{
					switch ((CsLexerToken)base.Token)
					{
					case CsLexerToken.Close_brace:
					case CsLexerToken.Open_parens:
					case CsLexerToken.Close_parens:
					case CsLexerToken.Dot:
					case CsLexerToken.Colon:
					case CsLexerToken.Semicolon:
					case CsLexerToken.Interr:
					case CsLexerToken.Op_eq:
					case CsLexerToken.Op_ne:
						generic = true;
						break;
					}
				}
			}
			finally
			{
				RestoreState(!generic);
			}
			if (generic)
			{
				node = CreateParentNode(tokenPosition, typeList, 166, node);
				node.Range.EndPoint = prevPosition;
			}
			return result;
		}
		return result;
	}

	protected virtual bool ParseAnonymousMethodExpression(ref ISyntaxNode node)
	{
		bool result = true;
		node = new SyntaxNode(base.TokenPosition, base.TokenString, 167);
		base.SyntaxTree.Push(node);
		try
		{
			MoveNext();
			if (base.Token == 93 && !ParseParameterListDeclaration())
			{
				result = false;
			}
			node.AddAttribute(new SyntaxAttribute(prevPosition, SyntaxConsts.DeclarationScope, null));
			if (!ParseMethodBody())
			{
				result = false;
			}
		}
		finally
		{
			base.SyntaxTree.Pop();
		}
		node.Range.EndPoint = prevPosition;
		return result;
	}

	protected virtual bool ParseNullCoalescingExpression(ref ISyntaxNode node)
	{
		bool result = ParseConditionalOrExpression(ref node);
		if (base.Token == 114)
		{
			node = CreateParentNode(base.TokenPosition, base.TokenString, 168, node);
			base.SyntaxTree.Push(node);
			try
			{
				MoveNext();
				ISyntaxNode node2 = null;
				if (!ParseNullCoalescingExpression(ref node2))
				{
					result = false;
				}
				if (node2 != null)
				{
					node.AddChild(node2);
				}
			}
			finally
			{
				base.SyntaxTree.Pop();
			}
			node.Range.EndPoint = prevPosition;
		}
		return result;
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

	protected virtual bool ParseArgumentExpression(ref ISyntaxNode node)
	{
		bool flag = true;
		node = new SyntaxNode(base.TokenPosition, string.Empty, 28);
		ISyntaxNode node2 = null;
		flag = ParseExpression(ref node2);
		if (node2 != null)
		{
			node.AddChild(node2);
		}
		node.Range.EndPoint = prevPosition;
		return flag;
	}

	protected virtual bool ParseArgumentExpressionList(ref ISyntaxNode node)
	{
		bool result = true;
		node = new SyntaxNode(base.TokenPosition, string.Empty, 29);
		MoveNext();
		if (base.Token != 92)
		{
			ISyntaxNode node2 = null;
			result = base.Token == 96 || ParseArgumentExpression(ref node2);
			if (node2 != null)
			{
				node.AddChild(node2);
			}
			while (base.Token == 96)
			{
				MoveNext();
				node2 = null;
				if (base.Token != 96 && !ParseArgumentExpression(ref node2))
				{
					result = false;
				}
				if (node2 != null)
				{
					node.AddChild(node2);
				}
			}
		}
		node.Range.EndPoint = prevPosition;
		return result;
	}

	protected virtual bool ParseExpression(ref ISyntaxNode node)
	{
		bool result = ParseConditionalExpression(ref node);
		if (!TryParseAssignmentExpression(ref node))
		{
			result = false;
		}
		return result;
	}

	protected virtual bool TryParseAssignmentExpression(ref ISyntaxNode node)
	{
		bool result = true;
		CsLexerToken csLexerToken = (CsLexerToken)base.Token;
		if (csLexerToken == CsLexerToken.Assign || (uint)(csLexerToken - 125) <= 9u)
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

	protected virtual bool ParseConditionalExpression(ref ISyntaxNode node)
	{
		bool result = ParseNullCoalescingExpression(ref node);
		if (base.Token == 113)
		{
			node = CreateParentNode(base.TokenPosition, base.TokenString, 128, node);
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
			if (Expected(CsLexerToken.Colon))
			{
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
			else
			{
				result = false;
			}
			node.Range.EndPoint = prevPosition;
		}
		return result;
	}

	protected virtual bool ParseConditionalOrExpression(ref ISyntaxNode node)
	{
		bool result = ParseConditionalAndExpression(ref node);
		if (base.Token == 124)
		{
			node = CreateParentNode(base.TokenPosition, base.TokenString, 128, node);
			MoveNext();
			ISyntaxNode node2 = null;
			if (!ParseConditionalOrExpression(ref node2))
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

	protected virtual bool ParseConditionalAndExpression(ref ISyntaxNode node)
	{
		bool result = ParseInclusiveOrExpression(ref node);
		if (base.Token == 123)
		{
			node = CreateParentNode(base.TokenPosition, base.TokenString, 131, node);
			MoveNext();
			ISyntaxNode node2 = null;
			if (!ParseConditionalAndExpression(ref node2))
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

	protected virtual bool ParseInclusiveOrExpression(ref ISyntaxNode node)
	{
		bool result = ParseExclusiveOrExpression(ref node);
		if (base.Token == 108)
		{
			node = CreateParentNode(base.TokenPosition, base.TokenString, 132, node);
			MoveNext();
			ISyntaxNode node2 = null;
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
		if (base.Token == 112)
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
		if (base.Token == 107)
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
		CsLexerToken csLexerToken = (CsLexerToken)base.Token;
		if ((uint)(csLexerToken - 121) <= 1u)
		{
			node = CreateParentNode(base.TokenPosition, base.TokenString, 136, node);
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
		}
		return result;
	}

	protected virtual bool ParseRelationalExpression(ref ISyntaxNode node)
	{
		bool result = ParseShiftExpression(ref node);
		switch ((CsLexerToken)base.Token)
		{
		case CsLexerToken.Op_lt:
		case CsLexerToken.Op_gt:
		case CsLexerToken.Op_le:
		case CsLexerToken.Op_ge:
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
			break;
		}
		case CsLexerToken.As:
		case CsLexerToken.Is:
			node = CreateParentNode(base.TokenPosition, base.TokenString, 138, node);
			MoveNext();
			if (!ParseAsIsType(node))
			{
				prevPosition = base.TokenPosition;
				result = false;
			}
			node.Range.EndPoint = prevPosition;
			break;
		}
		return result;
	}

	protected virtual bool ParseAsIsType(ISyntaxNode node)
	{
		bool flag = true;
		Point tokenPosition = base.TokenPosition;
		flag = ParseType(out var type);
		if (type != string.Empty)
		{
			node.AddAttribute(new SyntaxAttribute(tokenPosition, NetNodeType.Type.ToString(), type));
		}
		return flag;
	}

	protected virtual bool ParseShiftExpression(ref ISyntaxNode node)
	{
		bool result = ParseAdditiveExpression(ref node);
		CsLexerToken csLexerToken = (CsLexerToken)base.Token;
		if ((uint)(csLexerToken - 117) <= 1u)
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
		CsLexerToken csLexerToken = (CsLexerToken)base.Token;
		if ((uint)(csLexerToken - 101) <= 1u)
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
		CsLexerToken csLexerToken = (CsLexerToken)base.Token;
		if ((uint)(csLexerToken - 109) <= 2u)
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
		CsLexerToken csLexerToken = (CsLexerToken)base.Token;
		if ((uint)(csLexerToken - 100) <= 3u || csLexerToken == CsLexerToken.Star || (uint)(csLexerToken - 115) <= 1u)
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
			result = ParseUnaryExpression(ref node);
		}
		return result;
	}

	protected virtual bool ParseUnaryExpression(ref ISyntaxNode node)
	{
		bool flag = false;
		if (base.Token == 93 && TryParseCastExpression(ref node))
		{
			flag = true;
		}
		if (!flag)
		{
			flag = ParsePrimaryExpression(ref node);
		}
		return flag;
	}

	protected virtual bool ParseCastTargetExpression(ref ISyntaxNode node)
	{
		bool result = true;
		node = CreateParentNode(base.TokenPosition, string.Empty, 147, node);
		ISyntaxNode node2 = null;
		if (!ParseUnaryExpression(ref node2))
		{
			result = false;
		}
		if (node2 != null)
		{
			node.AddChild(node2);
		}
		node.Range.EndPoint = prevPosition;
		return result;
	}

	protected virtual bool TryParseCastExpression(ref ISyntaxNode node)
	{
		bool flag = true;
		Point tokenPosition = base.TokenPosition;
		string type = string.Empty;
		Point point = tokenPosition;
		SaveState();
		try
		{
			MoveNext();
			point = base.TokenPosition;
			if (ParseType(out type))
			{
				flag = base.Token == 94;
				if (flag)
				{
					ISyntaxNode syntaxNode = node;
					node = CreateParentNode(tokenPosition, type, 144, node);
					node.AddAttribute(new SyntaxAttribute(point, NetNodeType.Type.ToString(), type));
					MoveNext();
					ISyntaxNode node2 = null;
					flag = ParseCastTargetExpression(ref node2);
					if (node2 != null)
					{
						node.AddChild(node2);
					}
					node.Range.EndPoint = prevPosition;
					if (!flag)
					{
						if (syntaxNode != null && node.HasChildren)
						{
							node.ChildList.Remove(syntaxNode);
						}
						node = syntaxNode;
					}
				}
			}
			else
			{
				flag = false;
			}
		}
		finally
		{
			RestoreState(!flag);
		}
		return flag;
	}

	protected virtual bool ParseConstantExpression(ref ISyntaxNode node)
	{
		return ParseExpression(ref node);
	}

	protected virtual bool ParseSimpleExpression(ref ISyntaxNode node)
	{
		bool result = true;
		switch ((CsLexerToken)base.Token)
		{
		case CsLexerToken.False:
		case CsLexerToken.Null:
		case CsLexerToken.True:
		case CsLexerToken.Value:
		case CsLexerToken.Integer_Literal:
		case CsLexerToken.Float_Literal:
		case CsLexerToken.Double_Literal:
		case CsLexerToken.Decimal_Literal:
		case CsLexerToken.Character_Literal:
		case CsLexerToken.String_Literal:
		case CsLexerToken.Identifier_Literal:
			node = new SyntaxNode(base.TokenPosition, base.TokenString, 148);
			MoveNext();
			break;
		case CsLexerToken.Bool:
		case CsLexerToken.Byte:
		case CsLexerToken.Char:
		case CsLexerToken.Decimal:
		case CsLexerToken.Double:
		case CsLexerToken.Float:
		case CsLexerToken.Int:
		case CsLexerToken.Long:
		case CsLexerToken.Object:
		case CsLexerToken.Sbyte:
		case CsLexerToken.Short:
		case CsLexerToken.String:
		case CsLexerToken.Uint:
		case CsLexerToken.Ulong:
		case CsLexerToken.Ushort:
		case CsLexerToken.Void:
			node = new SyntaxNode(base.TokenPosition, base.TokenString, 148);
			MoveNext();
			if (base.Token != 95)
			{
				result = false;
			}
			break;
		case CsLexerToken.Open_parens:
			result = ParseParenthesizedExpression(ref node);
			break;
		case CsLexerToken.Delegate:
			result = ParseAnonymousMethodExpression(ref node);
			break;
		case CsLexerToken.This:
			result = ParseThisAccess(ref node);
			break;
		case CsLexerToken.Base:
			result = ParseBaseAccess(ref node);
			break;
		case CsLexerToken.New:
			result = ParseNewExpression(ref node);
			break;
		case CsLexerToken.Typeof:
			result = ParseTypeofExpression(ref node);
			break;
		case CsLexerToken.Sizeof:
			result = ParseSizeofExpression(ref node);
			break;
		case CsLexerToken.Checked:
			result = ParseCheckedExpression(ref node);
			break;
		case CsLexerToken.Unchecked:
			result = ParseUncheckedExpression(ref node);
			break;
		case CsLexerToken.Default:
			result = ParseDefaultExpression(ref node);
			break;
		default:
			result = false;
			SyntaxError();
			MoveNext();
			break;
		}
		return result;
	}

	protected virtual bool ParsePrimaryExpression(ref ISyntaxNode node)
	{
		bool flag = true;
		flag = ParseSimpleExpression(ref node);
		if (node != null && !TryParsePostPrimaryExpression(ref node))
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
			switch ((CsLexerToken)base.Token)
			{
			case CsLexerToken.Op_lt:
			{
				if (!TryParseGenericExpression(ref node, out var generic))
				{
					result = false;
				}
				if (!generic)
				{
					return result;
				}
				break;
			}
			case CsLexerToken.Dot:
				if (!ParseMemberAccess(ref node))
				{
					result = false;
				}
				break;
			case CsLexerToken.Open_parens:
				if (!ParseInvocationExpression(ref node))
				{
					result = false;
				}
				break;
			case CsLexerToken.Open_bracket:
				if (!ParseElementAccess(ref node))
				{
					result = false;
				}
				break;
			case CsLexerToken.Op_ptr:
				if (!ParsePointerMemberAccess(ref node))
				{
					result = false;
				}
				break;
			case CsLexerToken.Op_inc:
				if (!ParsePostIncrementExpression(ref node))
				{
					result = false;
				}
				break;
			case CsLexerToken.Op_dec:
				if (!ParsePostDecrementExpression(ref node))
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

	protected virtual bool ParseParenthesizedExpression(ref ISyntaxNode node)
	{
		bool flag = true;
		if (base.Token == 93)
		{
			node = new SyntaxNode(base.TokenPosition, base.TokenString, 162);
			MoveNext();
			ISyntaxNode node2 = null;
			flag = ParseExpression(ref node2);
			if (node2 != null)
			{
				node.AddChild(node2);
			}
			if (!Expected(CsLexerToken.Close_parens))
			{
				flag = false;
			}
			node.Range.EndPoint = prevPosition;
		}
		else
		{
			SyntaxError(93);
			flag = false;
		}
		return flag;
	}

	protected virtual bool ParseThisAccess(ref ISyntaxNode node)
	{
		node = new SyntaxNode(base.TokenPosition, base.TokenString, 153);
		MoveNext();
		return true;
	}

	protected virtual bool ParseBaseAccess(ref ISyntaxNode node)
	{
		bool result = true;
		node = new SyntaxNode(base.TokenPosition, base.TokenString, 152);
		MoveNext();
		switch ((CsLexerToken)base.Token)
		{
		case CsLexerToken.Open_bracket:
		{
			ISyntaxNode node2 = null;
			result = ParseArgumentExpressionList(ref node2);
			if (node2 != null)
			{
				node.AddChild(node2);
			}
			if (!Expected(CsLexerToken.Close_bracket))
			{
				result = false;
			}
			break;
		}
		default:
			SyntaxError();
			result = false;
			break;
		case CsLexerToken.Dot:
			break;
		}
		node.Range.EndPoint = prevPosition;
		return result;
	}

	protected virtual bool ParseNewExpression(ref ISyntaxNode node)
	{
		node = new SyntaxNode(base.TokenPosition, base.TokenString, 150);
		MoveNext();
		Point tokenPosition = base.TokenPosition;
		bool flag = ParseTypeName(out var type);
		if (type != string.Empty)
		{
			node.AddAttribute(new SyntaxAttribute(tokenPosition, NetNodeType.Type.ToString(), type));
			switch ((CsLexerToken)base.Token)
			{
			case CsLexerToken.Open_parens:
			{
				ISyntaxNode node3 = null;
				flag = ParseArgumentList(ref node3);
				if (node3 != null)
				{
					node.AddChild(node3);
				}
				break;
			}
			case CsLexerToken.Open_bracket:
			{
				node.NodeType = 149;
				ISyntaxNode node2 = null;
				flag = ParseArgumentExpressionList(ref node2);
				if (node2 != null)
				{
					node.AddChild(node2);
				}
				if (!Expected(CsLexerToken.Close_bracket))
				{
					flag = false;
				}
				if (!TryParseRankSpecifiers(out var _))
				{
					flag = false;
				}
				if (base.Token == 89)
				{
					node2 = null;
					if (!ParseArrayInitializerExpression(ref node2))
					{
						flag = false;
					}
					if (node2 != null)
					{
						node.AddChild(node2);
					}
				}
				break;
			}
			default:
				flag = false;
				SyntaxError(StringConsts.ErrNewExpression);
				break;
			}
		}
		else
		{
			prevPosition = base.TokenPosition;
			flag = false;
		}
		node.Range.EndPoint = prevPosition;
		return flag;
	}

	protected virtual bool ParseTypeofExpression(NetNodeType nodeType, ref ISyntaxNode node)
	{
		bool result = true;
		node = new SyntaxNode(base.TokenPosition, base.TokenString, (int)nodeType);
		MoveNext();
		if (Expected(CsLexerToken.Open_parens))
		{
			Point tokenPosition = base.TokenPosition;
			if (ParseType(out var type))
			{
				node.AddAttribute(new SyntaxAttribute(tokenPosition, NetNodeType.Type.ToString(), type));
				if (!Expected(CsLexerToken.Close_parens))
				{
					result = false;
				}
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
		node.Range.EndPoint = prevPosition;
		return result;
	}

	protected virtual bool ParseTypeofExpression(ref ISyntaxNode node)
	{
		return ParseTypeofExpression(NetNodeType.TypeofExpression, ref node);
	}

	protected virtual bool ParseSizeofExpression(ref ISyntaxNode node)
	{
		return ParseTypeofExpression(NetNodeType.SizeofExpression, ref node);
	}

	protected virtual bool ParseCheckedExpression(NetNodeType nodeType, ref ISyntaxNode node)
	{
		bool result = true;
		node = new SyntaxNode(base.TokenPosition, base.TokenString, (int)nodeType);
		MoveNext();
		if (base.Token == 93)
		{
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
			if (!Expected(CsLexerToken.Close_parens))
			{
				result = false;
			}
		}
		else
		{
			SyntaxError(93);
			result = false;
		}
		node.Range.EndPoint = prevPosition;
		return result;
	}

	protected virtual bool ParseCheckedExpression(ref ISyntaxNode node)
	{
		return ParseCheckedExpression(NetNodeType.CheckedExpression, ref node);
	}

	protected virtual bool ParseUncheckedExpression(ref ISyntaxNode node)
	{
		return ParseCheckedExpression(NetNodeType.UncheckedExpression, ref node);
	}

	protected virtual bool ParsePostIncrementExpression(ref ISyntaxNode node)
	{
		node = CreateParentNode(base.TokenPosition, base.TokenString, 163, node);
		MoveNext();
		return true;
	}

	protected virtual bool ParsePostDecrementExpression(ref ISyntaxNode node)
	{
		node = CreateParentNode(base.TokenPosition, base.TokenString, 164, node);
		MoveNext();
		return true;
	}

	protected virtual bool ParseMemberAccess(ref ISyntaxNode node, int nodeType)
	{
		bool result = true;
		node = CreateParentNode(base.TokenPosition, string.Empty, nodeType, node);
		Point currentPosition = base.CurrentPosition;
		MoveNext();
		if (ParseIdentifier(out var identifier))
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

	protected virtual bool ParsePointerMemberAccess(ref ISyntaxNode node)
	{
		return ParseMemberAccess(ref node, 155);
	}

	protected virtual bool ParseInvocationExpression(ref ISyntaxNode node)
	{
		bool flag = true;
		node = CreateParentNode(base.TokenPosition, base.TokenString, 157, node);
		ISyntaxNode node2 = null;
		flag = ParseArgumentList(ref node2);
		if (node2 != null)
		{
			node.AddChild(node2);
		}
		node.Range.EndPoint = prevPosition;
		return flag;
	}

	protected virtual bool ParseElementAccess(ref ISyntaxNode node)
	{
		bool flag = true;
		node = CreateParentNode(base.TokenPosition, base.TokenString, 156, node);
		ISyntaxNode node2 = null;
		flag = ParseArgumentExpressionList(ref node2);
		if (node2 != null)
		{
			node.AddChild(node2);
		}
		if (!Expected(CsLexerToken.Close_bracket))
		{
			flag = false;
		}
		node.Range.EndPoint = prevPosition;
		return flag;
	}

	protected virtual bool ParseOptionalExressionInBracket()
	{
		bool result = true;
		if (base.Token == 91)
		{
			MoveNext();
			if (base.Token != 92)
			{
				ISyntaxNode node = null;
				if (!ParseExpression(ref node))
				{
					result = false;
				}
				if (node != null)
				{
					AddNode(node);
				}
			}
			if (!Expected(CsLexerToken.Close_bracket))
			{
				result = false;
			}
		}
		return result;
	}

	protected virtual bool ParseVariableReference(ref ISyntaxNode node)
	{
		return ParseExpression(ref node);
	}

	protected virtual bool ParseArgument(ref ISyntaxNode node)
	{
		bool flag = true;
		node = new SyntaxNode(base.TokenPosition, string.Empty, 28);
		ISyntaxNode node2 = null;
		switch ((CsLexerToken)base.Token)
		{
		case CsLexerToken.Out:
		case CsLexerToken.Ref:
			MoveNext();
			flag = ParseVariableReference(ref node2);
			break;
		case CsLexerToken.ArgList:
			MoveNext();
			if (base.Token == 93)
			{
				flag = ParseArgumentList(ref node2);
			}
			break;
		default:
			SaveState();
			try
			{
				flag = ParseExpression(ref node2);
				if (!flag)
				{
					node2 = null;
				}
				savePrevPosition = prevPosition;
			}
			finally
			{
				RestoreState(!flag);
			}
			break;
		}
		if (node2 != null)
		{
			node.AddChild(node2);
		}
		node.Range.EndPoint = prevPosition;
		return flag;
	}

	protected virtual bool ParseArgumentList(ref ISyntaxNode node)
	{
		bool result = true;
		node = new SyntaxNode(base.TokenPosition, base.TokenString, 29);
		if (base.Token == 93)
		{
			MoveNext();
			while (!base.Eof && base.Token != 94)
			{
				ISyntaxNode node2 = null;
				if (!ParseArgument(ref node2))
				{
					result = false;
				}
				if (node2 != null)
				{
					node.AddChild(node2);
				}
				if (base.Token != 96)
				{
					break;
				}
				MoveNext();
			}
			if (!Expected(CsLexerToken.Close_parens))
			{
				result = false;
			}
			node.Range.EndPoint = prevPosition;
		}
		else
		{
			SyntaxError(93);
			result = false;
		}
		return result;
	}

	protected override bool ParseBlock()
	{
		bool result = true;
		Point tokenPosition = base.TokenPosition;
		if (Expected(CsLexerToken.Open_brace))
		{
			AddAttribute(new SyntaxAttribute(tokenPosition, SyntaxConsts.BlockScope, null));
			AddAttribute(new SyntaxAttribute(tokenPosition, SyntaxConsts.DefinitionScope, null));
			if (base.Token != 90 && !ParseStatementList())
			{
				result = false;
			}
			if (Expected(CsLexerToken.Close_brace))
			{
				AddAttribute(new SyntaxAttribute(prevPosition, SyntaxConsts.DefinitionScopeEnd, null));
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
		return result;
	}

	protected virtual bool ParseStatementList()
	{
		bool result = true;
		while (!base.Eof && base.Token != 90)
		{
			if (!ParseStatement())
			{
				result = false;
			}
		}
		return result;
	}

	protected virtual bool ParseStatement()
	{
		bool result = true;
		if (base.Token == 146)
		{
			if (!ParseDirective())
			{
				result = false;
			}
		}
		else if (base.Token == 14)
		{
			result = ParseLocalConstantDeclarationStatement();
		}
		else if (IsBuiltInType(base.Token))
		{
			result = ParseVariableDeclaration(base.TokenPosition, NetNodeType.LocalVariable);
			if (!Expected(CsLexerToken.Semicolon))
			{
				result = false;
			}
		}
		else
		{
			bool parsed = false;
			if (IsIdentifierToken(base.Token))
			{
				result = TryParseLabeledStatement(out parsed);
				if (!parsed)
				{
					result = TryParseVariableDeclarationStatement(out parsed);
				}
			}
			if (!parsed)
			{
				result = ParseEmbeddedStatement();
			}
		}
		return result;
	}

	protected virtual bool TryParseLabeledStatement(out bool parsed)
	{
		Point tokenPosition = base.TokenPosition;
		parsed = false;
		SaveState();
		string identifier;
		try
		{
			parsed = ParseIdentifier(out identifier) && Expected(CsLexerToken.Colon);
		}
		finally
		{
			RestoreState(!parsed);
		}
		bool result = parsed;
		if (parsed)
		{
			ISyntaxNode syntaxNode = new SyntaxNode(tokenPosition, identifier, 101, SyntaxNodeOptions.Indentation);
			AddNode(syntaxNode);
			base.SyntaxTree.Push(syntaxNode);
			try
			{
				result = ParseStatement();
			}
			finally
			{
				base.SyntaxTree.Pop();
			}
			syntaxNode.Range.EndPoint = prevPosition;
		}
		return result;
	}

	protected virtual bool TryParseVariableDeclarationStatement(out bool parsed)
	{
		bool flag = true;
		parsed = false;
		SaveState();
		try
		{
			parsed = ParseVariableDeclaration(base.TokenPosition, NetNodeType.LocalVariable);
		}
		finally
		{
			RestoreState(!parsed);
		}
		flag = parsed;
		if (flag && !Expected(CsLexerToken.Semicolon))
		{
			flag = false;
		}
		return flag;
	}

	protected virtual bool ParseEmbeddedStatement()
	{
		return (CsLexerToken)base.Token switch
		{
			CsLexerToken.If => ParseIfStatement(), 
			CsLexerToken.Switch => ParseSwitchStatement(), 
			CsLexerToken.While => ParseWhileStatement(), 
			CsLexerToken.Do => ParseDoStatement(), 
			CsLexerToken.For => ParseForStatement(), 
			CsLexerToken.Foreach => ParseForeachStatement(), 
			CsLexerToken.Break => ParseBreakStatement(), 
			CsLexerToken.Continue => ParseContinueStatement(), 
			CsLexerToken.Goto => ParseGotoStatement(), 
			CsLexerToken.Return => ParseReturnStatement(), 
			CsLexerToken.Throw => ParseThrowStatement(), 
			CsLexerToken.Try => ParseTryStatement(), 
			CsLexerToken.Checked => ParseCheckedStatement(), 
			CsLexerToken.Unchecked => ParseUncheckedStatement(), 
			CsLexerToken.Unsafe => ParseUnsafeStatement(), 
			CsLexerToken.Lock => ParseLockStatement(), 
			CsLexerToken.Using => ParseUsingStatement(), 
			CsLexerToken.Fixed => ParseFixedStatement(), 
			CsLexerToken.Yield => ParseYieldStatement(), 
			CsLexerToken.Open_brace => ParseBlockStatement(), 
			_ => ParseExpressionStatement(), 
		};
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

	protected virtual bool ParseExpressionStatement()
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, string.Empty, 103, SyntaxNodeOptions.Indentation);
		AddNode(syntaxNode);
		if (base.Token == 99)
		{
			syntaxNode.Name = base.TokenString;
			syntaxNode.NodeType = 125;
			MoveNext();
		}
		else
		{
			base.SyntaxTree.Push(syntaxNode);
			try
			{
				result = ParseStatementExpression();
				if (!Expected(CsLexerToken.Semicolon))
				{
					result = false;
				}
			}
			finally
			{
				base.SyntaxTree.Pop();
			}
		}
		syntaxNode.Range.EndPoint = prevPosition;
		return result;
	}

	protected virtual bool ParseStatementExpressionList()
	{
		bool flag = true;
		ISyntaxNode node = new SyntaxNode(base.TokenPosition, string.Empty, 126);
		AddNode(node);
		base.SyntaxTree.Push(node);
		try
		{
			flag = ParseStatementExpression();
			while (base.Token == 96)
			{
				MoveNext();
				if (!ParseStatementExpression())
				{
					flag = false;
				}
			}
			return flag;
		}
		finally
		{
			base.SyntaxTree.Pop();
		}
	}

	protected virtual bool ParseLocalConstantDeclarationStatement()
	{
		Point tokenPosition = base.TokenPosition;
		MoveNext();
		return ParseVariableDeclaration(tokenPosition, NetNodeType.Constant);
	}

	protected virtual bool ParseIfStatement()
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString, 63, (base.SyntaxTree.Current.NodeType != 64) ? SyntaxNodeOptions.Indentation : (SyntaxNodeOptions.Indentation | SyntaxNodeOptions.BackIndentation));
		AddNode(syntaxNode);
		base.SyntaxTree.Push(syntaxNode);
		try
		{
			MoveNext();
			ISyntaxNode node = null;
			if (!ParseParenthesizedStatementExpression(ref node))
			{
				result = false;
			}
			if (node != null)
			{
				syntaxNode.AddChild(node);
			}
			if (!ParseEmbeddedStatement())
			{
				result = false;
			}
			if (base.Token == 21 && !ParseElseStatement())
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

	protected virtual bool ParseElseStatement()
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString, 64, (base.SyntaxTree.Current.NodeType == 63) ? SyntaxNodeOptions.BackIndentation : SyntaxNodeOptions.None);
		AddNode(syntaxNode);
		base.SyntaxTree.Push(syntaxNode);
		try
		{
			MoveNext();
			if (!ParseEmbeddedStatement())
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

	protected virtual bool ParseSwitchStatement()
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString, 65, SyntaxNodeOptions.Indentation);
		AddNode(syntaxNode);
		base.SyntaxTree.Push(syntaxNode);
		try
		{
			MoveNext();
			ISyntaxNode node = null;
			if (!ParseParenthesizedStatementExpression(ref node))
			{
				result = false;
			}
			if (node != null)
			{
				syntaxNode.AddChild(node);
			}
			if (!ParseSwitchBlock())
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

	protected virtual bool ParseSwitchBlock()
	{
		bool result = true;
		Point tokenPosition = base.TokenPosition;
		if (Expected(CsLexerToken.Open_brace))
		{
			AddAttribute(new SyntaxAttribute(tokenPosition, SyntaxConsts.DefinitionScope, null));
			while (!base.Eof && (base.Token == 9 || base.Token == 17))
			{
				if (!ParseSwitchSection())
				{
					result = false;
				}
			}
			if (Expected(CsLexerToken.Close_brace))
			{
				AddAttribute(new SyntaxAttribute(prevPosition, SyntaxConsts.DefinitionScopeEnd, null));
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
		return result;
	}

	protected virtual bool ParseSwitchSection()
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString, 66, SyntaxNodeOptions.Indentation);
		AddNode(syntaxNode);
		base.SyntaxTree.Push(syntaxNode);
		try
		{
			result = ParseSwitchLabels();
			if (base.Token == 89)
			{
				if (!ParseBlock())
				{
					result = false;
				}
			}
			else
			{
				while (!base.Eof && base.Token != 9 && base.Token != 17 && base.Token != 90)
				{
					if (!ParseStatement())
					{
						result = false;
					}
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

	protected virtual bool ParseSwitchLabels()
	{
		bool result = true;
		do
		{
			if (!ParseSwitchLabel())
			{
				result = false;
			}
		}
		while (base.Token == 9 || base.Token == 17);
		return result;
	}

	protected virtual bool ParseSwitchLabel()
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString, 67, SyntaxNodeOptions.BackIndentation);
		AddNode(syntaxNode);
		switch ((CsLexerToken)base.Token)
		{
		case CsLexerToken.Case:
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
			break;
		}
		case CsLexerToken.Default:
			MoveNext();
			break;
		}
		if (!Expected(CsLexerToken.Colon))
		{
			result = false;
		}
		syntaxNode.Range.EndPoint = prevPosition;
		return result;
	}

	protected virtual bool ParseWhileStatement()
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString, 68, SyntaxNodeOptions.Indentation);
		AddNode(syntaxNode);
		base.SyntaxTree.Push(syntaxNode);
		try
		{
			MoveNext();
			ISyntaxNode node = null;
			if (!ParseParenthesizedStatementExpression(ref node))
			{
				result = false;
			}
			if (node != null)
			{
				syntaxNode.AddChild(node);
			}
			if (!ParseEmbeddedStatement())
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

	protected virtual bool ParseDoStatement()
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString, 69, SyntaxNodeOptions.Indentation);
		AddNode(syntaxNode);
		base.SyntaxTree.Push(syntaxNode);
		try
		{
			MoveNext();
			if (!ParseEmbeddedStatement())
			{
				result = false;
			}
			if (!ParseDoWhileStatement())
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

	protected virtual bool ParseDoWhileStatement()
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString, 70, SyntaxNodeOptions.BackIndentation);
		AddNode(syntaxNode);
		MoveNext();
		if (base.Token == 93)
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
			if (!Expected(CsLexerToken.Close_parens) || !Expected(CsLexerToken.Semicolon))
			{
				result = false;
			}
		}
		else
		{
			SyntaxError(93);
			result = false;
		}
		syntaxNode.Range.EndPoint = prevPosition;
		return result;
	}

	protected virtual bool ParseForStatement()
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString, 71, SyntaxNodeOptions.Indentation);
		AddNode(syntaxNode);
		base.SyntaxTree.Push(syntaxNode);
		try
		{
			MoveNext();
			if (Expected(CsLexerToken.Open_parens))
			{
				if (base.Token != 99 && !ParseForInitializer())
				{
					result = false;
				}
				if (Expected(CsLexerToken.Semicolon))
				{
					if (base.Token != 99 && !ParseForCondition())
					{
						result = false;
					}
				}
				else
				{
					result = false;
				}
				if (Expected(CsLexerToken.Semicolon))
				{
					if (base.Token != 94 && !ParseForIterator())
					{
						result = false;
					}
				}
				else
				{
					result = false;
				}
				if (!Expected(CsLexerToken.Close_parens))
				{
					result = false;
				}
			}
			else
			{
				result = false;
			}
			if (!ParseEmbeddedStatement())
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

	protected virtual bool ParseForInitializer()
	{
		bool flag = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString, 73);
		AddNode(syntaxNode);
		base.SyntaxTree.Push(syntaxNode);
		try
		{
			SaveState();
			try
			{
				flag = ParseVariableDeclaration(base.TokenPosition, NetNodeType.LocalVariable);
			}
			finally
			{
				RestoreState(!flag);
			}
			if (!flag)
			{
				flag = ParseStatementExpressionList();
			}
		}
		finally
		{
			base.SyntaxTree.Pop();
		}
		syntaxNode.Range.EndPoint = prevPosition;
		return flag;
	}

	protected virtual bool ParseForCondition()
	{
		bool flag = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString, 72);
		AddNode(syntaxNode);
		ISyntaxNode node = null;
		flag = ParseExpression(ref node);
		if (node != null)
		{
			syntaxNode.AddChild(node);
		}
		syntaxNode.Range.EndPoint = prevPosition;
		return flag;
	}

	protected virtual bool ParseForIterator()
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString, 74);
		AddNode(syntaxNode);
		base.SyntaxTree.Push(syntaxNode);
		try
		{
			result = ParseStatementExpressionList();
		}
		finally
		{
			base.SyntaxTree.Pop();
		}
		syntaxNode.Range.EndPoint = prevPosition;
		return result;
	}

	protected virtual bool ParseForeachStatement()
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString, 75, SyntaxNodeOptions.Indentation);
		AddNode(syntaxNode);
		base.SyntaxTree.Push(syntaxNode);
		try
		{
			MoveNext();
			if (Expected(CsLexerToken.Open_parens))
			{
				if (!ParseForeachInitializer())
				{
					result = false;
				}
				if (!Expected(CsLexerToken.Close_parens))
				{
					result = false;
				}
			}
			if (!ParseEmbeddedStatement())
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

	protected virtual bool ParseForeachInitializer()
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, string.Empty, 76);
		AddNode(syntaxNode);
		base.SyntaxTree.Push(syntaxNode);
		try
		{
			if (!ParseVariableDeclaration(base.TokenPosition, NetNodeType.LocalVariable))
			{
				result = false;
			}
			if (Expected(CsLexerToken.In))
			{
				ISyntaxNode node = null;
				result = ParseExpression(ref node);
				if (node != null)
				{
					syntaxNode.AddChild(node);
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

	protected virtual bool ParseBreakStatement()
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString, 87);
		AddNode(syntaxNode);
		MoveNext();
		if (!Expected(CsLexerToken.Semicolon))
		{
			result = false;
		}
		syntaxNode.Range.EndPoint = prevPosition;
		return result;
	}

	protected virtual bool ParseContinueStatement()
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString, 88);
		AddNode(syntaxNode);
		MoveNext();
		if (!Expected(CsLexerToken.Semicolon))
		{
			result = false;
		}
		syntaxNode.Range.EndPoint = prevPosition;
		return result;
	}

	protected virtual bool ParseGotoStatement()
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString, 89);
		AddNode(syntaxNode);
		MoveNext();
		switch ((CsLexerToken)base.Token)
		{
		case CsLexerToken.Default:
		case CsLexerToken.Value:
		case CsLexerToken.Identifier_Literal:
			MoveNext();
			break;
		case CsLexerToken.Case:
		{
			base.SyntaxTree.Push(syntaxNode);
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
			break;
		}
		default:
			result = false;
			break;
		}
		if (!Expected(CsLexerToken.Semicolon))
		{
			result = false;
		}
		syntaxNode.Range.EndPoint = prevPosition;
		return result;
	}

	protected virtual bool ParseReturnStatement(NetNodeType nodeType)
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString, (int)nodeType, SyntaxNodeOptions.Indentation);
		AddNode(syntaxNode);
		MoveNext();
		if (base.Token != 99)
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
		if (!Expected(CsLexerToken.Semicolon))
		{
			result = false;
		}
		syntaxNode.Range.EndPoint = prevPosition;
		return result;
	}

	protected virtual bool ParseReturnStatement()
	{
		return ParseReturnStatement(NetNodeType.ReturnStatement);
	}

	protected virtual bool ParseThrowStatement()
	{
		return ParseReturnStatement(NetNodeType.ThrowStatement);
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
			if (!ParseBlock())
			{
				result = false;
			}
			switch ((CsLexerToken)base.Token)
			{
			case CsLexerToken.Catch:
				if (!ParseCatchStatements())
				{
					result = false;
				}
				break;
			case CsLexerToken.Finally:
				if (!ParseFinallyStatement())
				{
					result = false;
				}
				break;
			default:
				result = false;
				SyntaxError(StringConsts.ErrCatchOrFinallyExpected);
				break;
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
			if (base.Token == 93)
			{
				MoveNext();
				if (!ParseType(out var _))
				{
					result = false;
				}
				if (IsIdentifierToken(base.Token))
				{
					MoveNext();
				}
				if (!Expected(CsLexerToken.Close_parens))
				{
					result = false;
				}
			}
			if (!ParseBlock())
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

	protected virtual bool ParseCatchStatements()
	{
		bool result = true;
		while (base.Token == 10)
		{
			result = ParseCatchStatement();
		}
		if (base.Token == 27 && !ParseFinallyStatement())
		{
			result = false;
		}
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
			result = ParseBlock();
		}
		finally
		{
			base.SyntaxTree.Pop();
		}
		syntaxNode.Range.EndPoint = prevPosition;
		return result;
	}

	protected virtual bool ParseCheckedStatement(NetNodeType nodeType)
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString, (int)nodeType, SyntaxNodeOptions.Indentation);
		AddNode(syntaxNode);
		base.SyntaxTree.Push(syntaxNode);
		try
		{
			MoveNext();
			if (!ParseBlock())
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

	protected virtual bool ParseCheckedStatement()
	{
		return ParseCheckedStatement(NetNodeType.CheckedStatement);
	}

	protected virtual bool ParseUncheckedStatement()
	{
		return ParseCheckedStatement(NetNodeType.UncheckedStatement);
	}

	protected virtual bool ParseUnsafeStatement()
	{
		return ParseCheckedStatement(NetNodeType.UnsafeStatement);
	}

	protected virtual bool ParseLockStatement()
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString, 98, SyntaxNodeOptions.Indentation);
		AddNode(syntaxNode);
		base.SyntaxTree.Push(syntaxNode);
		try
		{
			MoveNext();
			ISyntaxNode node = null;
			if (!ParseParenthesizedStatementExpression(ref node))
			{
				result = false;
			}
			if (node != null)
			{
				syntaxNode.AddChild(node);
			}
			if (!ParseEmbeddedStatement())
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

	protected virtual bool ParseUsingStatement()
	{
		bool flag = true;
		ISyntaxNode current = base.SyntaxTree.Current;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString, 99, (current.NodeType != 99 || current.FindAttribute(SyntaxConsts.DefinitionScope) != null) ? SyntaxNodeOptions.Indentation : SyntaxNodeOptions.BackIndentation);
		AddNode(syntaxNode);
		base.SyntaxTree.Push(syntaxNode);
		try
		{
			MoveNext();
			if (Expected(CsLexerToken.Open_parens))
			{
				SaveState();
				try
				{
					flag = ParseVariableDeclaration(base.TokenPosition, NetNodeType.LocalVariable);
				}
				finally
				{
					RestoreState(!flag);
				}
				if (!flag)
				{
					ISyntaxNode node = null;
					flag = ParseExpression(ref node);
					if (node != null)
					{
						syntaxNode.AddChild(node);
					}
				}
				if (!Expected(CsLexerToken.Close_parens))
				{
					flag = false;
				}
			}
			else
			{
				flag = false;
			}
			if (!ParseEmbeddedStatement())
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

	protected virtual bool ParseFixedStatement()
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString, 100, SyntaxNodeOptions.Indentation);
		AddNode(syntaxNode);
		base.SyntaxTree.Push(syntaxNode);
		try
		{
			MoveNext();
			if (Expected(CsLexerToken.Open_parens))
			{
				if (!ParseVariableDeclaration(base.TokenPosition, NetNodeType.FixedVariable))
				{
					result = false;
				}
				if (!Expected(CsLexerToken.Close_parens))
				{
					result = false;
				}
			}
			if (!ParseEmbeddedStatement())
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

	protected virtual bool ParseBlockStatement()
	{
		return ParseBlockStatement(null, SyntaxNodeOptions.None);
	}

	protected virtual bool ParseBlockStatement(ISyntaxAttributes attrs, SyntaxNodeOptions options)
	{
		bool result = false;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, string.Empty, 102, options);
		AddNode(syntaxNode);
		if (attrs != null)
		{
			syntaxNode.AddAttributes(attrs);
		}
		base.SyntaxTree.Push(syntaxNode);
		try
		{
			result = ParseBlock();
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
		return result;
	}

	public override ICodeCompletionRepository CreateRepository()
	{
		IReflectionRepository reflectionRepository = base.CreateRepository() as IReflectionRepository;
		reflectionRepository.RegisterType("void", typeof(void));
		reflectionRepository.RegisterType("bool", typeof(bool));
		reflectionRepository.RegisterType("byte", typeof(byte));
		reflectionRepository.RegisterType("char", typeof(char));
		reflectionRepository.RegisterType("decimal", typeof(decimal));
		reflectionRepository.RegisterType("double", typeof(double));
		reflectionRepository.RegisterType("float", typeof(float));
		reflectionRepository.RegisterType("int", typeof(int));
		reflectionRepository.RegisterType("long", typeof(long));
		reflectionRepository.RegisterType("object", typeof(object));
		reflectionRepository.RegisterType("sbyte", typeof(sbyte));
		reflectionRepository.RegisterType("short", typeof(short));
		reflectionRepository.RegisterType("string", typeof(string));
		reflectionRepository.RegisterType("uint", typeof(uint));
		reflectionRepository.RegisterType("ulong", typeof(ulong));
		reflectionRepository.RegisterType("ushort", typeof(ushort));
		reflectionRepository.RegisterSnippet("as", isStatement: false);
		reflectionRepository.RegisterSnippet("base", isStatement: false);
		reflectionRepository.RegisterSnippet("break", isStatement: false);
		reflectionRepository.RegisterSnippet("const", isStatement: false);
		reflectionRepository.RegisterSnippet("case", isStatement: false);
		reflectionRepository.RegisterSnippet("catch", isStatement: false);
		reflectionRepository.RegisterSnippet("continue", isStatement: false);
		reflectionRepository.RegisterSnippet("default", isStatement: false);
		reflectionRepository.RegisterSnippet("delegate", isStatement: false);
		reflectionRepository.RegisterSnippet("false", isStatement: false);
		reflectionRepository.RegisterSnippet("finally", isStatement: false);
		reflectionRepository.RegisterSnippet("fixed", isStatement: false);
		reflectionRepository.RegisterSnippet("goto", isStatement: false);
		reflectionRepository.RegisterSnippet("in", isStatement: false);
		reflectionRepository.RegisterSnippet("new", isStatement: false);
		reflectionRepository.RegisterSnippet("is", isStatement: false);
		reflectionRepository.RegisterSnippet("null", isStatement: false);
		reflectionRepository.RegisterSnippet("out", isStatement: false);
		reflectionRepository.RegisterSnippet("ref", isStatement: false);
		reflectionRepository.RegisterSnippet("return", isStatement: false);
		reflectionRepository.RegisterSnippet("sizeof", isStatement: false);
		reflectionRepository.RegisterSnippet("stackalloc", isStatement: false);
		reflectionRepository.RegisterSnippet("this", isStatement: false);
		reflectionRepository.RegisterSnippet("throw", isStatement: false);
		reflectionRepository.RegisterSnippet("true", isStatement: false);
		reflectionRepository.RegisterSnippet("typeof", isStatement: false);
		reflectionRepository.RegisterSnippet("virtual", isStatement: false);
		reflectionRepository.RegisterSnippet("void", isStatement: false);
		reflectionRepository.RegisterSnippet("yield", isStatement: false);
		reflectionRepository.RegisterSnippet("checked", isStatement: true);
		reflectionRepository.RegisterSnippet("class", isStatement: true);
		reflectionRepository.RegisterSnippet("do", isStatement: true);
		reflectionRepository.RegisterSnippet("else", isStatement: true);
		reflectionRepository.RegisterSnippet("enum", isStatement: true);
		reflectionRepository.RegisterSnippet("for", isStatement: true);
		reflectionRepository.RegisterSnippet("foreach", isStatement: true);
		reflectionRepository.RegisterSnippet("if", isStatement: true);
		reflectionRepository.RegisterSnippet("interface", isStatement: true);
		reflectionRepository.RegisterSnippet("lock", isStatement: true);
		reflectionRepository.RegisterSnippet("namespace", isStatement: true);
		reflectionRepository.RegisterSnippet("struct", isStatement: true);
		reflectionRepository.RegisterSnippet("switch", isStatement: true);
		reflectionRepository.RegisterSnippet("unchecked", isStatement: true);
		reflectionRepository.RegisterSnippet("unsafe", isStatement: true);
		reflectionRepository.RegisterSnippet("using", isStatement: true);
		reflectionRepository.RegisterSnippet("while", isStatement: true);
		return reflectionRepository;
	}

	public override string GetSingleLineComment()
	{
		return "//";
	}

	protected override IParameterInfo CreateParameterInfo()
	{
		return new CsParameterInfo();
	}

	protected override IListMembers CreateListMembers()
	{
		return new CsListMembers();
	}

	public override CodeCompletionType GetCompletionType(CodeCompletionArgs e)
	{
		if (e.CompletionType == CodeCompletionType.None)
		{
			switch (e.KeyChar)
			{
			case '<':
				return CodeCompletionType.ListMembers;
			case '>':
				return CodeCompletionType.CompleteWord;
			case '[':
				return CodeCompletionType.ParameterInfo;
			case ' ':
				return CodeCompletionType.SpecialListMembers;
			}
		}
		return base.GetCompletionType(e);
	}

	public override void ResetCodeCompletionChars()
	{
		base.CodeCompletionChars = SyntaxConsts.DefaultCsCodeCompletionChars.ToCharArray();
	}

	public override void ResetCodeCompletionStopChars()
	{
		base.CodeCompletionStopChars = SyntaxConsts.DefaultCsCodeCompletionStopChars.ToCharArray();
	}

	public override void ResetAutoIndentChars()
	{
		base.AutoIndentChars = SyntaxConsts.DefaultCsAutoIndentChars.ToCharArray();
	}

	public override void ResetSmartFormatChars()
	{
		base.SmartFormatChars = SyntaxConsts.DefaultCsSmartFormatChars.ToCharArray();
	}

	public override bool ShouldSerializeCodeCompletionChars()
	{
		return !IsEqualArrays(base.CodeCompletionChars, SyntaxConsts.DefaultCsCodeCompletionChars.ToCharArray());
	}

	public override bool ShouldSerializeCodeCompletionStopChars()
	{
		return !IsEqualArrays(base.CodeCompletionStopChars, SyntaxConsts.DefaultCsCodeCompletionStopChars.ToCharArray());
	}

	public override bool ShouldSerializeSmartFormatChars()
	{
		return !IsEqualArrays(base.SmartFormatChars, SyntaxConsts.DefaultCsSmartFormatChars.ToCharArray());
	}

	public override bool ShouldSerializeAutoIndentChars()
	{
		return !IsEqualArrays(base.AutoIndentChars, SyntaxConsts.DefaultCsAutoIndentChars.ToCharArray());
	}
}
