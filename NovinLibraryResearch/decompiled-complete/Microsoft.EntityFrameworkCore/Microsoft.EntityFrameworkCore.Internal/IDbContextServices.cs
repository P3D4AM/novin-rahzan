using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.Internal;

public interface IDbContextServices
{
	ICurrentDbContext CurrentContext { get; }

	IModel Model { get; }

	IDbContextOptions ContextOptions { get; }

	IServiceProvider InternalServiceProvider { get; }

	IDbContextServices Initialize([JetBrains.Annotations.NotNull] IServiceProvider scopedProvider, [JetBrains.Annotations.NotNull] IDbContextOptions contextOptions, [JetBrains.Annotations.NotNull] DbContext context);
}
