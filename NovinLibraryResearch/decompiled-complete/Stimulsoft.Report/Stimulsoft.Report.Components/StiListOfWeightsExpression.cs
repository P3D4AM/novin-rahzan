using System.ComponentModel;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Components;

[TypeConverter(typeof(StiListOfWeightsExpressionConverter))]
public class StiListOfWeightsExpression : StiExpression
{
	[Browsable(false)]
	public override bool ApplyFormat => false;

	public override bool FullConvert => true;

	public override StiEvent GetDefaultEvent()
	{
		return new StiGetListOfWeightsEvent();
	}

	public StiListOfWeightsExpression()
		: this(string.Empty)
	{
	}

	public StiListOfWeightsExpression(string value)
		: base(value)
	{
	}
}
