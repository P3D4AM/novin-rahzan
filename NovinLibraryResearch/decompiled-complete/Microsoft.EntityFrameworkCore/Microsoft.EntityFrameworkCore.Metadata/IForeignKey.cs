using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Metadata;

public interface IForeignKey : IAnnotatable
{
	IEntityType DeclaringEntityType { get; }

	IReadOnlyList<IProperty> Properties { get; }

	IEntityType PrincipalEntityType { get; }

	IKey PrincipalKey { get; }

	INavigation DependentToPrincipal { get; }

	INavigation PrincipalToDependent { get; }

	bool IsUnique { get; }

	bool IsRequired { get; }

	bool IsOwnership { get; }

	DeleteBehavior DeleteBehavior { get; }
}
