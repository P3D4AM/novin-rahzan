using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Stimulsoft.Dashboard.Components.TreeView;
using Stimulsoft.Dashboard.Drawing.Helpers;
using Stimulsoft.Dashboard.Viewer.Controls.TreeView;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Styles;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Dashboard.Viewer.Elements.TreeViewBox;

[ToolboxItem(false)]
public class StiOneTreeViewBoxElementControl : StiTreeViewBoxElementControl
{
	public override void RefreshControl(object sender, StiReportControlUpdateEventArgs e)
	{
		if (sender == base.Element && base.TreeViewBoxControl != null)
		{
			base.TreeViewBoxControl.Colors = GetColorCollection();
		}
	}

	public override void CreateChild()
	{
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Expected O, but got Unknown
		base.Child = (Control)(object)new StiTreeViewBox
		{
			Colors = GetColorCollection(),
			AllowCloseDropDownOnDoubleClick = true,
			AllowShowCheckedZones = false,
			CheckBoxes = false
		};
		if (base.Element is IStiControlElement)
		{
			base.Child.Font = StiDashboardStyleHelper.GetFont((IStiControlElement)base.Element);
		}
		((Control)this).Controls.Add(base.Child);
		base.TreeViewBoxControl.AfterSelect += new TreeViewEventHandler(TreeViewBoxControl_AfterSelect);
	}

	protected override void RefreshFilter()
	{
		StiTreeNode selectedNode = base.TreeViewBoxControl.SelectedNode;
		if (selectedNode == null)
		{
			return;
		}
		if (base.ShowAllValue && ((TreeNode)selectedNode).Text == StiLocHelper.LocAll)
		{
			base.TreeViewBoxElement.UserFilters = new List<StiDataFilterRule>();
		}
		else
		{
			if (!(((TreeNode)selectedNode).Tag is StiTreeItem { Meter: var meter } stiTreeItem))
			{
				return;
			}
			string expression = meter.Expression;
			base.TreeViewBoxElement.UserFilters = StiDataFilterCreator.CreateEqualBasedOnValue(stiTreeItem.Key, expression, base.TreeViewBoxElement).ToList();
		}
		RefreshAllChilds();
		RefreshViewerData();
	}

	protected override void ResetFilter()
	{
		base.TreeViewBoxElement.UserFilters = new List<StiDataFilterRule>();
	}

	protected override void Dispose(bool disposing)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		if (disposing && base.TreeViewBoxControl != null)
		{
			base.TreeViewBoxControl.AfterSelect -= new TreeViewEventHandler(TreeViewBoxControl_AfterSelect);
		}
		base.Dispose(disposing);
	}

	private void TreeViewBoxControl_AfterSelect(object sender, TreeViewEventArgs e)
	{
		StartRefreshingFilter();
	}

	public StiOneTreeViewBoxElementControl(IStiElement element)
		: base(element)
	{
	}
}
