using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.Sdk.Sfc.Metadata;

namespace Microsoft.SqlServer.Management.Sdk.Differencing.SPI;

public class MetadataNodeItemNamesProvider : NodeItemNamesAdapterProvider
{
	public override bool IsGraphSupported(ISfcSimpleNode source)
	{
		if (source.ObjectReference is IAlienObject)
		{
			return true;
		}
		if (source.ObjectReference is SfcInstance)
		{
			return true;
		}
		return false;
	}

	public override bool IsContainerInNatrualOrder(ISfcSimpleNode node, string name)
	{
		foreach (SfcMetadataRelation relation in new SfcMetadataDiscovery(node.ObjectReference.GetType()).Relations)
		{
			if (!name.Equals(relation.PropertyName))
			{
				continue;
			}
			foreach (Attribute relationshipAttribute in relation.RelationshipAttributes)
			{
				if (relationshipAttribute is SfcObjectAttribute { NaturalOrder: not false })
				{
					return true;
				}
			}
		}
		return false;
	}

	public override IEnumerable<string> GetRelatedContainerNames(ISfcSimpleNode source)
	{
		List<string> list = new List<string>();
		foreach (SfcMetadataRelation relation in new SfcMetadataDiscovery(source.ObjectReference.GetType()).Relations)
		{
			SfcRelationship relationship = relation.Relationship;
			if (relationship != SfcRelationship.ObjectContainer && relationship != SfcRelationship.ChildContainer)
			{
				continue;
			}
			foreach (Attribute relationshipAttribute in relation.RelationshipAttributes)
			{
				if (relationshipAttribute is SfcObjectAttribute sfcObjectAttribute && (sfcObjectAttribute.Deploy || sfcObjectAttribute.Design))
				{
					list.Add(relation.PropertyName);
					break;
				}
			}
		}
		return list;
	}

	public override IEnumerable<string> GetRelatedObjectNames(ISfcSimpleNode source)
	{
		List<string> list = new List<string>();
		foreach (SfcMetadataRelation relation in new SfcMetadataDiscovery(source.ObjectReference.GetType()).Relations)
		{
			SfcRelationship relationship = relation.Relationship;
			if (relationship != SfcRelationship.Object && relationship != SfcRelationship.ChildObject)
			{
				continue;
			}
			foreach (Attribute relationshipAttribute in relation.RelationshipAttributes)
			{
				if (relationshipAttribute is SfcObjectAttribute sfcObjectAttribute && (sfcObjectAttribute.Deploy || sfcObjectAttribute.Design))
				{
					list.Add(relation.PropertyName);
					break;
				}
			}
		}
		return list;
	}

	public override IEnumerable<string> GetPropertyNames(ISfcSimpleNode source)
	{
		List<string> list = new List<string>();
		foreach (SfcMetadataRelation property in new SfcMetadataDiscovery(source.ObjectReference.GetType()).Properties)
		{
			foreach (Attribute relationshipAttribute in property.RelationshipAttributes)
			{
				if (relationshipAttribute is SfcPropertyAttribute sfcPropertyAttribute && (sfcPropertyAttribute.Deploy || sfcPropertyAttribute.Design))
				{
					list.Add(property.PropertyName);
				}
			}
		}
		return list;
	}
}
