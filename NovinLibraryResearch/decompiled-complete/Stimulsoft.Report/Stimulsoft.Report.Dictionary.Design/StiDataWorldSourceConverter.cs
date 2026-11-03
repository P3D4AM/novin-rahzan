using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace Stimulsoft.Report.Dictionary.Design;

public class StiDataWorldSourceConverter : StiSqlSourceConverter
{
	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == typeof(InstanceDescriptor) && value != null)
		{
			StiDataWorldSource stiDataWorldSource = (StiDataWorldSource)value;
			Type[] types = new Type[8]
			{
				typeof(string),
				typeof(string),
				typeof(string),
				typeof(string),
				typeof(bool),
				typeof(bool),
				typeof(int),
				typeof(string)
			};
			ConstructorInfo constructor = typeof(StiDataWorldSource).GetConstructor(types);
			if (constructor != null)
			{
				object[] objs = new object[8]
				{
					stiDataWorldSource.NameInSource,
					stiDataWorldSource.Name,
					stiDataWorldSource.Alias,
					string.Empty,
					stiDataWorldSource.ConnectOnStart,
					stiDataWorldSource.ReconnectOnEachRow,
					stiDataWorldSource.CommandTimeout,
					stiDataWorldSource.Key
				};
				return CreateNewInstanceDescriptor(constructor, objs);
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
