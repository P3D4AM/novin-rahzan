using System.Data;

namespace Stimulsoft.Base;

public interface IStiBIDataCache
{
	bool Exists(IStiAppDataSource dataSource);

	bool Exists(string tableKey);

	void Remove(string tableKey);

	void Clean(string appKey);

	void CleanAll();

	long GetTableCount();

	long GetRowCount(string tableKey);

	DataTable GetSchema(string tableKey);

	DataTable GetData(string tableKey);

	DataTable RunQuery(string query);

	void SaveData(string appKey, string tableKey, DataTable dataTable);
}
