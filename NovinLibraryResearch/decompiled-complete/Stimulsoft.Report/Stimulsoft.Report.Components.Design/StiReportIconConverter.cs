using System;
using System.ComponentModel;
using System.Globalization;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Components.Design;

public class StiReportIconConverter : TypeConverter
{
	public override bool GetPropertiesSupported(ITypeDescriptorContext context)
	{
		return false;
	}

	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == typeof(string))
		{
			if (context.Instance is StiReport stiReport)
			{
				if (stiReport.ReportIcon == null)
				{
					return "[" + Loc.Get("Report", "NotAssigned") + "]";
				}
				return "(" + Loc.Get("PropertyMain", "Icon") + ")";
			}
			if (context.Instance is StiPage stiPage)
			{
				if (stiPage.Icon == null)
				{
					return "[" + Loc.Get("Report", "NotAssigned") + "]";
				}
				return "(" + Loc.Get("PropertyMain", "Icon") + ")";
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
