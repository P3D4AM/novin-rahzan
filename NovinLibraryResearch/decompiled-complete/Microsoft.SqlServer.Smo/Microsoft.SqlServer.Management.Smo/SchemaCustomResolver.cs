using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Smo;

public class SchemaCustomResolver
{
	public static object Resolve(object instance, params object[] parameters)
	{
		return SfcResolverHelper.GetDatabase(instance).Schemas[SfcResolverHelper.GetSchemaName(instance)];
	}

	public static object ResolveUrn(object instance, params object[] parameters)
	{
		return new Urn(SfcResolverHelper.GetDatabase(instance).Urn.ToString() + string.Format(SmoApplication.DefaultCulture, "/Schema[@Name = '{0}']", SfcSecureString.EscapeSquote(SfcResolverHelper.GetSchemaName(instance))));
	}
}
