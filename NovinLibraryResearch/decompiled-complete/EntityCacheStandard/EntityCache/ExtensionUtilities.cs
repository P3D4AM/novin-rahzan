using System;
using System.Linq.Expressions;

namespace EntityCache;

public static class ExtensionUtilities
{
	public static Expression<Func<TConcrete, bool>> ConvertToConcreteExpression<TConcrete, TInterface>(Expression<Func<TInterface, bool>> interfaceExpression)
	{
		if (!typeof(TInterface).IsAssignableFrom(typeof(TConcrete)))
		{
			throw new Exception("TInterface must be assignable from TConcrete to convert an expression.");
		}
		return TransformVisitor<TConcrete, TInterface>.Transform(interfaceExpression);
	}
}
