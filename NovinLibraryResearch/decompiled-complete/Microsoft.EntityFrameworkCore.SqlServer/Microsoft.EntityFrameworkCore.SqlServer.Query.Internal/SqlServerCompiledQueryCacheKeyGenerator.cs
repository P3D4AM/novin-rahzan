using System;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;

namespace Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

public class SqlServerCompiledQueryCacheKeyGenerator : RelationalCompiledQueryCacheKeyGenerator
{
	private readonly struct SqlServerCompiledQueryCacheKey
	{
		private readonly RelationalCompiledQueryCacheKey _relationalCompiledQueryCacheKey;

		private readonly bool _useRowNumberOffset;

		public SqlServerCompiledQueryCacheKey(RelationalCompiledQueryCacheKey relationalCompiledQueryCacheKey, bool useRowNumberOffset)
		{
			_relationalCompiledQueryCacheKey = relationalCompiledQueryCacheKey;
			_useRowNumberOffset = useRowNumberOffset;
		}

		public override bool Equals(object obj)
		{
			if (obj != null && obj is SqlServerCompiledQueryCacheKey other)
			{
				return Equals(other);
			}
			return false;
		}

		private bool Equals(SqlServerCompiledQueryCacheKey other)
		{
			if (_relationalCompiledQueryCacheKey.Equals(other._relationalCompiledQueryCacheKey))
			{
				return _useRowNumberOffset == other._useRowNumberOffset;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(_relationalCompiledQueryCacheKey, _useRowNumberOffset);
		}
	}

	public SqlServerCompiledQueryCacheKeyGenerator([JetBrains.Annotations.NotNull] CompiledQueryCacheKeyGeneratorDependencies dependencies, [JetBrains.Annotations.NotNull] RelationalCompiledQueryCacheKeyGeneratorDependencies relationalDependencies)
		: base(dependencies, relationalDependencies)
	{
	}

	public override object GenerateCacheKey(Expression query, bool async)
	{
		return new SqlServerCompiledQueryCacheKey(GenerateCacheKeyCore(query, async), (RelationalDependencies.ContextOptions.FindExtension<SqlServerOptionsExtension>()?.RowNumberPaging).GetValueOrDefault());
	}
}
