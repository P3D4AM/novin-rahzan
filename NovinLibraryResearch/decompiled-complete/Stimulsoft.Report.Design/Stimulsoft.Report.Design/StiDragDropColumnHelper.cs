using System.Collections.Generic;
using Stimulsoft.Base;
using Stimulsoft.Base.Meters;
using Stimulsoft.Data.Engine;
using Stimulsoft.Data.Helpers;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Report.Design;

public static class StiDragDropColumnHelper
{
	public static bool IsDataColumnOrDataBandOrTableElementDragged(StiDesignerControl designer)
	{
		if (!IsDataColumnDragged(designer) && !IsDataBandDragged(designer))
		{
			return IsTableElementDragged(designer);
		}
		return true;
	}

	public static bool IsDataColumnOrTableElementDragged(StiDesignerControl designer)
	{
		if (!IsDataColumnDragged(designer))
		{
			return IsTableElementDragged(designer);
		}
		return true;
	}

	public static bool IsDataColumnOrVariableOrTableElementDragged(StiDesignerControl designer)
	{
		if (!IsDataColumnDragged(designer) && !IsTableElementDragged(designer))
		{
			return IsVariableDragged(designer);
		}
		return true;
	}

	public static bool IsDataColumnOrDataBandDragged(StiDesignerControl designer)
	{
		if (!IsDataColumnDragged(designer))
		{
			return IsDataBandDragged(designer);
		}
		return true;
	}

	public static bool IsDataColumnDragged(StiDesignerControl designer)
	{
		if (!IsCommonTextDragged(designer) && !IsImageColumnDragged(designer))
		{
			return IsBoolColumnDragged(designer);
		}
		return true;
	}

	public static bool IsDataColumnOrVariableDragged(StiDesignerControl designer)
	{
		if (!IsDataColumnDragged(designer))
		{
			return IsVariableDragged(designer);
		}
		return true;
	}

	private static bool IsCommonTextDragged(StiDesignerControl designer)
	{
		return GetDraggedTextColumn(designer) != null;
	}

	private static bool IsImageColumnDragged(StiDesignerControl designer)
	{
		return GetDraggedImageDataColumn(designer) != null;
	}

	private static bool IsBoolColumnDragged(StiDesignerControl designer)
	{
		return GetDraggedBoolDataColumn(designer) != null;
	}

	public static bool IsDataBandDragged(StiDesignerControl designer)
	{
		return GetDraggedDataBand(designer) != null;
	}

	public static bool IsTableElementDragged(StiDesignerControl designer)
	{
		return GetDraggedTableElement(designer) != null;
	}

	public static bool IsCardsElementDragged(StiDesignerControl designer)
	{
		return GetDraggedCardsElement(designer) != null;
	}

	public static bool IsImageElementDragged(StiDesignerControl designer)
	{
		return GetDraggedImageElement(designer) != null;
	}

	public static bool IsVariableDragged(StiDesignerControl designer)
	{
		return GetDraggedVariable(designer) != null;
	}

	public static StiDataSource GetDraggedDataSource(StiDesignerControl designer)
	{
		StiDataColumn draggedDataColumn = GetDraggedDataColumn(designer);
		if (draggedDataColumn != null)
		{
			return draggedDataColumn.DataSource;
		}
		StiDataBand draggedDataBand = GetDraggedDataBand(designer);
		if (draggedDataBand != null)
		{
			if (draggedDataBand.DataSource != null)
			{
				return draggedDataBand.DataSource;
			}
			if (!string.IsNullOrEmpty(draggedDataBand.DataSourceName))
			{
				return designer.Report.DataSources[draggedDataBand.DataSourceName];
			}
		}
		IStiTableElement draggedTableElement = GetDraggedTableElement(designer);
		if (draggedTableElement != null)
		{
			List<IStiMeter> meters = draggedTableElement.GetMeters();
			if (meters.Count > 0)
			{
				string text = Stimulsoft.Data.Helpers.StiExpressionHelper.RemoveFunction(meters[0].Expression);
				if (text.StartsWith("["))
				{
					text = text.Substring(1);
				}
				if (text.EndsWith("]"))
				{
					text = text.Substring(0, text.Length - 1);
				}
				StiDataSource dataSourceFromDataColumn = StiDataColumn.GetDataSourceFromDataColumn(designer.Report.Dictionary, text);
				if (dataSourceFromDataColumn != null)
				{
					return dataSourceFromDataColumn;
				}
				text = text.ToLowerInvariant();
				foreach (StiDataSource dataSource in designer.Report.Dictionary.DataSources)
				{
					string text2 = dataSource.Name.Replace(" ", "").ToLowerInvariant();
					foreach (StiDataColumn column in dataSource.Columns)
					{
						string text3 = column.Name.Replace(" ", "").ToLowerInvariant();
						if (text2 + "." + text3 == text)
						{
							return dataSource;
						}
					}
				}
			}
		}
		return null;
	}

	public static StiDataColumn GetDraggedDataColumn(StiDesignerControl designer)
	{
		if (IsImageColumnDragged(designer))
		{
			return GetDraggedImageDataColumn(designer);
		}
		if (IsBoolColumnDragged(designer))
		{
			return GetDraggedBoolDataColumn(designer);
		}
		return GetDraggedTextColumn(designer);
	}

	public static string GetDraggedDataColumnPath(StiDesignerControl designer)
	{
		if (IsImageColumnDragged(designer))
		{
			return GetDraggedImageDataColumnPath(designer);
		}
		if (IsBoolColumnDragged(designer))
		{
			return GetDraggedBoolDataColumnPath(designer);
		}
		return GetDraggedTextColumnPath(designer);
	}

	public static IStiAppDataCell GetDraggedDataColumnOrVariable(StiDesignerControl designer)
	{
		StiDataColumn draggedDataColumn = GetDraggedDataColumn(designer);
		if (draggedDataColumn != null)
		{
			return draggedDataColumn;
		}
		return GetDraggedVariable(designer);
	}

	private static StiDataColumn GetDraggedTextColumn(StiDesignerControl designer)
	{
		string draggedTextColumnPath = GetDraggedTextColumnPath(designer);
		if (draggedTextColumnPath == null)
		{
			return null;
		}
		return StiDataColumn.GetDataColumnFromColumnName(designer.Report.Dictionary, draggedTextColumnPath, allowRelationName: true);
	}

	private static string GetDraggedTextColumnPath(StiDesignerControl designer)
	{
		string text = null;
		if (designer.PageView.DraggingComponent is IStiTextElement stiTextElement)
		{
			text = stiTextElement.GetSimpleText()?.Trim();
		}
		if (designer.PageView.DraggingComponent is StiText stiText)
		{
			text = stiText.Text?.Value.Trim();
		}
		if (designer.PageView.DraggingComponent is IStiComboBoxElement stiComboBoxElement)
		{
			text = stiComboBoxElement.GetKeyMeter()?.Expression?.Trim();
		}
		if (designer.PageView.DraggingComponent is IStiDatePickerElement stiDatePickerElement)
		{
			string expression = stiDatePickerElement.GetValueMeter()?.Expression?.Trim();
			if (StiDataExpressionHelper.GetDataColumnFromExpression(designer.PageView.GetCurrentPageView().Page as IStiDashboard, expression) is StiDataColumn stiDataColumn)
			{
				text = StiNameValidator.CorrectName(stiDataColumn.DataSource.Name, designer.Report) + "." + stiDataColumn.Name;
			}
		}
		if (string.IsNullOrWhiteSpace(text))
		{
			return null;
		}
		return ExtractExpression(text);
	}

	private static StiDataColumn GetDraggedImageDataColumn(StiDesignerControl designer)
	{
		string draggedImageDataColumnPath = GetDraggedImageDataColumnPath(designer);
		if (string.IsNullOrWhiteSpace(draggedImageDataColumnPath))
		{
			return null;
		}
		return StiDataColumn.GetDataColumnFromColumnName(designer.Report.Dictionary, draggedImageDataColumnPath, allowRelationName: true);
	}

	private static string GetDraggedImageDataColumnPath(StiDesignerControl designer)
	{
		string text = StiHyperlinkProcessor.GetDataColumnNameFromHyperlink((designer.PageView.DraggingComponent is IStiImageElement stiImageElement) ? stiImageElement.ImageHyperlink : null);
		if (string.IsNullOrWhiteSpace(text))
		{
			text = ((!(designer.PageView.DraggingComponent is StiImage stiImage)) ? null : stiImage.DataColumn?.Trim());
		}
		return text;
	}

	private static StiDataColumn GetDraggedBoolDataColumn(StiDesignerControl designer)
	{
		string draggedBoolDataColumnPath = GetDraggedBoolDataColumnPath(designer);
		if (string.IsNullOrWhiteSpace(draggedBoolDataColumnPath))
		{
			return null;
		}
		return StiDataColumn.GetDataColumnFromColumnName(designer.Report.Dictionary, draggedBoolDataColumnPath, allowRelationName: true);
	}

	private static string GetDraggedBoolDataColumnPath(StiDesignerControl designer)
	{
		string dataColumnName = ((!(designer.PageView.DraggingComponent is StiCheckBox stiCheckBox)) ? null : stiCheckBox.Checked.Value?.Trim());
		return ExtractExpression(dataColumnName);
	}

	public static StiVariable GetDraggedVariable(StiDesignerControl designer)
	{
		string text = null;
		if (designer.PageView.DraggingComponent is IStiTextElement stiTextElement)
		{
			text = stiTextElement.GetSimpleText()?.Trim();
		}
		if (designer.PageView.DraggingComponent is IStiComboBoxElement stiComboBoxElement)
		{
			text = stiComboBoxElement.GetKeyMeter()?.Expression;
		}
		if (designer.PageView.DraggingComponent is IStiDatePickerElement stiDatePickerElement)
		{
			text = stiDatePickerElement.GetValueMeter()?.Expression;
		}
		if (designer.PageView.DraggingComponent is StiText stiText)
		{
			text = stiText.Text?.Value.Trim();
		}
		if (string.IsNullOrWhiteSpace(text))
		{
			return null;
		}
		text = ExtractExpression(text);
		text = ExtractVariableName(text);
		return designer.Report.Dictionary.Variables[text];
	}

	public static IStiTableElement GetDraggedTableElement(StiDesignerControl designer)
	{
		return designer.PageView.DraggingComponent as IStiTableElement;
	}

	public static IStiCardsElement GetDraggedCardsElement(StiDesignerControl designer)
	{
		return designer.PageView.DraggingComponent as IStiCardsElement;
	}

	public static IStiImageElement GetDraggedImageElement(StiDesignerControl designer)
	{
		return designer.PageView.DraggingComponent as IStiImageElement;
	}

	public static StiDataBand GetDraggedDataBand(StiDesignerControl designer)
	{
		return designer.PageView.DraggingComponent as StiDataBand;
	}

	private static string ExtractVariableName(string variableName)
	{
		if (variableName == null)
		{
			return null;
		}
		if (variableName.StartsWith("["))
		{
			variableName = variableName.Substring(1);
		}
		if (variableName.EndsWith("]"))
		{
			variableName = variableName.Substring(0, variableName.Length - 1);
		}
		return variableName;
	}

	private static string ExtractExpression(string dataColumnName)
	{
		if (dataColumnName == null)
		{
			return null;
		}
		if (dataColumnName.StartsWith("{"))
		{
			dataColumnName = dataColumnName.Substring(1);
		}
		if (dataColumnName.EndsWith("}"))
		{
			dataColumnName = dataColumnName.Substring(0, dataColumnName.Length - 1);
		}
		return dataColumnName;
	}
}
