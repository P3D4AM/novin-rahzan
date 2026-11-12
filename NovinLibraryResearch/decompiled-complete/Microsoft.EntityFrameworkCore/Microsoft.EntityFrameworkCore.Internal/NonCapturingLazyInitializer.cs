using System;
using System.Threading;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Internal;

public static class NonCapturingLazyInitializer
{
	public static TValue EnsureInitialized<TParam, TValue>([JetBrains.Annotations.CanBeNull] ref TValue target, [JetBrains.Annotations.CanBeNull] TParam param, [JetBrains.Annotations.NotNull] Func<TParam, TValue> valueFactory) where TValue : class
	{
		TValue val = Volatile.Read(ref target);
		if (val != null)
		{
			return val;
		}
		Interlocked.CompareExchange(ref target, valueFactory(param), null);
		return target;
	}

	public static TValue EnsureInitialized<TParam1, TParam2, TValue>([JetBrains.Annotations.CanBeNull] ref TValue target, [JetBrains.Annotations.CanBeNull] TParam1 param1, [JetBrains.Annotations.CanBeNull] TParam2 param2, [JetBrains.Annotations.NotNull] Func<TParam1, TParam2, TValue> valueFactory) where TValue : class
	{
		TValue val = Volatile.Read(ref target);
		if (val != null)
		{
			return val;
		}
		Interlocked.CompareExchange(ref target, valueFactory(param1, param2), null);
		return target;
	}

	public static TValue EnsureInitialized<TValue>([JetBrains.Annotations.CanBeNull] ref TValue target, [JetBrains.Annotations.NotNull] TValue value) where TValue : class
	{
		TValue val = Volatile.Read(ref target);
		if (val != null)
		{
			return val;
		}
		Interlocked.CompareExchange(ref target, value, null);
		return target;
	}

	public static TValue EnsureInitialized<TParam, TValue>([JetBrains.Annotations.CanBeNull] ref TValue target, [JetBrains.Annotations.CanBeNull] TParam param, [JetBrains.Annotations.NotNull] Action<TParam> valueFactory) where TValue : class
	{
		if (Volatile.Read(ref target) != null)
		{
			return target;
		}
		valueFactory(param);
		return Volatile.Read(ref target);
	}
}
