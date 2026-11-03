using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace Stimulsoft.Report.Components.ShapeTypes.Design;

public class StiFlowchartCollateShapeTypeConverter : StiShapeTypeServiceConverter
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
			StiFlowchartCollateShapeType stiFlowchartCollateShapeType = (StiFlowchartCollateShapeType)value;
			Type[] types = new Type[1] { typeof(StiShapeDirection) };
			ConstructorInfo constructor = typeof(StiFlowchartCollateShapeType).GetConstructor(types);
			if (constructor != null)
			{
				object[] objs = new object[1] { stiFlowchartCollateShapeType.Direction };
				return CreateNewInstanceDescriptor(constructor, objs);
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
