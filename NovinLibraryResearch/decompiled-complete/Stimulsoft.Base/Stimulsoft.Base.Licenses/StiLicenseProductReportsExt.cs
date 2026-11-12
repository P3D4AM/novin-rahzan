namespace Stimulsoft.Base.Licenses;

internal static class StiLicenseProductReportsExt
{
	internal static bool IsReportsAvailable(this StiLicenseProduct product)
	{
		if (product.Ident != StiProductIdent.BIDesigner && product.Ident != StiProductIdent.BIDesktop && product.Ident != StiProductIdent.BIServer && product.Ident != StiProductIdent.BICloud && product.Ident != StiProductIdent.Ultimate && product.Ident != StiProductIdent.Flex && product.Ident != StiProductIdent.Java && product.Ident != StiProductIdent.Js && product.Ident != StiProductIdent.Net && product.Ident != StiProductIdent.NetCore && product.Ident != StiProductIdent.Php && product.Ident != StiProductIdent.Silverlight && product.Ident != StiProductIdent.Uwp && product.Ident != StiProductIdent.Web && product.Ident != StiProductIdent.Wpf && product.Ident != StiProductIdent.Angular)
		{
			return product.Ident == StiProductIdent.CloudReports;
		}
		return true;
	}
}
