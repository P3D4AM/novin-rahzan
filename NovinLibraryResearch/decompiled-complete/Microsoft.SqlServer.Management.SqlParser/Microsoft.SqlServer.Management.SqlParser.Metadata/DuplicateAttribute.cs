using System;

namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Interface, AllowMultiple = true)]
internal class DuplicateAttribute : Attribute, IPropertyInferredAttribute, IPropertyTypeAttribute, IPropertyAttribute
{
	private readonly string propertyName;

	private readonly string duplicateName;

	public string DuplicateName => duplicateName;

	public string PropertyName => propertyName;

	public InheritanceType InheritanceType => InheritanceType.Inherited;

	public DuplicateAttribute(string duplicateName)
	{
		propertyName = null;
		this.duplicateName = duplicateName;
	}

	public DuplicateAttribute(string propertyName, string duplicateName)
	{
		this.propertyName = propertyName;
		this.duplicateName = duplicateName;
	}
}
