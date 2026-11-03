using System.ComponentModel;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Components;

[TypeConverter(typeof(StiListOfValuesLowExpressionConverter))]
public class StiListOfValuesLowExpression : StiExpression
{
	[Browsable(false)]
	public override bool ApplyFormat => false;

	public override bool FullConvert => true;

	public override StiEvent GetDefaultEvent()
	{
		return new StiGetListOfValuesLowEvent();
	}

	public StiListOfValuesLowExpression()
		: this(string.Empty)
	{
	}

	public StiListOfValuesLowExpression(string value)
		: base(value)
	{
	}
}
