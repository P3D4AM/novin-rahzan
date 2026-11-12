using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Update;

public class SingularModificationCommandBatch : AffectedCountModificationCommandBatch
{
	public SingularModificationCommandBatch([JetBrains.Annotations.NotNull] ModificationCommandBatchFactoryDependencies dependencies)
		: base(dependencies)
	{
	}

	protected override bool CanAddCommand(ModificationCommand modificationCommand)
	{
		return ModificationCommands.Count == 0;
	}

	protected override bool IsCommandTextValid()
	{
		return true;
	}
}
