using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization;

internal abstract class MetadataGenerator
{
	private readonly ISerializationContext serializationContext;

	protected int ObjectCount => serializationContext.ObjectCount;

	protected int CollectionCount => CollectionIdListTable.Count;

	protected int RootObjectId => serializationContext.RootObjectId;

	protected IEnumerable<TypeDataTable> DataTables => serializationContext.DataTables;

	protected ISequentialValueList<byte[]> ByteArrayTable => serializationContext.ByteArrayTable;

	protected ISequentialValueList<int[]> CollectionIdListTable => serializationContext.CollectionIdTable;

	protected ISequentialValueList<CollationInfo> CollationInfoTable => serializationContext.CollationInfoTable;

	protected ISequentialValueList<ComputedColumnInfo> ComputedColumnInfoTable => serializationContext.ComputedColumnInfoTable;

	protected ISequentialValueList<DataTypeSpec> DataTypeSpecTable => serializationContext.DataTypeSpecTable;

	protected ISequentialValueList<IdentityColumnInfo> IdentityColumnInfoTable => serializationContext.IdentityColumnInfoTable;

	protected ISequentialValueList<int> IntTable => serializationContext.IntTable;

	protected ISequentialValueList<IPassword> PasswordTable => serializationContext.PasswordTable;

	protected ISequentialValueList<string> StringTable => serializationContext.StringTable;

	protected MetadataGenerator(ISerializationContext serializationContext)
	{
		this.serializationContext = serializationContext;
	}
}
