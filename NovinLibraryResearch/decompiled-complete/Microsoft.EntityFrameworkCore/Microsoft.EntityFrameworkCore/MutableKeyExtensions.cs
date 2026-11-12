using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore;

public static class MutableKeyExtensions
{
	public static IEnumerable<IMutableForeignKey> GetReferencingForeignKeys([JetBrains.Annotations.NotNull] this IMutableKey key)
	{
		return KeyExtensions.GetReferencingForeignKeys(key).Cast<IMutableForeignKey>();
	}
}
