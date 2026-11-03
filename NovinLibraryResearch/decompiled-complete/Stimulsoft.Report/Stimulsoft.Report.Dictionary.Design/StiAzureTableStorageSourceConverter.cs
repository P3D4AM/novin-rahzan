using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace Stimulsoft.Report.Dictionary.Design;

public class StiAzureTableStorageSourceConverter : StiSqlSourceConverter
{
	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == typeof(InstanceDescriptor) && value != null)
		{
			StiAzureTableStorageSource stiAzureTableStorageSource = (StiAzureTableStorageSource)value;
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
			ConstructorInfo constructor = typeof(StiAzureTableStorageSource).GetConstructor(types);
			if (constructor != null)
			{
				object[] objs = new object[8]
				{
					stiAzureTableStorageSource.NameInSource,
					stiAzureTableStorageSource.Name,
					stiAzureTableStorageSource.Alias,
					string.Empty,
					stiAzureTableStorageSource.ConnectOnStart,
					stiAzureTableStorageSource.ReconnectOnEachRow,
					stiAzureTableStorageSource.CommandTimeout,
					stiAzureTableStorageSource.Key
				};
				return CreateNewInstanceDescriptor(constructor, objs);
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
