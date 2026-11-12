using System.Collections.Generic;

namespace Microsoft.EntityFrameworkCore.Infrastructure;

public interface IDbContextOptions
{
	IEnumerable<IDbContextOptionsExtension> Extensions { get; }

	TExtension FindExtension<TExtension>() where TExtension : class, IDbContextOptionsExtension;
}
