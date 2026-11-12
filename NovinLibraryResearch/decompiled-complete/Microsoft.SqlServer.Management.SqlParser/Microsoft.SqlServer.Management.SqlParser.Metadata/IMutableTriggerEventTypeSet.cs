using System.Collections;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public interface IMutableTriggerEventTypeSet : ITriggerEventTypeSet, IEnumerable<string>, IEnumerable
{
	void Add(string item);

	bool Remove(string item);

	void Clear();
}
