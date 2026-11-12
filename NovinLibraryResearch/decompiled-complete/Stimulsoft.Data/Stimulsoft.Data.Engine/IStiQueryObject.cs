using System.Collections.Generic;
using Stimulsoft.Base;

namespace Stimulsoft.Data.Engine;

public interface IStiQueryObject : IStiRetrieval
{
	IStiAppDictionary GetDictionary();

	IEnumerable<IStiAppDataSource> GetDataSources(IEnumerable<string> dataNames);
}
