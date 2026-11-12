using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Dashboard.Components.ListBox;
using Stimulsoft.Dashboard.Drawing.Helpers;
using Stimulsoft.Dashboard.Viewer.Controls.ListBox;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Viewer.Elements.ListBox;

[ToolboxItem(false)]
public class StiOneHorizontalListBoxElementControl : StiHorizontalListBoxElementControl
{
	protected override void RefreshChildControl(StiListBoxItem[] items)
	{
		base.LockRefresh = true;
		base.ListBoxControl.Items.Clear();
		if (base.ShowAllValue)
		{
			base.ListBoxControl.Items.Add(new StiListBoxItem(StiLocHelper.LocAll, StiLocHelper.LocAll));
		}
		base.ListBoxControl.Items.AddRange(items);
		if (!base.ShowAllValue)
		{
			base.LockRefresh = false;
		}
		base.ListBoxControl.RefreshUI();
		base.ListBoxControl.ResetAllItemsSelectionAndSelectFirstOnly();
		base.IsFirstInitialized = true;
		base.LockRefresh = false;
		((Control)base.ListBoxControl).Invalidate();
	}

	protected override void RefreshFilter()
	{
		if (base.LockRefresh)
		{
			return;
		}
		try
		{
			base.LockRefresh = true;
			string keyMeterExpression = StiListBoxHelper.GetKeyMeterExpression(base.ListBoxElement);
			if (keyMeterExpression != null)
			{
				if (base.ListBoxControl.SelectedValue == GetSelectAllItem())
				{
					base.ListBoxElement.UserFilters = new List<StiDataFilterRule>();
				}
				else
				{
					object selectedValue = base.ListBoxControl.SelectedValue;
					base.ListBoxElement.UserFilters = StiDataFilterCreator.CreateEqualBasedOnValue(selectedValue, keyMeterExpression, base.Element as StiComponent).ToList();
				}
				RefreshAllChilds();
				RefreshViewerData();
			}
		}
		finally
		{
			base.LockRefresh = false;
		}
	}

	protected override void ListViewControl_ItemSelectionChanged(object sender, StiListBoxItemSelectionChangedEventArgs e)
	{
		if (!base.LockRefreshItems && e.ItemIndex != -1)
		{
			base.LockRefreshItems = true;
			base.ListBoxControl.Items.Where((StiListBoxItem i) => i != e.Item).ToList().ForEach(delegate(StiListBoxItem i)
			{
				i.Checked = false;
			});
			base.LockRefreshItems = false;
			if (base.ListBoxControl.Items.Count > 0)
			{
				StartRefreshingFilter();
			}
		}
	}

	public StiOneHorizontalListBoxElementControl(IStiElement element)
		: base(element)
	{
	}
}
