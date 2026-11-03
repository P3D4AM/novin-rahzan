using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using Stimulsoft.Report.Dictionary.DataSources.Google;

namespace Stimulsoft.Report.Dictionary.Design;

public class StiFirebaseSourceConverter : StiSqlSourceConverter
{
	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == typeof(InstanceDescriptor) && value != null)
		{
			StiFirebaseSource stiFirebaseSource = (StiFirebaseSource)value;
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
			ConstructorInfo constructor = typeof(StiFirebaseSource).GetConstructor(types);
			if (constructor != null)
			{
				object[] objs = new object[8]
				{
					stiFirebaseSource.NameInSource,
					stiFirebaseSource.Name,
					stiFirebaseSource.Alias,
					string.Empty,
					stiFirebaseSource.ConnectOnStart,
					stiFirebaseSource.ReconnectOnEachRow,
					stiFirebaseSource.CommandTimeout,
					stiFirebaseSource.Key
				};
				return CreateNewInstanceDescriptor(constructor, objs);
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
