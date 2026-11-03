using System;
using System.Data;
using System.IO;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Dictionary;

public class StiDBaseAdapterService : StiFileAdapterService
{
	public override StiDataColumnsCollection GetColumnsFromData(StiData data, StiDataSource dataSource)
	{
		return GetColumnsFromData(data, dataSource, CommandBehavior.SchemaOnly);
	}

	public override StiDataColumnsCollection GetColumnsFromData(StiData data, StiDataSource dataSource, CommandBehavior retrieveMode)
	{
		StiDataColumnsCollection stiDataColumnsCollection = new StiDataColumnsCollection();
		if (!(dataSource is StiDBaseSource stiDBaseSource))
		{
			return stiDataColumnsCollection;
		}
		try
		{
			if (!File.Exists(stiDBaseSource.Path))
			{
				throw new FileNotFoundException("Can't find file '" + stiDBaseSource.Path + "'");
			}
			using DataTable dataTable = StiDBaseHelper.GetTable(stiDBaseSource.Path, stiDBaseSource.CodePage, 1);
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
		string path = ((StiDBaseSource)dataSource).Path;
		path = (dataSource.Name = ((path != null && path.Trim().Length != 0) ? Path.GetFileNameWithoutExtension(path) : "dBase"));
		dataSource.Alias = path;
	}

	public override Type GetDataSourceType()
	{
		return typeof(StiDBaseSource);
	}

	public override Type[] GetDataTypes()
	{
		return new Type[1] { typeof(StiDBaseSource) };
	}

	public override void ConnectDataSourceToData(StiDictionary dictionary, StiDataSource dataSource, bool loadData)
	{
		try
		{
			StiDataLeader.Disconnect(dataSource);
			StiDBaseSource stiDBaseSource = dataSource as StiDBaseSource;
			if (File.Exists(stiDBaseSource.Path))
			{
				int num = ((!loadData) ? 1 : 0);
				DataTable table = StiDBaseHelper.GetTable(stiDBaseSource.Path, stiDBaseSource.CodePage, num);
				if (!loadData)
				{
					table.Rows.Clear();
				}
				table.TableName = stiDBaseSource.Name;
				dataSource.DataTable = table;
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

	public override StiFileDataConnector CreateConnector()
	{
		return (StiFileDataConnector)(object)StiDBaseConnector.Get();
	}
}
