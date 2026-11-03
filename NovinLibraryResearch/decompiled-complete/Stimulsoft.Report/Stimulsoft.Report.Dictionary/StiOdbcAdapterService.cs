using System;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Dictionary;

public class StiOdbcAdapterService : StiSqlAdapterService
{
	public override Type GetDataSourceType()
	{
		return typeof(StiOdbcSource);
	}

	public override StiSqlDataConnector CreateConnector(string connectionString = null)
	{
		return (StiSqlDataConnector)(object)StiOdbcConnector.Get(connectionString);
	}
}
