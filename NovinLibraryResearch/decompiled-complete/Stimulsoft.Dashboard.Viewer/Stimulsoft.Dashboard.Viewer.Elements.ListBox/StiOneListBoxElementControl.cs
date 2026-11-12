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
using Stimulsoft.Dashboard.Drawing.Helpers;
using Stimulsoft.Dashboard.Viewer.Controls;
using Stimulsoft.Dashboard.Viewer.Controls.ListBox;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Styles;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Dashboard.Viewer.Elements.ListBox;

[ToolboxItem(false)]
public class StiOneListBoxElementControl : StiListBoxElementControl
{
	private bool lockSearch;

	private object lastSelectedItem;

	private Stimulsoft.Dashboard.Viewer.Controls.ListBox.StiListBox ListBoxControl => base.Child as Stimulsoft.Dashboard.Viewer.Controls.ListBox.StiListBox;

	private StiHintTextBox SearchBox { get; set; }

	private Panel SeparatorPanel { get; set; }

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	protected internal List<StiListBoxItem> OriginItems { get; set; }

	public override void RefreshControl(object sender, StiReportControlUpdateEventArgs e)
	{
		if (sender == base.Element)
		{
			StiControlColorCollection colorCollection = GetColorCollection();
			if (ListBoxControl != null)
			{
				ListBoxControl.Colors = colorCollection;
			}
			if (SeparatorPanel != null)
			{
				((Control)SeparatorPanel).BackColor = colorCollection.BackColor;
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
		List<StiListBoxItem> list = items.ToList();
		if (base.ShowAllValue)
		{
			list.Insert(0, new StiListBoxItem(StiLocHelper.LocAll));
		}
		OriginItems = list;
		((ListBox)ListBoxControl).BeginUpdate();
		((ListBox)ListBoxControl).Items.Clear();
		ObjectCollection items2 = ((ListBox)ListBoxControl).Items;
		object[] array = list.ToArray();
		items2.AddRange(array);
		if (!base.ShowAllValue)
		{
			base.LockRefresh = false;
		}
		((ListBox)ListBoxControl).SelectedItem = ((IEnumerable)((ListBox)ListBoxControl).Items).Cast<StiListBoxItem>().FirstOrDefault();
		StiHintTextBox searchBox = SearchBox;
		bool visible = (((Control)SeparatorPanel).Visible = items != null && items.Length > 10);
		((Control)searchBox).Visible = visible;
		((ListBox)ListBoxControl).EndUpdate();
		base.IsFirstInitialized = true;
		base.LockRefresh = false;
	}

	public override void CreateChild()
	{
		Stimulsoft.Dashboard.Viewer.Controls.ListBox.StiListBox obj = new Stimulsoft.Dashboard.Viewer.Controls.ListBox.StiListBox
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
		CreateSearchBox();
		((ListBox)ListBoxControl).SelectedIndexChanged += ListBoxControl_SelectedIndexChanged;
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
				if (((ListBox)ListBoxControl).SelectedItem == GetSelectAllItem())
				{
					base.ListBoxElement.UserFilters = new List<StiDataFilterRule>();
				}
				else
				{
					object value = (((ListBox)ListBoxControl).SelectedItem as StiListBoxItem).Value;
					base.ListBoxElement.UserFilters = StiDataFilterCreator.CreateEqualBasedOnValue(value, keyMeterExpression, base.Element as StiComponent).ToList();
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

	protected override void ResetFilter()
	{
		base.ListBoxElement.UserFilters = new List<StiDataFilterRule>();
	}

	private StiListBoxItem GetSelectAllItem()
	{
		if (!base.ShowAllValue)
		{
			return null;
		}
		return OriginItems.FirstOrDefault();
	}

	private void CreateSearchBox()
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		StiControlColorCollection colorCollection = GetColorCollection();
		SeparatorPanel = new Panel
		{
			Height = StiScale.I4,
			Dock = (DockStyle)1,
			Visible = false,
			BackColor = colorCollection.BackColor
		};
		((Control)this).Controls.Add((Control)(object)SeparatorPanel);
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

	private void DoSearch()
	{
		StiListBoxItem allItem = GetSelectAllItem();
		string value = ((Control)SearchBox).Text?.ToLowerInvariant();
		IEnumerable<StiListBoxItem> enumerable;
		if (!string.IsNullOrEmpty(value))
		{
			enumerable = OriginItems.Where((StiListBoxItem i) => i == allItem || i.Label.ToLowerInvariant().Contains(value));
		}
		else
		{
			IEnumerable<StiListBoxItem> originItems = OriginItems;
			enumerable = originItems;
		}
		IEnumerable<StiListBoxItem> source = enumerable;
		((ListBox)ListBoxControl).BeginUpdate();
		object selectedItem = ((ListBox)ListBoxControl).SelectedItem;
		((ListBox)ListBoxControl).Items.Clear();
		ObjectCollection items = ((ListBox)ListBoxControl).Items;
		object[] array = source.ToArray();
		items.AddRange(array);
		((ListBox)ListBoxControl).SelectedItem = selectedItem;
		if (((ListBox)ListBoxControl).SelectedItem == null)
		{
			((ListBox)ListBoxControl).SelectedItem = lastSelectedItem;
		}
		((ListBox)ListBoxControl).EndUpdate();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			if (ListBoxControl != null)
			{
				((ListBox)ListBoxControl).SelectedIndexChanged -= ListBoxControl_SelectedIndexChanged;
				((Component)(object)ListBoxControl).Dispose();
			}
			if (SeparatorPanel != null)
			{
				((Component)(object)SeparatorPanel).Dispose();
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

	private void ListBoxControl_SelectedIndexChanged(object sender, EventArgs e)
	{
		lastSelectedItem = ((ListBox)ListBoxControl).SelectedItem;
		StartRefreshingFilter();
	}

	public StiOneListBoxElementControl(IStiElement element)
		: base(element)
	{
	}
}
