using System.Windows.Forms;

namespace DevComponents.AdvTree;

public class MultiNodeTreeDragFeedbackEventArgs : TreeDragFeedbackEventArgs
{
	private Node[] __29DA;

	public Node[] DragNodes
	{
		get
		{
			return __29DA;
		}
		set
		{
			__29DA = value;
		}
	}

	public MultiNodeTreeDragFeedbackEventArgs(Node parentNode, int insertPosition, Node[] dragNodes)
		: base(parentNode, insertPosition, dragNodes[0])
	{
		__29DA = dragNodes;
	}

	public MultiNodeTreeDragFeedbackEventArgs(Node parentNode, int insertPosition, Node[] dragNodes, DragDropEffects effect)
		: base(parentNode, insertPosition, dragNodes[0], effect)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		__29DA = dragNodes;
	}
}
