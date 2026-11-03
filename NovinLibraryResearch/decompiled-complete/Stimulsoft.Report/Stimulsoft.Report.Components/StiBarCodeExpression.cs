using System.ComponentModel;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Components;

[TypeConverter(typeof(StiBarCodeExpressionConverter))]
public class StiBarCodeExpression : StiUnifiedExpression
{
	[Browsable(false)]
	public override bool ApplyFormat => false;

	public override bool FullConvert => true;

	public override StiEvent GetDefaultEvent()
	{
		return new StiGetBarCodeEvent();
	}

	public StiBarCodeExpression()
		: this(string.Empty)
	{
	}

	public StiBarCodeExpression(string value)
		: base(value)
	{
	}

	public StiBarCodeExpression(StiComponent parent, string propertyName)
		: base(parent, propertyName)
	{
	}
}
