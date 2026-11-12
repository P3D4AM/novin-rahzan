using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Microsoft.EntityFrameworkCore.ChangeTracking;

public class GeometryValueComparer<TGeometry> : ValueComparer<TGeometry>
{
	public GeometryValueComparer()
		: base(GetEqualsExpression(), ValueComparer<TGeometry>.CreateDefaultHashCodeExpression(favorStructuralComparisons: false), GetSnapshotExpression())
	{
	}

	private static Expression<Func<TGeometry, TGeometry, bool>> GetEqualsExpression()
	{
		ParameterExpression parameterExpression = Expression.Parameter(typeof(TGeometry), "left");
		ParameterExpression parameterExpression2 = Expression.Parameter(typeof(TGeometry), "right");
		return Expression.Lambda<Func<TGeometry, TGeometry, bool>>(Expression.Call(parameterExpression, typeof(TGeometry).GetRuntimeMethod("EqualsTopologically", new Type[1] { typeof(TGeometry) }), parameterExpression2), new ParameterExpression[2] { parameterExpression, parameterExpression2 });
	}

	private static Expression<Func<TGeometry, TGeometry>> GetSnapshotExpression()
	{
		ParameterExpression parameterExpression = Expression.Parameter(typeof(TGeometry), "instance");
		Expression expression = Expression.Call(parameterExpression, typeof(TGeometry).GetRuntimeMethod("Copy", Type.EmptyTypes));
		if (typeof(TGeometry).FullName != "NetTopologySuite.Geometries.Geometry")
		{
			expression = Expression.Convert(expression, typeof(TGeometry));
		}
		return Expression.Lambda<Func<TGeometry, TGeometry>>(expression, new ParameterExpression[1] { parameterExpression });
	}
}
