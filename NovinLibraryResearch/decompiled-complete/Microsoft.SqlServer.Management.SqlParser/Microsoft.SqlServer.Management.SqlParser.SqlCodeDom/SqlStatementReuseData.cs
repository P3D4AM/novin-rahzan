using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

internal class SqlStatementReuseData
{
	private readonly State beforeParserState;

	private readonly State afterParserState;

	private readonly SimpleScannerState beforeScannerStateRelative;

	private readonly CompleteScannerState afterScannerStateRelative;

	private readonly int lookAheadTokenIndexRelative;

	private readonly LexLocation yylocRelative;

	private int localPositionOffset;

	private SideEffectSpan sideEffectSpan;

	private bool areChildrenParentsSet;

	public State BeforeParserState => beforeParserState;

	public State AfterParserState => afterParserState;

	public SimpleScannerState BeforeScannerStateRelative => beforeScannerStateRelative;

	public CompleteScannerState AfterScannerStateRelative => afterScannerStateRelative;

	public int LookAheadTokenIndexRelative => lookAheadTokenIndexRelative;

	public LexLocation YYLocRelative => yylocRelative;

	public int LocalPositionOffset
	{
		get
		{
			return localPositionOffset;
		}
		set
		{
			localPositionOffset = value;
		}
	}

	public SideEffectSpan SideEffectSpan
	{
		get
		{
			return sideEffectSpan;
		}
		set
		{
			sideEffectSpan = value;
		}
	}

	public bool AreChildrenParentsSet
	{
		get
		{
			return areChildrenParentsSet;
		}
		set
		{
			areChildrenParentsSet = value;
		}
	}

	public SqlStatementReuseData(State beforeParserState, State afterParserState, SimpleScannerState beforeScannerStateRelative, CompleteScannerState afterScannerStateRelative, int lookAheadTokenIndexRelative, LexLocation yylocRelative)
	{
		this.beforeParserState = beforeParserState;
		this.afterParserState = afterParserState;
		this.beforeScannerStateRelative = beforeScannerStateRelative;
		this.afterScannerStateRelative = afterScannerStateRelative;
		this.lookAheadTokenIndexRelative = lookAheadTokenIndexRelative;
		this.yylocRelative = yylocRelative;
	}
}
