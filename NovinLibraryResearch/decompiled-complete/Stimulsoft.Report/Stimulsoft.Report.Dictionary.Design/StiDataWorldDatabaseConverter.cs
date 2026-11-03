using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace Stimulsoft.Report.Dictionary.Design;

public class StiDataWorldDatabaseConverter : StiSqlDatabaseConverter
{
	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == typeof(InstanceDescriptor) && value != null)
		{
			StiDataWorldDatabase stiDataWorldDatabase = (StiDataWorldDatabase)value;
			Type[] types = new Type[5]
			{
				typeof(string),
				typeof(string),
				typeof(string),
				typeof(bool),
				typeof(string)
			};
			ConstructorInfo constructor = typeof(StiDataWorldDatabase).GetConstructor(types);
			if (constructor != null)
			{
				object[] objs = new object[5] { stiDataWorldDatabase.Name, stiDataWorldDatabase.Alias, stiDataWorldDatabase.ConnectionString, stiDataWorldDatabase.PromptUserNameAndPassword, stiDataWorldDatabase.Key };
				return CreateNewInstanceDescriptor(constructor, objs);
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
