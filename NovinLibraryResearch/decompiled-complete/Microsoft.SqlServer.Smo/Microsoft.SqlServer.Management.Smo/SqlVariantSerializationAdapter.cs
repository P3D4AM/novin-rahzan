using System;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Smo;

public class SqlVariantSerializationAdapter : IXmlSerializationAdapter
{
	public void ReadXml(XmlReader reader, out object deserializedObject)
	{
		throw new NotImplementedException();
	}

	public void WriteXml(XmlWriter writer, object objectToSerialize)
	{
		Type type = objectToSerialize?.GetType();
		if (type == typeof(object[]))
		{
			writer.WriteStartElement("SqlVariantArray");
			object[] array = (object[])objectToSerialize;
			foreach (object objectToSerialize2 in array)
			{
				WriteXml(writer, objectToSerialize2);
			}
			writer.WriteEndElement();
		}
		else
		{
			string value = type?.FullName ?? "DBNull";
			writer.WriteStartElement("SqlVariant");
			writer.WriteAttributeString("Type", value);
			if (objectToSerialize is IXmlSerializable xmlSerializable)
			{
				xmlSerializable.WriteXml(writer);
			}
			else
			{
				writer.WriteString(StringFromSqlVariant(objectToSerialize));
			}
			writer.WriteEndElement();
		}
	}

	internal static string StringFromSqlVariant(object sqlVariant)
	{
		if (sqlVariant == null)
		{
			return string.Empty;
		}
		Type type = sqlVariant.GetType();
		if (type == typeof(int))
		{
			return ((int)sqlVariant).ToString(SmoApplication.DefaultCulture);
		}
		if (type == typeof(byte))
		{
			return ((byte)sqlVariant).ToString(SmoApplication.DefaultCulture);
		}
		if (type == typeof(decimal))
		{
			return ((decimal)sqlVariant).ToString(SmoApplication.DefaultCulture);
		}
		if (type == typeof(short))
		{
			return ((short)sqlVariant).ToString(SmoApplication.DefaultCulture);
		}
		if (type == typeof(long))
		{
			return ((long)sqlVariant).ToString(SmoApplication.DefaultCulture);
		}
		if (type == typeof(double))
		{
			return ((double)sqlVariant).ToString(SmoApplication.DefaultCulture);
		}
		if (type == typeof(float))
		{
			return ((float)sqlVariant).ToString(SmoApplication.DefaultCulture);
		}
		if (type == typeof(DateTime))
		{
			return SqlSmoObject.SqlDateString((DateTime)sqlVariant, "yyyy-MM-ddTHH:mm:ss.fff");
		}
		if (type == typeof(DateTimeOffset))
		{
			return SqlSmoObject.SqlDateString((DateTimeOffset)sqlVariant);
		}
		if (type == typeof(byte[]))
		{
			return Convert.ToBase64String((byte[])sqlVariant);
		}
		if (type == typeof(bool))
		{
			((bool)sqlVariant).ToString(SmoApplication.DefaultCulture);
		}
		return sqlVariant.ToString();
	}
}
