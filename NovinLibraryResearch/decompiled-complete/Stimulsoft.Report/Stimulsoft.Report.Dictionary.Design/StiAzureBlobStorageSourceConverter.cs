using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace Stimulsoft.Report.Dictionary.Design;

public class StiAzureBlobStorageSourceConverter : StiSqlSourceConverter
{
	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == typeof(InstanceDescriptor) && value != null)
		{
			StiAzureBlobStorageSource stiAzureBlobStorageSource = (StiAzureBlobStorageSource)value;
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
			ConstructorInfo constructor = typeof(StiAzureBlobStorageSource).GetConstructor(types);
			if (constructor != null)
			{
				object[] objs = new object[8] { stiAzureBlobStorageSource.NameInSource, stiAzureBlobStorageSource.Name, stiAzureBlobStorageSource.Alias, stiAzureBlobStorageSource.SqlCommand, stiAzureBlobStorageSource.ConnectOnStart, stiAzureBlobStorageSource.ReconnectOnEachRow, stiAzureBlobStorageSource.CommandTimeout, stiAzureBlobStorageSource.Key };
				return CreateNewInstanceDescriptor(constructor, objs);
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
