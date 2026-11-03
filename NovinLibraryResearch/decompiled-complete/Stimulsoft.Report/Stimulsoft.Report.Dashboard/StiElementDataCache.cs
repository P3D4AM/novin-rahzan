using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using Stimulsoft.Base;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Base.Meters;
using Stimulsoft.Data.Engine;

namespace Stimulsoft.Report.Dashboard;

public static class StiElementDataCache
{
	private static object lockObject = new object();

	private static object lockCacheObject = new object();

	private static BackgroundWorker worker;

	private static List<IStiElement> elements = new List<IStiElement>();

	private static Dictionary<string, StiDataTable> cache = new Dictionary<string, StiDataTable>();

	private static IStiPivotTableCreator pivotCreator;

	public static event EventHandler DataInit;

	public static StiDataTable TryToGetOrCreate(IStiElement element, StiDataRequestOption option = 3)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			return GetOrCreate(element, option);
		}
		catch
		{
			return null;
		}
	}

	public static StiDataTable GetOrCreate(IStiElement element, StiDataRequestOption option = 3)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		lock (lockObject)
		{
			StiDataTable val = Get(element);
			if (val != null)
			{
				return val;
			}
			val = Create(element, option);
			Add(element, val);
			return val;
		}
	}

	public static IStiPivotGridContainer GetOrCreatePivot(IStiPivotTableElement element, IStiPivotTableCreator creator, StiDataRequestOption option = 3)
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		lock (lockObject)
		{
			if (StiPivotToContainerCache.Contains(element) && Get(element) != null)
			{
				return StiPivotToContainerCache.Get(element);
			}
			StiDataTable val = Get(element);
			if (val == null)
			{
				val = Create(element, option);
				Add(element, val);
				StiPivotToContainerCache.Remove(element);
			}
			creator.Create(element, val);
			return StiPivotToContainerCache.Get(element);
		}
	}

	public static StiDataTable GetOrCreateWithProgress(IStiElement element, StiDataRequestOption option = 3)
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		lock (lockObject)
		{
			StiDataTable val = Get(element);
			if (val != null)
			{
				return val;
			}
			if (elements.Contains(element))
			{
				return null;
			}
			StiComponentProgressHelper.Add((IStiAppComponent)element);
			elements.Add(element);
			InitWorker(option);
			return null;
		}
	}

	public static IStiPivotGridContainer GetOrCreatePivotWithProgress(IStiPivotTableElement element, IStiPivotTableCreator creator, StiDataRequestOption option = 3)
	{
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		lock (lockObject)
		{
			if (StiPivotToContainerCache.Contains(element) && Get(element) != null)
			{
				return StiPivotToContainerCache.Get(element);
			}
			if (elements.Contains(element))
			{
				return null;
			}
			pivotCreator = creator;
			StiComponentProgressHelper.Add((IStiAppComponent)element);
			elements.Add(element);
			InitWorker(option);
			return null;
		}
	}

	public static StiDataTable Get(IStiElement element)
	{
		lock (lockCacheObject)
		{
			string key = GetKey(element);
			return cache.ContainsKey(key) ? cache[key] : null;
		}
	}

	public static StiDataTable Create(IStiElement element, StiDataRequestOption option)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Invalid comparison between Unknown and I4
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Invalid comparison between Unknown and I4
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		bool flag = (option & 4) > 0;
		IStiDashboard stiDashboard = element.Page as IStiDashboard;
		List<IStiMeter> meters = element.GetMeters();
		if (meters == null || !meters.Any())
		{
			return StiDataTable.NullTable;
		}
		string text = StiGroupElementHelper.GetGroup(element);
		List<StiDataFilterRule> list = GetUserFilters(element, stiDashboard);
		List<StiDataSortRule> userSorts = GetUserSorts(element, option);
		List<StiDataFilterRule> dataFilters = GetDataFilters(element);
		List<StiDataActionRule> list2 = (flag ? null : GetTransformActions(element));
		List<StiDataFilterRule> list3 = (flag ? null : GetTransformFilters(element));
		List<StiDataSortRule> list4 = (flag ? null : GetTransformSorts(element));
		List<StiDataFilterRule> drillDownFilters = GetDrillDownFilters(element);
		IStiElement stiElement2;
		if ((int)StiDataJoiner.JoinEngine != 3)
		{
			IStiElement stiElement = stiDashboard;
			stiElement2 = stiElement;
		}
		else
		{
			stiElement2 = element;
		}
		IStiElement stiElement3 = stiElement2;
		IStiCrossFiltering val = (IStiCrossFiltering)((element is IStiCrossFiltering) ? element : null);
		if (val != null && !val.CrossFiltering)
		{
			text = string.Empty;
			list = null;
		}
		return StiDataAnalyzer.Analyze((IStiQueryObject)stiElement3, text, (IEnumerable<IStiMeter>)meters, option, (IEnumerable<StiDataSortRule>)userSorts, (IEnumerable<StiDataFilterRule>)list, (IEnumerable<StiDataFilterRule>)dataFilters, (IEnumerable<StiDataActionRule>)null, (IEnumerable<StiDataSortRule>)list4, (IEnumerable<StiDataFilterRule>)list3, (IEnumerable<StiDataActionRule>)list2, (IEnumerable<StiDataFilterRule>)drillDownFilters);
	}

	internal static void Add(IStiElement element, StiDataTable dataTable)
	{
		lock (lockCacheObject)
		{
			string key = GetKey(element);
			cache[key] = dataTable;
		}
	}

	private static void InitWorker(StiDataRequestOption option = 3)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		if (worker != null)
		{
			return;
		}
		worker = new BackgroundWorker();
		worker.DoWork += delegate
		{
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			while (true)
			{
				if (elements.Count == 0)
				{
					Thread.Sleep(10);
				}
				else
				{
					IStiElement stiElement;
					lock (lockObject)
					{
						stiElement = elements.FirstOrDefault();
						elements.Remove(stiElement);
					}
					try
					{
						StiDataTable val = Get(stiElement);
						if (val == null)
						{
							val = Create(stiElement, option);
							Add(stiElement, val);
							if (stiElement is IStiPivotTableElement)
							{
								StiPivotToContainerCache.Remove(stiElement as IStiPivotTableElement);
							}
						}
						if (stiElement is IStiPivotTableElement)
						{
							pivotCreator.Create(stiElement as IStiPivotTableElement, val);
						}
					}
					catch (Exception)
					{
						Add(stiElement, StiDataTable.NullTable);
					}
					finally
					{
						StiComponentProgressHelper.Remove((IStiAppComponent)stiElement, false);
						StiElementDataCache.DataInit?.Invoke(stiElement, EventArgs.Empty);
					}
				}
			}
		};
		worker.RunWorkerAsync();
	}

	public static string GetKey(IStiElement element)
	{
		string text = StiAppKey.GetOrGeneratedKey((IStiReportComponent)element) ?? string.Empty;
		List<IStiMeter> list = element?.GetMeters();
		if (list == null || !list.Any())
		{
			return text + ".0";
		}
		IStiDashboard stiDashboard = element.Page as IStiDashboard;
		int index = 0;
		List<string> list2 = list.Select((IStiMeter m) => index++.ToString() + m.GetUniqueCode()).ToList();
		List<StiDataFilterRule> list3 = stiDashboard?.GetUserFilters(element);
		IEnumerable<string> enumerable = list3?.Select((StiDataFilterRule s) => index++.ToString() + ((StiDataRule)s).GetUniqueCode());
		if (enumerable != null)
		{
			list2.AddRange(enumerable);
		}
		IEnumerable<string> enumerable2 = GetDataFilters(element)?.Select((StiDataFilterRule s) => index++.ToString() + ((StiDataRule)s).GetUniqueCode());
		if (enumerable2 != null)
		{
			list2.AddRange(enumerable2);
		}
		IEnumerable<string> enumerable3 = GetDrillDownFilters(element)?.Select((StiDataFilterRule s) => index++.ToString() + ((StiDataRule)s).GetUniqueCode());
		if (enumerable3 != null)
		{
			list2.AddRange(enumerable3);
		}
		IStiElement obj = ((element is IStiUserSorts) ? element : null);
		IEnumerable<string> enumerable4 = ((obj != null) ? ((IStiUserSorts)obj).UserSorts.Select((StiDataSortRule s) => index++.ToString() + ((StiDataRule)s).GetUniqueCode()) : null);
		if (enumerable4 != null)
		{
			list2.AddRange(enumerable4);
		}
		if (element is IStiPivotTableElement)
		{
			string text2 = (element as IStiTitleElement)?.Title?.Text ?? string.Empty;
			list2.Add(text2.GetHashCode().ToString());
			list2.AddRange((element as IStiPivotTableElement).PivotTableConditions.Select((IStiPivotTableElementCondition c) => c.GetUniqueCode().ToString()).ToList());
			list2.Add((element as IStiPivotTableElement).SummaryDirection.ToString());
		}
		long num = 0L;
		foreach (string item in list2)
		{
			num += item.GetHashCode();
		}
		int filterRulesHash = StiDataFilterRuleHelper.GetFilterRulesHash(((IStiAppComponent)stiDashboard).GetApp(), (IEnumerable<StiDataFilterRule>)list3);
		num += filterRulesHash;
		return $"{text}.{num}";
	}

	public static void CleanCache(string reportKey)
	{
		lock (lockObject)
		{
			if (reportKey == null)
			{
				cache.Clear();
				return;
			}
			cache.Keys.Where((string k) => k.StartsWith(reportKey)).ToList().ForEach(delegate(string k)
			{
				cache.Remove(k);
			});
		}
	}

	public static void Remove(string key)
	{
		lock (lockObject)
		{
			if (cache.ContainsKey(key))
			{
				cache.Remove(key);
			}
		}
	}

	private static List<StiDataFilterRule> GetUserFilters(IStiElement element, IStiDashboard dashboard)
	{
		return dashboard?.GetUserFilters(element);
	}

	private static List<StiDataSortRule> GetUserSorts(IStiElement element, StiDataRequestOption option)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Invalid comparison between Unknown and I4
		if ((option & 2) <= 0)
		{
			return null;
		}
		IStiElement obj = ((element is IStiUserSorts) ? element : null);
		if (obj == null)
		{
			return null;
		}
		return ((IStiUserSorts)obj).UserSorts.ToList();
	}

	private static List<StiDataFilterRule> GetDataFilters(IStiElement element)
	{
		IStiElement obj = ((element is IStiDataFilters) ? element : null);
		if (obj == null)
		{
			return null;
		}
		return ((IStiDataFilters)obj).DataFilters;
	}

	private static List<StiDataActionRule> GetTransformActions(IStiElement element)
	{
		IStiElement obj = ((element is IStiTransformActions) ? element : null);
		if (obj == null)
		{
			return null;
		}
		return ((IStiTransformActions)obj).TransformActions;
	}

	private static List<StiDataFilterRule> GetTransformFilters(IStiElement element)
	{
		IStiElement obj = ((element is IStiTransformFilters) ? element : null);
		if (obj == null)
		{
			return null;
		}
		return ((IStiTransformFilters)obj).TransformFilters;
	}

	private static List<StiDataSortRule> GetTransformSorts(IStiElement element)
	{
		IStiElement obj = ((element is IStiTransformSorts) ? element : null);
		if (obj == null)
		{
			return null;
		}
		return ((IStiTransformSorts)obj).TransformSorts;
	}

	private static List<StiDataFilterRule> GetDrillDownFilters(IStiElement element)
	{
		IStiElement obj = ((element is IStiDrillDownElement) ? element : null);
		if (obj == null)
		{
			return null;
		}
		return ((IStiDrillDownElement)obj).DrillDownFilters;
	}
}
