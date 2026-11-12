using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SqlServer.Management.Assessment.Probes;

namespace Microsoft.SqlServer.Management.Assessment;

internal class DefaultResultGrouper : IResultGrouper
{
	public void Consolidate(CharacteristicProviderSource providerSource, List<IAssessmentResult> results)
	{
		List<int> list = new List<int>();
		RequirementsWarningBuilder requirementsWarningBuilder = new RequirementsWarningBuilder();
		for (int i = 0; i < results.Count; i++)
		{
			if (results[i] is ProbeRequirementsError probeRequirementsError && probeRequirementsError.InnerExceptions.Any((Exception ex) => ex is ProbeRequirementsException))
			{
				requirementsWarningBuilder.Append(probeRequirementsError);
				list.Add(i);
			}
		}
		if (list.Count <= 0)
		{
			return;
		}
		list.Reverse();
		foreach (int item in list)
		{
			results.RemoveAt(item);
		}
		results.InsertRange(0, requirementsWarningBuilder.ToWarning(providerSource));
	}
}
