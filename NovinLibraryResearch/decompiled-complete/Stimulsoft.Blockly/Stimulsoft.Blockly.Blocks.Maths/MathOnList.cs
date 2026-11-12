using System;
using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Blockly.Model;

namespace Stimulsoft.Blockly.Blocks.Maths;

public class MathOnList : IronBlock
{
	private static Random rnd = new Random();

	public override object Evaluate(Context context)
	{
		string text = base.Fields.Get("OP");
		IEnumerable<object> source = base.Values.Evaluate("LIST", context) as IEnumerable<object>;
		double[] array = source.Select((object x) => (double)x).ToArray();
		switch (text)
		{
		case "SUM":
			return array.Sum();
		case "MIN":
			return array.Min();
		case "MAX":
			return array.Max();
		case "AVERAGE":
			return array.Average();
		case "MEDIAN":
			return Median(array);
		case "RANDOM":
			if (!array.Any())
			{
				return null;
			}
			return array[rnd.Next(array.Count())];
		case "MODE":
			if (!array.Any())
			{
				return null;
			}
			return (from x in array
				group x by x into x
				orderby x.Count() descending
				select x).First().Key;
		case "STD_DEV":
			throw new NotImplementedException("OP " + text + " not implemented");
		default:
			throw new ApplicationException("Unknown OP " + text);
		}
	}

	private object Median(IEnumerable<double> values)
	{
		if (!values.Any())
		{
			return null;
		}
		double[] array = values.OrderBy((double x) => x).ToArray();
		double num = (double)(array.Length - 1) / 2.0;
		return (array[(int)num] + array[(int)(num + 0.5)]) / 2.0;
	}
}
