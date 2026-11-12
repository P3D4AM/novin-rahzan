using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Components.TreeView;
using Stimulsoft.Dashboard.Components.TreeViewBox;
using Stimulsoft.Dashboard.Drawing.Helpers;
using Stimulsoft.Dashboard.Viewer.Controls.TreeView;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Styles;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Dashboard.Viewer.Elements.TreeViewBox;

[ToolboxItem(false)]
public class StiMultiTreeViewBoxElementControl : StiTreeViewBoxElementControl
{
	public StiKeyTreeViewBoxMeter KeyMeter => base.TreeViewBoxElement.KeyMeters?.LastOrDefault();

	public override void RefreshControl(object sender, StiReportControlUpdateEventArgs e)
	{
		if (sender == base.Element && base.TreeViewBoxControl != null)
		{
			base.TreeViewBoxControl.Colors = GetColorCollection();
		}
	}

	public override void CreateChild()
	{
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Expected O, but got Unknown
		base.Child = (Control)(object)new StiTreeViewBox
		{
			Colors = GetColorCollection(),
			AllowCloseDropDownOnDoubleClick = false,
			AllowShowCheckedZones = true,
			CheckBoxes = true,
			KeyMeter = KeyMeter,
			TextFormat = base.TreeViewBoxElement.TextFormat,
			IsTransparent = IsTransparent
		};
		if (IsTransparent)
		{
			(base.Child as StiTreeViewBox).DropDownBackColor = StiDashboardStyleHelper.GetBackColor(base.Element);
		}
		if (base.Element is IStiControlElement)
		{
			base.Child.Font = StiDashboardStyleHelper.GetFont((IStiControlElement)base.Element);
		}
		((Control)this).Controls.Add(base.Child);
		base.TreeViewBoxControl.AfterCheck += new TreeViewEventHandler(TreeViewControl_AfterCheck);
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
			base.TreeViewBoxElement.UserFilters = new List<StiDataFilterRule>();
		}
		else if (GetIsAllItemUnchecked())
		{
			base.TreeViewBoxElement.UserFilters = new StiDataFilterRule(columnName, StiDataFilterCondition.IsFalse).ToList();
		}
		else
		{
			base.TreeViewBoxElement.UserFilters = (from StiTreeItem i in from i in GetCheckedNodes()
					select i.Tag
				where i.Meter == KeyMeter && i.Key != null
				select StiDataFilterCreator.CreateEqualBasedOnValue(i.Key, columnName, base.TreeViewBoxElement)).ToList();
		}
		RefreshAllChilds();
		RefreshViewerData();
	}

	protected override void ResetFilter()
	{
		base.TreeViewBoxElement.UserFilters = new List<StiDataFilterRule>();
	}

	private bool GetIsAllItemChecked()
	{
		foreach (StiTreeNode node in base.TreeViewBoxControl.Nodes)
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
		foreach (StiTreeNode node in base.TreeViewBoxControl.Nodes)
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
			nodes = base.TreeViewBoxControl.Nodes;
		}
		List<TreeNode> list = new List<TreeNode>();
		foreach (StiTreeNode node in nodes)
		{
			if (((TreeNode)node).Tag != null && node.State == StiCheckState.Checked)
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
		if (disposing && base.TreeViewBoxControl != null)
		{
			base.TreeViewBoxControl.AfterCheck -= new TreeViewEventHandler(TreeViewControl_AfterCheck);
		}
		base.Dispose(disposing);
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
				StiTreeNode selectAllNode = base.TreeViewBoxControl.Nodes[0] as StiTreeNode;
				if (e.Node == selectAllNode)
				{
					base.TreeViewBoxControl.GetAllNodes().ForEach(delegate(StiTreeNode n)
					{
						n.State = selectAllNode.State;
					});
					StartRefreshingFilter();
					return;
				}
				if (GetIsAllItemChecked())
				{
					selectAllNode.State = StiCheckState.Checked;
					return;
				}
				if (GetIsAllItemUnchecked())
				{
					selectAllNode.State = StiCheckState.Unchecked;
					return;
				}
				selectAllNode.State = StiCheckState.Indeterminate;
			}
		}
		finally
		{
			base.LockRefresh = false;
		}
		StartRefreshingFilter();
	}

	public StiMultiTreeViewBoxElementControl(IStiElement element)
		: base(element)
	{
	}
}
