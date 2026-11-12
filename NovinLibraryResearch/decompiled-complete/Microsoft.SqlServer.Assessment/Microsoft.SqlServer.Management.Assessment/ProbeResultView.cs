using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.SqlServer.Management.Assessment;

internal class ProbeResultView : IDataView, IReadOnlyList<IDataRow>, IReadOnlyCollection<IDataRow>, IEnumerable<IDataRow>, IEnumerable
{
	private IDictionary<IEvaluationContext, IDataView> Calls { get; }

	public string Name { get; }

	public int Count => Calls.Sum((KeyValuePair<IEvaluationContext, IDataView> p) => p.Value.Count);

	public IDataRow this[int index]
	{
		get
		{
			foreach (KeyValuePair<IEvaluationContext, IDataView> call in Calls)
			{
				if (index < call.Value.Count)
				{
					return new ExtraDataContext(Name, call.Key, call.Value, index);
				}
				index -= call.Value.Count;
			}
			throw new ArgumentOutOfRangeException("index");
		}
	}

	public ProbeResultView(string name, IDictionary<IEvaluationContext, IDataView> calls)
	{
		Name = name;
		Calls = calls;
	}

	public IEnumerator<IDataRow> GetEnumerator()
	{
		foreach (KeyValuePair<IEvaluationContext, IDataView> p in Calls)
		{
			for (int i = 0; i < p.Value.Count; i++)
			{
				yield return new ExtraDataContext(Name, p.Key, p.Value, i);
			}
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
