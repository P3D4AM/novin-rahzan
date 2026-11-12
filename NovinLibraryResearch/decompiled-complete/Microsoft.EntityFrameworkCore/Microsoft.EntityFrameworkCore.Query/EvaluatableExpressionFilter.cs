using System;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Query;

public class EvaluatableExpressionFilter : IEvaluatableExpressionFilter
{
	private static readonly PropertyInfo _dateTimeNow = typeof(DateTime).GetTypeInfo().GetDeclaredProperty("Now");

	private static readonly PropertyInfo _dateTimeUtcNow = typeof(DateTime).GetTypeInfo().GetDeclaredProperty("UtcNow");

	private static readonly PropertyInfo _dateTimeToday = typeof(DateTime).GetTypeInfo().GetDeclaredProperty("Today");

	private static readonly PropertyInfo _dateTimeOffsetNow = typeof(DateTimeOffset).GetTypeInfo().GetDeclaredProperty("Now");

	private static readonly PropertyInfo _dateTimeOffsetUtcNow = typeof(DateTimeOffset).GetTypeInfo().GetDeclaredProperty("UtcNow");

	private static readonly MethodInfo _guidNewGuid = typeof(Guid).GetTypeInfo().GetDeclaredMethod("NewGuid");

	private static readonly MethodInfo _randomNextNoArgs = typeof(Random).GetRuntimeMethod("Next", Array.Empty<Type>());

	private static readonly MethodInfo _randomNextOneArg = typeof(Random).GetRuntimeMethod("Next", new Type[1] { typeof(int) });

	private static readonly MethodInfo _randomNextTwoArgs = typeof(Random).GetRuntimeMethod("Next", new Type[2]
	{
		typeof(int),
		typeof(int)
	});

	protected virtual EvaluatableExpressionFilterDependencies Dependencies { get; }

	public EvaluatableExpressionFilter([JetBrains.Annotations.NotNull] EvaluatableExpressionFilterDependencies dependencies)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(dependencies, "dependencies");
		Dependencies = dependencies;
	}

	public virtual bool IsEvaluatableExpression(Expression expression, IModel model)
	{
		if (!(expression is MemberExpression memberExpression))
		{
			if (expression is MethodCallExpression methodCallExpression)
			{
				MethodInfo method = methodCallExpression.Method;
				if (object.Equals(method, _guidNewGuid) || object.Equals(method, _randomNextNoArgs) || object.Equals(method, _randomNextOneArg) || object.Equals(method, _randomNextTwoArgs))
				{
					return false;
				}
			}
		}
		else
		{
			MemberInfo member = memberExpression.Member;
			if (object.Equals(member, _dateTimeNow) || object.Equals(member, _dateTimeUtcNow) || object.Equals(member, _dateTimeToday) || object.Equals(member, _dateTimeOffsetNow) || object.Equals(member, _dateTimeOffsetUtcNow))
			{
				return false;
			}
		}
		return true;
	}
}
