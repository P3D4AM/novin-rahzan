using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Smo;

public class UserDefinedTypeResolver
{
	public static object Resolve(object instance, params object[] parameters)
	{
		DataType dataType = SfcResolverHelper.GetDataType(instance);
		if (dataType == null || dataType.SqlDataType != SqlDataType.UserDefinedType)
		{
			return null;
		}
		return SfcResolverHelper.GetDatabase(instance).UserDefinedTypes[dataType.Name, dataType.Schema];
	}

	public static object ResolveUrn(object instance, params object[] parameters)
	{
		DataType dataType = SfcResolverHelper.GetDataType(instance);
		if (dataType == null || dataType.SqlDataType != SqlDataType.UserDefinedType)
		{
			return null;
		}
		return new Urn(SfcResolverHelper.GetDatabase(instance).Urn.ToString() + string.Format(SmoApplication.DefaultCulture, "/UserDefinedType[@Name = '{0}' and @Schema = '{1}']", SfcSecureString.EscapeSquote(dataType.Name), SfcSecureString.EscapeSquote(dataType.Schema)));
	}
}
