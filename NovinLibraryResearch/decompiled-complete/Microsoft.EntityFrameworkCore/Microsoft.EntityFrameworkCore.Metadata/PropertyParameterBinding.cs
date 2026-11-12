using System.Linq.Expressions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.Metadata;

public class PropertyParameterBinding : ParameterBinding
{
	public PropertyParameterBinding([JetBrains.Annotations.NotNull] IProperty property)
		: base(property.ClrType, property)
	{
	}

	public override Expression BindToParameter(ParameterBindingInfo bindingInfo)
	{
		IPropertyBase propertyBase = ConsumedProperties[0];
		return Expression.Call(EntityMaterializerSource.TryReadValueMethod.MakeGenericMethod(propertyBase.ClrType), Expression.Call(bindingInfo.MaterializationContextExpression, MaterializationContext.GetValueBufferMethod), Expression.Constant(bindingInfo.GetValueBufferIndex(propertyBase)), Expression.Constant(propertyBase, typeof(IPropertyBase)));
	}
}
