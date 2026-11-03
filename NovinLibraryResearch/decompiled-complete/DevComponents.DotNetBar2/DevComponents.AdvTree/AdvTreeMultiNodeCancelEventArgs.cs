namespace DevComponents.AdvTree;

public class AdvTreeMultiNodeCancelEventArgs : AdvTreeNodeCancelEventArgs
{
	public Node[] Nodes;

	public AdvTreeMultiNodeCancelEventArgs(eTreeAction action, Node[] nodes)
		: base(action, nodes[0])
	{
		Nodes = nodes;
	}
}
