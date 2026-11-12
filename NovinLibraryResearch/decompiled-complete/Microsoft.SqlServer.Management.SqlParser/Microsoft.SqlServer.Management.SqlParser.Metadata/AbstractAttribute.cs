using System;

namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[AttributeUsage(AttributeTargets.Property)]
internal class AbstractAttribute : Attribute, IPropertyTypeAttribute, IPropertyAttribute
{
	private readonly string propertyName;

	public string PropertyName => propertyName;

	public InheritanceType InheritanceType => InheritanceType.Inherited;

	public AbstractAttribute()
	{
		propertyName = null;
	}

	public AbstractAttribute(string propertyName)
	{
		this.propertyName = propertyName;
	}
}
