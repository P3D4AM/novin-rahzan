using System;
using System.Collections.Generic;
using System.Data;

namespace Stimulsoft.Base;

public abstract class StiNoSqlDataConnector : StiDataConnector
{
	private StiDataAssemblyHelper assemblyHelper;

	public string FolderAssembly { get; set; }

	public string NameAssembly { get; set; }

	public string ConnectionString { get; set; }

	protected StiDataAssemblyHelper AssemblyHelper
	{
		get
		{
			if (NameAssembly == null)
			{
				throw new NotSupportedException("NameAssembly is not specified!");
			}
			return assemblyHelper ?? (assemblyHelper = new StiDataAssemblyHelper(NameAssembly, FolderAssembly));
		}
	}

	public override bool IsAvailable
	{
		get
		{
			if (AssemblyHelper != null)
			{
				return AssemblyHelper.IsAllowed;
			}
			return false;
		}
	}

	public virtual bool AllowTestConnection => false;

	public abstract StiTestConnectionResult TestConnection();

	public abstract StiDataSchema RetrieveSchema(bool allowException = false);

	public abstract string GetSampleConnectionString();

	public abstract List<StiDataColumnSchema> GetColumns(string collectionName);

	public abstract DataTable GetDataTable(string collectionName, string query, int? index = null, int? count = null);

	public static StiNoSqlDataConnector Get(StiConnectionIdent ident, string connectionString)
	{
		if (ident == StiConnectionIdent.MongoDbDataSource)
		{
			return StiMongoDbConnector.Get(connectionString);
		}
		return null;
	}

	protected StiNoSqlDataConnector(string connectionString)
	{
		ConnectionString = connectionString;
	}
}
