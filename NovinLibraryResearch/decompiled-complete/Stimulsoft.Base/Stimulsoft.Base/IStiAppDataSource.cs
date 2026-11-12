using System.Collections.Generic;
using System.Data;

namespace Stimulsoft.Base;

public interface IStiAppDataSource : IStiAppCell
{
	string GetNameInSource();

	string GetName();

	DataTable GetDataTable(bool allowConnectToData);

	IStiAppDictionary GetDictionary();

	IEnumerable<IStiAppDataColumn> FetchColumns();

	IStiAppConnection GetConnection();

	IEnumerable<IStiAppDataRelation> FetchParentRelations(bool activePreferred);

	IEnumerable<IStiAppDataRelation> FetchChildRelations(bool activePreferred);

	IEnumerable<object[]> FetchColumnValues(string[] names);
}
