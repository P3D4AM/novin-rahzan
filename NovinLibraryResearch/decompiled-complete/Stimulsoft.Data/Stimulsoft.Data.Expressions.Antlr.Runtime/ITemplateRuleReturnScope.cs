namespace Stimulsoft.Data.Expressions.Antlr.Runtime;

public interface ITemplateRuleReturnScope
{
	object Template { get; }
}
public interface ITemplateRuleReturnScope<TTemplate> : ITemplateRuleReturnScope
{
	new TTemplate Template { get; }
}
