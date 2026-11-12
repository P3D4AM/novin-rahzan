using System.Collections.Generic;
using System.Data;
using Stimulsoft.Base.Data.Connectors.OnlineServices.Helpers;

namespace Stimulsoft.Base.Data.Connectors.OnlineServices;

public class StiDataWorldConnector : StiDbNoSqlDataConnector
{
	public override StiConnectionIdent ConnectionIdent => StiConnectionIdent.DataWorldDataSource;

	public override StiConnectionOrder ConnectionOrder => StiConnectionOrder.DataWorldDataSource;

	public override string Name => "Data.World";

	public override bool IsAvailable => true;

	public override List<StiDataColumnSchema> GetColumns(string collectionName)
	{
		return new StiDataWorldHelper(base.ConnectionString).GetColumns(collectionName);
	}

	public override DataTable GetDataTable(string collectionName, string query, int? index = null, int? count = null)
	{
		return new StiDataWorldHelper(base.ConnectionString).GetDataTable(collectionName, query);
	}

	public override string GetSampleConnectionString()
	{
		return "Owner=<owner>;Database=<id>;Token=<token>";
	}

	public override StiDataSchema RetrieveSchema(bool allowException = false)
	{
		return new StiDataWorldHelper(base.ConnectionString).RetrieveSchema(allowException);
	}

	public override StiTestConnectionResult TestConnection()
	{
		return new StiDataWorldHelper(base.ConnectionString).TestConnection();
	}

	public static StiDataWorldConnector Get(string connectionString = null)
	{
		return new StiDataWorldConnector(connectionString);
	}

	public StiDataWorldConnector(string connectionString = null)
		: base(connectionString)
	{
		base.NameAssembly = "Stimulsoft.Data.DataWorld.dll";
	}
}
