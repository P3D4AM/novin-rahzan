using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.Update.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Microsoft.EntityFrameworkCore.SqlServer.ValueGeneration.Internal;

public class SqlServerSequenceHiLoValueGenerator<TValue> : HiLoValueGenerator<TValue>
{
	private readonly IRawSqlCommandBuilder _rawSqlCommandBuilder;

	private readonly ISqlServerUpdateSqlGenerator _sqlGenerator;

	private readonly ISqlServerConnection _connection;

	private readonly ISequence _sequence;

	private readonly IDiagnosticsLogger<DbLoggerCategory.Database.Command> _commandLogger;

	public override bool GeneratesTemporaryValues => false;

	public SqlServerSequenceHiLoValueGenerator([JetBrains.Annotations.NotNull] IRawSqlCommandBuilder rawSqlCommandBuilder, [JetBrains.Annotations.NotNull] ISqlServerUpdateSqlGenerator sqlGenerator, [JetBrains.Annotations.NotNull] SqlServerSequenceValueGeneratorState generatorState, [JetBrains.Annotations.NotNull] ISqlServerConnection connection, [JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Database.Command> commandLogger)
		: base((HiLoValueGeneratorState)generatorState)
	{
		_sequence = generatorState.Sequence;
		_rawSqlCommandBuilder = rawSqlCommandBuilder;
		_sqlGenerator = sqlGenerator;
		_connection = connection;
		_commandLogger = commandLogger;
	}

	protected override long GetNewLowValue()
	{
		return (long)Convert.ChangeType(_rawSqlCommandBuilder.Build(_sqlGenerator.GenerateNextSequenceValueOperation(_sequence.Name, _sequence.Schema)).ExecuteScalar(new RelationalCommandParameterObject(_connection, null, null, null, _commandLogger)), typeof(long), CultureInfo.InvariantCulture);
	}

	protected override async Task<long> GetNewLowValueAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		return (long)Convert.ChangeType(await _rawSqlCommandBuilder.Build(_sqlGenerator.GenerateNextSequenceValueOperation(_sequence.Name, _sequence.Schema)).ExecuteScalarAsync(new RelationalCommandParameterObject(_connection, null, null, null, _commandLogger), cancellationToken), typeof(long), CultureInfo.InvariantCulture);
	}
}
