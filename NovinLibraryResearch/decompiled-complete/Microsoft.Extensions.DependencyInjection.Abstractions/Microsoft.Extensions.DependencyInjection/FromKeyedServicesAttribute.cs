using System;

namespace Microsoft.Extensions.DependencyInjection;

[AttributeUsage(AttributeTargets.Parameter)]
public class FromKeyedServicesAttribute : Attribute
{
	public object Key { get; }

	public FromKeyedServicesAttribute(object key)
	{
		Key = key;
	}
}
