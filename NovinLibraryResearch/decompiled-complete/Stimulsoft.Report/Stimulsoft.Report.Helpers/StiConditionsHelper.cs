using System;
using System.Collections;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Helpers;

public sealed class StiConditionsHelper
{
	public static StiConditionsCollection GetConditions(StiComponentsCollection comps, ref Hashtable globalConditions)
	{
		StiConditionsCollection stiConditionsCollection = new StiConditionsCollection();
		foreach (StiComponent comp in comps)
		{
			StiConditionsCollection conditions = comp.Conditions;
			if (conditions.Count <= 0)
			{
				continue;
			}
			foreach (StiBaseCondition item in conditions)
			{
				string key = (string)(item.Tag = Guid.NewGuid().ToString().Replace("-", ""));
				globalConditions.Add(key, comp);
			}
			stiConditionsCollection.AddRange(conditions);
		}
		return stiConditionsCollection;
	}

	public static void SetConditions(StiComponentsCollection comps, StiConditionsCollection conditions, Hashtable globalConditions)
	{
		foreach (StiComponent comp in comps)
		{
			comp.Conditions.Clear();
		}
		foreach (StiBaseCondition condition in conditions)
		{
			if (condition.Tag != null && globalConditions.Contains(condition.Tag))
			{
				StiBaseCondition stiBaseCondition2 = condition.Clone() as StiBaseCondition;
				stiBaseCondition2.Tag = null;
				((StiComponent)globalConditions[condition.Tag]).Conditions.Add(stiBaseCondition2);
			}
			else
			{
				SetConditionAllComponents(condition, comps);
			}
		}
	}

	private static void SetConditionAllComponents(StiBaseCondition condition, StiComponentsCollection comps)
	{
		foreach (StiComponent comp in comps)
		{
			StiBaseCondition stiBaseCondition = condition.Clone() as StiBaseCondition;
			stiBaseCondition.Tag = null;
			comp.Conditions.Add(stiBaseCondition);
		}
	}
}
