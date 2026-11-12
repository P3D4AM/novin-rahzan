using System;
using System.ComponentModel;
using System.Globalization;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Dashboard.Components.Table.Design;

public class StiTableConditionConverter : TypeConverter
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
		if (destinationType == typeof(string))
		{
			if (((context?.Instance is StiTableElement stiTableElement) ? stiTableElement.TableConditions : null) == null)
			{
				return "(" + Loc.Get("FormStyleDesigner", "NotSpecified") + ")";
			}
			return "(" + Loc.GetMain("Conditions") + ")";
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
