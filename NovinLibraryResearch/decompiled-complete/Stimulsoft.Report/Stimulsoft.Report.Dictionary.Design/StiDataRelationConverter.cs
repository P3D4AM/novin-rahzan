using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace Stimulsoft.Report.Dictionary.Design;

public class StiDataRelationConverter : TypeConverter
{
	public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
	{
		return TypeDescriptor.GetProperties(value, attributes);
	}

	public override bool GetPropertiesSupported(ITypeDescriptorContext context)
	{
		return true;
	}

	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == typeof(InstanceDescriptor) && value != null)
		{
			StiDataRelation stiDataRelation = (StiDataRelation)value;
			if (stiDataRelation.NameInSource == stiDataRelation.Alias && string.IsNullOrEmpty(stiDataRelation.Key))
			{
				Type[] types = new Type[5]
				{
					typeof(string),
					typeof(StiDataSource),
					typeof(StiDataSource),
					typeof(string[]),
					typeof(string[])
				};
				ConstructorInfo constructor = typeof(StiDataRelation).GetConstructor(types);
				if (constructor != null)
				{
					object[] objs = new object[5] { stiDataRelation.NameInSource, stiDataRelation.ParentSource, stiDataRelation.ChildSource, stiDataRelation.ParentColumns, stiDataRelation.ChildColumns };
					return CreateNewInstanceDescriptor(constructor, objs);
				}
			}
			else if (!stiDataRelation.Active)
			{
				Type[] types2 = new Type[8]
				{
					typeof(string),
					typeof(string),
					typeof(string),
					typeof(StiDataSource),
					typeof(StiDataSource),
					typeof(string[]),
					typeof(string[]),
					typeof(string)
				};
				ConstructorInfo constructor2 = typeof(StiDataRelation).GetConstructor(types2);
				if (constructor2 != null)
				{
					object[] objs2 = new object[8] { stiDataRelation.NameInSource, stiDataRelation.Name, stiDataRelation.Alias, stiDataRelation.ParentSource, stiDataRelation.ChildSource, stiDataRelation.ParentColumns, stiDataRelation.ChildColumns, stiDataRelation.Key };
					return CreateNewInstanceDescriptor(constructor2, objs2);
				}
			}
			else if (stiDataRelation.Active)
			{
				Type[] types3 = new Type[9]
				{
					typeof(string),
					typeof(string),
					typeof(string),
					typeof(StiDataSource),
					typeof(StiDataSource),
					typeof(string[]),
					typeof(string[]),
					typeof(string),
					typeof(bool)
				};
				ConstructorInfo constructor3 = typeof(StiDataRelation).GetConstructor(types3);
				if (constructor3 != null)
				{
					object[] objs3 = new object[9] { stiDataRelation.NameInSource, stiDataRelation.Name, stiDataRelation.Alias, stiDataRelation.ParentSource, stiDataRelation.ChildSource, stiDataRelation.ParentColumns, stiDataRelation.ChildColumns, stiDataRelation.Key, stiDataRelation.Active };
					return CreateNewInstanceDescriptor(constructor3, objs3);
				}
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
