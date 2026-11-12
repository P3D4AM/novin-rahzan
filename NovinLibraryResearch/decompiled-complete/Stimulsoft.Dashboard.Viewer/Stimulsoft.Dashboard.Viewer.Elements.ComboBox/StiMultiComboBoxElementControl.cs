using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Dashboard.Components.ComboBox;
using Stimulsoft.Dashboard.Viewer.Controls.ComboBox;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Styles;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Dashboard.Viewer.Elements.ComboBox;

[ToolboxItem(false)]
public class StiMultiComboBoxElementControl : StiComboBoxElementControl
{
	private StiCheckedComboBox ComboBoxControl => base.Child as StiCheckedComboBox;

	public override void RefreshControl(object sender, StiReportControlUpdateEventArgs e)
	{
		if (sender == base.Element && ComboBoxControl != null)
		{
			ComboBoxControl.Colors = GetColorCollection();
		}
	}

	protected override void RefreshChildControl(StiComboBoxItem[] items)
	{
		base.LockRefresh = true;
		ComboBoxControl?.SetItems(items.ToList());
		ComboBoxControl.AllCheckBoxState = (CheckState)1;
		ComboBoxControl.ShowAllValue = base.ShowAllValue;
		ComboBoxControl.ShowSearchBox = items.Count() > 10;
		ComboBoxControl.ShowCheckBoxes = true;
		ComboBoxControl.SetAllItemsCheckState((CheckState)1);
		base.IsFirstInitialized = true;
		base.LockRefresh = false;
	}

	public override void CreateChild()
	{
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Expected O, but got Unknown
		base.Child = (Control)(object)new StiCheckedComboBox();
		ComboBoxControl.Colors = GetColorCollection();
		ComboBoxControl.IsTransparent = IsTransparent;
		if (base.Element is IStiControlElement)
		{
			base.Child.Font = StiDashboardStyleHelper.GetFont((IStiControlElement)base.Element);
		}
		((Control)this).Controls.Add(base.Child);
		ComboBoxControl.ItemCheck += new ItemCheckEventHandler(CheckedComboBox_ItemCheck);
		ComboBoxControl.AllItemCheck += CheckedComboBox_AllItemCheck;
		ComboBoxControl.SelectedIndexChanged += ComboBoxControl_SelectedIndexChanged;
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
			string columnName = StiComboBoxHelper.GetKeyMeterExpression(base.ComboBoxElement);
			if (columnName == null)
			{
				return;
			}
			if (GetIsAllItemChecked())
			{
				base.ComboBoxElement.UserFilters = new List<StiDataFilterRule>();
			}
			else if (GetIsAllItemUnchecked())
			{
				base.ComboBoxElement.UserFilters = new StiDataFilterRule(columnName, StiDataFilterCondition.IsFalse).ToList();
			}
			else
			{
				StiCheckedComboBox stiCheckedComboBox = base.Child as StiCheckedComboBox;
				base.ComboBoxElement.UserFilters = (from i in stiCheckedComboBox.GetCheckedItems().Select((StiComboBoxItem item, int index) => new
					{
						Item = item,
						Index = index
					})
					where i.Item.Value != null
					select StiDataFilterCreator.CreateEqualBasedOnValue(i.Item.Value, columnName, base.Element as StiComponent)).ToList();
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
		return ComboBoxControl.OriginItems.All((StiComboBoxItem i) => i.Checked);
	}

	private bool GetIsAllItemUnchecked()
	{
		return ComboBoxControl.OriginItems.All((StiComboBoxItem i) => !i.Checked);
	}

	protected override bool IsAllItemsSelected()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Invalid comparison between Unknown and I4
		if (!base.ShowAllValue)
		{
			return false;
		}
		return (int)ComboBoxControl.AllCheckBoxState == 1;
	}

	protected override void Dispose(bool disposing)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		if (disposing && ComboBoxControl != null)
		{
			ComboBoxControl.ItemCheck -= new ItemCheckEventHandler(CheckedComboBox_ItemCheck);
			ComboBoxControl.AllItemCheck -= CheckedComboBox_AllItemCheck;
			ComboBoxControl.SelectedIndexChanged -= ComboBoxControl_SelectedIndexChanged;
		}
		base.Dispose(disposing);
	}

	private void CheckedComboBox_ItemCheck(object sender, ItemCheckEventArgs e)
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Invalid comparison between Unknown and I4
		if (base.LockRefresh || !base.ShowAllValue)
		{
			return;
		}
		try
		{
			base.LockRefresh = true;
			StiComboBoxItem stiComboBoxItem = ((ObjectCollection)ComboBoxControl.Items)[e.Index] as StiComboBoxItem;
			stiComboBoxItem.Checked = (int)e.NewValue == 1;
			if (GetIsAllItemChecked())
			{
				ComboBoxControl.SetAllItemsState((CheckState)1);
				return;
			}
			if (GetIsAllItemUnchecked())
			{
				ComboBoxControl.SetAllItemsState((CheckState)0);
				return;
			}
			ComboBoxControl.SetAllItemsState((CheckState)2);
			StartRefreshingFilter();
		}
		finally
		{
			base.LockRefresh = false;
		}
	}

	private void CheckedComboBox_AllItemCheck(object sender, EventArgs e)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Invalid comparison between Unknown and I4
		if (base.LockRefresh || !base.ShowAllValue)
		{
			return;
		}
		try
		{
			base.LockRefresh = true;
			bool value = (int)ComboBoxControl.AllCheckBoxState == 1;
			for (int i = 0; i < ((ObjectCollection)ComboBoxControl.Items).Count; i++)
			{
				ComboBoxControl.SetItemChecked(i, value);
			}
			ComboBoxControl.OriginItems.ForEach(delegate(StiComboBoxItem c)
			{
				c.Checked = value;
			});
			StartRefreshingFilter();
		}
		finally
		{
			base.LockRefresh = false;
		}
	}

	private void ComboBoxControl_SelectedIndexChanged(object sender, EventArgs e)
	{
		StartRefreshingFilter();
	}

	public StiMultiComboBoxElementControl(IStiElement element)
		: base(element)
	{
		if (base.Element is IStiControlElement)
		{
			((Control)this).Font = StiDashboardStyleHelper.GetFont((IStiControlElement)base.Element);
		}
	}
}
