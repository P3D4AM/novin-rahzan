namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

internal interface IPropertyTypeAttribute : IPropertyAttribute
{
	InheritanceType InheritanceType { get; }
}
