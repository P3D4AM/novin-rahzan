using System;
using System.Collections;
using System.Collections.Generic;

namespace Serilog;

internal class ImmutableStack<T> : IEnumerable<T>, IEnumerable
{
	internal struct Enumerator : IEnumerator<T>, IDisposable, IEnumerator
	{
		private readonly ImmutableStack<T> _stack;

		private ImmutableStack<T> _top;

		private T _current;

		public T Current => _current;

		object IEnumerator.Current => _current;

		public Enumerator(ImmutableStack<T> stack)
		{
			_stack = stack;
			_top = stack;
			_current = default(T);
		}

		public bool MoveNext()
		{
			if (_top.IsEmpty)
			{
				return false;
			}
			_current = _top.Top;
			_top = _top._under;
			return true;
		}

		public void Reset()
		{
			_top = _stack;
			_current = default(T);
		}

		public void Dispose()
		{
		}
	}

	private readonly ImmutableStack<T> _under;

	private readonly T _top;

	public int Count { get; }

	public static ImmutableStack<T> Empty { get; } = new ImmutableStack<T>();


	public bool IsEmpty => _under == null;

	public T Top => _top;

	private ImmutableStack()
	{
	}

	private ImmutableStack(ImmutableStack<T> under, T top)
	{
		_under = under ?? throw new ArgumentNullException("under");
		Count = under.Count + 1;
		_top = top;
	}

	public Enumerator GetEnumerator()
	{
		return new Enumerator(this);
	}

	IEnumerator<T> IEnumerable<T>.GetEnumerator()
	{
		return new Enumerator(this);
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return new Enumerator(this);
	}

	public ImmutableStack<T> Push(T t)
	{
		return new ImmutableStack<T>(this, t);
	}
}
