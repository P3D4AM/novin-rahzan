using System;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.Assessment;

internal interface IProbeResult
{
	string Name { get; }

	bool Failed { get; }

	IEnumerable<Exception> Exceptions { get; }

	IDataView Data { get; }

	bool Includes(IProbeResult result);
}
