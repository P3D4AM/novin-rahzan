using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.Update.Internal;

public interface IKeyValueIndexFactory
{
	IKeyValueIndex CreatePrincipalKeyValue([JetBrains.Annotations.NotNull] InternalEntityEntry entry, [JetBrains.Annotations.NotNull] IForeignKey foreignKey);

	IKeyValueIndex CreatePrincipalKeyValueFromOriginalValues([JetBrains.Annotations.NotNull] InternalEntityEntry entry, [JetBrains.Annotations.NotNull] IForeignKey foreignKey);

	IKeyValueIndex CreateDependentKeyValue([JetBrains.Annotations.NotNull] InternalEntityEntry entry, [JetBrains.Annotations.NotNull] IForeignKey foreignKey);

	IKeyValueIndex CreateDependentKeyValueFromOriginalValues([JetBrains.Annotations.NotNull] InternalEntityEntry entry, [JetBrains.Annotations.NotNull] IForeignKey foreignKey);
}
