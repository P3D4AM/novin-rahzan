using System;

namespace Antlr.Runtime.Tree;

[Serializable]
public class TreeRuleReturnScope<TTree> : IRuleReturnScope<TTree>, IRuleReturnScope
{
	private TTree _start;

	public TTree Start
	{
		get
		{
			return _start;
		}
		set
		{
			_start = value;
		}
	}

	object IRuleReturnScope.Start => Start;

	TTree IRuleReturnScope<TTree>.Stop => default(TTree);

	object IRuleReturnScope.Stop => default(TTree);
}
