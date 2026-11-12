using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Channels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Internal;

namespace Microsoft.AspNetCore.SignalR.Internal;

internal class HubMethodDescriptor
{
	private static readonly MethodInfo GetAsyncEnumeratorMethod = typeof(AsyncEnumeratorAdapters).GetRuntimeMethods().Single((MethodInfo m) => m.Name.Equals("GetAsyncEnumerator") && m.IsGenericMethod);

	private Func<object, CancellationToken, IAsyncEnumerator<object>> _convertToEnumerator;

	public ObjectMethodExecutor MethodExecutor { get; }

	public IReadOnlyList<Type> ParameterTypes { get; }

	public Type NonAsyncReturnType { get; }

	public bool IsChannel { get; }

	public bool IsStreamable => IsChannel;

	public Type StreamReturnType { get; }

	public IList<IAuthorizeData> Policies { get; }

	public HubMethodDescriptor(ObjectMethodExecutor methodExecutor, IEnumerable<IAuthorizeData> policies)
	{
		MethodExecutor = methodExecutor;
		ParameterTypes = methodExecutor.MethodParameters.Select((ParameterInfo p) => p.ParameterType).ToArray();
		Policies = policies.ToArray();
		NonAsyncReturnType = (MethodExecutor.IsMethodAsync ? MethodExecutor.AsyncResultType : MethodExecutor.MethodReturnType);
		if (IsChannelType(NonAsyncReturnType, out var payloadType))
		{
			IsChannel = true;
			StreamReturnType = payloadType;
		}
	}

	private static bool IsChannelType(Type type, out Type payloadType)
	{
		Type type2 = type.AllBaseTypes().FirstOrDefault((Type t) => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(ChannelReader<>));
		if (type2 == null)
		{
			payloadType = null;
			return false;
		}
		payloadType = type2.GetGenericArguments()[0];
		return true;
	}

	public IAsyncEnumerator<object> FromChannel(object channel, CancellationToken cancellationToken)
	{
		if (_convertToEnumerator == null)
		{
			_convertToEnumerator = CompileConvertToEnumerator(GetAsyncEnumeratorMethod, StreamReturnType);
		}
		return _convertToEnumerator(channel, cancellationToken);
	}

	private static Func<object, CancellationToken, IAsyncEnumerator<object>> CompileConvertToEnumerator(MethodInfo adapterMethodInfo, Type streamReturnType)
	{
		MethodInfo methodInfo = adapterMethodInfo.MakeGenericMethod(streamReturnType);
		ParameterInfo[] parameters = methodInfo.GetParameters();
		ParameterExpression parameterExpression = Expression.Parameter(typeof(object), "arg1");
		ParameterExpression parameterExpression2 = Expression.Parameter(typeof(CancellationToken), "arg2");
		List<Expression> arguments = new List<Expression>
		{
			Expression.Convert(parameterExpression, parameters[0].ParameterType),
			parameterExpression2
		};
		return Expression.Lambda<Func<object, CancellationToken, IAsyncEnumerator<object>>>(Expression.Convert(Expression.Call(null, methodInfo, arguments), typeof(IAsyncEnumerator<object>)), new ParameterExpression[2] { parameterExpression, parameterExpression2 }).Compile();
	}
}
