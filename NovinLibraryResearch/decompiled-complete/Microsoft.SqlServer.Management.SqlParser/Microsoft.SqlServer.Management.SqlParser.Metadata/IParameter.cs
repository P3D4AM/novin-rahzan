namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[Metadata(MetadataType.Abstract)]
public interface IParameter : ILocalVariable, IMetadataObject
{
	[Abstract]
	bool IsOutput { get; }

	[Abstract]
	bool IsReadOnly { get; }

	[Abstract]
	string DefaultValue { get; }
}
