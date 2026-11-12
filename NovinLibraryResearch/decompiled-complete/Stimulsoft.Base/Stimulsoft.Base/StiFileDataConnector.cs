using System;
using System.Data;
using System.Linq;

namespace Stimulsoft.Base;

public abstract class StiFileDataConnector : StiDataConnector
{
	public abstract StiFileType FileType { get; }

	public StiDataSchema RetrieveSchema(StiFileDataOptions options)
	{
		if (options == null)
		{
			return null;
		}
		DataSet dataSet = GetDataSet(options);
		if (dataSet == null)
		{
			return null;
		}
		StiDataSchema stiDataSchema = new StiDataSchema(ConnectionIdent);
		foreach (DataTable table in dataSet.Tables)
		{
			StiDataTableSchema stiDataTableSchema = StiDataTableSchema.NewTable(table.TableName);
			foreach (DataColumn column in table.Columns)
			{
				stiDataTableSchema.Columns.Add(new StiDataColumnSchema
				{
					Name = column.ColumnName,
					Type = column.DataType
				});
			}
			stiDataSchema.Tables.Add(stiDataTableSchema);
		}
		foreach (DataRelation relation in dataSet.Relations)
		{
			stiDataSchema.Relations.Add(new StiDataRelationSchema
			{
				Name = relation.RelationName,
				ParentSourceName = relation.ParentTable.TableName,
				ChildSourceName = relation.ChildTable.TableName,
				ParentColumns = relation.ParentColumns.Select((DataColumn col) => col.ColumnName).ToList(),
				ChildColumns = relation.ChildColumns.Select((DataColumn col) => col.ColumnName).ToList()
			});
		}
		stiDataSchema.Relations = stiDataSchema.Relations.OrderBy((StiDataRelationSchema e) => e.Name).ToList();
		stiDataSchema.Tables = stiDataSchema.Tables.OrderBy((StiDataTableSchema e) => e.Name).ToList();
		return stiDataSchema;
	}

	public StiTestConnectionResult TestConnection(StiFileDataOptions options)
	{
		try
		{
			if (options == null || options.Content == null || options.Content.Length == 0)
			{
				return StiTestConnectionResult.MakeWrong(Name + " content is not found!");
			}
			DataSet dataSet = GetDataSet(options);
			if (dataSet == null)
			{
				return StiTestConnectionResult.MakeWrong(Name + " content is not recognized!");
			}
			return StiTestConnectionResult.MakeFine();
		}
		catch (Exception exception)
		{
			return StiTestConnectionResult.MakeWrong(exception);
		}
	}

	public abstract DataSet GetDataSet(StiFileDataOptions options);

	public static StiFileDataConnector Get(StiConnectionIdent ident)
	{
		return ident switch
		{
			StiConnectionIdent.CsvDataSource => StiCsvConnector.Get(), 
			StiConnectionIdent.DBaseDataSource => StiDBaseConnector.Get(), 
			StiConnectionIdent.ExcelDataSource => StiExcelConnector.Get(), 
			StiConnectionIdent.JsonDataSource => StiJsonConnector.Get(), 
			StiConnectionIdent.XmlDataSource => StiXmlConnector.Get(), 
			_ => null, 
		};
	}
}
