using System;
using System.Linq.Expressions;

namespace Microsoft.Extensions.Internal;

internal struct CoercedAwaitableInfo
{
	public AwaitableInfo AwaitableInfo { get; }

	public Expression CoercerExpression { get; }

	public Type CoercerResultType { get; }

	public bool RequiresCoercion => CoercerExpression != null;

	public CoercedAwaitableInfo(AwaitableInfo awaitableInfo)
	{
		AwaitableInfo = awaitableInfo;
		CoercerExpression = null;
		CoercerResultType = null;
	}

	public CoercedAwaitableInfo(Expression coercerExpression, Type coercerResultType, AwaitableInfo coercedAwaitableInfo)
	{
		CoercerExpression = coercerExpression;
		CoercerResultType = coercerResultType;
		AwaitableInfo = coercedAwaitableInfo;
	}

	public static bool IsTypeAwaitable(Type type, out CoercedAwaitableInfo info)
	{
		if (AwaitableInfo.IsTypeAwaitable(type, out var awaitableInfo))
		{
			info = new CoercedAwaitableInfo(awaitableInfo);
			return true;
		}
		if (ObjectMethodExecutorFSharpSupport.TryBuildCoercerFromFSharpAsyncToAwaitable(type, out var coerceToAwaitableExpression, out var awaitableType) && AwaitableInfo.IsTypeAwaitable(awaitableType, out var awaitableInfo2))
		{
			info = new CoercedAwaitableInfo(coerceToAwaitableExpression, awaitableType, awaitableInfo2);
			return true;
		}
		info = default(CoercedAwaitableInfo);
		return false;
	}
}
