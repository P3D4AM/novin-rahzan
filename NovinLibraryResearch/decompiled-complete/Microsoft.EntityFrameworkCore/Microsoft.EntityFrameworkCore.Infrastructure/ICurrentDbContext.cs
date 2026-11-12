namespace Microsoft.EntityFrameworkCore.Infrastructure;

public interface ICurrentDbContext
{
	DbContext Context { get; }
}
