using System.Collections;
using System.Windows.Forms;

namespace Stimulsoft.Report.Design;

public sealed class StiNodesMemory
{
	private Hashtable savedNodes = new Hashtable();

	private string selectedText;

	private bool nodesSave;

	private static string GetFullPath(TreeNode node)
	{
		string text = node.Text;
		while (node.Parent != null)
		{
			node = node.Parent;
			text = text + "." + node.Text;
		}
		return text;
	}

	private void Clear()
	{
		savedNodes.Clear();
	}

	public void SaveExpNodes(TreeView treeView)
	{
		SaveExpNodes(treeView, forceNodesSave: true);
	}

	public void SaveExpNodes(TreeView treeView, bool forceNodesSave)
	{
		if (treeView.SelectedNode == null)
		{
			selectedText = null;
		}
		else
		{
			selectedText = GetFullPath(treeView.SelectedNode);
		}
		if (nodesSave || forceNodesSave)
		{
			Clear();
			SaveExpNodes(treeView.Nodes);
		}
		nodesSave = true;
	}

	private void SaveExpNodes(TreeNodeCollection nodes)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		foreach (TreeNode node in nodes)
		{
			TreeNode val = node;
			if (val.IsExpanded)
			{
				savedNodes[GetFullPath(val)] = val;
			}
			SaveExpNodes(val.Nodes);
		}
	}

	public void LoadExpNodes(TreeView treeView, bool restoreSelectedNode)
	{
		LoadExpNodes(treeView.Nodes, treeView, restoreSelectedNode);
	}

	private void LoadExpNodes(TreeNodeCollection nodes, TreeView treeView, bool restoreSelectedNode)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		foreach (TreeNode node in nodes)
		{
			TreeNode val = node;
			string fullPath = GetFullPath(val);
			if (restoreSelectedNode && fullPath == selectedText)
			{
				treeView.SelectedNode = val;
				treeView.SelectedNode.EnsureVisible();
			}
			if (savedNodes[fullPath] != null)
			{
				val.Expand();
			}
			LoadExpNodes(val.Nodes, treeView, restoreSelectedNode);
		}
	}
}
