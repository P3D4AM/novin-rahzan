using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.Assessment.Checks;

namespace Microsoft.SqlServer.Management.Assessment.Configuration;

public interface IRuleset
{
	string Name { get; set; }

	Version Version { get; set; }

	void GetSuspects(ISqlObjectLocator target, HashSet<string> ids, HashSet<string> tags);

	IEnumerable<ICheck> GetChecks(ISqlObjectLocator target, IEnumerable<ICheck> baseChecks, HashSet<string> ids, HashSet<string> tags);
}
