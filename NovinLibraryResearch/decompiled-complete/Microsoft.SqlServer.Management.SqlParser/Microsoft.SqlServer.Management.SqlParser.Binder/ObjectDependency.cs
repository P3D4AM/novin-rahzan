using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.Binder;

public struct ObjectDependency
{
	private readonly IMetadataObject targetObject;

	private readonly DependencyType dependencyType;

	public IMetadataObject TargetObject => targetObject;

	public DependencyType DependencyType => dependencyType;

	public ObjectDependency(IMetadataObject targetObject, DependencyType dependencyType)
	{
		this.targetObject = targetObject;
		this.dependencyType = dependencyType;
	}

	public override bool Equals(object obj)
	{
		if (!(obj is ObjectDependency))
		{
			return false;
		}
		return Equals((ObjectDependency)obj);
	}

	public bool Equals(ObjectDependency other)
	{
		if (targetObject == other.targetObject)
		{
			return dependencyType == other.dependencyType;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return ((targetObject != null) ? targetObject.GetHashCode() : 0) ^ dependencyType.GetHashCode();
	}
}
