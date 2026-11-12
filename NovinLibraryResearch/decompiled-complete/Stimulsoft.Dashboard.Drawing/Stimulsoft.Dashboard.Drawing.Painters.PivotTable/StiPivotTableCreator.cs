using Stimulsoft.Dashboard.Components.PivotTable;
using Stimulsoft.Dashboard.Helpers;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.CrossTab;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Drawing.Painters.PivotTable;

public class StiPivotTableCreator : IStiPivotTableCreator
{
	public void Create(IStiPivotTableElement element, StiDataTable dataTable)
	{
		StiPivotTableElement stiPivotTableElement = element as StiPivotTableElement;
		StiCrossTab stiCrossTab = StiPivotTableHelper.ConvertToCrossTab(stiPivotTableElement, dataTable);
		StiPivotTableToCrossTabCache.Put(element, stiCrossTab);
		StiPivotTableContainer stiPivotTableContainer = StiPivotToContainerCache.Get(element) as StiPivotTableContainer;
		if (stiPivotTableContainer == null)
		{
			stiPivotTableContainer = new StiPivotTableContainer(null, stiPivotTableElement);
			StiPivotToContainerCache.Put(element, stiPivotTableContainer);
		}
		stiPivotTableContainer.MakeGrid(stiCrossTab.CrossTabInfo.Cross, stiPivotTableElement);
	}
}
