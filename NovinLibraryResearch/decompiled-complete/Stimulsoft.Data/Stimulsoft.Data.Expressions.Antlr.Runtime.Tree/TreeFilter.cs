using Stimulsoft.Data.Expressions.Antlr.Runtime.Misc;

namespace Stimulsoft.Data.Expressions.Antlr.Runtime.Tree;

public class TreeFilter : TreeParser
{
	protected ITokenStream originalTokenStream;

	protected ITreeAdaptor originalAdaptor;

	public TreeFilter(ITreeNodeStream input)
		: this(input, new RecognizerSharedState())
	{
	}

	public TreeFilter(ITreeNodeStream input, RecognizerSharedState state)
		: base(input, state)
	{
		originalAdaptor = input.TreeAdaptor;
		originalTokenStream = input.TokenStream;
	}

	public virtual void ApplyOnce(object t, Action whichRule)
	{
		if (t == null)
		{
			return;
		}
		try
		{
			SetState(new RecognizerSharedState());
			SetTreeNodeStream(new CommonTreeNodeStream(originalAdaptor, t));
			((CommonTreeNodeStream)input).TokenStream = originalTokenStream;
			BacktrackingLevel = 1;
			whichRule();
			BacktrackingLevel = 0;
		}
		catch (RecognitionException)
		{
		}
	}

	public virtual void Downup(object t)
	{
		TreeVisitor treeVisitor = new TreeVisitor(new CommonTreeAdaptor());
		Func<object, object> preAction = delegate(object o)
		{
			ApplyOnce(o, Topdown);
			return o;
		};
		Func<object, object> postAction = delegate(object o)
		{
			ApplyOnce(o, Bottomup);
			return o;
		};
		treeVisitor.Visit(t, preAction, postAction);
	}

	protected virtual void Topdown()
	{
	}

	protected virtual void Bottomup()
	{
	}
}
