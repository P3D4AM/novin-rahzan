using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Metadata;

public interface IServiceProperty : IPropertyBase, IAnnotatable
{
	IEntityType DeclaringEntityType { get; }

	ServiceParameterBinding ParameterBinding { get; }
}
