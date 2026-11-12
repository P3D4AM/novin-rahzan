namespace Stimulsoft.Base.Licenses;

internal static class StiLicenseProductWebExt
{
	internal static bool IsWebFrameworkAvailable(this StiLicenseProduct product)
	{
		if (product.Ident != StiProductIdent.Ultimate && product.Ident != StiProductIdent.Net && product.Ident != StiProductIdent.Web && product.Ident != StiProductIdent.Angular)
		{
			return product.Ident == StiProductIdent.DbsAngular;
		}
		return true;
	}
}
