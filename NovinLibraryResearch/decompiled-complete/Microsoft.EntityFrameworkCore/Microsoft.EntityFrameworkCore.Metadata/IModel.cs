using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Metadata;

public interface IModel : IAnnotatable
{
	IEnumerable<IEntityType> GetEntityTypes();

	IEntityType FindEntityType([JetBrains.Annotations.NotNull] string name);

	IEntityType FindEntityType([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] string definingNavigationName, [JetBrains.Annotations.NotNull] IEntityType definingEntityType);
}
