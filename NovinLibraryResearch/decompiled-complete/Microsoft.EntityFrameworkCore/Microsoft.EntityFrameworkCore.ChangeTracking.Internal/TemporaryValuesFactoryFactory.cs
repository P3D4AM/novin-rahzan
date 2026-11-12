using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

public class TemporaryValuesFactoryFactory : SidecarValuesFactoryFactory
{
	protected override Expression CreateSnapshotExpression(Type entityType, ParameterExpression parameter, Type[] types, IList<IPropertyBase> propertyBases)
	{
		return Expression.Convert(Expression.New(Snapshot.CreateSnapshotType(types).GetDeclaredConstructor(new Type[0])), typeof(ISnapshot));
	}
}
