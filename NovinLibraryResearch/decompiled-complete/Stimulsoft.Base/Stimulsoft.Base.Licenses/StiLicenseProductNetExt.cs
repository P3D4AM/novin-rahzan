namespace Stimulsoft.Base.Licenses;

internal static class StiLicenseProductNetExt
{
	internal static bool IsNetFrameworkAvailable(this StiLicenseProduct product)
	{
		if (product.Ident != StiProductIdent.BIDesigner && product.Ident != StiProductIdent.BIDesktop && product.Ident != StiProductIdent.BIServer && product.Ident != StiProductIdent.BICloud && product.Ident != StiProductIdent.Ultimate && product.Ident != StiProductIdent.Net && product.Ident != StiProductIdent.Silverlight && product.Ident != StiProductIdent.Web && product.Ident != StiProductIdent.Wpf && product.Ident != StiProductIdent.DbsWin && product.Ident != StiProductIdent.DbsWeb && product.Ident != StiProductIdent.CloudDashboards && product.Ident != StiProductIdent.CloudReports && product.Ident != StiProductIdent.Angular)
		{
			return product.Ident == StiProductIdent.DbsAngular;
		}
		return true;
	}
}
