namespace Stimulsoft.Editor;

public interface ILexSyntaxBlock
{
	string Name { get; set; }

	string Desc { get; set; }

	ILexStyle Style { get; set; }

	ILexState LeaveState { get; set; }

	ILexReswordSets LexReswordSets { get; set; }

	string[] Expressions { get; set; }

	string Expression { get; }

	int Index { get; }

	int FindResword(string resword);

	int AddExpression(string expression);
}
