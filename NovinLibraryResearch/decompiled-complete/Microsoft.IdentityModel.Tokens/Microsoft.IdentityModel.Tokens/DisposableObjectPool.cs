using System;
using System.Threading;

namespace Microsoft.IdentityModel.Tokens;

internal sealed class DisposableObjectPool<T> where T : class, IDisposable
{
	internal struct Element
	{
		internal T Value;
	}

	private readonly Func<T> _factory;

	internal Element[] Items { get; }

	internal int Size { get; }

	internal DisposableObjectPool(Func<T> factory)
		: this(factory, Environment.ProcessorCount * 2)
	{
	}

	internal DisposableObjectPool(Func<T> factory, int size)
	{
		_factory = factory;
		Items = new Element[size];
		Size = size;
	}

	private T CreateInstance()
	{
		return _factory();
	}

	internal T Allocate()
	{
		Element[] items = Items;
		int num = 0;
		T val;
		while (true)
		{
			if (num < items.Length)
			{
				val = items[num].Value;
				if (val != null && val == Interlocked.CompareExchange(ref items[num].Value, null, val))
				{
					break;
				}
				num++;
				continue;
			}
			val = CreateInstance();
			break;
		}
		return val;
	}

	internal void Free(T obj)
	{
		Element[] items = Items;
		for (int i = 0; i < items.Length; i++)
		{
			if (items[i].Value == null)
			{
				items[i].Value = obj;
				break;
			}
		}
	}
}
