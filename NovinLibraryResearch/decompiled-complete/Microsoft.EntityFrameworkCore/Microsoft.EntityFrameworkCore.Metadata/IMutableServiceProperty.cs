using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Metadata;

public interface IMutableServiceProperty : IServiceProperty, IPropertyBase, IAnnotatable, IMutablePropertyBase, IMutableAnnotatable
{
	new IMutableEntityType DeclaringEntityType { get; }

	new ServiceParameterBinding ParameterBinding
	{
		get; [param: JetBrains.Annotations.CanBeNull]
		set;
	}
}
