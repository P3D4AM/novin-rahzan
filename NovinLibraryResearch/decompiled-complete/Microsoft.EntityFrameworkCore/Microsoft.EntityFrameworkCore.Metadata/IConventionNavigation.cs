using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Metadata;

public interface IConventionNavigation : INavigation, IPropertyBase, IAnnotatable, IConventionPropertyBase, IConventionAnnotatable
{
	new IConventionEntityType DeclaringEntityType { get; }

	new IConventionForeignKey ForeignKey { get; }
}
