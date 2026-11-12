using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Dashboard.Components.ListBox;
using Stimulsoft.Dashboard.Viewer.Controls.ListBox;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Styles;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Dashboard.Viewer.Elements.ListBox;

[ToolboxItem(false)]
public abstract class StiHorizontalListBoxElementControl : StiListBoxElementControl
{
	protected StiHorizontalListBox ListBoxControl => base.Child as StiHorizontalListBox;

	protected bool LockRefreshItems { get; set; }

	public override void RefreshControl(object sender, StiReportControlUpdateEventArgs e)
	{
		if (sender == base.Element && ListBoxControl != null)
		{
			ListBoxControl.Colors = GetColorCollection();
		}
	}

	public override void CreateChild()
	{
		StiHorizontalListBox obj = new StiHorizontalListBox
		{
			Colors = GetColorCollection()
		};
		((Control)obj).Dock = (DockStyle)5;
		obj.MultiSelection = base.ListBoxElement.SelectionMode == StiItemSelectionMode.Multi;
		obj.IsTransparent = IsTransparent;
		base.Child = (Control)(object)obj;
		if (base.Element is IStiControlElement)
		{
			((Control)ListBoxControl).Font = StiDashboardStyleHelper.GetFont((IStiControlElement)base.Element);
		}
		((Control)this).Controls.Add(base.Child);
		ListBoxControl.ItemSelectionChanged += ListViewControl_ItemSelectionChanged;
	}

	protected override void ResetFilter()
	{
		base.ListBoxElement.UserFilters = new List<StiDataFilterRule>();
	}

	protected abstract void ListViewControl_ItemSelectionChanged(object sender, StiListBoxItemSelectionChangedEventArgs e);

	protected object GetSelectAllItem()
	{
		if (!base.ShowAllValue)
		{
			return null;
		}
		return ListBoxControl.Items.Cast<StiListBoxItem>().FirstOrDefault()?.Value;
	}

	protected override void Dispose(bool disposing)
	{
		base.Dispose(disposing);
		if (ListBoxControl != null)
		{
			ListBoxControl.ItemSelectionChanged -= ListViewControl_ItemSelectionChanged;
			((Component)(object)ListBoxControl).Dispose();
		}
	}

	public StiHorizontalListBoxElementControl(IStiElement element)
		: base(element)
	{
	}
}
