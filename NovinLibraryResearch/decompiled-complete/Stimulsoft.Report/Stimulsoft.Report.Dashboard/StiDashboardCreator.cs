using System.Linq;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Dashboard;

public static class StiDashboardCreator
{
	public static IStiDashboard CreateDashboard(StiReport report)
	{
		StiComponent? stiComponent = StiOptions.Services.Components.FirstOrDefault((StiComponent c) => c is IStiDashboard);
		StiComponent stiComponent2 = ((stiComponent != null) ? ((StiService)stiComponent).Clone() : null) as StiComponent;
		if (stiComponent2 != null)
		{
			stiComponent2.Report = report;
			if (stiComponent2.Guid != null)
			{
				stiComponent2.NewGuid();
			}
		}
		return stiComponent2 as IStiDashboard;
	}

	public static IStiCardsElement CreateCardsElement(StiReport report)
	{
		StiComponent? stiComponent = StiOptions.Services.Components.FirstOrDefault((StiComponent c) => c is IStiCardsElement);
		StiComponent stiComponent2 = ((stiComponent != null) ? ((StiService)stiComponent).Clone() : null) as StiComponent;
		if (stiComponent2 != null)
		{
			stiComponent2.Page = report.GetCurrentPage();
			if (stiComponent2.Guid != null)
			{
				stiComponent2.NewGuid();
			}
		}
		return stiComponent2 as IStiCardsElement;
	}

	public static IStiButtonElement CreateButtonElement(StiReport report)
	{
		StiComponent? stiComponent = StiOptions.Services.Components.FirstOrDefault((StiComponent c) => c is IStiButtonElement);
		StiComponent stiComponent2 = ((stiComponent != null) ? ((StiService)stiComponent).Clone() : null) as StiComponent;
		if (stiComponent2 != null)
		{
			stiComponent2.Page = report.GetCurrentPage();
			if (stiComponent2.Guid != null)
			{
				stiComponent2.NewGuid();
			}
		}
		return stiComponent2 as IStiButtonElement;
	}

	public static IStiChartElement CreateChartElement(StiReport report)
	{
		StiComponent? stiComponent = StiOptions.Services.Components.FirstOrDefault((StiComponent c) => c is IStiChartElement);
		StiComponent stiComponent2 = ((stiComponent != null) ? ((StiService)stiComponent).Clone() : null) as StiComponent;
		if (stiComponent2 != null)
		{
			stiComponent2.Page = report.GetCurrentPage();
			if (stiComponent2.Guid != null)
			{
				stiComponent2.NewGuid();
			}
		}
		return stiComponent2 as IStiChartElement;
	}

	public static IStiShapeElement CreateShapeElement(StiReport report)
	{
		StiComponent? stiComponent = StiOptions.Services.Components.FirstOrDefault((StiComponent c) => c is IStiShapeElement);
		StiComponent stiComponent2 = ((stiComponent != null) ? ((StiService)stiComponent).Clone() : null) as StiComponent;
		if (stiComponent2 != null)
		{
			stiComponent2.Page = report.GetCurrentPage();
			if (stiComponent2.Guid != null)
			{
				stiComponent2.NewGuid();
			}
		}
		return stiComponent2 as IStiShapeElement;
	}

	public static IStiGaugeElement CreateGaugeElement(StiReport report)
	{
		StiComponent? stiComponent = StiOptions.Services.Components.FirstOrDefault((StiComponent c) => c is IStiGaugeElement);
		StiComponent stiComponent2 = ((stiComponent != null) ? ((StiService)stiComponent).Clone() : null) as StiComponent;
		if (stiComponent2 != null)
		{
			stiComponent2.Page = report.GetCurrentPage();
			if (stiComponent2.Guid != null)
			{
				stiComponent2.NewGuid();
			}
		}
		return stiComponent2 as IStiGaugeElement;
	}

	public static IStiPivotTableElement CreatePivotTableElement(StiReport report)
	{
		StiComponent? stiComponent = StiOptions.Services.Components.FirstOrDefault((StiComponent c) => c is IStiPivotTableElement);
		StiComponent stiComponent2 = ((stiComponent != null) ? ((StiService)stiComponent).Clone() : null) as StiComponent;
		if (stiComponent2 != null)
		{
			stiComponent2.Page = report.GetCurrentPage();
			if (stiComponent2.Guid != null)
			{
				stiComponent2.NewGuid();
			}
		}
		return stiComponent2 as IStiPivotTableElement;
	}

	public static IStiIndicatorElement CreateIndicatorElement(StiReport report)
	{
		StiComponent? stiComponent = StiOptions.Services.Components.FirstOrDefault((StiComponent c) => c is IStiIndicatorElement);
		StiComponent stiComponent2 = ((stiComponent != null) ? ((StiService)stiComponent).Clone() : null) as StiComponent;
		if (stiComponent2 != null)
		{
			stiComponent2.Page = report.GetCurrentPage();
			if (stiComponent2.Guid != null)
			{
				stiComponent2.NewGuid();
			}
		}
		return stiComponent2 as IStiIndicatorElement;
	}

	public static IStiProgressElement CreateProgressElement(StiReport report)
	{
		StiComponent? stiComponent = StiOptions.Services.Components.FirstOrDefault((StiComponent c) => c is IStiProgressElement);
		StiComponent stiComponent2 = ((stiComponent != null) ? ((StiService)stiComponent).Clone() : null) as StiComponent;
		if (stiComponent2 != null)
		{
			stiComponent2.Page = report.GetCurrentPage();
			if (stiComponent2.Guid != null)
			{
				stiComponent2.NewGuid();
			}
		}
		return stiComponent2 as IStiProgressElement;
	}

	public static IStiRegionMapElement CreateRegionMapElement(StiReport report)
	{
		StiComponent? stiComponent = StiOptions.Services.Components.FirstOrDefault((StiComponent c) => c is IStiRegionMapElement);
		StiComponent stiComponent2 = ((stiComponent != null) ? ((StiService)stiComponent).Clone() : null) as StiComponent;
		if (stiComponent2 != null)
		{
			stiComponent2.Page = report.GetCurrentPage();
			if (stiComponent2.Guid != null)
			{
				stiComponent2.NewGuid();
			}
		}
		return stiComponent2 as IStiRegionMapElement;
	}

	public static IStiOnlineMapElement CreateOnlineMapElement(StiReport report)
	{
		StiComponent? stiComponent = StiOptions.Services.Components.FirstOrDefault((StiComponent c) => c is IStiOnlineMapElement);
		StiComponent stiComponent2 = ((stiComponent != null) ? ((StiService)stiComponent).Clone() : null) as StiComponent;
		if (stiComponent2 != null)
		{
			stiComponent2.Page = report.GetCurrentPage();
			if (stiComponent2.Guid != null)
			{
				stiComponent2.NewGuid();
			}
		}
		return stiComponent2 as IStiOnlineMapElement;
	}

	public static IStiComboBoxElement CreateComboBoxElement(StiReport report)
	{
		StiComponent? stiComponent = StiOptions.Services.Components.FirstOrDefault((StiComponent c) => c is IStiComboBoxElement);
		StiComponent stiComponent2 = ((stiComponent != null) ? ((StiService)stiComponent).Clone() : null) as StiComponent;
		if (stiComponent2 != null)
		{
			stiComponent2.Page = report.GetCurrentPage();
			if (stiComponent2.Guid != null)
			{
				stiComponent2.NewGuid();
			}
		}
		return stiComponent2 as IStiComboBoxElement;
	}

	public static IStiDatePickerElement CreateDatePickerElement(StiReport report)
	{
		StiComponent? stiComponent = StiOptions.Services.Components.FirstOrDefault((StiComponent c) => c is IStiDatePickerElement);
		StiComponent stiComponent2 = ((stiComponent != null) ? ((StiService)stiComponent).Clone() : null) as StiComponent;
		if (stiComponent2 != null)
		{
			stiComponent2.Page = report.GetCurrentPage();
			if (stiComponent2.Guid != null)
			{
				stiComponent2.NewGuid();
			}
		}
		return stiComponent2 as IStiDatePickerElement;
	}

	public static IStiListBoxElement CreateListBoxElement(StiReport report)
	{
		StiComponent? stiComponent = StiOptions.Services.Components.FirstOrDefault((StiComponent c) => c is IStiListBoxElement);
		StiComponent stiComponent2 = ((stiComponent != null) ? ((StiService)stiComponent).Clone() : null) as StiComponent;
		if (stiComponent2 != null)
		{
			stiComponent2.Page = report.GetCurrentPage();
			if (stiComponent2.Guid != null)
			{
				stiComponent2.NewGuid();
			}
		}
		return stiComponent2 as IStiListBoxElement;
	}

	public static IStiTreeViewBoxElement CreateTreeViewBoxElement(StiReport report)
	{
		StiComponent? stiComponent = StiOptions.Services.Components.FirstOrDefault((StiComponent c) => c is IStiTreeViewBoxElement);
		StiComponent stiComponent2 = ((stiComponent != null) ? ((StiService)stiComponent).Clone() : null) as StiComponent;
		if (stiComponent2 != null)
		{
			stiComponent2.Page = report.GetCurrentPage();
			if (stiComponent2.Guid != null)
			{
				stiComponent2.NewGuid();
			}
		}
		return stiComponent2 as IStiTreeViewBoxElement;
	}

	public static IStiTreeViewElement CreateTreeViewElement(StiReport report)
	{
		StiComponent? stiComponent = StiOptions.Services.Components.FirstOrDefault((StiComponent c) => c is IStiTreeViewElement);
		StiComponent stiComponent2 = ((stiComponent != null) ? ((StiService)stiComponent).Clone() : null) as StiComponent;
		if (stiComponent2 != null)
		{
			stiComponent2.Page = report.GetCurrentPage();
			if (stiComponent2.Guid != null)
			{
				stiComponent2.NewGuid();
			}
		}
		return stiComponent2 as IStiTreeViewElement;
	}

	public static IStiImageElement CreateImageElement(StiReport report)
	{
		StiComponent? stiComponent = StiOptions.Services.Components.FirstOrDefault((StiComponent c) => c is IStiImageElement);
		StiComponent stiComponent2 = ((stiComponent != null) ? ((StiService)stiComponent).Clone() : null) as StiComponent;
		if (stiComponent2 != null)
		{
			stiComponent2.Page = report.GetCurrentPage();
			if (stiComponent2.Guid != null)
			{
				stiComponent2.NewGuid();
			}
		}
		return stiComponent2 as IStiImageElement;
	}

	public static IStiRegionMapElement CreateMapElement(StiReport report)
	{
		StiComponent? stiComponent = StiOptions.Services.Components.FirstOrDefault((StiComponent c) => c is IStiRegionMapElement);
		StiComponent stiComponent2 = ((stiComponent != null) ? ((StiService)stiComponent).Clone() : null) as StiComponent;
		if (stiComponent2 != null)
		{
			stiComponent2.Page = report.GetCurrentPage();
			if (stiComponent2.Guid != null)
			{
				stiComponent2.NewGuid();
			}
		}
		return stiComponent2 as IStiRegionMapElement;
	}

	public static IStiPivotTableElement CreatePivotElement(StiReport report)
	{
		StiComponent? stiComponent = StiOptions.Services.Components.FirstOrDefault((StiComponent c) => c is IStiPivotTableElement);
		StiComponent stiComponent2 = ((stiComponent != null) ? ((StiService)stiComponent).Clone() : null) as StiComponent;
		if (stiComponent2 != null)
		{
			stiComponent2.Page = report.GetCurrentPage();
			if (stiComponent2.Guid != null)
			{
				stiComponent2.NewGuid();
			}
		}
		return stiComponent2 as IStiPivotTableElement;
	}

	public static IStiTableElement CreateTableElement(StiReport report)
	{
		StiComponent? stiComponent = StiOptions.Services.Components.FirstOrDefault((StiComponent c) => c is IStiTableElement);
		StiComponent stiComponent2 = ((stiComponent != null) ? ((StiService)stiComponent).Clone() : null) as StiComponent;
		if (stiComponent2 != null)
		{
			stiComponent2.Page = report.GetCurrentPage();
			if (stiComponent2.Guid != null)
			{
				stiComponent2.NewGuid();
			}
		}
		return stiComponent2 as IStiTableElement;
	}

	public static IStiTextElement CreateTextElement(StiReport report)
	{
		StiComponent? stiComponent = StiOptions.Services.Components.FirstOrDefault((StiComponent c) => c is IStiTextElement);
		StiComponent stiComponent2 = ((stiComponent != null) ? ((StiService)stiComponent).Clone() : null) as StiComponent;
		if (stiComponent2 != null)
		{
			stiComponent2.Page = report.GetCurrentPage();
			if (stiComponent2.Guid != null)
			{
				stiComponent2.NewGuid();
			}
		}
		return stiComponent2 as IStiTextElement;
	}
}
