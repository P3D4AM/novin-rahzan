using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

public class ShadowValuesFactoryFactory : SnapshotFactoryFactory<ValueBuffer>
{
	protected override bool UseEntityVariable => false;

	protected override int GetPropertyIndex(IPropertyBase propertyBase)
	{
		return (propertyBase as IProperty)?.GetShadowIndex() ?? (-1);
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
		return Expression.Convert(Expression.Call(parameter, ValueBuffer.GetValueMethod, Expression.Constant(property.GetShadowIndex())), property.ClrType);
	}

	protected override Expression CreateReadValueExpression(ParameterExpression parameter, IPropertyBase property)
	{
		return CreateReadShadowValueExpression(parameter, property);
	}
}
