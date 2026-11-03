using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using Stimulsoft.Report.Dictionary.Databases.Azure;

namespace Stimulsoft.Report.Dictionary.Design;

public class StiCosmosDbDatabaseConverter : StiSqlDatabaseConverter
{
	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == typeof(InstanceDescriptor) && value != null)
		{
			StiCosmosDbDatabase stiCosmosDbDatabase = (StiCosmosDbDatabase)value;
			Type[] types = new Type[5]
			{
				typeof(string),
				typeof(string),
				typeof(string),
				typeof(bool),
				typeof(string)
			};
			ConstructorInfo constructor = typeof(StiCosmosDbDatabase).GetConstructor(types);
			if (constructor != null)
			{
				object[] objs = new object[5] { stiCosmosDbDatabase.Name, stiCosmosDbDatabase.Alias, stiCosmosDbDatabase.ConnectionString, stiCosmosDbDatabase.PromptUserNameAndPassword, stiCosmosDbDatabase.Key };
				return CreateNewInstanceDescriptor(constructor, objs);
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
