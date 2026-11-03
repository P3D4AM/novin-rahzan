using System;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Dictionary;

public class StiPostgreSQLAdapterService : StiSqlAdapterService
{
	public override Type GetDataSourceType()
	{
		return typeof(StiPostgreSQLSource);
	}

	public override StiSqlDataConnector CreateConnector(string connectionString = null)
	{
		return (StiSqlDataConnector)(object)StiPostgreSqlConnector.Get(connectionString);
	}
}
