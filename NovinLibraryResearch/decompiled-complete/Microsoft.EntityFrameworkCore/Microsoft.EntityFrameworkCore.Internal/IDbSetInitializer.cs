using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Internal;

public interface IDbSetInitializer
{
	void InitializeSets([JetBrains.Annotations.NotNull] DbContext context);
}
