using System.Collections.Generic;

namespace Babel.ParserGenerator;

internal class ModifiedMinimalList<T> : IMinimalList<T>
{
	private readonly IMinimalList<T> parentList;

	private readonly Dictionary<int, T> overrideValues;

	private readonly List<int> modifiedKeys;

	public Dictionary<int, T> OverrideValues => overrideValues;

	public List<int> ModifiedKeys => modifiedKeys;

	public int Count => parentList.Count;

	public T this[int index]
	{
		get
		{
			if (overrideValues == null || !overrideValues.TryGetValue(index, out var value))
			{
				return parentList[index];
			}
			return value;
		}
	}

	public ModifiedMinimalList(IMinimalList<T> parentList, Dictionary<int, T> overrideValues, List<int> modifiedKeys)
	{
		this.parentList = parentList;
		this.overrideValues = overrideValues;
		this.modifiedKeys = modifiedKeys;
	}
}
