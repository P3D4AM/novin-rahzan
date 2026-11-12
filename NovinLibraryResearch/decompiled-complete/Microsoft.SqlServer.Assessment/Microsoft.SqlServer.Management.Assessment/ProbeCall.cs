using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.SqlServer.Management.Assessment.Probes;
using Microsoft.SqlServer.Management.Assessment.Properties;

namespace Microsoft.SqlServer.Management.Assessment;

internal class ProbeCall
{
	private readonly MeasurementCollection emptyMeasurementCollection = new MeasurementCollection();

	public IDataView Result { get; private set; }

	public bool Failed { get; private set; }

	public string Name { get; }

	public Exception Exception { get; private set; }

	public Dictionary<string, object> Parameters { get; } = new Dictionary<string, object>();


	private IProbe Implementation { get; }

	public ProbeCall(string name, IProbe implementation)
	{
		Name = name;
		Implementation = implementation;
	}

	public async Task Invoke(RunSpace runSpace)
	{
		if (Failed || Implementation == null || !runSpace.Meets(Implementation.Subject))
		{
			Result = emptyMeasurementCollection;
			return;
		}
		if (!runSpace.Meets(Implementation.Requirements) || !runSpace.ProvidesAll(Implementation.ToolTypes))
		{
			Exception = new ProbeRequirementsException(Resources.ErrorProbeRequirements)
			{
				MissingDependencies = runSpace.GetMissingDependencies(Implementation.Requirements, Implementation.ToolTypes)
			};
			Failed = true;
			return;
		}
		try
		{
			Result = await Implementation.GetDataAsync(runSpace, Parameters).ConfigureAwait(continueOnCapturedContext: false);
		}
		catch (Exception ex)
		{
			if (ex is StackOverflowException || ex is OutOfMemoryException)
			{
				throw;
			}
			Exception = ex;
			Failed = true;
		}
	}
}
