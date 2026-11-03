using System;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Dictionary;

public class StiOleDbAdapterService : StiSqlAdapterService
{
	public override Type GetDataSourceType()
	{
		return typeof(StiOleDbSource);
	}

	public override StiSqlDataConnector CreateConnector(string connectionString = null)
	{
		return (StiSqlDataConnector)(object)StiOleDbConnector.Get(connectionString);
	}
}
