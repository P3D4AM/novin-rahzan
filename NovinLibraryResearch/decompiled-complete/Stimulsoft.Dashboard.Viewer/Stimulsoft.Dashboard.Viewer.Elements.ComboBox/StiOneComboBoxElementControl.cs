using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Dashboard.Components.ComboBox;
using Stimulsoft.Dashboard.Drawing.Helpers;
using Stimulsoft.Dashboard.Viewer.Controls.ComboBox;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Styles;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Dashboard.Viewer.Elements.ComboBox;

[ToolboxItem(false)]
public class StiOneComboBoxElementControl : StiComboBoxElementControl
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
		List<StiComboBoxItem> list = items.ToList();
		if (base.ShowAllValue)
		{
			list.Insert(0, new StiComboBoxItem(StiLocHelper.LocAll));
		}
		ComboBoxControl?.SetItems(list.ToList());
		ComboBoxControl.ShowAllValue = false;
		ComboBoxControl.ShowSearchBox = list.Count() > 10;
		ComboBoxControl.ShowCheckBoxes = false;
		if (ComboBoxControl != null)
		{
			ComboBoxControl.SelectedItem = ((IEnumerable)ComboBoxControl.Items).Cast<StiComboBoxItem>().FirstOrDefault();
		}
		base.IsFirstInitialized = true;
		base.LockRefresh = false;
	}

	public override void CreateChild()
	{
		base.Child = (Control)(object)new StiCheckedComboBox();
		ComboBoxControl.Colors = GetColorCollection();
		ComboBoxControl.IsTransparent = IsTransparent;
		if (base.Element is IStiControlElement)
		{
			base.Child.Font = StiDashboardStyleHelper.GetFont((IStiControlElement)base.Element);
		}
		((Control)this).Controls.Add(base.Child);
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
			string keyMeterExpression = StiComboBoxHelper.GetKeyMeterExpression(base.ComboBoxElement);
			if (keyMeterExpression != null)
			{
				if (IsAllItemsSelected())
				{
					base.ComboBoxElement.UserFilters = new List<StiDataFilterRule>();
				}
				else
				{
					object value = (ComboBoxControl.SelectedItem as StiComboBoxItem).Value;
					base.ComboBoxElement.UserFilters = StiDataFilterCreator.CreateEqualBasedOnValue(value, keyMeterExpression, base.ComboBoxElement).ToList();
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

	protected override bool IsAllItemsSelected()
	{
		if (!base.ShowAllValue)
		{
			return false;
		}
		return ((IEnumerable)ComboBoxControl.Items).Cast<StiComboBoxItem>().FirstOrDefault() == ComboBoxControl.SelectedItem;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && ComboBoxControl != null)
		{
			ComboBoxControl.SelectedIndexChanged -= ComboBoxControl_SelectedIndexChanged;
		}
		base.Dispose(disposing);
	}

	private void ComboBoxControl_SelectedIndexChanged(object sender, EventArgs e)
	{
		StartRefreshingFilter();
	}

	public StiOneComboBoxElementControl(IStiElement element)
		: base(element)
	{
	}
}
