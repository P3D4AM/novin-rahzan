namespace Stimulsoft.Base.Licenses;

internal static class StiLicenseProductDashboardsExt
{
	internal static bool IsDashboardsAvailable(this StiLicenseProduct product)
	{
		if (product.Ident != StiProductIdent.BIDesigner && product.Ident != StiProductIdent.BIDesktop && product.Ident != StiProductIdent.BIServer && product.Ident != StiProductIdent.BICloud && product.Ident != StiProductIdent.Ultimate && product.Ident != StiProductIdent.DbsJs && product.Ident != StiProductIdent.DbsWeb && product.Ident != StiProductIdent.DbsWin && product.Ident != StiProductIdent.DbsPhp && product.Ident != StiProductIdent.DbsAngular)
		{
			return product.Ident == StiProductIdent.CloudDashboards;
		}
		return true;
	}
}
