using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Dashboard.Components.ListBox;
using Stimulsoft.Dashboard.Viewer.Controls;
using Stimulsoft.Dashboard.Viewer.Controls.ListBox;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Styles;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Dashboard.Viewer.Elements.ListBox;

[ToolboxItem(false)]
public class StiMultiListBoxElementControl : StiListBoxElementControl
{
	private bool lockSearch;

	private StiCheckedListBox ListBoxControl => base.Child as StiCheckedListBox;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	protected internal List<StiListBoxItem> OriginItems { get; set; }

	private StiAllCheckBox AllCheckBox { get; set; }

	private StiHintTextBox SearchBox { get; set; }

	public override void RefreshControl(object sender, StiReportControlUpdateEventArgs e)
	{
		if (sender == base.Element)
		{
			StiControlColorCollection colorCollection = GetColorCollection();
			if (ListBoxControl != null)
			{
				ListBoxControl.Colors = colorCollection;
			}
			if (AllCheckBox != null)
			{
				AllCheckBox.Colors = colorCollection;
			}
			if (SearchBox != null)
			{
				((Control)SearchBox).BackColor = colorCollection.BackColor;
				((Control)SearchBox).ForeColor = colorCollection.ForeColor;
				SearchBox.BorderColor = colorCollection.SeparatorColor;
			}
		}
	}

	protected override void RefreshChildControl(StiListBoxItem[] items)
	{
		base.LockRefresh = true;
		OriginItems = items.ToList();
		((ListBox)ListBoxControl).BeginUpdate();
		((ObjectCollection)((CheckedListBox)ListBoxControl).Items).Clear();
		((ObjectCollection)((CheckedListBox)ListBoxControl).Items).AddRange((object[])items);
		SetItemStates(value: true);
		base.IsFirstInitialized = true;
		((Control)SearchBox).Visible = items != null && items.Length > 10;
		((ListBox)ListBoxControl).EndUpdate();
		base.LockRefresh = false;
	}

	public override void CreateChild()
	{
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Expected O, but got Unknown
		StiCheckedListBox obj = new StiCheckedListBox
		{
			Colors = GetColorCollection()
		};
		((Control)obj).Dock = (DockStyle)5;
		obj.IsTransparent = IsTransparent;
		base.Child = (Control)(object)obj;
		if (base.Element is IStiControlElement)
		{
			((Control)ListBoxControl).Font = StiDashboardStyleHelper.GetFont((IStiControlElement)base.Element);
		}
		((Control)this).Controls.Add(base.Child);
		CreateSelectAllChild();
		CreateSearchBox();
		((CheckedListBox)ListBoxControl).ItemCheck += new ItemCheckEventHandler(ListBoxControl_ItemCheck);
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
			string columnName = GetKeyMeterExpression();
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
				CheckedListBox listBoxControl = (CheckedListBox)(object)ListBoxControl;
				base.ListBoxElement.UserFilters = (from StiListBoxItem i in (IEnumerable)listBoxControl.CheckedItems
					select i.Value into i
					where i != null
					select StiDataFilterCreator.CreateEqualBasedOnValue(i, columnName, base.ListBoxElement)).ToList();
			}
			RefreshAllChilds();
			RefreshViewerData();
		}
		finally
		{
			base.LockRefresh = false;
		}
	}

	protected override void ResetFilter()
	{
		base.ListBoxElement.UserFilters = new List<StiDataFilterRule>();
	}

	private void CreateSelectAllChild()
	{
		if (base.ShowAllValue)
		{
			StiAllCheckBox stiAllCheckBox = new StiAllCheckBox();
			((CheckBox)stiAllCheckBox).Checked = true;
			((Control)stiAllCheckBox).Dock = (DockStyle)1;
			stiAllCheckBox.Colors = GetColorCollection();
			((Control)stiAllCheckBox).Font = StiDashboardStyleHelper.GetFont((IStiControlElement)base.Element);
			AllCheckBox = stiAllCheckBox;
			((CheckBox)AllCheckBox).CheckedChanged += AllCheckBox_CheckedChanged;
			((Control)this).Controls.Add((Control)(object)AllCheckBox);
		}
	}

	private void CreateSearchBox()
	{
		StiControlColorCollection colorCollection = GetColorCollection();
		StiHintTextBox obj = new StiHintTextBox
		{
			FixedHeight = false,
			Hint = Loc.GetCleaned("Editor", "TypeToSearch")
		};
		((Control)obj).Dock = (DockStyle)1;
		((Control)obj).Font = StiDashboardStyleHelper.GetFont((IStiControlElement)base.Element);
		((Control)obj).BackColor = colorCollection.BackColor;
		((Control)obj).ForeColor = colorCollection.ForeColor;
		obj.BorderColor = colorCollection.SeparatorColor;
		((Control)obj).Visible = false;
		((Control)obj).Height = StiScale.I(24);
		SearchBox = obj;
		((Control)SearchBox).TextChanged += SearchBox_TextChanged;
		((Control)this).Controls.Add((Control)(object)SearchBox);
	}

	private void SetItemStates(bool value)
	{
		for (int i = 0; i < ((ObjectCollection)((CheckedListBox)ListBoxControl).Items).Count; i++)
		{
			((CheckedListBox)ListBoxControl).SetItemChecked(i, value);
		}
		OriginItems.ForEach(delegate(StiListBoxItem c)
		{
			c.Checked = value;
		});
	}

	private bool GetIsAllItemChecked()
	{
		return OriginItems.All((StiListBoxItem i) => i.Checked);
	}

	private bool GetIsAllItemUnchecked()
	{
		return OriginItems.All((StiListBoxItem i) => !i.Checked);
	}

	private string GetKeyMeterExpression()
	{
		if (base.ListBoxElement.KeyMeter != null)
		{
			return base.ListBoxElement.KeyMeter.Expression;
		}
		if (base.ListBoxElement.NameMeter != null)
		{
			return base.ListBoxElement.NameMeter.Expression;
		}
		return null;
	}

	private void DoSearch()
	{
		string value = ((Control)SearchBox).Text?.ToLowerInvariant();
		IEnumerable<StiListBoxItem> enumerable;
		if (!string.IsNullOrEmpty(value))
		{
			enumerable = OriginItems.Where((StiListBoxItem i) => i.Label.ToLowerInvariant().Contains(value));
		}
		else
		{
			IEnumerable<StiListBoxItem> originItems = OriginItems;
			enumerable = originItems;
		}
		IEnumerable<StiListBoxItem> source = enumerable;
		((ListBox)ListBoxControl).BeginUpdate();
		((ObjectCollection)((CheckedListBox)ListBoxControl).Items).Clear();
		ObjectCollection items = ((CheckedListBox)ListBoxControl).Items;
		object[] array = source.ToArray();
		((ObjectCollection)items).AddRange(array);
		for (int j = 0; j < ((ObjectCollection)((CheckedListBox)ListBoxControl).Items).Count; j++)
		{
			if (((ObjectCollection)((CheckedListBox)ListBoxControl).Items)[j] is StiListBoxItem { Checked: not false })
			{
				((CheckedListBox)ListBoxControl).SetItemChecked(j, true);
			}
		}
		((ListBox)ListBoxControl).EndUpdate();
	}

	protected override void Dispose(bool disposing)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		if (disposing)
		{
			if (ListBoxControl != null)
			{
				((CheckedListBox)ListBoxControl).ItemCheck -= new ItemCheckEventHandler(ListBoxControl_ItemCheck);
				((Component)(object)ListBoxControl).Dispose();
			}
			if (AllCheckBox != null)
			{
				((CheckBox)AllCheckBox).CheckedChanged -= AllCheckBox_CheckedChanged;
				((Component)(object)AllCheckBox).Dispose();
			}
			if (SearchBox != null)
			{
				((Control)SearchBox).TextChanged -= SearchBox_TextChanged;
				((Component)(object)SearchBox).Dispose();
			}
		}
		base.Dispose(disposing);
	}

	private void SearchBox_TextChanged(object sender, EventArgs e)
	{
		if (!lockSearch)
		{
			DoSearch();
		}
	}

	private void AllCheckBox_CheckedChanged(object sender, EventArgs e)
	{
		if (!base.LockRefresh)
		{
			base.LockRefresh = true;
			SetItemStates(((CheckBox)AllCheckBox).Checked);
			StartRefreshingFilter();
			base.LockRefresh = false;
		}
	}

	private void ListBoxControl_ItemCheck(object sender, ItemCheckEventArgs e)
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Invalid comparison between Unknown and I4
		if (base.LockRefresh)
		{
			return;
		}
		StartRefreshingFilter();
		if (!base.ShowAllValue)
		{
			return;
		}
		try
		{
			base.LockRefresh = true;
			StiListBoxItem stiListBoxItem = ((ObjectCollection)((CheckedListBox)ListBoxControl).Items)[e.Index] as StiListBoxItem;
			stiListBoxItem.Checked = (int)e.NewValue == 1;
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
		finally
		{
			base.LockRefresh = false;
		}
	}

	public StiMultiListBoxElementControl(IStiElement element)
		: base(element)
	{
	}
}
