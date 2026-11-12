namespace Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

public interface IChangeTrackerFactory
{
	ChangeTracker Create();
}
