using System.ComponentModel;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Components;

[TypeConverter(typeof(StiListOfTagsExpressionConverter))]
public class StiListOfTagsExpression : StiExpression
{
	[Browsable(false)]
	public override bool ApplyFormat => false;

	public override bool FullConvert => true;

	public override StiEvent GetDefaultEvent()
	{
		return new StiGetListOfTagsEvent();
	}

	public StiListOfTagsExpression()
		: this(string.Empty)
	{
	}

	public StiListOfTagsExpression(string value)
		: base(value)
	{
	}
}
