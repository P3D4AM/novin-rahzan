using System;
using System.Collections.Generic;
using System.Data;

namespace Stimulsoft.Base;

public class StiCsvConnector : StiFileDataConnector
{
	public override StiConnectionIdent ConnectionIdent => StiConnectionIdent.CsvDataSource;

	public override StiConnectionOrder ConnectionOrder => StiConnectionOrder.CsvDataSource;

	public override string Name => "CSV";

	public override bool IsAvailable => true;

	public override StiFileType FileType => StiFileType.Csv;

	public override DataSet GetDataSet(StiFileDataOptions options)
	{
		if (!(options is StiCsvOptions stiCsvOptions))
		{
			throw new NotSupportedException("Only StiCsvOptions accepted!");
		}
		if (!string.IsNullOrWhiteSpace(stiCsvOptions.Path) && stiCsvOptions.Content == null)
		{
			return GetDataSetFromPath(stiCsvOptions);
		}
		if (stiCsvOptions.Content == null || stiCsvOptions.Content.Length == 0)
		{
			return null;
		}
		return StiCsvHelper.GetDataSet(stiCsvOptions.Content, stiCsvOptions.TableName, stiCsvOptions.CodePage, stiCsvOptions.Separator, stiCsvOptions.MaxDataRows);
	}

	private DataSet GetDataSetFromPath(StiCsvOptions options)
	{
		DataSet dataSet = null;
		List<StiDataLoaderHelper.Data> list = StiDataLoaderHelper.LoadMultiple(options.Path, "*.csv");
		if (list != null)
		{
			foreach (StiDataLoaderHelper.Data item in list)
			{
				try
				{
					DataTable table = StiCsvHelper.GetTable(item.Array, options.CodePage, options.Separator, options.MaxDataRows);
					if (table != null)
					{
						if (dataSet == null)
						{
							dataSet = new DataSet
							{
								EnforceConstraints = false
							};
						}
						table.TableName = item.Name;
						dataSet.Tables.Add(table);
					}
				}
				catch
				{
				}
			}
		}
		return dataSet;
	}

	public static StiCsvConnector Get()
	{
		return new StiCsvConnector();
	}

	private StiCsvConnector()
	{
	}
}
