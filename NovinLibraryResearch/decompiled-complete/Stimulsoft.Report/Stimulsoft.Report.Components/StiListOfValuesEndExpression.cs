using System.ComponentModel;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Components;

[TypeConverter(typeof(StiListOfValuesEndExpressionConverter))]
public class StiListOfValuesEndExpression : StiExpression
{
	[Browsable(false)]
	public override bool ApplyFormat => false;

	public override bool FullConvert => true;

	public override StiEvent GetDefaultEvent()
	{
		return new StiGetListOfValuesEndEvent();
	}

	public StiListOfValuesEndExpression()
		: this(string.Empty)
	{
	}

	public StiListOfValuesEndExpression(string value)
		: base(value)
	{
	}
}
