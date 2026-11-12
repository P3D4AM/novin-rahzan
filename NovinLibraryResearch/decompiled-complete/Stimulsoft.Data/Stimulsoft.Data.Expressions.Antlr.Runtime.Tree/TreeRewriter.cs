using System;
using Stimulsoft.Data.Expressions.Antlr.Runtime.Misc;

namespace Stimulsoft.Data.Expressions.Antlr.Runtime.Tree;

public class TreeRewriter : TreeParser
{
	protected bool showTransformations;

	protected ITokenStream originalTokenStream;

	protected ITreeAdaptor originalAdaptor;

	private Stimulsoft.Data.Expressions.Antlr.Runtime.Misc.Func<IAstRuleReturnScope> topdown_func;

	private Stimulsoft.Data.Expressions.Antlr.Runtime.Misc.Func<IAstRuleReturnScope> bottomup_func;

	public TreeRewriter(ITreeNodeStream input)
		: this(input, new RecognizerSharedState())
	{
	}

	public TreeRewriter(ITreeNodeStream input, RecognizerSharedState state)
		: base(input, state)
	{
		originalAdaptor = input.TreeAdaptor;
		originalTokenStream = input.TokenStream;
		topdown_func = () => Topdown();
		bottomup_func = () => Bottomup();
	}

	public virtual object ApplyOnce(object t, Stimulsoft.Data.Expressions.Antlr.Runtime.Misc.Func<IAstRuleReturnScope> whichRule)
	{
		if (t == null)
		{
			return null;
		}
		try
		{
			SetState(new RecognizerSharedState());
			SetTreeNodeStream(new CommonTreeNodeStream(originalAdaptor, t));
			((CommonTreeNodeStream)input).TokenStream = originalTokenStream;
			BacktrackingLevel = 1;
			IAstRuleReturnScope astRuleReturnScope = whichRule();
			BacktrackingLevel = 0;
			if (Failed)
			{
				return t;
			}
			if (showTransformations && astRuleReturnScope != null && !t.Equals(astRuleReturnScope.Tree) && astRuleReturnScope.Tree != null)
			{
				ReportTransformation(t, astRuleReturnScope.Tree);
			}
			if (astRuleReturnScope != null && astRuleReturnScope.Tree != null)
			{
				return astRuleReturnScope.Tree;
			}
			return t;
		}
		catch (RecognitionException)
		{
			return t;
		}
	}

	public virtual object ApplyRepeatedly(object t, Stimulsoft.Data.Expressions.Antlr.Runtime.Misc.Func<IAstRuleReturnScope> whichRule)
	{
		bool flag = true;
		while (flag)
		{
			object obj = ApplyOnce(t, whichRule);
			flag = !t.Equals(obj);
			t = obj;
		}
		return t;
	}

	public virtual object Downup(object t)
	{
		return Downup(t, showTransformations: false);
	}

	public virtual object Downup(object t, bool showTransformations)
	{
		this.showTransformations = showTransformations;
		TreeVisitor treeVisitor = new TreeVisitor(new CommonTreeAdaptor());
		t = treeVisitor.Visit(t, (object o) => ApplyOnce(o, topdown_func), (object o) => ApplyRepeatedly(o, bottomup_func));
		return t;
	}

	protected virtual IAstRuleReturnScope Topdown()
	{
		return null;
	}

	protected virtual IAstRuleReturnScope Bottomup()
	{
		return null;
	}

	protected virtual void ReportTransformation(object oldTree, object newTree)
	{
		ITree tree = oldTree as ITree;
		ITree tree2 = newTree as ITree;
		string arg = ((tree != null) ? tree.ToStringTree() : "??");
		string arg2 = ((tree2 != null) ? tree2.ToStringTree() : "??");
		Console.WriteLine("{0} -> {1}", arg, arg2);
	}
}
