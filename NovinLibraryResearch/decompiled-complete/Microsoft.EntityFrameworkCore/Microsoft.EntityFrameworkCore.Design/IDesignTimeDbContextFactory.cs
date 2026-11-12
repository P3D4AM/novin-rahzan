using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Design;

public interface IDesignTimeDbContextFactory<out TContext> where TContext : DbContext
{
	TContext CreateDbContext([JetBrains.Annotations.NotNull] string[] args);
}
