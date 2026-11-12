using System.Collections.Generic;

namespace Stimulsoft.Base;

public interface IStiAppDataRelation : IStiAppCell
{
	string GetName();

	IStiAppDictionary GetDictionary();

	IStiAppDataSource GetParentDataSource();

	IStiAppDataSource GetChildDataSource();

	IEnumerable<string> FetchParentColumns();

	IEnumerable<string> FetchChildColumns();

	bool GetActiveState();
}
