using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SqlServer.Management.Assessment.Configuration.Customization;
using Newtonsoft.Json;

namespace Microsoft.SqlServer.Management.Assessment.Configuration;

internal class RuleCollection : ICollection<CheckRule>, IEnumerable<CheckRule>, IEnumerable
{
	public IList<CheckDefinition> CheckDefinitions { get; } = new List<CheckDefinition>();


	public IList<CheckChange> CheckChanges { get; } = new List<CheckChange>();


	public IList<CheckSpecialization> CheckSpecializations { get; } = new List<CheckSpecialization>();


	public int Count => CheckDefinitions.Count + CheckChanges.Count + CheckSpecializations.Count;

	public bool IsReadOnly => false;

	[JsonConstructor]
	public RuleCollection(IEnumerable<CheckRule> items)
	{
		foreach (CheckRule item in items)
		{
			Add(item);
		}
	}

	public IEnumerator<CheckRule> GetEnumerator()
	{
		return CheckDefinitions.Concat(((IEnumerable<CheckRule>)CheckChanges).Concat((IEnumerable<CheckRule>)CheckSpecializations)).GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	public void Add(CheckRule item)
	{
		if (!(item is CheckChange item2))
		{
			if (!(item is CheckSpecialization item3))
			{
				if (item is CheckDefinition item4)
				{
					CheckDefinitions.Add(item4);
				}
			}
			else
			{
				CheckSpecializations.Add(item3);
			}
		}
		else
		{
			CheckChanges.Add(item2);
		}
	}

	public void Clear()
	{
		CheckDefinitions.Clear();
		CheckChanges.Clear();
		CheckSpecializations.Clear();
	}

	public bool Contains(CheckRule item)
	{
		if (!CheckDefinitions.Contains(item) && !CheckChanges.Contains(item))
		{
			return CheckSpecializations.Contains(item);
		}
		return true;
	}

	public void CopyTo(CheckRule[] array, int arrayIndex)
	{
		throw new NotImplementedException();
	}

	public bool Remove(CheckRule item)
	{
		if (!(item is CheckDefinition item2))
		{
			if (!(item is CheckChange item3))
			{
				if (item is CheckSpecialization item4)
				{
					return CheckSpecializations.Remove(item4);
				}
				return false;
			}
			return CheckChanges.Remove(item3);
		}
		return CheckDefinitions.Remove(item2);
	}
}
