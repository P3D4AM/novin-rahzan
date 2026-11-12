using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.ChangeTracking;

public abstract class ValueComparer : IEqualityComparer
{
	internal static readonly MethodInfo EqualityComparerHashCodeMethod = typeof(IEqualityComparer).GetRuntimeMethod("GetHashCode", new Type[1] { typeof(object) });

	internal static readonly MethodInfo EqualityComparerEqualsMethod = typeof(IEqualityComparer).GetRuntimeMethod("Equals", new Type[2]
	{
		typeof(object),
		typeof(object)
	});

	internal static readonly MethodInfo ObjectEqualsMethod = typeof(object).GetRuntimeMethod("Equals", new Type[2]
	{
		typeof(object),
		typeof(object)
	});

	internal static readonly MethodInfo ObjectGetHashCodeMethod = typeof(object).GetRuntimeMethod("GetHashCode", Type.EmptyTypes);

	public abstract Type Type { get; }

	public virtual LambdaExpression EqualsExpression { get; }

	public virtual LambdaExpression HashCodeExpression { get; }

	public virtual LambdaExpression SnapshotExpression { get; }

	protected ValueComparer([JetBrains.Annotations.NotNull] LambdaExpression equalsExpression, [JetBrains.Annotations.NotNull] LambdaExpression hashCodeExpression, [JetBrains.Annotations.NotNull] LambdaExpression snapshotExpression)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(equalsExpression, "equalsExpression");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(hashCodeExpression, "hashCodeExpression");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(snapshotExpression, "snapshotExpression");
		EqualsExpression = equalsExpression;
		HashCodeExpression = hashCodeExpression;
		SnapshotExpression = snapshotExpression;
	}

	public new abstract bool Equals(object left, object right);

	public abstract int GetHashCode(object instance);

	public abstract object Snapshot([JetBrains.Annotations.CanBeNull] object instance);

	public virtual Expression ExtractEqualsBody([JetBrains.Annotations.NotNull] Expression leftExpression, [JetBrains.Annotations.NotNull] Expression rightExpression)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(leftExpression, "leftExpression");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(rightExpression, "rightExpression");
		ParameterExpression parameterExpression = EqualsExpression.Parameters[0];
		ParameterExpression parameterExpression2 = EqualsExpression.Parameters[1];
		return new ReplacingExpressionVisitor(new Expression[2] { parameterExpression, parameterExpression2 }, new Expression[2] { leftExpression, rightExpression }).Visit(EqualsExpression.Body);
	}

	public virtual Expression ExtractHashCodeBody([JetBrains.Annotations.NotNull] Expression expression)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(expression, "expression");
		return ReplacingExpressionVisitor.Replace(HashCodeExpression.Parameters[0], expression, HashCodeExpression.Body);
	}

	public virtual Expression ExtractSnapshotBody([JetBrains.Annotations.NotNull] Expression expression)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(expression, "expression");
		return ReplacingExpressionVisitor.Replace(SnapshotExpression.Parameters[0], expression, SnapshotExpression.Body);
	}
}
public class ValueComparer<T> : ValueComparer, IEqualityComparer<T>
{
	private Func<T, T, bool> _equals;

	private Func<T, int> _hashCode;

	private Func<T, T> _snapshot;

	public override Type Type => typeof(T);

	public new virtual Expression<Func<T, T, bool>> EqualsExpression => (Expression<Func<T, T, bool>>)base.EqualsExpression;

	public new virtual Expression<Func<T, int>> HashCodeExpression => (Expression<Func<T, int>>)base.HashCodeExpression;

	public new virtual Expression<Func<T, T>> SnapshotExpression => (Expression<Func<T, T>>)base.SnapshotExpression;

	public ValueComparer(bool favorStructuralComparisons)
		: this(CreateDefaultEqualsExpression(), CreateDefaultHashCodeExpression(favorStructuralComparisons))
	{
	}

	public ValueComparer([JetBrains.Annotations.NotNull] Expression<Func<T, T, bool>> equalsExpression, [JetBrains.Annotations.NotNull] Expression<Func<T, int>> hashCodeExpression)
		: this(equalsExpression, hashCodeExpression, (Expression<Func<T, T>>)((T v) => v))
	{
	}

	public ValueComparer([JetBrains.Annotations.NotNull] Expression<Func<T, T, bool>> equalsExpression, [JetBrains.Annotations.NotNull] Expression<Func<T, int>> hashCodeExpression, [JetBrains.Annotations.NotNull] Expression<Func<T, T>> snapshotExpression)
		: base(equalsExpression, hashCodeExpression, snapshotExpression)
	{
	}

	protected static Expression<Func<T, T, bool>> CreateDefaultEqualsExpression()
	{
		Type type = typeof(T);
		ParameterExpression parameterExpression = Expression.Parameter(type, "v1");
		ParameterExpression parameterExpression2 = Expression.Parameter(type, "v2");
		if (typeof(IStructuralEquatable).GetTypeInfo().IsAssignableFrom(type.GetTypeInfo()))
		{
			return Expression.Lambda<Func<T, T, bool>>(Expression.Call(Expression.Constant(StructuralComparisons.StructuralEqualityComparer, typeof(IEqualityComparer)), ValueComparer.EqualityComparerEqualsMethod, Expression.Convert(parameterExpression, typeof(object)), Expression.Convert(parameterExpression2, typeof(object))), new ParameterExpression[2] { parameterExpression, parameterExpression2 });
		}
		Type type2 = type.UnwrapNullableType();
		if (type2.IsInteger() || type2 == typeof(string) || type2 == typeof(Guid) || type2 == typeof(bool) || type2 == typeof(decimal) || type2 == typeof(double) || type2 == typeof(float) || type2 == typeof(object))
		{
			return Expression.Lambda<Func<T, T, bool>>(Expression.Equal(parameterExpression, parameterExpression2), new ParameterExpression[2] { parameterExpression, parameterExpression2 });
		}
		MethodInfo methodInfo = type.GetRuntimeMethods().FirstOrDefault((MethodInfo m) => m.ReturnType == typeof(bool) && !m.IsStatic && "Equals".Equals(m.Name, StringComparison.Ordinal) && m.GetParameters().Length == 1 && m.GetParameters()[0].ParameterType == typeof(T));
		while (methodInfo == null && type != null)
		{
			methodInfo = type.GetTypeInfo().DeclaredMethods.FirstOrDefault((MethodInfo m) => m.IsStatic && m.ReturnType == typeof(bool) && "op_Equality".Equals(m.Name, StringComparison.Ordinal) && m.GetParameters().Length == 2 && m.GetParameters()[0].ParameterType == typeof(T) && m.GetParameters()[1].ParameterType == typeof(T));
			type = type.BaseType;
		}
		return Expression.Lambda<Func<T, T, bool>>((methodInfo == null) ? Expression.Call(ValueComparer.ObjectEqualsMethod, Expression.Convert(parameterExpression, typeof(object)), Expression.Convert(parameterExpression2, typeof(object))) : (methodInfo.IsStatic ? Expression.Call(methodInfo, parameterExpression, parameterExpression2) : Expression.Call(parameterExpression, methodInfo, parameterExpression2)), new ParameterExpression[2] { parameterExpression, parameterExpression2 });
	}

	protected static Expression<Func<T, int>> CreateDefaultHashCodeExpression(bool favorStructuralComparisons)
	{
		Type typeFromHandle = typeof(T);
		Type type = typeFromHandle.UnwrapNullableType();
		ParameterExpression parameterExpression = Expression.Parameter(typeFromHandle, "v");
		if (favorStructuralComparisons && typeof(IStructuralEquatable).GetTypeInfo().IsAssignableFrom(typeFromHandle.GetTypeInfo()))
		{
			return Expression.Lambda<Func<T, int>>(Expression.Call(Expression.Constant(StructuralComparisons.StructuralEqualityComparer, typeof(IEqualityComparer)), ValueComparer.EqualityComparerHashCodeMethod, Expression.Convert(parameterExpression, typeof(object))), new ParameterExpression[1] { parameterExpression });
		}
		return Expression.Lambda<Func<T, int>>((typeFromHandle == typeof(int)) ? parameterExpression : ((type == typeof(int) || type == typeof(short) || type == typeof(byte) || type == typeof(uint) || type == typeof(ushort) || type == typeof(sbyte) || type == typeof(char)) ? ((Expression)Expression.Convert(parameterExpression, typeof(int))) : ((Expression)Expression.Call(Expression.Convert(parameterExpression, typeof(object)), ValueComparer.ObjectGetHashCodeMethod))), new ParameterExpression[1] { parameterExpression });
	}

	public override bool Equals(object left, object right)
	{
		bool flag = left == null;
		bool flag2 = right == null;
		if (!(flag || flag2))
		{
			return Equals((T)left, (T)right);
		}
		return flag && flag2;
	}

	public override int GetHashCode(object instance)
	{
		if (instance != null)
		{
			return GetHashCode((T)instance);
		}
		return 0;
	}

	public virtual bool Equals(T left, T right)
	{
		return NonCapturingLazyInitializer.EnsureInitialized(ref _equals, this, (ValueComparer<T> c) => c.EqualsExpression.Compile())(left, right);
	}

	public virtual int GetHashCode(T instance)
	{
		return NonCapturingLazyInitializer.EnsureInitialized(ref _hashCode, this, (ValueComparer<T> c) => c.HashCodeExpression.Compile())(instance);
	}

	public override object Snapshot(object instance)
	{
		if (instance != null)
		{
			return Snapshot((T)instance);
		}
		return null;
	}

	public virtual T Snapshot([JetBrains.Annotations.CanBeNull] T instance)
	{
		return NonCapturingLazyInitializer.EnsureInitialized(ref _snapshot, this, (ValueComparer<T> c) => c.SnapshotExpression.Compile())(instance);
	}
}
