using System;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public class MetadataTypeInfo : TypeInfo
{
	private readonly MetadataType metadataType;

	private List<MetadataPropertyInfo> instanceProperties;

	private List<MetadataPropertyInfo> inferredProperties;

	private MetadataPropertyInfo parentProperty;

	public MetadataType MetadataType => metadataType;

	public bool IsAbstract => metadataType == MetadataType.Abstract;

	public bool IsConcrete => metadataType != MetadataType.Abstract;

	public bool IsValue => metadataType == MetadataType.Value;

	public bool IsReference => metadataType == MetadataType.Reference;

	public List<MetadataPropertyInfo> InstanceProperties => instanceProperties;

	public List<MetadataPropertyInfo> InferredProperties => inferredProperties;

	public IEnumerable<MetadataPropertyInfo> AllProperties
	{
		get
		{
			foreach (MetadataPropertyInfo instanceProperty in instanceProperties)
			{
				yield return instanceProperty;
			}
			foreach (MetadataPropertyInfo inferredProperty in inferredProperties)
			{
				yield return inferredProperty;
			}
		}
	}

	public MetadataPropertyInfo ParentProperty => parentProperty;

	public MetadataTypeInfo(Type type, int id, MetadataType metadataType)
		: base(type, id)
	{
		this.metadataType = metadataType;
	}

	public void SetProperties(IList<MetadataPropertyInfo> instanceProperties, IList<MetadataPropertyInfo> inferredProperties)
	{
		this.instanceProperties = new List<MetadataPropertyInfo>(instanceProperties);
		this.inferredProperties = new List<MetadataPropertyInfo>(inferredProperties);
		foreach (MetadataPropertyInfo allProperty in AllProperties)
		{
			if (allProperty.IsParent)
			{
				parentProperty = allProperty;
				break;
			}
		}
	}

	public MetadataPropertyInfo GetPropertyByName(string name)
	{
		foreach (MetadataPropertyInfo allProperty in AllProperties)
		{
			if (allProperty.Name.Equals(name))
			{
				return allProperty;
			}
		}
		return null;
	}
}
