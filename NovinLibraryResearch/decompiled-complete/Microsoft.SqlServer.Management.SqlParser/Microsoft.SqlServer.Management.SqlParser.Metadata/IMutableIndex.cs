namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public interface IMutableIndex : IIndex, IMetadataObject, IMutableMetadataObject
{
	new bool DisallowPageLocks { get; set; }

	new bool DisallowRowLocks { get; set; }

	new byte FillFactor { get; set; }

	new bool IgnoreDuplicateKeys { get; set; }

	new bool IsDisabled { get; set; }

	new bool PadIndex { get; set; }
}
