using System;

namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Interface, AllowMultiple = true)]
internal class ConstantAttribute : Attribute, IPropertyInferredAttribute, IPropertyTypeAttribute, IPropertyAttribute
{
	private readonly string propertyName;

	private readonly object value;

	private readonly InheritanceType inheritanceType;

	public string PropertyName => propertyName;

	public object Value => value;

	public InheritanceType InheritanceType => inheritanceType;

	public ConstantAttribute(object value)
		: this(null, value)
	{
	}

	public ConstantAttribute(InheritanceType inheritanceType, object value)
		: this(inheritanceType, null, value)
	{
	}

	public ConstantAttribute(string propertyName, object value)
		: this(InheritanceType.Inherited, propertyName, value)
	{
	}

	public ConstantAttribute(InheritanceType inheritanceType, string propertyName, object value)
	{
		this.propertyName = propertyName;
		this.value = value;
		this.inheritanceType = inheritanceType;
	}
}
