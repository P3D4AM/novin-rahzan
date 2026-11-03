using System.Collections;

namespace DevComponents.AdvTree;

public class NodeFlatIndexComparer : IComparer
{
	private AdvTree __29D9;

	public NodeFlatIndexComparer(AdvTree treeControl)
	{
		__29D9 = treeControl;
	}

	public virtual int Compare(object x, object y)
	{
		Node node = (Node)x;
		Node node2 = (Node)y;
		if (__29D9.GetNodeFlatIndex(node) < __29D9.GetNodeFlatIndex(node2))
		{
			return -1;
		}
		return 1;
	}
}
