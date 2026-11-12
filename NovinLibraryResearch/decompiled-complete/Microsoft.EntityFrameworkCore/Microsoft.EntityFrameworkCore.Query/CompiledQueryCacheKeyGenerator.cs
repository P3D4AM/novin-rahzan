using System;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Query;

public class CompiledQueryCacheKeyGenerator : ICompiledQueryCacheKeyGenerator
{
	protected readonly struct CompiledQueryCacheKey
	{
		private readonly Expression _query;

		private readonly IModel _model;

		private readonly QueryTrackingBehavior _queryTrackingBehavior;

		private readonly bool _async;

		public CompiledQueryCacheKey([JetBrains.Annotations.NotNull] Expression query, [JetBrains.Annotations.NotNull] IModel model, QueryTrackingBehavior queryTrackingBehavior, bool async)
		{
			_query = query;
			_model = model;
			_queryTrackingBehavior = queryTrackingBehavior;
			_async = async;
		}

		public override bool Equals(object obj)
		{
			if (obj == null || !(obj is CompiledQueryCacheKey compiledQueryCacheKey))
			{
				return false;
			}
			if (_model == compiledQueryCacheKey._model && _queryTrackingBehavior == compiledQueryCacheKey._queryTrackingBehavior && _async == compiledQueryCacheKey._async)
			{
				return ExpressionEqualityComparer.Instance.Equals(_query, compiledQueryCacheKey._query);
			}
			return false;
		}

		public override int GetHashCode()
		{
			HashCode hashCode = default(HashCode);
			hashCode.Add(_query, ExpressionEqualityComparer.Instance);
			hashCode.Add(_model);
			hashCode.Add(_queryTrackingBehavior);
			hashCode.Add(_async);
			return hashCode.ToHashCode();
		}
	}

	protected virtual CompiledQueryCacheKeyGeneratorDependencies Dependencies { get; }

	public CompiledQueryCacheKeyGenerator([JetBrains.Annotations.NotNull] CompiledQueryCacheKeyGeneratorDependencies dependencies)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(dependencies, "dependencies");
		Dependencies = dependencies;
	}

	public virtual object GenerateCacheKey(Expression query, bool async)
	{
		return GenerateCacheKeyCore(query, async);
	}

	protected CompiledQueryCacheKey GenerateCacheKeyCore([JetBrains.Annotations.NotNull] Expression query, bool async)
	{
		return new CompiledQueryCacheKey(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(query, "query"), Dependencies.Model, Dependencies.CurrentContext.Context.ChangeTracker.QueryTrackingBehavior, async);
	}
}
