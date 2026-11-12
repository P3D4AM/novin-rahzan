using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Components.TreeView;
using Stimulsoft.Dashboard.Drawing.Helpers;
using Stimulsoft.Dashboard.Viewer.Controls.TreeView;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Styles;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Dashboard.Viewer.Elements.TreeView;

[ToolboxItem(false)]
public class StiOneTreeViewElementControl : StiTreeViewElementControl
{
	public override void RefreshControl(object sender, StiReportControlUpdateEventArgs e)
	{
		if (sender == base.Element && base.TreeViewControl != null)
		{
			base.TreeViewControl.Colors = GetColorCollection();
		}
	}

	public override void RefreshChild(StiDataTable dataTable)
	{
		base.RefreshChild(dataTable);
		base.IsShowNoResult = dataTable == null;
		base.IsShowEmptyData = dataTable?.IsEmpty ?? false;
		if (base.IsShowNoResult || base.IsShowEmptyData)
		{
			return;
		}
		List<StiTreeItem> items = StiTreeViewHelper.FetchItems(dataTable, base.TreeViewElement.ShowBlanks);
		base.LockRefresh = true;
		((TreeView)base.TreeViewControl).BeginUpdate();
		((TreeView)base.TreeViewControl).Nodes.Clear();
		if (base.ShowAllValue)
		{
			StiTreeView treeViewControl = base.TreeViewControl;
			if (treeViewControl != null)
			{
				((TreeView)treeViewControl).Nodes.Add((TreeNode)(object)new StiTreeNode(StiLocHelper.LocAll));
			}
		}
		AddNodes(((TreeView)base.TreeViewControl).Nodes, items);
		((TreeView)base.TreeViewControl).SelectedNode = ((IEnumerable)((TreeView)base.TreeViewControl).Nodes).Cast<TreeNode>().FirstOrDefault();
		SetNodeStates(((TreeView)base.TreeViewControl).Nodes, value: true);
		TreeNode selectedNode = ((TreeView)base.TreeViewControl).SelectedNode;
		if (selectedNode != null)
		{
			selectedNode.EnsureVisible();
		}
		((TreeView)base.TreeViewControl).EndUpdate();
		base.IsFirstInitialized = true;
		base.LockRefresh = false;
	}

	public override void CreateChild()
	{
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Expected O, but got Unknown
		StiTreeView obj = new StiTreeView
		{
			Colors = GetColorCollection()
		};
		((Control)obj).Dock = (DockStyle)5;
		((TreeView)obj).CheckBoxes = false;
		base.Child = (Control)(object)obj;
		if (base.Element is IStiControlElement)
		{
			((Control)base.TreeViewControl).Font = StiDashboardStyleHelper.GetFont((IStiControlElement)base.Element);
		}
		((Control)this).Controls.Add(base.Child);
		if (!base.ShowAllValue)
		{
			StartRefreshingFilter();
		}
		((TreeView)base.TreeViewControl).AfterSelect += new TreeViewEventHandler(TreeViewControl_AfterSelect);
	}

	protected override void RefreshFilter()
	{
		if (((TreeView)base.TreeViewControl).SelectedNode == null)
		{
			return;
		}
		if (((TreeView)base.TreeViewControl).SelectedNode == GetSelectAllItem())
		{
			base.TreeViewElement.UserFilters = new List<StiDataFilterRule>();
		}
		else
		{
			if (!(((TreeView)base.TreeViewControl).SelectedNode.Tag is StiTreeItem stiTreeItem))
			{
				return;
			}
			string expression = stiTreeItem.Meter.Expression;
			base.TreeViewElement.UserFilters = StiDataFilterCreator.CreateEqualBasedOnValue(stiTreeItem.Key, expression, base.TreeViewElement).ToList();
		}
		RefreshAllChilds();
		RefreshViewerData();
	}

	protected override void ResetFilter()
	{
		base.TreeViewElement.UserFilters = new List<StiDataFilterRule>();
	}

	private TreeNode GetSelectAllItem()
	{
		if (!base.ShowAllValue)
		{
			return null;
		}
		return ((IEnumerable)((TreeView)base.TreeViewControl).Nodes).Cast<TreeNode>().FirstOrDefault();
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
			StiTreeNode stiTreeNode = new StiTreeNode(StiTreeViewHelper.Format(base.TreeViewElement, (item.Key == null) ? string.Empty : item.Key));
			((TreeNode)stiTreeNode).Checked = true;
			((TreeNode)stiTreeNode).Tag = item;
			StiTreeNode stiTreeNode2 = stiTreeNode;
			AddNodes(((TreeNode)stiTreeNode2).Nodes, item.Items);
			nodes.Add((TreeNode)(object)stiTreeNode2);
		}
	}

	protected override void Dispose(bool disposing)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		if (disposing && base.TreeViewControl != null)
		{
			((TreeView)base.TreeViewControl).AfterSelect -= new TreeViewEventHandler(TreeViewControl_AfterSelect);
		}
		base.Dispose(disposing);
	}

	private void TreeViewControl_AfterSelect(object sender, TreeViewEventArgs e)
	{
		if (!base.LockRefresh)
		{
			StartRefreshingFilter();
		}
	}

	public StiOneTreeViewElementControl(IStiElement element)
		: base(element)
	{
	}
}
