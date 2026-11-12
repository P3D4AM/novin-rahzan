using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.Update;

public interface IBatchExecutor
{
	int Execute([JetBrains.Annotations.NotNull] IEnumerable<ModificationCommandBatch> commandBatches, [JetBrains.Annotations.NotNull] IRelationalConnection connection);

	Task<int> ExecuteAsync([JetBrains.Annotations.NotNull] IEnumerable<ModificationCommandBatch> commandBatches, [JetBrains.Annotations.NotNull] IRelationalConnection connection, CancellationToken cancellationToken = default(CancellationToken));
}
