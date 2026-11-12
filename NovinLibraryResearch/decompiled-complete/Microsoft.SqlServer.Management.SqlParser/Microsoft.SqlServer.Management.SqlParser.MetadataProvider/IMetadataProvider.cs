using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public interface IMetadataProvider
{
	IServer Server { get; }

	IBuiltInFunctionLookup BuiltInFunctionLookup { get; }

	ICollationLookup CollationLookup { get; }

	ISystemDataTypeLookup SystemDataTypeLookup { get; }

	IMetadataFactory MetadataFactory { get; }

	MetadataProviderEventHandler BeforeBindHandler { get; }

	MetadataProviderEventHandler AfterBindHandler { get; }
}
