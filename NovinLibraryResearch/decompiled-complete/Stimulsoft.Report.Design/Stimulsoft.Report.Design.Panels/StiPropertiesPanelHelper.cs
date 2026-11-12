using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Stimulsoft.Base.Design;

namespace Stimulsoft.Report.Design.Panels;

public static class StiPropertiesPanelHelper
{
	public static StiPropertiesPanelService ActivatePanel(StiDesignerControl designer)
	{
		StiPropertiesPanelService stiPropertiesPanelService = designer?.GetPropertiesService();
		if (stiPropertiesPanelService?.DockingBar == null || stiPropertiesPanelService?.DockingItem == null)
		{
			return stiPropertiesPanelService;
		}
		stiPropertiesPanelService.DockingItem.Selected = true;
		return stiPropertiesPanelService;
	}

	public static void ActivatePanel(StiDesignerControl designer, object selectedObject)
	{
		StiPropertiesPanelService stiPropertiesPanelService = ActivatePanel(designer);
		if (selectedObject != null && stiPropertiesPanelService?.PropertyGrid != null)
		{
			((PropertyGrid)stiPropertiesPanelService.PropertyGrid).SelectedObject = selectedObject;
		}
	}

	public static void ActivatePanel(StiDesignerControl designer, object[] selectedObjects)
	{
		StiPropertiesPanelService stiPropertiesPanelService = ActivatePanel(designer);
		if (selectedObjects != null && stiPropertiesPanelService?.PropertyGrid != null)
		{
			((PropertyGrid)stiPropertiesPanelService.PropertyGrid).SelectedObjects = selectedObjects;
		}
	}

	public static StiPropertyGrid GetPropertyGrid(StiDesignerControl designer)
	{
		return designer?.GetPropertiesService()?.PropertyGrid;
	}

	public static void SetPropertyGrid(StiDesignerControl designer, object value)
	{
		StiPropertyGrid propertyGrid = GetPropertyGrid(designer);
		if (propertyGrid != null)
		{
			((PropertyGrid)propertyGrid).SelectedObject = value;
		}
	}

	public static Task SetPropertyGridAsync(StiDesignerControl designer, object value)
	{
		MethodInvoker val = default(MethodInvoker);
		return Task.Factory.StartNew(delegate
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Expected O, but got Unknown
			//IL_0024: Expected O, but got Unknown
			StiDesignerControl stiDesignerControl = designer;
			MethodInvoker obj = val;
			if (obj == null)
			{
				MethodInvoker val2 = delegate
				{
					StiPropertyGrid propertyGrid = GetPropertyGrid(designer);
					if (propertyGrid != null)
					{
						((PropertyGrid)propertyGrid).SelectedObject = value;
					}
				};
				MethodInvoker val3 = val2;
				val = val2;
				obj = val3;
			}
			((Control)stiDesignerControl).BeginInvoke((Delegate)(object)obj);
		});
	}

	public static bool IsRightDocked(StiDesignerControl designer)
	{
		StiPropertiesPanelService stiPropertiesPanelService = designer?.GetPropertiesService();
		if (stiPropertiesPanelService == null)
		{
			return false;
		}
		Rectangle rectangle = ((Control)designer).RectangleToScreen(((Control)designer).ClientRectangle);
		Rectangle rectangle2 = ((Control)stiPropertiesPanelService.PropertyGrid).RectangleToScreen(((Control)stiPropertiesPanelService.PropertyGrid).ClientRectangle);
		return rectangle.X + rectangle.Width / 2 < rectangle2.Left;
	}
}
