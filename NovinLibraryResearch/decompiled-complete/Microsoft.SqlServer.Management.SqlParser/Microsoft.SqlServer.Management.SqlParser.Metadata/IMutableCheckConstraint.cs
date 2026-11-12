namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public interface IMutableCheckConstraint : ICheckConstraint, IConstraint, IMetadataObject, IMutableConstraint, IMutableMetadataObject
{
	new bool IsEnabled { get; set; }

	new bool IsChecked { get; set; }

	new bool NotForReplication { get; set; }

	new string Text { get; set; }
}
