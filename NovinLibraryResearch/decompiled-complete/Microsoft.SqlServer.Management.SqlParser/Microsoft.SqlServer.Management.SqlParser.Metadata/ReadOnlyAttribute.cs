using System;

namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Interface, AllowMultiple = true)]
internal class ReadOnlyAttribute : Attribute, IPropertyAttribute
{
	private readonly string propertyName;

	public string PropertyName => propertyName;

	public ReadOnlyAttribute()
		: this(null)
	{
	}

	public ReadOnlyAttribute(string propertyName)
	{
		this.propertyName = propertyName;
	}
}
