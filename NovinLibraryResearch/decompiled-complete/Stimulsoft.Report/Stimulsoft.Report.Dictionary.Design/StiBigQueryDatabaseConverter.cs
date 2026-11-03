using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using Stimulsoft.Report.Dictionary.Databases.Google;

namespace Stimulsoft.Report.Dictionary.Design;

public class StiBigQueryDatabaseConverter : StiSqlDatabaseConverter
{
	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == typeof(InstanceDescriptor) && value != null)
		{
			StiBigQueryDatabase stiBigQueryDatabase = (StiBigQueryDatabase)value;
			Type[] types = new Type[5]
			{
				typeof(string),
				typeof(string),
				typeof(string),
				typeof(bool),
				typeof(string)
			};
			ConstructorInfo constructor = typeof(StiBigQueryDatabase).GetConstructor(types);
			if (constructor != null)
			{
				object[] objs = new object[5] { stiBigQueryDatabase.Name, stiBigQueryDatabase.Alias, stiBigQueryDatabase.ConnectionString, stiBigQueryDatabase.PromptUserNameAndPassword, stiBigQueryDatabase.Key };
				return CreateNewInstanceDescriptor(constructor, objs);
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
