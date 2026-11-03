using System;
using System.ComponentModel;
using System.Globalization;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Components.Design;

public class StiConditionsCollectionConverter : TypeConverter
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
		if (destinationType == typeof(string) && value is StiConditionsCollection stiConditionsCollection)
		{
			if (stiConditionsCollection.Count == 0)
			{
				return "[" + StiLocalization.Get("FormConditions", "NoConditions") + "]";
			}
			return "(" + StiLocalization.Get("PropertyMain", "Conditions") + ")";
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
