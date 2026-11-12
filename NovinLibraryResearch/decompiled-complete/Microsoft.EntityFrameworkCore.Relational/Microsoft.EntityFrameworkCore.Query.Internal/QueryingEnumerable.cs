using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.Query.Internal;

public class QueryingEnumerable<T> : IEnumerable<T>, IEnumerable, IAsyncEnumerable<T>
{
	private sealed class Enumerator : IEnumerator<T>, IEnumerator, IDisposable
	{
		private readonly RelationalQueryContext _relationalQueryContext;

		private readonly RelationalCommandCache _relationalCommandCache;

		private readonly IReadOnlyList<string> _columnNames;

		private readonly IReadOnlyList<ReaderColumn> _readerColumns;

		private readonly Func<QueryContext, DbDataReader, ResultContext, int[], ResultCoordinator, T> _shaper;

		private readonly Type _contextType;

		private readonly IDiagnosticsLogger<DbLoggerCategory.Query> _logger;

		private RelationalDataReader _dataReader;

		private int[] _indexMap;

		private ResultCoordinator _resultCoordinator;

		private IExecutionStrategy _executionStrategy;

		public T Current { get; private set; }

		object IEnumerator.Current => Current;

		public Enumerator(QueryingEnumerable<T> queryingEnumerable)
		{
			_relationalQueryContext = queryingEnumerable._relationalQueryContext;
			_relationalCommandCache = queryingEnumerable._relationalCommandCache;
			_columnNames = queryingEnumerable._columnNames;
			_readerColumns = queryingEnumerable._readerColumns;
			_shaper = queryingEnumerable._shaper;
			_contextType = queryingEnumerable._contextType;
			_logger = queryingEnumerable._logger;
		}

		public bool MoveNext()
		{
			try
			{
				using (_relationalQueryContext.ConcurrencyDetector.EnterCriticalSection())
				{
					if (_dataReader == null)
					{
						if (_executionStrategy == null)
						{
							_executionStrategy = _relationalQueryContext.ExecutionStrategyFactory.Create();
						}
						_executionStrategy.Execute(state: true, InitializeReader, null);
					}
					bool flag = _resultCoordinator.HasNext ?? _dataReader.Read();
					Current = default(T);
					if (flag)
					{
						while (true)
						{
							_resultCoordinator.ResultReady = true;
							_resultCoordinator.HasNext = null;
							Current = _shaper(_relationalQueryContext, _dataReader.DbDataReader, _resultCoordinator.ResultContext, _indexMap, _resultCoordinator);
							if (_resultCoordinator.ResultReady)
							{
								_resultCoordinator.ResultContext.Values = null;
								break;
							}
							if (!_dataReader.Read())
							{
								_resultCoordinator.HasNext = false;
								_resultCoordinator.ResultReady = true;
								Current = _shaper(_relationalQueryContext, _dataReader.DbDataReader, _resultCoordinator.ResultContext, _indexMap, _resultCoordinator);
								break;
							}
						}
					}
					return flag;
				}
			}
			catch (Exception exception)
			{
				_logger.QueryIterationFailed(_contextType, exception);
				throw;
			}
		}

		private bool InitializeReader(DbContext _, bool result)
		{
			IRelationalCommand relationalCommand = _relationalCommandCache.GetRelationalCommand(_relationalQueryContext.ParameterValues);
			_dataReader = relationalCommand.ExecuteReader(new RelationalCommandParameterObject(_relationalQueryContext.Connection, _relationalQueryContext.ParameterValues, _readerColumns, _relationalQueryContext.Context, _relationalQueryContext.CommandLogger));
			_indexMap = QueryingEnumerable<T>.BuildIndexMap(_columnNames, _dataReader.DbDataReader);
			_resultCoordinator = new ResultCoordinator();
			return result;
		}

		public void Dispose()
		{
			_dataReader?.Dispose();
			_dataReader = null;
		}

		public void Reset()
		{
			throw new NotImplementedException();
		}
	}

	private sealed class AsyncEnumerator : IAsyncEnumerator<T>, IAsyncDisposable
	{
		private readonly RelationalQueryContext _relationalQueryContext;

		private readonly RelationalCommandCache _relationalCommandCache;

		private readonly IReadOnlyList<string> _columnNames;

		private readonly IReadOnlyList<ReaderColumn> _readerColumns;

		private readonly Func<QueryContext, DbDataReader, ResultContext, int[], ResultCoordinator, T> _shaper;

		private readonly Type _contextType;

		private readonly IDiagnosticsLogger<DbLoggerCategory.Query> _logger;

		private readonly CancellationToken _cancellationToken;

		private RelationalDataReader _dataReader;

		private int[] _indexMap;

		private ResultCoordinator _resultCoordinator;

		private IExecutionStrategy _executionStrategy;

		public T Current { get; private set; }

		public AsyncEnumerator(QueryingEnumerable<T> queryingEnumerable, CancellationToken cancellationToken)
		{
			_relationalQueryContext = queryingEnumerable._relationalQueryContext;
			_relationalCommandCache = queryingEnumerable._relationalCommandCache;
			_columnNames = queryingEnumerable._columnNames;
			_readerColumns = queryingEnumerable._readerColumns;
			_shaper = queryingEnumerable._shaper;
			_contextType = queryingEnumerable._contextType;
			_logger = queryingEnumerable._logger;
			_cancellationToken = cancellationToken;
		}

		public async ValueTask<bool> MoveNextAsync()
		{
			_ = 2;
			try
			{
				using (_relationalQueryContext.ConcurrencyDetector.EnterCriticalSection())
				{
					if (_dataReader == null)
					{
						if (_executionStrategy == null)
						{
							_executionStrategy = _relationalQueryContext.ExecutionStrategyFactory.Create();
						}
						await _executionStrategy.ExecuteAsync(state: true, InitializeReaderAsync, null, _cancellationToken);
					}
					bool flag = _resultCoordinator.HasNext ?? (await _dataReader.ReadAsync(_cancellationToken));
					bool hasNext = flag;
					Current = default(T);
					if (hasNext)
					{
						while (true)
						{
							_resultCoordinator.ResultReady = true;
							_resultCoordinator.HasNext = null;
							Current = _shaper(_relationalQueryContext, _dataReader.DbDataReader, _resultCoordinator.ResultContext, _indexMap, _resultCoordinator);
							if (_resultCoordinator.ResultReady)
							{
								_resultCoordinator.ResultContext.Values = null;
								break;
							}
							if (!(await _dataReader.ReadAsync(_cancellationToken)))
							{
								_resultCoordinator.HasNext = false;
								_resultCoordinator.ResultReady = true;
								Current = _shaper(_relationalQueryContext, _dataReader.DbDataReader, _resultCoordinator.ResultContext, _indexMap, _resultCoordinator);
								break;
							}
						}
					}
					return hasNext;
				}
			}
			catch (Exception exception)
			{
				_logger.QueryIterationFailed(_contextType, exception);
				throw;
			}
		}

		private async Task<bool> InitializeReaderAsync(DbContext _, bool result, CancellationToken cancellationToken)
		{
			_dataReader = await _relationalCommandCache.GetRelationalCommand(_relationalQueryContext.ParameterValues).ExecuteReaderAsync(new RelationalCommandParameterObject(_relationalQueryContext.Connection, _relationalQueryContext.ParameterValues, _readerColumns, _relationalQueryContext.Context, _relationalQueryContext.CommandLogger), cancellationToken);
			_indexMap = QueryingEnumerable<T>.BuildIndexMap(_columnNames, _dataReader.DbDataReader);
			_resultCoordinator = new ResultCoordinator();
			return result;
		}

		public ValueTask DisposeAsync()
		{
			if (_dataReader != null)
			{
				RelationalDataReader dataReader = _dataReader;
				_dataReader = null;
				return dataReader.DisposeAsync();
			}
			return default(ValueTask);
		}
	}

	private readonly RelationalQueryContext _relationalQueryContext;

	private readonly RelationalCommandCache _relationalCommandCache;

	private readonly IReadOnlyList<string> _columnNames;

	private readonly IReadOnlyList<ReaderColumn> _readerColumns;

	private readonly Func<QueryContext, DbDataReader, ResultContext, int[], ResultCoordinator, T> _shaper;

	private readonly Type _contextType;

	private readonly IDiagnosticsLogger<DbLoggerCategory.Query> _logger;

	public QueryingEnumerable(RelationalQueryContext relationalQueryContext, RelationalCommandCache relationalCommandCache, IReadOnlyList<string> columnNames, IReadOnlyList<ReaderColumn> readerColumns, Func<QueryContext, DbDataReader, ResultContext, int[], ResultCoordinator, T> shaper, Type contextType, IDiagnosticsLogger<DbLoggerCategory.Query> logger)
	{
		_relationalQueryContext = relationalQueryContext;
		_relationalCommandCache = relationalCommandCache;
		_columnNames = columnNames;
		_readerColumns = readerColumns;
		_shaper = shaper;
		_contextType = contextType;
		_logger = logger ?? relationalQueryContext.QueryLogger;
	}

	public virtual IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
	{
		return new AsyncEnumerator(this, cancellationToken);
	}

	public virtual IEnumerator<T> GetEnumerator()
	{
		return new Enumerator(this);
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	public static int[] BuildIndexMap(IReadOnlyList<string> columnNames, DbDataReader dataReader)
	{
		if (columnNames == null)
		{
			return null;
		}
		Dictionary<string, int> dictionary = Enumerable.Range(0, dataReader.FieldCount).ToDictionary<int, string, int>(dataReader.GetName, (int i) => i, StringComparer.OrdinalIgnoreCase);
		int[] array = new int[columnNames.Count];
		for (int j = 0; j < columnNames.Count; j++)
		{
			string text = columnNames[j];
			if (!dictionary.TryGetValue(text, out var value))
			{
				throw new InvalidOperationException(RelationalStrings.FromSqlMissingColumn(text));
			}
			array[j] = value;
		}
		return array;
	}
}
