using System;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Dictionary;

public class StiSybaseAdapterService : StiSqlAdapterService
{
	public override Type GetDataSourceType()
	{
		return typeof(StiSybaseSource);
	}

	public override StiSqlDataConnector CreateConnector(string connectionString = null)
	{
		return (StiSqlDataConnector)(object)StiSybaseConnector.Get(connectionString);
	}
}
