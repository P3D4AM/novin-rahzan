using System;
using System.Data;
using System.IO;
using Stimulsoft.Base.Helpers;

namespace Stimulsoft.Base;

public static class DataSetExtensions
{
	private static string EncryptedId = "8pTP&%#5uKAvvS10";

	public static byte[] WriteToArray(this DataSet dataSet)
	{
		if (dataSet == null)
		{
			return null;
		}
		using MemoryStream memoryStream = new MemoryStream();
		dataSet.WriteXml(memoryStream, XmlWriteMode.WriteSchema);
		return memoryStream.ToArray();
	}

	public static string WriteToString(this DataSet dataSet)
	{
		if (dataSet == null)
		{
			return null;
		}
		using StringWriter stringWriter = new StringWriter();
		dataSet.WriteXml(stringWriter, XmlWriteMode.WriteSchema);
		return stringWriter.ToString();
	}

	public static string WriteToEncryptedString(this DataSet dataSet)
	{
		byte[] bytes = dataSet.WriteToArray();
		bytes = StiPacker.Pack(bytes);
		bytes = StiEncryption.Encrypt(bytes, EncryptedId);
		if (bytes == null)
		{
			return null;
		}
		return Convert.ToBase64String(bytes);
	}

	public static void Read(this DataSet dataSet, byte[] schema, byte[] data = null)
	{
		if (schema != null)
		{
			using MemoryStream stream = new MemoryStream(schema);
			dataSet.ReadXmlSchema(stream);
		}
		if (data != null)
		{
			using (MemoryStream stream2 = new MemoryStream(data))
			{
				dataSet.ReadXml(stream2, XmlReadMode.Auto);
			}
		}
	}

	public static void Read(this DataSet dataSet, string schema, string data = null)
	{
		if (schema != null)
		{
			using StringReader reader = new StringReader(schema);
			dataSet.ReadXmlSchema(reader);
		}
		if (data != null)
		{
			using (StringReader reader2 = new StringReader(data))
			{
				dataSet.ReadXml(reader2, XmlReadMode.Auto);
			}
		}
	}

	public static void ReadFromEncryptedString(this DataSet dataSet, string str)
	{
		if (str != null)
		{
			byte[] src = Convert.FromBase64String(str);
			src = StiEncryption.Decrypt(src, EncryptedId);
			src = StiPacker.Unpack(src);
			dataSet.Read(null, src);
		}
	}
}
