using System;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Dictionary;

public class StiSQLiteAdapterService : StiSqlAdapterService
{
	public override Type GetDataSourceType()
	{
		return typeof(StiSQLiteSource);
	}

	public override StiSqlDataConnector CreateConnector(string connectionString = null)
	{
		return (StiSqlDataConnector)(object)StiSqLiteConnector.Get(connectionString);
	}
}
