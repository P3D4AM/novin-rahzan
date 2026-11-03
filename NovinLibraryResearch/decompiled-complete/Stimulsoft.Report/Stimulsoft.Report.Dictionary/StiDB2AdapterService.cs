using System;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Dictionary;

public class StiDB2AdapterService : StiSqlAdapterService
{
	public override Type GetDataSourceType()
	{
		return typeof(StiDB2Source);
	}

	public override StiSqlDataConnector CreateConnector(string connectionString = null)
	{
		return (StiSqlDataConnector)(object)StiDb2Connector.Get(connectionString);
	}
}
