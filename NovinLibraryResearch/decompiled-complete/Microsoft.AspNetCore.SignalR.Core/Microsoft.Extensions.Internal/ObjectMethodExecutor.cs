using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Microsoft.Extensions.Internal;

internal class ObjectMethodExecutor
{
	private delegate ObjectMethodExecutorAwaitable MethodExecutorAsync(object target, object[] parameters);

	private delegate object MethodExecutor(object target, object[] parameters);

	private delegate void VoidMethodExecutor(object target, object[] parameters);

	private readonly object[] _parameterDefaultValues;

	private readonly MethodExecutorAsync _executorAsync;

	private readonly MethodExecutor _executor;

	private static readonly ConstructorInfo _objectMethodExecutorAwaitableConstructor = typeof(ObjectMethodExecutorAwaitable).GetConstructor(new Type[6]
	{
		typeof(object),
		typeof(Func<object, object>),
		typeof(Func<object, bool>),
		typeof(Func<object, object>),
		typeof(Action<object, Action>),
		typeof(Action<object, Action>)
	});

	public MethodInfo MethodInfo { get; }

	public ParameterInfo[] MethodParameters { get; }

	public TypeInfo TargetTypeInfo { get; }

	public Type AsyncResultType { get; }

	public Type MethodReturnType { get; internal set; }

	public bool IsMethodAsync { get; }

	private ObjectMethodExecutor(MethodInfo methodInfo, TypeInfo targetTypeInfo, object[] parameterDefaultValues)
	{
		if (methodInfo == null)
		{
			throw new ArgumentNullException("methodInfo");
		}
		MethodInfo = methodInfo;
		MethodParameters = methodInfo.GetParameters();
		TargetTypeInfo = targetTypeInfo;
		MethodReturnType = methodInfo.ReturnType;
		AsyncResultType = ((IsMethodAsync = CoercedAwaitableInfo.IsTypeAwaitable(MethodReturnType, out var info)) ? info.AwaitableInfo.ResultType : null);
		_executor = GetExecutor(methodInfo, targetTypeInfo);
		if (IsMethodAsync)
		{
			_executorAsync = GetExecutorAsync(methodInfo, targetTypeInfo, info);
		}
		_parameterDefaultValues = parameterDefaultValues;
	}

	public static ObjectMethodExecutor Create(MethodInfo methodInfo, TypeInfo targetTypeInfo)
	{
		return new ObjectMethodExecutor(methodInfo, targetTypeInfo, null);
	}

	public static ObjectMethodExecutor Create(MethodInfo methodInfo, TypeInfo targetTypeInfo, object[] parameterDefaultValues)
	{
		if (parameterDefaultValues == null)
		{
			throw new ArgumentNullException("parameterDefaultValues");
		}
		return new ObjectMethodExecutor(methodInfo, targetTypeInfo, parameterDefaultValues);
	}

	public object Execute(object target, object[] parameters)
	{
		return _executor(target, parameters);
	}

	public ObjectMethodExecutorAwaitable ExecuteAsync(object target, object[] parameters)
	{
		return _executorAsync(target, parameters);
	}

	public object GetDefaultValueForParameter(int index)
	{
		if (_parameterDefaultValues == null)
		{
			throw new InvalidOperationException("Cannot call GetDefaultValueForParameter, because no parameter default values were supplied.");
		}
		if (index < 0 || index > MethodParameters.Length - 1)
		{
			throw new ArgumentOutOfRangeException("index");
		}
		return _parameterDefaultValues[index];
	}

	private static MethodExecutor GetExecutor(MethodInfo methodInfo, TypeInfo targetTypeInfo)
	{
		ParameterExpression parameterExpression = Expression.Parameter(typeof(object), "target");
		ParameterExpression parameterExpression2 = Expression.Parameter(typeof(object[]), "parameters");
		List<Expression> list = new List<Expression>();
		ParameterInfo[] parameters = methodInfo.GetParameters();
		for (int i = 0; i < parameters.Length; i++)
		{
			ParameterInfo parameterInfo = parameters[i];
			UnaryExpression item = Expression.Convert(Expression.ArrayIndex(parameterExpression2, Expression.Constant(i)), parameterInfo.ParameterType);
			list.Add(item);
		}
		MethodCallExpression methodCallExpression = Expression.Call(Expression.Convert(parameterExpression, targetTypeInfo.AsType()), methodInfo, list);
		if (methodCallExpression.Type == typeof(void))
		{
			return WrapVoidMethod(Expression.Lambda<VoidMethodExecutor>(methodCallExpression, new ParameterExpression[2] { parameterExpression, parameterExpression2 }).Compile());
		}
		return Expression.Lambda<MethodExecutor>(Expression.Convert(methodCallExpression, typeof(object)), new ParameterExpression[2] { parameterExpression, parameterExpression2 }).Compile();
	}

	private static MethodExecutor WrapVoidMethod(VoidMethodExecutor executor)
	{
		return delegate(object target, object[] parameters)
		{
			executor(target, parameters);
			return (object)null;
		};
	}

	private static MethodExecutorAsync GetExecutorAsync(MethodInfo methodInfo, TypeInfo targetTypeInfo, CoercedAwaitableInfo coercedAwaitableInfo)
	{
		ParameterExpression parameterExpression = Expression.Parameter(typeof(object), "target");
		ParameterExpression parameterExpression2 = Expression.Parameter(typeof(object[]), "parameters");
		List<Expression> list = new List<Expression>();
		ParameterInfo[] parameters = methodInfo.GetParameters();
		for (int i = 0; i < parameters.Length; i++)
		{
			ParameterInfo parameterInfo = parameters[i];
			UnaryExpression item = Expression.Convert(Expression.ArrayIndex(parameterExpression2, Expression.Constant(i)), parameterInfo.ParameterType);
			list.Add(item);
		}
		MethodCallExpression methodCallExpression = Expression.Call(Expression.Convert(parameterExpression, targetTypeInfo.AsType()), methodInfo, list);
		ParameterExpression parameterExpression3 = Expression.Parameter(typeof(object), "awaitable");
		AwaitableInfo awaitableInfo = coercedAwaitableInfo.AwaitableInfo;
		Type type = coercedAwaitableInfo.CoercerResultType ?? methodInfo.ReturnType;
		Func<object, object> value = Expression.Lambda<Func<object, object>>(Expression.Convert(Expression.Call(Expression.Convert(parameterExpression3, type), awaitableInfo.GetAwaiterMethod), typeof(object)), new ParameterExpression[1] { parameterExpression3 }).Compile();
		ParameterExpression parameterExpression4 = Expression.Parameter(typeof(object), "awaiter");
		Func<object, bool> value2 = Expression.Lambda<Func<object, bool>>(Expression.MakeMemberAccess(Expression.Convert(parameterExpression4, awaitableInfo.AwaiterType), awaitableInfo.AwaiterIsCompletedProperty), new ParameterExpression[1] { parameterExpression4 }).Compile();
		ParameterExpression parameterExpression5 = Expression.Parameter(typeof(object), "awaiter");
		Func<object, object> value3 = ((!(awaitableInfo.ResultType == typeof(void))) ? Expression.Lambda<Func<object, object>>(Expression.Convert(Expression.Call(Expression.Convert(parameterExpression5, awaitableInfo.AwaiterType), awaitableInfo.AwaiterGetResultMethod), typeof(object)), new ParameterExpression[1] { parameterExpression5 }).Compile() : Expression.Lambda<Func<object, object>>(Expression.Block(Expression.Call(Expression.Convert(parameterExpression5, awaitableInfo.AwaiterType), awaitableInfo.AwaiterGetResultMethod), Expression.Constant(null)), new ParameterExpression[1] { parameterExpression5 }).Compile());
		ParameterExpression parameterExpression6 = Expression.Parameter(typeof(object), "awaiter");
		ParameterExpression parameterExpression7 = Expression.Parameter(typeof(Action), "continuation");
		Action<object, Action> value4 = Expression.Lambda<Action<object, Action>>(Expression.Call(Expression.Convert(parameterExpression6, awaitableInfo.AwaiterType), awaitableInfo.AwaiterOnCompletedMethod, parameterExpression7), new ParameterExpression[2] { parameterExpression6, parameterExpression7 }).Compile();
		Action<object, Action> value5 = null;
		if (awaitableInfo.AwaiterUnsafeOnCompletedMethod != null)
		{
			ParameterExpression parameterExpression8 = Expression.Parameter(typeof(object), "awaiter");
			ParameterExpression parameterExpression9 = Expression.Parameter(typeof(Action), "continuation");
			value5 = Expression.Lambda<Action<object, Action>>(Expression.Call(Expression.Convert(parameterExpression8, awaitableInfo.AwaiterType), awaitableInfo.AwaiterUnsafeOnCompletedMethod, parameterExpression9), new ParameterExpression[2] { parameterExpression8, parameterExpression9 }).Compile();
		}
		Expression expression = (coercedAwaitableInfo.RequiresCoercion ? ((Expression)Expression.Invoke(coercedAwaitableInfo.CoercerExpression, methodCallExpression)) : ((Expression)methodCallExpression));
		return Expression.Lambda<MethodExecutorAsync>(Expression.New(_objectMethodExecutorAwaitableConstructor, Expression.Convert(expression, typeof(object)), Expression.Constant(value), Expression.Constant(value2), Expression.Constant(value3), Expression.Constant(value4), Expression.Constant(value5, typeof(Action<object, Action>))), new ParameterExpression[2] { parameterExpression, parameterExpression2 }).Compile();
	}
}
