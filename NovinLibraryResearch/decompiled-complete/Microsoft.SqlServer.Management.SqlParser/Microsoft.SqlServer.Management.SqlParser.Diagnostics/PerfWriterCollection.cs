using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Microsoft.SqlServer.Management.SqlParser.Diagnostics;

internal class PerfWriterCollection : StringWriter, IList<TextWriter>, ICollection<TextWriter>, IEnumerable<TextWriter>, IEnumerable
{
	private readonly List<TextWriter> container;

	public TextWriter this[int index]
	{
		get
		{
			return container[index];
		}
		set
		{
			container[index] = value;
		}
	}

	public int Count => container.Count;

	public bool IsReadOnly => false;

	public PerfWriterCollection()
	{
		container = new List<TextWriter>();
	}

	public override void Write(char c)
	{
		using IEnumerator<TextWriter> enumerator = GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.Write(c);
		}
	}

	public override void Write(string val)
	{
		using IEnumerator<TextWriter> enumerator = GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.Write(val);
		}
	}

	public override void Write(char[] buffer, int index, int count)
	{
		using IEnumerator<TextWriter> enumerator = GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.Write(buffer, index, count);
		}
	}

	public override void Flush()
	{
		using IEnumerator<TextWriter> enumerator = GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.Flush();
		}
	}

	public int IndexOf(TextWriter item)
	{
		return container.IndexOf(item);
	}

	public void Insert(int index, TextWriter item)
	{
		container.Insert(index, item);
	}

	public void RemoveAt(int index)
	{
		container.RemoveAt(index);
	}

	public void Add(TextWriter item)
	{
		container.Add(item);
	}

	public void Clear()
	{
		container.Clear();
	}

	public bool Contains(TextWriter item)
	{
		return container.Contains(item);
	}

	public void CopyTo(TextWriter[] array, int arrayIndex)
	{
		container.CopyTo(array, arrayIndex);
	}

	public bool Remove(TextWriter item)
	{
		return container.Remove(item);
	}

	public IEnumerator<TextWriter> GetEnumerator()
	{
		return container.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
