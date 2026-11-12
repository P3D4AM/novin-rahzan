using System;

namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Interface, AllowMultiple = true)]
internal class ReadWriteAttribute : Attribute, IPropertyAttribute
{
	private readonly string propertyName;

	public string PropertyName => propertyName;

	public ReadWriteAttribute()
		: this(null)
	{
	}

	public ReadWriteAttribute(string propertyName)
	{
		this.propertyName = propertyName;
	}
}
