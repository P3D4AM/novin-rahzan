using System.Collections.Generic;

namespace Stimulsoft.Data.Engine;

public interface IStiRetrieval
{
	List<string> RetrieveUsedDataNames(string group);
}
