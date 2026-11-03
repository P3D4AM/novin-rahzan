using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace Stimulsoft.Report.Components.ShapeTypes.Design;

public class StiArrowShapeTypeConverter : StiShapeTypeServiceConverter
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
			StiArrowShapeType stiArrowShapeType = (StiArrowShapeType)value;
			Type[] types = new Type[3]
			{
				typeof(StiShapeDirection),
				typeof(float),
				typeof(float)
			};
			ConstructorInfo constructor = typeof(StiArrowShapeType).GetConstructor(types);
			if (constructor != null)
			{
				object[] objs = new object[3] { stiArrowShapeType.Direction, stiArrowShapeType.ArrowWidth, stiArrowShapeType.ArrowHeight };
				return CreateNewInstanceDescriptor(constructor, objs);
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
