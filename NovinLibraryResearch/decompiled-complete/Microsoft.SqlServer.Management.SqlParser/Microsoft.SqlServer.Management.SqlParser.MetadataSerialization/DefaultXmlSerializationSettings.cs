using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization;

internal class DefaultXmlSerializationSettings : IXmlSerializationSettings
{
	public virtual bool GetPropertyOverrideValue(MetadataTypeInfo typeInfo, MetadataPropertyInfo propertyInfo, out object value)
	{
		value = null;
		return false;
	}

	public virtual bool GetMetadataObjectOverride(IMetadataObject metadataObject, out IMetadataObject overrideObject)
	{
		overrideObject = null;
		return false;
	}

	public virtual XmlNestingType GetNestingType(MetadataTypeInfo typeInfo)
	{
		switch (typeInfo.Id)
		{
		case 21:
		case 39:
			return XmlNestingType.InlineStaticType;
		case 26:
		case 37:
		case 42:
		case 47:
		case 95:
		case 111:
			return XmlNestingType.Standalone;
		case 33:
			return XmlNestingType.InlineDynamicType;
		default:
			if (!typeInfo.IsValue)
			{
				return XmlNestingType.Standalone;
			}
			return XmlNestingType.InlineDynamicType;
		}
	}
}
