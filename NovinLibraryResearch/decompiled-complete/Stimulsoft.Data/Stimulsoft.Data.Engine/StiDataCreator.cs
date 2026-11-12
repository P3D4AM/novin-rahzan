using System;
using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Meters;
using Stimulsoft.Data.Extensions;
using Stimulsoft.Data.Parsers;

namespace Stimulsoft.Data.Engine;

public static class StiDataCreator
{
	public static StiDataTable Create(IStiAppDictionary dict, IEnumerable<IStiMeter> meters)
	{
		object[] source = meters?.Select((IStiMeter m) => GetData(dict, m, meters)).ToArray();
		IEnumerable<object> source2 = source.Where(ListExt.IsList);
		int rowsCount = ((!source2.Any()) ? 1 : source2.Max((object o) => ListExt.ToList(o).Count()));
		rowsCount = Math.Max(1, rowsCount);
		List<object[]> list = source.Select((object a) => Convert(a, rowsCount)).ToList();
		int num = meters.Count();
		List<object[]> list2 = new List<object[]>();
		for (int i = 0; i < rowsCount; i++)
		{
			object[] array = new object[num];
			for (int j = 0; j < num; j++)
			{
				array[j] = list[j][i];
			}
			list2.Add(array);
		}
		return new StiDataTable
		{
			Meters = meters.ToList(),
			Rows = list2
		};
	}

	private static object[] Convert(object obj, int rowsCount)
	{
		if (ListExt.IsList(obj))
		{
			IEnumerable<object> source = ListExt.ToList(obj);
			object[] array = Enumerable.Repeat(source.LastOrDefault(), rowsCount).ToArray();
			int index = 0;
			source.ToList().ForEach(delegate(object o)
			{
				array[index++] = o;
			});
			return array;
		}
		return Enumerable.Repeat(obj, rowsCount).ToArray();
	}

	private static object GetData(IStiAppDictionary dict, IStiMeter meter, IEnumerable<IStiMeter> meters)
	{
		if (meter is IStiDimensionMeter)
		{
			if (meters.Any((IStiMeter m) => m is IStiSkipNormalizeDate))
			{
				meters = null;
			}
			return new StiDimensionDataParser(dict, meter).Calculate(new object[1], meters.ToList()).FirstOrDefault();
		}
		if (meter is IStiMeasureMeter)
		{
			return new StiMeasureDataParser(dict, meter).CalculateMeter(meter);
		}
		return null;
	}
}
