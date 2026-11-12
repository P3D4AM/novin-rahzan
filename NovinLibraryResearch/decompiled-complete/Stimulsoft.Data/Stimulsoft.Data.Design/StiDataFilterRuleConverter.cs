using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using Stimulsoft.Data.Engine;

namespace Stimulsoft.Data.Design;

public class StiDataFilterRuleConverter : TypeConverter
{
	public override bool GetPropertiesSupported(ITypeDescriptorContext context)
	{
		return false;
	}

	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == typeof(InstanceDescriptor) && value != null && value is StiDataFilterRule stiDataFilterRule)
		{
			Type[] types = new Type[9]
			{
				typeof(string),
				typeof(string),
				typeof(string),
				typeof(StiDataFilterCondition),
				typeof(StiDataFilterOperation),
				typeof(string),
				typeof(string),
				typeof(bool),
				typeof(bool)
			};
			ConstructorInfo constructor = typeof(StiDataFilterRule).GetConstructor(types);
			if (constructor != null)
			{
				object[] objs = new object[9] { stiDataFilterRule.Key, stiDataFilterRule.Path, stiDataFilterRule.Path2, stiDataFilterRule.Condition, stiDataFilterRule.Operation, stiDataFilterRule.Value, stiDataFilterRule.Value2, stiDataFilterRule.IsEnabled, stiDataFilterRule.IsExpression };
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
