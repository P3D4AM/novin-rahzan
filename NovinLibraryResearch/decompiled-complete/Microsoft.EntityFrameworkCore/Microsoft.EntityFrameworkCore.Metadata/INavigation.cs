using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Metadata;

public interface INavigation : IPropertyBase, IAnnotatable
{
	IEntityType DeclaringEntityType { get; }

	IForeignKey ForeignKey { get; }
}
