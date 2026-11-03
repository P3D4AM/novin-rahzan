using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace Stimulsoft.Report.Dictionary.Design;

public class StiAzureTableStorageDatabaseConverter : StiSqlDatabaseConverter
{
	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == typeof(InstanceDescriptor) && value != null)
		{
			StiAzureTableStorageDatabase stiAzureTableStorageDatabase = (StiAzureTableStorageDatabase)value;
			Type[] types = new Type[5]
			{
				typeof(string),
				typeof(string),
				typeof(string),
				typeof(bool),
				typeof(string)
			};
			ConstructorInfo constructor = typeof(StiAzureTableStorageDatabase).GetConstructor(types);
			if (constructor != null)
			{
				object[] objs = new object[5] { stiAzureTableStorageDatabase.Name, stiAzureTableStorageDatabase.Alias, stiAzureTableStorageDatabase.ConnectionString, stiAzureTableStorageDatabase.PromptUserNameAndPassword, stiAzureTableStorageDatabase.Key };
				return CreateNewInstanceDescriptor(constructor, objs);
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
