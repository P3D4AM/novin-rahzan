using System;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Dictionary;

public class StiSqlCeAdapterService : StiSqlAdapterService
{
	public override Type GetDataSourceType()
	{
		return typeof(StiSqlCeSource);
	}

	public override StiSqlDataConnector CreateConnector(string connectionString = null)
	{
		return (StiSqlDataConnector)(object)StiSqlCeConnector.Get(connectionString);
	}
}
