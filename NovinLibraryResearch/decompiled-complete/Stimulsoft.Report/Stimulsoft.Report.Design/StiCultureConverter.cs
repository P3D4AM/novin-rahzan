using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;

namespace Stimulsoft.Report.Design;

public class StiCultureConverter : TypeConverter
{
	private static Hashtable hashtable;

	public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
	{
		return TypeDescriptor.GetProperties(value, attributes);
	}

	public override bool GetPropertiesSupported(ITypeDescriptorContext context)
	{
		return false;
	}

	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (hashtable == null)
		{
			hashtable = new Hashtable();
			CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
			foreach (CultureInfo cultureInfo in cultures)
			{
				string key = cultureInfo.DisplayName + "  (" + cultureInfo.Name + ")";
				hashtable.Add(key, cultureInfo.Name);
			}
		}
		if (destinationType == typeof(string) && value != null && hashtable.ContainsKey(value))
		{
			return hashtable[value];
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}

	public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
	{
		if (destinationType == typeof(string))
		{
			return true;
		}
		return base.CanConvertTo(context, destinationType);
	}
}
