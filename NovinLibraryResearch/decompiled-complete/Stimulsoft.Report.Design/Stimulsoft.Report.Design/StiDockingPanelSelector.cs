using System.Windows.Forms;
using Stimulsoft.Report.Design.Panels;

namespace Stimulsoft.Report.Design;

public class StiDockingPanelSelector
{
	internal static void PropertiesPanelSelect(StiDesignerControl designer)
	{
		StiPropertiesPanelService service = StiPropertiesPanelService.GetService(designer);
		PanelSelect(service, onlyActivate: true);
	}

	internal static void PanelSelect(StiPanelService service, bool onlyActivate, bool panelFocus = true)
	{
		if (service == null)
		{
			return;
		}
		if (service.DockingItem.Visible)
		{
			if (service is StiPropertiesPanelService && onlyActivate)
			{
				if (service.DockingBar.Items.Contains(service.DockingItem))
				{
					service.DockingBar.SelectedDockContainerItem = service.DockingItem;
				}
				if (panelFocus)
				{
					StiPropertiesPanelService stiPropertiesPanelService = service as StiPropertiesPanelService;
					((Control)stiPropertiesPanelService.PropertyGrid).Focus();
					stiPropertiesPanelService.DockingItem.Selected = true;
				}
			}
			else
			{
				StiDesignerControl.DockPanelClose(service);
			}
		}
		else
		{
			StiDesignerControl.DockPanelOpen(service);
		}
		service.Designer?.SaveDesignerParam();
		StiDesignerControl designer = service.Designer;
		if (designer != null)
		{
			((Control)designer).Refresh();
		}
	}
}
