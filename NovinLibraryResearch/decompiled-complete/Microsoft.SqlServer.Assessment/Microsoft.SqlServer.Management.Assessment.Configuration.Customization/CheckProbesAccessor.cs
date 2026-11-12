using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.Assessment.Checks;

namespace Microsoft.SqlServer.Management.Assessment.Configuration.Customization;

internal sealed class CheckProbesAccessor : IChangeAccessor<Check, List<ProbeReference>>
{
	public static CheckProbesAccessor Instance { get; } = new CheckProbesAccessor();


	private CheckProbesAccessor()
	{
	}

	public void SetValue(Check target, List<ProbeReference> newValue)
	{
		if (target == null)
		{
			throw new ArgumentNullException("target");
		}
		target.Probes.Clear();
		if (newValue != null && newValue.Count > 0)
		{
			target.Probes.AddRange(newValue);
		}
	}

	public List<ProbeReference> GetValue(Check target)
	{
		if (target == null)
		{
			throw new ArgumentNullException("target");
		}
		return target.Probes;
	}
}
