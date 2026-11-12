using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.Update;

public abstract class ModificationCommandBatch
{
	public abstract IReadOnlyList<ModificationCommand> ModificationCommands { get; }

	public abstract bool AddCommand([JetBrains.Annotations.NotNull] ModificationCommand modificationCommand);

	public abstract void Execute([JetBrains.Annotations.NotNull] IRelationalConnection connection);

	public abstract Task ExecuteAsync([JetBrains.Annotations.NotNull] IRelationalConnection connection, CancellationToken cancellationToken = default(CancellationToken));
}
