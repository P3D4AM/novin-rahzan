using System;
using System.Collections;
using System.Collections.Generic;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiStackedColumnAreaCoreXF : StiAxisAreaCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "StackedColumn");

	public override int Position => 1;

	private List<List<double>> PrepareSeriesRange(Type seriesType)
	{
		List<double> list = new List<double>();
		List<double> list2 = new List<double>();
		List<double> list3 = new List<double>();
		List<double> list4 = new List<double>();
		List<IStiSeries> series = GetSeries();
		for (int i = 0; i < ValuesCount; i++)
		{
			foreach (IStiSeries item in series)
			{
				if (!(item.GetType() == seriesType))
				{
					continue;
				}
				if (item.YAxis == StiSeriesYAxis.LeftYAxis)
				{
					list.Add(0.0);
					list2.Add(0.0);
					if (i < item.Values.Length)
					{
						double? num = item.Values[i];
						if (num > 0.0)
						{
							list2[i] += num.GetValueOrDefault();
						}
						else
						{
							list[i] += num.GetValueOrDefault();
						}
					}
					continue;
				}
				list3.Add(0.0);
				list4.Add(0.0);
				if (i < item.Values.Length)
				{
					double? num2 = item.Values[i];
					if (num2 > 0.0)
					{
						list4[i] += num2.GetValueOrDefault();
					}
					else
					{
						list3[i] += num2.GetValueOrDefault();
					}
				}
			}
		}
		return new List<List<double>> { list, list2, list3, list4 };
	}

	protected override void PrepareRange(IStiAxis specXAxis, IStiAxis specXTopAxis, IStiAxis specYAxis, IStiAxis specYRightAxis)
	{
		List<IStiSeries> series = GetSeries();
		Hashtable hashtable = new Hashtable();
		foreach (IStiSeries item in series)
		{
			hashtable[item.GetType()] = item.GetType();
		}
		List<double> list = new List<double>();
		List<double> list2 = new List<double>();
		List<double> list3 = new List<double>();
		List<double> list4 = new List<double>();
		foreach (Type value in hashtable.Values)
		{
			List<List<double>> list5 = PrepareSeriesRange(value);
			int num = 0;
			foreach (double item2 in list5[0])
			{
				if (list.Count <= num)
				{
					list.Add(item2);
				}
				else
				{
					list[num] = Math.Min(list[num], item2);
				}
				num++;
			}
			num = 0;
			foreach (double item3 in list5[1])
			{
				if (list2.Count <= num)
				{
					list2.Add(item3);
				}
				else
				{
					list2[num] = Math.Max(list2[num], item3);
				}
				num++;
			}
			num = 0;
			foreach (double item4 in list5[2])
			{
				if (list3.Count <= num)
				{
					list3.Add(item4);
				}
				else
				{
					list3[num] = Math.Min(list3[num], item4);
				}
				num++;
			}
			num = 0;
			foreach (double item5 in list5[3])
			{
				if (list4.Count <= num)
				{
					list4.Add(item5);
				}
				else
				{
					list4[num] = Math.Max(list4[num], item5);
				}
				num++;
			}
		}
		if (list.Count == 0)
		{
			list = list3;
		}
		if (list2.Count == 0)
		{
			list2 = list4;
		}
		if (list3.Count == 0)
		{
			list3 = list;
		}
		if (list4.Count == 0)
		{
			list4 = list2;
		}
		bool flag = true;
		foreach (double item6 in list)
		{
			if (flag)
			{
				specYAxis.Info.Minimum = item6;
				flag = false;
			}
			else
			{
				specYAxis.Info.Minimum = Math.Min(item6, specYAxis.Info.Minimum);
			}
		}
		flag = true;
		foreach (double item7 in list2)
		{
			if (flag)
			{
				specYAxis.Info.Maximum = item7;
				flag = false;
			}
			else
			{
				specYAxis.Info.Maximum = Math.Max(item7, specYAxis.Info.Maximum);
			}
		}
		flag = true;
		foreach (double item8 in list3)
		{
			if (flag)
			{
				specYRightAxis.Info.Minimum = item8;
				flag = false;
			}
			else
			{
				specYRightAxis.Info.Minimum = Math.Min(item8, specYRightAxis.Info.Minimum);
			}
		}
		flag = true;
		foreach (double item9 in list4)
		{
			if (flag)
			{
				specYRightAxis.Info.Maximum = item9;
				flag = false;
			}
			else
			{
				specYRightAxis.Info.Maximum = Math.Max(item9, specYRightAxis.Info.Maximum);
			}
		}
	}

	public StiStackedColumnAreaCoreXF(IStiArea area)
		: base(area)
	{
	}
}
