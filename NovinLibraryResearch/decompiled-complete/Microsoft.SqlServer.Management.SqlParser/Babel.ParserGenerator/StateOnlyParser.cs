namespace Babel.ParserGenerator;

internal abstract class StateOnlyParser
{
	public readonly SimpleParserScanner scanner;

	protected StateOnlyParser(SimpleParserScanner scanner)
	{
		this.scanner = scanner;
	}

	public abstract StateOnlyParser Create();

	public abstract void DoAction(int ruleNumber);
}
