using System;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public readonly struct InterceptionResult
{
	public bool IsSuppressed { get; }

	public static InterceptionResult Suppress()
	{
		return new InterceptionResult(suppress: true);
	}

	private InterceptionResult(bool suppress)
	{
		IsSuppressed = suppress;
	}
}
public readonly struct InterceptionResult<TResult>
{
	private readonly TResult _result;

	public TResult Result
	{
		get
		{
			if (!HasResult)
			{
				throw new InvalidOperationException(CoreStrings.NoInterceptionResult);
			}
			return _result;
		}
	}

	public bool HasResult { get; }

	public static InterceptionResult<TResult> SuppressWithResult([JetBrains.Annotations.CanBeNull] TResult result)
	{
		return new InterceptionResult<TResult>(result);
	}

	private InterceptionResult(TResult result)
	{
		_result = result;
		HasResult = true;
	}
}
