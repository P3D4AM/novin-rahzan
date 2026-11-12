using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Extensions.Internal;

internal static class ObjectMethodExecutorFSharpSupport
{
	private static object _fsharpValuesCacheLock = new object();

	private static Assembly _fsharpCoreAssembly;

	private static MethodInfo _fsharpAsyncStartAsTaskGenericMethod;

	private static PropertyInfo _fsharpOptionOfTaskCreationOptionsNoneProperty;

	private static PropertyInfo _fsharpOptionOfCancellationTokenNoneProperty;

	public static bool TryBuildCoercerFromFSharpAsyncToAwaitable(Type possibleFSharpAsyncType, out Expression coerceToAwaitableExpression, out Type awaitableType)
	{
		if (!IsFSharpAsyncOpenGenericType(possibleFSharpAsyncType.IsGenericType ? possibleFSharpAsyncType.GetGenericTypeDefinition() : null))
		{
			coerceToAwaitableExpression = null;
			awaitableType = null;
			return false;
		}
		Type type = possibleFSharpAsyncType.GetGenericArguments().Single();
		awaitableType = typeof(Task<>).MakeGenericType(type);
		MethodInfo method = _fsharpAsyncStartAsTaskGenericMethod.MakeGenericMethod(type);
		ParameterExpression parameterExpression = Expression.Parameter(typeof(object));
		coerceToAwaitableExpression = Expression.Lambda(Expression.Convert(Expression.Call(method, Expression.Convert(parameterExpression, possibleFSharpAsyncType), Expression.MakeMemberAccess(null, _fsharpOptionOfTaskCreationOptionsNoneProperty), Expression.MakeMemberAccess(null, _fsharpOptionOfCancellationTokenNoneProperty)), typeof(object)), parameterExpression);
		return true;
	}

	private static bool IsFSharpAsyncOpenGenericType(Type possibleFSharpAsyncGenericType)
	{
		if (!string.Equals(possibleFSharpAsyncGenericType?.FullName, "Microsoft.FSharp.Control.FSharpAsync`1", StringComparison.Ordinal))
		{
			return false;
		}
		lock (_fsharpValuesCacheLock)
		{
			if (_fsharpCoreAssembly != null)
			{
				return possibleFSharpAsyncGenericType.Assembly == _fsharpCoreAssembly;
			}
			return TryPopulateFSharpValueCaches(possibleFSharpAsyncGenericType);
		}
	}

	private static bool TryPopulateFSharpValueCaches(Type possibleFSharpAsyncGenericType)
	{
		Assembly assembly = possibleFSharpAsyncGenericType.Assembly;
		Type type = assembly.GetType("Microsoft.FSharp.Core.FSharpOption`1");
		Type type2 = assembly.GetType("Microsoft.FSharp.Control.FSharpAsync");
		if (type == null || type2 == null)
		{
			return false;
		}
		Type type3 = type.MakeGenericType(typeof(TaskCreationOptions));
		_fsharpOptionOfTaskCreationOptionsNoneProperty = type3.GetTypeInfo().GetRuntimeProperty("None");
		Type type4 = type.MakeGenericType(typeof(CancellationToken));
		_fsharpOptionOfCancellationTokenNoneProperty = type4.GetTypeInfo().GetRuntimeProperty("None");
		foreach (MethodInfo item in from m in type2.GetRuntimeMethods()
			where m.Name.Equals("StartAsTask", StringComparison.Ordinal)
			select m)
		{
			ParameterInfo[] parameters = item.GetParameters();
			if (parameters.Length == 3 && TypesHaveSameIdentity(parameters[0].ParameterType, possibleFSharpAsyncGenericType) && parameters[1].ParameterType == type3 && parameters[2].ParameterType == type4)
			{
				_fsharpAsyncStartAsTaskGenericMethod = item;
				_fsharpCoreAssembly = assembly;
				break;
			}
		}
		return _fsharpCoreAssembly != null;
	}

	private static bool TypesHaveSameIdentity(Type type1, Type type2)
	{
		if (type1.Assembly == type2.Assembly && string.Equals(type1.Namespace, type2.Namespace, StringComparison.Ordinal))
		{
			return string.Equals(type1.Name, type2.Name, StringComparison.Ordinal);
		}
		return false;
	}
}
