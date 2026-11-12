using System.ComponentModel;
using Stimulsoft.Dashboard.Components.ListBox;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Viewer.Elements.ListBox;

[ToolboxItem(false)]
public abstract class StiListBoxElementControl : StiFilterElementControl
{
	protected StiListBoxElement ListBoxElement => base.Element as StiListBoxElement;

	protected bool ShowAllValue => ListBoxElement.ShowAllValue;

	public override void RefreshChild(StiDataTable dataTable)
	{
		base.RefreshChild(dataTable);
		base.IsShowNoResult = dataTable == null;
		base.IsShowEmptyData = dataTable?.IsEmpty ?? false;
		if (!base.IsShowNoResult && !base.IsShowEmptyData)
		{
			StiListBoxItem[] items = StiListBoxHelper.FetchItems(ListBoxElement, dataTable, ListBoxElement.ShowBlanks);
			RefreshChildControl(items);
		}
	}

	protected abstract void RefreshChildControl(StiListBoxItem[] items);

	public StiListBoxElementControl(IStiElement element)
		: base(element)
	{
	}
}
