using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Data.Connectors.Google;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Dictionary.Adapters.Google;
using Stimulsoft.Report.Dictionary.DataSources.Google;
using Stimulsoft.Report.Dictionary.Design;

namespace Stimulsoft.Report.Dictionary.Databases.Google;

[TypeConverter(typeof(StiGoogleSheetsDatabaseConverter))]
public class StiGoogleSheetsDatabase : StiDatabase
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiGoogleSheetsDatabase;

	public override StiConnectionType ConnectionType => StiConnectionType.Google;

	[StiSerializable]
	public string ClientId { get; set; }

	[StiSerializable]
	public string ClientSecret { get; set; }

	[StiSerializable]
	public string SpreadsheetId { get; set; }

	[StiSerializable]
	[StiOrder(350)]
	[DefaultValue(true)]
	public bool FirstRowIsHeader { get; set; }

	public override StiDatabase CreateNew()
	{
		return new StiGoogleSheetsDatabase();
	}

	public StiGoogleSheetsConnector CreateGoogleSheetsConnector()
	{
		StiDataConnector obj = CreateConnector();
		return (StiGoogleSheetsConnector)(object)((obj is StiGoogleSheetsConnector) ? obj : null);
	}

	public override StiDataConnector CreateConnector(string connectionString = null)
	{
		return (StiDataConnector)(object)StiGoogleSheetsConnector.Get(ClientId, ClientSecret, SpreadsheetId, FirstRowIsHeader);
	}

	public override void ApplyDatabaseInformation(StiDatabaseInformation information, StiReport report)
	{
		ApplyDatabaseInformation(information, report, null);
	}

	public override void ApplyDatabaseInformation(StiDatabaseInformation information, StiReport report, StiDatabaseInformation informationAll)
	{
		foreach (DataTable table in information.Tables)
		{
			ApplyDatabaseInformationSource(information, report, informationAll, table);
		}
	}

	protected virtual void ApplyDatabaseInformationSource(StiDatabaseInformation information, StiReport report, StiDatabaseInformation informationAll, DataTable dataTable, StiSqlSourceType type = StiSqlSourceType.Table)
	{
		StiGoogleSheetsSource stiGoogleSheetsSource = new StiGoogleSheetsSource(base.Name, StiNameCreation.CreateName(report, dataTable.TableName, addOne: false, removeIncorrectSymbols: false, forceAdvancedNamingRule: true));
		stiGoogleSheetsSource.Alias = dataTable.TableName;
		foreach (DataColumn column in dataTable.Columns)
		{
			stiGoogleSheetsSource.Columns.Add(new StiDataColumn(column.ColumnName, column.DataType));
		}
		report.Dictionary.DataSources.Add(stiGoogleSheetsSource);
	}

	public override DialogResult Edit(StiDictionary dictionary, bool newDatabase)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		return StiDataEditorsHelper.Get().GoogleSheetsDatabaseEdit(this, dictionary, newDatabase);
	}

	public override async Task<DialogResult> EditAsync(StiDictionary dictionary, bool newDatabase)
	{
		return await StiDataEditorsHelper.Get().GoogleSheetsDatabaseEditAsync(this, dictionary, newDatabase);
	}

	public StiGoogleSheetsAdapterService GetDataAdapter()
	{
		return new StiGoogleSheetsAdapterService();
	}

	public override void RegData(StiDictionary dictionary, bool loadData)
	{
		StiGoogleSheetsAdapterService dataAdapter = GetDataAdapter();
		if (dataAdapter == null)
		{
			throw new StiException($"A data adapter for the database {((object)this).GetType()} not found");
		}
		dataAdapter.CreateConnectionInDataStore(dictionary, this);
	}

	public override StiDatabaseInformation GetDatabaseInformation(StiReport report)
	{
		try
		{
			StiDatabaseInformation stiDatabaseInformation = new StiDatabaseInformation();
			StiGoogleSheetsConnector val = CreateGoogleSheetsConnector();
			StiDataSchema val2 = val.RetrieveSchema(base.AllowException);
			if (val2 == null)
			{
				return stiDatabaseInformation;
			}
			stiDatabaseInformation.Tables.AddRange(GetDatabaseInformationTables(val2));
			return stiDatabaseInformation;
		}
		catch (Exception)
		{
			if (base.AllowException)
			{
				throw;
			}
			return null;
		}
	}

	protected virtual List<DataTable> GetDatabaseInformationTables(StiDataSchema dataSchema)
	{
		List<DataTable> list = new List<DataTable>();
		foreach (StiDataTableSchema table2 in dataSchema.Tables)
		{
			try
			{
				DataTable table = new DataTable(((StiObjectSchema)table2).Name);
				table2.Columns.ForEach(delegate(StiDataColumnSchema schemaColumn)
				{
					if (table.Columns[((StiObjectSchema)schemaColumn).Name] == null)
					{
						table.Columns.Add(((StiObjectSchema)schemaColumn).Name, schemaColumn.Type);
					}
				});
				list.Add(table);
			}
			catch
			{
			}
		}
		return list;
	}

	public StiGoogleSheetsDatabase()
	{
		FirstRowIsHeader = true;
	}

	public StiGoogleSheetsDatabase(string name, string alias, string key, string clientId, string clientSecret, string spreadsheetId, bool firstRowIsHeader)
		: base(name, alias, key)
	{
		ClientId = clientId;
		ClientSecret = clientSecret;
		SpreadsheetId = spreadsheetId;
		FirstRowIsHeader = firstRowIsHeader;
	}
}
