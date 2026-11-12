using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Base;

public class StiUniversalConverter : StiOrderConverter
{
	public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
	{
		PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(value, attributes);
		string[] names = (from PropertyDescriptor p in properties
			select p.Name into c
			orderby c
			select c).ToArray();
		return properties.Sort(names);
	}

	public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
	{
		if (destinationType == typeof(InstanceDescriptor))
		{
			return true;
		}
		if (destinationType == typeof(string))
		{
			return true;
		}
		return base.CanConvertTo(context, destinationType);
	}

	public override bool GetPropertiesSupported(ITypeDescriptorContext context)
	{
		return true;
	}

	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		ConstructorInfo constructorInfo = null;
		if (value != null)
		{
			ConstructorInfo[] constructors = value.GetType().GetConstructors();
			ConstructorInfo[] array = constructors;
			foreach (ConstructorInfo constructorInfo2 in array)
			{
				if (constructorInfo2.GetCustomAttributes(typeof(StiUniversalConstructorAttribute), inherit: false).Length != 0)
				{
					constructorInfo = constructorInfo2;
					break;
				}
			}
			if (constructorInfo == null)
			{
				if (destinationType == typeof(string))
				{
					return string.Empty;
				}
				return base.ConvertTo(context, culture, value, destinationType);
			}
		}
		if (destinationType == typeof(string) && constructorInfo != null)
		{
			object[] customAttributes = constructorInfo.GetCustomAttributes(typeof(StiUniversalConstructorAttribute), inherit: false);
			string name = ((StiUniversalConstructorAttribute)customAttributes[0]).Name;
			string text = StiLocalization.Get("PropertyMain", name, throwError: false);
			if (!string.IsNullOrEmpty(text))
			{
				return "(" + text + ")";
			}
			return "(" + name + ")";
		}
		if (destinationType == typeof(InstanceDescriptor) && value != null && constructorInfo != null)
		{
			CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
			try
			{
				Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
				ParameterInfo[] parameters = constructorInfo.GetParameters();
				Type[] array2 = new Type[parameters.Length];
				object[] array3 = new object[parameters.Length];
				for (int j = 0; j < parameters.Length; j++)
				{
					array2[j] = parameters[j].ParameterType;
					string text2 = parameters[j].Name.Substring(0, 1).ToUpperInvariant() + parameters[j].Name.Substring(1);
					PropertyInfo property = value.GetType().GetProperty(text2);
					if (property == null)
					{
						throw new ArgumentException($"Property '{text2}' not present in '{value.GetType()}'");
					}
					array3[j] = property.GetValue(value, null);
				}
				return new InstanceDescriptor(constructorInfo, array3);
			}
			finally
			{
				Thread.CurrentThread.CurrentCulture = currentCulture;
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
