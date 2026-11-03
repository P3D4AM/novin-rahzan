using System;
using System.ComponentModel;
using System.Globalization;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Viewer.Design;

public class StiPreviewSettingsConverter : TypeConverter
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
			return "(" + Loc.Get("PropertyMain", "PreviewSettings") + ")";
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
