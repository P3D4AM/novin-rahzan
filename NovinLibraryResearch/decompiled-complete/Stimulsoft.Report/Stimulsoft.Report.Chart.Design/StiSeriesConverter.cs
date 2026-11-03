using System;
using System.ComponentModel;
using System.Globalization;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart.Design;

public class StiSeriesConverter : StiUniversalConverter
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
		return ((StiUniversalConverter)this).CanConvertTo(context, destinationType);
	}

	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == typeof(string))
		{
			if (!(value is StiSeriesCollection stiSeriesCollection))
			{
				return Loc.Get("FormStyleDesigner", "NotSpecified");
			}
			if (stiSeriesCollection.Count == 0)
			{
				return "[" + Loc.GetMain("NoElements") + "]";
			}
			return "(" + Loc.GetMain("Series") + ")";
		}
		return ((StiUniversalConverter)this).ConvertTo(context, culture, value, destinationType);
	}
}
