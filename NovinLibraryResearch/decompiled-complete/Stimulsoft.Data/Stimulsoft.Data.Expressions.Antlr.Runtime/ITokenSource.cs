namespace Stimulsoft.Data.Expressions.Antlr.Runtime;

public interface ITokenSource
{
	string SourceName { get; }

	string[] TokenNames { get; }

	IToken NextToken();
}
