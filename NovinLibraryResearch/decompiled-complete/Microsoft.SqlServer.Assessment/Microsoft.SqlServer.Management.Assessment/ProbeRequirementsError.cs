using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.Assessment.Checks;
using Microsoft.SqlServer.Management.Assessment.Probes;

namespace Microsoft.SqlServer.Management.Assessment;

internal class ProbeRequirementsError : ProbeError
{
	public CharacteristicCollection MissingDependencies { get; set; }

	public ProbeRequirementsError(ICheck check, string message, SqlObjectType targetType, string targetPath)
		: base(check, message, targetType, targetPath)
	{
	}

	public ProbeRequirementsError(ICheck check, string message, IEnumerable<KeyValuePair<string, Exception>> innerExceptions, SqlObjectType targetType, string targetPath)
		: base(check, message, innerExceptions, targetType, targetPath)
	{
	}
}
