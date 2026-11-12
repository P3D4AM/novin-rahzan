using System;
using System.ComponentModel;
using System.Globalization;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Dashboard.Components.Text.Design;

public class StiTextConverter : TypeConverter
{
	public override bool GetPropertiesSupported(ITypeDescriptorContext context)
	{
		return false;
	}

	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == typeof(string))
		{
			if (value != null)
			{
				return "(" + Loc.Get("PropertyMain", "Text") + ")";
			}
			return "[" + Loc.Get("Report", "NotAssigned") + "]";
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
