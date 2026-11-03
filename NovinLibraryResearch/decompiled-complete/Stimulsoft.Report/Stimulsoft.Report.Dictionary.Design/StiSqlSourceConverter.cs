using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace Stimulsoft.Report.Dictionary.Design;

public class StiSqlSourceConverter : TypeConverter
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
			StiSqlSource stiSqlSource = (StiSqlSource)value;
			Type[] types = new Type[8]
			{
				typeof(string),
				typeof(string),
				typeof(string),
				typeof(string),
				typeof(bool),
				typeof(bool),
				typeof(int),
				typeof(string)
			};
			ConstructorInfo constructor = typeof(StiSqlSource).GetConstructor(types);
			if (constructor != null)
			{
				object[] objs = new object[8]
				{
					stiSqlSource.NameInSource,
					stiSqlSource.Name,
					stiSqlSource.Alias,
					string.Empty,
					stiSqlSource.ConnectOnStart,
					stiSqlSource.ReconnectOnEachRow,
					stiSqlSource.CommandTimeout,
					stiSqlSource.Key
				};
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
