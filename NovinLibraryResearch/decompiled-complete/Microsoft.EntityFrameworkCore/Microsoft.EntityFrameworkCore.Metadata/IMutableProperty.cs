using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Metadata;

public interface IMutableProperty : IProperty, IPropertyBase, IAnnotatable, IMutablePropertyBase, IMutableAnnotatable
{
	new IMutableEntityType DeclaringEntityType { get; }

	new bool IsNullable { get; set; }

	new ValueGenerated ValueGenerated { get; set; }

	new bool IsConcurrencyToken { get; set; }
}
