using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Metadata;

public interface IMutableNavigation : INavigation, IPropertyBase, IAnnotatable, IMutablePropertyBase, IMutableAnnotatable
{
	new IMutableEntityType DeclaringEntityType { get; }

	new IMutableForeignKey ForeignKey { get; }
}
