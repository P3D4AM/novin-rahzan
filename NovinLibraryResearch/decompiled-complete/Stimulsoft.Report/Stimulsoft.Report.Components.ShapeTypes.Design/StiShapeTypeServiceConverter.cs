using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace Stimulsoft.Report.Components.ShapeTypes.Design;

public class StiShapeTypeServiceConverter : TypeConverter
{
	public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
	{
		return TypeDescriptor.GetProperties(value, attributes);
	}

	public override bool GetPropertiesSupported(ITypeDescriptorContext context)
	{
		return true;
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
		if (destinationType == typeof(string))
		{
			if (value != null)
			{
				return value.ToString();
			}
			return "null";
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}

	protected object CreateNewInstanceDescriptor(ConstructorInfo info, object[] objs)
	{
		return new InstanceDescriptor(info, objs);
	}
}
