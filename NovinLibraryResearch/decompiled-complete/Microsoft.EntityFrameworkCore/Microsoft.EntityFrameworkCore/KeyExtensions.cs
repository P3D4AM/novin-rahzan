using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore;

public static class KeyExtensions
{
	public static IEnumerable<IForeignKey> GetReferencingForeignKeys([JetBrains.Annotations.NotNull] this IKey key)
	{
		IEnumerable<IForeignKey> referencingForeignKeys = Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(key, "key").AsKey("GetReferencingForeignKeys").ReferencingForeignKeys;
		return referencingForeignKeys ?? Enumerable.Empty<IForeignKey>();
	}

	public static bool IsPrimaryKey([JetBrains.Annotations.NotNull] this IKey key)
	{
		return key == key.DeclaringEntityType.FindPrimaryKey();
	}
}
