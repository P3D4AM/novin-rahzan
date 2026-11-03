using System;
using System.ComponentModel;
using System.Globalization;
using System.Text.RegularExpressions;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Report.Design;

public class StiFontIconConverter : TypeConverter
{
	public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
	{
		if (destinationType == typeof(StiFontIcons))
		{
			return true;
		}
		return base.CanConvertTo(context, destinationType);
	}

	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (value == null)
		{
			if (!StiVisualStatePermissionAttribute.IsFromDefaultStateAllowed(context))
			{
				return StiLocalization.Get("ChartRibbon", "AxesTicksNone");
			}
			return Loc.GetReport("FromDefaultState");
		}
		string input = value.ToString();
		return Regex.Replace(input, "(\\B[A-Z])", "-$1");
	}
}
