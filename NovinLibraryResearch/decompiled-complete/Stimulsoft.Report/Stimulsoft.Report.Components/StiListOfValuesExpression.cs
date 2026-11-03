using System.ComponentModel;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Components;

[TypeConverter(typeof(StiListOfValuesExpressionConverter))]
public class StiListOfValuesExpression : StiExpression
{
	[Browsable(false)]
	public override bool ApplyFormat => false;

	public override bool FullConvert => true;

	public override StiEvent GetDefaultEvent()
	{
		return new StiGetListOfValuesEvent();
	}

	public StiListOfValuesExpression()
		: this(string.Empty)
	{
	}

	public StiListOfValuesExpression(string value)
		: base(value)
	{
	}
}
