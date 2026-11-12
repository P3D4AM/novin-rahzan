using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.Internal;

public interface IEntityFinderFactory
{
	IEntityFinder Create([JetBrains.Annotations.NotNull] IEntityType type);
}
