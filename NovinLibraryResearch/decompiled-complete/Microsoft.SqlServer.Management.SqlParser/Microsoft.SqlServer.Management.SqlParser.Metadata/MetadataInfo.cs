using System;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public class MetadataInfo
{
	private readonly List<TypeInfo> types;

	public List<TypeInfo> Types => types;

	public IEnumerable<MetadataTypeInfo> MetadataTypes
	{
		get
		{
			foreach (TypeInfo type in Types)
			{
				if (type.IsMetadata)
				{
					yield return (MetadataTypeInfo)type;
				}
			}
		}
	}

	public MetadataInfo(IEnumerable<TypeInfo> types)
	{
		this.types = new List<TypeInfo>(types);
	}

	public TypeInfo GetTypeInfoById(int id)
	{
		return types[id];
	}

	public TypeInfo GetTypeInfoByType(Type type)
	{
		foreach (TypeInfo type2 in types)
		{
			if (type2.Type.Equals(type))
			{
				return type2;
			}
		}
		return null;
	}

	public TypeInfo GetTypeInfoByName(string name)
	{
		foreach (TypeInfo type in types)
		{
			if (type.Name.Equals(name))
			{
				return type;
			}
		}
		return null;
	}
}
