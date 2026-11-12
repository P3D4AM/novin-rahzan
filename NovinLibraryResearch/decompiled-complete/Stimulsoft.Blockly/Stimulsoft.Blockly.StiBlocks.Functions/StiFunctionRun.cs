using System;
using System.Collections.Generic;
using Stimulsoft.Base;
using Stimulsoft.Blockly.Blocks;
using Stimulsoft.Blockly.Model;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Blockly.StiBlocks.Functions;

public class StiFunctionRun : IronBlock
{
	public override object Evaluate(Context context)
	{
		StiFunction function = StiBlocklyFunctionBlockKeyCache.GetFunction(base.Type);
		List<object> list = new List<object>();
		for (int i = 0; i < function.ArgumentNames.Length; i++)
		{
			string name = function.ArgumentNames[i];
			Type conversionType = function.ArgumentTypes[i];
			object value = base.Values.Evaluate(name, context);
			object item = Convert.ChangeType(value, conversionType);
			list.Add(item);
		}
		return ((IStiAppFunction)function).Invoke((IEnumerable<object>)list.ToArray());
	}
}
