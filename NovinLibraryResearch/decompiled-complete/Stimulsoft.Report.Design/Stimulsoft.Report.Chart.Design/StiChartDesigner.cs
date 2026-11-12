using System;
using System.Windows.Forms;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Inplace;

namespace Stimulsoft.Report.Chart.Design;

public class StiChartDesigner : StiInplaceDesigner
{
	public static StiChartSeriesType PredefinedSeriesType { get; set; }

	internal static StiChartSeriesType ConvertSeriesToChartSeriesType(StiSeries series)
	{
		if (series == null)
		{
			return StiChartSeriesType.ClusteredColumn;
		}
		string text = series.GetType().ToString();
		int startIndex = text.LastIndexOf(".") + 1;
		text = text.Substring(startIndex).Substring(3).Replace("Series", "");
		return (StiChartSeriesType)Enum.Parse(typeof(StiChartSeriesType), text);
	}

	public static bool IsSimpleEditor(StiComponent component)
	{
		if (component is StiChart stiChart)
		{
			return stiChart.EditorType == StiChartEditorType.Simple;
		}
		return false;
	}

	public override bool IsAllowCenterWindow(object component)
	{
		return !IsSimpleEditor(component as StiChart);
	}

	public override DialogResult Design(StiComponent component)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		if (base.FirstRun && StiOptions.Designer.Editors.ChartEditor.SimpleEditorByDefault)
		{
			StiChart stiChart = component as StiChart;
			stiChart.EditorType = StiChartEditorType.Simple;
		}
		if (base.FirstRun)
		{
			base.Designer.UndoRedoSave(component.LocalizedName, onlyCurrentPage: true);
		}
		return base.Design(component);
	}

	protected override StiInplaceEditorForm CreateEditorForm(object component)
	{
		StiChart stiChart = component as StiChart;
		if (IsSimpleEditor(stiChart))
		{
			StiChartEditorFormV2 stiChartEditorFormV = new StiChartEditorFormV2(stiChart);
			stiChartEditorFormV.OldSeriesType = PredefinedSeriesType;
			return stiChartEditorFormV;
		}
		return new StiCloudChartForm(stiChart);
	}

	public StiChartDesigner(IStiDesignerBase designer)
		: base(designer)
	{
	}
}
