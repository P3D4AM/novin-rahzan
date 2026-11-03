using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace Stimulsoft.Report.Components.ShapeTypes.Design;

public class StiChevronShapeTypeConverter : StiShapeTypeServiceConverter
{
	public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
	{
		if (!(destinationType == typeof(InstanceDescriptor)))
		{
			return base.CanConvertTo(context, destinationType);
		}
		return true;
	}

	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == typeof(InstanceDescriptor) && value != null)
		{
			StiChevronShapeType stiChevronShapeType = (StiChevronShapeType)value;
			Type[] types = new Type[1] { typeof(StiShapeDirection) };
			ConstructorInfo constructor = typeof(StiChevronShapeType).GetConstructor(types);
			if (constructor != null)
			{
				object[] objs = new object[1] { stiChevronShapeType.Direction };
				return CreateNewInstanceDescriptor(constructor, objs);
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
