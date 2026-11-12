using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Metadata;

public interface IEntityType : ITypeBase, IAnnotatable
{
	IEntityType BaseType { get; }

	string DefiningNavigationName { get; }

	IEntityType DefiningEntityType { get; }

	IKey FindPrimaryKey();

	IKey FindKey([JetBrains.Annotations.NotNull] IReadOnlyList<IProperty> properties);

	IEnumerable<IKey> GetKeys();

	IForeignKey FindForeignKey([JetBrains.Annotations.NotNull] IReadOnlyList<IProperty> properties, [JetBrains.Annotations.NotNull] IKey principalKey, [JetBrains.Annotations.NotNull] IEntityType principalEntityType);

	IEnumerable<IForeignKey> GetForeignKeys();

	IIndex FindIndex([JetBrains.Annotations.NotNull] IReadOnlyList<IProperty> properties);

	IEnumerable<IIndex> GetIndexes();

	IProperty FindProperty([JetBrains.Annotations.NotNull] string name);

	IEnumerable<IProperty> GetProperties();

	IServiceProperty FindServiceProperty([JetBrains.Annotations.NotNull] string name);

	IEnumerable<IServiceProperty> GetServiceProperties();
}
