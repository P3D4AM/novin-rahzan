namespace DevComponents.AdvTree;

public class TreeDragDropEventArgs : AdvTreeMultiNodeCancelEventArgs
{
	public readonly Node OldParentNode;

	public readonly Node NewParentNode;

	public bool IsCopy;

	public int InsertPosition;

	public TreeDragDropEventArgs(eTreeAction action, Node[] nodes, Node oldParentNode, Node newParentNode, int insertPosition)
		: base(action, nodes)
	{
		NewParentNode = newParentNode;
		OldParentNode = oldParentNode;
		InsertPosition = insertPosition;
	}

	public TreeDragDropEventArgs(eTreeAction action, Node[] nodes, Node oldParentNode, Node newParentNode, bool isCopy, int insertPosition)
		: base(action, nodes)
	{
		NewParentNode = newParentNode;
		OldParentNode = oldParentNode;
		IsCopy = isCopy;
		InsertPosition = insertPosition;
	}
}
