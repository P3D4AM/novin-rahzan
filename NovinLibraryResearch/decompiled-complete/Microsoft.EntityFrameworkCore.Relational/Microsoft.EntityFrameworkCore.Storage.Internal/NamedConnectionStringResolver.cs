using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Storage.Internal;

public class NamedConnectionStringResolver : NamedConnectionStringResolverBase
{
	private readonly IDbContextOptions _options;

	protected override IServiceProvider ApplicationServiceProvider => _options.FindExtension<CoreOptionsExtension>()?.ApplicationServiceProvider;

	public NamedConnectionStringResolver([JetBrains.Annotations.NotNull] IDbContextOptions options)
	{
		_options = options;
	}
}
