using System;
using System.ComponentModel;
using System.Globalization;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Design;

public class StiStyleConditionsCollectionConverter : TypeConverter
{
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
		if (destinationType == typeof(string) && value is StiStyleConditionsCollection stiStyleConditionsCollection)
		{
			if (stiStyleConditionsCollection.Count != 0)
			{
				return "(" + Loc.Get("PropertyMain", "Conditions") + ")";
			}
			return "[" + Loc.Get("FormConditions", "NoConditions") + "]";
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
