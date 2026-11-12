using System;
using System.Collections;
using Stimulsoft.Blockly.Model;

namespace Stimulsoft.Blockly.Blocks.Lists;

public class ListsGetIndex : IronBlock
{
	private static Random rnd = new Random();

	public override object Evaluate(Context context)
	{
		IList list = base.Values.Evaluate("VALUE", context) as IList;
		string text = base.Fields.Get("MODE");
		string text2 = base.Fields.Get("WHERE");
		int num = -1;
		num = text2 switch
		{
			"FROM_START" => Convert.ToInt32(base.Values.Evaluate("AT", context)) - 1, 
			"FROM_END" => list.Count - Convert.ToInt32(base.Values.Evaluate("AT", context)), 
			"FIRST" => 0, 
			"LAST" => list.Count - 1, 
			"RANDOM" => rnd.Next(list.Count), 
			_ => throw new NotSupportedException("unsupported where (" + text2 + ")"), 
		};
		switch (text)
		{
		case "GET":
			return list[num];
		case "GET_REMOVE":
		{
			object result = list[num];
			list.RemoveAt(num);
			return result;
		}
		case "REMOVE":
			list.RemoveAt(num);
			return null;
		default:
			throw new NotSupportedException("unsupported mode (" + text + ")");
		}
	}
}
