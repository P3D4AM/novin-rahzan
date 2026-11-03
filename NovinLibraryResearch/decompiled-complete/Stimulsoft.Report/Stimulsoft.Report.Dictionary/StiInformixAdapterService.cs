using System;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Dictionary;

public class StiInformixAdapterService : StiSqlAdapterService
{
	public override Type GetDataSourceType()
	{
		return typeof(StiInformixSource);
	}

	public override StiSqlDataConnector CreateConnector(string connectionString = null)
	{
		return (StiSqlDataConnector)(object)StiInformixConnector.Get(connectionString);
	}
}
