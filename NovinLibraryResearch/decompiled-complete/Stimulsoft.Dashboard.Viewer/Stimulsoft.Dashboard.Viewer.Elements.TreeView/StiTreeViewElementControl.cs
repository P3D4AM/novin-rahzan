using System.ComponentModel;
using Stimulsoft.Dashboard.Components.TreeView;
using Stimulsoft.Dashboard.Viewer.Controls.TreeView;
using Stimulsoft.Dashboard.Viewer.Elements.ListBox;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Viewer.Elements.TreeView;

[ToolboxItem(false)]
public abstract class StiTreeViewElementControl : StiFilterElementControl
{
	protected StiTreeView TreeViewControl => base.Child as StiTreeView;

	protected StiTreeViewElement TreeViewElement => base.Element as StiTreeViewElement;

	protected bool ShowAllValue => TreeViewElement.ShowAllValue;

	public StiTreeViewElementControl(IStiElement element)
		: base(element)
	{
	}
}
