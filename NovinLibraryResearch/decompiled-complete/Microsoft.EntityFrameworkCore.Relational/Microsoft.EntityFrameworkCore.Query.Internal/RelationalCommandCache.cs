using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Caching.Memory;

namespace Microsoft.EntityFrameworkCore.Query.Internal;

public class RelationalCommandCache
{
	private readonly struct CommandCacheKey
	{
		private readonly SelectExpression _selectExpression;

		private readonly IReadOnlyDictionary<string, object> _parameterValues;

		public CommandCacheKey(SelectExpression selectExpression, IReadOnlyDictionary<string, object> parameterValues)
		{
			_selectExpression = selectExpression;
			_parameterValues = parameterValues;
		}

		public override bool Equals(object obj)
		{
			if (obj != null && obj is CommandCacheKey commandCacheKey)
			{
				return Equals(commandCacheKey);
			}
			return false;
		}

		private bool Equals(CommandCacheKey commandCacheKey)
		{
			if (_selectExpression != commandCacheKey._selectExpression)
			{
				return false;
			}
			if (_parameterValues.Count > 0)
			{
				foreach (KeyValuePair<string, object> parameterValue in _parameterValues)
				{
					object value = parameterValue.Value;
					if (!commandCacheKey._parameterValues.TryGetValue(parameterValue.Key, out var value2))
					{
						return false;
					}
					if (value == null != (value2 == null))
					{
						return false;
					}
					if (value is IEnumerable && value.GetType() == typeof(object[]))
					{
						return ((object[])value).Length == (value2 as object[])?.Length;
					}
				}
			}
			return true;
		}

		public override int GetHashCode()
		{
			return 0;
		}
	}

	private static readonly ConcurrentDictionary<object, object> _syncObjects = new ConcurrentDictionary<object, object>();

	private readonly IMemoryCache _memoryCache;

	private readonly IQuerySqlGeneratorFactory _querySqlGeneratorFactory;

	private readonly SelectExpression _selectExpression;

	private readonly ParameterValueBasedSelectExpressionOptimizer _parameterValueBasedSelectExpressionOptimizer;

	public RelationalCommandCache(IMemoryCache memoryCache, ISqlExpressionFactory sqlExpressionFactory, IParameterNameGeneratorFactory parameterNameGeneratorFactory, IQuerySqlGeneratorFactory querySqlGeneratorFactory, bool useRelationalNulls, SelectExpression selectExpression)
	{
		_memoryCache = memoryCache;
		_querySqlGeneratorFactory = querySqlGeneratorFactory;
		_selectExpression = selectExpression;
		_parameterValueBasedSelectExpressionOptimizer = new ParameterValueBasedSelectExpressionOptimizer(sqlExpressionFactory, parameterNameGeneratorFactory, useRelationalNulls);
	}

	public virtual IRelationalCommand GetRelationalCommand(IReadOnlyDictionary<string, object> parameters)
	{
		CommandCacheKey commandCacheKey = new CommandCacheKey(_selectExpression, parameters);
		IRelationalCommand value;
		while (!_memoryCache.TryGetValue<IRelationalCommand>(commandCacheKey, out value))
		{
			if (!_syncObjects.TryAdd(commandCacheKey, null))
			{
				continue;
			}
			try
			{
				(SelectExpression selectExpression, bool canCache) tuple = _parameterValueBasedSelectExpressionOptimizer.Optimize(_selectExpression, parameters);
				SelectExpression item = tuple.selectExpression;
				bool item2 = tuple.canCache;
				value = _querySqlGeneratorFactory.Create().GetCommand(item);
				if (item2)
				{
					_memoryCache.Set(commandCacheKey, value, new MemoryCacheEntryOptions
					{
						Size = 10L
					});
				}
			}
			finally
			{
				_syncObjects.TryRemove(commandCacheKey, out var _);
			}
			break;
		}
		return value;
	}
}
