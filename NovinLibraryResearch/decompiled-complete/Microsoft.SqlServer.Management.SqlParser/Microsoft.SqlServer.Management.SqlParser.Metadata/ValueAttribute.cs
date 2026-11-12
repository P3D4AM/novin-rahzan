using System;

namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Interface, AllowMultiple = true)]
internal class ValueAttribute : Attribute, IPropertyTypeAttribute, IPropertyAttribute
{
	private readonly InheritanceType inheritanceType;

	private readonly string propertyName;

	public string PropertyName => propertyName;

	public InheritanceType InheritanceType => inheritanceType;

	public ValueAttribute()
		: this(null)
	{
	}

	public ValueAttribute(string propertyName)
		: this(InheritanceType.Inherited, propertyName)
	{
	}

	public ValueAttribute(InheritanceType inheritanceType)
		: this(inheritanceType, null)
	{
	}

	public ValueAttribute(InheritanceType inheritanceType, string propertyName)
	{
		this.inheritanceType = inheritanceType;
		this.propertyName = propertyName;
	}
}
