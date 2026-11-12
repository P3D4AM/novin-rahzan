using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.Update.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Microsoft.EntityFrameworkCore.SqlServer.ValueGeneration.Internal;

public class SqlServerSequenceValueGeneratorFactory : ISqlServerSequenceValueGeneratorFactory
{
	private readonly ISqlServerUpdateSqlGenerator _sqlGenerator;

	public SqlServerSequenceValueGeneratorFactory([JetBrains.Annotations.NotNull] ISqlServerUpdateSqlGenerator sqlGenerator)
	{
		_sqlGenerator = sqlGenerator;
	}

	public virtual ValueGenerator Create(IProperty property, SqlServerSequenceValueGeneratorState generatorState, ISqlServerConnection connection, IRawSqlCommandBuilder rawSqlCommandBuilder, IDiagnosticsLogger<DbLoggerCategory.Database.Command> commandLogger)
	{
		Type type = property.ClrType.UnwrapNullableType().UnwrapEnumType();
		if (type == typeof(long))
		{
			return new SqlServerSequenceHiLoValueGenerator<long>(rawSqlCommandBuilder, _sqlGenerator, generatorState, connection, commandLogger);
		}
		if (type == typeof(int))
		{
			return new SqlServerSequenceHiLoValueGenerator<int>(rawSqlCommandBuilder, _sqlGenerator, generatorState, connection, commandLogger);
		}
		if (type == typeof(decimal))
		{
			return new SqlServerSequenceHiLoValueGenerator<decimal>(rawSqlCommandBuilder, _sqlGenerator, generatorState, connection, commandLogger);
		}
		if (type == typeof(short))
		{
			return new SqlServerSequenceHiLoValueGenerator<short>(rawSqlCommandBuilder, _sqlGenerator, generatorState, connection, commandLogger);
		}
		if (type == typeof(byte))
		{
			return new SqlServerSequenceHiLoValueGenerator<byte>(rawSqlCommandBuilder, _sqlGenerator, generatorState, connection, commandLogger);
		}
		if (type == typeof(char))
		{
			return new SqlServerSequenceHiLoValueGenerator<char>(rawSqlCommandBuilder, _sqlGenerator, generatorState, connection, commandLogger);
		}
		if (type == typeof(ulong))
		{
			return new SqlServerSequenceHiLoValueGenerator<ulong>(rawSqlCommandBuilder, _sqlGenerator, generatorState, connection, commandLogger);
		}
		if (type == typeof(uint))
		{
			return new SqlServerSequenceHiLoValueGenerator<uint>(rawSqlCommandBuilder, _sqlGenerator, generatorState, connection, commandLogger);
		}
		if (type == typeof(ushort))
		{
			return new SqlServerSequenceHiLoValueGenerator<ushort>(rawSqlCommandBuilder, _sqlGenerator, generatorState, connection, commandLogger);
		}
		if (type == typeof(sbyte))
		{
			return new SqlServerSequenceHiLoValueGenerator<sbyte>(rawSqlCommandBuilder, _sqlGenerator, generatorState, connection, commandLogger);
		}
		throw new ArgumentException(CoreStrings.InvalidValueGeneratorFactoryProperty("SqlServerSequenceValueGeneratorFactory", property.Name, property.DeclaringEntityType.DisplayName()));
	}
}
