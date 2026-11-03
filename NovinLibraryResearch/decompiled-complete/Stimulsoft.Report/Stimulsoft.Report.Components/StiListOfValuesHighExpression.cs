using System.ComponentModel;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Components;

[TypeConverter(typeof(StiListOfValuesHighExpressionConverter))]
public class StiListOfValuesHighExpression : StiExpression
{
	[Browsable(false)]
	public override bool ApplyFormat => false;

	public override bool FullConvert => true;

	public override StiEvent GetDefaultEvent()
	{
		return new StiGetListOfValuesHighEvent();
	}

	public StiListOfValuesHighExpression()
		: this(string.Empty)
	{
	}

	public StiListOfValuesHighExpression(string value)
		: base(value)
	{
	}
}
