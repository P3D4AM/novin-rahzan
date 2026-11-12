using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore;

public static class ConventionKeyExtensions
{
	public static IEnumerable<IConventionForeignKey> GetReferencingForeignKeys([JetBrains.Annotations.NotNull] this IConventionKey key)
	{
		return KeyExtensions.GetReferencingForeignKeys(key).Cast<IConventionForeignKey>();
	}
}
