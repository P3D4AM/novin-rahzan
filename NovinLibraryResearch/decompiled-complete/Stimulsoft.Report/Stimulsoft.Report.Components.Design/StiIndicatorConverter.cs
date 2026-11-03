using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Xml;

namespace Stimulsoft.Report.Components.Design;

public class StiIndicatorConverter : TypeConverter
{
	public override bool GetPropertiesSupported(ITypeDescriptorContext context)
	{
		return false;
	}

	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		if (destinationType == typeof(string))
		{
			if (value is StiDataBarIndicator stiDataBarIndicator)
			{
				return string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}", "DataBar", XmlConvert.EncodeName(stiDataBarIndicator.BrushType.ToString()), StiReportObjectStringConverter.ConvertColorToString(stiDataBarIndicator.PositiveColor), StiReportObjectStringConverter.ConvertColorToString(stiDataBarIndicator.NegativeColor), stiDataBarIndicator.ShowBorder, StiReportObjectStringConverter.ConvertColorToString(stiDataBarIndicator.PositiveBorderColor), StiReportObjectStringConverter.ConvertColorToString(stiDataBarIndicator.NegativeBorderColor), XmlConvert.EncodeName(stiDataBarIndicator.Direction.ToString()), stiDataBarIndicator.Value, stiDataBarIndicator.Minimum, stiDataBarIndicator.Maximum);
			}
			if (value is StiIconSetIndicator stiIconSetIndicator)
			{
				string? arg = XmlConvert.EncodeName(stiIconSetIndicator.Icon.ToString());
				ContentAlignment alignment = stiIconSetIndicator.Alignment;
				return string.Format("{0},{1},{2}", "IconSet", arg, XmlConvert.EncodeName(((object)(ContentAlignment)(ref alignment)).ToString()));
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}

	public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
	{
		if (sourceType == typeof(string))
		{
			return true;
		}
		return base.CanConvertFrom(context, sourceType);
	}

	public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
	{
		if (destinationType == typeof(string))
		{
			return true;
		}
		return base.CanConvertTo(context, destinationType);
	}

	public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
	{
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		if (value is string text)
		{
			char[] separator = new char[1] { ',' };
			string[] array = text.Split(separator);
			if (array[0] == "DataBar")
			{
				return new StiDataBarIndicator((StiBrushType)Enum.Parse(typeof(StiBrushType), XmlConvert.DecodeName(array[1])), StiReportObjectStringConverter.ConvertStringToColor(array[2]), StiReportObjectStringConverter.ConvertStringToColor(array[3]), array[4].ToLower(CultureInfo.InvariantCulture) == "true", StiReportObjectStringConverter.ConvertStringToColor(array[5]), StiReportObjectStringConverter.ConvertStringToColor(array[6]), (StiDataBarDirection)Enum.Parse(typeof(StiDataBarDirection), XmlConvert.DecodeName(array[7])), float.Parse(array[8]), float.Parse(array[9]), float.Parse(array[10]));
			}
			if (array[0] == "IconSet")
			{
				return new StiIconSetIndicator((StiIcon)Enum.Parse(typeof(StiIcon), XmlConvert.DecodeName(array[1])), (ContentAlignment)Enum.Parse(typeof(ContentAlignment), XmlConvert.DecodeName(array[2])));
			}
		}
		return base.ConvertFrom(context, culture, value);
	}
}
