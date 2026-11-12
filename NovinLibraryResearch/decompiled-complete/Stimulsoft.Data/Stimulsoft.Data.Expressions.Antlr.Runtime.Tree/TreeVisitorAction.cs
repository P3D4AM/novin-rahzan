using Stimulsoft.Data.Expressions.Antlr.Runtime.Misc;

namespace Stimulsoft.Data.Expressions.Antlr.Runtime.Tree;

public class TreeVisitorAction : ITreeVisitorAction
{
	private readonly Func<object, object> _preAction;

	private readonly Func<object, object> _postAction;

	public TreeVisitorAction(Func<object, object> preAction, Func<object, object> postAction)
	{
		_preAction = preAction;
		_postAction = postAction;
	}

	public object Pre(object t)
	{
		if (_preAction != null)
		{
			return _preAction(t);
		}
		return t;
	}

	public object Post(object t)
	{
		if (_postAction != null)
		{
			return _postAction(t);
		}
		return t;
	}
}
