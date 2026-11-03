using System;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Dictionary;

public class StiTeradataAdapterService : StiSqlAdapterService
{
	public override Type GetDataSourceType()
	{
		return typeof(StiTeradataSource);
	}

	public override StiSqlDataConnector CreateConnector(string connectionString = null)
	{
		return (StiSqlDataConnector)(object)StiTeradataConnector.Get(connectionString);
	}
}
