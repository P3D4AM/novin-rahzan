using System;
using System.Data.Common;
using System.Data.Sql;
using System.Security;
using System.Security.Permissions;
using Microsoft.Data.Common;

namespace Microsoft.Data.SqlClient;

public sealed class SqlClientFactory : DbProviderFactory, IServiceProvider
{
	public static readonly SqlClientFactory Instance = new SqlClientFactory();

	public override bool CanCreateDataSourceEnumerator => true;

	private SqlClientFactory()
	{
	}

	public override DbCommand CreateCommand()
	{
		return new SqlCommand();
	}

	public override DbCommandBuilder CreateCommandBuilder()
	{
		return new SqlCommandBuilder();
	}

	public override DbConnection CreateConnection()
	{
		return new SqlConnection();
	}

	public override DbConnectionStringBuilder CreateConnectionStringBuilder()
	{
		return new SqlConnectionStringBuilder();
	}

	public override DbDataAdapter CreateDataAdapter()
	{
		return new SqlDataAdapter();
	}

	public override DbParameter CreateParameter()
	{
		return new SqlParameter();
	}

	public CodeAccessPermission CreatePermission(PermissionState state)
	{
		return (CodeAccessPermission)(object)new SqlClientPermission(state);
	}

	public override DbDataSourceEnumerator CreateDataSourceEnumerator()
	{
		return (DbDataSourceEnumerator)(object)SqlDataSourceEnumerator.Instance;
	}

	object IServiceProvider.GetService(Type serviceType)
	{
		object result = null;
		if (serviceType == GreenMethods.SystemDataCommonDbProviderServices_Type)
		{
			result = GreenMethods.MicrosoftDataSqlClientSqlProviderServices_Instance();
		}
		return result;
	}
}
