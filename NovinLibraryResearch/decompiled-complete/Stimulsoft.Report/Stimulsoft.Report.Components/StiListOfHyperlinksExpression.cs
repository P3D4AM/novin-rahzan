using System.ComponentModel;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Components;

[TypeConverter(typeof(StiListOfHyperlinksExpressionConverter))]
public class StiListOfHyperlinksExpression : StiExpression
{
	[Browsable(false)]
	public override bool ApplyFormat => false;

	public override bool FullConvert => true;

	public override StiEvent GetDefaultEvent()
	{
		return new StiGetListOfHyperlinksEvent();
	}

	public StiListOfHyperlinksExpression()
		: this(string.Empty)
	{
	}

	public StiListOfHyperlinksExpression(string value)
		: base(value)
	{
	}
}
