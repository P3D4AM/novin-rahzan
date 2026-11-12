using Microsoft.SqlServer.Management.SqlParser.Common;
using Microsoft.SqlServer.Management.SqlParser.Parser;

namespace Babel.ParserGenerator;

internal class ParserScanner : SimpleParserScanner
{
	private readonly ParserTokenList tokenList;

	private int nextBatchNumber;

	public int LookaheadTokenIndex => tokenList.MaxReadKey;

	public int CurrentBatchNumber => nextBatchNumber - 1;

	private ParserScanner(TokenManager firstLevelTokenManager, ParserTokenList tokenList, TransactSqlVersion transactSqlVersion)
		: base(firstLevelTokenManager, tokenList, transactSqlVersion)
	{
		this.tokenList = tokenList;
	}

	public static ParserScanner CreateParserScanner(TokenManager firstLevelTokenManager, TransactSqlVersion transactSqlVersion)
	{
		ParserTokenList parserTokenList = new ParserTokenList(firstLevelTokenManager.TokenList, firstLevelTokenManager.Source.Length);
		return new ParserScanner(firstLevelTokenManager, parserTokenList, transactSqlVersion);
	}

	public TokenManager CreateTokenManager()
	{
		return new TokenManager(firstLevelTokenManager.Source, tokenList.CreateReadonlyMinimalList(), firstLevelTokenManager.NewLines, firstLevelTokenManager.BatchSeparators, firstLevelTokenManager.CompatibilityLevel);
	}

	private void InitializeRange(LexLocation tokenRange)
	{
		startTokenIndex = -1;
		endTokenIndex = -1;
		nextTokenIndex = tokenRange.startTokenNumber;
		tokenList.SetCurrentRange(tokenRange);
		yystate = EParseState.e_ParseNeutral;
		lastSimpleScannerState = new SimpleScannerState(GetScannerState(), nextTokenIndex);
		isStatementDelimiter = true;
		gdrTokenCount = 0;
		queryStatus = QueryStatus.None;
	}

	public bool MoveToNextBatch()
	{
		if (nextBatchNumber < firstLevelTokenManager.GetBatchCount())
		{
			InitializeRange(firstLevelTokenManager.GetBatchRange(nextBatchNumber));
			nextBatchNumber++;
			token = -1;
			NextToken();
			return true;
		}
		return false;
	}

	public void ForwardScannerState(CompleteScannerState state, ModifiedMinimalList<TokenData> prevTokenList, TokenUpdateDiff diff)
	{
		int num = endTokenIndex + 1;
		int tokenIndex = state.TokenIndex;
		tokenList.CopyModifications(prevTokenList, diff.UnfixTokenIndex(num), diff.UnfixTokenIndex(tokenIndex), num, tokenIndex);
		startTokenIndex = -1;
		endTokenIndex = -1;
		nextTokenIndex = state.TokenIndex;
		RestoreScannerState(state.InitialScannerState);
		token = -1;
		NextToken();
		if (state.IsTokenRead)
		{
			ReadToken();
		}
		RestoreScannerState(state.FinalScannerState);
	}
}
