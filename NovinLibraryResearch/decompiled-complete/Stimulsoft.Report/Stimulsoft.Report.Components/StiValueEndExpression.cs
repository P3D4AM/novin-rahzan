using System.ComponentModel;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Components;

[TypeConverter(typeof(StiValueEndExpressionConverter))]
public class StiValueEndExpression : StiExpression
{
	[Browsable(false)]
	public override bool ApplyFormat => false;

	public override bool FullConvert => true;

	public override StiEvent GetDefaultEvent()
	{
		return new StiGetValueEndEvent();
	}

	public StiValueEndExpression()
		: this(string.Empty)
	{
	}

	public StiValueEndExpression(string value)
		: base(value)
	{
	}
}
