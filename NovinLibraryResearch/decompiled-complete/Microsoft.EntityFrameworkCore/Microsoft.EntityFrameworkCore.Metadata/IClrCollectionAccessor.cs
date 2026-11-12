using System;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Metadata;

public interface IClrCollectionAccessor
{
	Type CollectionType { get; }

	bool Add([JetBrains.Annotations.NotNull] object entity, [JetBrains.Annotations.NotNull] object value, bool forMaterialization);

	bool Contains([JetBrains.Annotations.NotNull] object entity, [JetBrains.Annotations.NotNull] object value);

	bool Remove([JetBrains.Annotations.NotNull] object entity, [JetBrains.Annotations.NotNull] object value);

	object Create();

	object GetOrCreate([JetBrains.Annotations.NotNull] object entity, bool forMaterialization);
}
