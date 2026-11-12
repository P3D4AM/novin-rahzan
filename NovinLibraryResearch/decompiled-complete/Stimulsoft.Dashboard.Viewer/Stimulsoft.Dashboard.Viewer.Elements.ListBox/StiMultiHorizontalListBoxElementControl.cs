using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Dashboard.Components.ListBox;
using Stimulsoft.Dashboard.Drawing.Helpers;
using Stimulsoft.Dashboard.Viewer.Controls.ListBox;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Viewer.Elements.ListBox;

[ToolboxItem(false)]
public class StiMultiHorizontalListBoxElementControl : StiHorizontalListBoxElementControl
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
		if (base.ShowAllValue)
		{
			base.ListBoxControl.Items.ForEach(delegate(StiListBoxItem i)
			{
				i.Checked = true;
			});
		}
		if (!base.ShowAllValue)
		{
			base.LockRefresh = false;
		}
		base.ListBoxControl.RefreshUI();
		if (base.ShowAllValue)
		{
			base.ListBoxControl.SelectAllItems();
		}
		else
		{
			base.ListBoxControl.ResetAllItemsSelectionAndSelectFirstOnly();
		}
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
			string columnName = StiListBoxHelper.GetKeyMeterExpression(base.ListBoxElement);
			if (columnName == null)
			{
				return;
			}
			if (GetIsAllItemChecked())
			{
				base.ListBoxElement.UserFilters = new List<StiDataFilterRule>();
			}
			else if (GetIsAllItemUnchecked())
			{
				base.ListBoxElement.UserFilters = new StiDataFilterRule(columnName, StiDataFilterCondition.IsFalse).ToList();
			}
			else
			{
				base.ListBoxElement.UserFilters = (from i in base.ListBoxControl.Items
					where i.Checked && i.Value != null
					select StiDataFilterCreator.CreateEqualBasedOnValue(i.Value, columnName, base.ListBoxElement)).ToList();
			}
			RefreshAllChilds();
			RefreshViewerData();
		}
		finally
		{
			base.LockRefresh = false;
		}
	}

	private bool GetIsAllItemChecked()
	{
		return base.ListBoxControl.Items.All((StiListBoxItem i) => i.Checked);
	}

	private bool GetIsAllItemUnchecked()
	{
		return base.ListBoxControl.Items.All((StiListBoxItem i) => !i.Checked);
	}

	protected override void ListViewControl_ItemSelectionChanged(object sender, StiListBoxItemSelectionChangedEventArgs e)
	{
		if (base.LockRefreshItems || e.ItemIndex == -1)
		{
			return;
		}
		base.LockRefreshItems = true;
		if (base.ShowAllValue)
		{
			List<StiListBoxItem> items = base.ListBoxControl.Items;
			if (e.ItemIndex == 0)
			{
				bool isAll = items[0].Checked;
				items.Skip(1).ToList().ForEach(delegate(StiListBoxItem i)
				{
					i.Checked = isAll;
				});
				((Control)this).Invalidate();
			}
			else if (e.ItemIndex != 0)
			{
				bool flag = items.Skip(1).All((StiListBoxItem i) => i.Checked);
				if (flag != items[0].Checked)
				{
					items[0].Checked = flag;
					((Control)base.ListBoxControl).Invalidate();
				}
			}
		}
		base.LockRefreshItems = false;
		if (base.ListBoxControl.Items.Count > 0)
		{
			StartRefreshingFilter();
		}
	}

	public StiMultiHorizontalListBoxElementControl(IStiElement element)
		: base(element)
	{
	}
}
