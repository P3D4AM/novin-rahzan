using Microsoft.Identity.Client.OAuth2;

namespace Microsoft.Identity.Client.Instance.Validation;

internal class AdfsWebFingerResponseClaim : OAuth2ResponseBaseClaim
{
	public const string Subject = "subject";

	public const string Links = "links";

	public const string Rel = "rel";

	public const string Href = "href";
}
