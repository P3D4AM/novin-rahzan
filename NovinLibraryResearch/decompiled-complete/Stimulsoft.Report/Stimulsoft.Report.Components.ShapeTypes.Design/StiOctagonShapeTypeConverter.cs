using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace Stimulsoft.Report.Components.ShapeTypes.Design;

public class StiOctagonShapeTypeConverter : StiShapeTypeServiceConverter
{
	public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
	{
		if (destinationType == typeof(InstanceDescriptor))
		{
			return true;
		}
		return base.CanConvertTo(context, destinationType);
	}

	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == typeof(InstanceDescriptor) && value != null)
		{
			StiOctagonShapeType stiOctagonShapeType = (StiOctagonShapeType)value;
			Type[] types = new Type[2]
			{
				typeof(bool),
				typeof(float)
			};
			ConstructorInfo constructor = typeof(StiOctagonShapeType).GetConstructor(types);
			if (constructor != null)
			{
				object[] objs = new object[2] { stiOctagonShapeType.AutoSize, stiOctagonShapeType.Bevel };
				return CreateNewInstanceDescriptor(constructor, objs);
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
