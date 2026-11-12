using System;
using System.Collections;

namespace Stimulsoft.Controls.Win.Editors;

internal class VisualCollectionEnumerator : IEnumerable, IEnumerator
{
	private IList _ParentCollection;

	private int _CurrentIndex = -1;

	private int _Direction = 1;

	private int _Start = -1;

	private int _End = -1;

	public object Current
	{
		get
		{
			if (_CurrentIndex < 0)
			{
				throw new InvalidOperationException("IEnumerator Pointer is invalid. Use MoveNext to advance enumerator.");
			}
			return _ParentCollection[_CurrentIndex];
		}
	}

	internal int CurrentIndex
	{
		get
		{
			return _CurrentIndex;
		}
		set
		{
			_CurrentIndex = value;
		}
	}

	public VisualCollectionEnumerator(VisualItemCollection parentCollection, bool rightToLeft)
	{
		_ParentCollection = parentCollection;
		if (rightToLeft)
		{
			_Direction = -1;
			_Start = parentCollection.Count - 1;
			_End = -1;
		}
		else
		{
			_Start = 0;
			_End = parentCollection.Count;
		}
	}

	public IEnumerator GetEnumerator()
	{
		return this;
	}

	public bool MoveNext()
	{
		if (_CurrentIndex == _End && _CurrentIndex >= 0)
		{
			throw new InvalidOperationException("Enumerator cannot advance beyond the boundaries of array. Use Reset to reset enumerator");
		}
		if (_CurrentIndex == -1)
		{
			_CurrentIndex = _Start - _Direction;
		}
		_CurrentIndex += _Direction;
		if (_CurrentIndex == _End)
		{
			return false;
		}
		return true;
	}

	public void Reset()
	{
		_CurrentIndex = -1;
	}
}
