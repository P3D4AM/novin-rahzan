using System;

namespace Azure.Identity;

internal static class Validations
{
	private const string InvalidTenantIdErrorMessage = "Invalid tenant id provided. You can locate your tenant id by following the instructions listed here: https://docs.microsoft.com/partner-center/find-ids-and-domain-names";

	private const string NullTenantIdErrorMessage = "Tenant id cannot be null. You can locate your tenant id by following the instructions listed here: https://docs.microsoft.com/partner-center/find-ids-and-domain-names";

	private const string NonTlsAuthorityHostErrorMessage = "Authority host must be a TLS protected (https) endpoint.";

	public static string ValidateTenantId(string tenantId, string argumentName = null, bool allowNull = false)
	{
		if (tenantId != null)
		{
			if (tenantId.Length == 0)
			{
				throw (argumentName != null) ? new ArgumentException("Invalid tenant id provided. You can locate your tenant id by following the instructions listed here: https://docs.microsoft.com/partner-center/find-ids-and-domain-names", argumentName) : new ArgumentException("Invalid tenant id provided. You can locate your tenant id by following the instructions listed here: https://docs.microsoft.com/partner-center/find-ids-and-domain-names");
			}
			for (int i = 0; i < tenantId.Length; i++)
			{
				if (!IsValidTenantCharacter(tenantId[i]))
				{
					throw (argumentName != null) ? new ArgumentException("Invalid tenant id provided. You can locate your tenant id by following the instructions listed here: https://docs.microsoft.com/partner-center/find-ids-and-domain-names", argumentName) : new ArgumentException("Invalid tenant id provided. You can locate your tenant id by following the instructions listed here: https://docs.microsoft.com/partner-center/find-ids-and-domain-names");
				}
			}
		}
		else if (!allowNull)
		{
			throw (argumentName != null) ? new ArgumentNullException(argumentName, "Tenant id cannot be null. You can locate your tenant id by following the instructions listed here: https://docs.microsoft.com/partner-center/find-ids-and-domain-names") : new ArgumentNullException("Invalid tenant id provided. You can locate your tenant id by following the instructions listed here: https://docs.microsoft.com/partner-center/find-ids-and-domain-names", (Exception?)null);
		}
		return tenantId;
	}

	public static Uri ValidateAuthorityHost(Uri authorityHost)
	{
		if (authorityHost != null && string.Compare(authorityHost.Scheme, "https", StringComparison.OrdinalIgnoreCase) != 0)
		{
			throw new ArgumentException("Authority host must be a TLS protected (https) endpoint.");
		}
		return authorityHost;
	}

	private static bool IsValidTenantCharacter(char c)
	{
		if ((c < 'a' || c > 'z') && (c < 'A' || c > 'Z') && (c < '0' || c > '9') && c != '.')
		{
			return c == '-';
		}
		return true;
	}
}
