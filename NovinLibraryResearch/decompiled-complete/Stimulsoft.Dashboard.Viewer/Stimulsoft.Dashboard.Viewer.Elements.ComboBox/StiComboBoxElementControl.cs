using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Stimulsoft.Dashboard.Components.ComboBox;
using Stimulsoft.Dashboard.Viewer.Elements.ListBox;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Viewer.Elements.ComboBox;

[ToolboxItem(false)]
public abstract class StiComboBoxElementControl : StiFilterElementControl
{
	protected StiComboBoxElement ComboBoxElement => base.Element as StiComboBoxElement;

	protected bool ShowAllValue => ComboBoxElement.ShowAllValue;

	public override void RefreshChild(StiDataTable dataTable)
	{
		base.RefreshChild(dataTable);
		base.IsShowNoResult = dataTable == null;
		base.IsShowEmptyData = dataTable?.IsEmpty ?? false;
		if (!base.IsShowNoResult && !base.IsShowEmptyData)
		{
			StiComboBoxItem[] items = StiComboBoxHelper.FetchItems(ComboBoxElement, dataTable, ComboBoxElement.ShowBlanks);
			RefreshChildControl(items);
		}
	}

	protected abstract void RefreshChildControl(StiComboBoxItem[] items);

	protected override void ResetFilter()
	{
		ComboBoxElement.UserFilters = new List<StiDataFilterRule>();
	}

	protected abstract bool IsAllItemsSelected();

	protected override void OnSizeChanged(EventArgs e)
	{
		((Control)this).OnSizeChanged(e);
		base.Child.Width = ((Control)this).Width;
		base.Child.Top = (((Control)this).Height - base.Child.Height) / 2;
	}

	public StiComboBoxElementControl(IStiElement element)
		: base(element)
	{
	}
}
