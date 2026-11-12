using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Microsoft.EntityFrameworkCore.Internal;

public class DbContextPoolConfigurationSnapshot
{
	public virtual bool? AutoDetectChangesEnabled { get; }

	public virtual bool? LazyLoadingEnabled { get; }

	public virtual CascadeTiming? CascadeDeleteTiming { get; }

	public virtual CascadeTiming? DeleteOrphansTiming { get; }

	public virtual QueryTrackingBehavior? QueryTrackingBehavior { get; }

	public virtual bool? AutoTransactionsEnabled { get; }

	public DbContextPoolConfigurationSnapshot(bool? autoDetectChangesEnabled, QueryTrackingBehavior? queryTrackingBehavior, bool? autoTransactionsEnabled, bool? lazyLoadingEnabled, CascadeTiming? cascadeDeleteTiming, CascadeTiming? deleteOrphansTiming)
	{
		AutoDetectChangesEnabled = autoDetectChangesEnabled;
		QueryTrackingBehavior = queryTrackingBehavior;
		AutoTransactionsEnabled = autoTransactionsEnabled;
		LazyLoadingEnabled = lazyLoadingEnabled;
		CascadeDeleteTiming = cascadeDeleteTiming;
		DeleteOrphansTiming = deleteOrphansTiming;
	}
}
