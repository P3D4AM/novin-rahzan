using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.Internal;

public interface IEntityFinder
{
	object Find([JetBrains.Annotations.CanBeNull] object[] keyValues);

	ValueTask<object> FindAsync([JetBrains.Annotations.CanBeNull] object[] keyValues, CancellationToken cancellationToken = default(CancellationToken));

	void Load([JetBrains.Annotations.NotNull] INavigation navigation, [JetBrains.Annotations.NotNull] InternalEntityEntry entry);

	Task LoadAsync([JetBrains.Annotations.NotNull] INavigation navigation, [JetBrains.Annotations.NotNull] InternalEntityEntry entry, CancellationToken cancellationToken = default(CancellationToken));

	IQueryable Query([JetBrains.Annotations.NotNull] INavigation navigation, [JetBrains.Annotations.NotNull] InternalEntityEntry entry);

	object[] GetDatabaseValues([JetBrains.Annotations.NotNull] InternalEntityEntry entry);

	Task<object[]> GetDatabaseValuesAsync([JetBrains.Annotations.NotNull] InternalEntityEntry entry, CancellationToken cancellationToken = default(CancellationToken));
}
public interface IEntityFinder<TEntity> : IEntityFinder where TEntity : class
{
	new TEntity Find([JetBrains.Annotations.CanBeNull] object[] keyValues);

	new ValueTask<TEntity> FindAsync([JetBrains.Annotations.CanBeNull] object[] keyValues, CancellationToken cancellationToken = default(CancellationToken));

	new IQueryable<TEntity> Query([JetBrains.Annotations.NotNull] INavigation navigation, [JetBrains.Annotations.NotNull] InternalEntityEntry entry);
}
