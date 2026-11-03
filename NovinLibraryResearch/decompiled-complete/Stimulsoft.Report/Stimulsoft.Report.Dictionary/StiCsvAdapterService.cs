using System;
using System.Data;
using System.IO;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Dictionary;

public class StiCsvAdapterService : StiFileAdapterService
{
	public override StiDataColumnsCollection GetColumnsFromData(StiData data, StiDataSource dataSource)
	{
		return GetColumnsFromData(data, dataSource, CommandBehavior.SchemaOnly);
	}

	public override StiDataColumnsCollection GetColumnsFromData(StiData data, StiDataSource dataSource, CommandBehavior retrieveMode)
	{
		StiDataColumnsCollection stiDataColumnsCollection = new StiDataColumnsCollection();
		if (!(dataSource is StiCsvSource stiCsvSource))
		{
			return stiDataColumnsCollection;
		}
		try
		{
			if (!File.Exists(stiCsvSource.Path))
			{
				throw new FileNotFoundException("Can't find file '" + stiCsvSource.Path + "'");
			}
			using DataTable dataTable = StiCsvHelper.GetTable(stiCsvSource.Path, stiCsvSource.CodePage, stiCsvSource.Separator, 1);
			foreach (DataColumn column in dataTable.Columns)
			{
				string name = StiNameValidator.CorrectName(column.ColumnName, dataSource.Dictionary?.Report);
				stiDataColumnsCollection.Add(new StiDataColumn(name, column.Caption, column.DataType));
			}
		}
		catch (Exception e)
		{
			StiLogService.Write(((object)this).GetType(), e);
			if (!StiOptions.Engine.HideExceptions)
			{
				throw;
			}
		}
		return stiDataColumnsCollection;
	}

	public override StiDataParametersCollection GetParametersFromData(StiData data, StiDataSource dataSource)
	{
		return new StiDataParametersCollection();
	}

	public override void SetDataSourceNames(StiData data, StiDataSource dataSource)
	{
		base.SetDataSourceNames(data, dataSource);
		string path = ((StiCsvSource)dataSource).Path;
		path = (dataSource.Name = ((path != null && path.Trim().Length != 0) ? Path.GetFileNameWithoutExtension(path) : "Csv"));
		dataSource.Alias = path;
	}

	public override Type GetDataSourceType()
	{
		return typeof(StiCsvSource);
	}

	public override Type[] GetDataTypes()
	{
		return new Type[1] { typeof(StiCsvSource) };
	}

	public override void ConnectDataSourceToData(StiDictionary dictionary, StiDataSource dataSource, bool loadData)
	{
		try
		{
			StiDataLeader.Disconnect(dataSource);
			StiCsvSource stiCsvSource = dataSource as StiCsvSource;
			if (File.Exists(stiCsvSource.Path))
			{
				int num = ((!loadData) ? 1 : 0);
				DataTable table = StiCsvHelper.GetTable(stiCsvSource.Path, stiCsvSource.CodePage, stiCsvSource.Separator, num);
				if (!loadData)
				{
					table.Rows.Clear();
				}
				table.TableName = stiCsvSource.Name;
				dataSource.DataTable = table;
				CheckConvertNulls(stiCsvSource);
			}
		}
		catch (Exception e)
		{
			StiLogService.Write(((object)this).GetType(), e);
			if (!StiOptions.Engine.HideExceptions)
			{
				throw;
			}
		}
	}

	private void CheckConvertNulls(StiCsvSource dataSource)
	{
		if (dataSource.Dictionary == null || dataSource.Dictionary.Report == null || dataSource.Dictionary.Report.ConvertNulls || !dataSource.ConvertEmptyStringToNull)
		{
			return;
		}
		DataTable dataTable = dataSource.DataTable;
		bool[] array = new bool[dataTable.Columns.Count];
		for (int i = 0; i < dataTable.Columns.Count; i++)
		{
			DataColumn dataColumn = dataTable.Columns[i];
			StiDataColumn stiDataColumn = dataSource.Columns[dataColumn.ColumnName];
			if (stiDataColumn != null && stiDataColumn.Type != typeof(string))
			{
				array[i] = true;
			}
		}
		foreach (DataRow row in dataTable.Rows)
		{
			for (int j = 0; j < dataTable.Columns.Count; j++)
			{
				if (array[j])
				{
					object obj = row[j];
					if (obj is string && (string)obj == string.Empty)
					{
						row[j] = null;
					}
				}
			}
		}
	}

	public override StiFileDataConnector CreateConnector()
	{
		return (StiFileDataConnector)(object)StiCsvConnector.Get();
	}
}
