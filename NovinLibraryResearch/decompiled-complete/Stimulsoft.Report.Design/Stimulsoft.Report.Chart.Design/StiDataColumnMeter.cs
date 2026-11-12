using System.Collections.Generic;
using Stimulsoft.Base;
using Stimulsoft.Data.Functions;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.Chart.Design;

public class StiDataColumnMeter
{
	public StiSeries Series { get; set; }

	public string DataColumn { get; set; } = string.Empty;


	public StiChartSeriesType SeriesType { get; set; } = StiChartSeriesType.ClusteredColumn;


	public List<StiDataColumnMeter> ToList()
	{
		return new List<StiDataColumnMeter> { this };
	}

	public StiDataColumnMeter()
	{
	}

	public StiDataColumnMeter(string expression)
	{
		DataColumn = expression;
	}

	public StiDataColumnMeter(IStiAppDataCell cell)
	{
		if (cell is StiDataColumn stiDataColumn)
		{
			DataColumn = stiDataColumn.DataSource.Name + "." + stiDataColumn.Name;
		}
		else
		{
			DataColumn = Funcs.ToExpression(cell.GetName());
		}
	}
}
