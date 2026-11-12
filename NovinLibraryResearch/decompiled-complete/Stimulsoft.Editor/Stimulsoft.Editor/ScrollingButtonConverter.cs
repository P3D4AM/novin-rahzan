using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace Stimulsoft.Editor;

public class ScrollingButtonConverter : TypeConverter
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
		if (destinationType == typeof(InstanceDescriptor) && typeof(ScrollingButton).IsInstanceOfType(value))
		{
			ConstructorInfo constructor = typeof(ScrollingButton).GetConstructor(new Type[0]);
			if (constructor != null)
			{
				return new InstanceDescriptor(constructor, null, isComplete: false);
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
