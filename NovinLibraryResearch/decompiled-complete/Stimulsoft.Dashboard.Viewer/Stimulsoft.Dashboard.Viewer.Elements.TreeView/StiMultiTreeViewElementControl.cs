using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Components.TreeView;
using Stimulsoft.Dashboard.Drawing.Helpers;
using Stimulsoft.Dashboard.Viewer.Controls;
using Stimulsoft.Dashboard.Viewer.Controls.TreeView;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Styles;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Dashboard.Viewer.Elements.TreeView;

[ToolboxItem(false)]
public class StiMultiTreeViewElementControl : StiTreeViewElementControl
{
	private StiAllCheckBox AllCheckBox { get; set; }

	public StiKeyTreeViewMeter KeyMeter => base.TreeViewElement.KeyMeters?.LastOrDefault();

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
		if (!base.IsShowNoResult && !base.IsShowEmptyData)
		{
			List<StiTreeItem> items = StiTreeViewHelper.FetchItems(dataTable, base.TreeViewElement.ShowBlanks);
			base.LockRefresh = true;
			((TreeView)base.TreeViewControl).BeginUpdate();
			((TreeView)base.TreeViewControl).Nodes.Clear();
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
	}

	public override void CreateChild()
	{
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected O, but got Unknown
		StiTreeView obj = new StiTreeView
		{
			Colors = GetColorCollection()
		};
		((Control)obj).Dock = (DockStyle)5;
		base.Child = (Control)(object)obj;
		if (base.Element is IStiControlElement)
		{
			((Control)base.TreeViewControl).Font = StiDashboardStyleHelper.GetFont((IStiControlElement)base.Element);
		}
		((Control)this).Controls.Add(base.Child);
		CreateSelectAllChild();
		((TreeView)base.TreeViewControl).AfterCheck += new TreeViewEventHandler(TreeViewControl_AfterCheck);
	}

	protected override void RefreshFilter()
	{
		if (KeyMeter == null)
		{
			return;
		}
		string columnName = KeyMeter.Expression;
		if (GetIsAllItemChecked())
		{
			base.TreeViewElement.UserFilters = new List<StiDataFilterRule>();
		}
		else if (GetIsAllItemUnchecked())
		{
			base.TreeViewElement.UserFilters = new StiDataFilterRule(columnName, StiDataFilterCondition.IsFalse).ToList();
		}
		else
		{
			base.TreeViewElement.UserFilters = (from StiTreeItem m in from i in GetCheckedNodes()
					select i.Tag
				where m.Meter == KeyMeter && m.Key != null
				select StiDataFilterCreator.CreateEqualBasedOnValue(m.Key, columnName, base.TreeViewElement)).ToList();
		}
		RefreshAllChilds();
		RefreshViewerData();
	}

	protected override void ResetFilter()
	{
		base.TreeViewElement.UserFilters = new List<StiDataFilterRule>();
	}

	private void CreateSelectAllChild()
	{
		if (base.TreeViewElement.ShowAllValue)
		{
			StiAllCheckBox stiAllCheckBox = new StiAllCheckBox();
			((CheckBox)stiAllCheckBox).Checked = true;
			((Control)stiAllCheckBox).Dock = (DockStyle)1;
			stiAllCheckBox.Colors = GetColorCollection();
			((Control)stiAllCheckBox).Font = ((Control)base.TreeViewControl).Font;
			AllCheckBox = stiAllCheckBox;
			((CheckBox)AllCheckBox).CheckedChanged += AllCheckBox_CheckedChanged;
			((Control)this).Controls.Add((Control)(object)AllCheckBox);
		}
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

	private bool GetIsAllItemChecked()
	{
		foreach (StiTreeNode node in ((TreeView)base.TreeViewControl).Nodes)
		{
			if (!(((TreeNode)node).Text == StiLocHelper.LocAll) && node.State != StiCheckState.Checked)
			{
				return false;
			}
		}
		return true;
	}

	private bool GetIsAllItemUnchecked()
	{
		foreach (StiTreeNode node in ((TreeView)base.TreeViewControl).Nodes)
		{
			if (!(((TreeNode)node).Text == StiLocHelper.LocAll) && node.State != StiCheckState.Unchecked)
			{
				return false;
			}
		}
		return true;
	}

	private List<TreeNode> GetCheckedNodes(TreeNodeCollection nodes = null)
	{
		if (nodes == null)
		{
			nodes = ((TreeView)base.TreeViewControl).Nodes;
		}
		List<TreeNode> list = new List<TreeNode>();
		foreach (StiTreeNode node in nodes)
		{
			if (((TreeNode)node).Tag != null && node.State != StiCheckState.Unchecked)
			{
				list.Add((TreeNode)(object)node);
			}
			List<TreeNode> checkedNodes = GetCheckedNodes(((TreeNode)node).Nodes);
			list.AddRange(checkedNodes);
		}
		return list;
	}

	protected override void Dispose(bool disposing)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		if (disposing)
		{
			if (base.TreeViewControl != null)
			{
				((TreeView)base.TreeViewControl).AfterCheck -= new TreeViewEventHandler(TreeViewControl_AfterCheck);
			}
			if (AllCheckBox != null)
			{
				((CheckBox)AllCheckBox).CheckedChanged -= AllCheckBox_CheckedChanged;
			}
		}
		base.Dispose(disposing);
	}

	private void AllCheckBox_CheckedChanged(object sender, EventArgs e)
	{
		if (!base.LockRefresh)
		{
			base.LockRefresh = true;
			SetNodeStates(((TreeView)base.TreeViewControl).Nodes, ((CheckBox)AllCheckBox).Checked);
			StartRefreshingFilter();
			base.LockRefresh = false;
		}
	}

	private void TreeViewControl_AfterCheck(object sender, TreeViewEventArgs e)
	{
		if (base.LockRefresh)
		{
			return;
		}
		try
		{
			base.LockRefresh = true;
			if (base.ShowAllValue)
			{
				if (GetIsAllItemChecked())
				{
					((CheckBox)AllCheckBox).CheckState = (CheckState)1;
				}
				else if (GetIsAllItemUnchecked())
				{
					((CheckBox)AllCheckBox).CheckState = (CheckState)0;
				}
				else
				{
					((CheckBox)AllCheckBox).CheckState = (CheckState)2;
				}
			}
		}
		finally
		{
			base.LockRefresh = false;
		}
		StartRefreshingFilter();
	}

	public StiMultiTreeViewElementControl(IStiElement element)
		: base(element)
	{
	}
}
