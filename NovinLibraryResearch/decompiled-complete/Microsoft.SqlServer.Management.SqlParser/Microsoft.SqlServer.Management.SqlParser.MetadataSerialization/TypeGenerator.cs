using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization;

internal abstract class TypeGenerator
{
	public abstract void AddFinalProperty(MetadataPropertyInfo propertyInfo, int value);

	public abstract void AddIndirectProperty(MetadataPropertyInfo propertyInfo, int fieldNumber, int startOffset, int length);
}
