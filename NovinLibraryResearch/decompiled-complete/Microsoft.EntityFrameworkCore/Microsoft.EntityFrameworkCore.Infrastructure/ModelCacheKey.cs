using System;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Infrastructure;

public class ModelCacheKey
{
	private readonly Type _dbContextType;

	public ModelCacheKey([JetBrains.Annotations.NotNull] DbContext context)
	{
		_dbContextType = context.GetType();
	}

	protected virtual bool Equals([JetBrains.Annotations.NotNull] ModelCacheKey other)
	{
		return _dbContextType == other._dbContextType;
	}

	public override bool Equals(object obj)
	{
		if (obj is ModelCacheKey other)
		{
			return Equals(other);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return _dbContextType?.GetHashCode() ?? 0;
	}
}
