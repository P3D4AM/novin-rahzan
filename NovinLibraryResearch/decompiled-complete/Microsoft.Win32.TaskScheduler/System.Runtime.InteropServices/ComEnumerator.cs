using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace System.Runtime.InteropServices;

internal class ComEnumerator<T, TIn> : IEnumerator<T>, IDisposable, IEnumerator where T : class where TIn : class
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass2_0
	{
		public Func<int, TIn> indexer;

		public Func<int> getCount;
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass3_0
	{
		public Func<object, TIn> indexer;

		public Func<int> getCount;
	}

	protected readonly Func<TIn, T> converter;

	protected IEnumerator<TIn> iEnum;

	object IEnumerator.Current => Current;

	public virtual T Current
	{
		get
		{
			Func<TIn, T> func = converter;
			IEnumerator<TIn> enumerator = iEnum;
			return func((enumerator != null) ? enumerator.Current : null);
		}
	}

	public ComEnumerator(Func<int> getCount, Func<int, TIn> indexer, Func<TIn, T> converter)
	{
		_003C_003Ec__DisplayClass2_0 CS_0024_003C_003E8__locals0 = new _003C_003Ec__DisplayClass2_0();
		CS_0024_003C_003E8__locals0.indexer = indexer;
		CS_0024_003C_003E8__locals0.getCount = getCount;
		base._002Ector();
		this.converter = converter;
		iEnum = Enumerate();
		[IteratorStateMachine(typeof(ComEnumerator<, >._003C_003Ec__DisplayClass2_0._003C_003C_002Dctor_003Eg__Enumerate_007C0_003Ed))]
		IEnumerator<TIn> Enumerate()
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003C_003Ec__DisplayClass2_0._003C_003C_002Dctor_003Eg__Enumerate_007C0_003Ed(0)
			{
				_003C_003E4__this = CS_0024_003C_003E8__locals0
			};
		}
	}

	public ComEnumerator(Func<int> getCount, Func<object, TIn> indexer, Func<TIn, T> converter)
	{
		_003C_003Ec__DisplayClass3_0 CS_0024_003C_003E8__locals0 = new _003C_003Ec__DisplayClass3_0();
		CS_0024_003C_003E8__locals0.indexer = indexer;
		CS_0024_003C_003E8__locals0.getCount = getCount;
		base._002Ector();
		this.converter = converter;
		iEnum = Enumerate();
		[IteratorStateMachine(typeof(ComEnumerator<, >._003C_003Ec__DisplayClass3_0._003C_003C_002Dctor_003Eg__Enumerate_007C0_003Ed))]
		IEnumerator<TIn> Enumerate()
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003C_003Ec__DisplayClass3_0._003C_003C_002Dctor_003Eg__Enumerate_007C0_003Ed(0)
			{
				_003C_003E4__this = CS_0024_003C_003E8__locals0
			};
		}
	}

	public virtual void Dispose()
	{
		iEnum?.Dispose();
		iEnum = null;
	}

	public virtual bool MoveNext()
	{
		return iEnum?.MoveNext() ?? false;
	}

	public virtual void Reset()
	{
		iEnum?.Reset();
	}
}
