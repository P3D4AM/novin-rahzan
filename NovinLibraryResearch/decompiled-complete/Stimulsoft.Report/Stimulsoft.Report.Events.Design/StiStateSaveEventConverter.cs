using System.ComponentModel;
using System.Globalization;

namespace Stimulsoft.Report.Events.Design;

public class StiStateSaveEventConverter : StiEventConverter
{
	public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
	{
		if (value is string script)
		{
			return new StiStateSaveEvent(script);
		}
		return base.ConvertFrom(context, culture, value);
	}
}
