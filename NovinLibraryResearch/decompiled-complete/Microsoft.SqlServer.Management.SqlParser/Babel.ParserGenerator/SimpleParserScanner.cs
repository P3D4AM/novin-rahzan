using Microsoft.SqlServer.Management.SqlParser.Common;
using Microsoft.SqlServer.Management.SqlParser.Parser;
using Microsoft.SqlServer.Management.SqlParser.Parser.Internals;
using Microsoft.SqlServer.Management.SqlParser.Parser.Internals105;
using Microsoft.SqlServer.Management.SqlParser.Parser.Internals110;
using Microsoft.SqlServer.Management.SqlParser.Parser.Internals120;
using Microsoft.SqlServer.Management.SqlParser.Parser.Internals130;
using Microsoft.SqlServer.Management.SqlParser.Parser.Internals140;
using Microsoft.SqlServer.Management.SqlParser.Parser.Internals150;
using Microsoft.SqlServer.Management.SqlParser.Parser.Internals160;
using Microsoft.SqlServer.Management.SqlParser.Parser.InternalsAzure;

namespace Babel.ParserGenerator;

internal class SimpleParserScanner
{
	protected readonly TokenManager firstLevelTokenManager;

	protected readonly string source;

	protected readonly IWriteableMinimalList<TokenData> tokens;

	protected readonly int compatibilityLevel;

	protected readonly TransactSqlVersion transactSqlVersion;

	protected int token;

	protected LexLocation tokenLocation;

	protected int startTokenIndex;

	protected int endTokenIndex;

	protected SimpleScannerState lastSimpleScannerState;

	protected int nextTokenIndex;

	protected bool isStatementDelimiter;

	protected EParseState yystate;

	protected QueryStatus queryStatus;

	protected QueryState queryState;

	protected int gdrTokenCount;

	private const int QUERY_STATUS_POS = 0;

	private const int QUERY_STATUS_MASK = 65535;

	private const int YYSTATE_POS = 16;

	private const int YYSTATE_MASK = 15;

	private const int IS_STATEMENT_DELIMITER_POS = 20;

	private const int IS_STATEMENT_DELIMITER_MASK = 1;

	public SimpleScannerState SimpleScannerState => lastSimpleScannerState;

	public CompleteScannerState CompleteScannerState => new CompleteScannerState(lastSimpleScannerState, GetScannerState(), IsTokenRead);

	public TokenManager FirstLevelTokenManager => firstLevelTokenManager;

	public int Token => token;

	public bool IsTokenRead => token != 0;

	public string TokenText => firstLevelTokenManager.GetText(tokenLocation);

	public LexLocation TokenLocation => tokenLocation;

	protected SimpleParserScanner(TokenManager firstLevelTokenManager, IWriteableMinimalList<TokenData> tokens, TransactSqlVersion transactSqlVersion)
	{
		this.firstLevelTokenManager = firstLevelTokenManager;
		source = firstLevelTokenManager.Source;
		this.tokens = tokens;
		compatibilityLevel = firstLevelTokenManager.CompatibilityLevel;
		this.transactSqlVersion = transactSqlVersion;
	}

	private SimpleParserScanner(SimpleParserScanner parserScanner, IWriteableMinimalList<TokenData> tokens)
	{
		firstLevelTokenManager = parserScanner.firstLevelTokenManager;
		source = parserScanner.source;
		this.tokens = tokens;
		compatibilityLevel = parserScanner.compatibilityLevel;
		token = parserScanner.token;
		tokenLocation = parserScanner.tokenLocation;
		startTokenIndex = parserScanner.startTokenIndex;
		endTokenIndex = parserScanner.endTokenIndex;
		nextTokenIndex = parserScanner.nextTokenIndex;
		lastSimpleScannerState = parserScanner.lastSimpleScannerState;
		isStatementDelimiter = parserScanner.isStatementDelimiter;
		yystate = parserScanner.yystate;
		queryStatus = parserScanner.queryStatus;
		queryState = parserScanner.queryState;
		gdrTokenCount = parserScanner.gdrTokenCount;
		transactSqlVersion = parserScanner.transactSqlVersion;
	}

	internal SimpleParserScanner CreateTemporaryParserScanner()
	{
		TemporaryMinimalList<TokenData> temporaryMinimalList = new TemporaryMinimalList<TokenData>(tokens);
		return new SimpleParserScanner(this, temporaryMinimalList);
	}

	protected int GetScannerState()
	{
		return (int)queryStatus | ((int)yystate << 16) | ((isStatementDelimiter ? 1 : 0) << 20);
	}

	protected void RestoreScannerState(int scannerState)
	{
		queryStatus = (QueryStatus)(scannerState & 0xFFFF);
		yystate = (EParseState)((scannerState >> 16) & 0xF);
		isStatementDelimiter = ((scannerState >> 20) & 1) != 0;
	}

	public void NextToken()
	{
		if (!IsTokenRead)
		{
			ReadToken();
		}
		while (GetId(nextTokenIndex) >= 985)
		{
			nextTokenIndex++;
		}
		lastSimpleScannerState = new SimpleScannerState(GetScannerState(), nextTokenIndex);
		token = 0;
	}

	public void ReadToken()
	{
		token = yylex();
	}

	internal StringToken GetStringToken()
	{
		return firstLevelTokenManager.GetStringToken(startTokenIndex);
	}

	internal int yytransform(int parentTokenId, TokenTransformContext context)
	{
		int retval = 0;
		bool flag = true;
		switch (context)
		{
		case TokenTransformContext.LEFT_PAREN_COMPAT:
			retval = ((compatibilityLevel > 80) ? 403 : 402);
			flag = false;
			break;
		case TokenTransformContext.CREATE_DROP_ALTER:
		case TokenTransformContext.ADD:
			LookupIdSubToken(parentTokenId, context, ref retval);
			break;
		case TokenTransformContext.AFTER_END:
			LookupIdSubToken(parentTokenId, context, ref retval);
			if (retval != 0)
			{
				int num2 = endTokenIndex;
				while (GetId(--num2) != 233)
				{
				}
				KillToken(num2);
			}
			break;
		case TokenTransformContext.AFTER_WITH:
		case TokenTransformContext.AFTER_WITH_EXEC_OPTION:
		case TokenTransformContext.CREATE_DB_OPT:
		case TokenTransformContext.CREATE_DB_TYPE:
		case TokenTransformContext.ALTER_DB_OPT:
		case TokenTransformContext.DUMP_OPT:
		case TokenTransformContext.DUMP_ENCRYPTION_OPT:
		case TokenTransformContext.LOAD_OPT:
		case TokenTransformContext.TAB_OPT_NAME:
		case TokenTransformContext.INDEX_SORT_OPT:
		case TokenTransformContext.OPT_STORAGE_ID:
		case TokenTransformContext.IND_OPTIONS:
		case TokenTransformContext.IND_ACTIONS:
		case TokenTransformContext.ALTTAB_ACTIONS:
		case TokenTransformContext.SEQUENCE_OPTIONS:
		case TokenTransformContext.ALTTAB_ENABLE_DISABLE:
		case TokenTransformContext.AFFINITY_TYPE:
		case TokenTransformContext.POOL_AFFINITY_TYPE:
		case TokenTransformContext.EXTERNAL_POOL_AFFINITY_TYPE:
		case TokenTransformContext.EXTERNAL_LANGUAGE_FILE_SPEC_OPT:
		case TokenTransformContext.ALTER_EXTERNAL_LANGUAGE_OPT:
		case TokenTransformContext.EXTERNAL_LIBRARY_FILE_SPEC_OPT:
		case TokenTransformContext.ALTER_EXTERNAL_LIBRARY_OPT:
		case TokenTransformContext.ALTER_SERVER_TYPE:
		case TokenTransformContext.FULLTEXT_SEARCH:
		case TokenTransformContext.FTS_PROPERTY:
		case TokenTransformContext.AFTER_FTS_WITH:
		case TokenTransformContext.CREATE_USER_WITH:
		case TokenTransformContext.ALTER_USER_WITH:
		case TokenTransformContext.AG_REPLICA_OPTION:
		case TokenTransformContext.ALTER_SERVER_CONFIG_SET:
		case TokenTransformContext.ALTER_SERVER_CONFIG_SET_DIAG_LOG:
		case TokenTransformContext.ALTER_SERVER_CONFIG_SET_HADR_CLUSTER:
		case TokenTransformContext.OVER_CLAUSE:
		case TokenTransformContext.AFTER_BEGIN:
		case TokenTransformContext.FILESTREAM_DB_OPT:
		case TokenTransformContext.CREATE_TAB_OPT:
		case TokenTransformContext.CREATE_TAB_WITH_OPT:
		case TokenTransformContext.SYSTEM_VERSIONING_OPT:
		case TokenTransformContext.OPT_SXI_PATH_KIND:
		case TokenTransformContext.OPT_SXI_PATH_OPTION:
		case TokenTransformContext.IN_INLINE_INDEX_SYNTAX:
		case TokenTransformContext.CREATE_EXTERNALFILEFORMAT_WITH:
		case TokenTransformContext.CREATE_EXTERNALDATASOURCE_WITH:
		case TokenTransformContext.ALTER_EXTERNALDATASOURCE_WITH:
		case TokenTransformContext.CREATE_EXTERNALTABLE_WITH:
		case TokenTransformContext.EXTERNAL_TYPE:
		case TokenTransformContext.SECURITY_POLICY:
		case TokenTransformContext.QUERY_STORE_OPT:
		case TokenTransformContext.REMOTE_DATA_ARCHIVE_OPT:
		case TokenTransformContext.TAB_LOCK_OPT:
		case TokenTransformContext.COL_ENCRYPTION_OR_MASTER_KEY:
		case TokenTransformContext.GEODR_OPT:
		case TokenTransformContext.COL_ADD_DROP_OPT:
		case TokenTransformContext.COL_DEF_OPT:
		case TokenTransformContext.DB_SCOPED_CONFIG_OPT:
		case TokenTransformContext.GRAPH_ID_OR_FILETABLE:
		case TokenTransformContext.PERSISTENT_LOG_BUFFER_OPT:
		case TokenTransformContext.AUTO_TUNING_DATABASE_TUNING_MODE_OPT:
		case TokenTransformContext.CREATE_TABLE_WITH_LEDGER_OPT:
		case TokenTransformContext.CREATE_TAB_WITH_DATA_DELETION_OPT:
		case TokenTransformContext.CREATE_EXTERNALSTREAM_WITH:
			LookupIdSubToken(parentTokenId, context, ref retval);
			break;
		case TokenTransformContext.TOKEN_BEGIN_CS:
			retval = 978;
			break;
		case TokenTransformContext.TOKEN_END_CS:
			retval = 978;
			break;
		case TokenTransformContext.TOKEN_BEGIN_CS_LOOKUP:
		{
			int num = PeekNextToken();
			if (num != 173 && num != 297 && num != 336)
			{
				retval = 978;
			}
			break;
		}
		case TokenTransformContext.TOKEN_END_CS_LOOKUP:
		{
			StringToken stringToken = PeekNextStringToken();
			if (stringToken != StringToken.Catch && stringToken != StringToken.Conversation && stringToken != StringToken.Try)
			{
				retval = 979;
			}
			break;
		}
		}
		if (retval != 0)
		{
			token = retval;
			if (flag)
			{
				SetId(endTokenIndex, retval);
			}
		}
		return retval;
	}

	private void LookupIdSubToken(int parentTokenId, TokenTransformContext context, ref int retval)
	{
		GetOptLookupByContext(context).TryGetTokenId(GetStringToken(), ref retval);
	}

	private OptLookup GetOptLookupByContext(TokenTransformContext context)
	{
		switch (transactSqlVersion)
		{
		case TransactSqlVersion.Version105:
			return Microsoft.SqlServer.Management.SqlParser.Parser.Internals105.Parser.ParserConfig.GetOptLookupByContext(context);
		case TransactSqlVersion.Version110:
			return Microsoft.SqlServer.Management.SqlParser.Parser.Internals110.Parser.ParserConfig.GetOptLookupByContext(context);
		case TransactSqlVersion.Version120:
			return Microsoft.SqlServer.Management.SqlParser.Parser.Internals120.Parser.ParserConfig.GetOptLookupByContext(context);
		case TransactSqlVersion.Version130:
			return Microsoft.SqlServer.Management.SqlParser.Parser.Internals130.Parser.ParserConfig.GetOptLookupByContext(context);
		case TransactSqlVersion.Version140:
			return Microsoft.SqlServer.Management.SqlParser.Parser.Internals140.Parser.ParserConfig.GetOptLookupByContext(context);
		case TransactSqlVersion.Version150:
			return Microsoft.SqlServer.Management.SqlParser.Parser.Internals150.Parser.ParserConfig.GetOptLookupByContext(context);
		case TransactSqlVersion.Current:
		case TransactSqlVersion.Version160:
			return Microsoft.SqlServer.Management.SqlParser.Parser.Internals160.Parser.ParserConfig.GetOptLookupByContext(context);
		case TransactSqlVersion.Azure:
			return Microsoft.SqlServer.Management.SqlParser.Parser.InternalsAzure.Parser.ParserConfig.GetOptLookupByContext(context);
		default:
			return Microsoft.SqlServer.Management.SqlParser.Parser.Internals110.Parser.ParserConfig.GetOptLookupByContext(context);
		}
	}

	internal int GetId(int tokenNumber)
	{
		return tokens[tokenNumber].TokenId;
	}

	private void SetId(int tokenNumber, int tokenId)
	{
		int startIndex = tokens[tokenNumber].StartIndex;
		tokens[tokenNumber] = new TokenData(tokenId, startIndex);
	}

	private void KillToken(int tokenNumber)
	{
		int startIndex = tokens[tokenNumber].StartIndex;
		int id = ~tokens[tokenNumber].TokenId;
		tokens[tokenNumber] = new TokenData(id, startIndex);
	}

	private bool IsDead(int tokenNumber)
	{
		return GetId(tokenNumber) < 0;
	}

	private int GetOriginalId(int tokenNumber)
	{
		int id = GetId(tokenNumber);
		if (id >= 0)
		{
			return id;
		}
		return ~id;
	}

	private int yylex()
	{
		startTokenIndex = nextTokenIndex;
		endTokenIndex = nextTokenIndex;
		int id = GetId(startTokenIndex);
		if (id == 128)
		{
			tokenLocation = new LexLocation(startTokenIndex, (endTokenIndex < tokens.Count) ? (endTokenIndex + 1) : endTokenIndex);
			nextTokenIndex = endTokenIndex + 1;
			return id;
		}
		int num = id;
		id = EngineLGetToken(id);
		if (yystate == EParseState.e_ParseGDR && id == 173)
		{
			gdrTokenCount++;
		}
		id = EngineYYLex(id);
		id = NonEngineLogic(id);
		if (num != id)
		{
			SetId(endTokenIndex, id);
		}
		tokenLocation = new LexLocation(startTokenIndex, endTokenIndex + 1);
		nextTokenIndex = endTokenIndex + 1;
		return id;
	}

	private int EngineLGetToken(int next)
	{
		if (yystate == EParseState.e_ParseGDR && ShiftReduceParser.IsKeyword(next))
		{
			switch (next)
			{
			default:
				next = 173;
				break;
			case 143:
			case 211:
			case 244:
			case 271:
			case 296:
			case 308:
				break;
			}
		}
		else if (next == 59 && PeekNextToken() == 182)
		{
			next = 339;
		}
		return next;
	}

	private int EngineYYLex(int next)
	{
		bool flag = yystate == EParseState.e_ParseAfterWith;
		EParseState eParseState = yystate;
		if ((yystate != EParseState.e_ParseGDR || next != 173 || gdrTokenCount > 4) && yystate != EParseState.e_ParseGroupBy)
		{
			yystate = EParseState.e_ParseNeutral;
		}
		if ((isStatementDelimiter || flag) && next == 173)
		{
			int tokenId = 0;
			OptLookup.OptStmtStarters.TryGetTokenId(GetStringToken(), ref tokenId);
			if (!isStatementDelimiter && tokenId != 384)
			{
				tokenId = 0;
			}
			if (tokenId > 0)
			{
				next = tokenId;
			}
		}
		isStatementDelimiter = next == 59 || next == 140 || next == 206;
		if (next == 141 && AdvanceIfNextIs(324, killSkippedToken: true))
		{
			next = 142;
		}
		else if (next == 137 && AdvanceIfNextIs(243, killSkippedToken: true))
		{
			next = 266;
		}
		else if (transactSqlVersion != TransactSqlVersion.Version105 && transactSqlVersion != TransactSqlVersion.Version110 && transactSqlVersion != TransactSqlVersion.Version120 && transactSqlVersion != TransactSqlVersion.Version130 && next == 308 && AdvanceIfNextIs(316, killSkippedToken: true))
		{
			next = 309;
		}
		else if (next == 206 && AdvanceIfNextIs(StringToken.Catch, killSkippedToken: true))
		{
			next = 373;
		}
		else if (next == 206 && AdvanceIfNextIs(StringToken.Try, killSkippedToken: true))
		{
			next = 374;
		}
		else if (queryState == QueryState.IN_FROM_CLAUSE && next == 243 && AdvanceIfNextIs(StringToken.SystemTime, killSkippedToken: true))
		{
			next = 394;
		}
		else if (next == 173)
		{
			if (GetStringToken() == StringToken.Next && PeekNextStringToken() == StringToken.Value)
			{
				next = 375;
			}
			else if (GetStringToken() == StringToken.Within && AdvanceIfNextIs(247, killSkippedToken: true))
			{
				next = 376;
			}
			else if (GetStringToken() == StringToken.Openjson && PeekNextToken() == 40 && compatibilityLevel >= 130)
			{
				next = 395;
			}
			else if (queryState == QueryState.IN_WHERE_CLAUSE && PeekBackward() != 46 && GetStringToken() == StringToken.Match && PeekNextToken() == 40)
			{
				next = 396;
			}
			else if (eParseState == EParseState.e_ParseDump)
			{
				OptLookup.OptDumpTypes.TryGetTokenId(GetStringToken(), ref next);
			}
			else if (eParseState == EParseState.e_ParseLoad)
			{
				OptLookup.OptLoadTypes.TryGetTokenId(GetStringToken(), ref next);
			}
			else if (eParseState == EParseState.e_ParseOpen)
			{
				OptLookup.OptOpenCloseTypes.TryGetTokenId(GetStringToken(), ref next);
			}
			else if (eParseState == EParseState.e_ParseClose)
			{
				OptLookup.OptOpenCloseTypes.TryGetTokenId(GetStringToken(), ref next);
			}
			else if (eParseState == EParseState.e_ParseBegin)
			{
				OptLookup.OptBeginTypes.TryGetTokenId(GetStringToken(), ref next);
			}
			else if (eParseState == EParseState.e_ParseEventNotifObjectOn)
			{
				OptLookup.OptEventNotifObjectOn.TryGetTokenId(GetStringToken(), ref next);
			}
			else if (eParseState == EParseState.e_ParseGroupBy && compatibilityLevel >= 100)
			{
				OptLookup.OptGroupBy.TryGetTokenId(GetStringToken(), ref next);
			}
			else if (eParseState == EParseState.e_ParseIdxInclude)
			{
				if (GetStringToken() == StringToken.Include)
				{
					next = 362;
				}
			}
			else if ((queryStatus & (QueryStatus)288) != 0 && GetStringToken() == StringToken.Log && AdvanceIfNextIs(271, killSkippedToken: true))
			{
				next = 272;
			}
			else if ((queryStatus & QueryStatus.Q_CR_AL_FTI) != 0 && GetStringToken() == StringToken.Start && AdvanceIfNextIs(302, killSkippedToken: true))
			{
				next = 345;
			}
			else if (((queryStatus & QueryStatus.Q_MERGE) != 0 || (queryStatus & QueryStatus.Q_FN_PARSE) != 0) && GetStringToken() == StringToken.Using)
			{
				next = 385;
			}
			else if ((queryStatus & (QueryStatus)12288) != 0 && GetStringToken() == StringToken.Audit && AdvanceIfNextIs(StringToken.Specification, killSkippedToken: true))
			{
				next = 386;
			}
			else if ((queryStatus & QueryStatus.Q_DATABASE) != 0 && GetStringToken() == StringToken.Encryption && AdvanceIfNextIs(322, killSkippedToken: true))
			{
				next = 387;
			}
			else if ((queryStatus & QueryStatus.Q_DATABASE) != 0 && GetStringToken() == StringToken.Scoped && AdvanceIfNextIs(StringToken.Configuration, killSkippedToken: true))
			{
				next = 389;
			}
			else if (GetStringToken() == StringToken.At && AdvanceIfNextAre(StringToken.Time, StringToken.Zone, killSkippedTokens: true))
			{
				next = 138;
			}
		}
		return next;
	}

	private int NonEngineLogic(int next)
	{
		switch (next)
		{
		case 297:
			if (compatibilityLevel < 100)
			{
				int num3 = PeekBackward();
				if (num3 == 232 || num3 == 262 || (uint)(num3 - 981) <= 1u)
				{
					next = 983;
				}
			}
			break;
		case 186:
			if ((queryStatus & QueryStatus.P_BREAKDECPOINT) != 0)
			{
				string tokenText = TokenText;
				int num = tokenText.IndexOf('.');
				if (num != -1)
				{
					int startIndex = tokens[endTokenIndex].StartIndex;
					int num2 = ((endTokenIndex + 1 != tokens.Count) ? tokens[endTokenIndex + 1].StartIndex : source.Length);
					next = ((num != 0) ? ((num != tokenText.Length - 1) ? ((!Microsoft.SqlServer.Management.SqlParser.Parser.Internals.Scanner.IsValueTooLargeForTokenInteger(source, startIndex, startIndex + num)) ? ((!Microsoft.SqlServer.Management.SqlParser.Parser.Internals.Scanner.IsValueTooLargeForTokenInteger(source, startIndex + num + 1, num2)) ? 957 : 186) : 186) : ((!Microsoft.SqlServer.Management.SqlParser.Parser.Internals.Scanner.IsValueTooLargeForTokenInteger(source, startIndex, num2 - 1)) ? 956 : 186)) : ((!Microsoft.SqlServer.Management.SqlParser.Parser.Internals.Scanner.IsValueTooLargeForTokenInteger(source, startIndex + 1, num2)) ? 955 : 186));
				}
			}
			break;
		}
		return next;
	}

	internal void SetQueryStatus(QueryStatus qstatus)
	{
		queryStatus |= qstatus;
	}

	internal void ClearQueryStatus(QueryStatus qstatus)
	{
		queryStatus &= ~qstatus;
	}

	internal void SetParseState(EParseState state)
	{
		yystate = state;
	}

	internal void SetQueryState(QueryState qstate)
	{
		queryState = qstate;
	}

	internal void SetStmtDelimiter(bool value)
	{
		isStatementDelimiter = value;
	}

	internal void SetGDRState()
	{
		yystate = EParseState.e_ParseGDR;
		gdrTokenCount = 0;
	}

	internal void ResetAllParseStates()
	{
		yystate = EParseState.e_ParseNeutral;
		queryStatus = QueryStatus.None;
		queryState = QueryState.None;
	}

	internal void RepeatToken()
	{
		nextTokenIndex = startTokenIndex;
	}

	internal void RepeatToken(int tokenId)
	{
		SetId(endTokenIndex, tokenId);
		nextTokenIndex = startTokenIndex;
	}

	private int PeekBackward()
	{
		for (int num = startTokenIndex - 1; num >= 0; num--)
		{
			int id = GetId(num);
			if (id < 985)
			{
				return id;
			}
		}
		return 128;
	}

	private int PeekNextToken()
	{
		int num = endTokenIndex + 1;
		int id;
		while (true)
		{
			id = GetId(num);
			if (id < 985)
			{
				break;
			}
			num++;
		}
		return id;
	}

	private StringToken PeekNextStringToken()
	{
		int num = endTokenIndex + 1;
		int id;
		while (true)
		{
			id = GetId(num);
			if (id < 985)
			{
				break;
			}
			num++;
		}
		if (id == 173)
		{
			return firstLevelTokenManager.GetStringToken(num);
		}
		return StringToken.None;
	}

	private int PeekAfterNextToken()
	{
		int num = endTokenIndex + 1;
		bool flag = false;
		int id;
		while (true)
		{
			id = GetId(num);
			if (id < 985)
			{
				if (flag)
				{
					break;
				}
				flag = true;
			}
			num++;
		}
		return id;
	}

	private StringToken PeekAfterNextStringToken()
	{
		int num = endTokenIndex + 1;
		bool flag = false;
		while (true)
		{
			int id = GetId(num);
			if (id < 985)
			{
				if (id != 173)
				{
					break;
				}
				if (flag)
				{
					return firstLevelTokenManager.GetStringToken(num);
				}
				flag = true;
			}
			num++;
		}
		return StringToken.None;
	}

	private bool AdvanceIfNextIs(int target, bool killSkippedToken)
	{
		int num = endTokenIndex + 1;
		while (true)
		{
			int id = GetId(num);
			if (id == target)
			{
				if (killSkippedToken)
				{
					KillToken(endTokenIndex);
				}
				endTokenIndex = num;
				return true;
			}
			if (id <= 985)
			{
				break;
			}
			num++;
		}
		return false;
	}

	private bool AdvanceIfNextIs(StringToken stringToken, bool killSkippedToken)
	{
		int num = endTokenIndex + 1;
		while (true)
		{
			int id = GetId(num);
			if (id == 173 && firstLevelTokenManager.GetStringToken(num) == stringToken)
			{
				if (killSkippedToken)
				{
					KillToken(endTokenIndex);
				}
				endTokenIndex = num;
				return true;
			}
			if (id <= 985)
			{
				break;
			}
			num++;
		}
		return false;
	}

	private bool AdvanceIfNextAre(StringToken nextStringToken, StringToken afterNextStringToken, bool killSkippedTokens)
	{
		int num = endTokenIndex + 1;
		int tokenNumber = num;
		bool flag = false;
		while (true)
		{
			int id = GetId(num);
			if (!flag && id == 173 && firstLevelTokenManager.GetStringToken(num) == nextStringToken)
			{
				flag = true;
				tokenNumber = num;
			}
			else
			{
				if (flag && id == 173 && firstLevelTokenManager.GetStringToken(num) == afterNextStringToken)
				{
					if (killSkippedTokens)
					{
						KillToken(endTokenIndex);
						KillToken(tokenNumber);
					}
					endTokenIndex = num;
					return true;
				}
				if (id <= 985)
				{
					break;
				}
			}
			num++;
		}
		return false;
	}
}
