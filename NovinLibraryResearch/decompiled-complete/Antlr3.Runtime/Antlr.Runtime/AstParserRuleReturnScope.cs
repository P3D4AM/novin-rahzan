namespace Antlr.Runtime;

public class AstParserRuleReturnScope<TTree, TToken> : ParserRuleReturnScope<TToken>, IAstRuleReturnScope<TTree>, IAstRuleReturnScope, IRuleReturnScope
{
	private TTree _tree;

	public TTree Tree
	{
		get
		{
			return _tree;
		}
		set
		{
			_tree = value;
		}
	}

	object IAstRuleReturnScope.Tree => Tree;
}
