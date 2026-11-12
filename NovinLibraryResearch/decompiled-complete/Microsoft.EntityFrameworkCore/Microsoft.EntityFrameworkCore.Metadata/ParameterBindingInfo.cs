using System.Linq.Expressions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Metadata;

public readonly struct ParameterBindingInfo
{
	public IEntityType EntityType { get; }

	public Expression MaterializationContextExpression { get; }

	public ParameterBindingInfo([JetBrains.Annotations.NotNull] IEntityType entityType, [JetBrains.Annotations.NotNull] Expression materializationContextExpression)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(materializationContextExpression, "materializationContextExpression");
		EntityType = entityType;
		MaterializationContextExpression = materializationContextExpression;
	}

	public int GetValueBufferIndex([JetBrains.Annotations.NotNull] IPropertyBase property)
	{
		return property.GetIndex();
	}
}
