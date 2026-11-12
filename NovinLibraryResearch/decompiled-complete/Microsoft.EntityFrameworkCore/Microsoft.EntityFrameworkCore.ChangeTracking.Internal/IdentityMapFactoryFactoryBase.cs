using System;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

public abstract class IdentityMapFactoryFactoryBase
{
	protected virtual Type GetKeyType([JetBrains.Annotations.NotNull] IKey key)
	{
		if (key.Properties.Count <= 1)
		{
			return key.Properties.First().ClrType;
		}
		return typeof(object[]);
	}
}
