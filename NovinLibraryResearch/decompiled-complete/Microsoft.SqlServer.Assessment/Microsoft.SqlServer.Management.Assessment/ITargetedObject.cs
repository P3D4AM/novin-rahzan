namespace Microsoft.SqlServer.Management.Assessment;

public interface ITargetedObject
{
	ISqlObjectLocator Target { get; }
}
