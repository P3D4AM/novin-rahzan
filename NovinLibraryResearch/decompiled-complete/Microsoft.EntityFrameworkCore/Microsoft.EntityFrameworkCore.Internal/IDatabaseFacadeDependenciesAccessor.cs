namespace Microsoft.EntityFrameworkCore.Internal;

public interface IDatabaseFacadeDependenciesAccessor
{
	IDatabaseFacadeDependencies Dependencies { get; }

	DbContext Context { get; }
}
