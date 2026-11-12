using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.SqlServer.Management.Sdk.Sfc.Metadata;

namespace Microsoft.SqlServer.Management.Sdk.Sfc;

public static class SfcUtility
{
	private static string sqlceToolsPath = null;

	private static MethodInfo getChildTypeInfo = null;

	internal static Dictionary<string, Type> typeCache = new Dictionary<string, Type>();

	public static string GetSmlUri(Urn urn, Type instanceType)
	{
		return GetSmlUri(urn, instanceType, useCache: false);
	}

	internal static Type GetSmoChildType(string type, string parentName, Type instanceType)
	{
		if (getChildTypeInfo == null)
		{
			getChildTypeInfo = instanceType.Assembly().GetType("Microsoft.SqlServer.Management.Smo.SqlSmoObject").GetMethod("GetChildType", BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
		}
		return (Type)getChildTypeInfo.Invoke(null, new object[2] { type, parentName });
	}

	internal static string GetSmlUri(Urn urn, Type instanceType, bool useCache)
	{
		if (urn == null || string.IsNullOrEmpty(urn.ToString()))
		{
			return null;
		}
		StringBuilder stringBuilder = new StringBuilder();
		_ = instanceType.FullName;
		string rootTypeFullName = SfcRegistration.GetRegisteredDomainForType(instanceType).RootTypeFullName;
		string text = rootTypeFullName.Substring(0, rootTypeFullName.LastIndexOf('.') + 1);
		string text2 = null;
		int length = urn.XPathExpression.Length;
		bool flag = instanceType.FullName.StartsWith("Microsoft.SqlServer.Management.Smo", StringComparison.Ordinal);
		for (int i = 0; i < length; i++)
		{
			XPathExpressionBlock xPathExpressionBlock = urn.XPathExpression[i];
			string name = xPathExpressionBlock.Name;
			stringBuilder.Append("/" + SfcSecureString.XmlEscape(name));
			Type type = null;
			if (flag)
			{
				if (useCache)
				{
					if (!typeCache.ContainsKey(text2 + name))
					{
						typeCache[text2 + name] = GetSmoChildType(name, text2, instanceType);
					}
					type = typeCache[text2 + name];
				}
				else
				{
					type = GetSmoChildType(name, text2, instanceType);
				}
				text2 = type.Name;
			}
			else
			{
				type = SfcRegistration.GetObjectTypeFromFullName(text + name);
			}
			SfcMetadataDiscovery sfcMetadataDiscovery = new SfcMetadataDiscovery(type);
			bool flag2 = false;
			StringBuilder stringBuilder2 = new StringBuilder();
			foreach (SfcMetadataRelation readOnlyKey in sfcMetadataDiscovery.ReadOnlyKeys)
			{
				if (flag2)
				{
					stringBuilder2.Append(".");
				}
				else
				{
					flag2 = true;
				}
				string text3 = SfcSecureString.SmlEscape(xPathExpressionBlock.GetAttributeFromFilter(readOnlyKey.PropertyName));
				if (text3 == null)
				{
					return null;
				}
				stringBuilder2.Append(text3);
			}
			if (!string.IsNullOrEmpty(stringBuilder2.ToString()))
			{
				stringBuilder.Append("/" + SfcSecureString.XmlEscape(stringBuilder2.ToString()));
			}
		}
		return stringBuilder.ToString();
	}

	public static string GetUrn(object obj)
	{
		if (obj is SfcInstance)
		{
			return ((SfcInstance)obj).KeyChain.Urn;
		}
		if (obj is IAlienObject)
		{
			return (obj as IAlienObject).GetUrn();
		}
		return null;
	}

	internal static object GetParent(object obj)
	{
		if (obj is SfcInstance)
		{
			return ((SfcInstance)obj).Parent;
		}
		if (obj is IAlienObject)
		{
			return (obj as IAlienObject).GetParent();
		}
		return null;
	}

	internal static string GetXmlContent(XmlReader reader, string typeTag, bool isEmptyNode)
	{
		StringBuilder stringBuilder = new StringBuilder();
		XmlWriter xmlWriter = XmlWriter.Create(stringBuilder);
		xmlWriter.WriteStartDocument();
		xmlWriter.WriteStartElement(typeTag);
		if (!isEmptyNode)
		{
			do
			{
				xmlWriter.WriteNode(reader, defattr: false);
			}
			while (reader.IsStartElement());
		}
		xmlWriter.WriteEndElement();
		xmlWriter.WriteEndDocument();
		xmlWriter.Close();
		return stringBuilder.ToString();
	}

	internal static object GetXmlValue(string xmlContent, Type valueType)
	{
		XmlReader xmlReader = XmlReader.Create(new StringReader(xmlContent));
		return new XmlSerializer(valueType).Deserialize(xmlReader);
	}
}
