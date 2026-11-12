using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization;

internal interface IXmlSerializationSettings
{
	bool GetPropertyOverrideValue(MetadataTypeInfo typeInfo, MetadataPropertyInfo propertyInfo, out object value);

	bool GetMetadataObjectOverride(IMetadataObject metadataObject, out IMetadataObject overrideObject);

	XmlNestingType GetNestingType(MetadataTypeInfo typeInfo);
}
