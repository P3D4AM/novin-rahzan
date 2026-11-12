using System;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.Assessment.Expressions;

internal class StringBinaryExpression<TResult> : BinaryExpression
{
	private readonly Func<string, string, StringComparison, TResult> operation;

	public TResult NullParamResult { get; }

	public StringComparison StringComparison { get; } = StringComparison.Ordinal;


	public StringBinaryExpression(Func<string, string, StringComparison, TResult> func, TResult nullParamResult = default(TResult))
	{
		operation = func;
		NullParamResult = nullParamResult;
	}

	public StringBinaryExpression(Func<string, string, StringComparison, TResult> func, StringComparison stringComparison, TResult nullParamResult = default(TResult))
		: this(func, nullParamResult)
	{
		StringComparison = stringComparison;
	}

	public StringBinaryExpression(Func<string, string, StringComparison, TResult> func, IEnumerable<Expression> source, TResult nullParamResult = default(TResult))
		: base(source)
	{
		operation = func;
		NullParamResult = nullParamResult;
	}

	public StringBinaryExpression(Func<string, string, StringComparison, TResult> func, StringComparison stringComparison, IEnumerable<Expression> source, TResult nullParamResult = default(TResult))
		: this(func, source, nullParamResult)
	{
		StringComparison = stringComparison;
	}

	protected override object EvaluateCore(object arg1, object arg2)
	{
		if (arg1 == null || arg2 == null)
		{
			return NullParamResult;
		}
		return operation(arg1.Normalize<string>(), arg2.Normalize<string>(), StringComparison);
	}

	protected override CompositeExpression Copy()
	{
		return new StringBinaryExpression<TResult>(operation, StringComparison, NullParamResult);
	}
}
