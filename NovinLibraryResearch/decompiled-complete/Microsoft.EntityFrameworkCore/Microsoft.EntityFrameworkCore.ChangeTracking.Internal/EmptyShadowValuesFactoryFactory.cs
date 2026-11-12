using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

public class EmptyShadowValuesFactoryFactory : SnapshotFactoryFactory
{
	protected override bool UseEntityVariable => false;

	protected override int GetPropertyIndex(IPropertyBase propertyBase)
	{
		return propertyBase.GetShadowIndex();
	}

	protected override int GetPropertyCount(IEntityType entityType)
	{
		return entityType.ShadowPropertyCount();
	}

	protected override ValueComparer GetValueComparer(IProperty property)
	{
		return null;
	}

	protected override Expression CreateReadShadowValueExpression(ParameterExpression parameter, IPropertyBase property)
	{
		return Expression.Default(property.ClrType);
	}

	protected override Expression CreateReadValueExpression(ParameterExpression parameter, IPropertyBase property)
	{
		return Expression.Default(property.ClrType);
	}
}
