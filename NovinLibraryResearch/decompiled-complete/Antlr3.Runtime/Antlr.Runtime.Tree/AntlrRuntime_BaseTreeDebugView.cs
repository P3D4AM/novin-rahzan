using System.Diagnostics;

namespace Antlr.Runtime.Tree;

internal sealed class AntlrRuntime_BaseTreeDebugView
{
	private readonly BaseTree _tree;

	[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
	public ITree[] Children
	{
		get
		{
			if (_tree == null || _tree.Children == null)
			{
				return null;
			}
			ITree[] array = new ITree[_tree.Children.Count];
			_tree.Children.CopyTo(array, 0);
			return array;
		}
	}

	public AntlrRuntime_BaseTreeDebugView(BaseTree tree)
	{
		_tree = tree;
	}
}
