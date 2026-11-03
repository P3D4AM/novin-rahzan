using System.ComponentModel;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Components;

[TypeConverter(typeof(StiValueOpenExpressionConverter))]
public class StiValueOpenExpression : StiExpression
{
	[Browsable(false)]
	public override bool ApplyFormat => false;

	public override bool FullConvert => true;

	public override StiEvent GetDefaultEvent()
	{
		return new StiGetValueOpenEvent();
	}

	public StiValueOpenExpression()
		: this(string.Empty)
	{
	}

	public StiValueOpenExpression(string value)
		: base(value)
	{
	}
}
