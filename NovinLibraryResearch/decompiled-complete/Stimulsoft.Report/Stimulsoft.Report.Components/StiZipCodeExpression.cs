using System.ComponentModel;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Components;

[TypeConverter(typeof(StiZipCodeExpressionConverter))]
public class StiZipCodeExpression : StiUnifiedExpression
{
	[Browsable(false)]
	public override bool ApplyFormat => false;

	public override bool FullConvert => false;

	public override StiEvent GetDefaultEvent()
	{
		return new StiGetZipCodeEvent();
	}

	public StiZipCodeExpression()
		: this(string.Empty)
	{
	}

	public StiZipCodeExpression(string value)
		: base(value)
	{
	}

	public StiZipCodeExpression(StiComponent parent, string propertyName)
		: base(parent, propertyName)
	{
	}
}
