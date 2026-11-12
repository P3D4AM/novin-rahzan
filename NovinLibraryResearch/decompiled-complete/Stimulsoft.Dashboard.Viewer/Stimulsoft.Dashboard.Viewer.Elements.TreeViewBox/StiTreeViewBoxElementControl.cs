using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Components.TreeView;
using Stimulsoft.Dashboard.Components.TreeViewBox;
using Stimulsoft.Dashboard.Drawing.Helpers;
using Stimulsoft.Dashboard.Viewer.Controls.TreeView;
using Stimulsoft.Dashboard.Viewer.Elements.ListBox;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Viewer.Elements.TreeViewBox;

[ToolboxItem(false)]
public abstract class StiTreeViewBoxElementControl : StiFilterElementControl
{
	protected StiTreeViewBoxElement TreeViewBoxElement => base.Element as StiTreeViewBoxElement;

	protected bool ShowAllValue => TreeViewBoxElement.ShowAllValue;

	protected StiTreeViewBox TreeViewBoxControl => base.Child as StiTreeViewBox;

	public override void RefreshChild(StiDataTable dataTable)
	{
		base.RefreshChild(dataTable);
		base.IsShowNoResult = dataTable == null;
		base.IsShowEmptyData = dataTable?.IsEmpty ?? false;
		if (base.IsShowNoResult || base.IsShowEmptyData)
		{
			return;
		}
		List<StiTreeItem> items = StiTreeViewHelper.FetchItems(dataTable, TreeViewBoxElement.ShowBlanks);
		base.LockRefresh = true;
		TreeViewBoxControl.BeginUpdate();
		TreeViewBoxControl.Nodes.Clear();
		if (ShowAllValue)
		{
			StiTreeViewBox treeViewBoxControl = TreeViewBoxControl;
			if (treeViewBoxControl != null)
			{
				treeViewBoxControl.Nodes.Add((TreeNode)(object)new StiTreeNode(StiLocHelper.LocAll));
			}
		}
		AddNodes(TreeViewBoxControl.Nodes, items);
		TreeViewBoxControl.SelectedNode = ((IEnumerable)TreeViewBoxControl.Nodes).Cast<StiTreeNode>().FirstOrDefault();
		SetNodeStates(TreeViewBoxControl.Nodes, value: true);
		StiTreeNode selectedNode = TreeViewBoxControl.SelectedNode;
		if (selectedNode != null)
		{
			((TreeNode)selectedNode).EnsureVisible();
		}
		TreeViewBoxControl.EndUpdate();
		base.IsFirstInitialized = true;
		base.LockRefresh = false;
	}

	private void SetNodeStates(TreeNodeCollection nodes, bool value)
	{
		foreach (StiTreeNode node in nodes)
		{
			node.State = ((!value) ? StiCheckState.Unchecked : StiCheckState.Checked);
			SetNodeStates(((TreeNode)node).Nodes, value);
		}
	}

	private void AddNodes(TreeNodeCollection nodes, List<StiTreeItem> items)
	{
		if (items == null)
		{
			return;
		}
		foreach (StiTreeItem item in items)
		{
			StiTreeNode stiTreeNode = new StiTreeNode(StiTreeViewHelper.Format(TreeViewBoxElement, (item.Key == null) ? string.Empty : item.Key));
			((TreeNode)stiTreeNode).Checked = true;
			((TreeNode)stiTreeNode).Tag = item;
			StiTreeNode stiTreeNode2 = stiTreeNode;
			AddNodes(((TreeNode)stiTreeNode2).Nodes, item.Items);
			nodes.Add((TreeNode)(object)stiTreeNode2);
		}
	}

	protected override void OnSizeChanged(EventArgs e)
	{
		((Control)this).OnSizeChanged(e);
		base.Child.Width = ((Control)this).Width;
		base.Child.Top = (((Control)this).Height - base.Child.Height) / 2;
	}

	public StiTreeViewBoxElementControl(IStiElement element)
		: base(element)
	{
	}
}
