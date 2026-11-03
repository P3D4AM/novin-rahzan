using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using System.Xml;
using Stimulsoft.Report.Controls;

namespace Stimulsoft.Report.Components.Design;

public class StiRichTextExpressionConverter : TypeConverter
{
	public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
	{
		if (sourceType == typeof(string))
		{
			return false;
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

	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == null)
		{
			throw new ArgumentNullException("destinationType");
		}
		if (destinationType == typeof(string))
		{
			try
			{
				string text = null;
				text = ((!(value is StiExpression)) ? (value as string) : ((StiExpression)value).Value);
				StiRichText stiRichText = context.Instance as StiRichText;
				StiRichTextBox stiRichTextBox = new StiRichTextBox(drawTransparent: false);
				try
				{
					string text2 = StiRichText.UnpackRtf(XmlConvert.DecodeName(text));
					if (text2.TrimStart(new char[0]).ToLowerInvariant().IndexOf("rtf") == -1)
					{
						return text2;
					}
					((RichTextBox)stiRichTextBox).Rtf = text2;
					return ((Control)stiRichTextBox).Text;
				}
				finally
				{
					((IDisposable)stiRichTextBox)?.Dispose();
				}
			}
			catch
			{
			}
			return string.Empty;
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
