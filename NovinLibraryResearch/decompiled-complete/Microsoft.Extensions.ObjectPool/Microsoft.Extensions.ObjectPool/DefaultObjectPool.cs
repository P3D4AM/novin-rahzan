using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Microsoft.Extensions.ObjectPool;

public class DefaultObjectPool<T> : ObjectPool<T> where T : class
{
	[DebuggerDisplay("{Element}")]
	private struct ObjectWrapper
	{
		public T Element;

		public ObjectWrapper(T item)
		{
			Element = item;
		}

		public static implicit operator T(ObjectWrapper wrapper)
		{
			return wrapper.Element;
		}
	}

	private readonly ObjectWrapper[] _items;

	private readonly IPooledObjectPolicy<T> _policy;

	private readonly bool _isDefaultPolicy;

	private T _firstItem;

	private readonly PooledObjectPolicy<T> _fastPolicy;

	public DefaultObjectPool(IPooledObjectPolicy<T> policy)
		: this(policy, Environment.ProcessorCount * 2)
	{
	}

	public DefaultObjectPool(IPooledObjectPolicy<T> policy, int maximumRetained)
	{
		_policy = policy ?? throw new ArgumentNullException("policy");
		_fastPolicy = policy as PooledObjectPolicy<T>;
		_isDefaultPolicy = IsDefaultPolicy();
		_items = new ObjectWrapper[maximumRetained - 1];
		bool IsDefaultPolicy()
		{
			Type type = policy.GetType();
			if (type.IsGenericType)
			{
				return type.GetGenericTypeDefinition() == typeof(DefaultPooledObjectPolicy<>);
			}
			return false;
		}
	}

	public override T Get()
	{
		T val = _firstItem;
		if (val == null || Interlocked.CompareExchange(ref _firstItem, null, val) != val)
		{
			val = GetViaScan();
		}
		return val;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private T GetViaScan()
	{
		ObjectWrapper[] items = _items;
		T val = null;
		for (int i = 0; i < items.Length; i++)
		{
			val = items[i];
			if (val != null && Interlocked.CompareExchange(ref items[i].Element, null, val) == val)
			{
				break;
			}
		}
		return val ?? Create();
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private T Create()
	{
		PooledObjectPolicy<T> fastPolicy = _fastPolicy;
		return ((fastPolicy != null) ? fastPolicy.Create() : null) ?? _policy.Create();
	}

	public override void Return(T obj)
	{
		if ((_isDefaultPolicy || (_fastPolicy?.Return(obj) ?? _policy.Return(obj))) && (_firstItem != null || Interlocked.CompareExchange(ref _firstItem, obj, null) != null))
		{
			ReturnViaScan(obj);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void ReturnViaScan(T obj)
	{
		ObjectWrapper[] items = _items;
		for (int i = 0; i < items.Length && Interlocked.CompareExchange(ref items[i].Element, obj, null) != null; i++)
		{
		}
	}
}
