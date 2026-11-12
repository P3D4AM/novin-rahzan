using System;

namespace Antlr.Runtime.Misc;

public abstract class LookaheadStream<T> : FastQueue<T> where T : class
{
	private int _currentElementIndex;

	private T _previousElement;

	private T _eof = null;

	private int _lastMarker;

	private int _markDepth;

	public T EndOfFile
	{
		get
		{
			return _eof;
		}
		protected set
		{
			_eof = value;
		}
	}

	public T PreviousElement => _previousElement;

	public override int Count
	{
		get
		{
			throw new NotSupportedException("streams are of unknown size");
		}
	}

	public virtual int Index => _currentElementIndex;

	public virtual void Reset()
	{
		Clear();
		_currentElementIndex = 0;
		_p = 0;
		_previousElement = null;
	}

	public abstract T NextElement();

	public abstract bool IsEndOfFile(T o);

	public override T Dequeue()
	{
		T val = this[0];
		_p++;
		if (_p == _data.Count && _markDepth == 0)
		{
			_previousElement = val;
			Clear();
		}
		return val;
	}

	public virtual void Consume()
	{
		SyncAhead(1);
		Dequeue();
		_currentElementIndex++;
	}

	protected virtual void SyncAhead(int need)
	{
		int num = _p + need - 1 - _data.Count + 1;
		if (num > 0)
		{
			Fill(num);
		}
	}

	public virtual void Fill(int n)
	{
		for (int i = 0; i < n; i++)
		{
			T val = NextElement();
			if (IsEndOfFile(val))
			{
				_eof = val;
			}
			_data.Add(val);
		}
	}

	public virtual T LT(int k)
	{
		if (k == 0)
		{
			return null;
		}
		if (k < 0)
		{
			return LB(-k);
		}
		SyncAhead(k);
		if (_p + k - 1 > _data.Count)
		{
			return _eof;
		}
		return this[k - 1];
	}

	public virtual int Mark()
	{
		_markDepth++;
		_lastMarker = _p;
		return _lastMarker;
	}

	public virtual void Release(int marker)
	{
		if (_markDepth == 0)
		{
			throw new InvalidOperationException();
		}
		_markDepth--;
	}

	public virtual void Rewind(int marker)
	{
		_markDepth--;
		int num = _p - marker;
		_currentElementIndex -= num;
		_p = marker;
	}

	public virtual void Rewind()
	{
		int num = _p - _lastMarker;
		_currentElementIndex -= num;
		_p = _lastMarker;
	}

	public virtual void Seek(int index)
	{
		if (index < 0)
		{
			throw new ArgumentOutOfRangeException("index");
		}
		int num = _currentElementIndex - index;
		if (_p - num < 0)
		{
			throw new NotSupportedException("can't seek before the beginning of this stream's buffer");
		}
		_p -= num;
		_currentElementIndex = index;
	}

	protected virtual T LB(int k)
	{
		int num = _p - k;
		if (num == -1)
		{
			return _previousElement;
		}
		if (num >= 0)
		{
			return _data[num];
		}
		if (num < -1)
		{
			throw new NotSupportedException("can't look more than one token before the beginning of this stream's buffer");
		}
		throw new NotSupportedException("can't look past the end of this stream's buffer using LB(int)");
	}
}
