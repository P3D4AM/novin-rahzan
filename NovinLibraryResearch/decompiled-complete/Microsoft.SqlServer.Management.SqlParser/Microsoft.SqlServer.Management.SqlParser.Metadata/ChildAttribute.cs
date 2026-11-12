using System;

namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Interface, AllowMultiple = true)]
internal class ChildAttribute : Attribute, IPropertyTypeAttribute, IPropertyAttribute
{
	private readonly string propertyName;

	public string PropertyName => propertyName;

	public InheritanceType InheritanceType => InheritanceType.Inherited;

	public ChildAttribute()
	{
	}

	public ChildAttribute(string propertyName)
	{
		this.propertyName = propertyName;
	}
}
