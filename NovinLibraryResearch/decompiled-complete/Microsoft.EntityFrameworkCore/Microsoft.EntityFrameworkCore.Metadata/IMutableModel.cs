using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Metadata;

public interface IMutableModel : IModel, IAnnotatable, IMutableAnnotatable
{
	IMutableEntityType AddEntityType([JetBrains.Annotations.NotNull] string name);

	IMutableEntityType AddEntityType([JetBrains.Annotations.NotNull] Type clrType);

	IMutableEntityType AddEntityType([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] string definingNavigationName, [JetBrains.Annotations.NotNull] IMutableEntityType definingEntityType);

	IMutableEntityType AddEntityType([JetBrains.Annotations.NotNull] Type clrType, [JetBrains.Annotations.NotNull] string definingNavigationName, [JetBrains.Annotations.NotNull] IMutableEntityType definingEntityType);

	new IMutableEntityType FindEntityType([JetBrains.Annotations.NotNull] string name);

	IMutableEntityType FindEntityType([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] string definingNavigationName, [JetBrains.Annotations.NotNull] IMutableEntityType definingEntityType);

	void RemoveEntityType([JetBrains.Annotations.NotNull] IMutableEntityType entityType);

	new IEnumerable<IMutableEntityType> GetEntityTypes();

	void AddIgnored([JetBrains.Annotations.NotNull] string typeName);

	void RemoveIgnored([JetBrains.Annotations.NotNull] string typeName);

	bool IsIgnored([JetBrains.Annotations.NotNull] string typeName);
}
