using Microsoft.Identity.Client.Internal;

namespace Microsoft.Identity.Client.UI;

internal interface IWebUIFactory
{
	IWebUI CreateAuthenticationDialog(CoreUIParent coreUIParent, RequestContext requestContext);
}
