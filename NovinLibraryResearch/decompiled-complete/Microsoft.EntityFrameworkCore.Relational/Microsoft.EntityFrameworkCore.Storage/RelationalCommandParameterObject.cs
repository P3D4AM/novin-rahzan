using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Storage;

public readonly struct RelationalCommandParameterObject
{
	public IRelationalConnection Connection { get; }

	public IReadOnlyDictionary<string, object> ParameterValues { get; }

	public IReadOnlyList<ReaderColumn> ReaderColumns { get; }

	public DbContext Context { get; }

	public IDiagnosticsLogger<DbLoggerCategory.Database.Command> Logger { get; }

	[Obsolete("Use the overload with the readerColumns parameter")]
	public RelationalCommandParameterObject([JetBrains.Annotations.NotNull] IRelationalConnection connection, [JetBrains.Annotations.CanBeNull] IReadOnlyDictionary<string, object> parameterValues, [JetBrains.Annotations.CanBeNull] DbContext context, [JetBrains.Annotations.CanBeNull] IDiagnosticsLogger<DbLoggerCategory.Database.Command> logger)
		: this(connection, parameterValues, null, context, logger)
	{
	}

	public RelationalCommandParameterObject([JetBrains.Annotations.NotNull] IRelationalConnection connection, [JetBrains.Annotations.CanBeNull] IReadOnlyDictionary<string, object> parameterValues, [JetBrains.Annotations.CanBeNull] IReadOnlyList<ReaderColumn> readerColumns, [JetBrains.Annotations.CanBeNull] DbContext context, [JetBrains.Annotations.CanBeNull] IDiagnosticsLogger<DbLoggerCategory.Database.Command> logger)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(connection, "connection");
		Connection = connection;
		ParameterValues = parameterValues;
		ReaderColumns = readerColumns;
		Context = context;
		Logger = logger;
	}
}
