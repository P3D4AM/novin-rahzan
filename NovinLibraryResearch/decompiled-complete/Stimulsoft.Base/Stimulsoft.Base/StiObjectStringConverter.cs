using System;
using System.ComponentModel;
using System.Reflection;

namespace Stimulsoft.Base;

public class StiObjectStringConverter
{
	public virtual void SetProperty(PropertyInfo p, object parentObject, object obj)
	{
	}

	public virtual string ObjectToString(object obj)
	{
		if (obj is string result)
		{
			return result;
		}
		if (obj is byte[])
		{
			return Convert.ToBase64String(obj as byte[], StiBaseOptions.AllowInsertLineBreaksWhenSavingByteArray ? Base64FormattingOptions.InsertLineBreaks : Base64FormattingOptions.None);
		}
		if (obj is Type)
		{
			return obj.ToString();
		}
		return GetConverter(obj.GetType()).ConvertToString(obj);
	}

	public virtual object StringToObject(string str, Type type)
	{
		if (type == typeof(string))
		{
			return str;
		}
		if (type == typeof(byte[]))
		{
			return Convert.FromBase64String(str);
		}
		if (type == typeof(decimal))
		{
			return decimal.Parse(str);
		}
		if (type == typeof(Type))
		{
			Type type2 = StiTypeFinder.GetType(str);
			if (type2 != null)
			{
				return type2;
			}
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			Assembly[] array = assemblies;
			foreach (Assembly assembly in array)
			{
				type2 = assembly.GetType(str);
				if (type2 != null)
				{
					return type2;
				}
			}
			throw new TypeLoadException("Type \"" + str + "\" not found");
		}
		if (type == typeof(object))
		{
			return str;
		}
		return GetConverter(type).ConvertFromString(str);
	}

	private TypeConverter GetConverter(Type type)
	{
		return TypeDescriptor.GetConverter(type);
	}
}
