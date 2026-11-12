using System.Collections.Generic;

namespace Stimulsoft.Base;

public interface IStiAppDictionary
{
	IEnumerable<IStiAppDataSource> FetchDataSources();

	IEnumerable<IStiAppDataRelation> FetchDataRelations();

	IEnumerable<IStiAppVariable> FetchVariables();

	IStiAppDataSource GetDataSourceByName(string name);

	IStiAppDataColumn GetColumnByName(string name);

	IStiAppVariable GetVariableByName(string name);

	object GetVariableValueByName(string name);

	bool IsSystemVariable(string name);

	bool IsReadOnlyVariable(string name);

	object GetSystemVariableValue(string name);

	IStiApp GetApp();

	IEnumerable<IStiAppConnection> OpenConnections(IEnumerable<IStiAppConnection> connections);

	void CloseConnections(IEnumerable<IStiAppConnection> connections);
}
