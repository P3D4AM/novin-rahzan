using System;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Dictionary;

public class StiMySqlAdapterService : StiSqlAdapterService
{
	public override Type GetDataSourceType()
	{
		return typeof(StiMySqlSource);
	}

	public override StiSqlDataConnector CreateConnector(string connectionString = null)
	{
		return (StiSqlDataConnector)(object)StiMySqlConnector.Get(connectionString);
	}
}
