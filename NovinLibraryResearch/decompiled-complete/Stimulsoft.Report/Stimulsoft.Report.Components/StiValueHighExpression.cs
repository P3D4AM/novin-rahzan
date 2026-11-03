using System.ComponentModel;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Components;

[TypeConverter(typeof(StiValueHighExpressionConverter))]
public class StiValueHighExpression : StiExpression
{
	[Browsable(false)]
	public override bool ApplyFormat => false;

	public override bool FullConvert => true;

	public override StiEvent GetDefaultEvent()
	{
		return new StiGetValueHighEvent();
	}

	public StiValueHighExpression()
		: this(string.Empty)
	{
	}

	public StiValueHighExpression(string value)
		: base(value)
	{
	}
}
