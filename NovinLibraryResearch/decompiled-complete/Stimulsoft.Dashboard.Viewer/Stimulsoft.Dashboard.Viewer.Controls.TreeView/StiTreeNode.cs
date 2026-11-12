using System.Collections;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Dashboard.Viewer.Controls.TreeView;

public class StiTreeNode : TreeNode
{
	private StiCheckState state = StiCheckState.Unchecked;

	public StiCheckState State
	{
		get
		{
			return state;
		}
		set
		{
			if (state != value)
			{
				state = value;
				((TreeNode)this).Checked = state == StiCheckState.Checked;
				InvalidateNode((TreeNode)(object)this);
			}
		}
	}

	public void Toggle(StiCheckState state)
	{
		if (state == StiCheckState.Unchecked)
		{
			State = StiCheckState.Checked;
		}
		else
		{
			State = StiCheckState.Unchecked;
		}
		UpdateStates();
	}

	public void Toggle()
	{
		Toggle(State);
	}

	private StiCheckState GetChildsState()
	{
		if (((TreeNode)this).Parent == null || ((TreeNode)this).Parent.Nodes.Count == 1)
		{
			return State;
		}
		StiCheckState stiCheckState = (StiCheckState)0;
		foreach (TreeNode item in from TreeNode t in (IEnumerable)((TreeNode)this).Parent.Nodes
			where t is StiTreeNode
			select t)
		{
			if (item is StiTreeNode stiTreeNode)
			{
				stiCheckState |= stiTreeNode.State;
			}
			if (stiCheckState == StiCheckState.Indeterminate)
			{
				break;
			}
		}
		if (stiCheckState != 0)
		{
			return stiCheckState;
		}
		return StiCheckState.Unchecked;
	}

	private void InvalidateNode(TreeNode node)
	{
		Rectangle rectangle = new Rectangle(0, node.Bounds.Y, ((Control)((TreeNode)this).TreeView).Width, node.Bounds.Height);
		((Control)((TreeNode)this).TreeView).Invalidate(rectangle);
	}

	public void UpdateStates()
	{
		if (State != StiCheckState.Indeterminate)
		{
			UpdateChildStates();
		}
		UpdateParentNodeState(isBegining: true);
	}

	private void UpdateChildStates()
	{
		foreach (TreeNode item in from TreeNode c in (IEnumerable)((TreeNode)this).Nodes
			where c is StiTreeNode
			select c)
		{
			StiTreeNode stiTreeNode = item as StiTreeNode;
			stiTreeNode.State = State;
			((TreeNode)stiTreeNode).Checked = State != StiCheckState.Unchecked;
			stiTreeNode.UpdateChildStates();
			InvalidateNode((TreeNode)(object)stiTreeNode);
		}
	}

	private void UpdateParentNodeState(bool isBegining)
	{
		if (((TreeNode)this).Parent is StiTreeNode stiTreeNode)
		{
			StiCheckState stiCheckState = ((isBegining || State != StiCheckState.Indeterminate) ? GetChildsState() : StiCheckState.Indeterminate);
			if (stiTreeNode.State != stiCheckState)
			{
				stiTreeNode.State = stiCheckState;
				((TreeNode)stiTreeNode).Checked = stiCheckState != StiCheckState.Unchecked;
				stiTreeNode.UpdateParentNodeState(isBegining: false);
				InvalidateNode((TreeNode)(object)stiTreeNode);
			}
		}
	}

	public StiTreeNode()
	{
	}

	public StiTreeNode(string text)
		: base(text)
	{
	}

	public StiTreeNode(string text, StiTreeNode[] childs)
		: base(text, (TreeNode[])(object)childs)
	{
	}

	public StiTreeNode(string text, int imageIndex, int selectedImageIndex)
		: base(text, imageIndex, selectedImageIndex)
	{
	}

	public StiTreeNode(string text, int imageIndex, int selectedImageIndex, StiTreeNode[] childs)
		: base(text, imageIndex, selectedImageIndex, (TreeNode[])(object)childs)
	{
	}
}
