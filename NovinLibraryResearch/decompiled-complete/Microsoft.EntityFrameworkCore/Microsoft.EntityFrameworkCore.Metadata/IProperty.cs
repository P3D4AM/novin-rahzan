using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Metadata;

public interface IProperty : IPropertyBase, IAnnotatable
{
	IEntityType DeclaringEntityType { get; }

	bool IsNullable { get; }

	ValueGenerated ValueGenerated { get; }

	bool IsConcurrencyToken { get; }
}
