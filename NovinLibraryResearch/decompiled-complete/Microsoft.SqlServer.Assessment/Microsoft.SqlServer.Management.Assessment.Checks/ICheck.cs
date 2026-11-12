using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.Assessment.Configuration;
using Microsoft.SqlServer.Management.Assessment.Logics;

namespace Microsoft.SqlServer.Management.Assessment.Checks;

public interface ICheck
{
	string Description { get; set; }

	string DisplayName { get; set; }

	bool Enabled { get; set; }

	string HelpLink { get; set; }

	SeverityLevel Level { get; set; }

	string Id { get; }

	HashSet<string> Tags { get; }

	IPattern<ISqlObjectLocator> Target { get; set; }

	string OriginName { get; }

	Version OriginVersion { get; }

	ILogicsProvider Logics { get; }
}
