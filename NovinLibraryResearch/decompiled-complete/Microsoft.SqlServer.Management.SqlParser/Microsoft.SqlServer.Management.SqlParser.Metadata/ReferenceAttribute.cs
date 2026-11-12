using System;

namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Interface, AllowMultiple = true)]
internal class ReferenceAttribute : Attribute, IPropertyTypeAttribute, IPropertyAttribute
{
	private readonly string propertyName;

	public string PropertyName => propertyName;

	public InheritanceType InheritanceType => InheritanceType.Inherited;

	public ReferenceAttribute()
	{
	}

	public ReferenceAttribute(string propertyName)
	{
		this.propertyName = propertyName;
	}
}
