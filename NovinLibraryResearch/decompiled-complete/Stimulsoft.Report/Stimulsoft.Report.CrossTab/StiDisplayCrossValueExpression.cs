using System.ComponentModel;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.CrossTab.Design;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.CrossTab;

[TypeConverter(typeof(StiDisplayCrossValueExpressionConverter))]
public class StiDisplayCrossValueExpression : StiExpression
{
	public override bool FullConvert => false;

	public override StiEvent GetDefaultEvent()
	{
		return new StiGetDisplayCrossValueEvent();
	}

	public StiDisplayCrossValueExpression()
		: this(string.Empty)
	{
	}

	public StiDisplayCrossValueExpression(string value)
		: base(value)
	{
	}
}
