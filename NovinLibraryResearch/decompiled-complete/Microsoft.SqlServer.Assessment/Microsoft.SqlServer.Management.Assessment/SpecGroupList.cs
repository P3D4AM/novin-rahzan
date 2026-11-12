using System.Collections;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.Assessment;

internal class SpecGroupList : IEnumerable<KeyValuePair<HashSet<string>, IReadOnlyList<ProbeRequirementsError>>>, IEnumerable
{
	private List<HashSet<string>> SpecList { get; } = new List<HashSet<string>>();


	private List<List<ProbeRequirementsError>> ErrorList { get; } = new List<List<ProbeRequirementsError>>();


	public void Add(HashSet<string> specification, ProbeRequirementsError err)
	{
		int count = SpecList.Count;
		if (count == 0)
		{
			SpecList.Add(specification);
			List<ProbeRequirementsError> item = new List<ProbeRequirementsError> { err };
			ErrorList.Add(item);
			return;
		}
		for (int i = 0; i < count; i++)
		{
			HashSet<string> hashSet = SpecList[i].SplitWith(specification);
			if (hashSet.Count > 0)
			{
				List<ProbeRequirementsError> item2 = new List<ProbeRequirementsError>(ErrorList[i]) { err };
				SpecList.Add(hashSet);
				ErrorList.Add(item2);
			}
			if (specification.Count == 0)
			{
				break;
			}
		}
	}

	public IEnumerator<KeyValuePair<HashSet<string>, IReadOnlyList<ProbeRequirementsError>>> GetEnumerator()
	{
		for (int i = 0; i < SpecList.Count; i++)
		{
			if (SpecList[i].Count > 0)
			{
				yield return new KeyValuePair<HashSet<string>, IReadOnlyList<ProbeRequirementsError>>(SpecList[i], ErrorList[i]);
			}
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
