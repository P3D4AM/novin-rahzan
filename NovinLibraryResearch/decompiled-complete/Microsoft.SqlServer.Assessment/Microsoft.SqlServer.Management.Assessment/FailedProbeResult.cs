using System;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.Assessment;

internal class FailedProbeResult : IProbeResult
{
	public string Name { get; }

	public bool Failed { get; } = true;


	public IEnumerable<Exception> Exceptions { get; }

	public IDataView Data { get; } = new MeasurementCollection();


	public FailedProbeResult(string name, params Exception[] exceptions)
	{
		Name = name;
		Exceptions = exceptions;
	}

	public bool Includes(IProbeResult result)
	{
		return false;
	}
}
