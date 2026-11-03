using System;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Dictionary;

public class StiMSAccessAdapterService : StiSqlAdapterService
{
	public override Type GetDataSourceType()
	{
		return typeof(StiMSAccessSource);
	}

	public override StiSqlDataConnector CreateConnector(string connectionString = null)
	{
		return (StiSqlDataConnector)(object)StiMsAccessConnector.Get(connectionString);
	}
}
