using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

public class CompositeNullableValueFactory : CompositeValueFactory, INullableValueFactory<object[]>, IDependentKeyValueFactory<object[]>
{
	public virtual IEqualityComparer<object[]> EqualityComparer { get; }

	public CompositeNullableValueFactory([JetBrains.Annotations.NotNull] IReadOnlyList<IProperty> properties)
		: base(properties)
	{
		EqualityComparer = CompositeValueFactory.CreateEqualityComparer(properties);
	}
}
