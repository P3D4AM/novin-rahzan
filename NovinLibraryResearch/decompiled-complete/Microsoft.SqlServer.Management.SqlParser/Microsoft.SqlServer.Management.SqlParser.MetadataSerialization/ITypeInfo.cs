namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization;

internal interface ITypeInfo
{
	int Id { get; }

	string Name { get; }

	string BaseName { get; }
}
