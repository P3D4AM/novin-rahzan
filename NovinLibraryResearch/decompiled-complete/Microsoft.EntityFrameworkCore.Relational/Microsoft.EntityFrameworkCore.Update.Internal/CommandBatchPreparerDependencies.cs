using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.Update.Internal;

public sealed class CommandBatchPreparerDependencies
{
	public IModificationCommandBatchFactory ModificationCommandBatchFactory { get; }

	public IParameterNameGeneratorFactory ParameterNameGeneratorFactory { get; }

	public IComparer<ModificationCommand> ModificationCommandComparer { get; }

	public IKeyValueIndexFactorySource KeyValueIndexFactorySource { get; }

	public ILoggingOptions LoggingOptions { get; }

	public IDiagnosticsLogger<DbLoggerCategory.Update> UpdateLogger { get; }

	public IDbContextOptions Options { get; }

	[EntityFrameworkInternal]
	public CommandBatchPreparerDependencies([JetBrains.Annotations.NotNull] IModificationCommandBatchFactory modificationCommandBatchFactory, [JetBrains.Annotations.NotNull] IParameterNameGeneratorFactory parameterNameGeneratorFactory, [JetBrains.Annotations.NotNull] IComparer<ModificationCommand> modificationCommandComparer, [JetBrains.Annotations.NotNull] IKeyValueIndexFactorySource keyValueIndexFactorySource, [JetBrains.Annotations.NotNull] ILoggingOptions loggingOptions, [JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Update> updateLogger, [JetBrains.Annotations.NotNull] IDbContextOptions options)
	{
		ModificationCommandBatchFactory = modificationCommandBatchFactory;
		ParameterNameGeneratorFactory = parameterNameGeneratorFactory;
		ModificationCommandComparer = modificationCommandComparer;
		KeyValueIndexFactorySource = keyValueIndexFactorySource;
		LoggingOptions = loggingOptions;
		UpdateLogger = updateLogger;
		Options = options;
	}

	public CommandBatchPreparerDependencies With([JetBrains.Annotations.NotNull] IModificationCommandBatchFactory modificationCommandBatchFactory)
	{
		return new CommandBatchPreparerDependencies(modificationCommandBatchFactory, ParameterNameGeneratorFactory, ModificationCommandComparer, KeyValueIndexFactorySource, LoggingOptions, UpdateLogger, Options);
	}

	public CommandBatchPreparerDependencies With([JetBrains.Annotations.NotNull] IParameterNameGeneratorFactory parameterNameGeneratorFactory)
	{
		return new CommandBatchPreparerDependencies(ModificationCommandBatchFactory, parameterNameGeneratorFactory, ModificationCommandComparer, KeyValueIndexFactorySource, LoggingOptions, UpdateLogger, Options);
	}

	public CommandBatchPreparerDependencies With([JetBrains.Annotations.NotNull] IComparer<ModificationCommand> modificationCommandComparer)
	{
		return new CommandBatchPreparerDependencies(ModificationCommandBatchFactory, ParameterNameGeneratorFactory, modificationCommandComparer, KeyValueIndexFactorySource, LoggingOptions, UpdateLogger, Options);
	}

	public CommandBatchPreparerDependencies With([JetBrains.Annotations.NotNull] IKeyValueIndexFactorySource keyValueIndexFactorySource)
	{
		return new CommandBatchPreparerDependencies(ModificationCommandBatchFactory, ParameterNameGeneratorFactory, ModificationCommandComparer, keyValueIndexFactorySource, LoggingOptions, UpdateLogger, Options);
	}

	public CommandBatchPreparerDependencies With([JetBrains.Annotations.NotNull] ILoggingOptions loggingOptions)
	{
		return new CommandBatchPreparerDependencies(ModificationCommandBatchFactory, ParameterNameGeneratorFactory, ModificationCommandComparer, KeyValueIndexFactorySource, loggingOptions, UpdateLogger, Options);
	}

	public CommandBatchPreparerDependencies With([JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Update> updateLogger)
	{
		return new CommandBatchPreparerDependencies(ModificationCommandBatchFactory, ParameterNameGeneratorFactory, ModificationCommandComparer, KeyValueIndexFactorySource, LoggingOptions, updateLogger, Options);
	}

	public CommandBatchPreparerDependencies With([JetBrains.Annotations.NotNull] IDbContextOptions options)
	{
		return new CommandBatchPreparerDependencies(ModificationCommandBatchFactory, ParameterNameGeneratorFactory, ModificationCommandComparer, KeyValueIndexFactorySource, LoggingOptions, UpdateLogger, options);
	}
}
