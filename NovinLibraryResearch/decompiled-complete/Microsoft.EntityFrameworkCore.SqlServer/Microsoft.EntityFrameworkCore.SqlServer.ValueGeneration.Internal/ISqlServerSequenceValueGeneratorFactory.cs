using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Microsoft.EntityFrameworkCore.SqlServer.ValueGeneration.Internal;

public interface ISqlServerSequenceValueGeneratorFactory
{
	ValueGenerator Create([JetBrains.Annotations.NotNull] IProperty property, [JetBrains.Annotations.NotNull] SqlServerSequenceValueGeneratorState generatorState, [JetBrains.Annotations.NotNull] ISqlServerConnection connection, [JetBrains.Annotations.NotNull] IRawSqlCommandBuilder rawSqlCommandBuilder, [JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Database.Command> commandLogger);
}
