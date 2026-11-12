using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.SqlServer.Management.Assessment.Probes;

namespace Microsoft.SqlServer.Management.Assessment.Tools;

internal abstract class DependentTool<TDependency> : ITool where TDependency : class, ITool
{
	private static readonly CharacteristicCollection RequirementsSingleton = new CharacteristicCollection { 
	{
		"channel",
		new HashSet<string> { ToolKey.From(typeof(TDependency)) }
	} };

	protected TDependency BaseTool { get; }

	protected DependentTool(RunSpace runSpace)
	{
		BaseTool = runSpace.GetTool<TDependency>();
	}

	public virtual ICharacteristicCollection GetDependencies(RunSpace runSpace)
	{
		return RequirementsSingleton;
	}

	public Task<bool> IsAvailable(RunSpace runSpace)
	{
		if (!runSpace.Meets(RequirementsSingleton))
		{
			return Task.FromResult(result: false);
		}
		return IsChannelAvailable(runSpace);
	}

	protected abstract Task<bool> IsChannelAvailable(RunSpace runSpace);
}
