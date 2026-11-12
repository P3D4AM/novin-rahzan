using Antlr.Runtime.Misc;

namespace Antlr.Runtime.Tree;

public class TreeVisitor
{
	protected ITreeAdaptor adaptor;

	public TreeVisitor(ITreeAdaptor adaptor)
	{
		this.adaptor = adaptor;
	}

	public TreeVisitor()
		: this(new CommonTreeAdaptor())
	{
	}

	public object Visit(object t, ITreeVisitorAction action)
	{
		bool flag = adaptor.IsNil(t);
		if (action != null && !flag)
		{
			t = action.Pre(t);
		}
		for (int i = 0; i < adaptor.GetChildCount(t); i++)
		{
			object child = adaptor.GetChild(t, i);
			Visit(child, action);
		}
		if (action != null && !flag)
		{
			t = action.Post(t);
		}
		return t;
	}

	public object Visit(object t, Func<object, object> preAction, Func<object, object> postAction)
	{
		return Visit(t, new TreeVisitorAction(preAction, postAction));
	}
}
