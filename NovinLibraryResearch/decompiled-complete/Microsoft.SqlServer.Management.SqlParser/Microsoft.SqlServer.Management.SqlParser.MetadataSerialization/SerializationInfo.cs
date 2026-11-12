using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization;

internal abstract class SerializationInfo
{
	public abstract MetadataTypeInfo TypeInfo { get; }

	public abstract TId[] GetObjectData<TId>(ISerializationHandler<TId> serializer, IMetadataObject obj);
}
