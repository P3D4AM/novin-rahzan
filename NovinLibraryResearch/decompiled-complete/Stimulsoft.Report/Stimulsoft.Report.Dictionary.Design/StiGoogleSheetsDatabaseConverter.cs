using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using Stimulsoft.Report.Dictionary.Databases.Google;

namespace Stimulsoft.Report.Dictionary.Design;

public class StiGoogleSheetsDatabaseConverter : TypeConverter
{
	public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
	{
		return TypeDescriptor.GetProperties(value, attributes);
	}

	public override bool GetPropertiesSupported(ITypeDescriptorContext context)
	{
		return true;
	}

	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == typeof(InstanceDescriptor) && value != null)
		{
			StiGoogleSheetsDatabase stiGoogleSheetsDatabase = (StiGoogleSheetsDatabase)value;
			Type[] types = new Type[7]
			{
				typeof(string),
				typeof(string),
				typeof(string),
				typeof(string),
				typeof(string),
				typeof(string),
				typeof(bool)
			};
			ConstructorInfo constructor = typeof(StiGoogleSheetsDatabase).GetConstructor(types);
			if (constructor != null)
			{
				object[] objs = new object[7] { stiGoogleSheetsDatabase.Name, stiGoogleSheetsDatabase.Alias, stiGoogleSheetsDatabase.Key, stiGoogleSheetsDatabase.ClientId, stiGoogleSheetsDatabase.ClientSecret, stiGoogleSheetsDatabase.SpreadsheetId, stiGoogleSheetsDatabase.FirstRowIsHeader };
				return CreateNewInstanceDescriptor(constructor, objs);
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}

	public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
	{
		if (destinationType == typeof(InstanceDescriptor))
		{
			return true;
		}
		return base.CanConvertTo(context, destinationType);
	}

	protected object CreateNewInstanceDescriptor(ConstructorInfo info, object[] objs)
	{
		return new InstanceDescriptor(info, objs);
	}
}
