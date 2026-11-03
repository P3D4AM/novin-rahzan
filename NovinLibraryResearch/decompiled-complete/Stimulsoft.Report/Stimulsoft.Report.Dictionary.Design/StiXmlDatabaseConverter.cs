using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace Stimulsoft.Report.Dictionary.Design;

public class StiXmlDatabaseConverter : TypeConverter
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
			StiXmlDatabase stiXmlDatabase = (StiXmlDatabase)value;
			Type[] types = new Type[5]
			{
				typeof(string),
				typeof(string),
				typeof(string),
				typeof(string),
				typeof(StiXmlType)
			};
			ConstructorInfo constructor = typeof(StiXmlDatabase).GetConstructor(types);
			if (constructor != null)
			{
				object[] objs = new object[5] { stiXmlDatabase.Name, stiXmlDatabase.PathSchema, stiXmlDatabase.PathData, stiXmlDatabase.Key, stiXmlDatabase.XmlType };
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
