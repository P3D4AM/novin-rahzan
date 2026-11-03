using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using Stimulsoft.Report.Dictionary.DataSources.Google;

namespace Stimulsoft.Report.Dictionary.Design;

public class StiBigQuerySourceConverter : StiSqlSourceConverter
{
	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == typeof(InstanceDescriptor) && value != null)
		{
			StiBigQuerySource stiBigQuerySource = (StiBigQuerySource)value;
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
			ConstructorInfo constructor = typeof(StiBigQuerySource).GetConstructor(types);
			if (constructor != null)
			{
				object[] objs = new object[8]
				{
					stiBigQuerySource.NameInSource,
					stiBigQuerySource.Name,
					stiBigQuerySource.Alias,
					string.Empty,
					stiBigQuerySource.ConnectOnStart,
					stiBigQuerySource.ReconnectOnEachRow,
					stiBigQuerySource.CommandTimeout,
					stiBigQuerySource.Key
				};
				return CreateNewInstanceDescriptor(constructor, objs);
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
