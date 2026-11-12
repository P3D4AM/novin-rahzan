using System.Threading;

namespace Microsoft.Identity.Client.UI;

internal class CoreUIParent
{
	internal bool UseEmbeddedWebview { get; set; }

	internal SynchronizationContext SynchronizationContext { get; set; }

	internal SystemWebViewOptions SystemWebViewOptions { get; set; }

	internal bool UseHiddenBrowser { get; set; }

	internal object OwnerWindow { get; set; }

	public CoreUIParent()
	{
	}

	public CoreUIParent(object ownerWindow)
	{
		OwnerWindow = ownerWindow;
	}
}
