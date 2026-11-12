using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Data.Functions;

namespace Stimulsoft.Data.Engine;

public class StiDataSourcePicker
{
	public static IEnumerable<IStiAppDataSource> Fetch(IStiQueryObject query, string group, IEnumerable<string> dataNames, IEnumerable<IStiAppDataSource> dataSources)
	{
		dataNames = dataNames ?? query.RetrieveUsedDataNames(group);
		dataNames = dataNames.Where((string c) => !string.IsNullOrWhiteSpace(c)).Select(Funcs.ToDataName);
		var allColumns = dataSources?.SelectMany((IStiAppDataSource d) => from c in d.FetchColumns()
			select new
			{
				DataSource = d,
				Name = Funcs.ToDataName(c.GetName())
			});
		return dataNames.SelectMany((string dataName) => from pair in allColumns
			where Funcs.IsDataEqual(pair.DataSource, pair.Name, dataName)
			select pair into c
			select c.DataSource).Distinct();
	}
}
