using Stimulsoft.Base;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Blockly;

public static class StiObjConverter
{
	public static double ToDouble(object obj)
	{
		return (double)StiConvert.ChangeType(GetValue(obj), typeof(double));
	}

	public static object GetValue(object obj)
	{
		if (obj is StiExpression stiExpression)
		{
			return stiExpression.Value;
		}
		return obj;
	}
}
