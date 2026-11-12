using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Stimulsoft.Base.Data.Connectors;

namespace Stimulsoft.Base;

public class StiAzureBlobStorageConnector : StiDbNoSqlDataConnector
{
	private object azureBlobStorageProvider;

	private static readonly StiAzureBlobStorageConnector emptyConnector = new StiAzureBlobStorageConnector();

	public override StiConnectionIdent ConnectionIdent => StiConnectionIdent.AzureBlobStorage;

	public override StiConnectionOrder ConnectionOrder => StiConnectionOrder.AzureBlobStorage;

	public override string Name => "Azure Blob Storage";

	public override string[] NuGetPackages => new string[1] { "Stimulsoft.Data.AzureBlobStorage" };

	public override string NuGetVersion => "2021.4.3";

	public override string GetSampleConnectionString()
	{
		return "AccountKey=<key>;AccountName=<account>;ContainerName=<conainer>;BlobName=<blob>;" + Environment.NewLine + "BlobContentType=CSV;CodePage=<page>;CsvSeparator=<separator>; OR" + Environment.NewLine + "BlobContentType=Excel;FirstRowIsHeader=<" + bool.TrueString + ">; OR" + Environment.NewLine + "BlobContentType=JSON; OR" + Environment.NewLine + "BlobContentType=XML;";
	}

	public override StiTestConnectionResult TestConnection()
	{
		try
		{
			object obj = GetAzureBlobStorageProvider();
			MethodInfo method = obj.GetType().GetMethod("IsConnectionAvailable");
			return ((bool)method.Invoke(obj, null)) ? StiTestConnectionResult.MakeFine() : StiTestConnectionResult.MakeWrong("Couldn't open connection to server");
		}
		catch (Exception ex)
		{
			if (ex.InnerException != null)
			{
				return StiTestConnectionResult.MakeWrong(ex.InnerException.Message);
			}
			return StiTestConnectionResult.MakeWrong(ex.Message);
		}
	}

	public override StiDataSchema RetrieveSchema(bool allowException = false)
	{
		DataSet fullDataset = GetFullDataset();
		StiDataSchema stiDataSchema = new StiDataSchema(ConnectionIdent);
		foreach (DataTable table in fullDataset.Tables)
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
		foreach (DataRelation relation in fullDataset.Relations)
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

	public override List<StiDataColumnSchema> GetColumns(string collectionName)
	{
		return RetrieveSchema().Tables.FirstOrDefault((StiDataTableSchema table) => table.Name.ToLowerInvariant() == collectionName.ToLowerInvariant())?.Columns;
	}

	public override DataTable GetDataTable(string collectionName, string query, int? index = null, int? count = null)
	{
		return GetFullDataset().Tables[collectionName].Copy();
	}

	public List<string> GetContainerNamesList(string prefix = null)
	{
		try
		{
			object obj = GetAzureBlobStorageProvider();
			MethodInfo method = obj.GetType().GetMethod("GetContainerNamesList");
			object[] parameters = new string[1] { prefix };
			return method.Invoke(obj, parameters) as List<string>;
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

	public List<string> GetBlobNamesList(string prefix = null)
	{
		try
		{
			object obj = GetAzureBlobStorageProvider();
			MethodInfo method = obj.GetType().GetMethod("GetBlobNamesList");
			return method.Invoke(obj, new object[1] { prefix }) as List<string>;
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

	public string GetBlobContentTypeOrDefault()
	{
		byte[] array;
		try
		{
			object obj = GetAzureBlobStorageProvider();
			MethodInfo method = obj.GetType().GetMethod("GetBlobContentPart");
			array = method.Invoke(obj, new object[2] { 0, 10 }) as byte[];
		}
		catch
		{
			return null;
		}
		if (array.Length == 0)
		{
			return null;
		}
		char c;
		using (StreamReader streamReader = new StreamReader(new MemoryStream(array, 0, array.Length), Encoding.Default, detectEncodingFromByteOrderMarks: true, 128))
		{
			c = (char)streamReader.Read();
		}
		return c switch
		{
			'{' => "JSON", 
			'<' => "XML", 
			'P' => "Excel", 
			_ => null, 
		};
	}

	private object GetAzureBlobStorageProvider()
	{
		if (azureBlobStorageProvider != null)
		{
			return azureBlobStorageProvider;
		}
		azureBlobStorageProvider = base.AssemblyHelper.CreateObject("Stimulsoft.Data.AzureBlobStorage.StiAzureBlobStorageProvider", base.ConnectionString.Substring(0, base.ConnectionString.IndexOf(";BlobContentType=")));
		return azureBlobStorageProvider;
	}

	private DataSet GetFullDataset()
	{
		try
		{
			object obj = GetAzureBlobStorageProvider();
			MethodInfo method = obj.GetType().GetMethod("GetBlobContent");
			byte[] blobContent = method.Invoke(obj, null) as byte[];
			return GetDataSetFromBlobContent(blobContent);
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

	private DataSet GetDataSetFromBlobContent(byte[] blobContent)
	{
		string text = StiConnectionStringHelper.GetConnectionStringKey(base.ConnectionString, "BlobContentType") ?? string.Empty;
		DataSet dataSet;
		switch (text)
		{
		case "CSV":
		{
			string s = StiConnectionStringHelper.GetConnectionStringKey(base.ConnectionString, "CodePage") ?? "0";
			string value2 = Regex.Match(base.ConnectionString, "Delimiter=\"(.*?)\";").Groups[1].Value;
			dataSet = StiCsvConnector.Get().GetDataSet(new StiCsvOptions(blobContent, int.Parse(s), value2));
			break;
		}
		case "Excel":
		{
			string value = StiConnectionStringHelper.GetConnectionStringKey(base.ConnectionString, "FirstRowIsHeader") ?? bool.TrueString;
			dataSet = StiExcelConnector.Get().GetDataSet(new StiExcelOptions(blobContent, bool.Parse(value)));
			break;
		}
		case "JSON":
			dataSet = StiJsonConnector.Get().GetDataSet(new StiJsonOptions(blobContent));
			break;
		case "XML":
			dataSet = StiXmlConnector.Get().GetDataSet(new StiXmlOptions(blobContent));
			break;
		default:
			throw new Exception("Blob content type \"" + text + "\" is not supported.");
		}
		return dataSet ?? new DataSet();
	}

	public static StiAzureBlobStorageConnector Get(string connectionString = null)
	{
		if (connectionString == null)
		{
			return emptyConnector;
		}
		return new StiAzureBlobStorageConnector(connectionString);
	}

	private StiAzureBlobStorageConnector(string connectionString = null)
		: base(connectionString)
	{
		base.FolderAssembly = "Stimulsoft.Data.AzureBlobStorage-" + NuGetVersion;
		base.NameAssembly = "Stimulsoft.Data.AzureBlobStorage.dll";
	}
}
