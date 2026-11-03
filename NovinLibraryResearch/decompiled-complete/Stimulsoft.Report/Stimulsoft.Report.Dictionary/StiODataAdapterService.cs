using System;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Dictionary;

public class StiODataAdapterService : StiSqlAdapterService
{
	public override Type GetDataSourceType()
	{
		return typeof(StiODataSource);
	}

	public override StiSqlDataConnector CreateConnector(string connectionString = null)
	{
		return (StiSqlDataConnector)(object)StiODataConnector.Get(connectionString, (NameValueCollection)null);
	}

	public override void ConnectDataSourceToData(StiDictionary dictionary, StiDataSource dataSource, bool loadData)
	{
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		if (!loadData)
		{
			return;
		}
		StiDataLeader.Disconnect(dataSource);
		StiODataSource oDataSource = dataSource as StiODataSource;
		if (dictionary.Databases.ToList().FirstOrDefault((StiDatabase d) => d.Name == oDataSource.NameInSource) is StiODataDatabase stiODataDatabase)
		{
			StiSqlDataConnector obj = CreateConnector(stiODataDatabase.ConnectionString);
			StiODataConnector val = (StiODataConnector)(object)((obj is StiODataConnector) ? obj : null);
			val.AllowException = stiODataDatabase.AllowException;
			val.Headers.Add(stiODataDatabase.Headers);
			val.Version = stiODataDatabase.Version;
			val.CookieContainer = dictionary.Report.CookieContainer;
			DataTable table = new DataTable(oDataSource.Name);
			oDataSource.Columns.ToList().ForEach(delegate(StiDataColumn c)
			{
				table.Columns.Add(c.Name, c.Type);
			});
			((StiRestDataConnector)val).FillDataTable(table, oDataSource.SqlCommand);
			oDataSource.DataTable = table;
		}
	}
}
