using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.Update;

public interface IUpdateEntry
{
	IEntityType EntityType { get; }

	EntityState EntityState { get; set; }

	IUpdateEntry SharedIdentityEntry { get; }

	void SetOriginalValue([JetBrains.Annotations.NotNull] IProperty property, [JetBrains.Annotations.CanBeNull] object value);

	void SetPropertyModified([JetBrains.Annotations.NotNull] IProperty property);

	bool IsModified([JetBrains.Annotations.NotNull] IProperty property);

	bool HasTemporaryValue([JetBrains.Annotations.NotNull] IProperty property);

	bool IsStoreGenerated([JetBrains.Annotations.NotNull] IProperty property);

	object GetCurrentValue([JetBrains.Annotations.NotNull] IPropertyBase propertyBase);

	object GetOriginalValue([JetBrains.Annotations.NotNull] IPropertyBase propertyBase);

	TProperty GetCurrentValue<TProperty>([JetBrains.Annotations.NotNull] IPropertyBase propertyBase);

	TProperty GetOriginalValue<TProperty>([JetBrains.Annotations.NotNull] IProperty property);

	void SetStoreGeneratedValue([JetBrains.Annotations.NotNull] IProperty property, [JetBrains.Annotations.CanBeNull] object value);

	EntityEntry ToEntityEntry();
}
