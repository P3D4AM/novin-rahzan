using System;
using System.Collections;

namespace DevComponents.Editors;

internal class ᢇ : IEnumerator, IEnumerable
{
	private IList _ᢉ;

	private int _ᢊ = -1;

	private int _ᢋ = 1;

	private int _ቲ = -1;

	private int _ታ = -1;

	public object Current
	{
		get
		{
			if (_ᢊ < 0)
			{
				throw new InvalidOperationException("IEnumerator Pointer is invalid. Use MoveNext to advance enumerator.");
			}
			return _ᢉ[_ᢊ];
		}
	}

	internal int ᢈ
	{
		get
		{
			return _ᢊ;
		}
		set
		{
			_ᢊ = value;
		}
	}

	public ᢇ(VisualItemCollection parentCollection, bool rightToLeft)
	{
		_ᢉ = parentCollection;
		if (rightToLeft)
		{
			_ᢋ = -1;
			_ቲ = parentCollection.Count - 1;
			_ታ = -1;
		}
		else
		{
			_ቲ = 0;
			_ታ = parentCollection.Count;
		}
	}

	public IEnumerator GetEnumerator()
	{
		return this;
	}

	public bool MoveNext()
	{
		if (_ᢊ == _ታ && _ᢊ >= 0)
		{
			throw new InvalidOperationException("Enumerator cannot advance beyond the boundaries of array. Use Reset to reset enumerator");
		}
		if (_ᢊ == -1)
		{
			_ᢊ = _ቲ - _ᢋ;
		}
		_ᢊ += _ᢋ;
		if (_ᢊ == _ታ)
		{
			return false;
		}
		return true;
	}

	public void Reset()
	{
		_ᢊ = -1;
	}
}
