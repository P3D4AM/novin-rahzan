using System.Linq.Expressions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Metadata;

public class EntityTypeParameterBinding : ServiceParameterBinding
{
	public EntityTypeParameterBinding([JetBrains.Annotations.CanBeNull] IPropertyBase serviceProperty = null)
		: base(typeof(IEntityType), typeof(IEntityType), serviceProperty)
	{
	}

	public override Expression BindToParameter(Expression materializationExpression, Expression entityTypeExpression)
	{
		return Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityTypeExpression, "entityTypeExpression");
	}
}
