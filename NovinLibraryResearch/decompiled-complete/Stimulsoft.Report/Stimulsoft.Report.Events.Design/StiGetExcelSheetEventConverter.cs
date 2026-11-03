using System.ComponentModel;
using System.Globalization;

namespace Stimulsoft.Report.Events.Design;

public class StiGetExcelSheetEventConverter : StiEventConverter
{
	public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
	{
		if (value is string script)
		{
			return new StiGetExcelSheetEvent(script);
		}
		return base.ConvertFrom(context, culture, value);
	}
}
