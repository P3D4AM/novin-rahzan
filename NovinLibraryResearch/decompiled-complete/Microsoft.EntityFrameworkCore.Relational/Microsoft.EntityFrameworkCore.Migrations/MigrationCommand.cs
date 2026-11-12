using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Migrations;

public class MigrationCommand
{
	private readonly IRelationalCommand _relationalCommand;

	private readonly DbContext _context;

	private readonly IDiagnosticsLogger<DbLoggerCategory.Database.Command> _logger;

	public virtual bool TransactionSuppressed { get; }

	public virtual string CommandText => _relationalCommand.CommandText;

	public MigrationCommand([JetBrains.Annotations.NotNull] IRelationalCommand relationalCommand, [JetBrains.Annotations.CanBeNull] DbContext context, [JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Database.Command> logger, bool transactionSuppressed = false)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(relationalCommand, "relationalCommand");
		_relationalCommand = relationalCommand;
		_context = context;
		_logger = logger;
		TransactionSuppressed = transactionSuppressed;
	}

	public virtual int ExecuteNonQuery([JetBrains.Annotations.NotNull] IRelationalConnection connection, [JetBrains.Annotations.CanBeNull] IReadOnlyDictionary<string, object> parameterValues = null)
	{
		return _relationalCommand.ExecuteNonQuery(new RelationalCommandParameterObject(connection, parameterValues, null, _context, _logger));
	}

	public virtual Task<int> ExecuteNonQueryAsync([JetBrains.Annotations.NotNull] IRelationalConnection connection, [JetBrains.Annotations.CanBeNull] IReadOnlyDictionary<string, object> parameterValues = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		return _relationalCommand.ExecuteNonQueryAsync(new RelationalCommandParameterObject(connection, parameterValues, null, _context, _logger), cancellationToken);
	}
}
