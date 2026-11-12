using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Internal;

public interface ISingletonOptionsInitializer
{
	void EnsureInitialized([JetBrains.Annotations.NotNull] IServiceProvider serviceProvider, [JetBrains.Annotations.NotNull] IDbContextOptions options);
}
