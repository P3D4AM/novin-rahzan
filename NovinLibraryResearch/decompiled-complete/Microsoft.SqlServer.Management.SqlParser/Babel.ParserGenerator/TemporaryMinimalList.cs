using System.Collections.Generic;

namespace Babel.ParserGenerator;

internal class TemporaryMinimalList<T> : IWriteableMinimalList<T>, IMinimalList<T>
{
	private readonly IMinimalList<T> parentList;

	private Dictionary<int, T> overrideValues;

	public int Count => parentList.Count;

	public T this[int index]
	{
		get
		{
			if (overrideValues != null && overrideValues.TryGetValue(index, out var value))
			{
				return value;
			}
			return parentList[index];
		}
		set
		{
			if (overrideValues == null)
			{
				overrideValues = new Dictionary<int, T>();
			}
			overrideValues[index] = value;
		}
	}

	internal TemporaryMinimalList(IMinimalList<T> parentList)
	{
		this.parentList = parentList;
	}
}
