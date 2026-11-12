using System;
using System.Collections.Generic;
using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

internal class Scanner
{
	private class KeywordLookup
	{
		private readonly KeywordData[] keywords;

		public KeywordData this[StringToken token] => keywords[(int)token];

		public KeywordLookup(KeywordData[] keywords)
		{
			this.keywords = keywords;
		}
	}

	private class KeywordData
	{
		public readonly Tokens Token;

		public readonly int CompatibilityLevel;

		public KeywordData(Tokens token, int compatLevel)
		{
			Token = token;
			CompatibilityLevel = compatLevel;
		}
	}

	private struct SpecialTokenIntepretation
	{
		private bool IsCreate;

		private bool IsExternal;

		private bool IsData;

		private bool IsFile;

		private bool IsAlter;

		private bool IsDrop;

		public void clear()
		{
			IsCreate = false;
			IsExternal = false;
			IsData = false;
			IsFile = false;
			IsAlter = false;
			IsDrop = false;
		}

		public bool IsCreateOrDrop()
		{
			if (!IsCreate)
			{
				return IsDrop;
			}
			return true;
		}

		public bool IsCreateOrAlterOrDrop()
		{
			if (!IsCreateOrDrop())
			{
				return IsAlter;
			}
			return true;
		}

		public bool IsCreateOrDropExternal()
		{
			if (IsCreateOrDrop())
			{
				return IsExternal;
			}
			return false;
		}

		public bool IsCreateOrAlterOrDropExternal()
		{
			if (IsCreateOrAlterOrDrop())
			{
				return IsExternal;
			}
			return false;
		}

		public bool IsCreateOrAlterOrDropExternalData()
		{
			if (IsCreateOrAlterOrDropExternal())
			{
				return IsData;
			}
			return false;
		}

		public bool IsCreateOrDropExternalFile()
		{
			if (IsCreateOrDropExternal())
			{
				return IsFile;
			}
			return false;
		}

		public bool Update(Tokens token)
		{
			if (Tokens.TOKEN_CREATE == token)
			{
				clear();
				IsCreate = true;
			}
			else if (Tokens.TOKEN_ALTER == token)
			{
				clear();
				IsAlter = true;
			}
			else if (Tokens.TOKEN_DROP == token)
			{
				clear();
				IsDrop = true;
			}
			else if (Tokens.TOKEN_EXTERNAL == token)
			{
				if (!IsCreateOrAlterOrDrop())
				{
					clear();
					return false;
				}
				IsExternal = true;
			}
			else if (Tokens.TOKEN_FILE == token)
			{
				if (!IsCreateOrDropExternal())
				{
					clear();
					return false;
				}
				IsFile = true;
			}
			else if (Tokens.TOKEN_s_ET_DATASOURCE == token)
			{
				if (!IsCreateOrAlterOrDropExternal())
				{
					clear();
					return false;
				}
				IsData = true;
			}
			else if (Tokens.TOKEN_s_ET_FORMAT == token && IsCreateOrDropExternalFile())
			{
				clear();
			}
			else
			{
				if (Tokens.TOKEN_s_ET_SOURCE != token || !IsCreateOrAlterOrDropExternalData())
				{
					clear();
					return false;
				}
				clear();
			}
			return true;
		}
	}

	private static class CompareUtils
	{
		public static bool KeywordEquals(string keyword, string buffer, int startIndex, int length)
		{
			if (keyword.Length != length)
			{
				return false;
			}
			for (int i = 0; i < keyword.Length; i++)
			{
				char c = keyword[i];
				char c2 = buffer[i + startIndex];
				if (c2 != c && char.ToUpperInvariant(c2) != c)
				{
					return false;
				}
			}
			return true;
		}
	}

	private enum EntryStateValues
	{
		Base,
		TicString,
		Quoted,
		Bracketed,
		BinaryConstant
	}

	private enum QuotedIdentifierStateValues
	{
		QuotedIdentifierOn,
		QuotedIdentifierOff
	}

	private enum QuotedIdentifierSubStateValues
	{
		Base,
		AfterSet,
		AfterQuotedIdentifier
	}

	private enum ExecSubStateValues
	{
		Base,
		AfterExec,
		AfterVariable,
		BeforeFullIdentifier,
		AfterFullIdentifier
	}

	private abstract class ScannerErrorAction
	{
		public const int MAX_REPORTED_TOKEN_LENGTH = 256;

		public abstract void ReportError(ErrorHandler errorHandler, TokenManager tokenManager);
	}

	private sealed class UnclosedQuotationMarkScannerErrorAction : ScannerErrorAction
	{
		public readonly int tokenIndex;

		public UnclosedQuotationMarkScannerErrorAction(int tokenIndex)
		{
			this.tokenIndex = tokenIndex;
		}

		public override void ReportError(ErrorHandler errorHandler, TokenManager tokenManager)
		{
			string text = tokenManager.GetText(tokenIndex);
			if (text.Length > 256)
			{
				text = text.Substring(0, 256) + "...";
			}
			errorHandler.AddError(SR.UnclosedQuotationMark(text), ErrorType.SyntaxError, ErrorFlags.None, new LexLocation(tokenIndex, tokenIndex + 1));
		}
	}

	private sealed class ScanSourceHelper
	{
		private readonly ErrorHandler errorHandler;

		private string source;

		private MinimalList<TokenData> tokens;

		private List<int> batchSeparators;

		private List<int> newLines;

		private int lastTokenId;

		public ScanSourceHelper(ErrorHandler errorHandler)
		{
			this.errorHandler = errorHandler;
		}

		public TokenManager ScanSource(string source, ParseOptions options, out ParseOptions resultOptions)
		{
			this.source = source;
			Scanner scanner = new Scanner(options);
			scanner.SetSource(source, 0);
			int initialCapacity = source.Length / 6 + 1;
			int num = source.Length / 25 + 1;
			int capacity = ((!(options.BatchSeparator != string.Empty)) ? 1 : (num / 80 + 1));
			tokens = new MinimalList<TokenData>(initialCapacity);
			newLines = new List<int>(num);
			batchSeparators = new List<int>(capacity);
			List<ScannerErrorAction> list = null;
			while (true)
			{
				int num2 = scanner.Scan();
				int atomStartIndex = scanner.atomStartIndex;
				while (IsAccumulatorToken(num2))
				{
					int accumulatorId = num2;
					num2 = scanner.Scan();
					if (num2 == 128)
					{
						int terminatorId = GetTerminatorId(accumulatorId);
						int tokenIndex = RecordToken(terminatorId, atomStartIndex);
						if (list == null)
						{
							list = new List<ScannerErrorAction>();
						}
						list.Add(new UnclosedQuotationMarkScannerErrorAction(tokenIndex));
						break;
					}
				}
				if (num2 == 128)
				{
					break;
				}
				RecordToken(num2, atomStartIndex);
			}
			PopulateNewlines();
			resultOptions = new ParseOptions(options);
			resultOptions.IsQuotedIdentifierSet = scanner.IsQuotedIdentifierSet;
			TokenManager tokenManager = new TokenManager(source, tokens, newLines, batchSeparators, scanner.compatibilityLevel);
			if (list != null)
			{
				foreach (ScannerErrorAction item in list)
				{
					item.ReportError(errorHandler, tokenManager);
				}
			}
			return tokenManager;
		}

		private int RecordToken(int id, int startIndex)
		{
			if (!TokenManager.IsSignificantToken(id) && id == lastTokenId)
			{
				return tokens.Count - 1;
			}
			tokens.Add(new TokenData(id, startIndex));
			lastTokenId = id;
			if (id == 989)
			{
				batchSeparators.Add(tokens.Count - 1);
			}
			return tokens.Count - 1;
		}

		private void PopulateNewlines()
		{
			newLines.Add(-1);
			for (int i = 0; i < source.Length - 1; i++)
			{
				switch (source[i])
				{
				case '\r':
					if (source[i + 1] != '\n')
					{
						newLines.Add(i);
					}
					break;
				case '\n':
					newLines.Add(i);
					break;
				}
			}
			if (source.Length > 0)
			{
				char c = source[source.Length - 1];
				if (c == '\r' || c == '\n')
				{
					newLines.Add(source.Length - 1);
				}
			}
			newLines.Add(source.Length + 1);
		}
	}

	private delegate bool AdvancePredicate(char c);

	private enum CharacterClass
	{
		P_ALPHA,
		P_ALPHA_NON_FIRST,
		P_EXOTIC_MONEY,
		P_IGSPACE,
		P_MONEY,
		P_NUMB,
		P_OPER,
		P_STRING
	}

	private static KeywordLookup keywords80;

	private static KeywordLookup keywords90;

	private static KeywordLookup keywords100;

	private static KeywordLookup keywords110;

	private static KeywordLookup keywords120;

	private static KeywordLookup keywords130;

	private static KeywordLookup keywords140;

	private static KeywordLookup keywords150;

	private static KeywordLookup keywords160;

	private static KeywordLookup keywordsAzure;

	private const int EntryStateSize = 3;

	private const int QuotedIdentifierStateSize = 1;

	private const int CommentLevelSize = 5;

	private const int QuotedIdentifierSubStateSize = 2;

	private const int ExecSubStateSize = 3;

	private const int ReservedSize = 18;

	private const int MaxNumEntryStates = 8;

	private const int MaxNumQuotedIdentifierStates = 2;

	private const int MaxNumCommentLevels = 32;

	private const int MaxNumQuotedIdentifierSubStates = 4;

	private const int MaxNumExecSubStates = 8;

	private const int MaxIndexEntryState = 7;

	private const int MaxIndexQuotedIdentifierState = 1;

	private const int MaxIndexCommentLevel = 31;

	private const int MaxIndexQuotedIdentifierSubState = 3;

	private const int MaxIndexExecSubState = 7;

	private const int EntryStateOffset = 0;

	private const int QuotedIdentifierStateOffset = 3;

	private const int CommentLevelOffset = 4;

	private const int QuotedIdentifierSubStateOffset = 9;

	private const int ExecSubStateOffset = 11;

	private const int ReservedOffset = 14;

	private const int EntryStateMask = 7;

	private const int QuotedIdentifierStateMask = 8;

	private const int CommentLevelMask = 496;

	private const int QuotedIdentifierSubStateMask = 1536;

	private const int ExecSubStateMask = 14336;

	private const int ReservedMask = -16384;

	private string source;

	private int atomStartIndex;

	private int atomEndIndex;

	private int state;

	private readonly string batchSeparator;

	private readonly int compatibilityLevel;

	private readonly KeywordLookup Keywords;

	private SpecialTokenIntepretation tokenContext;

	private readonly AdvancePredicate IsCloseBracketPredicate = IsCloseBracket;

	private readonly AdvancePredicate IsCurrencyPredicate = IsCurrency;

	private readonly AdvancePredicate IsDigitPredicate = IsDigit;

	private readonly AdvancePredicate IsHexadecimalPredicate = IsHexadecimal;

	private readonly AdvancePredicate IsIdentifierStartPredicate = IsIdentifierStart;

	private readonly AdvancePredicate IsIdentifierCharacterPredicate = IsIdentifierCharacter;

	private readonly AdvancePredicate IsQuotePredicate = IsQuote;

	private readonly AdvancePredicate IsSpacePredicate = IsSpace;

	private readonly AdvancePredicate IsWhiteSpacePredicate = IsWhiteSpace;

	private readonly AdvancePredicate NotStarOrSlashPredicate = NotStarOrSlash;

	private readonly AdvancePredicate ToEOLPredicate = ToEOL;

	private static List<int> characterMapKeys;

	private static List<CharacterClass> characterMapValues;

	private EntryStateValues EntryState
	{
		get
		{
			return (EntryStateValues)(state & 7);
		}
		set
		{
			state &= -8;
			state |= (int)value;
		}
	}

	private QuotedIdentifierStateValues QuotedIdentifierState
	{
		get
		{
			return (QuotedIdentifierStateValues)((state & 8) >> 3);
		}
		set
		{
			state &= -9;
			state |= (int)value << 3;
		}
	}

	private uint CommentLevel
	{
		get
		{
			return (uint)((state & 0x1F0) >> 4);
		}
		set
		{
			state &= -497;
			state |= (int)(value << 4);
		}
	}

	private QuotedIdentifierSubStateValues QuotedIdentifierSubState
	{
		get
		{
			return (QuotedIdentifierSubStateValues)((state & 0x600) >> 9);
		}
		set
		{
			state &= -1537;
			state |= (int)value << 9;
		}
	}

	private ExecSubStateValues ExecSubState
	{
		get
		{
			return (ExecSubStateValues)((state & 0x3800) >> 11);
		}
		set
		{
			state &= -14337;
			state |= (int)value << 11;
		}
	}

	private bool IsQuotedIdentifierSet => QuotedIdentifierState == QuotedIdentifierStateValues.QuotedIdentifierOn;

	private static KeywordData[] CreateKeywordList80(bool includeDep)
	{
		KeywordData[] array = new KeywordData[882];
		array[33] = new KeywordData(Tokens.TOKEN_ADD, 60);
		array[34] = new KeywordData(Tokens.TOKEN_ALL, 42);
		array[141] = new KeywordData(Tokens.TOKEN_ALTER, 42);
		array[35] = new KeywordData(Tokens.TOKEN_AND, 42);
		array[36] = new KeywordData(Tokens.TOKEN_ANY, 42);
		array[8] = new KeywordData(Tokens.TOKEN_AS, 42);
		array[37] = new KeywordData(Tokens.TOKEN_ASC, 42);
		array[601] = new KeywordData(Tokens.TOKEN_AUTHORIZATION, 65);
		array[202] = new KeywordData(Tokens.TOKEN_DUMP, 70);
		array[145] = new KeywordData(Tokens.TOKEN_BEGIN, 42);
		array[279] = new KeywordData(Tokens.TOKEN_BETWEEN, 42);
		array[147] = new KeywordData(Tokens.TOKEN_BREAK, 42);
		array[207] = new KeywordData(Tokens.TOKEN_BROWSE, 42);
		array[64] = new KeywordData(Tokens.TOKEN_BULK, 42);
		array[10] = new KeywordData(Tokens.TOKEN_BY, 42);
		array[280] = new KeywordData(Tokens.TOKEN_CASCADE, 65);
		array[65] = new KeywordData(Tokens.TOKEN_CASE, 60);
		array[151] = new KeywordData(Tokens.TOKEN_CHECK, 42);
		array[468] = new KeywordData(Tokens.TOKEN_CHECKPOINT, 42);
		array[152] = new KeywordData(Tokens.TOKEN_CLOSE, 60);
		array[410] = new KeywordData(Tokens.TOKEN_CLUSTERED, 42);
		array[347] = new KeywordData(Tokens.TOKEN_COALESCE, 60);
		array[282] = new KeywordData(Tokens.TOKEN_COLLATE, 80);
		array[211] = new KeywordData(Tokens.TOKEN_COLUMN, 70);
		array[212] = new KeywordData(Tokens.TOKEN_COMMIT, 42);
		array[283] = new KeywordData(Tokens.TOKEN_COMPUTE, 42);
		array[470] = new KeywordData(Tokens.TOKEN_CONSTRAINT, 60);
		array[348] = new KeywordData(Tokens.TOKEN_CONTAINS, 70);
		array[604] = new KeywordData(Tokens.TOKEN_CONTAINSTABLE, 70);
		array[349] = new KeywordData(Tokens.TOKEN_CONTINUE, 42);
		array[286] = new KeywordData(Tokens.TOKEN_CONVERT, 42);
		array[213] = new KeywordData(Tokens.TOKEN_CREATE, 42);
		array[153] = new KeywordData(Tokens.TOKEN_CROSS, 65);
		array[288] = new KeywordData(Tokens.TOKEN_CURRENT, 60);
		array[564] = new KeywordData(Tokens.TOKEN_CURRENT_DATE, 60);
		array[565] = new KeywordData(Tokens.TOKEN_CURRENT_TIME, 60);
		array[731] = new KeywordData(Tokens.TOKEN_CURRENT_TIMESTAMP, 60);
		array[566] = new KeywordData(Tokens.TOKEN_CURRENT_USER, 60);
		array[214] = new KeywordData(Tokens.TOKEN_CURSOR, 60);
		array[352] = new KeywordData(Tokens.TOKEN_DATABASE, 42);
		array[72] = new KeywordData(Tokens.TOKEN_DBCC, 42);
		array[472] = new KeywordData(Tokens.TOKEN_DEALLOCATE, 60);
		array[290] = new KeywordData(Tokens.TOKEN_DECLARE, 42);
		array[291] = new KeywordData(Tokens.TOKEN_DEFAULT, 42);
		array[216] = new KeywordData(Tokens.TOKEN_DELETE, 42);
		array[74] = new KeywordData(Tokens.TOKEN_DENY, 60);
		array[75] = new KeywordData(Tokens.TOKEN_DESC, 42);
		array[354] = new KeywordData(Tokens.TOKEN_DISTINCT, 42);
		array[523] = new KeywordData(Tokens.TOKEN_DISTRIBUTED, 65);
		array[218] = new KeywordData(Tokens.TOKEN_DOUBLE, 60);
		array[76] = new KeywordData(Tokens.TOKEN_DROP, 42);
		array[79] = new KeywordData(Tokens.TOKEN_ELSE, 42);
		array[42] = new KeywordData(Tokens.TOKEN_END, 42);
		array[220] = new KeywordData(Tokens.TOKEN_ERRLVL, 42);
		array[221] = new KeywordData(Tokens.TOKEN_ESCAPE, 65);
		array[222] = new KeywordData(Tokens.TOKEN_EXCEPT, 42);
		array[80] = new KeywordData(Tokens.TOKEN_EXECUTE, 42);
		array[294] = new KeywordData(Tokens.TOKEN_EXECUTE, 42);
		array[223] = new KeywordData(Tokens.TOKEN_EXISTS, 42);
		array[81] = new KeywordData(Tokens.TOKEN_EXIT, 42);
		array[158] = new KeywordData(Tokens.TOKEN_FETCH, 60);
		array[82] = new KeywordData(Tokens.TOKEN_FILE, 42);
		array[482] = new KeywordData(Tokens.TOKEN_FILLFACTOR, 42);
		array[43] = new KeywordData(Tokens.TOKEN_FOR, 42);
		array[295] = new KeywordData(Tokens.TOKEN_FOREIGN, 60);
		array[360] = new KeywordData(Tokens.TOKEN_FREETEXT, 70);
		array[621] = new KeywordData(Tokens.TOKEN_FREETEXTTABLE, 70);
		array[83] = new KeywordData(Tokens.TOKEN_FROM, 42);
		array[84] = new KeywordData(Tokens.TOKEN_FULL, 65);
		array[362] = new KeywordData(Tokens.TOKEN_FUNCTION, 80);
		array[85] = new KeywordData(Tokens.TOKEN_GOTO, 42);
		array[161] = new KeywordData(Tokens.TOKEN_GRANT, 42);
		array[162] = new KeywordData(Tokens.TOKEN_GROUP, 42);
		array[229] = new KeywordData(Tokens.TOKEN_HAVING, 42);
		array[366] = new KeywordData(Tokens.TOKEN_HOLDLOCK, 42);
		array[367] = new KeywordData(Tokens.TOKEN_IDENTITY, 60);
		array[690] = new KeywordData(Tokens.TOKEN_IDENTITY_INSERT, 60);
		array[528] = new KeywordData(Tokens.TOKEN_IDENTITYCOL, 60);
		array[15] = new KeywordData(Tokens.TOKEN_IF, 42);
		array[16] = new KeywordData(Tokens.TOKEN_IN, 42);
		array[164] = new KeywordData(Tokens.TOKEN_INDEX, 42);
		array[165] = new KeywordData(Tokens.TOKEN_INNER, 65);
		array[231] = new KeywordData(Tokens.TOKEN_INSERT, 42);
		array[430] = new KeywordData(Tokens.TOKEN_INTERSECT, 42);
		array[91] = new KeywordData(Tokens.TOKEN_INTO, 42);
		array[17] = new KeywordData(Tokens.TOKEN_IS, 42);
		array[92] = new KeywordData(Tokens.TOKEN_JOIN, 65);
		array[46] = new KeywordData(Tokens.TOKEN_KEY, 60);
		array[94] = new KeywordData(Tokens.TOKEN_KILL, 42);
		array[95] = new KeywordData(Tokens.TOKEN_LEFT, 65);
		array[96] = new KeywordData(Tokens.TOKEN_LIKE, 42);
		array[235] = new KeywordData(Tokens.TOKEN_LINENO, 42);
		array[376] = new KeywordData(Tokens.TOKEN_NATIONAL, 70);
		array[306] = new KeywordData(Tokens.TOKEN_NOCHECK, 60);
		array[582] = new KeywordData(Tokens.TOKEN_NONCLUSTERED, 42);
		array[49] = new KeywordData(Tokens.TOKEN_NOT, 42);
		array[104] = new KeywordData(Tokens.TOKEN_NULL, 42);
		array[243] = new KeywordData(Tokens.TOKEN_NULLIF, 60);
		array[23] = new KeywordData(Tokens.TOKEN_OF, 60);
		array[50] = new KeywordData(Tokens.TOKEN_OFF, 42);
		array[308] = new KeywordData(Tokens.TOKEN_OFFSETS, 42);
		array[24] = new KeywordData(Tokens.TOKEN_ON, 42);
		array[106] = new KeywordData(Tokens.TOKEN_OPEN, 60);
		array[660] = new KeywordData(Tokens.TOKEN_OPENDATASOURCE, 70);
		array[435] = new KeywordData(Tokens.TOKEN_OPENQUERY, 70);
		array[485] = new KeywordData(Tokens.TOKEN_OPENROWSET, 70);
		array[309] = new KeywordData(Tokens.TOKEN_OPENXML, 80);
		array[246] = new KeywordData(Tokens.TOKEN_OPTION, 60);
		array[25] = new KeywordData(Tokens.TOKEN_OR, 42);
		array[178] = new KeywordData(Tokens.TOKEN_ORDER, 42);
		array[179] = new KeywordData(Tokens.TOKEN_OUTER, 65);
		array[107] = new KeywordData(Tokens.TOKEN_OVER, 42);
		array[313] = new KeywordData(Tokens.TOKEN_PERCENT, 70);
		array[110] = new KeywordData(Tokens.TOKEN_PLAN, 42);
		array[314] = new KeywordData(Tokens.TOKEN_PRIMARY, 60);
		array[184] = new KeywordData(Tokens.TOKEN_PRINT, 42);
		array[112] = new KeywordData(Tokens.TOKEN_PROCEDURE, 42);
		array[442] = new KeywordData(Tokens.TOKEN_PROCEDURE, 42);
		array[251] = new KeywordData(Tokens.TOKEN_PUBLIC, 42);
		array[443] = new KeywordData(Tokens.TOKEN_RAISERROR, 42);
		array[113] = new KeywordData(Tokens.TOKEN_READ, 60);
		array[388] = new KeywordData(Tokens.TOKEN_READTEXT, 42);
		array[537] = new KeywordData(Tokens.TOKEN_RECONFIGURE, 42);
		array[491] = new KeywordData(Tokens.TOKEN_REFERENCES, 60);
		array[539] = new KeywordData(Tokens.TOKEN_REPLICATION, 60);
		array[321] = new KeywordData(Tokens.TOKEN_LOAD, 70);
		array[390] = new KeywordData(Tokens.TOKEN_RESTRICT, 65);
		array[257] = new KeywordData(Tokens.TOKEN_RETURN, 42);
		array[259] = new KeywordData(Tokens.TOKEN_REVOKE, 42);
		array[187] = new KeywordData(Tokens.TOKEN_RIGHT, 65);
		array[391] = new KeywordData(Tokens.TOKEN_ROLLBACK, 42);
		array[392] = new KeywordData(Tokens.TOKEN_ROWCOUNT, 42);
		array[494] = new KeywordData(Tokens.TOKEN_ROWGUIDCOL, 70);
		array[117] = new KeywordData(Tokens.TOKEN_RULE, 42);
		array[118] = new KeywordData(Tokens.TOKEN_SAVE, 42);
		array[261] = new KeywordData(Tokens.TOKEN_SCHEMA, 65);
		array[266] = new KeywordData(Tokens.TOKEN_SELECT, 42);
		array[591] = new KeywordData(Tokens.TOKEN_SESSION_USER, 60);
		array[55] = new KeywordData(Tokens.TOKEN_SET, 42);
		array[326] = new KeywordData(Tokens.TOKEN_SETUSER, 42);
		array[395] = new KeywordData(Tokens.TOKEN_SHUTDOWN, 42);
		array[122] = new KeywordData(Tokens.TOKEN_ANY, 60);
		array[498] = new KeywordData(Tokens.TOKEN_STATISTICS, 42);
		array[547] = new KeywordData(Tokens.TOKEN_SYSTEM_USER, 60);
		array[191] = new KeywordData(Tokens.TOKEN_TABLE, 42);
		array[401] = new KeywordData(Tokens.TOKEN_TEXTSIZE, 42);
		array[125] = new KeywordData(Tokens.TOKEN_THEN, 60);
		array[28] = new KeywordData(Tokens.TOKEN_TO, 42);
		array[58] = new KeywordData(Tokens.TOKEN_TOP, 70);
		array[128] = new KeywordData(Tokens.TOKEN_TRANSACTION, 42);
		array[550] = new KeywordData(Tokens.TOKEN_TRANSACTION, 42);
		array[333] = new KeywordData(Tokens.TOKEN_TRIGGER, 42);
		array[402] = new KeywordData(Tokens.TOKEN_TRUNCATE, 42);
		array[334] = new KeywordData(Tokens.TOKEN_TSEQUAL, 42);
		array[193] = new KeywordData(Tokens.TOKEN_UNION, 42);
		array[274] = new KeywordData(Tokens.TOKEN_UNIQUE, 42);
		array[275] = new KeywordData(Tokens.TOKEN_UPDATE, 42);
		array[500] = new KeywordData(Tokens.TOKEN_UPDATETEXT, 60);
		array[61] = new KeywordData(Tokens.TOKEN_USEDB, 42);
		array[131] = new KeywordData(Tokens.TOKEN_USER, 60);
		array[276] = new KeywordData(Tokens.TOKEN_VALUES, 42);
		array[340] = new KeywordData(Tokens.TOKEN_VARYING, 60);
		array[132] = new KeywordData(Tokens.TOKEN_VIEW, 42);
		array[342] = new KeywordData(Tokens.TOKEN_WAITFOR, 42);
		array[134] = new KeywordData(Tokens.TOKEN_WHEN, 60);
		array[196] = new KeywordData(Tokens.TOKEN_WHERE, 42);
		array[596] = new KeywordData(Tokens.TOKEN_WHERE_CURRENT, 42);
		array[197] = new KeywordData(Tokens.TOKEN_WHILE, 42);
		array[135] = new KeywordData(Tokens.TOKEN_WITH, 42);
		array[462] = new KeywordData(Tokens.TOKEN_WRITETEXT, 42);
		if (includeDep)
		{
			array[77] = new KeywordData(Tokens.TOKEN_X_DUMP, 80);
			array[98] = new KeywordData(Tokens.TOKEN_X_LOAD, 80);
		}
		return array;
	}

	private static KeywordData[] CreateKeywordList90(bool includeDep)
	{
		KeywordData[] array = CreateKeywordList80(includeDep: false);
		array[358] = new KeywordData(Tokens.TOKEN_EXTERNAL, 90);
		array[183] = new KeywordData(Tokens.TOKEN_PIVOT, 90);
		array[258] = new KeywordData(Tokens.TOKEN_REVERT, 90);
		array[548] = new KeywordData(Tokens.TOKEN_TABLESAMPLE, 90);
		array[335] = new KeywordData(Tokens.TOKEN_UNPIVOT, 90);
		if (includeDep)
		{
			array[77] = new KeywordData(Tokens.TOKEN_X_DUMP, 90);
			array[98] = new KeywordData(Tokens.TOKEN_X_LOAD, 90);
		}
		return array;
	}

	private static KeywordData[] CreateKeywordList100(bool includeDep)
	{
		KeywordData[] array = CreateKeywordList90(includeDep: false);
		array[173] = new KeywordData(Tokens.TOKEN_MERGE, 100);
		return array;
	}

	private static KeywordData[] CreateKeywordList110(bool includeDep)
	{
		KeywordData[] array = CreateKeywordList100(includeDep: false);
		array[808] = new KeywordData(Tokens.TOKEN_SEMANTICKEYPHRASETABLE, 110);
		array[871] = new KeywordData(Tokens.TOKEN_SEMANTICSIMILARITYDETAILSTABLE, 110);
		array[825] = new KeywordData(Tokens.TOKEN_SEMANTICSIMILARITYTABLE, 110);
		array[551] = new KeywordData(Tokens.TOKEN_TRY_CONVERT, 110);
		return array;
	}

	private static KeywordData[] CreateKeywordList120(bool includeDep)
	{
		return CreateKeywordList110(includeDep: false);
	}

	private static KeywordData[] CreateKeywordList130(bool includeDep)
	{
		return CreateKeywordList120(includeDep: false);
	}

	private static KeywordData[] CreateKeywordList140(bool includeDep)
	{
		return CreateKeywordList130(includeDep: false);
	}

	private static KeywordData[] CreateKeywordList150(bool includeDep)
	{
		return CreateKeywordList140(includeDep: false);
	}

	private static KeywordData[] CreateKeywordList160(bool includeDep)
	{
		return CreateKeywordList150(includeDep: false);
	}

	public Scanner(ParseOptions options)
	{
		if (options == null)
		{
			throw new ArgumentNullException("options");
		}
		batchSeparator = options.BatchSeparator.ToUpperInvariant();
		state = 0;
		EntryState = EntryStateValues.Base;
		QuotedIdentifierState = ((!options.IsQuotedIdentifierSet) ? QuotedIdentifierStateValues.QuotedIdentifierOff : QuotedIdentifierStateValues.QuotedIdentifierOn);
		CommentLevel = 0u;
		QuotedIdentifierSubState = QuotedIdentifierSubStateValues.Base;
		ExecSubState = ExecSubStateValues.Base;
		switch (options.CompatibilityLevel)
		{
		case DatabaseCompatibilityLevel.Current:
		case DatabaseCompatibilityLevel.Azure:
			compatibilityLevel = 160;
			Keywords = keywordsAzure;
			break;
		case DatabaseCompatibilityLevel.Version160:
			compatibilityLevel = 160;
			Keywords = keywords160;
			break;
		case DatabaseCompatibilityLevel.Version150:
			compatibilityLevel = 150;
			Keywords = keywords150;
			break;
		case DatabaseCompatibilityLevel.Version140:
			compatibilityLevel = 140;
			Keywords = keywords140;
			break;
		case DatabaseCompatibilityLevel.Version130:
			compatibilityLevel = 130;
			Keywords = keywords130;
			break;
		case DatabaseCompatibilityLevel.Version120:
			compatibilityLevel = 120;
			Keywords = keywords120;
			break;
		case DatabaseCompatibilityLevel.Version110:
			compatibilityLevel = 110;
			Keywords = keywords110;
			break;
		case DatabaseCompatibilityLevel.Version100:
			compatibilityLevel = 100;
			Keywords = keywords100;
			break;
		case DatabaseCompatibilityLevel.Version90:
			compatibilityLevel = 90;
			Keywords = keywords90;
			break;
		case DatabaseCompatibilityLevel.Version80:
			compatibilityLevel = 80;
			Keywords = keywords80;
			break;
		default:
			compatibilityLevel = 100;
			Keywords = keywords100;
			break;
		}
	}

	private Tokens GetIdToken(string buffer, int startIndex, int length)
	{
		StringToken stringToken = StringTokenLookup.Find(buffer, startIndex, length);
		KeywordData keywordData = Keywords[stringToken];
		bool flag = false;
		Tokens tokens;
		Tokens tokens2;
		if (keywordData != null)
		{
			tokens = keywordData.Token;
			tokens2 = tokens;
		}
		else
		{
			tokens = Tokens.TOKEN_ID;
			tokens2 = tokens;
			if (buffer[startIndex] == '@')
			{
				tokens = Tokens.TOKEN_VARIABLE;
				if (length > 1 && buffer[startIndex + 1] == '@')
				{
					StringToken stringToken2 = StringTokenLookup.Find(buffer, startIndex + 2, length - 2);
					int tokenId = 0;
					if (OptLookup.OptGlobalVariable.TryGetTokenId(stringToken2, ref tokenId))
					{
						tokens = Tokens.TOKEN_GLOBAL_VAR;
					}
				}
			}
			else if (batchSeparator != string.Empty && CompareUtils.KeywordEquals(batchSeparator, buffer, startIndex, length))
			{
				tokens = Tokens.LEX_BATCH_SEPERATOR;
			}
			else if (tokenContext.IsCreateOrAlterOrDropExternal() || tokenContext.IsCreateOrAlterOrDropExternalData() || tokenContext.IsCreateOrDropExternalFile())
			{
				int tokenId2 = 0;
				if (OptLookup.OPT_EXTERNAL_TYPE_SUBTOKENS.TryGetTokenId(stringToken, ref tokenId2))
				{
					flag = true;
					tokens = (Tokens)tokenId2;
				}
			}
		}
		if (!tokenContext.Update(tokens) && flag)
		{
			tokens = tokens2;
		}
		return tokens;
	}

	public void SetSource(string source, int position)
	{
		this.source = source;
		atomStartIndex = position - 1;
		atomEndIndex = position;
	}

	public int GetNext(ref int state, out int start, out int end)
	{
		this.state = state;
		int result = Scan();
		start = atomStartIndex;
		end = atomEndIndex;
		if (end > start)
		{
			end--;
		}
		state = this.state;
		return result;
	}

	internal int GetNext(ref int state, out int start, out int end, out bool isPairMatch, out bool isExecAutoParamHelp)
	{
		isExecAutoParamHelp = false;
		if (ExecSubStateValues.AfterFullIdentifier == ExecSubState)
		{
			isExecAutoParamHelp = true;
		}
		int num = GetNext(ref state, out start, out end);
		isPairMatch = false;
		switch (num)
		{
		case 40:
		case 41:
		case 206:
		case 233:
			isPairMatch = true;
			break;
		case 173:
		{
			int num2 = end - start + 1;
			if (num2 == 3 && StringTokenLookup.Find(source.Substring(start, 3)) == StringToken.Try)
			{
				isPairMatch = true;
				num = 374;
			}
			else if (num2 == 5 && StringTokenLookup.Find(source.Substring(start, 5)) == StringToken.Catch)
			{
				isPairMatch = true;
				num = 373;
			}
			break;
		}
		}
		if (!isPairMatch && source.Length > atomEndIndex)
		{
			char c = source[atomEndIndex];
			isPairMatch = c == '(';
		}
		if (isExecAutoParamHelp && ExecSubStateValues.BeforeFullIdentifier == ExecSubState)
		{
			isExecAutoParamHelp = false;
		}
		return num;
	}

	public static TokenManager ScanSource(ErrorHandler errorHandler, string source, ParseOptions options, out ParseOptions resultOptions)
	{
		return new ScanSourceHelper(errorHandler).ScanSource(source, options, out resultOptions);
	}

	private int Scan()
	{
		atomStartIndex = atomEndIndex;
		int num = (TestEof() ? 128 : ((CommentLevel != 0) ? ScanMultilineComment() : (EntryState switch
		{
			EntryStateValues.Bracketed => ScanBracketed(), 
			EntryStateValues.Quoted => ScanQuoted(), 
			EntryStateValues.TicString => ScanTicString(), 
			EntryStateValues.BinaryConstant => ScanBinaryConstant(), 
			_ => ScanBase(), 
		})));
		if (num <= 985 && num != 128)
		{
			RunQuotedIdentifierSubStateTransitions(num);
			RunExecSubStateTransitions(num);
		}
		return num;
	}

	private void RunQuotedIdentifierSubStateTransitions(int tokenID)
	{
		if (tokenID == 149)
		{
			QuotedIdentifierSubState = QuotedIdentifierSubStateValues.AfterSet;
			return;
		}
		switch (QuotedIdentifierSubState)
		{
		case QuotedIdentifierSubStateValues.AfterSet:
		{
			bool flag = false;
			if (tokenID == 173 && StringTokenLookup.Find(source.Substring(atomStartIndex, atomEndIndex - atomStartIndex)) == StringToken.QuotedIdentifier)
			{
				flag = true;
			}
			QuotedIdentifierSubState = (flag ? QuotedIdentifierSubStateValues.AfterQuotedIdentifier : QuotedIdentifierSubStateValues.Base);
			break;
		}
		case QuotedIdentifierSubStateValues.AfterQuotedIdentifier:
			switch (tokenID)
			{
			case 269:
				QuotedIdentifierState = QuotedIdentifierStateValues.QuotedIdentifierOff;
				break;
			case 271:
				QuotedIdentifierState = QuotedIdentifierStateValues.QuotedIdentifierOn;
				break;
			}
			QuotedIdentifierSubState = QuotedIdentifierSubStateValues.Base;
			break;
		case QuotedIdentifierSubStateValues.Base:
			break;
		}
	}

	private void RunExecSubStateTransitions(int tokenID)
	{
		if (tokenID == 237)
		{
			ExecSubState = ExecSubStateValues.AfterExec;
			return;
		}
		bool flag = false;
		while (!flag)
		{
			switch (ExecSubState)
			{
			case ExecSubStateValues.Base:
				flag = true;
				break;
			case ExecSubStateValues.AfterExec:
				if (tokenID == 175)
				{
					ExecSubState = ExecSubStateValues.AfterVariable;
					flag = true;
				}
				else
				{
					ExecSubState = ExecSubStateValues.BeforeFullIdentifier;
					flag = false;
				}
				break;
			case ExecSubStateValues.AfterVariable:
				ExecSubState = ((tokenID == 61) ? ExecSubStateValues.BeforeFullIdentifier : ExecSubStateValues.Base);
				flag = true;
				break;
			case ExecSubStateValues.BeforeFullIdentifier:
				switch (tokenID)
				{
				case 173:
					ExecSubState = ExecSubStateValues.AfterFullIdentifier;
					break;
				default:
					ExecSubState = ExecSubStateValues.Base;
					break;
				case 46:
					break;
				}
				flag = true;
				break;
			case ExecSubStateValues.AfterFullIdentifier:
				if (tokenID == 46)
				{
					ExecSubState = ExecSubStateValues.BeforeFullIdentifier;
				}
				else
				{
					ExecSubState = ExecSubStateValues.Base;
				}
				flag = true;
				break;
			}
		}
	}

	private bool TestEof()
	{
		return atomEndIndex >= source.Length;
	}

	private int ScanMultilineComment()
	{
		AdvanceWhile(NotStarOrSlashPredicate, condition: true);
		if (TestEndOfLineOrFile(out var next))
		{
			if (IsEOL(next))
			{
				atomEndIndex++;
			}
			return 988;
		}
		atomEndIndex++;
		if (TestEndOfLineOrFile(out var next2))
		{
			if (IsEOL(next))
			{
				atomEndIndex++;
			}
			return 988;
		}
		if (next == '*' && next2 == '/')
		{
			atomEndIndex++;
			if (DecrementMultilineNesting() == 0)
			{
				return 988;
			}
		}
		else if (next == '/' && next2 == '*')
		{
			atomEndIndex++;
			IncrementMultilineNesting();
		}
		return ScanMultilineComment();
	}

	private int IncrementMultilineNesting()
	{
		return (int)(++CommentLevel);
	}

	private int DecrementMultilineNesting()
	{
		return (int)(--CommentLevel);
	}

	private int ScanBase()
	{
		char c = source[atomEndIndex++];
		switch (c)
		{
		case '\'':
			EntryState = EntryStateValues.TicString;
			return ScanTicString();
		case '"':
			EntryState = EntryStateValues.Quoted;
			return ScanQuoted();
		case '[':
			EntryState = EntryStateValues.Bracketed;
			return ScanBracketed();
		case '\n':
		case '\r':
			return 986;
		case 'N':
			return ScanOpeningN();
		case '.':
			return ScanDot();
		case '$':
			return ScanDollar();
		case ':':
			return ScanColon();
		case '%':
		case '&':
		case '*':
		case '+':
		case '-':
		case '/':
		case '=':
		case '^':
		case '|':
			return ScanCompoundOperator(c);
		default:
			if (IsWhiteSpace(c))
			{
				return ScanWhiteSpace();
			}
			if (IsIdentifierStart(c))
			{
				return ScanIdentifier();
			}
			if (IsDigit(c))
			{
				return ScanNumeric();
			}
			if (IsCurrency(c))
			{
				return ScanMoney();
			}
			if (c < '\u007f')
			{
				return c;
			}
			return 977;
		}
	}

	private int ScanDollar()
	{
		if (TestEndOfLineOrFile(out var next) || !IsIdentifierStart(next))
		{
			return ScanMoney();
		}
		AdvanceWhile(IsIdentifierCharacterPredicate, condition: true);
		return 177;
	}

	private int ScanCompoundOperator(char current)
	{
		if (!TestEndOfLineOrFile(out var next) && next == '=')
		{
			atomEndIndex++;
			switch (current)
			{
			case '+':
				return 190;
			case '-':
				return 191;
			case '*':
				return 192;
			case '/':
				return 193;
			case '%':
				return 194;
			case '&':
				return 195;
			case '|':
				return 196;
			case '^':
				return 197;
			case '=':
				atomEndIndex--;
				return 61;
			}
			atomEndIndex--;
		}
		if (current == '=' && next == '*')
		{
			atomEndIndex++;
			return 187;
		}
		if (current == '/' && next == '*')
		{
			atomEndIndex++;
			CommentLevel = 1u;
			return ScanMultilineComment();
		}
		if (current == '-' && next == '-')
		{
			atomEndIndex++;
			return ScanEndOfLineComment();
		}
		return current;
	}

	private int ScanColon()
	{
		if (!TestEndOfLineOrFile(out var next) && next == ':')
		{
			atomEndIndex++;
			return 139;
		}
		return 58;
	}

	private int ScanDot()
	{
		if (!TestEndOfLineOrFile(out var next) && IsDigit(next))
		{
			atomEndIndex--;
			return ScanForDecimal();
		}
		return 46;
	}

	private int ScanOpeningN()
	{
		if (!TestEndOfLineOrFile(out var next) && next == '\'')
		{
			atomEndIndex++;
			EntryState = EntryStateValues.TicString;
			return ScanTicString();
		}
		return ScanIdentifier();
	}

	private int ScanMoney()
	{
		AdvanceWhile(IsSpacePredicate, condition: true);
		if (TestEndOfLineOrFile(out var next))
		{
			return 185;
		}
		if (next == '+' || next == '-')
		{
			atomEndIndex++;
		}
		AdvanceWhile(IsDigitPredicate, condition: true);
		if (TestEndOfLineOrFile(out next))
		{
			return 185;
		}
		if (next == '.')
		{
			atomEndIndex++;
			AdvanceWhile(IsDigitPredicate, condition: true);
		}
		return 185;
	}

	private bool TestEndOfLineOrFile(out char next)
	{
		if (atomEndIndex < source.Length)
		{
			next = source[atomEndIndex];
			return TestEndOfLine(atomEndIndex);
		}
		next = '\0';
		return true;
	}

	private bool TestEndOfLine(int index)
	{
		switch (source[index])
		{
		case '\n':
			return true;
		case '\r':
			if (index + 1 < source.Length)
			{
				return source[index + 1] != '\n';
			}
			return false;
		default:
			return false;
		}
	}

	private static bool ToEOL(char c)
	{
		return true;
	}

	private int ScanEndOfLineComment()
	{
		AdvanceWhile(ToEOLPredicate, condition: true);
		return 987;
	}

	private int ScanNumeric()
	{
		if (!TestEndOfLineOrFile(out var next) && (next == 'x' || next == 'X'))
		{
			atomEndIndex++;
			EntryState = EntryStateValues.BinaryConstant;
			return ScanBinaryConstant();
		}
		AdvanceWhile(IsDigitPredicate, condition: true);
		return ScanForDecimal();
	}

	private int ScanBinaryConstant()
	{
		AdvanceWhile(IsHexadecimalPredicate, condition: true);
		if (TestEndOfLineOrFile(out var next) || next != '\\')
		{
			EntryState = EntryStateValues.Base;
			return 180;
		}
		atomEndIndex++;
		if (TestEof())
		{
			return 180;
		}
		if (TestEndOfLine(atomEndIndex))
		{
			atomEndIndex++;
			return 994;
		}
		next = source[atomEndIndex];
		if (next == '\r')
		{
			atomEndIndex += 2;
			return 994;
		}
		atomEndIndex--;
		EntryState = EntryStateValues.Base;
		return 180;
	}

	private int ScanForDecimal()
	{
		Tokens tokens = Tokens.TOKEN_INTEGER;
		if (!TestEndOfLineOrFile(out var next) && next == '.')
		{
			atomEndIndex++;
			tokens = Tokens.TOKEN_NUMERIC;
			AdvanceWhile(IsDigitPredicate, condition: true);
		}
		if (!TestEndOfLineOrFile(out next) && (next == 'e' || next == 'E'))
		{
			atomEndIndex++;
			tokens = Tokens.TOKEN_REAL;
			if (!TestEndOfLineOrFile(out next) && (next == '+' || next == '-'))
			{
				atomEndIndex++;
			}
			AdvanceWhile(IsDigitPredicate, condition: true);
		}
		if (tokens == Tokens.TOKEN_INTEGER && IsValueTooLargeForTokenInteger(source, atomStartIndex, atomEndIndex))
		{
			tokens = Tokens.TOKEN_NUMERIC;
		}
		return (int)tokens;
	}

	internal static bool IsValueTooLargeForTokenInteger(string source, int atomStartIndex, int atomEndIndex)
	{
		int num = atomEndIndex - atomStartIndex;
		if (num > 10)
		{
			return true;
		}
		if (num == 10)
		{
			for (int i = 0; i < 10; i++)
			{
				char c = source[atomStartIndex + i];
				char value = "2147483647"[i];
				int num2 = c.CompareTo(value);
				if (num2 > 0)
				{
					return true;
				}
				if (num2 < 0)
				{
					return false;
				}
			}
		}
		return false;
	}

	private int ScanWhiteSpace()
	{
		AdvanceWhile(IsWhiteSpacePredicate, condition: true);
		return 986;
	}

	private int ScanIdentifier()
	{
		AdvanceWhile(IsIdentifierCharacterPredicate, condition: true);
		if (!TestEndOfLineOrFile(out var next) && next == ':')
		{
			atomEndIndex++;
			if (TestEndOfLineOrFile(out next) || next != ':')
			{
				return 181;
			}
			atomEndIndex--;
		}
		return (int)GetIdToken(source, atomStartIndex, atomEndIndex - atomStartIndex);
	}

	private void AdvanceWhile(AdvancePredicate predicate, bool condition)
	{
		int length = source.Length;
		while (atomEndIndex < length)
		{
			char c = source[atomEndIndex];
			if (predicate(c) == condition && !TestEndOfLine(atomEndIndex))
			{
				atomEndIndex++;
				continue;
			}
			break;
		}
	}

	private static bool IsAccumulatorToken(int tokenId)
	{
		if (tokenId > 991)
		{
			return tokenId < 995;
		}
		return false;
	}

	private static int GetTerminatorId(int accumulatorId)
	{
		return accumulatorId switch
		{
			993 => 176, 
			992 => 173, 
			_ => accumulatorId, 
		};
	}

	private int ScanSequence(AdvancePredicate terminatorCharPredicate, int accumlatorId, int terminatorId)
	{
		while (true)
		{
			AdvanceWhile(terminatorCharPredicate, condition: false);
			if (TestEndOfLineOrFile(out var next))
			{
				if (next != 0)
				{
					atomEndIndex++;
				}
				return accumlatorId;
			}
			atomEndIndex++;
			if (TestEndOfLineOrFile(out var next2) || !terminatorCharPredicate(next2))
			{
				break;
			}
			atomEndIndex++;
		}
		EntryState = EntryStateValues.Base;
		return terminatorId;
	}

	private int ScanTicString()
	{
		return ScanSequence(IsTic, 993, 176);
	}

	private int ScanQuoted()
	{
		if (IsQuotedIdentifierSet)
		{
			return ScanSequence(IsQuotePredicate, 992, 173);
		}
		return ScanSequence(IsQuotePredicate, 993, 176);
	}

	private int ScanBracketed()
	{
		return ScanSequence(IsCloseBracketPredicate, 992, 173);
	}

	private static bool IsCloseBracket(char c)
	{
		return c == ']';
	}

	private static bool IsCurrency(char c)
	{
		if (!IsPMoney(c))
		{
			return IsPExoticMoney(c);
		}
		return true;
	}

	private static bool IsDigit(char c)
	{
		return IsPNumber(c);
	}

	private static bool IsEOL(char c)
	{
		if (c != '\n')
		{
			return c == '\r';
		}
		return true;
	}

	private static bool IsHexadecimal(char c)
	{
		if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'F') || (c >= 'a' && c <= 'f'))
		{
			return true;
		}
		return false;
	}

	internal static bool IsIdentifierStart(char c)
	{
		return IsPAlpha(c);
	}

	internal static bool IsIdentifierCharacter(char c)
	{
		if (!IsPAlpha(c) && !IsPAlphaNonFirst(c) && !IsPNumber(c))
		{
			return IsPMoney(c);
		}
		return true;
	}

	private static bool IsQuote(char c)
	{
		return c == '"';
	}

	private static bool IsSpace(char c)
	{
		return c == ' ';
	}

	private static bool IsWhiteSpace(char c)
	{
		return IsPIgSpace(c);
	}

	private static bool IsTic(char c)
	{
		return c == '\'';
	}

	private static bool NotStarOrSlash(char c)
	{
		if (c != '*')
		{
			return c != '/';
		}
		return false;
	}

	private static bool IsPAlpha(char c)
	{
		if (c < 'ª')
		{
			if ((c >= '@' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '#' || c == '_')
			{
				return true;
			}
			return false;
		}
		return LookupCharacterClass(c) == CharacterClass.P_ALPHA;
	}

	private static bool IsPAlphaNonFirst(char c)
	{
		if (c < '\u0300')
		{
			return false;
		}
		return LookupCharacterClass(c) == CharacterClass.P_ALPHA_NON_FIRST;
	}

	private static bool IsPExoticMoney(char c)
	{
		if (c == '\\')
		{
			return true;
		}
		if (c < '¢')
		{
			return false;
		}
		return LookupCharacterClass(c) == CharacterClass.P_EXOTIC_MONEY;
	}

	private static bool IsPIgSpace(char c)
	{
		if (c > ' ')
		{
			switch (c)
			{
			default:
				if (c != '\u2028' && c != '\u2029' && c != '\u202f' && c != '\u205f')
				{
					return c == '\u3000';
				}
				break;
			case '\u0085':
			case '\u00a0':
			case '\u1680':
			case '\u180e':
			case '\u2000':
			case '\u2001':
			case '\u2002':
			case '\u2003':
			case '\u2004':
			case '\u2005':
			case '\u2006':
			case '\u2007':
			case '\u2008':
			case '\u2009':
			case '\u200a':
				break;
			}
		}
		return true;
	}

	private static bool IsPMoney(char c)
	{
		return c == '$';
	}

	private static bool IsPNumber(char c)
	{
		if (c >= '0')
		{
			return c <= '9';
		}
		return false;
	}

	private static CharacterClass LookupCharacterClass(char c)
	{
		int num = characterMapKeys.BinarySearch(c);
		if (num < 0)
		{
			num = ~num;
			num--;
		}
		return characterMapValues[num];
	}

	static Scanner()
	{
		keywords80 = new KeywordLookup(CreateKeywordList80(includeDep: true));
		keywords90 = new KeywordLookup(CreateKeywordList90(includeDep: true));
		keywords100 = new KeywordLookup(CreateKeywordList100(includeDep: true));
		keywords110 = new KeywordLookup(CreateKeywordList110(includeDep: true));
		keywords120 = new KeywordLookup(CreateKeywordList120(includeDep: true));
		keywords130 = new KeywordLookup(CreateKeywordList130(includeDep: true));
		keywords140 = new KeywordLookup(CreateKeywordList140(includeDep: true));
		keywords150 = new KeywordLookup(CreateKeywordList150(includeDep: true));
		keywords160 = new KeywordLookup(CreateKeywordList160(includeDep: true));
		keywordsAzure = new KeywordLookup(CreateKeywordList160(includeDep: true));
		characterMapKeys = new List<int>();
		characterMapValues = new List<CharacterClass>();
		characterMapKeys.Add(0);
		characterMapValues.Add(CharacterClass.P_IGSPACE);
		characterMapKeys.Add(33);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(34);
		characterMapValues.Add(CharacterClass.P_STRING);
		characterMapKeys.Add(35);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(36);
		characterMapValues.Add(CharacterClass.P_MONEY);
		characterMapKeys.Add(37);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(39);
		characterMapValues.Add(CharacterClass.P_STRING);
		characterMapKeys.Add(40);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(48);
		characterMapValues.Add(CharacterClass.P_NUMB);
		characterMapKeys.Add(58);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(64);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(91);
		characterMapValues.Add(CharacterClass.P_STRING);
		characterMapKeys.Add(92);
		characterMapValues.Add(CharacterClass.P_EXOTIC_MONEY);
		characterMapKeys.Add(93);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(95);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(96);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(97);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(123);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(162);
		characterMapValues.Add(CharacterClass.P_EXOTIC_MONEY);
		characterMapKeys.Add(166);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(170);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(171);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(181);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(182);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(186);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(187);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(192);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(215);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(216);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(247);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(248);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(545);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(546);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(564);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(592);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(686);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(688);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(697);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(699);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(706);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(720);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(722);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(736);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(741);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(750);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(751);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(768);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(848);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(864);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(880);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(890);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(891);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(902);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(903);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(904);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(907);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(908);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(909);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(910);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(930);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(931);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(975);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(976);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(1014);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(1024);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(1154);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(1155);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(1159);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(1162);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(1231);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(1232);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(1270);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(1272);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(1274);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(1280);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(1296);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(1329);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(1367);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(1369);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(1370);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(1377);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(1416);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(1425);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(1442);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(1443);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(1466);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(1467);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(1470);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(1471);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(1472);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(1473);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(1475);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(1476);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(1477);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(1488);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(1515);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(1520);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(1523);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(1569);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(1595);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(1600);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(1611);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(1622);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(1632);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(1642);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(1646);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(1648);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(1649);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(1748);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(1749);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(1750);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(1758);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(1759);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(1765);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(1767);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(1769);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(1770);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(1774);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(1776);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(1786);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(1789);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(1807);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(1808);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(1809);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(1810);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(1837);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(1840);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(1867);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(1920);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(1958);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(1969);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(1970);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(2305);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(2308);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(2309);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(2362);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(2364);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(2365);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(2366);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(2382);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(2384);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(2385);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(2389);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(2392);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(2402);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(2404);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(2406);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(2416);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(2433);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(2436);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(2437);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(2445);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(2447);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(2449);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(2451);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(2473);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(2474);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(2481);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(2482);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(2483);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(2486);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(2490);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(2492);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(2493);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(2494);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(2501);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(2503);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(2505);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(2507);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(2510);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(2519);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(2520);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(2524);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(2526);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(2527);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(2530);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(2532);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(2534);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(2544);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(2546);
		characterMapValues.Add(CharacterClass.P_EXOTIC_MONEY);
		characterMapKeys.Add(2548);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(2562);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(2563);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(2565);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(2571);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(2575);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(2577);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(2579);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(2601);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(2602);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(2609);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(2610);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(2612);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(2613);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(2615);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(2616);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(2618);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(2620);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(2621);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(2622);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(2627);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(2631);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(2633);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(2635);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(2638);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(2649);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(2653);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(2654);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(2655);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(2662);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(2674);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(2677);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(2689);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(2692);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(2693);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(2700);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(2701);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(2702);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(2703);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(2706);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(2707);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(2729);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(2730);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(2737);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(2738);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(2740);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(2741);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(2746);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(2748);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(2749);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(2750);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(2758);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(2759);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(2762);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(2763);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(2766);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(2768);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(2769);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(2784);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(2785);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(2790);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(2800);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(2817);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(2820);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(2821);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(2829);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(2831);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(2833);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(2835);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(2857);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(2858);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(2865);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(2866);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(2868);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(2870);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(2874);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(2876);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(2877);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(2878);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(2884);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(2887);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(2889);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(2891);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(2894);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(2902);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(2904);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(2908);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(2910);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(2911);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(2914);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(2918);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(2928);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(2946);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(2947);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(2948);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(2949);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(2955);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(2958);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(2961);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(2962);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(2966);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(2969);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(2971);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(2972);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(2973);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(2974);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(2976);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(2979);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(2981);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(2984);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(2987);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(2990);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(2998);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(2999);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(3002);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3006);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(3011);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3014);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(3017);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3018);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(3022);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3031);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(3032);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3047);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(3056);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3073);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(3076);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3077);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(3085);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3086);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(3089);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3090);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(3113);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3114);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(3124);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3125);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(3130);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3134);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(3141);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3142);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(3145);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3146);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(3150);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3157);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(3159);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3168);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(3170);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3174);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(3184);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3202);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(3204);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3205);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(3213);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3214);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(3217);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3218);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(3241);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3242);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(3252);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3253);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(3258);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3262);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(3269);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3270);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(3273);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3274);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(3278);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3285);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(3287);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3294);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(3295);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3296);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(3298);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3302);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(3312);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3330);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(3332);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3333);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(3341);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3342);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(3345);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3346);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(3369);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3370);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(3386);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3390);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(3396);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3398);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(3401);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3402);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(3406);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3415);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(3416);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3424);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(3426);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3430);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(3440);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3458);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(3460);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3461);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(3479);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3482);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(3506);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3507);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(3516);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3517);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(3518);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3520);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(3527);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3530);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(3531);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3535);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(3541);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3542);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(3543);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3544);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(3552);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3570);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(3572);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3585);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(3633);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(3634);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(3636);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(3643);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3647);
		characterMapValues.Add(CharacterClass.P_EXOTIC_MONEY);
		characterMapKeys.Add(3648);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(3655);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(3663);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3664);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(3674);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3713);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(3715);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3716);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(3717);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3719);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(3721);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3722);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(3723);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3725);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(3726);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3732);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(3736);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3737);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(3744);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3745);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(3748);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3749);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(3750);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3751);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(3752);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3754);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(3756);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3757);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(3761);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(3762);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(3764);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(3770);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3771);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(3773);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(3774);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3776);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(3781);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3782);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(3783);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3784);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(3790);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3792);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(3802);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3804);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(3806);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3840);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(3841);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3864);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(3866);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3872);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(3882);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3893);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(3894);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3895);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(3896);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3897);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(3898);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3902);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(3904);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(3912);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3913);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(3947);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3953);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(3973);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3974);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(3976);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(3980);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3984);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(3992);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(3993);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(4029);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(4038);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(4039);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(4096);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(4130);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(4131);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(4136);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(4137);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(4139);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(4140);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(4147);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(4150);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(4154);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(4160);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(4170);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(4176);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(4182);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(4186);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(4256);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(4294);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(4304);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(4345);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(4352);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(4442);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(4447);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(4515);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(4520);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(4602);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(4608);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(4615);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(4616);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(4679);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(4680);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(4681);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(4682);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(4686);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(4688);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(4695);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(4696);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(4697);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(4698);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(4702);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(4704);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(4743);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(4744);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(4745);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(4746);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(4750);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(4752);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(4783);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(4784);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(4785);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(4786);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(4790);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(4792);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(4799);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(4800);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(4801);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(4802);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(4806);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(4808);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(4815);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(4816);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(4823);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(4824);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(4847);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(4848);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(4879);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(4880);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(4881);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(4882);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(4886);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(4888);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(4895);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(4896);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(4935);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(4936);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(4955);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(4969);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(4978);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(5024);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(5109);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(5121);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(5741);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(5743);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(5751);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(5761);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(5787);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(5792);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(5867);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(5870);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(5873);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(5888);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(5901);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(5902);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(5906);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(5909);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(5920);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(5938);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(5941);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(5952);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(5970);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(5972);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(5984);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(5997);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(5998);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(6001);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(6002);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(6004);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(6016);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(6068);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(6100);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(6103);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(6104);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(6107);
		characterMapValues.Add(CharacterClass.P_EXOTIC_MONEY);
		characterMapKeys.Add(6108);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(6109);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(6112);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(6122);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(6155);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(6159);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(6160);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(6170);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(6176);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(6264);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(6272);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(6313);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(6314);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(7680);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(7836);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(7840);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(7930);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(7936);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(7958);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(7960);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(7966);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(7968);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(8006);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(8008);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(8014);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(8016);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(8024);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(8025);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(8026);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(8027);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(8028);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(8029);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(8030);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(8031);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(8062);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(8064);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(8117);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(8118);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(8125);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(8126);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(8127);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(8130);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(8133);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(8134);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(8141);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(8144);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(8148);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(8150);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(8156);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(8160);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(8173);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(8178);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(8181);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(8182);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(8189);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(8204);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(8208);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(8234);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(8239);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(8255);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(8257);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(8288);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(8292);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(8298);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(8304);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(8305);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(8306);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(8319);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(8320);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(8352);
		characterMapValues.Add(CharacterClass.P_EXOTIC_MONEY);
		characterMapKeys.Add(8370);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(8400);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(8413);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(8417);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(8418);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(8421);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(8427);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(8450);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(8451);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(8455);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(8456);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(8458);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(8468);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(8469);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(8470);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(8473);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(8478);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(8484);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(8485);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(8486);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(8487);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(8488);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(8489);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(8490);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(8494);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(8495);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(8498);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(8499);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(8506);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(8509);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(8512);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(8517);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(8522);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(8544);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(8580);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(12293);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(12296);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(12321);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(12330);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(12336);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(12337);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(12342);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(12344);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(12349);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(12353);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(12439);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(12441);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(12443);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(12445);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(12448);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(12449);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(12539);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(12540);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(12544);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(12549);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(12589);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(12593);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(12687);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(12704);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(12728);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(12784);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(12800);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(13312);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(19894);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(19968);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(40870);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(40960);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(42125);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(44032);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(55204);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(63744);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(64046);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(64048);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(64107);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(64256);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(64263);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(64275);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(64280);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(64285);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(64286);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(64287);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(64297);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(64298);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(64311);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(64312);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(64317);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(64318);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(64319);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(64320);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(64322);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(64323);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(64325);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(64326);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(64434);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(64467);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(64830);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(64848);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(64912);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(64914);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(64968);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(65008);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(65020);
		characterMapValues.Add(CharacterClass.P_EXOTIC_MONEY);
		characterMapKeys.Add(65021);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(65024);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(65040);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(65056);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(65060);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(65075);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(65077);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(65101);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(65104);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(65129);
		characterMapValues.Add(CharacterClass.P_EXOTIC_MONEY);
		characterMapKeys.Add(65130);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(65136);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(65141);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(65142);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(65277);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(65279);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(65280);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(65284);
		characterMapValues.Add(CharacterClass.P_EXOTIC_MONEY);
		characterMapKeys.Add(65285);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(65296);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(65306);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(65313);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(65339);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(65343);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(65344);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(65345);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(65371);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(65381);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(65382);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(65471);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(65474);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(65480);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(65482);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(65488);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(65490);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(65496);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(65498);
		characterMapValues.Add(CharacterClass.P_ALPHA);
		characterMapKeys.Add(65501);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(65504);
		characterMapValues.Add(CharacterClass.P_EXOTIC_MONEY);
		characterMapKeys.Add(65506);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(65509);
		characterMapValues.Add(CharacterClass.P_EXOTIC_MONEY);
		characterMapKeys.Add(65511);
		characterMapValues.Add(CharacterClass.P_OPER);
		characterMapKeys.Add(65529);
		characterMapValues.Add(CharacterClass.P_ALPHA_NON_FIRST);
		characterMapKeys.Add(65532);
		characterMapValues.Add(CharacterClass.P_OPER);
	}
}
