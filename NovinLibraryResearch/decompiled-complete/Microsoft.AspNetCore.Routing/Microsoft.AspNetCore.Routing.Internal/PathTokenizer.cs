using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Microsoft.AspNetCore.Routing.Internal;

public struct PathTokenizer : IReadOnlyList<StringSegment>, IEnumerable<StringSegment>, IEnumerable, IReadOnlyCollection<StringSegment>
{
	public struct Enumerator : IEnumerator<StringSegment>, IEnumerator, IDisposable
	{
		private readonly string _path;

		private int _index;

		private int _length;

		public StringSegment Current => new StringSegment(_path, _index, _length);

		object IEnumerator.Current => Current;

		public Enumerator(PathTokenizer tokenizer)
		{
			_path = tokenizer._path;
			_index = -1;
			_length = -1;
		}

		public void Dispose()
		{
		}

		public bool MoveNext()
		{
			if (_path == null || _path.Length <= 1)
			{
				return false;
			}
			if (_index == -1)
			{
				_index = 1;
			}
			else
			{
				_index += _length + 1;
			}
			if (_index >= _path.Length)
			{
				return false;
			}
			int num = _path.IndexOf('/', _index);
			if (num != -1)
			{
				_length = num - _index;
				return true;
			}
			if (_path[_path.Length - 1] == '/')
			{
				return false;
			}
			_length = _path.Length - _index;
			return true;
		}

		public void Reset()
		{
			_index = -1;
			_length = -1;
		}
	}

	private readonly string _path;

	private int _count;

	public int Count
	{
		get
		{
			if (_count == -1)
			{
				if (_path.Length == 0)
				{
					_count = 0;
					return _count;
				}
				if (_path.Length == 1)
				{
					_count = 0;
					return _count;
				}
				_count = 1;
				for (int i = 1; i < _path.Length - 1; i++)
				{
					if (_path[i] == '/')
					{
						_count++;
					}
				}
			}
			return _count;
		}
	}

	public StringSegment this[int index]
	{
		get
		{
			if (index >= Count)
			{
				throw new IndexOutOfRangeException();
			}
			int num = 0;
			int num2 = 1;
			int startIndex = 1;
			while ((startIndex = _path.IndexOf('/', startIndex)) != -1)
			{
				if (num++ == index)
				{
					return new StringSegment(_path, num2, startIndex - num2);
				}
				num2 = startIndex + 1;
				startIndex++;
			}
			return new StringSegment(_path, num2, _path.Length - num2);
		}
	}

	public PathTokenizer(PathString path)
	{
		_path = path.Value;
		_count = -1;
	}

	public Enumerator GetEnumerator()
	{
		return new Enumerator(this);
	}

	IEnumerator<StringSegment> IEnumerable<StringSegment>.GetEnumerator()
	{
		return GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
