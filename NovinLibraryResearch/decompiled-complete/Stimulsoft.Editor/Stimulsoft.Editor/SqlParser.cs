using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;

namespace Stimulsoft.Editor;

[ToolboxItem(false)]
public class SqlParser : SyntaxParser
{
	private Hashtable reswords;

	private SqlLexerToken[] tokens;

	private const int stateNormal = 0;

	private const int stateComment = 1;

	protected LexerProc lexSymbolProc;

	protected LexerProc lexWhitespaceProc;

	protected LexerProc lexIdentifierProc;

	protected LexerProc lexNumberProc;

	protected LexerProc lexStringProc;

	protected LexerProc lexCommentProc;

	protected LexerProc lexCommentEndProc;

	protected Point prevPosition;

	public override bool CaseSensitive => false;

	protected override void InitStyles()
	{
		AddStyle(StringConsts.IdentsInternalName, Color.Teal);
		AddStyle(StringConsts.NumbersInternalName, Color.Green);
		AddStyle(StringConsts.ReswordsInternalName, Color.Blue);
		AddStyle(StringConsts.CommentsInternalName, Color.Green, (FontStyle)0, plainText: true);
		AddStyle(StringConsts.XMLCommentsInternalName, Color.Gray, (FontStyle)0, plainText: false);
		AddStyle(StringConsts.SymbolsInternalName, Color.Gray);
		AddStyle(StringConsts.WhiteSpaceInternalName, Color.Empty);
		AddStyle(StringConsts.StringsInternalName, Color.Maroon, (FontStyle)0, plainText: true);
		AddStyle(StringConsts.DirectivesInternalName, Color.Navy);
		AddStyle(StringConsts.SyntaxErrorsInternalName, Color.Red);
	}

	protected virtual void InitReswords()
	{
		tokens = new SqlLexerToken[7]
		{
			SqlLexerToken.Table,
			SqlLexerToken.View,
			SqlLexerToken.Sequence,
			SqlLexerToken.Schema,
			SqlLexerToken.User,
			SqlLexerToken.Into,
			SqlLexerToken.From
		};
		reswords = new Hashtable();
		reswords.Add("abort", SqlLexerToken.Abort);
		reswords.Add("accept", SqlLexerToken.Accept);
		reswords.Add("access", SqlLexerToken.Access);
		reswords.Add("account", SqlLexerToken.Account);
		reswords.Add("action", SqlLexerToken.Action);
		reswords.Add("add", SqlLexerToken.Add);
		reswords.Add("all", SqlLexerToken.All);
		reswords.Add("alter", SqlLexerToken.Alter);
		reswords.Add("and", SqlLexerToken.And);
		reswords.Add("any", SqlLexerToken.Any);
		reswords.Add("array", SqlLexerToken.Array);
		reswords.Add("as", SqlLexerToken.As);
		reswords.Add("asc", SqlLexerToken.Asc);
		reswords.Add("assert", SqlLexerToken.Assert);
		reswords.Add("assign", SqlLexerToken.Assign);
		reswords.Add("at", SqlLexerToken.At);
		reswords.Add("audit", SqlLexerToken.Audit);
		reswords.Add("authorization", SqlLexerToken.Authorization);
		reswords.Add("auto", SqlLexerToken.Auto);
		reswords.Add("avg", SqlLexerToken.Avg);
		reswords.Add("begin", SqlLexerToken.Begin);
		reswords.Add("between", SqlLexerToken.Between);
		reswords.Add("body", SqlLexerToken.Body);
		reswords.Add("by", SqlLexerToken.By);
		reswords.Add("case", SqlLexerToken.Case);
		reswords.Add("cascade", SqlLexerToken.Cascade);
		reswords.Add("casche", SqlLexerToken.Cashe);
		reswords.Add("char_base", SqlLexerToken.Char_base);
		reswords.Add("check", SqlLexerToken.Check);
		reswords.Add("close", SqlLexerToken.Close);
		reswords.Add("cluster", SqlLexerToken.Cluster);
		reswords.Add("clusters", SqlLexerToken.Clusters);
		reswords.Add("colauth", SqlLexerToken.Colauth);
		reswords.Add("column", SqlLexerToken.Column);
		reswords.Add("columns", SqlLexerToken.Columns);
		reswords.Add("comment", SqlLexerToken.Comment);
		reswords.Add("commit", SqlLexerToken.Commit);
		reswords.Add("compact", SqlLexerToken.Compact);
		reswords.Add("compress", SqlLexerToken.Compress);
		reswords.Add("connect", SqlLexerToken.Connect);
		reswords.Add("connections", SqlLexerToken.Connections);
		reswords.Add("constant", SqlLexerToken.Constant);
		reswords.Add("constraint", SqlLexerToken.Constraint);
		reswords.Add("count", SqlLexerToken.Count);
		reswords.Add("crash", SqlLexerToken.Crash);
		reswords.Add("create", SqlLexerToken.Create);
		reswords.Add("current", SqlLexerToken.Current);
		reswords.Add("cursor", SqlLexerToken.Cursor);
		reswords.Add("cycle", SqlLexerToken.Cycle);
		reswords.Add("data_base", SqlLexerToken.Data_base);
		reswords.Add("database", SqlLexerToken.Database);
		reswords.Add("dba", SqlLexerToken.Dba);
		reswords.Add("debugoff", SqlLexerToken.Debugoff);
		reswords.Add("debugon", SqlLexerToken.Debugon);
		reswords.Add("declare", SqlLexerToken.Declare);
		reswords.Add("default", SqlLexerToken.Default);
		reswords.Add("defferable", SqlLexerToken.Defferable);
		reswords.Add("deffered", SqlLexerToken.Deffered);
		reswords.Add("definition", SqlLexerToken.Definition);
		reswords.Add("delay", SqlLexerToken.Delay);
		reswords.Add("delete", SqlLexerToken.Delete);
		reswords.Add("delta", SqlLexerToken.Delta);
		reswords.Add("desc", SqlLexerToken.Desc);
		reswords.Add("describe", SqlLexerToken.Describe);
		reswords.Add("digits", SqlLexerToken.Digits);
		reswords.Add("dispose", SqlLexerToken.Dispose);
		reswords.Add("distinct", SqlLexerToken.Distinct);
		reswords.Add("do", SqlLexerToken.Do);
		reswords.Add("drop", SqlLexerToken.Drop);
		reswords.Add("else", SqlLexerToken.Else);
		reswords.Add("elsif", SqlLexerToken.Elsif);
		reswords.Add("end", SqlLexerToken.End);
		reswords.Add("entry", SqlLexerToken.Entry);
		reswords.Add("exception", SqlLexerToken.Exception);
		reswords.Add("exception_init", SqlLexerToken.Exception_init);
		reswords.Add("exclusive", SqlLexerToken.Exclusive);
		reswords.Add("exists", SqlLexerToken.Exists);
		reswords.Add("exit", SqlLexerToken.Exit);
		reswords.Add("false", SqlLexerToken.False);
		reswords.Add("fetch", SqlLexerToken.Fetch);
		reswords.Add("file", SqlLexerToken.File);
		reswords.Add("for", SqlLexerToken.For);
		reswords.Add("foreign", SqlLexerToken.Foreign);
		reswords.Add("form", SqlLexerToken.Form);
		reswords.Add("from", SqlLexerToken.From);
		reswords.Add("function", SqlLexerToken.Function);
		reswords.Add("generic", SqlLexerToken.Generic);
		reswords.Add("goto", SqlLexerToken.Goto);
		reswords.Add("grant", SqlLexerToken.Grant);
		reswords.Add("group", SqlLexerToken.Group);
		reswords.Add("groups", SqlLexerToken.Groups);
		reswords.Add("having", SqlLexerToken.Having);
		reswords.Add("identfied", SqlLexerToken.Identfied);
		reswords.Add("identified", SqlLexerToken.Identified);
		reswords.Add("if", SqlLexerToken.If);
		reswords.Add("immediate", SqlLexerToken.Immediate);
		reswords.Add("in", SqlLexerToken.In);
		reswords.Add("increment", SqlLexerToken.Increment);
		reswords.Add("index", SqlLexerToken.Index);
		reswords.Add("index_blist", SqlLexerToken.Index_Blist);
		reswords.Add("indexes", SqlLexerToken.Indexes);
		reswords.Add("index_none", SqlLexerToken.Index_None);
		reswords.Add("indicator", SqlLexerToken.Indicator);
		reswords.Add("initial", SqlLexerToken.Initial);
		reswords.Add("initially", SqlLexerToken.Initially);
		reswords.Add("inner", SqlLexerToken.Inner);
		reswords.Add("insert", SqlLexerToken.Insert);
		reswords.Add("intersect", SqlLexerToken.Intersect);
		reswords.Add("into", SqlLexerToken.Into);
		reswords.Add("is", SqlLexerToken.Is);
		reswords.Add("isolation", SqlLexerToken.Isolation);
		reswords.Add("join", SqlLexerToken.Join);
		reswords.Add("key", SqlLexerToken.Key);
		reswords.Add("left", SqlLexerToken.Left);
		reswords.Add("level", SqlLexerToken.Level);
		reswords.Add("like", SqlLexerToken.Like);
		reswords.Add("limit", SqlLexerToken.Limit);
		reswords.Add("limited", SqlLexerToken.Limited);
		reswords.Add("lock", SqlLexerToken.Lock);
		reswords.Add("long", SqlLexerToken.Long);
		reswords.Add("loop", SqlLexerToken.Loop);
		reswords.Add("max", SqlLexerToken.Max);
		reswords.Add("maxextents", SqlLexerToken.Maxextents);
		reswords.Add("min", SqlLexerToken.Min);
		reswords.Add("minvalue", SqlLexerToken.Minvalue);
		reswords.Add("minus", SqlLexerToken.Minus);
		reswords.Add("mod", SqlLexerToken.Mod);
		reswords.Add("mode", SqlLexerToken.Mode);
		reswords.Add("modify", SqlLexerToken.Modify);
		reswords.Add("new", SqlLexerToken.New);
		reswords.Add("no", SqlLexerToken.No);
		reswords.Add("noaudit", SqlLexerToken.Noaudit);
		reswords.Add("nocompress", SqlLexerToken.Nocompress);
		reswords.Add("not", SqlLexerToken.Not);
		reswords.Add("nowait", SqlLexerToken.Nowait);
		reswords.Add("null", SqlLexerToken.Null);
		reswords.Add("number", SqlLexerToken.Number);
		reswords.Add("number_base", SqlLexerToken.Number_base);
		reswords.Add("of", SqlLexerToken.Of);
		reswords.Add("off", SqlLexerToken.Off);
		reswords.Add("offline", SqlLexerToken.Offline);
		reswords.Add("on", SqlLexerToken.On);
		reswords.Add("online", SqlLexerToken.Online);
		reswords.Add("open", SqlLexerToken.Open);
		reswords.Add("option", SqlLexerToken.Option);
		reswords.Add("or", SqlLexerToken.Or);
		reswords.Add("order", SqlLexerToken.Order);
		reswords.Add("others", SqlLexerToken.Others);
		reswords.Add("out", SqlLexerToken.Out);
		reswords.Add("outer", SqlLexerToken.Outer);
		reswords.Add("package", SqlLexerToken.Package);
		reswords.Add("partition", SqlLexerToken.Partition);
		reswords.Add("password", SqlLexerToken.Password);
		reswords.Add("pctfree", SqlLexerToken.Pctfree);
		reswords.Add("pragma", SqlLexerToken.Pragma);
		reswords.Add("primary", SqlLexerToken.Primary);
		reswords.Add("prior", SqlLexerToken.Prior);
		reswords.Add("private", SqlLexerToken.Private);
		reswords.Add("privileges", SqlLexerToken.Privileges);
		reswords.Add("procedure", SqlLexerToken.Procedure);
		reswords.Add("public", SqlLexerToken.Public);
		reswords.Add("raise", SqlLexerToken.Raise);
		reswords.Add("range", SqlLexerToken.Range);
		reswords.Add("raw", SqlLexerToken.Raw);
		reswords.Add("record", SqlLexerToken.Record);
		reswords.Add("references", SqlLexerToken.References);
		reswords.Add("release", SqlLexerToken.Release);
		reswords.Add("rem", SqlLexerToken.Rem);
		reswords.Add("rename", SqlLexerToken.Rename);
		reswords.Add("replace", SqlLexerToken.Replace);
		reswords.Add("resource", SqlLexerToken.Resource);
		reswords.Add("return", SqlLexerToken.Return);
		reswords.Add("reverse", SqlLexerToken.Reverse);
		reswords.Add("revoke", SqlLexerToken.Revoke);
		reswords.Add("right", SqlLexerToken.Right);
		reswords.Add("rollback", SqlLexerToken.Rollback);
		reswords.Add("row", SqlLexerToken.Row);
		reswords.Add("rowid", SqlLexerToken.Rowid);
		reswords.Add("rowlabel", SqlLexerToken.Rowlabel);
		reswords.Add("rownum", SqlLexerToken.Rownum);
		reswords.Add("rows", SqlLexerToken.Rows);
		reswords.Add("rowtype", SqlLexerToken.Rowtype);
		reswords.Add("run", SqlLexerToken.Run);
		reswords.Add("savepoint", SqlLexerToken.Savepoint);
		reswords.Add("schema", SqlLexerToken.Schema);
		reswords.Add("select", SqlLexerToken.Select);
		reswords.Add("separate", SqlLexerToken.Separate);
		reswords.Add("sequence", SqlLexerToken.Sequence);
		reswords.Add("serializable", SqlLexerToken.Serializable);
		reswords.Add("session", SqlLexerToken.Session);
		reswords.Add("set", SqlLexerToken.Set);
		reswords.Add("share", SqlLexerToken.Share);
		reswords.Add("show", SqlLexerToken.Show);
		reswords.Add("shutdown", SqlLexerToken.Shutdown);
		reswords.Add("size", SqlLexerToken.Size);
		reswords.Add("space", SqlLexerToken.Space);
		reswords.Add("sql", SqlLexerToken.Sql);
		reswords.Add("sqlcode", SqlLexerToken.Sqlcode);
		reswords.Add("sqlerrm", SqlLexerToken.Sqlerrm);
		reswords.Add("start", SqlLexerToken.Start);
		reswords.Add("statement", SqlLexerToken.Statement);
		reswords.Add("status", SqlLexerToken.Status);
		reswords.Add("stddev", SqlLexerToken.Stddev);
		reswords.Add("subtype", SqlLexerToken.Subtype);
		reswords.Add("successful", SqlLexerToken.Successful);
		reswords.Add("sum", SqlLexerToken.Sum);
		reswords.Add("synonym", SqlLexerToken.Synonym);
		reswords.Add("sysdate", SqlLexerToken.Sysdate);
		reswords.Add("tabauth", SqlLexerToken.Tabauth);
		reswords.Add("table", SqlLexerToken.Table);
		reswords.Add("tables", SqlLexerToken.Tables);
		reswords.Add("task", SqlLexerToken.Task);
		reswords.Add("terminate", SqlLexerToken.Terminate);
		reswords.Add("then", SqlLexerToken.Then);
		reswords.Add("to", SqlLexerToken.To);
		reswords.Add("transaction", SqlLexerToken.Transaction);
		reswords.Add("trigger", SqlLexerToken.Trigger);
		reswords.Add("true", SqlLexerToken.True);
		reswords.Add("type", SqlLexerToken.Type);
		reswords.Add("uid", SqlLexerToken.Uid);
		reswords.Add("union", SqlLexerToken.Union);
		reswords.Add("unique", SqlLexerToken.Unique);
		reswords.Add("unlock", SqlLexerToken.Unlock);
		reswords.Add("update", SqlLexerToken.Update);
		reswords.Add("usage", SqlLexerToken.Usage);
		reswords.Add("use", SqlLexerToken.Use);
		reswords.Add("user", SqlLexerToken.User);
		reswords.Add("validate", SqlLexerToken.Validate);
		reswords.Add("values", SqlLexerToken.Values);
		reswords.Add("varchar2", SqlLexerToken.Varchar2);
		reswords.Add("variance", SqlLexerToken.Variance);
		reswords.Add("view", SqlLexerToken.View);
		reswords.Add("views", SqlLexerToken.Views);
		reswords.Add("when", SqlLexerToken.When);
		reswords.Add("whenever", SqlLexerToken.Whenever);
		reswords.Add("where", SqlLexerToken.Where);
		reswords.Add("while", SqlLexerToken.While);
		reswords.Add("with", SqlLexerToken.With);
		reswords.Add("work", SqlLexerToken.Work);
		reswords.Add("xor", SqlLexerToken.Xor);
		reswords.Add("bit", SqlLexerToken.Bit);
		reswords.Add("real", SqlLexerToken.Real);
		reswords.Add("char", SqlLexerToken.Char);
		reswords.Add("text", SqlLexerToken.Text);
		reswords.Add("date", SqlLexerToken.Date);
		reswords.Add("time", SqlLexerToken.Time);
		reswords.Add("interval", SqlLexerToken.Interval);
		reswords.Add("float", SqlLexerToken.Float);
		reswords.Add("bigint", SqlLexerToken.Bigint);
		reswords.Add("double", SqlLexerToken.Double);
		reswords.Add("string", SqlLexerToken.String);
		reswords.Add("binary", SqlLexerToken.Binary);
		reswords.Add("numeric", SqlLexerToken.Numeric);
		reswords.Add("decimal", SqlLexerToken.Decimal);
		reswords.Add("boolean", SqlLexerToken.Boolean);
		reswords.Add("tinyint", SqlLexerToken.Tinyint);
		reswords.Add("integer", SqlLexerToken.Integer);
		reswords.Add("varchar", SqlLexerToken.Varchar);
		reswords.Add("smallint", SqlLexerToken.Smallint);
		reswords.Add("varbinary", SqlLexerToken.Varbinary);
		reswords.Add("timestamp", SqlLexerToken.Timestamp);
		reswords.Add("longvarchar", SqlLexerToken.Longvarchar);
		reswords.Add("java_object", SqlLexerToken.Java_object);
		reswords.Add("longvarbinary", SqlLexerToken.Longvarbinary);
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
		lexCommentEndProc = LexCommentEnd;
		RegisterLexerProc(0, lexWhitespaceProc);
		RegisterLexerProc(0, '!', 'ÿ', lexSymbolProc);
		RegisterLexerProc(0, 'a', 'z', lexIdentifierProc);
		RegisterLexerProc(0, 'A', 'Z', lexIdentifierProc);
		RegisterLexerProc(0, new char[4] { '"', '_', '$', '#' }, lexIdentifierProc);
		RegisterLexerProc(0, '0', '9', lexNumberProc);
		RegisterLexerProc(0, '+', lexNumberProc);
		RegisterLexerProc(0, new char[3] { 'B', 'b', '\'' }, lexStringProc);
		RegisterLexerProc(0, new char[2] { '/', '-' }, lexCommentProc);
		RegisterLexerProc(1, lexCommentEndProc);
	}

	protected override int GetLexerStyle(int token)
	{
		if (IsDatatypeToken(token))
		{
			return 8;
		}
		if (IsReswordToken(token))
		{
			return 2;
		}
		if (token >= 264 && token <= 290)
		{
			return 5;
		}
		switch (token)
		{
		case 291:
		case 292:
		case 293:
			return 1;
		case 294:
			return 7;
		case 295:
			return 0;
		case 296:
			return 3;
		default:
			return 6;
		}
	}

	protected virtual int LexWhitespace()
	{
		LexSpace();
		return 297;
	}

	protected virtual int LexIdentifier()
	{
		switch (source[currentPos])
		{
		case '"':
		{
			int length = source.Length;
			currentPos++;
			while (currentPos < length)
			{
				char c = source[currentPos];
				if (c == '"')
				{
					currentPos++;
					break;
				}
				currentPos++;
			}
			return 295;
		}
		case 'N':
		case 'X':
		case 'n':
		case 'x':
			if (currentPos < source.Length - 1 && source[currentPos + 1] == '\'')
			{
				currentPos++;
				return LexString();
			}
			break;
		case 'U':
		case 'u':
			if (currentPos < source.Length - 2 && source[currentPos + 1] == '&' && source[currentPos + 2] == '\'')
			{
				currentPos += 2;
				return LexString();
			}
			break;
		}
		LexIdent();
		object obj = reswords[base.TokenString.ToLower(CultureInfo.InvariantCulture)];
		if (obj == null)
		{
			return 295;
		}
		return (int)obj;
	}

	protected virtual int LexSymbol()
	{
		SqlLexerToken result = SqlLexerToken.Whitespace_Literal;
		char c = source[currentPos];
		currentPos++;
		switch (c)
		{
		case ',':
			result = SqlLexerToken.Comma;
			break;
		case '.':
			result = SqlLexerToken.Dot;
			break;
		case ';':
			result = SqlLexerToken.Semicolon;
			break;
		case '(':
			result = SqlLexerToken.Open_parens;
			break;
		case ')':
			result = SqlLexerToken.Close_parens;
			break;
		case '=':
			result = SqlLexerToken.Op_Assign;
			break;
		case '*':
			result = SqlLexerToken.Op_Star;
			break;
		case '-':
			result = SqlLexerToken.Op_Minus;
			break;
		case '+':
			result = SqlLexerToken.Op_Plus;
			break;
		case '/':
			result = SqlLexerToken.Op_Div;
			break;
		case '<':
			switch (CurChar())
			{
			case '>':
				currentPos++;
				result = SqlLexerToken.Op_ne;
				break;
			case '=':
				currentPos++;
				result = SqlLexerToken.Op_le;
				break;
			default:
				result = SqlLexerToken.Op_lt;
				break;
			}
			break;
		case '>':
		{
			char c2 = CurChar();
			if (c2 == '=')
			{
				currentPos++;
				result = SqlLexerToken.Op_ge;
			}
			else
			{
				result = SqlLexerToken.Op_gt;
			}
			break;
		}
		case '~':
			result = SqlLexerToken.Tilde;
			break;
		case '!':
			result = SqlLexerToken.Bang;
			break;
		case '@':
			result = SqlLexerToken.Uxo;
			break;
		case '#':
			result = SqlLexerToken.Turma;
			break;
		case '%':
			result = SqlLexerToken.Percent;
			break;
		case '^':
			result = SqlLexerToken.Carret;
			break;
		case '&':
			result = SqlLexerToken.Ampersant;
			break;
		case '|':
			result = SqlLexerToken.Bitwise_or;
			break;
		case '`':
			result = SqlLexerToken.Cav;
			break;
		case '?':
			result = SqlLexerToken.Interr;
			break;
		case '$':
			result = SqlLexerToken.Dollar;
			break;
		}
		return (int)result;
	}

	protected virtual int LexNumber()
	{
		char c = source[currentPos];
		int length = source.Length;
		int result = 291;
		if (c == '+' || c == '-')
		{
			int num = currentPos + 1;
			if (num >= length || source[num] < '0' || source[num] > '9')
			{
				return LexSymbol();
			}
		}
		base.LexNum();
		if (currentPos < length)
		{
			c = source[currentPos];
			if (c == '.' && currentPos < length - 1)
			{
				c = source[currentPos + 1];
				if (c >= '0' && c <= '9')
				{
					currentPos++;
					LexNum();
					result = 292;
				}
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
						result = 293;
					}
				}
			}
		}
		return result;
	}

	protected virtual int LexString()
	{
		int length = source.Length;
		char c = source[currentPos];
		char c2 = c;
		bool flag = false;
		if (c == 'b' || c == 'B')
		{
			currentPos++;
			if (currentPos == length || source[currentPos] != '\'')
			{
				currentPos--;
				return LexIdentifier();
			}
			c2 = source[currentPos];
		}
		currentPos++;
		while (currentPos < length)
		{
			c = source[currentPos];
			if (c == c2 && !flag)
			{
				currentPos++;
				if (currentPos >= length || currentPos != c2)
				{
					break;
				}
				currentPos++;
			}
			flag = c == '\\' && !flag;
			currentPos++;
		}
		return 294;
	}

	protected virtual int LexComment()
	{
		char c = source[currentPos];
		currentPos++;
		int length = source.Length;
		if (currentPos < length)
		{
			char c2 = source[currentPos];
			switch (c)
			{
			case '-':
				if (c2 == '-')
				{
					currentPos = length + 1;
					return 296;
				}
				currentPos--;
				return LexNumber();
			case '/':
				switch (c2)
				{
				case '/':
					currentPos = length + 1;
					return 296;
				case '*':
					currentPos++;
					return LexCommentEnd();
				}
				break;
			}
		}
		currentPos--;
		return LexSymbol();
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
					return 296;
				}
			}
			else
			{
				currentPos++;
			}
		}
		base.State = 1;
		return 296;
	}

	protected virtual void AddNode(ISyntaxNode node)
	{
		base.SyntaxTree.Current.AddChild(node);
	}

	protected virtual void AddAttribute(ISyntaxAttribute attr)
	{
		base.SyntaxTree.Current.AddAttribute(attr);
	}

	protected virtual int MoveNext()
	{
		prevPosition = base.CurrentPosition;
		int result = NextToken();
		while (!base.Eof && !IsValidToken(result))
		{
			result = NextToken();
		}
		return result;
	}

	protected virtual void SyntaxError()
	{
		if (base.Stack.Count == 0)
		{
			SyntaxError(StringConsts.ErrSyntax);
		}
	}

	protected virtual void SyntaxError(string error)
	{
		if (base.Stack.Count == 0)
		{
			base.SyntaxTree.Current.AddError(new SyntaxError(base.TokenPosition, base.TokenString, error));
		}
	}

	protected virtual void SyntaxError(int token)
	{
		if (base.Stack.Count == 0)
		{
			Point tokenPosition = base.TokenPosition;
			string tokenString = base.TokenString;
			SqlLexerToken sqlLexerToken = (SqlLexerToken)token;
			ISyntaxError syntaxError = new SyntaxError(tokenPosition, tokenString, sqlLexerToken.ToString() + " " + StringConsts.ErrExpected);
			if (prevPosition.Y != base.TokenPosition.Y)
			{
				syntaxError.Position = prevPosition;
				syntaxError.Range.EndPoint = new Point(prevPosition.X + 1, prevPosition.Y);
			}
			base.SyntaxTree.Current.AddError(syntaxError);
		}
	}

	protected bool Expected(int token)
	{
		return Expected((SqlLexerToken)token);
	}

	protected bool Expected(SqlLexerToken token)
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

	protected bool Expected(SqlLexerToken token1, SqlLexerToken token2)
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

	protected bool Expected(SqlLexerToken[] tokens)
	{
		bool flag = false;
		for (int i = 0; i < tokens.Length; i++)
		{
			if (base.Token == (int)tokens[i])
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			if (tokens.Length != 0)
			{
				SyntaxError((int)tokens[0]);
			}
			else
			{
				SyntaxError();
			}
		}
		if (flag)
		{
			MoveNext();
		}
		return flag;
	}

	protected bool IdentifierExpected()
	{
		return Expected(SqlLexerToken.Identifier_Literal);
	}

	protected override bool IsValidToken(int tok)
	{
		if (tok != 297)
		{
			return tok != 296;
		}
		return false;
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

	protected virtual bool ParseArgumentList(ref ISyntaxNode node)
	{
		node = new SyntaxNode(base.TokenPosition, base.TokenString, 52);
		ISyntaxNode node2 = null;
		bool result = Expected(SqlLexerToken.Open_parens) && ParseExpressionList(ref node2) && Expected(SqlLexerToken.Close_parens);
		if (node2 != null)
		{
			node.AddChild(node2);
		}
		return result;
	}

	protected virtual bool ParseExpressionList(ref ISyntaxNode node)
	{
		bool result = true;
		node = new SyntaxNode(base.TokenPosition, string.Empty, 126);
		do
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
			if (base.Token != 283)
			{
				break;
			}
			MoveNext();
		}
		while (!base.Eof);
		return result;
	}

	protected virtual bool ParseExpression(ref ISyntaxNode node)
	{
		return ParseInclusiveOrExpression(ref node);
	}

	protected virtual bool ParseInclusiveOrExpression(ref ISyntaxNode node)
	{
		bool result = ParseAndExpression(ref node);
		SqlLexerToken sqlLexerToken = (SqlLexerToken)base.Token;
		if (sqlLexerToken == SqlLexerToken.Or)
		{
			node = CreateParentNode(base.TokenPosition, base.TokenString, 55, node);
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

	protected virtual bool ParseAndExpression(ref ISyntaxNode node)
	{
		bool result = ParseEqualityExpression(ref node);
		SqlLexerToken sqlLexerToken = (SqlLexerToken)base.Token;
		if (sqlLexerToken == SqlLexerToken.And)
		{
			node = CreateParentNode(base.TokenPosition, base.TokenString, 56, node);
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
		SqlLexerToken sqlLexerToken = (SqlLexerToken)base.Token;
		if (sqlLexerToken == SqlLexerToken.Like || sqlLexerToken == SqlLexerToken.Op_ne || sqlLexerToken == SqlLexerToken.Op_Assign)
		{
			node = CreateParentNode(base.TokenPosition, base.TokenString, (base.Token == 115) ? 58 : 60, node);
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
		bool result = ParseAdditiveExpression(ref node);
		SqlLexerToken sqlLexerToken = (SqlLexerToken)base.Token;
		if ((uint)(sqlLexerToken - 267) <= 3u)
		{
			node = CreateParentNode(base.TokenPosition, base.TokenString, 59, node);
			MoveNext();
			ISyntaxNode syntaxNode = null;
			if (!ParseRelationalExpression(ref node))
			{
				result = false;
			}
			if (syntaxNode != null)
			{
				node.AddChild(syntaxNode);
			}
			node.Range.EndPoint = prevPosition;
		}
		return result;
	}

	protected virtual bool ParseAdditiveExpression(ref ISyntaxNode node)
	{
		bool result = ParseMultiplicativeExpression(ref node);
		SqlLexerToken sqlLexerToken = (SqlLexerToken)base.Token;
		if ((uint)(sqlLexerToken - 289) <= 1u)
		{
			node = CreateParentNode(base.TokenPosition, base.TokenString, 54, node);
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
		SqlLexerToken sqlLexerToken = (SqlLexerToken)base.Token;
		if ((uint)(sqlLexerToken - 287) <= 1u)
		{
			node = CreateParentNode(base.TokenPosition, base.TokenString, 57, node);
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
		SqlLexerToken sqlLexerToken = (SqlLexerToken)base.Token;
		if (sqlLexerToken == SqlLexerToken.Not || (uint)(sqlLexerToken - 289) <= 1u)
		{
			node = CreateParentNode(base.TokenPosition, base.TokenString, 62, node);
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

	protected virtual bool ParsePrimaryExpression(ref ISyntaxNode node)
	{
		bool result = ParseSimpleExpression(ref node);
		if (node != null && !TryParsePostPrimaryExpression(ref node))
		{
			result = false;
		}
		return result;
	}

	protected virtual bool ParseSimpleExpression(ref ISyntaxNode node)
	{
		bool result = true;
		switch ((SqlLexerToken)base.Token)
		{
		case SqlLexerToken.False:
		case SqlLexerToken.True:
		case SqlLexerToken.Op_Star:
		case SqlLexerToken.Integer_Literal:
		case SqlLexerToken.Float_Literal:
		case SqlLexerToken.Double_Literal:
		case SqlLexerToken.String_Literal:
		case SqlLexerToken.Identifier_Literal:
			node = new SyntaxNode(base.TokenPosition, base.TokenString, 61);
			MoveNext();
			break;
		case SqlLexerToken.Open_parens:
			result = ParseParenthesizedExpression(ref node);
			break;
		default:
			if (IsReswordToken(base.Token))
			{
				node = new SyntaxNode(base.TokenPosition, base.TokenString, 61);
				MoveNext();
			}
			else
			{
				result = false;
				SyntaxError();
				MoveNext();
			}
			break;
		}
		return result;
	}

	protected virtual bool TryParsePostPrimaryExpression(ref ISyntaxNode node)
	{
		bool result = true;
		while (!base.Eof)
		{
			switch ((SqlLexerToken)base.Token)
			{
			case SqlLexerToken.Bang:
			case SqlLexerToken.Dot:
				if (!ParseMemberAccess(ref node))
				{
					result = false;
				}
				break;
			case SqlLexerToken.Open_parens:
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

	protected virtual bool ParseInvocationExpression(ref ISyntaxNode node)
	{
		bool flag = true;
		node = CreateParentNode(base.TokenPosition, base.TokenString, 64, node);
		ISyntaxNode node2 = null;
		flag = ParseArgumentList(ref node2);
		if (node2 != null)
		{
			node.AddChild(node2);
		}
		node.Range.EndPoint = prevPosition;
		return flag;
	}

	protected virtual bool ParseMemberAccess(ref ISyntaxNode node)
	{
		bool result = true;
		node = CreateParentNode(base.TokenPosition, string.Empty, 65, node);
		MoveNext();
		string identifier;
		if (IsReswordToken(base.Token))
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
		node.Range.EndPoint = prevPosition;
		return result;
	}

	protected virtual bool ParseParenthesizedExpression(ref ISyntaxNode node)
	{
		bool flag = true;
		if (base.Token == 264)
		{
			node = new SyntaxNode(base.TokenPosition, base.TokenString, 66);
			MoveNext();
			ISyntaxNode node2 = null;
			flag = ParseExpression(ref node2);
			if (node2 != null)
			{
				node.AddChild(node2);
			}
			if (!Expected(SqlLexerToken.Close_parens))
			{
				flag = false;
			}
			node.Range.EndPoint = prevPosition;
		}
		else
		{
			SyntaxError(264);
			flag = false;
		}
		return flag;
	}

	protected virtual bool ParseType(out string type)
	{
		bool flag = false;
		type = string.Empty;
		flag = IsDatatypeToken(base.Token);
		if (flag)
		{
			bool flag2 = IsDateOrIntervalToken(base.Token);
			type = base.TokenString;
			MoveNext();
			if (flag2 && base.Token == 294)
			{
				MoveNext();
			}
			if (base.Token == 264)
			{
				MoveNext();
				ISyntaxNode node = null;
				flag = ParseExpressionList(ref node) && Expected(SqlLexerToken.Close_parens);
			}
		}
		return flag;
	}

	protected virtual bool IsReswordToken(int token)
	{
		if (token >= 0)
		{
			return token <= 263;
		}
		return false;
	}

	protected virtual bool IsDatatypeToken(int token)
	{
		if (token >= 240)
		{
			return token <= 263;
		}
		return false;
	}

	protected virtual bool IsDateOrIntervalToken(int token)
	{
		if ((uint)(token - 244) <= 2u || token == 260)
		{
			return true;
		}
		return false;
	}

	protected virtual bool ParseIdentifier(out string identifier)
	{
		identifier = base.TokenString;
		bool flag = IsReswordToken(base.Token) || base.Token == 295;
		if (flag)
		{
			MoveNext();
		}
		else
		{
			flag = IdentifierExpected();
		}
		return flag;
	}

	protected virtual bool ParseQualifiedIdentifier(out string identifier)
	{
		bool flag = ParseIdentifier(out identifier);
		if (flag)
		{
			while (base.Token == 284)
			{
				identifier += base.TokenString;
				MoveNext();
				if (ParseIdentifier(out var identifier2))
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

	protected virtual bool ParseIdentifierList(out string list)
	{
		bool flag = ParseQualifiedIdentifier(out list);
		if (flag)
		{
			while (base.Token == 283)
			{
				list += base.TokenString;
				MoveNext();
				if (ParseQualifiedIdentifier(out var identifier))
				{
					list += identifier;
				}
				else
				{
					flag = false;
				}
			}
		}
		return flag;
	}

	protected virtual bool ParseUnitBody()
	{
		bool result = true;
		bool flag = base.SyntaxTree.Current != base.SyntaxTree.Root;
		while (!base.Eof)
		{
			switch ((SqlLexerToken)base.Token)
			{
			case SqlLexerToken.Create:
				result = ParseCreateQuery();
				break;
			case SqlLexerToken.Alter:
				result = ParseAlterQuery();
				break;
			case SqlLexerToken.Drop:
				result = ParseDropQuery();
				break;
			case SqlLexerToken.Insert:
				result = ParseInsertQuery();
				break;
			case SqlLexerToken.Delete:
				result = ParseDeleteQuery();
				break;
			case SqlLexerToken.Compact:
				result = ParseCompactQuery();
				break;
			case SqlLexerToken.Select:
				result = ParseSelectQuery();
				break;
			case SqlLexerToken.Update:
				result = ParseUpdateQuery();
				break;
			case SqlLexerToken.Commit:
				result = ParseCommitQuery();
				break;
			case SqlLexerToken.Rollback:
				result = ParseRollbackQuery();
				break;
			case SqlLexerToken.Grant:
				result = ParseGrantQuery();
				break;
			case SqlLexerToken.Revoke:
				result = ParseRevokeQuery();
				break;
			case SqlLexerToken.Set:
				result = ParseSetQuery();
				break;
			case SqlLexerToken.Describe:
				result = ParseDescribeQuery();
				break;
			case SqlLexerToken.Show:
				result = ParseShowQuery();
				break;
			case SqlLexerToken.Shutdown:
				result = ParseShutdownQuery();
				break;
			default:
				result = false;
				SyntaxError();
				MoveNext();
				break;
			}
			if (base.Token == 285)
			{
				MoveNext();
			}
		}
		return result;
	}

	protected virtual bool ParseSelectQuery()
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString.ToUpper(CultureInfo.InvariantCulture), 5, SyntaxNodeOptions.Indentation | SyntaxNodeOptions.Outlining);
		AddNode(syntaxNode);
		base.SyntaxTree.Push(syntaxNode);
		try
		{
			MoveNext();
			syntaxNode.AddAttribute(new SyntaxAttribute(prevPosition, SyntaxConsts.DeclarationScope, null));
			SqlLexerToken sqlLexerToken = (SqlLexerToken)base.Token;
			if (sqlLexerToken == SqlLexerToken.All || sqlLexerToken == SqlLexerToken.Distinct)
			{
				syntaxNode.AddAttribute(new SyntaxAttribute(base.TokenPosition, SqlNodeType.Attribute.ToString(), base.TokenString.ToUpper(CultureInfo.InvariantCulture)));
				MoveNext();
			}
			if (!ParseSelectColumnList())
			{
				result = false;
			}
			bool flag = false;
			do
			{
				switch ((SqlLexerToken)base.Token)
				{
				case SqlLexerToken.From:
					if (!ParseFromStatement())
					{
						result = false;
					}
					break;
				case SqlLexerToken.Where:
					if (!ParseWhereStatement())
					{
						result = false;
					}
					break;
				case SqlLexerToken.Group:
					if (!ParseGroupbyStatement())
					{
						result = false;
					}
					break;
				case SqlLexerToken.Having:
					if (!ParseHavingStatement())
					{
						result = false;
					}
					break;
				case SqlLexerToken.Order:
					if (!ParseOrderbyStatement())
					{
						result = false;
					}
					break;
				default:
					flag = true;
					break;
				}
			}
			while (!(base.Eof || flag));
			if (base.Token == 285)
			{
				MoveNext();
			}
		}
		finally
		{
			base.SyntaxTree.Pop();
		}
		syntaxNode.Range.EndPoint = prevPosition;
		return result;
	}

	protected virtual bool ParseSelectColumnList()
	{
		bool result = true;
		do
		{
			ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString, 71);
			AddNode(syntaxNode);
			ISyntaxNode node = null;
			if (!ParseExpression(ref node))
			{
				result = false;
			}
			if (node != null)
			{
				syntaxNode.AddChild(node);
			}
			if (base.Token == 11)
			{
				MoveNext();
				Point tokenPosition = base.TokenPosition;
				if (ParseQualifiedIdentifier(out var identifier))
				{
					syntaxNode.AddAttribute(new SyntaxAttribute(tokenPosition, SqlNodeType.ColumnAlias.ToString(), identifier));
				}
				else
				{
					result = false;
				}
			}
			syntaxNode.Range.EndPoint = prevPosition;
			if (base.Token != 283)
			{
				break;
			}
			MoveNext();
		}
		while (!base.Eof);
		return result;
	}

	protected virtual bool ParseFromStatement()
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString, 39);
		AddNode(syntaxNode);
		base.SyntaxTree.Push(syntaxNode);
		try
		{
			MoveNext();
			result = ParseFromClause();
		}
		finally
		{
			base.SyntaxTree.Pop();
		}
		syntaxNode.Range.EndPoint = prevPosition;
		return result;
	}

	protected virtual bool ParseFromClause()
	{
		bool result = true;
		bool flag;
		do
		{
			if (!ParseSelectTable())
			{
				result = false;
			}
			flag = true;
			switch ((SqlLexerToken)base.Token)
			{
			case SqlLexerToken.Left:
			case SqlLexerToken.Right:
			{
				Point tokenPosition2 = base.TokenPosition;
				string text2 = base.TokenString.ToUpper(CultureInfo.InvariantCulture);
				MoveNext();
				if (base.Token == 149)
				{
					text2 = text2 + " " + base.TokenString.ToUpper(CultureInfo.InvariantCulture);
				}
				if (Expected(SqlLexerToken.Join))
				{
					text2 = text2 + " " + base.TokenString.ToUpper(CultureInfo.InvariantCulture);
				}
				else
				{
					result = false;
				}
				AddAttribute(new SyntaxAttribute(tokenPosition2, SqlNodeType.Attribute.ToString(), text2));
				if (!ParseSelectTable())
				{
					result = false;
				}
				ISyntaxNode node = null;
				if (!Expected(SqlLexerToken.On) || !ParseExpression(ref node))
				{
					result = false;
				}
				if (node != null)
				{
					AddNode(node);
				}
				break;
			}
			case SqlLexerToken.Inner:
			{
				Point tokenPosition = base.TokenPosition;
				string text = base.TokenString.ToUpper(CultureInfo.InvariantCulture);
				MoveNext();
				if (Expected(SqlLexerToken.Join))
				{
					text = text + " " + base.TokenString.ToUpper(CultureInfo.InvariantCulture);
				}
				else
				{
					result = false;
				}
				AddAttribute(new SyntaxAttribute(tokenPosition, SqlNodeType.Attribute.ToString(), text));
				if (!ParseSelectTable())
				{
					result = false;
				}
				break;
			}
			case SqlLexerToken.Comma:
				flag = false;
				MoveNext();
				break;
			}
		}
		while (!(base.Eof || flag));
		return result;
	}

	protected virtual bool ParseSelectTable()
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, string.Empty, 67);
		AddNode(syntaxNode);
		base.SyntaxTree.Push(syntaxNode);
		try
		{
			string identifier;
			if (base.Token == 185)
			{
				if (!ParseSelectQuery())
				{
					result = false;
				}
			}
			else if (ParseQualifiedIdentifier(out identifier))
			{
				syntaxNode.Name = identifier;
			}
			else
			{
				result = false;
			}
			if (base.Token == 11)
			{
				MoveNext();
				Point tokenPosition = base.TokenPosition;
				if (ParseQualifiedIdentifier(out identifier))
				{
					AddAttribute(new SyntaxAttribute(tokenPosition, SqlNodeType.TableAlias.ToString(), identifier));
				}
				else
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

	protected virtual bool ParseGroupbyStatement()
	{
		bool flag = true;
		MoveNext();
		if (Expected(SqlLexerToken.By))
		{
			ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString, 42);
			AddNode(syntaxNode);
			MoveNext();
			ISyntaxNode node = null;
			flag = ParseExpressionList(ref node);
			if (node != null)
			{
				syntaxNode.AddChild(node);
			}
			syntaxNode.Range.EndPoint = prevPosition;
		}
		else
		{
			flag = false;
		}
		return flag;
	}

	protected virtual bool ParseOrderbyStatement()
	{
		bool result = true;
		MoveNext();
		if (Expected(SqlLexerToken.By))
		{
			ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString, 41);
			AddNode(syntaxNode);
			base.SyntaxTree.Push(syntaxNode);
			try
			{
				MoveNext();
				do
				{
					if (!ParseOrderColumn())
					{
						result = false;
					}
					if (base.Token == 283)
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
			syntaxNode.Range.EndPoint = prevPosition;
		}
		else
		{
			result = false;
		}
		return result;
	}

	protected virtual bool ParseOrderColumn()
	{
		bool flag = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, string.Empty, 71);
		AddNode(syntaxNode);
		ISyntaxNode node = null;
		flag = ParseExpression(ref node);
		if (node != null)
		{
			syntaxNode.AddChild(node);
		}
		SqlLexerToken sqlLexerToken = (SqlLexerToken)base.Token;
		if (sqlLexerToken == SqlLexerToken.Asc || sqlLexerToken == SqlLexerToken.Desc)
		{
			syntaxNode.AddAttribute(new SyntaxAttribute(base.TokenPosition, SqlNodeType.Attribute.ToString(), base.TokenString));
			MoveNext();
		}
		syntaxNode.Range.EndPoint = prevPosition;
		return flag;
	}

	protected virtual bool ParseHavingStatement()
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString, 40);
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
		syntaxNode.Range.EndPoint = prevPosition;
		return result;
	}

	protected virtual bool ParseCreateQuery()
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString.ToUpper(CultureInfo.InvariantCulture), 2, SyntaxNodeOptions.Indentation);
		AddNode(syntaxNode);
		base.SyntaxTree.Push(syntaxNode);
		try
		{
			MoveNext();
			switch ((SqlLexerToken)base.Token)
			{
			case SqlLexerToken.View:
				if (!ParseCreateView())
				{
					result = false;
				}
				break;
			case SqlLexerToken.Sequence:
				if (!ParseCreateSequence())
				{
					result = false;
				}
				break;
			case SqlLexerToken.Schema:
				if (!ParseCreateSchema())
				{
					result = false;
				}
				break;
			case SqlLexerToken.User:
				if (!ParseCreateUser())
				{
					result = false;
				}
				break;
			case SqlLexerToken.Table:
				if (!ParseCreateTable())
				{
					result = false;
				}
				break;
			default:
				result = false;
				SyntaxError();
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

	protected virtual bool ParseCreateView()
	{
		bool result = true;
		MoveNext();
		ISyntaxNode current = base.SyntaxTree.Current;
		current.NodeType = 19;
		current.AddAttribute(new SyntaxAttribute(prevPosition, SyntaxConsts.DeclarationScope, null));
		if (ParseQualifiedIdentifier(out var identifier))
		{
			current.Name = identifier;
			if (base.Token == 264 && !ParseColumnList())
			{
				result = false;
			}
			if (Expected(SqlLexerToken.As))
			{
				if (base.Token == 185)
				{
					if (!ParseSelectQuery())
					{
						result = false;
					}
				}
				else
				{
					result = Expected(SqlLexerToken.Select);
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
		current.Range.EndPoint = prevPosition;
		return result;
	}

	protected virtual bool ParseCreateSchema()
	{
		bool result = true;
		MoveNext();
		ISyntaxNode current = base.SyntaxTree.Current;
		current.NodeType = 20;
		current.AddAttribute(new SyntaxAttribute(prevPosition, SyntaxConsts.DeclarationScope, null));
		if (ParseQualifiedIdentifier(out var identifier))
		{
			current.Name = identifier;
		}
		else
		{
			result = false;
		}
		current.Range.EndPoint = prevPosition;
		return result;
	}

	protected virtual bool ParseCreateSequence()
	{
		bool result = true;
		MoveNext();
		ISyntaxNode current = base.SyntaxTree.Current;
		current.NodeType = 22;
		current.AddAttribute(new SyntaxAttribute(prevPosition, SyntaxConsts.DeclarationScope, null));
		if (ParseQualifiedIdentifier(out var identifier))
		{
			current.Name = identifier;
			bool flag = false;
			do
			{
				switch ((SqlLexerToken)base.Token)
				{
				case SqlLexerToken.Cashe:
				case SqlLexerToken.Increment:
				case SqlLexerToken.Minvalue:
				case SqlLexerToken.Start:
				{
					current.AddAttribute(new SyntaxAttribute(base.TokenPosition, SqlNodeType.Attribute.ToString(), base.TokenString.ToUpper(CultureInfo.InvariantCulture)));
					MoveNext();
					ISyntaxNode node = null;
					if (!ParseExpression(ref node))
					{
						result = false;
					}
					if (node != null)
					{
						current.AddChild(node);
					}
					break;
				}
				case SqlLexerToken.Cycle:
					current.AddAttribute(new SyntaxAttribute(base.TokenPosition, SqlNodeType.Attribute.ToString(), base.TokenString.ToUpper(CultureInfo.InvariantCulture)));
					MoveNext();
					break;
				default:
					flag = true;
					break;
				}
			}
			while (!(base.Eof || flag));
		}
		else
		{
			result = false;
		}
		current.Range.EndPoint = prevPosition;
		return result;
	}

	protected virtual bool ParseCreateTable()
	{
		bool result = true;
		MoveNext();
		ISyntaxNode current = base.SyntaxTree.Current;
		current.NodeType = 18;
		current.Options = SyntaxNodeOptions.Indentation | SyntaxNodeOptions.Outlining;
		current.AddAttribute(new SyntaxAttribute(prevPosition, SyntaxConsts.DeclarationScope, null));
		Point tokenPosition = base.TokenPosition;
		if (base.Token == 94)
		{
			string text = base.TokenString.ToUpper(CultureInfo.InvariantCulture);
			MoveNext();
			if (Expected(SqlLexerToken.Not))
			{
				text = text + " " + base.TokenString.ToUpper(CultureInfo.InvariantCulture);
			}
			else
			{
				result = false;
			}
			if (Expected(SqlLexerToken.Exists))
			{
				text = text + " " + base.TokenString.ToUpper(CultureInfo.InvariantCulture);
			}
			else
			{
				result = false;
			}
			current.AddAttribute(new SyntaxAttribute(tokenPosition, SqlNodeType.Attribute.ToString(), text));
		}
		if (ParseQualifiedIdentifier(out var identifier))
		{
			current.Name = identifier;
			if (base.Token == 264)
			{
				MoveNext();
				do
				{
					if (base.Token == 42)
					{
						if (!ParseConstraintDeclaration())
						{
							result = false;
						}
					}
					else if (!ParseColumnDeclaration())
					{
						result = false;
					}
					if (base.Token != 283)
					{
						break;
					}
					MoveNext();
				}
				while (!base.Eof);
				if (!Expected(SqlLexerToken.Close_parens))
				{
					result = false;
				}
			}
		}
		else
		{
			result = false;
		}
		current.Range.EndPoint = prevPosition;
		return result;
	}

	protected virtual bool ParseCreateUser()
	{
		bool result = ParseAlterUser();
		base.SyntaxTree.Current.NodeType = 21;
		return result;
	}

	protected virtual bool ParseDropQuery()
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString.ToUpper(CultureInfo.InvariantCulture), 4, SyntaxNodeOptions.Indentation);
		AddNode(syntaxNode);
		base.SyntaxTree.Push(syntaxNode);
		try
		{
			MoveNext();
			syntaxNode.AddAttribute(new SyntaxAttribute(prevPosition, SyntaxConsts.DeclarationScope, null));
			switch ((SqlLexerToken)base.Token)
			{
			case SqlLexerToken.View:
				if (!ParseDropView())
				{
					result = false;
				}
				break;
			case SqlLexerToken.Sequence:
				if (!ParseDropSequence())
				{
					result = false;
				}
				break;
			case SqlLexerToken.Schema:
				if (!ParseDropSchema())
				{
					result = false;
				}
				break;
			case SqlLexerToken.User:
				if (!ParseDropUser())
				{
					result = false;
				}
				break;
			case SqlLexerToken.Table:
				if (!ParseDropTable())
				{
					result = false;
				}
				break;
			default:
				result = false;
				SyntaxError();
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

	protected virtual bool ParseDropUser()
	{
		bool result = true;
		MoveNext();
		if (ParseQualifiedIdentifier(out var identifier))
		{
			ISyntaxNode current = base.SyntaxTree.Current;
			current.Name = identifier;
			current.NodeType = 27;
			current.Range.EndPoint = prevPosition;
		}
		else
		{
			result = false;
		}
		return result;
	}

	protected virtual bool ParseDropView()
	{
		bool flag = ParseDropUser();
		if (flag)
		{
			base.SyntaxTree.Current.NodeType = 25;
		}
		return flag;
	}

	protected virtual bool ParseDropSequence()
	{
		bool flag = ParseDropUser();
		if (flag)
		{
			base.SyntaxTree.Current.NodeType = 28;
		}
		return flag;
	}

	protected virtual bool ParseDropSchema()
	{
		bool flag = ParseDropUser();
		if (flag)
		{
			base.SyntaxTree.Current.NodeType = 26;
		}
		return flag;
	}

	protected virtual bool ParseDropTable()
	{
		bool result = true;
		MoveNext();
		ISyntaxNode current = base.SyntaxTree.Current;
		if (base.Token == 94)
		{
			Point tokenPosition = base.TokenPosition;
			string text = base.TokenString.ToUpper(CultureInfo.InvariantCulture);
			MoveNext();
			if (base.Token == 76)
			{
				current.AddAttribute(new SyntaxAttribute(tokenPosition, SqlNodeType.Attribute.ToString(), text + " " + base.TokenString.ToUpper(CultureInfo.InvariantCulture)));
				MoveNext();
			}
			else
			{
				result = false;
			}
		}
		if (ParseIdentifierList(out var list))
		{
			current.Name = list;
			current.NodeType = 29;
		}
		else
		{
			result = false;
		}
		current.Range.EndPoint = prevPosition;
		return result;
	}

	protected virtual bool ParseAlterQuery()
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString.ToUpper(CultureInfo.InvariantCulture), 3, SyntaxNodeOptions.Indentation);
		AddNode(syntaxNode);
		base.SyntaxTree.Push(syntaxNode);
		try
		{
			MoveNext();
			switch ((SqlLexerToken)base.Token)
			{
			case SqlLexerToken.User:
				if (!ParseAlterUser())
				{
					result = false;
				}
				break;
			case SqlLexerToken.Table:
				if (!ParseAlterTable())
				{
					result = false;
				}
				break;
			case SqlLexerToken.Create:
				if (!ParseAlterCreateTable())
				{
					result = false;
				}
				break;
			default:
				result = false;
				SyntaxError();
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

	protected virtual bool ParseAlterTable()
	{
		bool result = true;
		MoveNext();
		ISyntaxNode current = base.SyntaxTree.Current;
		current.AddAttribute(new SyntaxAttribute(prevPosition, SyntaxConsts.DeclarationScope, null));
		if (ParseQualifiedIdentifier(out var identifier))
		{
			current.Name = identifier;
			current.NodeType = 24;
			current.Options = SyntaxNodeOptions.Indentation | SyntaxNodeOptions.Outlining;
			bool flag = false;
			do
			{
				switch ((SqlLexerToken)base.Token)
				{
				case SqlLexerToken.Add:
					if (!ParseAlterTableAdd())
					{
						result = false;
					}
					break;
				case SqlLexerToken.Drop:
					if (IsNextQueryToken())
					{
						flag = true;
					}
					else if (!ParseAlterTableDrop())
					{
						result = false;
					}
					break;
				case SqlLexerToken.Alter:
					if (IsNextQueryToken())
					{
						flag = true;
					}
					else if (!ParseAlterTableAlter())
					{
						result = false;
					}
					break;
				default:
					flag = true;
					break;
				}
			}
			while (!(base.Eof || flag));
			current.Range.EndPoint = prevPosition;
		}
		else
		{
			result = false;
		}
		return result;
	}

	protected virtual bool IsNextQueryToken()
	{
		bool result = false;
		Point point = prevPosition;
		SaveState();
		try
		{
			MoveNext();
			switch ((SqlLexerToken)base.Token)
			{
			case SqlLexerToken.Create:
			case SqlLexerToken.Schema:
			case SqlLexerToken.Sequence:
			case SqlLexerToken.Table:
			case SqlLexerToken.User:
			case SqlLexerToken.View:
				result = true;
				break;
			}
		}
		finally
		{
			RestoreState();
			prevPosition = point;
		}
		return result;
	}

	protected virtual bool ParseAlterCreateTable()
	{
		MoveNext();
		if (base.Token == 209)
		{
			return ParseCreateTable();
		}
		return Expected(209);
	}

	protected virtual bool ParseAlterTableAdd()
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString, 30);
		AddNode(syntaxNode);
		base.SyntaxTree.Push(syntaxNode);
		try
		{
			MoveNext();
			if (base.Token == 33)
			{
				MoveNext();
			}
			if (base.Token == 42)
			{
				if (!ParseConstraintDeclaration())
				{
					result = false;
				}
			}
			else if (!ParseColumnDeclaration())
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

	protected virtual bool ParseColumnDeclaration()
	{
		bool result = true;
		Point tokenPosition = base.TokenPosition;
		if (ParseQualifiedIdentifier(out var identifier))
		{
			ISyntaxNode syntaxNode = new SyntaxNode(tokenPosition, identifier, 71);
			AddNode(syntaxNode);
			base.SyntaxTree.Push(syntaxNode);
			try
			{
				tokenPosition = base.TokenPosition;
				if (ParseType(out identifier))
				{
					syntaxNode.AddAttribute(new SyntaxAttribute(tokenPosition, SqlNodeType.TypeAttribute.ToString(), identifier));
				}
				else
				{
					result = false;
				}
				tokenPosition = base.TokenPosition;
				bool flag = false;
				do
				{
					tokenPosition = base.TokenPosition;
					switch ((SqlLexerToken)base.Token)
					{
					case SqlLexerToken.Default:
						if (!ParseDefaultStatement())
						{
							result = false;
						}
						break;
					case SqlLexerToken.Null:
						MoveNext();
						syntaxNode.AddAttribute(new SyntaxAttribute(tokenPosition, SqlNodeType.Attribute.ToString(), base.TokenString.ToUpper(CultureInfo.InvariantCulture)));
						break;
					case SqlLexerToken.Not:
					{
						string text = base.TokenString.ToUpper(CultureInfo.InvariantCulture);
						MoveNext();
						if (Expected(135))
						{
							syntaxNode.AddAttribute(new SyntaxAttribute(tokenPosition, SqlNodeType.Attribute.ToString(), text + " " + base.TokenString.ToUpper(CultureInfo.InvariantCulture)));
						}
						else
						{
							result = false;
						}
						break;
					}
					case SqlLexerToken.Unique:
						MoveNext();
						syntaxNode.AddAttribute(new SyntaxAttribute(tokenPosition, SqlNodeType.Attribute.ToString(), base.TokenString.ToUpper(CultureInfo.InvariantCulture)));
						break;
					case SqlLexerToken.Index_Blist:
					case SqlLexerToken.Index_None:
						tokenPosition = base.TokenPosition;
						MoveNext();
						syntaxNode.AddAttribute(new SyntaxAttribute(tokenPosition, SqlNodeType.Attribute.ToString(), base.TokenString.ToUpper(CultureInfo.InvariantCulture)));
						break;
					default:
						flag = true;
						break;
					}
				}
				while (!(base.Eof || flag));
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

	protected virtual bool ParseDefaultStatement()
	{
		bool flag = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString, 43);
		AddNode(syntaxNode);
		MoveNext();
		ISyntaxNode node = null;
		flag = ParseExpression(ref node);
		if (node != null)
		{
			syntaxNode.AddChild(node);
		}
		syntaxNode.Range.EndPoint = prevPosition;
		return flag;
	}

	protected virtual bool ParseConstraintDeclaration()
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, string.Empty, 73);
		AddNode(syntaxNode);
		base.SyntaxTree.Push(syntaxNode);
		try
		{
			MoveNext();
			if (ParseQualifiedIdentifier(out var identifier))
			{
				syntaxNode.Name = identifier;
				bool flag = false;
				do
				{
					switch ((SqlLexerToken)base.Token)
					{
					case SqlLexerToken.Primary:
						if (!ParsePrimaryKeyStatement())
						{
							result = false;
						}
						break;
					case SqlLexerToken.Foreign:
						if (!ParseForeignKeyStatement())
						{
							result = false;
						}
						break;
					case SqlLexerToken.Unique:
						if (!ParseUniqueStatement())
						{
							result = false;
						}
						break;
					case SqlLexerToken.Check:
						if (!ParseCheckStatement())
						{
							result = false;
						}
						break;
					case SqlLexerToken.Initially:
					{
						string text2 = base.TokenString.ToUpper(CultureInfo.InvariantCulture);
						Point tokenPosition2 = base.TokenPosition;
						MoveNext();
						if (base.Token == 57 || base.Token == 95)
						{
							syntaxNode.AddAttribute(new SyntaxAttribute(tokenPosition2, SqlNodeType.Attribute.ToString(), text2 = text2 + " " + base.TokenString.ToUpper(CultureInfo.InvariantCulture)));
							MoveNext();
						}
						else
						{
							result = false;
						}
						break;
					}
					case SqlLexerToken.Defferable:
						syntaxNode.AddAttribute(new SyntaxAttribute(base.TokenPosition, SqlNodeType.Attribute.ToString(), base.TokenString.ToUpper(CultureInfo.InvariantCulture)));
						break;
					case SqlLexerToken.Not:
					{
						string text = base.TokenString.ToUpper(CultureInfo.InvariantCulture);
						Point tokenPosition = base.TokenPosition;
						if (Expected(56))
						{
							syntaxNode.AddAttribute(new SyntaxAttribute(tokenPosition, SqlNodeType.Attribute.ToString(), text + " " + base.TokenString.ToUpper(CultureInfo.InvariantCulture)));
						}
						else
						{
							result = false;
						}
						break;
					}
					default:
						flag = true;
						break;
					}
					if (base.Token != 283)
					{
						continue;
					}
					bool flag2 = false;
					SaveState();
					try
					{
						MoveNext();
						switch ((SqlLexerToken)base.Token)
						{
						case SqlLexerToken.Check:
						case SqlLexerToken.Defferable:
						case SqlLexerToken.Foreign:
						case SqlLexerToken.Initially:
						case SqlLexerToken.Not:
						case SqlLexerToken.Primary:
						case SqlLexerToken.Unique:
							flag2 = true;
							break;
						}
					}
					finally
					{
						RestoreState(!flag2);
					}
				}
				while (!(base.Eof || flag));
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

	protected virtual bool ParsePrimaryKeyStatement()
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString, 33);
		AddNode(syntaxNode);
		base.SyntaxTree.Push(syntaxNode);
		try
		{
			MoveNext();
			result = Expected(112) && ParseColumnList();
		}
		finally
		{
			base.SyntaxTree.Pop();
		}
		syntaxNode.Range.EndPoint = prevPosition;
		return result;
	}

	protected virtual bool ParseForeignKeyStatement()
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString, 34);
		AddNode(syntaxNode);
		base.SyntaxTree.Push(syntaxNode);
		try
		{
			MoveNext();
			if (Expected(112))
			{
				result = ParseColumnList();
				if (Expected(SqlLexerToken.References))
				{
					if (!ParseReferenceStatement())
					{
						result = false;
					}
					while (base.Token == 141)
					{
						MoveNext();
						if (!ParseUpdateDeleteStatement())
						{
							result = false;
						}
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
		}
		finally
		{
			base.SyntaxTree.Pop();
		}
		syntaxNode.Range.EndPoint = prevPosition;
		return result;
	}

	protected virtual bool ParseReferenceStatement()
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString, 46);
		AddNode(syntaxNode);
		base.SyntaxTree.Push(syntaxNode);
		try
		{
			MoveNext();
			if (ParseQualifiedIdentifier(out var identifier))
			{
				syntaxNode.Name = identifier;
			}
			else
			{
				result = false;
			}
			if (!ParseOptionalColumnList())
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

	protected virtual bool ParseUpdateDeleteStatement()
	{
		bool flag = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString);
		MoveNext();
		switch ((SqlLexerToken)base.Token)
		{
		case SqlLexerToken.Update:
			syntaxNode.NodeType = 45;
			AddNode(syntaxNode);
			MoveNext();
			flag = ParseTriggeredAction();
			break;
		case SqlLexerToken.Delete:
			syntaxNode.NodeType = 44;
			AddNode(syntaxNode);
			MoveNext();
			flag = ParseTriggeredAction();
			break;
		default:
			flag = false;
			break;
		}
		syntaxNode.Range.EndPoint = prevPosition;
		return flag;
	}

	protected virtual bool ParseTriggeredAction()
	{
		bool result = true;
		string text = base.TokenString.ToUpper(CultureInfo.InvariantCulture);
		Point tokenPosition = base.TokenPosition;
		switch ((SqlLexerToken)base.Token)
		{
		case SqlLexerToken.No:
			MoveNext();
			if (base.Token == 4)
			{
				AddAttribute(new SyntaxAttribute(tokenPosition, SqlNodeType.TriggerAttribute.ToString(), text + " " + base.TokenString.ToUpper(CultureInfo.InvariantCulture)));
				MoveNext();
			}
			else
			{
				result = Expected(SqlLexerToken.Action);
			}
			break;
		case SqlLexerToken.Set:
		{
			MoveNext();
			SqlLexerToken sqlLexerToken = (SqlLexerToken)base.Token;
			if (sqlLexerToken == SqlLexerToken.Default || sqlLexerToken == SqlLexerToken.Null)
			{
				AddAttribute(new SyntaxAttribute(tokenPosition, SqlNodeType.TriggerAttribute.ToString(), text + " " + base.TokenString.ToUpper(CultureInfo.InvariantCulture)));
				MoveNext();
			}
			else
			{
				result = false;
				SyntaxError();
			}
			break;
		}
		case SqlLexerToken.Cascade:
			AddAttribute(new SyntaxAttribute(tokenPosition, SqlNodeType.TriggerAttribute.ToString(), text));
			break;
		default:
			result = false;
			SyntaxError();
			break;
		}
		return result;
	}

	protected virtual bool ParseUniqueStatement()
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString, 36);
		AddNode(syntaxNode);
		base.SyntaxTree.Push(syntaxNode);
		try
		{
			MoveNext();
			result = ParseColumnList();
		}
		finally
		{
			base.SyntaxTree.Pop();
		}
		syntaxNode.Range.EndPoint = prevPosition;
		return result;
	}

	protected virtual bool ParseCheckStatement()
	{
		bool flag = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString, 35);
		AddNode(syntaxNode);
		MoveNext();
		ISyntaxNode node = null;
		flag = Expected(SqlLexerToken.Open_parens) && ParseExpression(ref node) && Expected(SqlLexerToken.Close_parens);
		if (node != null)
		{
			syntaxNode.AddChild(node);
		}
		syntaxNode.Range.EndPoint = prevPosition;
		return flag;
	}

	protected virtual bool ParseColumnList()
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, string.Empty, 49);
		AddNode(syntaxNode);
		if (Expected(SqlLexerToken.Open_parens))
		{
			Point tokenPosition = base.TokenPosition;
			if (ParseIdentifierList(out var list))
			{
				syntaxNode.AddAttribute(new SyntaxAttribute(tokenPosition, SqlNodeType.IdentifierList.ToString(), list));
				result = Expected(SqlLexerToken.Close_parens);
			}
			else
			{
				result = false;
			}
		}
		syntaxNode.Range.EndPoint = prevPosition;
		return result;
	}

	protected virtual bool ParseAlterTableDrop()
	{
		bool flag = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString, 4);
		AddNode(syntaxNode);
		MoveNext();
		if (base.Token == 33)
		{
			MoveNext();
		}
		Point tokenPosition = base.TokenPosition;
		string identifier = string.Empty;
		switch ((SqlLexerToken)base.Token)
		{
		case SqlLexerToken.Constraint:
			MoveNext();
			tokenPosition = base.TokenPosition;
			if (!ParseQualifiedIdentifier(out identifier))
			{
				flag = false;
			}
			break;
		case SqlLexerToken.Primary:
			identifier = base.TokenString.ToUpper(CultureInfo.InvariantCulture);
			tokenPosition = base.TokenPosition;
			MoveNext();
			if (base.Token == 112)
			{
				identifier = identifier + " " + base.TokenString.ToUpper(CultureInfo.InvariantCulture);
				MoveNext();
			}
			else
			{
				flag = Expected(112);
			}
			break;
		default:
			if (!ParseQualifiedIdentifier(out identifier))
			{
				flag = false;
			}
			break;
		}
		if (flag)
		{
			syntaxNode.AddAttribute(new SyntaxAttribute(tokenPosition, SqlNodeType.ColumnName.ToString(), identifier));
		}
		syntaxNode.Range.EndPoint = prevPosition;
		return flag;
	}

	protected virtual bool ParseAlterTableAlter()
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString, 3);
		AddNode(syntaxNode);
		MoveNext();
		if (base.Token == 33)
		{
			MoveNext();
		}
		Point tokenPosition = base.TokenPosition;
		if (ParseQualifiedIdentifier(out var identifier))
		{
			syntaxNode.AddAttribute(new SyntaxAttribute(tokenPosition, SqlNodeType.ColumnName.ToString(), identifier));
			switch ((SqlLexerToken)base.Token)
			{
			case SqlLexerToken.Set:
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
			case SqlLexerToken.Drop:
				MoveNext();
				if (!Expected(SqlLexerToken.Default))
				{
					result = false;
				}
				break;
			default:
				SyntaxError();
				break;
			}
		}
		else
		{
			result = false;
		}
		syntaxNode.Range.EndPoint = prevPosition;
		return result;
	}

	protected virtual bool ParseAlterUser()
	{
		bool result = true;
		MoveNext();
		Point tokenPosition = base.TokenPosition;
		ISyntaxNode current = base.SyntaxTree.Current;
		current.NodeType = 23;
		if (ParseQualifiedIdentifier(out var identifier))
		{
			current.Name = identifier;
			do
			{
				if (Expected(SqlLexerToken.Set))
				{
					switch ((SqlLexerToken)base.Token)
					{
					case SqlLexerToken.Password:
					{
						ISyntaxAttribute syntaxAttribute3 = new SyntaxAttribute(base.TokenPosition, SqlNodeType.PasswordAttribute.ToString(), null);
						MoveNext();
						ISyntaxNode node = null;
						if (ParseExpression(ref node))
						{
							if (node != null && node.NodeType == 61)
							{
								syntaxAttribute3.Value = node.Name;
							}
						}
						else
						{
							result = false;
						}
						AddAttribute(syntaxAttribute3);
						break;
					}
					case SqlLexerToken.Groups:
					{
						ISyntaxAttribute syntaxAttribute2 = new SyntaxAttribute(base.TokenPosition, SqlNodeType.IdentifierList.ToString(), null);
						MoveNext();
						if (ParseIdentifierList(out var list))
						{
							syntaxAttribute2.Value = list;
						}
						else
						{
							result = false;
						}
						AddAttribute(syntaxAttribute2);
						break;
					}
					case SqlLexerToken.Account:
					{
						ISyntaxAttribute syntaxAttribute = new SyntaxAttribute(base.TokenPosition, SqlNodeType.AccountAttribute.ToString(), null);
						MoveNext();
						if (base.Token == 118 || base.Token == 222)
						{
							syntaxAttribute.Value = base.TokenString;
							MoveNext();
						}
						else
						{
							result = false;
						}
						AddAttribute(syntaxAttribute);
						break;
					}
					}
					continue;
				}
				result = false;
				break;
			}
			while (base.Token == 190);
		}
		else
		{
			result = false;
		}
		current.Range.EndPoint = prevPosition;
		return result;
	}

	protected virtual bool ParseInsertQuery()
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString.ToUpper(CultureInfo.InvariantCulture), 6, SyntaxNodeOptions.Indentation | SyntaxNodeOptions.Outlining);
		AddNode(syntaxNode);
		base.SyntaxTree.Push(syntaxNode);
		try
		{
			MoveNext();
			if (!Expected(SqlLexerToken.Into))
			{
				result = false;
			}
			syntaxNode.AddAttribute(new SyntaxAttribute(prevPosition, SyntaxConsts.DeclarationScope, null));
			Point tokenPosition = base.TokenPosition;
			if (ParseQualifiedIdentifier(out var identifier))
			{
				syntaxNode.AddAttribute(new SyntaxAttribute(tokenPosition, SqlNodeType.TableName.ToString(), identifier));
				if (!ParseOptionalColumnList())
				{
					result = false;
				}
				switch ((SqlLexerToken)base.Token)
				{
				case SqlLexerToken.Values:
					MoveNext();
					do
					{
						ISyntaxNode node = null;
						if (!Expected(SqlLexerToken.Open_parens) || !ParseExpressionList(ref node) || !Expected(SqlLexerToken.Close_parens))
						{
							result = false;
						}
						if (node != null)
						{
							syntaxNode.AddChild(node);
						}
						if (base.Token == 283)
						{
							MoveNext();
							continue;
						}
						break;
					}
					while (!base.Eof);
					break;
				case SqlLexerToken.Select:
					if (!ParseSelectQuery())
					{
						result = false;
					}
					break;
				case SqlLexerToken.Set:
					MoveNext();
					if (!ParseSetStatement())
					{
						result = false;
					}
					break;
				default:
					result = false;
					SyntaxError();
					break;
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

	protected virtual bool ParseOptionalColumnList()
	{
		bool flag = true;
		if (base.Token == 264)
		{
			MoveNext();
			Point tokenPosition = base.TokenPosition;
			flag = ParseIdentifierList(out var list);
			if (flag)
			{
				AddAttribute(new SyntaxAttribute(tokenPosition, SqlNodeType.ColumnList.ToString(), list));
			}
			if (!Expected(SqlLexerToken.Close_parens))
			{
				flag = false;
			}
		}
		return flag;
	}

	protected virtual bool ParseDeleteQuery()
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString.ToUpper(CultureInfo.InvariantCulture), 8, SyntaxNodeOptions.Indentation | SyntaxNodeOptions.Outlining);
		AddNode(syntaxNode);
		base.SyntaxTree.Push(syntaxNode);
		try
		{
			MoveNext();
			if (!Expected(SqlLexerToken.From))
			{
				result = false;
			}
			syntaxNode.AddAttribute(new SyntaxAttribute(prevPosition, SyntaxConsts.DeclarationScope, null));
			Point tokenPosition = base.TokenPosition;
			if (ParseQualifiedIdentifier(out var identifier))
			{
				syntaxNode.AddAttribute(new SyntaxAttribute(tokenPosition, SqlNodeType.TableName.ToString(), identifier));
				if (base.Token == 235 && !ParseWhereStatement())
				{
					result = false;
				}
				if (base.Token == 116 && !ParseLimitStatement())
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

	protected virtual bool ParseWhereStatement()
	{
		bool flag = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString, 37);
		AddNode(syntaxNode);
		MoveNext();
		ISyntaxNode node = null;
		flag = ParseExpression(ref node);
		if (node != null)
		{
			syntaxNode.AddChild(node);
		}
		syntaxNode.Range.EndPoint = prevPosition;
		return flag;
	}

	protected virtual bool ParseLimitStatement()
	{
		bool flag = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString, 38);
		AddNode(syntaxNode);
		MoveNext();
		ISyntaxNode node = null;
		flag = ParseExpression(ref node);
		if (node != null)
		{
			syntaxNode.AddChild(node);
		}
		syntaxNode.Range.EndPoint = prevPosition;
		return flag;
	}

	protected virtual bool ParseCompactQuery()
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString.ToUpper(CultureInfo.InvariantCulture), 15, SyntaxNodeOptions.Indentation);
		AddNode(syntaxNode);
		base.SyntaxTree.Push(syntaxNode);
		try
		{
			MoveNext();
			if (!Expected(SqlLexerToken.Table))
			{
				result = false;
			}
			Point tokenPosition = base.TokenPosition;
			if (ParseQualifiedIdentifier(out var identifier))
			{
				syntaxNode.AddAttribute(new SyntaxAttribute(tokenPosition, SqlNodeType.TableName.ToString(), identifier));
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

	protected virtual bool ParseUpdateQuery()
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString.ToUpper(CultureInfo.InvariantCulture), 7, SyntaxNodeOptions.Indentation | SyntaxNodeOptions.Outlining);
		AddNode(syntaxNode);
		base.SyntaxTree.Push(syntaxNode);
		try
		{
			MoveNext();
			syntaxNode.AddAttribute(new SyntaxAttribute(prevPosition, SyntaxConsts.DeclarationScope, null));
			Point tokenPosition = base.TokenPosition;
			if (ParseQualifiedIdentifier(out var identifier))
			{
				syntaxNode.AddAttribute(new SyntaxAttribute(tokenPosition, SqlNodeType.TableName.ToString(), identifier));
				if (Expected(190))
				{
					if (!ParseSetStatement())
					{
						result = false;
					}
				}
				else
				{
					result = false;
				}
				if (base.Token == 235 && !ParseWhereStatement())
				{
					result = false;
				}
				if (base.Token == 116 && !ParseLimitStatement())
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

	protected virtual bool ParseSetStatement()
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString, 47);
		AddNode(syntaxNode);
		base.SyntaxTree.Push(syntaxNode);
		try
		{
			do
			{
				Point tokenPosition = base.TokenPosition;
				if (ParseQualifiedIdentifier(out var identifier))
				{
					SyntaxNode syntaxNode2 = new SyntaxNode(tokenPosition, identifier, 71);
					AddNode(syntaxNode2);
					ISyntaxNode node = null;
					if (!Expected(SqlLexerToken.Op_Assign) || !ParseExpression(ref node))
					{
						result = false;
					}
					if (node != null)
					{
						syntaxNode2.AddChild(node);
					}
					syntaxNode2.Range.EndPoint = prevPosition;
					if (base.Token != 283)
					{
						break;
					}
					MoveNext();
				}
			}
			while (!base.Eof);
		}
		finally
		{
			base.SyntaxTree.Pop();
		}
		syntaxNode.Range.EndPoint = prevPosition;
		return result;
	}

	protected virtual bool ParseCommitQuery()
	{
		AddNode(new SyntaxNode(base.TokenPosition, base.TokenString.ToUpper(CultureInfo.InvariantCulture), 14, SyntaxNodeOptions.Indentation));
		MoveNext();
		return true;
	}

	protected virtual bool ParseRollbackQuery()
	{
		AddNode(new SyntaxNode(base.TokenPosition, base.TokenString.ToUpper(CultureInfo.InvariantCulture), 16, SyntaxNodeOptions.Indentation));
		MoveNext();
		return true;
	}

	protected virtual bool ParseShutdownQuery()
	{
		AddNode(new SyntaxNode(base.TokenPosition, base.TokenString.ToUpper(CultureInfo.InvariantCulture), 17, SyntaxNodeOptions.Indentation));
		MoveNext();
		return true;
	}

	protected virtual bool ParseGrantQuery()
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString.ToUpper(CultureInfo.InvariantCulture), 12, SyntaxNodeOptions.Indentation);
		AddNode(syntaxNode);
		base.SyntaxTree.Push(syntaxNode);
		try
		{
			MoveNext();
			Point tokenPosition = base.TokenPosition;
			if (ParseIdentifierList(out var list))
			{
				syntaxNode.AddAttribute(new SyntaxAttribute(tokenPosition, SqlNodeType.PrivilegesList.ToString(), list));
			}
			else
			{
				result = false;
			}
			if (!Expected(SqlLexerToken.On))
			{
				result = false;
			}
			tokenPosition = base.TokenPosition;
			if (ParseDatabaseObject(out var databaseObject))
			{
				syntaxNode.AddAttribute(new SyntaxAttribute(tokenPosition, SqlNodeType.DatabaseObjectAttribute.ToString(), databaseObject));
			}
			else
			{
				result = false;
			}
			if (!Expected(SqlLexerToken.To))
			{
				result = false;
			}
			tokenPosition = base.TokenPosition;
			if (ParseUserList(out list))
			{
				syntaxNode.AddAttribute(new SyntaxAttribute(tokenPosition, SqlNodeType.UserList.ToString(), list));
			}
			else
			{
				result = false;
			}
			if (base.Token == 237)
			{
				MoveNext();
				if (!Expected(SqlLexerToken.Grant) || !Expected(SqlLexerToken.Option))
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

	protected virtual bool ParseRevokeQuery()
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString.ToUpper(CultureInfo.InvariantCulture), 13, SyntaxNodeOptions.Indentation);
		AddNode(syntaxNode);
		base.SyntaxTree.Push(syntaxNode);
		try
		{
			MoveNext();
			if (base.Token == 88)
			{
				MoveNext();
				if (!Expected(SqlLexerToken.Option) || !Expected(SqlLexerToken.For))
				{
					result = false;
				}
			}
			Point tokenPosition = base.TokenPosition;
			if (ParseIdentifierList(out var list))
			{
				syntaxNode.AddAttribute(new SyntaxAttribute(tokenPosition, SqlNodeType.PrivilegesList.ToString(), list));
			}
			else
			{
				result = false;
			}
			if (!Expected(SqlLexerToken.On))
			{
				result = false;
			}
			tokenPosition = base.TokenPosition;
			if (ParseDatabaseObject(out var databaseObject))
			{
				syntaxNode.AddAttribute(new SyntaxAttribute(tokenPosition, SqlNodeType.DatabaseObjectAttribute.ToString(), databaseObject));
			}
			else
			{
				result = false;
			}
			if (!Expected(SqlLexerToken.From))
			{
				result = false;
			}
			tokenPosition = base.TokenPosition;
			if (ParseUserList(out list))
			{
				syntaxNode.AddAttribute(new SyntaxAttribute(tokenPosition, SqlNodeType.UserList.ToString(), list));
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

	protected virtual bool ParseDatabaseObject(out string databaseObject)
	{
		bool flag = true;
		databaseObject = string.Empty;
		SqlLexerToken sqlLexerToken = (SqlLexerToken)base.Token;
		if (sqlLexerToken == SqlLexerToken.Schema || sqlLexerToken == SqlLexerToken.Table)
		{
			databaseObject = base.TokenString.ToUpper(CultureInfo.InvariantCulture);
			MoveNext();
		}
		flag = ParseQualifiedIdentifier(out var identifier);
		if (flag)
		{
			databaseObject = ((databaseObject == string.Empty) ? identifier : (databaseObject + " " + identifier));
		}
		return flag;
	}

	protected virtual bool ParseUser(out string identifier)
	{
		if (base.Token == 160)
		{
			identifier = base.TokenString;
			MoveNext();
			return true;
		}
		return ParseQualifiedIdentifier(out identifier);
	}

	protected virtual bool ParseUserList(out string list)
	{
		bool flag = ParseUser(out list);
		if (flag)
		{
			while (base.Token == 283)
			{
				list += base.TokenString;
				MoveNext();
				if (ParseUser(out var identifier))
				{
					list += identifier;
				}
				else
				{
					flag = false;
				}
			}
		}
		return flag;
	}

	protected virtual bool ParseSetQuery()
	{
		bool flag = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString.ToUpper(CultureInfo.InvariantCulture), 9, SyntaxNodeOptions.Indentation);
		AddNode(syntaxNode);
		base.SyntaxTree.Push(syntaxNode);
		try
		{
			MoveNext();
			switch ((SqlLexerToken)base.Token)
			{
			case SqlLexerToken.Auto:
			{
				Point tokenPosition2 = base.TokenPosition;
				MoveNext();
				flag = Expected(SqlLexerToken.Commit);
				if (flag)
				{
					ISyntaxAttribute syntaxAttribute = new SyntaxAttribute(tokenPosition2, SqlNodeType.AutoCommitAttribute.ToString(), null);
					if (base.Token == 141 || base.Token == 139)
					{
						syntaxAttribute.Value = base.TokenString;
						syntaxNode.AddAttribute(syntaxAttribute);
						MoveNext();
					}
					else
					{
						SyntaxError();
						flag = false;
					}
				}
				break;
			}
			case SqlLexerToken.Transaction:
			{
				Point tokenPosition3 = base.TokenPosition;
				MoveNext();
				flag = Expected(SqlLexerToken.Isolation) && Expected(SqlLexerToken.Level);
				if (flag)
				{
					ISyntaxAttribute syntaxAttribute2 = new SyntaxAttribute(tokenPosition3, SqlNodeType.IsolationLevelAttribute.ToString(), null);
					if (base.Token == 188)
					{
						syntaxAttribute2.Value = base.TokenString;
						syntaxNode.AddAttribute(syntaxAttribute2);
						MoveNext();
					}
					else
					{
						SyntaxError();
						flag = false;
					}
				}
				break;
			}
			case SqlLexerToken.Schema:
			{
				Point tokenPosition4 = base.TokenPosition;
				MoveNext();
				if (ParseQualifiedIdentifier(out var identifier2))
				{
					syntaxNode.AddAttribute(new SyntaxAttribute(tokenPosition4, SqlNodeType.SchemaName.ToString(), identifier2));
				}
				else
				{
					flag = false;
				}
				break;
			}
			default:
			{
				Point tokenPosition = base.TokenPosition;
				if (ParseQualifiedIdentifier(out var identifier))
				{
					syntaxNode.AddAttribute(new SyntaxAttribute(tokenPosition, SqlNodeType.VariableName.ToString(), identifier));
					ISyntaxNode node = null;
					if (!Expected(SqlLexerToken.Op_Assign) || !ParseExpression(ref node))
					{
						flag = false;
					}
					if (node != null)
					{
						syntaxNode.AddChild(node);
					}
				}
				else
				{
					flag = false;
				}
				break;
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

	protected virtual bool ParseDescribeQuery()
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString.ToUpper(CultureInfo.InvariantCulture), 11, SyntaxNodeOptions.Indentation);
		AddNode(syntaxNode);
		base.SyntaxTree.Push(syntaxNode);
		try
		{
			MoveNext();
			Point tokenPosition = base.TokenPosition;
			if (ParseQualifiedIdentifier(out var identifier))
			{
				syntaxNode.AddAttribute(new SyntaxAttribute(tokenPosition, SqlNodeType.TableName.ToString(), identifier));
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

	protected virtual bool ParseShowQuery()
	{
		bool result = true;
		ISyntaxNode syntaxNode = new SyntaxNode(base.TokenPosition, base.TokenString.ToUpper(CultureInfo.InvariantCulture), 10, SyntaxNodeOptions.Indentation);
		AddNode(syntaxNode);
		base.SyntaxTree.Push(syntaxNode);
		try
		{
			MoveNext();
			switch ((SqlLexerToken)base.Token)
			{
			case SqlLexerToken.Connections:
			case SqlLexerToken.Schema:
			case SqlLexerToken.Status:
			case SqlLexerToken.Tables:
				syntaxNode.AddAttribute(new SyntaxAttribute(base.TokenPosition, SqlNodeType.Attribute.ToString(), base.TokenString));
				MoveNext();
				break;
			default:
				result = false;
				SyntaxError();
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

	public override void ResetOptions()
	{
		Options = SyntaxConsts.DefaultNetSyntaxOptions;
	}

	protected virtual bool ParseUnit()
	{
		bool flag = true;
		ISyntaxNode root = base.SyntaxTree.Root;
		root.NodeType = 1;
		root.Position = Point.Empty;
		flag = ParseUnitBody();
		base.SyntaxTree.Root.Range.EndPoint = prevPosition;
		if (!ClearStack())
		{
			flag = false;
		}
		return flag;
	}

	protected virtual bool ClearStack()
	{
		bool result = true;
		ISyntaxNode current = base.SyntaxTree.Current;
		while (current != base.SyntaxTree.Root)
		{
			ISyntaxError syntaxError = new SyntaxError(current.Position, current.Name, StringConsts.ErrEndOfFileFound);
			syntaxError.Range.EndPoint = new Point(current.Position.X + 1, current.Position.Y);
			current.AddError(syntaxError);
			current.Range.EndPoint = prevPosition;
			base.SyntaxTree.Pop();
			current = base.SyntaxTree.Current;
			result = false;
		}
		return result;
	}

	public override void ReparseText()
	{
		Reset();
		MoveNext();
		ParseUnit();
		base.ReparseText();
	}
}
