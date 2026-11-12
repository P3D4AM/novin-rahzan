using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Stimulsoft.Base.Databases;

internal static class StiPropertyHelper
{
	public static void Reset(object value)
	{
		PropertyInfo[] properties = value.GetType().GetProperties();
		PropertyInfo[] array = properties;
		foreach (PropertyInfo propertyInfo in array)
		{
			try
			{
				if (propertyInfo.CanWrite && propertyInfo.GetCustomAttributes(typeof(DefaultValueAttribute), inherit: true).FirstOrDefault() is DefaultValueAttribute defaultValueAttribute)
				{
					propertyInfo.SetValue(value, defaultValueAttribute.Value, null);
				}
			}
			catch
			{
			}
		}
	}
}
