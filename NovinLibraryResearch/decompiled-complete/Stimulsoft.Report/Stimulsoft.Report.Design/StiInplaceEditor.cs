using Stimulsoft.Data.Engine;
using Stimulsoft.Report.App;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Report.Design;

public class StiInplaceEditor
{
	public static int ButtonSize = 30;

	public static int ButtonMargin = 4;

	public static bool IsEditorActivated { get; set; }

	public static bool IsEditorAllowed(object comp)
	{
		if (!IsFieldsEditorAllowed(comp))
		{
			return IsSortAndFilterEditorAllowed(comp);
		}
		return true;
	}

	public static bool IsFieldsEditorAllowed(object comp)
	{
		if (!(comp is StiPanel) && !(comp is IStiButtonElement))
		{
			return !(comp is IStiComponentUI);
		}
		return false;
	}

	public static bool IsSortAndFilterEditorAllowed(object comp)
	{
		if (!IsFilterEditorAllowed(comp) && !IsSortEditorAllowed(comp) && !IsDataTopNEditorAllowed(comp))
		{
			return IsDataTransformationEditorAllowed(comp);
		}
		return true;
	}

	public static bool IsFilterEditorAllowed(object comp)
	{
		return comp is IStiDataFilters;
	}

	public static bool IsSortEditorAllowed(object comp)
	{
		return comp is IStiUserSorts;
	}

	public static bool IsTopNEditorAllowed(object comp)
	{
		return comp is IStiDataTopN;
	}

	public static bool IsDataTransformationEditorAllowed(object comp)
	{
		return comp is IStiDataTransformationElement;
	}

	public static bool IsInteractionEditorAllowed(object comp)
	{
		return comp is IStiElementInteraction;
	}

	public static bool IsDataTopNEditorAllowed(object comp)
	{
		return comp is IStiDataTopN;
	}

	public static bool IsTypeChangingAllowed(object comp)
	{
		if (!(comp is IStiTableElement) && !(comp is IStiCardsElement) && !(comp is IStiChartElement) && !(comp is IStiGaugeElement) && !(comp is IStiPivotTableElement) && !(comp is IStiIndicatorElement) && !(comp is IStiProgressElement) && !(comp is IStiRegionMapElement) && !(comp is IStiOnlineMapElement) && !(comp is IStiComboBoxElement) && !(comp is IStiDatePickerElement) && !(comp is IStiListBoxElement) && !(comp is IStiTreeViewBoxElement))
		{
			return comp is IStiTreeViewElement;
		}
		return true;
	}

	public static bool IsLeftLocation(StiComponent component)
	{
		if (IsRightLocation(component))
		{
			return false;
		}
		return component.Left - (double)ButtonMargin - (double)ButtonSize > 0.0;
	}

	public static bool IsRightLocation(StiComponent component)
	{
		if (component.Page == null)
		{
			return true;
		}
		double num = component.Right + (double)ButtonMargin + (double)ButtonSize;
		return num < component.Page.Width;
	}

	public static void OnFieldsEditorClick(StiComponent component)
	{
		if (IsFieldsEditorAllowed(component))
		{
			StiComponentDesigner componentDesigner = StiComponentDesigner.GetComponentDesigner(component.Report.Designer, ((object)component).GetType());
			if (componentDesigner != null && StiRestrictionsHelper.IsAllowChange(component))
			{
				component.Report.Designer.UndoRedoSave(component.LocalizedName, onlyCurrentPage: true);
				componentDesigner.OnDoubleClick(component);
			}
		}
	}

	public static void OnFilterEditorClick(StiComponent component)
	{
		if (IsFilterEditorAllowed(component))
		{
			StiComponentDesigner dataFiltersDesigner = StiDataFiltersDesigner.GetDataFiltersDesigner(component.Report.Designer, ((object)component).GetType());
			if (dataFiltersDesigner != null && StiRestrictionsHelper.IsAllowChange(component))
			{
				component.Report.Designer.UndoRedoSave(component.LocalizedName, onlyCurrentPage: true);
				dataFiltersDesigner.OnDoubleClick(component);
			}
		}
	}

	public static void OnSortEditorClick(StiComponent component)
	{
		if (IsSortEditorAllowed(component))
		{
			StiComponentDesigner dataSortDesigner = StiDataSortDesigner.GetDataSortDesigner(component.Report.Designer, ((object)component).GetType());
			if (dataSortDesigner != null && StiRestrictionsHelper.IsAllowChange(component))
			{
				component.Report.Designer.UndoRedoSave(component.LocalizedName, onlyCurrentPage: true);
				dataSortDesigner.OnDoubleClick(component);
			}
		}
	}

	public static void OnDataTopNEditorClick(StiComponent component)
	{
		if (IsTopNEditorAllowed(component))
		{
			StiComponentDesigner designer = StiDataTopNDesigner.GetDesigner(component.Report.Designer, ((object)component).GetType());
			if (designer != null && StiRestrictionsHelper.IsAllowChange(component))
			{
				component.Report.Designer.UndoRedoSave(component.LocalizedName, onlyCurrentPage: true);
				designer.OnDoubleClick(component);
			}
		}
	}

	public static void OnDataTransformationEditorClick(StiComponent component)
	{
		if (IsDataTransformationEditorAllowed(component))
		{
			StiComponentDesigner transformationDesigner = StiDataTransformationDesigner.GetTransformationDesigner(component.Report.Designer, ((object)component).GetType());
			if (transformationDesigner != null && StiRestrictionsHelper.IsAllowChange(component))
			{
				component.Report.Designer.UndoRedoSave(component.LocalizedName, onlyCurrentPage: true);
				transformationDesigner.OnDoubleClick(component);
			}
		}
	}

	public static void OnInteractionEditorClick(StiComponent component)
	{
		if (IsInteractionEditorAllowed(component))
		{
			StiComponentDesigner designer = StiElementInteractionDesigner.GetDesigner(component.Report.Designer, ((object)component).GetType());
			if (designer != null && StiRestrictionsHelper.IsAllowChange(component))
			{
				component.Report.Designer.UndoRedoSave(component.LocalizedName, onlyCurrentPage: true);
				designer.OnDoubleClick(component);
			}
		}
	}
}
