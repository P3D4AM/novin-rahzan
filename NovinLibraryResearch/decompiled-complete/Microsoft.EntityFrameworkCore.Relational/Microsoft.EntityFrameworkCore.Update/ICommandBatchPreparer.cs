using System.Collections.Generic;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Update;

public interface ICommandBatchPreparer
{
	IEnumerable<ModificationCommandBatch> BatchCommands([JetBrains.Annotations.NotNull] IList<IUpdateEntry> entries, [JetBrains.Annotations.NotNull] IUpdateAdapter updateAdapter);
}
