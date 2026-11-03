using System.ComponentModel;
using System.Globalization;

namespace Stimulsoft.Report.Events.Design;

public class StiMouseUpEventConverter : StiEventConverter
{
	public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
	{
		if (value is string script)
		{
			return new StiMouseUpEvent(script);
		}
		return base.ConvertFrom(context, culture, value);
	}
}
