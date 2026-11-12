using System;
using System.Runtime.CompilerServices;

namespace Ninject.Infrastructure;

public class ReferenceEqualWeakReference : WeakReference
{
	private readonly int cachedHashCode;

	public ReferenceEqualWeakReference(object target)
		: base(target)
	{
		cachedHashCode = RuntimeHelpers.GetHashCode(target);
	}

	public ReferenceEqualWeakReference(object target, bool trackResurrection)
		: base(target, trackResurrection)
	{
		cachedHashCode = RuntimeHelpers.GetHashCode(target);
	}

	public override bool Equals(object obj)
	{
		object? obj2 = (IsAlive ? Target : this);
		if (obj is WeakReference { IsAlive: not false } weakReference)
		{
			obj = weakReference.Target;
		}
		return obj2 == obj;
	}

	public override int GetHashCode()
	{
		return cachedHashCode;
	}
}
