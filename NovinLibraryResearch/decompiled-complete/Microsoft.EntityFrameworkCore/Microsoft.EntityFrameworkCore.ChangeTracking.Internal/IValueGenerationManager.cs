using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

public interface IValueGenerationManager
{
	void Generate([JetBrains.Annotations.NotNull] InternalEntityEntry entry);

	InternalEntityEntry Propagate([JetBrains.Annotations.NotNull] InternalEntityEntry entry);

	Task GenerateAsync([JetBrains.Annotations.NotNull] InternalEntityEntry entry, CancellationToken cancellationToken = default(CancellationToken));

	bool MayGetTemporaryValue([JetBrains.Annotations.NotNull] IProperty property, [JetBrains.Annotations.NotNull] IEntityType entityType);
}
