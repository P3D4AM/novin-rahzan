using System;
using System.ComponentModel;
using System.Globalization;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Dashboard.Components.Chart.Design;

public class StiColorsConverter : StiUniversalConverter
{
	public override bool GetPropertiesSupported(ITypeDescriptorContext context)
	{
		return false;
	}

	public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
	{
		if (destinationType == typeof(string))
		{
			return true;
		}
		return base.CanConvertTo(context, destinationType);
	}

	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == typeof(string))
		{
			return "(" + Loc.Get("PropertyCategory", "ColorsCategory") + ")";
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
