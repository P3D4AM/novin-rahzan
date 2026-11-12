namespace Antlr.Runtime;

public interface IRuleReturnScope
{
	object Start { get; }

	object Stop { get; }
}
public interface IRuleReturnScope<TLabel> : IRuleReturnScope
{
	new TLabel Start { get; }

	new TLabel Stop { get; }
}
