using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Net;
using Stimulsoft.Base.Data.Connectors.Rest.Helpers;
using Stimulsoft.Base.Design;

namespace Stimulsoft.Base;

public class StiODataConnector : StiRestDataConnector
{
	public override StiConnectionIdent ConnectionIdent => StiConnectionIdent.ODataDataSource;

	public override StiConnectionOrder ConnectionOrder => StiConnectionOrder.ODataDataSource;

	public override string Name => "OData";

	public override bool IsAvailable
	{
		get
		{
			if (base.AssemblyHelper != null)
			{
				return base.AssemblyHelper.IsAllowed;
			}
			return false;
		}
	}

	public override string[] NuGetPackages => new string[1] { "Stimulsoft.Data.OData" };

	public override string NuGetVersion => "2022.1.1";

	public StiODataVersion Version { get; set; } = StiODataVersion.V4;


	[Browsable(false)]
	[StiBrowsable(false)]
	public CookieContainer CookieContainer { get; set; }

	public NameValueCollection Headers { get; protected set; }

	[Browsable(false)]
	public bool AllowException { get; set; }

	public override StiDataSchema RetrieveSchema(bool allowException = false)
	{
		if (Version == StiODataVersion.V4)
		{
			return new StiODataHelperV2(base.ConnectionString, NuGetVersion, CookieContainer, Headers)
			{
				AllowException = AllowException
			}.RetrieveSchema();
		}
		return new StiODataHelper(base.ConnectionString, Headers)
		{
			AllowException = AllowException
		}.RetrieveSchema();
	}

	public override void FillDataTable(DataTable table, string query)
	{
		if (Version == StiODataVersion.V4)
		{
			StiODataHelperV2 stiODataHelperV = new StiODataHelperV2(base.ConnectionString, NuGetVersion, CookieContainer, Headers);
			stiODataHelperV.AllowException = AllowException;
			stiODataHelperV.FillDataTable(table, query);
		}
		else
		{
			StiODataHelper stiODataHelper = new StiODataHelper(base.ConnectionString, Headers);
			stiODataHelper.AllowException = AllowException;
			stiODataHelper.FillDataTable(table, query);
		}
	}

	public List<StiDataColumnSchema> GetColumns(string collectionName)
	{
		if (Version == StiODataVersion.V4)
		{
			return new StiODataHelperV2(base.ConnectionString, NuGetVersion, CookieContainer, Headers)
			{
				AllowException = AllowException
			}.GetColumns(collectionName);
		}
		return new StiODataHelper(base.ConnectionString, Headers)
		{
			AllowException = AllowException
		}.GetColumns(collectionName);
	}

	public override StiTestConnectionResult TestConnection()
	{
		return new StiODataHelper(base.ConnectionString, Headers).TestConnection();
	}

	public override string GetSampleConnectionString()
	{
		return "https://services.odata.org/V4/Northwind/Northwind.svc";
	}

	public static StiODataConnector Get(string connectionString = null, NameValueCollection headers = null)
	{
		return new StiODataConnector(connectionString, headers);
	}

	public StiODataConnector(string connectionString = null, NameValueCollection headers = null)
		: base(connectionString)
	{
		base.FolderAssembly = "Stimulsoft.Data.OData-" + NuGetVersion;
		base.NameAssembly = "Stimulsoft.Data.OData.dll";
		Headers = headers ?? new NameValueCollection();
	}
}
