namespace Microsoft.SqlServer.Management.SqlParser.Binder;

public enum TemporaryBindMode : sbyte
{
	None,
	ColumnAssignmentTarget,
	MergeActionSource,
	MergeActionTarget
}
