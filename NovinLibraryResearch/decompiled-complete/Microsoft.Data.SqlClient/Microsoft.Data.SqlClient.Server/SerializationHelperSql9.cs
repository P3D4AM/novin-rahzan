using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.Data.Common;

namespace Microsoft.Data.SqlClient.Server;

internal class SerializationHelperSql9
{
	[ThreadStatic]
	private static Hashtable s_types2Serializers;

	private SerializationHelperSql9()
	{
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static int SizeInBytes(Type t)
	{
		return SizeInBytes(Activator.CreateInstance(t));
	}

	internal static int SizeInBytes(object instance)
	{
		Type type = instance.GetType();
		Format format = GetFormat(type);
		DummyStream dummyStream = new DummyStream();
		Serializer serializer = GetSerializer(instance.GetType());
		serializer.Serialize(dummyStream, instance);
		return (int)dummyStream.Length;
	}

	internal static void Serialize(Stream s, object instance)
	{
		GetSerializer(instance.GetType()).Serialize(s, instance);
	}

	internal static object Deserialize(Stream s, Type resultType)
	{
		return GetSerializer(resultType).Deserialize(s);
	}

	private static Format GetFormat(Type t)
	{
		return GetUdtAttribute(t).Format;
	}

	private static Serializer GetSerializer(Type t)
	{
		if (s_types2Serializers == null)
		{
			s_types2Serializers = new Hashtable();
		}
		Serializer serializer = (Serializer)s_types2Serializers[t];
		if (serializer == null)
		{
			serializer = GetNewSerializer(t);
			s_types2Serializers[t] = serializer;
		}
		return serializer;
	}

	internal static int GetUdtMaxLength(Type t)
	{
		SqlUdtInfo fromType = SqlUdtInfo.GetFromType(t);
		if (Format.Native == fromType.SerializationFormat)
		{
			return SizeInBytes(t);
		}
		return fromType.MaxByteSize;
	}

	private static object[] GetCustomAttributes(Type t)
	{
		object[] array = t.GetCustomAttributes(typeof(SqlUserDefinedTypeAttribute), inherit: false);
		if (array == null || array.Length == 0)
		{
			object[] customAttributes = t.GetCustomAttributes(inherit: false);
			array = new object[0];
			if (customAttributes != null && customAttributes.Length != 0)
			{
				for (int i = 0; i < customAttributes.Length; i++)
				{
					if (!customAttributes[i].GetType().FullName.Equals("Microsoft.SqlServer.Server.SqlUserDefinedTypeAttribute"))
					{
						continue;
					}
					SqlUserDefinedTypeAttribute sqlUserDefinedTypeAttribute = null;
					PropertyInfo[] properties = customAttributes[i].GetType().GetProperties();
					PropertyInfo[] array2 = properties;
					foreach (PropertyInfo propertyInfo in array2)
					{
						if (propertyInfo.Name.Equals("Format"))
						{
							sqlUserDefinedTypeAttribute = new SqlUserDefinedTypeAttribute((Format)propertyInfo.GetValue(customAttributes[i], null));
							break;
						}
					}
					if (sqlUserDefinedTypeAttribute != null)
					{
						PropertyInfo[] properties2 = sqlUserDefinedTypeAttribute.GetType().GetProperties();
						foreach (PropertyInfo propertyInfo2 in properties2)
						{
							if (propertyInfo2.CanRead && propertyInfo2.CanWrite)
							{
								object value = customAttributes[i].GetType().GetProperty(propertyInfo2.Name).GetValue(customAttributes[i]);
								propertyInfo2.SetValue(sqlUserDefinedTypeAttribute, value);
							}
						}
					}
					array = new object[1] { sqlUserDefinedTypeAttribute };
					break;
				}
			}
		}
		return array;
	}

	internal static SqlUserDefinedTypeAttribute GetUdtAttribute(Type t)
	{
		SqlUserDefinedTypeAttribute result = null;
		object[] customAttributes = GetCustomAttributes(t);
		if (customAttributes != null && customAttributes.Length == 1)
		{
			result = (SqlUserDefinedTypeAttribute)customAttributes[0];
		}
		else
		{
			Type typeFromHandle = typeof(InvalidUdtException);
			Type[] types = new Type[2]
			{
				typeof(Type),
				typeof(string)
			};
			MethodInfo method = typeFromHandle.GetMethod("Create", types);
			method.Invoke(null, new object[2]
			{
				t,
				Strings.SqlUdtReason_NoUdtAttribute
			});
		}
		return result;
	}

	private static Serializer GetNewSerializer(Type t)
	{
		SqlUserDefinedTypeAttribute udtAttribute = GetUdtAttribute(t);
		return udtAttribute.Format switch
		{
			Format.Native => new NormalizedSerializer(t), 
			Format.UserDefined => new BinarySerializeSerializer(t), 
			_ => throw ADP.InvalidUserDefinedTypeSerializationFormat(udtAttribute.Format), 
		};
	}
}
