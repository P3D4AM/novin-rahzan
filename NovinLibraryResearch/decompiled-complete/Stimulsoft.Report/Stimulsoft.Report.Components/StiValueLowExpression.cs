using System.ComponentModel;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Components;

[TypeConverter(typeof(StiValueLowExpressionConverter))]
public class StiValueLowExpression : StiExpression
{
	[Browsable(false)]
	public override bool ApplyFormat => false;

	public override bool FullConvert => true;

	public override StiEvent GetDefaultEvent()
	{
		return new StiGetValueLowEvent();
	}

	public StiValueLowExpression()
		: this(string.Empty)
	{
	}

	public StiValueLowExpression(string value)
		: base(value)
	{
	}
}
