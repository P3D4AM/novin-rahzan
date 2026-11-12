namespace Microsoft.SqlServer.Management.SqlParser.Binder;

public enum DependencyType
{
	Unknown,
	ExecuteTime,
	CreateTime,
	RegularPreventsDrop,
	RegularPreventsAnyChange
}
