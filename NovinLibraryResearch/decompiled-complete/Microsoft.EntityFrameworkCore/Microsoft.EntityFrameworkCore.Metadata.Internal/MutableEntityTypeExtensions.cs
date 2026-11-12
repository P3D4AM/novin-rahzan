using System.Collections.Generic;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

public static class MutableEntityTypeExtensions
{
	public static void AddData([JetBrains.Annotations.NotNull] this IMutableEntityType entityType, [JetBrains.Annotations.NotNull] params object[] data)
	{
		entityType.AsEntityType("AddData").AddData(data);
	}

	public static void AddData([JetBrains.Annotations.NotNull] this IMutableEntityType entityType, [JetBrains.Annotations.NotNull] IEnumerable<object> data)
	{
		entityType.AsEntityType("AddData").AddData(data);
	}
}
