using System.Collections;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.Assessment;

internal class ListBiDictionary<TX, TY> : IEnumerable<KeyValuePair<TX, TY>>, IEnumerable
{
	private readonly List<KeyValuePair<TX, TY>> rep = new List<KeyValuePair<TX, TY>>();

	public void Add(TX x, TY y)
	{
		rep.Add(new KeyValuePair<TX, TY>(x, y));
	}

	public bool TryGetValue(TY y, out TX x)
	{
		int num = rep.FindIndex((KeyValuePair<TX, TY> p) => y.Equals(p.Value));
		if (num >= 0)
		{
			x = rep[num].Key;
			return true;
		}
		x = default(TX);
		return false;
	}

	public bool TryGetValue(TX x, out TY y)
	{
		int num = rep.FindIndex((KeyValuePair<TX, TY> p) => x.Equals(p.Key));
		if (num >= 0)
		{
			y = rep[num].Value;
			return true;
		}
		y = default(TY);
		return false;
	}

	public IEnumerator<KeyValuePair<TX, TY>> GetEnumerator()
	{
		return rep.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
