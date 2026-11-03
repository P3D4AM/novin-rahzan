using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.CodeDom;

public class StiRemit
{
	internal StiComponent Component;

	internal StiComponent BuildComponent;

	internal string Name;

	internal string RunningFooterBandName;

	internal string ScriptCalcItem;

	internal string ScriptGetValue;

	internal string ScriptCondition;

	internal StiAggregateFunctionService Function;

	internal bool UsePrintEvent;

	internal bool UseColumn;

	internal StiExpression Expression;

	internal bool isRunningTotal;

	internal bool isPageTotal;

	internal bool ConnectOnlyBegin;

	public StiRemit(StiComponent Component, string name, string scriptCalcItem, string scriptGetValue, string scriptCondition, StiAggregateFunctionService function, bool usePrintEvent, bool useColumn, StiExpression expression, bool isRunningTotal)
	{
		this.Component = Component;
		BuildComponent = Component;
		Name = name;
		ScriptCalcItem = scriptCalcItem;
		ScriptGetValue = scriptGetValue;
		ScriptCondition = scriptCondition;
		Function = function;
		UsePrintEvent = usePrintEvent;
		UseColumn = useColumn;
		Expression = expression;
		this.isRunningTotal = isRunningTotal;
	}
}
