using System.Collections.Generic;
using System.Threading;

namespace Microsoft.SqlServer.Management.HadrModel;

public abstract class ScenarioProvider<IValidatorProvider, ITasksProvider>
{
	public CancellationTokenSource TaskTokenSource { get; private set; }

	public ScenarioProvider()
	{
		TaskTokenSource = new CancellationTokenSource();
	}

	public abstract List<Validator> Validators();

	public abstract List<HadrTask> Tasks();

	public abstract List<HadrTask> RollbackTasks();
}
