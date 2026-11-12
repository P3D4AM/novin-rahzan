using System.Collections;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public interface ITriggerEventTypeSet : IEnumerable<string>, IEnumerable
{
	bool Contains(string item);
}
