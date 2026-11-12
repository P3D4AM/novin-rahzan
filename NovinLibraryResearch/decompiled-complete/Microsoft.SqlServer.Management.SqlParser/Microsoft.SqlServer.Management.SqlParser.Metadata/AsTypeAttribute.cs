using System;

namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Interface, AllowMultiple = true)]
internal class AsTypeAttribute : Attribute, IPropertyInferredAttribute, IPropertyTypeAttribute, IPropertyAttribute
{
	private readonly string propertyName;

	private readonly Type type;

	public string PropertyName => propertyName;

	public Type Type => type;

	public InheritanceType InheritanceType => InheritanceType.Inherited;

	public AsTypeAttribute(Type type)
	{
		this.type = type;
	}

	public AsTypeAttribute(string propertyName, Type type)
	{
		this.propertyName = propertyName;
		this.type = type;
	}
}
