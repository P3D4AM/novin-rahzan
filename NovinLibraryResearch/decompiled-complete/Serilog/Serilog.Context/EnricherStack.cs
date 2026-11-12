using System;
using System.Collections;
using System.Collections.Generic;
using Serilog.Core;

namespace Serilog.Context;

internal class EnricherStack : IEnumerable<ILogEventEnricher>, IEnumerable
{
	internal struct Enumerator : IEnumerator<ILogEventEnricher>, IDisposable, IEnumerator
	{
		private readonly EnricherStack _stack;

		private EnricherStack _top;

		private ILogEventEnricher? _current;

		public ILogEventEnricher Current => _current;

		object IEnumerator.Current => _current;

		public Enumerator(EnricherStack stack)
		{
			_stack = stack;
			_top = stack;
			_current = null;
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
			_current = null;
		}

		public void Dispose()
		{
		}
	}

	private readonly EnricherStack? _under;

	private readonly ILogEventEnricher? _top;

	public int Count { get; }

	public static EnricherStack Empty { get; } = new EnricherStack();


	public bool IsEmpty => _under == null;

	public ILogEventEnricher Top => _top;

	private EnricherStack()
	{
	}

	private EnricherStack(EnricherStack under, ILogEventEnricher top)
	{
		_under = Guard.AgainstNull(under, "under");
		Count = under.Count + 1;
		_top = top;
	}

	public Enumerator GetEnumerator()
	{
		return new Enumerator(this);
	}

	IEnumerator<ILogEventEnricher> IEnumerable<ILogEventEnricher>.GetEnumerator()
	{
		return new Enumerator(this);
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return new Enumerator(this);
	}

	public EnricherStack Push(ILogEventEnricher t)
	{
		return new EnricherStack(this, t);
	}
}
