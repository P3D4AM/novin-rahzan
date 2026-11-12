using System;
using System.Collections.Generic;
using System.Data;

namespace Stimulsoft.Base;

public class StiDBaseConnector : StiFileDataConnector
{
	public override StiConnectionIdent ConnectionIdent => StiConnectionIdent.DBaseDataSource;

	public override StiConnectionOrder ConnectionOrder => StiConnectionOrder.DBaseDataSource;

	public override string Name => "DBase";

	public override bool IsAvailable => true;

	public override StiFileType FileType => StiFileType.Dbf;

	public override DataSet GetDataSet(StiFileDataOptions options)
	{
		if (!(options is StiDBaseOptions stiDBaseOptions))
		{
			throw new NotSupportedException("Only StiDBaseOptions accepted!");
		}
		if (!string.IsNullOrWhiteSpace(stiDBaseOptions.Path) && stiDBaseOptions.Content == null)
		{
			return GetDataSetFromPath(stiDBaseOptions);
		}
		if (stiDBaseOptions.Content == null || stiDBaseOptions.Content.Length == 0)
		{
			return null;
		}
		return StiDBaseHelper.GetDataSet(stiDBaseOptions.Content, stiDBaseOptions.TableName, forceCodePage: false, stiDBaseOptions.CodePage);
	}

	private DataSet GetDataSetFromPath(StiDBaseOptions options)
	{
		DataSet dataSet = null;
		List<StiDataLoaderHelper.Data> list = StiDataLoaderHelper.LoadMultiple(options.Path, "*.dbf");
		if (list != null)
		{
			foreach (StiDataLoaderHelper.Data item in list)
			{
				try
				{
					DataTable table = StiDBaseHelper.GetTable(item.Array, options.CodePage, options.MaxDataRows);
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

	public static StiDBaseConnector Get()
	{
		return new StiDBaseConnector();
	}

	private StiDBaseConnector()
	{
	}
}
