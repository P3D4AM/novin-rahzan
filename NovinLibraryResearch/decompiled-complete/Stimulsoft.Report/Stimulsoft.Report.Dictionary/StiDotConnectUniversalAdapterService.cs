using System;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Dictionary;

public class StiDotConnectUniversalAdapterService : StiSqlAdapterService
{
	public override Type GetDataSourceType()
	{
		return typeof(StiDotConnectUniversalSource);
	}

	public override StiSqlDataConnector CreateConnector(string connectionString = null)
	{
		return (StiSqlDataConnector)(object)StiUniversalDevartConnector.Get(connectionString);
	}
}
