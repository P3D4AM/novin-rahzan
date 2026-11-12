using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Components.TreeView;
using Stimulsoft.Dashboard.Export.Settings;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Export.Tools;

public class StiTreeViewBoxElementExportTool : StiComboBoxElementExportTool
{
	public override void Render(IStiElement element, StiPanel destination, RectangleD rect, StiDashboardExportSettings settings)
	{
		base.Render(element, destination, rect, settings);
	}

	protected override int GetValuesCount(IStiElement element, StiDataTable dataTable)
	{
		IStiTreeViewBoxElement stiTreeViewBoxElement = element as IStiTreeViewBoxElement;
		return StiTreeViewHelper.FetchItems(dataTable, stiTreeViewBoxElement.ShowBlanks)?.Count ?? 0;
	}
}
