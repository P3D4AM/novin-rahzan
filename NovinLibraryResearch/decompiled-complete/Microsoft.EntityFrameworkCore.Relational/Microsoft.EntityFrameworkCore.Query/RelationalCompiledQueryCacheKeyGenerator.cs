using System;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Query;

public class RelationalCompiledQueryCacheKeyGenerator : CompiledQueryCacheKeyGenerator
{
	protected readonly struct RelationalCompiledQueryCacheKey
	{
		private readonly CompiledQueryCacheKey _compiledQueryCacheKey;

		private readonly bool _useRelationalNulls;

		private readonly bool _shouldBuffer;

		public RelationalCompiledQueryCacheKey(CompiledQueryCacheKey compiledQueryCacheKey, bool useRelationalNulls, bool shouldBuffer)
		{
			_compiledQueryCacheKey = compiledQueryCacheKey;
			_useRelationalNulls = useRelationalNulls;
			_shouldBuffer = shouldBuffer;
		}

		public override bool Equals(object obj)
		{
			if (obj != null && obj is RelationalCompiledQueryCacheKey other)
			{
				return Equals(other);
			}
			return false;
		}

		private bool Equals(RelationalCompiledQueryCacheKey other)
		{
			if (_compiledQueryCacheKey.Equals(other._compiledQueryCacheKey) && _useRelationalNulls == other._useRelationalNulls)
			{
				return _shouldBuffer == other._shouldBuffer;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(_compiledQueryCacheKey, _useRelationalNulls, _shouldBuffer);
		}
	}

	protected virtual RelationalCompiledQueryCacheKeyGeneratorDependencies RelationalDependencies { get; }

	public RelationalCompiledQueryCacheKeyGenerator([JetBrains.Annotations.NotNull] CompiledQueryCacheKeyGeneratorDependencies dependencies, [JetBrains.Annotations.NotNull] RelationalCompiledQueryCacheKeyGeneratorDependencies relationalDependencies)
		: base(dependencies)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(relationalDependencies, "relationalDependencies");
		RelationalDependencies = relationalDependencies;
	}

	public override object GenerateCacheKey(Expression query, bool async)
	{
		return GenerateCacheKeyCore(query, async);
	}

	protected new RelationalCompiledQueryCacheKey GenerateCacheKeyCore([JetBrains.Annotations.NotNull] Expression query, bool async)
	{
		return new RelationalCompiledQueryCacheKey(base.GenerateCacheKeyCore(query, async), RelationalOptionsExtension.Extract(RelationalDependencies.ContextOptions).UseRelationalNulls, Dependencies.IsRetryingExecutionStrategy);
	}
}
