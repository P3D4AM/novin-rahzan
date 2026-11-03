using System;
using System.ComponentModel;
using System.Globalization;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Design;

public class StiGlobalizationContainerConverter : TypeConverter
{
	public override bool GetPropertiesSupported(ITypeDescriptorContext context)
	{
		return false;
	}

	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == typeof(string))
		{
			return "(" + StiLocalization.Get("PropertyMain", "GlobalizationStrings") + ")";
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
