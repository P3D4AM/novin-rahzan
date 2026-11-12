using System.Reflection;

namespace Microsoft.EntityFrameworkCore.Infrastructure;

public static class ProductInfo
{
	public static string GetVersion()
	{
		return typeof(ProductInfo).GetTypeInfo().Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
	}
}
