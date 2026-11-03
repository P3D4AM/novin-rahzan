using System;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Dictionary;

public class StiFirebirdAdapterService : StiSqlAdapterService
{
	public override Type GetDataSourceType()
	{
		return typeof(StiFirebirdSource);
	}

	public override StiSqlDataConnector CreateConnector(string connectionString = null)
	{
		return (StiSqlDataConnector)(object)StiFirebirdConnector.Get(connectionString);
	}
}
