using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Microsoft.EntityFrameworkCore.SqlServer.ValueGeneration.Internal;

public class SqlServerValueGeneratorSelector : RelationalValueGeneratorSelector
{
	private readonly ISqlServerSequenceValueGeneratorFactory _sequenceFactory;

	private readonly ISqlServerConnection _connection;

	private readonly IRawSqlCommandBuilder _rawSqlCommandBuilder;

	private readonly IDiagnosticsLogger<DbLoggerCategory.Database.Command> _commandLogger;

	public new virtual ISqlServerValueGeneratorCache Cache => (ISqlServerValueGeneratorCache)base.Cache;

	public SqlServerValueGeneratorSelector([JetBrains.Annotations.NotNull] ValueGeneratorSelectorDependencies dependencies, [JetBrains.Annotations.NotNull] ISqlServerSequenceValueGeneratorFactory sequenceFactory, [JetBrains.Annotations.NotNull] ISqlServerConnection connection, [JetBrains.Annotations.NotNull] IRawSqlCommandBuilder rawSqlCommandBuilder, [JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Database.Command> commandLogger)
		: base(dependencies)
	{
		_sequenceFactory = sequenceFactory;
		_connection = connection;
		_rawSqlCommandBuilder = rawSqlCommandBuilder;
		_commandLogger = commandLogger;
	}

	public override ValueGenerator Select(IProperty property, IEntityType entityType)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(property, "property");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType");
		if (property.GetValueGeneratorFactory() != null || property.GetValueGenerationStrategy() != SqlServerValueGenerationStrategy.SequenceHiLo)
		{
			return base.Select(property, entityType);
		}
		return _sequenceFactory.Create(property, Cache.GetOrAddSequenceState(property, _connection), _connection, _rawSqlCommandBuilder, _commandLogger);
	}

	public override ValueGenerator Create(IProperty property, IEntityType entityType)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(property, "property");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType");
		if (!(property.ClrType.UnwrapNullableType() == typeof(Guid)))
		{
			return base.Create(property, entityType);
		}
		if (property.ValueGenerated != 0 && property.GetDefaultValueSql() == null)
		{
			return new SequentialGuidValueGenerator();
		}
		return new TemporaryGuidValueGenerator();
	}
}
