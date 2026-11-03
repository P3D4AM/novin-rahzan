using System.ComponentModel;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.CrossTab.Design;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.CrossTab;

[TypeConverter(typeof(StiCrossValueExpressionConverter))]
public class StiCrossValueExpression : StiExpression
{
	public override bool FullConvert => false;

	public override StiEvent GetDefaultEvent()
	{
		return new StiGetCrossValueEvent();
	}

	public StiCrossValueExpression()
		: this(string.Empty)
	{
	}

	public StiCrossValueExpression(string value)
		: base(value)
	{
	}
}
