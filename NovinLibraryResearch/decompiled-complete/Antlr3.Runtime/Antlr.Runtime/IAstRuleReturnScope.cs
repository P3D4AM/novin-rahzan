namespace Antlr.Runtime;

public interface IAstRuleReturnScope : IRuleReturnScope
{
	object Tree { get; }
}
public interface IAstRuleReturnScope<TAstLabel> : IAstRuleReturnScope, IRuleReturnScope
{
	new TAstLabel Tree { get; }
}
