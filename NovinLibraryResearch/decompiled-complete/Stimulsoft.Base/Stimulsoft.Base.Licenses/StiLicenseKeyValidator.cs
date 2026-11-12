using System.Linq;
using Stimulsoft.Base.Design;

namespace Stimulsoft.Base.Licenses;

public static class StiLicenseKeyValidator
{
	internal static bool IsValidOnWebFramework(StiLicenseKey key)
	{
		if (IsValidOnAnyPlatform(key))
		{
			return key.Products.Any((StiLicenseProduct p) => p.IsWebFrameworkAvailable());
		}
		return false;
	}

	internal static bool IsValidOnNetFramework(StiLicenseKey key)
	{
		if (IsValidOnAnyPlatform(key))
		{
			return key.Products.Any((StiLicenseProduct p) => p.IsNetFrameworkAvailable());
		}
		return false;
	}

	internal static bool IsValidOnAnyPlatform(StiLicenseKey key)
	{
		if (key != null && key.Signature != null && key.Products != null)
		{
			return key.Products.Any((StiLicenseProduct p) => p.ExpirationDate > StiVersion.Created);
		}
		return false;
	}

	internal static bool IsTrial(StiLicenseKey key)
	{
		if (key != null && key.Signature != null && key.Products != null)
		{
			return !key.Products.Any();
		}
		return true;
	}

	internal static bool IsValid(StiProductIdent ident, StiLicenseKey key)
	{
		if (IsValidOnAnyPlatform(key))
		{
			return key.Products.Any((StiLicenseProduct p) => p.Ident == ident || p.Ident == StiProductIdent.Ultimate);
		}
		return false;
	}

	internal static bool IsValidInReportsDesignerOrOnPlatform(StiProductIdent ident, StiLicenseKey key)
	{
		if (!StiDesignerAppStatus.IsRunning)
		{
			return IsValid(ident, key);
		}
		if (IsValidOnAnyPlatform(key))
		{
			return key.Products.Any((StiLicenseProduct p) => p.IsReportsAvailable());
		}
		return false;
	}

	internal static bool IsValidInDashboardsDesignerOrOnPlatform(StiProductIdent ident, StiLicenseKey key)
	{
		if (!StiDesignerAppStatus.IsRunning)
		{
			return IsValid(ident, key);
		}
		if (IsValidOnAnyPlatform(key))
		{
			return key.Products.Any((StiLicenseProduct p) => p.IsDashboardsAvailable());
		}
		return false;
	}

	internal static StiLicenseKey GetLicenseKey()
	{
		if (string.IsNullOrWhiteSpace(StiLicense.Key))
		{
			return null;
		}
		if (StiLicense.LicenseKey == null)
		{
			return null;
		}
		if (StiLicense.LicenseKey.Signature == null)
		{
			return null;
		}
		return StiLicense.LicenseKey;
	}
}
