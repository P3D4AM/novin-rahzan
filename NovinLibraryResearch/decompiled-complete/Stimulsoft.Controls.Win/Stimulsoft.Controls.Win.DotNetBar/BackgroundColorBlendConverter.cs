using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Globalization;
using System.Reflection;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class BackgroundColorBlendConverter : TypeConverter
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
		if (destinationType == null)
		{
			throw new ArgumentNullException("destinationType");
		}
		if (destinationType == typeof(InstanceDescriptor) && value is BackgroundColorBlend)
		{
			BackgroundColorBlend backgroundColorBlend = (BackgroundColorBlend)value;
			Type[] array = null;
			MemberInfo memberInfo = null;
			object[] array2 = null;
			array = new Type[2]
			{
				typeof(Color),
				typeof(float)
			};
			memberInfo = typeof(BackgroundColorBlend).GetConstructor(array);
			array2 = new object[2] { backgroundColorBlend.Color, backgroundColorBlend.Position };
			if (memberInfo != null)
			{
				return new InstanceDescriptor(memberInfo, array2);
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
