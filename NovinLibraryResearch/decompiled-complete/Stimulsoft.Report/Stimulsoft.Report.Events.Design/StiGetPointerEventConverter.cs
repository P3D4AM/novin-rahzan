using System.ComponentModel;
using System.Globalization;

namespace Stimulsoft.Report.Events.Design;

public class StiGetPointerEventConverter : StiEventConverter
{
	public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
	{
		if (value is string script)
		{
			return new StiGetPointerEvent(script);
		}
		return base.ConvertFrom(context, culture, value);
	}
}
