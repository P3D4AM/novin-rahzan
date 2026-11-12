using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Stimulsoft.Base.Data.Connectors.Google;

public class StiGoogleSheetsConnector : StiDataConnector
{
	private StiDataAssemblyHelper assemblyHelper;

	private object googleSheetsProvider;

	public string ClientId { get; set; }

	public string ClientSecret { get; set; }

	public string SpreadsheetId { get; set; }

	public bool FirstRowIsHeader { get; set; }

	public override StiConnectionIdent ConnectionIdent => StiConnectionIdent.GoogleSheetsStorage;

	public override StiConnectionOrder ConnectionOrder => StiConnectionOrder.GoogleSheetsStorage;

	public override string Name => "Google Sheets";

	public override bool IsAvailable => GetAssemblyData()?.IsAllowed ?? false;

	public override string[] NuGetPackages => new string[1] { "Stimulsoft.Data.GoogleSheets" };

	public override string NuGetVersion => "2021.4.3";

	private StiDataAssemblyHelper GetAssemblyData()
	{
		if (assemblyHelper != null)
		{
			return assemblyHelper;
		}
		assemblyHelper = new StiDataAssemblyHelper("Stimulsoft.Data.GoogleSheets.dll", "Stimulsoft.Data.GoogleSheets-" + NuGetVersion);
		return assemblyHelper;
	}

	private object GetGoogleSheetsProvider()
	{
		if (googleSheetsProvider != null)
		{
			return googleSheetsProvider;
		}
		StiDataAssemblyHelper assemblyData = GetAssemblyData();
		googleSheetsProvider = assemblyData.CreateObject("Stimulsoft.Data.GoogleSheets.StiGoogleSheetsProvider", ClientId, ClientSecret, SpreadsheetId, FirstRowIsHeader);
		return googleSheetsProvider;
	}

	public StiDataSchema RetrieveSchema(bool allowException = false)
	{
		StiDataSchema stiDataSchema = new StiDataSchema(ConnectionIdent);
		try
		{
			object obj = GetGoogleSheetsProvider();
			MethodInfo method = obj.GetType().GetMethod("GetTableNames");
			List<string> list = method.Invoke(obj, null) as List<string>;
			foreach (string item in list)
			{
				StiDataTableSchema stiDataTableSchema = StiDataTableSchema.NewTable(item);
				try
				{
					List<StiDataColumnSchema> columns = GetColumns(item);
					if (columns != null)
					{
						stiDataTableSchema.Columns = columns;
					}
				}
				catch
				{
				}
				stiDataSchema.Tables.Add(stiDataTableSchema);
			}
			return stiDataSchema.Sort();
		}
		catch (Exception)
		{
			if (allowException)
			{
				throw;
			}
			return null;
		}
	}

	public DataTable GetDataTable(string collectionName, string query, int? index = null, int? count = null)
	{
		try
		{
			object obj = GetGoogleSheetsProvider();
			MethodInfo method = obj.GetType().GetMethod("GetDataTable");
			return method.Invoke(obj, new object[3] { collectionName, index, count }) as DataTable;
		}
		catch (Exception ex)
		{
			if (ex.InnerException != null)
			{
				throw ex.InnerException;
			}
			throw;
		}
	}

	public List<StiDataColumnSchema> GetColumns(string collectionName)
	{
		return (from c in GetDataTable(collectionName, null, 0, 5)?.Columns.Cast<DataColumn>().ToList()
			select new StiDataColumnSchema(c.ColumnName, c.DataType)).ToList();
	}

	public static StiGoogleSheetsConnector Get(string clientId, string clientSecret, string spreadsheetId, bool firstRowIsHeader)
	{
		return new StiGoogleSheetsConnector
		{
			ClientId = clientId,
			ClientSecret = clientSecret,
			SpreadsheetId = spreadsheetId,
			FirstRowIsHeader = firstRowIsHeader
		};
	}
}
