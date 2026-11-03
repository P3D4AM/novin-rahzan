using System;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Dictionary;

public class StiOracleAdapterService : StiSqlAdapterService
{
	public override Type GetDataSourceType()
	{
		return typeof(StiOracleSource);
	}

	public override StiSqlDataConnector CreateConnector(string connectionString = null)
	{
		return (StiSqlDataConnector)(object)StiOracleConnector.Get(connectionString);
	}
}
