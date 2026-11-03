using System;
using System.ComponentModel;
using System.Globalization;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Dictionary.Design;

public class StiDataColumnsCollectionConverter : TypeConverter
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
			if (!(value is StiDataColumnsCollection { Count: not 0 }))
			{
				return string.Format("[{0}]", Loc.GetMain("NoElements"));
			}
			return string.Format("({0})", Loc.Get("Report", "Collection"));
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
