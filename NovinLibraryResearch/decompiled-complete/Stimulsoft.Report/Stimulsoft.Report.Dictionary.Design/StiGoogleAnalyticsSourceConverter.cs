using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using Stimulsoft.Report.Dictionary.DataSources.Google;

namespace Stimulsoft.Report.Dictionary.Design;

public class StiGoogleAnalyticsSourceConverter : StiSqlSourceConverter
{
	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == typeof(InstanceDescriptor) && value != null)
		{
			StiGoogleAnalyticsSource stiGoogleAnalyticsSource = (StiGoogleAnalyticsSource)value;
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
			ConstructorInfo constructor = typeof(StiGoogleAnalyticsSource).GetConstructor(types);
			if (constructor != null)
			{
				object[] objs = new object[8]
				{
					stiGoogleAnalyticsSource.NameInSource,
					stiGoogleAnalyticsSource.Name,
					stiGoogleAnalyticsSource.Alias,
					string.Empty,
					stiGoogleAnalyticsSource.ConnectOnStart,
					stiGoogleAnalyticsSource.ReconnectOnEachRow,
					stiGoogleAnalyticsSource.CommandTimeout,
					stiGoogleAnalyticsSource.Key
				};
				return CreateNewInstanceDescriptor(constructor, objs);
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
