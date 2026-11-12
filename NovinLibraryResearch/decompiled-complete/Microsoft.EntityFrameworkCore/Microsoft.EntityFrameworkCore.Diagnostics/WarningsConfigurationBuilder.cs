using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Utilities;
using Microsoft.Extensions.Logging;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public class WarningsConfigurationBuilder
{
	private readonly DbContextOptionsBuilder _optionsBuilder;

	public WarningsConfigurationBuilder([JetBrains.Annotations.NotNull] DbContextOptionsBuilder optionsBuilder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(optionsBuilder, "optionsBuilder");
		_optionsBuilder = optionsBuilder;
	}

	public virtual WarningsConfigurationBuilder Default(WarningBehavior warningBehavior)
	{
		return WithOption((WarningsConfiguration e) => e.WithDefaultBehavior(warningBehavior));
	}

	public virtual WarningsConfigurationBuilder Throw([JetBrains.Annotations.NotNull] params EventId[] eventIds)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(eventIds, "eventIds");
		return WithOption((WarningsConfiguration e) => e.WithExplicit(eventIds, WarningBehavior.Throw));
	}

	public virtual WarningsConfigurationBuilder Log([JetBrains.Annotations.NotNull] params EventId[] eventIds)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(eventIds, "eventIds");
		return WithOption((WarningsConfiguration e) => e.WithExplicit(eventIds, WarningBehavior.Log));
	}

	public virtual WarningsConfigurationBuilder Log([JetBrains.Annotations.NotNull] params (EventId Id, LogLevel Level)[] eventsAndLevels)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(eventsAndLevels, "eventsAndLevels");
		return WithOption((WarningsConfiguration e) => e.WithExplicit(eventsAndLevels));
	}

	public virtual WarningsConfigurationBuilder Ignore([JetBrains.Annotations.NotNull] params EventId[] eventIds)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(eventIds, "eventIds");
		return WithOption((WarningsConfiguration e) => e.WithExplicit(eventIds, WarningBehavior.Ignore));
	}

	private WarningsConfigurationBuilder WithOption(Func<WarningsConfiguration, WarningsConfiguration> withFunc)
	{
		CoreOptionsExtension coreOptionsExtension = _optionsBuilder.Options.FindExtension<CoreOptionsExtension>() ?? new CoreOptionsExtension();
		((IDbContextOptionsBuilderInfrastructure)_optionsBuilder).AddOrUpdateExtension(coreOptionsExtension.WithWarningsConfiguration(withFunc(coreOptionsExtension.WarningsConfiguration)));
		return this;
	}
}
