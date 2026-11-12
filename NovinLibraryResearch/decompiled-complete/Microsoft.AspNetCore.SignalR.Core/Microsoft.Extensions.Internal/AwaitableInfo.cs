using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Microsoft.Extensions.Internal;

internal struct AwaitableInfo
{
	public Type AwaiterType { get; }

	public PropertyInfo AwaiterIsCompletedProperty { get; }

	public MethodInfo AwaiterGetResultMethod { get; }

	public MethodInfo AwaiterOnCompletedMethod { get; }

	public MethodInfo AwaiterUnsafeOnCompletedMethod { get; }

	public Type ResultType { get; }

	public MethodInfo GetAwaiterMethod { get; }

	public AwaitableInfo(Type awaiterType, PropertyInfo awaiterIsCompletedProperty, MethodInfo awaiterGetResultMethod, MethodInfo awaiterOnCompletedMethod, MethodInfo awaiterUnsafeOnCompletedMethod, Type resultType, MethodInfo getAwaiterMethod)
	{
		AwaiterType = awaiterType;
		AwaiterIsCompletedProperty = awaiterIsCompletedProperty;
		AwaiterGetResultMethod = awaiterGetResultMethod;
		AwaiterOnCompletedMethod = awaiterOnCompletedMethod;
		AwaiterUnsafeOnCompletedMethod = awaiterUnsafeOnCompletedMethod;
		ResultType = resultType;
		GetAwaiterMethod = getAwaiterMethod;
	}

	public static bool IsTypeAwaitable(Type type, out AwaitableInfo awaitableInfo)
	{
		MethodInfo methodInfo = type.GetRuntimeMethods().FirstOrDefault((MethodInfo m) => m.Name.Equals("GetAwaiter", StringComparison.OrdinalIgnoreCase) && m.GetParameters().Length == 0 && m.ReturnType != null);
		if (methodInfo == null)
		{
			awaitableInfo = default(AwaitableInfo);
			return false;
		}
		Type returnType = methodInfo.ReturnType;
		PropertyInfo propertyInfo = returnType.GetRuntimeProperties().FirstOrDefault((PropertyInfo p) => p.Name.Equals("IsCompleted", StringComparison.OrdinalIgnoreCase) && p.PropertyType == typeof(bool) && p.GetMethod != null);
		if (propertyInfo == null)
		{
			awaitableInfo = default(AwaitableInfo);
			return false;
		}
		Type[] interfaces = returnType.GetInterfaces();
		if (!interfaces.Any((Type t) => t == typeof(INotifyCompletion)))
		{
			awaitableInfo = default(AwaitableInfo);
			return false;
		}
		MethodInfo awaiterOnCompletedMethod = returnType.GetTypeInfo().GetRuntimeInterfaceMap(typeof(INotifyCompletion)).InterfaceMethods.Single((MethodInfo m) => m.Name.Equals("OnCompleted", StringComparison.OrdinalIgnoreCase) && m.ReturnType == typeof(void) && m.GetParameters().Length == 1 && m.GetParameters()[0].ParameterType == typeof(Action));
		MethodInfo awaiterUnsafeOnCompletedMethod = ((!interfaces.Any((Type t) => t == typeof(ICriticalNotifyCompletion))) ? null : returnType.GetTypeInfo().GetRuntimeInterfaceMap(typeof(ICriticalNotifyCompletion)).InterfaceMethods.Single((MethodInfo m) => m.Name.Equals("UnsafeOnCompleted", StringComparison.OrdinalIgnoreCase) && m.ReturnType == typeof(void) && m.GetParameters().Length == 1 && m.GetParameters()[0].ParameterType == typeof(Action)));
		MethodInfo methodInfo2 = returnType.GetRuntimeMethods().FirstOrDefault((MethodInfo m) => m.Name.Equals("GetResult") && m.GetParameters().Length == 0);
		if (methodInfo2 == null)
		{
			awaitableInfo = default(AwaitableInfo);
			return false;
		}
		awaitableInfo = new AwaitableInfo(returnType, propertyInfo, methodInfo2, awaiterOnCompletedMethod, awaiterUnsafeOnCompletedMethod, methodInfo2.ReturnType, methodInfo);
		return true;
	}
}
