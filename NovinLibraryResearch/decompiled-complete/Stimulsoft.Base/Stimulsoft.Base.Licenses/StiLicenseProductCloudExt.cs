namespace Stimulsoft.Base.Licenses;

internal static class StiLicenseProductCloudExt
{
	internal static bool IsCloudAvailable(this StiLicenseProduct product)
	{
		if (product.Ident != StiProductIdent.BICloud && product.Ident != StiProductIdent.CloudDashboards)
		{
			return product.Ident == StiProductIdent.CloudReports;
		}
		return true;
	}
}
