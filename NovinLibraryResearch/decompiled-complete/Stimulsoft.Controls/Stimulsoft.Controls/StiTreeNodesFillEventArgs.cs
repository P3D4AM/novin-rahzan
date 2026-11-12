using System;
using System.Windows.Forms;

namespace Stimulsoft.Controls;

public class StiTreeNodesFillEventArgs : EventArgs
{
	public TreeNodeCollection Nodes;

	public StiTreeView TreeView;

	public StiTreeNodesFillEventArgs(TreeNodeCollection nodes, StiTreeView treeView)
	{
		Nodes = nodes;
		TreeView = treeView;
	}
}
