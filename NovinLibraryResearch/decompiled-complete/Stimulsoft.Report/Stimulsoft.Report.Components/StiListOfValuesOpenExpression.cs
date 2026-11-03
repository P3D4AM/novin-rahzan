using System.ComponentModel;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Components;

[TypeConverter(typeof(StiListOfValuesOpenExpressionConverter))]
public class StiListOfValuesOpenExpression : StiExpression
{
	[Browsable(false)]
	public override bool ApplyFormat => false;

	public override bool FullConvert => true;

	public override StiEvent GetDefaultEvent()
	{
		return new StiGetListOfValuesOpenEvent();
	}

	public StiListOfValuesOpenExpression()
		: this(string.Empty)
	{
	}

	public StiListOfValuesOpenExpression(string value)
		: base(value)
	{
	}
}
