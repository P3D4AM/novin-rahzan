using System;
using System.Windows.Forms;

namespace DevComponents.AdvTree;

public class TreeDragFeedbackEventArgs : EventArgs
{
	public bool AllowDrop = true;

	public Node ParentNode;

	public int InsertPosition;

	private Node __29B5;

	internal bool _29B6;

	private DragDropEffects __29B7;

	public Node DragNode
	{
		get
		{
			return __29B5;
		}
		set
		{
			__29B5 = value;
		}
	}

	public DragDropEffects Effect
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return __29B7;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			__29B7 = value;
			_29B6 = true;
		}
	}

	public TreeDragFeedbackEventArgs(Node parentNode, int insertPosition, Node dragNode)
	{
		ParentNode = parentNode;
		InsertPosition = insertPosition;
		__29B5 = dragNode;
	}

	public TreeDragFeedbackEventArgs(Node parentNode, int insertPosition, Node dragNode, DragDropEffects effect)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		ParentNode = parentNode;
		InsertPosition = insertPosition;
		__29B5 = dragNode;
		__29B7 = effect;
	}

	public TreeDragFeedbackEventArgs()
	{
	}
}
