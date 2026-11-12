using System;
using System.ComponentModel;
using System.Globalization;
using Stimulsoft.Base.Localization;
using Stimulsoft.Data.Engine;

namespace Stimulsoft.Dashboard.Components.Design;

public class StiDataTransformationConverter : TypeConverter
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
			if (context == null)
			{
				return "(" + Loc.Get("Report", "NotAssigned") + ")";
			}
			if (!(context.Instance is IStiDataTransformationElement stiDataTransformationElement) || stiDataTransformationElement.IsDefaultDataTransformation())
			{
				return "(" + Loc.Get("Report", "NotAssigned") + ")";
			}
			return "(" + Loc.Get("PropertyMain", "DataTransformation") + ")";
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
