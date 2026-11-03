using System;
using System.ComponentModel;
using System.Globalization;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Report.Components.Design;

public class StiSubReportConverter : TypeConverter
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
		if (!(context.Instance is StiSubReport stiSubReport))
		{
			return Loc.Get("FormStyleDesigner", "NotSpecified");
		}
		if (stiSubReport.SubReportPage != null)
		{
			return Loc.Get("FormViewer", "LabelPageN") + " " + stiSubReport.SubReportPage.Name;
		}
		if (!string.IsNullOrWhiteSpace(stiSubReport.SubReportUrl))
		{
			string fileNameFromHyperlink = StiHyperlinkProcessor.GetFileNameFromHyperlink(stiSubReport.SubReportUrl);
			if (fileNameFromHyperlink != null)
			{
				return Loc.GetMain("File") + ": " + fileNameFromHyperlink;
			}
			return Loc.GetMain("Hyperlink") + ": " + stiSubReport.SubReportUrl;
		}
		return Loc.Get("FormStyleDesigner", "NotSpecified");
	}
}
