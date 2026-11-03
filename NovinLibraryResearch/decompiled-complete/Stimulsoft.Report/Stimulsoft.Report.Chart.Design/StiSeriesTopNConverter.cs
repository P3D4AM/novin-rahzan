using System;
using System.ComponentModel;
using System.Globalization;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart.Design;

public class StiSeriesTopNConverter : TypeConverter
{
	public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
	{
		return TypeDescriptor.GetProperties(value, attributes);
	}

	public override bool GetPropertiesSupported(ITypeDescriptorContext context)
	{
		return true;
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
			if (!(value is StiSeriesTopN stiSeriesTopN))
			{
				return "(" + StiLocalization.Get("PropertyMain", "TopN") + ")";
			}
			switch (stiSeriesTopN.Mode)
			{
			case StiTopNMode.Top:
				return Loc.GetEnum("StiAnchorModeTop");
			case StiTopNMode.Bottom:
				return Loc.GetEnum("StiAnchorModeBottom");
			case StiTopNMode.None:
				return Loc.GetEnum("DialogResultNone");
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
