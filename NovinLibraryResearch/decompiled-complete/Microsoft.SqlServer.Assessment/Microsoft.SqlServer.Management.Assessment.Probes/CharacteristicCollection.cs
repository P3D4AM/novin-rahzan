using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Microsoft.SqlServer.Management.Assessment.Probes;

[Serializable]
public class CharacteristicCollection : Dictionary<string, HashSet<string>>, ICharacteristicCollection, IDictionary<string, HashSet<string>>, ICollection<KeyValuePair<string, HashSet<string>>>, IEnumerable<KeyValuePair<string, HashSet<string>>>, IEnumerable
{
	public static CharacteristicCollection Empty { get; } = new CharacteristicCollection();


	public int Total => base.Values.Sum((HashSet<string> v) => v.Count);

	public CharacteristicCollection()
	{
	}

	public CharacteristicCollection(ICharacteristicCollection other)
		: base((IDictionary<string, HashSet<string>>)other)
	{
	}

	protected CharacteristicCollection(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}

	public CharacteristicCollection(string key, params string[] specification)
	{
		Add(key, new HashSet<string>(specification));
	}

	public CharacteristicCollection(string key, HashSet<string> specification)
	{
		Add(key, specification);
	}

	public void UnionWith(ICharacteristicCollection other)
	{
		if (other == null)
		{
			return;
		}
		foreach (KeyValuePair<string, HashSet<string>> item in other)
		{
			if (TryGetValue(item.Key, out var value))
			{
				value.UnionWith(item.Value);
			}
			else
			{
				Add(item.Key, new HashSet<string>(item.Value));
			}
		}
	}

	public bool IsSupersetOf(ICharacteristicCollection other)
	{
		if (other == null)
		{
			return true;
		}
		foreach (KeyValuePair<string, HashSet<string>> item in other)
		{
			if (item.Value != null && item.Value.Count != 0)
			{
				if (!TryGetValue(item.Key, out var value))
				{
					return false;
				}
				if (!value.IsSupersetOf(item.Value))
				{
					return false;
				}
			}
		}
		return true;
	}

	public static CharacteristicCollection Union(IEnumerable<ICharacteristicCollection> collections)
	{
		CharacteristicCollection characteristicCollection = new CharacteristicCollection();
		if (collections != null)
		{
			foreach (ICharacteristicCollection collection in collections)
			{
				characteristicCollection.UnionWith(collection);
			}
		}
		return characteristicCollection;
	}

	public static ICharacteristicCollection Union(params ICharacteristicCollection[] collections)
	{
		return Union((IEnumerable<ICharacteristicCollection>)collections);
	}

	public void ExceptWith(ICharacteristicCollection other)
	{
		if (other == null)
		{
			return;
		}
		foreach (KeyValuePair<string, HashSet<string>> item in other)
		{
			if (TryGetValue(item.Key, out var value))
			{
				value.ExceptWith(item.Value);
				if (value.Count == 0)
				{
					Remove(item.Key);
				}
			}
		}
	}

	public void Add(string key, string value)
	{
		if (!TryGetValue(key, out var value2))
		{
			value2 = new HashSet<string>();
			Add(key, value2);
		}
		value2.Add(value);
	}

	internal void IntersectWith(ICharacteristicCollection other)
	{
		if (other == null || other.Count == 0)
		{
			Clear();
			return;
		}
		List<string> list = new List<string>();
		using (Enumerator enumerator = GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<string, HashSet<string>> current = enumerator.Current;
				if (other.TryGetValue(current.Key, out var value))
				{
					current.Value.IntersectWith(value);
					if (current.Value.Count > 0)
					{
						continue;
					}
				}
				list.Add(current.Key);
			}
		}
		foreach (string item in list)
		{
			Remove(item);
		}
	}

	internal void Append(string key, IEnumerable<string> value)
	{
		if (!TryGetValue(key, out var value2))
		{
			value2 = new HashSet<string>();
			Add(key, value2);
		}
		value2.UnionWith(value);
	}
}
