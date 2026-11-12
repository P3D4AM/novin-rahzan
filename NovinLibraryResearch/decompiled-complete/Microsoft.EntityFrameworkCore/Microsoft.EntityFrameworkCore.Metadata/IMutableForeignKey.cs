using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Metadata;

public interface IMutableForeignKey : IForeignKey, IAnnotatable, IMutableAnnotatable
{
	new IReadOnlyList<IMutableProperty> Properties { get; }

	new IMutableKey PrincipalKey { get; }

	new IMutableEntityType DeclaringEntityType { get; }

	new IMutableEntityType PrincipalEntityType { get; }

	new bool IsUnique { get; set; }

	new bool IsRequired { get; set; }

	new bool IsOwnership { get; set; }

	new DeleteBehavior DeleteBehavior { get; set; }

	new IMutableNavigation DependentToPrincipal { get; }

	new IMutableNavigation PrincipalToDependent { get; }

	void SetProperties([JetBrains.Annotations.NotNull] IReadOnlyList<IMutableProperty> properties, [JetBrains.Annotations.NotNull] IMutableKey principalKey);

	IMutableNavigation HasDependentToPrincipal([JetBrains.Annotations.CanBeNull] string name);

	IMutableNavigation HasDependentToPrincipal([JetBrains.Annotations.CanBeNull] MemberInfo property);

	IMutableNavigation HasPrincipalToDependent([JetBrains.Annotations.CanBeNull] string name);

	IMutableNavigation HasPrincipalToDependent([JetBrains.Annotations.CanBeNull] MemberInfo property);
}
