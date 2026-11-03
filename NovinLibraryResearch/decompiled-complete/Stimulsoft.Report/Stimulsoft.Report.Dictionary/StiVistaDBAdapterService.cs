using System;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Dictionary;

public class StiVistaDBAdapterService : StiSqlAdapterService
{
	public override Type GetDataSourceType()
	{
		return typeof(StiVistaDBSource);
	}

	public override StiSqlDataConnector CreateConnector(string connectionString = null)
	{
		return (StiSqlDataConnector)(object)StiVistaDbConnector.Get(connectionString);
	}
}
