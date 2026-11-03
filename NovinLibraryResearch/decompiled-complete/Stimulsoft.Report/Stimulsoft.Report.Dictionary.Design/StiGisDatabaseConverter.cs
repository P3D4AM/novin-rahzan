using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Dictionary.Design;

public class StiGisDatabaseConverter : TypeConverter
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
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		if (destinationType == typeof(InstanceDescriptor) && value != null)
		{
			StiGisDatabase stiGisDatabase = (StiGisDatabase)value;
			Type[] types = new Type[4]
			{
				typeof(string),
				typeof(string),
				typeof(string),
				typeof(StiGisDataType)
			};
			ConstructorInfo constructor = typeof(StiGisDatabase).GetConstructor(types);
			if (constructor != null)
			{
				object[] objs = new object[4] { stiGisDatabase.Name, stiGisDatabase.PathData, stiGisDatabase.Key, stiGisDatabase.DataType };
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

	private object CreateNewInstanceDescriptor(ConstructorInfo info, object[] objs)
	{
		return new InstanceDescriptor(info, objs);
	}
}
