using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.HadrData;

namespace Microsoft.SqlServer.Management.HadrModel;

public class FailoverScenarioProvider : ScenarioProvider<IValidatorProvider, ITasksProvider>
{
	private FailoverData failoverData;

	public FailoverScenarioProvider(FailoverData failoverData)
	{
		if (failoverData == null)
		{
			throw new ArgumentNullException("failoverData");
		}
		this.failoverData = failoverData;
	}

	public override List<Validator> Validators()
	{
		return new List<Validator>
		{
			new FailoverValidator(failoverData),
			new FailoverWaitRoleChangeValidator(failoverData),
			new FailoverQuorumVoteConfigurationValidator(failoverData)
		};
	}

	public override List<HadrTask> RollbackTasks()
	{
		return null;
	}

	public override List<HadrTask> Tasks()
	{
		return new List<HadrTask>
		{
			new FailoverTask(failoverData)
		};
	}
}
