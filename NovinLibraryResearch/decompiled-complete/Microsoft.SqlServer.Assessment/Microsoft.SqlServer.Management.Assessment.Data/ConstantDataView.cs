using System;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.Assessment.Data;

internal class ConstantDataView : IDataView, IReadOnlyList<IDataRow>, IReadOnlyCollection<IDataRow>, IEnumerable<IDataRow>, IEnumerable
{
	private readonly Measurement values;

	public int Count => 1;

	public IDataRow this[int index]
	{
		get
		{
			if (index != 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			return values;
		}
	}

	public ConstantDataView(int capacity)
	{
		values = new Measurement(capacity);
	}

	public ConstantDataView(Measurement data)
	{
		values = data;
	}

	public ConstantDataView(IDictionary<string, object> dictionary)
	{
		values = new Measurement(dictionary);
	}

	public ConstantDataView(string key, object value)
	{
		values = new Measurement { { key, value } };
	}

	public IEnumerator<IDataRow> GetEnumerator()
	{
		yield return values;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
