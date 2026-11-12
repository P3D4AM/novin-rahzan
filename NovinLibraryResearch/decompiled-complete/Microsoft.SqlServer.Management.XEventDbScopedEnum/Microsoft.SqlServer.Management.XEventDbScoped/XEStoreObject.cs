using System;
using System.Data;
using System.Globalization;
using System.Reflection;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.XEventDbScoped;

internal sealed class XEStoreObject : SqlObject, ISupportVersions, ISupportDatabaseEngineTypes
{
	public override Assembly ResourceAssembly => Assembly.GetExecutingAssembly();

	public ServerVersion GetServerVersion(object conn)
	{
		return ExecuteSql.GetServerVersion(conn);
	}

	public DatabaseEngineType GetDatabaseEngineType(object conn)
	{
		return ExecuteSql.GetDatabaseEngineType(conn);
	}

	public override EnumResult GetData(EnumResult parentResult)
	{
		string fixedStringProperty = GetFixedStringProperty("Name", removeEscape: true);
		base.Filter = null;
		if (fixedStringProperty != null)
		{
			base.StatementBuilder = new StatementBuilder();
			DatabaseEngineType databaseEngineType = ExecuteSql.GetDatabaseEngineType(base.ConnectionInfo);
			SqlEnumResult sqlEnumResult = new SqlEnumResult(base.StatementBuilder, ResultType.Reserved1, databaseEngineType);
			DataTable dataTable = new DataTable();
			dataTable.Locale = CultureInfo.InvariantCulture;
			dataTable.Columns.Add("Name", Type.GetType("System.String"));
			DataRow dataRow = dataTable.NewRow();
			dataRow[0] = Urn.UnEscapeString(fixedStringProperty);
			dataTable.Rows.Add(dataRow);
			sqlEnumResult.Databases = dataTable;
			sqlEnumResult.NameProperties.Add("Name");
			return base.GetData((EnumResult)sqlEnumResult);
		}
		return base.GetData(parentResult);
	}
}
