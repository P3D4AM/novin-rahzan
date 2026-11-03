using System;
using System.ComponentModel;
using System.Globalization;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.BarCodes;

namespace Stimulsoft.Report.Components.Design;

public class StiSimpeImageConverter : TypeConverter
{
	public override bool GetPropertiesSupported(ITypeDescriptorContext context)
	{
		return false;
	}

	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == typeof(string))
		{
			string text = ConvertForImage(context);
			if (text != null)
			{
				return text;
			}
			text = ConvertForReport(context);
			if (text != null)
			{
				return text;
			}
			text = ConvertToBarCode(context);
			if (text != null)
			{
				return text;
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}

	private static string ConvertForImage(ITypeDescriptorContext context)
	{
		if (!(context.Instance is StiImage stiImage))
		{
			return null;
		}
		if (stiImage.ExistImage() || !string.IsNullOrEmpty(stiImage.DataColumn) || !string.IsNullOrEmpty(stiImage.ImageData.Value) || !string.IsNullOrEmpty(stiImage.ImageURL.Value) || !string.IsNullOrEmpty(stiImage.File))
		{
			return "(" + Loc.Get("PropertyMain", "Image") + ")";
		}
		return "[" + Loc.Get("Report", "NotAssigned") + "]";
	}

	private static string ConvertForReport(ITypeDescriptorContext context)
	{
		if (!(context.Instance is StiReport stiReport))
		{
			return null;
		}
		if (stiReport.ReportImage == null)
		{
			return "[" + Loc.Get("Report", "NotAssigned") + "]";
		}
		return "(" + Loc.Get("PropertyMain", "Image") + ")";
	}

	private static string ConvertToBarCode(ITypeDescriptorContext context)
	{
		if (!(context.Instance is StiQRCodeBarCodeType stiQRCodeBarCodeType))
		{
			return null;
		}
		if (stiQRCodeBarCodeType.Image == null)
		{
			return "[" + Loc.Get("Report", "NotAssigned") + "]";
		}
		return "(" + Loc.Get("PropertyMain", "Image") + ")";
	}
}
