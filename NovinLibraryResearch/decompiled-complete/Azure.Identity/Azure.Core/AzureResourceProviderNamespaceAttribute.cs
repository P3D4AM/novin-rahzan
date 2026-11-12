using System;

namespace Azure.Core;

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
internal class AzureResourceProviderNamespaceAttribute : Attribute
{
	public string ResourceProviderNamespace { get; }

	public AzureResourceProviderNamespaceAttribute(string resourceProviderNamespace)
	{
		ResourceProviderNamespace = resourceProviderNamespace;
	}
}
