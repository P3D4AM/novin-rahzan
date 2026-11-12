using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization;

internal class XmlIdResolver : MetadataIdResolver
{
	public XmlIdResolver(IServer server)
		: base(server)
	{
	}

	public IMetadataObject ResolveObject(string strId)
	{
		MetadataObjectId id = ParseObjectId(strId);
		return ResolveObject(id);
	}

	private static MetadataObjectSelector ParseSelector(string strId, ref int idx)
	{
		if (strId[idx++] != '/')
		{
			throw new DeserializationException("Invalid key.");
		}
		int num = strId.IndexOf('[', idx);
		if (num < 0)
		{
			throw new DeserializationException("Invalid key.");
		}
		MetadataTypeInfo typeInfoByName = XmlConfig.GetTypeInfoByName(XmlTokenLookup.Find(strId, idx, num - idx));
		if (typeInfoByName == null)
		{
			throw new DeserializationException("Invalid key.");
		}
		num = (idx = num + 1);
		while (true)
		{
			num = strId.IndexOf(']', num);
			if (num < 0)
			{
				throw new DeserializationException("Invalid key.");
			}
			if (num + 1 >= strId.Length || strId[num + 1] != ']')
			{
				break;
			}
			num += 2;
		}
		string name = strId.Substring(idx, num - idx).Replace("]]", "]");
		idx = num + 1;
		return new MetadataObjectSelector(typeInfoByName, name);
	}

	private static MetadataObjectId ParseObjectId(string strId)
	{
		if (strId == "/")
		{
			return new MetadataObjectId(new List<MetadataObjectSelector>(0));
		}
		List<MetadataObjectSelector> list = new List<MetadataObjectSelector>();
		int idx = 0;
		while (idx < strId.Length)
		{
			list.Add(ParseSelector(strId, ref idx));
		}
		return new MetadataObjectId(list);
	}
}
