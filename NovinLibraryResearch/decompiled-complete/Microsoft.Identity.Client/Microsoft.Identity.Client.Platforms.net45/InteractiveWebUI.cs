using System;
using Microsoft.Identity.Client.Internal;
using Microsoft.Identity.Client.UI;

namespace Microsoft.Identity.Client.Platforms.net45;

internal class InteractiveWebUI : WebUI
{
	private WindowsFormsWebAuthenticationDialog _dialog;

	public InteractiveWebUI(CoreUIParent parent, RequestContext requestContext)
	{
		base.OwnerWindow = parent?.OwnerWindow;
		base.SynchronizationContext = parent?.SynchronizationContext;
		base.RequestContext = requestContext;
	}

	protected override AuthorizationResult OnAuthenticate()
	{
		WindowsFormsWebAuthenticationDialog obj = new WindowsFormsWebAuthenticationDialog(base.OwnerWindow)
		{
			RequestContext = base.RequestContext
		};
		WindowsFormsWebAuthenticationDialog windowsFormsWebAuthenticationDialog = obj;
		_dialog = obj;
		WindowsFormsWebAuthenticationDialog windowsFormsWebAuthenticationDialog2 = windowsFormsWebAuthenticationDialog;
		try
		{
			return _dialog.AuthenticateAAD(base.RequestUri, base.CallbackUri);
		}
		finally
		{
			((IDisposable)windowsFormsWebAuthenticationDialog2)?.Dispose();
		}
	}
}
