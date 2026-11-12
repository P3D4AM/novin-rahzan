using System;
using System.Data.Common;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Storage;

public class RelationalDataReader : IDisposable, IAsyncDisposable
{
	private readonly IRelationalConnection _connection;

	private readonly DbCommand _command;

	private readonly DbDataReader _reader;

	private readonly Guid _commandId;

	private readonly IDiagnosticsLogger<DbLoggerCategory.Database.Command> _logger;

	private readonly DateTimeOffset _startTime;

	private readonly Stopwatch _stopwatch;

	private int _readCount;

	private bool _disposed;

	public virtual DbDataReader DbDataReader => _reader;

	public virtual DbCommand DbCommand => _command;

	public RelationalDataReader([JetBrains.Annotations.NotNull] IRelationalConnection connection, [JetBrains.Annotations.NotNull] DbCommand command, [JetBrains.Annotations.NotNull] DbDataReader reader, Guid commandId, [JetBrains.Annotations.CanBeNull] IDiagnosticsLogger<DbLoggerCategory.Database.Command> logger)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(connection, "connection");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(command, "command");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(reader, "reader");
		_connection = connection;
		_command = command;
		_reader = reader;
		_commandId = commandId;
		_logger = logger;
		_startTime = DateTimeOffset.UtcNow;
		_stopwatch = Stopwatch.StartNew();
	}

	public virtual bool Read()
	{
		_readCount++;
		return _reader.Read();
	}

	public virtual Task<bool> ReadAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		_readCount++;
		return _reader.ReadAsync(cancellationToken);
	}

	public virtual void Dispose()
	{
		if (_disposed)
		{
			return;
		}
		InterceptionResult interceptionResult = default(InterceptionResult);
		try
		{
			_reader.Close();
			if (_logger != null)
			{
				interceptionResult = _logger.DataReaderDisposing(_connection, _command, _reader, _commandId, _reader.RecordsAffected, _readCount, _startTime, _stopwatch.Elapsed);
			}
		}
		finally
		{
			_disposed = true;
			if (!interceptionResult.IsSuppressed)
			{
				_reader.Dispose();
				_command.Parameters.Clear();
				_command.Dispose();
				_connection.Close();
			}
		}
	}

	public virtual async ValueTask DisposeAsync()
	{
		if (_disposed)
		{
			return;
		}
		InterceptionResult interceptionResult = default(InterceptionResult);
		try
		{
			_reader.Close();
			if (_logger != null)
			{
				interceptionResult = _logger.DataReaderDisposing(_connection, _command, _reader, _commandId, _reader.RecordsAffected, _readCount, _startTime, _stopwatch.Elapsed);
			}
		}
		finally
		{
			_disposed = true;
			if (!interceptionResult.IsSuppressed)
			{
				await _reader.DisposeAsyncIfAvailable();
				_command.Parameters.Clear();
				await _command.DisposeAsyncIfAvailable();
				await _connection.CloseAsync();
			}
		}
	}
}
