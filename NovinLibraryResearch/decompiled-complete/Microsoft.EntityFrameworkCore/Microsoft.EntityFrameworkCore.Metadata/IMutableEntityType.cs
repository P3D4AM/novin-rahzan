using System;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Metadata;

public interface IMutableEntityType : IEntityType, ITypeBase, IAnnotatable, IMutableTypeBase, IMutableAnnotatable
{
	new IMutableModel Model { get; }

	new IMutableEntityType BaseType
	{
		get; [param: JetBrains.Annotations.CanBeNull]
		set;
	}

	new IMutableEntityType DefiningEntityType { get; }

	bool IsKeyless { get; set; }

	IMutableKey SetPrimaryKey([JetBrains.Annotations.CanBeNull] IReadOnlyList<IMutableProperty> properties);

	new IMutableKey FindPrimaryKey();

	IMutableKey AddKey([JetBrains.Annotations.NotNull] IReadOnlyList<IMutableProperty> properties);

	new IMutableKey FindKey([JetBrains.Annotations.NotNull] IReadOnlyList<IProperty> properties);

	new IEnumerable<IMutableKey> GetKeys();

	void RemoveKey([JetBrains.Annotations.NotNull] IMutableKey key);

	IMutableForeignKey AddForeignKey([JetBrains.Annotations.NotNull] IReadOnlyList<IMutableProperty> properties, [JetBrains.Annotations.NotNull] IMutableKey principalKey, [JetBrains.Annotations.NotNull] IMutableEntityType principalEntityType);

	new IMutableForeignKey FindForeignKey([JetBrains.Annotations.NotNull] IReadOnlyList<IProperty> properties, [JetBrains.Annotations.NotNull] IKey principalKey, [JetBrains.Annotations.NotNull] IEntityType principalEntityType);

	new IEnumerable<IMutableForeignKey> GetForeignKeys();

	void RemoveForeignKey([JetBrains.Annotations.NotNull] IMutableForeignKey foreignKey);

	IMutableIndex AddIndex([JetBrains.Annotations.NotNull] IReadOnlyList<IMutableProperty> properties);

	new IMutableIndex FindIndex([JetBrains.Annotations.NotNull] IReadOnlyList<IProperty> properties);

	new IEnumerable<IMutableIndex> GetIndexes();

	void RemoveIndex([JetBrains.Annotations.NotNull] IMutableIndex index);

	IMutableProperty AddProperty([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] Type propertyType, [JetBrains.Annotations.CanBeNull] MemberInfo memberInfo);

	new IMutableProperty FindProperty([JetBrains.Annotations.NotNull] string name);

	new IEnumerable<IMutableProperty> GetProperties();

	void RemoveProperty([JetBrains.Annotations.NotNull] IMutableProperty property);

	IMutableServiceProperty AddServiceProperty([JetBrains.Annotations.NotNull] MemberInfo memberInfo);

	new IMutableServiceProperty FindServiceProperty([JetBrains.Annotations.NotNull] string name);

	new IEnumerable<IMutableServiceProperty> GetServiceProperties();

	IMutableServiceProperty RemoveServiceProperty([JetBrains.Annotations.NotNull] string name);
}
