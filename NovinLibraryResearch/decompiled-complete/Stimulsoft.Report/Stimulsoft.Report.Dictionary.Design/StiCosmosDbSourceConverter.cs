using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using Stimulsoft.Report.Dictionary.DataSources.Azure;

namespace Stimulsoft.Report.Dictionary.Design;

public class StiCosmosDbSourceConverter : StiSqlSourceConverter
{
	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == typeof(InstanceDescriptor) && value != null)
		{
			StiCosmosDbSource stiCosmosDbSource = (StiCosmosDbSource)value;
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
			ConstructorInfo constructor = typeof(StiCosmosDbSource).GetConstructor(types);
			if (constructor != null)
			{
				object[] objs = new object[8]
				{
					stiCosmosDbSource.NameInSource,
					stiCosmosDbSource.Name,
					stiCosmosDbSource.Alias,
					string.Empty,
					stiCosmosDbSource.ConnectOnStart,
					stiCosmosDbSource.ReconnectOnEachRow,
					stiCosmosDbSource.CommandTimeout,
					stiCosmosDbSource.Key
				};
				return CreateNewInstanceDescriptor(constructor, objs);
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
