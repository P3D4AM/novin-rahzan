using System.Collections;

namespace Stimulsoft.Base.Serializing;

public class StiGraphs
{
	private Hashtable hashCode = new Hashtable();

	private Hashtable hashObj = new Hashtable();

	public object this[int code]
	{
		get
		{
			return hashCode[code];
		}
		set
		{
			hashCode[code] = value;
		}
	}

	public int this[object obj]
	{
		get
		{
			object obj2 = hashObj[obj];
			if (obj2 == null)
			{
				return -1;
			}
			return (int)obj2;
		}
		set
		{
			hashCode[obj] = value;
		}
	}

	public void Add(object obj)
	{
		if (obj != null && hashObj[obj] == null)
		{
			hashCode.Add(hashCode.Count, obj);
			hashObj.Add(obj, hashObj.Count);
		}
	}

	public void Add(object obj, int index)
	{
		if (obj != null && hashObj[obj] == null)
		{
			hashCode.Add(index, obj);
			hashObj.Add(obj, index);
		}
	}
}
