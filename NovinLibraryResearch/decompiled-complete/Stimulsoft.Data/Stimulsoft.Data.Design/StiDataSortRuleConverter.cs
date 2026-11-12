using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using Stimulsoft.Data.Engine;

namespace Stimulsoft.Data.Design;

public class StiDataSortRuleConverter : TypeConverter
{
	public override bool GetPropertiesSupported(ITypeDescriptorContext context)
	{
		return false;
	}

	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == typeof(InstanceDescriptor) && value != null && value is StiDataSortRule stiDataSortRule)
		{
			Type[] types = new Type[2]
			{
				typeof(string),
				typeof(StiDataSortDirection)
			};
			ConstructorInfo constructor = typeof(StiDataSortRule).GetConstructor(types);
			if (constructor != null)
			{
				object[] objs = new object[2] { stiDataSortRule.Key, stiDataSortRule.Direction };
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
