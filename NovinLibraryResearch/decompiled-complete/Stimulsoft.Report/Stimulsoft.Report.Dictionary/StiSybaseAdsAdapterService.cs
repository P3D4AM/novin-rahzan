using System;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Dictionary;

public class StiSybaseAdsAdapterService : StiSqlAdapterService
{
	public override Type GetDataSourceType()
	{
		return typeof(StiSybaseAdsSource);
	}

	public override StiSqlDataConnector CreateConnector(string connectionString = null)
	{
		return (StiSqlDataConnector)(object)StiSybaseAdsConnector.Get(connectionString);
	}
}
