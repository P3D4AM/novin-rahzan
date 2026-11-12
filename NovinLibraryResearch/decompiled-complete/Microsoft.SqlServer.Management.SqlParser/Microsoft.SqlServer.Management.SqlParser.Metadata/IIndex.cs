namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[Metadata(MetadataType.Abstract)]
[ReadOnly("Name")]
public interface IIndex : IMetadataObject
{
	[ReadOnly]
	[Parent]
	ITabular Parent { get; }

	[Abstract]
	IndexType Type { get; }

	[ReadWrite]
	bool DisallowPageLocks { get; }

	[ReadWrite]
	bool DisallowRowLocks { get; }

	[ReadWrite]
	byte FillFactor { get; }

	[ReadWrite]
	bool IgnoreDuplicateKeys { get; }

	[ReadWrite]
	bool IsDisabled { get; }

	[ReadWrite]
	bool PadIndex { get; }
}
