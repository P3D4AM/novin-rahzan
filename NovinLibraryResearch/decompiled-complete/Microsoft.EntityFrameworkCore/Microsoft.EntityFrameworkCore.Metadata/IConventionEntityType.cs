using System;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microsoft.EntityFrameworkCore.Metadata;

public interface IConventionEntityType : IEntityType, ITypeBase, IAnnotatable, IConventionTypeBase, IConventionAnnotatable
{
	new IConventionModel Model { get; }

	IConventionEntityTypeBuilder Builder { get; }

	new IConventionEntityType BaseType { get; }

	new IConventionEntityType DefiningEntityType { get; }

	bool IsKeyless { get; }

	ConfigurationSource GetConfigurationSource();

	void HasBaseType([JetBrains.Annotations.CanBeNull] IConventionEntityType entityType, bool fromDataAnnotation = false);

	void HasNoKey(bool? keyless, bool fromDataAnnotation = false);

	IConventionKey SetPrimaryKey([JetBrains.Annotations.CanBeNull] IReadOnlyList<IConventionProperty> properties, bool fromDataAnnotation = false);

	new IConventionKey FindPrimaryKey();

	ConfigurationSource? GetPrimaryKeyConfigurationSource();

	IConventionKey AddKey([JetBrains.Annotations.NotNull] IReadOnlyList<IConventionProperty> properties, bool fromDataAnnotation = false);

	new IConventionKey FindKey([JetBrains.Annotations.NotNull] IReadOnlyList<IProperty> properties);

	new IEnumerable<IConventionKey> GetKeys();

	void RemoveKey([JetBrains.Annotations.NotNull] IConventionKey key);

	IConventionForeignKey AddForeignKey([JetBrains.Annotations.NotNull] IReadOnlyList<IConventionProperty> properties, [JetBrains.Annotations.NotNull] IConventionKey principalKey, [JetBrains.Annotations.NotNull] IConventionEntityType principalEntityType, bool setComponentConfigurationSource = true, bool fromDataAnnotation = false);

	new IConventionForeignKey FindForeignKey([JetBrains.Annotations.NotNull] IReadOnlyList<IProperty> properties, [JetBrains.Annotations.NotNull] IKey principalKey, [JetBrains.Annotations.NotNull] IEntityType principalEntityType);

	new IEnumerable<IConventionForeignKey> GetForeignKeys();

	void RemoveForeignKey([JetBrains.Annotations.NotNull] IConventionForeignKey foreignKey);

	IConventionIndex AddIndex([JetBrains.Annotations.NotNull] IReadOnlyList<IConventionProperty> properties, bool fromDataAnnotation = false);

	new IConventionIndex FindIndex([JetBrains.Annotations.NotNull] IReadOnlyList<IProperty> properties);

	new IEnumerable<IConventionIndex> GetIndexes();

	void RemoveIndex([JetBrains.Annotations.NotNull] IConventionIndex index);

	IConventionProperty AddProperty([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] Type propertyType, [JetBrains.Annotations.CanBeNull] MemberInfo memberInfo, bool setTypeConfigurationSource = true, bool fromDataAnnotation = false);

	new IConventionProperty FindProperty([JetBrains.Annotations.NotNull] string name);

	new IEnumerable<IConventionProperty> GetProperties();

	void RemoveProperty([JetBrains.Annotations.NotNull] IConventionProperty property);

	IConventionServiceProperty AddServiceProperty([JetBrains.Annotations.NotNull] MemberInfo memberInfo, bool fromDataAnnotation = false);

	new IConventionServiceProperty FindServiceProperty([JetBrains.Annotations.NotNull] string name);

	new IEnumerable<IConventionServiceProperty> GetServiceProperties();

	IConventionServiceProperty RemoveServiceProperty([JetBrains.Annotations.NotNull] string name);
}
