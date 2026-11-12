using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.Scaffolding.Internal;

public interface ICSharpEntityTypeGenerator
{
	string WriteCode([JetBrains.Annotations.NotNull] IEntityType entityType, [JetBrains.Annotations.NotNull] string @namespace, bool useDataAnnotations);
}
