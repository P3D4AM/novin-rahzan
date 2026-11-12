using System;
using System.ComponentModel;
using System.Globalization;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Components.Design;

public class StiElementStyleConverter : TypeConverter
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
			IStiDashboardElementStyle stiDashboardElementStyle = context.Instance as IStiDashboardElementStyle;
			if (value is StiElementStyleIdent && stiDashboardElementStyle != null)
			{
				StiElementStyleIdent stiElementStyleIdent = (StiElementStyleIdent)value;
				if (stiElementStyleIdent == StiElementStyleIdent.Custom && !string.IsNullOrWhiteSpace(stiDashboardElementStyle.CustomStyleName))
				{
					return stiDashboardElementStyle.CustomStyleName;
				}
				return GetItemText(stiElementStyleIdent);
			}
			return "(" + Loc.Get("Report", "NotAssigned") + ")";
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}

	private static string GetItemText(object item)
	{
		if (item is StiElementStyleIdent)
		{
			if ((StiElementStyleIdent)item == StiElementStyleIdent.Auto)
			{
				return Loc.GetMain("Auto");
			}
			string text = item.ToString();
			string text2 = StiLocalization.Get("PropertyColor", text, throwError: false);
			return text2 ?? text;
		}
		if (item is StiBaseStyle)
		{
			return (item as StiBaseStyle).Name;
		}
		if (item is string)
		{
			return item as string;
		}
		return "";
	}
}
