using System;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Base.Services;
using Stimulsoft.Data.Extensions;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.Engine;

public class StiSparklineV2Builder : StiComponentV2Builder
{
	public override void Prepare(StiComponent masterComp)
	{
		base.Prepare(masterComp);
		if (masterComp.Height > masterComp.Page.Height || masterComp.Height > masterComp.Parent.Height)
		{
			masterComp.Height = Math.Min(masterComp.Page.Height, masterComp.Parent.Height);
		}
	}

	public override StiComponent InternalRender(StiComponent masterComp)
	{
		StiSparkline stiSparkline = masterComp as StiSparkline;
		StiSparkline stiSparkline2 = ((StiService)stiSparkline).Clone() as StiSparkline;
		stiSparkline2.Values = GetValuesFromValueDataColumn(stiSparkline);
		return stiSparkline2;
	}

	internal static decimal[] GetValuesFromValueDataColumn(StiSparkline sparkline, int maxRows = 0)
	{
		if (string.IsNullOrWhiteSpace(sparkline.ValueDataColumn))
		{
			return null;
		}
		object obj = sparkline.Report[sparkline.ValueDataColumn];
		if (obj != null)
		{
			return GetValuesFromValue(obj);
		}
		obj = sparkline.Report.Dictionary.Variables[sparkline.ValueDataColumn]?.ValueObject;
		if (obj != null)
		{
			return GetValuesFromValue(obj);
		}
		StiDataSource dataSource = sparkline.GetDataSource();
		if (dataSource != null)
		{
			StiDataColumn dataColumnFromColumnName = StiDataColumn.GetDataColumnFromColumnName(sparkline.Report.Dictionary, sparkline.ValueDataColumn);
			if (dataColumnFromColumnName != null && dataColumnFromColumnName.IsArray())
			{
				return ListExt.ToDecimalArray(dataSource[dataColumnFromColumnName.Name]);
			}
			dataSource.SaveState("SparklineRender_DataColumn");
			if (!string.IsNullOrWhiteSpace(sparkline.DataRelationName))
			{
				dataSource.SetDetails(sparkline.DataRelationName);
			}
			decimal[] array = new decimal[dataSource.Count];
			dataSource.First();
			for (int i = 0; i < dataSource.Count; i++)
			{
				object dataFromDataColumn = StiDataColumn.GetDataFromDataColumn(sparkline.Report.Dictionary, sparkline.ValueDataColumn);
				array[i] = StiValueHelper.TryToDecimal(dataFromDataColumn);
				if (maxRows > 0 && i > maxRows)
				{
					break;
				}
				dataSource.Next();
			}
			dataSource.RestoreState("SparklineRender_DataColumn");
			return array;
		}
		StiBusinessObject businessObjectFromDataColumn = StiDataColumn.GetBusinessObjectFromDataColumn(sparkline.Report.Dictionary, sparkline.ValueDataColumn);
		if (businessObjectFromDataColumn != null)
		{
			businessObjectFromDataColumn.SaveState("SparklineRender_DataColumn");
			businessObjectFromDataColumn.CreateEnumerator();
			businessObjectFromDataColumn.specTotalsCalculation = true;
			decimal[] array2 = new decimal[businessObjectFromDataColumn.Count];
			businessObjectFromDataColumn.First();
			for (int j = 0; j < businessObjectFromDataColumn.Count; j++)
			{
				object dataFromBusinessObject = StiDataColumn.GetDataFromBusinessObject(sparkline.Report.Dictionary, sparkline.ValueDataColumn);
				array2[j] = StiValueHelper.TryToDecimal(dataFromBusinessObject);
				if (maxRows > 0 && j > maxRows)
				{
					break;
				}
				businessObjectFromDataColumn.Next();
			}
			businessObjectFromDataColumn.RestoreState("SparklineRender_DataColumn");
			return array2;
		}
		return null;
	}

	private static decimal[] GetValuesFromValue(object value)
	{
		if (ListExt.IsList(value))
		{
			return ListExt.ToDecimalArray(value);
		}
		return new decimal[1] { StiValueHelper.TryToDecimal(value) };
	}
}
