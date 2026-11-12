using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using Stimulsoft.Data.Engine;

namespace Stimulsoft.Data.Design;

public class StiDataActionRuleConverter : TypeConverter
{
	public override bool GetPropertiesSupported(ITypeDescriptorContext context)
	{
		return false;
	}

	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == typeof(InstanceDescriptor) && value != null && value is StiDataActionRule stiDataActionRule)
		{
			Type[] types = new Type[11]
			{
				typeof(string),
				typeof(string),
				typeof(StiDataActionType),
				typeof(int),
				typeof(int),
				typeof(StiDataActionPriority),
				typeof(string),
				typeof(string),
				typeof(bool),
				typeof(bool),
				typeof(string)
			};
			ConstructorInfo constructor = typeof(StiDataActionRule).GetConstructor(types);
			if (constructor != null)
			{
				object[] objs = new object[11]
				{
					stiDataActionRule.Key, stiDataActionRule.Path, stiDataActionRule.Type, stiDataActionRule.StartIndex, stiDataActionRule.RowsCount, stiDataActionRule.Priority, stiDataActionRule.ValueFrom, stiDataActionRule.ValueTo, stiDataActionRule.MatchCase, stiDataActionRule.MatchWholeWord,
					stiDataActionRule.InitialValue
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

	private object CreateNewInstanceDescriptor(ConstructorInfo info, object[] objs)
	{
		return new InstanceDescriptor(info, objs);
	}
}
